// DebugTrace.cs
// (C) 2018 Masato Kokubo

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace DebugTrace {
	using Logger;
	using System.Text.RegularExpressions;

	/// <summary>
	/// A utility class for debugging.
	/// </summary>
	///
	/// <remarks>
	/// Call DebugTrace.enter and DebugTrace.leave methods when enter and leave your methods,
	/// then outputs execution trace of the program.
	/// </remarks>
	///
	/// <since>1.0.0</since>
	/// <author>Masato Kokubo</author>
	public static class DebugTrace {
		// State class
		private class State {
			public int NestLevel       {get; set;} // The nest level
			public int BeforeNestLevel {get; set;} // The before nest level
			public int DataNestLevel   {get; set;} // The data nest level

			public override string ToString() {
				return "(State)["
					+ "NestLevel: " + NestLevel
					+ ", BeforeNestLevel: " + BeforeNestLevel
					+ ", DataNestLevel: " + DataNestLevel
					+ "]";
			}
		}

		// StackTraceElement struct
		private struct StackTraceElement {
			public StackTraceElement(string typeName, string methodName, string fileName, int lineNumber) {
				TypeName   = typeName  ;
				MethodName = methodName;
				FileName   = fileName  ;
				LineNumber = lineNumber;
			}

			public string TypeName   {get;} // The class name
			public string MethodName {get;} // The method nme
			public string FileName   {get;} // The file name
			public int    LineNumber {get;} // The line number
		}

		// Set of classes that dose not output the type name
		private static readonly ISet<Type> noOutputTypes = new HashSet<Type>() {
			typeof(bool    ),
			typeof(char    ),
			typeof(int     ),
			typeof(long    ),
			typeof(ulong   ),
			typeof(float   ),
			typeof(double  ),
			typeof(decimal ),
			typeof(string  ),
			typeof(char[]  ),
			typeof(byte[]  ),
			typeof(DateTime),
		};

		// Set of element types of array that dose not output the type name
		private static readonly ISet<Type> noOutputElementTypes = new HashSet<Type>() {
			typeof(bool    ),
			typeof(char    ),
			typeof(sbyte   ),
			typeof(byte    ),
			typeof(short   ),
			typeof(ushort  ),
			typeof(int     ),
			typeof(uint    ),
			typeof(long    ),
			typeof(ulong   ),
			typeof(float   ),
			typeof(double  ),
			typeof(decimal ),
			typeof(string  ),
			typeof(char[]  ),
			typeof(byte[]  ),
			typeof(DateTime),
		};

		private static readonly IDictionary<Type, string> typeNameMap = new Dictionary<Type, string>() {
			{typeof(bool   ), "bool"   },
			{typeof(char   ), "char"   },
			{typeof(sbyte  ), "sbyte"  },
			{typeof(byte   ), "byte"   },
			{typeof(short  ), "short"  },
			{typeof(ushort ), "ushort" },
			{typeof(int    ), "int"    },
			{typeof(uint   ), "uint"   },
			{typeof(long   ), "long"   },
			{typeof(ulong  ), "ulong"  },
			{typeof(float  ), "float"  },
			{typeof(double ), "double" },
			{typeof(decimal), "decimal"},
			{typeof(string ), "string"},
		};

		// Set of component types of array that output on the single line
		private static readonly ISet<Type> singleLineTypes = new HashSet<Type>() {
			typeof(bool          ), typeof(bool          []), typeof(bool          [,]), typeof(bool          [][]),
			typeof(char          ), typeof(char          []), typeof(char          [,]), typeof(char          [][]),
			typeof(sbyte         ), typeof(sbyte         []), typeof(sbyte         [,]), typeof(sbyte         [][]),
			typeof(byte          ), typeof(byte          []), typeof(byte          [,]), typeof(byte          [][]),
			typeof(short         ), typeof(short         []), typeof(short         [,]), typeof(short         [][]),
			typeof(ushort        ), typeof(ushort        []), typeof(ushort        [,]), typeof(ushort        [][]),
			typeof(int           ), typeof(int           []), typeof(int           [,]), typeof(int           [][]),
			typeof(uint          ), typeof(uint          []), typeof(uint          [,]), typeof(uint          [][]),
			typeof(long          ), typeof(long          []), typeof(long          [,]), typeof(long          [][]),
			typeof(ulong         ), typeof(ulong         []), typeof(ulong         [,]), typeof(ulong         [][]),
			typeof(float         ), typeof(float         []), typeof(float         [,]), typeof(float         [][]),
			typeof(double        ), typeof(double        []), typeof(double        [,]), typeof(double        [][]),
			typeof(decimal       ), typeof(decimal       []), typeof(decimal       [,]), typeof(decimal       [][]),
			typeof(string        ), typeof(string        []), typeof(string        [,]), typeof(string        [][]),
			typeof(DateTime      ), typeof(DateTime      []), typeof(DateTime      [,]), typeof(DateTime      [][]),
			typeof(DateTimeOffset), typeof(DateTimeOffset[]), typeof(DateTimeOffset[,]), typeof(DateTimeOffset[][]),
			typeof(TimeSpan      ), typeof(TimeSpan      []), typeof(TimeSpan      [,]), typeof(TimeSpan      [][]),
			typeof(Guid          ), typeof(Guid          []), typeof(Guid          [,]), typeof(Guid          [][]),
		};

		private static Resource resource = new Resource("DebugTrace");

		private static string   logLevel                 = resource.GetString ("LogLevel"               , "default"); // Log Level
		private static string   enterString              = resource.GetString ("EnterString"            , "Enter {0}.{1} ({2}:{3:D})"); // string at enter
		private static string   leaveString              = resource.GetString ("LeaveString"            , "Leave {0}.{1} ({2}:{3:D})"); // string at leave
		private static string   threadBoundaryString     = resource.GetString ("ThreadBoundaryString"   , "______________________________ Thread {0} ______________________________"); // string of threads boundary
		private static string   classBoundaryString      = resource.GetString ("ClassBoundaryString"    , "____ {0} ____"); // string of classes boundary
		private static string   indentString             = resource.GetString ("IndentString"           , "| "); // string of method call indent
		private static string   dataIndentString         = resource.GetString ("DataIndentString"       , "  "); // string of data indent
		private static string   limitString              = resource.GetString ("LimitString"            , "..."); // string to represent that it has exceeded the limit
		private static string   defaultNameSpaceString   = resource.GetString ("DefaultNameSpaceString" , "..."); // string replacing the default package part
		private static string   nonPrintString           = resource.GetString ("NonPrintString"         , "***"); // string of value in the case of properties that do not display the value
		private static string   cyclicReferenceString    = resource.GetString ("CyclicReferenceString"  , " *** cyclic reference *** "); // string to represent that the cyclic reference occurs
		private static string   varNameValueSeparator    = resource.GetString ("VarNameValueSeparator"  , " = "); // Separator between the variable name and value
		private static string   keyValueSeparator        = resource.GetString ("KeyValueSeparator"      , ": "); // Separator between the key and value for IDictionary obj
		private static string   fieldNameValueSeparator  = resource.GetString ("FieldNameValueSeparator", ": "); // Separator between the field name and value
		private static string   printSuffixFormat        = resource.GetString ("PrintSuffixFormat"      , " ({2}:{3:D})"); // Format string of Print suffix
		private static string   dateTimeFormat           = resource.GetString ("DateTimeFormat"         , "{0:G}"); // Format string of a DateTime and a string
		private static int      collectionLimit          = resource.GetInt    ("CollectionLimit"        , 512); // Limit of ICollection elements to output
		private static int      byteArrayLimit           = resource.GetInt    ("ByteArrayLimit"         , 8192); // Limit of byte array elements to output
		private static int      stringLimit              = resource.GetInt    ("StringLimit"            , 8192); // Limit of string characters to output
		private static string[] nonPrintProperties       = resource.GetStrings("NonPrintProperties"     , new string[0]); // Non Print properties (<class name>#<property name>)
		private static string   defaultNameSpace         = resource.GetString ("DefaultNameSpace"       , ""); // Default package part
		private static string[] reflectionClasses        = resource.GetStrings("ReflectionClasses"      , new string[0]); // List of class names that output content in reflection even if ToString method is implemented
	//	private static Dictionary<string, string> dictionaryNameIDictionary = Dictionary<string, string>(); // Name to dictionaryNmae dictionary 

		// Logger
		private static ILogger logger;

		// Whether tracing is enabled
		private static bool enabled;

		// Array of indent strings
		private static readonly string[] indentStrings = new string[64];

		// Array of data indent strings
		private static readonly string[] dataIndentStrings = new string[64];

		// Dictionary of thread id to the indent state
		private static readonly IDictionary<int, State> states = new Dictionary<int, State>();

		// Before thread id
		private static int beforeThreadId;

		// Reflected objects
		private static readonly ICollection<object> reflectedObjects = new List<object>();

		private static string lastLog = "";

		/// <summary>
		/// class constructor
		/// </summary>
		static DebugTrace() {
			string loggerName = null;
			try {
				loggerName = resource.GetString("Logger", null);
				if (loggerName != null) {
					if (!loggerName.Contains('.'))
						loggerName = typeof(ILogger).Namespace + '.' + loggerName;
					logger = (ILogger)Type.GetType(loggerName).GetConstructor(new Type[0]).Invoke(new object[0]);
				}
			}
			catch (Exception e) {
				System.Console.Error.WriteLine($"DebugTrace-net: {e.ToString()}({loggerName})");
			}

			if (logger == null)
				logger = new Console.Error();

			// Set a logging level
			logger.Level = logLevel;
			enabled = logger.IsEnabled;

			// make code indent strings
			indentStrings[0] = "";
			for (var index = 1; index < indentStrings.Length; ++index)
				indentStrings[index] = indentStrings[index - 1] + indentString;

			// make data indent strings
			dataIndentStrings[0] = "";
			for (var index = 1; index < dataIndentStrings.Length; ++index)
				dataIndentStrings[index] = dataIndentStrings[index - 1] + dataIndentString;

			// output version log
			var versionAttribute = (AssemblyInformationalVersionAttribute)
				Attribute.GetCustomAttribute(Resource.selfAssembly, typeof(AssemblyInformationalVersionAttribute));
			logger.Log($"DebugTrace-net {versionAttribute?.InformationalVersion}");
			logger.Log($"  properties: {resource.fileInfo.FullName}");
			logger.Log($"  Logger: {logger.GetType().FullName}");
		}

		/// <summary>
		/// Append a timestamp to the head of the string.
		/// </summary>
		///
		/// <param name="string ">a string</param>
		/// <returns>a string appended a timestamp string</returns>
		public static String AppendDateTime(string str) {
			return string.Format(dateTimeFormat, DateTime.Now) + " " + str;
		}


		/// <summary>
		/// Returns indent state.
		/// </summary>
		private static State CurrentState {
			get {
				State state;
				int threadId = Thread.CurrentThread.ManagedThreadId;

				if (states.ContainsKey(threadId)) {
					state = states[threadId];
				} else {
					state = new State();
					states[threadId] = state;
				}

				return state;
			}
		}

		/// <summary>
		/// Returns whether tracing is enabled.
		/// </summary>
		///
		/// <returns>true if tracing is enabled; false otherwise</returns>
		public static bool IsEnabled {get => enabled;}

		/// <summary>
		/// Returns a string corresponding to the current indent.
		/// </summary>
		private static string GetIndentString(State state) {
			return indentStrings[
				state.NestLevel < 0 ? 0 :
				state.NestLevel >= indentStrings.Length ? indentStrings.Length - 1
					: state.NestLevel]
				+ dataIndentStrings[
				state.DataNestLevel < 0 ? 0 :
				state.DataNestLevel >= dataIndentStrings.Length ? dataIndentStrings.Length - 1
					: state.DataNestLevel];
		}

		/// <summary>
		/// Up the nest level.
		/// </summary>
		private static void UpNest(State state) {
			state.BeforeNestLevel = state.NestLevel;
			++state.NestLevel;
		}

		/// <summary>
		/// Down the nest level.
		/// </summary>
		private static void DownNest(State state) {
			state.BeforeNestLevel = state.NestLevel;
			--state.NestLevel;
		}

		/// <summary>
		/// Up the data nest level.
		/// </summary>
		private static void UpDataNest(State state) {
			++state.DataNestLevel;
		}

		/// <summary>
		/// Down the data nest level.
		/// </summary>
		private static void DownDataNest(State state) {
			--state.DataNestLevel;
		}

		/// <summary>
		/// Common start processing of output.
		/// </summary>
		private static void PrintStart() {
			var thread = Thread.CurrentThread;
			int threadId = thread.ManagedThreadId;
			if (threadId !=  beforeThreadId) {
				// Thread changing
				logger.Log(""); // Line break
				logger.Log(string.Format(threadBoundaryString, threadId));
				logger.Log(""); // Line break

				beforeThreadId = threadId;
			}
		}

		/// <summary>
		/// Common end processing of output.
		/// </summary>
		private static void PrintEnd() {
			beforeThreadId = Thread.CurrentThread.ManagedThreadId;
		}

		/// <summary>
		/// Call this method at entrance of your methods.
		/// </summary>
		public static void Enter() {
			if (enabled) {
				lock (states) {
					PrintStart(); // Common start processing of output

					var state = CurrentState;
					if (state.BeforeNestLevel > state.NestLevel)
						logger.Log(GetIndentString(state)); // Line break

					lastLog = GetIndentString(state) + GetCallerInfo(enterString);
					logger.Log(lastLog);

					UpNest(state);

					PrintEnd(); // Common end processing of output
				}
			}
		}


		/// <summary>
		/// Call this method at exit of your methods.
		/// </summary>
		public static void Leave() {
			if (enabled) {
				lock (states) {
					PrintStart(); // Common start processing of output

					var state = CurrentState;
					DownNest(state);

					lastLog = GetIndentString(state) + GetCallerInfo(leaveString);
					logger.Log(lastLog);

					PrintEnd(); // Common end processing of output
				}
			}
		}

		/// <summary>
		/// Returns a string of the caller information.
		/// </summary>
		private static string GetCallerInfo(string baseString) {
			var element = GetStackTraceElement();

			return string.Format(baseString,
				ReplaceTypeName(element.TypeName),
				element.MethodName,
				element.FileName,
				element.LineNumber);
		}

		/// <summary>
		/// Outputs the message to the log.
		/// </summary>
		///
		/// <param name="message">a message</param>
		public static void Print(string message) {
			if (enabled)
				PrintSub(message);
		}

		/// <summary>
		/// Outputs a message to the log.
		/// </summary>
		///
		/// <param name="messageSupplier">a message supplier</param>
		public static void Print(Func<string> messageSupplier) {
			if (enabled)
				PrintSub(messageSupplier());
		}

		/// <summary>
		/// Outputs the message to the log.
		/// </summary>
		private static void PrintSub(string message) {
			lock (states) {
				PrintStart(); // Common start processing of output

				var lastLog = "";
				if (message != "") {
					var element = GetStackTraceElement();
					var suffix = string.Format(printSuffixFormat,
						ReplaceTypeName(element.TypeName),
						element.MethodName,
						element.FileName,
						element.LineNumber);
					lastLog = GetIndentString(CurrentState) + message + suffix;
				}
				logger.Log(lastLog);

				PrintEnd(); // Common end processing of output
			}
		}

		/// <summary>
		/// Outputs the name and value to the log.
		/// </summary>
		///
		/// <param name="name">the name of the value</param>
		/// <param name="value">the value to output (accept null)</param>
		private static void PrintSub(string name, object value) {
			lock (states) {
				PrintStart(); // Common start processing of output

				reflectedObjects.Clear();

				var state = CurrentState;
				var strings = new List<string>();
				var buff = new StringBuilder();

				buff.Append(name).Append(varNameValueSeparator);
				var normalizedName = name.Substring(name.LastIndexOf('.') + 1).Trim();
				normalizedName = normalizedName.Substring(normalizedName.LastIndexOf(' ') + 1);
				Append(state, strings, buff, value, false);

				var element = GetStackTraceElement();
				var suffix = string.Format(printSuffixFormat,
					element.TypeName,
					element.MethodName,
					element.FileName,
					element.LineNumber);
				buff.Append(suffix);
				LineFeed(state, strings, buff);

				strings.ForEach(str => logger.Log(str));
				lastLog = string.Join("\n", strings);

				PrintEnd(); // Common end processing of output
			}
		}

		/// <summary>
		/// Returns a caller stack trace element.
		/// </summary>
		///
		/// <returns>a caller stack trace element</returns>
		private static StackTraceElement GetStackTraceElement() {
			var elements = Environment.StackTrace.Split('\n')
				.Select(str => str.Trim())
				.Where(str => str != "" && !str.Contains(".DebugTrace.") && !str.Contains("StackTrace"))
				.Select(str => {
					//  0  1            2  3             4
					// "at Class.Method() in filePath:line N"
					(var str1, var str2) = Split(str, ' ');
					(var str3, var str4) = Split(str2, ')');
					str3 = str3 + ')';
					(var typeName, var methodName) = LastSplit(str3, '.');
					(var str5, var str6) = Split(str4.Trim(), ' ');
					(var fileName, var str7) = LastSplit(str6, ':');
					fileName = Path.GetFileName(fileName);
					(var str8, var lineNoStr) = Split(str7.Trim(), ' ');
					var lineNo = lineNoStr == "" ? 0 : int.Parse(lineNoStr);
					return new StackTraceElement(typeName, methodName, fileName, lineNo);
				});

			var result = elements.Count() > 0 
				? elements.ElementAt(0)
				: new StackTraceElement("--", "--", "--", 0);

			return result;
		}

		private static (string, string) Split(string str, char separator) {
			var index = str.IndexOf(separator);
			return index < 0
				? (str, "")
				: (str.Substring(0, index), str.Substring(index + 1));
		}

		private static (string, string) LastSplit(string str, char separator) {
			var index = str.LastIndexOf(separator);
			return index < 0
				? (str, "")
				: (str.Substring(0, index), str.Substring(index + 1));
		}

		/// <summary>
		/// Line Feed.
		/// </summary>
		///
		/// <param name="state">the indent state</param>
		/// <param name="strings">the string list</param>
		/// <param name="buff">the string buffer</param>
		private static void LineFeed(State state, IList<string> strings, StringBuilder buff) {
			strings.Add(GetIndentString(CurrentState) + buff.ToString());
			buff.Clear();
		}

		/// <summary>
		/// Outputs the name and value to the log.
		/// </summary>
		///
		/// <param name="name">the name of the value</param>
		/// <param name="value">the value to output (accept null)</param>
		public static void Print(string name, object value) {
			if (enabled)
				PrintSub(name, value);
		}

		/// <summary>
		/// Outputs the name and value to the log.
		/// </summary>
		///
		/// <param name="">T> type of the value</param>
		/// <param name="name">the name of the value</param>
		/// <param name="valueSupplier">the supplier of value to output</param>
		public static void Print<T>(string name, Func<T> valueSupplier) {
			if (enabled)
				PrintSub(name, valueSupplier());
		}

		/// <summary>
		/// Appends the value for logging to the string buffer.
		/// </summary>
		///
		/// <param name="state">the indent state</param>
		/// <param name="strings">the string list</param>
		/// <param name="buff">the string buffer</param>
		/// <param name="value">the value object</param>
		/// <param name="isElement">true if the value is element of a container class, false otherwise</param>
		/// 
		private static void Append(State state, IList<string> strings, StringBuilder buff, object value, bool isElement) {
			if (value == null) {
				buff.Append("null");
			} else {
				var type = value.GetType();

				var typeName = GetTypeName(type, value, isElement);
				if (typeName != null)
					buff.Append(typeName);

				switch (value) {
				case bool         boolValue: buff.Append(boolValue ? "true" : "false"); break;
				case char         charValue: buff.Append('\''); Append(state, strings, buff, charValue); buff.Append('\''); break;
				case sbyte       sbyteValue: buff.Append(sbyteValue  ); break;
				case byte         byteValue: buff.Append(byteValue   ); break;
				case short       shortValue: buff.Append(shortValue  ); break;
				case ushort     ushortValue: buff.Append(ushortValue ); break;
				case int           intValue: buff.Append(intValue    ); break;
				case uint         uintValue: buff.Append(uintValue   ); break;
				case long         longValue: buff.Append(longValue   ).Append('L' ); break;
				case ulong       ulongValue: buff.Append(ulongValue  ).Append("uL"); break;
				case float       floatValue: buff.Append(floatValue  ).Append('f' ); break;
				case double     doubleValue: buff.Append(doubleValue ).Append('d' ); break;
				case decimal   decimalValue: buff.Append(decimalValue).Append('m' ); break;
				case DateTime      dateTime: buff.Append(string.Format(dateTimeFormat, dateTime)); break;
				case char[]       charArray: Append(state, strings, buff, new string(charArray)); break;
				case byte[]       byteArray: Append(state, strings, buff, byteArray); break;
				case string     stringValue: Append(state, strings, buff, stringValue); break;
				case IDictionary dictionary: Append(state, strings, buff, dictionary); break;
				case ICollection collection: Append(state, strings, buff, collection); break;
				case Enum         enumValue: buff.Append(enumValue); break;
				default:
					// Other
				//	bool isReflection = reflectionClasses.Contains(type.FullName); // TODO
					bool isReflection = true;
				//	if (!isReflection && !HasToString(type)) {
				//		isReflection = true;
				//		reflectionClasses.Add(type.FullName);
				//	}

					if (isReflection) {
						// Use Reflection
						if (reflectedObjects.Any(obj => value == obj))
							// Cyclic reference
							buff.Append(cyclicReferenceString).Append(value);
						else {
							// Use Reflection
							reflectedObjects.Add(value);
							AppendReflectString(state, strings, buff, value);
							reflectedObjects.Remove(reflectedObjects.Count - 1);
						}
					} else {
						// Use ToString method
						buff.Append(value);
					}
					break;
				}
			}
		}

		/// <summary>
		/// Returns the type name to be output to the log.<br>
		/// If dose not output, returns null.
		/// </summary>
		///
		/// <param name="type">the type of the value</param>
		/// <param name="value">the value object</param>
		/// <param name="isElement">true if the value is element of a container class, false otherwise</param>
		/// <param name="nest">current nest count</param>
		/// <returns>the type name to be output to the log</returns>
		private static string GetTypeName(Type type, object value, bool isElement, int nest = 0) {
			string typeName = null;

			if (type.IsArray) {
				// Array
				typeName = GetTypeName(type.GetElementType(), null, false, nest + 1);
				if (typeName != null) {
					var bracket = "[";
					if (value != null)
						bracket += ((Array)value).Length;
					bracket += ']';
					int braIndex = typeName.IndexOf('[');
					if (braIndex < 0)
						braIndex = typeName.Length;
					typeName = typeName.Substring(0, braIndex) + bracket + typeName.Substring(braIndex) + ' ';
				}
			} else if (type.Name.StartsWith("Tuple`")) {
				typeName = "Tuple";

			} else if (type.Name.StartsWith("ValueTuple`")) {
				typeName = "ValueTuple";

			} else {
				// Not Array
				var noOutputType = isElement ? noOutputElementTypes.Contains(type) : noOutputTypes.Contains(type);
				if (nest > 0 || !noOutputType) {
					// Output the type name
					if (typeNameMap.ContainsKey(type)) {
						typeName = typeNameMap[type];
					} else {
						typeName = ReplaceTypeName(type.FullName);
						if (value is ICollection collection)
							typeName += " Count:" + collection.Count;
					}
				}
				if (typeName != null && nest == 0)
					typeName += ' ';
			}

			return typeName;
		}

		private static Regex typeRemoveRegex = new Regex(@"(`[0-9]+)|(, [^, \]]+)+");

		/// <summary>
		/// Replace a class name.
		/// </summary>
		///
		/// <param name="typeName">a class name</param>
		/// <returns>the replaced ckass name</returns>
		private static string ReplaceTypeName(string typeName) {
			typeName = typeRemoveRegex.Replace(typeName, "");

			if (defaultNameSpace != "" && typeName.StartsWith(defaultNameSpace))
				typeName = defaultNameSpaceString + typeName.Substring(defaultNameSpace.Length);
			return typeName;
		}

		/// <summary>
		/// Appends a character representation for logging to the string buffer.
		/// </summary>
		///
		/// <param name="state">the indent state</param>
		/// <param name="strings">the string list</param>
		/// <param name="buff">the string buffer</param>
		/// <param name="ch">a character</param>
		private static void Append(State state, IList<string> strings, StringBuilder buff, char ch) {
			switch (ch) {
			case '\0': buff.Append(@"\0"); break; // 00 NUL
			case '\a': buff.Append(@"\a"); break; // 07 BEL
			case '\b': buff.Append(@"\b"); break; // 08 BS
			case '\t': buff.Append(@"\t"); break; // 09 HT
			case '\n': buff.Append(@"\n"); break; // 0A LF
			case '\v': buff.Append(@"\v"); break; // 0B VT
			case '\f': buff.Append(@"\f"); break; // 0C FF
			case '\r': buff.Append(@"\r"); break; // 0D CR
			case '"' : buff.Append(@"\"""); break; // "
			case '\'': buff.Append(@"\'"); break; // '
			case '\\': buff.Append(@"\\"); break; // \
			default:
				if (ch < ' ' || ch == '\u007F')
					buff.Append(string.Format(@"\u{0:X4}", (ushort)ch));
				else
					buff.Append(ch);
				break;
			}
		}

		/// <summary>
		/// Appends a CharSequence representation for logging to the string buffer.
		/// </summary>
		///
		/// <param name="state">the indent state</param>
		/// <param name="strings">the string list</param>
		/// <param name="buff">the string buffer</param>
		/// <param name="str">a string object</param>
		private static void Append(State state, IList<string> strings, StringBuilder buff, string str) {
			buff.Append('"');
			for (int index = 0; index < str.Length; ++index) {
				if (index >= stringLimit) {
					buff.Append(limitString);
					break;
				}
				Append(state, strings, buff, str[index]);
			}
			buff.Append('"');
		}

		/// <summary>
		/// Appends a byte array representation for logging to the string buffer.
		/// </summary>
		///
		/// <param name="state">the indent state</param>
		/// <param name="strings">the string list</param>
		/// <param name="buff">the string buffer</param>
		/// <param name="bytes">a byte array</param>
		private static void Append(State state, IList<string> strings, StringBuilder buff, byte[] bytes) {
			var multiLine = bytes.Length > 16 && byteArrayLimit > 16;

			buff.Append('{');
			if (multiLine) {
				LineFeed(state, strings, buff);
				UpDataNest(state);
			}

			int offset = 0;
			for (int index = 0; index < bytes.Length; ++index) {
				if (offset > 0) buff.Append(" ");

				if (index >= byteArrayLimit) {
					buff.Append(limitString);
					break;
				}

				buff.Append(string.Format("{0:X2}", bytes[index]));
				++offset;

				if (multiLine && offset == 16) {
					LineFeed(state, strings, buff);
					offset = 0;
				}
			}

			if (multiLine) {
				if (buff.Length > 0)
					LineFeed(state, strings, buff);
				DownDataNest(state);
			}
			buff.Append('}');
		}

		/// <summary>
		/// Appends a Collection representation for logging to the string buffer.
		/// </summary>
		///
		/// <param name="state">the indent state</param>
		/// <param name="strings">the string list</param>
		/// <param name="buff">the string buffer</param>
		/// <param name="collection">a Collection object</param>
		private static void Append(State state, IList<string> strings, StringBuilder buff, ICollection collection) {
			var multiLine = !isSingleLine(collection);

			buff.Append('{');
			var index = 0;
			foreach (var element in collection) {
				if (index == 0) { 
					if (multiLine) {
						LineFeed(state, strings, buff);
						UpDataNest(state);
					}
				} else {
					if (!multiLine)
						buff.Append(", ");
				}

				if (index < collectionLimit)
					Append(state, strings, buff, element, true);
				else
					buff.Append(limitString);

				if (multiLine) {
					buff.Append(",");
					LineFeed(state, strings, buff);
				}

				if (index++ >= collectionLimit) break;
			}

			if (multiLine)
				DownDataNest(state);
			buff.Append('}');
		}

		/// <summary>
		/// Appends a IDictionary representation for logging to the string buffer.
		/// </summary>
		///
		/// <param name="state">the indent state</param>
		/// <param name="strings">the string list</param>
		/// <param name="buff">the string buffer</param>
		/// <param name="dictionary">a IDictionary</param>
		private static void Append(State state, IList<string> strings, StringBuilder buff, IDictionary dictionary) {
			var multiLine = !isSingleLine(dictionary);

			buff.Append('{');
			var index = 0;
			foreach (var key in dictionary.Keys) {
				var value = dictionary[key];
				if (index == 0) {
					if (multiLine) {
						LineFeed(state, strings, buff);
						UpDataNest(state);
					}
				} else {
					if (!multiLine)
						buff.Append(", ");
				}

				if (index < collectionLimit) {
					Append(state, strings, buff, key, true);
					buff.Append(keyValueSeparator);
					Append(state, strings, buff, value, true);
				} else
					buff.Append(limitString);

				if (multiLine) {
					buff.Append(",");
					LineFeed(state, strings, buff);
				}

				if (index++ >= collectionLimit) break;
			}

			if (multiLine)
				DownDataNest(state);
			buff.Append('}');
		}

		/// <summary>
		/// Returns true, if this class or super classes without object class has ToString method.
		/// </summary>
		///
		/// <param name="obj">an object</param>
		/// <returns>true if this class or super classes without object class has ToString method; false otherwise</returns>
		private static bool HasToString(Type type) {
			var result = false;

			try {
				while (type != typeof(object) && type != typeof(ValueType)) {
					if (type.GetMethod("ToString", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance) != null) {
						result = true;
						break;
					}
					type = type.BaseType;
				}
			}
			catch (Exception e) {
			}

			return result;
		}

		/// <summary>
		/// Returns a string representation of the obj uses reflection.
		/// </summary>
		///
		/// <param name="state">the indent state</param>
		/// <param name="strings">the string list</param>
		/// <param name="obj">an object</param>
		private static void AppendReflectString(State state, IList<string> strings, StringBuilder buff, object obj) {
			var type = obj.GetType();
			var extended = type.BaseType != typeof(object) && type.BaseType != typeof(ValueType);
			var isTuple = type.Name.StartsWith("Tuple`") || type.Name.StartsWith("ValueTuple`");
			var multiLine = !isSingleLine(obj);

			buff.Append(isTuple ? '(' : '{');
			if (multiLine) {
				LineFeed(state, strings, buff);
				UpDataNest(state);
			}

			AppendReflectStringSub(state, strings, buff, obj, type, extended, multiLine);

			if (multiLine)
				DownDataNest(state);
			buff.Append(isTuple ? ')' : '}');
		}

		/// <summary>
		/// Returns a string representation of the obj uses reflection.
		/// </summary>
		///
		/// <param name="state">the indent state</param>
		/// <param name="strings">the string list</param>
		/// <param name="obj">an object</param>
		/// <param name="type">the type of the object</param>
		/// <param name="extended">the type is extended type</param>
		private static void AppendReflectStringSub(State state, IList<string> strings, StringBuilder buff, object obj, Type type, bool extended, bool multiLine) {
			Type baseType = type.BaseType;
			if (baseType != typeof(object) && baseType != typeof(ValueType))
				// Call for the base type
				AppendReflectStringSub(state, strings, buff, obj, baseType, extended, multiLine);

			var typeNamePrefix = type.FullName + "#";
			var isTuple = type.Name.StartsWith("Tuple`") || type.Name.StartsWith("ValueTuple`");

			if (extended) {
				buff.Append(string.Format(classBoundaryString, ReplaceTypeName(type.FullName)));
				LineFeed(state, strings, buff);
			}

			// field
			var fieldInfos = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
			int fieldIndex = 0;
			foreach (var fieldInfo in fieldInfos) {
				if (!multiLine && fieldIndex > 0) buff.Append(", ");

				object value = null;
				try {
					value = fieldInfo.GetValue(obj);
				}
				catch (Exception e) {
					value = e.ToString();
				}

				var fieldName = fieldInfo.Name;
				if (!isTuple)
					buff.Append(fieldName).Append(fieldNameValueSeparator);

				if (value != null && nonPrintProperties.Contains(typeNamePrefix + fieldName))
					buff.Append(nonPrintString);
				else
					Append(state, strings, buff, value, false);

				if (multiLine) {
					buff.Append(",");
					LineFeed(state, strings, buff);
				}
				++fieldIndex;
			}

			// property
			var propertyInfos = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
			int propertyIndex = 0;
			foreach (var propertyInfo in propertyInfos) {
				if (!multiLine && propertyIndex > 0) buff.Append(", ");

				object value = null;
				try {
					value = propertyInfo.GetValue(obj);
				}
				catch (Exception e) {
					value = e.ToString();
				}

				var propertyName = propertyInfo.Name;
				if (!isTuple)
					buff.Append(propertyName).Append(fieldNameValueSeparator);

				if (value != null && nonPrintProperties.Contains(typeNamePrefix + propertyName))
					buff.Append(nonPrintString);
				else
					Append(state, strings, buff, value, false);

				if (multiLine) {
					buff.Append(",");
					LineFeed(state, strings, buff);
				}
				++propertyIndex;
			}
		}

		/// <summary>
		/// Returns whether the value should be output on one line.
		/// </summary>
		///
		/// <param name="value">the output value</param>
		/// <param name="isElement">whether the value is an element of the collection</param>
		/// <returns>true if the value should be output on one line, false otherwise</returns>
		private static bool isSingleLine(object value, bool isElement = false) {
			var type = value.GetType();

			if (singleLineTypes.Contains(type)) return true;
			if (value is Enum) return true;
			if (isElement) return false;

			if (value is IDictionary dictinary) {
				var index = 0;
				foreach (var key in dictinary.Keys) {
					if (!isSingleLine(key) || !isSingleLine(dictinary[key], true)) return false;
					if (index++ >= collectionLimit) break;
				}
				return true;
			}

			if (value is IEnumerable values) {
				var index = 0;
				foreach (var element in values) {
					if (!isSingleLine(element, true)) return false;
					if (index++ >= collectionLimit) break;
				}
				return true;
			}

			if (type.BaseType != typeof(object) && type.BaseType != typeof(ValueType))
				return false;

			var isTuple = type.Name.StartsWith("Tuple`") || type.Name.StartsWith("ValueTuple`");

			// property
			var propertyInfos = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
			foreach (var propertyInfo in propertyInfos) {
				try {
					if (!isSingleLine(propertyInfo.GetValue(value), !isTuple))
						return false;
				}
				catch (Exception e) {
					return false;
				}
			}

			// field
			var fieldInfos = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
			foreach (var fieldInfo in fieldInfos) {
				try {
					if (!isSingleLine(fieldInfo.GetValue(value), !isTuple))
						return false;
				}
				catch (Exception e) {
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Returns the last log string output.
		/// </summary>
		///
		/// <returns>the last log string output.</returns>
		public static string LastLog {get => lastLog;}
	}
}

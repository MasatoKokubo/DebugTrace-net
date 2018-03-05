// Trace.cs
// (C) 2018 Masato Kokubo

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;

namespace DebugTrace {
	using DebugTrace.Logger;

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
	public abstract class Trace : ITrace {
		public static string   LogLevel                {get; private set;} // Log Level
		public static string   EnterString             {get; private set;} // string at enter
		public static string   LeaveString             {get; private set;} // string at leave
		public static string   ThreadBoundaryString    {get; private set;} // string of threads boundary
		public static string   ClassBoundaryString     {get; private set;} // string of classes boundary
		public static string   CodeIndentString        {get; private set;} // string of method call indent
		public static string   DataIndentString        {get; private set;} // string of data indent
		public static string   LimitString             {get; private set;} // string to represent that it has exceeded the limit
		public static string   DefaultNameSpaceString  {get; private set;} // string replacing the default package part
		public static string   NonPrintString          {get; private set;} // string of value in the case of properties that do not display the value
		public static string   CyclicReferenceString   {get; private set;} // string to represent that the cyclic reference occurs
		public static string   VarNameValueSeparator   {get; private set;} // Separator between the variable name and value
		public static string   KeyValueSeparator       {get; private set;} // Separator between the key and value for IDictionary obj
		public static string   FieldNameValueSeparator {get; private set;} // Separator between the field name and value
		public static string   PrintSuffixFormat       {get; private set;} // Format string of Print suffix
		public static string   DateTimeFormat          {get; private set;} // Format string of a DateTime and a string
		public static int      CollectionLimit         {get; private set;} // Limit of ICollection elements to output
		public static int      ByteArrayLimit          {get; private set;} // Limit of byte array elements to output
		public static int      StringLimit             {get; private set;} // Limit of string characters to output
		public static string[] NonPrintProperties      {get; private set;} // Non Print properties (<class name>#<property name>)
		public static string   DefaultNameSpace        {get; private set;} // Default package part
		public static string[] ReflectionClasses       {get; private set;} // List of class names that output content in reflection even if ToString method is implemented

		// Array of indent strings
		protected static string[] indentStrings;

		// Array of data indent strings
		protected static string[] dataIndentStrings;

		// Logger
		protected static ILogger logger;

		/// <summary>
		/// Returns whether tracing is IsEnabled.
		/// </summary>
		public bool IsEnabled {get {return logger.IsEnabled;}}

		// Set of classes that dose not output the type name
		protected abstract ISet<Type> NoOutputTypes {get;}

		// Set of element types of array that dose not output the type name
		protected abstract ISet<Type> NoOutputElementTypes {get;}

		protected abstract IDictionary<Type, string> TypeNameMap {get;}

		// Set of component types of array that output on the single line
		protected ISet<Type> SingleLineTypes {get;} = new HashSet<Type>() {
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

		// Dictionary of thread id to the indent state
		protected readonly IDictionary<int, State> states = new Dictionary<int, State>();

		// Before thread id
		protected int beforeThreadId;

		// Reflected objects
		protected readonly ICollection<object> reflectedObjects = new List<object>();

		/// <summary>
		/// Returns the last log string output.
		/// </summary>
		public string LastLog {get; private set;} = "";

		/// <summary>
		/// class constructor
		/// </summary>
		static Trace() {
			InitClass();
		}

		/// <summary>
		/// Initializes this class.
		/// </summary>
		public static void InitClass() {
			Resource resource = new Resource("DebugTrace");

			LogLevel                 = resource.GetString ("LogLevel"               , "default");
			EnterString              = resource.GetString ("EnterString"            , "Enter {0}.{1} ({2}:{3:D})");
			LeaveString              = resource.GetString ("LeaveString"            , "Leave {0}.{1} ({2}:{3:D})");
			ThreadBoundaryString     = resource.GetString ("ThreadBoundaryString"   , "______________________________ Thread {0} ______________________________");
			ClassBoundaryString      = resource.GetString ("ClassBoundaryString"    , "____ {0} ____");
			CodeIndentString         = resource.GetString ("CodeIndentString"       , "| ");
			DataIndentString         = resource.GetString ("DataIndentString"       , "  ");
			LimitString              = resource.GetString ("LimitString"            , "...");
			DefaultNameSpaceString   = resource.GetString ("DefaultNameSpaceString" , "...");
			NonPrintString           = resource.GetString ("NonPrintString"         , "***");
			CyclicReferenceString    = resource.GetString ("CyclicReferenceString"  , " *** cyclic reference *** ");
			VarNameValueSeparator    = resource.GetString ("VarNameValueSeparator"  , " = ");
			KeyValueSeparator        = resource.GetString ("KeyValueSeparator"      , ": ");
			FieldNameValueSeparator  = resource.GetString ("FieldNameValueSeparator", ": ");
			PrintSuffixFormat        = resource.GetString ("PrintSuffixFormat"      , " ({2}:{3:D})");
			DateTimeFormat           = resource.GetString ("DateTimeFormat"         , "{0:G}");
			CollectionLimit          = resource.GetInt    ("CollectionLimit"        , 512);
			ByteArrayLimit           = resource.GetInt    ("ByteArrayLimit"         , 8192);
			StringLimit              = resource.GetInt    ("StringLimit"            , 8192);
			NonPrintProperties       = resource.GetStrings("NonPrintProperties"     , new string[0]);
			DefaultNameSpace         = resource.GetString ("DefaultNameSpace"       , "");
			ReflectionClasses        = resource.GetStrings("ReflectionClasses"      , new string[0]);

			// Array of indent strings
			indentStrings = new string[64];

			// Array of data indent strings
			dataIndentStrings = new string[64];

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
			logger.Level = LogLevel;

			// make code indent strings
			indentStrings[0] = "";
			for (var index = 1; index < indentStrings.Length; ++index)
				indentStrings[index] = indentStrings[index - 1] + CodeIndentString;

			// make data indent strings
			dataIndentStrings[0] = "";
			for (var index = 1; index < dataIndentStrings.Length; ++index)
				dataIndentStrings[index] = dataIndentStrings[index - 1] + DataIndentString;

			// output version log
			var versionAttribute = (AssemblyInformationalVersionAttribute)
				Attribute.GetCustomAttribute(Resource.SelfAssembly, typeof(AssemblyInformationalVersionAttribute));
			logger.Log($"DebugTrace-net {versionAttribute?.InformationalVersion}");
			logger.Log($"  properties: {resource.FileInfo.FullName}");
			logger.Log($"  Logger: {logger.GetType().FullName}");
		}

		/// Returns the indent state of the current thread.
		/// </summary>
		///
		/// <returns>the indent state of the current thread</returns>
		private protected State GetCurrentState() {
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

		/// <summary>
		/// Returns a string corresponding to the current indent.
		/// </summary>
		protected string GetIndentString(State state) {
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
		protected void UpNest(State state) {
			state.BeforeNestLevel = state.NestLevel;
			++state.NestLevel;
		}

		/// <summary>
		/// Down the nest level.
		/// </summary>
		protected void DownNest(State state) {
			state.BeforeNestLevel = state.NestLevel;
			--state.NestLevel;
		}

		/// <summary>
		/// Up the data nest level.
		/// </summary>
		protected void UpDataNest(State state) {
			++state.DataNestLevel;
		}

		/// <summary>
		/// Down the data nest level.
		/// </summary>
		protected void DownDataNest(State state) {
			--state.DataNestLevel;
		}

		/// <summary>
		/// Common start processing of output.
		/// </summary>
		protected void PrintStart() {
			var thread = Thread.CurrentThread;
			int threadId = thread.ManagedThreadId;
			if (threadId !=  beforeThreadId) {
				// Thread changing
				logger.Log(""); // Line break
				logger.Log(string.Format(ThreadBoundaryString, threadId));
				logger.Log(""); // Line break

				beforeThreadId = threadId;
			}
		}

		/// <summary>
		/// Common end processing of output.
		/// </summary>
		protected void PrintEnd() {
			beforeThreadId = Thread.CurrentThread.ManagedThreadId;
		}

		/// <summary>
		/// Resets the nest level
		/// </summary>
		public void ResetNest() {
			if (!IsEnabled) return;

			lock (states) {
				GetCurrentState().Reset();
			}
		}

		/// <summary>
		/// Call this method at entrance of your methods.
		/// </summary>
		public void Enter() {
			if (!IsEnabled) return;

			lock (states) {
				PrintStart(); // Common start processing of output

				var state = GetCurrentState();
				if (state.BeforeNestLevel > state.NestLevel)
					logger.Log(GetIndentString(state)); // Line break

				LastLog = GetIndentString(state) + GetCallerInfo(EnterString);
				logger.Log(LastLog);

				UpNest(state);

				PrintEnd(); // Common end processing of output
			}
		}

		/// <summary>
		/// Call this method at exit of your methods.
		/// </summary>
		public void Leave() {
			if (!IsEnabled) return;

			lock (states) {
				PrintStart(); // Common start processing of output

				var state = GetCurrentState();
				DownNest(state);

				LastLog = GetIndentString(state) + GetCallerInfo(LeaveString);
				logger.Log(LastLog);

				PrintEnd(); // Common end processing of output
			}
		}

		/// <summary>
		/// Returns a string of the caller information.
		/// </summary>
		protected string GetCallerInfo(string baseString) {
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
		public void Print(string message) {
			if (!IsEnabled) return;
			PrintSub(message);
		}

		/// <summary>
		/// Outputs a message to the log.
		/// </summary>
		///
		/// <param name="messageSupplier">a message supplier</param>
		public void Print(Func<string> messageSupplier) {
			if (!IsEnabled) return;
			PrintSub(messageSupplier());
		}

		/// <summary>
		/// Outputs the message to the log.
		/// </summary>
		protected void PrintSub(string message) {
			lock (states) {
				PrintStart(); // Common start processing of output

				var lastLog = "";
				if (message != "") {
					var element = GetStackTraceElement();
					var suffix = string.Format(PrintSuffixFormat,
						ReplaceTypeName(element.TypeName),
						element.MethodName,
						element.FileName,
						element.LineNumber);
					lastLog = GetIndentString(GetCurrentState()) + message + suffix;
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
		protected void PrintSub(string name, object value) {
			lock (states) {
				PrintStart(); // Common start processing of output

				reflectedObjects.Clear();

				var state = GetCurrentState();
				var strings = new List<string>();
				var buff = new StringBuilder();

				buff.Append(name).Append(VarNameValueSeparator);
				var normalizedName = name.Substring(name.LastIndexOf('.') + 1).Trim();
				normalizedName = normalizedName.Substring(normalizedName.LastIndexOf(' ') + 1);
				Append(state, strings, buff, value, false);

				var element = GetStackTraceElement();
				var suffix = string.Format(PrintSuffixFormat,
					element.TypeName,
					element.MethodName,
					element.FileName,
					element.LineNumber);
				buff.Append(suffix);
				LineFeed(state, strings, buff);

				strings.ForEach(str => logger.Log(str));
				LastLog = string.Join("\n", strings);

				PrintEnd(); // Common end processing of output
			}
		}

		/// <summary>
		/// Returns a caller stack trace element.
		/// </summary>
		///
		/// <returns>a caller stack trace element</returns>
		protected StackTraceElement GetStackTraceElement() {
			var elements = Environment.StackTrace.Split('\n')
				.Select(str => str.Trim())
				.Where(str => str != "" && !str.Contains("DebugTrace.Trace.") && !str.Contains("StackTrace"))
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

		protected static (string, string) Split(string str, char separator) {
			var index = str.IndexOf(separator);
			return index < 0
				? (str, "")
				: (str.Substring(0, index), str.Substring(index + 1));
		}

		protected static (string, string) LastSplit(string str, char separator) {
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
		protected void LineFeed(State state, IList<string> strings, StringBuilder buff) {
			strings.Add(GetIndentString(GetCurrentState()) + buff.ToString());
			buff.Clear();
		}

		/// <summary>
		/// Outputs the name and value to the log.
		/// </summary>
		///
		/// <param name="name">the name of the value</param>
		/// <param name="value">the value to output (accept null)</param>
		public void Print(string name, object value) {
			if (!IsEnabled) return;
			PrintSub(name, value);
		}

		/// <summary>
		/// Outputs the name and value to the log.
		/// </summary>
		///
		/// <param name="">T> type of the value</param>
		/// <param name="name">the name of the value</param>
		/// <param name="valueSupplier">the supplier of value to output</param>
		public void Print<T>(string name, Func<T> valueSupplier) {
			if (!IsEnabled) return;
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
		protected abstract void Append(State state, IList<string> strings, StringBuilder buff, object value, bool isElement);

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
		protected abstract string GetTypeName(Type type, object value, bool isElement, int nest = 0);

		protected Regex typeRemoveRegex = new Regex(@"(`[0-9]+)|(, [^, \]]+)+");

		/// <summary>
		/// Replace a class name.
		/// </summary>
		///
		/// <param name="typeName">a class name</param>
		/// <returns>the replaced ckass name</returns>
		protected string ReplaceTypeName(string typeName) {
			typeName = typeRemoveRegex.Replace(typeName, "");

			if (DefaultNameSpace != "" && typeName.StartsWith(DefaultNameSpace))
				typeName = DefaultNameSpaceString + typeName.Substring(DefaultNameSpace.Length);
			return typeName;
		}

		/// <summary>
		/// Appends a character representation for logging to the string buffer.
		/// </summary>
		///
		/// <param name="buff">the string buffer</param>
		/// <param name="ch">a character</param>
		/// <param name="enclosure">the enclosure character</param>
		/// <param name="escape">escape characters if true, dose not escape characters otherwise</param>
		/// <returns>true if successful, false otherwise<returns>
		protected bool Append(StringBuilder buff, char ch, char enclosure, bool escape) {
			if (escape) {
				// escape
				switch (ch) {
				case '\0': buff.Append(@"\0"); break; // 00 NUL
				case '\a': buff.Append(@"\a"); break; // 07 BEL
				case '\b': buff.Append(@"\b"); break; // 08 BS
				case '\t': buff.Append(@"\t"); break; // 09 HT
				case '\n': buff.Append(@"\n"); break; // 0A LF
				case '\v': buff.Append(@"\v"); break; // 0B VT
				case '\f': buff.Append(@"\f"); break; // 0C FF
				case '\r': buff.Append(@"\r"); break; // 0D CR
				case '\\': buff.Append(@"\\"); break; // \
				default:
					if (ch < ' ' || ch == '\u007F')
						buff.Append(string.Format(@"\u{0:X4}", (ushort)ch));
					else {
						if (ch == enclosure)
							buff.Append('\\');
						buff.Append(ch);
					}
					break;
				}
			} else {
				// dose not escape
				if (ch < ' ' || ch == '\u007F' || ch == enclosure)
					return false;
				else
					buff.Append(ch);
			}
			return true;
		}

		/// <summary>
		/// Appends a CharSequence representation for logging to the string buffer.
		/// </summary>
		///
		/// <param name="state">the indent state</param>
		/// <param name="strings">the string list</param>
		/// <param name="buff">the string buffer</param>
		/// <param name="str">a string object</param>
		/// <returns>true if successful, false otherwise<returns>
		protected bool Append(StringBuilder buff, string str, bool escape) {
			var beforeLength = buff.Length;
			var needAtChar = false;
			buff.Append('"');
			for (int index = 0; index < str.Length; ++index) {
				if (index >= StringLimit) {
					buff.Append(LimitString);
					break;
				}
				var ch = str[index];
				if (!Append(buff, ch, '"', escape)) {
					buff.Length = beforeLength;
					return false;
				}
				if (!escape && ch == '\\')
					needAtChar = true;
			}
			buff.Append('"');
			if (needAtChar)
				buff.Insert(beforeLength, '@');
			return true;
		}

		/// <summary>
		/// Appends a Collection representation for logging to the string buffer.
		/// </summary>
		///
		/// <param name="state">the indent state</param>
		/// <param name="strings">the string list</param>
		/// <param name="buff">the string buffer</param>
		/// <param name="collection">a Collection object</param>
		protected void Append(State state, IList<string> strings, StringBuilder buff, ICollection collection) {
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

				if (index < CollectionLimit)
					Append(state, strings, buff, element, true);
				else
					buff.Append(LimitString);

				if (multiLine) {
					buff.Append(",");
					LineFeed(state, strings, buff);
				}

				if (index++ >= CollectionLimit) break;
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
		protected void Append(State state, IList<string> strings, StringBuilder buff, IDictionary dictionary) {
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

				if (index < CollectionLimit) {
					Append(state, strings, buff, key, true);
					buff.Append(KeyValueSeparator);
					Append(state, strings, buff, value, true);
				} else
					buff.Append(LimitString);

				if (multiLine) {
					buff.Append(",");
					LineFeed(state, strings, buff);
				}

				if (index++ >= CollectionLimit) break;
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
		protected bool HasToString(Type type) {
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
			catch (Exception) {
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
		protected void AppendReflectString(State state, IList<string> strings, StringBuilder buff, object obj) {
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
		protected void AppendReflectStringSub(State state, IList<string> strings, StringBuilder buff, object obj, Type type, bool extended, bool multiLine) {
			Type baseType = type.BaseType;
			if (baseType != typeof(object) && baseType != typeof(ValueType))
				// Call for the base type
				AppendReflectStringSub(state, strings, buff, obj, baseType, extended, multiLine);

			var typeNamePrefix = type.FullName + "#";
			var isTuple = type.Name.StartsWith("Tuple`") || type.Name.StartsWith("ValueTuple`");

			if (extended) {
				buff.Append(string.Format(ClassBoundaryString, ReplaceTypeName(type.FullName)));
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
					buff.Append(fieldName).Append(FieldNameValueSeparator);

				if (value != null && NonPrintProperties.Contains(typeNamePrefix + fieldName))
					buff.Append(NonPrintString);
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
					buff.Append(propertyName).Append(FieldNameValueSeparator);

				if (value != null && NonPrintProperties.Contains(typeNamePrefix + propertyName))
					buff.Append(NonPrintString);
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
		protected bool isSingleLine(object value, bool isElement = false) {
			if (value == null) return true;
			var type = value.GetType();
			if (SingleLineTypes.Contains(type)) return true;
			if (value is Enum) return true;
			if (isElement) return false;

			if (value is IDictionary dictinary) {
				var index = 0;
				foreach (var key in dictinary.Keys) {
					if (!isSingleLine(key) || !isSingleLine(dictinary[key], true)) return false;
					if (index++ >= CollectionLimit) break;
				}
				return true;
			}

			if (value is IEnumerable values) {
				var index = 0;
				foreach (var element in values) {
					if (!isSingleLine(element, true)) return false;
					if (index++ >= CollectionLimit) break;
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
				catch (Exception) {
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
				catch (Exception) {
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Append a timestamp to the head of the string.
		/// </summary>
		///
		/// <param name="string ">a string</param>
		/// <returns>a string appended a timestamp string</returns>
		public static String AppendDateTime(string str) {
			return string.Format(DateTimeFormat, DateTime.Now) + " " + str;
		}
	}
}

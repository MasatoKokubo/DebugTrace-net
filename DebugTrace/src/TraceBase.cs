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
    public abstract class TraceBase : ITrace {
        public static Resource Resource {get; private set;}

        public static string   EnterString             {get; set;} // string at enter
        public static string   LeaveString             {get; set;} // string at leave
        public static string   ThreadBoundaryString    {get; set;} // string of threads boundary
        public static string   ClassBoundaryString     {get; set;} // string of classes boundary
        public static string   CodeIndentString        {get; set;} // string of method call indent
        public static string   DataIndentString        {get; set;} // string of data indent
        public static string   LimitString             {get; set;} // string to represent that it has exceeded the limit
        public static string   DefaultNameSpaceString  {get; set;} // string replacing the default package part
        public static string   NonPrintString          {get; set;} // string of value in the case of properties that do not display the value
        public static string   CyclicReferenceString   {get; set;} // string to represent that the cyclic reference occurs
        public static string   VarNameValueSeparator   {get; set;} // Separator between the variable name and value
        public static string   KeyValueSeparator       {get; set;} // Separator between the key and value for IDictionary object or property/field and value
        public static string   PrintSuffixFormat       {get; set;} // Format string of Print suffix
        public static string   DateTimeFormat          {get; set;} // Format string of a DateTime and a string
        public static int      MaxDataOutputWidth      {get; set;} // Maximum data output width
        public static int      CollectionLimit         {get; set;} // Limit of ICollection elements to output
        public static int      StringLimit             {get; set;} // Limit of string characters to output
        public static int      ReflectionNestLimit     {get; set;} // Limit of reflection nesting
        public static string[] NonPrintProperties      {get; set;} // Non Print properties (<class name>#<property name>)
        public static string   DefaultNameSpace        {get; set;} // Default package part
        public static ISet<string> ReflectionClasses   {get; set;} // Class names that output content in reflection even if ToString method is implemented

        // Array of indent strings
        protected static string[] indentStrings;

        // Array of data indent strings
        protected static string[] dataIndentStrings;

        /// <summary>
        /// Returns the logger.
        /// </summary>
        public static ILogger Logger {get; set;} = Console.Error.Instance; // the logger

        /// <summary>
        /// Returns whether tracing is IsEnabled.
        /// </summary>
        public bool IsEnabled {get => Logger.IsEnabled;}

        // Set of classes that dose not output the type name
        protected abstract ISet<Type> NoOutputTypes {get;}

        // Set of element types of array that dose not output the type name
        protected abstract ISet<Type> NoOutputElementTypes {get;}

        // Dictionary of type to type name
        protected abstract IDictionary<Type, string> TypeNameMap {get;}

        // Dictionary of thread id to indent state
        protected readonly IDictionary<int, State> states = new Dictionary<int, State>();

        // Before thread id
        protected int beforeThreadId;

        // Reflected objects
        protected readonly IList<object> reflectedObjects = new List<object>();

        /// <summary>
        /// Returns the last log string output.
        /// </summary>
        public string LastLog {get; private set;} = "";

        /// <summary>
        /// class constructor
        /// </summary>
        static TraceBase() {
            InitClass();
        }

        /// <summary>
        /// Initializes this class.
        /// </summary>
        public static void InitClass() {
            Resource = new Resource("DebugTrace");

            EnterString              = Resource.GetString (nameof(EnterString            ), Resource.Unescape(@"Enter {0}.{1} ({2}:{3:D})"));
            LeaveString              = Resource.GetString (nameof(LeaveString            ), Resource.Unescape(@"Leave {0}.{1} ({2}:{3:D})"));
            ThreadBoundaryString     = Resource.GetString (nameof(ThreadBoundaryString   ), Resource.Unescape(@"______________________________ Thread {0} ______________________________"));
            ClassBoundaryString      = Resource.GetString (nameof(ClassBoundaryString    ), Resource.Unescape(@"____ {0} ____"));
            CodeIndentString         = Resource.GetString (nameof(CodeIndentString       ), Resource.Unescape(@"|\s"));
            DataIndentString         = Resource.GetString (nameof(DataIndentString       ), Resource.Unescape(@"\s\s"));
            LimitString              = Resource.GetString (nameof(LimitString            ), Resource.Unescape(@"..."));
            DefaultNameSpaceString   = Resource.GetString (nameof(DefaultNameSpaceString ), Resource.Unescape(@"..."));
            NonPrintString           = Resource.GetString (nameof(NonPrintString         ), Resource.Unescape(@"***"));
            CyclicReferenceString    = Resource.GetString (nameof(CyclicReferenceString  ), Resource.Unescape(@"*** Cyclic Reference ***"));
            VarNameValueSeparator    = Resource.GetString (nameof(VarNameValueSeparator  ), Resource.Unescape(@"\s=\s"));
            KeyValueSeparator        = Resource.GetString (nameof(KeyValueSeparator      ), Resource.Unescape(@":\s"));
            PrintSuffixFormat        = Resource.GetString (nameof(PrintSuffixFormat      ), Resource.Unescape(@"\s({2}:{3:D})"));
            DateTimeFormat           = Resource.GetString (nameof(DateTimeFormat         ), Resource.Unescape(@"{0:yyyy-MM-dd HH:mm:ss.fff}"));
            MaxDataOutputWidth       = Resource.GetInt    (nameof(MaxDataOutputWidth     ), 80);
            CollectionLimit          = Resource.GetInt    (nameof(CollectionLimit        ), 512);
            StringLimit              = Resource.GetInt    (nameof(StringLimit            ), 8192);
            ReflectionNestLimit      = Resource.GetInt    (nameof(ReflectionNestLimit    ), 4);
            NonPrintProperties       = Resource.GetStrings(nameof(NonPrintProperties     ), new string[0]);
            DefaultNameSpace         = Resource.GetString (nameof(DefaultNameSpace       ), "");
            ReflectionClasses        = new HashSet<string>(Resource.GetStrings(nameof(ReflectionClasses), new string[0]));
            ReflectionClasses.Add(typeof(Tuple).FullName); // Tuple
            ReflectionClasses.Add(typeof(ValueTuple).FullName); // ValueTuple

            // Array of indent strings
            indentStrings = new string[64];

            // Array of data indent strings
            dataIndentStrings = new string[64];

            string loggerName = Resource.GetString("Logger", null);
            if (loggerName != null) {
                Exception e1 = null;
                Exception e2 = null;
                ILogger logger = null;
                if (!loggerName.Split(',')[0].Contains('.'))
                    // Add default namesapce if no namespace
                    loggerName = typeof(ILogger).Namespace + '.' + loggerName;
                try {
                    logger = (ILogger)Type.GetType(loggerName)
                        .GetProperty(nameof(Console.Out.Instance), BindingFlags.Public | BindingFlags.Static)
                        .GetValue(null);
                    if (logger != null)
                        Logger = logger;
                }
                catch (Exception e) {
                    e1 = e;
                }
                if (logger == null && !loggerName.Contains(',')) {
                    // Try with the class name that added the assembly name
                    loggerName = loggerName + ',' + loggerName;
                    try {
                        logger = (ILogger)Type.GetType(loggerName)
                            .GetProperty(nameof(Console.Out.Instance), BindingFlags.Public | BindingFlags.Static)
                            .GetValue(null);
                        if (logger != null)
                            Logger = logger;
                    }
                    catch (Exception e) {
                        e2 = e;
                    }
                }
                if (logger == null) {
                    if (e2 != null)
                        System.Console.Error.WriteLine($"DebugTrace-net: {e2.ToString()}({loggerName})");
                    else if (e1 != null)
                        System.Console.Error.WriteLine($"DebugTrace-net: {e1.ToString()}({loggerName})");
                }
            }

            // Set the logging level
            var logLevel = Resource.GetString("LogLevel", null);
            if (logLevel != null)
                Logger.Level = logLevel;

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
            Logger.Log($"DebugTrace-net {versionAttribute?.InformationalVersion}");
            Logger.Log($"  Properties file: {Resource.FileInfo.FullName}");
            Logger.Log($"  Logger wrapper: {Logger.GetType().FullName}");
        }

        /// <summary>
        /// Returns the indent state of the current thread.
        /// </summary>
        ///
        /// <returns>the indent state of the current thread</returns>
        /// <param name="threadId">the thread id</returns>
        private protected State GetCurrentState(int threadId = -1) {
            State state;
            if (threadId == -1)
                threadId = Thread.CurrentThread.ManagedThreadId;

            if (states.ContainsKey(threadId)) {
                state = states[threadId];
            } else {
                state = new State();
                state.ThreadId = threadId;
                states[threadId] = state;
            }

            return state;
        }

        /// <summary>
        /// Returns a string corresponding to the current indent.
        /// </summary>
        protected string GetIndentString(int nestLevel, int dataNestLevel) {
            return indentStrings[
                nestLevel < 0 ? 0 :
                nestLevel >= indentStrings.Length ? indentStrings.Length - 1
                    : nestLevel]
            + dataIndentStrings[
                dataNestLevel < 0 ? 0 :
                dataNestLevel >= dataIndentStrings.Length ? dataIndentStrings.Length - 1
                    : dataNestLevel];
        }

        /// <summary>
        /// Common start processing of output.
        /// </summary>
        /// <param name="state">the state</param>
        protected void PrintStart(State state) {
            if (state.ThreadId !=  beforeThreadId) {
                // Thread changing
                Logger.Log(""); // Line break
                Logger.Log(string.Format(ThreadBoundaryString, state.ThreadId));
                Logger.Log(""); // Line break

                beforeThreadId = state.ThreadId;
            }
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
        /// <returns>current thread id</returns>
        public int Enter() {
            if (!IsEnabled) return -1;

            lock (states) {
                var state = GetCurrentState();

                PrintStart(state); // Common start processing of output

                if (state.BeforeNestLevel > state.NestLevel)
                    Logger.Log(GetIndentString(state.NestLevel, 0)); // Line break

                LastLog = GetIndentString(state.NestLevel, 0) + GetCallerInfo(EnterString);
                Logger.Log(LastLog);

                state.UpNest();

                return state.ThreadId;
            }
        }

        /// <summary>
        /// Call this method at exit of your methods.
        /// </summary>
        /// <param name="threadId">the thread id</param>
        public void Leave(int threadId = -1) {
            if (!IsEnabled) return;

            lock (states) {
                var state = GetCurrentState(threadId);
                PrintStart(state); // Common start processing of output

                state.DownNest();

                LastLog = GetIndentString(state.NestLevel, 0) + GetCallerInfo(LeaveString);
                Logger.Log(LastLog);
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
                var state = GetCurrentState();
                PrintStart(state); // Common start processing of output

                LastLog = "";
                if (message != "") {
                    var element = GetStackTraceElement();
                    var suffix = string.Format(PrintSuffixFormat,
                        ReplaceTypeName(element.TypeName),
                        element.MethodName,
                        element.FileName,
                        element.LineNumber);
                    LastLog = GetIndentString(state.NestLevel, 0) + message + suffix;
                }
                Logger.Log(LastLog);
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
                var state = GetCurrentState();
                PrintStart(state); // Common start processing of output

                reflectedObjects.Clear();

                var buff = new LogBuffer();

                buff.Append(name).Append(VarNameValueSeparator);
                var normalizedName = name.Substring(name.LastIndexOf('.') + 1).Trim();
                normalizedName = normalizedName.Substring(normalizedName.LastIndexOf(' ') + 1);
                Append(buff, value, false);

                var element = GetStackTraceElement();
                var suffix = string.Format(PrintSuffixFormat,
                    element.TypeName,
                    element.MethodName,
                    element.FileName,
                    element.LineNumber);
                buff.Append(suffix);
                buff.LineFeed();

                var lastLogBuff = new StringBuilder();
                foreach ((int dataNestLevel, string line) in buff.Lines) {
                    var log = GetIndentString(state.NestLevel, dataNestLevel) + line;
                    Logger.Log(log);
                    lastLogBuff.Append(log).Append('\n');
                }
                LastLog = lastLogBuff.ToString();
            }
        }

        private static string thisClassFullName = typeof(TraceBase).FullName + '.';

        /// <summary>
        /// Returns a caller stack trace element.
        /// </summary>
        ///
        /// <returns>a caller stack trace element</returns>
        protected StackTraceElement GetStackTraceElement() {
            var elements = Environment.StackTrace.Split('\n')
                .Select(str => str.Trim())
                .Where(str => str != "" && !str.Contains(thisClassFullName) && !str.Contains("StackTrace"))
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
        /// <param name="name">the name of the value</param>
        /// <param name="valueSupplier">the supplier of value to output</param>
        public void Print(string name, Func<object> valueSupplier) {
            if (!IsEnabled) return;
            PrintSub(name, valueSupplier());
        }

        /// <summary>
        /// Appends the value for logging to the string buffer.
        /// </summary>
        ///
        /// <param name="buff">the logging buffer</param>
        /// <param name="value">the value object</param>
        /// <param name="isElement">true if the value is element of a container class, false otherwise</param>
        /// <returns>isMultiLine">true if output multiple lines, false otherwise</returns>
        protected bool Append(LogBuffer buff, object value, bool isElement) {
            bool isMultiLine = false;

            if (value == null) {
                buff.Append("null");
            } else {
                var type = value.GetType();

                var typeName = GetTypeName(type, value, isElement);

                switch (value) {
                case bool       boolValue: buff.Append(typeName); Append(buff,    boolValue); break;
                case char       charValue: buff.Append(typeName); Append(buff,    charValue); break;
                case sbyte     sbyteValue: buff.Append(typeName); Append(buff,   sbyteValue); break;
                case byte       byteValue: buff.Append(typeName); Append(buff,    byteValue); break;
                case short     shortValue: buff.Append(typeName); Append(buff,   shortValue); break;
                case ushort   ushortValue: buff.Append(typeName); Append(buff,  ushortValue); break;
                case int         intValue: buff.Append(typeName); Append(buff,     intValue); break;
                case uint       uintValue: buff.Append(typeName); Append(buff,    uintValue); break;
                case long       longValue: buff.Append(typeName); Append(buff,    longValue); break;
                case ulong     ulongValue: buff.Append(typeName); Append(buff,   ulongValue); break;
                case float     floatValue: buff.Append(typeName); Append(buff,   floatValue); break;
                case double   doubleValue: buff.Append(typeName); Append(buff,  doubleValue); break;
                case decimal decimalValue: buff.Append(typeName); Append(buff, decimalValue); break;
                case DateTime    dateTime: buff.Append(typeName); Append(buff,     dateTime); break;

                case string   stringValue:
                    buff.Append(typeName);
                    AppendString(buff, stringValue, false);
                    break;

                case IDictionary dictionary:
                    isMultiLine = AppendDictionary(buff, dictionary, false);
                    break;

                case ICollection collection:
                    isMultiLine = AppendCollection(buff, collection, false);
                    break;

                case Enum enumValue: buff.Append(typeName); buff.Append(enumValue); break;

                default:
                    // Other
                    var fullTypeName = GetFullTypeName(type);
                    bool isReflection = ReflectionClasses.Contains(fullTypeName);
                    if (!isReflection && !HasToString(type)) {
                        isReflection = true;
                        ReflectionClasses.Add(fullTypeName);
                    }

                    if (isReflection) {
                        // Use Reflection
                        if (reflectedObjects.Any(obj => object.ReferenceEquals(value, obj)))
                            // Cyclic reference
                            buff.Append(CyclicReferenceString);

                        else if (reflectedObjects.Count > ReflectionNestLimit)
                            // Over reflection level limitation
                            buff.Append(LimitString);

                        else {
                            // Use Reflection
                            reflectedObjects.Add(value);
                            isMultiLine = AppendUsedReflection(buff, value, false);
                            reflectedObjects.RemoveAt(reflectedObjects.Count - 1);
                        }
                    } else {
                        // Use ToString method
                        buff.Append(typeName).Append(value);
                    }
                    break;
                }
            }

            return isMultiLine;
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
        protected string GetTypeName(Type type, object value, bool isElement, int nest = 0) {
            if (type.IsArray) {
                // Array
                return GetArrayTypeName(type, value, isElement, nest);

            } else {
                string typeName = GetTypeName(type);

                if (typeName.StartsWith("ValueTuple")) {
                    // (x, y)
                    typeName = "";

                } else {
                    // Not Array
                    var noOutputType = isElement ? NoOutputElementTypes.Contains(type) : NoOutputTypes.Contains(type);
                    if (noOutputType)
                        typeName = "";
                    if (nest > 0 || !noOutputType) {
                        // Output the type name
                        if (TypeNameMap.ContainsKey(type)) {
                            typeName = TypeNameMap[type];
                        } else {
                            typeName = ReplaceTypeName(typeName);
                            if (value is ICollection collection)
                                typeName += " Count:" + collection.Count;
                        }
                    }
                    if (typeName != "" && nest == 0)
                        typeName += ' ';
                }
                return typeName;
            }
        }

        protected abstract string GetArrayTypeName(Type type, object value, bool isElement, int nest);

        // GetTypeName
        private string GetTypeName(Type type) {
            var builder = new StringBuilder();
            AppendTypeName(builder, type);
            return  builder.ToString();
        }

        // AppendTypeName
        private void AppendTypeName(StringBuilder builder, Type type) {
            var typeName = GetFullTypeName(type);
            if (typeName == "System.Tuple")
                typeName = "Tuple";
            else if (typeName == "System.ValueTuple")
                typeName = "ValueTuple";
            builder.Append(typeName);

            var genericTypes = type.GenericTypeArguments;
            if (genericTypes.Length > 0) {
                var delimiter = "";
                builder.Append('<');
                foreach (var genericType in genericTypes) {
                    builder.Append(delimiter);
                    AppendTypeName(builder, genericType);
                    delimiter = ", ";
                }
                builder.Append('>');
            }
        }

        // GetFullTypeName
        private string GetFullTypeName(Type type) {
            if (TypeNameMap.ContainsKey(type))
                return TypeNameMap[type];

            string typeName = type.Namespace + '.' + type.Name;
            var backquoteIndex = typeName.IndexOf('`');
            if (backquoteIndex >= 0)
                typeName = typeName.Substring(0, backquoteIndex);

            return typeName;
        }

        /// <summary>
        /// Replace a class name.
        /// </summary>
        ///
        /// <param name="typeName">a class name</param>
        /// <returns>the replaced ckass name</returns>
        protected string ReplaceTypeName(string typeName) {
            if (DefaultNameSpace != "" && typeName.StartsWith(DefaultNameSpace))
                typeName = DefaultNameSpaceString + typeName.Substring(DefaultNameSpace.Length);
            return typeName;
        }

        protected abstract void Append(LogBuffer buff, bool     value);
        protected abstract void Append(LogBuffer buff, char     value);
        protected abstract void Append(LogBuffer buff, sbyte    value);
        protected abstract void Append(LogBuffer buff, byte     value);
        protected abstract void Append(LogBuffer buff, short    value);
        protected abstract void Append(LogBuffer buff, ushort   value);
        protected abstract void Append(LogBuffer buff, int      value);
        protected abstract void Append(LogBuffer buff, uint     value);
        protected abstract void Append(LogBuffer buff, long     value);
        protected abstract void Append(LogBuffer buff, ulong    value);
        protected abstract void Append(LogBuffer buff, float    value);
        protected abstract void Append(LogBuffer buff, double   value);
        protected abstract void Append(LogBuffer buff, decimal  value);
        protected abstract void Append(LogBuffer buff, DateTime value);

        /// <summary>
        /// Appends a character representation for logging to the string buffer.
        /// </summary>
        ///
        /// <param name="buff">the logging buffer</param>
        /// <param name="ch">a character</param>
        /// <param name="enclosure">the enclosure character</param>
        /// <param name="escape">escape characters if true, dose not escape characters otherwise</param>
        /// <returns>true if successful, false otherwise<returns>
        protected bool AppendChar(LogBuffer buff, char ch, char enclosure, bool escape) {
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
        /// <param name="buff">the logging buffer</param>
        /// <param name="str">a string object</param>
        /// <param name="escape">escape characters if true, dose not escape characters otherwise</param>
        protected void AppendString(LogBuffer buff, string str, bool escape) {
            buff.Save(); // Save current point
            var needAtChar = false;
            buff.Append('"');
            for (int index = 0; index < str.Length; ++index) {
                if (index >= StringLimit) {
                    buff.Append(LimitString);
                    break;
                }
                var ch = str[index];
                if (!AppendChar(buff, ch, '"', escape)) {
                    buff.Restore(); // Restore saved point
                    buff.PopSave(); // Pop saveed point
                    AppendString(buff, str, true);
                    return;
                }
                if (!escape && ch == '\\')
                    needAtChar = true;
            }
            buff.Append('"');
            if (needAtChar)
                buff.Insert(buff.PeekSave().builderLength, '@');
            buff.PopSave(); // Pop saveed point
        }

        /// <summary>
        /// Appends a IDictionary representation for logging to the string buffer.
        /// </summary>
        ///
        /// <param name="strings">the string list</param>
        /// <param name="buff">the string buffer</param>
        /// <param name="dictionary">a IDictionary</param>
        /// <param name="isMultiLine">output multiple lines if true, single line otherwise</param>
        /// <returns>false if outputed on a single line, otherwise true</returns>
        protected bool AppendDictionary(LogBuffer buff, IDictionary dictionary, bool isMultiLine) {
            buff.Save(); // Save current point
            buff.Append(GetTypeName(dictionary.GetType(), dictionary, false));
            buff.Append('{');
            var index = 0;

            bool lineFeeded = false;
            bool success = true;
            foreach (var key in dictionary.Keys) {
                if (isMultiLine) {
                    if (index == 0) {
                        buff.LineFeed();
                        if (!lineFeeded) {
                            buff.UpNest();
                            lineFeeded = true;
                        }
                    }
                }

                if (index >= CollectionLimit) {
                    buff.Append(LimitString).Append(", ");
                    break;
                }

                var value = dictionary[key];

                buff.Save(); // Save current point
                bool elementIsMultiLine = Append(buff, key, false);
                buff.Append(KeyValueSeparator);
                elementIsMultiLine |= Append(buff, value, false);

                if (elementIsMultiLine || buff.Length > MaxDataOutputWidth) {
                    if (!isMultiLine) {
                        success = false; // can not be outputed on a single line
                        buff.PopSave(); // Pop saveed point
                        break;
                    }

                    if (buff.PeekSave().builderLength > 0) {
                        buff.Restore(); // Restore saved point
                        buff.LineFeed();
                        elementIsMultiLine = Append(buff, key, false);
                        buff.Append(KeyValueSeparator);
                        elementIsMultiLine |= Append(buff, value, false);
                    }
                }
                buff.PopSave(); // Pop saveed point

                buff.Append(", ");
                if (elementIsMultiLine)
                    buff.LineFeed();

                ++index;
            }

            if (!success) {
                buff.Restore(); // Restore saved point
                buff.PopSave(); // Pop saveed point
                AppendDictionary(buff, dictionary, true);
                return true;
            }

            if (dictionary.Count > 0 && !lineFeeded)
                buff.Length -= 2;

            if (lineFeeded) {
                if (buff.Length > 0)
                    buff.LineFeed();
                buff.DownNest();
            }
            buff.Append('}');

            buff.PopSave(); // Pop saveed point
            return isMultiLine;
        }

        /// <summary>
        /// Appends a Collection representation for logging to the string buffer.
        /// </summary>
        ///
        /// <param name="buff">the logging buffer</param>
        /// <param name="collection">a Collection object</param>
        /// <param name="isMultiLine">output multiple lines if true, single line otherwise</param>
        /// <returns>false if outputed on a single line, otherwise true</returns>
        protected bool AppendCollection(LogBuffer buff, ICollection collection, bool isMultiLine) {
            buff.Save(); // Save current point
            buff.Append(GetTypeName(collection.GetType(), collection, false));
            buff.Append('{');
            var index = 0;

            bool lineFeeded = false;
            bool success = true;
            foreach (var element in collection) {
                if (isMultiLine) {
                    if (index == 0) {
                        buff.LineFeed();
                        if (!lineFeeded) {
                            buff.UpNest();
                            lineFeeded = true;
                        }
                    }
                }

                if (index >= CollectionLimit) {
                    buff.Append(LimitString).Append(", ");
                    break;
                }

                buff.Save(); // Save current point
                bool elementIsMultiLine = Append(buff, element, true);
                if (elementIsMultiLine || buff.Length > MaxDataOutputWidth) {
                    if (!isMultiLine) {
                        success = false;
                        buff.PopSave(); // Pop saveed point
                        break;
                    }

                    if (buff.PeekSave().builderLength > 0) {
                        buff.Restore(); // Restore saved point
                        buff.LineFeed();
                        elementIsMultiLine = Append(buff, element, true);
                    }
                }
                buff.PopSave(); // Pop saveed point

                buff.Append(", ");
                if (elementIsMultiLine)
                    buff.LineFeed();

                ++index;
            }

            if (!success) {
                buff.Restore(); // Restore saved point
                buff.PopSave(); // Pop saveed point
                AppendCollection(buff, collection, true);
                return true;
            }

            if (collection.Count > 0 && !lineFeeded)
                buff.Length -= 2;

            if (lineFeeded) {
                if (buff.Length > 0)
                    buff.LineFeed();
                buff.DownNest();
            }
            buff.Append('}');

            buff.PopSave(); // Pop saveed point
            return isMultiLine;
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
        /// <param name="buff">the logging buffer</param>
        /// <param name="obj">an object</param>
        /// <param name="isMultiLine">output multiple lines if true, single line otherwise</param>
        /// <returns>false if can not be outputed on a single line, otherwise true</returns>
        protected bool AppendUsedReflection(LogBuffer buff, object obj, bool isMultiLine) {
            buff.Save(); // Save current point
            var type = obj.GetType();
            buff.Append(GetTypeName(type, obj, false));
            var isExtended = type.BaseType != typeof(object) && type.BaseType != typeof(ValueType);
            var isTuple = IsTuple(type);

            buff.Append(isTuple ? '(' : '{');
            if (isMultiLine) {
                buff.LineFeed();
                buff.UpNest();
            }

            if (!AppendUsedReflectionSub(buff, obj, type, isExtended, isMultiLine)) {
                buff.Restore(); // Restore saved point
                buff.PopSave(); // Pop saveed point
                return AppendUsedReflection(buff, obj, true);
            }

            if (isMultiLine) {
                if (buff.Length > 0)
                    buff.LineFeed();
                buff.DownNest();
            }
            buff.Append(isTuple ? ')' : '}');

            buff.PopSave(); // Pop saveed point
            return isMultiLine;
        }

        /// <summary>
        /// Returns a string representation of the obj uses reflection.
        /// </summary>
        ///
        /// <param name="buff">the logging buffer</param>
        /// <param name="obj">an object</param>
        /// <param name="type">the type of the object</param>
        /// <param name="isExtended">if true, the type is isExtended type</param>
        /// <param name="isMultiLine">output multiple lines if true, single line otherwise</param>
        /// <returns>false if can not be outputed on a single line, otherwise true</returns>
        protected bool AppendUsedReflectionSub(LogBuffer buff, object obj, Type type, bool isExtended, bool isMultiLine) {
            Type baseType = type.BaseType;
            if (baseType != typeof(object) && baseType != typeof(ValueType))
                // Call for the base type
                AppendUsedReflectionSub(buff, obj, baseType, isExtended, isMultiLine);

            var typeNamePrefix = type.Namespace + '.' + type.Name + "#";

            if (isExtended) {
                if (!isMultiLine) return false; // can not be outputed on a single line

                if (buff.Length > 0)
                    buff.LineFeed();
                buff.Append(string.Format(ClassBoundaryString, ReplaceTypeName(type.Namespace + '.' + type.Name)));
                buff.LineFeed();
            }

            // field
            var fieldInfos = type.GetFields(
                  BindingFlags.DeclaredOnly
                | BindingFlags.Public
                | BindingFlags.Instance);
            int fieldIndex = 0;
            foreach (var fieldInfo in fieldInfos) {
                if (buff.Length > MaxDataOutputWidth) {
                    if (!isMultiLine) return false; // can not be outputed on a single line
                    buff.LineFeed();
                }

                buff.Save(); // Save current point
                bool elementIsMultiLine = AppendReflectValue(buff, type, obj, fieldInfo);
                if (elementIsMultiLine || buff.Length > MaxDataOutputWidth) {
                    if (!isMultiLine) {
                        buff.PopSave(); // Pop saveed point
                        return false;
                    }

                    if (buff.PeekSave().builderLength > 0) {
                        buff.Restore(); // Restore saved point
                        buff.LineFeed();
                        elementIsMultiLine = AppendReflectValue(buff, type, obj, fieldInfo);
                    }
                }
                buff.PopSave(); // Pop saveed point

                buff.Append(", ");
                if (elementIsMultiLine)
                    buff.LineFeed();

                ++fieldIndex;
            }

            // property
            var propertyInfos = type.GetProperties(
                      BindingFlags.DeclaredOnly
                    | BindingFlags.Public
                    | BindingFlags.Instance);
            int propertyIndex = 0;
            foreach (var propertyInfo in propertyInfos) {
                buff.Save(); // Save current point
                bool elementIsMultiLine = AppendReflectValue(buff, type, obj, propertyInfo);
                if (elementIsMultiLine || buff.Length > MaxDataOutputWidth) {
                    if (!isMultiLine) {
                        buff.PopSave(); // Pop saveed point
                        return false;
                    }

                    if (buff.PeekSave().builderLength > 0) {
                        buff.Restore(); // Restore saved point
                        buff.LineFeed();
                        elementIsMultiLine = AppendReflectValue(buff, type, obj, propertyInfo);
                    }
                }
                buff.PopSave(); // Pop saveed point

                buff.Append(", ");
                if (elementIsMultiLine)
                    buff.LineFeed();

                ++propertyIndex;
            }

            if (!isMultiLine && fieldIndex + propertyIndex > 0)
                buff.Length -= 2;

            return true;
        }

        // AppendReflectValue / MemberInfo
        private bool AppendReflectValue(LogBuffer buff, Type type, object obj, MemberInfo memberInfo) {
            if (!IsTuple(type))
                buff.Append(memberInfo.Name).Append(KeyValueSeparator);

            object value = null;
            try {
                switch (memberInfo) {
                case FieldInfo fieldInfo:
                    value = fieldInfo.GetValue(obj);
                    break;
                case PropertyInfo propertyInfo:
                    value = propertyInfo.GetValue(obj);
                    break;
                }
            }
            catch (Exception e) {
                value = e.ToString();
            }

            if (value != null && NonPrintProperties.Contains(GetFullTypeName(type) + '.' + memberInfo.Name)) {
                buff.Append(NonPrintString);
                return false;
            }

            return Append(buff, value, false);
        }

        // IsTuple
        private static bool IsTuple(Type type) {
            return type.Name.StartsWith("Tuple`") || type.Name.StartsWith("ValueTuple`");
        }

        /// <summary>
        /// Returns current DateTime string.
        /// </summary>
        ///
        /// <returns>current timestamp string</returns>
        public static string DateTimeString {
            get {
                return string.Format(DateTimeFormat, DateTime.Now);
            }
        }
    }
}

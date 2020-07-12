// TraceBase.cs
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
    /// <summary>
    /// The base class of classes that implements <c>ITrace</c> interface.
    /// </summary>
    /// <since>1.0.0</since>
    /// <author>Masato Kokubo</author>
    public abstract class TraceBase : ITrace {
        /// <summary>
        /// Resources including DebugTrace operation option
        /// </summary>
        public static Resource Resource {get; private set;}

        /// <summary>
        /// Format string of log output when entering methods
        /// </summary>
        public static string EnterFormat {get; set;} = ""; // since 2.0.0 EnterFormat <- EnterString

        /// <summary>
        /// Format string of log output when leaving methods
        /// </summary>
        public static string LeaveFormat {get; set;} = ""; // since 2.0.0 LeaveFormat <- LeaveString

        /// <summary>
        /// Format string of log output at threads boundary
        /// </summary>
        public static string ThreadBoundaryFormat {get; set;} = ""; // since 2.0.0 ThreadBoundaryFormat <- ThreadBoundaryString

        /// <summary>
        /// Format string of log output at classes boundary
        /// </summary>
        public static string ClassBoundaryFormat {get; set;} = ""; // since 2.0.0 ClassBoundaryFormat <- ClassBoundaryString

        /// <summary>
        /// String of one code indent
        /// </summary>
        public static string IndentString {get; set;} = ""; // since 2.0.0 IndentString <- CodeIndentString

        /// <summary>
        /// String of one data indent
        /// </summary>
        public static string DataIndentString {get; set;} = "";

        /// <summary>
        /// String to represent that it has exceeded the limit
        /// </summary>
        public static string LimitString {get; set;} = "";

        /// <summary>
        /// String replacing the default namespace part
        /// </summary>
        public static string DefaultNameSpaceString {get; set;} = "";

        /// <summary>
        /// String to be output instead of not outputting value
        /// </summary>
        public static string NonOutputString {get; set;} = ""; // since 2.0.0 NonOutputString <- NonPrintString

        /// <summary>
        /// String to represent that the cyclic reference occurs
        /// </summary>
        public static string CyclicReferenceString {get; set;} = "";

        /// <summary>
        /// Separator string between the variable name and value
        /// </summary>
        public static string VarNameValueSeparator {get; set;} = "";

        /// <summary>
        /// Separator string between the key and value of dictionary
        /// </summary>
        public static string KeyValueSeparator {get; set;} = "";

        /// <summary>
        /// Output format of <c>Print</c> method suffix
        /// </summary>
        public static string PrintSuffixFormat {get; set;} = "";

        /// <summary>
        /// Output format of the count of collection
        /// <since>1.5.1</since>
        /// </summary>
        public static string CountFormat {get; set;} = "";

        /// <summary>
        /// Minimum value to output the number of elements of collection
        /// <since>2.0.0</since>
        /// </summary>
        public static int MinimumOutputCount {get; set;}

        /// <summary>
        /// Output format of the length of <c>string</c>
        /// <since>1.5.1</since>
        /// </summary>
        public static string LengthFormat {get; set;} = ""; // since 2.0.0 LengthFormat <- StringLengthFormat

        /// <summary>
        /// Minimum value to output the length of string
        /// <since>2.0.0</since>
        /// </summary>
        public static int MinimumOutputLength {get; set;}

        /// <summary>
        /// Output format of <c>DateTime</c>
        /// </summary>
        public static string DateTimeFormat {get; set;} = "";

        /// <summary>
        /// Output format of date and time when outputting logs
        /// </summary>
        public static string LogDateTimeFormat {get; set;} = "";

        /// <summary>
        /// Maximum output width of data
        /// </summary>
        public static int MaximumDataOutputWidth {get; set;} // since 2.0.0 MaximumDataOutputWidth <- MaxDataOutputWidth

        /// <summary>
        /// Limit value of <c>ICollection</c> elements to output
        /// </summary>
        public static int CollectionLimit {get; set;}

        /// <summary>
        /// Limit value of <c>string</c> characters to output
        /// </summary>
        public static int StringLimit {get; set;}

        /// <summary>
        /// Limit value of reflection nest
        /// </summary>
        public static int ReflectionNestLimit {get; set;}

        /// <summary>
        /// Properties and fields not to be output value
        /// </summary>
        public static IList<string> NonOutputProperties {get; set;} = new List<string>(); // since 2.0.0 NonOutputProperties <- NonPrintProperties

        /// <summary>
        /// Default namespace of your C# source
        /// </summary>
        public static string DefaultNameSpace {get; set;} = "";

        /// <summary>
        /// Classe names that output content by reflection even if <c>ToString</c> method is implemented
        /// </summary>
        public static ISet<string> ReflectionClasses {get; set;} = new HashSet<string>();

        /// <summary>
        /// If <c>true</c>, outputs the contents by reflection even for fields which are not <c>public</c>
        /// </summary>
        public static bool OutputNonPublicFields {get; set;}

        /// <summary>
        /// If <c>true</c>, outputs the contents by reflection even for properties which are not <c>public</c>
        /// </summary>
        public static bool OutputNonPublicProperties {get; set;}

        /// <summary>
        /// Array of indent strings
        /// </summary>
        protected static string[] indentStrings = new string[0];

        /// <summary>
        /// Array of data indent strings
        /// </summary>
        protected static string[] dataIndentStrings = new string[0];

        /// <summary>
        /// The logger
        /// </summary>
        public static ILogger Logger {get; set;} = Console.Error.Instance; // the logger

        /// <summary>
        /// Whether tracing is IsEnabled
        /// </summary>
        public bool IsEnabled {get => Logger.IsEnabled;}

        /// <summary>
        /// Set of classes that dose not output the type name
        /// </summary>
        protected abstract ISet<Type> NoOutputTypes {get;}

        /// <summary>
        /// Set of element types of array that dose not output the type name
        /// </summary>
        protected abstract ISet<Type> NoOutputElementTypes {get;}

        /// <summary>
        /// Dictionary of type to type name
        /// </summary>
        protected abstract IDictionary<Type, string> TypeNameMap {get;}

        /// <summary>
        /// Dictionary of thread id to indent state
        /// </summary>
        protected readonly IDictionary<int, State> states = new Dictionary<int, State>();

        /// <summary>
        /// Previous thread id
        /// </summary>
        protected int beforeThreadId;

        /// <summary>
        /// Reflected objects
        /// </summary>
        protected readonly IList<object> reflectedObjects = new List<object>();

        /// <summary>
        /// The last log string output
        /// </summary>
        public string LastLog {get; private set;} = "";

        /// <summary>
        /// Class constructor
        /// </summary>
        static TraceBase() {
            Resource = new Resource("DebugTrace");
            InitClass("");
        }

        /// <summary>
        /// Initializes this class.
        /// </summary>
        /// <param name="baseName">the base name of the resource properties file</param>
        public static void InitClass(string baseName) {
            if (baseName != "")
                Resource = new Resource(baseName);

            EnterFormat               = Resource.GetString (nameof(EnterFormat), "EnterString", Resource.Unescape(@"Enter {0}.{1} ({2}:{3:D})"));
            LeaveFormat               = Resource.GetString (nameof(LeaveFormat), "LeaveString", Resource.Unescape(@"Leave {0}.{1} ({2}:{3:D}) duration: {4}"));
            ThreadBoundaryFormat      = Resource.GetString (nameof(ThreadBoundaryFormat), "ThreadBoundaryString", Resource.Unescape(@"______________________________ Thread {0} ______________________________"));
            ClassBoundaryFormat       = Resource.GetString (nameof(ClassBoundaryFormat ), "ClassBoundaryString" , Resource.Unescape(@"____ {0} ____"));
            IndentString              = Resource.GetString (nameof(IndentString), "CodeIndentString", Resource.Unescape(@"|\s"));
            DataIndentString          = Resource.GetString (nameof(DataIndentString         ), Resource.Unescape(@"\s\s"));
            LimitString               = Resource.GetString (nameof(LimitString              ), Resource.Unescape(@"..."));
            NonOutputString           = Resource.GetString (nameof(NonOutputString), "NonPrintString", Resource.Unescape(@"***"));
            CyclicReferenceString     = Resource.GetString (nameof(CyclicReferenceString    ), Resource.Unescape(@"*** Cyclic Reference ***"));
            VarNameValueSeparator     = Resource.GetString (nameof(VarNameValueSeparator    ), Resource.Unescape(@"\s=\s"));
            KeyValueSeparator         = Resource.GetString (nameof(KeyValueSeparator        ), Resource.Unescape(@":\s"));
            PrintSuffixFormat         = Resource.GetString (nameof(PrintSuffixFormat        ), Resource.Unescape(@"\s({2}:{3:D})"));
            CountFormat               = Resource.GetString (nameof(CountFormat              ), Resource.Unescape(@"\sCount:{0}")); // since 1.5.1
            MinimumOutputCount        = Resource.GetInt    (nameof(MinimumOutputCount       ), 5); // since 2.0.0
            LengthFormat              = Resource.GetString (nameof(LengthFormat), "StringLengthFormat", Resource.Unescape(@"(Length:{0})")); // since 1.5.1
            MinimumOutputLength       = Resource.GetInt    (nameof(MinimumOutputLength      ), 5); // since 2.0.0
            DateTimeFormat            = Resource.GetString (nameof(DateTimeFormat           ), Resource.Unescape(@"{0:yyyy-MM-dd HH:mm:ss.fffffffK}"));
            LogDateTimeFormat         = Resource.GetString (nameof(LogDateTimeFormat        ), Resource.Unescape(@"{0:yyyy-MM-dd HH:mm:ss.fff} [{1:D2}] {2}")); // since 1.3.0
            MaximumDataOutputWidth    = Resource.GetInt    (nameof(MaximumDataOutputWidth), "MaxDataOutputWidth", 70);
            CollectionLimit           = Resource.GetInt    (nameof(CollectionLimit          ), 512);
            StringLimit               = Resource.GetInt    (nameof(StringLimit              ), 8192);
            ReflectionNestLimit       = Resource.GetInt    (nameof(ReflectionNestLimit      ), 4);
            NonOutputProperties       = new List<string>(Resource.GetStrings(nameof(NonOutputProperties), "NonPrintProperties", new string[0]));
            NonOutputProperties.Add("System.Threading.Tasks.Task.Result");
            DefaultNameSpace          = Resource.GetString (nameof(DefaultNameSpace         ), "");
            DefaultNameSpaceString    = Resource.GetString (nameof(DefaultNameSpaceString   ), Resource.Unescape(@"..."));
            ReflectionClasses         = new HashSet<string>(Resource.GetStrings(nameof(ReflectionClasses), new string[0]));
            ReflectionClasses.Add(typeof(Tuple).FullName ?? ""); // Tuple
            ReflectionClasses.Add(typeof(ValueTuple).FullName ?? ""); // ValueTuple
            OutputNonPublicFields     = Resource.GetBool   (nameof(OutputNonPublicFields    ), false); // since 1.4.4
            OutputNonPublicProperties = Resource.GetBool   (nameof(OutputNonPublicProperties), false); // since 1.4.4

            // Array of indent strings
            indentStrings = new string[32];

            // Array of data indent strings
            dataIndentStrings = new string[32];

            var loggerStr = Resource.GetString("Logger", "");
            if (loggerStr != "") {
                Exception? e1 = null;
                Exception? e2 = null;
                var loggerNames = loggerStr.Split(Loggers.SeparatorChar).Select(str => str.Trim());
                var loggers = new List<ILogger>();
                foreach (var loggerName in loggerNames) {
                    ILogger? logger = null;
                    var loggerFullName = loggerName.Split(',')[0].Contains('.')
                        ? loggerName
                        : typeof(ILogger).Namespace + '.' + loggerName; // Add default namesapce if no namespace
                    try {
                        logger = (ILogger?)Type.GetType(loggerFullName)?
                            .GetProperty(nameof(Console.Out.Instance), BindingFlags.Public | BindingFlags.Static)?
                            .GetValue(null);
                    }
                    catch (Exception e) {
                        e1 = e;
                    }
                    if (logger == null && !loggerFullName.Contains(',')) {
                        // Try with the class name that added the assembly name
                        loggerFullName = loggerFullName + ',' + loggerFullName;
                        try {
                            logger = (ILogger?)Type.GetType(loggerFullName)?
                                .GetProperty(nameof(Console.Out.Instance), BindingFlags.Public | BindingFlags.Static)?
                                .GetValue(null);
                        }
                        catch (Exception e) {
                            e2 = e;
                        }
                    }

                    if (logger != null)
                        loggers.Add(logger);
                    else {
                        if (e2 != null)
                            System.Console.Error.WriteLine($"DebugTrace-net: {e2.ToString()}({loggerName})");
                        else if (e1 != null)
                            System.Console.Error.WriteLine($"DebugTrace-net: {e1.ToString()}({loggerName})");
                    }
                }
                if (loggers.Count == 1)
                    // single logger
                    Logger = loggers[0];
                else if (loggers.Count > 1)
                    // multiple logger
                    Logger = new Loggers(loggers.ToArray());
            }

            // Set the logging level
            var logLevel = Resource.GetString("LogLevel", "");
            if (logLevel != "")
                Logger.Level = logLevel;

            // make code indent strings
            indentStrings[0] = "";
            for (var index = 1; index < indentStrings.Length; ++index)
                indentStrings[index] = indentStrings[index - 1] + IndentString;

            // make data indent strings
            dataIndentStrings[0] = "";
            for (var index = 1; index < dataIndentStrings.Length; ++index)
                dataIndentStrings[index] = dataIndentStrings[index - 1] + DataIndentString;

            // output version log
            var versionAttribute = (AssemblyInformationalVersionAttribute?)
                Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyInformationalVersionAttribute));
            Logger.Log($"DebugTrace-net {versionAttribute?.InformationalVersion}");
            Logger.Log($"  logger: {Logger}");
            Logger.Log($"  properties file path: {Resource.FileInfo.FullName}");
        }

        /// <summary>
        /// Returns the indent state of the current thread.
        /// </summary>
        /// <param name="threadId">the thread id</param>
        /// <returns>the indent state of the current thread</returns>
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
        /// Returns a string corresponding to the code and data nest level.
        /// </summary>
        /// <param name="nestLevel">the code nest level</param>
        /// <param name="dataNestLevel">the data nest level</param>
        /// <returns>a string corresponding to the current indent</returns>
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
                Logger.Log(string.Format(ThreadBoundaryFormat, state.ThreadId));
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
        /// Outputs a log when enters the method.
        /// </summary>
        /// <returns>the current thread id</returns>
        public int Enter() {
            if (!IsEnabled) return -1;

            lock (states) {
                var state = GetCurrentState();

                PrintStart(state); // Common start processing of output

                if (state.PreviousNestLevel > state.NestLevel)
                    Logger.Log(GetIndentString(state.NestLevel, 0)); // Empty Line

                LastLog = GetIndentString(state.NestLevel, 0) + GetCallerInfo(EnterFormat);
                Logger.Log(LastLog);

                state.PreviousLineCount = 1;

                state.UpNest();

                return state.ThreadId;
            }
        }

        /// <summary>
        /// Outputs a log when leaves the method.
        /// </summary>
        /// <param name="threadId">the thread id</param>
        public void Leave(int threadId = -1) {
            if (!IsEnabled) return;

            lock (states) {
                var state = GetCurrentState(threadId);
                PrintStart(state); // Common start processing of output

                if (state.PreviousLineCount > 1)
                    Logger.Log(GetIndentString(state.NestLevel, 0)); // Empty Line

                var timeSpan = DateTime.UtcNow - state.DownNest();;

                LastLog = GetIndentString(state.NestLevel, 0) + GetCallerInfo(LeaveFormat, timeSpan);
                Logger.Log(LastLog);

                state.PreviousLineCount = 1;
            }
        }

        /// <summary>
        /// Returns the caller's class name, method name, file name and line number
        /// of the caller method and the time span string embedded in <c>baseString</c>.
        /// </summary>
        /// <param name="baseString">the string for formatting</param>
        /// <param name="timeSpan">the time span</param>
        /// <returns>a string embedded caller information</returns>
        protected string GetCallerInfo(string baseString, TimeSpan? timeSpan = null) {
            var element = GetStackTraceElement();

            return string.Format(baseString,
                ReplaceTypeName(element.TypeName),
                element.MethodName,
                element.FileName,
                element.LineNumber,
                timeSpan);
        }

        /// <summary>
        /// Outputs the message to the log.
        /// </summary>
        /// <param name="message">the message</param>
        public void Print(string message) {
            if (!IsEnabled) return;
            PrintSub(message);
        }

        /// <summary>
        /// Outputs the message to the log.
        /// </summary>
        /// <param name="messageSupplier">the message supplier</param>
        public void Print(Func<string> messageSupplier) {
            if (!IsEnabled) return;
            PrintSub(messageSupplier());
        }

        /// <summary>
        /// Outputs the message to the log.
        /// </summary>
        /// <param name="message">the message</param>
        protected void PrintSub(string message) {
            lock (states) {
                var state = GetCurrentState();
                PrintStart(state); // Common start processing of output

                if (state.PreviousLineCount > 1)
                    Logger.Log(GetIndentString(state.NestLevel, 0)); // Empty Line

                LastLog = GetIndentString(state.NestLevel, 0);
                if (message != "") {
                    var element = GetStackTraceElement();
                    var suffix = string.Format(PrintSuffixFormat,
                        ReplaceTypeName(element.TypeName),
                        element.MethodName,
                        element.FileName,
                        element.LineNumber);
                    LastLog += message + suffix;
                }
                Logger.Log(LastLog);

                state.PreviousLineCount = 1;
            }
        }

        /// <summary>
        /// Outputs the name and value to the log.
        /// </summary>
        /// <param name="name">the name of the value</param>
        /// <param name="value">the value to output (nullable)</param>
        protected void PrintSub(string name, object? value, bool innerUse = false) {
            lock (states) {
                var state = GetCurrentState();
                PrintStart(state); // Common start processing of output

                reflectedObjects.Clear();

                var buff = new LogBuffer();

                buff.Append(name).NoBreakAppend(VarNameValueSeparator);
                var normalizedName = name.Substring(name.LastIndexOf('.') + 1).Trim();
                normalizedName = normalizedName.Substring(normalizedName.LastIndexOf(' ') + 1);
                var valueBuff = ToString(value);
                buff.Append(valueBuff);

                var element = GetStackTraceElement();
                var suffix = string.Format(PrintSuffixFormat,
                    element.TypeName,
                    element.MethodName,
                    element.FileName,
                    element.LineNumber);
                buff.NoBreakAppend(suffix);

                var lines = buff.Lines;
                if (state.PreviousLineCount > 1 ||lines.Count > 1)
                    Logger.Log(GetIndentString(state.NestLevel, 0)); // Empty Line

                var lastLogBuff = new StringBuilder();
                foreach ((var dataNestLevel, var line) in lines) {
                    var log = GetIndentString(state.NestLevel, dataNestLevel) + line;
                    Logger.Log(log);
                    lastLogBuff.Append(log).Append('\n');
                }
                LastLog = lastLogBuff.ToString();

                state.PreviousLineCount = lines.Count;
            }
        }

        private static string thisClassFullName = typeof(TraceBase).FullName + '.';

        /// <summary>
        /// Returns the caller stack trace element.
        /// </summary>
        /// <returns>the caller stack trace element</returns>
        protected StackTraceElement GetStackTraceElement() {
            var elements = GetStackTraceElements(1);

            var result = elements.Count() > 0
                ? elements.ElementAt(0)
                : new StackTraceElement("--", "--", "--", 0);

            return result;
        }

        /// <summary>
        /// Returns stack trace elements.
        /// </summary>
        /// <param name="maxCount">maximum number of stack trace elements to return</returns>
        /// <returns>stack trace elements</returns>
        /// <since>1.5.5</since>
        protected StackTraceElement[] GetStackTraceElements(int maxCount) {
            return Environment.StackTrace.Split('\n')
                .Select(str => str.Trim())
                .Where(str => str != "" && !str.Contains(thisClassFullName) && !str.Contains("StackTrace"))
                .Take(maxCount)
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
                })
                .ToArray();
        }

        /// <summary>
        /// Returns a tuple of two strings obtained by splitting the string by the separator.
        /// Searches the string from the top.
        /// </summary>
        /// <param name="str">the string</param>
        /// <param name="separator">the separator</param>
        /// <returns>a tuple of strings</returns>
        protected static (string, string) Split(string str, char separator) {
            var index = str.IndexOf(separator);
            return index < 0
                ? (str, "")
                : (str.Substring(0, index), str.Substring(index + 1));
        }

        /// <summary>
        /// Returns a tuple of two strings obtained by splitting the string by the separator.
        /// Searches the string from the end.
        /// </summary>
        /// <param name="str">the string</param>
        /// <param name="separator">the separator</param>
        /// <returns>a tuple of strings</returns>
        protected static (string, string) LastSplit(string str, char separator) {
            var index = str.LastIndexOf(separator);
            return index < 0
                ? (str, "")
                : (str.Substring(0, index), str.Substring(index + 1));
        }

        /// <summary>
        /// Outputs the name and value to the log.
        /// </summary>
        /// <param name="name">the name of the value</param>
        /// <param name="value">the value to output (nullable)</param>
        public void Print(string name, object? value) {
            if (!IsEnabled) return;
            PrintSub(name, value);
        }

        /// <summary>
        /// Outputs the name and value to the log.
        /// Outputs an array of StackTraceElement to the log.
        /// </summary>
        /// <param name="name">the name of the value</param>
        /// <param name="valueSupplier">the supplier of value to output</param>
        public void Print(string name, Func<object?> valueSupplier) {
            if (!IsEnabled) return;
            PrintSub(name, valueSupplier());
        }

        /// <summary>
        /// Outputs an array of StackTraceElement to the log.
        /// </summary>
        /// <param name="maxCount">maximum number of stack trace elements to output</returns>
        /// <since>1.5.5</since>
        public void PrintStack(int maxCount) {
            Print("Stack", GetStackTraceElements(maxCount));
        }

        /// <summary>
        /// Creates a string buffer from the value.
        /// </summary>
        /// <param name="value">the value object</param>
        /// <param name="isElement"><c>true</c> if the value is element of a container class, <c>false</c> otherwise</param>
        /// <returns>a LogBuffer</returns>
        protected LogBuffer ToString(object? value, bool isElement = false) {
            var buff = new LogBuffer();

            if (value == null) {
                buff.Append("null");
                return buff;
            }

            var type = value.GetType();

            var typeName = GetTypeName(type, value, isElement);
            var fullTypeName = GetFullTypeName(type);
            bool isReflection = ReflectionClasses.Contains(fullTypeName);
            if (!isReflection) {

                switch (value) {
                case bool       boolValue: buff.Append(typeName); Append(buff,    boolValue); return buff;
                case char       charValue: buff.Append(typeName); Append(buff,    charValue); return buff;
                case sbyte     sbyteValue: buff.Append(typeName); Append(buff,   sbyteValue); return buff;
                case byte       byteValue: buff.Append(typeName); Append(buff,    byteValue); return buff;
                case short     shortValue: buff.Append(typeName); Append(buff,   shortValue); return buff;
                case ushort   ushortValue: buff.Append(typeName); Append(buff,  ushortValue); return buff;
                case int         intValue: buff.Append(typeName); Append(buff,     intValue); return buff;
                case uint       uintValue: buff.Append(typeName); Append(buff,    uintValue); return buff;
                case long       longValue: buff.Append(typeName); Append(buff,    longValue); return buff;
                case ulong     ulongValue: buff.Append(typeName); Append(buff,   ulongValue); return buff;
                case float     floatValue: buff.Append(typeName); Append(buff,   floatValue); return buff;
                case double   doubleValue: buff.Append(typeName); Append(buff,  doubleValue); return buff;
                case decimal decimalValue: buff.Append(typeName); Append(buff, decimalValue); return buff;
                case DateTime    dateTime: buff.Append(typeName); Append(buff,     dateTime); return buff;
                case string   stringValue: buff.Append(typeName); AppendString(buff, stringValue); return buff;
                case IDictionary dictionary: return ToStringDictionary(dictionary);
                case IEnumerable enumerable: return ToStringEnumerable(enumerable);
                case Enum       enumValue: buff.Append(typeName); buff.Append(enumValue); return buff;
                default:
                    if (!HasToString(type)) {
                        isReflection = true;
                        ReflectionClasses.Add(fullTypeName);
                    }
                    break;
                }
            }

            if (isReflection) {
                // Use Reflection
                if (reflectedObjects.Any(obj => object.ReferenceEquals(value, obj)))
                    // Cyclic reference
                    buff.Append(CyclicReferenceString);

                else if (reflectedObjects.Count > ReflectionNestLimit)
                    // Over reflection level limitation
                    buff.NoBreakAppend(LimitString);

                else {
                    // Use Reflection
                    reflectedObjects.Add(value);
                    var valueBuff = ToStringReflection(value);
                    buff.Append(valueBuff);
                    reflectedObjects.RemoveAt(reflectedObjects.Count - 1);
                }
            } else {
                // Use ToString method
                buff.Append(typeName).Append(value);
            }

            return buff;
        }

        /// <summary>
        /// Returns the type name to be output to the log.
        /// If dose not output, returns null.
        /// </summary>
        /// <param name="type">the type of the value</param>
        /// <param name="value">the value object</param>
        /// <param name="isElement"><c>true</c> if the value is element of a container class, <c>false</c> otherwise</param>
        /// <param name="nest">current nest count</param>
        /// <returns>the type name to be output to the log</returns>
        protected string GetTypeName(Type type, object? value, bool isElement, int nest = 0) {
            var typeName = "";
            if (type.IsArray) {
                // Array
                typeName = GetArrayTypeName(type, value, isElement, nest);

            } else {
                typeName = GetTypeName(type);

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
                            var count = -1;
                            try {
                                var countProperty = type.GetProperty("Count");
                                count = (int?)countProperty?.GetValue(value) ?? -1;
                            }
                            catch (Exception) {}
                            if (count == -1) {
                                if (type.IsEnum)
                                    typeName = "enum " + typeName;
                                else if (type.IsValueType)
                                    typeName += " struct";
                            } else if (count >= MinimumOutputCount)
                                typeName += string.Format(CountFormat, count);
                        }
                    }
                }
            }

            if (typeName != "" && nest == 0)
                typeName += ' ';
            return typeName;
        }

        /// <summary>
        /// Returns the type name of the array to be output to the log.
        /// If dose not output, returns <c>null</c>.
        /// </summary>
        /// <param name="type">the type of the value</param>
        /// <param name="value">the value object</param>
        /// <param name="isElement"><c>true</c> if the value is element of a container class, <c>false</c> otherwise</param>
        /// <param name="nest">current nest count</param>
        /// <returns>the type name to be output to the log</returns>
        protected abstract string GetArrayTypeName(Type type, object? value, bool isElement, int nest);

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
        /// <param name="typeName">a class name</param>
        /// <returns>the replaced ckass name</returns>
        protected string ReplaceTypeName(string typeName) {
            if (DefaultNameSpace != "" && typeName.StartsWith(DefaultNameSpace))
                typeName = DefaultNameSpaceString + typeName.Substring(DefaultNameSpace.Length);
            return typeName;
        }

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected abstract void Append(LogBuffer buff, bool value);

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected abstract void Append(LogBuffer buff, char value);

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected abstract void Append(LogBuffer buff, sbyte value);

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected abstract void Append(LogBuffer buff, byte value);

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected abstract void Append(LogBuffer buff, short value);

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected abstract void Append(LogBuffer buff, ushort value);

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected abstract void Append(LogBuffer buff, int value);

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected abstract void Append(LogBuffer buff, uint value);

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected abstract void Append(LogBuffer buff, long value);

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected abstract void Append(LogBuffer buff, ulong value);

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected abstract void Append(LogBuffer buff, float value);

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected abstract void Append(LogBuffer buff, double value);

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected abstract void Append(LogBuffer buff, decimal value);

        /// <summary>
        /// Appends a string representation of the value to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="value">the value</param>
        /// <returns></returns>
        protected abstract void Append(LogBuffer buff, DateTime value);

        /// <summary>
        /// Appends a character representation for logging to the string buffer.
        /// </summary>
        /// <param name="buff">the logging buffer</param>
        /// <param name="ch">a character</param>
        /// <param name="enclosure">the enclosure character</param>
        /// <param name="escape">escape characters if true, dose not escape characters otherwise</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise</returns>
        protected void AppendChar(LogBuffer buff, char ch, char enclosure, bool escape) {
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
                buff.Append(ch);
            }
        }

        /// <summary>
        /// Appends a CharSequence representation for logging to the string buffer.
        /// </summary>
        /// <param name="buff">the logging buffer</param>
        /// <param name="str">a string object</param>
        /// <param name="escape">escape characters if true, dose not escape characters otherwise</param>
        /// <returns>always false</returns>
        protected void AppendString(LogBuffer buff, string str) {
            if (str.Length >= MinimumOutputLength)
                buff.Append(string.Format(LengthFormat, str.Length));

            var hasBackslash = false;
            var hasEscaped = false;
            for (var index = 0; index < str.Length; ++index) {
                if (index >= StringLimit)
                    break;
                var ch = str[index];
                if (ch == '\\')
                    hasBackslash = true;
                else if (ch < ' ' || ch == '"' || ch == '\u007F')
                    hasEscaped = true;
            }

            if (hasBackslash && !hasEscaped)
                buff.Append('@');
            buff.Append('"');

            for (var index = 0; index < str.Length; ++index) {
                if (index >= StringLimit) {
                    buff.Append(LimitString);
                    break;
                }
                AppendChar(buff, str[index], '"', hasEscaped);
            }

            buff.Append('"');
        }

        /// <summary>
        /// Creates a string buffer from the dictionary.
        /// </summary>
        /// <param name="dictionary">a IDictionary</param>
        /// <param name="isMultiLine">output multiple lines if true, single line otherwise</param>
        /// <returns>a LogBuffer</returns>
        protected LogBuffer ToStringDictionary(IDictionary dictionary) {
            var buff = new LogBuffer();

            buff.Append(GetTypeName(dictionary.GetType(), dictionary, false));
            buff.Append('{');

            var bodyBuff = ToStringDictionaryBody(dictionary);

            var isMultiLines = bodyBuff.IsMultiLines || buff.Length + bodyBuff.Length > MaximumDataOutputWidth;

            if (isMultiLines) {
                buff.LineFeed();
                buff.UpNest();
            }

            buff.Append(bodyBuff);

            if (isMultiLines) {
                buff.LineFeed();
                buff.DownNest();
            }

            buff.Append('}');

            return buff;
        }

       private LogBuffer ToStringDictionaryBody(IDictionary dictionary) {
            var buff = new LogBuffer();

            var wasMultiLines = false;
            var index = 0;
            foreach (var key in dictionary.Keys) {
                if (index > 0)
                    buff.NoBreakAppend(", "); // Append a delimiter

                if (index >= CollectionLimit) {
                    buff.Append(LimitString);
                    break;
                }

                var value = dictionary[key!];

                var elementBuff = ToString(key, true).NoBreakAppend(KeyValueSeparator).Append(ToString(value, true));
                if (index > 0 && (wasMultiLines || elementBuff.IsMultiLines))
                    buff.LineFeed();
                buff.Append(elementBuff);

                wasMultiLines = elementBuff.IsMultiLines;
                ++index;
            }

            return buff;
        }

        /// <summary>
        /// Creates a string buffer from the enumerable.
        /// </summary>
        /// <param name="enumerable">a IEnumerable object</param>
        /// <param name="isMultiLine">output multiple lines if true, single line otherwise</param>
        /// <returns>a LogBuffer</returns>
        /// <since>1.4.1</since>

        protected LogBuffer ToStringEnumerable(IEnumerable enumerable) {
            var buff = new LogBuffer();

            buff.Append(GetTypeName(enumerable.GetType(), enumerable, false));
            buff.Append('{');

            var bodyBuff = ToStringEnumerableBody(enumerable);

            var isMultiLines = bodyBuff.IsMultiLines || buff.Length + bodyBuff.Length > MaximumDataOutputWidth;

            if (isMultiLines) {
                buff.LineFeed();
                buff.UpNest();
            }

            buff.Append(bodyBuff);

            if (isMultiLines) {
                buff.LineFeed();
                buff.DownNest();
            }

            buff.Append('}');

            return buff;
        }

        private LogBuffer ToStringEnumerableBody(IEnumerable enumerable) {
            var buff = new LogBuffer();

            var wasMultiLines = false;
            var index = 0;
            foreach (var element in enumerable) {
                if (index > 0)
                    buff.NoBreakAppend(", "); // Append a delimiter

                if (index >= CollectionLimit) {
                    buff.Append(LimitString);
                    break;
                }

                var elementBuff = ToString(element, true);
                if (index > 0 && (wasMultiLines || elementBuff.IsMultiLines))
                    buff.LineFeed();
                buff.Append(elementBuff);

                wasMultiLines = elementBuff.IsMultiLines;
                ++index;
            }

            return buff;
        }

        private static readonly Type[] zeroTypes = new Type[0];
        private static readonly ParameterModifier[] zeroParameterModifiers = new ParameterModifier[0];

        /// <summary>
        /// Returns true, if this class or super classes without object class has ToString method.
        /// </summary>
        /// <param name="type">the type</param>
        /// <returns><c>true</c> if the type or it base types without object and ValueType class has ToString method, <c>false</c> otherwise</returns>
        protected bool HasToString(Type type) {
            var result = false;

            try {
                while (type != typeof(object) && type != typeof(ValueType)) {
                    if (type.GetMethod(
                            "ToString", // name
                            BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance, // bindingAttr
                            null, // binder
                            zeroTypes, // types
                            zeroParameterModifiers // modifiers
                        ) != null) {
                        result = true;
                        break;
                    }
                    if (type.BaseType == null)
                        break;
                    type = type.BaseType;
                }
            }
            catch (Exception) {
            }

            return result;
        }

        /// <summary>
        /// Creates a string builder from the value.
        /// </summary>
        /// <param name="buff">the logging buffer</param>
        /// <param name="obj">an object</param>
        /// <param name="isMultiLine">output multiple lines if true, single line otherwise</param>
        /// <returns>a LogBuffer</returns>
        protected LogBuffer ToStringReflection(object obj) {
            var buff = new LogBuffer();

            var type = obj.GetType();
            buff.Append(GetTypeName(type, obj, false));
            var isExtended = type.BaseType != typeof(object) && type.BaseType != typeof(ValueType);
            var isTuple = IsTuple(type);

            var bodyBuff = ToStringReflectionBody(obj, type, isExtended);

            buff.Append(isTuple ? '(' : '{');
            if (bodyBuff.IsMultiLines) {
                buff.LineFeed();
                buff.UpNest();
            }

            buff.Append(bodyBuff);

            if (bodyBuff.IsMultiLines) {
                if (buff.Length > 0)
                    buff.LineFeed();
                buff.DownNest();
            }
            buff.Append(isTuple ? ')' : '}');

            return buff;
        }

        private LogBuffer ToStringReflectionBody(object obj, Type type, bool isExtended) {
            var buff = new LogBuffer();

            Type? baseType = type.BaseType;
            if (baseType != null && baseType != typeof(object) && baseType != typeof(ValueType)) {
                // Call for the base type
                var baseBuff =  ToStringReflectionBody(obj, baseType, isExtended);
                buff.Append(baseBuff);
            }

            var typeNamePrefix = type.Namespace + '.' + type.Name + "#";

            if (isExtended) {
                if (buff.Length > 0)
                    buff.LineFeed();
                buff.Append(string.Format(ClassBoundaryFormat, ReplaceTypeName(type.Namespace + '.' + type.Name)));
                buff.LineFeed();
            }

            var wasMultiLines = false;
            var fieldPropertyIndex = 0;

            // fields
            var fieldInfos = type.GetFields(
                  BindingFlags.DeclaredOnly
                | BindingFlags.Public
                | (OutputNonPublicFields ? BindingFlags.NonPublic : 0) // since 1.4.4
                | BindingFlags.Instance)
                .Where(fieldInfo => !fieldInfo.Name.EndsWith("__BackingField")); // Exclude property backing fields // since 1.4.4

            foreach (var fieldInfo in fieldInfos) {
                if (fieldPropertyIndex > 0)
                    buff.NoBreakAppend(", "); // Append a delimiter

                var valueBuff = ToStringReflectValue(type, obj, fieldInfo);
                if (fieldPropertyIndex > 0 && (wasMultiLines || valueBuff.IsMultiLines))
                    buff.LineFeed();
                buff.Append(valueBuff);

                wasMultiLines = valueBuff.IsMultiLines;
                ++fieldPropertyIndex;
            }

            // properties
            var propertyInfos = type.GetProperties(
                  BindingFlags.DeclaredOnly
                | BindingFlags.Public
                | (OutputNonPublicProperties ? BindingFlags.NonPublic : 0) // since 1.4.4
                | BindingFlags.Instance);

            foreach (var propertyInfo in propertyInfos) {
                var parameterInfos = propertyInfo.GetIndexParameters();
                if (parameterInfos.Length > 0) continue; // Not support indexed properties

                if (fieldPropertyIndex > 0)
                    buff.NoBreakAppend(", "); // Append a delimiter

                var valueBuff = ToStringReflectValue(type, obj, propertyInfo);
                if (fieldPropertyIndex > 0 && (wasMultiLines || valueBuff.IsMultiLines))
                    buff.LineFeed();
                buff.Append(valueBuff);

                wasMultiLines = valueBuff.IsMultiLines;
                ++fieldPropertyIndex;
            }

            return buff;
        }

        // AppendReflectValue / MemberInfo
        private LogBuffer ToStringReflectValue(Type type, object obj, MemberInfo memberInfo) {
            var buff = new LogBuffer();

            AppendAccessModifire(buff, memberInfo);
            var nonOutput = NonOutputProperties.Contains(GetFullTypeName(type) + '.' + memberInfo.Name);

            Type? memberType = null;
            object? value = null;
            try {
                switch (memberInfo) {
                case FieldInfo fieldInfo:
                    // field
                    memberType = fieldInfo.FieldType;
                    if (!nonOutput)
                        value = fieldInfo.GetValue(obj);
                    break;
                case PropertyInfo propertyInfo:
                    // property
                    memberType = propertyInfo.PropertyType;
                    if (!nonOutput)
                        value = propertyInfo.GetValue(obj);
                    break;
                }
            }
            catch (Exception e) {
                value = e.ToString();
            }

            if (!IsTuple(type)) {
                //  not Tuple
                if (memberType != null && (value == null || memberType != value.GetType()))
                    buff.Append(GetFullTypeName(memberType)).Append(' ');
                buff.Append(memberInfo.Name);
                buff.NoBreakAppend(KeyValueSeparator);
            }

            if (nonOutput)
                buff.Append(NonOutputString);
            else
                buff.Append(ToString(value));

            return buff;
        }

        /// <summary>
        /// Appends the access modifire of the member information to the log buffer.
        /// </summary>
        /// <param name="buff">the log buffer</param>
        /// <param name="memberInfo">the member information</param>
        /// <since>1.5.0</since>
        protected abstract void AppendAccessModifire(LogBuffer buff, MemberInfo memberInfo);

        // IsTuple
        private static bool IsTuple(Type type) {
            return type.Name.StartsWith("Tuple`") || type.Name.StartsWith("ValueTuple`");
        }

        /// <summary>
        /// Throws an <c>NullReferenceException</c> if the object is <c>null</c>.
        /// </summary>
        ///
        /// <typeparam name="T">the object type</typeparam>
        /// <param name="obj">the object</param>
        /// <param name="message">the message of the NullReferenceException</param>
        /// <returns>the object</returns>
        /// <exception cref="NullReferenceException">if the object is null</exception>
        /// <since>1.5.0</since>
        public static T RequreNonNull<T>(T obj, string message) {
            if (obj == null)
                throw new NullReferenceException(message);
            return obj;
        }
    }
}

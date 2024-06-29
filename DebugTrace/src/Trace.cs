// Trace.cs
// (C) 2018 Masato Kokubo
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace DebugTrace;

/// <summary>
/// The base class of classes that implements <c>ITrace</c> interface.
/// </summary>
/// <since>1.0.0</since>
/// <author>Masato Kokubo</author>
public class Trace {
    /// <summary>
    /// Resources including DebugTrace operation option
    /// </summary>
    internal static Resource Resource {get; private set;}

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
    private static string[] indentStrings = new string[0];

    /// <summary>
    /// Array of data indent strings
    /// </summary>
    private static string[] dataIndentStrings = new string[0];

    /// <summary>
    /// The logger
    /// </summary>
    public static ILogger Logger {get; set;} = Console.Error.Instance; // the logger

    /// <summary>
    /// Whether tracing is IsEnabled
    /// </summary>
    public static bool IsEnabled {get => Logger.IsEnabled;}

    /// <summary>
    /// Set of classes that dose not output the type name
    /// </summary>
    private static ISet<Type> NoOutputTypes {get;} = new HashSet<Type>() {
        typeof(bool    ),
        typeof(char    ),
        typeof(int     ),
        typeof(uint    ),
        typeof(long    ),
        typeof(ulong   ),
        typeof(float   ),
        typeof(double  ),
        typeof(decimal ),
        typeof(string  ),
        typeof(DateTime),
    };

    /// <summary>
    /// Set of element types of array that dose not output the type name
    /// </summary>
    private static ISet<Type> NoOutputElementTypes {get;} = new HashSet<Type>() {
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
        typeof(DateTime),
    };

    /// <summary>
    /// Dictionary of type to type name
    /// </summary>
    private static IDictionary<Type, string> TypeNameMap {get;} = new Dictionary<Type, string>() {
        {typeof(object ), "object" },
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
        {typeof(string ), "string" },
    };

    /// <summary>
    /// Dictionary of thread id to indent state
    /// </summary>
    private static readonly IDictionary<int, State> states = new Dictionary<int, State>();

    /// <summary>
    /// Previous thread id
    /// </summary>
    private static int beforeThreadId;

    /// <summary>
    /// Reflected objects
    /// </summary>
    private static  readonly IList<object> reflectedObjects = new List<object>();

    /// <summary>
    /// The last log string output
    /// </summary>
    public static string LastLog {get; private set;} = "";

    /// <summary>
    /// Class constructor
    /// </summary>
    static Trace() {
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

        EnterFormat               = Resource.GetString(nameof(EnterFormat              ), "EnterString", Resource.Unescape(@"Enter {0}.{1} ({2}:{3:D})"));
        LeaveFormat               = Resource.GetString(nameof(LeaveFormat              ), "LeaveString", Resource.Unescape(@"Leave {0}.{1} ({2}:{3:D}) duration: {4}"));
        ThreadBoundaryFormat      = Resource.GetString(nameof(ThreadBoundaryFormat     ), "ThreadBoundaryString", Resource.Unescape(@"______________________________ Thread {0} ______________________________"));
        ClassBoundaryFormat       = Resource.GetString(nameof(ClassBoundaryFormat      ), "ClassBoundaryString" , Resource.Unescape(@"____ {0} ____"));
        IndentString              = Resource.GetString(nameof(IndentString             ), "CodeIndentString", Resource.Unescape(@"|\s"));
        DataIndentString          = Resource.GetString(nameof(DataIndentString         ), Resource.Unescape(@"\s\s"));
        LimitString               = Resource.GetString(nameof(LimitString              ), Resource.Unescape(@"..."));
        NonOutputString           = Resource.GetString(nameof(NonOutputString          ), "NonPrintString", Resource.Unescape(@"***"));
        CyclicReferenceString     = Resource.GetString(nameof(CyclicReferenceString    ), Resource.Unescape(@"*** Cyclic Reference ***"));
        VarNameValueSeparator     = Resource.GetString(nameof(VarNameValueSeparator    ), Resource.Unescape(@"\s=\s"));
        KeyValueSeparator         = Resource.GetString(nameof(KeyValueSeparator        ), Resource.Unescape(@":\s"));
        PrintSuffixFormat         = Resource.GetString(nameof(PrintSuffixFormat        ), Resource.Unescape(@"\s({2}:{3:D})"));
        CountFormat               = Resource.GetString(nameof(CountFormat              ), Resource.Unescape(@"\sCount:{0}")); // since 1.5.1
        MinimumOutputCount        = Resource.GetInt   (nameof(MinimumOutputCount       ), 128); // 128 <- 5 since 3.0.0, since 2.0.0
        LengthFormat              = Resource.GetString(nameof(LengthFormat             ), "StringLengthFormat", Resource.Unescape(@"(Length:{0})")); // since 1.5.1
        MinimumOutputLength       = Resource.GetInt   (nameof(MinimumOutputLength      ), 256); // 256 <- 5 since 3.0.0, since 2.0.0
        DateTimeFormat            = Resource.GetString(nameof(DateTimeFormat           ), Resource.Unescape(@"{0:yyyy-MM-dd HH:mm:ss.fffffffK}"));
        LogDateTimeFormat         = Resource.GetString(nameof(LogDateTimeFormat        ), Resource.Unescape(@"{0:yyyy-MM-dd HH:mm:ss.fff} [{1:D2}] {2}")); // since 1.3.0
        MaximumDataOutputWidth    = Resource.GetInt   (nameof(MaximumDataOutputWidth   ), "MaxDataOutputWidth", 70);
        CollectionLimit           = Resource.GetInt   (nameof(CollectionLimit          ), 128); // 128 <- 512 since 3.0.0
        StringLimit               = Resource.GetInt   (nameof(StringLimit              ), 256); // 256 <- 8192 since 3.0.0
        ReflectionNestLimit       = Resource.GetInt   (nameof(ReflectionNestLimit      ), 4);
        NonOutputProperties       = new List<string>(Resource.GetStrings(nameof(NonOutputProperties), "NonPrintProperties", new string[0]));
        NonOutputProperties.Add("System.Threading.Tasks.Task.Result");
        DefaultNameSpace          = Resource.GetString(nameof(DefaultNameSpace         ), "");
        DefaultNameSpaceString    = Resource.GetString(nameof(DefaultNameSpaceString   ), Resource.Unescape(@"..."));
        ReflectionClasses         = new HashSet<string>(Resource.GetStrings(nameof(ReflectionClasses), new string[0]));
        ReflectionClasses.Add(typeof(Tuple).FullName ?? ""); // Tuple
        ReflectionClasses.Add(typeof(ValueTuple).FullName ?? ""); // ValueTuple
        OutputNonPublicFields     = Resource.GetBool  (nameof(OutputNonPublicFields    ), false); // since 1.4.4
        OutputNonPublicProperties = Resource.GetBool  (nameof(OutputNonPublicProperties), false); // since 1.4.4

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

        // my version
        var versionAttribute = (AssemblyInformationalVersionAttribute?)
            Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyInformationalVersionAttribute));
        string version = versionAttribute?.InformationalVersion ?? "?.?.?";
        var plusIndex = version.IndexOf('+');
        if (plusIndex >= 0)
            version = version.Substring(0, plusIndex);

        // output version log
        Logger.Log($"DebugTrace-net {version} on {RuntimeInformation.FrameworkDescription}");
        Logger.Log($"  properties file path: {Resource.FileInfo.FullName}");
        Logger.Log($"  logger: {Logger}");
    }

    /// <summary>
    /// Returns the indent state of the current thread.
    /// </summary>
    /// <param name="threadId">the thread id</param>
    /// <returns>the indent state of the current thread</returns>
    private static State GetCurrentState(int threadId = -1) {
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
    private static string GetIndentString(int nestLevel, int dataNestLevel) {
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
    private static void PrintStart(State state) {
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
    public static void ResetNest() {
        if (!IsEnabled) return;

        lock (states) {
            GetCurrentState().Reset();
        }
    }

    /// <summary>
    /// Outputs a log when enters the method.
    /// </summary>
    /// <returns>the current thread id</returns>
    public static int Enter() {
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
    public static void Leave(int threadId = -1) {
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
    private static string GetCallerInfo(string baseString, TimeSpan? timeSpan = null) {
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
    /// <returns>the message</returns>
    public static string Print(string message) {
        if (IsEnabled)
            PrintSub(message);
        return message;
    }

    /// <summary>
    /// Outputs the message to the log.
    /// </summary>
    /// <param name="messageSupplier">the message supplier</param>
    /// <returns>the message if IsEnabled, otherwise a empty string</returns>
    public static string Print(Func<string> messageSupplier) {
        if (IsEnabled) {
            var message = messageSupplier();
            PrintSub(message);
            return message;
        }
        return "";
    }

    /// <summary>
    /// Outputs the message to the log.
    /// </summary>
    /// <param name="message">the message</param>
    private static void PrintSub(string message) {
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
    /// <param name="value">the value to output</param>
    /// <param name="printOptions">print options</param>
    private static void PrintSub(string name, object? value, PrintOptions printOptions) {
        lock (states) {
            var state = GetCurrentState();
            PrintStart(state); // Common start processing of output

            reflectedObjects.Clear();

            var buff = new LogBuffer();

            buff.Append(name);
        //  var normalizedName = name.Substring(name.LastIndexOf('.') + 1).Trim();
        //  normalizedName = normalizedName.Substring(normalizedName.LastIndexOf(' ') + 1);
            var valueBuff = ToString(value, printOptions);
            buff.Append(VarNameValueSeparator, valueBuff);

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

    private static string thisClassFullName = typeof(Trace).FullName + '.';

    /// <summary>
    /// Returns the caller stack trace element.
    /// </summary>
    /// <returns>the caller stack trace element</returns>
    private static StackTraceElement GetStackTraceElement() {
        var elements = GetStackTraceElements(1);

        var result = elements.Count() > 0
            ? elements.ElementAt(0)
            : new StackTraceElement("--", "--", "--", 0);

        return result;
    }

    /// <summary>
    /// Returns stack trace elements.
    /// </summary>
    /// <param name="maxCount">maximum number of stack trace elements to return</param>
    /// <returns>stack trace elements</returns>
    /// <since>1.5.5</since>
    private static StackTraceElement[] GetStackTraceElements(int maxCount) {
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
    private static (string, string) Split(string str, char separator) {
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
    private static (string, string) LastSplit(string str, char separator) {
        var index = str.LastIndexOf(separator);
        return index < 0
            ? (str, "")
            : (str.Substring(0, index), str.Substring(index + 1));
    }

    /// <summary>
    /// Outputs the name and value to the log.
    /// </summary>
    /// <typeparam name="T">the type of the value</typeparam>
    /// <param name="name">the name of the value</param>
    /// <param name="value">the value to output</param>
    /// <param name="forceReflection">if true, outputs using reflection even if it has ToString() method</param>
    /// <param name="outputNonPublicFields">if true, outputs non-public field when using reflection (Overrides Debugtarace.properties value)</param>
    /// <param name="outputNonPublicProperties">if true, outputs non-public properties when using reflection (Overrides Debugtarace.properties value)</param>
    /// <param name="minimumOutputCount">the minimum value to output the number of elements for IDictionary and IEnumerable (Overrides Debugtarace.properties value)</param>
    /// <param name="minimumOutputLength">the minimum value to output the length of string (Overrides Debugtarace.properties value)</param>
    /// <param name="collectionLimit">output limit for IDictionary and IEnumerable elements (Overrides Debugtarace.properties value)</param>
    /// <param name="stringLimit">output limit of characters for string (Overrides Debugtarace.properties value)</param>
    /// <param name="reflectionNestLimit">the nest limit when using reflection (Overrides Debugtarace.properties value)</param>
    /// <returns>the value</returns>
    public static T? Print<T>(string name, T? value,
            bool forceReflection = false,
            bool? outputNonPublicFields = null,
            bool? outputNonPublicProperties = null,
            int minimumOutputCount = -1,
            int minimumOutputLength = -1,
            int collectionLimit = -1,
            int stringLimit = -1,
            int reflectionNestLimit = -1) {
        if (IsEnabled) {
            var printOptions = new PrintOptions(
                forceReflection,
                outputNonPublicFields     == null ? OutputNonPublicFields     : outputNonPublicFields.Value,
                outputNonPublicProperties == null ? OutputNonPublicProperties : outputNonPublicProperties.Value,
                minimumOutputCount        == -1   ? MinimumOutputCount        : minimumOutputCount,
                minimumOutputLength       == -1   ? MinimumOutputLength       : minimumOutputLength,
                collectionLimit           == -1   ? CollectionLimit           : collectionLimit,
                stringLimit               == -1   ? StringLimit               : stringLimit,
                reflectionNestLimit       == -1   ? ReflectionNestLimit       : reflectionNestLimit
            );
            PrintSub(name, value, printOptions);
        }
        return value;
    }

    /// <summary>
    /// Outputs the name and value to the log.
    /// Outputs an array of StackTraceElement to the log.
    /// </summary>
    /// <typeparam name="T">the type of the value</typeparam>
    /// <param name="name">the name of the value</param>
    /// <param name="valueSupplier">the supplier of value to output</param>
    /// <param name="forceReflection">if true, outputs using reflection even if it has ToString() method</param>
    /// <param name="outputNonPublicFields">if true, outputs non-public field when using reflection (Overrides Debugtarace.properties value)</param>
    /// <param name="outputNonPublicProperties">if true, outputs non-public properties when using reflection (Overrides Debugtarace.properties value)</param>
    /// <param name="minimumOutputCount">the minimum value to output the number of elements for IDictionary and IEnumerable (Overrides Debugtarace.properties value)</param>
    /// <param name="minimumOutputLength">the minimum value to output the length of string (Overrides Debugtarace.properties value)</param>
    /// <param name="collectionLimit">output limit for IDictionary and IEnumerable elements (Overrides Debugtarace.properties value)</param>
    /// <param name="stringLimit">output limit of characters for string (Overrides Debugtarace.properties value)</param>
    /// <param name="reflectionNestLimit">the nest limit when using reflection (Overrides Debugtarace.properties value)</param>
    /// <returns>the value if IsEnabled, otherwise a default value of the T type</returns>
    public static T? Print<T>(string name, Func<T?> valueSupplier,
            bool forceReflection = false,
            bool? outputNonPublicFields = null,
            bool? outputNonPublicProperties = null,
            int minimumOutputCount = -1,
            int minimumOutputLength = -1,
            int collectionLimit = -1,
            int stringLimit = -1,
            int reflectionNestLimit = -1) {
        if (IsEnabled) {
            var printOptions = new PrintOptions(
                forceReflection,
                outputNonPublicFields     == null ? OutputNonPublicFields     : outputNonPublicFields.Value,
                outputNonPublicProperties == null ? OutputNonPublicProperties : outputNonPublicProperties.Value,
                minimumOutputCount        == -1   ? MinimumOutputCount        : minimumOutputCount,
                minimumOutputLength       == -1   ? MinimumOutputLength       : minimumOutputLength,
                collectionLimit           == -1   ? CollectionLimit           : collectionLimit,
                stringLimit               == -1   ? StringLimit               : stringLimit,
                reflectionNestLimit       == -1   ? ReflectionNestLimit       : reflectionNestLimit
            );
            try {
                var value = valueSupplier();
                PrintSub(name, value, printOptions);
                return value;
            }
            catch (Exception ex) {
                PrintSub(name, ex.ToString(), printOptions);
            }
        }
        return default;
    }

    /// <summary>
    /// Outputs an array of StackTraceElement to the log.
    /// </summary>
    /// <param name="maxCount">maximum number of stack trace elements to output</param>
    /// <since>1.5.5</since>
    public static void PrintStack(int maxCount) {
        Print("Stack", GetStackTraceElements(maxCount));
    }

    /// <summary>
    /// Creates a string buffer from the value.
    /// </summary>
    /// <param name="value">the value object</param>
    /// <param name="printOptions">print options</param>
    /// <param name="isElement"><c>true</c> if the value is element of a container class, <c>false</c> otherwise</param>
    /// <returns>a LogBuffer</returns>
    private static LogBuffer ToString(object? value, PrintOptions printOptions, bool isElement = false) {
        var buff = new LogBuffer();

        if (value == null) {
            buff.Append("null");
            return buff;
        }

        var type = value.GetType();

        var typeName = GetTypeName(type, value, printOptions, isElement);
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
            case string   stringValue: buff.Append(typeName); AppendString(buff, stringValue, printOptions); return buff;
            case IDictionary dictionary: return ToStringDictionary(dictionary, printOptions);
            case IEnumerable enumerable: return ToStringEnumerable(enumerable, printOptions);
            case Enum       enumValue: buff.Append(typeName); buff.Append(enumValue); return buff;
            default:
                if (printOptions.ForceReflection || !HasToString(type)) {
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

            else if (reflectedObjects.Count >= printOptions.ReflectionNestLimit)
                // Over reflection level limitation
                buff.NoBreakAppend(LimitString);

            else {
                // Use Reflection
                reflectedObjects.Add(value);
                var valueBuff = ToStringReflection(value, printOptions);
                buff.Append(null, valueBuff);
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
    /// <param name="printOptions">print options</param>
    /// <param name="isElement"><c>true</c> if the value is element of a container class, <c>false</c> otherwise</param>
    /// <param name="nest">current nest count</param>
    /// <returns>the type name to be output to the log</returns>
    private static string GetTypeName(Type type, object? value, PrintOptions printOptions, bool isElement, int nest = 0) {
        var typeName = "";
        if (type.IsArray) {
            // Array
            typeName = GetArrayTypeName(type, value, printOptions, isElement, nest);

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
                        } else if (count >= printOptions.MinimumOutputCount)
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
    /// <param name="printOptions">print options</param>
    /// <param name="isElement"><c>true</c> if the value is element of a container class, <c>false</c> otherwise</param>
    /// <param name="nest">current nest count</param>
    /// <returns>the type name to be output to the log</returns>
    private static string GetArrayTypeName(Type type, object? value, PrintOptions printOptions, bool isElement, int nest) {
        string typeName = GetTypeName(type.GetElementType()!, null, printOptions, false, nest + 1);

        if (typeName.Length > 0) {
            var bracket = "[";
            if (value != null)
                bracket += ((Array)value).Length;
            bracket += ']';
            int braIndex = typeName.IndexOf('[');
            if (braIndex < 0)
                braIndex = typeName.Length;
            typeName = typeName.Substring(0, braIndex) + bracket + typeName.Substring(braIndex);
        }

        return typeName;
    }

    // GetTypeName
    private static string GetTypeName(Type type) {
        var builder = new StringBuilder();
        AppendTypeName(builder, type);
        return  builder.ToString();
    }

    // AppendTypeName
    private static void AppendTypeName(StringBuilder builder, Type type) {
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
    private static string GetFullTypeName(Type type) {
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
    private static string ReplaceTypeName(string typeName) {
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
    private static void Append(LogBuffer buff, bool value) {buff.Append(value ? "true" : "false");}

    /// <summary>
    /// Appends a string representation of the value to the log buffer.
    /// </summary>
    /// <param name="buff">the log buffer</param>
    /// <param name="value">the value</param>
    /// <returns></returns>
    private static void Append(LogBuffer buff, char value) {
        buff.Append('\'');
        AppendChar(buff, value, '\'', true); buff.Append('\'');
    }

    /// <summary>
    /// Appends a string representation of the value to the log buffer.
    /// </summary>
    /// <param name="buff">the log buffer</param>
    /// <param name="value">the value</param>
    /// <returns></returns>
    private static void Append(LogBuffer buff, sbyte value) {buff.Append(value);}

    /// <summary>
    /// Appends a string representation of the value to the log buffer.
    /// </summary>
    /// <param name="buff">the log buffer</param>
    /// <param name="value">the value</param>
    /// <returns></returns>
    private static void Append(LogBuffer buff, byte value) {buff.Append(value);}

    /// <summary>
    /// Appends a string representation of the value to the log buffer.
    /// </summary>
    /// <param name="buff">the log buffer</param>
    /// <param name="value">the value</param>
    /// <returns></returns>
    private static void Append(LogBuffer buff, short value) {buff.Append(value);}

    /// <summary>
    /// Appends a string representation of the value to the log buffer.
    /// </summary>
    /// <param name="buff">the log buffer</param>
    /// <param name="value">the value</param>
    /// <returns></returns>
    private static void Append(LogBuffer buff, ushort value) {buff.Append(value);}

    /// <summary>
    /// Appends a string representation of the value to the log buffer.
    /// </summary>
    /// <param name="buff">the log buffer</param>
    /// <param name="value">the value</param>
    /// <returns></returns>
    private static void Append(LogBuffer buff, int value) {buff.Append(value);}

    /// <summary>
    /// Appends a string representation of the value to the log buffer.
    /// </summary>
    /// <param name="buff">the log buffer</param>
    /// <param name="value">the value</param>
    /// <returns></returns>
    private static void Append(LogBuffer buff, uint value) {buff.Append(value).Append('u' );}

    /// <summary>
    /// Appends a string representation of the value to the log buffer.
    /// </summary>
    /// <param name="buff">the log buffer</param>
    /// <param name="value">the value</param>
    /// <returns></returns>
    private static void Append(LogBuffer buff, long value) {buff.Append(value).Append('L' );}

    /// <summary>
    /// Appends a string representation of the value to the log buffer.
    /// </summary>
    /// <param name="buff">the log buffer</param>
    /// <param name="value">the value</param>
    /// <returns></returns>
    private static void Append(LogBuffer buff, ulong value) {buff.Append(value).Append("uL");}

    /// <summary>
    /// Appends a string representation of the value to the log buffer.
    /// </summary>
    /// <param name="buff">the log buffer</param>
    /// <param name="value">the value</param>
    /// <returns></returns>
     private static void Append(LogBuffer buff, float value) {buff.Append(value).Append('f' );}

    /// <summary>
    /// Appends a string representation of the value to the log buffer.
    /// </summary>
    /// <param name="buff">the log buffer</param>
    /// <param name="value">the value</param>
    /// <returns></returns>
    private static void Append(LogBuffer buff, double value) {buff.Append(value).Append('d' );}

    /// <summary>
    /// Appends a string representation of the value to the log buffer.
    /// </summary>
    /// <param name="buff">the log buffer</param>
    /// <param name="value">the value</param>
    /// <returns></returns>
    private static void Append(LogBuffer buff, decimal value) {buff.Append(value).Append('m' );}

    /// <summary>
    /// Appends a string representation of the value to the log buffer.
    /// </summary>
    /// <param name="buff">the log buffer</param>
    /// <param name="value">the value</param>
    /// <returns></returns>
    private static void Append(LogBuffer buff, DateTime value) {buff.Append(string.Format(DateTimeFormat, value));}

    /// <summary>
    /// Appends a character representation for logging to the string buffer.
    /// </summary>
    /// <param name="buff">the logging buffer</param>
    /// <param name="ch">a character</param>
    /// <param name="enclosure">the enclosure character</param>
    /// <param name="escape">escape characters if true, dose not escape characters otherwise</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise</returns>
    private static void AppendChar(LogBuffer buff, char ch, char enclosure, bool escape) {
        if (escape) {
            // escape
            switch (ch) {
            case '\0': buff.NoBreakAppend(@"\0"); break; // 00 NUL
            case '\a': buff.NoBreakAppend(@"\a"); break; // 07 BEL
            case '\b': buff.NoBreakAppend(@"\b"); break; // 08 BS
            case '\t': buff.NoBreakAppend(@"\t"); break; // 09 HT
            case '\n': buff.NoBreakAppend(@"\n"); break; // 0A LF
            case '\v': buff.NoBreakAppend(@"\v"); break; // 0B VT
            case '\f': buff.NoBreakAppend(@"\f"); break; // 0C FF
            case '\r': buff.NoBreakAppend(@"\r"); break; // 0D CR
            case '\\': buff.NoBreakAppend(@"\\"); break; // \
            default:
                if (ch < ' ' || ch == '\u007F')
                    buff.NoBreakAppend(string.Format(@"\u{0:X4}", (ushort)ch));
                else {
                    if (ch == enclosure)
                        buff.NoBreakAppend('\\');
                    buff.NoBreakAppend(ch);
                }
                break;
            }
        } else {
            // dose not escape
            buff.NoBreakAppend(ch);
        }
    }

    /// <summary>
    /// Appends a CharSequence representation for logging to the string buffer.
    /// </summary>
    /// <param name="buff">the logging buffer</param>
    /// <param name="str">a string object</param>
    /// <param name="printOptions">print options</param>
    /// <returns>always false</returns>
    private static void AppendString(LogBuffer buff, string str, PrintOptions printOption) {
        if (str.Length >= printOption.MinimumOutputLength)
            buff.NoBreakAppend(string.Format(LengthFormat, str.Length));

        var hasBackslash = false;
        var hasEscaped = false;
        for (var index = 0; index < str.Length; ++index) {
            if (index >= printOption.StringLimit)
                break;
            var ch = str[index];
            if (ch == '\\')
                hasBackslash = true;
            else if (ch < ' ' || ch == '"' || ch == '\u007F')
                hasEscaped = true;
        }

        if (hasBackslash && !hasEscaped)
            buff.NoBreakAppend('@');
        buff.NoBreakAppend('"');

        for (var index = 0; index < str.Length; ++index) {
            if (index >= printOption.StringLimit) {
                buff.NoBreakAppend(LimitString);
                break;
            }
            AppendChar(buff, str[index], '"', hasEscaped);
        }

        buff.NoBreakAppend('"');
    }

    /// <summary>
    /// Creates a string buffer from the dictionary.
    /// </summary>
    /// <param name="dictionary">a IDictionary</param>
    /// <param name="printOptions">print options</param>
    /// <returns>a LogBuffer</returns>
    private static LogBuffer ToStringDictionary(IDictionary dictionary, PrintOptions printOptions) {
        var buff = new LogBuffer();

        buff.Append(GetTypeName(dictionary.GetType(), dictionary, printOptions, false));
        buff.Append('{');

        var bodyBuff = ToStringDictionaryBody(dictionary, printOptions);

        var isMultiLines = bodyBuff.IsMultiLines || buff.Length + bodyBuff.Length > MaximumDataOutputWidth;

        if (isMultiLines) {
            buff.LineFeed();
            buff.UpNest();
        }

        buff.Append(null, bodyBuff);

        if (isMultiLines) {
            buff.LineFeed();
            buff.DownNest();
        }

        buff.Append('}');

        return buff;
    }

    private static LogBuffer ToStringDictionaryBody(IDictionary dictionary, PrintOptions printOptions) {
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

            var elementBuff = ToString(key, printOptions, true)
                .Append(KeyValueSeparator, ToString(value, printOptions, true));
            if (index > 0 && (wasMultiLines || elementBuff.IsMultiLines))
                buff.LineFeed();
            buff.Append(null, elementBuff);

            wasMultiLines = elementBuff.IsMultiLines;
            ++index;
        }

        return buff;
    }

    /// <summary>
    /// Creates a string buffer from the enumerable.
    /// </summary>
    /// <param name="enumerable">a IEnumerable object</param>
    /// <param name="printOptions">print options</param>
    /// <returns>a LogBuffer</returns>
    /// <since>1.4.1</since>

    private static LogBuffer ToStringEnumerable(IEnumerable enumerable, PrintOptions printOptions) {
        var buff = new LogBuffer();

        buff.Append(GetTypeName(enumerable.GetType(), enumerable, printOptions, false));
        buff.Append('{');

        var bodyBuff = ToStringEnumerableBody(enumerable, printOptions);

        var isMultiLines = bodyBuff.IsMultiLines || buff.Length + bodyBuff.Length > MaximumDataOutputWidth;

        if (isMultiLines) {
            buff.LineFeed();
            buff.UpNest();
        }

        buff.Append(null, bodyBuff);

        if (isMultiLines) {
            buff.LineFeed();
            buff.DownNest();
        }

        buff.Append('}');

        return buff;
    }

    private static LogBuffer ToStringEnumerableBody(IEnumerable enumerable, PrintOptions printOptions) {
        var buff = new LogBuffer();

        var wasMultiLines = false;
        var index = 0;
        foreach (var element in enumerable) {
            if (index > 0)
                buff.NoBreakAppend(", "); // Append a delimiter

            if (index >= printOptions.CollectionLimit) {
                buff.Append(LimitString);
                break;
            }

            var elementBuff = ToString(element, printOptions, true);
            if (index > 0 && (wasMultiLines || elementBuff.IsMultiLines))
                buff.LineFeed();
            buff.Append(null, elementBuff);

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
    private static bool HasToString(Type type) {
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
    /// <param name="obj">an object</param>
    /// <param name="printOptions">print options</param>
    /// <returns>a LogBuffer</returns>
    private static LogBuffer ToStringReflection(object obj, PrintOptions printOptions) {
        var buff = new LogBuffer();

        var type = obj.GetType();
        buff.Append(GetTypeName(type, obj, printOptions, false));
        var isExtended = type.BaseType != typeof(object) && type.BaseType != typeof(ValueType);
        var isTuple = IsTuple(type);

        var bodyBuff = ToStringReflectionBody(obj, type, isExtended, printOptions);

        buff.Append(isTuple ? '(' : '{');
        if (bodyBuff.IsMultiLines) {
            buff.LineFeed();
            buff.UpNest();
        }

        buff.Append(null, bodyBuff);

        if (bodyBuff.IsMultiLines) {
            if (buff.Length > 0)
                buff.LineFeed();
            buff.DownNest();
        }
        buff.Append(isTuple ? ')' : '}');

        return buff;
    }

    private static LogBuffer ToStringReflectionBody(object obj, Type type, bool isExtended, PrintOptions printOptions) {
        var buff = new LogBuffer();

        Type? baseType = type.BaseType;
        if (baseType != null && baseType != typeof(object) && baseType != typeof(ValueType)) {
            // Call for the base type
            var baseBuff =  ToStringReflectionBody(obj, baseType, isExtended, printOptions);
            buff.Append(null, baseBuff);
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
            | (printOptions.OutputNonPublicFields ? BindingFlags.NonPublic : 0) // since 1.4.4
            | BindingFlags.Instance)
            .Where(fieldInfo => !fieldInfo.Name.EndsWith("__BackingField")); // Exclude property backing fields // since 1.4.4

        foreach (var fieldInfo in fieldInfos) {
            if (fieldPropertyIndex > 0)
                buff.NoBreakAppend(", "); // Append a delimiter

            var valueBuff = ToStringReflectValue(type, obj, fieldInfo, printOptions);
            if (fieldPropertyIndex > 0 && (wasMultiLines || valueBuff.IsMultiLines))
                buff.LineFeed();
            buff.Append(null, valueBuff);

            wasMultiLines = valueBuff.IsMultiLines;
            ++fieldPropertyIndex;
        }

        // properties
        var propertyInfos = type.GetProperties(
                BindingFlags.DeclaredOnly
            | BindingFlags.Public
            | (printOptions.OutputNonPublicProperties ? BindingFlags.NonPublic : 0) // since 1.4.4
            | BindingFlags.Instance);

        foreach (var propertyInfo in propertyInfos) {
            var parameterInfos = propertyInfo.GetIndexParameters();
            if (parameterInfos.Length > 0) continue; // Not support indexed properties

            if (fieldPropertyIndex > 0)
                buff.NoBreakAppend(", "); // Append a delimiter

            var valueBuff = ToStringReflectValue(type, obj, propertyInfo, printOptions);
            if (fieldPropertyIndex > 0 && (wasMultiLines || valueBuff.IsMultiLines))
                buff.LineFeed();
            buff.Append(null, valueBuff);

            wasMultiLines = valueBuff.IsMultiLines;
            ++fieldPropertyIndex;
        }

        return buff;
    }

    // AppendReflectValue / MemberInfo
    private static LogBuffer ToStringReflectValue(Type type, object obj, MemberInfo memberInfo, PrintOptions printOptions) {
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

        var separator = null as string;
        if (!IsTuple(type)) {
            //  not Tuple
            if (memberType != null && (value == null || memberType != value.GetType()))
                buff.Append(GetFullTypeName(memberType)).Append(' ');
            buff.Append(memberInfo.Name);
            separator = KeyValueSeparator;
        }

        if (nonOutput) {
            if (separator != null)
                buff.NoBreakAppend(separator);
            buff.NoBreakAppend(NonOutputString);
    } else
            buff.Append(separator, ToString(value, printOptions));

        return buff;
    }

    /// <summary>
    /// Appends the access modifire of the member information to the log buffer.
    /// </summary>
    /// <param name="buff">the log buffer</param>
    /// <param name="memberInfo">the member information</param>
    /// <since>1.5.0</since>
    private static void AppendAccessModifire(LogBuffer buff, MemberInfo memberInfo) {
        switch (memberInfo) {
        case FieldInfo fieldInfo:
            if (!fieldInfo.IsPublic) {
                if      (fieldInfo.IsPrivate          ) buff.Append("private ");
                else if (fieldInfo.IsFamily           ) buff.Append("protected ");
                else if (fieldInfo.IsAssembly         ) buff.Append("internal ");
                else if (fieldInfo.IsFamilyOrAssembly ) buff.Append("protected internal ");
                else if (fieldInfo.IsFamilyAndAssembly) buff.Append("private protected ");
            }
            break;
        case PropertyInfo propertyInfo:
            if (!propertyInfo.GetMethod?.IsPublic ?? false) {
                if      (propertyInfo.GetMethod?.IsPrivate           ?? false) buff.Append("private ");
                else if (propertyInfo.GetMethod?.IsFamily            ?? false) buff.Append("protected ");
                else if (propertyInfo.GetMethod?.IsAssembly          ?? false) buff.Append("internal ");
                else if (propertyInfo.GetMethod?.IsFamilyOrAssembly  ?? false) buff.Append("protected internal ");
                else if (propertyInfo.GetMethod?.IsFamilyAndAssembly ?? false) buff.Append("private protected ");
            }
            break;
        }
    }

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

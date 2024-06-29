// NLog.cs
// (C) 2018 Masato Kokubo
using System.Collections.Generic;

namespace DebugTrace;

/// <summary>
/// A logger using NLog.
/// </summary>
///
/// <since>1.0.0</since>
/// <author>Masato Kokubo</author>
public class NLog : ILogger {
    private static readonly Dictionary<string, global::NLog.LogLevel> levelDictionary = 
        new Dictionary<string, global::NLog.LogLevel>() {
            {"TRACE", global::NLog.LogLevel.Trace},
            {"DEBUG", global::NLog.LogLevel.Debug},
            {"INFO" , global::NLog.LogLevel.Info },
            {"WARN" , global::NLog.LogLevel.Warn },
            {"ERROR", global::NLog.LogLevel.Error},
            {"FATAL", global::NLog.LogLevel.Fatal},
            {"OFF"  , global::NLog.LogLevel.Off  },
        };

    // NLog Logger
    private readonly global::NLog.Logger logger = global::NLog.LogManager.GetLogger(typeof(ILogger).Namespace);

    private static readonly string defaultLevelStr = "DEBUG";
    private string levelStr = defaultLevelStr;
    private global::NLog.LogLevel level = levelDictionary[defaultLevelStr];

    /// <summary>
    /// The only instance of this class
    /// </summary>
    public static NLog Instance {get;} = new NLog();

    private NLog() {
    }

    /// <summary>
    /// Set the logging level
    /// </summary>
    public string Level {
        get => levelStr;
        set {
            Trace.RequreNonNull(value, "value"); // since 1.1.1
            var upperValue = value.ToUpper();
            if (levelDictionary.ContainsKey(upperValue)) {
                level = levelDictionary[upperValue];
                levelStr = value;
            } else
                System.Console.Error.WriteLine($"LogLevel: \"{value}\" is unknown.");
        }
    }

    /// <summary>
    /// Whether logging is enabled.
    /// </summary>
    public bool IsEnabled {get => logger.IsEnabled(level);}

    /// <summary>
    /// Output the message to the log.
    /// </summary>
    ///
    /// <param name="message">the message</param>
    public void Log(string message) {
        logger.Log(level, message);
    }

    /// <summary>
    /// Returns a string representation of this object.
    /// </summary>
    ///
    /// <returns>a string representation of this object</returns>
    /// <since>1.5.0</since>
    public override string ToString() => GetType().FullName ?? "";
}

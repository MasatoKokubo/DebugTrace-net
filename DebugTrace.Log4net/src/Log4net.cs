// Log4net.cs
// (C) 2018 Masato Kokubo
using System.Collections.Generic;
using System.Reflection;
using log4net;
using log4net.Core;

[assembly: log4net.Config.XmlConfigurator(ConfigFile="Log4net.config", Watch=true)]

namespace DebugTrace;

/// <summary>
/// A logger using Log4net.
/// </summary>
///
/// <since>1.0.0</since>
/// <author>Masato Kokubo</author>
public class Log4net : ILogger {
    private static readonly Dictionary<string, Level> levelDictionary = 
        new Dictionary<string, Level>() {
            {"ALL"      , log4net.Core.Level.All      },
            {"FINEST"   , log4net.Core.Level.Finest   },
            {"VERBOSE"  , log4net.Core.Level.Verbose  },
            {"FINER"    , log4net.Core.Level.Finer    },
            {"TRACE"    , log4net.Core.Level.Trace    },
            {"FINE"     , log4net.Core.Level.Fine     },
            {"DEBUG"    , log4net.Core.Level.Debug    },
            {"INFO"     , log4net.Core.Level.Info     },
            {"NOTICE"   , log4net.Core.Level.Notice   },
            {"WARN"     , log4net.Core.Level.Warn     },
            {"ERROR"    , log4net.Core.Level.Error    },
            {"SEVERE"   , log4net.Core.Level.Severe   },
            {"CRITICAL" , log4net.Core.Level.Critical },
            {"ALERT"    , log4net.Core.Level.Alert    },
            {"FATAL"    , log4net.Core.Level.Fatal    },
            {"EMERGENCY", log4net.Core.Level.Emergency},
            {"OFF"      , log4net.Core.Level.Off      },
        };

    // Log4net Logger
    private readonly log4net.Core.ILogger logger = LogManager.GetLogger(
        LogManager.CreateRepository(
            Assembly.GetExecutingAssembly(),
            typeof(log4net.Repository.Hierarchy.Hierarchy)).Name,
        "DebugTrace").Logger;

    private static readonly string defaultLevelStr = "DEBUG";
    private string levelStr = defaultLevelStr;
    private Level level = levelDictionary[defaultLevelStr];

    /// <summary>
    /// The only instance of this class
    /// </summary>
    public static Log4net Instance {get;} = new Log4net();

    private Log4net() {
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
    public bool IsEnabled {get => logger.IsEnabledFor(level);}

    /// <summary>
    /// Output the message to the log.
    /// </summary>
    public void Log(string message) {
        logger.Log(null, level, message, null);
    }

    /// <summary>
    /// Returns a string representation of this object.
    /// </summary>
    /// <returns>a string representation of this object</returns>
    /// <since>1.5.0</since>
    public override string ToString() => GetType().FullName ?? "";

}

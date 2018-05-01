// Log4net.cs
// (C) 2018 Masato Kokubo
using System.Collections.Generic;
using log4net;
using log4net.Core;

namespace DebugTrace {
    /// <summary>
    /// A logger using Log4net.
    /// </summary>
    ///
    /// <since>1.0.0</since>
    /// <author>Masato Kokubo</author>
    public class Log4net : ILogger {
        private static readonly Dictionary<string, Level> levelDictinary = 
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
        private log4net.Core.ILogger logger = LogManager.GetLogger(typeof(ILogger).Namespace).Logger;

        private static string defaultLevelStr = "DEBUG";
        private string levelStr = defaultLevelStr;
        private Level level = levelDictinary[defaultLevelStr];

        public static Log4net Instance {get;} = new Log4net();

        private Log4net() {
        }

        /// <summary>
        /// Set the logging level
        /// </summary>
        public string Level {
            get => levelStr;
            set {
                var upperValue = value.ToUpper();
                if (levelDictinary.ContainsKey(upperValue)) {
                    level = levelDictinary[upperValue];
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
    }
}

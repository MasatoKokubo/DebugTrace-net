// Console.cs
// (C) 2018 Masato Kokubo

using System;
using System.Collections.Generic;
using System.Linq;

namespace DebugTrace {
    /// <summary>
    /// Using multiple loggers.
    /// </summary>
    /// <since>1.5.0</since>
    /// <author>Masato Kokubo</author>
    public class Loggers : ILogger {
        /// <summary>
        /// The separator string of loggers.
        /// </summary>
        public static string Separator {get;} = ";";

        /// <summary>
        /// The separator character of loggers.
        /// </summary>
        public static char SeparatorChar {get => Separator[0];}

        /// <summary>
        /// IEnumerable of loggers.
        /// </summary>
        public IEnumerable<ILogger> Members {get; private set;} = new List<ILogger>();

        /// <summary>
        /// Set the logging level
        /// </summary>
        public string Level {
            get => string.Join(Separator, Members.Select(logger => logger.Level));
            set {
                TraceBase.RequreNonNull(value, "value");
                var levels = value.Split(SeparatorChar).Select(str => str.Trim()).ToList();
                if (levels.Count == 0)
                    return;
                var levelsIndex = 0;
                foreach (var logger in Members) {
                    logger.Level = levels[levelsIndex];
                    if (levelsIndex < levels.Count - 1)
                        ++levelsIndex;
                }
            }
        }

        /// <summary>
        /// Whether logging is enabled.
        /// </summary>
        public bool IsEnabled {get => Members.Any(logger => logger.IsEnabled);}

        private Loggers() {
        }

        /// <summary>
        /// Constuct a Loggers object.
        /// </summary>
        /// <param name="loggers">an array of ILoggers</param>
        /// <exception cref="NullReferenceException">if the loggers is null</exception>
        public Loggers(params ILogger[] loggers) {
            Members = TraceBase.RequreNonNull(loggers, "loggers");
        }

        /// <summary>
        /// Output the message to the log.
        /// </summary>
        /// <param name="message">the message</param>
        public void Log(string message) {
            foreach (var logger in Members)
                logger.Log(message);
        }

        /// <summary>
        /// Returns a string representation of this object.
        /// </summary>
        /// <returns>a string representation of this object</returns>
        /// <since>1.5.0</since>
        public override string ToString() => string.Join(Separator + " ", Members.Select(logger => logger.ToString()));
    }
}

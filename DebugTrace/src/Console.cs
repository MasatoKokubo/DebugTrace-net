// Console.cs
// (C) 2018 Masato Kokubo

namespace DebugTrace {
    /// <summary>
    /// A logger using System.out or System.err.
    /// </summary>
    ///
    /// <since>1.0.0</since>
    /// <author>Masato Kokubo</author>
    public abstract class Console : ILogger {
        /// <summary>
        /// Set the logging level
        /// </summary>
        public string Level {get; set;}

        /// <summary>
        /// Whether logging is enabled.
        /// </summary>
        public bool IsEnabled {get => true;}

        /// <summary>
        /// Output the message to the log.
        /// </summary>
        public abstract void Log(string message);

        /// <summary>
        /// A logger using System.out.
        /// </summary>
        public class Out : Console {
            public static Out Instance {get;} = new Out();

            private Out() {
            }

            /// <summary>
            /// Output the message to the log.
            /// </summary>
            public override void Log(string message) {
                System.Console.Out.WriteLine(TraceBase.DateTimeString + ' ' + message);
            }
        }

        /// <summary>
        /// A logger using System.err.
        /// </summary>
        public class Error : Console {
            public static Error Instance {get;} = new Error();

            private Error() {
            }

            /// <summary>
            /// Output the message to the log.
            /// </summary>
            public override void Log(string message) {
                System.Console.Error.WriteLine(TraceBase.DateTimeString + ' ' + message);
            }
        }
    }
}

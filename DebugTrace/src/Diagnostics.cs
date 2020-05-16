// Diagnostics.cs
// (C) 2018 Masato Kokubo
using System;
using System.Threading;

namespace DebugTrace {
    /// <summary>
    /// The base class of <c>Diagnostics.Debug</c> and <c>Diagnostics.Trace</c> class.
    /// </summary>
    /// <since>1.5.5</since>
    /// <author>Masato Kokubo</author>
    public abstract class Diagnostics : ILogger {
        /// <summary>
        /// Set the logging level
        /// </summary>
        public string Level {get; set;} = "";

        /// <summary>
        /// Whether logging is enabled.
        /// </summary>
        public bool IsEnabled {get;} = true;

        /// <summary>
        /// Output the message to the log.
        /// </summary>
        /// <param name="message">the message</param>
        public abstract void Log(string message);

        /// <summary>
        /// Returns a string representation of this object.
        /// </summary>
        /// <returns>a string representation of this object</returns>
        /// <since>1.5.0</since>
        public override string ToString() => GetType().FullName ?? "";

        /// <summary>
        /// A logger using <c>System.Diagnostics.Debug</c>.
        /// </summary>
        public class Debug : Diagnostics {
            /// <summary>
            /// The only <c>Console.Out</c> object.
            /// </summary>
            public static Debug Instance {get;} = new Debug();

            private Debug() {
            }

            /// <summary>
            /// Output the message to the log.
            /// </summary>
            /// <param name="message">the message</param>
            public override void Log(string message) {
                System.Diagnostics.Debug.WriteLine(string.Format(TraceBase.LogDateTimeFormat,
                    DateTime.Now, Thread.CurrentThread.ManagedThreadId, message));
            }
        }

        /// <summary>
        /// A logger using <c>System.Diagnostics.Trace</c>.
        /// </summary>
        public class Trace : Diagnostics {
            /// <summary>
            /// The only <c>DebugTrace.Diagnostics.Trace</c> object.
            /// </summary>
            public static Trace Instance {get;} = new Trace();

            private Trace() {
            }

            /// <summary>
            /// Output the message to the log.
            /// </summary>
            /// <param name="message">the message</param>
            public override void Log(string message) {
                System.Diagnostics.Trace.WriteLine(string.Format(TraceBase.LogDateTimeFormat,
                    DateTime.Now, Thread.CurrentThread.ManagedThreadId, message));
            }
        }
    }
}

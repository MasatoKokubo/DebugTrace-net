// Console.cs
// (C) 2018 Masato Kokubo
using System;
using System.Threading;

namespace DebugTrace;

/// <summary>
/// The base class of <c>Console.Out</c> and <c>Console.Error</c> class.
/// </summary>
/// <since>1.0.0</since>
/// <author>Masato Kokubo</author>
public abstract class Console : ILogger {
    /// <summary>
    /// Set the logging level
    /// </summary>
    public string Level {get; set;} = "";

    /// <summary>
    /// Whether logging is enabled.
    /// </summary>
    public bool IsEnabled {get => true;}

    /// <summary>
    /// Output the message to the log.
    /// </summary>
    ///
    /// <param name="message">the message</param>
    public abstract void Log(string message);

    /// <summary>
    /// Returns a string representation of this object.
    /// </summary>
    /// <returns>a string representation of this object</returns>
    /// <since>1.5.0</since>
    public override string ToString() => GetType().FullName ?? "";

    /// <summary>
    /// A logger using <c>System.Console.Out</c>.
    /// </summary>
    public class Out : Console {
        /// <summary>
        /// The only <c>DebugTrace.Console.Out</c> object.
        /// </summary>
        public static Out Instance {get;} = new Out();

        private Out() {
        }

        /// <summary>
        /// Output the message to the log.
        /// </summary>
        /// <param name="message">the message</param>
        public override void Log(string message) {
            System.Console.Out.WriteLine(string.Format(Trace.LogDateTimeFormat,
                DateTime.Now, Thread.CurrentThread.ManagedThreadId, message));
        }
    }

    /// <summary>
    /// A logger using <c>System.Console.Error</c>.
    /// </summary>
    public class Error : Console {
        /// <summary>
        /// The only <c>DebugTrace.Console.Error</c> object.
        /// </summary>
        public static Error Instance {get;} = new Error();

        private Error() {
        }

        /// <summary>
        /// Output the message to the log.
        /// </summary>
        /// <param name="message">the message</param>
        public override void Log(string message) {
            System.Console.Error.WriteLine(string.Format(Trace.LogDateTimeFormat,
                DateTime.Now, Thread.CurrentThread.ManagedThreadId, message));
        }
    }
}

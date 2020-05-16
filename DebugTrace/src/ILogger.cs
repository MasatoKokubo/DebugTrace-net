// ILogger.cs
// (C) 2018 Masato Kokubo
namespace DebugTrace {
    /// <summary>
    /// Interface of Logger classes.
    /// </summary>
    /// <since>1.0.0</since>
    /// <author>Masato Kokubo</author>
    public interface ILogger {
        /// <summary>
        /// The logging level
        /// </summary>
        string Level {get; set;}

        /// <summary>
        /// Whether logging is enabled.
        /// </summary>
        bool IsEnabled {get;}

        /// <summary>
        /// Output the message to the log.
        /// </summary>
        /// <param name="message">the message</param>
        void Log(string message);
    }
}

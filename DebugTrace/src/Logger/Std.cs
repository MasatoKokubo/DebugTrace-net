// Std.cs
// (C) 2018 Masato Kokubo

using System;

namespace DebugTrace.Logger {
	/// <summary>
	/// A logger using System.out or System.err.
	/// </summary>
	///
	/// <since>1.0.0</since>
	/// <author>Masato Kokubo</author>
	public abstract class Std : ILogger {
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
		public class Out : Std {
			/// <summary>
			/// Output the message to the log.
			/// </summary>
			public override void Log(string message) {
			//	Console.Out.WriteLine(DebugTrace.appendTimestamp(message));
				Console.Out.WriteLine(message); // TODO
			}
		}

		/// <summary>
		/// A logger using System.err.
		/// </summary>
		public class Err : Std {
			/// <summary>
			/// Output the message to the log.
			/// </summary>
			public override void Log(string message) {
			//	Console.Error.WriteLine(DebugTrace.appendTimestamp(message));
				Console.Error.WriteLine(message); // TODO
			}
		}
	}
}

// ITrace.cs
// (C) 2018 Masato Kokubo

using System;

namespace DebugTrace {
	/// <summary>
	/// A utility class for debugging.
	/// </summary>
	///
	/// <remarks>
	/// Call DebugTrace.enter and DebugTrace.leave methods when enter and leave your methods,
	/// then outputs execution trace of the program.
	/// </remarks>
	///
	/// <since>1.0.0</since>
	/// <author>Masato Kokubo</author>
	public interface ITrace {
		/// <summary>
		/// Returns whether tracing is enabled.
		/// </summary>
		///
		/// <returns>true if tracing is enabled; false otherwise</returns>
		bool IsEnabled {get;}

		/// <summary>
		/// Returns the last log string output.
		/// </summary>
		///
		/// <returns>the last log string output.</returns>
		string LastLog {get;}

		/// <summary>
		/// Resets the nest level
		/// </summary>
		void ResetNest();

		/// <summary>
		/// Call this method at entrance of your methods.
		/// </summary>
		void Enter();

		/// <summary>
		/// Call this method at exit of your methods.
		/// </summary>
		void Leave();

		/// <summary>
		/// Outputs the message to the log.
		/// </summary>
		///
		/// <param name="message">a message</param>
		void Print(string message);

		/// <summary>
		/// Outputs a message to the log.
		/// </summary>
		///
		/// <param name="messageSupplier">a message supplier</param>
		void Print(Func<string> messageSupplier);

		/// <summary>
		/// Outputs the name and value to the log.
		/// </summary>
		///
		/// <param name="name">the name of the value</param>
		/// <param name="value">the value to output (accept null)</param>
		void Print(string name, object value);

		/// <summary>
		/// Outputs the name and value to the log.
		/// </summary>
		///
		/// <param name="">T> type of the value</param>
		/// <param name="name">the name of the value</param>
		/// <param name="valueSupplier">the supplier of value to output</param>
		void Print<T>(string name, Func<T> valueSupplier);

	}
}

// NLog.cs
// (C) 2018 Masato Kokubo
using System;
using System.Collections.Generic;

namespace DebugTrace {
	/// <summary>
	/// A logger using NLog.
	/// </summary>
	///
	/// <since>1.0.0</since>
	/// <author>Masato Kokubo</author>
	public class NLog : ILogger {
		private static readonly Dictionary<string, global::NLog.LogLevel> levelDictinary = 
			new Dictionary<string, global::NLog.LogLevel>() {
				{"Trace", global::NLog.LogLevel.Trace},
				{"Debug", global::NLog.LogLevel.Debug},
				{"Info" , global::NLog.LogLevel.Info },
				{"Warn" , global::NLog.LogLevel.Warn },
				{"Error", global::NLog.LogLevel.Error},
				{"Fatal", global::NLog.LogLevel.Fatal},
				{"Off"  , global::NLog.LogLevel.Off  },
			};

		// NLog Logger
		private global::NLog.Logger logger = global::NLog.LogManager.GetLogger(typeof(ILogger).Namespace);

		private static string defaultLevelStr = "Debug";
		private string levelStr = defaultLevelStr;
		private global::NLog.LogLevel level = levelDictinary[defaultLevelStr];

		public static NLog Instance {get;} = new NLog();

		private NLog() {
		}

		/// <summary>
		/// Set the logging level
		/// </summary>
		public string Level {
			get => levelStr;
			set {
				if (!levelDictinary.ContainsKey(value))
					new ArgumentException(value);

				level = levelDictinary[value];
				levelStr = value;
			}
		}

		/// <summary>
		/// Whether logging is enabled.
		/// </summary>
		public bool IsEnabled {get => logger.IsEnabled(level);}

		/// <summary>
		/// Output the message to the log.
		/// </summary>
		public void Log(string message) {
			logger.Log(level, message);
		}
	}
}

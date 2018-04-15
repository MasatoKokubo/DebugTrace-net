// Log4net.cs
// (C) 2018 Masato Kokubo
using System;
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
				{"Trace", log4net.Core.Level.Trace},
				{"Debug", log4net.Core.Level.Debug},
				{"Info" , log4net.Core.Level.Info },
				{"Warn" , log4net.Core.Level.Warn },
				{"Error", log4net.Core.Level.Error},
				{"Fatal", log4net.Core.Level.Fatal},
			};

		// Log4net Logger
		private log4net.Core.ILogger logger = LogManager.GetLogger(typeof(ILogger).Namespace).Logger;

		private static string defaultLevelStr = "Debug";
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
				if (!levelDictinary.ContainsKey(value))
					new ArgumentException(value);

				level = levelDictinary[value];
				levelStr = value;
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

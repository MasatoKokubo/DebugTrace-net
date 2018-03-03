// Resource.cs
// (C) 2018 Masato Kokubo

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DebugTrace {
	/// <summary>
	/// Uses this class when gets resources.
	/// </summary>
	///
	/// <since>1.0.0</since>
	/// <author>Masato Kokubo</author>
	public class Resource {
		internal static Assembly selfAssembly = Assembly.GetExecutingAssembly();
		internal static DirectoryInfo selfDirInfo = new FileInfo(selfAssembly.Location).Directory;
		internal FileInfo fileInfo;
		internal IDictionary<string, string> values = new Dictionary<string, string>();

		public Resource(String baseName) {
			fileInfo = new FileInfo(Path.Combine(selfDirInfo.FullName, baseName + ".properties"));
			if (!fileInfo.Exists)
				return;

			var lineBuff = new StringBuilder();
			var eof = false;
			using (var reader = new StreamReader(fileInfo.FullName, new UTF8Encoding())) {
				while (!eof) {
					while (true) {
						var line = reader.ReadLine();
						if (line == null) {
							// end of file
							eof = true;
							break;
						}

						if (line.EndsWith(@"\")) {
							// join next line
							lineBuff.Append(line.Substring(0, line.Length - 1).Trim());
						} else {
							lineBuff.Append(line.Trim());
							break;
						}
					}

					var str = lineBuff.ToString();
					lineBuff.Clear();
					if (str == "" || str.StartsWith("#"))
						continue; // empty or comment line

					int equalIndex = str.IndexOf('=');
					if (equalIndex < 1) continue; // not "name = value" format

					var name = str.Substring(0, equalIndex).Trim();
					var value = str.Substring(equalIndex + 1).Trim();
					if (value == "")  continue; // name not exist

					var buff = new StringBuilder();
					var escape = false;
					foreach (var ch in value) {
						if (escape) {
							switch (ch) {
							case 't' : buff.Append('\t'); break; // 09 HT
							case 'n' : buff.Append('\n'); break; // 0A LF
							case 'r' : buff.Append('\r'); break; // 0D CR
							case 's' : buff.Append(' ' ); break; // SPACE
							case '\\': buff.Append('\\'); break; // \
							}
						} else if (ch == '\\') {
							escape = true;
						} else {
							buff.Append(ch);
						}
					}
					values[name] = buff.ToString();
				}
			}
		}

		public string GetString(string name, string defaultValue) {
			try {
				return values[name];
			}
			catch (Exception) {
				return defaultValue;
			}
		}

		public string[] GetStrings(string name, string[] defaultValue) {
			try {
				return values[name].Split(',').Select(str => str.Trim()).ToArray();
			}
			catch (Exception) {
				return defaultValue;
			}
		}

		public int GetInt(string name, int defaultValue) {
			try {
				return int.Parse(values[name]);
			}
			catch (Exception) {
				return defaultValue;
			}
		}
	}
}

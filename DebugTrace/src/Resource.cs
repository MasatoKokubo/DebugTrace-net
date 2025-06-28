// Resource.cs
// (C) 2018 Masato Kokubo
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DebugTrace;

/// <summary>
/// Uses this class when gets resources.
/// </summary>
/// <since>1.0.0</since>
/// <author>Masato Kokubo</author>
public class Resource {
    private static DirectoryInfo currentDirInfo = new (".");

    /// <summary>
    /// The <c>FileInfo</c> of the resource properties file
    /// </summary>
    public FileInfo FileInfo {get; private set;}

    private Dictionary<string, string> values = new ();

    /// <summary>
    /// Construct a <c>Resource</c>.
    /// </summary>
    /// <param name="baseName">the base name of the resource properties file</param>
    public Resource(String baseName) {
        FileInfo = new FileInfo(Path.Combine(currentDirInfo.FullName, baseName + ".properties"));
        if (!FileInfo.Exists)
            return;

        var lineBuff = new StringBuilder();
        var eof = false;
        using (var reader = new StreamReader(FileInfo.FullName, new UTF8Encoding())) {
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
                var value = Unescape(str.Substring(equalIndex + 1).Trim());

                if (value != "")
                    values[name] = value;
            }
        }
    }

    /// <summary>
    /// Converts the string containing escape sequences into a normal string and returns it.
    /// </summary>
    /// <param name="escapedString">the string containing escape sequences</param>
    /// <returns>a normal string</returns>
    public static string Unescape(string escapedString) {
        var buff = new StringBuilder();
        var escape = false;
        foreach (var ch in escapedString) {
            if (escape) {
                switch (ch) {
                case 't' : buff.Append('\t'); break; // 09 HT
                case 'n' : buff.Append('\n'); break; // 0A LF
                case 'r' : buff.Append('\r'); break; // 0D CR
                case 's' : buff.Append(' ' ); break; // SPACE
                default  : buff.Append(ch  ); break; // others
                }
                escape = false;
            } else if (ch == '\\') {
                escape = true;
            } else {
                buff.Append(ch);
            }
        }
        return buff.ToString();
    }

    /// <summary>
    /// Returns the resource value as a stirng.
    /// </summary>
    /// <param name="name">the resource name</param>
    /// <param name="defaultValue">resource value when the specified resource name is not defined</param>
    /// <returns>the resource value</returns>
    public string GetString(string name, string defaultValue) {
        return values.ContainsKey(name) ? values[name] : defaultValue;
    }

    /// <summary>
    /// Returns the resource value as a stirng.
    /// </summary>
    /// <param name="name1">the resource name 1</param>
    /// <param name="name2">the resource name 2</param>
    /// <param name="defaultValue">resource value when the specified resource name is not defined</param>
    /// <returns>the resource value</returns>
    /// <since>2.0.0</since>
    public string GetString(string name1, string name2, string defaultValue) {
        return values.ContainsKey(name1) ? values[name1] : values.ContainsKey(name2) ? values[name2] : defaultValue;
    }

    /// <summary>
    /// Returns the resource value as array of stirngs.
    /// </summary>
    /// <param name="name">the resource name</param>
    /// <param name="defaultValue">resource value when the specified resource name is not defined</param>
    /// <returns>the resource value</returns>
    public string[] GetStrings(string name, string[] defaultValue) {
        return values.ContainsKey(name)
            ? values[name].Split(',').Select(str => str.Trim()).ToArray()
            : defaultValue;
    }

    /// <summary>
    /// Returns the resource value as array of stirngs.
    /// </summary>
    /// <param name="name1">the resource name 1</param>
    /// <param name="name2">the resource name 2</param>
    /// <param name="defaultValue">resource value when the specified resource name is not defined</param>
    /// <returns>the resource value</returns>
    public string[] GetStrings(string name1, string name2, string[] defaultValue) {
        return values.ContainsKey(name1)
            ? values[name1].Split(',').Select(str => str.Trim()).ToArray()
            : values.ContainsKey(name2)
                ? values[name2].Split(',').Select(str => str.Trim()).ToArray()
                : defaultValue;
    }

    /// <summary>
    /// Returns the resource value as a int.
    /// </summary>
    /// <param name="name">the resource name</param>
    /// <param name="defaultValue">resource value when the specified resource name is not defined</param>
    /// <returns>the resource value</returns>
    public int GetInt(string name, int defaultValue) {
        try {
            return values.ContainsKey(name) ? int.Parse(values[name]) : defaultValue;
        }
        catch (Exception) {
            return defaultValue;
        }
    }

    /// <summary>
    /// Returns the resource value as a int.
    /// </summary>
    /// <param name="name1">the resource name 1</param>
    /// <param name="name2">the resource name 2</param>
    /// <param name="defaultValue">resource value when the specified resource name is not defined</param>
    /// <returns>the resource value</returns>
    /// <since>2.0.0</since>
    public int GetInt(string name1, string name2, int defaultValue) {
        try {
            return values.ContainsKey(name1)
                ? int.Parse(values[name1])
                : values.ContainsKey(name2) ? int.Parse(values[name2]) : defaultValue;
        }
        catch (Exception) {
            return defaultValue;
        }
    }

    /// <summary>
    /// Returns the resource value as a bool.
    /// </summary>
    /// <param name="name">the resource name</param>
    /// <param name="defaultValue">resource value when the specified resource name is not defined</param>
    /// <returns>the resource value</returns>
    /// <since>1.4.4</since>
    public bool GetBool(string name, bool defaultValue) {
        try {
            return values.ContainsKey(name) ? bool.Parse(values[name]) : defaultValue;
        }
        catch (Exception) {
            return defaultValue;
        }
    }
}

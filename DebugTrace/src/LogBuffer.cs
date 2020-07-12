// LogBuffer.cs
// (C) 2018 Masato Kokubo
using System.Collections.Generic;
using System.Text;

namespace DebugTrace {
    /// <summary>
    /// Buffers logs.
    /// </summary>
    /// <since>1.0.0</since>
    /// <author>Masato Kokubo</author>
    public class LogBuffer {
        private int nestLevel = 0;
        private int appendNestLevel = 0; // since 2.0.0
        private IList<(int, string)> lines = new List<(int, string)>();
        private StringBuilder lastLine = new StringBuilder();

        /// <summary>
        /// Breaks the current line.
        /// </summary>
        public void LineFeed() {
            lines.Add((nestLevel + appendNestLevel, lastLine.ToString().TrimEnd(' ')));
            appendNestLevel = 0;
            lastLine.Clear();
        }

        /// <summary>
        /// Ups the nest level.
        /// </summary>
        public void UpNest() => ++nestLevel;

        /// <summary>
        /// Downs the nest.
        /// </summary>
        public void DownNest() => --nestLevel;

        /// <summary>
        /// Appends a string representation of the value.
        /// </summary>
        /// <param name="value">the value to append</param>
        /// <param name="nestLevel">the nest level of the value</param>
        /// <param name="noBreak">if true, does not break even if the maximum width is exceeded</param>
        /// <returns>this object</returns>
        /// <since>2.0.0</since>
        public LogBuffer Append(object value, int nestLevel = 0, bool noBreak = false) {
            var str = value.ToString() ?? "";
            if (!noBreak && Length > 0 && Length + str.Length > TraceBase.MaximumDataOutputWidth)
                LineFeed();
            appendNestLevel = nestLevel;
            lastLine.Append(str);
            return this;
        }

        /// <summary>
        /// Appends a string representation of the value.
        /// Does not break even if the maximum width is exceeded.
        /// </summary>
        /// <param name="value">the value to append</param>
        /// <returns>this object</returns>
        /// <since>2.0.0</since>
        public LogBuffer NoBreakAppend(object value) => Append(value, 0, true);
 
        /// <summary>
        /// Appends lines of another <c>LogBuffer</c>.
        /// </summary>
        /// <param name="buff">another <c>LogBuffer</c></param>
        /// <returns>this object</returns>
        public LogBuffer Append(LogBuffer buff) {
            var index = 0;
            foreach ((var nestLevel, var str) in buff.Lines) {
                if (index > 0)
                    LineFeed();
                Append(str, nestLevel);
                ++index;
            }
            return this;
        }

        /// <summary>
        /// Log length of the last line.
        /// </summary>
        /// <since>2.0.0</since>
        public int Length {get {return lastLine.Length;} set {lastLine.Length = value;}}

        /// <summary>
        /// True if multiple lines.
        /// </summary>
        /// <since>2.0.0</since>
        public bool IsMultiLines => lines.Count > 1 || lines.Count == 1 && Length > 0;

        /// <summary>
        /// Tuples of data indentation level and log string
        /// </summary>
        /// <since>2.0.0</since>
        public IList<(int, string)> Lines {
            get {
                var lines = new List<(int, string)>(this.lines);
                if (lastLine.Length > 0)
                    lines.Add((nestLevel, lastLine.ToString()));
                return lines;
            }
        }

    }
}

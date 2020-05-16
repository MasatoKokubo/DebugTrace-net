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

        /// <summary>
        /// Tuples of data indentation level and log string
        /// </summary>
        public IList<(int, string)> Lines {get;} = new List<(int, string)>();

        /// <summary>
        /// Buffer for a line of logs
        /// </summary>
        public StringBuilder builder = new StringBuilder();

        /// <summary>
        /// Breaks the current line.
        /// </summary>
        public void LineFeed() {
            Lines.Add((nestLevel + appendNestLevel, builder.ToString().TrimEnd(' ')));
            appendNestLevel = 0;
            builder.Clear();
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
            builder.Append(str);
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
            buff.LineFeed();
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
        /// Log length of the current line.
        /// </summary>
        /// <since>2.0.0</since>
        public int Length {get {return builder.Length;} set {builder.Length = value;}}

        /// <summary>
        /// True if multiple lines.
        /// </summary>
        /// <since>2.0.0</since>
        public bool IsMultiLines => Lines.Count > 1 || Lines.Count == 1 && Length > 0;
    }
}

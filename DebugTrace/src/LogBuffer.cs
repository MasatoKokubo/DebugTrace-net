// State.cs
// (C) 2018 Masato Kokubo

using System;
using System.Collections.Generic;
using System.Text;

namespace DebugTrace {
    /// <summary>
    /// Buffers logs.
    /// </summary>
    ///
    /// <since>1.0.0</since>
    /// <author>Masato Kokubo</author>
    public class LogBuffer {
        private int nestLevel = 0;

        /// <summary>
        /// Log lines
        /// </summary>
        public IList<(int, string)> Lines {get;} = new List<(int, string)>();

        /// <summary>
        /// Buffering one line of logs
        /// </summary>
        public StringBuilder builder = new StringBuilder();

        /// <summary>
        /// Stack of save points
        /// </summary>
        public Stack<(int, int)> savePoints;

        /// <summary>
        /// Breaks the line
        /// </summary>
        public void LineFeed() {
            Lines.Add((nestLevel, builder.ToString()));
            builder.Clear();
        }

        /// <summary>
        /// Ups the nest level.
        /// </summary>
        public void UpNest() {
            ++nestLevel;
        }

        /// <summary>
        /// Downs the nest.
        /// </summary>
        public void DownNest() {
            --nestLevel;
        }

        /// <summary>
        /// Appends a string representation of the value.
        /// </summary>
        /// <param name="value">the value</param>
        /// <returns>this object</returns>
        public LogBuffer Append(object value) {
            if (value != null)
                builder.Append(value);
            return this;
        }

        /// <summary>
        /// Inserts a string representation of the value at the specified position.
        /// </summary>
        /// <param name="index">the index of insertion position</param>
        /// <param name="value">the value</param>
        /// <returns>this object</returns>
        public LogBuffer Insert(int index, object value) {
            builder.Insert(index, value);
            return this;
        }

        /// <summary>
        /// Log length of the current line
        /// </summary>
        public int Length {get {return builder.Length;} set {builder.Length = value;}}

        /// <summary>
        /// Saves the current logging point.
        /// </summary>
        public void Save() {
            if (savePoints == null)
                savePoints = new Stack<(int, int)>();
            savePoints.Push((Lines.Count, builder.Length));
        }

        /// <summary>
        /// Peeks the last saved logging point.
        /// </summary>
        /// <returns>the last saved logging point</returns>
        public (int linesCount, int builderLength) PeekSave() {
            if (savePoints == null || savePoints.Count == 0)
                throw new InvalidOperationException("savePoint == null || savePoint.Count == 0");

            return savePoints.Peek();
        }

        /// <summary>
        /// Pops the last saved logging point.
        /// </summary>
        /// <returns>the last saved logging point</returns>
        public (int linesCount, int builderLength) PopSave() {
            if (savePoints == null || savePoints.Count == 0)
                throw new InvalidOperationException("savePoint == null || savePoint.Count == 0");

            return savePoints.Pop();
        }

        /// <summary>
        /// Restores the last saved logging point.
        /// </summary>
        public void Restore() {
            (int linesCount, int builderLength) = PeekSave();
            if (linesCount > Lines.Count)
                throw new InvalidOperationException($"saved lines count: {linesCount} > Lines.Count: {Lines.Count}");

            if (linesCount < Lines.Count) {
                (int lineNestLevel, string line) = Lines[linesCount];
                if (builderLength > line.Length)
                    throw new InvalidOperationException($"saved builder length: {builderLength} < line.Length: {line.Length}");

                nestLevel = lineNestLevel;
                builder.Clear();
                builder.Append(line.Substring(0, builderLength));

                while (Lines.Count > linesCount)
                    Lines.RemoveAt(Lines.Count - 1);
            } else {
                if (builderLength > builder.Length)
                    throw new InvalidOperationException($"saved builder length: {builderLength} > current builder length: {builder.Length}");

                builder.Length = builderLength;
            }
        }

    }
}

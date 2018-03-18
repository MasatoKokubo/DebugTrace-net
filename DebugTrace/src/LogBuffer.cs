// State.cs
// (C) 2018 Masato Kokubo

using System;
using System.Collections.Generic;
using System.Text;

namespace DebugTrace {
	/// <summary>
	/// 
	/// </summary>
	///
	/// <since>1.0.0</since>
	/// <author>Masato Kokubo</author>
	public class LogBuffer {
		private int nestLevel = 0;
		public IList<(int, string)> Lines {get;} = new List<(int, string)>();
		public StringBuilder builder = new StringBuilder();
		public Stack<(int, int)> savePoints;

		public void LineFeed() {
			Lines.Add((nestLevel, builder.ToString()));
			builder.Clear();
		}

		public void UpNest() {
			++nestLevel;
		}

		public void DownNest() {
			--nestLevel;
		}

		public LogBuffer Append(object value) {
			if (value != null)
				builder.Append(value);
			return this;
		}

		public LogBuffer Insert(int index, object value) {
			builder.Insert(index, value);
			return this;
		}

		public int Length {get {return builder.Length;} set {builder.Length = value;}}

		public void Save() {
			if (savePoints == null)
				savePoints = new Stack<(int, int)>();
			savePoints.Push((Lines.Count, builder.Length));
		}

		public (int linesCount, int builderLength) PeekSave() {
			if (savePoints == null || savePoints.Count == 0)
				throw new InvalidOperationException("savePoint == null || savePoint.Count == 0");

			return savePoints.Peek();
		}

		public (int linesCount, int builderLength) PopSave() {
			if (savePoints == null || savePoints.Count == 0)
				throw new InvalidOperationException("savePoint == null || savePoint.Count == 0");

			return savePoints.Pop();
		}

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

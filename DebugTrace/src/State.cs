// State.cs
// (C) 2018 Masato Kokubo

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DebugTrace {
	/// <summary>
	/// Output suitable for c #.
	/// </summary>
	///
	/// <since>1.0.0</since>
	/// <author>Masato Kokubo</author>
	public class State {
		public int NestLevel       {get; set;} // The nest level
		public int BeforeNestLevel {get; set;} // The before nest level
		public int DataNestLevel   {get; set;} // The data nest level

		public void Reset() {
			NestLevel       = 0;
			BeforeNestLevel = 0;
			DataNestLevel   = 0;
		}

		public override string ToString() {
			return "(State)["
				+ "NestLevel: " + NestLevel
				+ ", BeforeNestLevel: " + BeforeNestLevel
				+ ", DataNestLevel: " + DataNestLevel
				+ "]";
		}
	}
}
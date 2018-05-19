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
    /// Output suitable for C#.
    /// </summary>
    ///
    /// <since>1.0.0</since>
    /// <author>Masato Kokubo</author>
    public class State {
        public int ThreadId          {get; set;} // The thread id
        public int NestLevel         {get; set;} // The nest level
        public int PreviousNestLevel {get; private set;} // The previous nest level
        public int PreviousLineCount {get; set;} // The previous line count

        public void Reset() {
            NestLevel       = 0;
            PreviousNestLevel = 0;
            PreviousLineCount = 0;
        }

        public override string ToString() {
            return "(State)["
                + "ThreadId: " + ThreadId
                + ", NestLevel: " + NestLevel
                + ", PreviousNestLevel: " + PreviousNestLevel
                + ", PreviousLineCount: " + PreviousLineCount
                + "]";
        }

        /// <summary>
        /// Up the nest level.
        /// </summary>
        public void UpNest() {
            PreviousNestLevel = NestLevel;
            ++NestLevel;
        }

        /// <summary>
        /// Down the nest level.
        /// </summary>
        public void DownNest() {
            PreviousNestLevel = NestLevel;
            --NestLevel;
        }
    }
}

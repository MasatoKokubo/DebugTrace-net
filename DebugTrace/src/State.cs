// State.cs
// (C) 2018 Masato Kokubo

using System;
using System.Collections.Generic;

namespace DebugTrace {
    /// <summary>
    /// Have a trace state for a thread.
    /// </summary>
    ///
    /// <since>1.0.0</since>
    /// <author>Masato Kokubo</author>
    public class State {
        public int ThreadId          {get; set;} // The thread id
        public int NestLevel         {get; set;} // The nest level
        public int PreviousNestLevel {get; private set;} // The previous nest level
        public int PreviousLineCount {get; set;} // The previous line count

        private Stack<DateTime> DateTimes = new Stack<DateTime>(); // Datetime Stack - since 1.4.3

        public void Reset() {
            NestLevel       = 0;
            PreviousNestLevel = 0;
            PreviousLineCount = 0;
            DateTimes.Clear();
        }

        public override string ToString() {
            return "(State)["
                + "ThreadId: " + ThreadId
                + ", NestLevel: " + NestLevel
                + ", PreviousNestLevel: " + PreviousNestLevel
                + ", PreviousLineCount: " + PreviousLineCount
                + ", DateTimes: " + DateTimes
                + "]";
        }

        /// <summary>
        /// Up the nest level.
        /// </summary>
        public void UpNest() {
            PreviousNestLevel = NestLevel;
            if (NestLevel >= 0)
                DateTimes.Push(DateTime.UtcNow);
            ++NestLevel;
        }

        /// <summary>
        /// Down the nest level.
        /// </summary>
        /// <returns>The DateTime when the corresponding UpNest method was invoked</returns>
        public DateTime DownNest() {
            PreviousNestLevel = NestLevel;
            --NestLevel;
            return DateTimes.Count > 0 ? DateTimes.Pop() : DateTime.UtcNow;
        }
    }
}

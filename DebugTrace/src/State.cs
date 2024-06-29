// State.cs
// (C) 2018 Masato Kokubo
using System;
using System.Collections.Generic;

namespace DebugTrace;

/// <summary>
/// Have a trace state for a thread.
/// </summary>
/// <since>1.0.0</since>
/// <author>Masato Kokubo</author>
internal class State {
    /// <summary>
    /// The thread id
    /// </summary>
    public int ThreadId {get; set;}

    /// <summary>
    /// The nest level
    /// </summary>
    public int NestLevel {get; set;}

    /// <summary>
    /// The previous nest level
    /// </summary>
    public int PreviousNestLevel {get; private set;}

    /// <summary>
    /// The previous line count
    /// </summary>
    public int PreviousLineCount {get; set;}

    // Datetime Stack - since 1.4.3
    private Stack<DateTime> DateTimes = new Stack<DateTime>();

    /// <summary>
    /// Resets
    /// </summary>
    public void Reset() {
        NestLevel = 0;
        PreviousNestLevel = 0;
        PreviousLineCount = 0;
        DateTimes.Clear();
    }

    /// <summary>
    /// Returns a string representation of this object.
    /// </summary>
    /// <returns>a string representation of this object</returns>
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
    /// Ups the nest level.
    /// </summary>
    public void UpNest() {
        PreviousNestLevel = NestLevel;
        if (NestLevel >= 0)
            DateTimes.Push(DateTime.UtcNow);
        ++NestLevel;
    }

    /// <summary>
    /// Downs the nest level.
    /// </summary>
    /// <returns>The DateTime when the corresponding UpNest method was invoked</returns>
    public DateTime DownNest() {
        PreviousNestLevel = NestLevel;
        --NestLevel;
        return DateTimes.Count > 0 ? DateTimes.Pop() : DateTime.UtcNow;
    }
}

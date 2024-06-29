// Examples.cs
// (C) 2018 Masato Kokubo
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DebugTrace; // TODO: Debug

namespace Examples;

/// <summary>Examples1</summary>
[TestClass]
public class Examples1 {
    /// <summary>Example1_1</summary>
    [TestMethod]
    public void Example1_1() {
        Trace.Enter(); // TODO: Debug

        var a = 3;
        Trace.Print("a", a); // TODO: Debug

        var b = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        Trace.Print("b", b); // TODO: Debug

        b.AddRange(b);
        Trace.Print("b", b); // TODO: Debug

        b.AddRange(b);
        Trace.Print("b", b); // TODO: Debug

        b.AddRange(b);
        Trace.Print("b", b); // TODO: Debug

        b.AddRange(b);
        Trace.Print("b", b); // TODO: Debug

        b.AddRange(b);
        Trace.Print("b", b); // TODO: Debug

        Trace.Leave(); // TODO: Debug
    }

}

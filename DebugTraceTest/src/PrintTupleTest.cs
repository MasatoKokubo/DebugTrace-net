// PrintTupleTest.cs
// (C) 2018 Masato Kokubo
using System;
using DebugTrace;
using DebugTrace;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DebugTraceTest;

[TestClass]
public class PrintTupleTest {
    private static int maxDataOutputWidth;

    // ClassInitialize
    [ClassInitialize]
    public static void ClassInit(TestContext testContext) {
        maxDataOutputWidth = Trace.MaximumDataOutputWidth;
        Trace.MaximumDataOutputWidth = 100;
    }

    // ClassCleanup
    [ClassCleanup]
    public static void ClassCleanup() {
        Trace.MaximumDataOutputWidth = maxDataOutputWidth;
    }

    // (int, int)
    [DataTestMethod]
    [DataRow(-1, 1, "v = (-1, 1) ")]
    [DataRow(-2147483648, 2147483647, "v = (-2147483648, 2147483647) ")]
    public void PrintIntInt(int v1, int v2, string expect) {
        var v = (v1, v2);
        Assert.AreEqual(v, Trace.Print("v", v));
        StringAssert.Contains(Trace.LastLog, expect);
    }

    // Tuple<int, int>
    [DataTestMethod]
    [DataRow(-1, 1, "v = Tuple<int, int> (-1, 1) ")]
    [DataRow(-2147483648, 2147483647, "v = Tuple<int, int> (-2147483648, 2147483647) ")]
    public void PrintTupleIntInt(int v1, int v2, string expect) {
        var v  = new Tuple<int, int>(v1, v2);
        Assert.AreEqual(v, Trace.Print("v", v));
        StringAssert.Contains(Trace.LastLog, expect);
    }

    // (int, int, int)
    [DataTestMethod]
    [DataRow(-1, 0, 1, "v = (-1, 0, 1) ")]
    [DataRow(-2147483648, 0, 2147483647, "v = (-2147483648, 0, 2147483647) ")]
    public void PrintIntIntInt(int v1, int v2, int v3, string expect) {
        var v = (v1, v2, v3);
        Assert.AreEqual(v, Trace.Print("v", v));
        StringAssert.Contains(Trace.LastLog, expect);
    }

    // Tuple<int, int, int>
    [DataTestMethod]
    [DataRow(-1, 0, 1, "v = Tuple<int, int, int> (-1, 0, 1) ")]
    [DataRow(-2147483648, 0, 2147483647, "v = Tuple<int, int, int> (-2147483648, 0, 2147483647) ")]
    public void PrintTupleIntIntInt(int v1, int v2, int v3, string expect) {
        var v = new Tuple<int, int, int>(v1, v2, v3);
        Assert.AreEqual(v, Trace.Print("v", v));
        StringAssert.Contains(Trace.LastLog, expect);
    }

    // ((int, int), (int, int))
    [DataTestMethod]
    [DataRow(-1, 0, 1, 2, "v = ((-1, 0), (1, 2)) ")]
    public void PrintIntIntIntInt(int v1, int v2, int v3, int v4, string expect) {
        var v = ((v1, v2), (v3, v4));
        Assert.AreEqual(v, Trace.Print("v", v));
        StringAssert.Contains(Trace.LastLog, expect);
    }

    // Tuple<Tuple<int, int>, Tuple<int, int>>
    [DataTestMethod]
    [DataRow(-1, 0, 1, 2, "v = Tuple<Tuple<int, int>, Tuple<int, int>> (Tuple<int, int> (-1, 0), Tuple<int, int> (1, 2)) ")]
    public void PrintTupleIntIntIntInt(int v1, int v2, int v3, int v4, string expect) {
        var v = new Tuple<Tuple<int, int>, Tuple<int, int>>(new Tuple<int, int>(v1, v2), new Tuple<int, int>(v3, v4));
        Assert.AreEqual(v, Trace.Print("v", v));
        StringAssert.Contains(Trace.LastLog, expect);
    }
}

// PrintCollectionTest.cs
// (C) 2018 Masato Kokubo
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DebugTrace.CSharp;

namespace DebugTraceTest {
    [TestClass]
    public class PrintCollectionTest {
        private static int maxDataOutputWidth;

        [ClassInitialize]
        public static void ClassInit(TestContext context) {
            maxDataOutputWidth = DebugTrace.TraceBase.MaximumDataOutputWidth;
            DebugTrace.TraceBase.MaximumDataOutputWidth = int.MaxValue;
        }

        [ClassCleanup]
        public static void ClassCleanup() {
            DebugTrace.TraceBase.MaximumDataOutputWidth = maxDataOutputWidth;
        }

        // List<int> count < 5
        [DataTestMethod]
        [DataRow(new [] {0, 1, 2, 3}, "v = System.Collections.Generic.List<int> {0, 1, 2, 3} (")]
        public void PrintIntList4(int[] v, string expect) {
            Trace.Print("v", v.ToList());
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // List<int> count >= 5
        [DataTestMethod]
        [DataRow(new [] {0, 1, 2, 3, 4}, "v = System.Collections.Generic.List<int> Count:5 {0, 1, 2, 3, 4} (")]
        public void PrintIntList5(int[] v, string expect) {
            Trace.Print("v", v.ToList());
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // List<Point> count < 5
        [DataTestMethod]
        [DataRow(new [] {0, 1, 2}, new [] {10, 11, 12},
            "v = System.Collections.Generic.List<DebugTraceTest.Point> {" +
            "DebugTraceTest.Point struct {X: 0, Y: 10}, " +
            "DebugTraceTest.Point struct {X: 1, Y: 11}, " +
            "DebugTraceTest.Point struct {X: 2, Y: 12}" +
            "} (")]
        public void PrintPointList(int[] x, int[] y, string expect) {
            Trace.Print("v", x.Select((e, index) => new Point(e, y[index])).ToList());
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // Dictionary<int, string> count < 5
        [DataTestMethod]
        [DataRow(new [] {0, 1, 2, 3}, new [] {"0", "1", "2", "3"},
            "d = System.Collections.Generic.Dictionary<int, string> {0: \"0\", 1: \"1\", 2: \"2\", 3: \"3\"} (")]
        public void PrintDictionary4(int[] k, string[] v, string expect) {
            var d = new Dictionary<int, string>();
            for (int index = 0; index < k.Length; ++index)
                d[k[index]] = v[index];
            Trace.Print("d", d);
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // Dictionary<int, string> count >= 5
        [DataTestMethod]
        [DataRow(new [] {0, 1, 2, 3, 4}, new [] {"0", "1", "2", "3", "4"},
            "d = System.Collections.Generic.Dictionary<int, string> Count:5 {0: \"0\", 1: \"1\", 2: \"2\", 3: \"3\", 4: \"4\"} (")]
        public void PrintDictionary5(int[] k, string[] v, string expect) {
            var d = new Dictionary<int, string>();
            for (int index = 0; index < k.Length; ++index)
                d[k[index]] = v[index];
            Trace.Print("d", d);
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // HashSet<int> count < 5
        [DataTestMethod]
        [DataRow(new [] {0, 1, 2, 3}, "s = System.Collections.Generic.HashSet<int> {0, 1, 2, 3} (")]
        public void PrintHashSet4(int[] e, string expect) {
            var s = new HashSet<int>(e);
            Trace.Print("s", s);
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // HashSet<int> count >= 5
        [DataTestMethod]
        [DataRow(new [] {0, 1, 2, 3, 4}, "s = System.Collections.Generic.HashSet<int> Count:5 {0, 1, 2, 3, 4} (")]
        public void PrintHashSet5(int[] e, string expect) {
            var s = new HashSet<int>(e);
            Trace.Print("s", s);
            StringAssert.Contains(Trace.LastLog, expect);
        }
    }
}

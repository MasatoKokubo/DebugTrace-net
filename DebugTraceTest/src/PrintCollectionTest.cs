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
            maxDataOutputWidth = DebugTrace.TraceBase.MaxDataOutputWidth;
            DebugTrace.TraceBase.MaxDataOutputWidth = int.MaxValue;
        }

        [ClassCleanup]
        public static void ClassCleanup() {
            DebugTrace.TraceBase.MaxDataOutputWidth = maxDataOutputWidth;
        }

        // List<int>
        [DataTestMethod]
        [DataRow(new [] {0, 1, 2}, "v = System.Collections.Generic.List<int> Count:3 {0, 1, 2} (")]
        public void PrintIntList(int[] v, string expect) {
            Trace.Print("v", v.ToList());
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // List<Point>
        [DataTestMethod]
        [DataRow(new [] {0, 1, 2}, new [] {10, 11, 12},
            "v = System.Collections.Generic.List<DebugTraceTest.Point> Count:3 {" +
            "DebugTraceTest.Point struct {X: 0, Y: 10}, " +
            "DebugTraceTest.Point struct {X: 1, Y: 11}, " +
            "DebugTraceTest.Point struct {X: 2, Y: 12}" +
            "} (")]
        public void PrintPointList(int[] x, int[] y, string expect) {
            Trace.Print("v", x.Select((e, index) => new Point(e, y[index])).ToList());
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // Dictionary<int, string>
        [DataTestMethod]
        [DataRow(new [] {0, 1}, new [] {"0", "1"},
            "d = System.Collections.Generic.Dictionary<int, string> Count:2 {0: (Length:1)\"0\", 1: (Length:1)\"1\"} (")]
        public void PrintDictionary(int[] k, string[] v, string expect) {
            var d = new Dictionary<int, string>();
            for (int index = 0; index < k.Length; ++index)
                d[k[index]] = v[index];
            Trace.Print("d", d);
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // HashSet<int>
        [DataTestMethod]
        [DataRow(new [] {0, 1, 2}, "s = System.Collections.Generic.HashSet<int> Count:3 {0, 1, 2} (")]
        public void PrintHashSet(int[] e, string expect) {
            var s = new HashSet<int>(e);
            Trace.Print("s", s);
            StringAssert.Contains(Trace.LastLog, expect);
        }
    }
}

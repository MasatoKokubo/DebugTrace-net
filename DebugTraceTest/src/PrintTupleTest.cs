using System;
using DebugTrace;
using static DebugTrace.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DebugTraceTest {
    [TestClass]
    public class PrintTupleTest {
        private static int maxDataOutputWidth;

        // ClassInitialize
        [ClassInitialize]
        public static void ClassInit(TestContext testContext) {
            maxDataOutputWidth = TraceBase.MaxDataOutputWidth;
            TraceBase.MaxDataOutputWidth = 100;
        }

        // ClassCleanup
        [ClassCleanup]
        public static void ClassCleanup() {
            TraceBase.MaxDataOutputWidth = maxDataOutputWidth;
        }

        // (int, int)
        [DataTestMethod]
        [DataRow(-1, 1, "v = (-1, 1) ")]
        [DataRow(-2147483648, 2147483647, "v = (-2147483648, 2147483647) ")]
        public void CSPrintIntInt(int v1, int v2, string expect) {
            Trace.Print("v", (v1, v2));
            Assert.IsTrue(Trace.LastLog.IndexOf(expect) >= 0);
        }

        // Tuple<int, int>
        [DataTestMethod]
        [DataRow(-1, 1, "v = Tuple<int, int> (-1, 1) ")]
        [DataRow(-2147483648, 2147483647, "v = Tuple<int, int> (-2147483648, 2147483647) ")]
        public void CSPrintTupleIntInt(int v1, int v2, string expect) {
            Trace.Print("v", new Tuple<int, int>(v1, v2));
            Assert.IsTrue(Trace.LastLog.IndexOf(expect) >= 0);
        }

        // (int, int, int)
        [DataTestMethod]
        [DataRow(-1, 0, 1, "v = (-1, 0, 1) ")]
        [DataRow(-2147483648, 0, 2147483647, "v = (-2147483648, 0, 2147483647) ")]
        public void CSPrintIntIntInt(int v1, int v2, int v3, string expect) {
            Trace.Print("v", (v1, v2, v3));
            Assert.IsTrue(Trace.LastLog.IndexOf(expect) >= 0);
        }

        // Tuple<int, int, int>
        [DataTestMethod]
        [DataRow(-1, 0, 1, "v = Tuple<int, int, int> (-1, 0, 1) ")]
        [DataRow(-2147483648, 0, 2147483647, "v = Tuple<int, int, int> (-2147483648, 0, 2147483647) ")]
        public void CSPrintTupleIntIntInt(int v1, int v2, int v3, string expect) {
            Trace.Print("v", new Tuple<int, int, int>(v1, v2, v3));
            Assert.IsTrue(Trace.LastLog.IndexOf(expect) >= 0);
        }

        // ((int, int), (int, int))
        [DataTestMethod]
        [DataRow(-1, 0, 1, 2, "v = ((-1, 0), (1, 2)) ")]
        public void CSPrintIntIntIntInt(int v1, int v2, int v3, int v4, string expect) {
            Trace.Print("v", ((v1, v2), (v3, v4)));
            Assert.IsTrue(Trace.LastLog.IndexOf(expect) >= 0);
        }

        // Tuple<Tuple<int, int>, Tuple<int, int>>
        [DataTestMethod]
        [DataRow(-1, 0, 1, 2, "v = Tuple<Tuple<int, int>, Tuple<int, int>> (Tuple<int, int> (-1, 0), Tuple<int, int> (1, 2)) ")]
        public void CSPrintTupleIntIntIntInt(int v1, int v2, int v3, int v4, string expect) {
            Trace.Print("v", new Tuple<Tuple<int, int>, Tuple<int, int>>(new Tuple<int, int>(v1, v2), new Tuple<int, int>(v3, v4)));
            Assert.IsTrue(Trace.LastLog.IndexOf(expect) >= 0);
        }

    }
}

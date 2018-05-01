using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DebugTrace.CSharp;

namespace DebugTraceTest {
    [TestClass]
    public class PrintClassTest {
        // Print a class that does not have ToString method
        [TestMethod]
        public void DoseNotHaveToString() {
            var p = new Point(1, 2);
            Trace.Print("p", p);
            Assert.IsTrue(Trace.LastLog.IndexOf(p.GetType().FullName) >= 0);
            Assert.IsTrue(Trace.LastLog.IndexOf("{" + $"X: {p.X}, Y: {p.Y}" + "}") >= 0);
        }

        // Print a class that has ToString method
        [TestMethod]
        public void HasToString() {
            var p3 = new Point3(1, 2, 3);
            Trace.Print("p3", p3);
            Assert.IsTrue(Trace.LastLog.IndexOf(p3.GetType().FullName) >= 0);
            Assert.IsTrue(Trace.LastLog.IndexOf(p3.ToString()) >= 0);
        }

        // Print a class that has type parameter
        [TestMethod]
        public void HasTypeParameter() {
            var node = new Node<int>(1);
            Trace.Print("node", node);
            Assert.IsTrue(Trace.LastLog.IndexOf("DebugTraceTest.Node<int>") >= 0);
        }
    
    }
}

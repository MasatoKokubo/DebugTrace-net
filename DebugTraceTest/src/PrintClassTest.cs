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
            StringAssert.Contains(Trace.LastLog, p.GetType().FullName);
            StringAssert.Contains(Trace.LastLog, "{" + $"X: {p.X}, Y: {p.Y}" + "}");
        }

        // Print a class that has ToString method
        [TestMethod]
        public void HasToString() {
            var p3 = new Point3(1, 2, 3);
            Trace.Print("p3", p3);
            StringAssert.Contains(Trace.LastLog, p3.GetType().FullName);
            StringAssert.Contains(Trace.LastLog, p3.ToString());
        }

        // Print a class that has type parameter
        [TestMethod]
        public void HasTypeParameter() {
            var node = new Node<int>(1);
            Trace.Print("node", node);
            StringAssert.Contains(Trace.LastLog, "DebugTraceTest.Node<int>");
        }
    
    }
}

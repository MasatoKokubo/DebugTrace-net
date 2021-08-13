// EnterLeaveNestTest.cs
// (C) 2018 Masato Kokubo
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DebugTrace.CSharp;

namespace DebugTraceTest {
    [TestClass]
    public class EnterLeaveNestTest {
        [TestMethod]
        public void EnterLeaveNestTest1() {
            Trace.Enter(); // ToDo: Remove after debugging
            Method1();
            Trace.Leave(); // ToDo: Remove after debugging
            Trace.Leave(); // ToDo: Remove after debugging
            Trace.Enter(); // ToDo: Remove after debugging
            Thread.Sleep(1111);
            Trace.Leave(); // ToDo: Remove after debugging
            Trace.ResetNest();
        }

        private void Method1() {
            Trace.Enter(); // ToDo: Remove after debugging
            Thread.Sleep(1111);
            Method2();
            Trace.Leave(); // ToDo: Remove after debugging
        }

        private void Method2() {
            Trace.Enter(); // ToDo: Remove after debugging
            Thread.Sleep(1111);
            Method3();
            Trace.Leave(); // ToDo: Remove after debugging
        }

        private void Method3() {
            Trace.Enter(); // ToDo: Remove after debugging
            Thread.Sleep(1111);
            Method4();
            Trace.Leave(); // ToDo: Remove after debugging
        }

        private void Method4() {
            Trace.Enter(); // ToDo: Remove after debugging
            Thread.Sleep(1111);
            Trace.Leave(); // ToDo: Remove after debugging
        }
    }
}

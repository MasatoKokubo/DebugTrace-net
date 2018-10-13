using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DebugTrace.CSharp;

namespace DebugTraceTest {
    [TestClass]
    public class EnterLeaveNestTest {
        [TestMethod]
        public void EnterLeaveNestTest1() {
            Trace.Enter(); // for Debugging
            Method1();
            Trace.Leave(); // for Debugging
            Trace.Leave(); // for Debugging
            Trace.Enter(); // for Debugging
            Thread.Sleep(1111);
            Trace.Leave(); // for Debugging
            Trace.ResetNest();
        }

        private void Method1() {
            Trace.Enter(); // for Debugging
            Thread.Sleep(1111);
            Method2();
            Trace.Leave(); // for Debugging
        }

        private void Method2() {
            Trace.Enter(); // for Debugging
            Thread.Sleep(1111);
            Method3();
            Trace.Leave(); // for Debugging
        }

        private void Method3() {
            Trace.Enter(); // for Debugging
            Thread.Sleep(1111);
            Method4();
            Trace.Leave(); // for Debugging
        }

        private void Method4() {
            Trace.Enter(); // for Debugging
            Thread.Sleep(1111);
            Trace.Leave(); // for Debugging
        }
    }
}

// EnterLeaveNestTest.cs
// (C) 2018 Masato Kokubo
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DebugTrace;

namespace DebugTraceTest;

[TestClass]
public class EnterLeaveNestTest {
    [TestMethod]
    public void EnterLeaveNestTest1() {
        Trace.Enter();
        Method1();
        Trace.Leave();
        Trace.Leave();
        Trace.Enter();
        Thread.Sleep(1111);
        Trace.Leave();
        Trace.ResetNest();
    }

    private void Method1() {
        Trace.Enter();
        Thread.Sleep(1111);
        Method2();
        Trace.Leave();
    }

    private void Method2() {
        Trace.Enter();
        Thread.Sleep(1111);
        Method3();
        Trace.Leave();
    }

    private void Method3() {
        Trace.Enter();
        Thread.Sleep(1111);
        Method4();
        Trace.Leave();
    }

    private void Method4() {
        Trace.Enter();
        Thread.Sleep(1111);
        Trace.Leave();
    }
}

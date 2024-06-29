// EnterLeaveAsyncAwaitTest.cs
// (C) 2018 Masato Kokubo
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DebugTrace;

namespace DebugTraceTest;

[TestClass]
public class EnterLeaveAsyncAwaitTest {
    // TaskExample
    [TestMethod]
    public void Test() {
        Trace.Enter();
        TaskExample().Wait();
        Trace.Leave();
        Trace.Print("");
        Assert.IsFalse(Trace.LastLog.Contains(DebugTrace.Trace.IndentString), "The indent level is 0");
    }

    public async Task<int> TaskExample() {
        var threasdId = Trace.Enter();
        var task = await Task.Run<int>(() => {
            Trace.Enter();
            Thread.Sleep(100);
            Trace.Leave();
            return 0;
        });
        Trace.Leave(threasdId);
        return task;
    }
}

// EnterLeaveAsyncAwaitTest.cs
// (C) 2018 Masato Kokubo
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DebugTrace.CSharp;

namespace DebugTraceTest;

[TestClass]
public class EnterLeaveAsyncAwaitTest {
    // TaskExample
    [TestMethod]
    public void Test() {
        Trace.Enter(); // ToDo: Remove after debugging
        TaskExample().Wait();
        Trace.Leave(); // ToDo: Remove after debugging
        Trace.Print(""); // ToDo: Remove after debugging
        Assert.IsFalse(Trace.LastLog.Contains(DebugTrace.TraceBase.IndentString), "The indent level is 0");
    }

    public async Task<int> TaskExample() {
        var threasdId = Trace.Enter(); // ToDo: Remove after debugging
        var task = await Task.Run<int>(() => {
            Trace.Enter(); // ToDo: Remove after debugging
            Thread.Sleep(100);
            Trace.Leave(); // ToDo: Remove after debugging
            return 0;
        });
        Trace.Leave(threasdId); // ToDo: Remove after debugging
        return task;
    }
}

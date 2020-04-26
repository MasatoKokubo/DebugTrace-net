using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DebugTrace.CSharp;

namespace DebugTraceTest {
    [TestClass]
    public class EnterLeaveAsyncAwaitTest {
        // TaskExample
        [TestMethod]
        public void Test() {
            Trace.Enter(); // for Debugging
            TaskExample().Wait();
            Trace.Leave(); // for Debugging
            Trace.Print(""); // for Debugging
            Assert.IsFalse(Trace.LastLog.Contains(DebugTrace.TraceBase.IndentString), "The indent level is 0");
        }

        public async Task<int> TaskExample() {
            var threasdId = Trace.Enter(); // for Debugging
            var task = await Task.Run<int>(() => {
                Trace.Enter(); // for Debugging
                Thread.Sleep(100);
                Trace.Leave(); // for Debugging
                return 0;
            });
            Trace.Leave(threasdId); // for Debugging
            return task;
        }
    }
}

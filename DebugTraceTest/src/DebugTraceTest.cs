using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DebugTraceTest {
	using DebugTrace;

	[TestClass]
	public class DebugTraceTest {
		[TestMethod]
		public void TestPrint() {
		/**/DebugTrace.Enter();
			var intValue = 10;
		/**/DebugTrace.Print("intValue", intValue);
		/**/DebugTrace.Leave();
		}
	}
}

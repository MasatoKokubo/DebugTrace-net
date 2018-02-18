using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//using DebugTrace;

namespace DebugTraceTest {
	[TestClass]
	public class DebugTraceTest {
		[TestMethod]
		public void TestPrint() {
			int intValue = 10;
			//	DebugTrace.print("intValue", intValue);
			Console.Write("{0} = {1}", "intValue", intValue);
		}
	}
}

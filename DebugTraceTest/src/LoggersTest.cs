using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DebugTrace;

namespace DebugTraceTest {
    [TestClass]
    public class LoggersTest {
        // TestCleanup
        [TestCleanup]
        public void TestCleanup() {
        }

        // 0 Loggers
        [TestMethod]
        public void _0_Loggers() {
            var loggers = new Loggers();
            Assert.AreEqual(0, loggers.Members.Count());

            loggers.Level = "Warn";
            Assert.AreEqual("", loggers.Level);

            Assert.IsFalse(loggers.IsEnabled);
        }

        // 1 Logger
        [TestMethod]
        public void _1_Logger() {
            var loggers = new Loggers(Console.Out.Instance);
            Assert.AreEqual(1, loggers.Members.Count());
            Assert.AreEqual(typeof(Console.Out), loggers.Members.ToList()[0].GetType());

            loggers.Level = "Warn";
            Assert.AreEqual("Warn", loggers.Level);
            Assert.AreEqual("Warn",  loggers.Members.ToList()[0].Level);

            Assert.IsTrue(loggers.IsEnabled);
        }

        // 2 Loggers
        [TestMethod]
        public void _2_Loggers() {
            var loggers = new Loggers(Console.Out.Instance, Console.Error.Instance);
            Assert.AreEqual(2, loggers.Members.Count());
            Assert.AreEqual(typeof(Console.Out  ), loggers.Members.ToList()[0].GetType());
            Assert.AreEqual(typeof(Console.Error), loggers.Members.ToList()[1].GetType());

            loggers.Level = " Warn";
            Assert.AreEqual("Warn;Warn", loggers.Level);
            Assert.AreEqual("Warn", loggers.Members.ToList()[0].Level);
            Assert.AreEqual("Warn", loggers.Members.ToList()[1].Level);

            loggers.Level = " Info ; Error ";
            Assert.AreEqual("Info;Error", loggers.Level);
            Assert.AreEqual("Info",  loggers.Members.ToList()[0].Level);
            Assert.AreEqual("Error", loggers.Members.ToList()[1].Level);

            Assert.IsTrue(loggers.IsEnabled);
        }
    }
}

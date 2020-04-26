using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DebugTrace;
using static DebugTrace.CSharp;
using Console = DebugTrace.Console;

namespace DebugTraceTest {
    [TestClass]
    public class LoggersTest {
        private static FileInfo? log4netFileInfo;
        private static FileInfo? nLogFileInfo;
        private ILogger? beforeLogger;
        private string? beforeLevel;

        // ClassInit
        [ClassInitialize]
        public static void ClassInit(TestContext context) {
            nLogFileInfo = new FileInfo("C:/Logs/DebugTrace/NLog.log");
            try {
                using (var stream = nLogFileInfo.Open(FileMode.Truncate)) {}
            }
            catch (Exception) {}
        }

        // ClassCleanup
        [ClassCleanup]
        public static void ClassCleanup() {
        }

        // TestInit
        [TestInitialize]
        public void TestInit() {
            beforeLogger = TraceBase.Logger;
            beforeLevel = beforeLogger.Level;
        }

        // TestCleanup
        [TestCleanup]
        public void TestCleanup() {
            if (beforeLogger != null)
                TraceBase.Logger = beforeLogger;
            if (beforeLevel != null)
                TraceBase.Logger.Level = beforeLevel;
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

        // NLogLevel
        [DataTestMethod]
        [DataRow("TRACE"    )]
        [DataRow("DEBUG"    )]
        [DataRow("INFO"     )]
        [DataRow("WARN"     )]
        [DataRow("ERROR"    )]
        [DataRow("FATAL"    )]

        [DataRow("Trace"    )]
        [DataRow("Debug"    )]
        [DataRow("Info"     )]
        [DataRow("Warn"     )]
        [DataRow("Error"    )]
        [DataRow("Fatal"    )]

        [DataRow("trace"    )]
        [DataRow("debug"    )]
        [DataRow("info"     )]
        [DataRow("warn"     )]
        [DataRow("error"    )]
        [DataRow("fatal"    )]
        public void NLogLevel(string level) {
            // setup:
            TraceBase.Logger = global::DebugTrace.NLog.Instance;
            TraceBase.Logger.Level = level;

            nLogFileInfo?.Refresh();
            var beforeLength = 0L;
            try {
                beforeLength = nLogFileInfo?.Length ?? 0;
            }
            catch (Exception) {}

            // when:
            Trace.Print($"NLogLevel {level} log");

            // then:
            var lastLog = nLogFileInfo == null ? "" : GetLastLog(nLogFileInfo, beforeLength);
            StringAssert.Contains(lastLog, $" {level.ToUpper()} ");
        }

        // GetLastLog
        public static string GetLastLog(FileInfo fileInfo, long beforeLength) {
            using (var stream = fileInfo.OpenRead()) {
                stream.Seek(beforeLength, SeekOrigin.Begin);
                fileInfo.Refresh();
                byte[] bytes = new byte[fileInfo.Length - beforeLength];
                stream.Read(bytes, 0, bytes.Length);
                return Encoding.UTF8.GetString(bytes);
            }
        }
    }
}

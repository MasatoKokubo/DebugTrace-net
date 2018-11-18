using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net.Config;
using DebugTrace;
using static DebugTrace.CSharp;
using Console = DebugTrace.Console;

namespace DebugTraceTest {
    [TestClass]
    public class LoggersTest {
        private static FileInfo log4netFileInfo;
        private static FileInfo nLogFileInfo;
        private ILogger beforeLogger;
        private string beforeLevel;

        // ClassInit
        [ClassInitialize]
        public static void ClassInit(TestContext context) {
            var log4netConfig = new XmlDocument();
            using (StreamReader reader = new StreamReader(new FileStream("log4net.config", FileMode.Open, FileAccess.Read))) {
                log4netConfig.Load(reader);
            }
            var rep = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(),
                typeof(log4net.Repository.Hierarchy.Hierarchy));
            XmlConfigurator.Configure(rep, log4netConfig["log4net"]);

            log4netFileInfo = new FileInfo("C:/Logs/DebugTrace/Log4net.log");
            try {
                using (var stream = log4netFileInfo.Open(FileMode.Truncate)) {}
            }
            catch (Exception) {}

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
            TraceBase.Logger = beforeLogger;
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

            nLogFileInfo.Refresh();
            var beforeLength = 0L;
            try {
                beforeLength = nLogFileInfo.Length;
            }
            catch (Exception) {}

            // when:
            Trace.Print($"NLogLevel {level} log");

            // then:
            var lastLog = GetLastLog(nLogFileInfo, beforeLength);
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

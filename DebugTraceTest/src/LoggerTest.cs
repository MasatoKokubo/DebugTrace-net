// LoggerTest.cs
// (C) 2018 Masato Kokubo
using System;
using System.IO;
using System.Text;
using DebugTrace;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DebugTraceTest;

[TestClass]
public class LoggerTest {
    private static ILogger? beforeLogger;
    private static FileInfo? log4netFileInfo;
    private static FileInfo? nLogFileInfo;

    [ClassInitialize]
    public static void ClassInit(TestContext context) {
        Trace.Enter();
        beforeLogger = Trace.Logger;
        log4netFileInfo = new FileInfo("/Logs/DebugTrace/Log4net.log");
        nLogFileInfo = new FileInfo("/Logs/DebugTrace/NLog.log");
        try {
            using (var stream = log4netFileInfo.Open(FileMode.Truncate)) {}
        }
        catch (Exception) {}
        try {
            using (var stream = nLogFileInfo.Open(FileMode.Truncate)) {}
        }
        catch (Exception) {}
        Trace.Leave();
    }

    [ClassCleanup]
    public static void ClassCleanup() {
        Trace.Enter();
        if (beforeLogger != null)
            Trace.Logger = beforeLogger;
        Trace.Leave();
    }

    // Log4netLevel
    [DataTestMethod]
    [DataRow("ALL"      )]
    [DataRow("FINEST"   )]
    [DataRow("VERBOSE"  )]
    [DataRow("FINER"    )]
    [DataRow("TRACE"    )]
    [DataRow("FINE"     )]
    [DataRow("DEBUG"    )]
    [DataRow("INFO"     )]
    [DataRow("NOTICE"   )]
    [DataRow("WARN"     )]
    [DataRow("ERROR"    )]
    [DataRow("SEVERE"   )]
    [DataRow("CRITICAL" )]
    [DataRow("ALERT"    )]
    [DataRow("FATAL"    )]
    [DataRow("EMERGENCY")]
    [DataRow("OFF"      )]

    [DataRow("All"      )]
    [DataRow("Finest"   )]
    [DataRow("Verbose"  )]
    [DataRow("Finer"    )]
    [DataRow("Trace"    )]
    [DataRow("Fine"     )]
    [DataRow("Debug"    )]
    [DataRow("Info"     )]
    [DataRow("Notice"   )]
    [DataRow("Warn"     )]
    [DataRow("Error"    )]
    [DataRow("Severe"   )]
    [DataRow("Critical" )]
    [DataRow("Alert"    )]
    [DataRow("Fatal"    )]
    [DataRow("Emergency")]
    [DataRow("Off"      )]

    [DataRow("all"      )]
    [DataRow("finest"   )]
    [DataRow("verbose"  )]
    [DataRow("finer"    )]
    [DataRow("trace"    )]
    [DataRow("fine"     )]
    [DataRow("debug"    )]
    [DataRow("info"     )]
    [DataRow("notice"   )]
    [DataRow("warn"     )]
    [DataRow("error"    )]
    [DataRow("severe"   )]
    [DataRow("critical" )]
    [DataRow("alert"    )]
    [DataRow("fatal"    )]
    [DataRow("emergency")]
    [DataRow("off"      )]
    public void Log4netLevel(string level) {
        // setup:
        Trace.Logger = Log4net.Instance;
        Trace.Logger.Level = level;

        log4netFileInfo?.Refresh();
        var beforeLength = 0L;
        try {
            beforeLength = log4netFileInfo?.Length ?? 0L;
        }
        catch (Exception) { }

        // when:
        Trace.Print($"Log4netLevel {level} log");

        // then:
        var lastLog = log4netFileInfo != null ? GetLastLog(log4netFileInfo, beforeLength) : "";
        StringAssert.Contains(lastLog, $" {level.ToUpper()} ");
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
        Trace.Logger = global::DebugTrace.NLog.Instance;
        Trace.Logger.Level = level;

        nLogFileInfo?.Refresh();
        var beforeLength = 0L;
        try {
            beforeLength = nLogFileInfo?.Length ?? 0L;
        }
        catch (Exception) {}

        // when:
        Trace.Print($"NLogLevel {level} log");

        // then:
        var lastLog = nLogFileInfo != null ? GetLastLog(nLogFileInfo, beforeLength) : "";
        StringAssert.Contains(lastLog, $" {level.ToUpper()} ");
    }

    // Log4netAndNLogLevel
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
    public void Log4netAndNLogLevel(string level) {
        // setup:
        Trace.Logger = new Loggers(Log4net.Instance, global::DebugTrace.NLog.Instance);
        Trace.Logger.Level = level;

        log4netFileInfo?.Refresh();
        nLogFileInfo?.Refresh();
        var log4netBeforeLength = 0L;
        var nLogBeforeLength = 0L;
        try {
            log4netBeforeLength = log4netFileInfo?.Length ?? 0L;
            nLogBeforeLength = nLogFileInfo?.Length ?? 0L;
        }
        catch (Exception) {}

        // when:
        Trace.Print($"Log4netAndNLogLevel {level} log");

        // then:
        var log4netLastLog = log4netFileInfo != null ? GetLastLog(log4netFileInfo, log4netBeforeLength) : "";
        StringAssert.Contains(log4netLastLog, $" {level.ToUpper()} ");

        var nLogLastLog = nLogFileInfo != null ? GetLastLog(nLogFileInfo, nLogBeforeLength) : "";
        StringAssert.Contains(nLogLastLog, $" {level.ToUpper()} ");
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

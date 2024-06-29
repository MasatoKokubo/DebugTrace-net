// PropertyV1Test.cs
// (C) 2018 Masato Kokubo
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DebugTrace;
using DebugTrace;

namespace DebugTraceTest;

[TestClass]
public class PropertyV1Test {
    private static int minimumOutputLength;

    // testProperties
    // ClassInit
    [ClassInitialize]
    public static void ClassInit(TestContext context) {
        Trace.InitClass("DebugTrace_PropertyV1Test");
        DebugTrace.Trace.MinimumOutputLength = 5;
    }

    // ClassCleanup
    [ClassCleanup]
    public static void ClassCleanup() {
        DebugTrace.Trace.MinimumOutputLength = minimumOutputLength;
        Trace.InitClass("DebugTrace");
    }

    // Trace.InitClass
    [TestMethod]
    public void TraceBaseInitClass() {
        Assert.AreEqual(Resource.Unescape("_Enter_ {0}.{1} ({2}:{3:D})")             , Trace.EnterFormat                      );
        Assert.AreEqual(Resource.Unescape("_Leave_ {0}.{1} ({2}:{3:D})")             , Trace.LeaveFormat                      );

        Assert.AreEqual(Resource.Unescape("||")                                      , Trace.IndentString                     );

        Assert.AreEqual(Resource.Unescape("<NonPrint>")                              , Trace.NonOutputString                  );

        Assert.AreEqual(Resource.Unescape("(_Length_:{0})")                          , Trace.LengthFormat                     ); // since 1.5.1

        Assert.AreEqual(                  "60"                                       , Trace.MaximumDataOutputWidth.ToString());
    }

    // EnterFormat
    [TestMethod]
    public void EnterFormat() {
        Trace.Enter();
        StringAssert.Contains(Trace.LastLog, "_Enter_");
    }

    // LeaveFormat
    [TestMethod]
    public void LeaveFormat() {
        Trace.Leave();
        StringAssert.Contains(Trace.LastLog, "_Leave_");
    }

    // IndentString
    [TestMethod]
    public void IndentString() {
        Trace.Enter();
        Trace.Enter();
        StringAssert.Contains(Trace.LastLog, Trace.IndentString);
        Trace.Leave();
        Trace.Leave();
    }

    // NonOutputString
    [TestMethod]
    public void NonOutputString() {
        Trace.Print("point", new Point(1, 2));
        StringAssert.Contains(Trace.LastLog, Trace.NonOutputString);
    }

    // LengthFormat
    [TestMethod]
    public void LengthFormat() {
        Trace.Print("value", "ABCDE");
        StringAssert.Contains(Trace.LastLog, string.Format(Trace.LengthFormat, 5));
    }
}

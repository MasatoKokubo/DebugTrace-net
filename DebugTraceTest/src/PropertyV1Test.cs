// PropertyV1Test.cs
// (C) 2018 Masato Kokubo
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DebugTrace.CSharp;
using DebugTrace;

namespace DebugTraceTest {
    [TestClass]
    public class PropertyV1Test {
        // testProperties
        // ClassInit
        [ClassInitialize]
        public static void ClassInit(TestContext context) {
            TraceBase.InitClass("DebugTrace_PropertyV1Test");
        }

        // ClassCleanup
        [ClassCleanup]
        public static void ClassCleanup() {
            TraceBase.InitClass("DebugTrace");
        }

        // TraceBase.InitClass
        [TestMethod]
        public void TraceBaseInitClass() {
            Assert.AreEqual(Resource.Unescape("_Enter_ {0}.{1} ({2}:{3:D})")             , TraceBase.EnterFormat                      );
            Assert.AreEqual(Resource.Unescape("_Leave_ {0}.{1} ({2}:{3:D})")             , TraceBase.LeaveFormat                      );

            Assert.AreEqual(Resource.Unescape("||")                                      , TraceBase.IndentString                     );

            Assert.AreEqual(Resource.Unescape("<NonPrint>")                              , TraceBase.NonOutputString                  );

            Assert.AreEqual(Resource.Unescape("(_Length_:{0})")                          , TraceBase.LengthFormat                     ); // since 1.5.1

            Assert.AreEqual(                  "60"                                       , TraceBase.MaximumDataOutputWidth.ToString());
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
            StringAssert.Contains(Trace.LastLog, TraceBase.IndentString);
            Trace.Leave();
            Trace.Leave();
        }

        // NonOutputString
        [TestMethod]
        public void NonOutputString() {
            Trace.Print("point", new Point(1, 2));
            StringAssert.Contains(Trace.LastLog, TraceBase.NonOutputString);
        }

        // LengthFormat
        [TestMethod]
        public void LengthFormat() {
            Trace.Print("value", "ABCDE");
            StringAssert.Contains(Trace.LastLog, string.Format(TraceBase.LengthFormat, 5));
        }
    }
}

// PropertyTest.cs
// (C) 2018 Masato Kokubo
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DebugTrace.CSharp;
using DebugTrace;

namespace DebugTraceTest {
    [TestClass]
    public class PropertyTest {
        // testProperties
        // ClassInit
        [ClassInitialize]
        public static void ClassInit(TestContext context) {
            TraceBase.InitClass("DebugTrace_PropertyTest");
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
            Assert.AreEqual(Resource.Unescape("_Thread_ {0}")                            , TraceBase.ThreadBoundaryFormat             );
            Assert.AreEqual(Resource.Unescape("_ {0} _")                                 , TraceBase.ClassBoundaryFormat              );
            Assert.AreEqual(Resource.Unescape("||")                                      , TraceBase.IndentString                     );
            Assert.AreEqual(Resource.Unescape("``")                                      , TraceBase.DataIndentString                 );
            Assert.AreEqual(Resource.Unescape("<Limit>")                                 , TraceBase.LimitString                      );
            Assert.AreEqual(Resource.Unescape("<DefaultNameSpace>")                      , TraceBase.DefaultNameSpaceString           );
            Assert.AreEqual(Resource.Unescape("<NonOutput>")                             , TraceBase.NonOutputString                  );
            Assert.AreEqual(Resource.Unescape("<CyclicReference>")                       , TraceBase.CyclicReferenceString            );
            Assert.AreEqual(Resource.Unescape(@"\s<=\s")                                 , TraceBase.VarNameValueSeparator            );
            Assert.AreEqual(Resource.Unescape(@"\s::\s")                                 , TraceBase.KeyValueSeparator                );
            Assert.AreEqual(Resource.Unescape(@"\s[{2}:{3:D}]")                          , TraceBase.PrintSuffixFormat                );
            Assert.AreEqual(Resource.Unescape(@"\s_Count_:{0}")                          , TraceBase.CountFormat                      ); // since 1.5.1
            Assert.AreEqual(                  "3"                                        , TraceBase.MinimumOutputCount    .ToString()); // since 2.0.0
            Assert.AreEqual(Resource.Unescape("(_Length_:{0})")                          , TraceBase.LengthFormat                     ); // since 1.5.1
            Assert.AreEqual(                  "4"                                        , TraceBase.MinimumOutputLength   .ToString()); // since 2.0.0
            Assert.AreEqual(Resource.Unescape("{0:MM-dd-yyyy hh:mm:ss.fffffffK}")        , TraceBase.DateTimeFormat                   );
            Assert.AreEqual(Resource.Unescape("{0:MM-dd-yyyy hh:mm:ss.fff} [{1:D2}] {2}"), TraceBase.LogDateTimeFormat                ); // since 1.3.0
            Assert.AreEqual(                  "60"                                       , TraceBase.MaximumDataOutputWidth.ToString());
            Assert.AreEqual(                  "8"                                        , TraceBase.CollectionLimit       .ToString());
            Assert.AreEqual(                  "32"                                       , TraceBase.StringLimit           .ToString());
            Assert.AreEqual(                  "2"                                        , TraceBase.ReflectionNestLimit   .ToString());
            Assert.AreEqual(                  "DebugTraceTest"                           , TraceBase.DefaultNameSpace                 );
            Assert.IsTrue(TraceBase.ReflectionClasses.Contains("DebugTraceTest.Point3"));
            Assert.IsTrue(TraceBase.ReflectionClasses.Contains("System.DateTime"));
            Assert.AreEqual(true           , TraceBase.OutputNonPublicFields    ); // since 1.4.4
            Assert.AreEqual(true           , TraceBase.OutputNonPublicProperties); // since 1.4.4
            Assert.AreEqual(typeof(Loggers), TraceBase.Logger.GetType()         ); // since 1.5.0
            CollectionAssert.AreEqual(
                new List<Type>() {typeof(DebugTrace.Console.Out), typeof(DebugTrace.Console.Error)},
                ((Loggers)TraceBase.Logger).Members.Select(logger => logger.GetType()).ToList()); // since 1.5.0
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

        // DataIndentString
        [TestMethod]
        public void DataIndentString() {
            Trace.Print("contact", new Contact(1, "Akane", "Apple", new DateTime(2018, 4, 1)));
            StringAssert.Contains(Trace.LastLog, TraceBase.DataIndentString);
        }

        // LimitString / CollectionLimit
        [TestMethod]
        public void LimitString() {
            Trace.Print("value", new int[TraceBase.CollectionLimit]);
            StringAssert.Contains(Trace.LastLog, ", 0}");

            Trace.Print("value", new int[TraceBase.CollectionLimit + 1]);
            StringAssert.Contains(Trace.LastLog, ", 0, " + TraceBase.LimitString + "}");
        }

        // DefaultNameSpaceString / DefaultNameSpace
        [TestMethod]
        public void DefaultNameSpaceString() {
            Trace.Print("point", new Point(1, 2));
            StringAssert.Contains(Trace.LastLog, TraceBase.DefaultNameSpaceString + ".Point");
        }

        // NonOutputString
        [TestMethod]
        public void NonOutputString() {
            Trace.Print("point", new Point(1, 2));
            StringAssert.Contains(Trace.LastLog, TraceBase.NonOutputString);
        }

        // CyclicReferenceString
        [TestMethod]
        public void CyclicReferenceString() {
            var node1 = new Node<int>(1);
            var node2 = new Node<int>(2, node1, node1);
            node1.Left = node2;
            node1.Right = node2;
            Trace.Print("node1", node1);
            StringAssert.Contains(Trace.LastLog, TraceBase.CyclicReferenceString);
        }

        // VarNameValueSeparator
        [TestMethod]
        public void VarNameValueSeparator() {
            var value = 1;
            Trace.Print("value", value);
            StringAssert.Contains(Trace.LastLog, "value" + TraceBase.VarNameValueSeparator + value);
        }

        // KeyValueSeparator
        [TestMethod]
        public void KeyValueSeparator() {
            Trace.Print("value", new Dictionary<int, int>() {{1, 2}});
            StringAssert.Contains(Trace.LastLog, "1" + TraceBase.KeyValueSeparator + "2");
        }

        // CountFormat
        [TestMethod]
        public void CountFormat() {
            Trace.Print("value", new List<int>() {1, 2, 3});
            StringAssert.Contains(Trace.LastLog, string.Format(TraceBase.CountFormat, 3));
        }

        // LengthFormat
        [TestMethod]
        public void LengthFormat() {
            Trace.Print("value", "ABCDE");
            StringAssert.Contains(Trace.LastLog, string.Format(TraceBase.LengthFormat, 5));
        }

        // ReflectionClasses
        [TestMethod]
        public void ReflectionClasses() {
            var rectangle = new Rectangle(1, 2, 3, 4);
            Trace.Print("rectangle", rectangle); // use ToString method
            StringAssert.Contains(Trace.LastLog, rectangle.ToString());

            var point3 = new Point3(1, 2, 3);
            Trace.Print("point3", point3); // use reflection
            StringAssert.Contains(Trace.LastLog, "X" + TraceBase.KeyValueSeparator + point3.X);

            var dateTime = DateTime.Now;
            Trace.Print("dateTime", dateTime); // use reflection
            StringAssert.Contains(Trace.LastLog, TraceBase.KeyValueSeparator);
        }

        public class Inner {
            private            int           PrivateField = 1;
            protected          int         ProtectedField = 2;
            internal           int          InternalField = 3;
            protected internal int ProtectedInternalField = 4;
            private protected  int  PrivateProtectedField = 5;
            public             int            PublicField = 6;

            private            int           PrivateProperty {get;} = 1;
            protected          int         ProtectedProperty {get;} = 2;
            internal           int          InternalProperty {get;} = 3;
            protected internal int ProtectedInternalProperty {get;} = 4;
            private protected  int  PrivateProtectedProperty {get;} = 5;
            public             int            PublicProperty {get;} = 6;
        }

        // OutputNonPublicFields / since 1.4.4
        // OutputNonPublicProperties / since 1.4.4
        [TestMethod]
        public void OutputNonPublicFields_Properties() {
            TraceBase.OutputNonPublicFields     = false;
            TraceBase.OutputNonPublicProperties = false;
            Trace.Print("value", new Inner());

            Assert.IsFalse(Trace.LastLog.Contains(                     "private PrivateField"));
            Assert.IsFalse(Trace.LastLog.Contains(                 "protected ProtectedField"));
            Assert.IsFalse(Trace.LastLog.Contains(                   "internal InternalField"));
            Assert.IsFalse(Trace.LastLog.Contains("protected internal ProtectedInternalField"));
            Assert.IsFalse(Trace.LastLog.Contains(  "private protected PrivateProtectedField"));
            Assert.IsTrue (Trace.LastLog.Contains(                              "PublicField"));

            Assert.IsFalse(Trace.LastLog.Contains(                     "private PrivateProperty"));
            Assert.IsFalse(Trace.LastLog.Contains(                 "protected ProtectedProperty"));
            Assert.IsFalse(Trace.LastLog.Contains(                   "internal InternalProperty"));
            Assert.IsFalse(Trace.LastLog.Contains("protected internal ProtectedInternalProperty"));
            Assert.IsFalse(Trace.LastLog.Contains(  "private protected PrivateProtectedProperty"));
            Assert.IsTrue (Trace.LastLog.Contains(                              "PublicProperty"));

            TraceBase.OutputNonPublicFields     = true;
            TraceBase.OutputNonPublicProperties = false;
            Trace.Print("value", new Inner());

            Assert.IsTrue (Trace.LastLog.Contains(                     "private PrivateField"));
            Assert.IsTrue (Trace.LastLog.Contains(                 "protected ProtectedField"));
            Assert.IsTrue (Trace.LastLog.Contains(                   "internal InternalField"));
            Assert.IsTrue (Trace.LastLog.Contains("protected internal ProtectedInternalField"));
            Assert.IsTrue (Trace.LastLog.Contains(  "private protected PrivateProtectedField"));
            Assert.IsTrue (Trace.LastLog.Contains(                              "PublicField"));

            Assert.IsFalse(Trace.LastLog.Contains(                     "private PrivateProperty"));
            Assert.IsFalse(Trace.LastLog.Contains(                 "protected ProtectedProperty"));
            Assert.IsFalse(Trace.LastLog.Contains(                   "internal InternalProperty"));
            Assert.IsFalse(Trace.LastLog.Contains("protected internal ProtectedInternalProperty"));
            Assert.IsFalse(Trace.LastLog.Contains(  "private protected PrivateProtectedProperty"));
            Assert.IsTrue (Trace.LastLog.Contains(                              "PublicProperty"));

            TraceBase.OutputNonPublicFields     = false;
            TraceBase.OutputNonPublicProperties = true;
            Trace.Print("value", new Inner());

            Assert.IsFalse(Trace.LastLog.Contains(                     "private PrivateField"));
            Assert.IsFalse(Trace.LastLog.Contains(                 "protected ProtectedField"));
            Assert.IsFalse(Trace.LastLog.Contains(                   "internal InternalField"));
            Assert.IsFalse(Trace.LastLog.Contains("protected internal ProtectedInternalField"));
            Assert.IsFalse(Trace.LastLog.Contains(  "private protected PrivateProtectedField"));
            Assert.IsTrue (Trace.LastLog.Contains(                              "PublicField"));

            Assert.IsTrue (Trace.LastLog.Contains(                     "private PrivateProperty"));
            Assert.IsTrue (Trace.LastLog.Contains(                 "protected ProtectedProperty"));
            Assert.IsTrue (Trace.LastLog.Contains(                   "internal InternalProperty"));
            Assert.IsTrue (Trace.LastLog.Contains("protected internal ProtectedInternalProperty"));
            Assert.IsTrue (Trace.LastLog.Contains(  "private protected PrivateProtectedProperty"));
            Assert.IsTrue (Trace.LastLog.Contains(                              "PublicProperty"));

            TraceBase.OutputNonPublicFields     = true;
            TraceBase.OutputNonPublicProperties = true;
            Trace.Print("value", new Inner());

            Assert.IsTrue (Trace.LastLog.Contains(                     "private PrivateField"));
            Assert.IsTrue (Trace.LastLog.Contains(                 "protected ProtectedField"));
            Assert.IsTrue (Trace.LastLog.Contains(                   "internal InternalField"));
            Assert.IsTrue (Trace.LastLog.Contains("protected internal ProtectedInternalField"));
            Assert.IsTrue (Trace.LastLog.Contains(  "private protected PrivateProtectedField"));
            Assert.IsTrue (Trace.LastLog.Contains(                              "PublicField"));

            Assert.IsTrue (Trace.LastLog.Contains(                     "private PrivateProperty"));
            Assert.IsTrue (Trace.LastLog.Contains(                 "protected ProtectedProperty"));
            Assert.IsTrue (Trace.LastLog.Contains(                   "internal InternalProperty"));
            Assert.IsTrue (Trace.LastLog.Contains("protected internal ProtectedInternalProperty"));
            Assert.IsTrue (Trace.LastLog.Contains(  "private protected PrivateProtectedProperty"));
            Assert.IsTrue (Trace.LastLog.Contains(                              "PublicProperty"));
        }
    }
}

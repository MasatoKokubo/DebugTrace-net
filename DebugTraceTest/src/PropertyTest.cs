// PropertyTest.cs
// (C) 2018 Masato Kokubo
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DebugTrace;

namespace DebugTraceTest;

[TestClass]
public class PropertyTest {
    [ClassInitialize]
    public static void ClassInit(TestContext context) {
        Trace.Enter();
        Trace.InitClass("DebugTrace_PropertyTest");
        Trace.Leave();
    }

    [ClassCleanup]
    public static void ClassCleanup() {
        Trace.Enter();
        Trace.InitClass("DebugTrace");
        Trace.Leave();
    }

    // Trace.InitClass
    [TestMethod]
    public void TraceInitClass() {
        Assert.AreEqual(Resource.Unescape("_Enter_ {0}.{1} ({2}:{3:D})")             , Trace.EnterFormat                      );
        Assert.AreEqual(Resource.Unescape("_Leave_ {0}.{1} ({2}:{3:D})")             , Trace.LeaveFormat                      );
        Assert.AreEqual(Resource.Unescape("_Thread_ {0}")                            , Trace.ThreadBoundaryFormat             );
        Assert.AreEqual(Resource.Unescape("_ {0} _")                                 , Trace.ClassBoundaryFormat              );
        Assert.AreEqual(Resource.Unescape("||")                                      , Trace.IndentString                     );
        Assert.AreEqual(Resource.Unescape("``")                                      , Trace.DataIndentString                 );
        Assert.AreEqual(Resource.Unescape("<Limit>")                                 , Trace.LimitString                      );
        Assert.AreEqual(Resource.Unescape("<DefaultNameSpace>")                      , Trace.DefaultNameSpaceString           );
        Assert.AreEqual(Resource.Unescape("<NonOutput>")                             , Trace.NonOutputString                  );
        Assert.AreEqual(Resource.Unescape("<CyclicReference>")                       , Trace.CyclicReferenceString            );
        Assert.AreEqual(Resource.Unescape(@"\s<=\s")                                 , Trace.VarNameValueSeparator            );
        Assert.AreEqual(Resource.Unescape(@"\s::\s")                                 , Trace.KeyValueSeparator                );
        Assert.AreEqual(Resource.Unescape(@"\s[{2}:{3:D}]")                          , Trace.PrintSuffixFormat                );
        Assert.AreEqual(Resource.Unescape(@"\s_Count_:{0}")                          , Trace.CountFormat                      ); // since 1.5.1
        Assert.AreEqual(                  "3"                                        , Trace.MinimumOutputCount    .ToString()); // since 2.0.0
        Assert.AreEqual(Resource.Unescape("(_Length_:{0})")                          , Trace.LengthFormat                     ); // since 1.5.1
        Assert.AreEqual(                  "4"                                        , Trace.MinimumOutputLength   .ToString()); // since 2.0.0
        Assert.AreEqual(Resource.Unescape("{0:MM-dd-yyyy hh:mm:ss.fffffffK}")        , Trace.DateTimeFormat                   );
        Assert.AreEqual(Resource.Unescape("{0:MM-dd-yyyy hh:mm:ss.fff} [{1:D2}] {2}"), Trace.LogDateTimeFormat                ); // since 1.3.0
        Assert.AreEqual(                  "60"                                       , Trace.MaximumDataOutputWidth.ToString());
        Assert.AreEqual(                  "8"                                        , Trace.CollectionLimit       .ToString());
        Assert.AreEqual(                  "32"                                       , Trace.StringLimit           .ToString());
        Assert.AreEqual(                  "2"                                        , Trace.ReflectionLimit       .ToString());
        Assert.AreEqual(                  "DebugTraceTest"                           , Trace.DefaultNameSpace                 );
        Assert.IsTrue(Trace.ReflectionClasses.Contains("DebugTraceTest.Point3"));
        Assert.IsTrue(Trace.ReflectionClasses.Contains("System.DateTime"));
        Assert.AreEqual(true           , Trace.OutputNonPublicFields    ); // since 1.4.4
        Assert.AreEqual(true           , Trace.OutputNonPublicProperties); // since 1.4.4
        Assert.AreEqual(typeof(Loggers), Trace.Logger.GetType()         ); // since 1.5.0
        CollectionAssert.AreEqual(
            new List<Type>() {typeof(DebugTrace.Console.Out), typeof(DebugTrace.Console.Error)},
            ((Loggers)Trace.Logger).Members.Select(logger => logger.GetType()).ToList()); // since 1.5.0
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

    // DataIndentString
    [TestMethod]
    public void DataIndentString() {
        Trace.Print("contact", new Contact(1, "Akane", "Apple", new DateTime(2018, 4, 1)));
        StringAssert.Contains(Trace.LastLog, Trace.DataIndentString);
    }

    // LimitString / CollectionLimit
    [TestMethod]
    public void LimitString() {
        Trace.Print("value", new int[Trace.CollectionLimit]);
        StringAssert.Contains(Trace.LastLog, ", 0}");

        Trace.Print("value", new int[Trace.CollectionLimit + 1]);
        StringAssert.Contains(Trace.LastLog, ", 0, " + Trace.LimitString + "}");
    }

    // DefaultNameSpaceString / DefaultNameSpace
    [TestMethod]
    public void DefaultNameSpaceString() {
        Trace.Print("point", new Point(1, 2));
        StringAssert.Contains(Trace.LastLog, Trace.DefaultNameSpaceString + ".Point");
    }

    // NonOutputString
    [TestMethod]
    public void NonOutputString() {
        Trace.Print("point", new Point(1, 2));
        StringAssert.Contains(Trace.LastLog, Trace.NonOutputString);
    }

    // CyclicReferenceString
    [TestMethod]
    public void CyclicReferenceString() {
        var node1 = new Node<int>(1);
        var node2 = new Node<int>(2, node1, node1);
        node1.Left = node2;
        node1.Right = node2;
        Trace.Print("node1", node1);
        StringAssert.Contains(Trace.LastLog, Trace.CyclicReferenceString);
    }

    // VarNameValueSeparator
    [TestMethod]
    public void VarNameValueSeparator() {
        var value = 1;
        Trace.Print("value", value);
        StringAssert.Contains(Trace.LastLog, "value" + Trace.VarNameValueSeparator + value);
    }

    // KeyValueSeparator
    [TestMethod]
    public void KeyValueSeparator() {
        Trace.Print("value", new Dictionary<int, int>() {{1, 2}});
        StringAssert.Contains(Trace.LastLog, "1" + Trace.KeyValueSeparator + "2");
    }

    // CountFormat
    [TestMethod]
    public void CountFormat() {
        Trace.Print("value", new List<int>() {1, 2, 3});
        StringAssert.Contains(Trace.LastLog, string.Format(Trace.CountFormat, 3));
    }

    // LengthFormat
    [TestMethod]
    public void LengthFormat() {
        Trace.Print("value", "ABCDE");
        StringAssert.Contains(Trace.LastLog, string.Format(Trace.LengthFormat, 5));
    }

    // ReflectionClasses
    [TestMethod]
    public void ReflectionClasses() {
        var rectangle = new Rectangle(1, 2, 3, 4);
        Trace.Print("rectangle", rectangle); // use ToString method
        StringAssert.Contains(Trace.LastLog, rectangle.ToString());

        var point3 = new Point3(1, 2, 3);
        Trace.Print("point3", point3); // use reflection
        StringAssert.Contains(Trace.LastLog, "X" + Trace.KeyValueSeparator + point3.X);

        var dateTime = DateTime.Now;
        Trace.Print("dateTime", dateTime); // use reflection
        StringAssert.Contains(Trace.LastLog, Trace.KeyValueSeparator);
    }

    // OutputNonPublicFields / since 1.4.4
    // OutputNonPublicProperties / since 1.4.4
    [TestMethod]
    public void OutputNonPublicFields_Properties() {
        var fieldsProperties = new FieldsProperties();

        Trace.OutputNonPublicFields     = false;
        Trace.OutputNonPublicProperties = false;
        Trace.Print("value", fieldsProperties);

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

        Trace.OutputNonPublicFields     = true;
        Trace.OutputNonPublicProperties = false;
        Trace.Print("value", fieldsProperties);

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

        Trace.OutputNonPublicFields     = false;
        Trace.OutputNonPublicProperties = true;
        Trace.Print("value", fieldsProperties);

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

        Trace.OutputNonPublicFields     = true;
        Trace.OutputNonPublicProperties = true;
        Trace.Print("value", fieldsProperties);

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

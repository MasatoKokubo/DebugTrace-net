using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DebugTrace.CSharp;
using DebugTrace;

namespace DebugTraceTest {
    [TestClass]
    public class PropertyTest {
        // testProperties
        private static readonly IDictionary<string, string> testProperties = new Dictionary<string, string>() {
            {nameof(TraceBase.EnterString            ), @"_Enter_ {0}.{1} ({2}:{3:D})"},
            {nameof(TraceBase.LeaveString            ), @"_Leave_ {0}.{1} ({2}:{3:D})"},
            {nameof(TraceBase.ThreadBoundaryString   ), @"_Thread_ {0}"},
            {nameof(TraceBase.ClassBoundaryString    ), @"_ {0} _"},
            {nameof(TraceBase.CodeIndentString       ), @"||"},
            {nameof(TraceBase.DataIndentString       ), @"``"},
            {nameof(TraceBase.LimitString            ), @"<Limit>"},
            {nameof(TraceBase.DefaultNameSpaceString ), @"<DefaultNameSpace>"},
            {nameof(TraceBase.NonPrintString         ), @"<NonPrint>"},
            {nameof(TraceBase.CyclicReferenceString  ), @"<CyclicReference>"},
            {nameof(TraceBase.VarNameValueSeparator  ), @"\s<=\s"},
            {nameof(TraceBase.KeyValueSeparator      ), @"\s::\s"},
            {nameof(TraceBase.PrintSuffixFormat      ), @"\s[{2}:{3:D}]"},
            {nameof(TraceBase.DateTimeFormat         ), @"{0:MM-dd-yyyy hh:mm:ss.fff}"},
            {nameof(TraceBase.MaxDataOutputWidth     ), "40"},
            {nameof(TraceBase.CollectionLimit        ), "8"},
            {nameof(TraceBase.StringLimit            ), "32"},
            {nameof(TraceBase.ReflectionNestLimit    ), "2"},
            {nameof(TraceBase.NonPrintProperties     ), "DebugTraceTest.Point.X, DebugTraceTest.Point.Y"},
            {nameof(TraceBase.DefaultNameSpace       ), "DebugTraceTest"},
            {nameof(TraceBase.ReflectionClasses      ), "DebugTraceTest.Point3,System.DateTime"},
        };

        // emptyProperties
        private static readonly IDictionary<string, string> emptyProperties = new Dictionary<string, string>();

        // ClassInit
        [ClassInitialize]
        public static void ClassInit(TestContext context) {
            WriteToProperties(testProperties);
            TraceBase.InitClass();
        }

        // ClassCleanup
        [ClassCleanup]
        public static void ClassCleanup() {
            WriteToProperties(emptyProperties);
            TraceBase.InitClass();
        }

        // WriteToProperties
        private static void WriteToProperties(IDictionary<string, string> values) {
            var resourceFileInfo = TraceBase.Resource.FileInfo;
            if (resourceFileInfo.Exists)
                resourceFileInfo.Delete();

            using (FileStream stream = resourceFileInfo.Open(FileMode.Create, FileAccess.Write, FileShare.None)) {
                var encoding = new UTF8Encoding();

                foreach (var key in values.Keys) {
                    var line = $"{key} = {values[key]}\n";
                    var bytes = encoding.GetBytes(line);
                    stream.Write(bytes, 0, bytes.Length);
                }
            }
        }

        // TraceBase.InitClass
        [TestMethod]
        public void TraceBaseInitClass() {
            Assert.AreEqual(TraceBase.EnterString            , Resource.Unescape(testProperties[nameof(TraceBase.EnterString            )]));
            Assert.AreEqual(TraceBase.LeaveString            , Resource.Unescape(testProperties[nameof(TraceBase.LeaveString            )]));
            Assert.AreEqual(TraceBase.ThreadBoundaryString   , Resource.Unescape(testProperties[nameof(TraceBase.ThreadBoundaryString   )]));
            Assert.AreEqual(TraceBase.ClassBoundaryString    , Resource.Unescape(testProperties[nameof(TraceBase.ClassBoundaryString    )]));
            Assert.AreEqual(TraceBase.CodeIndentString       , Resource.Unescape(testProperties[nameof(TraceBase.CodeIndentString       )]));
            Assert.AreEqual(TraceBase.DataIndentString       , Resource.Unescape(testProperties[nameof(TraceBase.DataIndentString       )]));
            Assert.AreEqual(TraceBase.LimitString            , Resource.Unescape(testProperties[nameof(TraceBase.LimitString            )]));
            Assert.AreEqual(TraceBase.DefaultNameSpaceString , Resource.Unescape(testProperties[nameof(TraceBase.DefaultNameSpaceString )]));
            Assert.AreEqual(TraceBase.NonPrintString         , Resource.Unescape(testProperties[nameof(TraceBase.NonPrintString         )]));
            Assert.AreEqual(TraceBase.CyclicReferenceString  , Resource.Unescape(testProperties[nameof(TraceBase.CyclicReferenceString  )]));
            Assert.AreEqual(TraceBase.VarNameValueSeparator  , Resource.Unescape(testProperties[nameof(TraceBase.VarNameValueSeparator  )]));
            Assert.AreEqual(TraceBase.KeyValueSeparator      , Resource.Unescape(testProperties[nameof(TraceBase.KeyValueSeparator      )]));
            Assert.AreEqual(TraceBase.PrintSuffixFormat      , Resource.Unescape(testProperties[nameof(TraceBase.PrintSuffixFormat      )]));
            Assert.AreEqual(TraceBase.DateTimeFormat         , Resource.Unescape(testProperties[nameof(TraceBase.DateTimeFormat         )]));
            Assert.AreEqual(TraceBase.MaxDataOutputWidth .ToString(),            testProperties[nameof(TraceBase.MaxDataOutputWidth     )]);
            Assert.AreEqual(TraceBase.CollectionLimit    .ToString(),            testProperties[nameof(TraceBase.CollectionLimit        )]);
            Assert.AreEqual(TraceBase.StringLimit        .ToString(),            testProperties[nameof(TraceBase.StringLimit            )]);
            Assert.AreEqual(TraceBase.ReflectionNestLimit.ToString(),            testProperties[nameof(TraceBase.ReflectionNestLimit    )]);
            AssertAreEqual (TraceBase.NonPrintProperties     ,                   testProperties[nameof(TraceBase.NonPrintProperties     )].Split(',').Select(s => s.Trim()).ToArray());
            Assert.AreEqual(TraceBase.DefaultNameSpace       ,                   testProperties[nameof(TraceBase.DefaultNameSpace       )]);

            var reflectionClasses = new HashSet<string>(testProperties[nameof(TraceBase.ReflectionClasses)].Split(',').Select(s => s.Trim()));
            reflectionClasses.Add(typeof(Tuple).FullName); // Tuple
            reflectionClasses.Add(typeof(ValueTuple).FullName); // ValueTuple
            AssertAreEqual(TraceBase.ReflectionClasses, reflectionClasses);
        }

        // AssertAreEqual
        private static void AssertAreEqual<T>(IEnumerable<T> enumerable1, IEnumerable<T> enumerable2) {
            if (enumerable1 == null) {
                if (enumerable2 == null) return;
                Assert.Fail($"enumerable1 == null, enumerable2 != null");
            }
            if (enumerable2 == null)
                Assert.Fail($"enumerable1 != null, enumerable2 == null");
            if (enumerable1.Count() != enumerable2.Count())
                Assert.Fail($"enumerable1.Count() = {enumerable1.Count()}, enumerable2.Count() = {enumerable2.Count()}");
            for (var index = 0; index < enumerable1.Count(); ++index)
                if (!enumerable1.ElementAt(index).Equals(enumerable1.ElementAt(index)))
                    Assert.Fail($"enumerable1.ElementAt({index}) = {enumerable1.ElementAt(index)}, enumerable2.ElementAt({index}) = {enumerable1.ElementAt(index)}");
        }

        // EnterString
        [TestMethod]
        public void EnterString() {
            Trace.Enter();
            Assert.IsTrue(Trace.LastLog.IndexOf("_Enter_") >= 0);
        }

        // LeaveString
        [TestMethod]
        public void LeaveString() {
            Trace.Leave();
            Assert.IsTrue(Trace.LastLog.IndexOf("_Leave_") >= 0);
        }

        // CodeIndentString
        [TestMethod]
        public void CodeIndentString() {
            Trace.Enter();
            Trace.Enter();
            Assert.IsTrue(Trace.LastLog.IndexOf(TraceBase.CodeIndentString) >= 0);
            Trace.Leave();
            Trace.Leave();
        }

        // DataIndentString
        [TestMethod]
        public void DataIndentString() {
            Trace.Print("contact", new Contact(1, "Akane", "Apple", new DateTime(2018, 4, 1)));
            Assert.IsTrue(Trace.LastLog.IndexOf(TraceBase.DataIndentString) >= 0);
        }

        // LimitString / CollectionLimit
        [TestMethod]
        public void LimitString() {
            Trace.Print("value", new int[TraceBase.CollectionLimit]);
            Assert.IsTrue(Trace.LastLog.IndexOf(", 0}") >= 0);

            Trace.Print("value", new int[TraceBase.CollectionLimit + 1]);
            Assert.IsTrue(Trace.LastLog.IndexOf(", 0, " + TraceBase.LimitString + "}") >= 0);
        }

        // DefaultNameSpaceString / DefaultNameSpace
        [TestMethod]
        public void DefaultNameSpaceString() {
            Trace.Print("point", new Point(1, 2));
            Assert.IsTrue(Trace.LastLog.IndexOf(TraceBase.DefaultNameSpaceString + ".Point") >= 0);
        }

        // NonPrintString
        [TestMethod]
        public void NonPrintString() {
            Trace.Print("point", new Point(1, 2));
            Assert.IsTrue(Trace.LastLog.IndexOf(TraceBase.NonPrintString) >= 0);
        }

        // CyclicReferenceString
        [TestMethod]
        public void CyclicReferenceString() {
            var node1 = new Node<int>(1);
            var node2 = new Node<int>(2, node1, node1);
            node1.Left = node2;
            node1.Right = node2;
            Trace.Print("node1", node1);
            Assert.IsTrue(Trace.LastLog.IndexOf(TraceBase.CyclicReferenceString) >= 0);
        }

        // VarNameValueSeparator
        [TestMethod]
        public void VarNameValueSeparator() {
            var value = 1;
            Trace.Print("value", value);
            Assert.IsTrue(Trace.LastLog.IndexOf("value" + TraceBase.VarNameValueSeparator + value) >= 0);
        }

        // KeyValueSeparator
        [TestMethod]
        public void KeyValueSeparator() {
            Trace.Print("value", new Dictionary<int, int>() {{1, 2}});
            Assert.IsTrue(Trace.LastLog.IndexOf("1" + TraceBase.KeyValueSeparator + "2") >= 0);
        }

        // ReflectionClasses
        [TestMethod]
        public void ReflectionClasses() {
            var rectangle = new Rectangle(1, 2, 3, 4);
            Trace.Print("rectangle", rectangle); // use ToString method
            Assert.IsTrue(Trace.LastLog.IndexOf(rectangle.ToString()) >= 0);

            var point3 = new Point3(1, 2, 3);
            Trace.Print("point3", point3); // use reflection
            Assert.IsTrue(Trace.LastLog.IndexOf("X" + TraceBase.KeyValueSeparator + point3.X) >=  0);

            var dateTime = DateTime.Now;
            Trace.Print("dateTime", dateTime); // use reflection
            Assert.IsTrue(Trace.LastLog.IndexOf(TraceBase.KeyValueSeparator) >=  0);
        }

    }
}

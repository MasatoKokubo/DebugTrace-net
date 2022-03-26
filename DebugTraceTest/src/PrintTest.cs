// PrintTest.cs
// (C) 2018 Masato Kokubo
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DebugTrace;
using static DebugTrace.CSharp;

namespace DebugTraceTest;

[TestClass]
public class PrintTest {
    // TestCleanup
    [TestCleanup]
    public void TestCleanup() {
        TraceBase.OutputNonPublicFields     = false;
        TraceBase.OutputNonPublicProperties = false;
    }

    // bool
    [DataTestMethod]
    [DataRow(null , "v = null (")]
    [DataRow(false, "v = false (")]
    [DataRow(true , "v = true (")]
    public void PrintBool(bool? v, string expect) {
        Assert.AreEqual(v, Trace.Print("v", v));
        StringAssert.Contains(Trace.LastLog, expect);
    }

    // char
    [DataTestMethod]
    [DataRow(null    , "v = null (")]
    [DataRow('\0'    , "v = '\\0' (")]
    [DataRow('\a'    , "v = '\\a' (")]
    [DataRow('\b'    , "v = '\\b' (")]
    [DataRow('\t'    , "v = '\\t' (")]
    [DataRow('\n'    , "v = '\\n' (")]
    [DataRow('\v'    , "v = '\\v' (")]
    [DataRow('\f'    , "v = '\\f' (")]
    [DataRow('\r'    , "v = '\\r' (")]
    [DataRow('"'     , "v = '\"' (")]
    [DataRow('\''    , "v = '\\'' (")]
    [DataRow('\\'    , "v = '\\\\' (")]
    [DataRow('\u0001', "v = '\\u0001' (")]
    [DataRow('\u007F', "v = '\\u007F' (")]
    public void PrintChar(char? v, string expect) {
        Assert.AreEqual(v, Trace.Print("v", v));
        StringAssert.Contains(Trace.LastLog, expect);
    }

    // string
    [DataTestMethod]
    [DataRow(null    , "v = null (")]
    [DataRow("ABC"   , "v = \"ABC\" (")]
    [DataRow("\0"    , "v = \"\\0\" (")]
    [DataRow("\a"    , "v = \"\\a\" (")]
    [DataRow("\b"    , "v = \"\\b\" (")]
    [DataRow("\t"    , "v = \"\\t\" (")]
    [DataRow("\n"    , "v = \"\\n\" (")]
    [DataRow("\v"    , "v = \"\\v\" (")]
    [DataRow("\f"    , "v = \"\\f\" (")]
    [DataRow("\r"    , "v = \"\\r\" (")]
    [DataRow("\""    , "v = \"\\\"\" (")]
    [DataRow("'"     , "v = \"'\" (")]
    [DataRow("\\"    , "v = @\"\\\" (")]
    [DataRow("\u0001", "v = \"\\u0001\" (")]
    [DataRow("\u007F", "v = \"\\u007F\" (")]
    [DataRow("ABCDE" , "v = (Length:5)\"ABCDE\" (")]
    public void PrintString(string? v, string expect) {
        Assert.AreEqual(v, Trace.Print("v", v));
        StringAssert.Contains(Trace.LastLog, expect);
        Trace.Print("Trace.LastLog", Trace.LastLog);
    }

    // sbyte
    [DataTestMethod]
    [DataRow(null       , "v = null (")]
    [DataRow((sbyte)-128, "v = sbyte -128 (")]
    [DataRow((sbyte)  -1, "v = sbyte -1 (")]
    [DataRow((sbyte)   0, "v = sbyte 0 (")]
    [DataRow((sbyte)   1, "v = sbyte 1 (")]
    [DataRow((sbyte) 127, "v = sbyte 127 (")]
    public void PrintSByte(sbyte? v, string expect) {
        Assert.AreEqual(v, Trace.Print("v", v));
        StringAssert.Contains(Trace.LastLog, expect);
    }

    // byte
    [DataTestMethod]
    [DataRow(null     , "v = null (")]
    [DataRow((byte)  0, "v = byte 0 (")]
    [DataRow((byte)  1, "v = byte 1 (")]
    [DataRow((byte)255, "v = byte 255 (")]
    public void PrintByte(byte? v, string expect) {
        Assert.AreEqual(v, Trace.Print("v", v));
        StringAssert.Contains(Trace.LastLog, expect);
    }

    // short
    [DataTestMethod]
    [DataRow(null         , "v = null (")]
    [DataRow((short)-32768, "v = short -32768 (")]
    [DataRow((short)    -1, "v = short -1 (")]
    [DataRow((short)     0, "v = short 0 (")]
    [DataRow((short)     1, "v = short 1 (")]
    [DataRow((short) 32767, "v = short 32767 (")]
    public void PrintShort(short? v, string expect) {
        Assert.AreEqual(v, Trace.Print("v", v));
        StringAssert.Contains(Trace.LastLog, expect);
    }

    // ushort
    [DataTestMethod]
    [DataRow(null         , "v = null (")]
    [DataRow((ushort)    0, "v = ushort 0 (")]
    [DataRow((ushort)    1, "v = ushort 1 (")]
    [DataRow((ushort)65535, "v = ushort 65535 (")]
    public void PrintUShort(ushort? v, string expect) {
        Assert.AreEqual(v, Trace.Print("v", v));
        StringAssert.Contains(Trace.LastLog, expect);
    }

    // int
    [DataTestMethod]
    [DataRow(null       , "v = null (")]
    [DataRow(-2147483648, "v = -2147483648 (")]
    [DataRow(         -1, "v = -1 (")]
    [DataRow(          0, "v = 0 (")]
    [DataRow(          1, "v = 1 (")]
    [DataRow( 2147483647, "v = 2147483647 (")]
    public void PrintInt(int? v, string expect) {
        Assert.AreEqual(v, Trace.Print("v", v));
        StringAssert.Contains(Trace.LastLog, expect);
    }

    // uint
    [DataTestMethod]
    [DataRow(null       , "v = null (")]
    [DataRow(         0u, "v = 0u (")]
    [DataRow(         1u, "v = 1u (")]
    [DataRow(4294967295u, "v = 4294967295u (")]
    public void PrintUInt(uint? v, string expect) {
        Assert.AreEqual(v, Trace.Print("v", v));
        StringAssert.Contains(Trace.LastLog, expect);
    }

    // long
    [DataTestMethod]
    [DataRow(null                 , "v = null (")]
    [DataRow(-9223372036854775808L, "v = -9223372036854775808L (")]
    [DataRow(                  -1L, "v = -1L (")]
    [DataRow(                   0L, "v = 0L (")]
    [DataRow(                   1L, "v = 1L (")]
    [DataRow( 9223372036854775807L, "v = 9223372036854775807L (")]
    public void PrintLong(long? v, string expect) {
        Assert.AreEqual(v, Trace.Print("v", v));
        StringAssert.Contains(Trace.LastLog, expect);
    }

    // ulong
    [DataTestMethod]
    [DataRow(null                  , "v = null (")]
    [DataRow(                   0uL, "v = 0uL (")]
    [DataRow(                   1uL, "v = 1uL (")]
    [DataRow(18446744073709551615uL, "v = 18446744073709551615uL (")]
    public void PrintULong(ulong? v, string expect) {
        Assert.AreEqual(v, Trace.Print("v", v));
        StringAssert.Contains(Trace.LastLog, expect);
    }

    // decimal
    [DataTestMethod]
    [DataRow(null                    , "v = null (")]
    [DataRow("-9876543210.0123456789", "v = -9876543210.0123456789m (")]
    [DataRow(         "-1.0"         , "v = -1.0m (")]
    [DataRow(          "0.0"         , "v = 0.0m (")]
    [DataRow(          "1.0"         , "v = 1.0m (")]
    [DataRow(         "-1"           , "v = -1m (")]
    [DataRow(          "0"           , "v = 0m (")]
    [DataRow(          "1"           , "v = 1m (")]
    [DataRow( "9876543210.0123456789", "v = 9876543210.0123456789m (")]
    public void PrintDecimal(string? str, string expect) {
        decimal? v = str == null ? null : decimal.Parse(str);
        Trace.Print("v", v);
        StringAssert.Contains(Trace.LastLog, expect);
    }

    // float
    [DataTestMethod]
    [DataRow(null      , "v = null (")]
    [DataRow(-3210.012f, "v = -3210.012f (")]
    [DataRow( -210.012f, "v = -210.012f (")]
    [DataRow(  -10.01f , "v = -10.01f (")]
    [DataRow(   -1.0f  , "v = -1f (")]
    [DataRow(    0.0f  , "v = 0f (")]
    [DataRow(    1.0f  , "v = 1f (")]
    [DataRow(   10.01f , "v = 10.01f (")]
    [DataRow(  210.012f, "v = 210.012f (")]
    [DataRow( 3210.012f, "v = 3210.012f (")]
    public void PrintFloat(float? v, string expect) {
        Assert.AreEqual(v, Trace.Print("v", v));
        StringAssert.Contains(Trace.LastLog, expect);
    }

    // double
    [DataTestMethod]
    [DataRow(null             , "v = null (")]
    [DataRow(-76543210.0123456, "v = -76543210.0123456d (")]
    [DataRow( -6543210.0123456, "v = -6543210.0123456d (")]
    [DataRow(  -543210.012345, "v = -543210.012345d (")]
    [DataRow(   -43210.01234, "v = -43210.01234d (")]
    [DataRow(    -3210.0123, "v = -3210.0123d (")]
    [DataRow(     -210.012, "v = -210.012d (")]
    [DataRow(      -10.01 , "v = -10.01d (")]
    [DataRow(       -1.0  , "v = -1d (")]
    [DataRow(        0.0  , "v = 0d (")]
    [DataRow(        1.0  , "v = 1d (")]
    [DataRow(       10.01 , "v = 10.01d (")]
    [DataRow(      210.012, "v = 210.012d (")]
    [DataRow(     3210.0123, "v = 3210.0123d (")]
    [DataRow(    43210.01234, "v = 43210.01234d (")]
    [DataRow(   543210.012345, "v = 543210.012345d (")]
    [DataRow(  6543210.0123456, "v = 6543210.0123456d (")]
    [DataRow( 76543210.0123456, "v = 76543210.0123456d (")]
    public void PrintDouble(double? v, string expect) {
        Assert.AreEqual(v, Trace.Print("v", v));
        StringAssert.Contains(Trace.LastLog, expect);
    }

    // DateTime
    [DataTestMethod]
    [DataRow(2018, 5, 26, 12, 34, 56, 789, DateTimeKind.Local      , "v = 2018-05-26 12:34:56.7890000")]
    [DataRow(2018, 5, 26, 12, 34, 56, 789, DateTimeKind.Unspecified, "v = 2018-05-26 12:34:56.7890000 (")]
    [DataRow(2018, 5, 26, 12, 34, 56, 789, DateTimeKind.Utc        , "v = 2018-05-26 12:34:56.7890000Z (")]
    public void PrintDateTime(int year, int month, int day, int hour, int minute, int second,
            int millisecond, DateTimeKind kind, string expect) {
        var v = new DateTime(year, month, day, hour, minute, second, millisecond, kind);
        if (kind == DateTimeKind.Local) {
            var timeSpan = TimeZoneInfo.Local.GetUtcOffset(v);
            expect += String.Format(timeSpan.Ticks >= 0L ? @"+{0:hh\:mm} (" : @"-{0:hh\:mm} (", timeSpan);
        }
        Assert.AreEqual(v, Trace.Print("v", v));
        StringAssert.Contains(Trace.LastLog, expect);
    }

    // Guid
    [DataTestMethod]
    [DataRow("CE8BF46B-723B-44B4-BBF5-288B6C736127", "v = System.Guid struct ce8bf46b-723b-44b4-bbf5-288b6c736127 (")]
    public void PrintGuid(string str, string expect) {
        var v = new Guid(str);
        Assert.AreEqual(v, Trace.Print("v", v));
        StringAssert.Contains(Trace.LastLog, expect);
    }

    // Point
    [DataTestMethod]
    [DataRow(null, null, "v = null (")]
    [DataRow(1, 2, "v = DebugTraceTest.Point struct {X: 1, Y: 2} (")]
    public void PrintPoint(int? x, int? y, string expect) {
        Point? v = x == null || y == null ? null : new Point((int)x, (int)y);
        Assert.AreEqual(v, Trace.Print("v", v));
        StringAssert.Contains(Trace.LastLog, expect);
    }

    // Task since 1.4.1
    [DataTestMethod]
    [DataRow(false, false, "Result: ***")]
    [DataRow(false, true , "Result: ***")]
    [DataRow(true , false, "Result: ***")]
    [DataRow(true , true , "Result: ***")]
    public void PrintTask(bool outputNonPublicFields, bool outputNonPublicProperties, string expect) {
        var task = Task<int>.Run(() => {Thread.Sleep(400); return 1;});
        Thread.Sleep(10); // wait Running 
        Assert.AreEqual(TaskStatus.Running, task.Status);

        TraceBase.OutputNonPublicFields     = outputNonPublicFields;
        TraceBase.OutputNonPublicProperties = outputNonPublicProperties;

        Trace.Print("v", task);
        StringAssert.Contains(Trace.LastLog, expect);
        Assert.AreEqual(TaskStatus.Running, task.Status);

        Trace.Print("task.Result", task.Result);
        StringAssert.Contains(Trace.LastLog, "task.Result = 1 (");
    }

    // ValueTuple since 1.5.1
    [TestMethod]
    public void PrintValueTuple() {
        var v1 = (1, 2);
        Assert.AreEqual(v1, Trace.Print("v1", v1));
        StringAssert.Contains(Trace.LastLog, "v1 = (1, 2)");

        var v2 = ((1, 2), (3, 4));
        Assert.AreEqual(v2, Trace.Print("v2", v2));
        StringAssert.Contains(Trace.LastLog, "v2 = ((1, 2), (3, 4))");
    }

    // Tuple since 1.5.1
    [TestMethod]
     void PrintTuple() {
        var v1 = new Tuple<int, int>(1, 2);
        Assert.AreEqual(v1, Trace.Print("v1", v1));
        StringAssert.Contains(Trace.LastLog, "v1 = Tuple<int, int> (1, 2)");

        var v2 = new Tuple<Tuple<int, int>, Tuple<int, int>>(new Tuple<int, int>(1, 2), new Tuple<int, int>(3, 4));
        Assert.AreEqual(v2, Trace.Print("v2", v2));
        StringAssert.Contains(Trace.LastLog, "v2 = Tuple<Tuple<int, int>, Tuple<int, int>> (Tuple<int, int> (1, 2), Tuple<int, int> (3, 4))");
    }

    // enum since 1.5.3
    public enum Fruits {Apple, Grape, Kiwi, Orange, Pineapple};
    [DataTestMethod]
    [DataRow(Fruits.Apple, "enum DebugTraceTest.Fruits Apple")]
    [DataRow(Fruits.Grape, "enum DebugTraceTest.Fruits Grape")]
    public void PrintEnum(Fruits Fruits, string log) {
        Trace.Print("v", Fruits);
        StringAssert.Contains(Trace.LastLog, log);
    }

    public class Foo {
        public override string ToString() {return "F";}
    }
    public class FooSub : Foo {}
    public class Bar {
	    public Foo? Foo {get;}
        public Bar(Foo? foo) {
            Foo = foo;
        }
    }

    // Reflection since 1.5.4
    [TestMethod]
    public void PrintReflection() {
        Trace.Print("v", new Bar(new Foo()));
        StringAssert.Contains(Trace.LastLog, "v = DebugTraceTest.Bar {Foo: DebugTraceTest.Foo F}");

        Trace.Print("v", new Bar(new FooSub()));
        StringAssert.Contains(Trace.LastLog, "v = DebugTraceTest.Bar {DebugTraceTest.Foo Foo: DebugTraceTest.FooSub F}");

        Trace.Print("v", new Bar(null));
        StringAssert.Contains(Trace.LastLog, "v = DebugTraceTest.Bar {DebugTraceTest.Foo Foo: null}");
    }

    // PrintStack since 1.6.0
    [DataTestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    [DataRow(4)]
    [DataRow(5)]
    [DataRow(6)]
    [DataRow(7)]
    [DataRow(8)]
    [DataRow(9)]
    public void PrintStack(int maxCount) {
        func1(maxCount);
    }

    private void func1(int maxCount) {
        func2(maxCount);
    }

    private void func2(int maxCount) {
        func3(maxCount);
    }

    private void func3(int maxCount) {
        Array.ForEach(new int[] {0}, num => Trace.PrintStack(maxCount));
    }
}

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DebugTrace;
using static DebugTrace.CSharp;

namespace DebugTraceTest {
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
        [DataRow(false, "v = false (")]
        [DataRow(true , "v = true (")]
        public void PrintBool(bool v, string expect) {
            Trace_.Print("v", v);
            StringAssert.Contains(Trace_.LastLog, expect);
        }

        // char
        [DataTestMethod]
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
        public void PrintChar(char v, string expect) {
            Trace_.Print("v", v);
            StringAssert.Contains(Trace_.LastLog, expect);
        }

        // string
    // 1.5.1
    //  [DataTestMethod]
    //  [DataRow("\0"    , "v = \"\\0\" (")]
    //  [DataRow("\a"    , "v = \"\\a\" (")]
    //  [DataRow("\b"    , "v = \"\\b\" (")]
    //  [DataRow("\t"    , "v = \"\\t\" (")]
    //  [DataRow("\n"    , "v = \"\\n\" (")]
    //  [DataRow("\v"    , "v = \"\\v\" (")]
    //  [DataRow("\f"    , "v = \"\\f\" (")]
    //  [DataRow("\r"    , "v = \"\\r\" (")]
    //  [DataRow("\""    , "v = \"\\\"\" (")]
    //  [DataRow("'"     , "v = \"'\" (")]
    //  [DataRow("\\"    , "v = @\"\\\" (")]
    //  [DataRow("\u0001", "v = \"\\u0001\" (")]
    //  [DataRow("\u007F", "v = \"\\u007F\" (")]
        [DataRow("\0"    , "v = (Length:1)\"\\0\" (")]
        [DataRow("\a"    , "v = (Length:1)\"\\a\" (")]
        [DataRow("\b"    , "v = (Length:1)\"\\b\" (")]
        [DataRow("\t"    , "v = (Length:1)\"\\t\" (")]
        [DataRow("\n"    , "v = (Length:1)\"\\n\" (")]
        [DataRow("\v"    , "v = (Length:1)\"\\v\" (")]
        [DataRow("\f"    , "v = (Length:1)\"\\f\" (")]
        [DataRow("\r"    , "v = (Length:1)\"\\r\" (")]
        [DataRow("\""    , "v = (Length:1)\"\\\"\" (")]
        [DataRow("'"     , "v = (Length:1)\"'\" (")]
        [DataRow("\\"    , "v = (Length:1)@\"\\\" (")]
        [DataRow("\u0001", "v = (Length:1)\"\\u0001\" (")]
        [DataRow("\u007F", "v = (Length:1)\"\\u007F\" (")]
        public void PrintString(string v, string expect) {
            Trace_.Print("v", v);
            StringAssert.Contains(Trace_.LastLog, expect);
            Trace_.Print("Trace_.LastLog", Trace_.LastLog);
        }

        // sbyte
        [DataTestMethod]
        [DataRow((sbyte)-128, "v = sbyte -128 (")]
        [DataRow((sbyte)  -1, "v = sbyte -1 (")]
        [DataRow((sbyte)   0, "v = sbyte 0 (")]
        [DataRow((sbyte)   1, "v = sbyte 1 (")]
        [DataRow((sbyte) 127, "v = sbyte 127 (")]
        public void PrintSByte(sbyte v, string expect) {
            Trace_.Print("v", v);
            StringAssert.Contains(Trace_.LastLog, expect);
        }

        // byte
        [DataTestMethod]
        [DataRow((byte)  0, "v = byte 0 (")]
        [DataRow((byte)  1, "v = byte 1 (")]
        [DataRow((byte)255, "v = byte 255 (")]
        public void PrintByte(byte v, string expect) {
            Trace_.Print("v", v);
            StringAssert.Contains(Trace_.LastLog, expect);
        }

        // short
        [DataTestMethod]
        [DataRow((short)-32768, "v = short -32768 (")]
        [DataRow((short)    -1, "v = short -1 (")]
        [DataRow((short)     0, "v = short 0 (")]
        [DataRow((short)     1, "v = short 1 (")]
        [DataRow((short) 32767, "v = short 32767 (")]
        public void PrintShort(short v, string expect) {
            Trace_.Print("v", v);
            StringAssert.Contains(Trace_.LastLog, expect);
        }

        // ushort
        [DataTestMethod]
        [DataRow((ushort)    0, "v = ushort 0 (")]
        [DataRow((ushort)    1, "v = ushort 1 (")]
        [DataRow((ushort)65535, "v = ushort 65535 (")]
        public void PrintUShort(ushort v, string expect) {
            Trace_.Print("v", v);
            StringAssert.Contains(Trace_.LastLog, expect);
        }

        // int
        [DataTestMethod]
        [DataRow(-2147483648, "v = -2147483648 (")]
        [DataRow(         -1, "v = -1 (")]
        [DataRow(          0, "v = 0 (")]
        [DataRow(          1, "v = 1 (")]
        [DataRow( 2147483647, "v = 2147483647 (")]
        public void PrintInt(int v, string expect) {
            Trace_.Print("v", v);
            StringAssert.Contains(Trace_.LastLog, expect);
        }

        // uint
        [DataTestMethod]
        [DataRow(         0u, "v = 0u (")]
        [DataRow(         1u, "v = 1u (")]
        [DataRow(4294967295u, "v = 4294967295u (")]
        public void PrintUInt(uint v, string expect) {
            Trace_.Print("v", v);
            StringAssert.Contains(Trace_.LastLog, expect);
        }

        // long
        [DataTestMethod]
        [DataRow(-9223372036854775808L, "v = -9223372036854775808L (")]
        [DataRow(                  -1L, "v = -1L (")]
        [DataRow(                   0L, "v = 0L (")]
        [DataRow(                   1L, "v = 1L (")]
        [DataRow( 9223372036854775807L, "v = 9223372036854775807L (")]
        public void PrintLong(long v, string expect) {
            Trace_.Print("v", v);
            StringAssert.Contains(Trace_.LastLog, expect);
        }

        // ulong
        [DataTestMethod]
        [DataRow(                   0uL, "v = 0uL (")]
        [DataRow(                   1uL, "v = 1uL (")]
        [DataRow(18446744073709551615uL, "v = 18446744073709551615uL (")]
        public void PrintULong(ulong v, string expect) {
            Trace_.Print("v", v);
            StringAssert.Contains(Trace_.LastLog, expect);
        }

        // decimal
        [DataTestMethod]
        [DataRow("-9876543210.0123456789", "v = -9876543210.0123456789m (")]
        [DataRow(         "-1.0"         , "v = -1.0m (")]
        [DataRow(          "0.0"         , "v = 0.0m (")]
        [DataRow(          "1.0"         , "v = 1.0m (")]
        [DataRow(         "-1"           , "v = -1m (")]
        [DataRow(          "0"           , "v = 0m (")]
        [DataRow(          "1"           , "v = 1m (")]
        [DataRow( "9876543210.0123456789", "v = 9876543210.0123456789m (")]
        public void PrintDecimal(string v, string expect) {
            Trace_.Print("v", decimal.Parse(v));
            StringAssert.Contains(Trace_.LastLog, expect);
        }

        // float
        [DataTestMethod]
        [DataRow(-3210.012f, "v = -3210.012f (")]
        [DataRow( -210.012f, "v = -210.012f (")]
        [DataRow(  -10.01f , "v = -10.01f (")]
        [DataRow(   -1.0f  , "v = -1f (")]
        [DataRow(    0.0f  , "v = 0f (")]
        [DataRow(    1.0f  , "v = 1f (")]
        [DataRow(   10.01f , "v = 10.01f (")]
        [DataRow(  210.012f, "v = 210.012f (")]
        [DataRow( 3210.012f, "v = 3210.012f (")]
        public void PrintFloat(float v, string expect) {
            Trace_.Print("v", v);
            StringAssert.Contains(Trace_.LastLog, expect);
        }

        // double
        [DataTestMethod]
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
        public void PrintDouble(double v, string expect) {
            Trace_.Print("v", v);
            StringAssert.Contains(Trace_.LastLog, expect);
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
            Trace_.Print("v", v);
            StringAssert.Contains(Trace_.LastLog, expect);
        }

        // Guid
        [DataTestMethod]
    // 1.5.3
    //  [DataRow("CE8BF46B-723B-44B4-BBF5-288B6C736127", "v = System.Guid ce8bf46b-723b-44b4-bbf5-288b6c736127 (")]
        [DataRow("CE8BF46B-723B-44B4-BBF5-288B6C736127", "v = System.Guid struct ce8bf46b-723b-44b4-bbf5-288b6c736127 (")]
    ////
        public void PrintGuid(string v, string expect) {
            Trace_.Print("v", new Guid(v));
            StringAssert.Contains(Trace_.LastLog, expect);
        }

        // Point
        [DataTestMethod]
    // 1.5.3
    //  [DataRow(1, 2, "v = DebugTraceTest.Point {X: 1, Y: 2} (")]
        [DataRow(1, 2, "v = DebugTraceTest.Point struct {X: 1, Y: 2} (")]
    ////
        public void PrintPoint(int x, int y, string expect) {
            Trace_.Print("v", new Point(x, y));
            StringAssert.Contains(Trace_.LastLog, expect);
        }

        // Task since 1.4.1
        [DataTestMethod]
        [DataRow(false, false, "Result: ***")]
        [DataRow(false, true , "Result: ***")]
        [DataRow(true , false, "Result: ***")]
        [DataRow(true , true , "Result: ***")]
        public void PrintTask(bool outputNonPublicFields, bool outputNonPublicProperties, string expect) {
            var task = Task<int>.Run(() => {Thread.Sleep(200); return 1;});
            Thread.Sleep(10); // wait Running 
            Assert.AreEqual(TaskStatus.Running, task.Status);

            TraceBase.OutputNonPublicFields     = outputNonPublicFields;
            TraceBase.OutputNonPublicProperties = outputNonPublicProperties;

            Trace_.Print("v", task);
            StringAssert.Contains(Trace_.LastLog, expect);
            Assert.AreEqual(TaskStatus.Running, task.Status);

            Trace_.Print("task.Result", task.Result);
            StringAssert.Contains(Trace_.LastLog, "task.Result = 1 (");
        }

        // ValueTuple since 1.5.1
        [TestMethod]
        public void PrintValueTuple() {
            Trace_.Print("v", (1, 2));
            StringAssert.Contains(Trace_.LastLog, "v = (1, 2)");

            Trace_.Print("v", ((1, 2), (3, 4)));
            StringAssert.Contains(Trace_.LastLog, "v = ((1, 2), (3, 4))");
        }

        // Tuple since 1.5.1
        [TestMethod]
        public void PrintTuple() {
            Trace_.Print("v", new Tuple<int, int>(1, 2));
            StringAssert.Contains(Trace_.LastLog, "v = Tuple<int, int> (1, 2)");

            Trace_.Print("v", new Tuple<Tuple<int, int>, Tuple<int, int>>(new Tuple<int, int>(1, 2), new Tuple<int, int>(3, 4)));
            StringAssert.Contains(Trace_.LastLog, "v = Tuple<Tuple<int, int>, Tuple<int, int>> (Tuple<int, int> (1, 2), Tuple<int, int> (3, 4))");
        }

        // enum 1.5.3
        public enum Fruits {Apple, Grape, Kiwi, Orange, Pineapple};
        [DataTestMethod]
        [DataRow(Fruits.Apple, "Fruits enum Apple")]
        [DataRow(Fruits.Grape, "Fruits enum Grape")]
        public void PrintEnum(Fruits Fruits, string log) {
            Trace_.Print("v", Fruits);
            StringAssert.Contains(Trace_.LastLog, log);
        }
    }
}

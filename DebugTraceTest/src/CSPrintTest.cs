using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DebugTrace.CSharp;

namespace DebugTraceTest {
	[TestClass]
	public class CSPrintTest {
		// bool
		[DataTestMethod]
		[DataRow(false, "v = false ")]
		[DataRow(true , "v = true ")]
		public void CSPrintBool(bool v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.StartsWith(expect));
		}

		// char
		[DataTestMethod]
		[DataRow('\0'    , "v = '\\0' ")]
		[DataRow('\a'    , "v = '\\a' ")]
		[DataRow('\b'    , "v = '\\b' ")]
		[DataRow('\t'    , "v = '\\t' ")]
		[DataRow('\n'    , "v = '\\n' ")]
		[DataRow('\v'    , "v = '\\v' ")]
		[DataRow('\f'    , "v = '\\f' ")]
		[DataRow('\r'    , "v = '\\r' ")]
		[DataRow('"'     , "v = '\"' ")]
		[DataRow('\''    , "v = '\\'' ")]
		[DataRow('\\'    , "v = '\\\\' ")]
		[DataRow('\u0001', "v = '\\u0001' ")]
		[DataRow('\u007F', "v = '\\u007F' ")]
		public void CSPrintChar(char v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.StartsWith(expect));
		}

		// string
		[DataTestMethod]
		[DataRow("\0"    , "v = \"\\0\" ")]
		[DataRow("\a"    , "v = \"\\a\" ")]
		[DataRow("\b"    , "v = \"\\b\" ")]
		[DataRow("\t"    , "v = \"\\t\" ")]
		[DataRow("\n"    , "v = \"\\n\" ")]
		[DataRow("\v"    , "v = \"\\v\" ")]
		[DataRow("\f"    , "v = \"\\f\" ")]
		[DataRow("\r"    , "v = \"\\r\" ")]
		[DataRow("\""    , "v = \"\\\"\" ")]
		[DataRow("'"     , "v = \"'\" ")]
		[DataRow("\\"    , "v = @\"\\\" ")]
		[DataRow("\u0001", "v = \"\\u0001\" ")]
		[DataRow("\u007F", "v = \"\\u007F\" ")]
		public void CSPrintString(string v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.StartsWith(expect));
			Trace.Print("Trace.LastLog", Trace.LastLog);
		}

		// sbyte
		[DataTestMethod]
		[DataRow((sbyte)-128, "v = sbyte -128 ")]
		[DataRow((sbyte)  -1, "v = sbyte -1 ")]
		[DataRow((sbyte)   0, "v = sbyte 0 ")]
		[DataRow((sbyte)   1, "v = sbyte 1 ")]
		[DataRow((sbyte) 127, "v = sbyte 127 ")]
		public void CSPrintSByte(sbyte v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.StartsWith(expect));
		}

		// byte
		[DataTestMethod]
		[DataRow((byte)  0, "v = byte 0 ")]
		[DataRow((byte)  1, "v = byte 1 ")]
		[DataRow((byte)255, "v = byte 255 ")]
		public void CSPrintByte(byte v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.StartsWith(expect));
		}

		// short
		[DataTestMethod]
		[DataRow((short)-32768, "v = short -32768 ")]
		[DataRow((short)    -1, "v = short -1 ")]
		[DataRow((short)     0, "v = short 0 ")]
		[DataRow((short)     1, "v = short 1 ")]
		[DataRow((short) 32767, "v = short 32767 ")]
		public void CSPrintShort(short v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.StartsWith(expect));
		}

		// ushort
		[DataTestMethod]
		[DataRow((ushort)    0, "v = ushort 0 ")]
		[DataRow((ushort)    1, "v = ushort 1 ")]
		[DataRow((ushort)65535, "v = ushort 65535 ")]
		public void CSPrintUShort(ushort v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.StartsWith(expect));
		}

		// int
		[DataTestMethod]
		[DataRow(-2147483648, "v = -2147483648 ")]
		[DataRow(         -1, "v = -1 ")]
		[DataRow(          0, "v = 0 ")]
		[DataRow(          1, "v = 1 ")]
		[DataRow( 2147483647, "v = 2147483647 ")]
		public void CSPrintInt(int v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.StartsWith(expect));
		}

		// uint
		[DataTestMethod]
		[DataRow(         0u, "v = 0u ")]
		[DataRow(         1u, "v = 1u ")]
		[DataRow(4294967295u, "v = 4294967295u ")]
		public void CSPrintUInt(uint v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.StartsWith(expect));
		}

		// long
		[DataTestMethod]
		[DataRow(-9223372036854775808L, "v = -9223372036854775808L ")]
		[DataRow(                  -1L, "v = -1L ")]
		[DataRow(                   0L, "v = 0L ")]
		[DataRow(                   1L, "v = 1L ")]
		[DataRow( 9223372036854775807L, "v = 9223372036854775807L ")]
		public void CSPrintLong(long v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.StartsWith(expect));
		}

		// ulong
		[DataTestMethod]
		[DataRow(                   0uL, "v = 0uL ")]
		[DataRow(                   1uL, "v = 1uL ")]
		[DataRow(18446744073709551615uL, "v = 18446744073709551615uL ")]
		public void CSPrintULong(ulong v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.StartsWith(expect));
		}

		// decimal
		[DataTestMethod]
		[DataRow("-9876543210.0123456789", "v = -9876543210.0123456789m ")]
		[DataRow(         "-1.0"         , "v = -1.0m ")]
		[DataRow(          "0.0"         , "v = 0.0m ")]
		[DataRow(          "1.0"         , "v = 1.0m ")]
		[DataRow(         "-1"           , "v = -1m ")]
		[DataRow(          "0"           , "v = 0m ")]
		[DataRow(          "1"           , "v = 1m ")]
		[DataRow( "9876543210.0123456789", "v = 9876543210.0123456789m ")]
		public void CSPrintDecimal(string v, string expect) {
			Trace.Print("v", decimal.Parse(v));
			Assert.IsTrue(Trace.LastLog.StartsWith(expect));
		}

		// float
		[DataTestMethod]
		[DataRow(-3210.012f, "v = -3210.012f ")]
		[DataRow( -210.012f, "v = -210.012f ")]
		[DataRow(  -10.01f , "v = -10.01f ")]
		[DataRow(   -1.0f  , "v = -1f ")]
		[DataRow(    0.0f  , "v = 0f ")]
		[DataRow(    1.0f  , "v = 1f ")]
		[DataRow(   10.01f , "v = 10.01f ")]
		[DataRow(  210.012f, "v = 210.012f ")]
		[DataRow( 3210.012f, "v = 3210.012f ")]
		public void CSPrintFloat(float v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.StartsWith(expect));
		}

		// double
		[DataTestMethod]
		[DataRow(-76543210.0123456, "v = -76543210.0123456d ")]
		[DataRow( -6543210.0123456, "v = -6543210.0123456d ")]
		[DataRow(  -543210.012345, "v = -543210.012345d ")]
		[DataRow(   -43210.01234, "v = -43210.01234d ")]
		[DataRow(    -3210.0123, "v = -3210.0123d ")]
		[DataRow(     -210.012, "v = -210.012d ")]
		[DataRow(      -10.01 , "v = -10.01d ")]
		[DataRow(       -1.0  , "v = -1d ")]
		[DataRow(        0.0  , "v = 0d ")]
		[DataRow(        1.0  , "v = 1d ")]
		[DataRow(       10.01 , "v = 10.01d ")]
		[DataRow(      210.012, "v = 210.012d ")]
		[DataRow(     3210.0123, "v = 3210.0123d ")]
		[DataRow(    43210.01234, "v = 43210.01234d ")]
		[DataRow(   543210.012345, "v = 543210.012345d ")]
		[DataRow(  6543210.0123456, "v = 6543210.0123456d ")]
		[DataRow( 76543210.0123456, "v = 76543210.0123456d ")]
		public void CSPrintDouble(double v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.StartsWith(expect));
		}

	}
}

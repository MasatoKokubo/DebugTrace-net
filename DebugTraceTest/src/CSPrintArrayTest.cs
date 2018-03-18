using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DebugTrace.CSharp;

namespace DebugTraceTest {
	[TestClass]
	public class CSPrintArrayTest {
		// bool[]
		[DataTestMethod]
		[DataRow(new bool[] {}, "v = bool[0] {} ")]
		[DataRow(new bool[] {false}, "v = bool[1] {false} ")]
		[DataRow(new bool[] {false, true}, "v = bool[2] {false, true} ")]
		public void CSPrintBoolArray(bool[] v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.IndexOf(expect) >= 0);
		}

		// char[]
		[DataTestMethod]
		[DataRow(new char[] {}, "v = char[0] {}")]
		[DataRow(new char[] {'A'}, "v = char[1] {'A'} ")]
		[DataRow(new char[] {'A', 'B'}, "v = char[2] {'A', 'B'} ")]
		public void CSPrintCharArray(char[] v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.IndexOf(expect) >= 0);
		}

		// string[]
		[DataTestMethod]
		[DataRow(new string[] {}, "v = string[0] {}")]
		[DataRow(new string[] {"A"}, "v = string[1] {\"A\"}")]
		[DataRow(new string[] {"A", "B"}, "v = string[2] {\"A\", \"B\"}")]
		public void CSPrintStringArray(string[] v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.IndexOf(expect) >= 0);
			Trace.Print("Trace.LastLog", Trace.LastLog);
		}

		// sbyte[]
		[DataTestMethod]
		[DataRow(new sbyte[] {}, "v = sbyte[0] {} ")]
		[DataRow(new sbyte[] {-128}, "v = sbyte[1] {-128} ")]
		[DataRow(new sbyte[] {-128, 127}, "v = sbyte[2] {-128, 127} ")]
		public void CSPrintSByteArray(sbyte[] v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.IndexOf(expect) >= 0);
		}

		// byte[]
		[DataTestMethod]
		[DataRow(new byte[] {}, "v = byte[0] {} ")]
		[DataRow(new byte[] {0}, "v = byte[1] {0} ")]
		[DataRow(new byte[] {0, 255}, "v = byte[2] {0, 255} ")]
		public void CSPrintByteArray(byte[] v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.IndexOf(expect) >= 0);
		}

		// short[]
		[DataTestMethod]
		[DataRow(new short[] {}, "v = short[0] {} ")]
		[DataRow(new short[] {-32768}, "v = short[1] {-32768} ")]
		[DataRow(new short[] {-32768, 32767}, "v = short[2] {-32768, 32767} ")]
		public void CSPrintShortArray(short[] v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.IndexOf(expect) >= 0);
		}

		// ushort[]
		[DataTestMethod]
		[DataRow(new ushort[] {}, "v = ushort[0] {} ")]
		[DataRow(new ushort[] {0}, "v = ushort[1] {0} ")]
		[DataRow(new ushort[] {0, 65535}, "v = ushort[2] {0, 65535} ")]
		public void CSPrintUShortArray(ushort[] v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.IndexOf(expect) >= 0);
		}

		// int[]
		[DataTestMethod]
		[DataRow(new int[] {}, "v = int[0] {} ")]
		[DataRow(new int[] {-2147483648}, "v = int[1] {-2147483648} ")]
		[DataRow(new int[] {-2147483648, 2147483647}, "v = int[2] {-2147483648, 2147483647} ")]
		public void CSPrintIntArray(int[] v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.IndexOf(expect) >= 0);
		}

		// uint[]
		[DataTestMethod]
		[DataRow(new uint[] {}, "v = uint[0] {} ")]
		[DataRow(new uint[] {0}, "v = uint[1] {0u} ")]
		[DataRow(new uint[] {0, 4294967295}, "v = uint[2] {0u, 4294967295u} ")]
		public void CSPrintUIntArray(uint[] v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.IndexOf(expect) >= 0);
		}

		// long[]
		[DataTestMethod]
		[DataRow(new long[] {}, "v = long[0] {} ")]
		[DataRow(new long[] {-9223372036854775808L}, "v = long[1] {-9223372036854775808L} ")]
		[DataRow(new long[] {-9223372036854775808L, 9223372036854775807L}, "v = long[2] {-9223372036854775808L, 9223372036854775807L} ")]
		public void CSPrintLongArray(long[] v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.IndexOf(expect) >= 0);
		}

		// ulong[]
		[DataTestMethod]
		[DataRow(new ulong[] {}, "v = ulong[0] {} ")]
		[DataRow(new ulong[] {0}, "v = ulong[1] {0uL} ")]
		[DataRow(new ulong[] {0, 18446744073709551615uL}, "v = ulong[2] {0uL, 18446744073709551615uL} ")]
		public void CSPrintULongArray(ulong[] v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.IndexOf(expect) >= 0);
		}

		// decimal[]
		[DataTestMethod]
		[DataRow(new string[] {}, "v = decimal[0] {} ")]
		[DataRow(new string[] {"0"}, "v = decimal[1] {0m} ")]
		[DataRow(new string[] {"0", "9876543210.0123456789"}, "v = decimal[2] {0m, 9876543210.0123456789m} ")]
		public void CSPrintDecimalArray(string[] v, string expect) {
			Trace.Print("v", v.Select(e => decimal.Parse(e)).ToArray());
			Assert.IsTrue(Trace.LastLog.IndexOf(expect) >= 0);
		}

		// float[]
		[DataTestMethod]
		[DataRow(new float[] {}, "v = float[0] {} ")]
		[DataRow(new float[] {0f}, "v = float[1] {0f} ")]
		[DataRow(new float[] {0f, 3210.012f}, "v = float[2] {0f, 3210.012f} ")]
		public void CSPrintFloatArray(float[] v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.IndexOf(expect) >= 0);
		}

		// double[]
		[DataRow(new double[] {}, "v = float[0] {} ")]
		[DataRow(new double[] {0}, "v = float[1] {0d} ")]
		[DataRow(new double[] {0, 76543210.0123456}, "v = double[2] {0d, 76543210.0123456d} ")]
		public void CSPrintDoubleArray(double[] v, string expect) {
			Trace.Print("v", v);
			Assert.IsTrue(Trace.LastLog.IndexOf(expect) >= 0);
		}

	}
}

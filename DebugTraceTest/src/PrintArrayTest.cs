using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DebugTrace.CSharp;

namespace DebugTraceTest {
    [TestClass]
    public class PrintArrayTest {
        private static int maxDataOutputWidth;

        [ClassInitialize]
        public static void ClassInit(TestContext context) {
            maxDataOutputWidth = DebugTrace.TraceBase.MaxDataOutputWidth;
            DebugTrace.TraceBase.MaxDataOutputWidth = int.MaxValue;
        }

        [ClassCleanup]
        public static void ClassCleanup() {
            DebugTrace.TraceBase.MaxDataOutputWidth = maxDataOutputWidth;
        }

        private class HasIndexer<T> {
            private T[] array = new T[100];
            public T this[int index] {
               get {return array[index];}
               set {array[index] = value;}
            }
        }

        // bool[]
        [DataTestMethod]
        [DataRow(new bool[] {}, "v = bool[0] {} (")]
        [DataRow(new bool[] {false}, "v = bool[1] {false} (")]
        [DataRow(new bool[] {false, true}, "v = bool[2] {false, true} (")]
        public void PrintBoolArray(bool[] v, string expect) {
            Trace.Print("v", v);
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // bool[][]
        [DataTestMethod]
        [DataRow(new [] {false}, new [] {true, false}, new [] {true, false, true},
            "v = bool[3][] {bool[1] {false}, bool[2] {true, false}, bool[3] {true, false, true}} (")]
        public void PrintBoolArray2(bool[] e0, bool[] e1, bool[] e2, string expect) {
            var v = new [] {e0, e1, e2};
            Trace.Print("v", v);
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // char[]
        [DataTestMethod]
        [DataRow(new char[] {}, "v = char[0] {}")]
        [DataRow(new char[] {'A'}, "v = char[1] {'A'} (")]
        [DataRow(new char[] {'A', 'B'}, "v = char[2] {'A', 'B'} (")]
        public void PrintCharArray(char[] v, string expect) {
            Trace.Print("v", v);
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // char[][]
        [DataTestMethod]
        [DataRow(new [] {'A'}, new [] {'B', 'C'}, new [] {'D', 'E', 'F'},
            "v = char[3][] {char[1] {'A'}, char[2] {'B', 'C'}, char[3] {'D', 'E', 'F'}} (")]
        public void PrintCharArray2(char[] e0, char[] e1, char[] e2, string expect) {
            var v = new [] {e0, e1, e2};
            Trace.Print("v", v);
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // string[]
        [DataTestMethod]
        [DataRow(new string[] {}, "v = string[0] {}")]
//      [DataRow(new string[] {"A"}, "v = string[1] {\"A\"}")]
        [DataRow(new string[] {"A"}, "v = string[1] {(Length:1)\"A\"}")] // 1.5.1
//      [DataRow(new string[] {"A", "B"}, "v = string[2] {\"A\", \"B\"}")]
        [DataRow(new string[] {"A", "B"}, "v = string[2] {(Length:1)\"A\", (Length:1)\"B\"}")] // 1.5.1
        public void PrintStringArray(string[] v, string expect) {
            Trace.Print("v", v);
            StringAssert.Contains(Trace.LastLog, expect);
            Trace.Print("Trace.LastLog", Trace.LastLog);
        }

        // sbyte[]
        [DataTestMethod]
        [DataRow(new sbyte[] {}, "v = sbyte[0] {} (")]
        [DataRow(new sbyte[] {-128}, "v = sbyte[1] {-128} (")]
        [DataRow(new sbyte[] {-128, 127}, "v = sbyte[2] {-128, 127} (")]
        public void PrintSByteArray(sbyte[] v, string expect) {
            Trace.Print("v", v);
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // byte[]
        [DataTestMethod]
        [DataRow(new byte[] {}, "v = byte[0] {} (")]
        [DataRow(new byte[] {0}, "v = byte[1] {0} (")]
        [DataRow(new byte[] {0, 255}, "v = byte[2] {0, 255} (")]
        public void PrintByteArray(byte[] v, string expect) {
            Trace.Print("v", v);
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // short[]
        [DataTestMethod]
        [DataRow(new short[] {}, "v = short[0] {} (")]
        [DataRow(new short[] {-32768}, "v = short[1] {-32768} (")]
        [DataRow(new short[] {-32768, 32767}, "v = short[2] {-32768, 32767} (")]
        public void PrintShortArray(short[] v, string expect) {
            Trace.Print("v", v);
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // ushort[]
        [DataTestMethod]
        [DataRow(new ushort[] {}, "v = ushort[0] {} (")]
        [DataRow(new ushort[] {0}, "v = ushort[1] {0} (")]
        [DataRow(new ushort[] {0, 65535}, "v = ushort[2] {0, 65535} (")]
        public void PrintUShortArray(ushort[] v, string expect) {
            Trace.Print("v", v);
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // int[]
        [DataTestMethod]
        [DataRow(new int[] {}, "v = int[0] {} (")]
        [DataRow(new int[] {-2147483648}, "v = int[1] {-2147483648} (")]
        [DataRow(new int[] {-2147483648, 2147483647}, "v = int[2] {-2147483648, 2147483647} (")]
        public void PrintIntArray(int[] v, string expect) {
            Trace.Print("v", v);
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // int[][]
        [DataTestMethod]
        [DataRow(new [] {10}, new [] {11, 12}, new [] {13, 14, 15},
            "v = int[3][] {int[1] {10}, int[2] {11, 12}, int[3] {13, 14, 15}} (")]
        public void PrintIntArray2(int[] e0, int[] e1, int[] e2, string expect) {
            var v = new [] {e0, e1, e2};
            Trace.Print("v", v);
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // int[][][]
        [DataTestMethod]
        [DataRow(
            new [] {10}, new[] {11, 12}, new[] {13, 14, 15},
            new [] {20}, new[] {21, 22}, new[] {23, 24, 25},
            "v = int[2][][] {int[3][] {int[1] {10}, int[2] {11, 12}, int[3] {13, 14, 15}}, int[3][] {int[1] {20}, int[2] {21, 22}, int[3] {23, 24, 25}}} (")]
        public void PrintIntArray3(int[] e00, int[] e01, int[] e02, int[] e10, int[] e11, int[] e12, string expect) {
            var v = new[] {new[] {e00, e01, e02}, new[] {e10, e11, e12}};
            Trace.Print("v", v);
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // uint[]
        [DataTestMethod]
        [DataRow(new uint[] {}, "v = uint[0] {} (")]
        [DataRow(new uint[] {0}, "v = uint[1] {0u} (")]
        [DataRow(new uint[] {0, 4294967295}, "v = uint[2] {0u, 4294967295u} (")]
        public void PrintUIntArray(uint[] v, string expect) {
            Trace.Print("v", v);
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // long[]
        [DataTestMethod]
        [DataRow(new long[] {}, "v = long[0] {} (")]
        [DataRow(new long[] {-9223372036854775808L}, "v = long[1] {-9223372036854775808L} (")]
        [DataRow(new long[] {-9223372036854775808L, 9223372036854775807L}, "v = long[2] {-9223372036854775808L, 9223372036854775807L} (")]
        public void PrintLongArray(long[] v, string expect) {
            Trace.Print("v", v);
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // ulong[]
        [DataTestMethod]
        [DataRow(new ulong[] {}, "v = ulong[0] {} (")]
        [DataRow(new ulong[] {0}, "v = ulong[1] {0uL} (")]
        [DataRow(new ulong[] {0, 18446744073709551615uL}, "v = ulong[2] {0uL, 18446744073709551615uL} (")]
        public void PrintULongArray(ulong[] v, string expect) {
            Trace.Print("v", v);
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // decimal[]
        [DataTestMethod]
        [DataRow(new string[] {}, "v = decimal[0] {} (")]
        [DataRow(new string[] {"0"}, "v = decimal[1] {0m} (")]
        [DataRow(new string[] {"0", "9876543210.0123456789"}, "v = decimal[2] {0m, 9876543210.0123456789m} (")]
        public void PrintDecimalArray(string[] v, string expect) {
            Trace.Print("v", v.Select(e => decimal.Parse(e)).ToArray());
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // float[]
        [DataTestMethod]
        [DataRow(new float[] {}, "v = float[0] {} (")]
        [DataRow(new float[] {0f}, "v = float[1] {0f} (")]
        [DataRow(new float[] {0f, 3210.012f}, "v = float[2] {0f, 3210.012f} (")]
        public void PrintFloatArray(float[] v, string expect) {
            Trace.Print("v", v);
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // double[]
        [DataTestMethod]
        [DataRow(new double[] {}, "v = double[0] {} (")]
        [DataRow(new double[] {0}, "v = double[1] {0d} (")]
        [DataRow(new double[] {0, 76543210.0123456}, "v = double[2] {0d, 76543210.0123456d} (")]
        public void PrintDoubleArray(double[] v, string expect) {
            Trace.Print("v", v);
            StringAssert.Contains(Trace.LastLog, expect);
        }

        // HasIndexer<int>
        [DataTestMethod]
        [DataRow(new int[] {-1, 0, 1}, "v = DebugTraceTest.HasIndexer<int> {} (")]
        public void PrintHasIndexer(int[] v, string expect) {
            var hasIndexer = new HasIndexer<int>();
            for (var index = 0; index < v.Length; ++index)
                hasIndexer[index] = v[index];
            Trace.Print("v", hasIndexer);
            StringAssert.Contains(Trace.LastLog, expect);
        }

    }
}

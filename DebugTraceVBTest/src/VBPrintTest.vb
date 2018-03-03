Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports DebugTrace.VB

Namespace DebugTraceTest
	<TestClass()>
	Public Class VBPrintTest
		' Boolean
		<DataTestMethod()>
		<DataRow(False, "v = False ")>
		<DataRow(True, "v = True ")>
		Public Sub VBPrintBoolean(v As Boolean, expect As String)
			Trace.Print("v", v)
			Assert.IsTrue(Trace.LastLog.StartsWith(expect))
		End Sub

		' SByte
		<DataTestMethod()>
		<DataRow(CSByte(-128), "v = SByte -128 ")>
		<DataRow(CSByte(  -1), "v = SByte -1 ")>
		<DataRow(CSByte(   0), "v = SByte 0 ")>
		<DataRow(CSByte(   1), "v = SByte 1 ")>
		<DataRow(CSByte( 127), "v = SByte 127 ")>
		Public Sub VBPrintSByte(v As SByte, expect As String)
			Trace.Print("v", v)
			Assert.IsTrue(Trace.LastLog.StartsWith(expect))
		End Sub

		' Byte
		<DataTestMethod()>
		<DataRow(CByte(  0), "v = Byte 0 ")>
		<DataRow(CByte(  1), "v = Byte 1 ")>
		<DataRow(CByte(255), "v = Byte 255 ")>
		Public Sub VBPrintByte(v As Byte, expect As String)
			Trace.Print("v", v)
			Assert.IsTrue(Trace.LastLog.StartsWith(expect))
		End Sub

		' Short
		<DataTestMethod()>
		<DataRow(-32767S, "v = -32767S ")>
		<DataRow(    -1S, "v = -1S ")>
		<DataRow(     0S, "v = 0S ")>
		<DataRow(     1S, "v = 1S ")>
		<DataRow( 32767S, "v = 32767S ")>
		Public Sub VBPrintShort(v As Short, expect As String)
			Trace.Print("v", v)
			Assert.IsTrue(Trace.LastLog.StartsWith(expect))
		End Sub

		' UShort
		<DataTestMethod()>
		<DataRow(    0US, "v = 0US ")>
		<DataRow(    1US, "v = 1US ")>
		<DataRow(65535US, "v = 65535US ")>
		Public Sub VBPrintUShort(v As UShort, expect As String)
			Trace.Print("v", v)
			Assert.IsTrue(Trace.LastLog.StartsWith(expect))
		End Sub

		' Integer
		<DataTestMethod()>
		<DataRow(-2147483647, "v = -2147483647 ")>
		<DataRow(         -1, "v = -1 ")>
		<DataRow(          0, "v = 0 ")>
		<DataRow(          1, "v = 1 ")>
		<DataRow(2147483647, "v = 2147483647 ")>
		Public Sub VBPrintInteger(v As Integer, expect As String)
			Trace.Print("v", v)
			Assert.IsTrue(Trace.LastLog.StartsWith(expect))
		End Sub

		' UInteger
		<DataTestMethod()>
		<DataRow(         0UI, "v = 0U ")>
		<DataRow(         1UI, "v = 1U ")>
		<DataRow(4294967295UI, "v = 4294967295U ")>
		Public Sub VBPrintUInteger(v As UInteger, expect As String)
			Trace.Print("v", v)
			Assert.IsTrue(Trace.LastLog.StartsWith(expect))
		End Sub

		' Long
		<DataTestMethod()>
		<DataRow(-9223372036854775807L, "v = -9223372036854775807L ")>
		<DataRow(                  -1L, "v = -1L ")>
		<DataRow(                   0L, "v = 0L ")>
		<DataRow(                   1L, "v = 1L ")>
		<DataRow( 9223372036854775807L, "v = 9223372036854775807L ")>
		Public Sub VBPrintLong(v As Long, expect As String)
			Trace.Print("v", v)
			Assert.IsTrue(Trace.LastLog.StartsWith(expect))
		End Sub

		' ULong
		<DataTestMethod()>
		<DataRow(                   0UL, "v = 0UL ")>
		<DataRow(                   1UL, "v = 1UL ")>
		<DataRow(18446744073709551615UL, "v = 18446744073709551615UL ")>
		Public Sub VBPrintULong(v As ULong, expect As String)
			Trace.Print("v", v)
			Assert.IsTrue(Trace.LastLog.StartsWith(expect))
		End Sub

		' decimal
		<DataTestMethod()>
		<DataRow("-9876543210.0123456789", "v = -9876543210.0123456789D ")>
		<DataRow(         "-1.0"         , "v = -1.0D ")>
		<DataRow(          "0.0"         , "v = 0.0D ")>
		<DataRow(          "1.0"         , "v = 1.0D ")>
		<DataRow(         "-1"           , "v = -1D ")>
		<DataRow(          "0"           , "v = 0D ")>
		<DataRow(          "1"           , "v = 1D ")>
		<DataRow( "9876543210.0123456789", "v = 9876543210.0123456789D ")>
		Public Sub VBPrintDecimal(v As String, expect As String)
			Trace.Print("v", decimal.Parse(v))
			Assert.IsTrue(Trace.LastLog.StartsWith(expect))
		End Sub

		' Single
		<DataTestMethod()>
		<DataRow(-3210.012F, "v = -3210.012F ")>
		<DataRow( -210.012F, "v = -210.012F ")>
		<DataRow(  -10.01F , "v = -10.01F ")>
		<DataRow(   -1.0F  , "v = -1F ")>
		<DataRow(    0.0F  , "v = 0F ")>
		<DataRow(    1.0F  , "v = 1F ")>
		<DataRow(   10.01F , "v = 10.01F ")>
		<DataRow(  210.012F, "v = 210.012F ")>
		<DataRow( 3210.012F, "v = 3210.012F ")>
		Public Sub VBPrintSingle(v As Single, expect As String)
			Trace.Print("v", v)
			Assert.IsTrue(Trace.LastLog.StartsWith(expect))
		End Sub

		' Double
		<DataTestMethod()>
		<DataRow(-76543210.0123456, "v = -76543210.0123456 ")>
		<DataRow( -6543210.0123456, "v = -6543210.0123456 ")>
		<DataRow(  -543210.012345, "v = -543210.012345 ")>
		<DataRow(   -43210.01234, "v = -43210.01234 ")>
		<DataRow(    -3210.0123, "v = -3210.0123 ")>
		<DataRow(     -210.012, "v = -210.012 ")>
		<DataRow(      -10.01 , "v = -10.01 ")>
		<DataRow(       -1.0  , "v = -1 ")>
		<DataRow(        0.0  , "v = 0 ")>
		<DataRow(        1.0  , "v = 1 ")>
		<DataRow(       10.01 , "v = 10.01 ")>
		<DataRow(      210.012, "v = 210.012 ")>
		<DataRow(     3210.0123, "v = 3210.0123 ")>
		<DataRow(    43210.01234, "v = 43210.01234 ")>
		<DataRow(   543210.012345, "v = 543210.012345 ")>
		<DataRow(  6543210.0123456, "v = 6543210.0123456 ")>
		<DataRow( 76543210.0123456, "v = 76543210.0123456 ")>
		Public Sub VBPrintDouble(v As Double, expect As String)
			Trace.Print("v", v)
			Assert.IsTrue(Trace.LastLog.StartsWith(expect))
		End Sub

	End Class
End Namespace

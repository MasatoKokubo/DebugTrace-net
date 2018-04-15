Imports System
Imports DebugTrace.VisualBasic

Namespace DebugTraceExample
	Module VBExample
		Public Sub Main(args As String())
			Trace.Enter()
			Sub1()
			Trace.Leave()
		End Sub

		Public Sub Sub1()
			Trace.Enter()
			Sub2()
			Trace.Leave()
		End Sub

		Public Sub Sub2()
			Trace.Enter()
			Sub3()
			Trace.Leave()
		End Sub

		Public Sub Sub3()
			Trace.Enter()
			With Nothing : Dim value As String = Nothing : Trace.Print("value", value) : End With
			With Nothing : Dim value As Boolean = True : Trace.Print("value", value) : End With
			With Nothing : Dim value As SByte = -1 : Trace.Print("value", value) : End With
			With Nothing : Dim value As Byte = 1 : Trace.Print("value", value) : End With
			With Nothing : Dim value As Short = 1S : Trace.Print("value", value) : End With
			With Nothing : Dim value As UShort = 1US : Trace.Print("value", value) : End With
			With Nothing : Dim value As Integer = 1 : Trace.Print("value", value) : End With
			With Nothing : Dim value As UInteger = 1UI : Trace.Print("value", value) : End With
			With Nothing : Dim value As Long = 1L : Trace.Print("value", value) : End With
			With Nothing : Dim value As ULong = 1UL : Trace.Print("value", value) : End With
			With Nothing : Dim value As Single = 1.2345F : Trace.Print("value", value) : End With
			With Nothing : Dim value As Double = 1.123456789 : Trace.Print("value", value) : End With
			With Nothing : Dim value As Decimal = 1.123456789D : Trace.Print("value", value) : End With
			With Nothing : Dim value As Char = "A"c : Trace.Print("value", value) : End With
			With Nothing : Dim value As String = "A" : Trace.Print("value", value) : End With

			With Nothing : Dim value As Boolean() = {False, True} : Trace.Print("value", value) : End With
            With Nothing : Dim value As Object() = {1, 2, 3} : Trace.Print("value", value) : End With
            Trace.Leave()
        End Sub
	End Module
End Namespace

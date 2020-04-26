Imports System.Threading
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports DebugTrace.VisualBasic ' for Debugging

Namespace Readme
    ''' <summary>ReadmeExample1</summary>
    <TestClass()>
    Public Class ReadmeExample1
        ''' <summary>Example1</summary>
        <TestMethod()>
        Public Sub Example1()
            Trace.Enter() ' for Debugging

            Dim contacts = New Contact() {
                New Contact(1, "Akane", "Apple", New DateTime(1991, 2, 3)),
                New Contact(2, "Yukari", "Apple", New DateTime(1992, 3, 4))
            }
            Trace.Print("contacts", contacts) ' for Debugging

            Trace.Leave() ' for Debugging
        End Sub
    End Class

    ''' <summary>Entity</summary>
    Public class Entity
        Public Property Id As Integer

        Public Sub New(id_ As Integer)
            Id = id_
        End Sub
    End Class

    ''' <summary>ContactBase</summary>
    public class ContactBase : Inherits Entity
        Public Property FirstName As String
        Public Property LastName As String

        Public Sub New(id_ As Integer, firstName_ As String, lastName_ As String)
            MyBase.New(id_)
            FirstName = firstName_ : LastName = lastName_
        End Sub
    End Class

    ''' <summary>Contact</summary>
    Public Class Contact : Inherits ContactBase
        Public Birthday As DateTime

        Public Sub New(id_ As Integer, firstName_ As String, lastName_ As String, birthday_ As DateTime)
            MyBase.New(id_, firstName_, lastName_)
            Birthday = birthday_
        End Sub
    End Class

    ''' <summary>ReadmeExample2</summary>
    <TestClass()>
    public class ReadmeExample2
        ''' <summary>Example2</summary>
        <DataTestMethod()>
        <DataRow(1)>
        Public Sub Example2(value As Integer)
            Trace.Enter() ' for Debugging

            Dim task = TaskExample(value)
            task.Wait()
            Trace.Print("task", task) ' for Debugging

            Trace.Leave() ' for Debugging
        End Sub

        Private Async Function TaskExample(value As Integer) As Task(Of Integer)
            Dim threasdId = Trace.Enter() ' for Debugging
            Dim t = Await task.Run(Of Integer)(
                Function()
                    Trace.Enter() ' for Debugging
                    Thread.Sleep(100)
                    Dim result = value * value
                    Trace.Print("result", result) ' for Debugging
                    Trace.Leave() ' For Debugging
                    Return result
                End Function
                )
            Trace.Leave(threasdId) ' for Debugging
            Return t
        End Function
    End Class
End Namespace

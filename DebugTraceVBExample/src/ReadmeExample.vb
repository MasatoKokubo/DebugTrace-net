Imports DebugTrace.VisualBasic ' for Debugging

Namespace Global.Readme
    ''' <summary>ReadmeExample</summary>
    Public Class ReadmeExample
        Public Shared Sub _Main(args As String())
            Trace.Enter() ' for Debugging

            Dim contact = New Contact() {
                New Contact(1, "Akane", "Apple", New DateTime(1991, 2, 3)),
                New Contact(2, "Yukari", "Apple", New DateTime(1992, 3, 4))
            }
            Trace.Print("contact", contact) ' for Debugging

            Trace.Leave() ' for Debugging
        End Sub
    End Class

    ''' <summary>Entity</summary>
    Public Class Entity
        Public Property ID As Integer

        Public Sub New(id_ As Integer)
            ID = id_
        End Sub
    End Class

    ''' <summary>ContactBase</summary>
    Public Class ContactBase : Inherits Entity
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
End Namespace

Imports DebugTrace
Imports DebugTrace.VisualBasic
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Namespace DebugTraceTest
    <TestClass>
    Public Class PropertyTest
        ' ClassCleanup
        <TestCleanup>
        Public Sub ClassCleanup()
            TraceBase.OutputNonPublicFields = False
            TraceBase.OutputNonPublicProperties = False
        End Sub

        Public Class Inner
            Private PrivateField As Integer = 1
            Protected ProtectedField As Integer = 2
            Friend FriendField As Integer = 3
            Protected Friend ProtectedFriendField As Integer = 4
            'Private Protected PrivateProtectedField As Integer = 5
            Public PublicField As Integer = 6

            Private ReadOnly Property PrivateProperty As Integer = 1
            Protected ReadOnly Property ProtectedProperty As Integer = 2
            Friend ReadOnly Property FriendProperty As Integer = 3
            Protected Friend ReadOnly Property ProtectedFriendProperty As Integer = 4
            'Private Protected ReadOnly Property PrivateProtectedProperty As Integer = 5
            Public ReadOnly Property PublicProperty As Integer = 6
        End Class

        ' OutputNonPublicFields / since 1.4.4
        ' OutputNonPublicProperties / since 1.4.4
        <TestMethod>
        Public Sub OutputNonPublicFields_Properties()
            TraceBase.OutputNonPublicFields = False
            TraceBase.OutputNonPublicProperties = False
            Trace.Print("value", New Inner())

            Assert.IsFalse(Trace.LastLog.Contains("Private PrivateField"))
            Assert.IsFalse(Trace.LastLog.Contains("Protected ProtectedField"))
            Assert.IsFalse(Trace.LastLog.Contains("Friend FriendField"))
            Assert.IsFalse(Trace.LastLog.Contains("Protected Friend ProtectedFriendField"))
            'Assert.IsFalse(Trace.LastLog.Contains("Private Protected PrivateProtectedField"))
            Assert.IsTrue(Trace.LastLog.Contains("PublicField"))

            Assert.IsFalse(Trace.LastLog.Contains("Private PrivateProperty"))
            Assert.IsFalse(Trace.LastLog.Contains("Protected ProtectedProperty"))
            Assert.IsFalse(Trace.LastLog.Contains("Friend FriendProperty"))
            Assert.IsFalse(Trace.LastLog.Contains("Protected Friend ProtectedFriendProperty"))
            'Assert.IsFalse(Trace.LastLog.Contains("Private Protected PrivateProtectedProperty"))
            Assert.IsTrue(Trace.LastLog.Contains("PublicProperty"))

            TraceBase.OutputNonPublicFields = True
            TraceBase.OutputNonPublicProperties = False
            Trace.Print("value", New Inner())

            Assert.IsTrue(Trace.LastLog.Contains("Private PrivateField"))
            Assert.IsTrue(Trace.LastLog.Contains("Protected ProtectedField"))
            Assert.IsTrue(Trace.LastLog.Contains("Friend FriendField"))
            Assert.IsTrue(Trace.LastLog.Contains("Protected Friend ProtectedFriendField"))
            'Assert.IsTrue (Trace.LastLog.Contains("Private Protected PrivateProtectedField"))
            Assert.IsTrue(Trace.LastLog.Contains("PublicField"))

            Assert.IsFalse(Trace.LastLog.Contains("Private PrivateProperty"))
            Assert.IsFalse(Trace.LastLog.Contains("Protected ProtectedProperty"))
            Assert.IsFalse(Trace.LastLog.Contains("Friend FriendProperty"))
            Assert.IsFalse(Trace.LastLog.Contains("Protected Friend ProtectedFriendProperty"))
            'Assert.IsFalse(Trace.LastLog.Contains("Private Protected PrivateProtectedProperty"))
            Assert.IsTrue(Trace.LastLog.Contains("PublicProperty"))

            TraceBase.OutputNonPublicFields = False
            TraceBase.OutputNonPublicProperties = True
            Trace.Print("value", New Inner())

            Assert.IsFalse(Trace.LastLog.Contains("Private PrivateField"))
            Assert.IsFalse(Trace.LastLog.Contains("Protected ProtectedField"))
            Assert.IsFalse(Trace.LastLog.Contains("Friend FriendField"))
            Assert.IsFalse(Trace.LastLog.Contains("Protected Friend ProtectedFriendField"))
            'Assert.IsFalse(Trace.LastLog.Contains("Private Protected PrivateProtectedField"))
            Assert.IsTrue(Trace.LastLog.Contains("PublicField"))

            Assert.IsTrue(Trace.LastLog.Contains("Private PrivateProperty"))
            Assert.IsTrue(Trace.LastLog.Contains("Protected ProtectedProperty"))
            Assert.IsTrue(Trace.LastLog.Contains("Friend FriendProperty"))
            Assert.IsTrue(Trace.LastLog.Contains("Protected Friend ProtectedFriendProperty"))
            'Assert.IsTrue (Trace.LastLog.Contains("Private Protected PrivateProtectedProperty"))
            Assert.IsTrue(Trace.LastLog.Contains("PublicProperty"))

            TraceBase.OutputNonPublicFields = True
            TraceBase.OutputNonPublicProperties = True
            Trace.Print("value", New Inner())

            Assert.IsTrue(Trace.LastLog.Contains("Private PrivateField"))
            Assert.IsTrue(Trace.LastLog.Contains("Protected ProtectedField"))
            Assert.IsTrue(Trace.LastLog.Contains("Friend FriendField"))
            Assert.IsTrue(Trace.LastLog.Contains("Protected Friend ProtectedFriendField"))
            'Assert.IsTrue (Trace.LastLog.Contains("Private Protected PrivateProtectedField"))
            Assert.IsTrue(Trace.LastLog.Contains("PublicField"))

            Assert.IsTrue(Trace.LastLog.Contains("Private PrivateProperty"))
            Assert.IsTrue(Trace.LastLog.Contains("Protected ProtectedProperty"))
            Assert.IsTrue(Trace.LastLog.Contains("Friend FriendProperty"))
            Assert.IsTrue(Trace.LastLog.Contains("Protected Friend ProtectedFriendProperty"))
            'Assert.IsTrue (Trace.LastLog.Contains("Private Protected PrivateProtectedProperty"))
            Assert.IsTrue(Trace.LastLog.Contains("PublicProperty"))
        End Sub
    End Class
End Namespace

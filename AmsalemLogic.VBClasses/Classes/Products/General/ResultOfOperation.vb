Imports AmsalemLogic
Imports Microsoft.VisualBasic

Public Class ResultOfOperation

    Public Shared ReadOnly SuccessInstance As ResultOfOperation = New ResultOfOperation(True, "Success")

    Private Success_ As Boolean = False
    Public Property Success() As Boolean
        Get
            Return Success_
        End Get
        Set(ByVal value As Boolean)
            Success_ = value
        End Set
    End Property

    Private Message_ As String = ""
    Public Property Message() As String
        Get
            Return Message_
        End Get
        Set(ByVal value As String)
            Message_ = value
        End Set
    End Property

    Public Property Additional As String

    Public Property Additional2 As String

    Public Property Additional3 As String

    Public Property Additional4 As String


    Public Sub New()

    End Sub

    Public Sub New(ByVal Success As Boolean)
        Me.Success = Success
    End Sub

    Public Sub New(ByVal Success As Boolean, ByVal Message As String)
        Me.New(Success)
        Me.Message = Message
    End Sub
    Public Sub New(ByVal Success As Boolean, ByVal Message As String, ByVal Additional As String)
        Me.New(Success)
        Me.Message = Message
        Me.Additional = Additional
    End Sub
    Public Sub New(ByVal Success As Boolean, ByVal Message As String, ByVal Additional As String, ByVal Additional2 As String)
        Me.New(Success)
        Me.Message = Message
        Me.Additional = Additional
        Me.Additional2 = Additional2
    End Sub
    Public Sub New(ByVal Success As Boolean, ByVal Message As String, ByVal Additional As String, ByVal Additional2 As String, Additional3 As String)
        Me.New(Success)
        Me.Message = Message
        Me.Additional = Additional
        Me.Additional2 = Additional2
        Me.Additional3 = Additional3
    End Sub
    Public Sub New(ByVal Success As Boolean, ByVal Message As String, ByVal Additional As String, ByVal Additional2 As String, Additional3 As String, additional4 As String)
        Me.New(Success)
        Me.Message = Message
        Me.Additional = Additional
        Me.Additional2 = Additional2
        Me.Additional3 = Additional3
        Me.Additional4 = additional4
    End Sub

    Public Sub AppendMessage(appendMessage As String, Optional LineBreak As String = vbCrLf)
        If Not String.IsNullOrEmpty(Me.Message) Then
            Me.Message &= LineBreak & appendMessage
        Else
            Me.Message = appendMessage
        End If
    End Sub

    Friend Function Validate(check As Func(Of ResultOfOperation, Boolean), additionalMessage As String) As ResultOfOperation
        Dim result = check(Me)
        Me.Success = Me.Success AndAlso result
        If Not result Then
            Me.AppendMessage(additionalMessage)
        End If
        Return Me
    End Function
End Class

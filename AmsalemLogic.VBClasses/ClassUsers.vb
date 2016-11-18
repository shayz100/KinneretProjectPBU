Option Strict On
Imports Microsoft.VisualBasic
Imports System.Text
Imports System.Web


Public Class ClassUsers
    Public LastException As System.Exception

#Region "Prop"

    Private AgentName_ As String


    Public Property AgentName() As String
        Get
            Return AgentName_
        End Get
        Set(ByVal value As String)
            AgentName_ = value
        End Set
    End Property

    Private Title_ As String
    Public Property Title() As String
        Get
            Return Title_
        End Get
        Set(ByVal value As String)
            Title_ = value
        End Set
    End Property

    Private UserId_ As String
    Public Property UserId() As String
        Get
            Return UserId_
        End Get
        Set(ByVal value As String)
            UserId_ = value
        End Set
    End Property
  

    Private WorkerClockId_ As Integer
    Public Property WorkerClockId() As Integer
        Get
            Return WorkerClockId_
        End Get
        Set(ByVal value As Integer)
            WorkerClockId_ = value
        End Set
    End Property

    Private AgentEmail_ As String
    Public Property AgentEmail() As String
        Get
            Return AgentEmail_
        End Get
        Set(ByVal value As String)
            AgentEmail_ = value
        End Set
    End Property

    Private ManagerName_ As String
    Public Property ManagerName() As String
        Get
            Return ManagerName_
        End Get
        Set(ByVal value As String)
            ManagerName_ = value
        End Set
    End Property

    Private ManagerEmail_ As String
    Public Property ManagerEmail() As String
        Get
            Return ManagerEmail_
        End Get
        Set(ByVal value As String)
            ManagerEmail_ = value
        End Set
    End Property

    Public Property ManagerWorkerClockID As Integer

    Private AssistName_ As String
    Public Property AssistName() As String
        Get
            Return AssistName_
        End Get
        Set(ByVal value As String)
            AssistName_ = value
        End Set
    End Property

    Private AssistEmail_ As String
    Public Property AssistEmail() As String
        Get
            Return AssistEmail_
        End Get
        Set(ByVal value As String)
            AssistEmail_ = value
        End Set
    End Property

    Private QEP_ As String
    Public Property QEP() As String
        Get
            Return QEP_
        End Get
        Set(ByVal value As String)
            QEP_ = value
        End Set
    End Property

    Private EXt_Phone_ As String
    Public Property EXt_Phone() As String
        Get
            Return EXt_Phone_
        End Get
        Set(ByVal value As String)
            EXt_Phone_ = value
        End Set
    End Property
    '--------------------------


    Private LogIn_ As String
    Public Property LogIn() As String
        Get
            Return LogIn_
        End Get
        Set(ByVal value As String)
            LogIn_ = value
        End Set
    End Property

    Private Desk_ As String
    Public Property Desk() As String
        Get
            Return Desk_
        End Get
        Set(ByVal value As String)
            Desk_ = value
        End Set
    End Property

    Dim DeskInt As Integer

    Private Department_ As String
    Public Property Department() As String
        Get
            Return Department_
        End Get
        Set(ByVal value As String)
            Department_ = value
        End Set
    End Property

    Private LastEnterd_ As String = ""
    Public Property LastEnterd() As String
        Get
            Return LastEnterd_
        End Get
        Set(ByVal value As String)
            LastEnterd_ = value
        End Set
    End Property

    Private OnBehalfOf_ As String = ""
    Public Property OnBehalfOf() As String
        Get
            Return OnBehalfOf_
        End Get
        Set(ByVal value As String)
            OnBehalfOf_ = value
        End Set
    End Property

    Public Property OnBehalfOfClockID() As Integer

    Private Admin_ As Integer
    Public Property Admin() As Integer
        Get
            Return Admin_
        End Get
        Set(ByVal value As Integer)
            Admin_ = value
        End Set
    End Property

    Private AxCompany_ As String
    Public Property AxCompany() As String
        Get
            Return AxCompany_
        End Get
        Set(ByVal value As String)
            AxCompany_ = value
        End Set
    End Property

    Public Property PermissionGroup As List(Of Integer) = New List(Of Integer)

    Public Property OnBehalf As Boolean = False

    Public Property IsAgent As Boolean = False
#End Region


    Public Shared Function GetCurrentUser(Optional LOGON_USER As String = Nothing) As ClassUsers
        Dim result = New ClassUsers()
        result.AgentName = "Nitzan"
        result.AgentEmail = ""
        Return result
    End Function

End Class

Public Class ClassUsersForDDL

    Public Property AgentName As String

    Public Property ID As String

End Class

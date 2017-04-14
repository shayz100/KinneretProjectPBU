Option Strict On
Imports System.Web

Namespace Administration

    Public Class UserPermissionHandler

        Property UNAUTHORIZED_MESSAGE As String = "you dont have permission to do this action"

        Function IsActionAllowed(PermissionGroups As List(Of Integer), ActionCode As String, ActionType As String) As Boolean
            Dim allowd = True
            Return allowd
        End Function

    End Class
End Namespace

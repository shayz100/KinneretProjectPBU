Imports System.Web.Mvc
Imports AmsalemLogic.NewLogic
Imports AmsalemLogic.NewLogic.Classes
Imports AmsalemLogic.VBClasses
Imports AmsalemLogic.VBClasses.Administration

Namespace Controllers
    Public Class CreditCardesController
        Inherits Controller

        Function MyView() As ActionResult
            PermissionCheck("Credit Card Admin")
            Dim Handler = New PaidByUsHandler()
            Dim ListOfCardes = Handler.GetAllCards
            Return View(ListOfCardes)
            'Return View()
        End Function

        Sub PermissionCheck(permissionName As String)
            Dim user = ClassUsers.GetCurrentUser()
            Dim userPermission = New UserPermissionHandler()
            Dim permission = userPermission.IsActionAllowed(user.PermissionGroup, permissionName, "")
            If (Not permission) Then
                Throw New ApplicationException(userPermission.UNAUTHORIZED_MESSAGE)
            End If
            'TODO- Do Actions 
        End Sub
    End Class
End Namespace
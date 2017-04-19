Imports System.Runtime.Serialization

<DataContract>
Public Class CurrencyItem
    Sub New(code As String, description As String)
        Me.Code = code
        Me.Description = description
    End Sub

    <DataMember>
    Public Property Code As String

    <DataMember>
    Public Property Description As String
End Class

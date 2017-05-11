Imports System.Web.Mvc
Imports AmsalemLogic.NewLogic.Classes
Imports AmsalemLogic.VBClasses
Imports AmsalemLogic.VBClasses.Administration
Imports AmsalemLogic
Imports Amsalem.Types
Imports AmsalemLogic.NewLogic.Classes.Products.ArchiveMongoDB
Imports System.Drawing
Imports System.IO
Imports AmsalemLogic.NewLogic.Entity_Framework

Namespace Controllers
    Public Class TransactionController
        Inherits Controller

        Function Index() As ActionResult
            Return View()

        End Function


        Function NewForm() As ActionResult
            Dim MockupController = New MockupController()
            Dim Currencies = MockupController.GetCurrenciesOfAxCompany(String.Empty)
            Dim AxCompanies = MockupController.GetAxCompanies()
            Dim Vendors = MockupController.GetVendors(String.Empty)
            Dim VendorsNameList = New List(Of String)
            Dim VendorsString = ""
            For Each Vendor In Vendors
                VendorsString += Vendor.Name + ","
            Next
            ViewBag.CurrenciesList = Currencies
            ViewBag.AxCompaniesList = AxCompanies
            ViewBag.VendorsArray = VendorsString
            Return View()

        End Function

        Function Create(transaction As PaidByUsTransactionDTO) As ActionResult
            Dim handler = New PaidByUsHandler()
            Dim mockUp = New MockupController()
            transaction.SupplierAccountNum = mockUp.GetVendorIdByName(transaction.SupplierName)
            Dim user = ClassUsers.GetCurrentUser()
            Dim rop = handler.CreateNewTransaction(transaction, user)
            Return Json(rop)

        End Function

        Function Details(id As Integer) As ActionResult
            Dim handler = New PaidByUsHandler()
            Dim transaction = handler.GetTransaction(id)
            ViewBag.Transaction = transaction
            ViewBag.ImageHash = handler.GetTransactionHash(transaction)
            Return View()
        End Function

        'Function SwitchCard(Id As Integer, Id2 As Integer) As ActionResult
        Function SwitchCard() As ActionResult
            Dim transaction = New PaidByUsTransactionDTO()
            Dim user = ClassUsers.GetCurrentUser()
            Dim handler = New PaidByUsHandler()
            'transaction = handler.GetTransaction(Id)
            'Dim rop = handler.ReplaceCard(transaction, user, Id2)
            transaction = handler.GetTransaction(10015)
            Dim rop = handler.ReplaceCard(transaction, user, 1)
            ViewBag.Transaction = transaction
            ViewBag.ImageHash = handler.GetTransactionHash(transaction)
            Return View()

        End Function

        Function SendToEmail(transId As Integer) As ActionResult
            'TODO
            'Get transaction by id
            Return View()

        End Function

    End Class
End Namespace
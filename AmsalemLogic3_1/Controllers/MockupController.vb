Imports System.Web.Mvc
Imports AmsalemLogic.VBClasses
Imports Amsalem.Types.BackOffice


Namespace Controllers
    Public Class MockupController
        Inherits Controller

        Public Function GetAxCompanies() As List(Of BackOfficeCompany)
            Dim ans = New List(Of BackOfficeCompany)
            ans.Add(New BackOfficeCompany With
                    {
                        .BackOfficeType = Amsalem.Types.EBackOfficeType.AX,
                        .Code = 1,
                        .Company = "AMSA"
                    })

            ans.Add(New BackOfficeCompany With
                   {
                       .BackOfficeType = Amsalem.Types.EBackOfficeType.AX,
                       .Code = 6,
                   .Company = "HKG"
                   })
            ans.Add(New BackOfficeCompany With
                  {
                      .BackOfficeType = Amsalem.Types.EBackOfficeType.AX,
                      .Code = 5,
                  .Company = "USA"
                  })
            Return ans
        End Function


        Public Function GetVendors(backOfficeCompany As String) As List(Of BackOfficeVendor)
            Dim ans = New List(Of BackOfficeVendor)
            ans.Add(New BackOfficeVendor With
                    {
                        .label = "Dolphin Reef, 3 km south to Eilat(450074)",
                        .Name = "Dolphin Reef",
                        .VendorID = 450074
                        })

            ans.Add(New BackOfficeVendor With
                   {
                       .label = "DANIEL DEAD SEA,(450071)",
                       .Name = "DANIEL DEAD SEA",
                       .VendorID = 450071
                       })
            Return ans
        End Function

        Public Function GetCurrenciesOfAxCompany(AxCompany As String) As List(Of CurrencyItem)
            Dim AllCurrencies = New List(Of CurrencyItem)
            AllCurrencies.Add(New CurrencyItem("ILS", "New israeli shekel"))
            AllCurrencies.Add(New CurrencyItem("USD", "Dollars USA"))
            AllCurrencies.Add(New CurrencyItem("EUR", "EURO -europe"))
            Return AllCurrencies
        End Function

    End Class
End Namespace
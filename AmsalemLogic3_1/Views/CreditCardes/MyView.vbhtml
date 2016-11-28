@ModelType IEnumerable(Of Amsalem.Types.CreditCards.CreditCard)
@Code
ViewData("Title") = "MyView"
End Code

<h2>MyView</h2>

<p>
    @Html.ActionLink("Create New", "Create")
</p>
<table class="table">
    <tr>
        <th>
            @Html.DisplayNameFor(Function(model) model.CreditCardOwner)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.CustomerID)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.ContactPersonId)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.CreditCardType)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.CreditCardInternalIdentifier)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.OwnerName)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.CreditCardIdentifier)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.CreditCardExpirationDate)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.CreditCardNote)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.BankNumber)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.CreditCardOwnerID)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.CreditCardAccount)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.CreditCardTerminal)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.BackOffice)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.Source)
        </th>
        <th>
            @Html.DisplayNameFor(Function(model) model.AxCompany)
        </th>
        <th></th>
    </tr>

@For Each item In Model
    @<tr>
        <td>
            @Html.DisplayFor(Function(modelItem) item.CreditCardOwner)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.CustomerID)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.ContactPersonId)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.CreditCardType)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.CreditCardInternalIdentifier)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.OwnerName)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.CreditCardIdentifier)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.CreditCardExpirationDate)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.CreditCardNote)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.BankNumber)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.CreditCardOwnerID)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.CreditCardAccount)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.CreditCardTerminal)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.BackOffice)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.Source)
        </td>
        <td>
            @Html.DisplayFor(Function(modelItem) item.AxCompany)
        </td>
        <td>
            @Html.ActionLink("Edit", "Edit", New With {.id = item.CreditCardNumber }) |
            @Html.ActionLink("Details", "Details", New With {.id = item.CreditCardNumber }) |
            @Html.ActionLink("Delete", "Delete", New With {.id = item.CreditCardNumber })
        </td>
    </tr>
Next

</table>

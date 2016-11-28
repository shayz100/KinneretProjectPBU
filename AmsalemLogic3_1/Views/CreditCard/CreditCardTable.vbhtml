@ModelType IEnumerable(Of Amsalem.Types.CreditCards.CreditCard)

<!DOCTYPE html>

<html>
<head>

    <script src="~/Scripts/jquery-3.1.1.min.js"></script>
    <link href="~/Content/DataTables/css/jquery.dataTables.css" rel="stylesheet" />
    <script src="~/Scripts/DataTables/jquery.dataTables.js"></script>

    <script>

        $(document).ready(function () {
            $('#myTable').DataTable();
        });

    </script>

</head>

<body>

    <div class="col-md-10 col-md-offset-1">
        <div class="panel panel-default panel-table">
            <div class="panel-body">
                <table id="myTable" class="table table-striped table-bordered table-list">
                    <thead>
                        <tr>
                            <th>Picture</th>
                            <th>Credit</th>
                            <th>Status</th>
                            <th>Back Office</th>
                            <th>CompID</th>
                            <th>No.</th>
                            <th>Expire</th>
                            <th>CVV</th>
                        </tr>
                    </thead>

                <tbody>

                    @For Each item In Model

                        @<tr>
                             <td align="center">
                                 <a class="btn btn-default"><span class="glyphicon glyphicon-picture"></span></a>
                             </td>

                             <td align="center">
                                 <a class="btn btn-default"><span class="glyphicon glyphicon-usd"></span></a>
                             </td>
                            
                            <td>

                                @Select Case item.Status
                                    Case 0
                                        @<b style = "color:red">Cancelled</b>
                                    Case 1
                                        @<b style="color:green">OK</b>

                                End Select

                            </td>

                             <td>
                                 @Html.DisplayFor(Function(modelItem) item.AxCompany)
                             </td>

                             <td>
                                 @Html.DisplayFor(Function(modelItem) item.CreditCardType) 
                             </td>

                             <td>
                                 @Html.DisplayFor(Function(modelItem) item.CreditCardInternalIdentifier) 
                             </td>

                             <td>
                                 @Html.DisplayFor(Function(modelItem) item.CreditCardExpirationDate)
                             </td>

                             <td>
                                 @Html.DisplayFor(Function(modelItem) item.CreditCardIdentifier)
                             </td>


                        </tr>
                    Next
                    

                                       
                        @*<td align = "center" >
                            <a class="btn btn-default"><span class="glyphicon glyphicon-picture"></a>
                        </td>                                                                                                                                                     

                    <td align="center">
                        <a class="btn btn-default"><span class="glyphicon glyphicon-usd"></a>
                    </td>   
                            @item.Status

                            @item.DataAreaId

                            @item.CompanyId

                            @item.CreditCardNo

                            @item.ExpiryDate

                            @item.Cvv*@
                       

                     </tbody>
                   </table>
                 </div>
               </div>
             </div>
           </body>
         </html>


﻿<!doctype html>
<html>
<head>

    <title>Paid By Us Transactions Table</title>
    <link href="~/Content/themes/base/jquery-ui.css" rel="stylesheet" />
    <link href="~/Content/toastr.css" rel="stylesheet" />
    <script src="~/Scripts/jquery-3.1.1.js"></script>
    <script src="~/Scripts/additional-methods.js"></script>
    <script src="~/Scripts/toastr.js"></script>
    <script>


        var list = "@ViewBag.VendorsArray";
        var availableTags = list.split(',');

        $(document).ready(function () {


            $("#Supplier").autocomplete({

                source: availableTags
            });

            $("#NewTransactionForm").validate({

                rules: {
                    SupplierAccount: {
                        required: true
                    },

                    OriginalAmount: {
                        min: 0
                    }
                },

                messages: {
                    OriginalAmount: {
                        min: "Please enter amount greater than zero"
                    }
                },

                submitHandler: function (form) {
                    $.ajax({
                        url: '/Transaction/Create',
                        data: $("#NewTransactionForm").serialize(),
                        type: 'POST',
                        success: function (data) {
                            if (data.Message == "Success") {
                                id = 6;
                                window.location = '/Transaction/Details/' + id;
                            }
                            //toastr.error(data.Message);
                            toastr.error(data.Additional4);


                        }
                    });
                }

            });



        })


    </script>

</head>
<body>

    <div class="container">
        <form id="NewTransactionForm" style="margin-top:20px" method="post">
            <fieldset>
                <legend class="text-center">Add New Transaction</legend>

                <div class="row form-group">
                    <div class="col-lg-3 col-lg-offset-3">
                        <label for="OriginAmount">Original Amount</label>
                        <input class="form-control" name="OriginAmount" type="number" placeholder="Amount">
                    </div>

                    <div class="form-group col-lg-3">
                        <label for="OriginalCurrencyCode">Original Currency Code</label>

                        <select class="form-control" name="OriginalCurrencyCode">
                            @For Each currency In ViewBag.CurrenciesList

                                @<option>@currency.Code</option>

                            Next
                        </select>
                    </div>
                </div>

                <div Class="row form-group">
                    <div Class="form-group col-lg-offset-3 col-lg-3">
                        <Label for="BackOfficeCompany">Back Office Company</Label>
                        <select Class="form-control" name="BackOfficeCompany">
                            @For Each company In ViewBag.AxCompaniesList

                                @<option>@company.Company</option>

                            Next

                        </select>
                    </div>

                    <div Class="col-lg-3">
                        <Label for="SupplierAccount">Supplier</Label>
                        <input Class="form-control" name="SupplierAccount" type="text" placeholder="Supplier Name" id="Supplier">
                    </div>
                </div>

                <div Class="row form-group">

                    <div Class="form-group col-lg-offset-3 col-lg-3">
                        <Label for="TripNumber">Trip Number</Label>
                        <input type="text" Class="form-control" name="TripNumber" placeholder="Trip Number">
                    </div>
                </div>

                <div Class="row form-group">

                    <div Class="form-group col-lg-offset-3 col-lg-3">

                        <Label for="ForWho">For</Label>
                        <input type="text" Class="form-control" name="ForWho" placeholder="Customer Name">

                    </div>

                    <div Class="form-group col-lg-3">

                        <Label for="For">Item</Label>
                        <input type="text" Class="form-control" name="Item" placeholder="Item">

                    </div>

                </div>

                <div Class="form-group">
                    <Button type="submit" Class="btn btn-success form-group col-lg-offset-3 col-lg-6">Get Card</Button>
                </div>

            </fieldset>

        </form>
    </div>

</body>
</html>
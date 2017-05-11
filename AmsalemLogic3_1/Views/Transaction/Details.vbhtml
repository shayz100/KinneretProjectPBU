@Modeltype AmsalemLogic.NewLogic.Classes.PaidByUsTransactionDTO
<!DOCTYPE html>
<html>
<head>

    <title>Bootstrap Website</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-alpha.5/css/bootstrap.min.css" integrity="sha384-AysaV+vQoT3kOAXZkl02PThvDr8HYKPZhNT5h/CXfBThSRXQ6jW5DO2ekP5ViFdi" crossorigin="anonymous">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-alpha.5/js/bootstrap.min.js" integrity="sha384-BLiI7JTZm+JWlgKa0M0kGRpJbF2J8q+qreVrKBC47e3K6BW78kGLrCkeRX6I9RoK" crossorigin="anonymous"></script>
    <script>
        $(document).ready(function () {

            $("#button1").click(function () {
                alert("cliekced");
                $.ajax({
                    url: '/Transaction/SwitchCard',
                    data: {"Id": transaction.id, "Id2": 0},
                    type: 'POST',
                    success: function (data) {

                    }
                });
            });

            $("#button2").click(function () {
                $.ajax({
                    url: '/Transaction/SwitchCard',
                    data: { transId: transaction.id, cause: 1 },
                    type: 'POST',
                    success: function (data) {

                    }
                });
            });

        });
    </script>

    <style>
        .abc {
            border: 1px solid black;
            background-color: #ddd;
        }

        body {
        }

        .btn {
            background-color: mediumblue;
            font-family: 'Times New Roman', Times, serif;
            font-size: large;
        }

        .center-block {
            font-family: 'Times New Roman', Times, serif;
            font-size: 45px;
            display: table;
            margin: auto;
        }

        .col1 {
            font-family: 'Times New Roman', Times, serif;
            color: black;
            font-size: 24px;
        }

        .col2 {
            font-family: 'Times New Roman', Times, serif;
            color: darkslateblue;
            font-size: 24px;
        }

        .img-responsive {
            position: absolute;
            right: 0%;
            top: 60%;
            transform: translate(-50%, -50%);
        }

        .topleft {
            position: absolute;
            top: 5%;
            left: 40%;
            transform: translate(-50%, -50%);
            font-size: 16px;
            color: red;
        }

        .center {
            position: absolute;
            top: 60%;
            left: 50%;
            transform: translate(-50%, -50%);
            font-size: 16px;
            color: red;
        }

        .row-grid + .row-grid {
            margin-top: 20px;
        }
    </style>
</head>
<body>

    <div class="row" style="margin-top:30px">
        <div class="col-sm-2 "><button class="btn btn-primary view-pdf">Send PDF</button></div>
        <div class="dropdown col-sm-3">
            <button id="dLabel" data-target="#" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false" class="btn btn-primary">
                Switch Card
                <span class="caret"></span>
            </button>

            <ul class="dropdown-menu" aria-labelledby="dLabel">
                <li><a id="button1" href="#">Cause 1</a></li>
                <li><a id="button2" href="#">Cause 2</a></li>
            </ul>
        </div>
        <div class="col-sm-7 "><img src="http://www.amsalem.com/wp-content/uploads/2016/12/logo_268_81.png" class="img-fluid float-xs-right" /></div>
    </div>

    <div class="container">
        <div class="row row-grid">
            <div class="center-block">Transaction Details</div>
        </div>
        <div class="row row-grid">
            <div class="col1 col-sm-3">To:</div>
            <div class="col2 col-sm-5">@ViewBag.Transaction.SupplierName</div>
        </div>

        <div class="row row-grid">
            <div class="col1 col-sm-6">authorizing billing with the following details:</div>
        </div>

        <div class="row row-grid">
            <div class="col1 col-sm-3">For:</div>
            <div class="col2 col-sm-9">@ViewBag.Transaction.CustomerName, @ViewBag.Transaction.TripNumber</div>
            <div class="img-responsive">
                <img src='@(Url.Action("ShowImage", "CreditCard", New With {.id = ViewBag.ImageHash}))' Class="img float-xs-right" width="350" height="380" id="creditCardImage" />

                <div class="topleft">pay to @ViewBag.Transaction.SupplierName only</div>
                <div class="center">@ViewBag.Transaction.TripNumber</div>
            </div>

        </div>


        <div class="row row-grid">
            <div class="col1 col-sm-3">For commodity:</div>
            <div class="col2 col-sm-6">@ViewBag.Transaction.Item</div>
        </div>



        <div class="row row-grid">
            <div class="col1 col-sm-3">Price:</div>
            <div class="col2 col-sm-5">@ViewBag.Transaction.OriginalAmount</div>
        </div>



        <div class="row row-grid">
            <div class="col1 col-sm-3">Currency:</div>
            <div class="col2 col-sm-4">@ViewBag.Transaction.OriginalCurrencyCode</div>
        </div>

        <div class="row row-grid">
            <div class="col1 col-sm-3">Issued by:</div>
            <div class="col2 col-sm-3">@ViewBag.Transaction.ModifiedByName</div>
        </div>

    </div>

</body>
</html>






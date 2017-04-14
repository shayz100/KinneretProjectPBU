

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta http-equiv="x-ua-compatible" content="ie=edge">
    <title>Bootstrap Website</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-alpha.5/css/bootstrap.min.css" integrity="sha384-AysaV+vQoT3kOAXZkl02PThvDr8HYKPZhNT5h/CXfBThSRXQ6jW5DO2ekP5ViFdi" crossorigin="anonymous">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/tether/1.3.7/js/tether.min.js" integrity="sha384-XTs3FgkjiBgo8qjEjBk0tGmf3wPrWtA6coPfQDfFEY8AnYJwjalXCiosYRBIBZX8" crossorigin="anonymous"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-alpha.5/js/bootstrap.min.js" integrity="sha384-BLiI7JTZm+JWlgKa0M0kGRpJbF2J8q+qreVrKBC47e3K6BW78kGLrCkeRX6I9RoK" crossorigin="anonymous"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>

    <style>
        .abc {
            border: 1px solid black;
            background-color: #ddd;
        }

        body {
            background-color: lightcyan;
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
            font-size: large;
            color: black;
            font-size: 28px;
        }

        .col2 {
            font-family: 'Times New Roman', Times, serif;
            font-size: large;
            color: red;
            font-size: 28px;
        }

        .img-responsive {
            position: absolute;
            right: 0%;
            top: 50%;
            transform: translate(-50%, -50%);
        }

        .topleft {
            position: absolute;
            top: 5%;
            left: 50%;
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

    <div class="container ">
        <form style="margin-top:40px">
            <div class="row">
                <div class="col-md-2 "><button class="btn btn-primary view-pdf">Send PDF</button></div>
                <div class="col-md-2 "><button2 class="btn btn-primary">Switch Card</button2></div>
                <div class="col-md-8 "><img src="http://www.amsalem.com/wp-content/uploads/2016/12/logo_268_81.png" class="img-fluid float-xs-right" /></div>
            </div>
        </form>
    </div>

    <div class="container">
        <div class="row row-grid">
            <div class="center-block">Transaction Details</div>
        </div>
        <div class="row row-grid">
            <div class="col1 col-md-3">To:</div>
            <div class="col2 col-md-5">@ViewBag.Transaction.SupplierName</div>
        </div>

        <div class="row row-grid">
            <div class="col1 col-md-8">We authorize credit card billing with the following details:</div>
        </div>

        <div class="row row-grid">
            <div class="col1 col-md-3">For:</div>
            <div class="col2 col-md-5">@ViewBag.Transaction.ForWho, @ViewBag.Transaction.TripNumber</div>
            <div class="img-responsive">
                <img src="https://www.bcu.com.au/images/Classic_front-(JUNE).png" class="img float-xs-right" width="350" height="380" />
                <div class="topleft">pay to @ViewBag.Transaction.SupplierName only</div>
                <div class="center">@ViewBag.Transaction.TripNumber</div>
            </div>

        </div>


        <div class="row row-grid">
            <div class="col1 col-md-3">For commodity:</div>
            <div class="col2 col-md-6">Hotel Room, From 24.3.2017 To 27.3.2017</div>
        </div>



        <div class="row row-grid">
            <div class="col1 col-md-3">Price:</div>
            <div class="col2 col-md-5">750</div>
        </div>



        <div class="row row-grid">
            <div class="col1 col-md-3">Currency:</div>
            <div class="col2 col-md-4">USD</div>
        </div>

        <div class="row row-grid">
            <div class="col1 col-md-3">Issued by:</div>
            <div class="col2 col-md-3">Orgad M</div>
        </div>

    </div>

</body>
</html>




@ModelType IEnumerable(Of Amsalem.Types.CreditCards.CreditCard)

<!DOCTYPE html>

<html>
<head>

    <link href="~/Content/DataTables/css/jquery.dataTables.css" rel="stylesheet" />
    <script src="~/Scripts/jquery-3.1.1.min.js"></script>
    <script src="~/Scripts/DataTables/jquery.dataTables.js"></script>
    <script src="~/Scripts/bootstrap-filestyle.js"></script>
    <script src="~/Scripts/jquery.validate.js"></script>
    <script src="~/Scripts/additional-methods.js"></script>

    <script>

        $(document).ready(function () {
            $('#myTable').DataTable();
            //$(":file").filestyle({ buttonName: "btn-primary" });

            $("#uploadForm").validate({
                rules: {
                    file: {
                        required: true,
                        accept: "image/png"

                    }
                },
                messages: {

                    file: {

                        accept: "Please Choose *.png File"

                    }

                }
            });

        });

        function showCardImage(elm) {

            var CardNumber = $(elm).closest("tr").find(".card-number").html();
            var CardExpire = $(elm).closest("tr").find(".card-date").html();
            var CardNumber = CardNumber.replace(/\r?\n|\r/g, '').trim();
            var CardExpire = CardExpire.replace(/\r?\n|\r/g, '').trim();
            var ConcatString = CardNumber.concat(CardExpire.replace("/", ""))

            // Get the modal
            var modal = document.getElementById("ImageModal");

            // Get the image and insert it inside the modal
            var $modalImg = $("#img01");
            modal.style.display = "block";
            $modalImg.attr("src", "@(Url.Action("ShowImage", "CreditCard"))/" + ConcatString);

            // Get the <span> element that closes the modal
            var span = document.getElementsByClassName("close")[0];

            // When the user clicks on <span> (x), close the modal
            span.onclick = function () {
                modal.style.display = "none";
            }

        }


        function uploadFile(elm) {
            var uploadFileName = "";
            var CardNumber = $(elm).closest("tr").find(".card-number").html();
            var CardExpire = $(elm).closest("tr").find(".card-date").html();
            var CardNumber = CardNumber.replace(/\r?\n|\r/g, '').trim();
            var CardExpire = CardExpire.replace(/\r?\n|\r/g, '').trim();
            uploadFileName = CardNumber.concat(CardExpire.replace("/", ""));
            $("#fileName").val(uploadFileName);
            // Get the modal
            var modal = document.getElementById('myModal1');

            // Get the <span> element that closes the modal
            var span1 = document.getElementsByClassName("close1")[0];

            // When the user clicks on the button, open the modal
            modal.style.display = "block";

            // When the user clicks on <span> (x), close the modal
            span1.onclick = function () {
                modal.style.display = "none";
                document.getElementById("uploadForm").reset();
            }

        }

        function DropdownShow(element) {
            var elm = element.parentNode.querySelector('.dropdown-content')
            elm.classList.toggle('show');
        }

            window.onclick = function (event) {

                if (!event.target.matches('.image-exist-class')) {

                    var dropdowns = document.getElementsByClassName("dropdown-content");
                    var i;
                    for (i = 0; i < dropdowns.length; i++) {
                        var openDropdown = dropdowns[i];
                        if (openDropdown.classList.contains('show')) {
                            openDropdown.classList.remove('show');
                        }
                    }

                }

            }

    </script>

</head>

<body>

    <!-- The Image Modal -->
    <div id="ImageModal" class="modal" style="z-index:4">

        <!-- The Close Button -->
        <span class="close glyphicon glyphicon-remove blue" onclick="document.getElementById('ImageModal').style.display = 'none'"></span>

        <!-- Modal Content (The Image) -->
        <img class="modal-content" id="img01">

    </div>

    <!-- The File Upload Modal -->
    <div id="myModal1" class="modal1">

        <!-- Modal content -->
        <div class="modal-content1">
            <div class="modal-header">
                <span class="close1 glyphicon glyphicon-remove"></span>
                <h2>Please Choose *.png File</h2>
            </div>
            <div class="modal-body">
                <form id="uploadForm" action="@Url.Action("UploadImage", "CreditCard")" method="post" enctype="multipart/form-data">
                    <input type="file" accept=".png" class="filestyle" data-buttonName="btn-primary" name="file">
                    <input id="fileName" type="hidden" name="fileName" value=""> @*For The File Name Renaming*@
                    <br>
                    <input class="submit" type="submit" value="Upload">
                </form>
            </div>
            <div class="modal-footer">
                <h2>Make Sure The Size Is 200X200</h2>
            </div>
        </div>
    </div>

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
                                    <div class="dropdown">
                                        @Select Case item.ImageExist
                                            Case True
                                                @<button onclick="DropdownShow(this)" class="btn btn-default glyphicon glyphicon-picture image-exist-class"></button>
                                            Case False
                                                @<button onclick="DropdownShow(this)" class="btn btn-default glyphicon glyphicon-remove image-exist-class"></button>

                                        End Select
                                        <div class="dropdown-content">
                                            <a onclick="showCardImage(this)">Show</a>
                                            <a onclick="uploadFile(this)">Edit</a>
                                        </div>
                                    </div>
                                </td>

                                <td align="center">
                                    <a class="btn btn-default"><span class="glyphicon glyphicon-usd"></span></a>
                                </td>

                                <td>

                                    @Select Case item.Status
                                        Case 0
                                            @<b style="color:red">Cancelled</b>
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

                                <td class="card-number">
                                    @Html.DisplayFor(Function(modelItem) item.CreditCardInternalIdentifier)
                                </td>

                                <td class="card-date">
                                    @Html.DisplayFor(Function(modelItem) item.CreditCardExpirationDate)
                                </td>

                                <td>
                                    @Html.DisplayFor(Function(modelItem) item.CreditCardIdentifier)
                                </td>

                            </tr>

                        Next

                    </tbody>
                </table>
            </div>
        </div>
    </div>
</body>
</html>

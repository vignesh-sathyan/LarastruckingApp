var isNeedToloaded = true;

$(document).ready(function () {

    AutoComplete()
    GetDriverList();
    GetDocumetTypeList();

    var accidentReportId = $("#hdnReportAccident").val();

    if (accidentReportId > 0 && accidentReportId != undefined) {

        EditReportAccidentDetail();
        GetDriverInfo();
    }
    else {

    }

});
function goodbye(e) {
    if (isNeedToloaded) {

        if (!e) e = window.event;
        //e.cancelBubble is supported by IE - this will kill the bubbling process.
        e.cancelBubble = true;
        e.returnValue = 'You sure you want to leave?'; //This is displayed on the dialog

        //e.stopPropagation works in Firefox.
        if (e.stopPropagation) {
            e.stopPropagation();
            e.preventDefault();
        }

    }
}
window.onbeforeunload = goodbye;

$("table").on("mouseover", 'tr', function () {

    $(this).find(".far").css('color', 'white');
    $(this).find(".fa-map-marked-alt").css('color', 'white');
});

$("table").on("mouseout", 'tr', function () {

    $(this).find(".far").css('color', '#007bff');
    $(this).find(".fa-download").css('color', '#007bff');
    $(this).find(".fa-map-marked-alt").css('color', '#007bff');
    $(this).find(".fa-trash-alt").css('color', 'red');

});

function EditReportAccidentDetail() {
    var accidentReportId = $("#hdnReportAccident").val();
    $.ajax({
        url: baseUrl + "/AccidentReport/GetReportAccidentById",
        type: "Get",
        data: { AccidentReportId: accidentReportId },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {


            if (response != false) {
                if (response.AccidentDate != null) {
                    var date = ConvertDate(response.AccidentDate, true);
                    $("#dtAccidentDate").val(date);
                }

                $("#hdnEquipmentId").val(response.EquipmentId);
                $("#txtLicencePlate").val(response.LicencePlate);
                $("#txtVIN").val(response.VIN);
                $("#txtModel").val(response.Model);
                $("#txtYear").val(response.Year);
                $("#txtVehicleType").val(response.VehicleType);
                $("#txtAddress").val(response.Address);
                $("#driverId").val(response.DriverId);
                $("#txtPhone").val(response.PhoneNo);
                $("#txtEmailId").val(response.EmailId);
                $("#txtEquipmentNo").val(response.EquipmentNo);
                
                $("#txtComment").val(response.Comments);
                $("#txtPoliceReportNo").val(response.PoliceReportNo);

                $("#hdnReportAccident").val(response.AccidentReportId);

                if (response.AccidentReportDocumentList != null) {
                    $("table tbody").empty();
                    for (var i = 0; i < response.AccidentReportDocumentList.length; i++) {
                        var markup = "<tr class='edit_icon' name='delete-row' value='Remove' ondblclick='javascript:remove_row(this," + response.AccidentReportDocumentList[i].DocumentId + ")'><td>" + response.AccidentReportDocumentList[i].DocumentTypeName + "</td><td><input type='hidden' name='DocumentId' value='" + response.AccidentReportDocumentList[i].DocumentId + "'/><input type='hidden' name='uploadDocument' value=''/><input type='hidden' name='documentType' value='" + response.AccidentReportDocumentList[i].AccidentDocumentId + "'/><input type='hidden' name='documentName' value='" + response.AccidentReportDocumentList[i].DocumentName + "'/>" + response.AccidentReportDocumentList[i].DocumentName + "</td> <td><input type='hidden' name='fileName' value='" + response.AccidentReportDocumentList[i].ImageName + "' />" + response.AccidentReportDocumentList[i].ImageName + "</td>><td><button type='button' class='edit_icon' name='delete-row' value='Remove' onclick='remove_row(this," + response.AccidentReportDocumentList[i].DocumentId + ")'> <i style='color:red' class='far fa-trash-alt'></i> </button></td></tr > ";
                        $("table tbody").append(markup);

                    }
                }
            }
        },
        error: function () {

        }
    });
}

function GetDriverList() {

    $.ajax({
        url: baseUrl + '/AccidentReport/GetDriverList',
        data: {},
        type: "Get",
        async: false,
        success: function (data) {

            var ddlValue = "";
            $("#driverId").empty();

            ddlValue += '<option value="">Please Select Driver</option>'
            for (var i = 0; i < data.length; i++) {
                ddlValue += '<option value="' + data[i].DriverID + '">' + data[i].FirstName + " " + data[i].LastName + '</option>';
            }
            $("#driverId").append(ddlValue);
        }
    });
}

function GetDocumetTypeList() {

    $.ajax({
        url: baseUrl + '/AccidentReport/GetDocumetTypeList',
        type: "GET",
        // cache: false,
        success: function (data) {

            var ddlValue = "";
            $("#ddldocumenttype").empty();
            for (var i = 0; i < data.length; i++) {
                ddlValue += '<option value="' + data[i].AccidentDocumentId + '">' + data[i].Name + '</option>';
            }
            $("#ddldocumenttype").append(ddlValue);
        }
    });
}

function GetDriverInfo() {

    var id = $("#driverId").val();
    $.ajax({
        url: baseUrl + '/Driver/GetDriverInfoById?DriverId=' + id,
        type: "Get",
        success: function (data) {

            if (data != false) {
                $("#txtEmailId").val(data.EmailId);
                $("#txtPhone").val(data.Phone);
            }
            else {
                $("#txtEmailId").val("");
                $("#txtPhone").val("");
            }
        }
    });
}
function SaveAccidentInfo() {
    debugger
    var values = {};
    values.AccidentReportId = $("#hdnReportAccident").val();
    values.EquipmentId = $("#hdnEquipmentId").val();
    values.Address = $("#txtAddress").val();
    values.DriverId = $("#driverId").val();
    values.AccidentDate = $("#dtAccidentDate").val();
    values.AccidentTime = $("#txtAccidentTime").val();
    values.Comments = $("#txtComment").val();
    values.PoliceReportNo = $("#txtPoliceReportNo").val();

    var urlpath = baseUrl + "/AccidentReport/SaveAccidentReport"
    if (values.AccidentReportId > 0) {
        urlpath = baseUrl + "/AccidentReport/UpdateAccidentReport"
    }

    if (validateContact()) {
        if (values.EquipmentId > 0) {


            $.ajax({
                url: urlpath,
                type: "POST",
                data: JSON.stringify(values),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (response) {
                    isNeedToloaded = false;
                    if (response.IsSuccess) {
                        $("#hdnReportAccident").val(response.Data);
                        //toastr.success(response.Message);
                        //SuccessPopup(response.Message);
                        var encryptedRowIds = Encrypt(response.Data);

                        window.location.href = baseUrl + "/AccidentReport/Index/" + encryptedRowIds;

                    }
                },
                error: function (data) {

                    alert(data);
                }
            });
        }
        else {
            $.alert({
                title: 'Alert!',
                content: '<b>Please select a valid License Plate.</b>',
                type: 'red',
                typeAnimated: true,
            });
        }
    }
}

function CheckDuplicateRecord() {

    var IsExist = true;
    var newDocumentId = $("#ddldocumenttype").val();
    if (newDocumentId != 8) {  // 6=> Other
        var count = 0;
        $("#tblAccidentReportDocument tbody tr").each(function () {
            var oldDocumentId = $(this).find("input[name=documentType]").val();

            if (newDocumentId == oldDocumentId) {
                count = count + 1;

            }
        });
        if (count > 0) {


            //toastr.warning("This Document Type already exist.")
            AlertPopup("This Document Type already exist.")

            IsExist = false;
            return IsExist;
        }
        else {
            return IsExist;
        }
    }
    else {
        return IsExist;
    }

}

function AddRow() {

    if (CheckDuplicateRecord() && CheckImageExtension()) {
        var accidentReportId = $("#hdnReportAccident").val();
        var fileUploader = $("#imgdocument");
        if (fileUploader.length) {
            var filesUploaded = fileUploader[0].files;
            if (accidentReportId > 0) {
                for (let i = 0; i < filesUploaded.length; i++) {
                    var data = new FormData();

                    if (filesUploaded.length) {
                        for (let i = 0; i < filesUploaded.length; i++) {
                            data.append("AccidentImage", filesUploaded[i]);
                        }
                    }
                    data.append("AccidentReportId", accidentReportId);
                    data.append("AccidentDocumentId", $.trim($("#ddldocumenttype").val()));
                    data.append("DocumentName", $.trim($("#txtDocumentName").val()));
                    $.ajax({
                        type: "POST",
                        url: baseUrl + '/AccidentReport/UploadAccidentDocument',
                        contentType: false,
                        processData: false,
                        data: data,
                        async: false,
                        beforeSend: function () {
                            showLoader();
                        },
                        success: function (data, textStatus, jqXHR) {
                            if (data == true) {

                                EditReportAccidentDetail();
                                hideLoader();
                            }
                            hideLoader();

                        },
                        error: function (xhr, status, p3, p4) {

                            var err = "Error " + " " + status + " " + p3 + " " + p4;
                            if (xhr.responseText && xhr.responseText[0] == "{")
                                err = JSON.parse(xhr.responseText).Message;
                            console.log(err);
                            hideLoader();
                        }
                    });
                }
            }
            else {
                // This comment is done previouly before 5Dec 2020
                //toastr.warning("Please click on Save button to save this Accident detail into database.Then you can upload image.").attr('style', 'width: 600px !important');

                //$.alert({
                //    title: 'Alert!',
                //    content: 'Please click on Save button to save this Accident detail into database.Then you can upload image!',
                //});
                ConfirmationPopup("Please click on Save button to save this Accident detail into database.Then you can upload image!");
            }


        }


        ClearUploadDocument();
    }
};

function ClearUploadDocument() {
    $("#imgdocument").val(null);
    $("#txtDocumentName").val("");
}

function encodeImagetoBase64(element) {

    if (CheckImageExtension()) {
        var size = parseFloat(element.files[0].size / 1024).toFixed(2);
        if (size <= 5120) //1048576=1MB
        {
        }
        else {
           // toastr.warning("File must be less then or equal to 5MB")
            AlertPopup("File must be less then or equal to 5MB")
        }
    }

}

//Check Image 
function CheckImageExtension() {
    var accidentReportId = $("#hdnReportAccident").val();
    if (accidentReportId > 0) {


        var Isvalid = false;

        var file = $('input[type="file"]').val();
        if (file != null && file != "") {
            var exts = ['jpg', 'jpeg', 'JPG', 'JPEG', 'png', 'pdf', 'PDF', 'JFIF', 'jfif'];
            // first check if file field has any value
            if (file) {
                // split file name at dot
                var get_ext = file.split('.');
                // reverse name to check extension
                get_ext = get_ext.reverse();
                // check file type is valid as given in 'exts' array
                if ($.inArray(get_ext[0].toLowerCase(), exts) > -1) {
                    Isvalid = true;
                    return Isvalid;
                } else {
                    //toastr.warning('Uploads must be in JPG, JPEG, PNG, PDF, or JFIF formats.');
                    AlertPopup('Uploads must be in JPG, JPEG, PNG, PDF, or JFIF formats.');
                    $("#imgdocument").val(null);
                    return Isvalid;
                }
            }
        }
        else {
            //toastr.warning("Please Browse to select your Upload File.", "")
            AlertPopup("Please Browse to select your Upload File.", "")
            return Isvalid;
        }
    }
    else {
       ConfirmationPopup("Please click on Save button to save this Accident detail into database.Then you can upload image!");

    }
}

//AutoComplete for search by License Plate and VIN
function AutoComplete() {
    $('#txtEquipmentNo').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: baseUrl + "/AccidentReport/SearchByEquipmentNo",
                type: "POST",
                dataType: "json",
                data: { search: request.term },
                success: function (data) {

                    console.log(data);

                    response($.map(data, function (item) {


                        return { label: item.EquipmentNo, value: item.EquipmentNo, track: item };
                    }))
                }
            })
        },
        open: function (event, ui) {
            $(".ui-autocomplete").css("z-index", "2147483647");
            $(".ui-autocomplete").css("width", "300px");
            $(".ui-autocomplete").css("max-height", "450px");
            $(".ui-autocomplete").css("overflow-y", "scroll");
            $(".ui-widget").css("font-size", "12px");
        },
        select: function (event, ui) {
            $("#txtLicencePlate").val(ui.item.track.LicencePlate);
            $("#txtVIN").val(ui.item.track.VIN);
            $("#hdnEquipmentId").val(ui.item.track.EDID);
            $("#txtModel").val(ui.item.track.Model);
            $("#txtYear").val(ui.item.track.Year);
            $("#txtEquipmentNo").val(ui.item.track.EquipmentNo);
            $("#txtVehicleType").val(ui.item.track.VehicleTypeName);

        }
    });
    $('#txtLicencePlate').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: baseUrl + "/AccidentReport/SearchByLicensePlate",
                type: "POST",
                dataType: "json",
                data: { search: request.term },
                success: function (data) {

                    console.log(data);

                    response($.map(data, function (item) {


                        return { label: item.LicencePlate, value: item.LicencePlate, track: item };
                    }))
                }
            })
        },
        open: function (event, ui) {
            $(".ui-autocomplete").css("z-index", "2147483647");
            $(".ui-autocomplete").css("width", "300px");
            $(".ui-autocomplete").css("max-height", "450px");
            $(".ui-autocomplete").css("overflow-y", "scroll");
            $(".ui-widget").css("font-size", "12px");
        },
        select: function (event, ui) {
            $("#txtLicencePlate").val(ui.item.track.LicencePlate);
            $("#txtVIN").val(ui.item.track.VIN);
            $("#hdnEquipmentId").val(ui.item.track.EDID);
            $("#txtModel").val(ui.item.track.Model);
            $("#txtYear").val(ui.item.track.Year);
            $("#txtEquipmentNo").val(ui.item.track.EquipmentNo);
            $("#txtVehicleType").val(ui.item.track.VehicleTypeName);

        }
    });


    $('#txtVIN').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: baseUrl + "/AccidentReport/SearchByVIN",
                type: "POST",
                dataType: "json",
                data: { search: request.term },
                success: function (data) {

                    console.log(data);

                    response($.map(data, function (item) {


                        return { label: item.VIN, value: item.VIN, track: item };
                    }))
                }
            })
        },
        open: function (event, ui) {
            $(".ui-autocomplete").css("z-index", "2147483647");
            $(".ui-autocomplete").css("width", "300px");
            $(".ui-autocomplete").css("max-height", "450px");
            $(".ui-autocomplete").css("overflow-y", "scroll");
            $(".ui-widget").css("font-size", "12px");
        },
        select: function (event, ui) {
            $("#txtLicencePlate").val(ui.item.track.LicencePlate);
            $("#txtVIN").val(ui.item.track.VIN);
            $("#hdnEquipmentId").val(ui.item.track.EDID);
            $("#txtModel").val(ui.item.track.Model);
            $("#txtYear").val(ui.item.track.Year);
            $("#txtEquipmentNo").val(ui.item.track.EquipmentNo);
            $("#txtVehicleType").val(ui.item.track.VehicleTypeName);

        }
    });
}

// Find and remove selected table rows
function remove_row(_this, documentId) {
    var encryptedRowIds = Encrypt(documentId);
    $.confirm({
        title: 'Confirmation!',
        content: '<b>Are you sure you want to Delete ?</b> ',
        type: 'red',
        typeAnimated: true,
        buttons: {
            delete: {
                btnClass: 'btn-blue',
                action: function () {
                    $.ajax({
                        url: baseUrl + 'AccidentReport/DeleteAccidentDocument',
                        data: { "documentId": encryptedRowIds },
                        type: "GET",
                        async: false,
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",

                        success: function (data) {


                            if (data.IsSuccess == true) {
                               // toastr.success(data.Message);
                                SuccessPopup(data.Message);
                                
                                $(_this).closest("tr").remove();
                            }
                            else {
                                //toastr.error(data.Message);
                                AlertPopup(data.Message);
                            }

                        }
                    });
                }
            },
            cancel: {
                //btnClass: 'btn-red',
            }
        }
    })

}




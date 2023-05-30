var isNeedToloaded = true;

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

$(document).ready(function () {

    var driverid = $("#hdfDriverId").val();

    if (driverid > 0 && driverid != undefined) {

        EditDriverDetail();

        //For Mandatory Docs to activate Driver
        //DriverDocsDetail();
    }
    else {
        //Florida=>3929 as default value
        $("#ddlState").val(3929);
    }
    GetDocumetTypeList();

    //var driverid = $("#hdfDriverId").val();
    ////For Mandatory Docs to activate Driver
    //DriverDocsDetail();
});

//#region colour change on grid icon
$("#dvUploadDocs").on("mouseover", 'tr', function () {

    $(this).find(".chng-color-edit").css('color', 'white');
    $(this).find(".chng-color-Trash").css('color', 'white');
});


$("#dvUploadDocs").on("mouseout", 'tr', function () {

    $(this).find(".chng-color-edit").css('color', '#007bff');
    $(this).find(".chng-color-Trash").css('color', 'red');

});
//#endregion
$("#chkIsActive").on('change', function () {
    if ($(this).is(':checked')) {
           $.confirm({
            title: 'Confirmation!',
               content: '<b>Are you sure you want to make this driver active?</b> ',
            type: 'red',
            typeAnimated: true,
            buttons: {
                Active: {
                    btnClass: 'btn-blue',
                    action: function () {
                        $("#chkIsActive").prop("checked", true);
                    }
                },
                cancel: {
                    action: function () {
                        $("#chkIsActive").prop("checked", false);
                    }
                    //btnClass: 'btn-red',
                }
            }
        })
    }
    else {
        $.confirm({
            title: 'Confirmation!',
            content: '<b>Are you sure you want to make this driver inactive?</b> ',
            type: 'red',
            typeAnimated: true,
            buttons: {
                InActive: {
                    btnClass: 'btn-blue',
                    action: function () {
                        $("#chkIsActive").prop("checked", false);
                    }
                },
                cancel: {
                    action: function () {
                        $("#chkIsActive").prop("checked", true);
                    }
                    
                }
            }
        })
    }

});


function GetCityList() {

    var stateId = $("#ddState").val();

    $.ajax({
        url: baseUrl + '/Driver/GetCityList',
        data: { 'StateId': stateId },
        type: "POST",
        // cache: false,
        success: function (data) {

            var ddlValue = "";
            $("#ddlcity").empty();

            ddlValue += '<option value="0">Please Select City</option>'
            for (var i = 0; i < data.length; i++) {
                ddlValue += '<option value="' + data[i].ID + '">' + data[i].Name + '</option>';
            }
            $("#ddlcity").append(ddlValue);
        }
    });
}

function GetDocumetTypeList() {



    $.ajax({
        url: baseUrl + '/Driver/GetDocumetTypeList',
        type: "GET",
        // cache: false,
        success: function (data) {

            var ddlValue = "";
            $("#ddldocumenttype").empty();

            //ddlValue += '<option value="0">Select Document Type</option>'
            for (var i = 0; i < data.length; i++) {
                ddlValue += '<option value="' + data[i].ID + '">' + data[i].Name + '</option>';
            }
            $("#ddldocumenttype").append(ddlValue);
        }
    });
}

function DriverEdit(listId) {

    $.ajax({
        url: baseUrl + '/Driver/Edit',
        data: { 'id': listId },
        type: "POST",
        // cache: false,
        success: function (data) {
            console.log(data);
            $("#DriverID").val(listId);
            $("#FirstName").val(data.FirstName);
            $("#MiddleName").val(data.MiddleName);
            $("#LastName").val(data.LastName);
            $("#Address1").val(data.Address1);
            $("#Address2").val(data.Address2);
            $("#City").val(data.city);
            $("#State").val(data.State);

            //if (data.IsActive == true) {
            //    $("#IsActive").prop("checked", true);
            //}
            //else {
            //    $("#IsActive").prop("checked", false);
            //}
            $("#BtnDriver").hide();
            $("#BtnUpdate").removeClass("hidden");

        }
    });
}

function EditDriverDetail() {
    debugger;
    
    var driverid = $("#hdfDriverId").val();
    $.ajax({
        url: baseUrl + "/Driver/GetDriverInfoById",
        type: "Get",
        data: { DriverId: driverid },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != false) {
                debugger
                $("#txtFirstName").val(response.FirstName);
                $("#txtLastName").val(response.LastName);
                $("#txtAddress1").val(response.Address1);
                $("#txtAddress2").val(response.Address2);
                $("#txtCity").val(response.City);
                $("#txtPhone").val(response.Phone);
                $("#ddlState").val(response.State);
                $("#txtZipCode").val(response.ZipCode);
                $("#ddlCountry").val(response.Country);
                $("#txtCellNumber").val(response.CellNumber);
                $("#txtEmailId").val(response.EmailId);
                $("#txtExtension").val(response.Extension);
                $("#ddlBloodGroup").val(response.BloodGroup);
                $("#txtMedicalConditions").val(response.MedicalConditions);
                $("#txtSTANumber").val(response.STANumber);
                $("#txtExpirationDate").val(ConvertDate(response.ExpirationDate));
                $("#ddlVehicle").val(response.Vehicle);
                $("#txtEmergencyContactOne").val(response.EmergencyContactOne);
                $("#txtEmergencyPhoneNoOne").val(response.EmergencyPhoneNoOne);
                $("#RelationshipStatus1").val(response.RelationshipStatus1);
                $("#txtCitizenShip").val(response.CitizenShip);
                $("#txtEmergencyContactTwo").val(response.EmergencyContactTwo);
                $("#txtEmergencyPhoneNoTwo").val(response.EmergencyPhoneNoTwo);
                $("#RelationshipStatus2").val(response.RelationshipStatus2);
                if (response.LanguageId != null) {
                    $("#ddlLanguage").val(response.LanguageId);
                }
                else {
                    $("#ddlLanguage").val(1);
                }
              
                debugger;
                if (response.IsActive == true) {
                    $("#chkIsActive").prop("checked", true);
                }
                else {
                    $("#chkIsActive").prop("checked", false);
                }
                $("table tbody").empty();
                if (response.DriverDocumentList != null) {
                    for (var i = 0; i < response.DriverDocumentList.length; i++) {

                        var markup = "<tr><td>" + response.DriverDocumentList[i].DocumentTypeName + "</td><td><input type='hidden' name='DocumentId' value='" + response.DriverDocumentList[i].DocumentId + "'/><input type='hidden' name='uploadDocument' value=''/><input type='hidden' name='documentType' value='" + response.DriverDocumentList[i].DocumentTypeId + "'/><input type='hidden' name='documentName' value='" + response.DriverDocumentList[i].DocumentName + "'/>" + response.DriverDocumentList[i].DocumentName + "</td> <td><input type='hidden' name='fileName' value='" + response.DriverDocumentList[i].ImageName + "' />" + response.DriverDocumentList[i].ImageName + "</td><td><input type='hidden' name='expiryDate' value='" + ConvertDate(response.DriverDocumentList[i].DocumentExpiryDate) + "' />" + ConvertDate(response.DriverDocumentList[i].DocumentExpiryDate) + "</td><td><a href='javascript:void(0)' onclick='javascript: ViewDamageDocument(this);' data-file-url=" + response.DriverDocumentList[i].ImageURL+" title='preview' class='edit_icon chng-color-edit fileViewer'> <i class='far fa-eye'></i></a> | <button type='button' class='edit_icon chng-color-Trash' name='delete-row' value='Remove' onclick='remove_row(this," + response.DriverDocumentList[i].DocumentId + ")'> <i class='far fa-trash-alt'></i> </button></td></tr>";
                        $("table tbody").append(markup);

                    }
                }
            }
        },
        error: function () {

        }
    });
}

function ConvertJsonDateToDate(jsondate) {

    var parsedDate = new Date(parseInt(jsondate.substr(6)));
    var jsDate = new Date(parsedDate);
    var date = jsDate.getDate();
    if (date < 10) {
        date = '0' + date;
    }
    var month = jsDate.getMonth()
    if (month < 10) {
        month = '0' + month;
    }
    var date = jsDate.getFullYear() + "-" + month + "-" + date
    return date;
}

function AddRow() {

    if (CheckDuplicateRecord() && CheckImageExtension()) {
        var driverId = $("#hdfDriverId").val();
        var fileUploader = $("#imgdocument");
        if (fileUploader.length) {
            var filesUploaded = fileUploader[0].files;
            if (driverId > 0) {
                for (let i = 0; i < filesUploaded.length; i++) {
                    var data = new FormData();

                    if (filesUploaded.length) {
                        for (let i = 0; i < filesUploaded.length; i++) {
                            data.append("DriverDocument", filesUploaded[i]);
                        }
                    }
                    //var documenttype = $("#ddldocumenttype").val();
                    //var documenttext = $('#ddldocumenttype').find('option:selected').text();
                    //var documentname = $("#txtDocumentName").val();
                    var expirydate = $("#dtExpiryDate").val();
                    data.append("DriverId", driverId);
                    data.append("DocumentType", $("#ddldocumenttype").val());
                    debugger

                    var documentName = $("#txtDocumentName").val();
                    if (documentName != "") {


                        if ($("#txtDocumentName").val() != "") {

                            data.append("DocumentName", $("#txtDocumentName").val());
                        }
                        else {
                            data.append("DocumentName", $("#txtDocumentName").find('option:selected').text());

                        }

                        data.append("ExpiryDate", $("#dtExpiryDate").val());
                        if (expirydate != null && expirydate != "") {
                            $.ajax({
                                type: "POST",
                                url: baseUrl + '/Driver/UploadDriverDocument',
                                contentType: false,
                                processData: false,
                                data: data,
                                async: false,
                                beforeSend: function () {
                                    showLoader();
                                },
                                success: function (data, textStatus, jqXHR) {

                                    isNeedToloaded = false;

                                    DriverDocsDetail();
                                    if (data == true) {

                                        setInterval(function () {
                                            window.location.href = baseUrl + "Driver/Index/" + driverId;
                                        }, 2000)


                                        hideLoader();
                                    }
                                    else {

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
                      

                        AlertPopup("Please Enter Document Name");
                    }
                }
            }

            else {

                $.alert({
                    title: 'Alert!',
                    content: '<b>Please click on Save button to save this Driver detail into database.Then you can upload file!</b>',
                    type: 'blue',
                    typeAnimated: true,
                });
                //$.alert({
                //    title: 'Alert!',
                //    content: 'Please click on Save button to save this Accident detail into database.Then you can upload file!',
                //});
            }


        }


        ClearUploadDocument();
    }
};

function ClearUploadDocument() {
    $("#imgdocument").val(null);
    $("#txtDocumentName").val("");
}

// Find and remove selected table rows
function remove_row(_this, documentId) {
    var driverid = $("#hdfDriverId").val();

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
                        url: baseUrl + 'Driver/DeletetDriverDocument',
                        data: { "documentId": documentId },
                        type: "GET",
                        async: false,
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",

                        success: function (data) {
                            isNeedToloaded = false;
                            DriverDocsDetail();
                            if (data.IsSuccess == true) {
                                //toastr.success(data.Message);
                                $(_this).closest("tr").remove();

                                $.alert({
                                    title: 'Success!',
                                    content: "<b>" + data.Message + "</b>",
                                    type: 'green',
                                    typeAnimated: true,
                                    buttons: {
                                        Ok: {
                                            btnClass: 'btn-green',
                                            action: function () {
                                                window.location.href = baseUrl + "Driver/Index/" + driverid;
                                            }
                                        },
                                    }
                                });
                              
                            }
                            else {
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

function encodeImagetoBase64(element) {

    if (CheckImageExtension()) {
        var size = parseFloat(element.files[0].size / 1024).toFixed(2);
        if (size <= 5120) //1048576=1MB //5120
        {
            //var file = element.files[0];
            //var reader = new FileReader();
            //reader.onloadend = function () {
            //    $("#hdnimgdocument").attr("Value", reader.result);
            //    $("#hdnimgdocument").text(reader.result);
            //}
            //reader.readAsDataURL(file);
        }
        else {
            AlertPopup("File must be less then or equal to 1MB")
        }
    }

}

function SaveDrierDetail() {
    debugger;
    var values = {};

    values.DriverId = $("#hdfDriverId").val();
    values.FirstName = $("#txtFirstName").val();
    values.LastName = $("#txtLastName").val();
    values.Address1 = $("#txtAddress1").val();
    values.Address2 = $("#txtAddress2").val();
    values.CitizenShip = $.trim($("#txtCitizenShip").val());
    values.City = $("#txtCity").val();
    values.Phone = $("#txtPhone").val();
    values.State = $("#ddlState").val();
    values.ZipCode = $("#txtZipCode").val();
    values.Country = $("#ddlCountry").val();
    values.CellNumber = $("#txtCellNumber").val();
    values.EmailId = $("#txtEmailId").val();
    values.Extension = $("#txtExtension").val();
    values.BloodGroup = $("#ddlBloodGroup").val();
    values.MedicalConditions = $("#txtMedicalConditions").val();
    values.STANumber = $("#txtSTANumber").val();
    values.ExpirationDate = $("#txtExpirationDate").val();
    values.Vehicle = $("#ddlVehicle").val();
    values.EmergencyContactOne = $("#txtEmergencyContactOne").val();
    values.EmergencyPhoneNoOne = $("#txtEmergencyPhoneNoOne").val();
    values.RelationshipStatus1 = $("#RelationshipStatus1").val();

    values.EmergencyContactTwo = $("#txtEmergencyContactTwo").val();
    values.EmergencyPhoneNoTwo = $("#txtEmergencyPhoneNoTwo").val();
    values.RelationshipStatus2 = $("#RelationshipStatus2").val();
    values.LanguageId = $("#ddlLanguage").val();

    if ($("#chkIsActive").prop("checked") == true) {
        values.IsActive = 1;
    }
    else {
        values.IsActive = 0;
    }
    //if ($("form").valid()) {
    var urlpath = baseUrl + "/Driver/Index"
    if (values.DriverId > 0) {
        urlpath = baseUrl + "/Driver/UpdateDriver"
    }
    if (validateContact()) {

        $.ajax({
            url: urlpath,
            type: "POST",
            data: JSON.stringify(values),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                showLoader();
            },
            success: function (response) {
                isNeedToloaded = false;
                if (response.IsSuccess) {
                    hideLoader();
                

                    $("#hdfDriverId").val(response.Data);
                    //toastr.success(response.Message);
                    //window.location.href = baseUrl + "/Driver/Index/" + response.Data;
                    window.location.href = baseUrl + "/Driver/ViewDriver";
                }
                else {
                    hideLoader();
                    AlertPopup(response.Message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                hideLoader();
            }
        });

    }

}

function CheckDuplicateRecord() {
    var IsExist = true;
    var newDocumentId = $("#ddldocumenttype").val();
    if (newDocumentId != 7) {  // 7=> Other
        var count = 0;
        $("#tblDriverDocument tbody tr").each(function () {
            var oldDocumentId = $(this).find("input[name=documentType]").val();
            if (newDocumentId == oldDocumentId) {
                count = count + 1;

            }
        });
        if (count > 0) {

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

function CheckImageExtension() {
    var driverId = $("#hdfDriverId").val();
    var Isvalid = false;
    if (driverId > 0) {
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
                    // alert('File extension must be JPG or JPEG or PNG.');
                    AlertPopup('Uploads must be in JPG, JPEG, PNG, PDF, or JFIF formats.');
                    $("#imgdocument").val(null);
                    return Isvalid;
                }
            }
        }
        else {
            AlertPopup("Please Browse to select your Upload File.", "")
            return Isvalid;
        }
    }
    else {

        $.alert({
            title: 'Alert!',
            content: '<b>Please click on Save button to save this Driver detail into database.Then you can upload file!</b>',
            type: 'blue',
            typeAnimated: true,
        });
        //$.alert({
        //    title: 'Alert!',
        //    content: 'Please click on Save button to save this Driver detail into database.Then you can upload file!',
        //});
    }
}


function DriverDocsDetail() {
    debugger;
    var driverid = $("#hdfDriverId").val();
    $.ajax({
        url: baseUrl + "/Driver/DriverDocumetsType",
        type: "Get",
        data: { "driverID": driverid },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response != false) {
                debugger;
                if (response.IsActive == true) {
                    $("#chkIsActive").prop("checked", true);
                    setInterval(function () {
                        window.location.href = baseUrl + "Driver/Index/" + driverid;
                    }, 2000)
                }

                else {
                    $("#chkIsActive").prop("checked", false);
                    setInterval(function () {
                        window.location.href = baseUrl + "Driver/Index/" + driverid;
                    }, 2000)
                }

            }
        },
        error: function () {

        }
    });
}


//#region View Document
function ViewDamageDocument(_this) {

    var fileUrl = $(_this).attr("data-file-url");
    if (fileUrl != undefined) {
        var extn = fileUrl.substring(fileUrl.lastIndexOf('.') + 1);

        var isImg = isExtension(extn, _imgExts);
        var $divViewer = $("#divViewer");
        var docHeight = $(document).height();
        if (isImg) {
            var width;
            var widthAuto = '';
            $("<img/>", {
                load: function () {
                    width = this.width;
                    if (width > 770) {
                        widthAuto = 'auto';
                    }
                    else {
                        widthAuto = width + 'px';
                    }
                    //var imgTag = '<img src="' + fileUrl + '" style="width:' + widthAuto+ 'px;height:' + (h - 110) + 'px;" class="img-fluid" />';
                    var imgTag = '<img src="' + fileUrl + '" style="width:' + widthAuto + ';" class="img-fluid" />';
                    $divViewer.html(imgTag);
                },
                src: fileUrl
            });



        }
        else {
            var docHeight = $(document).height();
            var iframe = '<iframe id="myiframe" src="' + fileUrl + '" style="width: 100%;height:' + (docHeight - 200) + 'px"></iframe>';
            $divViewer.html(iframe);
        }
        $("#modalDocument").modal("show");
    }
}


//#endregion

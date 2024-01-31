var glbDamageFile = [];
var glbProofOfTempFile = [];
var glbDamageFileCollection = [];
var damageDescriptionCollection = [];
var glbGpsHistory = [];
var glbarEvent = [];
var glbarWaitingTime = [];
var pickDel = []
var isSigned = false;
var ShipmentRouts = [];
var selectedStatusIndex = 0;
var selectedStatusText = 0;
var actualPickupArrivedHasValue = "";
var isValidationSucess = true;
var isProofofTemp = false;
var isDamageDetail = false;
var IsTemperatureRequired = false;
//#region Ready State
$(function () {
    // $("#btnSave").css("background", "linear-gradient(to right, #196fcc 0%, #1cbde2 100%)");

    hideSwapButton(1);

    var lat;
    var long;
    var formatedCurrentAddress;

    getLocation();

    btnUpload();

    btnUploadDamaged();
    //bind shipment status dropdown
    shipmentStatus();

    // For Bind the Reson Text box
    ReasonBox();

    // For Converting the Temperature
    convertTemp();

    // For Save the form
    // btnSave();

    // For Shipment Route Details Details Radio Button 
    checkShipmentRadionButton();

    // get Direction Routes
    //initMap();

    // For Save GPS Tracking History
    window.setInterval(getLateLong, 3000);

    // Get shipment commment deail

    GetShipmentComments();


    // change shipment Route link colour
    $('.tblChange-bgcolor').hover(

        function () {
            $(this).find(".chng-color-Pickup").css('color', 'white');
            $(this).find(".chng-color-Deli").css('color', 'white');
        },

        function () {
            $(this).find(".chng-color-Pickup").css('color', '#007bff');
            $(this).find(".chng-color-Deli").css('color', '#007bff');
        }
    );

    //#region colour change on grid icon
    $(".tblcolourChange").on("mouseover", 'tr', function () {
        $(this).find(".chng-color-view").css('color', 'white');
        $(this).find(".chng-color-Trash").css('color', 'white');
    });

    $(".tblcolourChange").on("mouseout", 'tr', function () {
        $(this).find(".chng-color-view").css('color', '#007bff');
        $(this).find(".chng-color-Trash").css('color', 'red');
    });
    // $(".onoffswitch-switch").hide();
    //$(".btnSave").css("color", "green");

});
//#endregion


function DisplayRouteDetail() {

    var rowCount = $('#tblShipmentRoute tbody tr').length;
    if ($("#ddlShipmentStatus").val() != 12) {
        if (rowCount < 2) {
            $("#divRouteDetail").hide();
        }
    }
}

$(".divSwap").change(function () {

    btnSave();
})
function hideSwapButton(Id) {

    if (Id == 1) {

        $("#divSwapSubmit").show();
        $("#divSwapTryAgain").hide();
        $("#divSwapSuccess").hide();
    }
    else if (Id == 2) {

        $("#divSwapSubmit").hide();

        $("#divSwapSuccess").hide();
        $('#swapTryAgain1').load(location.href + " #divSwapTryAgain");
        $("#divSwapTryAgain").show();
        $("#swapTryAgain1").show();
    }
    else {
        $("#divSwapSubmit").hide();
        $("#divSwapTryAgain").hide();
        $("#divSwapSuccess").show();
    }
}
//#region Button Upload
var btnSave = function () {
    //$("#btnSave").on("click", function () {

    var isValidationSuccess = statusChangeValidations();
    // alert('isValidationSuccess == '+isValidationSuccess+' and isValidationSucess = ' + isValidationSucess)
    var mendetory = false;
    var descriptionMendetory = false;
    if (($("#ddlShipmentStatus").val() == 4) || ($("#ddlShipmentStatus").val() == 3) || ($("#ddlShipmentStatus").val() == 13)) {
        if ($("#ddlShipmentSubStatus").val() == "") {
            mendetory = true;
        }
    }
    if (($("#ddlShipmentStatus").val() == 4) || ($("#ddlShipmentStatus").val() == 3)) {
        if (($("#ddlShipmentSubStatus").val() == 7 || $("#ddlShipmentSubStatus").val() == 11) && $("#txtOtherReason").val() == "") {
            descriptionMendetory = true;
        }
    }
    // if (isValidationSuccess && isValidationSucess) {
    if (isValidationSuccess) {
        if (!mendetory) {
            if (!descriptionMendetory) {
                if ($("#ddlShipmentStatus").val() == 6) {
                    if ($("#txtActualPickupDepart").val() != "" && $("#txtActualPickupDepart").val() != undefined) {
                        $.confirm({
                            title: 'Confirmation!',
                            content: 'Remember to attach the appropriate seals to your equipment before leaving',
                            type: 'blue',
                            typeAnimated: true,
                            buttons: {
                                OK: {
                                    btnClass: 'btn-green',
                                    action: function () {
                                        saveShipmentDetail();
                                    }
                                },


                            }
                        });
                    }
                    else {
                        saveShipmentDetail();
                    }
                }
                else {
                    saveShipmentDetail();
                }

                // FOR Driver GPS Tracking history 
                fn_SaveGPSTracker();
                // For Save Wating time Notification
                fn_SaveWaitingTime();
            }
            else {
                //$.alert({
                //    title: 'Alert!',
                //    content: 'You selected "Other" as your sub-status. Please enter a brief description of the problem.',
                //    type: 'red',
                //    typeAnimated: true,
                //});
                toastr.error('You selected "Other" as your sub-status. Please enter a brief description of the problem')
                $("#txtOtherReason").addClass('highlight');
                //$("#btnSave").css("background", "#eb6865");
                //$("#btnSave").attr('value', 'TRY AGAIN');
                hideSwapButton(2)
            }
        }
        else {
            //$.alert({
            //    title: 'Alert!',
            //    content: 'Please select a sub-status.',
            //    type: 'red',
            //    typeAnimated: true,
            //});
            toastr.error('Please select a sub-status.')
            $("#ddlShipmentSubStatus").focus();
            //$("#btnSave").css("background", "#eb6865");
            //$("#btnSave").attr('value', 'TRY AGAIN');
            hideSwapButton(2)
        }
    }
    else {
        //$("#btnSave").css("background", "#eb6865");
        //$("#btnSave").attr('value', 'TRY AGAIN');
        hideSwapButton(2)

        //toastr.warning('Data not saved. Please complete validation first');
        //$("#ddlShipmentStatus").val(selectedStatusIndex);
    }
    //})
}
//#endregion

function saveShipmentDetail() {

    var data = new FormData();
    var ShipmentFreightDetailId = $("input[name='radiofreightId']:checked").val();
    var ShipmentId = $.trim($("#hdShipmentId").val());
    var shipmentRouteId = $("input[name='radioShipmentRouteId']:checked").val();
    var ActualPickupArrived = $.trim($("#txtActualPickupArrived").val());
    var ActualPickupDepart = $.trim($("#txtActualPickupDepart").val());
    var ActualDeliveryArrived = $.trim($("#txtActualDeliveryArrived").val());
    var ActualDeliveryDepart = $.trim($("#txtActualDeliveryDepart").val());
    var ReceiverName = $.trim($("#txtReceiverName").val());
    var ShipmentStatus = $.trim($("#ddlShipmentStatus").val());
    var ShipmentSubStatus = $.trim($("#ddlShipmentSubStatus").val());
    var OtherReason = $.trim($("#txtOtherReason").val());

    var shipmentComment = $.trim($("#txtShipmentComments").val());

    if ($("#ddlShipmentSubStatus").val() != 11 || $("#ddlShipmentSubStatus").val() != 7) {
        OtherReason = "";
    }
    if (glbDamageFileCollection.length) {

        for (let i = 0; i < glbDamageFileCollection.length; i++) {
            for (let j = 0; j < glbDamageFileCollection[i].length; j++) {
                data.append("DamagedFiles", glbDamageFileCollection[i][j]);

            }
        }
    }

    data.append("ShipmentFreightDetailId", ShipmentFreightDetailId);
    data.append("ShipmentId", ShipmentId);
    data.append("ShipmentRouteId", shipmentRouteId);
    data.append("DriverPickupArrival", ActualPickupArrived);
    data.append("DriverPickupDeparture", ActualPickupDepart);
    data.append("DriverDeliveryArrival", ActualDeliveryArrived);
    data.append("DriverDeliveryDeparture", ActualDeliveryDepart);
    data.append("ReceiverName", ReceiverName);
    data.append("StatusId", ShipmentStatus);
    data.append("SubStatusId", ShipmentSubStatus);
    data.append("Reason", OtherReason);
    data.append("ShipmentComment", shipmentComment);
    $.ajax({
        type: "POST",
        url: baseUrl + '/Driver/SavePreTripShipmentDetail',
        beforeSend: function () {
            showLoader();
        },
        contentType: false,
        processData: false,
        data: data,
        success: function (data, textStatus, jqXHR) {
            hideLoader();
            if (data.IsSuccess == true) {
                toastr.success(data.Message);
                //$("#btnSave").css("background", "#6bb671");
                //$("#btnSave").attr('value', 'SUCCESS');
                hideSwapButton(3)
                setInterval(function () {
                    window.location.href = baseUrl + "Driver/Dashboard";
                }, 2000)
            }
            else {
                toastr.error(data.Message);
            }

        },
        error: function (xhr, status, p3, p4) {
            hideLoader();
            var err = "Error " + " " + status + " " + p3 + " " + p4;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).Message;
            console.log(err);
        }
    });

}

//#region  Show Hide
var HideSubstatus = function (status, subStatus) {
    
    //#region  Sub status drop down and discription
    $("#subStatusDiv").hide();
    if (status == 13) {
        $("#subStatusDiv").show();
        $('#txtOtherReasonDiv').hide();
        $('#PickupArrivalDiv').hide();
        $('#divDamage').hide();
        $('#divProofOfTemp').hide();
        $('#DelDepartDiv').hide();
        $('#PickupDepartDiv').hide();
        $('#DelArrivalDiv').hide();
        $('#ReceiverNameDiv').hide();
        $('#SignatureDiv').hide();

        $(".divProofOfTemps").hide();

        if (status == 3) {
            $('#divDamage').show();

        }
        else {
            $('#divDamage').hide();
        }
        if (isDamageDetail) {
            $(".divDamageFile").show();
        }
        else {
            $(".divDamageFile").hide();
        }

    }

    if (status == 3 || status == 4) {
        $("#subStatusDiv").show();
        $('#txtOtherReasonDiv').hide();

        $('#PickupArrivalDiv').hide();
        $('#divDamage').hide();
        $('#divProofOfTemp').hide();
        $('#DelDepartDiv').hide();
        $('#PickupDepartDiv').hide();
        $('#DelArrivalDiv').hide();
        $('#ReceiverNameDiv').hide();
        $('#SignatureDiv').hide();

        $(".divProofOfTemps").hide();

        if (status == 3) {
            $('#divDamage').show();

        }
        else {
            $('#divDamage').hide();
        }
        if (isDamageDetail) {
            $(".divDamageFile").show();
        }
        else {
            $(".divDamageFile").hide();
        }

    }

    if ((subStatus == 11 || subStatus == 7) && (status == 3 || status == 4)) {
        $('#txtOtherReasonDiv').show();


        $('#PickupArrivalDiv').hide();
        $('#divDamage').hide();
        $('#divProofOfTemp').hide();
        $('#DelDepartDiv').hide();
        $('#PickupDepartDiv').hide();
        $('#DelArrivalDiv').hide();
        $('#ReceiverNameDiv').hide();
        $('#SignatureDiv').hide();
        if (status == 3) {
            $('#divDamage').show();

        }
        else {
            $('#divDamage').hide();

        }

        if (isDamageDetail) {
            $(".divDamageFile").show();
        }
        else {
            $(".divDamageFile").hide();
        }
    }
    else {
        $('#txtOtherReasonDiv').hide();
    }
    //#endregion

    //#region  first step if status is "DISPATCHED"

    if (status == 12 && actualPickupArrivedHasValue != "") //DISPATCHED 
    {
        $('#PickupArrivalDiv').show();
        $('#divDamage').hide();
        $(".divDamageFile").hide();
        $('#divProofOfTemp').show();
        $('#DelDepartDiv').hide();
        $('#PickupDepartDiv').hide();
        $('#DelArrivalDiv').hide();
        $('#ReceiverNameDiv').hide();
        $('#SignatureDiv').hide();
        var FreightRadioValue = $("input[name='radiofreightId']:checked").val();
        var radioValue = $("input[name='radioShipmentRouteId']:checked").val();
        var ProofFiles = glbProofOfTempFile.filter(x => x.ShipmentFreightDetailId == FreightRadioValue && x.shipmentRouteId == radioValue && x.IsLoading == true);
        if (ProofFiles.length > 0) {
            $('#PickupDepartDiv').show();
        }
        if (isProofofTemp) {
            $(".divProofOfTemps").show();
        }

    }
    else if (status == 12 && actualPickupArrivedHasValue == "") //DISPATCHED
    {
        $('#PickupArrivalDiv').show();
        $('#DelDepartDiv').hide();
        $('#divDamage').hide();
        $(".divDamageFile").hide();
        $('#PickupDepartDiv').hide();
        $('#divProofOfTemp').hide();
        $('#DelArrivalDiv').hide();
        $('#ReceiverNameDiv').hide();
        $('#SignatureDiv').hide();

        if (isProofofTemp) {
            $(".divProofOfTemps").show();
        }
        // $("#ddlShipmentStatus option[value=" + 5 + "]").remove(); //removing Loading status
    }
    else if (status == 5) //LOADING AT PICK UP LOCATION
    {
        $('#PickupArrivalDiv').show();
        $('#divDamage').hide();

        $('#divProofOfTemp').show();

        $('#DelDepartDiv').hide();
        $('#DelArrivalDiv').hide();
        $('#ReceiverNameDiv').hide();
        $('#SignatureDiv').hide();
        $('#PickupDepartDiv').hide();
        //if (actualPickupArrivedHasValue == "") {
        //    $('#PickupArrivalDiv').show();
        //}
        if (isProofofTemp) {
            $(".divProofOfTemps").show();
        }
        if (isDamageDetail) {
            $(".divDamageFile").show();
        }
        else {
            $(".divDamageFile").hide();
        }

    }
    else if (status == 6)  //IN-ROUTE TO DELIVERY LOCATION
    {
        $('#PickupArrivalDiv').show();
        $('#PickupDepartDiv').show();

        $('#divDamage').hide();
        $(".divDamageFile").hide();

        if ($("#txtActualPickupDepart").val() == "" || $("#txtActualPickupDepart").val() == undefined) {
            $('#divProofOfTemp').show();
        }
        else {
            $('#divProofOfTemp').hide();
        }


        $('#DelArrivalDiv').hide();
        $('#ReceiverNameDiv').hide();
        $('#SignatureDiv').hide();
        $('#DelDepartDiv').hide();
        if (isProofofTemp) {
            $(".divProofOfTemps").show();
        }
        else {
            $('.divProofOfTemps').hide();
        }
        if (isDamageDetail) {
            $(".divDamageFile").show();
        }
        else {
            $(".divDamageFile").hide();
        }
    }
    else if (status == 7) //DELIVERED
    {
        $('#PickupArrivalDiv').hide();
        $('#PickupDepartDiv').hide();
        $('#DelDepartDiv').show();
        $('#DelArrivalDiv').show();
        $('#ReceiverNameDiv').show();
        $('#SignatureDiv').show();
        $('#divDamage').hide();
        $(".divDamageFile").hide();

        $('#divProofOfTemp').hide();

        if ($("#txtActualDeliveryDepart").val() != "" && $("#txtActualDeliveryDepart").val() != undefined) {
            $("#SignatureButton").hide();
        }
        else {
            $("#SignatureButton").show();
        }

        if (!isProofofTemp) {
            $(".divProofOfTemps").show();
        }

        if (isDamageDetail) {
            $(".divDamageFile").show();
        }
        else {
            $(".divDamageFile").hide();
        }
    }
    //#endregion        changeBorderColor();

};
//#endregion

//#region convert Temperture F to C
var convertTemp = function () {
    var actualTemp;
    $("#ddlTemperatureUnit").on("change", function () {
        var unit = $(this).val();
        var temp = $("#txtActualTemp").val();
        if (temp != '' && !isNaN(temp)) {
            if (unit == 'C') {
                actualTemp = FahrenheitToCelsius(temp);
            }
            else {
                actualTemp = CelsiusToFahrenheit(temp);
            }

            $("#txtActualTemp").val(actualTemp)
        }
    })
}
//#endregion

//#region Button Upload
var btnUpload = function () {
    $(".btnProofOfTemp").on("click", function () {
        if (isFormValid('divProofOfTemp')) {

            if ($("#txtActualTemp").val() == "") {

                $("#btnProofOfTemp").css("background", "linear-gradient(90deg, rgba(245,31,31,1) 0%, rgba(241,116,116,1) 100%)");
                $("#btnProofOfTemp").text('TRY AGAIN');
                toastr.error("Please enter your shipment's loading temperature.");

            }
            else {
                $("#btnProofOfTemp").prop('disabled', true);
                var fileUploader = $("#fuProofOfTemperature")[0].files;
                var actualTemp = $("#txtActualTemp").val();;
                var data = new FormData();
                if (actualTemp != "" && !isNaN(actualTemp)) {

                    var unit = $("#ddlTemperatureUnit").val();
                    if (unit == 'C') {
                        actualTemp = CelsiusToFahrenheit(actualTemp);
                    }

                }
                data.append("IsLoading", true);
                // Date Time Format 
                var d = new Date($.now());

                date = ((d.getMonth() + 1) + "-" + d.getDate() + "-" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds());
                //
                var FreightRadioValue = $("input[name='radiofreightId']:checked").val();
                var radioValue = $("input[name='radioShipmentRouteId']:checked").val();

                if (fileUploader.length) {
                    for (let i = 0; i < fileUploader.length; i++) {
                        data.append("UploadedTemperatureProofFiles", fileUploader[i]);
                    }
                }
                data.append("ActualTemperature", actualTemp);
                data.append("ShipmentRouteId", radioValue);
                data.append("ShipmentFreightDetailId", FreightRadioValue);
                data.append("AWBPOORD", $("#txtAwbPoOrd").val());
                showLoader();
                $.ajax({
                    type: "POST",
                    url: baseUrl + '/Driver/SaveProofOfTemperature',
                    contentType: false,
                    processData: false,
                    data: data,
                    async: false,
                    success: function (data, textStatus, jqXHR) {
                        hideLoader();

                        if (data.IsSuccess == true) {
                            $("#btnProofOfTemp").css("background", "#7ca337");
                            $("#btnProofOfTemp").html('<i class="fa fa-check-circle fa-lg" aria-hidden="true"></i> SUCCESS');
                            toastr.success(data.Message);
                            GetShipmentProofOfTempFiles(radioValue, FreightRadioValue);
                        }
                        else {
                            toastr.error(data.Message);
                        }
                        setTimeout(function () {

                            $("#btnProofOfTemp").css("background", "#7ca337");
                            $("#btnProofOfTemp").html('UPLOAD');
                            $("#lblfuProofOfTemperature").css("background", "#7ca337");
                            $("#lblfuProofOfTemperature").html('TAKE PICTURE');
                            $("#btnProofOfTemp").prop('disabled', false);


                        }, 3000);



                    },
                    error: function (xhr, status, p3, p4) {
                        hideLoader();
                        $("#btnProofOfTemp").prop('disabled', false);
                        var err = "Error " + " " + status + " " + p3 + " " + p4;
                        if (xhr.responseText && xhr.responseText[0] == "{")
                            err = JSON.parse(xhr.responseText).Message;
                        console.log(err);
                        $("#btnProofOfTemp").css("background", "linear-gradient(90deg, rgba(245,31,31,1) 0%, rgba(241,116,116,1) 100%)");
                        $("#btnProofOfTemp").text('TRY AGAIN');
                    }
                });

                $("#txtActualTemp").val("");
                $("#fuProofOfTemperature").val("");
                bindProofOfTempFileTbl();
                changeBorderColor();
            }
        }
        else {
            $("#btnProofOfTemp").css("background", "linear-gradient(90deg, rgba(245,31,31,1) 0%, rgba(241,116,116,1) 100%)");
            $("#btnProofOfTemp").text('TRY AGAIN');
            toastr.error("Please upload your shipment's loading proof of temperature.");
        }
    })


}
//#endregion

//#region Bind Proof Of Temp files 

function bindProofOfTempFileTbl() {

    var tr = "";
    $("#tblProofOfTemp tbody").empty();

    var FreightRadioValue = $("input[name='radiofreightId']:checked").val();
    var radioValue = $("input[name='radioShipmentRouteId']:checked").val();

    if (radioValue > 0 && FreightRadioValue > 0) {
        var ProofFiles = glbProofOfTempFile.filter(x => x.ShipmentFreightDetailId == FreightRadioValue && x.shipmentRouteId == radioValue);
        if (ProofFiles.length > 0) {
            $(".divProofOfTemps").show();
            for (var i = 0; i < ProofFiles.length; i++) {
                isProofofTemp = true;
                var location = ProofFiles[i].IsLoading == true ? "Loading" : "Delivery";
                tr += '<tr data-file-url=' + ProofFiles[i].ImageUrl + ' ondblclick="javascript:ViewDocument(this)">' +

                    '<td>' + ProofFiles[i].actualTemp + '</td>' +
                    '<td>' + location + '</td>' +
                    '<td>' + ProofFiles[i].Date + '</td>' +
                    '<td><button type="button" data-file-url=' + ProofFiles[i].ImageUrl + ' onclick="ViewDocument(this)" class="delete_icon chng-color-view"><i class="far fa-eye"></i></button><button type="button" class="delete_icon chng-color-Trash" onclick="remove_proofOfTemp_row(this,' + ProofFiles[i].proofImageId + ')"> <i class="far fa-trash-alt"></i></button></td>' +
                    '</tr>'

            }

        }
        else {
            $(".divProofOfTemps").hide();
            isProofofTemp = false;
        }
    }
    $("#tblProofOfTemp tbody").append(tr);

}

//#endregion

//#region Delete row for Proof of Temp Files

function remove_proofOfTemp_row(_this, proofImageId, index) {

    if (proofImageId == 0) {
        $(_this).closest('tr').remove();
        glbDamageFile.splice(index, 1);
    }
    else {

        $.confirm({
            title: 'Confirm!',
            content: 'Are you sure you want to Delete ?',
            type: 'red',
            typeAnimated: true,
            buttons: {
                Delete: {
                    btnClass: 'btn-blue',
                    action: function () {
                        $.ajax({
                            url: baseUrl + 'Driver/DeleteProofOfTemprature',
                            data: { imageId: proofImageId },
                            type: "GET",
                            async: false,
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",

                            success: function (data) {
                                if (data.IsSuccess == true) {
                                    $(_this).closest("tr").remove();
                                    toastr.success(data.Message);

                                }
                                else {
                                    toastr.error(data.Message);
                                }

                            }
                        });
                    }
                },
                cancel: {
                    btnClass: 'btn-red',
                }

            }
        });
    }
}

//#endregion


$("#fuDamageFiles").change(function () {
    $("#lblfuDamageFiles").css("background", "#7ca337");
    $("#lblfuDamageFiles").html('<i class="fa fa-check-circle fa-lg" aria-hidden="true"></i> SUCCESS');
});


$("#fuProofOfTemperature").change(function () {
    $("#lblfuProofOfTemperature").css("background", "#7ca337");
    $("#lblfuProofOfTemperature").html('<i class="fa fa-check-circle fa-lg" aria-hidden="true"></i> SUCCESS');
});

//#region Button Damaged Upload
var btnUploadDamaged = function () {

    $(".btnDamageFile").on("click", function () {

        if (isFormValid('divDamageFiles')) {

            if ($("#txtDamagedFileName").val() == "") {

                $("#btnDamageFile").css("background", "linear-gradient(90deg, rgba(245,31,31,1) 0%, rgba(241,116,116,1) 100%)");
                $("#btnDamageFile").text('TRY AGAIN');
                toastr.error("Please enter your shipment's damage description.");
            }
            else {

                $("#btnDamageFile").prop('disabled', true);
                //$("#btnDamageFile").css("background", "gray");
                //$("#btnDamageFile").css('box-shadow', "none");


                var fileUploaderDamaged = $("#fuDamageFiles");

                // Date Time Format 
                var d = new Date($.now());
                date = ((d.getMonth() + 1) + "-" + d.getDate() + "-" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds());
                //
                var radioValue = $("input[name='radioShipmentRouteId']:checked").val();

                if (fileUploaderDamaged.length) {
                    glbDamageFileCollection.push(fileUploaderDamaged[0].files);
                    var data = new FormData();
                    var fileName = $.trim($("#txtDamagedFileName").val());
                    if (glbDamageFileCollection.length) {

                        for (let i = 0; i < glbDamageFileCollection.length; i++) {
                            for (let j = 0; j < glbDamageFileCollection[i].length; j++) {
                                data.append("DamagedFiles", glbDamageFileCollection[i][j]);

                            }
                        }
                    }
                    data.append("ImageDescription", fileName);
                    data.append("ShipmentRouteId", radioValue);
                    data.append("AWBPOORD", $("#txtAwbPoOrd").val());
                    showLoader();
                    $.ajax({
                        type: "POST",
                        url: baseUrl + '/Driver/SaveDamagedFiles',
                        contentType: false,
                        processData: false,
                        data: data,
                        async: false,
                        success: function (data, textStatus, jqXHR) {
                            hideLoader();
                            if (data.IsSuccess == true) {
                                toastr.success(data.Message);
                                GetShipmentDamagedFiles(radioValue);

                            }
                            else {
                                toastr.error(data.Message);
                                GetShipmentDamagedFiles(radioValue);
                            }
                            $("#btnDamageFile").css("background", "#7ca337");
                            $("#btnDamageFile").html('<i class="fa fa-check-circle fa-lg" aria-hidden="true"></i> SUCCESS');
                            setTimeout(function () {

                                // $("#btnDamageFile").css("background", "linear-gradient(90deg, rgba(6,100,185,1) 0%, rgba(111,206,255,1) 100%)");
                                $("#btnDamageFile").css("background", "#7ca337");
                                $("#btnDamageFile").html('UPLOAD');

                                $("#lblfuDamageFiles").css("background", "#7ca337");
                                $("#lblfuDamageFiles").html('TAKE PICTURE');

                                $("#btnDamageFile").prop('disabled', false);
                            }, 3000);

                            //$("#btnDamageFile").css("background", "#b54b3a");

                        },
                        error: function (xhr, status, p3, p4) {
                            $("#btnDamageFile").css("background", "linear-gradient(90deg, rgba(245,31,31,1) 0%, rgba(241,116,116,1) 100%)");
                            $("#btnDamageFile").text('TRY AGAIN');
                            $("#btnDamageFile").prop('disabled', false);
                            //$("#btnDamageFile").css("background", "#b54b3a");


                            var err = "Error " + " " + status + " " + p3 + " " + p4;
                            if (xhr.responseText && xhr.responseText[0] == "{")
                                err = JSON.parse(xhr.responseText).Message;
                            console.log(err);
                        }
                    });

                }
                $("#txtDamagedFileName").val("");
                $("#fuDamageFiles").val("");
                bindDamageFileTbl();
            }



        }
        else {
            $("#btnDamageFile").css("background", "linear-gradient(90deg, rgba(245,31,31,1) 0%, rgba(241,116,116,1) 100%)");
            $("#btnDamageFile").text('TRY AGAIN');
            toastr.error("Please upload your shipment's damaged file.");
        }
    })


}
//#endregion

//#region bind Damaged file Upload

function bindDamageFileTbl() {

    var tr = "";
    $("#tblDamagedFiles tbody").empty();

    var radioValue = $("input[name='radioShipmentRouteId']:checked").val();

    if (radioValue > 0) {

        var damageFiles = glbDamageFile.filter(x => x.shipmentRouteId == radioValue);
        if (damageFiles.length > 0) {
            isDamageDetail = true;
            $(".divDamageFile").show();
            for (var i = 0; i < damageFiles.length; i++) {
                tr += '<tr data-file-url=' + damageFiles[i].ImageUrl + ' ondblclick="javascript:ViewDocument(this)">' +
                    '<td>' + damageFiles[i].DamageDescription + '</td>' +
                    '<td>' + damageFiles[i].Date + '</td>' +
                    '<td><button type="button" data-file-url=' + damageFiles[i].ImageUrl + ' onClick="ViewDocument(this)" class="delete_icon chng-color-view"><i class="far fa-eye"></i></button><button type="button" class="delete_icon chng-color-Trash" onclick="remove_DamageFiles_row(this,' + damageFiles[i].DamagedID + ',' + i + ')"> <i class="far fa-trash-alt"></i></button></td>' +
                    '</tr>'

            }

        }
        else {
            $(".divDamageFile").hide();
            isDamageDetail = false;
        }
    }


    $("#tblDamagedFiles tbody").append(tr);

}
//#endregion

//#region Delete Damage Files

function remove_DamageFiles_row(_this, DamagedID, index) {

    if (DamagedID == 0) {
        $(_this).closest('tr').remove();
        glbDamageFile.splice(index, 1);
    }
    else {
        $.confirm({
            title: 'Confirm!',
            content: 'Are you sure you want to Delete ?',
            type: 'red',
            typeAnimated: true,
            buttons: {
                Delete: {
                    btnClass: 'btn-blue',
                    action: function () {

                        $.ajax({
                            url: baseUrl + 'Driver/DeleteDamageFiles',
                            data: { imageId: DamagedID },
                            type: "GET",
                            async: false,
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",

                            success: function (data) {
                                if (data.IsSuccess == true) {
                                    $(_this).closest("tr").remove();
                                    toastr.success(data.Message);

                                }
                                else {
                                    toastr.error(data.Message);
                                }

                            }
                        });
                    }
                },
                cancel: {
                    btnClass: 'btn-red',
                }

            }
        });
    }

}

//#endregion

//#region Remove row in grid
function remove_damage_document_row(_this) {
    $(_this).closest("tr").remove();
}
//#endregion

//#region Shipment Details for Multiple Routes

function GetShipmentDetails(ShippingRoutesId) {

    $.ajax({
        url: baseUrl + '/Driver/GetShipmentLocationDetails/' + ShippingRoutesId,
        type: "Get",
        dataType: "json",
        async: false,
        success: function (data) {

            if (data != null) {
                IsTemperatureRequired = data.IsTemperatureRequired;
                if (data.AirWayBill != "" && data.AirWayBill != null) {
                    $("#txtAwbPoOrd").val(data.AirWayBill);
                }
                else if (data.CustomerPO != "" && data.CustomerPO != null) {
                    $("#txtAwbPoOrd").val(data.CustomerPO);
                }
                else if (data.OrderNo != "" && data.OrderNo != null) {
                    $("#txtAwbPoOrd").val(data.OrderNo);
                }
                else if (data.CustomerRef != "" && data.CustomerRef != null) {
                    $("#txtAwbPoOrd").val(data.CustomerRef);
                }
                else if (data.ContainerNo != "" && data.ContainerNo != null) {
                    $("#txtAwbPoOrd").val(data.ContainerNo);
                }
                else if (data.PurchaseDoc != "" && data.PurchaseDoc != null) {
                    $("#txtAwbPoOrd").val(data.PurchaseDoc);
                }
                else {
                    $("#txtAwbPoOrd").val("");
                }

                $("#hdShipmentId").val(data.ShipmentId);
                $("#hdnEquipmentNo").val(data.EquipmentNo);

                $("#ddlShipmentStatus").val(data.StatusId);

                selectedStatusIndex = $("#ddlShipmentStatus").val();
                selectedStatusText = $("#ddlShipmentStatus").find("option:selected").text().trim();
                BindSubStatus();

                //$("#ddlShipmentSubStatus").val(data.SubStatusId);
                // $("#txtOtherReason").val(data.ShipmentReason);
                $("#txtOtherReason").val("");
                $("#txtReceiverName").val(data.ReceiverName);
                $("#txtSign").attr('src', data.DigitalSignature);

                if ($("#ddlShipmentSubStatus").val() == 7 || $("#ddlShipmentSubStatus").val() == 11) {
                    $("#txtOtherReason").prop('disabled', false);
                    $("#txtOtherReason").val(data.ShipmentReason);

                }

            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}
//#endregion

//#region

function removeItemFromStatusDDL(id) {
    
    if (id == 3 || id == 4 || id == 13) {

        var statusId = GetShipmentLastStatus(id)
        id = statusId;
        // HideSubstatus(id, $("#ddlShipmentSubStatus").val());
    }

    if (id == 2) {

        var pickupArrival = $("#txtActualPickupArrived").val();
        if (pickupArrival != "") {
            $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 6 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 7 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 12 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 13 + "]").remove();
            $("#ddlShipmentStatus").val(5);
        }
        else {
            $("#ddlShipmentStatus option[value=" + 3 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 4 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 5 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 6 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 7 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 13 + "]").remove();
        }

        HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());
    }
    else if (id == 12) {

        //var pickupArrival = $("#txtActualPickupArrived").val();
        //if (pickupArrival != "") {
        $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
        $("#ddlShipmentStatus option[value=" + 6 + "]").remove();
        $("#ddlShipmentStatus option[value=" + 7 + "]").remove();
        $("#ddlShipmentStatus option[value=" + 12 + "]").remove();
        $("#ddlShipmentStatus").val(5);
        //}
        //else {
        //    $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
        //    $("#ddlShipmentStatus option[value=" + 6 + "]").remove();
        //    $("#ddlShipmentStatus option[value=" + 7 + "]").remove();
        //    $("#ddlShipmentStatus option[value=" + 12 + "]").remove();
        //    $("#ddlShipmentStatus").val(5);
        //}

        HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());
    }
    else if (id == 5) {
        $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
        $("#ddlShipmentStatus option[value=" + 5 + "]").remove();
        $("#ddlShipmentStatus option[value=" + 7 + "]").remove();
        $("#ddlShipmentStatus option[value=" + 12 + "]").remove();
        $("#ddlShipmentStatus").val(6);
        HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());
    }
    else if (id == 6) {
        $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
        $("#ddlShipmentStatus option[value=" + 5 + "]").remove();
        $("#ddlShipmentStatus option[value=" + 6 + "]").remove();
        $("#ddlShipmentStatus option[value=" + 12 + "]").remove();

        $("#ddlShipmentStatus").val(7);
        HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());
    }
    else if (id == 7) {
        $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
        $("#ddlShipmentStatus option[value=" + 3 + "]").remove();
        $("#ddlShipmentStatus option[value=" + 4 + "]").remove();
        $("#ddlShipmentStatus option[value=" + 5 + "]").remove();
        $("#ddlShipmentStatus option[value=" + 6 + "]").remove();
        $("#ddlShipmentStatus option[value=" + 12 + "]").remove();
        $("#ddlShipmentStatus option[value=" + 13 + "]").remove();


        $('#divDamage').hide();
        HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());
    }

    DisplayRouteDetail();

};
//#endregion

function GetShipmentLastStatus(statusId) {
    var result = 0;
    var shipmentId = $.trim($("#hdShipmentId").val());
    $.ajax({
        url: baseUrl + '/Driver/GetShipmentLastStatus',
        data: { "statusId": statusId, "shipmentId": shipmentId },
        type: "Post",
        async: false,
        success: function (data) {

            result = data;
        },
        error: function () { }
    });
    return result;
}

//#region shipment status
function shipmentStatus() {

    $.ajax({
        url: baseUrl + '/Driver/GetDriverStatus',
        data: {},
        type: "GET",
        async: false,
        success: function (data) {
            var ddlValue = "";
            $("#ddlShipmentStatus").empty();
            for (var i = 0; i < data.length; i++) {



                if ($("#hdfDriverLanguage").val() == 2) {
                    ddlValue += '<option value="' + data[i].StatusId + '">' + data[i].SpanishStatusAbbreviation + '</option>';
                }
                else {
                    ddlValue += '<option value="' + data[i].StatusId + '">' + data[i].StatusAbbreviation + '</option>';
                }


            }
            $("#ddlShipmentStatus").append(ddlValue);

        }
    });


}
//#endregion

//#region Status DDL change event for validation and to bind sub-status

$("#ddlShipmentStatus").change(function () {
    BindSubStatus();

    HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());

});
//#endregion

//#region validation based on status change
function statusChangeValidations() {

    var retval = true;
    if ($("#ddlShipmentStatus").val() == 12) {
        if ($("#txtActualPickupArrived").val() == "" || $("#txtActualPickupArrived").val() == undefined) {
            $("#txtActualPickupArrived").focus();
            toastr.error("Please submit your pickup arrival timestamp.");
            retval = false;
        }
    }
    else if ($("#ddlShipmentStatus").val() == 5) {
        var proofOfTempImage;
        var loopCount = 0;

        $('#tblProofOfTemp td').each(function () {
            loopCount += 1
            if (loopCount < 4)
                proofOfTempImage = this.innerText;
        });
        if ($("#txtActualPickupArrived").val() == "" || $("#txtActualPickupArrived").val() == undefined) {
            $("#txtActualPickupArrived").focus();
            toastr.error("Please submit your pickup arrival timestamp.");
            retval = false;

        }
        else if (proofOfTempImage == "" || proofOfTempImage == undefined) {
            if (IsTemperatureRequired) {
                $("#txtActualTemp").focus();
                toastr.error("Please submit proof of temperature.");
                retval = false;
            }

        }
    }
    else if ($("#ddlShipmentStatus").val() == 6) {
        
        var proofOfTempImage;
        var loopCount = 0;
        var isPickup = false;

        $('#tblProofOfTemp td').each(function () {
            loopCount += 1
            if (loopCount < 4)
                proofOfTempImage = this.innerText;
        });
        if ($("#txtActualPickupDepart").val() == "" || $("#txtActualPickupDepart").val() == undefined) {
            if ($("#txtActualPickupArrived").val() == "" || $("#txtActualPickupArrived").val() == undefined) {
                $("#txtActualPickupArrived").focus();
                toastr.error("Please submit your pickup arrival timestamp.");
                retval = false;
                isPickup = true;
            }
        }
        else if (proofOfTempImage == "" || proofOfTempImage == undefined) {
            $("#txtActualTemp").focus();
            if (IsTemperatureRequired) {
                toastr.error("Please upload an image as proof of temperature.");
                retval = false;
                isPickup = true;
            }
        }
        if (!isPickup && ($("#txtActualPickupDepart").val() == "" || $("#txtActualPickupDepart").val() == undefined)) {
            $("#txtActualPickupDepart").focus();
            toastr.error("Please submit your pickup departure timestamp.");
            retval = false;
        }
    }
    else if ($("#ddlShipmentStatus").val() == 7) {
        fn_SelectSignDetail();
        if ($("#txtActualDeliveryDepart").val() == "" || $("#txtActualDeliveryDepart").val() == undefined) {
            if (($("#txtActualDeliveryArrived").val() == "" || $("#txtActualDeliveryArrived").val() == undefined)) {
                toastr.error("Please submit your delivery arrival timestamp.");
                retval = false;
            }
        }
        else {

            if (($("#txtActualDeliveryArrived").val() == "" || $("#txtActualDeliveryArrived").val() == undefined)) {
                toastr.error("Please submit your delivery arrival timestamp.");
                retval = false;
            }

            else if ($("#txtReceiverName").val() == "" || $("#txtReceiverName").val() == undefined) {
                $("#txtReceiverName").focus();
                toastr.error("Please enter the name of the person receiving your shipment.");
                retval = false;
            }
            else if (isSigned != true) {
                $('#SignatureButton').focus();
                toastr.error("Please upload the receiver's signature.");
                retval = false;
            }
            else if ($("#txtActualDeliveryDepart").val() == "" || $("#txtActualDeliveryDepart").val() == undefined) {
                $("#txtActualDeliveryDepart").focus();
                toastr.error("Please submit your delivery departure timestamp.");
                retval = false;
            }

        }
    }
    else if ($("#ddlShipmentStatus").val() == 4) {
        retval = true;
        isValidationSucess
    }
    else if ($("#ddlShipmentStatus").val() == 3) {
        retval = true;
        isValidationSucess
    }
    return retval;
}
//#endregion
//#region Get all Temp Proof
var GetAllTempProof = function () {
    var ShipmentFreightDetailId = $("input[name='radiofreightId']:checked").val();
    var shipmentRouteId = $("input[name='radioShipmentRouteId']:checked").val();
    //alert("GetAllTempProof");
    $.ajax({
        url: baseUrl + '/Driver/GetShipmentProofOfTempFiles/',
        type: "Get",
        dataType: "json",
        async: false,
        data: { "id": shipmentRouteId, "ShipmentFreightDetailId": ShipmentFreightDetailId },
        success: function (data) {
            if (data != null) {

                //#region for binding Proof Of Temp Image
                for (let i = 0; i < data.length; i++) {
                    if (data[i].ProofImage == null) {

                    }
                }
                bindProofOfTempFileTbl();
            }
            //#endregion

        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}
//#endregion
function removeLastComma(str) {
    return str.replace(/,(\s+)?$/, '');
}

//#region Get Shipment Freight Details

function GetShipmentFreight(ShipmentRouteStopeId) {

    $.ajax({
        url: baseUrl + '/Driver/GetShipmentFreight/' + ShipmentRouteStopeId,
        type: "Get",
        dataType: "html",
        async: false,
        success: function (data) {

            var outPut = JSON.parse(data);
            //for (var i = 0; i < data.length; i++) {
            ShipmentFreight = outPut;
            //}
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}
//#endregion

//#region Select signature Details
var fn_SelectSignDetail = function () {

    var shipmentRouteId = $("input[name='radioShipmentRouteId']:checked").val();
    $.ajax({
        url: baseUrl + '/Driver/SelectSignatureDetail/' + shipmentRouteId,
        type: "Get",
        dataType: "json",
        async: false,
        success: function (data) {
            isSigned = data;
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    })
}
//#endregion



//#region Bind Substatus 
function BindSubStatus() {
    var statusid = $("#ddlShipmentStatus").val();
    $.ajax({
        url: baseUrl + '/Driver/GetDriverSubStatus',
        data: { statusid: statusid },
        type: "GET",
        async: false,
        // cache: false,
        success: function (data) {

            var ddlValue = "";
            $("#ddlShipmentSubStatus").empty();

            if ($("#hdfDriverLanguage").val() == 2) {
                ddlValue = "<option value=''>Seleccionar sub-estado</option>";
            }
            else {
                ddlValue = "<option value=''>Select Sub-Status</option>";
            }

            for (var i = 0; i < data.length; i++) {
                if ($("#hdfDriverLanguage").val() == 2) {
                    ddlValue += '<option value="' + data[i].SubStatusId + '">' + data[i].SpanishSubStatusName + '</option>';
                }
                else {
                    ddlValue += '<option value="' + data[i].SubStatusId + '">' + data[i].SubStatusName + '</option>';
                }
            }
            $("#ddlShipmentSubStatus").append(ddlValue);


        }
    });
}
//#endregion

//#region ReasonBox
ReasonBox = function () {
    $("#ddlShipmentSubStatus").change(function () {

        $("#ddlShipmentSubStatus").removeClass('highlight');
        if (this.value == 11 || this.value == 7) {
            $("#txtOtherReason").prop('disabled', false);
        }
        else {

            $("#txtOtherReason").prop('disabled', true);
        }
        $("#txtOtherReason").val("");

        HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val())
    });
}
//#endregion

//#region Radio button Check
function CheckRouteStops(routeId, PickDelId) {


    var pickupId = $(PickDelId).attr('PickupLocationId');
    var DeliveryId = $(PickDelId).attr('DeliveryLocationId');
    var DriverId = $(PickDelId).attr('DriverId');
    var EquipmentNo = $(PickDelId).attr('EqipmentNo');
    var CustomerId = $(PickDelId).attr('CustomerId');

    var ar = {
        pickupId: pickupId,
        DeliveryId: DeliveryId,
        DriverId: DriverId,
        EquipmentNo: EquipmentNo,
        CustomerId: CustomerId,
    }
    pickDel.push(ar);

    $("#txtDamagedFileName").val("");
    $("#fuDamageFiles").val("");
    $("#txtActualPickupArrived").val("");
    $("#txtActualPickupDepart").val("");
    $("#txtActualDeliveryArrived").val("");
    $("#txtActualDeliveryDepart").val("");
    shipmentStatus();
    GetShipmentDetails(routeId);
    GetShipmentFreightDetails(routeId);
    bindDamageFileTbl();
    GetPreTripCheckTimings(routeId);
    bindProofOfTempFileTbl();


}
//#endregion

//#region Get Shipment Freight Details

function GetShipmentFreightDetails(ShipmentRouteStopeId) {

    $.ajax({
        url: baseUrl + '/Driver/GetShipmentFreightDetails/' + ShipmentRouteStopeId,
        type: "Get",
        dataType: "html",
        async: false,
        success: function (data) {

            if (data.length > 0) {

                $("#dvShipmentFreigtDetails").show();

                //$("#txtComments").val(data.Comments);
                bindFreighttable(data);
            }

            else {
                $("#dvShipmentFreigtDetails").hide();

            }


        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}
//#endregion


//#region bind Freight Details
bindFreighttable = function (_data) {

    $("#tblFreightDetailsTable").empty();

    $("#tblFreightDetailsTable").append(_data);


    //function for select first readio button
    checkFreightRadionButton();

}
//#endregion

//#region select default first Freight radion button
function checkFreightRadionButton() {
    var radioValue = $("input[name='radioShipmentRouteId']:checked").val();
    if (radioValue > 0) {
        $($("#tblFreightDetails tbody tr")[0]).find("input[type=radio]").prop("checked", true);

        var data = $($("#tblFreightDetails tbody tr")[0]).find('td .lblTemperatureRequired').text();

        var FreightRadioValue = $("input[name='radiofreightId']:checked").val();
        var radioValue = $("input[name='radioShipmentRouteId']:checked").val();
        GetShipmentProofOfTempFiles(radioValue, FreightRadioValue);


    }
}
//#endregion

//#region Shipment Route Radion button checked by Default
function checkShipmentRadionButton() {

    if ($("#tblShipmentRoute tbody tr").length > 0) {
        $($("#tblShipmentRoute tbody tr")[0]).find("input[type=radio]").prop("checked", true);
        var radioValue = $("input[name='radioShipmentRouteId']:checked").val();
        var pickupId = $("input[name='radioShipmentRouteId']:checked").attr('PickupLocationId');
        var DeliveryId = $("input[name='radioShipmentRouteId']:checked").attr('DeliveryLocationId');
        var DriverId = $("input[name='radioShipmentRouteId']:checked").attr('DriverId');
        var EquipmentNo = $("input[name='radioShipmentRouteId']:checked").attr('EqipmentNo');
        var CustomerId = $("input[name='radioShipmentRouteId']:checked").attr('CustomerId');

        var ar = {
            pickupId: pickupId,
            DeliveryId: DeliveryId,
            DriverId: DriverId,
            EquipmentNo: EquipmentNo,
            CustomerId: CustomerId,

        }
        pickDel.push(ar);

        GetShipmentFreightDetails(radioValue);
        GetShipmentDetails(radioValue);

        GetShipmentDamagedFiles(radioValue);

        GetPreTripCheckTimings(radioValue);
    }

}
//#endregion

//#region Get Pre-Trip Check Timing Details 
var GetPreTripCheckTimings = function (ShippingRoutesId) {
    $.ajax({
        url: baseUrl + "Driver/GetPreTripCheckTimings/" + ShippingRoutesId,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            showLoader();
        },
        success: function (response) {
            if (response != null) {

                $("#txtActualPickupArrived").val(ConvertDate(response.DriverPickupArrival, true));
                $("#txtActualPickupDepart").val(ConvertDate(response.DriverPickupDeparture, true));
                $("#txtActualDeliveryArrived").val(ConvertDate(response.DriverDeliveryArrival, true));
                $("#txtActualDeliveryDepart").val(ConvertDate(response.DriverDeliveryDeparture, true));
                $("#txtReceiverName").val(response.ReceiverName);
                if (response.DriverPickupArrival == null) {
                    $("#txtActualPickupArrived").attr('disabled', false);
                    $('#txtActualPickupArrived').attr('readonly', 'readonly');
                    $('#txtActualPickupArrived').css('background-color', '#ffffff');
                    actualPickupArrivedHasValue = "";
                }
                else {
                    $("#txtActualPickupArrived").attr('disabled', true);
                    actualPickupArrivedHasValue = $("#txtActualPickupArrived").val();
                }

                if (response.DriverPickupDeparture == null) {
                    $("#txtActualPickupDepart").attr('disabled', false);
                    $('#txtActualPickupDepart').attr('readonly', 'readonly');
                    $('#txtActualPickupDepart').css('background-color', '#ffffff');
                }
                else {
                    $("#txtActualPickupDepart").attr('disabled', true);
                }
                if (response.DriverDeliveryArrival == null) {
                    $("#txtActualDeliveryArrived").attr('disabled', false);
                    $('#txtActualDeliveryArrived').attr('readonly', 'readonly');
                    $('#txtActualDeliveryArrived').css('background-color', '#ffffff');
                }
                else {
                    $("#txtActualDeliveryArrived").attr('disabled', true);
                }
                if (response.DriverDeliveryDeparture == null) {
                    $("#txtActualDeliveryDepart").attr('disabled', false);
                    $('#txtActualDeliveryDepart').attr('readonly', 'readonly');
                    $('#txtActualDeliveryDepart').css('background-color', '#ffffff');
                }
                else {
                    $("#txtActualDeliveryDepart").attr('disabled', true);
                }
                if (response.ReceiverName == null) {
                    $("#txtReceiverName").attr('disabled', false);

                }
                else {
                    $("#txtReceiverName").attr('disabled', true);
                }


                //HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());
                //$("#ddlShipmentStatus").val();
                //alert('dll selected id is =' + data.StatusId);
                removeItemFromStatusDDL($("#ddlShipmentStatus").val());
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        },
        complete: function () {
            hideLoader();
        }
    });
}
//#endregion

//#region  bind Damaged Files

function GetShipmentDamagedFiles(ShipmentRouteStopeId) {
    $.ajax({
        url: baseUrl + '/Driver/GetShipmentDamagedFiles/' + ShipmentRouteStopeId,
        type: "Get",
        dataType: "json",
        async: false,
        success: function (data) {
            if (data != null) {
                glbDamageFile = new Array();
                //#region for binding damage Image
                for (let i = 0; i < data.length; i++) {
                    var damageFile = {};
                    damageFile.DamagedID = data[i].DamagedID;
                    damageFile.ImageUrl = data[i].ImageUrl;
                    damageFile.DamageDescription = data[i].DamagedDescription;
                    damageFile.FileName = data[i].DamagedImage;
                    damageFile.Date = ConvertDate(data[i].DamagedDate, true);
                    damageFile.shipmentRouteId = data[i].ShipmentRouteID;
                    damageFile.ImageUrl = data[i].ImageUrl;
                    glbDamageFile.push(damageFile);
                }
                bindDamageFileTbl();
            }
            //#endregion

        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}

//#endregion

//#region  bind Proof Of Temp Files

function GetShipmentProofOfTempFiles(ShipmentRouteStopeId, ShipmentFreightDetailId) {

    $.ajax({
        url: baseUrl + '/Driver/GetShipmentProofOfTempFiles/',
        type: "Get",
        dataType: "json",
        async: false,
        data: { "id": ShipmentRouteStopeId, "ShipmentFreightDetailId": ShipmentFreightDetailId },
        success: function (data) {
            if (data != null) {
                glbProofOfTempFile = new Array();

                //#region for binding Proof Of Temp Image
                for (let i = 0; i < data.length; i++) {
                    var ProofOfTempFile = {};
                    ProofOfTempFile.proofImageId = data[i].proofImageId;
                    ProofOfTempFile.actualTemp = data[i].proofActualTemp;
                    ProofOfTempFile.FileName = data[i].ProofImage;
                    ProofOfTempFile.Date = ConvertDate(data[i].ProofDate, true);
                    ProofOfTempFile.shipmentRouteId = data[i].ShipmentRouteID;
                    ProofOfTempFile.ShipmentFreightDetailId = data[i].ShipmentFreightDetailId;
                    ProofOfTempFile.ImageUrl = data[i].ImageUrl;
                    ProofOfTempFile.IsLoading = data[i].IsLoading;
                    glbProofOfTempFile.push(ProofOfTempFile);
                }
                bindProofOfTempFileTbl();
            }
            //#endregion

        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}

//#endregion

//#region show signature Popup
var fn_ShowSignPadDetail = function () {
    $("#divPopUpSign").modal("show");
    $("#clear").click();

}
//#endregion

//#region Save signature Popup
var fn_UpdateSignPadDetail = function () {
    var data = new FormData();
    var shipmentRouteId = $("input[name='radioShipmentRouteId']:checked").val();
    var canvas = $("#canvas").get(0);
    var imgData = canvas.toDataURL();
    data.append("DigitalSignature", imgData);
    data.append("ShipmentRouteId", shipmentRouteId);
    $.ajax({
        url: baseUrl + "Driver/UpdateSignaturePadDetail",
        type: "POST",
        contentType: false,
        processData: false,
        data: data,
        beforeSend: function () {
            showLoader();
        },
        success: function (response) {

            $("#save").css("background", "#7ca337");
            $("#save").html('<i class="fa fa-check-circle fa-lg" aria-hidden="true"></i> SUCCESS');

            $("#SignatureButton").css("background", "#7ca337");
            $("#SignatureButton").html('<i class="fa fa-check-circle fa-lg" aria-hidden="true"></i> SUCCESS');

            var d = new Date($.now());
            var month = (d.getMonth() + 1) > 9 ? (d.getMonth() + 1) : ("0" + (d.getMonth() + 1));
            var date = (d.getDate()) > 9 ? (d.getDate()) : ("0" + (d.getDate()));

            var hours = (d.getHours()) > 9 ? (d.getHours()) : ("0" + (d.getHours()));
            var minutes = (d.getMinutes()) > 9 ? (d.getMinutes()) : ("0" + (d.getMinutes()));
            var date = (month + "-" + date + "-" + d.getFullYear() + " " + hours + ":" + minutes);

            $("#txtActualDeliveryDepart").val(date);


            fn_CheckPickupArrivalPickupDepartDelArrival();
            fn_GetGPSTracker('Departed from', 0, 1);
            fn_GetWaitingTime('txtActualPickupArrived', 'txtActualPickupDepart', 'txtActualDeliveryArrived', 'txtActualDeliveryDepart');

            setTimeout(function () {
                $("#save").css("background", "linear-gradient(90deg, rgba(6,100,185,1) 0%, rgba(111,206,255,1) 100%)");
                $("#save").html('SUBMIT');
                // $("#SignatureButton").css("background", "linear-gradient(90deg, rgba(6,100,185,1) 0%, rgba(111,206,255,1) 100%)");
                $("#SignatureButton").css("background", "#7ca337");
                $("#SignatureButton").html('SIGNATURE');

            }, 3000);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $("#save").css("background", "linear-gradient(90deg, rgba(245,31,31,1) 0%, rgba(241,116,116,1) 100%)");
            $("#save").text('TRY AGAIN');
        },
        complete: function () {
            $("#divPopUpSign").modal("hide");
            hideLoader();

        }
    });

}
//#endregion


//#region Disabled Calender class

$(document).ready(function () {
    $(".calenderDisableRemove").removeClass("ui-state-disabled");
});

//#endregion


//#region get GPS Tracker

var fn_GetdriverStatus = function (event) {

    row = $("input[name='radioShipmentRouteId']:checked").closest("tr");
    var pickuplocation = row.find("td:eq(1)").text().trim();
    var deliveryLocation = row.find("td:eq(3)").text().trim();


    var d = new Date($.now());
    date = ((d.getMonth() + 1) + "-" + d.getDate() + "-" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds());

    var ShipmentId = $.trim($("#hdShipmentId").val());
    var shipmentRouteId = $("input[name='radioShipmentRouteId']:checked").val();

    //if (event != "") {
    //    var status = $("#ddlShipmentStatus").find("option:selected").text().trim();

    //    if (status == "IMMED ATTENTION") {
    //        event = pickuplocation;// $("#" + pickuplocation).val();
    //    }
    //    else if (status == "ON HOLD") {
    //        event = pickuplocation;// $("#" + pickuplocation).val();
    //    }
    //    else if (status == "DISPATCHED") {
    //        event = pickuplocation;//$("#" + pickuplocation).val();
    //    }
    //    else if (status == "LOADING") {
    //        var eventL = pickuplocation; //$("#" + pickuplocation).val();
    //        //event = status.replace("pick up location", eventL);
    //        event = ("LOADING at " + eventL);
    //    }
    //    else if (status == "IN-ROUTE") {
    //        var eventR = deliveryLocation;//$("#" + deliveryLocation).val();
    //        //event = status.replace("delivery location", eventR);
    //        event = ("IN-ROUTE to " + eventL);
    //    }

    //    else if (status == "DELIVERED") {
    //        event = deliveryLocation;// $("#" + deliveryLocation).val();
    //    }
    //}
    //else {
    //    var event = '';

    //}
    //if (status != "LOADING" && status != "IN-ROUTE") {
    //    var events = (status + ' at ' + event).trim();
    //}
    //else {
    //    var events = event;
    //}
    var status = $("#ddlShipmentStatus").find("option:selected").text().trim().toUpperCase();
    var events = ("Status is changed to " + status);
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var latitude = position.coords.latitude;
            var longitude = position.coords.longitude;


            var arEvent = {
                Latitude: latitude,
                longitude: longitude,
                CreatedOn: date,
                ShipmentId: ShipmentId,
                ShipmentRouteId: shipmentRouteId,
                Event: events,
            }
            glbarEvent.push(arEvent);

        });
    } else {
        console.log("Browser doesn't support geolocation!");
    }
}
//#endregion


//#region Get GPS Pick location and Delivery Location

var fn_GetGPSTracker = function (event, IsPickupLocation, IsDeliveryLocation) {

    row = $("input[name='radioShipmentRouteId']:checked").closest("tr");

    var pickupLocation = "";
    var deliveryLocation = "";

    if (IsPickupLocation == 1) {
        pickupLocation = row.find("td:eq(1)").text().trim();
    }
    else {
        pickupLocation = '';
    }

    if (IsDeliveryLocation == 1) {
        deliveryLocation = row.find("td:eq(3)").text().trim();
    }
    else {
        deliveryLocation = '';
    }

    // Date Time Format 
    var d = new Date($.now());
    date = ((d.getMonth() + 1) + "-" + d.getDate() + "-" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds());
    //
    var ShipmentId = $.trim($("#hdShipmentId").val());
    var shipmentRouteId = $("input[name='radioShipmentRouteId']:checked").val();

    //if (pickuplocation != "") {
    //    var pickupLocation = pickuplocation;// $("#" + pickuplocation).val();
    //}
    //else {
    //    var pickupLocation = '';
    //}

    //if (deliveryLocation != "") {
    //    var deliveryLocation = deliveryLocation;//$("#" + deliveryLocation).val();
    //}
    //else {
    //    var deliveryLocation = '';
    //}

    var events = (event + ' ' + pickupLocation + ' ' + deliveryLocation).trim();
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var latitude = position.coords.latitude;
            var longitude = position.coords.longitude;


            var arEvent = {
                Latitude: latitude,
                longitude: longitude,
                CreatedOn: date,
                ShipmentId: ShipmentId,
                ShipmentRouteId: shipmentRouteId,
                Event: events,
            }

            glbarEvent.push(arEvent);


        });
    } else {
        console.log("Browser doesn't support geolocation.");
    }
}
//#endregion


var getLateLong = function () {

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var latitude = position.coords.latitude;
            var longitude = position.coords.longitude;
            var d = new Date($.now());
            date = ((d.getMonth() + 1) + "-" + d.getDate() + "-" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds());
            var ShipmentId = $.trim($("#hdShipmentId").val());
            var shipmentRouteId = $("input[name='radioShipmentRouteId']:checked").val();
            var arEvent = {
                Latitude: latitude,
                longitude: longitude,
                CreatedOn: date,
                ShipmentId: ShipmentId,
                ShipmentRouteId: shipmentRouteId,
                Event: null,
            }

            glbarEvent.push(arEvent);
            fn_SaveGPSTracker();
        });
    } else {
        console.log("Browser doesn't support geolocation!");
    }
}

//#region Save GPS Tracker History 
function fn_SaveGPSTracker() {

    var data = glbarEvent;
    $.ajax({
        url: baseUrl + "Driver/SaveGPSTracker",
        type: "POST",
        dataType: "json",
        data: { dto: data },
        success: function (response) {
            glbarEvent = [];
        },
        error: function (jqXHR, textStatus, errorThrown) {

        },
        complete: function () {


        }
    });
}
//#endregion

//#region To redirect Direction Map 
function fn_getDirection(myLocation) {

    //#region code done by abid for map functionality
    //let _url = baseUrl + "Driver/GPSTracker?location='" + myLocation + "'";
    //
    ////alert(_url)
    //window.open(_url, '_blank');
    // myLocation = 'APK Perks Pvt.Ltd, Block-H, Bulding Number 141, 3rd Floor, Sector 63, Noida, Uttar Pradesh 201201';
    //#endregion

    //
    //console.log('default = ' + myLocation);
    //console.log('after removing company = ' + myLocation.replace(/^[^,]+, */, ''));

    myLocation = myLocation.replace(/^[^,]+, */, '')
    window.open("https://www.google.com/maps/dir/" + formatedCurrentAddress + "/" + myLocation);

}

function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
    } else {
        //alert("Geolocation is not supported by this browser.");
        toastr.error("Geolocation is not supported by this browser.");
    }
}
function showPosition(position) {

    location.latitude = position.coords.latitude;
    location.longitude = position.coords.longitude;

    var geocoder = new google.maps.Geocoder();

    var latLng = new google.maps.LatLng(location.latitude, location.longitude);
    if (geocoder) {
        geocoder.geocode({ 'latLng': latLng }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                formatedCurrentAddress = results[0].formatted_address;
                //console.log(results[0].formatted_address);
                $('#address').html('Address:' + results[0].formatted_address);
            }
            else {
                $('#address').html('Geocoding failed: ' + status);
                //console.log("Geocoding failed: " + status);
            }
        }); //geocoder.geocode()
    }
} //showPosition




//#endregion

//#region View Document
function ViewDocument(_this) {
    var fileUrl = $(_this).attr("data-file-url");
    if (fileUrl != undefined) {
        var extn = fileUrl.substring(fileUrl.lastIndexOf('.') + 1);

        var isImg = isExtension(extn, _imgExts);
        var $divViewer = $("#divViewer");
        var docHeight = $(document).height();
        if (isImg) {
            $('.btnPrintImage').show();
            $('.btnPrintPdf').hide();
            var imgTag = '<img src="' + fileUrl + '" style="width:auto;height:auto" class="img-fluid" />';
            $divViewer.html(imgTag);

        }
        else {
            $('.btnPrintImage').hide();
            $('.btnPrintPdf').show();
            var docHeight = $(document).height();
            var iframe = '<iframe id="myiframe" src="' + fileUrl + '" style="width: 100%;height:' + (docHeight - 200) + 'px"></iframe>';
            $divViewer.html(iframe);
        }
        $("#modalDocument").modal("show");
    }
}
//#endregion

//#region CHECK EXTENSION
var _imgExts = ["jpg", "jpeg", "png", "jfif"];
var isExtension = function (ext, extnArray) {
    var result = false;
    var i;
    if (ext) {
        ext = ext.toLowerCase();
        for (i = 0; i < extnArray.length; i++) {
            if (extnArray[i].toLowerCase() === ext) {
                result = true;
                break;
            }
        }
    }
    return result;
}
//#endregion//#region print pdf
var printPdf = function () {
    $('.btnPrintPdf').on('click', function () {
        $('#myiframe').get(0).contentWindow.print();
    })
}
//#endregion

//#region print image
var printImage = function () {
    $('.btnPrintImage').on('click', function () {
        var divToPrint = document.getElementById("divViewer");
        newWin = window.open("", "");
        newWin.document.write(divToPrint.outerHTML);
        newWin.print();
        newWin.close();

    })
}
//#endregionfunction CheckImageExtension(_this) {

    var Isvalid = false;

    //var file = $('input[type="file"]').val();
    var file = _this.value;
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
                toastr.error('Uploads must be in JPG, JPEG, PNG, PDF, or JFIF formats.');
                $(_this).val(null);
                return Isvalid;
            }
        }
    }
    else {
        toastr.error("Please Browse to select your Upload File.", "")
        return Isvalid;
    }

}

//#region camera snap 
function opencam() {
    navigator.getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.oGetUserMedia || navigator.msGetUserMedia;
    if (navigator.getUserMedia) {
        navigator.getUserMedia({ video: { facingMode: { exact: "environment" } } }, streamWebCam, throwError);
    }

}

function closecam() {

    video.pause();

    try {
        video.srcObject = null;
    } catch (error) {
        video.src = null;
    }

    var track = strr.getTracks()[0];  // if only one media track
    // ...
    track.stop();

}
var video = document.getElementById('video');
var canvas = document.getElementById('canvas-cam');
var context = canvas.getContext('2d');
var strr;
function streamWebCam(stream) {
    const mediaSource = new MediaSource(stream);
    try {
        video.srcObject = stream;
    } catch (error) {
        video.src = URL.createObjectURL(mediaSource);
    }
    video.play();
    strr = stream;
}
function throwError(e) {
    $("#snap").addClass("disabledbutton");
    $("#btnWebCameraSave").attr("Disabled", true);
    // alert(e.name + " or Problem occure to open the camera..");
}
$('#open,#DamageOpen').click(function (event) {
    $("#snap").removeClass("disabledbutton");
    $("#btnWebCameraSave").attr("Disabled", false);
    opencam();
    $('#control').show();
});
$('#close').click(function (event) {
    closecam();
});
$('#snap').click(function (event) {
    var proof = $("#IsProofCamfile").val();

    if (proof == "true") {
        var temp = $("#txtwebActualTemp").val();

        if (temp != "") {
            canvas.width = video.clientWidth;
            canvas.height = video.clientHeight;
            context.drawImage(video, 0, 0);
            $('#vid').css('z-index', '20');
            $('#capture').css('z-index', '30');
            $(".lbl-message").show();
        }
        else {
            toastr.error("Please fill the Actual Temperature.");
            //$.alert({
            //    title: 'Alert!',
            //    content: 'Please fill the Actual Temperature.',
            //});
        }
    }
    else {
        var damageFile = $("#txtWebDamagedFileName").val();

        if (damageFile != "") {
            canvas.width = video.clientWidth;
            canvas.height = video.clientHeight;
            context.drawImage(video, 0, 0);
            $('#vid').css('z-index', '20');
            $('#Damagecapture').css('z-index', '30');
            $(".lbl-message").show();
        }
        else {
            toastr.error("Please fill the Damage Description.");
            //$.alert({
            //    title: 'Alert!',
            //    content: 'Please fill the Damage Description.',
            //});
        }
    }

});
$('#retake').click(function (event) {
    $('#vid').css('z-index', '30');
    $('#capture').css('z-index', '20');
});
//#endregion

//#region Save web-Camera  

$("#btnWebCameraSave").on("click", function () {
    var proof = $("#IsProofCamfile").val();

    if (proof == "true") {
        var data = new FormData();
        var ShipmentFreightDetailId = $("input[name='radiofreightId']:checked").val();
        var shipmentRouteId = $("input[name='radioShipmentRouteId']:checked").val();
        var ActualTemp = $.trim($("#txtwebActualTemp").val());
        var unit = $("#ddlwebTemperatureUnit").val();
        if (unit == 'C') {
            ActualTemp = CelsiusToFahrenheit(ActualTemp);
        }
        var canvas = $("#canvas-cam").get(0);
        var ImgData = canvas.toDataURL();
        var camImgData = ImgData.replace('data:image/png;base64,', '');

        data.append("ImageUrl", camImgData);
        data.append("ShipmentRouteId", shipmentRouteId);
        data.append("ShipmentFreightDetailId", ShipmentFreightDetailId);
        data.append("ActualTemperature", ActualTemp);


        $.ajax({
            type: "POST",
            url: baseUrl + '/Driver/saveShipmentWebCamera',
            contentType: false,
            processData: false,
            data: data,
            success: function (data, textStatus, jqXHR) {
                if (data.IsSuccess == true) {

                    toastr.success(data.Message);
                    $("#divPopUpCamera").modal("hide");
                    $("#txtwebActualTemp").val("");
                    $("#ddlwebTemperatureUnit").val("");
                    $("#canvas-cam").val("");
                    GetShipmentProofOfTempFiles(shipmentRouteId, ShipmentFreightDetailId);

                }
                else {
                    toastr.error(data.Message);
                    $("#divPopUpCamera").modal("hide");
                }

            },
            error: function (xhr, status, p3, p4) {

                var err = "Error " + " " + status + " " + p3 + " " + p4;
                if (xhr.responseText && xhr.responseText[0] == "{")
                    err = JSON.parse(xhr.responseText).Message;
                console.log(err);
            }
        });
    }
    else {
        var data = new FormData();
        var shipmentRouteId = $("input[name='radioShipmentRouteId']:checked").val();
        var DamagedFileName = $.trim($("#txtWebDamagedFileName").val());
        var canvas = $("#canvas-cam").get(0);
        var ImgData = canvas.toDataURL();
        var camImgData = ImgData.replace('data:image/png;base64,', '');

        data.append("ImageUrl", camImgData);
        data.append("ShipmentRouteId", shipmentRouteId);
        data.append("ImageDescription", DamagedFileName);


        $.ajax({
            type: "POST",
            url: baseUrl + '/Driver/saveShipmentDamageWebCamera',
            contentType: false,
            processData: false,
            data: data,
            success: function (data, textStatus, jqXHR) {
                if (data.IsSuccess == true) {

                    toastr.success(data.Message);

                    $("#divPopUpCamera").modal("hide");
                    $("#txtWebDamagedFileName").val("");
                    $("#canvas-cam").val("");
                    GetShipmentDamagedFiles(shipmentRouteId);

                }
                else {
                    toastr.error(data.Message);
                    $("#divPopUpCamera").modal("hide");
                }

            },
            error: function (xhr, status, p3, p4) {

                var err = "Error " + " " + status + " " + p3 + " " + p4;
                if (xhr.responseText && xhr.responseText[0] == "{")
                    err = JSON.parse(xhr.responseText).Message;
                console.log(err);
            }
        });


    }
})

//#endregion

//#region Get Waiting time for Pick location and Delivery Location

var fn_GetWaitingTime = function (pickupArrivalDate, pickupDepartDate, DeliveryArrivalDate, DeliveryDepartDate) {

    var pickupArrivalDate = $("#" + pickupArrivalDate).val();
    if (pickupArrivalDate == undefined || pickupArrivalDate == null) {
        pickupArrivalDate = "";
    }

    var pickupDepartDate = $("#" + pickupDepartDate).val();
    if (pickupDepartDate == undefined || pickupDepartDate == null) {
        pickupDepartDate = "";
    }

    var deliveryArrivalDate = $("#" + DeliveryArrivalDate).val();
    if (deliveryArrivalDate == undefined || deliveryArrivalDate == null) {
        deliveryArrivalDate = "";
    }

    var deliveryDepartDate = $("#" + DeliveryDepartDate).val();
    if (deliveryDepartDate == undefined || deliveryDepartDate == null) {
        deliveryDepartDate = "";
    }

    var ShipmentId = $.trim($("#hdShipmentId").val());
    var shipmentRouteId = $("input[name='radioShipmentRouteId']:checked").val();

    //if (pickupArrivalDate >= $("#" + ESTPickupArrival).val()) {
    //    var pickupArrivalDate = $("#" + pickupArrivalDate).val();
    //}
    //else if (pickupArrivalDate < $("#" + ESTPickupArrival).val()) {
    //    var pickupArrivalDate = $("#" + ESTPickupArrival).val();
    //}
    //else {
    //    var pickupArrivalDate = '';
    //}
    //if (pickupDepartDate != null && pickupDepartDate != undefined) {
    //    pickupDepartDate = $("#" + pickupDepartDate).val();

    //}
    //else {
    //    pickupDepartDate = '';
    //}

    //if (DeliveryArrivalDate != null && ESTDeliveryArrival != null && DeliveryArrivalDate >= $("#" + ESTDeliveryArrival).val()) {
    //    var DeliveryArrivalDate = $("#" + DeliveryArrivalDate).val();
    //}
    //else if (DeliveryArrivalDate != null && ESTDeliveryArrival != null && DeliveryArrivalDate < $("#" + ESTDeliveryArrival).val()) {
    //    var DeliveryArrivalDate = $("#" + ESTDeliveryArrival).val();
    //}
    //else {
    //    var DeliveryArrivalDate = '';
    //}
    //if (DeliveryDepartDate != null && DeliveryDepartDate != undefined) {
    //    DeliveryDepartDate = $("#" + DeliveryDepartDate).val();
    //}
    //else {
    //    DeliveryDepartDate = '';
    //}

    var arEvent = {
        PickupArrivedOn: pickupArrivalDate,
        PickupDepartedOn: pickupDepartDate,
        DeliveryArrivedOn: deliveryArrivalDate,
        DeliveryDepartedOn: deliveryDepartDate,
        ShipmentId: ShipmentId,
        ShipmentRouteId: shipmentRouteId,
        PickUpLocationId: pickDel[0].pickupId,
        DeliveryLocationId: pickDel[0].DeliveryId,
        DriverId: pickDel[0].DriverId,
        EquipmentNo: pickDel[0].EquipmentNo,
        CustomerId: pickDel[0].CustomerId,

    }
    glbarWaitingTime.push(arEvent);

}
//#endregion

//#region to check textboxes have data or not

function fn_ToCheckPickupArrivalTextbox() {
    if ($("#txtActualPickupArrived").val() != "" && $("#txtActualPickupArrived").val() != undefined) {
        var currentDT = Actualbinddate("#txtActualPickupDepart");
        fn_alertforStatusSubmit();
    }
    else {
        $("#txtActualPickupArrived").focus();
        //$.alert({
        //    title: 'Alert!',
        //    content: " Please fill Pickup Arrival date time first",
        //    type: 'red',
        //    typeAnimated: true,
        //});
        toastr.error("Please fill Pickup Arrival date time first.");
    }

}

function fn_CheckPickupArrivalAndPickupDepart() {

    if (($("#txtActualPickupArrived").val() != "" && $("#txtActualPickupArrived").val() != undefined) && ($("#txtActualPickupDepart").val() != "" && $("#txtActualPickupDepart").val() != undefined)) {
        var currentDT = Actualbinddate("#txtActualDeliveryArrived");
        fn_alertforStatusSubmit();
    }
    else {

        //$.alert({
        //    title: 'Alert!',
        //    content: "Please fill Pickup Arrival and Pickup Depart date time first",
        //    type: 'red',
        //    typeAnimated: true,
        //});
        toastr.error("Please fill Pickup Arrival and Pickup Depart date time first.");
    }
}

function fn_CheckPickupArrivalPickupDepartDelArrival() {
    if (($("#txtActualPickupArrived").val() != "" && $("#txtActualPickupArrived").val() != undefined) && ($("#txtActualPickupDepart").val() != "" && $("#txtActualPickupDepart").val() != undefined)) {

        fn_SelectSignDetail();


        var currentDT = Actualbinddate("#txtActualDeliveryDepart");
        fn_alertforStatusSubmit();


    }
    else {
        $("#txtActualDeliveryArrived").focus();
        //$.alert({
        //    title: 'Alert!',
        //    content: "Please fill Pickup Arrival, Pickup Depart date time first",
        //    type: 'red',
        //    typeAnimated: true,
        //});

        toastr.error("Please fill Pickup Arrival, Pickup Depart date time first.");
    }

}
//#endregion

//#region Current date and Time
var Actualbinddate = function (_this) {

    var todaydate = new Date();

    var month = todaydate.getMonth() + 1;

    var dates = todaydate.getDate();

    var dates = (month < 10 ? ("0" + month) : month) + "-" + (dates < 10 ? ("0" + dates) : dates) + "-" + todaydate.getFullYear();

    var time = (todaydate.getHours() < 10 ? ("0" + todaydate.getHours()) : todaydate.getHours()) + ":" + (todaydate.getMinutes() < 10 ? ("0" + todaydate.getMinutes()) : todaydate.getMinutes());

    var dateTime = dates + ' ' + time;

    $(_this).val(dateTime);


}
//#endregion


//#region Save Waiting Time 
function fn_SaveWaitingTime() {

    // var data = glbarWaitingTime;
    if (glbarWaitingTime.length > 0) {
        var Modal = {
            PickupArrivedOn: glbarWaitingTime[0].PickupArrivedOn,
            PickupDepartedOn: glbarWaitingTime[0].PickupDepartedOn,
            DeliveryArrivedOn: glbarWaitingTime[0].DeliveryArrivedOn,
            DeliveryDepartedOn: glbarWaitingTime[0].DeliveryDepartedOn,
            ShipmentId: glbarWaitingTime[0].ShipmentId,
            ShipmentRouteId: glbarWaitingTime[0].ShipmentRouteId,
            DriverId: glbarWaitingTime[0].DriverId,
            EquipmentNo: glbarWaitingTime[0].EquipmentNo,
            CustomerId: glbarWaitingTime[0].CustomerId,
            PickUpLocationId: glbarWaitingTime[0].PickUpLocationId,
            DeliveryLocationId: glbarWaitingTime[0].DeliveryLocationId,
        };

        $.ajax({
            url: baseUrl + "Driver/SaveWaitingTime",
            type: "POST",
            dataType: "json",
            data: { dto: Modal },
            success: function (response) {

            },
            error: function (jqXHR, textStatus, errorThrown) {

            },
            complete: function () {

            }
        });
    }
}
//#endregion

//#region Alert for submit button
function fn_alertforStatusSubmit() {
    //$.alert({
    //    title: 'Success!',
    //    content: "<b>Your data has successfully been added to your shipment.<br/> Don't forget to click on the Submit button to save all changes.</b>",
    //    type: 'green',
    //    typeAnimated: true,
    //});
    toastr.success("Your data has successfully been added to your shipment.<br/> Don't forget to click on the Submit button to save all changes.");
}

function fn_alertforSubmit() {
    //$.alert({
    //    title: 'Success!',
    //    content: "<b>Your data has successfully been added to your shipment. <br/> Don't forget to click on the Submit button to save all changes.</b>",
    //    type: 'green',
    //    typeAnimated: true,
    //});
    toastr.success("Your data has successfully been added to your shipment.<br/> Don't forget to click on the Submit button to save all changes.");
}
//#endregion

//#region Get Shipment Comments
function GetShipmentComments() {

    var shipmentId = $.trim($("#hdShipmentId").val());
    $.ajax({
        url: baseUrl + '/Driver/GetShipmentComments/',
        type: "Get",
        dataType: "json",
        async: false,
        data: { "shipmentId": shipmentId },
        success: function (data) {

            var commentList = "";
            $("#tblShipmentComments").empty();
            if (data != null) {
                for (var i = 0; i < data.length; i++) {

                    commentList += '<tr><td>' + data[i].comment + '</td></tr>'

                }
            }
            $("#tblShipmentComments").append(commentList);

        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}
//#endregion



//#region validation based on status change
function changeBorderColor() {

    if ($("#ddlShipmentStatus").val() == 12) {
        if ($("#txtActualPickupArrived").val() == "" || $("#txtActualPickupArrived").val() == undefined) {
            $("#txtActualPickupArrived").focus();
            $('#txtActualPickupArrived').css('border-color', 'red');
        }
    }
    else if ($("#ddlShipmentStatus").val() == 5) {
        var proofOfTempImage;
        var loopCount = 0;

        $('#tblProofOfTemp td').each(function () {
            loopCount += 1
            if (loopCount < 4)
                proofOfTempImage = this.innerText;
        });
        if ($("#txtActualPickupArrived").val() == "" || $("#txtActualPickupArrived").val() == undefined) {
            $("#txtActualPickupArrived").focus();
            $('#txtActualPickupArrived').css('border-color', 'red');
        }
        else if (proofOfTempImage == "" || proofOfTempImage == undefined) {
            $("#txtActualTemp").focus();
            $('#txtActualTemp').css('border-color', 'red');
            $('#lblfuProofOfTemperature').css('border', '1px solid red');
        }
        else {
            $('#txtActualTemp').css('border-color', '#ced4da');
            $('#lblfuProofOfTemperature').css('border', '0px');
        }
    }
    else if ($("#ddlShipmentStatus").val() == 6) {
        
        var proofOfTempImage;
        var loopCount = 0;
        var isPickup = false;

        $('#tblProofOfTemp td').each(function () {
            loopCount += 1
            if (loopCount < 4)
                proofOfTempImage = this.innerText;
        });
        if ($("#txtActualPickupDepart").val() == "" || $("#txtActualPickupDepart").val() == undefined) {
            if ($("#txtActualPickupArrived").val() == "" || $("#txtActualPickupArrived").val() == undefined) {
                $("#txtActualPickupArrived").focus();
                $('#txtActualPickupArrived').css('border-color', 'red');
                isPickup = true;
            }
        }
        else if (proofOfTempImage == "" || proofOfTempImage == undefined) {

            $("#txtActualTemp").focus();
            $('#txtActualTemp').css('border-color', 'red');

            isPickup = true;

        }
        if (!isPickup && ($("#txtActualPickupDepart").val() == "" || $("#txtActualPickupDepart").val() == undefined)) {
            $("#txtActualPickupDepart").focus();
            $('#txtActualPickupDepart').css('border-color', 'red');
        }
    }
    else if ($("#ddlShipmentStatus").val() == 7) {
        if (($("#txtActualDeliveryArrived").val() == "" || $("#txtActualDeliveryArrived").val() == undefined)) {
            $('#txtActualDeliveryArrived').css('border-color', 'red');

        }

        else if ($("#txtReceiverName").val() == "" || $("#txtReceiverName").val() == undefined) {
            $("#txtReceiverName").focus();
            $('#txtReceiverName').css('border-color', 'red');
            $('#SignatureButton').css('border', '1px solid red');
        }
        else if (isSigned != true) {
            $('#SignatureButton').focus();
            $('#SignatureButton').css('border', '1px solid red');
        }
        else if ($("#txtActualDeliveryDepart").val() == "" || $("#txtActualDeliveryDepart").val() == undefined) {
            $("#txtActualDeliveryDepart").focus();
            $('#txtActualDeliveryDepart').css('border-color', 'red');
        }


    }
    else if ($("#ddlShipmentStatus").val() == 4) {

    }
    else if ($("#ddlShipmentStatus").val() == 3) {

    }

}
//#endregion
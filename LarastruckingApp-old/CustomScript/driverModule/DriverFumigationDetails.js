var glbDamageFile = [];
var glbProofOfTempFile = [];
var glbDamageFileCollection = [];
var damageDescriptionCollection = [];
var glbGpsHistory = [];
var glbarEvent = [];
var pickDel = [];
var glbarWaitingTime = [];
var selectedStatusIndex = 0;
var actualPickupArrivedHasValue = "";
var isValidLoading = true;
//var isDeliveryTemp = true;
//#region Ready State
$(function () {
    hideSwapButton(1);
    var lat;
    var long;
    var formatedCurrentAddress;
    getLocation();
    btnUpload();
    btnUploadDamaged();
    //bind shipment status dropdown
    shipmentStatus();
    //bind shipment Substatus dropdown
    // ddlSubstatus();
    // For Bind the Reson Text box
    ReasonBox();
    // For Converting the Temperature
    convertTemp();
    // For Save the form
    //btnSave();
    // For Save Web Camera
    // fn_saveWebCamera();
    // For Fumigation Route Details Details Radio Button 
    checkFumigationRadionButton();
    // get Direction Routes
    //initMap();
    GetFumigationComments();
    // For Save GPS Tracking History
    //window.setInterval(getLateLong, 5000);

    //#region Camera Capture Image 
    $('#control').hide();
    $('#video').resize(function () {
        //$('#cont').height($('#video').height());
        //$('#cont').width($('#video').width());
        $('#control').height($('#video').height() * 0.1);
        $('#control').css('top', $('#video').height() * 0.9);
        $('#control').width($('#video').width());
        $('#control').show();

    });
    //#endregion

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
    //#endregion

    $('#tblPreTripDetails').on('dblclick', 'tbody tr', function () {
        var table = $('#tblPreTripDetails').DataTable();
        var data_row = table.row($(this).closest('tr')).data();
        window.location.href = baseUrl + '/Driver/Detail/' + data_row.Id;

    });


});
//#endregion

var HideSubstatus = function (status, subStatus) {

    //#region  Sub status drop down and discription
    if (status == 3 || status == 4) {
        $("#subStatusDiv").show();
        $('#DelArrivalDiv').hide();
        $('#DelDepartDiv').hide();
        // $('#DeliveryTempDiv').hide();
        $('#divProofOfTemp').hide();
        $('#divTempReq').hide();

        $('#ReceiverNameDiv').hide();
        $('#SignatureDiv').hide();

        $('#FumReleaseDiv').hide();
        $('#FumDepartDiv').hide();
        $('#FumInDiv').hide();


        $('#LoadingStartDiv').hide();
        $('#LoadingFinishDiv').hide();
        $('#PickUpDepartDiv').hide();
        $('#PickupArrivalDiv').hide();
        $('#LoadingTempDiv').hide();

        // $(".divDamageDoc").hide();
        //$(".divProofOfTemper").hide();

        if (status == 3) {
            $('#divDamageFiles').show();
        }
        else {
            $('#divDamageFiles').hide();
        }

    }
    else {
        $("#subStatusDiv").hide();
    }
    if ((subStatus == 11 || subStatus == 7) && (status == 3 || status == 4)) {
        $('#txtOtherReasonDiv').show();
    }
    else {
        $('#txtOtherReasonDiv').hide();
    }
    //#endregion

    //#region  first step if status is "DISPATCHED"
    var isPickup = $("input[name='radioFumigationRouteId']:checked").attr('data-ispickup');
    if (isPickup.toLowerCase() == "true") {
        if (status == 12 && actualPickupArrivedHasValue != "") //DISPATCHED 
        {
            $('#PickupArrivalDiv').show();
            $('#divTempReq').show();
            $('#divDamageFiles').hide();
            $('#divProofOfTemp').show();
            $('#LoadingTempDiv').show();

            // $('#PickupArrivalDiv').hide();
            $('#FumReleaseDiv').hide();
            $('#FumDepartDiv').hide();
            $('#DelArrivalDiv').hide();

            // $('#DeliveryTempDiv').hide();
            $('#LoadingStartDiv').hide();
            $('#LoadingFinishDiv').hide();
            $('#PickUpDepartDiv').hide();
            $('#FumInDiv').hide();
            // $('#ReceiverNameAndSignDiv').hide();
            $('#ReceiverNameDiv').hide();
            $('#SignatureDiv').hide();
            $('#DelDepartDiv').hide();
        }
        else if (status == 12 && actualPickupArrivedHasValue == "") //DISPATCHED
        {
            $('#PickupArrivalDiv').show();
            $('#FumReleaseDiv').hide();
            $('#FumDepartDiv').hide();
            $('#DelArrivalDiv').hide();
            $('#divProofOfTemp').hide();
            $('#divTempReq').hide();
            $('#LoadingTempDiv').hide();
            // $('#DeliveryTempDiv').hide();
            $('#divDamageFiles').hide();

            $('#LoadingStartDiv').hide();
            $('#LoadingFinishDiv').hide();
            $('#PickUpDepartDiv').hide();
            $('#FumInDiv').hide();
            //$('#ReceiverNameAndSignDiv').hide();
            $('#ReceiverNameDiv').hide();
            $('#SignatureDiv').hide();
            $('#DelDepartDiv').hide();


            // $("#ddlShipmentStatus option[value=" + 5 + "]").remove(); //removing Loading status
        }
        else if (status == 5) //LOADING AT PICK UP LOCATION
        {
            $('#PickupArrivalDiv').show();
            $('#LoadingStartDiv').show();
            $('#LoadingFinishDiv').show();
            $('#PickUpDepartDiv').show();
            $('#divDamageFiles').hide();

            $('#FumReleaseDiv').hide();
            $('#FumDepartDiv').hide();
            $('#DelArrivalDiv').hide();
            $('#divProofOfTemp').show();
            $('#divTempReq').show();
            $('#LoadingTempDiv').show();
            //$('#DeliveryTempDiv').hide();

            $('#FumInDiv').hide();
            // $('#ReceiverNameAndSignDiv').hide();
            $('#ReceiverNameDiv').hide();
            $('#SignatureDiv').hide();
            $('#DelDepartDiv').hide();

            var proofOfTempImage;
            var loopCount = 0;
            $('#tblProofOfTemp td').each(function () {
                loopCount += 1
                if (loopCount < 3)
                    proofOfTempImage = this.innerText;
            });
            if (proofOfTempImage == "" || proofOfTempImage == undefined) {
                $('#divProofOfTemp').show();
                $('#divTempReq').show();
                $('#LoadingTempDiv').show();
                $('#txtActualTemp').focus();

            }
        }
        else if (status == 6)  //IN-ROUTE TO DELIVERY LOCATION
        {
            $('#FumInDiv').show();

            $('#LoadingStartDiv').show();
            $('#LoadingFinishDiv').show();
            $('#PickUpDepartDiv').show();
            $('#PickupArrivalDiv').show();

            $('#FumReleaseDiv').hide();
            $('#FumDepartDiv').hide();

            $('#DelArrivalDiv').hide();
            $('#divProofOfTemp').hide();
            $('#divTempReq').hide();
            $('#LoadingTempDiv').hide();
            // $('#DeliveryTempDiv').hide();
            $('#divDamageFiles').hide();
            //   $('#ReceiverNameAndSignDiv').hide();

            $('#ReceiverNameDiv').hide();
            $('#SignatureDiv').hide();

            $('#DelDepartDiv').hide();
            if ($("#txtActualLoadingStartTime").val() == "" || $("#txtActualLoadingStartTime").val() == undefined) {
                $('#LoadingStartDiv').show();
            }
            if ($("#txtActualLoadingFinishTime").val() == "" || $("#txtActualLoadingFinishTime").val() == undefined) {
                $('#LoadingFinishDiv').show();
            }
            if ($("#txtActualPickupDepart").val() == "" || $("#txtActualPickupDepart").val() == undefined) {
                $('#PickUpDepartDiv').show();
            }

        }
        else if (status == 9) // In Fum 
        {
            $('#PickupArrivalDiv').show();
            $('#LoadingStartDiv').show();
            $('#LoadingFinishDiv').show();
            $('#PickUpDepartDiv').show();
            $('#FumInDiv').show();


            $('#FumReleaseDiv').hide();
            $('#FumDepartDiv').hide();



            //$('#DelArrivalDiv').show();
            //$('#DelDepartDiv').show();
            // $('#DeliveryTempDiv').show();
            //$('#divProofOfTemp').show();
            //$('#divTempReq').show();
            //$('#divDamageFiles').show();
            //$('#ReceiverNameAndSignDiv').show();

            $('#DelArrivalDiv').hide();
            $('#DelDepartDiv').hide();
            // $('#DeliveryTempDiv').hide();
            $('#divProofOfTemp').hide();
            $('#divTempReq').hide();
            $('#divDamageFiles').hide();
            $('#ReceiverNameDiv').hide();
            $('#SignatureDiv').hide();
            //  $('#ReceiverNameAndSignDiv').hide();
            $('#LoadingTempDiv').hide();
        }
        else if (status == 7) //DELIVERED
        {
            $('#FumInDiv').show();

            $('#LoadingStartDiv').show();
            $('#LoadingFinishDiv').show();
            $('#PickUpDepartDiv').show();
            $('#PickupArrivalDiv').show();

            $('#FumReleaseDiv').hide();
            $('#FumDepartDiv').hide();

            $('#DelArrivalDiv').hide();
            $('#divProofOfTemp').hide();
            $('#divTempReq').hide();
            $('#LoadingTempDiv').hide();
            // $('#DeliveryTempDiv').hide();
            $('#divDamageFiles').hide();
            //$('#ReceiverNameAndSignDiv').hide();
            $('#ReceiverNameDiv').hide();
            $('#SignatureDiv').hide();
            $('#DelDepartDiv').hide();

            if ($("#DelDepartDiv").val() == "" || $("#DelDepartDiv").val() == undefined) {
                $('#PickUpDepartDiv').show();
            }
        }
    }
    else {
        if (status == 2 || status == 5 || status == 9 || status == 12 || status == 13) //DISPATCHED 
        {
            $('#PickupArrivalDiv').hide();
            $('#divTempReq').hide();
            $('#divDamageFiles').hide();
            $('#divProofOfTemp').hide();
            $('#LoadingTempDiv').hide();

            // $('#PickupArrivalDiv').hide();
            $('#FumReleaseDiv').hide();
            $('#FumDepartDiv').hide();
            $('#DelArrivalDiv').hide();

            //  $('#DeliveryTempDiv').hide();
            $('#LoadingStartDiv').hide();
            $('#LoadingFinishDiv').hide();
            $('#PickUpDepartDiv').hide();
            $('#FumInDiv').hide();
            $('#ReceiverNameDiv').hide();
            $('#SignatureDiv').hide();
            //$('#ReceiverNameAndSignDiv').hide();
            $('#DelDepartDiv').hide();
        }
        else if (status == 6)  //IN-ROUTE TO DELIVERY LOCATION
        {
            $('#FumInDiv').hide();
            $('#LoadingStartDiv').hide();
            $('#LoadingFinishDiv').hide();
            $('#PickUpDepartDiv').hide();
            $('#PickupArrivalDiv').hide();

            $('#FumReleaseDiv').show();
            $('#FumDepartDiv').show();

            $('#DelArrivalDiv').hide();
            $('#divProofOfTemp').hide();
            $('#divTempReq').hide();
            $('#LoadingTempDiv').hide();
            $('#divDamageFiles').hide();
            $('#ReceiverNameDiv').hide();
            $('#SignatureDiv').hide();
            $('#DelDepartDiv').hide();


        }
        else if (status == 7) //DELIVERED
        {

            $('#DelArrivalDiv').show();
            $('#DelDepartDiv').show();
            // $('#DeliveryTempDiv').show();
            $('#divProofOfTemp').hide();
            $('#divTempReq').show();
            $('#divDamageFiles').hide();
            //$('#ReceiverNameAndSignDiv').show();
            $('#ReceiverNameDiv').show();
            $('#SignatureDiv').show();

            $('#FumReleaseDiv').show();
            $('#FumDepartDiv').show();
            $('#FumInDiv').hide();


            $('#FumInDiv').hide();
            $('#LoadingStartDiv').hide();
            $('#LoadingFinishDiv').hide();
            $('#PickUpDepartDiv').hide();
            $('#PickupArrivalDiv').hide();
            $('#LoadingTempDiv').hide();
            if ($("#txtActualDeliveryDepart").val() != "" && $("#txtActualDeliveryDepart").val() != undefined) {
                $("#SignatureButton").hide();
            }
            else {
                $("#SignatureButton").show();
            }


        }

    }
    if (status == 13) //IN STORAGE
    {
        $("#subStatusDiv").show();
        $('#PickupArrivalDiv').hide();
        $('#divTempReq').hide();
        $('#divDamageFiles').hide();
        $('#divProofOfTemp').hide();
        $('#LoadingTempDiv').hide();

        // $('#PickupArrivalDiv').hide();
        $('#FumReleaseDiv').hide();
        $('#FumDepartDiv').hide();
        $('#DelArrivalDiv').hide();

        //  $('#DeliveryTempDiv').hide();
        $('#LoadingStartDiv').hide();
        $('#LoadingFinishDiv').hide();
        $('#PickUpDepartDiv').hide();
        $('#FumInDiv').hide();
        $('#ReceiverNameDiv').hide();
        $('#SignatureDiv').hide();
        //$('#ReceiverNameAndSignDiv').hide();
        $('#DelDepartDiv').hide();
    }
    //#endregion  
}

function IsStatusExist(statusId) {
    var result = false;
    var fumigationId = $.trim($("#hdFumigationId").val());
    $.ajax({
        url: baseUrl + '/Driver/IsStatusExist',
        data: { "statusId": statusId, "fumigationId": fumigationId },
        type: "Post",
        async: false,
        success: function (data) {
            //if (data.length > 0) {
            //    result = true;
            //}
            result = data;
        },
        error: function () { }
    });
    return result;
}

function GetLastStatus(statusId) {
    var result = 0;
    var fumigationId = $.trim($("#hdFumigationId").val());
    $.ajax({
        url: baseUrl + '/Driver/GetLastStatus',
        data: { "statusId": statusId, "fumigationId": fumigationId },
        type: "Post",
        async: false,
        success: function (data) {

            result = data;
        },
        error: function () { }
    });
    return result;
}

function ValidateReuiredField(statusId) {

    var result = "";
    var fumigationId = $.trim($("#hdFumigationId").val());
    $.ajax({
        url: baseUrl + '/Driver/ValidateReuiredField',
        data: { "statusId": statusId, "fumigationId": fumigationId },
        type: "Post",
        async: false,
        success: function (data) {
            if (data.length > 0) {
                result = data;
            }
        },
        error: function () { }
    });
    return result;
}

function removeLastComma(str) {
    return str.replace(/,(\s+)?$/, '');
}

//#region Shipment Status DDL change event to bind sub status
$("#ddlShipmentStatus").change(function () {

    BindSubStatus();
    HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());

});
//#endregion

//#region validation based on status change
function statusChangeValidations() {
    //#region validations
    var retval = true;
    var isPickup = $("input[name='radioFumigationRouteId']:checked").attr('data-ispickup');
    if (isPickup.toLowerCase() == "true") {
        var fumigationRouteId = $("input[name='radioFumigationRouteId']:checked").val();

        if ($("#txtActualPickupArrived").val() == "" || $("#txtActualPickupArrived").val() == undefined) {
            $("#txtActualPickupArrived").focus();
            retval = false;
            toastr.error("Please fill Pickup Arrival date time.");
        }
        else if ($("#ddlShipmentStatus").val() == 5) {
            var proofOfTempImage;
            var loopCount = 0;
            $('#tblProofOfTemp td').each(function () {
                loopCount += 1
                if (loopCount < 3)
                    proofOfTempImage = this.innerText;
            });
            if ($("#txtActualLoadingStartTime").val() == "" && $("#txtActualPickupArrived").val() == "") {
                if ($("#txtActualPickupArrived").val() == "" || $("#txtActualPickupArrived").val() == undefined) {
                    $("#txtActualPickupArrived").focus();
                    retval = false;
                    toastr.error("Please fill Pickup Arrival date time.");
                }
            }
            else {

                if (isValidLoading && (proofOfTempImage == "" || proofOfTempImage == undefined)) {

                    retval = false;
                    toastr.error("Please submit proof of temperature.");


                }
                else if (isValidLoading && $("#txtActualLoadingStartTime").val() == "") {
                    retval = false;
                    toastr.error("Please enter your Loading Start Time.");


                }
                else if (isValidLoading && $("#txtActualLoadingFinishTime").val() == "" && $("#txtActualPickupDepart").val() != "") {
                    //isValidLoading = false;
                    retval = false;
                    toastr.error("Please enter your Loading Finish Time.");
                }
                else if (isValidLoading && $("#txtActualPickupDepart").val() == "" && $("#txtActualLoadingStartTime").val() != "" && $("#txtActualLoadingFinishTime").val() != "") {
                    retval = false;
                    toastr.error("Please submit your pickup departure timestamp.");
                }

            }
        }
        else if ($("#ddlShipmentStatus").val() == 9) {//IN-FUMIGATION

            if ($("#txtActualFumIn").val() == "" || $("#txtActualFumIn").val() == undefined) {
                $("#txtActualFumIn").focus();
                retval = false;
                toastr.error("Please enter the Fum. In Time.");
            }

        }
    }
    else {

        if ($("#ddlShipmentStatus").val() == 6) {//IN-ROUTE TO DELIVERY LOCATION


            if ($("#txtActualPickupDepart").val() == "" || $("#txtActualPickupDepart").val() == undefined) {
                $("#txtActualPickupDepart").focus();
                retval = false;
                toastr.error("Please submit your pickup departure timestamp.");

            }
            else if (($("#txtActualFumigationRelease").val() == "" || $("#txtActualFumigationRelease").val() == undefined)) {
                retval = false;
                $("#txtActualFumigationRelease").focus();
                toastr.error("Please enter the Fum. Release Time.");

            }



            else if (($("#txtActualFumigationDepart").val() == "" || $("#txtActualFumigationDepart").val() == undefined)) {
                retval = false;
                $("#txtActualFumigationDepart").focus();
                toastr.error("Please enter your Fum. Departure Time.");

            }

            else {
                $.alert({
                    title: 'Confirmation!',
                    content: '<b>Remember to attach the appropriate seals to your equipment before leaving!</b>',
                    type: 'blue',
                    typeAnimated: true,
                });
            }

        }

        else if ($("#ddlShipmentStatus").val() == 7) {

            if (($("#txtActualFumigationRelease").val() == "" || $("#txtActualFumigationRelease").val() == undefined)) {
                retval = false;
                $("#txtActualFumigationRelease").focus();
                toastr.error("Please enter the Fum. Release Time.");

            }



            else if (($("#txtActualFumigationDepart").val() == "" || $("#txtActualFumigationDepart").val() == undefined)) {
                retval = false;
                $("#txtActualFumigationDepart").focus()
                toastr.error("Please enter your Fum. Departure Time.");
            }

            else if (($("#txtActualDeliveryDepart").val() == "" || $("#txtActualDeliveryDepart").val() == undefined)) {
                if ($("#txtActualDeliveryArrived").val() == "" || $("#txtActualDeliveryArrived").val() == undefined) {

                    retval = false;
                    $("#txtActualDeliveryArrived").focus();
                    toastr.error("Please enter your Delivery Arrival Time.");
                }
            }
            else {


                if (($("#txtReceiverName").val() == "" || $("#txtReceiverName").val() == undefined)) {
                    retval = false;
                    $("#txtReceiverName").focus();
                    toastr.error("Please enter your Receiver's Name.");
                }

                else if (($("#page img").attr('src') == "" || $("#page img").attr('src') == undefined)) {
                    retval = false;
                    $("#txtReceiverName").focus();
                    toastr.error("Please upload a signature.");
                }

                else if ($("#txtActualDeliveryDepart").val() == "" || $("#txtActualDeliveryDepart").val() == undefined) {
                    $("#txtActualDeliveryDepart").focus();
                    retval = false;
                    toastr.error("Please enter your Delivery Departure Time.");

                }

            }
        }
        //#endregion    }    return retval;
}
//#endregion

function PickupBindDate(_this) {
    var isValid = true;
    Actualbinddate(_this);
    var dtfield = $(_this).attr("id");



    if ($("#txtActualPickupArrived").val() == "" || $("#txtActualPickupArrived").val() == undefined) {
        //if (dtfield == "txtActualPickupArrived") {
        //    isValid = false;
        //}
        isValid = false;
        //$("#txtActualPickupArrived").focus()
        ////$(_this).val("");
        //$.alert({
        //    title: 'Alert!',
        //    content: 'Please fill Pickup Arrival  date time.',
        //    type: 'red',
        //    typeAnimated: true,
        //})

    }
    else {
        if (isValid && dtfield == "txtActualPickupArrived") {
            isValid = false;
            fn_alertforSubmit();
            isValidLoading = false;
        }


    }



    var proofOfTempImage;
    var loopCount = 0;
    $('#tblProofOfTemp td').each(function () {
        loopCount += 1
        if (loopCount < 3)
            proofOfTempImage = this.innerText;
    });

    if (isValid && (proofOfTempImage == "" || proofOfTempImage == undefined)) {
        isValid = false;

        //$.alert({
        //    title: 'Alert!',
        //    content: 'Please upload proof of temperature.',
        //    type: 'red',
        //    typeAnimated: true,
        //});
        // $("#ddlShipmentStatus").val(selectedStatusIndex);
    }


    if (isValid && ($("#txtActualLoadingStartTime").val() == "" || $("#txtActualLoadingStartTime").val() == undefined)) {
        //if (dtfield == "txtActualLoadingStartTime") {
        //    isValid = false;
        //}
        isValid = false;

        //$("#txtActualLoadingStartTime").focus()
        ////$(_this).val("");
        //$.alert({
        //    title: 'Alert!',
        //    content: 'Please fill Loading Start date time.',
        //    type: 'red',
        //    typeAnimated: true,
        //});

    }
    else {
        if (isValid && dtfield == "txtActualLoadingStartTime") {
            isValid = false;
            fn_alertforSubmit();
            isValidLoading = false;
        }

    }


    if (isValid && ($("#txtActualLoadingFinishTime").val() == "" || $("#txtActualLoadingFinishTime").val() == undefined)) {
        //if (dtfield == "txtActualLoadingFinishTime") {
        //    isValid = false;
        //}
        isValid = false;
        // $("#txtActualLoadingFinishTime").focus()
        //$(_this).val("");

        //$.alert({
        //    title: 'Alert!',
        //    content: 'Please fill Loading Finish date time.',
        //    type: 'red',
        //    typeAnimated: true,
        //});
        //$("#ddlShipmentStatus").val(selectedStatusIndex);
    }
    else {
        if (isValid && dtfield == "txtActualLoadingFinishTime") {
            isValid = false;
            fn_alertforSubmit();
            isValidLoading = false;
        }

    }




    if (isValid && ($("#txtActualPickupDepart").val() == "" || $("#txtActualPickupDepart").val() == undefined)) {
        //if (dtfield == "txtActualPickupDepart") {
        //    isValid = false;
        //}
        isValid = false;
        //$("#txtActualPickupDepart").focus()
        //$(_this).val("");
        //$.alert({
        //    title: 'Alert!',
        //    content: 'Please fill Pickup Depart date time.',
        //    type: 'red',
        //    typeAnimated: true,
        //});

    }
    else {
        if (isValid && dtfield == "txtActualPickupDepart") {
            isValid = false;
            fn_alertforSubmit();
        }

    }


    if (isValid && ($("#txtActualFumIn").val() == "" || $("#txtActualFumIn").val() == undefined)) {
        //if (dtfield == "txtActualFumIn") {
        //    isValid = false;
        //}
        isValid = false;
        //$("#txtActualFumIn").focus()
        //$(_this).val("");
        //$.alert({
        //    title: 'Alert!',
        //    content: 'Please fill Fum. In date time.',
        //    type: 'red',
        //    typeAnimated: true,
        //});
    }
    else {
        if (isValid && dtfield == "txtActualFumIn") {
            isValid = false;
            fn_alertforSubmit();
        }

    }


}

function DeliveryBindDate(_this) {
    var isValid = true;
    Actualbinddate(_this);
    var dtfield = $(_this).attr("id");




    if (isValid && ($("#txtActualFumigationRelease").val() == "" || $("#txtActualFumigationRelease").val() == undefined)) {
        //if (dtfield == "txtActualFumIn") {
        //    isValid = false;
        //}
        isValid = false;
        //$("#txtActualFumigationRelease").focus()
        //$.alert({
        //    title: 'Alert!',
        //    content: 'Please fill Fum. Release date time.',
        //    type: 'red',
        //    typeAnimated: true,
        //});
    }
    else {
        if (isValid && dtfield == "txtActualFumigationRelease") {
            isValid = false;
            fn_alertforSubmit();
        }

    }


    if (isValid && ($("#txtActualFumigationDepart").val() == "" || $("#txtActualFumigationDepart").val() == undefined)) {
        //if (dtfield == "txtActualFumIn") {
        //    isValid = false;
        //}
        isValid = false;
        //$("#txtActualFumigationDepart").focus()
        //$.alert({
        //    title: 'Alert!',
        //    content: 'Please fill Fum. Depart date time.',
        //    type: 'red',
        //    typeAnimated: true,
        //});
    }
    else {
        if (isValid && dtfield == "txtActualFumigationDepart") {
            isValid = false;
            fn_alertforSubmit();
        }

    }

    if (isValid && ($("#txtActualDeliveryArrived").val() == "" || $("#txtActualDeliveryArrived").val() == undefined)) {
        //if (dtfield == "txtActualFumIn") {
        //    isValid = false;
        //}
        isValid = false;
        //$("#txtActualDeliveryArrived").focus()
        //$.alert({
        //    title: 'Alert!',
        //    content: 'Please fill Delivery Arrival date time.',
        //    type: 'red',
        //    typeAnimated: true,
        //});
    }
    else {
        if (isValid && dtfield == "txtActualDeliveryArrived") {
            isValid = false;
            fn_alertforSubmit();
        }

    }

    if (isValid && ($("#txtReceiverName").val() == "" || $("#txtReceiverName").val() == undefined)) {
        //if (dtfield == "txtActualFumIn") {
        //    isValid = false;
        //}
        isValid = false;
        //$("#txtReceiverName").focus()

        //$.alert({
        //    title: 'Alert!',
        //    content: "Please fill Receiver Name.",
        //    type: 'red',
        //    typeAnimated: true,
        //});
    }
    else {
        if (isValid && dtfield == "txtReceiverName") {
            isValid = false;

        }

    }
    if (isValid && ($("#page img").attr('src') == "" || $("#page img").attr('src') == undefined)) {
        //if (dtfield == "txtActualFumIn") {
        //    isValid = false;
        //}
        isValid = false;
        //$("#txtReceiverName").focus()
        //$.alert({
        //    title: 'Alert!',
        //    content: "Please upload signature.",
        //    type: 'red',
        //    typeAnimated: true,
        //});
    }
    else {
        //if (isValid && dtfield == "txtReceiverName") {
        //    isValid = false;

        //}

    }


    if (isValid && ($("#txtActualDeliveryDepart").val() == "" || $("#txtActualDeliveryDepart").val() == undefined)) {
        //if (dtfield == "txtActualFumIn") {
        //    isValid = false;
        //}
        isValid = false;
        //$("#txtActualDeliveryDepart").focus()

        //$.alert({
        //    title: 'Alert!',
        //    content: 'Please fill Delivery Depart date time.',
        //    type: 'red',
        //    typeAnimated: true,
        //});
    }
    else {
        if (isValid && dtfield == "txtActualDeliveryDepart") {
            isValid = false;
            fn_alertforSubmit();
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

    //$("#btnFumigationSave").on("click", function () {
    var validation = statusChangeValidations();
    var mendetory = false;
    //var statusText = $("#ddlShipmentStatus").find("option:selected").text().trim();
    var statusText = $("#ddlShipmentStatus").val();
    var subStatusValue = $("#ddlShipmentSubStatus").val();
    //var subStatusText = $("#ddlShipmentSubStatus").find("option:selected").text().trim();
    var subStatusText = $("#ddlShipmentSubStatus").val();
    var txtOtherReason = $("#txtOtherReason").val()
    var message = "";
    if ((statusText == 4 || statusText == 3 || statusText == 13)) {//SHIPMENT ON HOLD,IMMEDIATE ATTENTION

        if (subStatusValue == "") {
            mendetory = true;
            mendetory = true;
            message = "Please select a sub-status.";
        }

        if (!mendetory && (subStatusText == 7 || subStatusText == 11) && txtOtherReason == "" && subStatusValue > 0) {
            mendetory = true;
            message = 'You selected "Other" as your sub-status. Please enter a brief description of the problem.';
        }
        if (mendetory) {
            toastr.error(message);

        }
    }
    if (validation) {
        if (!mendetory) {
            var data = new FormData();
            var FumigationId = $.trim($("#hdFumigationId").val());
            var FumigationRoutsId = $("input[name='radioFumigationRouteId']:checked").val();
            var ActualPickupArrived = $.trim($("#txtActualPickupArrived").val());
            var ActualPickupDepart = $.trim($("#txtActualPickupDepart").val());
            var ActualDeliveryArrived = $.trim($("#txtActualDeliveryArrived").val());
            var ActualDeliveryDepart = $.trim($("#txtActualDeliveryDepart").val());
            var ActualFumigationDepart = $.trim($("#txtActualFumigationDepart").val());
            var ReceiverName = $.trim($("#txtReceiverName").val());
            var ShipmentStatus = $.trim($("#ddlShipmentStatus").val());
            var ShipmentSubStatus = $.trim($("#ddlShipmentSubStatus").val());
            var OtherReason = $.trim($("#txtOtherReason").val());
            var fumIn = $.trim($("#txtActualFumIn").val());
            var loadingStartTime = $.trim($("#txtActualLoadingStartTime").val());
            var loadingFinishTime = $.trim($("#txtActualLoadingFinishTime").val());
            var fumigationRelease = $.trim($("#txtActualFumigationRelease").val());
            var fumigationComment = $.trim($("#txtFumigationComments").val());

            data.append("FumigationId", FumigationId);
            data.append("FumigationRoutsId", FumigationRoutsId);
            data.append("DriverPickupArrival", ActualPickupArrived);
            data.append("DriverPickupDeparture", ActualPickupDepart);
            data.append("DriverDeliveryArrival", ActualDeliveryArrived);
            data.append("DriverDeliveryDeparture", ActualDeliveryDepart);

            data.append("DepartureDate", ActualFumigationDepart);

            data.append("ReceiverName", ReceiverName);
            data.append("StatusId", ShipmentStatus);
            data.append("SubStatusId", ShipmentSubStatus);
            data.append("Reason", OtherReason);
            data.append("DriverFumigationIn", fumIn);
            data.append("DriverLoadingStartTime", loadingStartTime);
            data.append("DriverLoadingFinishTime", loadingFinishTime);
            data.append("DriverFumigationRelease", fumigationRelease);
            data.append("FumigationComment", fumigationComment);

            $.ajax({
                type: "POST",
                beforeSend: function () {
                    showLoader();
                },
                url: baseUrl + '/Driver/SaveFumigationtDetail',
                contentType: false,
                processData: false,
                data: data,
                success: function (data, textStatus, jqXHR) {
                    hideLoader();
                    if (data.IsSuccess == true) {

                        toastr.success(data.Message);
                        setInterval(function () {
                            window.location.href = baseUrl + "Driver/Dashboard";
                        }, 2000)
                        hideSwapButton(3)
                    }
                    else {

                        toastr.warning(data.Message);
                        hideSwapButton(2)
                    }
                    // FOR Driver GPS Tracking history 
                    fn_SaveGPSTracker();
                    // For Fumimgation Waiting Time
                    fn_SaveFumigationWaitingTime();
                },
                error: function (xhr, status, p3, p4) {
                    hideSwapButton(2)
                    var err = "Error " + " " + status + " " + p3 + " " + p4;
                    if (xhr.responseText && xhr.responseText[0] == "{")
                        err = JSON.parse(xhr.responseText).Message;
                    console.log(err);
                }
            });


        }
        else {
            hideSwapButton(2)
        }
    }
    else {
        hideSwapButton(2)
    }
    //})

}
//#endregion

//#region Radio button Check
function CheckRouteStops(routeId, PickDelId) {
    debugger
    var pickupId = $(PickDelId).attr('PickupLocationId');
    var DeliveryId = $(PickDelId).attr('DeliveryLocationId');
    var DriverId = $(PickDelId).attr('DriverId');
    var EquipmentNo = $(PickDelId).attr('EqipmentNo');
    var CustomerId = $(PickDelId).attr('CustomerId');

    shipmentStatus();

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

    GetFumigationDetails(routeId);
    GetFumigationFreightDetails(routeId);
    GetDriverActualTimings(routeId);
    bindDamageFileTbl();
    GetFumigationDamagedFiles(routeId);

}
//#endregion

//#region Shipment Route Radion button checked by Default
function checkFumigationRadionButton() {
    if ($("#tblFumigationRoute tbody tr").length > 0) {
        $($("#tblFumigationRoute tbody tr")[0]).find("input[type=radio]").prop("checked", true);
        var radioValue = $("input[name='radioFumigationRouteId']:checked").val();

        var pickupId = $("input[name='radioFumigationRouteId']:checked").attr('PickupLocationId');
        var DeliveryId = $("input[name='radioFumigationRouteId']:checked").attr('DeliveryLocationId');
        var DriverId = $("input[name='radioFumigationRouteId']:checked").attr('DriverId');
        var EquipmentNo = $("input[name='radioFumigationRouteId']:checked").attr('EqipmentNo');
        var CustomerId = $("input[name='radioFumigationRouteId']:checked").attr('CustomerId');


        var ar = {
            pickupId: pickupId,
            DeliveryId: DeliveryId,
            DriverId: DriverId,
            EquipmentNo: EquipmentNo,
            CustomerId: CustomerId,

        }
        pickDel.push(ar);

        GetFumigationFreightDetails(radioValue);
        GetFumigationDetails(radioValue);
        GetDriverActualTimings(radioValue);
        GetFumigationDamagedFiles(radioValue);
    }

}
//#endregion

//#region select default first Freight radion button
function checkFreightRadionButton() {
    var radioValue = $("input[name='radioFumigationRouteId']:checked").val();
    if (radioValue > 0) {
        $($("#tblFreightDetails tbody tr")[0]).find("input[type=radio]").prop("checked", true);

        var data = $($("#tblFreightDetails tbody tr")[0]).find('td .lblTemperatureRequired').text();
        //  $("#txtTempReq").val(data);

        var radioValue = $("input[name='radioFumigationRouteId']:checked").val();
        GetFumigationProofOfTempFiles(radioValue);

    }
}
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

function GetTemperatureREQ(_this) {
    $("#txtActualTemp").val("");
    $("#fuProofOfTemperature").val("");
    var data = $(_this).closest('tr').find('td .lblTemperatureRequired').text();
    // $("#txtTempReq").val(data);

    bindProofOfTempFileTbl();

}

//#region bind Freight Details
bindFreighttable = function (_data) {
    $("#tblFumiFreightDetailsTable").empty();

    $("#tblFumiFreightDetailsTable").append(_data);


    //function for select first readio button
    checkFreightRadionButton();

}
//#endregion

//#region Fumigation Details for Multiple Routes

function GetFumigationDetails(FumigationRoutesId) {

    $.ajax({
        url: baseUrl + '/Driver/GetFumigationLocationDetails/' + FumigationRoutesId,
        type: "Get",
        dataType: "json",
        async: false,
        success: function (data) {

            if (data != null) {
                if (data.AirWayBill != "" && data.AirWayBill != null) {
                    $("#txtAwbContNo").val(data.AirWayBill);
                }
                else if (data.ContainerNo != "" && data.ContainerNo != null) {
                    $("#txtAwbContNo").val(data.ContainerNo);
                }
                else {
                    $("#txtAwbContNo").val('');
                }

                $("#hdFumigationId").val(data.FumigationId);
                $("#txtShipRefNo").val(data.ShipmentRefNo);
                $("#txtCustomerPO").val(data.CustomerPO);
                $("#txtPickupLocation").val(data.PickUpLocation);
                $("#txtDeliveryLocation").val(data.DeliveryAddress);
                $("#txtEstimatedPickupArrival").val(ConvertDate(data.PickUpArrivalDate, true));
                $("#txtEstimatedDeliveryArrival").val(ConvertDate(data.DeliveryArrive, true));
                $("#txtFumigationLocation").val(data.FumigationAddress);
                $("#ddlShipmentStatus").val(data.StatusId);
                // removeItemFromStatusDDL(data.StatusId);


                selectedStatusIndex = data.StatusId;
                BindSubStatus();
                $("#ddlShipmentSubStatus").val(data.SubStatusId);
                $("#txtOtherReason").val(data.FumigationReason);
                if (data.SubStatusId == 7 || data.SubStatusId == 11) {
                    $("#txtOtherReason").prop('disabled', false);
                }


                $("#txtReceiverName").val(data.ReceiverName);
                $("#txtSign").attr('src', data.DigitalSignature);

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
        debugger
        var statusId = GetLastStatus(id)
        id = statusId;
        // HideSubstatus(id, $("#ddlShipmentSubStatus").val());
    }

    if (id == 2) {
        var isPickup = $("input[name='radioFumigationRouteId']:checked").attr('data-ispickup');
        if (isPickup.toLowerCase() == "true") {
            $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 3 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 4 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 5 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 6 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 7 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 9 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 13 + "]").remove();
            HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());
            var driverPickupArrival = $("#txtActualPickupArrived").val();
            if (driverPickupArrival != "") {
                shipmentStatus();
                $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 6 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 7 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 9 + "]").remove();
                HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());
            }
        }
        else {
            shipmentStatus();
            $("#ddlShipmentStatus option[value=" + 5 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 3 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 4 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 6 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 7 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 9 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 12 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 13 + "]").remove();
            HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());
        }
    }
    else if (id == 12) {

        var isPickup = $("input[name='radioFumigationRouteId']:checked").attr('data-ispickup');
        if (isPickup.toLowerCase() == "true") {
            $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 6 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 7 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 9 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 12 + "]").remove();
            $("#ddlShipmentStatus").val(5);
            HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());

            if ($("#txtActualPickupDepart").val() != "") {
                shipmentStatus();
                $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 5 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 6 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 7 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 12 + "]").remove();
                $("#ddlShipmentStatus").val(9);
                HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());
            }
        }
        else {
            $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 3 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 4 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 6 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 7 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 9 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 5 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 13 + "]").remove();
            HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());

        }

    }
    else if (id == 5) {
        var isPickup = $("input[name='radioFumigationRouteId']:checked").attr('data-ispickup');
        if (isPickup.toLowerCase() == "true") {
            $("#ddlShipmentStatus option[value=" + 12 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 6 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 7 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 9 + "]").remove();
            $("#ddlShipmentStatus").val(5);
            HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());
            if ($("#txtActualPickupDepart").val() != "") {
                shipmentStatus();
                $("#ddlShipmentStatus option[value=" + 12 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 5 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 6 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 7 + "]").remove();
                HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());
            }
        }
        else {
            $("#ddlShipmentStatus option[value=" + 12 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 3 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 4 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 6 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 7 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 9 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 13 + "]").remove();
            HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());
        }

    }
    else if (id == 9) {
        var isPickup = $("input[name='radioFumigationRouteId']:checked").attr('data-ispickup');
        if (isPickup.toLowerCase() == "true") {
            $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
            if ($("#txtActualFumIn").val() != "") {
                $("#ddlShipmentStatus option[value=" + 3 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 4 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 12 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 13 + "]").remove();
            }
            $("#ddlShipmentStatus").val(9);
            $("#ddlShipmentStatus option[value=" + 5 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 6 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 7 + "]").remove();

            HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());
        }
        else {

            if (IsStatusExist(id)) {
                $("#ddlShipmentStatus option[value=" + 12 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 5 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 7 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 9 + "]").remove();
                $("#ddlShipmentStatus").val(6);
                HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());
            }
            else {
                $("#ddlShipmentStatus option[value=" + 12 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 3 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 4 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 5 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 7 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 6 + "]").remove();
                HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());
            }
        }
    }
    else if (id == 6) {
        var isPickup = $("input[name='radioFumigationRouteId']:checked").attr('data-ispickup');
        if (isPickup.toLowerCase() == "true") {
            $("#ddlShipmentStatus option[value=" + 12 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 3 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 4 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 5 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 7 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 9 + "]").remove();
            $("#ddlShipmentStatus option[value=" + 13 + "]").remove();
            $("#ddlShipmentStatus").val(6);
        }
        else {
            if ($("#txtActualFumigationDepart").val() == "") {
                $("#ddlShipmentStatus option[value=" + 12 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 5 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 7 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 9 + "]").remove();
                $("#ddlShipmentStatus").val(6);
            }
            else {
                $("#ddlShipmentStatus option[value=" + 12 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 5 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 6 + "]").remove();
                $("#ddlShipmentStatus option[value=" + 9 + "]").remove();
                $("#ddlShipmentStatus").val(7);
            }
        }
        HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());
    }
    else if (id == 7) {
        $("#ddlShipmentStatus option[value=" + 12 + "]").remove();
        $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
        $("#ddlShipmentStatus option[value=" + 3 + "]").remove();
        $("#ddlShipmentStatus option[value=" + 4 + "]").remove();
        $("#ddlShipmentStatus option[value=" + 5 + "]").remove();
        $("#ddlShipmentStatus option[value=" + 6 + "]").remove();
        $("#ddlShipmentStatus option[value=" + 9 + "]").remove();
        $("#ddlShipmentStatus option[value=" + 13 + "]").remove();
        $("#ddlShipmentStatus").val(7);
        HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());
    }


    //else if (id == 3 || id == 4) {
    //    var isPickup = $("input[name='radioFumigationRouteId']:checked").attr('data-ispickup');
    //    if (isPickup.toLowerCase() == "true") {

    //        if ($("#txtActualFumIn").val() != "") {
    //            $("#ddlShipmentStatus option[value=" + 12 + "]").remove();
    //            $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
    //            $("#ddlShipmentStatus option[value=" + 3 + "]").remove();
    //            $("#ddlShipmentStatus option[value=" + 4 + "]").remove();
    //            $("#ddlShipmentStatus option[value=" + 5 + "]").remove();
    //            $("#ddlShipmentStatus option[value=" + 6 + "]").remove();
    //            $("#ddlShipmentStatus option[value=" + 7 + "]").remove();
    //        }
    //        else {
    //            var statusId = GetLastStatus(id);

    //            if (statusId != 2) {
    //                $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
    //            }
    //            if (statusId != 5) {
    //                $("#ddlShipmentStatus option[value=" + 5 + "]").remove();
    //            }
    //            if (statusId != 6) {
    //                $("#ddlShipmentStatus option[value=" + 6 + "]").remove();
    //            }
    //            if (statusId != 9) {
    //                $("#ddlShipmentStatus option[value=" + 9 + "]").remove();
    //            }
    //            if (statusId != 7) {
    //                $("#ddlShipmentStatus option[value=" + 7 + "]").remove();
    //            }

    //        }
    //    }
    //    else {


    //        var statusId = GetLastStatus(id);

    //        if (statusId != 2) {
    //            $("#ddlShipmentStatus option[value=" + 2 + "]").remove();
    //        }
    //        if (statusId != 5) {
    //            $("#ddlShipmentStatus option[value=" + 5 + "]").remove();
    //        }
    //        if (statusId != 6 && statusId != 9) {
    //            $("#ddlShipmentStatus option[value=" + 6 + "]").remove();
    //        }

    //        $("#ddlShipmentStatus option[value=" + 9 + "]").remove();

    //        if (statusId != 7) {
    //            $("#ddlShipmentStatus option[value=" + 7 + "]").remove();
    //        }

    //    }
    //    HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());
    //}
};
//#endregion

//#region shipment status
function shipmentStatus() {

    $.ajax({
        url: baseUrl + '/Driver/GetDriverFumimgationStatus',
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
            $("#txtOtherReason").prop('disabled', true);
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
        $("#txtOtherReason").val("");
        if (this.value == 11 || this.value == 7) {
            $("#txtOtherReason").prop('disabled', false);
        }
        else {

            $("#txtOtherReason").prop('disabled', true);
        }
        HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());
    });
}
//#endregion

//#region Get Shipment Freight Details

function GetFumigationFreightDetails(FumigationRouteStopeId) {
    $.ajax({
        url: baseUrl + '/Driver/GetFumigationFreightDetails/' + FumigationRouteStopeId,
        type: "Get",
        dataType: "html",
        async: false,
        success: function (data) {
            if (data.length > 0) {
                $("#dvFumigationFreigtDetails").show();
                bindFreighttable(data);
            }

            else {
                $("#dvFumigationFreigtDetails").hide();
                // $("#txtTempReq").val("");

            }


        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
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

//#region Button Upload
var btnUpload = function () {
    $(".btnProofOfTemp").on("click", function () {
        var isvalid = true;
        var tempMessage = "";
        var isPickup = $("input[name='radioFumigationRouteId']:checked").attr('data-ispickup');
        var actualTemp = $("#txtActualTemp").val();
        //var deliveryTemp = $("#txtDeliveryTemp").val();
        if (isPickup.toLowerCase() == "true") {
            if (actualTemp == "") {
                isvalid = false;
                tempMessage = "Please enter your shipment's loading temperature.";
                $("#txtActualTemp").focus();
            }

        }
        // else {

        //if (deliveryTemp == "") {
        //    isvalid = false;
        //    tempMessage = "Please fill Delivery Temp.";
        //   // $("#txtDeliveryTemp").focus();
        //}

        //  }
        if (isvalid) {

            if (isFormValid('divProofOfTemp')) {

                var fileUploader = $("#fuProofOfTemperature")[0].files;

                var unit = $("#ddlTemperatureUnit").val();
                if (!isNaN(actualTemp)) {
                    if (unit == 'C') {
                        actualTemp = CelsiusToFahrenheit(actualTemp);
                        //deliveryTemp = CelsiusToFahrenheit(deliveryTemp);
                    }
                }
                // Date Time Format 
                var d = new Date($.now());
                date = ((d.getMonth() + 1) + "-" + d.getDate() + "-" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds());
                //

                var radioValue = $("input[name='radioFumigationRouteId']:checked").val();
                var data = new FormData();

                if (fileUploader.length) {
                    for (let i = 0; i < fileUploader.length; i++) {
                        data.append("UploadedTemperatureProofFiles", fileUploader[i]);
                    }
                }

                data.append("ActualTemperature", actualTemp);
                data.append("FumigationRoutsId", radioValue);
                showLoader();
                debugger
                //data.append("DeliveryTemp", deliveryTemp);
                $.ajax({
                    type: "POST",
                    url: baseUrl + '/Driver/SaveFumimgationProofOfTemperature',
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
                            GetFumigationProofOfTempFiles(radioValue);
                        }
                        else {
                            toastr.warning(data.Message);
                            $("#txtActualTemp").val("");
                            $("#fuProofOfTemperature").val("");
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
            }
            else {
                $("#btnProofOfTemp").css("background", "linear-gradient(90deg, rgba(245,31,31,1) 0%, rgba(241,116,116,1) 100%)");
                $("#btnProofOfTemp").text('TRY AGAIN');
                toastr.error("Please upload your shipment's loading proof of temperature.");
            }
        }
        else {
            toastr.error(tempMessage);
            $("#btnProofOfTemp").css("background", "linear-gradient(90deg, rgba(245,31,31,1) 0%, rgba(241,116,116,1) 100%)");
            $("#btnProofOfTemp").text('TRY AGAIN');
            //toastr.error("Please upload your shipment's loading proof of temperature.");
        }
    })
}
//#endregion

//#region Bind Proof Of Temp files 

function bindProofOfTempFileTbl() {
    $(".divProofOfTemper").hide();
    var tr = "";
    $("#tblProofOfTemp tbody").empty();
    var radioValue = $("input[name='radioFumigationRouteId']:checked").val();
    var isPickup = $("input[name='radioFumigationRouteId']:checked").attr('data-ispickup');
    // isDeliveryTemp = true;
    if (radioValue > 0) {
        var ProofFiles = glbProofOfTempFile.filter(x => x.FumigationRouteId == radioValue);
        if (ProofFiles.length > 0) {
            for (var i = 0; i < ProofFiles.length; i++) {

                if (JSON.parse(isPickup.toLowerCase()) == ProofFiles[i].IsLoading) {
                    $(".divProofOfTemper").show();
                    //  isDeliveryTemp = false;
                    tr += '<tr data-file-url=' + ProofFiles[i].ImageUrl + ' ondblclick="javascript:ViewDocument(this)">' +

                        '<td>' + ProofFiles[i].actualTemp + '</td>' +
                        //'<td>' + ProofFiles[i].FileName + '</td>' +
                        '<td>' + ProofFiles[i].Date + '</td>' +
                        '<td><button type="button" data-file-url=' + ProofFiles[i].ImageUrl + ' onclick="ViewDocument(this)" class="delete_icon chng-color-view"><i class="far fa-eye"></i></button><button type="button" class="delete_icon chng-color-Trash" onclick="remove_proofOfTemp_row(this,' + ProofFiles[i].proofImageId + ')"> <i class="far fa-trash-alt"></i></button></td>' +
                        '</tr>'
                }
            }

        }
    }

    $("#tblProofOfTemp tbody").append(tr);

}

//#endregion

//#region  bind Proof Of Temp Files

function GetFumigationProofOfTempFiles(FumigationRouteId) {

    $.ajax({
        url: baseUrl + '/Driver/GetFumigationProofOfTempFiles/',
        type: "Get",
        dataType: "json",
        async: false,
        data: { "id": FumigationRouteId },
        success: function (data) {
            debugger
            if (data != null) {
                glbProofOfTempFile = new Array();
                //#region for binding Proof Of Temp Image
                for (let i = 0; i < data.length; i++) {
                    var ProofOfTempFile = {};
                    ProofOfTempFile.proofImageId = data[i].proofImageId;
                    ProofOfTempFile.actualTemp = data[i].proofActualTemp;
                    ProofOfTempFile.FileName = data[i].ProofImage;
                    ProofOfTempFile.Date = ConvertDate(data[i].ProofDate, true);
                    ProofOfTempFile.FumigationRouteId = data[i].FumigationRouteId;
                    ProofOfTempFile.ImageUrl = data[i].ImageUrl;
                    ProofOfTempFile.IsLoading = data[i].IsLoading;
                    glbProofOfTempFile.push(ProofOfTempFile);
                }
                debugger
                bindProofOfTempFileTbl();
            }
            //#endregion

        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
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
                            url: baseUrl + 'Driver/DeleteFumigationProofOfTemprature',
                            data: { imageId: proofImageId },
                            type: "GET",
                            async: false,
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",

                            success: function (data) {
                                if (data.IsSuccess == true) {
                                    $(_this).closest("tr").remove();
                                    toastr.success(data.Message), 2000;

                                }
                                else {
                                    toastr.warning(data.Message), 2000;
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
                var fileUploaderDamaged = $("#fuDamageFiles");
                var damageDescription = "";

                // Date Time Format 
                var d = new Date($.now());
                date = ((d.getMonth() + 1) + "-" + d.getDate() + "-" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds());
                //
                var radioValue = $("input[name='radioFumigationRouteId']:checked").val();

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
                    data.append("FumigationRoutsId", radioValue);

                    $.ajax({
                        type: "POST",
                        url: baseUrl + '/Driver/SaveFumigationDamagedFiles',
                        contentType: false,
                        processData: false,
                        data: data,
                        async: false,
                        success: function (data, textStatus, jqXHR) {
                            if (data.IsSuccess == true) {
                                toastr.success(data.Message);
                                GetFumigationDamagedFiles(radioValue);

                            }
                            else {
                                toastr.warning(data.Message);
                                GetFumigationDamagedFiles(radioValue);
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
                $("#fuDamageFiles").val("");
                $("#txtDamagedFileName").val("");
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
    $(".divDamageDoc").hide();

    var tr = "";
    $("#tblDamagedFiles tbody").empty();

    var radioValue = $("input[name='radioFumigationRouteId']:checked").val();

    if (radioValue > 0) {

        var damageFiles = glbDamageFile.filter(x => x.FumigationRouteId == radioValue);
        if (damageFiles.length > 0) {
            $(".divDamageDoc").show();
            for (var i = 0; i < damageFiles.length; i++) {
                tr += '<tr data-file-url=' + damageFiles[i].ImageUrl + ' ondblclick="javascript:ViewDocument(this)">' +
                    '<td>' + damageFiles[i].DamageDescription + '</td>' +
                    //'<td>' + damageFiles[i].FileName + '</td>' +
                    '<td>' + damageFiles[i].Date + '</td>' +
                    '<td><button type="button" data-file-url=' + damageFiles[i].ImageUrl + ' onclick="ViewDocument(this)" class="delete_icon chng-color-view"><i class="far fa-eye"></i></button><button type="button" class="delete_icon chng-color-Trash" onclick="remove_DamageFiles_row(this,' + damageFiles[i].DamagedID + ',' + i + ')"> <i class="far fa-trash-alt"></i></button></td>' +
                    '</tr>'
            }
        }
    }


    $("#tblDamagedFiles tbody").append(tr);
}
//#endregion

//#region  bind Damaged Files

function GetFumigationDamagedFiles(FumigationRouteId) {
    $.ajax({
        url: baseUrl + '/Driver/GetFumigationDamagedFiles/' + FumigationRouteId,
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
                    damageFile.FumigationRouteId = data[i].FumigationRouteId;
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
                            url: baseUrl + 'Driver/DeleteFumigationDamageFiles',
                            data: { imageId: DamagedID },
                            type: "GET",
                            async: false,
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",

                            success: function (data) {
                                if (data.IsSuccess == true) {
                                    $(_this).closest("tr").remove();
                                    toastr.success(data.Message), 2000;

                                }
                                else {
                                    toastr.warning(data.Message), 2000;
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

//#region Get Driver Actual Timings Details
var GetDriverActualTimings = function (FumigationRoutsId) {
    $.ajax({
        url: baseUrl + "Driver/GetDriverActualTimings/" + FumigationRoutsId,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            showLoader();
        },
        success: function (response) {
            if (response != null) {
                actualPickupArrivedHasValue = "";
                $("#txtActualPickupArrived").val(ConvertDate(response.DriverPickupArrival, true));
                $("#txtActualPickupDepart").val(ConvertDate(response.DriverPickupDeparture, true));
                $("#txtActualDeliveryArrived").val(ConvertDate(response.DriverDeliveryArrival, true));
                $("#txtActualDeliveryDepart").val(ConvertDate(response.DriverDeliveryDeparture, true));
                $("#txtActualFumigationDepart").val(ConvertDate(response.DepartureDate, true));

                $("#txtActualFumIn").val(ConvertDate(response.DriverFumigationIn, true));
                $("#txtActualLoadingStartTime").val(ConvertDate(response.DriverLoadingStartTime, true));
                $("#txtActualLoadingFinishTime").val(ConvertDate(response.DriverLoadingFinishTime, true));
                $("#txtActualFumigationRelease").val(ConvertDate(response.DriverFumigationRelease, true));

                $("#txtReceiverName").val(response.ReceiverName);

                if (response.DriverPickupArrival == null) {
                    $("#txtActualPickupArrived").attr('disabled', false);
                    $('#txtActualPickupArrived').attr('readonly', 'readonly');
                    $('#txtActualPickupArrived').css('background-color', '#ffffff');
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

                if (response.DepartureDate == null) {
                    $("#txtActualFumigationDepart").attr('disabled', false);
                    $('#txtActualFumigationDepart').attr('readonly', 'readonly');
                    $('#txtActualFumigationDepart').css('background-color', '#ffffff');
                }
                else {
                    $("#txtActualFumigationDepart").attr('disabled', true);
                }

                if (response.ReceiverName == null) {
                    $("#txtReceiverName").attr('disabled', false);

                }
                else {
                    $("#txtReceiverName").attr('disabled', true);
                }

                if (response.DriverFumigationIn == null) {
                    $("#txtActualFumIn").attr('disabled', false);
                    $('#txtActualFumIn').attr('readonly', 'readonly');
                    $('#txtActualFumIn').css('background-color', '#ffffff');
                }
                else {
                    $("#txtActualFumIn").attr('disabled', true);
                }

                if (response.DriverLoadingStartTime == null) {
                    $("#txtActualLoadingStartTime").attr('disabled', false);
                    $('#txtActualLoadingStartTime').attr('readonly', 'readonly');
                    $('#txtActualLoadingStartTime').css('background-color', '#ffffff');
                }
                else {
                    $("#txtActualLoadingStartTime").attr('disabled', true);
                }

                if (response.DriverLoadingFinishTime == null) {
                    $("#txtActualLoadingFinishTime").attr('disabled', false);
                    $('#txtActualLoadingFinishTime').attr('readonly', 'readonly');
                    $('#txtActualLoadingFinishTime').css('background-color', '#ffffff');
                }
                else {
                    $("#txtActualLoadingFinishTime").attr('disabled', true);
                }

                if (response.DriverFumigationRelease == null) {
                    $("#txtActualFumigationRelease").attr('disabled', false);
                    $('#txtActualFumigationRelease').attr('readonly', 'readonly');
                    $('#txtActualFumigationRelease').css('background-color', '#ffffff');

                }
                else {
                    $("#txtActualFumigationRelease").attr('disabled', true);
                }
                // HideSubstatus($("#ddlShipmentStatus").val(), $("#ddlShipmentSubStatus").val());
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

//#region Disabled Calender class

$(document).ready(function () {
    $(".calenderDisableRemove").removeClass("ui-state-disabled");
});

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
    var FumigationRouteId = $("input[name='radioFumigationRouteId']:checked").val();
    var canvas = $("#canvas").get(0);
    var imgData = canvas.toDataURL();

    data.append("DigitalSignature", imgData);
    data.append("FumigationRoutsId", FumigationRouteId);
    $.ajax({
        url: baseUrl + "Driver/UpdateFumigationSignaturePadDetail",
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
            fn_GetGPSTracker('Departed from', '', 'txtDeliveryLocation');


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



//#region get GPS Tracker

var fn_GetdriverStatus = function (event, pickuplocation, deliveryLocation) {
    debugger
    var d = new Date($.now());
    date = ((d.getMonth() + 1) + "-" + d.getDate() + "-" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds());

    var FumigationId = $.trim($("#hdFumigationId").val());
    var FumigationRoutsId = $("input[name='radioFumigationRouteId']:checked").val();

    //if (event != "") {
    //    var status = $("#ddlShipmentStatus").find("option:selected").text().trim();

    //    status = $.trim(status).toUpperCase();

    //    if (status == "IMMED ATTENTION") {//Immediate Attention
    //        event = $("#" + pickuplocation).val();
    //    }
    //    if (status == "ON HOLD") {//Change of Status / On Hold"
    //        event = $("#" + pickuplocation).val();
    //    }
    //    else if (status == "DISPATCHED") {
    //        event = $("#" + pickuplocation).val();
    //    }
    //    else if (status == "LOADING") {//Loading at pick up location
    //        var eventL = $("#" + pickuplocation).val();
    //        event = status.replace("pick up location", eventL);
    //    }
    //    else if (status == "IN-ROUTE") {//In-Route to delivery location
    //        var eventR = $("#" + deliveryLocation).val();
    //        event = status.replace("delivery location", eventR);
    //    }

    //    else if (status == "DELIVERED") {
    //        event = $("#" + deliveryLocation).val();
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
                FumigationId: FumigationId,
                FumigationRoutsId: FumigationRoutsId,
                Event: events,
            }
            glbarEvent.push(arEvent);

        });
    } else {
        console.log("Browser doesn't support geolocation!");
    }
}
//#endregion

//#region get let long every 3 sec
var getLateLong = function () {
    debugger
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var latitude = position.coords.latitude;
            var longitude = position.coords.longitude;
            var d = new Date($.now());
            date = ((d.getMonth() + 1) + "-" + d.getDate() + "-" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds());
            var FumigationId = $.trim($("#hdFumigationId").val());
            var FumigationRoutsId = $("input[name='radioFumigationRouteId']:checked").val();
            var arEvent = {
                Latitude: latitude,
                longitude: longitude,
                CreatedOn: date,
                FumigationId: FumigationId,
                FumigationRoutsId: FumigationRoutsId,
                Event: null,
            }
            glbarEvent.push(arEvent);
            fn_SaveGPSTracker();
        });
    } else {
        console.log("Browser doesn't support geolocation!");
    }
}
//#endregion

//#region Get GPS Pick location and Delivery Location

var fn_GetGPSTracker = function (event, pickuplocation, deliveryLocation) {
    debugger
    // Date Time Format 
    var d = new Date($.now());
    date = ((d.getMonth() + 1) + "-" + d.getDate() + "-" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds());
    //
    var FumigationId = $.trim($("#hdFumigationId").val());
    var FumigationRoutsId = $("input[name='radioFumigationRouteId']:checked").val();

    if (pickuplocation != "") {
        var pickupLocation = $("#" + pickuplocation).val();
    }
    else {
        var pickupLocation = '';
    }

    if (deliveryLocation != "") {
        var deliveryLocation = $("#" + deliveryLocation).val();
    }
    else {
        var deliveryLocation = '';
    }

    var events = (event + ' ' + pickupLocation + ' ' + deliveryLocation).trim();
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var latitude = position.coords.latitude;
            var longitude = position.coords.longitude;


            var arEvent = {
                Latitude: latitude,
                longitude: longitude,
                CreatedOn: date,
                FumigationId: FumigationId,
                FumigationRoutsId: FumigationRoutsId,
                Event: events,
            }
            glbarEvent.push(arEvent);


        });
    } else {
        console.log("Browser doesn't support geolocation!");
    }
}
//#endregion

//#region Save GPS Tracker History 
function fn_SaveGPSTracker() {
    debugger
    var data = glbarEvent;
    $.ajax({
        url: baseUrl + "Driver/SaveFumigationGPSTracker",
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

//#region To redirectthe Direction Map 
function fn_getDirection(myLocation) {

    //let _url = baseUrl + "Driver/GPSTracker?location='" + myLocation + "'";
    //window.open(_url, '_blank');
    myLocation = myLocation.replace(/^[^,]+, */, '');
    window.open("https://www.google.com/maps/dir/" + formatedCurrentAddress + "/" + myLocation);

}
function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
    } else {
        alert("Geolocation is not supported by this browser.");
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
                console.log(results[0].formatted_address);
                $('#address').html('Address:' + results[0].formatted_address);
            }
            else {
                $('#address').html('Geocoding failed: ' + status);
                console.log("Geocoding failed: " + status);
            }
        }); //geocoder.geocode()
    }
} //showPosition
//#endregion

//#region camera snap 

function opencam() {
    navigator.getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.oGetUserMedia || navigator.msGetUserMedia;
    if (navigator.getUserMedia) {
        navigator.getUserMedia({ video: true }, streamWebCam, throwError);
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
    // alert(e.name);
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
            $.alert({
                title: 'Alert!',
                content: 'Please fill the Loading Temperature.',
            });
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
            $.alert({
                title: 'Alert!',
                content: 'Please fill the Damage Description.',
            });
        }
    }

});
$('#retake').click(function (event) {
    $('#vid').css('z-index', '30');
    $('#capture').css('z-index', '20');
});

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
//#endregion

//#region print pdf
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
//#endregion

function CheckImageExtension(_this) {
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
                toastr.warning('Uploads must be in JPG, JPEG, PNG, PDF, or JFIF formats.');
                $(_this).val(null);
                return Isvalid;
            }
        }
    }
    else {
        toastr.warning("Please Browse to select your Upload File.", "")
        return Isvalid;
    }

}

//#region show Camera Popup
var fn_ShowCameraPopup = function (isproof) {
    $("#IsProofCamfile").val(isproof);
    $("#DamageTitle").hide();
    $("#ProofTitle").hide();
    if (isproof) {
        $("#ProofTitle").show();
    }
    else {
        $("#DamageTitle").show();
    }
    $("#divPopUpCamera").modal("show");


}
//#endregion

//#region Save web-Camera  

$("#btnWebCameraSave").on("click", function () {
    var proof = $("#IsProofCamfile").val();

    if (proof == "true") {
        var data = new FormData();
        var FumigationId = $.trim($("#hdFumigationId").val());
        var FumigationRoutsId = $("input[name='radioFumigationRouteId']:checked").val();
        var ActualTemp = $.trim($("#txtwebActualTemp").val());
        var unit = $("#ddlwebTemperatureUnit").val();
        if (unit == 'C') {
            ActualTemp = CelsiusToFahrenheit(ActualTemp);
        }
        var canvas = $("#canvas-cam").get(0);
        var ImgData = canvas.toDataURL();
        var camImgData = ImgData.replace('data:image/png;base64,', '');


        data.append("ImageUrl", camImgData);
        data.append("FumigationRoutsId", FumigationRoutsId);
        data.append("ActualTemperature", ActualTemp);


        $.ajax({
            type: "POST",
            url: baseUrl + '/Driver/saveWebCamera',
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
                    GetFumigationProofOfTempFiles(FumigationRoutsId);

                }
                else {
                    toastr.warning(data.Message);
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
        var FumigationId = $.trim($("#hdFumigationId").val());
        var FumigationRoutsId = $("input[name='radioFumigationRouteId']:checked").val();
        var DamagedFileName = $.trim($("#txtWebDamagedFileName").val());
        var canvas = $("#canvas-cam").get(0);
        var ImgData = canvas.toDataURL();
        var camImgData = ImgData.replace('data:image/png;base64,', '');

        data.append("ImageUrl", camImgData);
        data.append("FumigationRouteId", FumigationRoutsId);
        data.append("ImageDescription", DamagedFileName);


        $.ajax({
            type: "POST",
            url: baseUrl + '/Driver/SaveDamageWebCamera',
            contentType: false,
            processData: false,
            data: data,
            success: function (data, textStatus, jqXHR) {
                if (data.IsSuccess == true) {
                    toastr.success(data.Message);
                    $("#divPopUpCamera").modal("hide");
                    $("#txtWebDamagedFileName").val("");
                    $("#canvas-cam").val("");
                    GetFumigationDamagedFiles(FumigationRoutsId);

                }
                else {
                    toastr.warning(data.Message);
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

//#region Alert for submit button
function fn_alertforStatusSubmit() {
    toastr.success("Your data has successfully been added to your shipment.</br>  Don't forget to click on the Submit button to save all changes.");
}

function fn_alertforSubmit() {
    toastr.success("Your data has successfully been added to your shipment.<br/>  Don't forget to click on the Submit button to save all changes.");
}
//#endregion

//#region Get Fumigation Waiting time for Pick location and Delivery Location

var fn_GetFumigationWaitingTime = function (pickupArrivalDate, ESTPickupArrival, pickupDepartDate, DeliveryArrivalDate, ESTDeliveryArrival, DeliveryDepartDate) {

    var FumigationId = $.trim($("#hdFumigationId").val());
    var FumigationRoutsId = $("input[name='radioFumigationRouteId']:checked").val();

    if (pickupArrivalDate >= $("#" + ESTPickupArrival).val()) {
        var pickupArrivalDate = $("#" + pickupArrivalDate).val();
    }
    else if (pickupArrivalDate < $("#" + ESTPickupArrival).val()) {
        var pickupArrivalDate = $("#" + ESTPickupArrival).val();
    }
    else {
        var pickupArrivalDate = '';
    }
    if (pickupDepartDate != null && pickupDepartDate != undefined) {
        pickupDepartDate = $("#" + pickupDepartDate).val();

    }
    else {
        pickupDepartDate = '';
    }

    if (DeliveryArrivalDate != null && ESTDeliveryArrival != null && DeliveryArrivalDate >= $("#" + ESTDeliveryArrival).val()) {
        var DeliveryArrivalDate = $("#" + DeliveryArrivalDate).val();
    }
    else if (DeliveryArrivalDate != null && ESTDeliveryArrival != null && DeliveryArrivalDate < $("#" + ESTDeliveryArrival).val()) {
        var DeliveryArrivalDate = $("#" + ESTDeliveryArrival).val();
    }
    else {
        var DeliveryArrivalDate = '';
    }

    if (DeliveryDepartDate != null && DeliveryDepartDate != undefined) {
        DeliveryDepartDate = $("#" + DeliveryDepartDate).val();
    }
    else {
        DeliveryDepartDate = '';
    }
    var arEvent = {
        PickupArrivedOn: pickupArrivalDate,
        PickupDepartedOn: pickupDepartDate,
        DeliveryArrivedOn: DeliveryArrivalDate,
        DeliveryDepartedOn: DeliveryDepartDate,
        FumigationId: FumigationId,
        FumigationRoutsId: FumigationRoutsId,
        PickUpLocationId: pickDel[0].pickupId,
        DeliveryLocationId: pickDel[0].DeliveryId,
        DriverId: pickDel[0].DriverId,
        EquipmentNo: pickDel[0].EquipmentNo,
        CustomerId: pickDel[0].CustomerId,


    }
    glbarWaitingTime.push(arEvent);


}
//#endregion

//#region Save Waiting Time 
function fn_SaveFumigationWaitingTime() {

    // var data = glbarWaitingTime;

    var Modal = {
        PickupArrivedOn: glbarWaitingTime[0].PickupArrivedOn,
        PickupDepartedOn: glbarWaitingTime[0].PickupDepartedOn,
        DeliveryArrivedOn: glbarWaitingTime[0].DeliveryArrivedOn,
        DeliveryDepartedOn: glbarWaitingTime[0].DeliveryDepartedOn,
        FumigationId: glbarWaitingTime[0].FumigationId,
        FumigationRoutsId: glbarWaitingTime[0].FumigationRoutsId,
        DriverId: glbarWaitingTime[0].DriverId,
        EquipmentNo: glbarWaitingTime[0].EquipmentNo,
        CustomerId: glbarWaitingTime[0].CustomerId,
        PickUpLocationId: glbarWaitingTime[0].PickUpLocationId,
        DeliveryLocationId: glbarWaitingTime[0].DeliveryLocationId,
    };

    $.ajax({
        url: baseUrl + "Driver/SaveFumigationWaitingTime",
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
//#endregion//#region Current date and Time
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


//#region Get Fumigation Comments
function GetFumigationComments() {

    var fumigationId = $.trim($("#hdFumigationId").val());
    $.ajax({
        url: baseUrl + '/Driver/GetFumigationComments/',
        type: "Get",
        dataType: "json",
        async: false,
        data: { "fumigationId": fumigationId },
        success: function (data) {

            var commentList = "";
            $("#tblFumigationComments").empty();
            if (data != null) {
                for (var i = 0; i < data.length; i++) {

                    commentList += '<tr><td>' + data[i].Comment + '</td></tr>'

                }
            }
            $("#tblFumigationComments").append(commentList);

        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}
//#endregion
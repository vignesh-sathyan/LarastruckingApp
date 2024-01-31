var glbRouteStops = new Array();
//global function for accessorial charges
var glbAccessorialFee = new Array();
var glbEquipmentNdriver = new Array();
//global function for proof temp

//global Damage Document

var glbDamageFile = new Array();
var glbProofOfTemprature = new Array();
var contactInfoCounts = 0;
var isNeedToloaded = true;

var isApproveDamageDocument = false;
var isApproveProofofTemp = false;
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


$(function () {
    $(".divShipmentRefNo").hide();
    //$(".divProofOfTemp").hide();
    $("#fumigationEnvelop").css("color", "gray");
    $("#fumigationEnvelop").css("font-size", "25px");

    //bind customer dropdown
    bindCustomerDropdown();

    //bind shipment status dropdown
    shipmentStatus();

    // bind substatus on changes of status
    ddlStatus();

    //bind sub status dropdown
    ddlSubStatus();

    //function for bind pricing method dropdown list
    GetPricingMethod();

    //function for open address popup
    openAddressModal();

    //bind fumigation type
    bindFumigationType();

    HideTextBoxes();

    fumigationType();

    //bind location
    bindLocation();


    //function for bind equipment popup
    openEquipmentModal();

    //add route
    btnAddRoute();

    //save shipment
    btnSave();

    ////bind accessorial fee UI
    bindAccessorialfeeType();
    ////accessorial check box
    chkAccessorialchargs();

    ddlPricingMethod();

    btnApprovedFumigation();

    //#region clear route stops
    btnClearRoute();

    GetFumigationById();

    if ($("#hdnfumigationId").val() > 0) {

        $("#divDamageNproofOfTemp").show();
        $(".pricingMethod").show();

    }
    else {

        $("#divDamageNproofOfTemp").hide();
        $(".pricingMethod").hide();

    }
    convertTemp();

    btnProofOfTemp();
    btnUpload();
    printPdf();

    printImage();
    //prove Proof Of Temprature
    btnApprovedProofOfTemp();
    //fileDownload();
    //Approve damage document
    btnDamageDocument();

    convertActualTemp();
	TimeChangeUnloading();
    TimeChangePickUp();
    TimeChangeLoadingPickUp();
	TimeChange();
	
	//TimeChange();

    //Arrived Pickup Location
    $("#arrivedFumPUDialog").dialog({
        autoOpen: false,
        modal: true,
        buttons: [
            {
                html: "<i class='fa fa-arrow-circle-left' aria-hidden='true'></i>&nbsp;GO BACK",
                class: "cancelButton",
                click: function () {
                    $(this).dialog("close");
                }
            },
            {
                text: "OK",
                class: "okButton",
                click: function () {
                    var selectedDateTime = $("#datetimepickerFumPU").val();
                    console.log("Selected Date-Time:", selectedDateTime);
                    $("#dtPickUpArrival").val(selectedDateTime);
                    $(this).dialog("close");
                }
            }

        ]
    });
    //Loading Pickup Location
    $("#loadingFumPUDialog").dialog({
        autoOpen: false,
        modal: true,
        buttons: [
            {
                html: "<i class='fa fa-arrow-circle-left' aria-hidden='true'></i>&nbsp;GO BACK",
                class: "cancelButton",
                click: function () {
                    $(this).dialog("close");
                }
            },
            {
                text: "OK",
                class: "okButton",
                click: function () {
                    var selectedDateTime = $("#datetimepickerFumLoadingPU").val();
                    console.log("Selected Date-Time:", selectedDateTime);
                    $("#dtActLoadingStart").val(selectedDateTime);
                    $(this).dialog("close");
                }
            }

        ]
    });

    //Arrived Delivery Location
    $("#arrivedFumDeliveryDialog").dialog({
        autoOpen: false,
        modal: true,
        buttons: [
            {
                html: "<i class='fa fa-arrow-circle-left' aria-hidden='true'></i>&nbsp;GO BACK",
                class: "cancelButton",
                click: function () {
                    $(this).dialog("close");
                }
            },
            {
                text: "OK",
                class: "okButton",
                click: function () {
                    var selectedDateTime = $("#datetimepickerFumDL").val();
                    console.log("Selected Date-Time:", selectedDateTime);
                    $("#dtDeliveryArrival").val(selectedDateTime);
                    $(this).dialog("close");
                }
            }

        ]

    });

    //Unloading Delivery Location
    $("#UnloadingFumDialog").dialog({
        autoOpen: false,
        modal: true,
        buttons: [
            {
                html: "<i class='fa fa-arrow-circle-left' aria-hidden='true'></i>&nbsp;GO BACK",
                class: "cancelButton",
                click: function () {
                    $(this).dialog("close");
                }
            },
            {
                text: "OK",
                class: "okButton",
                click: function () {
                    var selectedDateTime = $("#datetimepickerFumUL").val();
                    console.log("Selected Date-Time:", selectedDateTime);
                    $("#dtActDeliveryDeparture").val(selectedDateTime);
                    $(this).dialog("close");
                }
            }

        ]

    });

	
	$('#datetimepickerFumPU').datetimepicker({
        format: 'm/d/Y H:i',
        inline: true,
        step: 30,
        value: new Date().toISOString().slice(0, 19),
        onChangeDateTime: function(dp, $input) {
            $('#displayDateTimeFumPU').val($input.val()); // Set the selected date/time into the input field
        },

    });
    $('#datetimepickerFumLoadingPU').datetimepicker({
        format: 'm/d/Y H:i',
        inline: true,
        step: 30,
        value: new Date().toISOString().slice(0, 19),
        onChangeDateTime: function (dp, $input) {
            $('#displayDateTimeFumLoadingPU').val($input.val()); // Set the selected date/time into the input field
        },

    });

    $('#datetimepickerFumDL').datetimepicker({
        format: 'm/d/Y H:i',
        inline: true,
        step: 30,
        value: new Date().toISOString().slice(0, 19),
        onChangeDateTime: function(dp, $input) {
            $('#displayDateTimeFumDL').val($input.val()); // Set the selected date/time into the input field
        }
    });
    $('#datetimepickerFumUL').datetimepicker({
        format: 'm/d/Y H:i',
        inline: true,
        step: 30,
        value: new Date().toISOString().slice(0, 19),
        onChangeDateTime: function(dp, $input) {
            $('#displayDateTimeFumUL').val($input.val()); // Set the selected date/time into the input field
        }
    });

});

var TimeChange = function() {
    var x = setInterval(function() {
        var $timePicker = $('#arrivedFumDeliveryDialog').find('.xdsoft_time_box');
        //var $timePicker = $('#arrivedDeliveryDialog').find('.xdsoft_time_variant');
        ///console.log("$timePicker.length: ",$timePicker.length);
        var $currentTime = $('#arrivedFumDeliveryDialog').find('.xdsoft_time_variant').find('.xdsoft_current');
        if ($currentTime.length > 0) {
            //console.log("Fumigation $currentTime.length: ", $currentTime.length);
            if ($currentTime.length > 0) {
                var currentOffset = $currentTime.offset().top - $timePicker.offset().top;
				if(currentOffset>0){
				 clearInterval(x);
                //console.log("currentOffset: ", currentOffset);
                //console.log("currentOffset: ", $currentTime.offset().top);
                //console.log("$timePicker.offset().top: ", $timePicker.offset().top);
                $timePicker.scrollTop(currentOffset); // Scroll to the position of the current time elementss
				}
            }
           
        } else {
            console.log("time not found");
        } // Find the element representing the current time

    }, 100);
}
var TimeChangeUnloading = function() {
    var x = setInterval(function() {
        var $timePicker = $('#UnloadingFumDialog').find('.xdsoft_time_box');
        //var $timePicker = $('#arrivedDeliveryDialog').find('.xdsoft_time_variant');
        ///console.log("$timePicker.length: ",$timePicker.length);
        var $currentTime = $('#UnloadingFumDialog').find('.xdsoft_time_variant').find('.xdsoft_current');
        if ($currentTime.length > 0) {
           // console.log("Fumigation $currentTime.length: ", $currentTime.length);
            if ($currentTime.length > 0) {
                var currentOffset = $currentTime.offset().top - $timePicker.offset().top;
				if(currentOffset>0){
				 clearInterval(x);
                //console.log("currentOffset: ", currentOffset);
               // console.log("currentOffset: ", $currentTime.offset().top);
                //console.log("$timePicker.offset().top: ", $timePicker.offset().top);
                $timePicker.scrollTop(currentOffset); // Scroll to the position of the current time elementss
				}
            }
           
        } else {
            console.log("time not found");
        } // Find the element representing the current time

    }, 100);
}
var TimeChangePickUp = function() {
    var x = setInterval(function() {
        var $timePicker = $('#arrivedFumPUDialog').find('.xdsoft_time_box');
        //var $timePicker = $('#arrivedDeliveryDialog').find('.xdsoft_time_variant');
        ///console.log("$timePicker.length: ",$timePicker.length);
        var $currentTime = $('#arrivedFumPUDialog').find('.xdsoft_time_variant').find('.xdsoft_current');
        if ($currentTime.length > 0) {
            //console.log("Fumigation $currentTime.length: ", $currentTime.length);
            if ($currentTime.length > 0) {
                var currentOffset = $currentTime.offset().top - $timePicker.offset().top;
				if(currentOffset>0){
				 clearInterval(x);
                //console.log("currentOffset: ", currentOffset);
                //console.log("currentOffset: ", $currentTime.offset().top);
                //console.log("$timePicker.offset().top: ", $timePicker.offset().top);
                $timePicker.scrollTop(currentOffset); // Scroll to the position of the current time elementss
				}
            }
           
        } else {
            console.log("time not found");
        } // Find the element representing the current time

    }, 100);
}

var TimeChangeLoadingPickUp = function () {
    var x = setInterval(function () {
        var $timePicker = $('#loadingFumPUDialog').find('.xdsoft_time_box');
        //var $timePicker = $('#arrivedDeliveryDialog').find('.xdsoft_time_variant');
        ///console.log("$timePicker.length: ",$timePicker.length);
        var $currentTime = $('#loadingFumPUDialog').find('.xdsoft_time_variant').find('.xdsoft_current');
        if ($currentTime.length > 0) {
            //console.log("Fumigation $currentTime.length: ", $currentTime.length);
            if ($currentTime.length > 0) {
                var currentOffset = $currentTime.offset().top - $timePicker.offset().top;
                if (currentOffset > 0) {
                    clearInterval(x);
                    //console.log("currentOffset: ", currentOffset);
                    //console.log("currentOffset: ", $currentTime.offset().top);
                    //console.log("$timePicker.offset().top: ", $timePicker.offset().top);
                    $timePicker.scrollTop(currentOffset); // Scroll to the position of the current time elementss
                }
            }

        } else {
            console.log("time not found");
        } // Find the element representing the current time

    }, 100);
}


var GetValues = function () {
    var values = [];
    var url = window.location.pathname;
    var fumigationId = url.substring(url.lastIndexOf('/') + 1);
    var AWB = "";
    $.ajax({
        url: baseUrl + "/Fumigation/Fumigation/GetFumigationById",
        type: "GET",
        data: { fumigationId: fumigationId },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            for (let i = 0; i < response.GetFumigationRouteDetail.length; i++) {
                if (response.GetFumigationRouteDetail[i].AirWayBill != "") {
                    AWB = response.GetFumigationRouteDetail[i].AirWayBill;
                }
                else if (response.GetFumigationRouteDetail[i].ContainerNo != "") {
                    AWB = response.GetFumigationRouteDetail[i].ContainerNo;
                }
                else {
                    AWB = response.GetFumigationRouteDetail[i].CustomerPO;
                }
                //console.log("resp eqp: ", response);
                //console.log("fu eqp: ", response.FumigationEquipmentNDriver.length);
                var FumigationEquipmentNdriver;
                var FumigationEquipment;
                var deliveryDriver;
                var pickupDriver;
                var DriverID;
                if (response.FumigationEquipmentNDriver.length > 0) {
                    for (let j = 0; j < response.FumigationEquipmentNDriver.length; j++) {
                        if (response.FumigationEquipmentNDriver[j].DriverName != "") {
                            FumigationEquipmentNdriver = response.FumigationEquipmentNDriver[j].DriverName;
                        }
                        if (response.FumigationEquipmentNDriver[j].EquipmentName != "") {
                            FumigationEquipment = response.FumigationEquipmentNDriver[j].EquipmentName;
                        }
                        if (response.FumigationEquipmentNDriver[j].IsPickUp == false && response.FumigationEquipmentNDriver[j].DriverName != "") {
                            deliveryDriver = response.FumigationEquipmentNDriver[j].DriverName;


                            DriverID = response.FumigationEquipmentNDriver[j].DriverId;

                        }

                        if (response.FumigationEquipmentNDriver[j].IsPickUp == true && response.FumigationEquipmentNDriver[j].DriverName != "") {
                            pickupDriver = response.FumigationEquipmentNDriver[j].DriverName;
                            // console.log("PickupDriver: ", pickupDriver);
                        }

                        //console.log("deliveryDriver: " + response.FumigationEquipmentNDriver[j].IsPickUp + " : " + response.FumigationEquipmentNDriver[j].DriverName + " : " + deliveryDriver + " : " + pickupDriver);
                    }


                    var DriverName = "";
                    if (FumigationEquipmentNdriver != "") {

                        DriverName = FumigationEquipmentNdriver;

                    }
                    else {
                        DriverName = "null";
                    }
                    var deliveryDriverName = "";
                    if (deliveryDriver != "undefined") {
                        deliveryDriverName = deliveryDriver;
                    }

                    values.push({
                        "PickUpLocation": response.GetFumigationRouteDetail[i].PickUpLocationText,
                        "DeliveryLocation": response.GetFumigationRouteDetail[i].DeliveryLocationText,
                        "TrailerPosition": response.GetFumigationRouteDetail[i].TrailerPosition,
                        "FumigationLocation": response.GetFumigationRouteDetail[i].FumigationSiteText,
                        "Boxes": response.GetFumigationRouteDetail[i].BoxCount,
                        "Pallets": response.GetFumigationRouteDetail[i].PalletCount,
                        "AirWayBill": AWB,
                        "EquipmentName": FumigationEquipment,
                        "PickupDriverName": DriverName,
                        "DeliveryDriverName": deliveryDriver,
                        "PickupDriverName": pickupDriver,
                        "DriverID": DriverID
                    });
                }
            }
        }
    });

    return values;
}


//#region get Fumigation  detail by id for edit
function GetFumigationById() {


    var url = window.location.pathname;
    var fumigationId = url.substring(url.lastIndexOf('/') + 1);
    if (fumigationId > 0) {
        $.ajax({
            url: baseUrl + "/Fumigation/Fumigation/GetFumigationById",
            type: "GET",
            data: { fumigationId: fumigationId },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
       
                glbRouteStops = new Array();
                //global function for accessorial charges
                glbAccessorialFee = new Array();

                //global function equipment and driver
                glbEquipmentNdriver = new Array();

                //global Damage Document
                glbDamageFile = new Array();

                //global function for proof temp
                glbProofOfTemprature = new Array();

                if (response != null) {
                    //    //#region bind shipment detail
                    $("#hdnfumigationId").val(response.FumigationId);
                    // console.log("hdnFumigationId: " + response.FumigationId);
                    contactInfoCounts = response.ContactInfoCount;
                    var ddlCustomer = "<option selected='selected' value=" + response.CustomerId + ">" + response.CustomerName + "</option>";
                    $("#ddlCustomer").empty();
                    $("#ddlCustomer").append(ddlCustomer);
                    $(".ddlCustomer").text(response.CustomerName);

                    $("#txtRequestedBy").val(response.RequestedBy);
                    $("#txtReason").val(response.Reason);
                    $("#ddlStatus").val(response.StatusId);
                    //$("#txtVendorNConsignee").val(response.VendorNconsignee);
                    bindSubStauts();
                    $("#ddlSubStatus").val(response.SubStatusId);
                    $("#txtShipRefNo").val(response.ShipmentRefNo);

                    if (response.IsOnHold) {
                        $("#fumigationEnvelop").css("color", "red");
                        $("#hdnIsOnhold").val(1);
                    }
                    else {
                        $("#fumigationEnvelop").css("color", "gray");
                        $("#hdnIsOnhold").val(0);
                    }


                    //#region for binding equipment and driver
                    for (let i = 0; i < response.FumigationEquipmentNDriver.length; i++) {
                        // console.log("response.FumigationEquipmentNDriver: ", response.FumigationEquipmentNDriver);
                        var equipmentNdriver = {};
                        equipmentNdriver.RouteNo = response.FumigationEquipmentNDriver[i].RouteNo;
                        equipmentNdriver.FumigationEquipmentNDriverId = response.FumigationEquipmentNDriver[i].FumigationEquipmentNDriverId;
                        equipmentNdriver.EquipmentId = response.FumigationEquipmentNDriver[i].EquipmentId == null ? null : response.FumigationEquipmentNDriver[i].EquipmentId;
                        // console.log("glbEquipmentNdriver: ", glbEquipmentNdriver);
                        equipmentNdriver.EquipmentName = response.FumigationEquipmentNDriver[i].EquipmentName == null ? '|' : equipmentNdriver.EquipmentName = response.FumigationEquipmentNDriver[i].EquipmentName;
                        equipmentNdriver.DriverId = response.FumigationEquipmentNDriver[i].DriverId;
                        equipmentNdriver.DriverName = response.FumigationEquipmentNDriver[i].DriverName;
                        //console.log("DriverName: ", response.FumigationEquipmentNDriver[i].DriverName);
                        equipmentNdriver.IsPickUp = response.FumigationEquipmentNDriver[i].IsPickUp;
                        equipmentNdriver.IsDeleted = response.FumigationEquipmentNDriver[i].IsDeleted;
                        equipmentNdriver.FumigationRoutsId = response.FumigationEquipmentNDriver[i].FumigationRoutsId;
                        glbEquipmentNdriver.push(equipmentNdriver);

                    }
                    console.log("glbEquipmentNdriver by ID: ", glbEquipmentNdriver);
                    for (var i = 0; i < response.GetFumigationRouteDetail.length; i++) {

                        var objRouteStop = {};
                        objRouteStop.FumigationRoutsId = response.GetFumigationRouteDetail[i].FumigationRoutsId;
                        objRouteStop.RouteNo = response.GetFumigationRouteDetail[i].RouteNo;
                        objRouteStop.FumigationTypeId = response.GetFumigationRouteDetail[i].FumigationTypeId;
                        objRouteStop.VendorNConsignee = response.GetFumigationRouteDetail[i].VendorNConsignee;
                        objRouteStop.AirWayBill = response.GetFumigationRouteDetail[i].AirWayBill;
                        objRouteStop.CustomerPO = response.GetFumigationRouteDetail[i].CustomerPO;
                        objRouteStop.ContainerNo = response.GetFumigationRouteDetail[i].ContainerNo;
                        objRouteStop.PickUpLocation = response.GetFumigationRouteDetail[i].PickUpLocation;
                        objRouteStop.PickUpLocationText = response.GetFumigationRouteDetail[i].PickUpLocationText;
                        objRouteStop.PickUpArrival = response.GetFumigationRouteDetail[i].PickUpArrival == null ? "" : ConvertDateEdit(response.GetFumigationRouteDetail[i].PickUpArrival, true);
                        console.log("objRouteStop.PickUpArrival: ", ConvertDateEdit(response.GetFumigationRouteDetail[i].PickUpArrival, true));
                        //console.log("response.GetFumigationRouteDetail[i].PickUpArrival: ", ConvertDate(response.GetFumigationRouteDetail[i].PickUpArrival,true));
                        objRouteStop.FumigationSite = response.GetFumigationRouteDetail[i].FumigationSite;
                        objRouteStop.FumigationSiteText = response.GetFumigationRouteDetail[i].FumigationSiteText;
                        objRouteStop.FumigationArrival = response.GetFumigationRouteDetail[i].FumigationArrival == null ? "" : ConvertDateEdit(response.GetFumigationRouteDetail[i].FumigationArrival, true);

                        objRouteStop.ReleaseDate = response.GetFumigationRouteDetail[i].ReleaseDate == null ? "" : ConvertDateEdit(response.GetFumigationRouteDetail[i].ReleaseDate, true);

                        objRouteStop.DepartureDate = response.GetFumigationRouteDetail[i].DepartureDate == null ? "" : ConvertDateEdit(response.GetFumigationRouteDetail[i].DepartureDate, true);
                        objRouteStop.Commodity = response.GetFumigationRouteDetail[i].Commodity;
                        objRouteStop.PricingMethod = response.GetFumigationRouteDetail[i].PricingMethod;
                        objRouteStop.PricingMethodText = response.GetFumigationRouteDetail[i].PricingMethodText;

                        objRouteStop.TrailerDays = response.GetFumigationRouteDetail[i].TrailerDays;

                        objRouteStop.DeliveryLocation = response.GetFumigationRouteDetail[i].DeliveryLocation;
                        objRouteStop.DeliveryLocationText = response.GetFumigationRouteDetail[i].DeliveryLocationText;
                        objRouteStop.DeliveryArrival = response.GetFumigationRouteDetail[i].DeliveryArrival == null ? "" : ConvertDateEdit(response.GetFumigationRouteDetail[i].DeliveryArrival, true);;
                        objRouteStop.PalletCount = response.GetFumigationRouteDetail[i].PalletCount;
                        objRouteStop.BoxCount = response.GetFumigationRouteDetail[i].BoxCount;
                        objRouteStop.BoxType = response.GetFumigationRouteDetail[i].BoxType;
                        objRouteStop.Temperature = response.GetFumigationRouteDetail[i].Temperature;
                        objRouteStop.TemperatureType = response.GetFumigationRouteDetail[i].TemperatureType;

                        objRouteStop.MinFee = response.GetFumigationRouteDetail[i].MinFee;
                        objRouteStop.AddFee = response.GetFumigationRouteDetail[i].AddFee;
                        objRouteStop.UpTo = response.GetFumigationRouteDetail[i].UpTo;
                        objRouteStop.TrailerPosition = response.GetFumigationRouteDetail[i].TrailerPosition;
                        objRouteStop.TotalFee = response.GetFumigationRouteDetail[i].TotalFee;
                        objRouteStop.ReceiverName = response.GetFumigationRouteDetail[i].ReceiverName;
                        objRouteStop.DigitalSignature = response.GetFumigationRouteDetail[i].DigitalSignature;

                        objRouteStop.DriverPickupArrival = response.GetFumigationRouteDetail[i].DriverPickupArrival == null ? "" : ConvertDateEdit(response.GetFumigationRouteDetail[i].DriverPickupArrival, true);
                        //console.log("DriverPickupArrival " + ConvertDateEdit(response.GetFumigationRouteDetail[i].DriverPickupArrival, true));
                        // console.log("DriverPickupArrival " +response.GetFumigationRouteDetail[i].DriverPickupArrival);
                        objRouteStop.DriverLoadingStartTime = response.GetFumigationRouteDetail[i].DriverLoadingStartTime == null ? "" : ConvertDateEdit(response.GetFumigationRouteDetail[i].DriverLoadingStartTime, true);
                        objRouteStop.DriverLoadingFinishTime = response.GetFumigationRouteDetail[i].DriverLoadingFinishTime == null ? "" : ConvertDateEdit(response.GetFumigationRouteDetail[i].DriverLoadingFinishTime, true);
                        objRouteStop.DriverPickupDeparture = response.GetFumigationRouteDetail[i].DriverPickupDeparture == null ? "" : ConvertDateEdit(response.GetFumigationRouteDetail[i].DriverPickupDeparture, true);
                        //console.log("DriverPickupDeparture " + ConvertDateEdit(response.GetFumigationRouteDetail[i].DriverPickupDeparture, true));
                        objRouteStop.DriverFumigationIn = response.GetFumigationRouteDetail[i].DriverFumigationIn == null ? "" : ConvertDateEdit(response.GetFumigationRouteDetail[i].DriverFumigationIn, true);
                        objRouteStop.DriverFumigationRelease = response.GetFumigationRouteDetail[i].DriverFumigationRelease == null ? "" : ConvertDateEdit(response.GetFumigationRouteDetail[i].DriverFumigationRelease, true);

                        objRouteStop.DriverDeliveryArrival = response.GetFumigationRouteDetail[i].DriverDeliveryArrival == null ? "" : ConvertDateEdit(response.GetFumigationRouteDetail[i].DriverDeliveryArrival, true);
                        //console.log("DriverDeliveryArrival " + ConvertDateEdit(response.GetFumigationRouteDetail[i].DriverDeliveryArrival, true));
                        objRouteStop.DriverDeliveryDeparture = response.GetFumigationRouteDetail[i].DriverDeliveryDeparture == null ? "" : ConvertDateEdit(response.GetFumigationRouteDetail[i].DriverDeliveryDeparture, true);
                        // console.log("DriverDeliveryDeparture " + ConvertDateEdit(response.GetFumigationRouteDetail[i].DriverDeliveryDeparture, true));
                        var PickUpEquipmentNdriver = glbEquipmentNdriver.filter(x => x.RouteNo == objRouteStop.RouteNo && x.FumigationRoutsId == objRouteStop.FumigationRoutsId && x.IsPickUp);
                        objRouteStop.PickUpEquipmentNdriver = PickUpEquipmentNdriver;
                        console.log("PickUpEquipmentNdriver ", PickUpEquipmentNdriver);
                        if (PickUpEquipmentNdriver.length > 0) {


                            var equpments = "";
                            var drivers = "";
                            $.each(PickUpEquipmentNdriver, function (index, message) {

                                equpments = equpments + message.EquipmentName + ","
                                if (message.DriverId > 0) {

                                    drivers = drivers + message.DriverName + ","
                                }
                            });

                            equpments = equpments.substring(0, equpments.indexOf(","));
                            drivers = drivers.substring(0, drivers.indexOf(","));
                            //console.log("equpments ", equpments);
                            objRouteStop.PickUpEquipment = equpments;
                            objRouteStop.PickUpDriver = drivers;
                            //objRouteStop.DeliveryEquipment = drivers;
                            //objRouteStop.DeliveryDriver = drivers;

                        }
                        else {
                            objRouteStop.PickUpEquipment = "";
                            objRouteStop.PickUpDriver = "";
                        }


                        var DeliveryEquipmentNdriver = glbEquipmentNdriver.filter(x => x.RouteNo == objRouteStop.RouteNo && x.FumigationRoutsId == objRouteStop.FumigationRoutsId && x.IsPickUp == false);
                        objRouteStop.DeliveryEquipmentNdriver = DeliveryEquipmentNdriver;
                        //  console.log("DeliveryEquipmentNdriver ", DeliveryEquipmentNdriver);
                        if (DeliveryEquipmentNdriver.length > 0) {


                            var equpments = "";
                            var drivers = "";
                            $.each(DeliveryEquipmentNdriver, function (index, message) {
                                equpments = equpments + message.EquipmentName + ","
                                if (message.DriverId > 0) {
                                    drivers = drivers + message.DriverName + ","
                                }
                            });

                            equpments = equpments.substring(0, equpments.indexOf(","));
                            drivers = drivers.substring(0, drivers.indexOf(","));

                            objRouteStop.DeliveryEquipment = equpments;
                            objRouteStop.DeliveryDriver = drivers;

                        }
                        else {
                            objRouteStop.DeliveryEquipment = "";
                            objRouteStop.DeliveryDriver = "";
                        }

                        glbRouteStops.push(objRouteStop);
                    }

                    BindRouteTable();

                    checkRadionButton();

                    if (response.GetFumigationRouteDetail.length == 1) {
                        edit_route_stops(0)
                    }
                    else {
                        edit_route_stops(0)
                    }

                    //#region for bing accessorialprice
                    for (var i = 0; i < response.GetFumigationAccessorialPrice.length; i++) {

                        var objaccessorialcharges = {};
                        objaccessorialcharges.FumigationAccessorialPriceId = response.GetFumigationAccessorialPrice[i].FumigationAccessorialPriceId;
                        objaccessorialcharges.RouteNo = response.GetFumigationAccessorialPrice[i].RouteNo;
                        objaccessorialcharges.AccessorialFeeTypeId = response.GetFumigationAccessorialPrice[i].AccessorialFeeTypeId;
                        objaccessorialcharges.FeeType = response.GetFumigationAccessorialPrice[i].FeeType;
                        objaccessorialcharges.Amount = response.GetFumigationAccessorialPrice[i].Amount;
                        objaccessorialcharges.AmtPerUnit = response.GetFumigationAccessorialPrice[i].AmtPerUnit;
                        objaccessorialcharges.Unit = response.GetFumigationAccessorialPrice[i].Unit;
                        objaccessorialcharges.IsDeleted = false;
                        objaccessorialcharges.Reason = response.GetFumigationAccessorialPrice[i].Reason;
                        glbAccessorialFee.push(objaccessorialcharges);
                        //Bind accessorial charges from quote
                    }

                    if (glbAccessorialFee.length > 0) {

                        getAccessorialPrice();
                    }

                    //#endregion

                    //#region for binding damage Image
                    for (let i = 0; i < response.DamageImages.length; i++) {
                        var damageFile = {};
                        damageFile.DamageId = response.DamageImages[i].DamageId;
                        damageFile.FumigationRouteId = response.DamageImages[i].FumigationRouteId;
                        damageFile.DamageId = response.DamageImages[i].DamageId;
                        damageFile.DamageImage = "";
                        damageFile.ImageUrl = response.DamageImages[i].ImageUrl;
                        damageFile.ImageDescription = response.DamageImages[i].ImageDescription;
                        damageFile.ImageName = response.DamageImages[i].ImageName;
                        damageFile.Date = ConvertDate(response.DamageImages[i].CreatedOn, true);
                        damageFile.RouteNo = response.DamageImages[i].RouteNo;
                        damageFile.IsApproved = response.DamageImages[i].IsApproved;
                        glbDamageFile.push(damageFile);
                    }
                    bindDamageFileTbl();
                    //#endregion

                    //#region for binding proof of temprature
                    for (let i = 0; i < response.ProofOfTemprature.length; i++) {
                        var proofOFtemp = {};
                        proofOFtemp.ProofOfTempratureId = response.ProofOfTemprature[i].ProofOfTempratureId;
                        proofOFtemp.FumigationRouteId = response.ProofOfTemprature[i].FumigationRouteId;
                        proofOFtemp.ImageUrl = response.ProofOfTemprature[i].ImageUrl;
                        proofOFtemp.ActualTemperature = response.ProofOfTemprature[i].ActualTemperature;
                        proofOFtemp.ImageName = response.ProofOfTemprature[i].ImageName;
                        proofOFtemp.IsApproved = response.ProofOfTemprature[i].IsApproved;
                        proofOFtemp.IsLoading = response.ProofOfTemprature[i].IsLoading;
                        proofOFtemp.Date = ConvertDate(response.ProofOfTemprature[i].CreatedOn, true);
                        glbProofOfTemprature.push(proofOFtemp);
                    }
                    bindProofOfTempTbl();
                    //#endregion
                    var commentList = "";
                    $("#tblFumigationComments").empty();
                    if (response.FumigationComment.length > 0) {
                        for (var i = 0; i < response.FumigationComment.length; i++) {
                            commentList += '<tr><td>' + response.FumigationComment[i].Comment + '</td></tr>'
                        }
                    }
                    $("#tblFumigationComments").append(commentList);
                }
            },
            error: function () {

            }
        });
    }



}
//#endregion


function CalculateTrailerDays() {
    var pickupArrival = $("#dtPickUpArrival").val();
    var deliverArrival = $("#dtDeliveryArrival").val();
    if (pickupArrival != "" && deliverArrival != "") {
        if (new Date(pickupArrival) < new Date(deliverArrival)) {
            var delivery = new Date(deliverArrival);
            var pickup = new Date(pickupArrival);
            var dayDiff = Math.ceil((delivery - pickup) / (1000 * 60 * 60 * 24));
            if (dayDiff > 0) {

                $("#txtTrailerDays").val(dayDiff);
            }
        }
        //else {
        //    $("#dtDeliveryDate").val("");
        //    toastr.warning("Please select valid date.");
        //}
    }
}
//#region shipment status
function shipmentStatus() {
    $.ajax({
        url: baseUrl + 'Fumigation/Fumigation/GetShipmentStatus',
        data: {},
        type: "GET",
        async: false,
        success: function (data) {
            console.log("data from new Driver order: ", data);
            var ddlValue = "";
            $("#ddlStatus").empty();
            for (var i = 0; i < data.length; i++) {
                ddlValue += '<option value="' + data[i].StatusId + '">' + data[i].StatusName + '</option>';
            }
            $("#ddlStatus").append(ddlValue);

        }
    });
}
//#endregion

//#region shipment sub status
ddlStatus = function () {
    $("#ddlStatus").change(function () {

        var statusid = $("#ddlStatus").val();

        var equipment = $("#txtPickUpEquipment").val();
        var driver = $("#txtPickUpdriver").val();

        if (statusid != 8 && statusid != 1 && glbEquipmentNdriver.length == 0) {
            $.alert({
                title: 'Alert!',
                content: '<b>Please select equipment and driver first.</b>',
                type: 'red',
                typeAnimated: true,
            });
            shipmentStatus();
        }
        else {
            $('#txtPickUpEquipment').removeAttr('readonly');
            $('#txtPickUpdriver').removeAttr('readonly');
        }
        if (statusid == 15 && glbEquipmentNdriver.length != 0) {
            $("#arrivedFumPUDialog").dialog("open");

            var currentDate = new Date();
            var formattedDateTime = currentDate.toLocaleString('en-US', {
                month: '2-digit',
                day: '2-digit',
                year: 'numeric',
                hour: '2-digit',
                minute: '2-digit',
                hour12: false // Ensure 24-hour format
            }).replace(',', '');
            $("#displayDateTimeFumPU").val(formattedDateTime);
        }
        if (statusid == 5 && glbEquipmentNdriver.length != 0) {
            $("#loadingFumPUDialog").dialog("open");

            var currentDate = new Date();
            var formattedDateTime = currentDate.toLocaleString('en-US', {
                month: '2-digit',
                day: '2-digit',
                year: 'numeric',
                hour: '2-digit',
                minute: '2-digit',
                hour12: false // Ensure 24-hour format
            }).replace(',', '');
            $("#displayDateTimeFumLoadingPU").val(formattedDateTime);
        }
        if (statusid == 16 && glbEquipmentNdriver.length != 0) {
            $("#arrivedFumDeliveryDialog").dialog("open");
            var currentDate = new Date();
            var formattedDateTime = currentDate.toLocaleString('en-US', {
                month: '2-digit',
                day: '2-digit',
                year: 'numeric',
                hour: '2-digit',
                minute: '2-digit',
                hour12: false // Ensure 24-hour format
            }).replace(',', '');

            $("#displayDateTimeFumDL").val(formattedDateTime);
        }
        if (statusid == 17 && glbEquipmentNdriver.length != 0) {
            $("#UnloadingFumDialog").dialog("open");
            var currentDate = new Date();
            var formattedDateTime = currentDate.toLocaleString('en-US', {
                month: '2-digit',
                day: '2-digit',
                year: 'numeric',
                hour: '2-digit',
                minute: '2-digit',
                hour12: false // Ensure 24-hour format
            }).replace(',', '');
            $("#displayDateTimeFumUL").val(formattedDateTime);
        }
        if (statusid == 7 && glbEquipmentNdriver.length != 0) {
            var values = {};
            values = GetJsonValue();
            var prevalues = GetValues();
            $.confirm({
                title: 'Confirm!',
                content: 'Do you want to complete this fumication now?',
                type: 'blue',
                typeAnimated: true,
                buttons: {
                    confirm: {
                      text: 'Confirm',
                      btnClass: 'btn-green', 
                      action: function () {
                        //$.alert('Confirmed!');
                        $("#ddlStatus").val(11);
                        values.StatusId = 11;
                        $.ajax({
                            url: baseUrl + "/Fumigation/Fumigation/EditFumigation",
                            type: "POST",
                            beforeSend: function () {
                                showLoader();
                                //console.log("pickup Drive btnsave: ", "FumigationEquipmentNdriver" in values);
                                var deliveryDriver = "";
                                var pickupDriver = "";
                                const ddlstatus = $("#ddlStatus option:selected").text();
                                if (values.FumigationEquipmentNdriver.length > 0 && prevalues.length > 0) {
                                    for (let i = 0; i < values.FumigationEquipmentNdriver.length; i++) {
                                        if (values.FumigationEquipmentNdriver[i].IsPickUp == false) {
                                            if (values.FumigationEquipmentNdriver[i].DriverName != "" && values.FumigationEquipmentNdriver[i].DriverName != "undefined" && values.FumigationEquipmentNdriver[i].DriverName != undefined && values.FumigationEquipmentNdriver[i].DriverName != "Select Driver") {
                                                deliveryDriver = values.FumigationEquipmentNdriver[i].DriverName;
                                                //pickupDriver = values.FumigationEquipmentNdriver[i].DriverName;
                                            }
                                        }
                                    }

                                    //console.log("deliver: ", deliveryDriver);
                                    if (deliveryDriver != "") {
                                        if (ddlstatus == "DRIVER ASSIGNED") {
                                            console.log("sending delivery");
                                            SendDeliveryMessage();
                                        }
                                    }
                                    else {

                                        SendEditMessage();
                                    }
                                }


                            },
                            data: JSON.stringify(values),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",

                            success: function (response) {
                                isNeedToloaded = false;
                                hideLoader();
                                if (response.IsSuccess) {
                                    //toastr.success(response.Message);
                                    //setTimeout(function () {
                                    //    window.location.href = baseUrl + "/Fumigation/Fumigation/ViewFumigationList";
                                    //}, 1000)

                                    $.alert({
                                        title: 'Success!',
                                        content: "<b>" + response.Message + "</b>",
                                        type: 'green',
                                        typeAnimated: true,
                                        buttons: {
                                            Ok: {
                                                btnClass: 'btn-green',
                                                action: function () {
                                                    window.location.href = baseUrl + "/Fumigation/Fumigation/ViewFumigationList";
                                                }
                                            },
                                        }
                                    });

                                }
                                else {
                                    hideLoader();
                                    //toastr.error(response.Message);
                                    //AlertPopup(response.Message);
                                    AlertPopup("Email Failed...");
                                }
                            },
                            error: function () {
                                hideLoader();
                                // toastr.error("Something went wrong.");
                                AlertPopup("Something went wrong.");

                            }
                        });
                      }
                    },
                    cancel: {
                        text: 'Cancel',
                        btnClass: 'btn-red',
                        cancel: function () {
                            /// $.alert('Canceled!');

                        },
                    }
                }
            });
        }

        if (statusid == 11) {
            if (isApproveDamageDocument) {
                shipmentStatus();
                $.alert({
                    title: 'Alert!',
                    content: '<b>Please approve shipment file.</b>',
                    type: 'red',
                    typeAnimated: true,
                });
            }
            if (isApproveProofofTemp) {
                shipmentStatus();
                $.alert({
                    title: 'Alert!',
                    content: '<b>Please approve proof of temperature document.</b>',
                    type: 'red',
                    typeAnimated: true,
                });
            }
        }
        bindSubStauts();
    });
}
//#endregion

//#region bindSubStatus
function bindSubStauts() {
    var statusid = $("#ddlStatus").val();
    $.ajax({
        url: baseUrl + 'Fumigation/Fumigation/GetShipmentSubStatus',
        data: { statusid: statusid },
        type: "GET",
        async: false,
        // cache: false,
        success: function (data) {

            var ddlValue = "";
            $("#ddlSubStatus").empty();
            ddlValue += '<option value="">SELECT SUB-STATUS</option>'
            for (var i = 0; i < data.length; i++) {
                ddlValue += '<option value="' + data[i].SubStatusId + '">' + data[i].SubStatusName + '</option>';
            }
            $("#ddlSubStatus").append(ddlValue);

        }
    });
}
//#endregion
//#region
ddlSubStatus = function () {
    $("#ddlSubStatus").change(function () {
        checkOtherStaus();
    });
}
//#endregion

//#region check other status condition
function checkOtherStaus() {
    var ddlSubStatus = $.trim($("#ddlSubStatus").find('option:selected').text());
    if ($.trim(ddlSubStatus).toUpperCase() == $.trim("OTHER").toUpperCase() || $.trim(ddlSubStatus).toUpperCase() == $.trim("Other").toUpperCase()) {
        $("#txtReason").prop('disabled', false);
    }
    else {
        $("#txtReason").val("");
        $("#txtReason").prop('disabled', true);
    }
}
//#endregion

//#region bindFumigationType
function bindFumigationType() {
    $.ajax({
        url: baseUrl + 'Fumigation/Fumigation/GetFumigationTypeList',
        type: "GET",
        async: false,
        // cache: false,
        success: function (data) {

            var ddlValue = "";
            $("#ddlFumigationType").empty();
            ddlValue += '<option value="">SELECT FUMIGATION TYPE</option>'
            for (var i = 0; i < data.length; i++) {

                ddlValue += '<option value="' + data[i].FumigationTypeId + '">' + data[i].FumigationName + '</option>';
            }
            $("#ddlFumigationType").append(ddlValue);

        }
    });
}
//#endregion


//#region bind pricing method dropdownlist
function GetPricingMethod() {
    $.ajax({
        url: baseUrl + 'Quote/Quote/GetPricingMehtod',
        data: {},
        type: "GET",
        async: false,
        // cache: false,
        success: function (data) {

            var ddlValue = "";
            $("#ddlPricingMethod").empty();

            ddlValue += '<option value="">SELECT PRICING METHOD</option>'
            for (var i = 0; i < data.length; i++) {
                ddlValue += '<option value="' + data[i].PricingMethodId + '">' + data[i].PricingMethodName + '</option>';
            }
            $("#ddlPricingMethod").append(ddlValue);
        }
    });
}
//#endregion

//#region function for apply selectize on customer dropdown
var bindCustomerDropdown = function () {
    $('#ddlCustomer').selectize({
        createOnBlur: false,
        maxItems: 1,
        valueField: 'id',
        labelField: 'text',
        searchField: 'text',
        closeAfterSelect: false,
        selectOnTab: true,
        plugins: ['restore_on_backspace'],
        load: function (query, callback) {
            if (!query.length) return callback();
            $.ajax({
                url: baseUrl + "Customer/GetAllCustomer/?searchText=" + query,
                type: 'GET',
                dataType: 'json',
                beforeSend: function (xhr, settings) {
                },
                error: function () {
                    callback();
                },
                success: function (response) {

                    var customers = [];
                    $.each(response, function (index, value) {
                        item = {}
                        item.id = value.CustomerID;
                        item.text = value.CustomerName;
                        item.email = value.Email;
                        item.phone = value.Phone;
                        item.ContactInfoCount = value.ContactInfoCount;
                        customers.push(item);
                    });

                    callback(customers);
                },
                complete: function () {
                }
            });
        },
        render: {
            item: function (item, escape) {
                return '<div>' +
                    ('<span class="name ddlCustomer" data-ContactInfoCount=' + item.ContactInfoCount + '>' + escape(item.text) + '</span>') +
                    '</div>';
            },
            option: function (item, escape) {
                var label = item.text;
                return '<div style="padding: 2px 5px">' +
                    '<span style="display: block;">' + escape(label) + '</span>' +
                    '</div>';
            }
        },
        create: function (input, callback) {

            $('#ddlCustomer').html("");
            $('#ddlCustomer').append($("<option selected='selected'></option>").val(input).html(input))
        },
        onFocus: function () {

            var value = this.getValue();
            this.clear(true);
            this.unlock();
        },
        onChange: function () {

            var contactInfoCount = $(".ddlCustomer").attr("data-ContactInfoCount");
            contactInfoCounts = contactInfoCount;
            if (contactInfoCount == 0) {
                $.alert({
                    title: 'Alert!',
                    content: '<b>' + msgMissingContactInfo + '</b>',
                    type: 'red',
                    typeAnimated: true,
                });
            }


        }
    });
}
//#endregion

//#region for open address popup
var openAddressModal = function () {
    $(".btnOpenAddressModal").on("click", function () {
        $('#formAddress').trigger("reset");

        $("#modalAddAddress").modal("show");
    });
}
//#endregion

function fumigationType() {
    $("#ddlFumigationType").change(function () {
        $("#txtAirWayBill").val("");
        $("#txtContainerNo").val("");
        $("#txtCustomerPO").val("");
        ShowHideTextBox();

    });
}


//#regin show hide on change of fumigation type

function ShowHideTextBox() {

    var fumigationText = $.trim($("#ddlFumigationType").find('option:selected').text());
    if ($.trim(fumigationText).toLowerCase() == $.trim("AIR SHIPMENT").toLowerCase() || $.trim(fumigationText).toLowerCase() == $.trim("On Floor").toLowerCase() || $.trim(fumigationText).toLowerCase() == $.trim("ACL In-House").toLowerCase()) {
        $(".divAirWayBill").show();
        $(".divCustomerPO").hide();
        $(".divContainerNo").hide();
    }
    else if ($.trim(fumigationText).toLowerCase() == $.trim("Restacking").toLowerCase()) {
        $(".divAirWayBill").hide();
        $(".divCustomerPO").hide();
        $(".divContainerNo").show();
    }
	 else if ($.trim(fumigationText).toLowerCase() == $.trim("SEGREGATION").toLowerCase()) {
     $(".divAirWayBill").hide();
     $(".divCustomerPO").hide();
     $(".divContainerNo").show();
 }
    else {
        $(".divAirWayBill").hide();
        $(".divCustomerPO").show();
        $(".divContainerNo").hide();
    }
    if ($.trim(fumigationText).toLowerCase() == $.trim("Select Fumigation Type").toLowerCase()) {
        $(".divAirWayBill").hide();
        $(".divCustomerPO").hide();
        $(".divContainerNo").hide();
    }
}

function HideTextBoxes() {
    $(".divAirWayBill").hide();
    $(".divCustomerPO").hide();
    $(".divContainerNo").hide();
}
//#endregion

//#region temprature converter

var convertTemp = function () {
    var actualTemp;
    $("#ddlTemperatureUnit").on("change", function () {

        var unit = $(this).val();
        var temp = $("#txtReqTemperature").val();
        if (temp != '') {
            if (unit == 'C') {
                actualTemp = FahrenheitToCelsius(temp);
            }
            else {
                actualTemp = CelsiusToFahrenheit(temp);
            }

            $("#txtReqTemperature").val(actualTemp)
        }
    })
}
//#endregion

//#region calculate shipment total price 
function CalculateTotalPrice() {

    var minfee = $("#txtMinFee").val();
    var upto = $("#txtUpTo").val();
    var addfee = $("#txtAddFee").val();
    var QutVlmWgt = $("#txtPalletsCount").val();
    var selectedtext = $.trim($("#ddlPricingMethod").find('option:selected').text());


    var noOfBox = $("#txtBoxCount").val();
    $("#txtTotalFee").val("");
    if ($.trim(selectedtext).toLowerCase() == $.trim("Per Pound").toLowerCase() || $.trim(selectedtext).toLowerCase() == $.trim("Per Kg").toLowerCase()) {
        $("#txtTotalFee").val(ConvertStringToFloat(minfee));
    }
    else if ($.trim(selectedtext).toLowerCase() == $.trim("Per Box").toLowerCase()) {
        if (noOfBox > 0) {
            var totalprice = 0;
            if (Number(noOfBox) <= Number(upto)) {
                totalprice = minfee;
            }
            else {
                totalprice = Number(minfee) + ((Number(noOfBox) - Number(upto)) * (Number(addfee)));

            }
            $("#txtTotalFee").val(ConvertStringToFloat(totalprice));
        }
    }
    else if ($.trim(selectedtext).toLowerCase() == $.trim("Per Pallet").toLowerCase()) {
        if (QutVlmWgt > 0) {
            var totalprice = 0;
            if (Number(QutVlmWgt) <= Number(upto)) {
                totalprice = minfee;
            }
            else {
                totalprice = Number(minfee) + ((Number(QutVlmWgt) - Number(upto)) * (Number(addfee)));

            }
            $("#txtTotalFee").val(ConvertStringToFloat(totalprice));
        }
    }

    else if ($.trim(selectedtext).toLowerCase() == $.trim("Per Trip").toLowerCase()) {
        if (minfee != "") {
            $("#txtTotalFee").val(ConvertStringToFloat(minfee));
        }

    }
    else if ($.trim(selectedtext).toLowerCase() == $.trim("FIXED FEE").toLowerCase() || $.trim(selectedtext).toLowerCase() == $.trim("PER ROUND TRIP").toLowerCase() || $.trim(selectedtext).toLowerCase() == $.trim("PER DAY").toLowerCase()) {
        if (minfee != "") {
            $("#txtTotalFee").val(ConvertStringToFloat(minfee));
        }
    }
    getUptoCount();
}
//#endregion

function getUptoCount() {

    var minFee = $("#txtMinFee").val();
    var addPrice = $("#txtAddFee").val();
    var uptoPrice = 0;
    if (minFee != "" && addPrice != "" && Number(addPrice) > 0) {
        uptoPrice = (Number(minFee) / Number(addPrice));
    }
    $("#txtUpTo").val(ConvertStringToFloat(uptoPrice));
}

//#region add route 
btnAddRoute = function () {
    $("#btnAddRoute").on("click", function () {
        if ($("#ddlCustomer").val() > 0) {
            if (contactInfoCounts > 0) {
                if ($("#ddlPickUpLocation").val() > 0) {
                    if ($("#ddlFumigationSite").val() > 0) {
                        if ($("#ddlDeliveryLocation").val() > 0) {
                            console.log("isFormValid ", isFormValid("formRouteStop"));
                            if (isFormValid("formRouteStop")) {
                                if (validateDate()) {
                                    AddRouteStops();
                                    BindRouteTable();
                                    checkRadionButton();
                                }
                            }
                        }
                        else {
                            // toastr.warning("Please select Delivery Location")
                            AlertPopup("Please select Delivery Location");
                        }
                    }
                    else {
                        // toastr.warning("Please select Fumigation Site.")
                        AlertPopup("Please select Fumigation Site.")
                    }
                }
                else {
                    //toastr.warning("Please select a Pickup Location.")
                    AlertPopup("Please select a Pickup Location.")
                }
            }
            else {
                //$.alert({
                //    title: 'Alert!',
                //    content: '<b>' + msgMissingContactInfo + '</b>',
                //    type: 'red',
                //    typeAnimated: true,
                //});
                ConfirmationPopup("msgMissingContactInfo")

            }
        }
        else {
            //$.alert({
            //    title: 'Alert!',
            //    content: '<b>Please select a customer.</b>',
            //    type: 'red',
            //    typeAnimated: true,
            //});
            AlertPopup('Please select a customer.');
        }
    });
}
//#endregion 

function validateDate() {

    var isvalid = true;
    var dtPickUpArrival = $("#dtPickUpArrival").val();
    var dtFumigationArrival = $("#dtFumigationArrival").val();
    var dtDeliveryArrival = $("#dtDeliveryArrival").val();
    var dtReleaseDate = $("#dtRelease").val();

    var todayDate = new Date();
    var month = todayDate.getMonth() + 1;
    var day = todayDate.getDate() - 1;

    var yesterday = "";
    yesterday = (month < 10 ? '0' : '') + month + '-' +
        (day < 10 ? '0' : '') + day + '-' +
        todayDate.getFullYear();

    //Seperating only the date from full date with time

    var pickupdate = dtPickUpArrival.substring(0, dtPickUpArrival.indexOf(' '));
    var fumigationdate = dtFumigationArrival.substring(0, dtFumigationArrival.indexOf(' '));
    var deliverydate = dtDeliveryArrival.substring(0, dtDeliveryArrival.indexOf(' '));
    var releasedate = dtReleaseDate.substring(0, dtReleaseDate.indexOf(' '));


    //Replacing string for obtaining valid date format (mm-dd-yyy to mm/dd/yyyy)

    var pickupnew = pickupdate.replace("-", "/");
    var yesterdaynew = yesterday.replace("-", "/");
    var fumigationnew = fumigationdate.replace("-", "/");
    var deliverydatenew = deliverydate.replace("-", "/");
    var releasedatenew = releasedate.replace("-", "/");



    /* if (dtPickUpArrival != "" && dtFumigationArrival != "") {
         if (new Date(pickupnew) < new Date(yesterdaynew)) {
             // toastr.warning("The Req. Loading should be greater than, or equal to, yesterday's date.");
             AlertPopup("The Req. Loading should be greater than, or equal to, yesterday's date.");
             isvalid = false;
             return isvalid
             //console.log("FumigationsError : " + isvalid);
         }
 
         if (isvalid && new Date(pickupnew) <= new Date(fumigationnew)) {
             isvalid = true;
 
         }
         else {
             //$("#dtPickUpToDate").val("");
             //  toastr.warning("The Est. Fum. In should be greater than the Req. Loading.");
             AlertPopup("The Est. Fum. In should be greater than the Req. Loading.");
             return isvalid = false;
 
         }
         if (dtDeliveryArrival != "") {
             if (isvalid && new Date(deliverydatenew) >= new Date(fumigationnew)) {
                 isvalid = true;
             }
             else {
                 //  toastr.warning("The Delivery Est. Arrival should be greater than the Est. Fum. In.");
                 AlertPopup("The Delivery Est. Arrival should be greater than the Est. Fum. In.");
                 return isvalid = false;
             }
         }
         if (dtReleaseDate != "") {
             if (isvalid && new Date(releasedatenew) >= new Date(fumigationnew)) {
                 isvalid = true;
             }
             else {
                 // toastr.warning("The Est. Release should be greater than the Est. Fum. In.");
                 AlertPopup("The Est. Release should be greater than the Est. Fum. In.");
                 return isvalid = false;
             }
         }
 
     }*/


    return isvalid;
}


function GetMaxRouteNo(fumigationId) {
    var maxRouteNo = 0;
    $.ajax({
        url: baseUrl + 'Fumigation/Fumigation/GetMaxRouteNo',
        data: { "fumigationId": fumigationId },
        type: "Get",
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: function (data) {

            maxRouteNo = data;
        }
    });
    return maxRouteNo;
}



//#region add route detail in array
function AddRouteStops() {
    var FumEquipmentDrivers;
    var fumigationId = $("#hdnfumigationId").val();

    var routeNo = 0
    /* if (fumigationId > 0) {
         routeNo = GetMaxRouteNo(fumigationId) + 1;
         console.log("routeNo: " + routeNo);
     }*/
    // else {
    if (glbRouteStops.length > 0) {
        // console.log("glbRouteStops: ", glbRouteStops);
        var max = glbRouteStops.reduce(function (prev, current) {
            return (prev.RouteNo > current.RouteNo) ? prev : current
        });
        routeNo = max.RouteNo + 1;
    }
    else {
        routeNo = 1;
    }
    // }



    //Update route detail
    if ($("#tblShipmentDetail").attr("data-row-no") > 0) {

        var tblRowsCount = $("#tblShipmentDetail").attr("data-row-no");
        var rowindex = Number(tblRowsCount) - 1;
        //console.log("rowindex: ", rowindex);
        var routedetail;
        if (rowindex == 0) {

            for (let j = 0; j < glbRouteStops.length; j++) {

                routedetail = glbRouteStops[j];
                if (j == 0) {
                    console.log("glbRouteStops.length: ", glbRouteStops.length);
                    routedetail.FumigationTypeId = $.trim($("#ddlFumigationType").val());

                    routedetail.VendorNConsignee = $.trim($("#txtVendorNConsignee").val());

                    routedetail.AirWayBill = $.trim($("#txtAirWayBill").val());
                    routedetail.CustomerPO = $.trim($("#txtCustomerPO").val());
                    routedetail.ContainerNo = $.trim($("#txtContainerNo").val());
                    routedetail.PickUpLocation = $.trim($("#ddlPickUpLocation").val());
                    routedetail.PickUpLocationText = $.trim($("#ddlPickUpLocation").find('option:selected').text());
                    routedetail.PickUpArrival = $.trim($("#dtPickUpArrival").val());
                    //console.log("pickupArrival: " + $.trim($("#dtPickUpArrival").val()));


                    routedetail.DriverLoadingStartTime = $.trim($("#dtActLoadingStart").val());
                    routedetail.DriverLoadingFinishTime = $.trim($("#dtActLoadingFinish").val());
                    routedetail.DriverFumigationIn = $.trim($("#dtActFumIn").val());
                    routedetail.DriverFumigationRelease = $.trim($("#dtActFumRelease").val());



                    routedetail.FumigationSite = $.trim($("#ddlFumigationSite").val());
                    routedetail.FumigationSiteText = $.trim($("#ddlFumigationSite").find('option:selected').text());
                    routedetail.FumigationArrival = $.trim($("#dtFumigationArrival").val());

                    routedetail.ReleaseDate = $.trim($("#dtRelease").val());
                    routedetail.DepartureDate = $.trim($("#dtDeparture").val());

                    routedetail.Commodity = $.trim($("#txtCommodity").val());
                    routedetail.PricingMethod = $.trim($("#ddlPricingMethod").val());
                    routedetail.TrailerDays = $.trim($("#txtTrailerDays").val());
                    routedetail.PricingMethodText = $.trim($("#ddlPricingMethod").find('option:selected').text());

                    routedetail.DeliveryLocation = $.trim($("#ddlDeliveryLocation").val());
                    routedetail.DeliveryLocationText = $.trim($("#ddlDeliveryLocation").find('option:selected').text());
                    routedetail.DeliveryArrival = $.trim($("#dtDeliveryArrival").val());
                    routedetail.PalletCount = $.trim($("#txtPalletsCount").val());
                    routedetail.BoxCount = $.trim($("#txtBoxCount").val());
                    routedetail.BoxType = $.trim($("#ddlBoxType").val());
                    routedetail.Temperature = $.trim($("#txtReqTemperature").val());
                    routedetail.TemperatureType = $.trim($("#ddlTemperatureUnit").val());
                    routedetail.MinFee = $.trim($("#txtMinFee").val());
                    routedetail.AddFee = $.trim($("#txtAddFee").val());
                    routedetail.UpTo = $.trim($("#txtUpTo").val());

                    routedetail.TrailerPosition = $.trim($("#txtTrailerPosition").val());
                    routedetail.TotalFee = $.trim($("#txtTotalFee").val());
                    routedetail.PickUpEquipment = $.trim($("#txtPickUpEquipment").val());
                    console.log("glbEquipmentNdriver if j==0: ", glbEquipmentNdriver);
                    routedetail.PickUpDriver = $.trim($("#txtPickUpdriver").val());
                    console.log("hdnequp: ", $("#hdnPickUpEquipment").val());
                    var PickupEQuipmentDriver = [];
                    var DeliveryEQuipmentDriver = [];
                    // PickupEQuipmentDriver = JSON.parse($.trim($("#hdnPickUpEquipment").val()));
                    // DeliveryEQuipmentDriver = JSON.parse($.trim($("#hdnDeliveryEquipment").val()));

                    if (routedetail.PickUpDriver != "" && $("#hdnPickUpEquipment").val() != "") {
                        routedetail.PickUpEquipmentNdriver = JSON.parse($.trim($("#hdnPickUpEquipment").val()));
                        console.log("hdnequp val after parse: ", routedetail.PickUpEquipmentNdriver);
                    }
                    else {
                        routedetail.PickUpEquipmentNdriver = $.trim($("#hdnPickUpEquipment").val());
                    }
                    // routedetail.PickUpEquipmentNdriver = PickupEQuipmentDriver;
                    // routedetail.DeliveryEquipmentNdriver = DeliveryEQuipmentDriver;
                    //  routedetail.DeliveryEquipment = $.trim($("#txtDeliveryEquipment").val());

                    // routedetail.DeliveryDriver = $.trim($("#txtDeliveryDriver").val());
                    //console.log("routedetail.DeliveryDriver :", routedetail.DeliveryDriver);

                    if (routedetail.DeliveryDriver != "" && $("#hdnDeliveryEquipment").val() != "") {
                        routedetail.DeliveryEquipmentNdriver = JSON.parse($.trim($("#hdnDeliveryEquipment").val()));
                    }
                    else {
                        routedetail.DeliveryEquipmentNdriver = $.trim($("#hdnDeliveryEquipment").val());
                    }
                    //routedetail.DeliveryEquipmentNdriver = JSON.parse($.trim($("#hdnDeliveryEquipment").val()));
                    routedetail.DeliveryDriver = $.trim($("#txtDeliveryDriver").val());
                    routedetail.DeliveryEquipment = $.trim($("#txtDeliveryEquipment").val());
                    routedetail.DriverPickupArrival = $.trim($("#dtActPickUpArrival").val());
                    routedetail.DriverPickupDeparture = $.trim($("#dtActPickupDeparture").val());
                    routedetail.DriverDeliveryArrival = $.trim($("#dtActDeliveryArrival").val());
                    routedetail.DriverDeliveryDeparture = $.trim($("#dtActDeliveryDeparture").val());



                    var glbEquipmentNdriverList = glbEquipmentNdriver.filter(x => x.RouteNo == 0);

                    if (glbEquipmentNdriverList.length > 0) {
                        for (var i = 0; i < glbEquipmentNdriverList.length; i++) {
                            glbEquipmentNdriverList[i].RouteNo = routedetail.RouteNo;

                        }
                    }

                    if (j == 1) {
                        $.alert({
                            title: 'Success!',
                            content: "<b>Your data has successfully been updated to your shipment.<br/>  Don't forget to click on the Submit button to save all changes.</b>",
                            type: 'green',
                            typeAnimated: true,
                        });

                    }


                    $("#tblShipmentDetail").attr("data-row-no", 0);
                    $("#txtVendorNConsignee").val("");
                    $("#btnAddRoute").text("ADD LOADING LOCATION & FREIGHT");
                }
                else {
                    var routeNumber = routedetail + 1;
                    //routedetail = glbRouteStops[rowindex];
                    routedetail.FumigationTypeId = $.trim($("#ddlFumigationType").val());

                    // routedetail.VendorNConsignee = $.trim($("#txtVendorNConsignee").val());

                    // routedetail.AirWayBill = $.trim($("#txtAirWayBill").val());
                    // routedetail.CustomerPO = $.trim($("#txtCustomerPO").val());
                    // routedetail.ContainerNo = $.trim($("#txtContainerNo").val());
                    // routedetail.PickUpLocation = $.trim($("#ddlPickUpLocation").val());
                    // routedetail.PickUpLocationText = $.trim($("#ddlPickUpLocation").find('option:selected').text());
                    routedetail.PickUpArrival = $.trim($("#dtPickUpArrival").val());
                    //console.log("pickupArrival: " + $.trim($("#dtPickUpArrival").val()));


                    routedetail.DriverLoadingStartTime = $.trim($("#dtActLoadingStart").val());
                    routedetail.DriverLoadingFinishTime = $.trim($("#dtActLoadingFinish").val());
                    routedetail.DriverFumigationIn = $.trim($("#dtActFumIn").val());
                    routedetail.DriverFumigationRelease = $.trim($("#dtActFumRelease").val());



                    // routedetail.FumigationSite = $.trim($("#ddlFumigationSite").val());
                    // routedetail.FumigationSiteText = $.trim($("#ddlFumigationSite").find('option:selected').text());
                    routedetail.FumigationArrival = $.trim($("#dtFumigationArrival").val());

                    routedetail.ReleaseDate = $.trim($("#dtRelease").val());
                    routedetail.DepartureDate = $.trim($("#dtDeparture").val());

                    // routedetail.Commodity = $.trim($("#txtCommodity").val());
                    // routedetail.PricingMethod = $.trim($("#ddlPricingMethod").val());
                    // routedetail.TrailerDays = $.trim($("#txtTrailerDays").val());
                    // routedetail.PricingMethodText = $.trim($("#ddlPricingMethod").find('option:selected').text());

                    //  routedetail.DeliveryLocation = $.trim($("#ddlDeliveryLocation").val());
                    // routedetail.DeliveryLocationText = $.trim($("#ddlDeliveryLocation").find('option:selected').text());
                    routedetail.DeliveryArrival = $.trim($("#dtDeliveryArrival").val());
                    //routedetail.PalletCount = $.trim($("#txtPalletsCount").val());
                    //  routedetail.BoxCount = $.trim($("#txtBoxCount").val());
                    // routedetail.BoxType = $.trim($("#ddlBoxType").val());
                    // routedetail.Temperature = $.trim($("#txtReqTemperature").val());
                    // routedetail.TemperatureType = $.trim($("#ddlTemperatureUnit").val());
                    // routedetail.MinFee = $.trim($("#txtMinFee").val());
                    //routedetail.AddFee = $.trim($("#txtAddFee").val());
                    // routedetail.UpTo = $.trim($("#txtUpTo").val());

                    // routedetail.TrailerPosition = $.trim($("#txtTrailerPosition").val());
                    // routedetail.TotalFee = $.trim($("#txtTotalFee").val());
                    routedetail.PickUpEquipment = $.trim($("#txtPickUpEquipment").val());
                    //console.log("edit equipment : ", $.trim($("#txtPickUpEquipment").val()));
                    routedetail.PickUpDriver = $.trim($("#txtPickUpdriver").val());
                    //console.log("glbEquipmentNdriver edit first: ", glbEquipmentNdriver);

                    //  var upd_obj = glbEquipmentNdriver.findIndex((obj => obj.RouteNo > 0));
                    //  console.log("upd_obj: ", upd_obj);
                    var PickupEQuipmentDriver = [];
                    var DeliveryEQuipmentDriver = [];
                    // PickupEQuipmentDriver = JSON.parse($.trim($("#hdnPickUpEquipment").val()));
                    // DeliveryEQuipmentDriver = JSON.parse($.trim($("#hdnDeliveryEquipment").val()));
                    console.log("route no check: ", glbRouteStops[rowindex]);
                    if (glbEquipmentNdriver.length > 0) {
                        var EquipmentD = glbEquipmentNdriver.find((element) => element.RouteNo == glbRouteStops[rowindex].RouteNo && element.IsPickUp == true);
                        var pickOb = {
                            FumigationEquipmentNDriverId: 0,
                            IsPickUp: EquipmentD.IsPickUp,
                            EquipmentId: EquipmentD.EquipmentId,
                            EquipmentName: EquipmentD.EquipmentName,
                            DriverId: EquipmentD.DriverId,
                            DriverName: EquipmentD.DriverName,
                            RouteNo: routeNo,
                            IsDeleted: false

                        };
                        var equipCheck = glbEquipmentNdriver.find((element) => element.RouteNo == glbRouteStops[rowindex].RouteNo && element.IsPickUp == false);
                        if (equipCheck == undefined) {
                            console.log("equipCheck: " + equipCheck);
                            var EquipmentDel = glbEquipmentNdriver.find((element) => element.RouteNo == glbRouteStops[rowindex].RouteNo && element.IsPickUp == true);

                            //console.log("glbEquipmentNdriver[1].IsPickUp: ", glbEquipmentNdriver[1].IsPickUp);
                            var delOb = {
                                FumigationEquipmentNDriverId: 0,
                                IsPickUp: EquipmentDel.IsPickUp,
                                EquipmentId: EquipmentDel.EquipmentId,
                                EquipmentName: EquipmentDel.EquipmentName,
                                DriverId: EquipmentDel.DriverId,
                                DriverName: EquipmentDel.DriverName,
                                RouteNo: routeNo,
                                IsDeleted: false

                            };


                            PickupEQuipmentDriver.push(pickOb);
                            DeliveryEQuipmentDriver.push(delOb);
                        }
                        else {
                            var EquipmentDel = glbEquipmentNdriver.find((element) => element.RouteNo == glbRouteStops[rowindex].RouteNo && element.IsPickUp == false);

                            //console.log("glbEquipmentNdriver[1].IsPickUp: ", glbEquipmentNdriver[1].IsPickUp);
                            var delOb = {
                                FumigationEquipmentNDriverId: 0,
                                IsPickUp: EquipmentDel.IsPickUp,
                                EquipmentId: EquipmentDel.EquipmentId,
                                EquipmentName: EquipmentDel.EquipmentName,
                                DriverId: EquipmentDel.DriverId,
                                DriverName: EquipmentDel.DriverName,
                                RouteNo: routeNo,
                                IsDeleted: false

                            };


                            PickupEQuipmentDriver.push(pickOb);
                            DeliveryEQuipmentDriver.push(delOb);
                        }
                        //console.log("PickupEQuipmentDriver: ",PickupEQuipmentDriver);
                    }
                    for (let h = 0; h < glbRouteStops.length; h++) {
                        console.log("glbRouteStops.length * 2: ", glbRouteStops.length * 2 + " : glbEquipmentNdriver.length : ", glbEquipmentNdriver.length);
                        if ((glbRouteStops.length * 2) == (glbEquipmentNdriver.length)) {
                            console.log("driver and equipment count is coorect");
                            for (let k = 0; k < glbEquipmentNdriver.length; k++) {
                                if (glbEquipmentNdriver[k].RouteNo > 1) {
                                    // glbEquipmentNdriver[k].push(pickOb);
                                    // glbEquipmentNdriver[k].IsPickUp = glbEquipmentNdriver[0].IsPickUp;
                                    // console.log("glbEquipmentNdriver greater than 1: ", glbEquipmentNdriver[k]);
                                    if (glbEquipmentNdriver[k].IsPickUp == false) {
                                        var EquipmentD = glbEquipmentNdriver.find((element) => element.RouteNo == glbRouteStops[rowindex].RouteNo && element.IsPickUp == false);
                                        // console.log("EquipmentD delivery: ",EquipmentD);
                                        console.log("glbEquipmentNdriver[k].IsPickUp: ", glbEquipmentNdriver[k].IsPickUp);
                                        glbEquipmentNdriver[k].EquipmentId = EquipmentD.EquipmentId;
                                        glbEquipmentNdriver[k].EquipmentName = EquipmentD.EquipmentName;
                                        glbEquipmentNdriver[k].DriverId = EquipmentD.DriverId;
                                        glbEquipmentNdriver[k].DriverName = EquipmentD.DriverName;
                                    }
                                    else {
                                        //console.log("glbEquipmentNdriver[k].IsPickUp: ", glbEquipmentNdriver[k].IsPickUp);
                                        // glbEquipmentNdriver[k].IsPickUp = glbEquipmentNdriver[1].IsPickUp;
                                        var EquipmentD = glbEquipmentNdriver.find((element) => element.RouteNo == glbRouteStops[rowindex].RouteNo && element.IsPickUp == true);
                                        console.log("EquipmentD pickup : ", EquipmentD);
                                        glbEquipmentNdriver[k].EquipmentId = EquipmentD.EquipmentId;
                                        glbEquipmentNdriver[k].EquipmentName = EquipmentD.EquipmentName;
                                        glbEquipmentNdriver[k].DriverId = EquipmentD.DriverId;
                                        glbEquipmentNdriver[k].DriverName = EquipmentD.DriverName;
                                    }

                                }
                                else {
                                    console.log("fumigation quipment add for not added");
                                    console.log("glbRouteStops.length ee: ", glbRouteStops.length);
                                    console.log("routeNo: ", routeNo);
                                    //if (glbEquipmentNdriver[k].RouteNo != 1) {
                                    //glbEquipmentNdriver.push(pickOb);
                                    //glbEquipmentNdriver.push(delOb);
                                    //}
                                }

                            }
                        }
                        else {
                            // console.log("driver and equipment count is not coorect");
                            // console.log("glbRouteStops.length: ",glbRouteStops.length);
                            // console.log("routeNo: ",routeNo);
                            for (let l = 1; l <= glbRouteStops.length; l++) {
                                if (l != 1) {
                                    var EquipmentD = glbEquipmentNdriver.find((element, index) => element.RouteNo == glbRouteStops[rowindex].RouteNo && element.IsPickUp == true);
                                    var EquipmentDel = glbEquipmentNdriver.find((element, index) => element.RouteNo == glbRouteStops[rowindex].RouteNo && element.IsPickUp == false);
                                    console.log("EquipmentD: ", EquipmentD);
                                    console.log("EquipmentDel: ", EquipmentDel);
                                    if (EquipmentD != undefined && EquipmentDel != undefined) {

                                        var pickOb = {
                                            FumigationEquipmentNDriverId: 0,
                                            IsPickUp: EquipmentD.IsPickUp,
                                            EquipmentId: EquipmentD.EquipmentId,
                                            EquipmentName: EquipmentD.EquipmentName,
                                            DriverId: EquipmentD.DriverId,
                                            DriverName: EquipmentD.DriverName,
                                            RouteNo: routeNo,
                                            IsDeleted: false

                                        };

                                        //console.log("glbEquipmentNdriver[1].IsPickUp: ", glbEquipmentNdriver[1].IsPickUp);
                                        var delOb = {
                                            FumigationEquipmentNDriverId: 0,
                                            IsPickUp: EquipmentDel.IsPickUp,
                                            EquipmentId: EquipmentDel.EquipmentId,
                                            EquipmentName: EquipmentDel.EquipmentName,
                                            DriverId: EquipmentDel.DriverId,
                                            DriverName: EquipmentDel.DriverName,
                                            RouteNo: routeNo,
                                            IsDeleted: false

                                        };
                                        pickOb.RouteNo = l;
                                        delOb.RouteNo = l;
                                        console.log("length of l: ", l);
                                        glbEquipmentNdriver.push(pickOb);
                                        glbEquipmentNdriver.push(delOb);
                                    }

                                }
                            }

                        }
                    }

                    //if (routedetail.PickUpDriver != "" && $("#hdnPickUpEquipment").val()!="") {
                    //    routedetail.PickUpEquipmentNdriver = JSON.parse($.trim($("#hdnPickUpEquipment").val()));
                    //    console.log("hdnequp val after parse: ", routedetail.PickUpEquipmentNdriver);
                    //}
                    //else {
                    //    routedetail.PickUpEquipmentNdriver = $.trim($("#hdnPickUpEquipment").val());
                    //}
                    console.log("PickupEQuipmentDriver: ", PickupEQuipmentDriver);
                    routedetail.PickUpEquipmentNdriver = PickupEQuipmentDriver;
                    routedetail.DeliveryEquipmentNdriver = DeliveryEQuipmentDriver;
                    routedetail.DeliveryEquipment = $.trim($("#txtDeliveryEquipment").val());

                    routedetail.DeliveryDriver = $.trim($("#txtDeliveryDriver").val());
                    //console.log("routedetail.DeliveryDriver :", routedetail.DeliveryDriver);

                    //if (routedetail.DeliveryDriver != "" && $("#hdnDeliveryEquipment").val()!="") {
                    //    routedetail.DeliveryEquipmentNdriver = JSON.parse($.trim($("#hdnDeliveryEquipment").val()));
                    //}
                    //else {
                    //    routedetail.DeliveryEquipmentNdriver = $.trim($("#hdnDeliveryEquipment").val());
                    //}
                    //routedetail.DeliveryEquipmentNdriver = JSON.parse($.trim($("#hdnDeliveryEquipment").val()));
                    routedetail.DriverPickupArrival = $.trim($("#dtActPickUpArrival").val());
                    routedetail.DriverPickupDeparture = $.trim($("#dtActPickupDeparture").val());
                    routedetail.DriverDeliveryArrival = $.trim($("#dtActDeliveryArrival").val());
                    routedetail.DriverDeliveryDeparture = $.trim($("#dtActDeliveryDeparture").val());


                    console.log("after equpment add: ", glbEquipmentNdriver);
                    var glbEquipmentNdriverList = glbEquipmentNdriver.filter(x => x.RouteNo == 0);

                    if (glbEquipmentNdriverList.length > 0) {
                        for (var i = 0; i < glbEquipmentNdriverList.length; i++) {
                            glbEquipmentNdriverList[i].RouteNo = routedetail.RouteNo;

                        }
                    }

                    if (j == 1) {
                        $.alert({
                            title: 'Success!',
                            content: "<b>Your data has successfully been updated to your shipment.<br/>  Don't forget to click on the Submit button to save all changes.</b>",
                            type: 'green',
                            typeAnimated: true,
                        });

                    }


                    $("#tblShipmentDetail").attr("data-row-no", 0);
                    $("#txtVendorNConsignee").val("");
                    $("#btnAddRoute").text("ADD LOADING LOCATION & FREIGHT");
                }

            }

        }
        else {

            routedetail = glbRouteStops[rowindex];
            console.log("glbEquipmentNdriver edit not first: ", glbEquipmentNdriver + "routeindex: " + glbRouteStops[rowindex]);
            routedetail.FumigationTypeId = $.trim($("#ddlFumigationType").val());

            routedetail.VendorNConsignee = $.trim($("#txtVendorNConsignee").val());

            routedetail.AirWayBill = $.trim($("#txtAirWayBill").val());
            routedetail.CustomerPO = $.trim($("#txtCustomerPO").val());
            routedetail.ContainerNo = $.trim($("#txtContainerNo").val());
            routedetail.PickUpLocation = $.trim($("#ddlPickUpLocation").val());
            routedetail.PickUpLocationText = $.trim($("#ddlPickUpLocation").find('option:selected').text());
            routedetail.PickUpArrival = $.trim($("#dtPickUpArrival").val());
            //console.log("pickupArrival: " + $.trim($("#dtPickUpArrival").val()));


            routedetail.DriverLoadingStartTime = $.trim($("#dtActLoadingStart").val());
            routedetail.DriverLoadingFinishTime = $.trim($("#dtActLoadingFinish").val());
            routedetail.DriverFumigationIn = $.trim($("#dtActFumIn").val());
            routedetail.DriverFumigationRelease = $.trim($("#dtActFumRelease").val());



            routedetail.FumigationSite = $.trim($("#ddlFumigationSite").val());
            routedetail.FumigationSiteText = $.trim($("#ddlFumigationSite").find('option:selected').text());
            routedetail.FumigationArrival = $.trim($("#dtFumigationArrival").val());

            routedetail.ReleaseDate = $.trim($("#dtRelease").val());
            routedetail.DepartureDate = $.trim($("#dtDeparture").val());

            routedetail.Commodity = $.trim($("#txtCommodity").val());
            routedetail.PricingMethod = $.trim($("#ddlPricingMethod").val());
            routedetail.TrailerDays = $.trim($("#txtTrailerDays").val());
            routedetail.PricingMethodText = $.trim($("#ddlPricingMethod").find('option:selected').text());

            routedetail.DeliveryLocation = $.trim($("#ddlDeliveryLocation").val());
            routedetail.DeliveryLocationText = $.trim($("#ddlDeliveryLocation").find('option:selected').text());
            routedetail.DeliveryArrival = $.trim($("#dtDeliveryArrival").val());
            routedetail.PalletCount = $.trim($("#txtPalletsCount").val());
            routedetail.BoxCount = $.trim($("#txtBoxCount").val());
            routedetail.BoxType = $.trim($("#ddlBoxType").val());
            routedetail.Temperature = $.trim($("#txtReqTemperature").val());
            routedetail.TemperatureType = $.trim($("#ddlTemperatureUnit").val());
            routedetail.MinFee = $.trim($("#txtMinFee").val());
            routedetail.AddFee = $.trim($("#txtAddFee").val());
            routedetail.UpTo = $.trim($("#txtUpTo").val());

            routedetail.TrailerPosition = $.trim($("#txtTrailerPosition").val());
            routedetail.TotalFee = $.trim($("#txtTotalFee").val());
            routedetail.PickUpEquipment = $.trim($("#txtPickUpEquipment").val());
            console.log("edit equipment : ", $.trim($("#txtPickUpEquipment").val()));
            routedetail.PickUpDriver = $.trim($("#txtPickUpdriver").val());
            console.log("hdnequp: ", $("#hdnPickUpEquipment").val());
            var PickupEQuipmentDriver = [];
            var DeliveryEQuipmentDriver = [];
            // PickupEQuipmentDriver = JSON.parse($.trim($("#hdnPickUpEquipment").val()));
            // DeliveryEQuipmentDriver = JSON.parse($.trim($("#hdnDeliveryEquipment").val()));

            if (glbEquipmentNdriver.length > 0) {
                var pickOb = {
                    FumigationEquipmentNDriverId: 0,
                    IsPickUp: glbEquipmentNdriver[0].IsPickUp,
                    EquipmentId: glbEquipmentNdriver[0].EquipmentId,
                    EquipmentName: glbEquipmentNdriver[0].EquipmentName,
                    DriverId: glbEquipmentNdriver[0].DriverId,
                    DriverName: glbEquipmentNdriver[0].DriverName,
                    RouteNo: routeNo,
                    IsDeleted: false

                };

                //console.log("glbEquipmentNdriver[1].IsPickUp: ", glbEquipmentNdriver[1].IsPickUp);
                var delOb = {
                    FumigationEquipmentNDriverId: 0,
                    IsPickUp: glbEquipmentNdriver[1].IsPickUp,
                    EquipmentId: glbEquipmentNdriver[1].EquipmentId,
                    EquipmentName: glbEquipmentNdriver[1].EquipmentName,
                    DriverId: glbEquipmentNdriver[1].DriverId,
                    DriverName: glbEquipmentNdriver[1].DriverName,
                    RouteNo: routeNo,
                    IsDeleted: false

                };
                PickupEQuipmentDriver.push(pickOb);
                DeliveryEQuipmentDriver.push(delOb);
            }
            //if (routedetail.PickUpDriver != "" && $("#hdnPickUpEquipment").val()!="") {
            //    routedetail.PickUpEquipmentNdriver = JSON.parse($.trim($("#hdnPickUpEquipment").val()));
            //    console.log("hdnequp val after parse: ", routedetail.PickUpEquipmentNdriver);
            //}
            //else {
            //    routedetail.PickUpEquipmentNdriver = $.trim($("#hdnPickUpEquipment").val());
            //}
            routedetail.PickUpEquipmentNdriver = PickupEQuipmentDriver;
            routedetail.DeliveryEquipmentNdriver = DeliveryEQuipmentDriver;
            routedetail.DeliveryEquipment = $.trim($("#txtDeliveryEquipment").val());

            routedetail.DeliveryDriver = $.trim($("#txtDeliveryDriver").val());
            //console.log("routedetail.DeliveryDriver :", routedetail.DeliveryDriver);

            //if (routedetail.DeliveryDriver != "" && $("#hdnDeliveryEquipment").val()!="") {
            //    routedetail.DeliveryEquipmentNdriver = JSON.parse($.trim($("#hdnDeliveryEquipment").val()));
            //}
            //else {
            //    routedetail.DeliveryEquipmentNdriver = $.trim($("#hdnDeliveryEquipment").val());
            //}
            //routedetail.DeliveryEquipmentNdriver = JSON.parse($.trim($("#hdnDeliveryEquipment").val()));
            routedetail.DriverPickupArrival = $.trim($("#dtActPickUpArrival").val());
            routedetail.DriverPickupDeparture = $.trim($("#dtActPickupDeparture").val());
            routedetail.DriverDeliveryArrival = $.trim($("#dtActDeliveryArrival").val());
            routedetail.DriverDeliveryDeparture = $.trim($("#dtActDeliveryDeparture").val());



            var glbEquipmentNdriverList = glbEquipmentNdriver.filter(x => x.RouteNo == 0);

            if (glbEquipmentNdriverList.length > 0) {
                for (var i = 0; i < glbEquipmentNdriverList.length; i++) {
                    glbEquipmentNdriverList[i].RouteNo = routedetail.RouteNo;

                }
            }



            $.alert({
                title: 'Success!',
                content: "<b>Your data has successfully been updated to your shipment.<br/>  Don't forget to click on the Submit button to save all changes.</b>",
                type: 'green',
                typeAnimated: true,
            });

            $("#tblShipmentDetail").attr("data-row-no", 0);
            $("#txtVendorNConsignee").val("");
            $("#btnAddRoute").text("ADD LOADING LOCATION & FREIGHT");
        }
    }
    else {//Add route detail
        var objRouteStop = {};
        objRouteStop.FumigationRoutsId = 0;
        objRouteStop.RouteNo = routeNo;
        //console.log("RouteNo: ", objRouteStop.RouteNo);
        //console.log("RouteNo: ", objRouteStop.FumigationRoutsId);
        objRouteStop.FumigationId = $("#hdnfumigationId").val();
        objRouteStop.FumigationTypeId = $.trim($("#ddlFumigationType").val());
        objRouteStop.VendorNConsignee = $.trim($("#txtVendorNConsignee").val());

        objRouteStop.AirWayBill = $.trim($("#txtAirWayBill").val());

        objRouteStop.CustomerPO = $.trim($("#txtCustomerPO").val());
        objRouteStop.ContainerNo = $.trim($("#txtContainerNo").val());
        objRouteStop.PickUpLocation = $.trim($("#ddlPickUpLocation").val());
        objRouteStop.PickUpLocationText = $.trim($("#ddlPickUpLocation").find('option:selected').text());
        objRouteStop.PickUpArrival = $.trim($("#dtPickUpArrival").val());
        // console.log("pickupArrival: " + $.trim($("#dtPickUpArrival").val()));
        objRouteStop.FumigationSite = $.trim($("#ddlFumigationSite").val());
        objRouteStop.FumigationSiteText = $.trim($("#ddlFumigationSite").find('option:selected').text());
        objRouteStop.FumigationArrival = $.trim($("#dtFumigationArrival").val());


        objRouteStop.DriverLoadingStartTime = $.trim($("#dtActLoadingStart").val());
        objRouteStop.DriverLoadingFinishTime = $.trim($("#dtActLoadingFinish").val());
        objRouteStop.DriverFumigationIn = $.trim($("#dtActFumIn").val());
        objRouteStop.DriverFumigationRelease = $.trim($("#dtActFumRelease").val());

        objRouteStop.ReleaseDate = $.trim($("#dtRelease").val());
        objRouteStop.DepartureDate = $.trim($("#dtDeparture").val());
        objRouteStop.Commodity = $.trim($("#txtCommodity").val());
        objRouteStop.PricingMethod = $.trim($("#ddlPricingMethod").val());
        objRouteStop.PricingMethodText = $.trim($("#ddlPricingMethod").find('option:selected').text());

        objRouteStop.TrailerDays = $.trim($("#txtTrailerDays").val());

        objRouteStop.DeliveryLocation = $.trim($("#ddlDeliveryLocation").val());
        objRouteStop.DeliveryLocationText = $.trim($("#ddlDeliveryLocation").find('option:selected').text());
        objRouteStop.DeliveryArrival = $.trim($("#dtDeliveryArrival").val());
        objRouteStop.PalletCount = $.trim($("#txtPalletsCount").val());
        objRouteStop.BoxCount = $.trim($("#txtBoxCount").val());
        objRouteStop.BoxType = $.trim($("#ddlBoxType").val());
        objRouteStop.Temperature = $.trim($("#txtReqTemperature").val());
        objRouteStop.TemperatureType = $.trim($("#ddlTemperatureUnit").val());

        objRouteStop.MinFee = $.trim($("#txtMinFee").val());
        objRouteStop.AddFee = $.trim($("#txtAddFee").val());
        objRouteStop.UpTo = $.trim($("#txtUpTo").val());
        objRouteStop.TrailerPosition = $.trim($("#txtTrailerPosition").val());
        objRouteStop.TotalFee = $.trim($("#txtTotalFee").val());

        //console.log("add equipment : ", $.trim($("#txtPickUpEquipment").val()));
        var hdnEq = $("#hdnPickUpEquipment").val();
        var chqEq = $("#chkEquipment").val();
        var driverIDchk = $("#hdnDriver").val();
        console.log("hdnEQ: ", hdnEq);
        console.log("chqEq: ", chqEq);
        console.log("add glbEquipmentNdriver before add : ", glbEquipmentNdriver);
        console.log("driverIDchk: ", driverIDchk);
        if (routeNo > 1) {
            // var PickupEquipmentD = JSON.parse($.trim($("#hdnPickUpEquipment").val()));
            // var DeliveryEquipmentD = JSON.parse($.trim($("#hdnDeliveryEquipment").val()));
            //console.log("PickupEquipmentD: ", PickupEquipmentD[0]);
            // PickupEquipmentD.push(PickupEquipmentD[0]);
            // DeliveryEquipmentD.push(DeliveryEquipmentD[0]);
            //  console.log("PickupEquipmentD: ", PickupEquipmentD);
            objRouteStop.PickUpEquipment = $.trim($("#txtPickUpEquipment").val());
            objRouteStop.PickUpDriver = $.trim($("#txtPickUpdriver").val());
            // objRouteStop.PickUpEquipmentNdriver = PickupEquipmentD;
            // objRouteStop.DeliveryEquipmentNdriver = DeliveryEquipmentD;
            //  for (i = 0; i < glbEquipmentNdriver.length; i++) {
            if (glbEquipmentNdriver.length > 0) {
                var pickOb = {
                    FumigationEquipmentNDriverId: 0,
                    IsPickUp: glbEquipmentNdriver[0].IsPickUp,
                    EquipmentId: glbEquipmentNdriver[0].EquipmentId,
                    EquipmentName: glbEquipmentNdriver[0].EquipmentName,
                    DriverId: glbEquipmentNdriver[0].DriverId,
                    DriverName: glbEquipmentNdriver[0].DriverName,
                    RouteNo: routeNo,
                    IsDeleted: false

                };
                glbEquipmentNdriver.push(pickOb);
                var DelOb = {
                    FumigationEquipmentNDriverId: 0,
                    IsPickUp: glbEquipmentNdriver[1].IsPickUp,
                    EquipmentId: glbEquipmentNdriver[1].EquipmentId,
                    EquipmentName: glbEquipmentNdriver[1].EquipmentName,
                    DriverId: glbEquipmentNdriver[1].DriverId,
                    DriverName: glbEquipmentNdriver[1].DriverName,
                    RouteNo: routeNo,
                    IsDeleted: false

                };
                glbEquipmentNdriver.push(DelOb);
            }

            // }
            //var pickObj = {
            //    FumigationEquipmentNDriverId: 0,
            //    IsPickUp: PickupEquipmentD[0].IsPickUp,
            //    EquipmentId: PickupEquipmentD[0].EquipmentId,
            //    EquipmentName: PickupEquipmentD[0].EquipmentName,
            //    DriverId: PickupEquipmentD[0].DriverId,
            //    DriverName: PickupEquipmentD[0].DriverName,
            //    RouteNo: routeNo,
            //    IsDeleted: false

            //};
            //var DelObj = {
            //    FumigationEquipmentNDriverId: 0,
            //    IsPickUp: DeliveryEquipmentD[0].IsPickUp,
            //    EquipmentId: DeliveryEquipmentD[0].EquipmentId,
            //    EquipmentName: DeliveryEquipmentD[0].EquipmentName,
            //    DriverId: DeliveryEquipmentD[0].DriverId,
            //    DriverName: DeliveryEquipmentD[0].DriverName,
            //    RouteNo: routeNo,
            //    IsDeleted: false

            //};
            //if (pickObj!="") {
            //    glbEquipmentNdriver.push(pickObj);
            //}
            //if (pickObj != "") {
            //    glbEquipmentNdriver.push(DelObj);
            //}

            //
            console.log("add glbEquipmentNdriver : ", glbEquipmentNdriver);
            console.log("objRouteStop more than 1 route : ", glbRouteStops);
            objRouteStop.DeliveryEquipment = $.trim($("#txtDeliveryEquipment").val());
            objRouteStop.DeliveryDriver = $.trim($("#txtDeliveryDriver").val());
            // objRouteStop.DeliveryEquipmentNdriver = JSON.parse($.trim($("#hdnDeliveryEquipment").val()));

            // Pickup Date Stamp //
            $("#dtPickUpArrival").val(glbRouteStops[0].PickUpArrival);
            $("#dtActPickUpArrival").val(glbRouteStops[0].DriverPickupArrival);
            $("#dtActLoadingStart").val(glbRouteStops[0].DriverLoadingStartTime);
            $("#dtActLoadingFinish").val(glbRouteStops[0].DriverLoadingFinishTime);
            $("#dtActPickupDeparture").val(glbRouteStops[0].DriverPickupDeparture);


            // fumigation Date Stamp //

            $("#dtFumigationArrival").val(glbRouteStops[0].FumigationArrival);
            $("#dtActFumIn").val(glbRouteStops[0].DriverFumigationIn);
            $("#dtRelease").val(glbRouteStops[0].ReleaseDate);
            $("#dtActFumRelease").val(glbRouteStops[0].DriverFumigationRelease);
            $("#dtDeparture").val(glbRouteStops[0].DepartureDate);


            // Delivery Time Stamp //
            $("#dtDeliveryArrival").val(glbRouteStops[0].DeliveryArrival);
            $("#dtActDeliveryArrival").val(glbRouteStops[0].DriverDeliveryArrival);
            $("#dtActDeliveryDeparture").val(glbRouteStops[0].DriverDeliveryDeparture);
            var pickup_eq = glbRouteStops[0].PickUpEquipment;
            var checkbox = $('input[type="checkbox"][data-equipment-name="' + pickup_eq + '"]');
            checkbox.prop('checked', true);
            console.log("glbRouteStops[0]: ", glbRouteStops[0]);
            console.log("pickup_eq: ", pickup_eq);


        }
        else {

            objRouteStop.PickUpEquipment = $.trim($("#txtPickUpEquipment").val());
            console.log("else condition: ", objRouteStop.PickUpEquipment);
            // console.log("else condition: ", $("#hdnPickUpEquipment").val());
            objRouteStop.PickUpDriver = $.trim($("#txtPickUpdriver").val());
            objRouteStop.PickUpEquipmentNdriver = $.trim($("#hdnPickUpEquipment").val());

            //console.log("add PickUpEquipmentNdriver : ", objRouteStop.PickUpEquipmentNdriver);
            objRouteStop.DeliveryEquipment = $.trim($("#txtDeliveryEquipment").val());
            objRouteStop.DeliveryDriver = $.trim($("#txtDeliveryDriver").val());
            objRouteStop.DeliveryEquipmentNdriver = $.trim($("#hdnDeliveryEquipment").val());

        }

        objRouteStop.DriverPickupArrival = $.trim($("#dtActPickUpArrival").val());
        objRouteStop.DriverPickupDeparture = $.trim($("#dtActPickupDeparture").val());
        objRouteStop.DriverDeliveryArrival = $.trim($("#dtActDeliveryArrival").val());
        objRouteStop.DriverDeliveryDeparture = $.trim($("#dtActDeliveryDeparture").val());

        //objRouteStop.DriverPickupArrival = "";
        // objRouteStop.DriverLoadingStartTime = "";
        // objRouteStop.DriverLoadingFinishTime = "";
        // objRouteStop.DriverPickupDeparture = "";
        //  objRouteStop.DriverFumigationIn = "";
        // objRouteStop.DriverFumigationRelease = "";
        // objRouteStop.DriverDeliveryArrival = "";
        // objRouteStop.DriverDeliveryDeparture = "";


        objRouteStop.IsDeleted = false;

        //var result = [];
        //if (objRouteStop.PickUpEquipmentNdriver != "") {
        //    var pickup = JSON.parse(objRouteStop.PickUpEquipmentNdriver);
        //    result = pickup;
        //}

        //if (objRouteStop.DeliveryEquipmentNdriver != "") {
        //    var delivery = JSON.parse(objRouteStop.DeliveryEquipmentNdriver);
        //    result = delivery;
        //}

        //if (objRouteStop.PickUpEquipmentNdriver != "" && objRouteStop.DeliveryEquipmentNdriver != "") {
        //    result = JSON.stringify(pickup.concat(delivery));
        //    result = JSON.parse(result);
        //}


        //if (result.length > 0) {
        //    for (var i = 0; i < result.length; i++) {
        //        result[i].RouteNo = routeNo;
        //        glbEquipmentNdriver.push(result[i]);
        //    }
        //}
        var glbEquipmentNdriverList = glbEquipmentNdriver.filter(x => x.RouteNo == 0);

        if (glbEquipmentNdriverList.length > 0) {
            for (var i = 0; i < glbEquipmentNdriverList.length; i++) {
                glbEquipmentNdriverList[i].RouteNo = routeNo;

            }
        }
        //console.log("AddTrailers: ", $('#btnPickUpContinue'));
        //console.log("objRouteStop: ", objRouteStop);
        if ($("#hdnfumigationId").val() > 0) {
            var values = {};
            values = objRouteStop;
            console.log("objRrouteStop: ", objRouteStop);
            //console.log("hdnfumigationId: ", objRouteStop);
            $.ajax({
                url: baseUrl + 'Fumigation/Fumigation/AddRouteStops',
                data: JSON.stringify(values),
                type: "Post",
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: function (data) {
                    if (data > 0) {
                        objRouteStop.FumigationRoutsId = data;
                        glbRouteStops.push(objRouteStop);

                    }
                }
            });

        }
        else {
            // console.log("glbRouteStops else: ", glbRouteStops);
            glbRouteStops.push(objRouteStop);
        }

        $.alert({
            title: 'Success!',
            content: "<b>Your data has successfully been added to your shipment.<br/>  Don't forget to click on the Submit button to save all changes.</b>",
            type: 'green',
            typeAnimated: true,
            buttons: {
                ok: {
                    text: 'OK',
                    btnClass: 'btn-success',
                }
            },
        });
    }
    bindLocation();
    $("#txtVendorNConsignee").val("");
}
//#endregion
function SplitString(text, count, insertDots) {
    return text.slice(0, count) + (((text.length > count) && insertDots) ? "..." : "");
}
function BindRouteTable() {

    var dtRouteBody = "";
    $("#tblShipmentDetail tbody").empty();
    //console.log("glbRouteStops.length: " + glbRouteStops.length);
    if (glbRouteStops.length > 0) {

        var sequenceNo = 0;
        for (var i = 0; i < glbRouteStops.length; i++) {
            if (!glbRouteStops[i].IsDeleted) {

                sequenceNo = sequenceNo + 1;
                dtRouteBody += "<tr ondblclick='javascript: edit_route_stops(" + i + ");'>" +
                    "<td> <input type='radio' name='rdSelectedRoute' onchange='checkdata()' value='" + glbRouteStops[i].RouteNo + "' /> </td>" +
                    "<td><label name='rdSelectedRoute'>" + sequenceNo + "</label></td>" +
                    '<td><label data-toggle="tooltip" data-placement="top" title="' + GetAddress(glbRouteStops[i].PickUpLocationText) + '">' + GetCompanyName(glbRouteStops[i].PickUpLocationText) + '</label></td>' +
                    "<td>" + glbRouteStops[i].PickUpArrival + "</td>" +
                    "<td>" + glbRouteStops[i].AirWayBill + " " + glbRouteStops[i].CustomerPO + " " + glbRouteStops[i].ContainerNo + "</td>" +
                    "<td>" + glbRouteStops[i].PickUpEquipment + "</td>" +
                    "<td>" + glbRouteStops[i].PickUpDriver + "</td>" +
                    '<td><label data-toggle="tooltip" data-placement="top" title="' + GetAddress(glbRouteStops[i].FumigationSiteText) + '">' + GetCompanyName(glbRouteStops[i].FumigationSiteText) + '</label></td>' +
                    //"<td>" + glbRouteStops[i].BoxType + "</td>" +
                    "<td>" + glbRouteStops[i].PricingMethodText + "</td>" +
                    //"<td>" + glbRouteStops[i].BoxCount + "</td>" +
                    '<td><label data-toggle="tooltip" data-placement="top" title="' + GetAddress(glbRouteStops[i].DeliveryLocationText) + '">' + GetCompanyName(glbRouteStops[i].DeliveryLocationText) + '</label></td>' +
                    "<td>" + glbRouteStops[i].DeliveryArrival + "</td>" +
                    "<td>" + glbRouteStops[i].DeliveryEquipment + "</td>" +
                    "<td>" + glbRouteStops[i].DeliveryDriver + "</td>" +
                    "<td><button type = 'button' class='edit_icon' onclick = 'edit_route_stops(" + i + ")' > <i class='far fa-edit'></i> </button > <button type='button' class='delete_icon' onclick='remove_row_from_route(this)'> <i class='far fa-trash-alt'></i> </button> <button type='button' class='delete_icon' onclick='ShowSignaturePopUp(" + i + ")'> <i class='far far fa-eye'></i> </button></td>" +
                    "</tr>";
            }
        }
        // clearRouteStops();
        $("#tblShipmentDetail tbody").append(dtRouteBody);
    }
}
function replaceBR(string) {

    var str = string.replace("<br/>", "");
    return str;
}
//#region select default first radion button
function checkRadionButton() {

    if ($("#tblShipmentDetail tbody tr").length > 0) {
        $($("#tblShipmentDetail tbody tr")[0]).find("input[type=radio]").prop("checked", true);

        //bind damage document
        //bindDamageFileTbl();


    }
}
//#endregion


function ShowSignaturePopUp(index) {
    var routedetail = glbRouteStops[index];
    //$('#formAddress').trigger("reset");
    $("#lblReceiverName").text(routedetail.ReceiverName);
    $("#imgsignature").attr("src", routedetail.DigitalSignature);
    $("#modalSignature").modal("show");
    $('#modalSignature').draggable();
}

//#region bindupto level on dropdown change
var ddlPricingMethod = function () {
    $("#ddlPricingMethod").change(function () {


        CalculateTotalPrice();
    });
}
//#endregion


//#region show and hide shipment detail on the baseis of 
function checkdata() {

    if (glbAccessorialFee.length > 0) {
        getAccessorialPrice();
    }

    //bind damage document
    bindDamageFileTbl();
    //bind proof of temp
    bindProofOfTempTbl();

}
//#endregion
//#region clear route stops
btnClearRoute = function () {

    $("#btnClearRoute").on("click", function () {
        clearRouteStops();
        $("#tblShipmentDetail").attr("data-row-no", 0);
        //$("#btnAddRoute").text("ADD LOADING LOCATION & FREIGHT");

    })
}
//#region clear route stops field
clearRouteStops = function () {
    var ddlpickup = "<option selected='selected' value='0'> SEARCH LOCATION </option>";
    $("#ddlPickUpLocation").empty();
    $("#ddlPickUpLocation").append(ddlpickup);
    $(".ddlPickUpLocation").text("SEARCH LOCATION");

    var ddldelivery = "<option  selected='selected' value='0'> SEARCH LOCATION </option>";
    $("#ddlDeliveryLocation").empty();
    $("#ddlDeliveryLocation").append(ddldelivery);
    $(".ddlDeliveryLocation").text("SEARCH LOCATION");

    var ddldelivery = "<option  selected='selected' value='0'> SEARCH LOCATION </option>";
    $("#ddlFumigationSite").empty();
    $("#ddlFumigationSite").append(ddldelivery);
    $(".ddlFumigationSite").text("SEARCH LOCATION");

    $("#ddlFumigationType").val("");
    $("#txtAirWayBill").val("");
    $("#txtVendorNConsignee").val("");
    $("#txtCustomerPO").val("");

    //var ddlcustomer = "<option selected='selected' value='0'> SEARCH CUSTOMER </option>";
    //$("#ddlCustomer").empty();
    //$("#ddlCustomer").append(ddlcustomer);
    //$(".ddlCustomer").text("SEARCH CUSTOMER");
    // $("#txtRequestedBy").val("");


    $("#txtContainerNo").val("");
    $("#dtPickUpArrival").val("");
    $("#dtFumigationArrival").val("");
    $("#dtDeliveryArrival").val("");
    $("#txtPalletsCount").val("");
    $("#txtBoxCount").val("");
    //$("#ddlBoxType").val("");
    $("#txtReqTemperature").val("");
    $("#ddlTemperatureUnit").val("F");
    $("#ddlPricingMethod").val("");
    $("#txtVendorNConsignee").val("");
    $("#dtRelease").val("");
    $("#dtDeparture").val("");
    $("#txtCommodity").val("");
    //$("#ddlPricingMethod").val(0);
    $("#txtTrailerDays").val("");

    $("#txtMinFee").val("");
    $("#txtAddFee").val("");
    $("#txtUpTo").val("");
    $("#txtTrailerPosition").val("");
    $("#txtTotalFee").val("");
    // $("#txtPickUpEquipment").val("");
    //   $("#txtPickUpdriver").val("");
    // $("#txtPickUpdriver").val("");
    $("#hdnPickUpEquipment").val("");
    // $("#txtDeliveryEquipment").val("");
    //   $("#txtDeliveryDriver").val("");
    $("#hdnDeliveryEquipment").val("");
    $("#dtActLoadingStart").val("");
    $("#dtActLoadingFinish").val("");
    $("#dtActFumIn").val("");
    $("#dtActPickUpArrival").val("");
    $("#dtActPickupDeparture").val("");
    $("#dtActDeliveryArrival").val("");
    $("#dtActDeliveryDeparture").val("");
    $("#dtActFumRelease").val("");
    $("#ddlBoxType").val(0);
    HideTextBoxes();
    // $('.selectize-control').css('pointer-events', 'auto');
    $("#tblShipmentDetail").attr("data-row-no", 0);
    //$("#btnAddRoute").text("ADD LOADING LOCATION & FREIGHT");
}
//#endregion

//#region edit shipment detail by id
function edit_route_stops(index) {

    $($("#tblShipmentDetail tbody tr")).find("input[type=radio]").prop("checked", false);
    $($("#tblShipmentDetail tbody tr")[index]).find("input[type=radio]").prop("checked", true);

    var routedetail = glbRouteStops[index];
    console.log("txtPickUpEquipment: ", routedetail.PickUpEquipment);

    $("#ddlFumigationType").val(routedetail.FumigationTypeId);

    $("#txtAirWayBill").val(routedetail.AirWayBill);
    $("#txtVendorNConsignee").val(routedetail.VendorNConsignee);
    $("#txtCustomerPO").val(routedetail.CustomerPO);
    $("#txtContainerNo").val(routedetail.ContainerNo);
    $("#dtPickUpArrival").val(routedetail.PickUpArrival);
    $("#dtActPickUpArrival").val(routedetail.DriverPickupArrival);
    $("#dtFumigationArrival").val(routedetail.FumigationArrival);

    $("#dtRelease").val(routedetail.ReleaseDate);
    $("#dtDeparture").val(routedetail.DepartureDate);
    $("#txtCommodity").val(routedetail.Commodity);
    $("#ddlPricingMethod").val(routedetail.PricingMethod);
    $("#txtTrailerDays").val(routedetail.TrailerDays);

    $("#dtDeliveryArrival").val(routedetail.DeliveryArrival);
    $("#txtPalletsCount").val(routedetail.PalletCount);
    $("#txtBoxCount").val(routedetail.BoxCount);

    $("#ddlBoxType").val(routedetail.BoxType);
    $("#txtReqTemperature").val(routedetail.Temperature);
    $("#ddlTemperatureUnit").val(routedetail.TemperatureType);

    $("#txtMinFee").val(routedetail.MinFee);
    $("#txtAddFee").val(routedetail.AddFee);
    $("#txtUpTo").val(routedetail.UpTo);
    $("#txtTrailerPosition").val(routedetail.TrailerPosition);
    $("#txtTotalFee").val(routedetail.TotalFee);

    // $("#dtActPickUpArrival").val(routedetail.DriverPickupArrival);
    $("#dtActLoadingStart").val(routedetail.DriverLoadingStartTime);
    $("#dtActLoadingFinish").val(routedetail.DriverLoadingFinishTime);
    $("#dtActPickupDeparture").val(routedetail.DriverPickupDeparture);
    $("#dtActFumIn").val(routedetail.DriverFumigationIn);
    $("#dtActFumRelease").val(routedetail.DriverFumigationRelease);
    $("#dtActDeliveryArrival").val(routedetail.DriverDeliveryArrival);
    $("#dtActDeliveryDeparture").val(routedetail.DriverDeliveryDeparture);



    $("#txtPickUpEquipment").val(routedetail.PickUpEquipment);
    $("#txtPickUpdriver").val(routedetail.PickUpDriver);
    //console.log("routedetail.PickUpEquipmentNdriver: ", routedetail.PickUpEquipmentNdriver);
    if (routedetail.PickUpEquipmentNdriver != "") {
        //console.log("routedetail.PickUpEquipmentNdriver: ", routedetail.PickUpEquipmentNdriver);
        $("#hdnPickUpEquipment").val(JSON.stringify(routedetail.PickUpEquipmentNdriver));
    }

    $("#txtDeliveryEquipment").val(routedetail.DeliveryEquipment);
    $("#txtDeliveryDriver").val(routedetail.DeliveryDriver);
    if (routedetail.DeliveryEquipmentNdriver != "") {
        $("#hdnDeliveryEquipment").val(JSON.stringify(routedetail.DeliveryEquipmentNdriver));
    }


    $("#ddlPickUpLocation").val(routedetail.PickUpLocation);
    $("#ddlPickUpLocation").find('option:selected').text(routedetail.PickUpLocationText);

    $("#ddlFumigationSite").val(routedetail.FumigationSite);
    $("#ddlFumigationSite").find('option:selected').text(routedetail.FumigationSiteText);

    $("#ddlDeliveryLocation").val(routedetail.DeliveryLocation);
    $("#ddlDeliveryLocation").find('option:selected').text(routedetail.DeliveryLocationText);

    if (routedetail.PickUpLocation > 0) {
        var ddlpickup = "<option selected='selected' value=" + routedetail.PickUpLocation + ">" + replaceBR(routedetail.PickUpLocationText) + "</option>";
        $("#ddlPickUpLocation").empty();
        $("#ddlPickUpLocation").append(ddlpickup);
        $(".ddlPickUpLocation").text(replaceBR(routedetail.PickUpLocationText));
    }
    if (routedetail.DeliveryLocation > 0) {
        var ddldelivery = "<option  selected='selected' value=" + routedetail.DeliveryLocation + ">" + replaceBR(routedetail.DeliveryLocationText) + "</option>";
        $("#ddlDeliveryLocation").empty();
        $("#ddlDeliveryLocation").append(ddldelivery);
        $(".ddlDeliveryLocation").text(replaceBR(routedetail.DeliveryLocationText));
    }
    if (routedetail.FumigationSite > 0) {
        var ddlfumigation = "<option  selected='selected' value=" + routedetail.FumigationSite + ">" + replaceBR(routedetail.FumigationSiteText) + "</option>";
        $("#ddlFumigationSite").empty();
        $("#ddlFumigationSite").append(ddlfumigation);
        $(".ddlFumigationSite").text(replaceBR(routedetail.FumigationSiteText));
    }

    //$('.selectize-control').css('pointer-events', 'none');
    ShowHideTextBox();
    $("#tblShipmentDetail").attr("data-row-no", index + 1);
    $("#btnAddRoute").text("UPDATE LOADING LOCATION & FREIGHT");
    checkdata();
}
//#endregion

//#region Find and remove route and detail
function remove_row_from_route(_this) {
    $.confirm({
        title: 'Confirmation!',
        content: "<b>Are you sure you want to delete?</b>",
        type: 'red',
        typeAnimated: true,
        buttons: {
            delete: {
                btnClass: 'btn-green',
                action: function () {
                    row = $(_this).closest("tr");
                    var deletedrow = $(row).find("input[name='rdSelectedRoute']").val();// $(row).find("label[name='rdSelectedRoute']").text();

                    var routeStops = glbRouteStops.filter(x => x.RouteNo == deletedrow);
                    routeStops[0].IsDeleted = true;
                    BindRouteTable();
                }
            },
            cancel: {
                btnClass: 'btn-red',
            }
        }
    })

}
//#endregion

//#region function for drop location dropdown
var bindLocation = function () {
    manageLocation("ddlPickUpLocation");
    manageLocation("ddlFumigationSite");
    manageLocation("ddlDeliveryLocation");
}
//#endregion

//#region selectize on pickup and delivery location
var manageLocation = function (htmlcontrol) {

    var $select = $('#' + htmlcontrol).selectize();
    $select[0].selectize.destroy();

    $('#' + htmlcontrol).selectize({
        maxItems: 1,
        createOnBlur: false,
        valueField: 'id',
        labelField: 'text',
        searchField: 'text',
        closeAfterSelect: false,
        selectOnTab: true,
        plugins: ['restore_on_backspace'],
        load: function (query, callback) {
            if (!query.length) return callback();
            $.ajax({
                url: baseUrl + "Address/GetAddress/?searchText=" + query,
                type: 'GET',
                dataType: 'json',
                beforeSend: function (xhr, settings) {
                },
                error: function () {
                    callback();
                },
                success: function (response) {

                    var result = [];
                    $.each(response, function (index, value) {
                        item = {}
                        item.id = value.AddressId;
                        var nickName = (value.CompanyNickname == null ? "" : ' (' + value.CompanyNickname + ')');
                        item.text = $.trim(value.CompanyName) + ', ' + value.Address1 + ' ' + value.City + ' ' + value.StateName + ' ' + value.Zip + ' ' + nickName;
                        result.push(item);
                    });

                    callback(result);

                },
                complete: function () {

                }
            });
        },
        render: {
            item: function (item, escape) {
                return '<div>' +
                    ('<span class="name ' + htmlcontrol + '">' + escape(item.text) + '</span>') +
                    '</div>';
            },
            option: function (item, escape) {
                var label = item.text;
                return '<div style="padding: 2px 5px">' +
                    '<span style="display: block;">' + escape(label) + '</span>' +
                    '</div>';
            }
        },
        onFocus: function () {
            var value = this.getValue();
            this.clear(true);
            this.unlock();
        }

    });

}
//#endregion

//#region for open equipment popup
var openEquipmentModal = function () {
    $("#btnPickUpequipment").on("click", function () {

        var pickUpArrival = $("#dtPickUpArrival").val();
        if (pickUpArrival != "") {
            $("#hdnIsPickUpLocation").val(true);
            $("#modalEquipment").modal("show");
            $("#btnPickUpContinue").show();
            $("#btnDeliveryContinue").hide();
            //GetJsonValue(); //Added by DART to prefill the equipment in delivery popup.
            GetEquipmentList();
        }
        else {
            $.alert("Please select pickup arrival");
        }

    });
    $("#btnDeliveryequipment").on("click", function () {
        var fumigationArrival = $("#dtFumigationArrival").val();
        var deliveryArrival = $("#dtDeliveryArrival").val();
        if (fumigationArrival != "" && deliveryArrival != "") {
            $("#modalEquipment").modal("show");
            $("#btnPickUpContinue").hide();
            $("#btnDeliveryContinue").show();
            $("#hdnIsPickUpLocation").val(false);

            GetEquipmentList();
        }
        else {
            $.alert("Please select fumigation arrival and delivery arrival.")
        }

    });
}
//#endregion

//#region get json value
//function GetJsonValue() {

//    var fumigations = [];

//    for (i = 0; i < glbRouteStops.length; i++) {

//        var glbRouteStop = glbRouteStops[i];
//        var values = {};

//        values.FumigationId = $.trim($("#hdnfumigationId").val());
//        values.CustomerId = $.trim($("#ddlCustomer").val());
//        values.StatusId = $.trim($("#ddlStatus").val());
//        values.SubStatusId = $.trim($("#ddlSubStatus").val());
//        values.Reason = $.trim($("#txtReason").val());
//        //values.VendorNconsignee = $.trim($("#txtVendorNConsignee").val());
//        values.RequestedBy = $.trim($("#txtRequestedBy").val());
//        values.ShipmentRefNo = $.trim($("#txtShipRefNo").val());
//        values.Comments = $.trim($("#txtCustComments").val());
//        values.FumigationComment = $.trim($("#txtFumigationComments").val());

//        values.FumigationRouteDetail = [glbRouteStop];
//        values.AccessorialPrice = glbAccessorialFee;
//        values.FumigationEquipmentNdriver = glbEquipmentNdriver;
//        fumigations.push(values);

//    }

//    return fumigations;
//}

//#endregion


function GetJsonValue() {

    var values = {};

    values.FumigationId = $.trim($("#hdnfumigationId").val());
    values.CustomerId = $.trim($("#ddlCustomer").val());
    values.StatusId = $.trim($("#ddlStatus").val());
    values.SubStatusId = $.trim($("#ddlSubStatus").val());
    values.Reason = $.trim($("#txtReason").val());
    //values.VendorNconsignee = $.trim($("#txtVendorNConsignee").val());
    values.RequestedBy = $.trim($("#txtRequestedBy").val());
    values.ShipmentRefNo = $.trim($("#txtShipRefNo").val());
    values.Comments = $.trim($("#txtCustComments").val());
    values.FumigationComment = $.trim($("#txtFumigationComments").val());

    values.FumigationRouteDetail = glbRouteStops;
    values.AccessorialPrice = glbAccessorialFee;


    // console.log("glbEquipmentNdriver Get JsonValue Function:", glbEquipmentNdriver);
    // console.log("glbEquipmentNdriver Get Json:", $("#EqId").val());
    //  console.log("glbEquipmentNdriver Get Json:", glbEquipmentNdriver.length + " : " + glbRouteStops.length);

    var tempLen = glbEquipmentNdriver.length;
    var delObjExists = false;
    var chkEquipment = $("#chkEquipment").val();
    console.log("chkEquipment: ", chkEquipment);
    var deliveryEquipment = glbEquipmentNdriver.filter(x => x.IsPickUp == false && x.EquipmentId !== "");
    console.log("deliveryEquipment: ", deliveryEquipment);
    //if(chkEquipment==null || chkEquipment=="null"){
    for (let i = 0; i < tempLen; i++) {
        //delObjExists = false;
        console.log("delobject: ", glbEquipmentNdriver[i].RouteNo);
        if (glbEquipmentNdriver[i].IsPickUp == false && glbEquipmentNdriver[i].RouteNo != 0) {
            console.log("delobject in loop: ", glbEquipmentNdriver[i].RouteNo);
            delObjExists = true;
        }
       // console.log("glbEquipmentNdriver before push: ", glbEquipmentNdriver);
        //console.log("glbEquipmentNdriver RouteNo: ", glbEquipmentNdriver[i].RouteNo);
        //console.log("glbEquipmentNdriver before push: ", glbEquipmentNdriver);
       // console.log("delObjExists: " + glbEquipmentNdriver[i].IsPickUp + " : " + glbEquipmentNdriver[i].RouteNo + " : " + delObjExists);
        if (glbEquipmentNdriver.length >= 1 && glbEquipmentNdriver[i].IsPickUp == true && glbEquipmentNdriver.length <= 2) {
            var deliveryObj = {
                FumigationEquipmentNDriverId: 0,
                IsPickUp: false,
                EquipmentId: glbEquipmentNdriver[i].EquipmentId,
                EquipmentName: glbEquipmentNdriver[i].EquipmentName,
                //DriverId: glbEquipmentNdriver[i].DriverId,
                //DriverName: glbEquipmentNdriver[i].DriverName,
                RouteNo: glbEquipmentNdriver[i].RouteNo,
                IsDeleted: false
            }
            if (glbEquipmentNdriver[i + 1] == null && glbEquipmentNdriver[i].RouteNo == 0 && delObjExists == false) {
                glbEquipmentNdriver.push(deliveryObj);
            }
            console.log("glbEquipmentNdriver Get JsonValue Function after push:", glbEquipmentNdriver);
        }
    }
    //}

    console.log("glbEquipmentNdriver: Get Json Final Value : ", glbEquipmentNdriver);
    values.FumigationEquipmentNdriver = glbEquipmentNdriver;

    return values;
}
function SendMessage() {

    var values = {};
    values = GetJsonValue();
    //  console.log("values: ", values);
    var prevalues = GetValues();
    var PickupDetails = [];
    var driverid;
    //console.log("FumigationRouteDetail: ", values.FumigationRouteDetail.length);
    for (let i = 0; i < values.FumigationRouteDetail.length; i++) {
        var AWB = "";
        if (values.FumigationRouteDetail[i].AirWayBill != "") {
            AWB = values.FumigationRouteDetail[i].AirWayBill;
        }
        else if (values.FumigationRouteDetail[i].ContainerNo != "") {
            AWB = values.FumigationRouteDetail[i].ContainerNo;
        }
        else {
            AWB = values.FumigationRouteDetail[i].CustomerPO;
        }
        PickupDetails.push({
            "PickupLocation": values.FumigationRouteDetail[i].PickUpLocationText,
            "TrailerPosition": values.FumigationRouteDetail[i].TrailerPosition,
            "FumigationLocation": values.FumigationRouteDetail[i].FumigationSiteText,
            "AWB": AWB,
            "Equipment": values.FumigationRouteDetail[i].PickUpEquipment,
            "pallets": values.FumigationRouteDetail[i].PalletCount,
            "boxes": values.FumigationRouteDetail[i].BoxCount
        });

        broker = values.RequestedBy;
        const FumigationEquipmentNdriver = "FumigationEquipmentNdriver"[i] in values;
        // console.log("FumigationEquipmentNdriver: ", FumigationEquipmentNdriver);
        //console.log("values.FumigationEquipmentNdriver[i].DriverId: ", values.FumigationEquipmentNdriver[i].DriverId);
        // var customer = values.CustomerName;
        if (values.FumigationEquipmentNdriver[i].DriverId != "" && values.FumigationEquipmentNdriver[i].DriverId != "undefined") {
            // console.log("driver not empty");
            driverid = values.FumigationEquipmentNdriver[i].DriverId;
            // break;
        }
        else {
            driverid = values.FumigationEquipmentNdriver[0].DriverId;
        }
        // console.log("driverid: ", driverid);
        // console.log("PickupDetails: ", PickupDetails);
        var driverphone;
    }
    //console.log("driverid: ", driverid);
    var sameEquipment = true;
    var samePickUp = true;
    var sameFumSite = true;
    var finalSMSBodyStr = "";
    var commonStr = "";
    var AWB_str = "";
    var fumSMSArr = PickupDetails;
    var comments = values.Comments;
    //var fumedit = false;

    //for (var i = 0; i < fumSMSArr.length; i++) {

    //    if (fumSMSArr[i].Equipment != prevalues[i].EquipmentName || fumSMSArr[i].PickupLocation != prevalues[i].PickUpLocation || fumSMSArr[i].boxes != prevalues[i].Boxes || fumSMSArr[i].pallets != prevalues[i].Pallets || fumSMSArr[i].TrailerPosition != prevalues[i].TrailerPosition || fumSMSArr[i].FumigationLocation != prevalues[i].FumigationLocation || fumSMSArr[i].AWB != prevalues[i].AirWayBill) {
    //        fumedit = true;
    //    }

    //}
    // console.log("fumedit: " + fumedit);
    for (var i = 0; i < fumSMSArr.length; i++) {
        commonStr = "LARAS DISPATCH\nFUMIGATION LOADING \n\n";

        if (i > 0) {
            if (fumSMSArr[i - 1].Equipment != fumSMSArr[i].Equipment) {
                sameEquipment = false;
            }
            if (sameEquipment) {
                commonStr += "TR# " + fumSMSArr[i].Equipment + "\n";
            }
            if (fumSMSArr[i - 1].PickupLocation.split(",")[0] != fumSMSArr[i].PickupLocation.split(",")[0]) {
                samePickUp = false;
            }
            if (samePickUp) {
                //commonStr += "PU: " + fumSMSArr[i].PickupLocation.split(",")[0] + "\n";
            }
            if (samePickUp) {
                var PickArrival = ConvertDateNew(fumSMSArr[i].PickUpArrival, true);
                console.log("PickArrival: " + PickArrival);
                if (PickArrival != "NaN/NaN NaN:NaN") {
                    commonStr += "CARGAR: " + fumSMSArr[i].PickupLocation.split(",")[0] + " " + PickArrival + "\n";
                }

                //commonStr += "CARGAR: " + fumSMSArr[i].PickupLocation.split(",")[0] + "\n";
            }
            if (fumSMSArr[i - 1].FumigationLocation.split(",")[0] != fumSMSArr[i].FumigationLocation.split(",")[0]) {
                sameFumSite = false;
            }
            if (sameFumSite) {
                commonStr += "FUM: " + fumSMSArr[i].FumigationLocation.split(",")[0] + "\n";
            }
            commonStr += "BROKER: " + broker + "\n";
        }
        else {
            commonStr += "TR# " + fumSMSArr[i].Equipment + "\n";
            //commonStr += "PU: " + fumSMSArr[i].PickupLocation.split(",")[0] + "\n";
            var PickArrival = ConvertDateNew(fumSMSArr[i].PickUpArrival, true);
            if (PickArrival != "NaN/NaN NaN:NaN") {
                commonStr += "CARGAR: " + fumSMSArr[i].PickupLocation.split(",")[0] + " " + PickArrival + "\n";
            }

            commonStr += "FUM: " + fumSMSArr[i].FumigationLocation.split(",")[0] + "\n";
            commonStr += "BROKER: " + broker + "\n";
        }

    }
    for (var j = 0; j < fumSMSArr.length; j++) {
        // console.log("QQ: " + sameEquipment + " : " + samePickUp + " : " + sameFumSite);
        AWB_str += "\n";
        if (!sameEquipment) {
            AWB_str += "TR# " + fumSMSArr[j].Equipment + "\n";
        }
        if (!samePickUp) {
            //AWB_str += "PU: " + fumSMSArr[j].PickupLocation.split(",")[0] + "\n";
        }
        if (!samePickUp) {
            var PickArrival = ConvertDateNew(fumSMSArr[j].PickUpArrival, true);
            if (PickArrival != "NaN/NaN NaN:NaN") {
                AWB_str += "CARGAR: " + fumSMSArr[j].PickupLocation.split(",")[0] + " " + PickArrival + "\n";
            }

        }
        if (!sameFumSite) {
            AWB_str += "FUM: " + fumSMSArr[j].FumigationLocation.split(",")[0] + "\n";
        }
        //console.log("fumSMSArr[j].TrailerPosition: ", fumSMSArr[j].TrailerPosition);
        if (fumSMSArr[j].TrailerPosition) {
            AWB_str += fumSMSArr[j].TrailerPosition + "\n";
        }

        AWB_str += fumSMSArr[j].AWB + "\n";
        AWB_str += fumSMSArr[j].pallets + " PLTS" + " / " + fumSMSArr[j].boxes + " BXS\n";
        //if (comments != "") {
        //AWB_str += "INSTRUCCIONES: " + comments;
        //}
    }
    if (comments != "") {
        AWB_str += "INSTRUCCIONES: " + comments;
    }
    finalSMSBodyStr = commonStr + "\n" + AWB_str;
    // console.log(finalSMSBodyStr);
    var obj = {};
    $.when(
        $.ajax({
            type: 'GET',
            url: baseUrl + "/Shipment/Shipment/DriverPhone",
            data: { "driverid": driverid },
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            async: false,
            success: function (msg) {
                // Do something interesting here.
                console.log("data sent", msg);
                driverphone = msg;
            },
            error: function (xhr, err) {
                console.log("error : " + err);
            }
        })

    ).then(function () {
        //if (fumedit == true) {
        if (driverphone != "" || driverphone != "undefined") {
            console.log("driverphone ", driverphone);
            obj.pickupDetails = finalSMSBodyStr;
            obj.phone = driverphone;
            $.ajax({
                type: 'POST',
                url: baseUrl + "/CustomScript/SendSMS.aspx/PickupMessage",
                data: JSON.stringify(obj),
                contentType: 'application/json',
                dataType: 'json',
                success: function (msg) {
                    // Do something interesting here.
                    console.log("pickup message sent");
                },
                error: function (xhr, err) {
                    console.log("error : " + err + "PickupMessage error");
                }
            });
        }
        //  }

    });


}

function SendEditMessage() {

    var values = {};
    values = GetJsonValue();

    var prevalues = GetValues();
    console.log("prevalues: ", prevalues);
    var PickupDetails = [];
    var driverid;
    //console.log("FumigationRouteDetail: ", values.FumigationRouteDetail.length);
    for (let i = 0; i < values.FumigationRouteDetail.length; i++) {
        var AWB = "";
        if (values.FumigationRouteDetail[i].AirWayBill != "") {
            AWB = values.FumigationRouteDetail[i].AirWayBill;
        }
        else if (values.FumigationRouteDetail[i].ContainerNo != "") {
            AWB = values.FumigationRouteDetail[i].ContainerNo;
        }
        else {
            AWB = values.FumigationRouteDetail[i].CustomerPO;
        }


        broker = values.RequestedBy;
        var driverphone;
        var driverID;
        var PickupDriver;
        var DeliveryDriver;
        var pickupDriver;
        // var customer = values.CustomerName;
        for (let j = 0; j < values.FumigationEquipmentNdriver.length; j++) {
            if (values.FumigationEquipmentNdriver[j].DriverId != "" && values.FumigationEquipmentNdriver[j].DriverId != undefined) {
                driverid = values.FumigationEquipmentNdriver[j].DriverId;
            }
            if (values.FumigationEquipmentNdriver[j].IsPickUp == true && values.FumigationEquipmentNdriver[j].DriverName != "") {
                PickupDriver = values.FumigationEquipmentNdriver[j].DriverName;
                // console.log("PickupDriver: ", pickupDriver);
            }
            if (values.FumigationEquipmentNdriver[j].IsPickUp == false && values.FumigationEquipmentNdriver[j].DriverName != "") {
                DeliveryDriver = values.FumigationEquipmentNdriver[j].DriverName;
                //  console.log("PickupDriver: ", pickupDriver);
            }
            //console.log("driverid: ", driverid);
            //driverID = values.FumigationEquipmentNdriver[0].DriverId;
        }
        PickupDetails.push({
            "PickupLocation": values.FumigationRouteDetail[i].PickUpLocationText,
            "TrailerPosition": values.FumigationRouteDetail[i].TrailerPosition,
            "FumigationLocation": values.FumigationRouteDetail[i].FumigationSiteText,
            "AWB": AWB,
            "Equipment": values.FumigationRouteDetail[i].PickUpEquipment,
            "pallets": values.FumigationRouteDetail[i].PalletCount,
            "boxes": values.FumigationRouteDetail[i].BoxCount,
            "DriverID": driverid,
            "PickupDriverName": PickupDriver,
            "DeliveryDriverName": DeliveryDriver,
            "PickUpArrival": values.FumigationRouteDetail[i].PickUpArrival
        });

    }

    var sameEquipment = true;
    var samePickUp = true;
    var sameFumSite = true;
    var finalSMSBodyStr = "";
    var commonStr = "";
    var AWB_str = "";
    var fumSMSArr = PickupDetails;
    var comments = values.Comments;
    console.log("fumSMSArr: ", fumSMSArr);
    var fumedit = false;

    for (var i = 0; i < fumSMSArr.length; i++) {
        // console.log("Equipment: " + fumSMSArr[i].Equipment);
        // console.log("EDitArray: " + fumSMSArr[i].DriverID);
        //  console.log("prevaluesArray: " + prevalues[i].DriverID);
        if (fumSMSArr[i].Equipment != "") {


            if (fumSMSArr[i].PickupDriverName != prevalues[i].PickupDriverName) {
                fumedit = true;
            }
        }

    }
    console.log("fumedit: " + fumedit);
    for (var i = 0; i < fumSMSArr.length; i++) {
        commonStr = "LARAS DISPATCH\nFUMIGATION LOADING \n\n";

        if (i > 0) {
            if (fumSMSArr[i - 1].Equipment != fumSMSArr[i].Equipment) {
                sameEquipment = false;
            }
            if (sameEquipment) {
                commonStr += "TR# " + fumSMSArr[i].Equipment + "\n";
            }
            if (fumSMSArr[i - 1].PickupLocation.split(",")[0] != fumSMSArr[i].PickupLocation.split(",")[0]) {
                samePickUp = false;
            }
            if (samePickUp) {
                var PickArrival = ConvertDateNew(fumSMSArr[i].PickUpArrival, true);
                console.log("pick: " + PickArrival);
                if (PickArrival != "NaN/NaN NaN:NaN") {
                    commonStr += "CARGAR: " + fumSMSArr[i].PickupLocation.split(",")[0] + " " + PickArrival + "\n";
                }

                //commonStr += "CARGAR: " + fumSMSArr[i].PickupLocation.split(",")[0] + "\n";
            }
            if (fumSMSArr[i - 1].FumigationLocation.split(",")[0] != fumSMSArr[i].FumigationLocation.split(",")[0]) {
                sameFumSite = false;
            }
            if (sameFumSite) {
                commonStr += "FUM: " + fumSMSArr[i].FumigationLocation.split(",")[0] + "\n";
            }
            commonStr += "BROKER: " + broker + "\n";
        }
        else {
            commonStr += "TR# " + fumSMSArr[i].Equipment + "\n";
            var PickArrival = ConvertDateNew(fumSMSArr[i].PickUpArrival, true);
            if (PickArrival != "NaN/NaN NaN:NaN") {
                commonStr += "CARGAR: " + fumSMSArr[i].PickupLocation.split(",")[0] + " " + PickArrival + "\n";
            }

            commonStr += "FUM: " + fumSMSArr[i].FumigationLocation.split(",")[0] + "\n";
            commonStr += "BROKER: " + broker + "\n";
        }

    }
    for (var j = 0; j < fumSMSArr.length; j++) {
        // console.log("QQ: " + sameEquipment + " : " + samePickUp + " : " + sameFumSite);
        AWB_str += "\n";
        if (!sameEquipment) {
            AWB_str += "TR# " + fumSMSArr[j].Equipment + "\n";
        }
        if (!samePickUp) {
            var PickArrival = ConvertDateNew(fumSMSArr[j].PickUpArrival, true);
            AWB_str += "CARGAR: " + fumSMSArr[j].PickupLocation.split(",")[0] + " " + PickArrival + "\n";
        }
        if (!sameFumSite) {
            AWB_str += "FUM: " + fumSMSArr[j].FumigationLocation.split(",")[0] + "\n";
        }
        // console.log("fumSMSArr[j].TrailerPosition: ", fumSMSArr[j].TrailerPosition);
        if (fumSMSArr[j].TrailerPosition) {
            AWB_str += fumSMSArr[j].TrailerPosition + "\n";
        }

        AWB_str += fumSMSArr[j].AWB + "\n";
        AWB_str += fumSMSArr[j].pallets + " PLTS" + " / " + fumSMSArr[j].boxes + " BXS" + "\n";
    }
    if (comments != "") {
        AWB_str += "INSTRUCCIONES: " + comments;
    }
    finalSMSBodyStr = commonStr + "\n" + AWB_str;
    //console.log(finalSMSBodyStr);
    var obj = {};

    if (prevalues[0].DriverName == "null" && driverid != "") {
        $.when(
            $.ajax({
                type: 'GET',
                url: baseUrl + "/Shipment/Shipment/DriverPhone",
                data: { "driverid": driverid },
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                async: false,
                success: function (msg) {
                    // Do something interesting here.
                    console.log("data sent", msg);
                    driverphone = msg;
                },
                error: function (xhr, err) {
                    console.log("error : " + err);
                }
            })

        ).then(function () {

            if (driverphone != "" || driverphone != "undefined") {
                console.log("driverphone ", driverphone);
                obj.pickupDetails = finalSMSBodyStr;
                obj.phone = driverphone;
                $.ajax({
                    type: 'POST',
                    url: baseUrl + "/CustomScript/SendSMS.aspx/PickupMessage",
                    data: JSON.stringify(obj),
                    contentType: 'application/json',
                    dataType: 'json',
                    success: function (msg) {
                        // Do something interesting here.
                        console.log("pickup message sent");
                        console.log("pickup message sent", msg.d);
                    },
                    error: function (xhr, err) {
                        console.log("error : " + err + "PickupMessage error");
                    }
                });
            }


        });
    }
    else {
        $.when(
            $.ajax({
                type: 'GET',
                url: baseUrl + "/Shipment/Shipment/DriverPhone",
                data: { "driverid": driverid },
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                async: false,
                success: function (msg) {
                    // Do something interesting here.
                    console.log("data sent", msg);
                    driverphone = msg;
                },
                error: function (xhr, err) {
                    console.log("error : " + err);
                }
            })

        ).then(function () {

            if (fumedit == true) {
                if (driverphone != "" || driverphone != "undefined") {
                    console.log("driverphone ", driverphone);
                    obj.pickupDetails = finalSMSBodyStr;
                    obj.phone = driverphone;
                    $.ajax({
                        type: 'POST',
                        url: baseUrl + "/CustomScript/SendSMS.aspx/PickupMessage",
                        data: JSON.stringify(obj),
                        contentType: 'application/json',
                        dataType: 'json',
                        success: function (msg) {
                            // Do something interesting here.
                            console.log("pickup message sent", JSON.stringify(msg));
                            console.log("pickup message sent", msg.d);
                        },
                        error: function (xhr, err) {
                            console.log("error : " + err + "PickupMessage error");
                        }
                    });
                }
            }

        });
    }

}

function SendDeliveryMessage() {

    var values = {};
    values = GetJsonValue();
    console.log("values: ", values);
    var prevalues = GetValues();
    console.log("prevalues: ", prevalues);
    var DeliveryDetails = [];

    for (let i = 0; i < values.FumigationRouteDetail.length; i++) {
        var AWB = "";
        var customer;
        if (values.FumigationRouteDetail[i].AirWayBill != "") {
            AWB = values.FumigationRouteDetail[i].AirWayBill;
        }
        else if (values.FumigationRouteDetail[i].ContainerNo != "") {
            AWB = values.FumigationRouteDetail[i].ContainerNo;
        }
        else {
            AWB = values.FumigationRouteDetail[i].CustomerPO;
        }


        customer = values.CustomerId;
        var customername = CustomerName(customer);
        var driverphone;
        var driverID;
        var deliveryDriver;
        console.log("customername: ", customername);
        for (var j = 0; j < values.FumigationEquipmentNdriver.length; j++) {
            if (values.FumigationEquipmentNdriver[j].DriverId != "" || values.FumigationEquipmentNdriver[j].DriverId != 0) {
                driverid = values.FumigationEquipmentNdriver[j].DriverId;
            }
            driverID = values.FumigationEquipmentNdriver[j].DriverId;
            if (values.FumigationEquipmentNdriver[j].IsPickUp == false && values.FumigationEquipmentNdriver[j].DriverName != "") {
                deliveryDriver = values.FumigationEquipmentNdriver[j].DriverName;
            }


            //console.log("QQ: " + values.FumigationEquipmentNdriver[j].IsPickUp + " : " + values.FumigationEquipmentNdriver[j].DriverName + " : " + deliveryDriver);
        }
        var deliveryDriverName = "";
        if (deliveryDriver != "undefined") {
            deliveryDriverName = deliveryDriver;
            console.log("deliveryDriverName: ", deliveryDriverName);
        }
        DeliveryDetails.push({
            "DeliveryLocation": values.FumigationRouteDetail[i].DeliveryLocationText,
            "TrailerPosition": values.FumigationRouteDetail[i].TrailerPosition,
            "FumigationLocation": values.FumigationRouteDetail[i].FumigationSiteText,
            "AWB": AWB,
            "Equipment": values.FumigationRouteDetail[i].DeliveryEquipment,
            "pallets": values.FumigationRouteDetail[i].PalletCount,
            "boxes": values.FumigationRouteDetail[i].BoxCount,
            "DeliveryDriverName": deliveryDriverName,
            "DriverID": driverid
        });
        //console.log("driverid: ", driverid, deliveryDriverName);
        console.log("driverID: ", driverID, deliveryDriver);
        //console.log()
    }
    var sameEquipment = true;
    var samePickUp = true;
    var sameFumSite = true;
    var finalSMSBodyStr = "";
    var commonStr = "";
    var AWB_str = "";
    var fumSMSArr = DeliveryDetails;
    var comments = values.Comments;
    var fumedit = false;

    for (var i = 0; i < fumSMSArr.length; i++) {
        // console.log("Equipment: " + fumSMSArr[i].Equipment + " pre: " + prevalues[i].EquipmentName + " DeliveryLocation: " + fumSMSArr[i].DeliveryLocation + " pre: " + prevalues[i].DeliveryLocation + " boxes: " + fumSMSArr[i].boxes + " pre: " + prevalues[i].Boxes + " pallets: " + fumSMSArr[i].pallets + " pre: " + prevalues[i].Pallets + " TrailerPosition: " + fumSMSArr[i].TrailerPosition + " pre: " + prevalues[i].TrailerPosition + " FumigationLocation: " + fumSMSArr[i].FumigationLocation + " pre: " + prevalues[i].FumigationLocation + " AWB: " + fumSMSArr[i].AWB + " pre: " + prevalues[i].AirWayBill);
        if (fumSMSArr[i].Equipment != "") {
            if (fumSMSArr[i].DeliveryDriverName != prevalues[i].DeliveryDriverName) {
                fumedit = true;
            }

            // console.log("Equipment1: " + fumSMSArr[i].DeliveryDriverName + " : " + prevalues[i].DeliveryDriverName);
        }


    }
    console.log("fumedit ", fumedit);
    for (var i = 0; i < fumSMSArr.length; i++) {
        commonStr = "LARAS DISPATCH\nFUMIGATION DELIVERY \n";
        if (fumSMSArr.length == 1) {
            commonStr += "TR# " + fumSMSArr[i].Equipment + "\n";
            commonStr += "FUM: " + fumSMSArr[i].FumigationLocation.split(",")[0] + "\n";
            commonStr += fumSMSArr[i].TrailerPosition + "\n";
            commonStr += "DEL: " + fumSMSArr[i].DeliveryLocation.split(",")[0] + "\n";
            commonStr += "CUENTA: " + customername + "\n";
            commonStr += fumSMSArr[i].AWB + "\n";
            commonStr += fumSMSArr[i].pallets + " PLTS" + " / " + fumSMSArr[i].boxes + " BXS" + "\n";
            if (comments != "") {
                //commonStr += "INSTRUCCIONES: " + comments;
            }
            finalSMSBodyStr = commonStr;
            //console.log("if length 1: " + finalSMSBodyStr);

        }

        else {
            if (i > 0) {
                if (fumSMSArr[i - 1].Equipment != fumSMSArr[i].Equipment) {
                    sameEquipment = false;
                }
                if (sameEquipment) {
                    commonStr += "TR# " + fumSMSArr[i].Equipment + "\n";
                }
                if (fumSMSArr[i - 1].FumigationLocation.split(",")[0] != fumSMSArr[i].FumigationLocation.split(",")[0]) {
                    sameFumSite = false;
                }
                if (sameFumSite) {
                    commonStr += "FUM: " + fumSMSArr[i].FumigationLocation.split(",")[0] + "\n";
                }
                if (fumSMSArr[i - 1].DeliveryLocation.split(",")[0] != fumSMSArr[i].DeliveryLocation.split(",")[0]) {
                    samePickUp = false;
                }
                if (samePickUp) {
                    commonStr += "DEL: " + fumSMSArr[i].DeliveryLocation.split(",")[0] + "\n";
                }
                if (samePickUp && sameFumSite && sameEquipment) {
                    commonStr += "CUENTA: " + customername + "\n";
                    commonStr += fumSMSArr[i].AWB + "\n";
                    commonStr += fumSMSArr[i].pallets + " PLTS" + " / " + fumSMSArr[i].boxes + " BXS" + "\n";
                    if (comments != "") {
                        //commonStr += "INSTRUCCIONES: " + comments;
                    }

                }

            }



        }
    }
    for (var j = 0; j < fumSMSArr.length; j++) {

        AWB_str += "\n";
        if (!sameEquipment) {
            AWB_str += "TR# " + fumSMSArr[j].Equipment + "\n";
            if (fumSMSArr[j].TrailerPosition) {
                AWB_str += fumSMSArr[j].TrailerPosition + "\n";
            }

        }

        if (!sameFumSite) {
            AWB_str += "FUM: " + fumSMSArr[j].FumigationLocation.split(",")[0] + "\n";
        }
        if (!samePickUp) {
            AWB_str += "DEL: " + fumSMSArr[j].DeliveryLocation.split(",")[0] + "\n";

        }
        //if (!sameEquipment && !sameFumSite && !samePickUp) {
        AWB_str += "CUENTA: " + customername + "\n";
        AWB_str += fumSMSArr[j].AWB + "\n";
        AWB_str += fumSMSArr[j].pallets + " PLTS" + " / " + fumSMSArr[j].boxes + " BXS" + "\n";
        //if (comments != "") {
        // AWB_str += "INSTRUCCIONES: " + comments;
        //}


    }
    if (comments != "") {
        AWB_str += "INSTRUCCIONES: " + comments;
    }
    finalSMSBodyStr = commonStr + "\n\n" + AWB_str;
    // console.log("AWB_str: " + AWB_str);
    var obj = {};
    // console.log("finalSMSBodyStr: " + finalSMSBodyStr);
    //  console.log("prevalues.DriverName: " + prevalues[0].DriverName + " : " + driverID);
    //console.log("prevalues.DriverName: ", prevalues.EquipmentName);
    if (prevalues[0].DriverName == "null" && driverID != "") {
        $.when(
            $.ajax({
                type: 'GET',
                url: baseUrl + "/Shipment/Shipment/DriverPhone",
                data: { "driverid": driverID },
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                async: false,
                success: function (msg) {
                    // Do something interesting here.
                    console.log("data sent", msg);
                    driverphone = msg;
                },
                error: function (xhr, err) {
                    console.log("error : " + err + "driver Id not available");
                }
            })

        ).then(function () {

            if (driverphone != "" || driverphone != "undefined") {
                console.log("driverphone ", driverphone);
                obj.pickupDetails = finalSMSBodyStr;
                obj.phone = driverphone;
                console.log("obj: ", obj);
                $.ajax({
                    type: 'POST',
                    url: baseUrl + "/CustomScript/SendSMS.aspx/PickupMessage",
                    data: JSON.stringify(obj),
                    // data: obj,
                    contentType: 'application/json',
                    dataType: 'json',
                    success: function (msg) {
                        // Do something interesting here.
                        console.log("delivery message sent first");
                        console.log("delivery message parse: ", JSON.stringify(msg));
                        console.log("delivery message sent ", msg.d);
                    },
                    error: function (xhr, err) {
                        console.log("error : " + err + "DeliveryMessage Error");
                    }
                });
            }


        });
    }
    else {
        $.when(
            $.ajax({
                type: 'GET',
                url: baseUrl + "/Shipment/Shipment/DriverPhone",
                data: { "driverid": driverid },
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                async: false,
                success: function (msg) {
                    // Do something interesting here.
                    console.log("data sent 111", msg);
                    driverphone = msg;
                },
                error: function (xhr, err) {
                    console.log("error : " + err + "driverId not available");
                }
            })

        ).then(function () {
            if (fumedit == true) {
                if (driverphone != "" || driverphone != "undefined") {
                    console.log("driverphone ", driverphone);
                    obj.pickupDetails = finalSMSBodyStr;
                    obj.phone = driverphone;
                    $.ajax({
                        type: 'POST',
                        url: baseUrl + "/CustomScript/SendSMS.aspx/PickupMessage",
                        data: JSON.stringify(obj),
                        contentType: 'application/json',
                        dataType: 'json',
                        success: function (msg) {
                            // Do something interesting here.
                            console.log("delivery message parse: ", JSON.stringify(msg));
                            console.log("delivery message sent ", msg.d);
                        },
                        error: function (xhr, err) {
                            console.log("error : " + err + " DeliveryMessage Error");
                        }
                    });
                }
            }


        });
    }



}

function CustomerName(customerid) {

    var customername;
    $.ajax({
        type: 'GET',
        url: baseUrl + "/Shipment/Shipment/CustomerName",
        data: { "CustomerID": customerid },
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        success: function (msg) {

            console.log("data sent", msg);
            customername = msg;
        },
        error: function (xhr, err) {
            console.log("error : " + err);
        }
    })

    return customername;
}

//Go BACK... Added on 08-Feb-2023
$("#btnGoBack").on("click", function () {
    /*if (window.location.href == baseUrl + "Fumigation/Fumigation") {
        window.close();
    } else {
        window.location.href = baseUrl + "Fumigation/Fumigation/ViewFumigationList";
    }*/
    if (document.getElementsByClassName("jconfirm").length == 0 && window.location.href.toLowerCase().indexOf("index") >= 0) {
        window.location.href = baseUrl + "Fumigation/Fumigation/ViewFumigationList";
    } else if (document.getElementsByClassName("jconfirm").length == 0 && window.location.href.toLowerCase().indexOf("index") < 0) {
        window.close();
    }
    //alert(window.location.href);

    //history.back();
})
$("html").unbind().keyup(function (e) {
    console.log("Which Key: " + $(e.target));
    if (!$(e.target).is('input') && !$(e.target).is('textarea')) {
        console.log(e.which);
        //event.preventDefault();
        /*if (e.key === 'Backspace' || e.keyCode === 8) {
            //alert('backspace pressed');
            //window.location.href = baseUrl + "Shipment/Shcideipment/ViewShipmentList";
            if (document.getElementsByClassName("jconfirm").length == 0) {
                //window.location.href = baseUrl + "Fumigation/Fumigation/ViewFumigationList";
                if (document.getElementsByClassName("jconfirm").length == 0 && window.location.href.toLowerCase().indexOf("index") >= 0) {
                    window.location.href = baseUrl + "Fumigation/Fumigation/ViewFumigationList";
                } else if (document.getElementsByClassName("jconfirm").length == 0 && window.location.href.toLowerCase().indexOf("index") < 0) {
                    window.close();
                }
            } else if (document.getElementsByClassName("jconfirm").length >= 1) {
                return;
            }
        }
        if (e.key === 'Enter' || e.keyCode === 13) {
            console.log("ENTER: " + document.getElementsByClassName("jconfirm").length);
            if (document.getElementsByClassName("jconfirm").length == 0) {
                $("#btnSave").click();
            } else if (document.getElementsByClassName("jconfirm").length >= 1) {
                window.location.href = baseUrl + "Fumigation/Fumigation/ViewFumigationList";
            }
        }*/

        if (e.key === 'Backspace' || e.keyCode === 8) {
            //alert('backspace pressed');
            //window.location.href = baseUrl + "Shipment/Shipment/ViewShipmentList";
            if (document.getElementsByClassName("jconfirm").length == 0 && window.location.href.toLowerCase().indexOf("index") >= 0) {
                window.location.href = baseUrl + "Fumigation/Fumigation/ViewFumigationList";
            } else if (document.getElementsByClassName("jconfirm").length >= 1) {
                return;
            } else if (document.getElementsByClassName("jconfirm").length == 0 && window.location.href.toLowerCase().indexOf("index") < 0) {
                window.close();
            }
        }
        if (e.key === 'Enter' || e.keyCode === 13) {
            var titleTxt = "";
            if (document.getElementsByClassName("jconfirm-title")[0] != undefined) {
                titleTxt = document.getElementsByClassName("jconfirm-title")[0].innerHTML;
            }
            console.log("ENTER: " + document.getElementsByClassName("jconfirm").length + " : " + titleTxt);
            if (document.getElementsByClassName("jconfirm").length == 0) {
                $("#btnSave").click();
            } else if (document.getElementsByClassName("jconfirm").length >= 1 && titleTxt.toLowerCase() == "success!") {
                window.location.href = baseUrl + "Fumigation/Fumigation/ViewFumigationList";
            }
        }
    }
});
//

//#region save function for create Shipment
var btnSave = function () {
    $("#btnSave").on("click", function () {

        var values = {};
        values = GetJsonValue();
        var prevalues = GetValues();
        console.log("btnsave: ", values);
        console.log("prevalues: ", prevalues);
        //console.log("pickdriver: ", values.FumigationRouteDetail[0].PickUpEquipmentNdriver[0].DriverId);
        if ($("#ddlCustomer").val() > 0) {

            var mendetory = false;
            var message = "";
            if ((values.StatusId == 3 || values.StatusId == 4)) {

                if (values.SubStatusId == "") {
                    mendetory = true;
                    message = "Please select a sub-status.";
                }

                if (!mendetory && (values.SubStatusId == 7 || values.SubStatusId == 11) && values.Reason == "" && values.SubStatusId > 0) {
                    mendetory = true;
                    message = 'You selected "Other" as your sub-status. Please enter a brief description of the problem.';
                }
            }
            var requestedBy = $("#txtRequestedBy").val();
            if (mendetory == false && (requestedBy == "" || requestedBy == undefined)) {
                mendetory = true;
                message = "Please fill Requested By.";
            }
            if (mendetory) {
                $.alert({
                    title: 'Alert!',
                    content: message,
                    type: 'red',
                    typeAnimated: true,
                });
            }

            if (!mendetory) {
                if (validateContact()) {


                    if (glbRouteStops.length > 0) {
                        if (values.FumigationId > 0) {
                            //console.log("Picks: " , values.DriverPickupArrival);

                            $.ajax({
                                url: baseUrl + "/Fumigation/Fumigation/EditFumigation",
                                type: "POST",
                                beforeSend: function () {
                                    showLoader();
                                    // console.log("pickup Drive btnsave: ","FumigationEquipmentNdriver" in values);
                                    var deliveryDriver = "";
                                    var pickupDriver = "";
                                    const ddlstatus = $("#ddlStatus option:selected").text();
                                    console.log("pickup Drive btnsave: ", "FumigationEquipmentNdriver" in values);
                                    if (values.FumigationEquipmentNdriver.length > 0 && prevalues.length > 0) {
                                        for (let i = 0; i < values.FumigationEquipmentNdriver.length; i++) {
                                            if (values.FumigationEquipmentNdriver[i].IsPickUp == false) {
                                                if (values.FumigationEquipmentNdriver[i].DriverName != "" && values.FumigationEquipmentNdriver[i].DriverName != "undefined" && values.FumigationEquipmentNdriver[i].DriverName != undefined && values.FumigationEquipmentNdriver[i].DriverName != "Select Driver") {
                                                    deliveryDriver = values.FumigationEquipmentNdriver[i].DriverName;
                                                    //pickupDriver = values.FumigationEquipmentNdriver[i].DriverName;
                                                }
                                            }
                                        }

                                        console.log("deliver: ", deliveryDriver);
                                        if (deliveryDriver != "") {
                                            if (ddlstatus == "DRIVER ASSIGNED") {
                                                console.log("sending delivery");
                                                SendDeliveryMessage();
                                            }
                                        }
                                        else {

                                            SendEditMessage();
                                        }
                                    }


                                },
                                data: JSON.stringify(values),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",

                                success: function (response) {
                                    isNeedToloaded = false;
                                    hideLoader();
                                    if (response.IsSuccess) {
                                        //toastr.success(response.Message);
                                        //setTimeout(function () {
                                        //    window.location.href = baseUrl + "/Fumigation/Fumigation/ViewFumigationList";
                                        //}, 1000)

                                        $.alert({
                                            title: 'Success!',
                                            content: "<b>" + response.Message + "</b>",
                                            type: 'green',
                                            typeAnimated: true,
                                            buttons: {
                                                Ok: {
                                                    btnClass: 'btn-green',
                                                    action: function () {
                                                        window.location.href = baseUrl + "/Fumigation/Fumigation/ViewFumigationList";
                                                    }
                                                },
                                            }
                                        });

                                    }
                                    else {
                                        hideLoader();
                                        //toastr.error(response.Message);
                                        //AlertPopup(response.Message);
                                        AlertPopup("Email Failed...");
                                    }
                                },
                                error: function () {
                                    hideLoader();
                                    // toastr.error("Something went wrong.");
                                    AlertPopup("Something went wrong.");

                                }
                            });


                        }

                        else {

                            $.ajax({
                                url: baseUrl + "/Fumigation/Fumigation/CreateFumigation",
                                type: "POST",
                                data: JSON.stringify(values),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                beforeSend: function () {
                                    if (values.FumigationEquipmentNdriver.length != 0) {
                                        if (values.FumigationEquipmentNdriver[0].DriverId != "") {
                                            SendMessage();
                                        }
                                    }

                                    showLoader();
                                },
                                success: function (response) {
                                    isNeedToloaded = false;
                                    hideLoader();
                                    if (response.IsSuccess) {


                                        $.alert({
                                            title: 'Success!',
                                            content: "<b>" + response.Message + "</b>",
                                            type: 'green',
                                            typeAnimated: true,
                                            buttons: {
                                                Ok: {
                                                    btnClass: 'btn-green',
                                                    action: function () {

                                                        var prevTab = window.opener;
                                                        // close the previous tab
                                                        //  console.log("prev tab: ", prevTab);

                                                        prevTab.close();
                                                        window.addEventListener('beforeunload', function () {
                                                            // get a reference to the source tab
                                                            var sourceTab = window.opener;
                                                            // if the source tab is still open
                                                            if (sourceTab && !sourceTab.closed) {
                                                                // reload the source tab
                                                                sourceTab.location.reload();
                                                            }
                                                        });
                                                        window.close();
                                                        //window.location.href = baseUrl + "/Fumigation/Fumigation/ViewFumigationList";
                                                    }
                                                },
                                            }
                                        });

                                    }
                                    else {
                                        hideLoader();
                                        // toastr.error(response.Message);
                                        AlertPopup(response.Message);
                                    }
                                },
                                error: function () {
                                    hideLoader();
                                    //  toastr.error("Something went wrong.");
                                    AlertPopup("Something went wrong.");
                                }
                            });


                        }
                    }
                    else {
                        AlertPopup("Please enter route detail.");
                    }
                }

            }
        }
        else {

            //$.alert({
            //    title: 'Alert!',
            //    content: '<b>Please select a customer.</b>',
            //    type: 'red',
            //    //typeAnimated: true,
            //});
            AlertPopup('Please select a customer.')
        }
    })
}
//#endregion


var SendTempReport = function () {


    var values = {};
    values = GetJsonValue();
    var prevalues = GetValues();

    console.log("SendTempReport: ", values);



    if ($("#ddlCustomer").val() > 0) {

        var mendetory = false;
        var message = "";
        if ((values.StatusId == 3 || values.StatusId == 4)) {

            if (values.SubStatusId == "") {
                mendetory = true;
                message = "Please select a sub-status.";
            }

            if (!mendetory && (values.SubStatusId == 7 || values.SubStatusId == 11) && values.Reason == "" && values.SubStatusId > 0) {
                mendetory = true;
                message = 'You selected "Other" as your sub-status. Please enter a brief description of the problem.';
            }
        }
        var requestedBy = $("#txtRequestedBy").val();
        if (mendetory == false && (requestedBy == "" || requestedBy == undefined)) {
            mendetory = true;
            message = "Please fill Requested By.";
        }
        if (mendetory) {
            $.alert({
                title: 'Alert!',
                content: message,
                type: 'red',
                typeAnimated: true,
            });
        }



        if (!mendetory) {
            if (validateContact()) {


                if (glbRouteStops.length > 0) {
                    if (values.FumigationId > 0) {
                        //console.log("Picks: " , values.DriverPickupArrival);
                        $.ajax({
                            url: baseUrl + "/Fumigation/Fumigation/EditFumigation",
                            type: "POST",
                            beforeSend: function () {
                                showLoader();
                                GetFumigationById();

                            },
                            data: JSON.stringify(values),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",

                            success: function (response) {
                                isNeedToloaded = false;
                                hideLoader();
                                if (response.IsSuccess) {

                                    GetFumigationById();
                                }
                                else {
                                    hideLoader();
                                    //toastr.error(response.Message);
                                    //AlertPopup(response.Message);
                                }
                            },
                            error: function () {
                                hideLoader();
                                // toastr.error("Something went wrong.");
                                //AlertPopup("Something went wrong.");

                            }
                        });
                    }

                    else {

                        $.ajax({
                            url: baseUrl + "/Fumigation/Fumigation/CreateFumigation",
                            type: "POST",
                            data: JSON.stringify(values),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            beforeSend: function () {

                                showLoader();
                            },
                            success: function (response) {
                                isNeedToloaded = false;
                                hideLoader();
                                if (response.IsSuccess) {

                                    GetFumigationById();

                                }
                                else {
                                    hideLoader();
                                    // toastr.error(response.Message);
                                    // AlertPopup(response.Message);
                                }
                            },
                            error: function () {
                                hideLoader();
                                //  toastr.error("Something went wrong.");
                                // AlertPopup("Something went wrong.");
                            }
                        });


                    }
                }
                else {
                    AlertPopup("Please enter route detail.");
                }
            }

        }
    }
    else {

        //$.alert({
        //    title: 'Alert!',
        //    content: '<b>Please select a customer.</b>',
        //    type: 'red',
        //    //typeAnimated: true,
        //});
        AlertPopup('Please select a customer.')
    }

}


function ConvertFloat(el) {
    el.value = parseFloat(el.value).toFixed(2);
}

//#region bind accessorial fee type
function bindAccessorialfeeType() {
    $.ajax({
        url: baseUrl + 'Fumigation/Fumigation/GetAccessorialFeeType',
        data: {},
        type: "GET",
        async: false,
        success: function (data) {


            var divfixedfee = "";
            var divunitfee = "";
            var divother = "";
            //$("#ddlStatus").empty();
            for (var i = 0; i < data.length; i++) {
                if ($.trim(data[i].PricingMethod).toLowerCase() == $.trim("fixed fee").toLowerCase()) {
                    divfixedfee += '<div class="col-md-4">' +
                        '<div class="form-group row checkbox-container">' +
                        '<input class="col-md-1 chkfixedfee" name="chkfixedfee" id="chkfixedfee_' + data[i].Id + '" type="checkbox" value=' + data[i].Id + ' />' +
                        '<label class="col-sm-7 col-form-label" for="txtLoadingPerUnit">' + data[i].Name + '(' + data[i].PricingMethod + ')</label>' +
                        '<input type="text" onchange="ConvertFloat(this)" onkeypress="return onlyNumeric(event)" readonly=readonly   name="txt_' + data[i].Id + '" id="txt_' + data[i].Id + '" onfocusout="btnCalculatetotalFee()" class="col-sm-3 form-control" />' +
                        '</div>' +
                        '</div>'

                }
                else if ($.trim(data[i].PricingMethod).toLowerCase() == $.trim("per unit").toLowerCase()) {
                    divunitfee += '<div class="col-md-6">' +
                        '<div class="form-group row checkbox-container">' +
                        '<input class="col-md-1 chkunitfee" name="chkunitfee" id="chkunitfee_' + data[i].Id + '" type="checkbox" value=' + data[i].Id + ' />' +
                        '<label class="col-sm-5 col-form-label" for="txtLoadingPerUnit">' + data[i].Name + '(' + data[i].PricingMethod + ')</label>' +
                        '<input type="text" onchange="ConvertFloat(this)" onkeypress="return  onlyNumeric(event)" readonly=readonly  placeholder="Unit" id="txtUnit_' + data[i].Id + '" onfocusout="btnCalculatePerUnitFee(' + data[i].Id + ')" class="col-sm-1  form-control" />' +
                        '<input type="text" onchange="ConvertFloat(this)" onkeypress="return onlyNumeric(event)" readonly=readonly   placeholder="Amt/Unit" id="txtPerUnitAmount_' + data[i].Id + '" onfocusout="btnCalculatePerUnitFee(' + data[i].Id + ')" class="col-sm-2 form-control" />' +
                        '<input type="text" onchange="ConvertFloat(this)" onkeypress="return onlyNumeric(event)" readonly=readonly  placeholder="Amount" readonly="readonly" id="txtAmount_' + data[i].Id + '" class="col-sm-2 form-control" />' +
                        '</div>' +
                        '</div>'
                }
                else if ($.trim(data[i].PricingMethod).toLowerCase() == $.trim("other").toLowerCase()) {
                    divother += '<div class="col-md-6">' +
                        '<div class="form-group row checkbox-container">' +
                        '<input class="col-md-1 chkOtherFee" name="chkother"  id="chkother_' + data[i].Id + '" type="checkbox" value=' + data[i].Id + ' />' +
                        '<label class="col-sm-5 col-form-label" for="txtLoadingPerUnit">' + data[i].Name + '</label><input class="col-md-3 form-control txtReason" style="padding-left:0px;" readonly=readonly onfocusout="btnCalculatetotalFee()" placeholder= "   DESCRIPTION"  id="txtReason_' + data[i].Id + '" type="text"/>' +
                        '<input type="text" onchange="ConvertFloat(this)" onkeypress="return onlyNumeric(event)" readonly=readonly  id="txtAmount_' + data[i].Id + '" onfocusout="btnCalculatetotalFee()" class="col-sm-2 form-control" />' +
                        '</div>' +
                        '</div>'

                }
            }

            $("#divfixedfee").html(divfixedfee);
            $("#divunitfee").html(divunitfee);
            $("#divother").html(divother);

        }
    });
}
//#endregion

//#region calculate per unit total fee
var btnCalculatePerUnitFee = function (id) {
    var unit = $("#txtUnit_" + id + "").val();
    var amtperunit = $("#txtPerUnitAmount_" + id + "").val();
    var totalamount = 0;
    if (unit != "" && amtperunit != "") {
        totalamount = ConvertStringToFloat(unit) * ConvertStringToFloat(amtperunit);
    }

    $("#txtAmount_" + id + "").val(ConvertStringToFloat(totalamount));
    btnCalculatetotalFee();
}
//#endregion

//#region  add accessorial charges into array

var btnCalculatetotalFee = function () {

    var radioValue = $("input[name='rdSelectedRoute']:checked").val();

    if (radioValue > 0) {
        //glbAccessorialFee = glbAccessorialFee.filter(x => x.RouteNo != radioValue && x.IsDeleted == false);
        //glbAccessorialFee = glbAccessorialFee.filter(x => x.FumigationAccessorialPriceId != 0 && x.IsDeleted == false);

        $("input[name=chkunitfee]:checked").each(function () {

            var accessorialFeeTypeId = this.value;

            var unit = $("#txtUnit_" + accessorialFeeTypeId + "").val();
            var amtperunit = $("#txtPerUnitAmount_" + accessorialFeeTypeId + "").val();
            var totalamount = 0;
            if (unit != "" && amtperunit != "") {
                totalamount = ConvertStringToFloat(unit) * ConvertStringToFloat(amtperunit);
            }
            var accessorialChargesDatabyRouteId = glbAccessorialFee.filter(x => x.RouteNo == radioValue && x.AccessorialFeeTypeId == accessorialFeeTypeId && x.IsDeleted == false);

            if (accessorialChargesDatabyRouteId.length > 0) {
                accessorialChargesDatabyRouteId[0].Unit = unit;
                accessorialChargesDatabyRouteId[0].AmtPerUnit = amtperunit;
                accessorialChargesDatabyRouteId[0].Amount = totalamount;
            }
            else {
                if (totalamount != "" && totalamount > 0) {
                    glbAccessorialFee.push({
                        FumigationAccessorialPriceId: 0,
                        AccessorialFeeTypeId: accessorialFeeTypeId,
                        RouteNo: radioValue,
                        Unit: unit,
                        AmtPerUnit: amtperunit,
                        Amount: totalamount,
                        FeeType: "per unit",
                        IsDeleted: false,
                        Reason: ""
                    })
                }
            }
        });


        $("input[name=chkunitfee]:not(:checked)").each(function () {

            var accessorialFeeTypeId = this.value;
            var accessorialChargesDatabyRouteId = glbAccessorialFee.filter(x => x.RouteNo == radioValue && x.AccessorialFeeTypeId == accessorialFeeTypeId && x.IsDeleted == false);

            if (accessorialChargesDatabyRouteId.length > 0) {
                accessorialChargesDatabyRouteId[0].IsDeleted = true;

            }
        });


        $("input[name=chkfixedfee]:checked").each(function () {
            var accessorialFeeTypeId = this.value;
            var fixedAmount = $("#txt_" + accessorialFeeTypeId + "").val();
            var accessorialChargesDatabyRouteId = glbAccessorialFee.filter(x => x.RouteNo == radioValue && x.AccessorialFeeTypeId == accessorialFeeTypeId && x.IsDeleted == false);

            if (accessorialChargesDatabyRouteId.length > 0) {
                accessorialChargesDatabyRouteId[0].Amount = fixedAmount;
            }
            else {
                if (fixedAmount != "" && fixedAmount > 0) {
                    glbAccessorialFee.push({
                        FumigationAccessorialPriceId: 0,
                        AccessorialFeeTypeId: accessorialFeeTypeId,
                        RouteNo: radioValue,
                        Unit: 0,
                        AmtPerUnit: 0,
                        Amount: fixedAmount,
                        FeeType: "fixed fee",
                        IsDeleted: false,
                        Reason: ""
                    })
                }
            }
        });


        $("input[name=chkfixedfee]:not(:checked)").each(function () {

            var accessorialFeeTypeId = this.value;

            var accessorialChargesDatabyRouteId = glbAccessorialFee.filter(x => x.RouteNo == radioValue && x.AccessorialFeeTypeId == accessorialFeeTypeId && x.IsDeleted == false);

            if (accessorialChargesDatabyRouteId.length > 0) {
                accessorialChargesDatabyRouteId[0].IsDeleted = true;

            }
        });


        $("input[name=chkother]:checked").each(function () {

            var accessorialFeeTypeId = this.value;
            var otherAmount = $("#txtAmount_" + accessorialFeeTypeId + "").val();
            var reason = $("#txtReason_" + accessorialFeeTypeId + "").val();
            var accessorialChargesDatabyRouteId = glbAccessorialFee.filter(x => x.RouteNo == radioValue && x.AccessorialFeeTypeId == accessorialFeeTypeId && x.IsDeleted == false);
            if (accessorialChargesDatabyRouteId.length > 0) {
                accessorialChargesDatabyRouteId[0].Amount = otherAmount;
                accessorialChargesDatabyRouteId[0].Reason = reason;
            }
            else {

                if (otherAmount != "" && otherAmount > 0) {
                    glbAccessorialFee.push({
                        FumigationAccessorialPriceId: 0,
                        AccessorialFeeTypeId: accessorialFeeTypeId,
                        RouteNo: radioValue,
                        Unit: 0,
                        AmtPerUnit: 0,
                        Amount: otherAmount,
                        FeeType: "other",
                        IsDeleted: false,
                        Reason: reason
                    })
                }
            }
        });

        $("input[name=chkother]:not(:checked)").each(function () {

            var accessorialFeeTypeId = this.value;
            var accessorialChargesDatabyRouteId = glbAccessorialFee.filter(x => x.RouteNo == radioValue && x.AccessorialFeeTypeId == accessorialFeeTypeId && x.IsDeleted == false);

            if (accessorialChargesDatabyRouteId.length > 0) {
                accessorialChargesDatabyRouteId[0].IsDeleted = true;

            }
        });

        calculateTotalAccessorialAmount();
    }


}
//#endregion

//#region calculate total accessorial amount
function calculateTotalAccessorialAmount() {
    var radioValue = $("input[name='rdSelectedRoute']:checked").val();
    var accessorialChargesDatabyRouteId = glbAccessorialFee.filter(x => x.RouteNo == radioValue && x.IsDeleted == false);
    var totalAccessorialFee = 0;
    $.each(accessorialChargesDatabyRouteId, function (key, value) {
        totalAccessorialFee = Number(totalAccessorialFee) + Number(value.Amount);
    })
    $("#txtTotalAccessorialchargs").val(totalAccessorialFee);
}
//#endregion

//#region alert a message if route not selected and we add accessorial charges
function alertMessage(_this) {
    var radioValue = $("input[name='rdSelectedRoute']:checked").val();
    if (radioValue > 0) {
        return true;
    }
    else {
        // $(_this).prop("checked", false);
        // toastr.warning("Please select at least one Route Stop.")
        return true;
    }
}
//#endregion

//#region calcultae accessorial price on checkbox check
var chkAccessorialchargs = function () {

    $("#formAccessorialchargs").on("change", ".chkunitfee", function () {
        var accessorialFeeTypeId = this.value;
        if ($(this).is(':checked')) {
            if (alertMessage(this)) {
                $("#txtUnit_" + accessorialFeeTypeId + "").attr("readonly", false);
                $("#txtPerUnitAmount_" + accessorialFeeTypeId + "").attr("readonly", false);
                $("#txtAmount_" + accessorialFeeTypeId + "").attr("readonly", false);
                btnCalculatetotalFee();
            }

        } else {
            $("#txtUnit_" + accessorialFeeTypeId + "").val("");
            $("#txtPerUnitAmount_" + accessorialFeeTypeId + "").val("");
            $("#txtAmount_" + accessorialFeeTypeId + "").val("");

            $("#txtUnit_" + accessorialFeeTypeId + "").attr("readonly", true);
            $("#txtPerUnitAmount_" + accessorialFeeTypeId + "").attr("readonly", true);
            $("#txtAmount_" + accessorialFeeTypeId + "").attr("readonly", true);
            btnCalculatetotalFee();
        }
    });

    $("#formAccessorialchargs").on("change", ".chkfixedfee", function () {
        var accessorialFeeTypeId = this.value;

        if ($(this).is(':checked')) {
            if (alertMessage(this)) {
                $("#txt_" + accessorialFeeTypeId + "").attr("readonly", false);
                btnCalculatetotalFee();
            }

        }
        else {
            $("#txt_" + accessorialFeeTypeId + "").val("");
            $("#txt_" + accessorialFeeTypeId + "").attr("readonly", true);
            btnCalculatetotalFee();
        }
    });

    $("#formAccessorialchargs").on("change", ".chkOtherFee", function () {
        var accessorialFeeTypeId = this.value;

        if ($(this).is(':checked')) {
            if (alertMessage(this)) {
                $("#txtAmount_" + accessorialFeeTypeId + "").attr("readonly", false);
                $("#txtReason_" + accessorialFeeTypeId + "").attr("readonly", false);

                btnCalculatetotalFee();
            }
        }
        else {
            $("#txtAmount_" + accessorialFeeTypeId + "").val("");
            $("#txtReason_" + accessorialFeeTypeId + "").val("");
            $("#txtAmount_" + accessorialFeeTypeId + "").attr("readonly", true);
            $("#txtReason_" + accessorialFeeTypeId + "").attr("readonly", true);

            btnCalculatetotalFee();
        }

    });
}
//#endregion 

//#region function get accessorial price form array
function getAccessorialPrice() {

    ClearAccessorialCharges();
    var radioValue = $("input[name='rdSelectedRoute']:checked").val();

    var glbAccessorialChargesByRouteId = glbAccessorialFee.filter(x => x.RouteNo == radioValue && x.IsDeleted == false);

    if (glbAccessorialChargesByRouteId.length > 0) {
        ///var data = glbAccessorialChargesByRouteId[0];

        $.each(glbAccessorialChargesByRouteId, function (key, value) {

            var accessorialFeeTypeId = value.AccessorialFeeTypeId;
            if (value.FeeType == "per unit") {

                $("#txtUnit_" + accessorialFeeTypeId + "").attr("readonly", false);
                $("#txtPerUnitAmount_" + accessorialFeeTypeId + "").attr("readonly", false);
                $("#txtAmount_" + accessorialFeeTypeId + "").attr("readonly", false);

                $("#txtUnit_" + accessorialFeeTypeId + "").val(value.Unit);
                $("#txtPerUnitAmount_" + accessorialFeeTypeId + "").val(value.AmtPerUnit);
                $("#txtAmount_" + accessorialFeeTypeId + "").val(value.Amount);
                $("#chkunitfee_" + accessorialFeeTypeId + "").prop("checked", true);
            }
            else if (value.FeeType == "fixed fee") {
                $("#txt_" + accessorialFeeTypeId + "").attr("readonly", false);
                $("#txt_" + accessorialFeeTypeId + "").val(value.Amount);
                $("#chkfixedfee_" + accessorialFeeTypeId + "").prop("checked", true);
            }
            else if (value.FeeType == "other") {
                $("#txtAmount_" + accessorialFeeTypeId + "").attr("readonly", false);
                $("#txtReason_" + accessorialFeeTypeId + "").attr("readonly", false);
                $("#txtAmount_" + accessorialFeeTypeId + "").val(value.Amount == 0 ? "" : value.Amount);
                $("#txtReason_" + accessorialFeeTypeId + "").val(value.Reason == "" ? "" : value.Reason);
                $("#chkother_" + accessorialFeeTypeId + "").prop("checked", true);
            }
        });
        calculateTotalAccessorialAmount();


    }

}
//#endregion

//#region clear accessorial charges
function ClearAccessorialCharges() {

    $("input[name=chkunitfee]").each(function () {

        var accessorialFeeTypeId = this.value;
        //.attr("readonly", true);
        $("#txtUnit_" + accessorialFeeTypeId + "").val("");
        $("#txtPerUnitAmount_" + accessorialFeeTypeId + "").val("");
        $("#txtAmount_" + accessorialFeeTypeId + "").val("");

        $("#txtUnit_" + accessorialFeeTypeId + "").attr("readonly", true);
        $("#txtPerUnitAmount_" + accessorialFeeTypeId + "").attr("readonly", true);
        $("#txtAmount_" + accessorialFeeTypeId + "").attr("readonly", true);

        $("#chkunitfee_" + accessorialFeeTypeId + "").prop("checked", false);

    });

    $("input[name=chkfixedfee]:checked").each(function () {
        var accessorialFeeTypeId = this.value;
        $("#txt_" + accessorialFeeTypeId + "").val("");
        $("#txt_" + accessorialFeeTypeId + "").attr("readonly", true);
        $("#chkfixedfee_" + accessorialFeeTypeId + "").prop("checked", false);

    });

    $("input[name=chkother]:checked").each(function () {
        var accessorialFeeTypeId = this.value;
        $("#txtAmount_" + accessorialFeeTypeId + "").val("");
        $("#txtReason_" + accessorialFeeTypeId + "").val("");
        $("#txtAmount_" + accessorialFeeTypeId + "").attr("readonly", true);
        $("#txtReason_" + accessorialFeeTypeId + "").attr("readonly", true);
        $("#chkother_" + accessorialFeeTypeId + "").prop("checked", false);
    });

    $("#txtTotalAccessorialchargs").val("");


}
//#endregion 

//#region delete accessorial price during route deletion
function RemoveAccessorialPrice(routeno) {

    var accessorialChargesDatabyRouteId = glbAccessorialFee.filter(x => x.RouteNo == routeno);
    if (accessorialChargesDatabyRouteId.length > 0) {
        for (var i = 0; i < accessorialChargesDatabyRouteId.length; i++) {
            accessorialChargesDatabyRouteId[i].IsDeleted = true;
        }
        //glbAccessorialFee = glbAccessorialFee.filter(x => x.RouteNo != routeno);
    }
}
//#endregion
//#region convert Temperture F to C
var convertActualTemp = function () {
    var actualTemp;
    $("#ddlActualTemperatureUnit").on("change", function () {
        var unit = $(this).val();
        var temp = $("#txtActualTemp").val();
        if (temp != '') {
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
var copyData;
var filesArray = [];
window.onload = function() {
    document.getElementById("fileBasket").
        addEventListener("paste", handlePaste);
};
 document.onpaste = function(event) {
	var items = (event.clipboardData || event.originalEvent.clipboardData).items;
	var fileNames = "";
    console.log('items new : ', items);
    console.log('items new : ', items.length);

      for (var i = 0; i < items.length; i++) {
      console.log('item', items[i]);	
      var item = items[i];
      if (item.kind === 'file') {
        var blob = item.getAsFile();
		fileNames += blob.name + "<br />";
		var dataTransfer = new DataTransfer();
			// Loop through the files array and add files to the DataTransfer object
			//filesArray.forEach(function(file) {
			  dataTransfer.items.add(blob);
			  filesArray.push(blob);
			copyData = filesArray;
			 
			console.log("copyData after file upload: ",copyData);
		console.log("blob: ",blob);
	  }
	  }
	  $("#fileBasket").html(fileNames);
} 
function handlePaste(e) {
	console.log("clipboard itmes: ",e.clipboardData.files);
	 
    for (var i = 0 ; i < e.clipboardData.items.length ; i++) {
        var item = e.clipboardData.items[i];
        console.log("Item type: " + item.type);
        console.log("Item : " , item);
		//$("#fileBasket").html(fileNames)
        if (item.type.indexOf("image") != -1) {
            uploadFile(item.getAsFile());
			
			var tempData = item.getAsFile();
			
        } else {
            console.log("Discarding non-image paste data");
        }
    }
	//copyData = filesArray;
}
function uploadFile(file) {
    var xhr = new XMLHttpRequest();

    xhr.upload.onprogress = function(e) {
        var percentComplete = (e.loaded / e.total) * 100;
        console.log("Uploaded: " + percentComplete + "%");
        console.log("Uploaded file: ",file);
			var dataTransfer = new DataTransfer();
			// Loop through the files array and add files to the DataTransfer object
			//filesArray.forEach(function(file) {
			  dataTransfer.items.add(file);
			  filesArray.push(file);
			copyData = filesArray;
			
			console.log("copyData after file upload: ",copyData);
		 $("#fileBasket").html(file.name);
    };

    xhr.onload = function() {
        if (xhr.status == 200) {
            alert("Sucess! Upload completed");
        } else {
            alert("Error! Upload failed");
        }
    };

    xhr.onerror = function() {
        alert("Error! Upload failed. Can not connect to server.");
    };

    xhr.open("POST", "FileUploader", true);
    xhr.setRequestHeader("Content-Type", file.type);
    xhr.send(file);
}
var tempData;
var filesUpload;
$(document).ready(function () {




    $("#fileBasket").on("dragenter", function (evt) {
        evt.preventDefault();
        evt.stopPropagation();
        $("#fileBasket").css("border", "3px dashed #000");

    });

    $("#fileBasket").on("dragover", function (evt) {
        evt.preventDefault();
        evt.stopPropagation();

    });
    $("#fileBasket").on("dragleave", function (evt) {
        evt.preventDefault();
        evt.stopPropagation();
        $("#fileBasket").css("border", "1px dashed #000");
    });

    $("#fileBasket").on("drop", function (evt) {
        evt.preventDefault();
        evt.stopPropagation();
        filesUpload = evt.originalEvent.dataTransfer.files;
        console.log("files: ", filesUpload);
        var fileNames = "";
        if (filesUpload.length > 0) {
            //fileNames += "Uploading <br/>"
            for (var i = 0; i < filesUpload.length; i++) {
                fileNames += filesUpload[i].name + "<br />";
            }
        }
        $("#fileBasket").html(fileNames)

        tempData = new FormData();
        for (var i = 0; i < filesUpload.length; i++) {
            tempData.append(filesUpload[i].name, filesUpload[i]);
        }
        $("#fileBasket").css("border", "1px dashed #000");
    });
    return filesUpload;
});

//#endregion
//#region Button Upload Proof of Temp
var btnProofOfTemp = function () {


    $(".btnProofOfTemp").on("click", function () {
        var tblRowsCount = $("#tblShipmentDetail").attr("data-row-no");
        SendTempReport();

        setTimeout(function () {
            if (isFormValid('divProofOfTemp')) {
                var url = window.location.pathname;
                var fumigationId = url.substring(url.lastIndexOf('/') + 1);
                var fileUploader = $("#fuProofOfTemperature");
                var actualTemp = $("#txtActualTemp").val();
                var dtPickup = $("#dtPickUpArrival").val();
                var dtActPickup = $("#dtActPickUpArrival").val();
                var dtActStart = $("#dtActLoadingStart").val();
                var dtActFinish = $("#dtActLoadingFinish").val();
                var dtActFumIn = $("#dtActFumIn").val();
                var deliveryTemp = $("#txtDeliveryTemp").val();
				console.log("copyData: ",copyData);
				
				
                var tempupload = filesUpload;
                if(tempupload==undefined){
					tempupload = copyData;
				}
                console.log("dtPickup: ", dtPickup);
                console.log("dtActPickup: ", dtActPickup);
                console.log("dtActStart: ", dtActStart);
				console.log("tempUpload: ", tempupload);
                var isValid = true;
                var message = "";
                if ((actualTemp == "")) {
                    message = "Please enter Loading Temp.";
                    isValid = false;
                    $.alert({
                        title: 'Alert!',
                        content: '<b>' + message + '</b>',
                        type: 'red',
                        typeAnimated: true,
                    });

                }
                //else if ((dtPickup == "")) {
                //    message = "Please enter Pickup Arrival.";
                //    isValid = false;
                //    $.alert({
                //        title: 'Alert!',
                //        content: '<b>' + message + '</b>',
                //        type: 'red',
                //        typeAnimated: true,
                //    });

                //}
                //else if ((dtActPickup == "")) {
                //    message = "Please enter Actual Arrival.";
                //    isValid = false;
                //    $.alert({
                //        title: 'Alert!',
                //        content: '<b>' + message + '</b>',
                //        type: 'red',
                //        typeAnimated: true,
                //    });

                //}
                //else if ((dtActStart == "")) {
                //    message = "Please enter Actual Loading Start.";
                //    isValid = false;
                //    $.alert({
                //        title: 'Alert!',
                //        content: '<b>' + message + '</b>',
                //        type: 'red',
                //        typeAnimated: true,
                //    });

                //}
                //else if ((dtActFinish == "")) {
                //    message = "Please enter Actual Loading Finish.";
                //    isValid = false;
                //    $.alert({
                //        title: 'Alert!',
                //        content: '<b>' + message + '</b>',
                //        type: 'red',
                //        typeAnimated: true,
                //    });

                //}
                //else if ((dtActFumIn == "")) {
                //    message = "Please enter Actual Fumigation In.";
                //    isValid = false;
                //    $.alert({
                //        title: 'Alert!',
                //        content: '<b>' + message + '</b>',
                //        type: 'red',
                //        typeAnimated: true,
                //    });

                //}
                else if ((tempupload == undefined)) {
                    message = "Please Drag & Drop Temperature Files.";
                    isValid = false;
                    $.alert({
                        title: 'Alert!',
                        content: '<b>' + message + '</b>',
                        type: 'red',
                        typeAnimated: true,
                    });

                }
                else {
                    isValid = true;
                }
                if (isValid) {

                    var unit = $("#ddlActualTemperatureUnit").val();
                    if (unit == 'C') {
                        actualTemp = CelsiusToFahrenheit(actualTemp);
                        deliveryTemp = CelsiusToFahrenheit(deliveryTemp);
                    }
                    console.log("actualTemp: ", actualTemp);
                    // Date Time Format 
                    var d = new Date($.now());
                    date = (d.getDate() + "-" + (d.getMonth() + 1) + "-" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds());
                    //

                    if (tempupload.length) {

                        var filesUploaded = tempupload;

                        var routeNo = $("input[name='rdSelectedRoute']:checked").val();
                        var routeStops = glbRouteStops.filter(x => x.RouteNo == routeNo);
                        if (routeStops[0].FumigationRoutsId > 0) {

                            var radioValue = $("input[name='rdSelectedRoute']:checked").val();
                            console.log("radioValue: ", radioValue);
                            if (radioValue > 0) {
                                var data = new FormData();


                                if (filesUploaded.length) {
                                    var draggedFiles = [];
                                    console.info("filesUploaded: ");
                                    console.log(filesUploaded);
                                    for (let i = 0; i < filesUploaded.length; i++) {
                                        //data.append("ProofOfTemprature_"+(i+1), filesUploaded[i]);
                                        console.info("QQ: " + filesUploaded[i]);
                                        console.log(filesUploaded[i]);
                                        //draggedFiles.push(filesUploaded[i]);
                                        data.append("filesObj", filesUploaded[i]);
                                    }
                                    //data.append("filesObj", JSON.stringify(draggedFiles));
                                }

                                var rowindex = Number(tblRowsCount) - 1;
                                var routeStops = glbRouteStops[rowindex];
                                console.log("routeStops: ", routeStops);
                                data.append("ActualTemperature", actualTemp);

                                console.log("rowindex: ", rowindex);
                                // data.append("DeliveryTemp", deliveryTemp);
                                data.append("FumigationRouteId", routeStops.FumigationRoutsId);
                                console.log("FumigationRouteId", routeStops.FumigationRoutsId);
                                data.append("FumigationId", fumigationId);
                                console.log("data: ", data);
                                showLoader();
                                $.ajax({
                                    type: "POST",
                                    url: baseUrl + '/Fumigation/Fumigation/UploadProofofTemperature',
                                    contentType: false,
                                    processData: false,
                                    data: data,
                                    beforeSend: function () {

                                        //$.alert({
                                        //    title: 'Mail Sent!',
                                        //    content: "<b>Email sent to Customer.</b>",
                                        //    type: 'green',
                                        //    typeAnimated: true,
                                        //    buttons: {
                                        //        Ok: {
                                        //            btnClass: 'btn-green',
                                        //            action: function () {

                                        //                GetFumigationById();
                                        //            }
                                        //        },
                                        //    }
                                        //});
                                    },

                                    // async: false,
                                    success: function (data, textStatus, jqXHR) {
                                        if (data.IsSuccess == true) {

                                            $.alert({

                                                title: 'Success!',
                                                content: "<b>Email has been sent to the customer.</b>",
                                                type: 'green',
                                                typeAnimated: true,
                                                buttons: {
                                                    Ok: {
                                                        btnClass: 'btn-green',
                                                        action: function () {
                                                            hideLoader();
                                                            GetFumigationById();
                                                        }
                                                    },
                                                }
                                            });

                                        }
                                        else {
                                            //toastr.warning(data.Message);
                                            //AlertPopup(data.Message);
                                            AlertPopup("EMAIL FAILURE....")
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

                        }
                        else {

                            //  toastr.warning("Please click on SUBMIT button to save this Freight detail into database.Then you can upload image for this route.");
                            AlertPopup("Please click on SUBMIT button to save this Freight detail into database.Then you can upload image for this route.");
                        }
                        $("#txtActualTemp").val("");
                        $("#fuProofOfTemperature").val(null);
                        bindProofOfTempTbl();
                    }

                }
            }
        }, 1000);
    })
}
//#endregion

function bindProofOfTempTbl() {
    isApproveProofofTemp = false;
    var tr = "";
    $("#tblProofOfTemp tbody").empty();


    var routeNo = $("input[name='rdSelectedRoute']:checked").val();
    var routeStops = glbRouteStops.filter(x => x.RouteNo == routeNo);
    if (routeStops[0].FumigationRoutsId > 0) {

        var proofoftemp = glbProofOfTemprature.filter(x => x.FumigationRouteId == routeStops[0].FumigationRoutsId);
        if (proofoftemp.length > 0) {
            for (var i = 0; i < proofoftemp.length; i++) {
                var imgUrl = proofoftemp[i].ImageUrl;
                var tempImg = imgUrl.replace('\\', '/');
                console.log("imgUrl: ", tempImg);
                var location = proofoftemp[i].IsLoading == true ? "Loading" : "Delivery";
                tr += '<tr data-file-url=' + tempImg + ' ondblclick="javascript: ViewProofOfTemperature(this,' + proofoftemp[i].ProofOfTempratureId + ',' + proofoftemp[i].IsApproved + ');">' +
                    //'<td>' + (i + 1) + '</td>' +
                    '<td>' + proofoftemp[i].ActualTemperature + '</td>' +
                    '<td>' + location + '</td>' +
                    //'<td>' + proofoftemp[i].ImageName + '</td>' +
                    '<td>' + proofoftemp[i].Date + '</td>' +
                    '<td><button type="button" class="delete_icon" onclick="remove_proofOfTemp_row(this,' + proofoftemp[i].ProofOfTempratureId + ')"> <i class="far fa-trash-alt"></i></button><button type="button" data-file-url=' + tempImg + ' onclick="ViewProofOfTemperature(this,' + proofoftemp[i].ProofOfTempratureId + ',' + proofoftemp[i].IsApproved + ')" class="delete_icon"><i class="far fa-eye"></i></button> <a href=' + proofoftemp[i].ImageUrl + ' download title="download" class="edit_icon fileDownload" ><i class="fa fa-download"></i></a> &nbsp; '
                if (proofoftemp[i].IsApproved) {
                    tr += '<a style="color:green"><i class="fa fa-check"></a></td>'
                } else {
                    isApproveProofofTemp = true;
                    tr += '<a><i class="fa fa-times"></a></td>'

                }
                tr += '</tr>'
            }

        }
    }
    $("#tblProofOfTemp tbody").append(tr);
}

function remove_proofOfTemp_row(_this, proofOfTemp) {

    $.confirm({
        title: 'Confirmation!',
        content: '<b>Are you sure you want to Delete ?</b> ',
        type: 'red',
        typeAnimated: true,
        buttons: {
            delete: {
                btnClass: 'btn-green',
                action: function () {
                    $.ajax({
                        url: baseUrl + 'Fumigation/Fumigation/DeleteProofOfTemprature',
                        data: { imageId: proofOfTemp },
                        type: "GET",
                        async: false,
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",

                        success: function (data) {


                            if (data.IsSuccess == true) {

                                $(_this).closest("tr").remove();
                            }

                        }
                    });
                }
            },
            cancel: {
                btnClass: 'btn-red',
            }
        }
    })

}


//#region Button Upload Damage Document
var btnUpload = function () {
    $(".btnDamageFile").on("click", function () {

        if (isFormValid('divDamage')) {
            var fileUploader = $("#fuDamageFiles");
            var damageDescription = "";


            // Date Time Format 
            var d = new Date($.now());
            date = (d.getDate() + "-" + (d.getMonth() + 1) + "-" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds());
            //
            var radioValue = $("input[name='rdSelectedRoute']:checked").val();
            if (fileUploader.length) {
                damageDescription = $("#txtDamageDescription").val();

                var filesUploaded = fileUploader[0].files;

                var data = new FormData();
                //for (let i = 0; i < filesUploaded.length; i++) {
                if (radioValue > 0) {

                    var damageFile = $("#fuDamageFiles")[0].files;

                    if (damageFile.length) {
                        for (let i = 0; i < damageFile.length; i++) {
                            data.append("DamageImage", damageFile[i]);
                        }
                    }
                    //}
                    var damageFiles = glbRouteStops.filter(x => x.RouteNo == radioValue);
                    data.append("ImageDescription", damageDescription);
                    data.append("FumigationRouteId", damageFiles[0].FumigationRoutsId);
                    if (damageFiles[0].FumigationRoutsId > 0) {

                        $.ajax({
                            type: "POST",
                            url: baseUrl + '/Fumigation/Fumigation/UploadDamageDocument',
                            contentType: false,
                            processData: false,
                            data: data,
                            async: false,
                            success: function (data, textStatus, jqXHR) {
                                if (data.IsSuccess == true) {
                                    GetFumigationById();

                                }
                                else {
                                    // toastr.warning(data.Message);
                                    AlertPopup(data.Message);

                                }
                                GetFumigationById();

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
                        // toastr.warning("Please click on SUBMIT button to save this route into database.Then you can upload image for this route.");
                        AlertPopup("Please click on SUBMIT button to save this route into database.Then you can upload image for this route.");
                    }
                }
            }

            $("#txtDamageDescription").val("");
            $("#fuDamageFiles").val(null);
            bindDamageFileTbl();
        }
    })
}
//#endregion

function bindDamageFileTbl() {
    isApproveDamageDocument = false;
    var tr = "";
    $("#tblUploadedFiles tbody").empty();
    var radioValue = $("input[name='rdSelectedRoute']:checked").val();
    if (radioValue > 0) {
        var damageFiles = glbDamageFile.filter(x => x.RouteNo == radioValue);
        if (damageFiles.length > 0) {
            for (var i = 0; i < damageFiles.length; i++) {

                tr +=
                    '<tr data-file-url=' + damageFiles[i].ImageUrl + ' ondblclick="javascript: ViewDamageDocument(this,' + damageFiles[i].DamageId + ',' + damageFiles[i].IsApproved + ');">' +
                    //'<td>' + (i + 1) + '</td>' +
                    '<td>' + damageFiles[i].ImageDescription + '</td>' +
                    //'<td>' + damageFiles[i].ImageName + '</td>' +
                    '<td>' + damageFiles[i].Date + '</td>' +
                    '<td><button type="button" class="delete_icon" onclick="remove_damage_document_row(this,' + damageFiles[i].DamageId + ')"> <i class="far fa-trash-alt"></i></button><button type="button" data-file-url=' + damageFiles[i].ImageUrl + ' onclick="ViewDamageDocument(this,' + damageFiles[i].DamageId + ',' + damageFiles[i].IsApproved + ')" class="delete_icon"><i class="far fa-eye"></i></button><a href=' + damageFiles[i].ImageUrl + ' download title="download" class="edit_icon fileDownload" ><i class="fa fa-download"></i></a> &nbsp;'
                if (damageFiles[i].IsApproved) {

                    tr += '<a style="color:green"><i class="fa fa-check"></a></td>'
                } else {
                    isApproveDamageDocument = true;
                    tr += '<a><i class="fa fa-times"></a></td>'

                }
                tr += '</tr>'
            }
        }
    }
    $("#tblUploadedFiles tbody").append(tr);
}

function remove_damage_document_row(_this, damageId) {

    $.confirm({
        title: 'Confirmation!',
        content: '<b>Are you sure you want to Delete ?</b> ',
        type: 'red',
        typeAnimated: true,
        buttons: {
            delete: {
                btnClass: 'btn-green',
                action: function () {
                    $.ajax({
                        url: baseUrl + 'Fumigation/Fumigation/DeleteDamageImage',
                        data: { damageId: damageId },
                        type: "GET",
                        async: false,
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",

                        success: function (data) {


                            if (data.IsSuccess == true) {

                                $(_this).closest("tr").remove();
                            }

                        }
                    });
                }
            },
            cancel: {
                btnClass: 'btn-red',
            }
        }
    })

}


function ViewProofOfTemperature(_this, proofOfTempId, IsApproved) {
    $("#proofOfTempratureId").val(0);
    $("#btnDamageDocument").hide();
    $("#btnApprovedProofOfTemp").hide();
    if (proofOfTempId > 0 && !IsApproved) {
        $("#proofOfTempratureId").val(proofOfTempId);
        $("#btnApprovedProofOfTemp").show();
    }
    OpenModel(_this)
}

function ViewDamageDocument(_this, damageDocumentId, IsApproved) {
    $("#damageDocumentId").val(0);
    $("#btnDamageDocument").hide();
    $("#btnApprovedProofOfTemp").hide();
    if (damageDocumentId > 0 && !IsApproved) {
        $("#damageDocumentId").val(damageDocumentId);
        $("#btnDamageDocument").show();
    }
    OpenModel(_this)
}

//#region View Document
function OpenModel(_this) {


    var fileUrl = $(_this).attr("data-file-url");
    if (fileUrl != undefined) {
        var extn = fileUrl.substring(fileUrl.lastIndexOf('.') + 1);

        var isImg = isExtension(extn, _imgExts);
        var $divViewer = $("#divViewer");
        var docHeight = $(document).height();
        if (isImg) {
            $('.btnPrintImage').show();
            $('.btnPrintPdf').hide();
            //var h = $(window).height();
            ////var imgTag = '<img src="' + fileUrl + '" style="width: 100%; height:' + (docHeight - 800) + 'px" class="img-fluid" />';
            //var imgTag = '<img src="' + fileUrl + '" style="width:auto; height:' + (h - 110) + 'px;" class="img-fluid" />';
            //$divViewer.html(imgTag);

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
                //  toastr.warning('Uploads must be in JPG, JPEG, PNG, PDF, or JFIF formats.');
                AlertPopup('Uploads must be in JPG, JPEG, PNG, PDF, or JFIF formats.');
                $(_this).val(null);
                return Isvalid;
            }
        }
    }
    else {
        //  toastr.warning("Please Browse to select your Upload File.", "")
        AlertPopup("Please Browse to select your Upload File.", "")
        return Isvalid;
    }

}


//#region approved on hold condition
btnApprovedFumigation = function () {

    $("#btnApprovedFumigation").on("click", function () {

        var ddlStatus = $("#ddlStatus option:selected").text();
        if ($("#hdnIsOnhold").val() > 0) {
            if ($.trim(ddlStatus).toLowerCase() == $.trim("Shipment On Hold").toLowerCase() || $.trim(ddlStatus).toLowerCase() == $.trim("IMMEDIATE ATTENTION").toLowerCase()) {


                $.confirm({
                    title: 'Confirmation!',
                    content: '<b>Are you sure you want to approve?</b> ',
                    type: 'blue',
                    typeAnimated: true,
                    buttons: {
                        confirm: {
                            btnClass: 'btn-green',
                            action: function () {
                                var values = {};
                                values.FumigationId = $("#hdnfumigationId").val();
                                values.StatusId = $("#ddlStatus").val();
                                values.SubStatusId = $("#ddlSubStatus").val();
                                values.Reason = $("#txtReason").val();
                                $.ajax({
                                    url: baseUrl + '/Fumigation/Fumigation/ApprovedFumigation',
                                    data: JSON.stringify(values),
                                    type: "Post",
                                    //async: false,
                                    contentType: "application/json; charset=utf-8",
                                    //dataType: "json",
                                    beforeSend: function () {
                                        showLoader();
                                    },
                                    success: function (data) {
                                        hideLoader();
                                        if (data.IsSuccess == true) {

                                            //toastr.success(data.Message);
                                            //setTimeout(function () {
                                            //    GetFumigationById();
                                            //}, 1000)
                                            $.alert({
                                                title: 'Success!',
                                                content: "<b>" + data.Message + "</b>",
                                                type: 'green',
                                                typeAnimated: true,
                                                buttons: {
                                                    Ok: {
                                                        btnClass: 'btn-green',
                                                        action: function () {
                                                            GetFumigationById();
                                                        }
                                                    },
                                                }
                                            });
                                        }

                                        else {
                                            //toastr.error(data.Message);
                                            AlertPopup(data.Message);

                                        }
                                        GetFumigationById();
                                    },
                                    error: function () {
                                        hideLoader();
                                    }
                                });
                            }
                        },
                        cancel: {
                            btnClass: 'btn-red',
                        }
                    }
                })
            }
        }

    })

}
//#endregion

//#region Approve damage document
btnDamageDocument = function () {

    $("#btnDamageDocument").on("click", function () {

        $.confirm({
            title: 'Confirmation!',
            content: '<b>Are you sure want to approve this image?</b> ',
            type: 'blue',
            typeAnimated: true,
            buttons: {
                confirm: {
                    btnClass: 'btn-green',
                    action: function () {

                        var damageId = $("#damageDocumentId").val();
                        $.ajax({
                            url: baseUrl + '/Fumigation/Fumigation/ApprovedDamageImage',
                            data: { damageId: damageId },
                            type: "Get",
                            //async: false,
                            contentType: "application/json; charset=utf-8",
                            //dataType: "json",
                            success: function (data) {
                                $("#modalDocument").modal("hide");
                                //if (data == true) {

                                //    //toastr.success(data.Message);
                                //    setTimeout(function () {
                                //        GetFumigationById();
                                //    }, 1000)
                                //}
                                //else {
                                //    //toastr.error(data.Message);
                                //}
                                GetFumigationById();
                            },
                            error: function () {
                                GetFumigationById();
                            }
                        });
                    }
                },
                cancel: {
                    btnClass: 'btn-red',
                }
            }
        })

    })
}
//#endregion

//#region Approve Proof Of Temprature
btnApprovedProofOfTemp = function () {

    $("#btnApprovedProofOfTemp").on("click", function () {
        $.confirm({
            title: 'Confirmation!',
            content: '<b>Are you sure want to approve this image?</b> ',
            type: 'blue',
            typeAnimated: true,
            buttons: {
                confirm: {
                    btnClass: 'btn-green',
                    action: function () {

                        var tempId = $("#proofOfTempratureId").val();
                        $.ajax({
                            url: baseUrl + '/Fumigation/Fumigation/ApprovedProofOFTemp',
                            data: { proofOfTempratureId: tempId },
                            type: "Get",
                            //async: false,
                            contentType: "application/json; charset=utf-8",
                            //dataType: "json",
                            success: function (data) {
                                $("#modalDocument").modal("hide");
                                //if (data == true) {

                                //    //toastr.success(data.Message);
                                //    //setTimeout(function () {
                                //    //    GetFumigationById();
                                //    //}, 1000)

                                //    //$.alert({
                                //    //    title: 'Success!',
                                //    //    content: "<b>" + data.Message + "</b>",
                                //    //    type: 'green',
                                //    //    typeAnimated: true,
                                //    //    buttons: {
                                //    //        Ok: {
                                //    //            btnClass: 'btn-green',
                                //    //            action: function () {
                                //    //                GetFumigationById();
                                //    //            }
                                //    //        },
                                //    //    }
                                //    //});

                                //}
                                //else {
                                //    //toastr.error(data.Message);
                                //    //AlertPopup(data.Message);
                                //}
                                GetFumigationById();
                            },
                            error: function () {
                                GetFumigationById();
                            }
                        });
                    }
                },
                cancel: {
                    btnClass: 'btn-red',
                }
            }
        })

    })
}
//#endregion

$("table").on("mouseover", 'tr', function () {

    $(this).find(".far").css('color', 'white');
    $(this).find(".fa-download").css('color', 'white');
    $(this).find(".fa-check").css('color', 'white');
});

$("table").on("mouseout", 'tr', function () {

    $(this).find(".far").css('color', '#007bff');
    $(this).find(".fa-trash-alt").css('color', 'red');
    $(this).find(".fa-download").css('color', '#007bff');
    $(this).find(".fa-check").css('color', 'green');
});

/*Drag and drop */

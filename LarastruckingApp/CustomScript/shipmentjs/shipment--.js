

//global array for freight detail
var glbFreightDetail = [];

//global array for route stops
var glbRouteStops = [];

//global function for accessorial chargesbtnAddRouteStop
var glbAccessorialFee = [];

//global Damage Document

var glbDamageFile = [];

//global function for proof temp


var glbProofOfTemprature = [];

//global function to get equipte list with driver
var isNeedToloaded = true;
var equipment = [];

var contactInfoCounts = 0;
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
    $(".divWaitingTime").hide();
    //$(".divProofOfTemp").hide();
    $("#shipmentEnvelop").css("color", "gray");
    //bind shipment status dropdown
    shipmentStatus();
    // bind substatus on changes of status
    ddlStatus();
    //bind sub status dropdown
    ddlSubStatus();

    //Show and hide pricing method field
    HidePricingMethodField();

    //bind accessorial fee UI
    bindAccessorialfeeType();

    //function for open address popup
    openAddressModal();

    //bind customer dropdown
    bindCustomerDropdown();

    //bind pickup location 
    bindPickupLocation();

    //bind delivery location
    bindDeliveryLocation();

    //get quote id
    //getQuoteDetailById();

    //get shipment by id
    getShipmentDetailById();

    //function for bind equipment popup
    openEquipmentModal();

    //function for add route stopes
    btnAddRouteStop();

    //function for add roud trip
    btnAddRoundTrip();
    //function for swap location
    swaplocation();

    //clear route stops
    btnClearRoute();
    //function for add freight detail
    btnAddFreight();

    //clear freight field on button click
    btnClearFreight();

    //accessorial check box
    chkAccessorialchargs();

    //Convert F to C and C to F
    convertTemp();
    //Save Shipment detail
    btnSave();

    //save damage document
    btnUpload();

    //save proof of temp
    btnProofOfTemp();

    //Convert temperature
    convertActualTemp();


    //approved shipment
    btnApprovedShipment();

    checkFreightRadionButton();



    freighttypeOnchange();
    ddlPricingMethod();


    if ($("#hdnShipmentId").val() > 0) {

        $("#divDamageNproofOfTemp").show();
        $(".pricingMethod").show();
        $("#btnGpsShipment").show();
        //$(".divIsPartila").show();

    }
    else {
        $(".divPartial").hide();
        $("#divDamageNproofOfTemp").hide();
        $(".pricingMethod").hide();
        $("#btnGpsShipment").hide();
        // $(".divIsPartila").hide();
    }

    $(".divHazardous").hide();
    $("#shipmentEnvelop").css("font-size", "25px");

    showhideplusminus();

    printPdf();

    printImage();
    //prove Proof Of Temprature
    btnApprovedProofOfTemp();

    //Approve damage document
    btnDamageDocument();
});

//#region function swap location

var swaplocation = function () {

    $("#swapp").on("click", function () {

        var pickupLocationId = $.trim($("#ddlPickupLocation").val());
        var pickupLocationText = $.trim($("#ddlPickupLocation").find('option:selected').text());

        var deliveryLocationId = $.trim($("#ddlDeliveryLocation").val());
        var deliveryLocationText = $.trim($("#ddlDeliveryLocation").find('option:selected').text());

        if (pickupLocationId > 0) {
            var ddlpickup = "<option selected='selected' value=" + deliveryLocationId + ">" + deliveryLocationText + "</option>";
            $("#ddlPickupLocation").empty();
            $("#ddlPickupLocation").append(ddlpickup);
            $(".ddlPickupLocation").text(deliveryLocationText);
        }
        if (deliveryLocationId > 0) {
            var ddldelivery = "<option  selected='selected' value=" + pickupLocationId + ">" + pickupLocationText + "</option>";
            $("#ddlDeliveryLocation").empty();
            $("#ddlDeliveryLocation").append(ddldelivery);
            $(".ddlDeliveryLocation").text(pickupLocationText);
        }
    })
}

//#endregion

//#region for open equipment popup
var openEquipmentModal = function () {
    $("#btnequipment").on("click", function () {

        showLoader();
        var glbRouteStop = glbRouteStops.filter(x => x.IsAppointmentNeeded == true && x.IsAppointmentRequired == false)
        if (glbRouteStop.length > 0) {

            $.alert({
                title: 'Alert!',
                content: '<b>Please confirm appointment.</b>',
                type: 'red',
                typeAnimated: true,
            });
            // $.alert("Please confirm appointment.")

        }
        else {
            if ($("#dtRouteBody tr").length > 0) {
                $("#modalEquipment").modal("show");
                $('#modalEquipment').draggable();
            }
            else {
                $.alert({
                    title: 'Alert!',
                    content: '<b>Please add Route Details before assigning drivers and equipment.</b>',
                    type: 'red',
                    typeAnimated: true,
                });
                // $.alert("Please add Route Details before assigning drivers and equipment.")
            }
        }
        hideLoader();
    });
}
//#endregion


//#region for open address popup
var openAddressModal = function () {
    $(".btnOpenAddressModal").on("click", function () {
        $('#formAddress').trigger("reset");

        $("#modalAddAddress").modal("show");
        $('#modalAddAddress').draggable();
    });
}
//#endregion

//#region shipment status
function shipmentStatus() {
    $.ajax({
        url: baseUrl + 'Shipment/Shipment/GetShipmentStatus',
        data: {},
        type: "GET",
        async: false,
        success: function (data) {

            var ddlValue = "";
            $("#ddlStatus").empty();
            for (var i = 0; i < data.length; i++) {
                ddlValue += '<option value="' + data[i].StatusId + '">' + $.trim(data[i].StatusName).toUpperCase() + '</option>';
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

        var equipment = $("#txtEquipment").val();
        var driver = $("#txtdriver").val();

        if (statusid != 8 && statusid != 1 && (equipment == "" && driver == "")) {

            $.alert({
                title: 'Alert!',
                content: '<b>Please select equipment and driver first.</b>',
                type: 'red',
                typeAnimated: true,
            });
            shipmentStatus();
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
        url: baseUrl + 'Shipment/Shipment/GetShipmentSubStatus',
        data: { statusid: statusid },
        type: "GET",
        async: false,
        // cache: false,
        success: function (data) {
            var ddlValue = "";
            $("#ddlSubStatus").empty();
            ddlValue += '<option value="">SELECT SUB-STATUS</option>'
            for (var i = 0; i < data.length; i++) {
                ddlValue += '<option value="' + data[i].SubStatusId + '">' + $.trim(data[i].SubStatusName).toUpperCase() + '</option>';
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

    if ($("#ddlSubStatus").val() == 11 || $("#ddlSubStatus").val() == 7) {
        $("#txtReason").prop('disabled', false);
    }
    else {
        $("#txtReason").val("");
        $("#txtReason").prop('disabled', true);
    }
}
//#endregion

//#region function for apply selectize on customer dropdown
var bindCustomerDropdown = function () {

    var $select = $('#ddlCustomer').selectize();
    $select[0].selectize.destroy();

    $('#ddlCustomer').selectize({
        //createOnBlur: true,
        sortField: 'text',
        maxItems: 1,
        valueField: 'id',
        labelField: 'text',
        searchField: 'text',
        plugins: ['restore_on_backspace'],
        //highlight: true,
        closeAfterSelect: false,
        selectOnTab: true,
        allowEmptyOption: true,
        options: [],
        //onType: function (value) {
        //    searchBoxHasValue = true;
        //    if (value == null || value == undefined || value == '') {
        //        searchBoxHasValue = false;
        //        var $options = $('.option', this.$dropdown);
        //        this.setActiveOption($($options[0]));
        //    }

        //},
        load: function (query, callback) {
            if (!query.length) return callback();
            $.ajax({
                url: baseUrl + "Customer/GetAllCustomer/?searchText=" + query,
                type: 'GET',
                dataType: 'json',
                //beforeSend: function (xhr, settings) {
                //},
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
                        item.IsWaitingTimeRequired = value.IsWaitingTimeRequired;
                        item.ContactInfoCount = value.ContactInfoCount;
                        customers.push(item);
                    });

                    callback(customers);
                },
                //complete: function () {
                //}
            });
        },
        render: {
            item: function (item, escape) {
                return '<div>' +
                    ('<span class="name ddlCustomer" data-ContactInfoCount=' + item.ContactInfoCount + ' date-IsWaitingTimeRequired=' + item.IsWaitingTimeRequired + '>' + escape(item.text) + '</span>') +
                    '</div>';
            },
            option: function (item, escape) {
                var label = item.text;
                return '<div style="padding: 2px 5px">' +
                    '<span style="display: block;">' + escape(label) + '</span>' +
                    '</div>';
            }
        },
        //create: function (input, callback) {

        //    $('#ddlCustomer').html("");
        //    $('#ddlCustomer').append($("<option selected='selected'></option>").val(input).html(input))
        //},
        onFocus: function () {

            var value = this.getValue();
            this.clear(true);
            this.unlock();
        },
        onChange: function () {
            
            var IsWaitingTimeRequired = $(".ddlCustomer").attr("date-IsWaitingTimeRequired");
            if (JSON.parse(IsWaitingTimeRequired) == true) {
                $('#chkDeliveryWaitingTime').prop('checked', true);
                $('#chkPickUpWaitingTime').prop('checked', true);
                $(".divWaitingTime").show();
            }
            else {
                $('#chkDeliveryWaitingTime').prop('checked', false);
                $('#chkPickUpWaitingTime').prop('checked', false);
                $(".divWaitingTime").hide();
            }
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
                        item.IsAppointmentRequired = value.IsAppointmentRequired;
                        item.Phone = value.Phone;
                        item.Website = value.Website;
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
                    ("<span data-website='" + escape(item.Website) + "' data-phone='" + escape(item.Phone) + "'  data-IsAppointmentRequired='" + escape(item.IsAppointmentRequired) + "' class='" + htmlcontrol + "'>" + escape(item.text) + "</span>") +
                    '</div>';
            },
            option: function (item, escape) {
                var label = item.text;
                var IsAppointmentRequired = item.IsAppointmentRequired;
                return '<div style="padding: 2px 5px">' +
                    '<span  style="display: block;">' + escape(label) + '</span>' +
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

//#region function for pickup location dropdown
var bindPickupLocation = function () {
    manageLocation("ddlPickupLocation");
}
//#endregion

//#region function for drop location dropdown
var bindDeliveryLocation = function () {
    manageLocation("ddlDeliveryLocation");
}
//#endregion

//#region validate date 

function ValidatePickUpArrivalDate() {
    
    var isvalid = true;
    var pickupArrival = $("#dtArrivalDate").val();
    var deliverArrival = $("#dtDeliveryDate").val();

    var todayDate = new Date();
    var month = todayDate.getMonth() + 1;
    var day = todayDate.getDate() - 1;

    var yesterday = "";
    yesterday = (month < 10 ? '0' : '') + month + '-' +
        (day < 10 ? '0' : '') + day + '-' +
        todayDate.getFullYear();

    //yesterday = new Date(Date.parse(todayDate));

    if (pickupArrival != "" ) {

       // AlertPopup("Please review your Pickup Est. Arrival. It should be greater than, or equal to, yesterday's date.");
        isvalid = true;
        return isvalid
    }


    if (isvalid && pickupArrival != "" && deliverArrival != "") {
        if (new Date(pickupArrival) <= new Date(deliverArrival)) {
            return isvalid
        }
        else {
            //$("#dtArrivalDate").val("");

            AlertPopup("Please review your Delivery Est. Arrival. It should be greater than your Pickup Est. Arrival.");
            isvalid = false;
            return isvalid
        }

    }
    return isvalid
}

function ValidatePickUpDepartureDate() {
    var isvalid = true;
    var pickDeparture = $("#dtPickUpToDate").val();
    var deliveryDeparture = $("#dtDeliveryToDate").val();

    var todayDate = new Date();
    var month = todayDate.getMonth() + 1;
    var day = todayDate.getDate() - 1;

    var yesterday = "";
    yesterday = (month < 10 ? '0' : '') + month + '-' +
        (day < 10 ? '0' : '') + day + '-' +
        todayDate.getFullYear();

    //yesterday = new Date(Date.parse(todayDate));

    if (pickDeparture != "") {

        ///AlertPopup("Please review your Pickup Est. Departure. It should be greater than, or equal to, yesterday's date.");
        isvalid = true;
        return isvalid
    }


    if (isvalid && pickDeparture != "" && deliveryDeparture != "") {
        if (new Date(pickDeparture) <= new Date(deliveryDeparture)) {
            return isvalid;
        }
        else {
            //$("#dtPickUpToDate").val("");
            AlertPopup("Please review your Delivery Est. Departure. It should be greater than your Pickup Est. Departure.");
            isvalid = false;

        }
    }
    return isvalid;
}
function ValidateDeliveryArrivalDate() {
    var isvalid = true;
    var pickupArrival = $("#dtArrivalDate").val();
    var deliverArrival = $("#dtDeliveryDate").val();
    if (pickupArrival != "" && deliverArrival != "") {
        if (new Date(pickupArrival) < new Date(deliverArrival)) {

        }
        else {
            $("#dtDeliveryDate").val("");
            AlertPopup("Please select valid date.");
        }
    }
}


function ValidateDeliveryDepartureDate() {
    var pickDeparture = $("#dtPickUpToDate").val();
    var deliveryDeparture = $("#dtDeliveryToDate").val();
    if (pickDeparture != "" && deliveryDeparture != "") {
        if (new Date(pickDeparture) < new Date(deliveryDeparture)) {

        }
        else {
            $("#dtDeliveryToDate").val("");
            AlertPopup("Please select valid date.");
        }
    }
}

//#endregion


//#region get shipment detail detail by id for edit
getShipmentDetailById = function () {

    var url = window.location.pathname;
    var shipmentId = url.substring(url.lastIndexOf('/') + 1);
    if (shipmentId > 0) {
        $.ajax({
            url: baseUrl + "/Shipment/Shipment/GetShipmentById",
            type: "GET",
            data: { shipmentId: shipmentId },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                if (response != null) {


                    //global array for freight detail
                    glbFreightDetail = [];

                    //global array for route stops
                    glbRouteStops = [];

                    //global function for accessorial charges
                    glbAccessorialFee = [];

                    //global Damage Document

                    glbDamageFile = [];

                    //global function for proof temp

                    glbProofOfTemprature = [];

                    //global function to get equipte list with driver
                    equipment = [];
                    //#region bind shipment detail

                    $("#hdnShipmentId").val(response.ShipmentId);
                    contactInfoCounts = response.ContactInfoCount;

                    if (response.IsWaitingTimeRequired == true) {
                        $('#chkDeliveryWaitingTime').prop('checked', true);
                        $('#chkPickUpWaitingTime').prop('checked', true);
                        $(".divWaitingTime").show();
                    }
                    else {
                        $('#chkDeliveryWaitingTime').prop('checked', false);
                        $('#chkPickUpWaitingTime').prop('checked', false);
                        $(".divWaitingTime").hide();
                    }
                    var ddlCustomer = "<option selected='selected' value=" + response.CustomerId + ">" + response.CustomerName + "</option>";
                    $("#ddlCustomer").empty();
                    $("#ddlCustomer").append(ddlCustomer);
                    $(".ddlCustomer").text(response.CustomerName);
                    $(".ddlCustomer").attr("date-IsWaitingTimeRequired", response.IsWaitingTimeRequired);

                    $("#txtRequestedBy").val(response.RequestedBy);
                    $("#txtReason").val(response.Reason);
                    $("#ddlStatus").val(response.StatusId);
                    $("#txtVendorNConsignee").val(response.VendorNconsignee);


                    if (response.IsOnHold) {
                        $("#shipmentEnvelop").css("color", "red");
                    }
                    else {
                        $("#shipmentEnvelop").css("color", "gray");
                    }
                    bindSubStauts();

                    $("#ddlSubStatus").val(response.SubStatusId);
                    $("#txtShipRefNo").val(response.ShipmentRefNo);
                    $("#txtShipRefNo").val(response.ShipmentRefNo);
                    $("#txtAirWayBill").val(response.AirWayBill);
                    $("#txtCustomerPO").val(response.CustomerPO);
                    $("#txtOrderNo").val(response.OrderNo);
                    $("#txtCustomerRefNo").val(response.CustomerRef);
                    $("#txtContainerNo").val(response.ContainerNo);
                    $("#txtPurchaseDoc").val(response.PurchaseDoc);
                    // $("#txtDriverInstruction").val(response.DriverInstruction);
                    checkOtherStaus();
                    //#endregion

                    //#region for bind route stops
                    for (var i = 0; i < response.ShipmentRoutesStop.length; i++) {

                        var objRouteStops = {};
                        objRouteStops.ShipmentRouteStopId = response.ShipmentRoutesStop[i].ShipmentRouteStopId;
                        objRouteStops.RouteNo = response.ShipmentRoutesStop[i].RouteNo;
                        objRouteStops.PickupLocationId = response.ShipmentRoutesStop[i].PickupLocationId;
                        objRouteStops.PickupLocationText = response.ShipmentRoutesStop[i].PickupLocation;
                        objRouteStops.DeliveryLocationId = response.ShipmentRoutesStop[i].DeliveryLocationId;
                        objRouteStops.DeliveryLocationText = response.ShipmentRoutesStop[i].DeliveryLocation;
                        objRouteStops.PickDateTime = response.ShipmentRoutesStop[i].PickDateTime == null ? "" : ConvertDateEdit(response.ShipmentRoutesStop[i].PickDateTime, true);
                        objRouteStops.PickDateTimeTo = response.ShipmentRoutesStop[i].PickDateTimeTo == null ? "" : ConvertDateEdit(response.ShipmentRoutesStop[i].PickDateTimeTo, true);

                        objRouteStops.DeliveryDateTime = response.ShipmentRoutesStop[i].DeliveryDateTime == null ? "" : ConvertDateEdit(response.ShipmentRoutesStop[i].DeliveryDateTime, true);
                        objRouteStops.DeliveryDateTimeTo = response.ShipmentRoutesStop[i].DeliveryDateTimeTo == null ? "" : ConvertDateEdit(response.ShipmentRoutesStop[i].DeliveryDateTimeTo, true);
                        //objRouteStops.Comment = response.ShipmentRoutesStop[i].Comment;
                        objRouteStops.IsDeleted = response.ShipmentRoutesStop[i].IsDeleted;
                        objRouteStops.ActPickupArrival = response.ShipmentRoutesStop[i].ActPickupArrival == null ? "" : ConvertDate(response.ShipmentRoutesStop[i].ActPickupArrival, true);
                        objRouteStops.ActPickupDeparture = response.ShipmentRoutesStop[i].ActPickupDeparture == null ? "" : ConvertDate(response.ShipmentRoutesStop[i].ActPickupDeparture, true);
                        objRouteStops.ActDeliveryArrival = response.ShipmentRoutesStop[i].ActDeliveryArrival == null ? "" : ConvertDate(response.ShipmentRoutesStop[i].ActDeliveryArrival, true);
                        objRouteStops.ActDeliveryDeparture = response.ShipmentRoutesStop[i].ActDeliveryDeparture == null ? "" : ConvertDate(response.ShipmentRoutesStop[i].ActDeliveryDeparture, true);
                        objRouteStops.ReceiverName = response.ShipmentRoutesStop[i].ReceiverName;
                        objRouteStops.DigitalSignature = response.ShipmentRoutesStop[i].DigitalSignature;

                        objRouteStops.IsAppointmentNeeded = response.ShipmentRoutesStop[i].IsAppointmentNeeded;
                        objRouteStops.IsAppointmentRequired = response.ShipmentRoutesStop[i].IsAppointmentRequired;
                        objRouteStops.Website = response.ShipmentRoutesStop[i].Website;
                        objRouteStops.Phone = response.ShipmentRoutesStop[i].Phone;

                        objRouteStops.IsWaitingTimeNeeded = response.ShipmentRoutesStop[i].IsWaitingTimeNeeded;
                        objRouteStops.IsPickUpWaitingTimeRequired = response.ShipmentRoutesStop[i].IsPickUpWaitingTimeRequired;
                        objRouteStops.IsDeliveryWaitingTimeRequired = response.ShipmentRoutesStop[i].IsDeliveryWaitingTimeRequired;

                        glbRouteStops.push(objRouteStops);

                    }

                    bindRoutetable();
                    checkRadionButton();
                    if (response.ShipmentRoutesStop.length == 1) {
                        edit_route_stops(0);
                    }
                    //#endregion

                    //#region for bind shipment detail

                    for (var i = 0; i < response.ShipmentFreightDetail.length; i++) {

                        var objFreightDetail = {};
                        objFreightDetail.FreightDetailId = response.ShipmentFreightDetail[i].ShipmentFreightDetailId;
                        objFreightDetail.RouteNo = response.ShipmentFreightDetail[i].RouteNo;
                        objFreightDetail.PickupLocationId = response.ShipmentFreightDetail[i].PickupLocationId;
                        objFreightDetail.DeliveryLocationId = response.ShipmentFreightDetail[i].DeliveryLocationId;
                        objFreightDetail.Commodity = response.ShipmentFreightDetail[i].Commodity;


                        objFreightDetail.freighttypetext = response.ShipmentFreightDetail[i].FreightType;
                        objFreightDetail.FreightTypeId = response.ShipmentFreightDetail[i].FreightTypeId;

                        objFreightDetail.pricingmethodtext = response.ShipmentFreightDetail[i].PricingMethod;
                        objFreightDetail.PricingMethodId = response.ShipmentFreightDetail[i].PricingMethodId;



                        objFreightDetail.Hazardous = response.ShipmentFreightDetail[i].Hazardous;
                        objFreightDetail.Temperature = response.ShipmentFreightDetail[i].Temperature;
                        objFreightDetail.MinFee = response.ShipmentFreightDetail[i].MinFee == null ? "" : response.ShipmentFreightDetail[i].MinFee;
                        objFreightDetail.UpTo = response.ShipmentFreightDetail[i].UpTo == null ? "" : response.ShipmentFreightDetail[i].UpTo;

                        objFreightDetail.QutWgtVlm = response.ShipmentFreightDetail[i].QutWgtVlm;
                        objFreightDetail.UnitPrice = response.ShipmentFreightDetail[i].UnitPrice == null ? "" : response.ShipmentFreightDetail[i].UnitPrice;

                        objFreightDetail.TotalPrice = response.ShipmentFreightDetail[i].TotalPrice == null ? "" : response.ShipmentFreightDetail[i].TotalPrice;

                        objFreightDetail.NoOfBox = response.ShipmentFreightDetail[i].NoOfBox == null ? "" : response.ShipmentFreightDetail[i].NoOfBox;
                        objFreightDetail.Weight = response.ShipmentFreightDetail[i].Weight == null ? "" : response.ShipmentFreightDetail[i].Weight;
                        objFreightDetail.Unit = response.ShipmentFreightDetail[i].Unit == null ? "" : response.ShipmentFreightDetail[i].Unit;
                        objFreightDetail.PcsType = response.ShipmentFreightDetail[i].PcsType == null ? "" : response.ShipmentFreightDetail[i].PcsType;
                        objFreightDetail.PcsTypeId = response.ShipmentFreightDetail[i].PcsTypeId == null ? "" : response.ShipmentFreightDetail[i].PcsTypeId;
                        objFreightDetail.Comments = response.ShipmentFreightDetail[i].Comments;
                        objFreightDetail.IsDeleted = false;
                        objFreightDetail.TrailerCount = response.ShipmentFreightDetail[i].TrailerCount;
                        objFreightDetail.IsPartialShipment = response.ShipmentFreightDetail[i].IsPartialShipment;
                        objFreightDetail.PartialBox = response.ShipmentFreightDetail[i].PartialBox;
                        objFreightDetail.PartialPallet = response.ShipmentFreightDetail[i].PartialPallet;
                        glbFreightDetail.push(objFreightDetail);
                    }

                    bindshipmenttable();
                    checkFreightRadionButton();

                    //function for bind pricing method dropdown list
                    GetPricingMethod();

                    //function for bind for freight type dropdown list.
                    GetFreightType();

                    if (response.ShipmentRoutesStop.length == 1) {
                        edit_from_shipment(0);
                    }

                    //#endregion


                    //#region for bing accessorialprice
                    for (var i = 0; i < response.AccessorialPrice.length; i++) {

                        var objaccessorialcharges = {};
                        objaccessorialcharges.ShipmentAccessorialPriceId = response.AccessorialPrice[i].ShipmentAccessorialPriceId;
                        objaccessorialcharges.RouteNo = response.AccessorialPrice[i].RouteNo;
                        objaccessorialcharges.AccessorialFeeTypeId = response.AccessorialPrice[i].AccessorialFeeTypeId;
                        objaccessorialcharges.FeeType = response.AccessorialPrice[i].FeeType;
                        objaccessorialcharges.Amount = response.AccessorialPrice[i].Amount;
                        objaccessorialcharges.AmtPerUnit = response.AccessorialPrice[i].AmtPerUnit;
                        objaccessorialcharges.Unit = response.AccessorialPrice[i].Unit;
                        objaccessorialcharges.IsDeleted = false;
                        objaccessorialcharges.Reason = response.AccessorialPrice[i].Reason;
                        glbAccessorialFee.push(objaccessorialcharges);
                        //Bind accessorial charges from quote
                    }
                    getAccessorialPrice();
                    //#endregion

                    //#region for binding damage Image
                    for (let i = 0; i < response.DamageImages.length; i++) {
                        var damageFile = {};
                        damageFile.DamageId = response.DamageImages[i].DamageId;
                        damageFile.ShipmentRouteStopId = response.DamageImages[i].ShipmentRouteStopId;
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
                        proofOFtemp.FreightDetailId = response.ProofOfTemprature[i].FreightDetailId;
                        proofOFtemp.ImageUrl = response.ProofOfTemprature[i].ImageUrl;
                        proofOFtemp.ActualTemperature = response.ProofOfTemprature[i].ActualTemperature;
                        proofOFtemp.ImageName = response.ProofOfTemprature[i].ImageName;
                        proofOFtemp.Date = ConvertDate(response.ProofOfTemprature[i].CreatedOn, true);
                        proofOFtemp.IsApproved = response.ProofOfTemprature[i].IsApproved;
                        proofOFtemp.IsLoading = response.ProofOfTemprature[i].IsLoading;
                        glbProofOfTemprature.push(proofOFtemp);
                    }
                    bindProofOfTempTbl();
                    //#endregion
                    
                    //#region for binding equipment and driver
                    for (let i = 0; i < response.ShipmentEquipmentNdriver.length; i++) {
                        
                        var equipmentNdriver = {};
                        equipmentNdriver.ShipmentEquipmentNDriverId = response.ShipmentEquipmentNdriver[i].ShipmentEquipmentNDriverId;
                        equipmentNdriver.EquipmentId = response.ShipmentEquipmentNdriver[i].EquipmentId;
                        equipmentNdriver.EquipmentName = response.ShipmentEquipmentNdriver[i].EquipmentName;
                        equipmentNdriver.DriverId = response.ShipmentEquipmentNdriver[i].DriverId;
                        equipmentNdriver.DriverName = response.ShipmentEquipmentNdriver[i].DriverName;
                        equipment.push(equipmentNdriver);
                    }

                    getEquipmentNDrier();
                    GetEquipmentList();
                    //#endregion

                    var commentList = "";
                    $("#tblShipmentComments").empty();
                    if (response.ShipmentComments.length > 0) {
                        for (var i = 0; i < response.ShipmentComments.length; i++) {

                            commentList += '<tr><td>' + response.ShipmentComments[i].comment + '</td></tr>'

                        }
                    }

                    //$("#divShipmentComment").html(commentList);
                    //$("#divShipmentComment label").attr("display", "contents");
                    //$("#divShipmentComment label").attr("margin-bottom", "0rem");
                    $("#tblShipmentComments").append(commentList);

                }
            },
            error: function () {

            }
        });
    }

}
//#endregion



//#region approved on hold condition
btnApprovedShipment = function () {

    $("#btnApprovedShipment").on("click", function () {

        var ddlStatus = $("#ddlStatus option:selected").text();
        if ($.trim(ddlStatus).toLowerCase() == $.trim("Shipment On Hold").toLowerCase() || $.trim(ddlStatus).toLowerCase() == $.trim("IMMEDIATE ATTENTION").toLowerCase()) {

            $.confirm({
                title: 'Confirmation!',
                content: '<b>Are you sure you want to approve?</b> ',
                type: 'red',
                typeAnimated: true,
                buttons: {
                    confirm: {
                        btnClass: 'btn-blue',
                        action: function () {
                            var values = {};
                            values.ShipmentId = $("#hdnShipmentId").val();
                            values.StatusId = $("#ddlStatus").val();
                            values.SubStatusId = $("#ddlSubStatus").val();
                            values.Reason = $("#txtReason").val();
                            $.ajax({
                                url: baseUrl + '/Shipment/Shipment/ApprovedShipment',
                                data: JSON.stringify(values),
                                type: "Post",
                                beforeSend: function () {
                                    showLoader();
                                },
                                //async: false,
                                contentType: "application/json; charset=utf-8",
                                //dataType: "json",
                                success: function (data) {
                                    hideLoader();
                                    if (data.IsSuccess == true) {

                                        $.alert({
                                            title: 'Success!',
                                            content: "<b>" + data.Message + "</b>",
                                            type: 'green',
                                            typeAnimated: true,
                                            buttons: {
                                                Ok: {
                                                    btnClass: 'btn-green',
                                                    action: function () {
                                                        getShipmentDetailById();
                                                    }
                                                },
                                            }
                                        });
                                    }
                                    else {
                                        AlertPopup(data.Message);
                                    }
                                },
                                error: function () {
                                    hideLoader();

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
    })

}
//#end region

//#region bind route 
bindRoutetable = function () {

    var dtRouteBody = "";
    $("#dtRouteBody").empty();
    var sequenceNo = 0;
    for (var i = 0; i < glbRouteStops.length; i++) {
        if (!glbRouteStops[i].IsDeleted) {
            sequenceNo = sequenceNo + 1;

            var pickuplocation = $.parseHTML(glbRouteStops[i].PickupLocationText);
            var deliverylcocation = $.parseHTML(glbRouteStops[i].DeliveryLocationText);

            dtRouteBody += "<tr ondblclick='javascript: edit_route_stops(" + i + ");'>" +
                "<td> <input type='radio' name='rdSelectedRoute' onchange='checkdata()' value='" + glbRouteStops[i].RouteNo + "' /> </td>" +
                "<td><label name='lblSrNo'>" + sequenceNo + "</label> </td>" +
                "<td>" +
                "<label name='lblPickupLocationId' style='display:none'>" + glbRouteStops[i].PickupLocationId + "</label>" +
                '<label data-toggle="tooltip" data-placement="top" title="' + GetAddress(glbRouteStops[i].PickupLocationText) + '">' + GetCompanyName(glbRouteStops[i].PickupLocationText) + '</label>' +
                "</td>" +
                "<td>" +
                "<label name='lblPickupDate'>" + glbRouteStops[i].PickDateTime + "</label>" +
                "</td>" +
                "<td>" +
                "<label name='lblDeliveryLocationId' style='display:none'>" + glbRouteStops[i].DeliveryLocationId + "</label>"
            if (glbRouteStops[i].IsAppointmentNeeded == true && glbRouteStops[i].IsAppointmentRequired == false) {
                dtRouteBody += '<label data-toggle="tooltip" style="color:red" data-placement="top" title="' + GetAddress(glbRouteStops[i].DeliveryLocationText) + '">' + GetCompanyName(glbRouteStops[i].DeliveryLocationText) + '</label>'
            }
            else {
                dtRouteBody += '<label data-toggle="tooltip" data-placement="top" title="' + GetAddress(glbRouteStops[i].DeliveryLocationText) + '">' + GetCompanyName(glbRouteStops[i].DeliveryLocationText) + '</label>'
            }
            dtRouteBody += "</td>" +
                "<td>" +
                "<label name='lblDeliveryDate'>" + glbRouteStops[i].DeliveryDateTime + "</label>" +
                "</td>" +
                "<td>" +
                "<label name='lblDeliveryDate'>" + glbRouteStops[i].ActPickupArrival + "</label>" +
                "</td>" +
                "<td>" +
                "<label name='lblDeliveryDate'>" + glbRouteStops[i].ActPickupDeparture + "</label>" +
                "</td>" +
                "<td>" +
                "<label name='lblDeliveryDate'>" + glbRouteStops[i].ActDeliveryArrival + "</label>" +
                "</td>" +
                "<td>" +
                "<label name='lblDeliveryDate'>" + glbRouteStops[i].ActDeliveryDeparture + "</label>" +
                "</td>" +
                "<td>" +
                "<button type='button' class='edit_icon' onclick='ShowSignaturePopUp(" + i + ")'> <i class='far far fa-eye'></i> </button>| <button type = 'button' class='edit_icon' onclick = 'edit_route_stops(" + i + ")' > <i class='far fa-edit'></i> </button >|<button type='button' class='delete_icon' onclick='remove_row_from_route(this)'> <i class='far fa-trash-alt'></i> </button>" +
                "</td>" +
                "</tr>";
        }
    }
    $("#dtRouteBody").append(dtRouteBody);
}
//#endregion

//#region bind shipment table

function bindshipmenttable() {

    var shipmenttabledata = "";
    var radioValue = $("input[name='rdSelectedRoute']:checked").val();
    var srno = 1;
    var totalPrice = 0;
    var shipmentId = $("#hdnShipmentId").val()
    $("#tblShipmentDetail tbody").empty();
    for (var i = 0; i < glbFreightDetail.length; i++) {
        if (radioValue == glbFreightDetail[i].RouteNo && glbFreightDetail[i].IsDeleted == false) {

            var Qty = "";
            if (glbFreightDetail[i].QutWgtVlm > 0) {
                Qty = glbFreightDetail[i].QutWgtVlm + " Pallet, ";
            }
            if (glbFreightDetail[i].NoOfBox > 0) {
                Qty += glbFreightDetail[i].NoOfBox + " Box, ";
            }
            if (glbFreightDetail[i].Weight != "" || glbFreightDetail[i].Weight != "") {
                Qty += glbFreightDetail[i].Weight + " " + glbFreightDetail[i].Unit + ",";
            }
            if (glbFreightDetail[i].TrailerCount > 0) {
                Qty += glbFreightDetail[i].TrailerCount + " Trailer";
            }
            Qty = Qty.replace(/(^\s*,)|(,\s*$)/g, '');

            shipmenttabledata += "<tr align='center' ondblclick='javascript: edit_from_shipment(" + i + ");'>"
                + "<td><input type='radio' name='rdSelectedFreight' onchange='checkFreight(" + glbFreightDetail[i].FreightDetailId + ")' value=" + glbFreightDetail[i].FreightDetailId + " /></td>"
                + "<td><label name='serialno'>" + srno + "</label></td>"
                //+ "<td><label name = 'pickNdropRadiobtValue'>" + glbFreightDetail[i].RouteNo + "</label></td>"
                + "<td><label name='commoditytext'>" + glbFreightDetail[i].Commodity + "</label></td>"
                + "<td><label name='freighttypetext'>" + glbFreightDetail[i].freighttypetext + "</label></td>"
                + "<td><label name='pricingmethodtext'>" + glbFreightDetail[i].pricingmethodtext + "</label></td>"
            if (IsPricingMethod == true && shipmentId > 0) {
                shipmenttabledata += "<td  align='right'><label name='minfee'>" + glbFreightDetail[i].MinFee + "</label></td>"
                    + "<td  align='right'><label name='addfee'>" + glbFreightDetail[i].UnitPrice + "</label></td>"
                    + "<td  align='right'><label name='upto'>" + glbFreightDetail[i].UpTo + "</label></td>"
            }
            shipmenttabledata += "<td  align='right'>" + Qty + "</td>"


            if (IsPricingMethod == true && shipmentId > 0) {
                shipmenttabledata += "<td  align='right'><label name='totalprice'>" + glbFreightDetail[i].TotalPrice + "</label></td>"

            }
            shipmenttabledata += "<td><button type='button' class='edit_icon' onclick='edit_from_shipment(" + i + ")'> <i class='far fa-edit'></i> </button>|<button type='button' class='delete_icon' onclick='remove_row_from_shipment(" + i + ")'> <i class='far fa-trash-alt'></i> </button></td>"
                + "</tr>"
            srno = srno + 1;
            totalPrice = parseFloat(totalPrice) + parseFloat(glbFreightDetail[i].TotalPrice);
        }
    }


    $("#tblShipmentDetail tbody").append(shipmenttabledata);

}
//#endregion

//#region check freight type dropdown
function checkFreight(shipmentFreightDetailId) {
    bindProofOfTempTbl();
}
//#endregion

//#region bind accessorial fee type
function bindAccessorialfeeType() {
    $.ajax({
        url: baseUrl + 'Shipment/Shipment/GetAccessorialFeeType',
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
                        '<label class="col-sm-5 col-form-label" for="txtLoadingPerUnit">' + data[i].Name + '</label><input class="col-md-3 form-control txtReason" style="padding-left:0px;" readonly=readonly onfocusout="btnCalculatetotalFee()" placeholder= " DESCRIPTION"  id="txtReason_' + data[i].Id + '" type="text"/>' +
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

//#region select default first radion button
function checkRadionButton() {
    if ($("#dtRouteBody tr").length > 0) {
        $($("#dtRouteBody tr")[0]).find("input[type=radio]").prop("checked", true);
        //function for bind pricing method dropdown list
        GetPricingMethod();

        //function for bind for freight type dropdown list.
        GetFreightType();
        //bind damage document
        bindDamageFileTbl();

        checkFreightRadionButton();
    }
}
//#endregion

//#region select default first freight radion button
function checkFreightRadionButton() {

    if ($("#tblShipmentDetail tbody tr").length > 0) {
        $($("#tblShipmentDetail tbody tr")[0]).find("input[type=radio]").prop("checked", true);
        bindProofOfTempTbl();
    }
}
//#endregion

//#region show and hide shipment detail on the baseis of 
function checkdata() {

    //function for bind pricing method dropdown list
    GetPricingMethod();

    //function for bind for freight type dropdown list.
    GetFreightType();

    //bind shipment table
    bindshipmenttable();

    if (glbAccessorialFee.length > 0) {
        getAccessorialPrice();
    }

    //bind damage document
    bindDamageFileTbl();

    $("#tblProofOfTemp tbody").empty();
    checkFreightRadionButton();
}
//#endregion


function CustomerRouteDetail() {
    var radioValue = $("input[name='rdSelectedRoute']:checked").val();
    var values = {};
    if (radioValue > 0) {
        var selectedRouteStop = glbRouteStops.filter(x => x.RouteNo == radioValue);
        if (selectedRouteStop.length > 0) {
            var data = selectedRouteStop[0];
            values.CustomerId = $("#ddlCustomer").val();
            values.PickupLocationId = data.PickupLocationId;
            values.DeliveryLocationId = data.DeliveryLocationId;
            values.PickupArrivalDate = data.PickDateTime;
        }
    }
    return values;
}

//#region bind freight type dropdownlist
function GetFreightType() {

    var values = CustomerRouteDetail();
    if (values != null) {
        $.ajax({
            url: baseUrl + 'Shipment/Shipment/GetFreightType',
            data: JSON.stringify(values),
            type: "Post",
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                if (ValidateRouteStops()) {
                    $.ajax({
                        url: baseUrl + 'Quote/Quote/GetFreightType',
                        type: "Get",
                        async: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {

                            var ddlResultValue = "";
                            $("#ddlFreightType").empty();
                            ddlResultValue += '<option value="0">SELECT FREIGHT TYPE</option>'
                            for (var i = 0; i < result.length; i++) {
                                if (data[0].FreightTypeId == result[i].FreightTypeId) {
                                    ddlResultValue += '<option selected="selected" value="' + result[i].FreightTypeId + '">' + result[i].FreightTypeName + '</option>';
                                }
                                else {
                                    ddlResultValue += '<option value="' + result[i].FreightTypeId + '">' + result[i].FreightTypeName + '</option>';
                                }
                            }
                            $("#ddlFreightType").append(ddlResultValue);
                        }
                    });
                }
                else {
                    var ddlValue = "";
                    $("#ddlFreightType").empty();
                    ddlValue += '<option value="0">SELECT FREIGHT TYPE</option>'
                    for (var i = 0; i < data.length; i++) {
                        ddlValue += '<option value="' + data[i].FreightTypeId + '">' + data[i].FreightTypeName + '</option>';
                    }
                    $("#ddlFreightType").append(ddlValue);
                }
            }
        });
    }
}
//#endregion

//#region check quote freighttype change
function CheckQuoteFreightTypeChange(freightTypeId, freightType) {

    var values = CustomerRouteDetail();
    if (values != null) {
        $.ajax({
            url: baseUrl + 'Shipment/Shipment/GetFreightType',
            data: JSON.stringify(values),
            type: "Post",
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (ValidateRouteStops()) {
                    var isChanged = true;
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].FreightTypeId == freightTypeId) {
                            isChanged = false
                        }
                    }
                    if (isChanged) {
                        AlertPopup("Quote not available for freight type: " + freightType);
                    }
                }
            }
        });
    }
}
//#endregion

//#region check quote PricingMethod change
function CheckQuotePricingMethodChange(pricingMethodId, pricingMethod) {

    var values = CustomerRouteDetail();
    if (values != null) {
        $.ajax({
            url: baseUrl + 'Shipment/Shipment/GetPricingMethod',
            data: JSON.stringify(values),
            type: "Post",
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                ShowSignaturePopUp
                if (ValidateRouteStops()) {
                    var isChanged = true;
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].PricingMethodId == pricingMethodId) {
                            isChanged = false
                        }
                    }
                    if (isChanged) {
                        AlertPopup("Quote not available for pricing method: " + pricingMethod);
                    }
                }
            }
        });
    }
}
//#endregion

//#region bind pricing method dropdownlist
function GetPricingMethod() {

    var values = CustomerRouteDetail();
    if (values != null) {
        $.ajax({
            url: baseUrl + 'Shipment/Shipment/GetPricingMethod',
            data: JSON.stringify(values),
            type: "Post",
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                ShowSignaturePopUp
                if (ValidateRouteStops()) {
                    $.ajax({
                        url: baseUrl + 'Quote/Quote/GetPricingMehtod',
                        type: "Get",
                        async: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {
                            var ddlResultValue = "";
                            $("#ddlPricingMethod").empty();

                            ddlResultValue += '<option value="0">SELECT PRICING METHOD</option>'
                            for (var i = 0; i < result.length; i++) {
                                if (data[0].PricingMethodId == result[i].PricingMethodId) {
                                    ddlResultValue += '<option selected="selected" value="' + result[i].PricingMethodId + '">' + $.trim(result[i].PricingMethodName).toUpperCase() + '</option>';
                                }
                                else {
                                    ddlResultValue += '<option value="' + result[i].PricingMethodId + '">' + $.trim(result[i].PricingMethodName).toUpperCase() + '</option>';
                                }
                            }
                            $("#ddlPricingMethod").append(ddlResultValue);

                        }
                    });
                }
                else {

                    var ddlValue = "";
                    $("#ddlPricingMethod").empty();

                    ddlValue += '<option value="0">SELECT PRICING METHOD</option>'
                    for (var i = 0; i < data.length; i++) {
                        ddlValue += '<option value="' + data[i].PricingMethodId + '">' + $.trim(data[i].PricingMethodName).toUpperCase() + '</option>';
                    }
                    $("#ddlPricingMethod").append(ddlValue);
                }
            }
        });
    }
}
//#endregion


//#region validate pickup and delivery  location
function validateLocation() {

    var isvalid = true;
    var pickuplocationid = $("#ddlPickupLocation").val();
    var deliveryLocationid = $("#ddlDeliveryLocation").val();
    if (pickuplocationid == deliveryLocationid) {

        AlertPopup("Pickup and Delivery location should not be same");
        isvalid = false;
    }
    if (isvalid) {
        isvalid = ValidatePickUpArrivalDate();
    }
    if (isvalid) {
        isvalid = ValidatePickUpDepartureDate();
    }


    return isvalid;
}
//#endregion

//#region add route stopes

var btnAddRouteStop = function () {

    $("#btnAddRouteStop").on("click", function () {

        if ($("#ddlCustomer").val() > 0) {
            if (validateLocation()) {
                if (contactInfoCounts > 0) {
                    if (isFormValid("formRouteStop")) {
                        if ($("#ddlPickupLocation").val() > 0) {
                            if ($("#ddlDeliveryLocation").val() > 0) {

                                if (ValidateRouteStops()) {

                                    if ($("#hdnShipmentId").val() > 0) {
                                        if ($("#tblRouteStope").attr("data-row-no") > 0) {
                                            addRouteStops();
                                            //clearDriverNEquipment();
                                            GetEquipmentList();
                                        } else {

                                            $.confirm({
                                                title: 'Confirmation!',
                                                content: '<b>Driver and equipment selections will be lost if you add route details to this shipment.</b> ',
                                                type: 'red',
                                                typeAnimated: true,
                                                buttons: {
                                                    confirm: {
                                                        btnClass: 'btn-blue',
                                                        action: function () {
                                                            addRouteStops();
                                                            clearDriverNEquipment();
                                                            GetEquipmentList();
                                                        }
                                                    },
                                                    cancel: {
                                                        //btnClass: 'btn-red',
                                                    }
                                                }
                                            })
                                        }
                                    }
                                    else {
                                        if (isFormValid("formRouteStop")) {
                                            addRouteStops();
                                            // clearDriverNEquipment();
                                            GetEquipmentList();
                                        }
                                    }


                                }
                                else {
                                    if ($("#tblRouteStope").attr("data-row-no") > 0) {
                                        addRouteStops();
                                        //clearDriverNEquipment();
                                        GetEquipmentList();
                                    }
                                    else {
                                        //commented  by sudhansu on 8Dec2020
                                        //Uncommented  by sudhansu on 10Dec2020
                                        $.confirm({
                                            title: 'Confirmation!',
                                            content: '<b>Quote not available for ' + $("#ddlCustomer").text() + ' / location. Do you want to continue?</b> ',
                                            type: 'red',
                                            typeAnimated: true,
                                            buttons: {
                                                continue: {
                                                    btnClass: 'btn-blue',
                                                    action: function () {
                                                        if ($("#hdnShipmentId").val() > 0) {
                                                            $.confirm({
                                                                title: 'Confirmation!',
                                                                content: '<b>Driver and equipment selections will be lost if you add route details to this shipment.</b> ',
                                                                type: 'red',
                                                                typeAnimated: true,
                                                                buttons: {
                                                                    confirm: {
                                                                        btnClass: 'btn-blue',
                                                                        action: function () {
                                                                            addRouteStops();
                                                                            clearDriverNEquipment();
                                                                            GetEquipmentList();
                                                                        }
                                                                    },
                                                                    cancel: {
                                                                        //btnClass: 'btn-red',
                                                                    }
                                                                }
                                                            })
                                                        }
                                                        else {
                                                            if (isFormValid("formRouteStop")) {
                                                                addRouteStops();
                                                                // clearDriverNEquipment();
                                                                GetEquipmentList();
                                                            }
                                                        }

                                                    }
                                                },
                                                cancel: {
                                                    //btnClass: 'btn-red',
                                                }
                                            }
                                        })
                                    }
                                }
                            }
                            else {
                                AlertPopup("Please select a Delivery Location.")
                            }
                        }
                        else {
                            AlertPopup("Please select a Pickup Location.")
                        }

                    }
                }
                else {

                    AlertPopup(msgMissingContactInfo);
                }
            }
        }
        else {
            AlertPopup('Please select a customer..');
        }

    });
}
//#endregion

//#region add route stopes
var btnAddRoundTrip = function () {
    $("#btnRoundTrip").on("click", function () {

        if ($("#ddlCustomer").val() > 0) {
            if (validateLocation()) {
                if (contactInfoCounts > 0) {
                    if (isFormValid("formRouteStop")) {
                        if ($("#ddlPickupLocation").val() > 0) {
                            if ($("#ddlDeliveryLocation").val() > 0) {
                                if (ValidateRouteStops()) {

                                    if ($("#hdnShipmentId").val() > 0) {

                                        $.confirm({
                                            title: 'Confirmation!',
                                            content: '<b>Driver and equipment selections will be lost if you add route details to this shipment.</b> ',
                                            type: 'red',
                                            typeAnimated: true,
                                            buttons: {
                                                confirm: {
                                                    btnClass: 'btn-blue',
                                                    action: function () {
                                                        AddRoundTrip();
                                                        clearDriverNEquipment();
                                                        GetEquipmentList();
                                                    }
                                                },
                                                cancel: {
                                                    //btnClass: 'btn-red',
                                                }
                                            }
                                        })

                                    }
                                    else {
                                        if (isFormValid("formRouteStop")) {
                                            AddRoundTrip();
                                            clearDriverNEquipment();
                                            GetEquipmentList();
                                        }
                                    }

                                }
                                else {

                                    $.confirm({
                                        title: 'Confirmation!',
                                        content: '<b>Quote not available for ' + $("#ddlCustomer").text() + ' / location. Do you want to continue?</b> ',
                                        type: 'red',
                                        typeAnimated: true,
                                        buttons: {
                                            continue: {
                                                btnClass: 'btn-blue',
                                                action: function () {
                                                    if ($("#hdnShipmentId").val() > 0) {

                                                        $.confirm({
                                                            title: 'Confirmation!',
                                                            content: '<b>Driver and equipment selections will be lost if you add route details to this shipment.</b> ',
                                                            type: 'red',
                                                            typeAnimated: true,
                                                            buttons: {
                                                                confirm: {
                                                                    btnClass: 'btn-blue',
                                                                    action: function () {
                                                                        AddRoundTrip();
                                                                        clearDriverNEquipment();
                                                                        GetEquipmentList();
                                                                    }
                                                                },
                                                                cancel: {
                                                                    //btnClass: 'btn-red',
                                                                }
                                                            }
                                                        })
                                                    }
                                                    else {
                                                        if (isFormValid("formRouteStop")) {
                                                            AddRoundTrip();
                                                            clearDriverNEquipment();
                                                            GetEquipmentList();
                                                        }
                                                    }

                                                }
                                            },
                                            cancel: {
                                                //btnClass: 'btn-red',
                                            }
                                        }
                                    })

                                }
                            }
                            else {

                                AlertPopup("Please select a Delivery Location.")
                            }
                        }
                        else {

                            AlertPopup("Please select a Pickup Location.")
                        }

                    }
                }
                else {
                    AlertPopup(msgMissingContactInfo)
                }

            }
        }
        else {
            AlertPopup("Please select a customer.")
        }

    });
}
//#endregion
function addRouteStops() {

    var isAppointmentRequired = $(".ddlDeliveryLocation").attr("data-IsAppointmentRequired");
    var deliveryLocationText = $.trim($("#ddlDeliveryLocation").find('option:selected').text());
    var website = $(".ddlDeliveryLocation").attr("data-website");
    var phone = $(".ddlDeliveryLocation").attr("data-phone");
    isAppointmentRequired = JSON.parse(isAppointmentRequired.toLowerCase());
    var https = "https://";
    var fullurl = https.concat(website);
    if (isAppointmentRequired == true) {
        $.confirm({
            title: 'Appointment Required',
            content: '<b>Address: </b>' + deliveryLocationText + " <br/><b>WebSite: </b><a target=_blank' href='" + fullurl + "'>" + website + "</a><br/><b>Phone: </b>" + phone + "",
            type: 'blue',
            typeAnimated: true,
            buttons: {
                Schedule: {
                    btnClass: 'btn-blue',
                    action: function () {
                        addRouteStop(false);
                    }
                },
                'Appointment Confirm': {
                    btnClass: 'btn-blue',
                    action: function () {
                        addRouteStop(true);
                    }
                }
            }
        })
    }
    else {
        addRouteStop(false)
    }

}

//#region add route stopage in table
function addRouteStop(isAppointmentRequireds) {

    var shipmentId = $("#hdnShipmentId").val();
    var isAppointmentNeeded = $(".ddlDeliveryLocation").attr("data-IsAppointmentRequired");
    var isAppointmentRequired = isAppointmentRequireds;
    var deliveryLocationText = $.trim($("#ddlDeliveryLocation").find('option:selected').text());
    var website = $(".ddlDeliveryLocation").attr("data-website");
    var phone = $(".ddlDeliveryLocation").attr("data-phone");

    var IsAppointmentConfirm = false;

    var isPickUpWaitingTimeRequired = false;
    if ($("#chkPickUpWaitingTime").is(':checked')) {
        isPickUpWaitingTimeRequired = true;
    }
    var isDeliveryWaitingTimeRequired = false;

    if ($("#chkDeliveryWaitingTime").is(':checked')) {
        isDeliveryWaitingTimeRequired = true;
    }

    var pickupLocationId = $.trim($("#ddlPickupLocation").val());
    var pickupLocationText = $.trim($("#ddlPickupLocation").find('option:selected').text());
    var deliveryLocationId = $.trim($("#ddlDeliveryLocation").val());


    var dtPickup = $.trim($("#dtArrivalDate").val());
    var dtPickUpToDate = $.trim($("#dtPickUpToDate").val());

    var dtDelivery = $.trim($("#dtDeliveryDate").val());
    var dtDeliveryToDate = $.trim($("#dtDeliveryToDate").val());
    //var comment = $.trim($("#txtComment").val());
    glbRouteStops
    //var tblRowsCount = $("#dtRouteBody tr").length + 1;
    //var tblRowsCount = glbRouteStops.length + 1;
    var routeStopId = 0;
    var shipmentId = $("#hdnShipmentId").val();
    var tblRowsCount = 0;
    if (shipmentId > 0) {
        tblRowsCount = GetMaxRouteNo(shipmentId) + 1;
    }
    else {
        if (glbRouteStops.length > 0) {
            var max = glbRouteStops.reduce(function (prev, current) {
                return (prev.RouteNo > current.RouteNo) ? prev : current
            });
            tblRowsCount = max.RouteNo + 1;
        }
        else {
            tblRowsCount = 1;
        }
    }


    var IsWaitingTimeNeeded = $(".ddlCustomer").attr("date-IsWaitingTimeRequired");

    if ($("#tblRouteStope").attr("data-row-no") > 0) {
        var tblRowsCount = $("#tblRouteStope").attr("data-row-no");
        var rowindex = Number(tblRowsCount) - 1;
        var routedetail = glbRouteStops[rowindex];
        // routedetail.RouteNo = tblRowsCount;
        routedetail.PickupLocationId = pickupLocationId;
        routedetail.PickupLocationText = pickupLocationText;
        routedetail.DeliveryLocationId = deliveryLocationId;
        routedetail.DeliveryLocationText = deliveryLocationText;
        routedetail.PickDateTime = dtPickup;
        routedetail.PickDateTimeTo = dtPickUpToDate;
        routedetail.DeliveryDateTime = dtDelivery;
        routedetail.DeliveryDateTimeTo = dtDeliveryToDate;
        // routedetail.Comment = comment;
        routedetail.IsDeleted = false;
        routedetail.IsAppointmentNeeded = JSON.parse(isAppointmentNeeded);
        if (routedetail.IsAppointmentRequired == false) {
            routedetail.IsAppointmentRequired = JSON.parse(isAppointmentRequired);
        }
        routedetail.Website = website;
        routedetail.Phone = phone;

        routedetail.IsWaitingTimeNeeded = JSON.parse(IsWaitingTimeNeeded);
        routedetail.IsPickUpWaitingTimeRequired = JSON.parse(isPickUpWaitingTimeRequired);
        routedetail.IsDeliveryWaitingTimeRequired = JSON.parse(isDeliveryWaitingTimeRequired);
        SuccessPopup("Your data has successfully been updated to your shipment.<br/>Don't forget to click on the Submit button to save all changes.")
        $('.selectize-control').css('pointer-events', 'auto');
        $("#tblRouteStope").attr("data-row-no", 0);
        $("#btnAddRouteStop").text("Add Trip");
        $("#btnRoundTrip").show();
        //bind route table
        bindRoutetable();
    }
    else {

        var objRouteStops = {};
        objRouteStops.ShipmentId = $("#hdnShipmentId").val();
        objRouteStops.ShipmentRouteStopId = routeStopId;
        objRouteStops.RouteNo = tblRowsCount;
        objRouteStops.PickupLocationId = pickupLocationId;
        objRouteStops.PickupLocationText = pickupLocationText;
        objRouteStops.DeliveryLocationId = deliveryLocationId;
        objRouteStops.DeliveryLocationText = deliveryLocationText;
        objRouteStops.PickDateTime = dtPickup;
        objRouteStops.PickDateTimeTo = dtPickUpToDate;
        objRouteStops.DeliveryDateTime = dtDelivery;
        objRouteStops.DeliveryDateTimeTo = dtDeliveryToDate;
        //objRouteStops.Comment = comment;
        objRouteStops.IsDeleted = false;
        objRouteStops.ActPickupArrival = "";
        objRouteStops.ActPickupDeparture = "";
        objRouteStops.ActDeliveryArrival = "";
        objRouteStops.ActDeliveryDeparture = "";
        objRouteStops.ReceiverName = "";
        objRouteStops.DigitalSignature = "";
        objRouteStops.IsAppointmentNeeded = JSON.parse(isAppointmentNeeded);
        objRouteStops.IsAppointmentRequired = JSON.parse(isAppointmentRequired);
        objRouteStops.Website = website;
        objRouteStops.Phone = phone;

        objRouteStops.IsWaitingTimeNeeded = JSON.parse(IsWaitingTimeNeeded);
        objRouteStops.IsPickUpWaitingTimeRequired = JSON.parse(isPickUpWaitingTimeRequired);
        objRouteStops.IsDeliveryWaitingTimeRequired = JSON.parse(isDeliveryWaitingTimeRequired);
        if ($("#hdnShipmentId").val() > 0) {

            var values = {};
            values = objRouteStops;

            $.ajax({
                url: baseUrl + 'Shipment/Shipment/AddRouteStops',
                data: JSON.stringify(values),
                type: "Post",
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: function (data) {


                    if (data > 0) {
                        objRouteStops.ShipmentRouteStopId = data;
                        glbRouteStops.push(objRouteStops);
                    }
                }
            });

        }
        else {
            glbRouteStops.push(objRouteStops);
        }

        SuccessPopup("Your data has successfully been added to your shipment.<br/> Don't forget to click on the Submit button to save all changes.");

        // glbRouteStops.push(objRouteStops);
        //bind route table
        bindRoutetable();
        GetEquipmentList();
    }
    //clear route stops
    clearRouteStops();
    if (glbRouteStops.length > 0) {

        for (var i = 0; i < glbRouteStops.length; i++) {
            if (glbRouteStops[i].IsAppointmentNeeded == true) {
                if (glbRouteStops[i].IsAppointmentRequired == false) {
                    //10=>appointement pending
                    $("#ddlStatus").val(10);
                }
                else {
                    //1=>Dispetched
                    $("#ddlStatus").val(1);
                }
            }
        }
    }

    //select default first radion button 
    checkRadionButton()

    manageLocation("ddlPickupLocation");
    manageLocation("ddlDeliveryLocation");
}
//#endregion 


//#region add round trip in table
var AddRoundTrip = function () {
    var shipmentId = $("#hdnShipmentId").val();
    var pickupLocationId = $.trim($("#ddlPickupLocation").val());
    var pickupLocationText = $.trim($("#ddlPickupLocation").find('option:selected').text());
    var deliveryLocationId = $.trim($("#ddlDeliveryLocation").val());
    var deliveryLocationText = $.trim($("#ddlDeliveryLocation").find('option:selected').text());

    var dtPickup = $.trim($("#dtArrivalDate").val());
    var dtPickUpToDate = $.trim($("#dtPickUpToDate").val());

    var dtDelivery = $.trim($("#dtDeliveryDate").val());
    var dtDeliveryToDate = $.trim($("#dtDeliveryToDate").val());
    //var comment = $.trim($("#txtComment").val());
    glbRouteStops
    //var tblRowsCount = $("#dtRouteBody tr").length + 1;
    // var tblRowsCount = glbRouteStops.length + 1;
    var routeStopId = 0;



    var tblRowsCount = 0;
    if (shipmentId > 0) {
        tblRowsCount = GetMaxRouteNo(shipmentId) + 1;
    }
    else {
        if (glbRouteStops.length > 0) {
            var max = glbRouteStops.reduce(function (prev, current) {
                return (prev.RouteNo > current.RouteNo) ? prev : current
            });
            tblRowsCount = max.RouteNo + 1;
        }
        else {
            tblRowsCount = 1;
        }
    }


    var objRouteStops = {};
    objRouteStops.ShipmentId = $("#hdnShipmentId").val();
    objRouteStops.ShipmentRouteStopId = routeStopId;
    objRouteStops.RouteNo = tblRowsCount;
    objRouteStops.PickupLocationId = pickupLocationId;
    objRouteStops.PickupLocationText = pickupLocationText;
    objRouteStops.DeliveryLocationId = deliveryLocationId;
    objRouteStops.DeliveryLocationText = deliveryLocationText;
    objRouteStops.PickDateTime = dtPickup;
    objRouteStops.PickDateTimeTo = dtPickUpToDate;
    objRouteStops.DeliveryDateTime = dtDelivery;
    objRouteStops.DeliveryDateTimeTo = dtDeliveryToDate;
    //objRouteStops.Comment = comment;
    objRouteStops.IsDeleted = false;
    objRouteStops.ActPickupArrival = "";
    objRouteStops.ActPickupDeparture = "";
    objRouteStops.ActDeliveryArrival = "";
    objRouteStops.ActDeliveryDeparture = "";
    objRouteStops.ReceiverName = "";
    objRouteStops.DigitalSignature = "";
    if ($("#hdnShipmentId").val() > 0) {

        var values = {};
        values = objRouteStops;

        $.ajax({
            url: baseUrl + 'Shipment/Shipment/AddRouteStops',
            data: JSON.stringify(values),
            type: "Post",
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {


                if (data > 0) {
                    objRouteStops.ShipmentRouteStopId = data;
                    glbRouteStops.push(objRouteStops);
                }
            }
        });

    }
    else {
        glbRouteStops.push(objRouteStops);
    }

    //bind route table
    bindRoutetable();


    var tblRowsCount = $("#dtRouteBody tr").length + 1;
    var objRoundTrip = {};
    objRoundTrip.ShipmentId = $("#hdnShipmentId").val();
    objRoundTrip.ShipmentRouteStopId = routeStopId;
    objRoundTrip.RouteNo = tblRowsCount;
    objRoundTrip.PickupLocationId = deliveryLocationId;
    objRoundTrip.PickupLocationText = deliveryLocationText;
    objRoundTrip.DeliveryLocationId = pickupLocationId;
    objRoundTrip.DeliveryLocationText = pickupLocationText;
    objRoundTrip.PickDateTime = dtPickup;
    objRoundTrip.PickDateTimeTo = dtPickUpToDate;
    objRoundTrip.DeliveryDateTime = dtDelivery;
    objRoundTrip.DeliveryDateTimeTo = dtDeliveryToDate;
    //objRoundTrip.Comment = comment;
    objRoundTrip.ActPickupArrival = "";
    objRoundTrip.ActPickupDeparture = "";
    objRoundTrip.ActDeliveryArrival = "";
    objRoundTrip.ActDeliveryDeparture = "";
    objRoundTrip.IsDeleted = false;
    objRoundTrip.ReceiverName = "";
    objRoundTrip.DigitalSignature = "";

    if ($("#hdnShipmentId").val() > 0) {

        var values = {};
        values = objRoundTrip;

        $.ajax({
            url: baseUrl + 'Shipment/Shipment/AddRouteStops',
            data: JSON.stringify(values),
            type: "Post",
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {

                if (data > 0) {
                    objRoundTrip.ShipmentRouteStopId = data;
                    glbRouteStops.push(objRoundTrip);
                }
            }
        });

    }
    else {
        glbRouteStops.push(objRoundTrip);
    }


    //bind route table
    bindRoutetable();

    //clear route stops
    clearRouteStops();

    //select default first radion button 
    checkRadionButton()

    manageLocation("ddlPickupLocation");
    manageLocation("ddlDeliveryLocation");
}
//#endregion 

function GetMaxRouteNo(shipmentId) {
    var maxRouteNo = 0;
    $.ajax({
        url: baseUrl + 'Shipment/Shipment/GetMaxRouteNo',
        data: { "shipmentId": shipmentId },
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

//#region check selected pickup and delivery location and arrival time is exist in Quote

function ValidateRouteStops() {

    var result = false;
    var currentRouteValue = CustomerRouteDetail();

    var values = {};
    if (currentRouteValue.CustomerId != undefined) {
        values.CustomerId = currentRouteValue.CustomerId;
        values.PickupLocation = currentRouteValue.PickupLocationId;
        values.DeliveryLocation = currentRouteValue.DeliveryLocationId;
        values.ArrivalDate = currentRouteValue.PickupArrivalDate;
    }
    else {

        values.CustomerId = $("#ddlCustomer").val();
        values.PickupLocation = $.trim($("#ddlPickupLocation").val());
        values.DeliveryLocation = $.trim($("#ddlDeliveryLocation").val());
        values.ArrivalDate = $.trim($("#dtArrivalDate").val());
    }
    $.ajax({
        url: baseUrl + 'Shipment/Shipment/ValidateRouteStops',
        data: JSON.stringify(values),
        type: "Post",
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: function (data) {


            if (data == true) {

                result = true;
            }
            else {
                //toastr.warning("Quote not available for " + $("#ddlCustomer").text() + " / location.");
                result = false;
            }
        }
    });
    return result;
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
function replaceBR(string) {

    var str = string.replace("<br/>", "");
    return str;
}
//#region edit shipment detail by id
function edit_route_stops(index) {
    $($("#dtRouteBody tr")).find("input[type=radio]").prop("checked", false);
    $($("#dtRouteBody tr")[index]).find("input[type=radio]").prop("checked", true);

    var routedetail = glbRouteStops[index];


    $("#ddlPickupLocation").val(routedetail.PickupLocationId);
    //$("#ddlPickupLocation").find('option:selected').text(routedetail.PickupLocationText);
    $("#ddlDeliveryLocation").val(routedetail.DeliveryLocationId)
    //  $("#ddlDeliveryLocation").find('option:selected').text(routedetail.DeliveryLocationText);
    $("#dtArrivalDate").val(routedetail.PickDateTime);
    $("#dtPickUpToDate").val(routedetail.PickDateTimeTo);
    $("#dtDeliveryDate").val(routedetail.DeliveryDateTime);
    $("#dtDeliveryToDate").val(routedetail.DeliveryDateTimeTo);
    //$("#txtComment").val(routedetail.Comment);


    if (routedetail.PickupLocationId > 0) {
        var ddlpickup = "<option selected='selected' value=" + routedetail.PickupLocationId + ">" + replaceBR(routedetail.PickupLocationText) + "</option>";
        $("#ddlPickupLocation").empty();
        $("#ddlPickupLocation").append(ddlpickup);
        $(".ddlPickupLocation").text(replaceBR(routedetail.PickupLocationText));
    }
    if (routedetail.DeliveryLocationId > 0) {
        var ddldelivery = "<option  selected='selected' value=" + routedetail.DeliveryLocationId + ">" + replaceBR(routedetail.DeliveryLocationText) + "</option>";
        $("#ddlDeliveryLocation").empty();
        $("#ddlDeliveryLocation").append(ddldelivery);
        $(".ddlDeliveryLocation").text(replaceBR(routedetail.DeliveryLocationText));
    }
    if (routedetail.IsAppointmentNeeded == true && routedetail.IsAppointmentRequired == false) {
        $(".ddlDeliveryLocation").attr("data-IsAppointmentRequired", true);
        $(".ddlDeliveryLocation").attr("data-website", routedetail.Website);
        $(".ddlDeliveryLocation").attr("data-phone", routedetail.Phone);
    }
    else {
        $(".ddlDeliveryLocation").attr("data-IsAppointmentRequired", false);
        $(".ddlDeliveryLocation").attr("data-website", "");
        $(".ddlDeliveryLocation").attr("data-phone", "");
    }

    if (routedetail.IsWaitingTimeNeeded == true) {
        $(".ddlCustomer").attr("date-IsWaitingTimeRequired", true);
        $('#chkDeliveryWaitingTime').prop('checked', false);
        $('#chkPickUpWaitingTime').prop('checked', false);

        if (routedetail.IsPickUpWaitingTimeRequired == true) {
            $('#chkPickUpWaitingTime').prop('checked', true);
        }
        if (routedetail.IsDeliveryWaitingTimeRequired == true) {
            $('#chkDeliveryWaitingTime').prop('checked', true);
        }
        $(".divWaitingTime").show();
    } else {
        $(".ddlCustomer").attr("date-IsWaitingTimeRequired", false);
        $(".divWaitingTime").hide();
    }

    //$('.selectize-control').css('pointer-events', 'none');

    $("#tblRouteStope").attr("data-row-no", index + 1);
    $("#btnAddRouteStop").text("Update");
    $("#btnRoundTrip").hide();
    checkdata();
}
//#endregion

//#region Find and remove route and detail
function remove_row_from_route(_this) {
    $.confirm({
        title: 'Confirmation!',
        content: "<b>Are you sure you want to Delete ?</b> ",
        type: 'red',
        typeAnimated: true,
        buttons: {
            delete: {
                btnClass: 'btn-blue',
                action: function () {
                    row = $(_this).closest("tr");
                    var deletedrow = $(row).find("input[name='rdSelectedRoute']").val();

                    //Remove row form accessorial charges
                    RemoveAccessorialPrice(deletedrow);

                    //Rebind accessorial charges
                    if (glbAccessorialFee.length > 0) {
                        getAccessorialPrice();
                    }


                    // Remove route
                    //glbRouteStops = $.grep(glbRouteStops, function (value) {
                    //    return value.RouteNo != deletedrow;
                    //});

                    var routeStops = glbRouteStops.filter(x => x.RouteNo == deletedrow);
                    routeStops[0].IsDeleted = true;

                    //Bind Route
                    bindRoutetable();
                    //$(_this).parents("tr").remove();

                    //Rearrange serial no
                    // manageRowsAfterDeletionfright();

                    //remove row from shipment druing route deletion
                    remove_shipment_row(_this);

                    //select default selected  first radion button 
                    checkRadionButton();

                    //bind shipment and accessorial charges
                    checkdata();

                }
            },
            cancel: {
                //btnClass: 'btn-red',
            }
        }
    })


}
//#endregion

//#region clear route stops field
clearRouteStops = function () {

    var ddlpickup = "<option selected='selected' value='0'>SEARCH PICKUP LOCATION</option>";
    $("#ddlPickupLocation").empty();
    $("#ddlPickupLocation").append(ddlpickup);
    $(".ddlPickupLocation").text("SEARCH PICKUP LOCATION");


    var ddldelivery = "<option  selected='selected' value='0'>SEARCH DELIVERY LOCATION</option>";
    $("#ddlDeliveryLocation").empty();
    $("#ddlDeliveryLocation").append(ddldelivery);
    $(".ddlDeliveryLocation").text("SEARCH DELIVERY LOCATION");

    $("#dtArrivalDate").val("");
    $("#dtArrivalDate").val("");
    $("#dtDeliveryDate").val("");
    $("#dtPickUpToDate").val("");
    $("#dtDeliveryToDate").val("");
    // $("#txtComment").val("");
    $('.selectize-control').css('pointer-events', 'auto');
    $("#tblRouteStope").attr("data-row-no", 0);
}
//#endregion

//#region clear route stops
btnClearRoute = function () {

    $("#btnClearRoute").on("click", function () {
        clearRouteStops();
        $("#btnAddRouteStop").text("Add Trip");
        $("#btnRoundTrip").show();
    })
}

//#endregion

//#region manage route rows after deletion
var manageRowsAfterDeletionfright = function () {

    var srlno = 1;
    $("#dtRouteBody tr").each(function () {
        $(this).find("label[name=lblSrNo]").text(srlno);
        $(this).find("input[name=rdSelectedRoute]").val(srlno);
        srlno++;
    });
}
//#endregion

//#region remove row from shipment druing route deletion
function remove_shipment_row(_this) {

    row = $(_this).closest("tr");
    var deletedrow = $(row).find("input[name='rdSelectedRoute']").val();


    //glbFreightDetail = $.grep(glbFreightDetail, function (value) {
    //    
    //    return value.RouteNo != deletedrow;
    //});

    var freightdetail = glbFreightDetail.filter(x => x.RouteNo == deletedrow);
    for (var i = 0; i < freightdetail.length; i++) {
        freightdetail[i].IsDeleted = true;
    }

    //bind shipment table
    bindshipmenttable();


}
//#endregion

//#region add freight 
var btnAddFreight = function () {
    $("#btnAddFreight").on("click", function () {

        if (isFormValid("formAddFreight")) {
            if ($("#ddlFreightType").val() > 0) {

                if ($("#ddlPricingMethod").val() > 0) {

                    AddShipmentDetail();
                    checkFreightRadionButton();
                }
                else {

                    AlertPopup("Please select a Pricing Method.")
                }

            }
            else {
                AlertPopup("Please select a Freight Type.")
            }
        }
    });
}
//#endregion

//#region add or edit  Shipment Detail 
function AddShipmentDetail() {


    if ($("#dtRouteBody tr").length > 0) {
        var radioValue = $("input[name='rdSelectedRoute']:checked").val();

        if (radioValue > 0) {

            var row = $("input[name='rdSelectedRoute']:checked").closest("tr")[0];

            var pickupLocationId = $(row).find("label[name='lblPickupLocationId']").text();
            var deliveryLocationId = $(row).find("label[name='lblDeliveryLocationId']").text();
            var commoditytext = $("#txtCommodity").val();

            var freighttypetext = $("#ddlFreightType").find('option:selected').text();
            var freighttypevalue = $("#ddlFreightType").val();

            var pricingmethodtext = $("#ddlPricingMethod").find('option:selected').text();
            var pricingmethodvalue = $("#ddlPricingMethod").val();

            var minfee = $("#txtMinFee").val();
            var upto = $("#txtUpTo").val();
            var addfee = $("#txtAddPrice").val();
            var totalprice = $("#txtTotalPrice").val();

            var QutVlmWgt = $("#txtQutVlmWgt").val();
            var hazardous = $("#ddlHazardous").val();
            var temperature = $("#txtTemperature").val();
            var temperaturetype = $("#ddlTemperatureUnit").val();
            var totalprice = $("#txtTotalPrice").val();
            var comments = $("#txtComments").val();
            var selectedtext = $.trim($("#ddlFreightType").find('option:selected').text());

            var weight = $("#txtWeight").val();
            var ddlUnit = $("#ddlUnit").val();
            var QutVlmWgt = $("#txtQutVlmWgt").val();
            var noOfBox = $("#txtNoOfBox").val();
            var trailerCount = $("#txtTrailer").val();





            var isPartialShipment = 0;
            if ($("#chkPartialShipment").is(':checked')) {
                
                // alert('Partial')
                isPartialShipment = 1;
            }

            var partialPallet = $("#txtPartalPallet").val();
            var partialBox = $("#txtPartialBox").val();
            
            //alert('Partial2')
            var pcsType = "";
            var pcsTypeId = 0;

            if ($("#tblShipmentDetail").attr("data-row-no") > 0) {
                //Edit array 
                var tblRowsCount = $("#tblShipmentDetail").attr("data-row-no");

                var rowindex = Number(tblRowsCount) - 1;
                var freightDetail = glbFreightDetail[rowindex];

                freightDetail.IsPartialShipment = isPartialShipment;
                freightDetail.PartialPallet = partialPallet;
                freightDetail.PartialBox = partialBox;

                freightDetail.RouteNo = radioValue;
                freightDetail.PickupLocationId = pickupLocationId;
                freightDetail.DeliveryLocationId = deliveryLocationId;
                freightDetail.Commodity = commoditytext;


                freightDetail.freighttypetext = freighttypetext;
                freightDetail.FreightTypeId = freighttypevalue;

                freightDetail.pricingmethodtext = pricingmethodtext;
                freightDetail.PricingMethodId = pricingmethodvalue;

                freightDetail.MinFee = minfee;
                freightDetail.UpTo = upto;
                freightDetail.UnitPrice = addfee;
                freightDetail.TotalPrice = totalprice;


                freightDetail.Hazardous = hazardous;
                freightDetail.Temperature = temperature;
                freightDetail.TemperatureType = temperaturetype;
                freightDetail.QutWgtVlm = QutVlmWgt;

                freightDetail.NoOfBox = noOfBox;
                freightDetail.Weight = weight;
                if (ddlUnit != "" || ddlUnit != undefined) {
                    freightDetail.Unit = ddlUnit;
                }

                freightDetail.PcsTypeId = pcsTypeId;
                freightDetail.PcsType = pcsType;
                freightDetail.Comments = comments;
                freightDetail.TrailerCount = trailerCount;

                SuccessPopup("Your data has successfully been updated to your shipment.<br/>  Don't forget to click on the Submit button to save all changes.");

                $("#tblShipmentDetail").attr("data-row-no", 0);
                $("#btnAddFreight").text("Add");

            }
            else {
                //Add in array
                var objFreightDetail = {};
                objFreightDetail.ShipmentId = 0;
                objFreightDetail.ShipmentRouteId = 0;

                objFreightDetail.RouteNo = radioValue;
                objFreightDetail.PickupLocationId = pickupLocationId;
                objFreightDetail.DeliveryLocationId = deliveryLocationId;
                objFreightDetail.Commodity = commoditytext;
                objFreightDetail.freighttypetext = freighttypetext;
                objFreightDetail.FreightTypeId = freighttypevalue;
                objFreightDetail.pricingmethodtext = pricingmethodtext;
                objFreightDetail.PricingMethodId = pricingmethodvalue;

                objFreightDetail.MinFee = minfee;
                objFreightDetail.UpTo = upto;
                objFreightDetail.UnitPrice = addfee;
                objFreightDetail.TotalPrice = totalprice;

                objFreightDetail.Hazardous = hazardous;
                objFreightDetail.Temperature = temperature;
                objFreightDetail.TemperatureType = temperaturetype;
                objFreightDetail.QutWgtVlm = QutVlmWgt;
                objFreightDetail.IsDeleted = false;


                objFreightDetail.NoOfBox = noOfBox;
                objFreightDetail.Weight = weight;
                objFreightDetail.Unit = ddlUnit;
                objFreightDetail.PcsTypeId = pcsTypeId;
                objFreightDetail.PcsType = pcsType;
                objFreightDetail.Comments = comments;
                objFreightDetail.TrailerCount = trailerCount;

                objFreightDetail.IsPartialShipment = isPartialShipment;
                objFreightDetail.PartialPallet = partialPallet;
                objFreightDetail.PartialBox = partialBox;
                if ($("#hdnShipmentId").val() > 0) {

                    var routeStops = glbRouteStops.filter(x => x.RouteNo == radioValue);
                    objFreightDetail.ShipmentId = $("#hdnShipmentId").val();
                    objFreightDetail.ShipmentRouteId = routeStops[0].ShipmentRouteStopId;
                    objFreightDetail.CustomerId = $("#ddlCustomer").val();
                    var values = {};
                    values = objFreightDetail;

                    $.ajax({
                        url: baseUrl + 'Shipment/Shipment/AddFreightDetail',
                        data: JSON.stringify(values),
                        type: "Post",
                        async: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",

                        success: function (data) {


                            if (data > 0) {
                                objFreightDetail.FreightDetailId = data;
                                glbFreightDetail.push(objFreightDetail);
                            }
                        }
                    });

                }
                else {
                    objFreightDetail.FreightDetailId = 0;
                    glbFreightDetail.push(objFreightDetail);
                }
                SuccessPopup("Your data has successfully been updated to your shipment.<br/>Don't forget to click on the Submit button to save all changes.");
            }

            //Clear text box after submit
            ClearAddFreightDetail();


            //bind shipment table
            bindshipmenttable();

            //calculate accessorial charges
            btnCalculatetotalFee();

        }
        else {
            AlertPopup("Please select at least one Route Stop.")
        }
    }
    else {
        AlertPopup("Please add at least one Pickup and Delivery location.")
    }
}
//#endregion

//#region  freight clear
function ClearAddFreightDetail() {
    
    $("#formAddFreight input:text").val("");
    $("#ddlFreightType").val(0);
    $("#ddlPricingMethod").val(0);
    $("#ddlHazardous").val(0);
    $("#tblShipmentDetail").attr("data-row-no", 0);
    $("#btnAddFreight").text("Add");
    $("#ddlUnit").val("KG");
    $("#txtWeight").val("");
    $("#txtTrailer").val("");
    $("#txtNoOfBox").val("");

    $("#chkPartialShipment").prop("checked", false);
    $("#txtPartialBox").val("");
    $("#txtPartalPallet").val("");
    $(".divPartial").hide();
}
//#endregion

//#region calculate shipment total price 
function CalculateTotalPrice() {
    var minfee = $("#txtMinFee").val();
    var upto = $("#txtUpTo").val();
    var addfee = $("#txtAddPrice").val();
    var QutVlmWgt = $("#txtQutVlmWgt").val();
    var selectedtext = $.trim($("#ddlPricingMethod").find('option:selected').text());
    var selectedfreight = $.trim($("#ddlFreightType").find('option:selected').text());
    var weight = $("#txtWeight").val();
    var trailerCount = $("#txtTrailer").val();

    var noOfBox = $("#txtNoOfBox").val();
    $("#txtTotalPrice").val("");
    if ($.trim(selectedtext).toLowerCase() == $.trim("Per Pound").toLowerCase() || $.trim(selectedtext).toLowerCase() == $.trim("Per Kg").toLowerCase()) {
        if (weight > 0) {
            var totalprice = 0;
            if (Number(weight) <= Number(upto)) {
                totalprice = minfee;
            }
            else {
                totalprice = Number(minfee) + ((Number(weight) - Number(upto)) * (Number(addfee)));

            }
            $("#txtTotalPrice").val(ConvertStringToFloat(totalprice));
        }
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
            $("#txtTotalPrice").val(ConvertStringToFloat(totalprice));
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
            $("#txtTotalPrice").val(ConvertStringToFloat(totalprice));
        }
    }
    else if ($.trim(selectedtext).toLowerCase() == $.trim("Per Trip").toLowerCase() && $.trim(selectedfreight).toLowerCase() == $.trim("Trailer Move").toLowerCase()) {
        if (trailerCount > 0) {
            var totalprice = 0;
            if (Number(trailerCount) <= Number(upto)) {
                totalprice = minfee;
            }
            else {
                totalprice = Number(minfee) + ((Number(trailerCount) - Number(upto)) * (Number(addfee)));
            }
            $("#txtTotalPrice").val(ConvertStringToFloat(totalprice));
        }
    }
    else if ($.trim(selectedtext).toLowerCase() == $.trim("Per Trip").toLowerCase() && $.trim(selectedfreight).toLowerCase() != $.trim("Trailer Move").toLowerCase()) {
        if (minfee != "") {
            $("#txtTotalPrice").val(ConvertStringToFloat(minfee));
        }

    }
    else if ($.trim(selectedtext).toLowerCase() == $.trim("FIXED FEE").toLowerCase() || $.trim(selectedtext).toLowerCase() == $.trim("PER ROUND TRIP").toLowerCase() || $.trim(selectedtext).toLowerCase() == $.trim("PER DAY").toLowerCase()) {
        if (minfee != "") {
            $("#txtTotalPrice").val(ConvertStringToFloat(minfee));
        }
    }

    getUptoCount();
}
//#endregion
//#region edit shipment detail by id
function edit_from_shipment(index) {

    $($("#tblShipmentDetail tbody tr")).find("input[type=radio]").prop("checked", false);
    $($("#tblShipmentDetail tbody tr")[index]).find("input[type=radio]").prop("checked", true);



    ClearAddFreightDetail();
    var freightdetail = glbFreightDetail[index]
    $("#txtMinFee").val(freightdetail.MinFee);
    $("#txtUpTo").val(freightdetail.UpTo);
    $("#txtAddPrice").val(freightdetail.UnitPrice);
    $("#txtTotalPrice").val(freightdetail.TotalPrice);


    $("#ddlHazardous").val(freightdetail.Hazardous == true ? "1" : "0");
    $("#txtTemperature").val(freightdetail.Temperature);
    $("#txtCommodity").val(freightdetail.Commodity);
    $("#ddlFreightType").val(freightdetail.FreightTypeId);
    $("#ddlPricingMethod").val(freightdetail.PricingMethodId);
    $("#txtComments").val(freightdetail.Comments);


    $("#txtWeight").val(freightdetail.Weight == 0 ? "" : freightdetail.Weight);
    if (freightdetail.Unit != "") {
        $("#ddlUnit").val(freightdetail.Unit);
    }

    $("#txtQutVlmWgt").val(freightdetail.QutWgtVlm == 0 ? "" : freightdetail.QutWgtVlm);
    //#region usefull code
    //if (freightdetail.freighttypetext == "Seafood" || freightdetail.freighttypetext == "Bonded") {
    //    $("#txtWeight").val(freightdetail.Weight);
    //    $("#ddlUnit").val(freightdetail.Unit);

    //    $("#divWeight").show();

    //    $("#divNoOfPcs").hide();

    //    $("#txtQutVlmWgt").val("");

    //}
    //else if (freightdetail.freighttypetext == "Trailer Move" || freightdetail.freighttypetext == "Trailer Rental") {
    //    $("#lblNoof").text("Triler");
    //    $("#txtWeight").val("");
    //    $("#divWeight").hide();
    //    $("#txtQutVlmWgt").val(freightdetail.QutWgtVlm);
    //    $("#divNoOfPcs").show();
    //}
    //else {
    //    $("#txtQutVlmWgt").val(freightdetail.QutWgtVlm);
    //    $("#divWeight").hide();

    //    $("#divNoOfPcs").show();
    //    $("#lblNoof").text("Pallet");
    //}
    //#endregion code
    $("#txtNoOfBox").val(freightdetail.NoOfBox == 0 ? "" : freightdetail.NoOfBox);
    $("#txtTrailer").val(freightdetail.TrailerCount == 0 ? "" : freightdetail.TrailerCount);

    showhideFreightType(freightdetail.freighttypetext);
    if (freightdetail.IsPartialShipment == 1) {
        $(".divPartial").show();
        $("#chkPartialShipment").prop("checked", true);
        $("#txtPartialBox").val(freightdetail.PartialBox);
        $("#txtPartalPallet").val(freightdetail.PartialPallet);
    }
    else {
        $(".divPartial").hide();
    }

    $("#tblShipmentDetail").attr("data-row-no", index + 1);
    $("#btnAddFreight").text("Update");
    checkFreight(freightdetail.FreightDetailId);
}
//#endregion

//#region Find and remove selected table rows from Shipment table
function remove_row_from_shipment(index) {
    $.confirm({
        title: 'Confirmation!',
        content: "<b>Are you sure you want to Delete ?</b> ",
        type: 'red',
        typeAnimated: true,
        buttons: {
            delete: {
                btnClass: 'btn-blue',
                action: function () {
                    //Remove freight detail by id 

                    glbFreightDetail[index].IsDeleted = true;
                    //Bind shipment detail
                    bindshipmenttable();
                }
            },
            cancel: {
                //btnClass: 'btn-red',
            }
        }
    })



}
//#endregion

//#region clear freight on button click

btnClearFreight = function () {
    $("#btnClearFreight").on("click", function () {
        ClearAddFreightDetail();
    });
}
//#endregion

//#region get json value
function GetJsonValue() {
    var values = {};

    values.QuoteId = $("#hdnQuoteId").val();
    values.ShipmentId = $("#hdnShipmentId").val();
    values.CustomerId = $("#ddlCustomer").val();
    values.StatusId = $("#ddlStatus").val();
    values.SubStatusId = $("#ddlSubStatus").val();
    values.RequestedBy = $("#txtRequestedBy").val();
    values.Reason = $("#txtReason").val();
    values.ShipmentRefNo = $("#txtShipRefNo").val();
    values.AirWayBill = $("#txtAirWayBill").val();
    values.CustomerPO = $("#txtCustomerPO").val();
    values.OrderNo = $("#txtOrderNo").val();
    values.CustomerRef = $("#txtCustomerRefNo").val();
    values.ContainerNo = $("#txtContainerNo").val();
    values.PurchaseDoc = $("#txtPurchaseDoc").val();
    values.VendorNconsignee = $("#txtVendorNConsignee").val();

    var equipmentdriver = $("#hdnEquipment").val();
    if (equipmentdriver != "") {
        values.ShipmentEquipmentNdriver = equipment;//JSON.parse(equipmentdriver);
    }

    values.DriverInstruction = $("#txtDriverInstruction").val();

    if (values.CustomerId > 0) {

        values.CustomerName = $("#ddlCustomer").text();
    }
    else {
        values.CustomerName = $("#ddlCustomer").val()
        values.CustomerId = 0;
    }

    var routeStopsDeatail = [];
    for (var i = 0; i < glbRouteStops.length; i++) {

        routeStopsDeatail.push({
            ShipmentRouteStopId: glbRouteStops[i].ShipmentRouteStopId,
            RouteNo: glbRouteStops[i].RouteNo,
            PickupLocationId: glbRouteStops[i].PickupLocationId,
            PickupLocation: glbRouteStops[i].PickupLocationText,
            PickDateTime: glbRouteStops[i].PickDateTime,
            PickDateTimeTo: glbRouteStops[i].PickDateTimeTo,
            DeliveryLocationId: glbRouteStops[i].DeliveryLocationId,
            DeliveryLocation: glbRouteStops[i].DeliveryLocationText,
            DeliveryDateTime: glbRouteStops[i].DeliveryDateTime,
            DeliveryDateTimeTo: glbRouteStops[i].DeliveryDateTimeTo,
            // Comment: glbRouteStops[i].Comment,
            IsDeleted: glbRouteStops[i].IsDeleted,
            IsAppointmentRequired: glbRouteStops[i].IsAppointmentRequired,
            IsPickUpWaitingTimeRequired: glbRouteStops[i].IsPickUpWaitingTimeRequired,
            IsDeliveryWaitingTimeRequired: glbRouteStops[i].IsDeliveryWaitingTimeRequired,
        });

        if (glbRouteStops[i].IsAppointmentNeeded == true && glbRouteStops[i].IsAppointmentRequired == false) {
            values.StatusId = 10;
        }
    }
    values.ShipmentRoutesStop = routeStopsDeatail;


    var FreightDetails = [];
    for (var i = 0; i < glbFreightDetail.length; i++) {

        FreightDetails.push({
            ShipmentFreightDetailId: glbFreightDetail[i].FreightDetailId,
            RouteNo: glbFreightDetail[i].RouteNo,
            PickupLocationId: glbFreightDetail[i].PickupLocationId,
            DeliveryLocationId: glbFreightDetail[i].DeliveryLocationId,
            Commodity: glbFreightDetail[i].Commodity,
            PricingMethodId: glbFreightDetail[i].PricingMethodId,
            PricingMethod: glbFreightDetail[i].pricingmethodtext,
            FreightTypeId: glbFreightDetail[i].FreightTypeId,
            FreightType: glbFreightDetail[i].freighttypetext,
            Hazardous: glbFreightDetail[i].Hazardous == 1 ? true : false,
            Temperature: glbFreightDetail[i].Temperature,
            TemperatureType: glbFreightDetail[i].TemperatureType,
            QutWgtVlm: glbFreightDetail[i].QutWgtVlm,
            IsDeleted: glbFreightDetail[i].IsDeleted,
            MinFee: glbFreightDetail[i].MinFee,
            UpTo: glbFreightDetail[i].UpTo,
            UnitPrice: glbFreightDetail[i].UnitPrice,
            TotalPrice: glbFreightDetail[i].TotalPrice,
            NoOfBox: glbFreightDetail[i].NoOfBox,
            Weight: glbFreightDetail[i].Weight,
            Unit: glbFreightDetail[i].Unit,
            Comments: glbFreightDetail[i].Comments,
            TrailerCount: glbFreightDetail[i].TrailerCount,
            IsPartialShipment: glbFreightDetail[i].IsPartialShipment,
            PartialBox: glbFreightDetail[i].PartialBox,
            PartialPallet: glbFreightDetail[i].PartialPallet,
        })
    }
    values.ShipmentFreightDetail = FreightDetails;
    values.AccessorialPrice = glbAccessorialFee;



    return values;
}
//#endregion

//#region save function for create Shipment
var btnSave = function () {

    $("#btnSave").on("click", function () {
        var values = {};
        values = GetJsonValue();
        var data = new FormData();
        if (glbDamageFile.length) {
            for (let i = 0; i < glbDamageFile.length; i++) {
                data.append("DamagedFiles", glbDamageFile[i].DamageImage);
            }
        }

        var mendetory = false;
        var message = "";




        if (mendetory == false && (values.StatusId == 3 || values.StatusId == 4)) {

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
            //glbAccessorialFee
            if (validateContact()) {

                //#region code for edit Quote 


                if (values.ShipmentId > 0) {

                    $.ajax({
                        url: baseUrl + "/Shipment/Shipment/EditShipment",
                        type: "POST",
                        beforeSend: function () {
                            showLoader();
                        },
                        data: JSON.stringify(values),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",

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
                                                window.location.href = baseUrl + "/Shipment/Shipment/ViewShipmentList";
                                            }
                                        },
                                    }
                                });
                            }
                            else {
                                hideLoader();
                                AlertPopup(response.Message);
                            }
                        },
                        error: function () {
                            hideLoader();
                            AlertPopup("Something went wrong.");
                        }
                    });
                }
                else {

                    if (glbFreightDetail.length > 0) {
                        $.ajax({
                            url: baseUrl + "/Shipment/Shipment/CreateShipment",
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
                                    $.alert({
                                        title: 'Success!',
                                        content: "<b>" + response.Message + "</b>",
                                        type: 'green',
                                        typeAnimated: true,
                                        buttons: {
                                            Ok: {
                                                btnClass: 'btn-green',
                                                action: function () {
                                                    window.location.href = baseUrl + "/Shipment/Shipment/ViewShipmentList";
                                                }
                                            },
                                        }
                                    });
                                }
                                else {
                                    hideLoader();
                                    AlertPopup(response.Message);
                                }
                            },
                            error: function () {
                                hideLoader();
                                AlertPopup("Something went wrong.");
                            }
                        });
                    }
                    else {
                        AlertPopup("Please add Freight Details to your shipment.");
                    }
                }
                //#endregion
            }
        }
    })
}
//#endregion
function ConvertFloat(el) {
    el.value = parseFloat(el.value).toFixed(2);
}
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
        // glbAccessorialFee = glbAccessorialFee.filter(x => x.RouteNo != radioValue && x.IsDeleted == false);
        //glbAccessorialFee = glbAccessorialFee.filter(x => x.RouteNo != radioValue && x.IsDeleted == false);
        //glbAccessorialFee = glbAccessorialFee.filter(x => x.ShipmentAccessorialPriceId != 0 && x.IsDeleted == false);
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
                        ShipmentAccessorialPriceId: 0,
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
                        ShipmentAccessorialPriceId: 0,
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
                        ShipmentAccessorialPriceId: 0,
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
    $("#txtTotalAccessorialchargs").val(parseFloat(totalAccessorialFee).toFixed(2));
}
//#endregion

//#region alert a message if route not selected and we add accessorial charges
function alertMessage(_this) {
    var radioValue = $("input[name='rdSelectedRoute']:checked").val();
    if (radioValue > 0) {
        return true;
    }
    else {
        $(_this).prop("checked", false);

        AlertPopup("Please select at least one Route Stop.")
        return false;
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

//#region temprature converter

var convertTemp = function () {
    var actualTemp;
    $("#ddlTemperatureUnit").on("change", function () {
        var unit = $(this).val();
        var temp = $("#txtTemperature").val();
        if (temp != '') {
            if (unit == 'C') {
                actualTemp = FahrenheitToCelsius(temp);
            }
            else {
                actualTemp = CelsiusToFahrenheit(temp);
            }

            $("#txtTemperature").val(actualTemp)
        }
    })
}
//#endregion

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



                if (radioValue > 0) {
                    var data = new FormData();
                    var damageFile = $("#fuDamageFiles")[0].files;

                    if (damageFile.length) {
                        for (let i = 0; i < damageFile.length; i++) {
                            data.append("DamageImage", damageFile[i]);
                        }
                    }
                    var damageFiles = glbRouteStops.filter(x => x.RouteNo == radioValue);
                    data.append("ImageDescription", damageDescription);
                    data.append("ShipmentRouteStopId", damageFiles[0].ShipmentRouteStopId);
                    if (damageFiles[0].ShipmentRouteStopId > 0) {

                        $.ajax({
                            type: "POST",
                            url: baseUrl + '/Shipment/Shipment/UploadDamageDocument',
                            contentType: false,
                            processData: false,
                            data: data,
                            async: false,
                            success: function (data, textStatus, jqXHR) {
                                if (data.IsSuccess == true) {
                                    getShipmentDetailById();

                                }
                                else {
                                    AlertPopup(data.Message);
                                }
                                getShipmentDetailById();
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

                tr += '<tr data-file-url=' + damageFiles[i].ImageUrl + ' ondblclick="javascript: ViewDamageDocument(this,' + damageFiles[i].DamageId + ',' + damageFiles[i].IsApproved + ');">' +
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
                btnClass: 'btn-blue',
                action: function () {
                    $.ajax({
                        url: baseUrl + 'Shipment/Shipment/DeleteDamageImage',
                        data: { DamageId: damageId },
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
                //btnClass: 'btn-red',
            }
        }
    })

}

//#region Button Upload Proof of Temp
var btnProofOfTemp = function () {
    $(".btnProofOfTemp").on("click", function () {
        if (isFormValid('divProofOfTemp')) {
            var fileUploader = $("#fuProofOfTemperature");
            var actualTemp = $("#txtActualTemp").val();
            var unit = $("#ddlActualTemperatureUnit").val();
            if (unit == 'C') {
                actualTemp = CelsiusToFahrenheit(actualTemp);
            }
            // Date Time Format 
            var d = new Date($.now());
            date = (d.getDate() + "-" + (d.getMonth() + 1) + "-" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds());
            //

            if (fileUploader.length) {

                var filesUploaded = fileUploader[0].files;

                var freightDetailId = $("input[name='rdSelectedFreight']:checked").val();
                if (freightDetailId > 0) {


                    var radioValue = $("input[name='rdSelectedRoute']:checked").val();
                    if (radioValue > 0) {
                        var data = new FormData();


                        if (filesUploaded.length) {
                            for (let i = 0; i < filesUploaded.length; i++) {
                                data.append("ProofOfTemprature", filesUploaded[i]);
                            }
                        }
                        var routeStops = glbRouteStops.filter(x => x.RouteNo == radioValue);

                        data.append("ActualTemperature", actualTemp);
                        data.append("ShipmentRouteStopId", routeStops[0].ShipmentRouteStopId);
                        data.append("FreightDetailId", freightDetailId);
                        $.ajax({
                            type: "POST",
                            url: baseUrl + '/Shipment/Shipment/UploadProofofTemperature',
                            contentType: false,
                            processData: false,
                            data: data,
                            async: false,
                            success: function (data, textStatus, jqXHR) {
                                if (data.IsSuccess == true) {

                                    getShipmentDetailById();

                                }
                                else {
                                    AlertPopup(data.Message);
                                }
                                getShipmentDetailById();
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
                    AlertPopup("Please click on SUBMIT button to save this Freight detail into database.Then you can upload image for this route.");
                }
                $("#txtActualTemp").val("");
                $("#fuProofOfTemperature").val(null);
                bindProofOfTempTbl();
            }

        }
    })
}
//#endregion

function bindProofOfTempTbl() {
    isApproveProofofTemp = false;
    var tr = "";
    $("#tblProofOfTemp tbody").empty();
    var freightDetailId = $("input[name='rdSelectedFreight']:checked").val();

    if (freightDetailId > 0) {
        var proofoftemp = glbProofOfTemprature.filter(x => x.FreightDetailId == freightDetailId);
        if (proofoftemp.length > 0) {
            for (var i = 0; i < proofoftemp.length; i++) {
                //var location = proofoftemp[i].IsLoading == true ? "Loading" : "Delivery";
                tr += '<tr data-file-url=' + proofoftemp[i].ImageUrl + ' ondblclick="javascript: ViewProofOfTemperature(this,' + proofoftemp[i].ProofOfTempratureId + ',' + proofoftemp[i].IsApproved + ');">' +
                    //'<td>' + (i + 1) + '</td>' +
                    '<td>' + proofoftemp[i].ActualTemperature + '</td>' +
                    //'<td>' + location + '</td>' +
                    //'<td>' + proofoftemp[i].ImageName + '</td>' +
                    '<td>' + proofoftemp[i].Date + '</td>' +
                    '<td><button type="button" class="delete_icon" onclick="remove_proofOfTemp_row(this,' + proofoftemp[i].ProofOfTempratureId + ')"> <i class="far fa-trash-alt"></i></button><button type="button" data-file-url=' + proofoftemp[i].ImageUrl + ' onclick="ViewProofOfTemperature(this,' + proofoftemp[i].ProofOfTempratureId + ',' + proofoftemp[i].IsApproved + ')" class="delete_icon"><i class="far fa-eye"></i></button> <a href=' + proofoftemp[i].ImageUrl + ' download title="download" class="edit_icon fileDownload" ><i class="fa fa-download"></i></a> &nbsp;'
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
                btnClass: 'btn-blue',
                action: function () {
                    $.ajax({
                        url: baseUrl + 'Shipment/Shipment/DeleteProofOfTemprature',
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
                //btnClass: 'btn-red',
            }
        }
    })

}

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
//#endregion

//#
function getUptoCount() {

    var minFee = $("#txtMinFee").val();
    var addPrice = $("#txtAddPrice").val();
    var uptoPrice = 0;
    if (minFee != "" && addPrice != "" && Number(addPrice) > 0) {
        uptoPrice = (Number(minFee) / Number(addPrice));
    }
    $("#txtUpTo").val(ConvertStringToFloat(uptoPrice));
}

function showhideFreightType(selectedtext) {
    if ($.trim(selectedtext).toLowerCase() == $.trim("Trailer Move").toLowerCase()) {

        $(".divTrailer").show();
        $(".divWeight").hide();
        $(".divBox").hide();
        $(".divNoOfPcs").hide();

        $("#txtWeight").val("");
        $("#txtNoOfBox").val("");

        $("#txtQutVlmWgt").val("");
        $("#ddlPricingMethod").val(5);
    }
    else {
        $(".divTrailer").hide();
        $(".divWeight").show();
        $(".divBox").show();
        $(".divNoOfPcs").show();


        $("#txtTrailer").val("");

    }

}

function freighttypeOnchange() {
    $("#ddlFreightType").change(function () {
        var selectedtext = $.trim($("#ddlFreightType").find('option:selected').text());
        var selectedValue = $("#ddlFreightType").find('option:selected').val()
        showhideFreightType(selectedtext);
        CalculateTotalPrice();
        BindUpTo();
        CheckQuoteFreightTypeChange(selectedValue, selectedtext)
    });
}

function HidePricingMethodField() {
    $(".divTrailer").hide();
    $(".divWeight").hide();
    $(".divBox").hide();
    $(".divNoOfPcs").hide();
}

//#region bindupto level on dropdown change
var ddlPricingMethod = function () {
    $("#ddlPricingMethod").change(function () {

        var pricingMethod = $("#ddlPricingMethod").find('option:selected').text();
        var pricingMethodValue = $("#ddlPricingMethod").find('option:selected').val();
        var arr = pricingMethod.split(/ (.*)/);
        $("#lblupto").text(arr[1]);

        if ($.trim(pricingMethod).toLowerCase() == $.trim("Per Pound").toLowerCase()) {
            $("#ddlUnit").val("LBS");
        }
        if ($.trim(pricingMethod).toLowerCase() == $.trim("Per Kg").toLowerCase()) {
            $("#ddlUnit").val("KG");
        }
        CalculateTotalPrice();
        CheckQuotePricingMethodChange(pricingMethodValue, pricingMethod)
    });
}
//#endregion

//#region bind upto level on page load
function BindUpTo() {

    var FreightType = $("#ddlPricingMethod").find('option:selected').text();
    var arr = FreightType.split(/ (.*)/);
    $("#lblupto").text(arr[1]);

}
//#endregion


//#region Redirect to shipment Details & GPS TRACKER 
var fn_RedirectToGPSTrackerPage = function () {

    var shipmentId = $("#hdnShipmentId").val();
    //$("#btnGpsShipment").on("click", function () {
    window.open(baseUrl + '/GpsTracker/GpsTracker/Index/' + shipmentId + ' ');
    //  location.href = baseUrl + '/GpsTracker/GpsTracker/Index/' + shipmentId;
    //})
}
//#endregion

function CheckImageExtension(_this) {

    var Isvalid = false;

    //var file = $('input[type="file"]').val();
    var file = _this.value;
    if (file != null && file != "") {
        var exts = ['jpg', 'jpeg', 'JPG', 'JPEG', 'png', 'pdf', 'PDF', 'JFIF', 'jfif','xlsx','csv'];
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
                AlertPopup('Uploads must be in JPG, JPEG, PNG, PDF, or JFIF formats.');
                $(_this).val(null);
                return Isvalid;
            }
        }
    }
    else {
        AlertPopup("Please Browse to select your Upload File.", "")
        return Isvalid;
    }

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


//#region Approve damage document
btnDamageDocument = function () {

    $("#btnDamageDocument").on("click", function () {
        $.confirm({
            title: 'Confirmation!',
            content: '<b>Are you sure want to approve this image?</b> ',
            type: 'red',
            typeAnimated: true,
            buttons: {
                confirm: {
                    btnClass: 'btn-blue',
                    action: function () {

                        var damageId = $("#damageDocumentId").val();
                        $.ajax({
                            url: baseUrl + '/Shipment/Shipment/ApprovedDamageImage',
                            data: { damageId: damageId },
                            type: "Get",
                            //async: false,
                            contentType: "application/json; charset=utf-8",
                            //dataType: "json",
                            success: function (data) {
                                $("#modalDocument").modal("hide");
                                //if (data == true) {
                                //    setTimeout(function () {
                                //        getShipmentDetailById();
                                //    }, 1000)

                                //}
                                //else {
                                //    //toastr.error(data.Message);
                                //}
                                getShipmentDetailById();
                            },
                            error: function () {
                                getShipmentDetailById();
                            }
                        });
                    }
                },
                cancel: {
                    //btnClass: 'btn-red',
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
            type: 'red',
            typeAnimated: true,
            buttons: {
                confirm: {
                    btnClass: 'btn-blue',
                    action: function () {
                        
                        var tempId = $("#proofOfTempratureId").val();
                        $.ajax({
                            url: baseUrl + '/Shipment/Shipment/ApprovedProofOFTemp',
                            data: { ProofOfTempratureId: tempId },
                            type: "Get",
                            //async: false,
                            contentType: "application/json; charset=utf-8",
                            //dataType: "json",
                            success: function (data) {
                                $("#modalDocument").modal("hide");
                                //if (data == true) {

                                //    //toastr.success(data.Message);
                                //    setTimeout(function () {
                                //        getShipmentDetailById();
                                //    }, 1000)
                                //}
                                //else {
                                //    //toastr.error(data.Message);
                                //}
                                getShipmentDetailById();
                            },
                            error: function () {
                                getShipmentDetailById();
                            }
                        });
                    }
                },
                cancel: {
                    //btnClass: 'btn-red',
                }
            }
        })

    })
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

$("#chkPartialShipment").click(function () {
    var ischecked = $(this).is(':checked');
    if (!ischecked) {
        $(".divPartial").hide();

    }
    else {
        $(".divPartial").show();
    }

});


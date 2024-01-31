//global array for accessorial charges
var glbAccessorialCharges = [];

//global array for freight detail
var glbFreightDetail = [];

//global array for route stops
var glbRouteStops = [];

//global function for accessorial charges
var glbAccessorialFee = [];
var contactInfoCounts = 0;
var isNeedToloaded = true;
$("table").on("mouseover", 'tr', function () {

    $(this).find(".far").css('color', 'white');
    $(this).find(".fa-map-marked-alt").css('color', 'white');
});

$("table").on("mouseout", 'tr', function () {

    $(this).find(".far").css('color', '#007bff');
    $(this).find(".fa-download").css('color', '#007bff');
    $(this).find(".fa-map-marked-alt").css('color', '007bff');
    $(this).find(".fa-trash-alt").css('color', 'red');

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


$(function () {


    //function for save quote detail
    btnSave();
    //function for bind commodity dropdown list
    GetCommodityList();

    //function for bind pricing method dropdown list
    GetPricingMethod();


    //bind accessorial fee UI
    bindAccessorialfeeType();

    //function for bind for freight type dropdown list.
    GetFreightType();

    //function for bind upto to level on dropdown change
    ddlPricingMethod();

    //function for apply selectize on customer dropdown
    bindCustomerDropdown();
    //function for pickup location dropdown
    bindPickupLocation();

    //function for drop location dropdown
    bindDeliveryLocation();

    //function for open address popup
    openAddressModal();

    //function for add route stopes
    btnAddRouteStop();

    //function for add freight detail
    btnAddFreight();

    ////function for add accessorial charges
    //btnAccessorialchargs();

    //accessorial check box
    chkAccessorialchargs();
    ////function for a
    //chkAccessorialCharges();

    //function for swap location
    swaplocation();

    //clear freight field on button click
    btnClearFreight();

    //bind date on Quote an page load
    binddate();

    //get quote detail by id for edit
    getQuoteDetailById();

    //Quote Preview : This code will be use in future
    btnPreview();


    //SendQuote
    btnSend();

    //function calculate up to price
    calculatefinalamount();

    //function hide and show freight type
    HidePricingMethodField();

    //freight type on change
    freighttypeOnchange();
});



//#region get quote detail by id for edit
getQuoteDetailById = function () {

    var url = window.location.pathname;
    var quoteId = url.substring(url.lastIndexOf('/') + 1);
    if (quoteId > 0) {
        $.ajax({
            url: baseUrl + "/Quote/Quote/GetQuoteById",
            type: "GET",
            data: { quoteId: quoteId },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {

                if (response != null) {

                    //#region bind quote detail
                    $("#hdnQuoteId").val(response.QuoteId);
                    contactInfoCounts = response.ContactInfoCount;
                    var ddlCustomer = "<option selected='selected' value=" + response.CustomerId + ">" + response.CustomerName + "</option>";
                    $("#txtCustomer").empty();
                    $("#txtCustomer").append(ddlCustomer);
                    $(".txtCustomer").text(response.CustomerName);

                    $("#txtEmailId").val(response.Email);
                    $("#txtPhone").val(response.Phone);
                    $("#txtQuoteName").val(response.QuotesName);
                    $("#dtQuoteDate").val(ConvertDate(response.QuoteDate));
                    $("#dtValiedUpToDate").val(ConvertDate(response.ValidUptoDate));
                    $("#txtFinalTotalAmount").val(response.FinalTotalAmount);
                    $("#ddlQuoteStatusId").val(response.QuoteStatusId);
                    //#endregion

                    //#region for bind route stops
                    for (var i = 0; i < response.RouteStops.length; i++) {
                        var objRouteStops = {};
                        objRouteStops.RouteStopId = response.RouteStops[i].RouteStopId;
                        objRouteStops.RouteNo = response.RouteStops[i].RouteNo;
                        objRouteStops.PickupLocationId = response.RouteStops[i].PickupLocationId;
                        objRouteStops.PickupLocationText = response.RouteStops[i].PickupLocation;
                        objRouteStops.DeliveryLocationId = response.RouteStops[i].DeliveryLocationId;
                        objRouteStops.DeliveryLocationText = response.RouteStops[i].DeliveryLocation;
                        objRouteStops.PickDateTime = response.RouteStops[i].PickDateTime == null ? "" : ConvertDate(response.RouteStops[i].PickDateTime, true);
                        objRouteStops.DeliveryDateTime = response.RouteStops[i].DeliveryDateTime == null ? "" : ConvertDate(response.RouteStops[i].DeliveryDateTime, true);
                        glbRouteStops.push(objRouteStops);

                    }

                    bindRoutetable();
                    checkRadionButton();
                    //#endregion

                    //#region for bind shipment detail

                    for (var i = 0; i < response.ShipmentDetail.length; i++) {
                        var objFreightDetail = {};
                        objFreightDetail.FreightDetailId = response.ShipmentDetail[i].FreightDetailId;
                        objFreightDetail.RouteNo = response.ShipmentDetail[i].RouteNo;
                        objFreightDetail.PickupLocationId = response.ShipmentDetail[i].PickupLocationId;
                        objFreightDetail.DeliveryLocationId = response.ShipmentDetail[i].DeliveryLocationId;
                        objFreightDetail.commoditytext = response.ShipmentDetail[i].Commodity;


                        objFreightDetail.freighttypetext = response.ShipmentDetail[i].FreightType;
                        objFreightDetail.FreightTypeId = response.ShipmentDetail[i].FreightTypeId;

                        objFreightDetail.pricingmethodtext = response.ShipmentDetail[i].PricingMethod;
                        objFreightDetail.PricingMethodId = response.ShipmentDetail[i].PricingMethodId;

                        objFreightDetail.Hazardous = response.ShipmentDetail[i].Hazardous;
                        objFreightDetail.Temperature = response.ShipmentDetail[i].Temperature;

                        objFreightDetail.MinFee = response.ShipmentDetail[i].MinFee;
                        objFreightDetail.UpTo = response.ShipmentDetail[i].UpTo;

                        objFreightDetail.QutVlmWgt = response.ShipmentDetail[i].QutVlmWgt;
                        objFreightDetail.UnitPrice = response.ShipmentDetail[i].UnitPrice;

                        objFreightDetail.TotalPrice = response.ShipmentDetail[i].TotalPrice;

                        objFreightDetail.NoOfBox = response.ShipmentDetail[i].NoOfBox;
                        objFreightDetail.Weight = response.ShipmentDetail[i].Weight;

                        if (ddlUnit != "" && ddlUnit != undefined) {
                            objFreightDetail.Unit = response.ShipmentDetail[i].Unit;
                        }


                        objFreightDetail.TrailerCount = response.ShipmentDetail[i].TrailerCount;

                        glbFreightDetail.push(objFreightDetail);
                    }
                    bindshipmenttable();

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

                    $('#btnSend').prop('disabled', false);
                }
            },
            error: function () {

            }
        });
    }

}
//#endregion

//#region bind route 
bindRoutetable = function () {
    
    var dtRouteBody = "";
    $("#dtRouteBody").empty();
    var sequenceNo = 0;
    for (var i = 0; i < glbRouteStops.length; i++) {
        sequenceNo = sequenceNo + 1;
        dtRouteBody += "<tr>" +
            "<td> <input type='radio' name='rdSelectedRoute' onchange='checkdata()' value='" + glbRouteStops[i].RouteNo + "' /> </td>" +
            "<td><label name='lblSrNo'>" + sequenceNo + "</label> </td>" +
            "<td>" +
            "<label name='lblPickupLocationId' style='display:none'>" + glbRouteStops[i].PickupLocationId + "</label>" +
            "<label name='lblPickupLocationText' data-toggle='tooltip' data-placement='top' title='" + GetAddress(glbRouteStops[i].PickupLocationText) + "' >" + GetCompanyName(glbRouteStops[i].PickupLocationText) + "</label>" +
            "</td>" +
            "<td>" +
            "<label name='lblPickupDate'>" + glbRouteStops[i].PickDateTime + "</label>" +
            "</td>" +
            "<td>" +
            "<label name='lblDeliveryLocationId' style='display:none'>" + glbRouteStops[i].DeliveryLocationId + " </label>" +
            "<label name='lblDeliveryLocationText' data-toggle='tooltip' data-placement='top' title='" + GetAddress(glbRouteStops[i].DeliveryLocationText) + "' >" + GetCompanyName(glbRouteStops[i].DeliveryLocationText) + "</label>" +
            "</td>" +
            "<td>" +
            "<label name='lblDeliveryDate'>" + glbRouteStops[i].DeliveryDateTime + "</label>" +
            "</td>" +
            "<td>" +
            "<button type='button' class='delete_icon' onclick='remove_row_from_route(this)'> <i class='far fa-trash-alt'></i> </button>" +
            "</td>" +
            "</tr>";
    }
    $("#dtRouteBody").append(dtRouteBody);
}
//#endregion


//#region select date  in Quote Date and Valid Thru
var binddate = function () {

    var todaydate = new Date();

    var quotemonth = todaydate.getMonth() + 1;

    var quotedate = todaydate.getDate();

    var quoteDate = (quotemonth < 10 ? ("0" + quotemonth) : quotemonth) + "-" + (quotedate < 10 ? ("0" + quotedate) : quotedate) + "-" + todaydate.getFullYear();


    var validthru = addMonths(todaydate, 1)

    var validmonth = validthru.getMonth() + 1;

    var validdate = validthru.getDate();

    var validThrudate = (validmonth < 10 ? ("0" + validmonth) : validmonth) + "-" + (validdate < 10 ? ("0" + validdate) : validdate) + "-" + validthru.getFullYear();

    $("#dtQuoteDate").val(quoteDate);
    $("#dtValiedUpToDate").val(validThrudate);
}
//#endregion

//#region add one month
function addMonths(date, count) {

    if (date && count) {
        var m, d = (date = new Date(+date)).getDate()
        date.setMonth(date.getMonth() + count, 1)
        m = date.getMonth()
        date.setDate(d)
        if (date.getMonth() !== m) date.setDate(0)
    }
    return date
}
//#endregion
function GetMaxRouteNo(quoteId) {    var maxRouteNo = 0;
    $.ajax({
        url: baseUrl + '/Quote/Quote/GetMaxRouteNo',
        data: { "quoteId": quoteId },
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

//#region add route stopage in table
var addRouteStops = function () {

    var pickupLocationId = $.trim($("#ddlPickupLocation").val());
    var pickupLocationText = $.trim($("#ddlPickupLocation").find('option:selected').text());
    var deliveryLocationId = $.trim($("#ddlDeliveryLocation").val());
    var deliveryLocationText = $.trim($("#ddlDeliveryLocation").find('option:selected').text());

    var dtPickup = $.trim($("#dtArrivalDate").val());
    var dtDelivery = $.trim($("#dtDeliveryDate").val());

    //  var tblRowsCount = $("#dtRouteBody tr").length + 1;
    var tblRowsCount = 0

    var quoteId = $("#hdnQuoteId").val();

    if (quoteId > 0) {
        tblRowsCount = GetMaxRouteNo(quoteId) + 1;
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

    var routeStopId = 0;

    var objRouteStops = {};

    objRouteStops.RouteStopId = routeStopId;
    objRouteStops.RouteNo = tblRowsCount;
    objRouteStops.PickupLocationId = pickupLocationId;
    objRouteStops.PickupLocationText = pickupLocationText;
    objRouteStops.DeliveryLocationId = deliveryLocationId;
    objRouteStops.DeliveryLocationText = deliveryLocationText;
    objRouteStops.PickDateTime = dtPickup;
    objRouteStops.DeliveryDateTime = dtDelivery;
    glbRouteStops.push(objRouteStops);

    //bind route table
    bindRoutetable();

    //select default first radion button 
    checkRadionButton();

    manageLocation("ddlPickupLocation");
    manageLocation("ddlDeliveryLocation");
    $.alert({
        title: 'Success!',
        content: "<b>Route Added Successfully.<br/> Don't forget to click on Get Quote button.</b>",
        type: 'green',
        typeAnimated: true,
    });
}
//#endregion 

//#region validate  delivery date
function CheckDeliveryDate() {

    var dd = $("#dtDeliveryDate").val();
    var pd = $("#dtArrivalDate").val();
    if (pd != "" && dd != "") {
        if ($("#dtDeliveryDate").val() > $("#dtArrivalDate").val()) {

        }
        else {
            //toastr.warning("The Pickup Arrival and the Delivery Arrival cannot be the same.")
            AlertPopup("The Pickup Arrival and the Delivery Arrival cannot be the same.")
            $("#dtDeliveryDate").val("");

        }

    }
}
//#endregion

//#region validate  pickup date
function CheckPickUpDate() {

    var dd = $("#dtDeliveryDate").val();
    var pd = $("#dtArrivalDate").val();
    if (pd != "" && dd != "") {
        if ($("#dtDeliveryDate").val() > $("#dtArrivalDate").val()) {

        }
        else {
            //toastr.warning("The Pickup Arrival and the Delivery Arrival cannot be the same.")
            AlertPopup("The Pickup Arrival and the Delivery Arrival cannot be the same.")
            $("#dtArrivalDate").val("");
        }

    }
}
//#endregion

//#region validate  Quote Date and Valid Thru
function CheckQuoteDate() {

    var quoteDate = $("#dtQuoteDate").val();
    var validThru = $("#dtValiedUpToDate").val();
    if (quoteDate != "" && validThru != "") {
        if (new Date(validThru) > new Date(quoteDate) || new Date(validThru) == new Date(quoteDate)) {

        }
        else {
           // toastr.warning("The date is not valid.")
            AlertPopup("The date is not valid.")
            $("#dtQuoteDate").val("");
        }

    }
}
function CheckValidDate() {

    var quoteDate = $("#dtQuoteDate").val();
    var validThru = $("#dtValiedUpToDate").val();
    if (quoteDate != "" && validThru != "") {
        if (new Date(validThru) > new Date(quoteDate) || new Date(validThru) == new Date(quoteDate)) {

        }
        else {
          //  toastr.warning("The date is not valid.")
            AlertPopup("The date is not valid.")
            $("#dtValiedUpToDate").val("");
        }

    }
}
//#endregion

//#region Find and remove route and detail
function remove_row_from_route(_this) {
    
    $.confirm({
        title: 'Confirmation!',
        content: "<b>Are you sure you want to delete? <br/>  Don't forget to click on the Submit button to save all changes.</b> ",
        type: 'red',
        typeAnimated: true,
        buttons: {
            delete: {
                btnClass: 'btn-green',
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
                    glbRouteStops = $.grep(glbRouteStops, function (value) {
                        return value.RouteNo != deletedrow;
                    });

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

                    //calculate accessorial charges
                    btnCalculatetotalFee();

                    //Calculate total final amount
                    calculatefinalamount();
                }
            },
            cancel: {
                btnClass: 'btn-red',
            }
        }
    })




}
//#endregion

//#region select default first radion button
function checkRadionButton() {
    if ($("#dtRouteBody tr").length > 0) {
        $($("#dtRouteBody tr")[0]).find("input[type=radio]").prop("checked", true);

        //$("input[type=radio]").prop("checked", true);
    }
}
//#endregion

//#region show and hide shipment detail on the baseis of 
function checkdata() {
    //bind shipment table
    bindshipmenttable();

    if (glbAccessorialFee.length > 0) {
        getAccessorialPrice();
    }
    calculatefinalamount();
}
//#endregion

//#region calculate total Quantity
var totalQuantity = function () {
    var radioValue = $("input[name='rdSelectedRoute']:checked").val();
    var totalQuantity = 0;
    for (var i = 0; i < glbFreightDetail.length; i++) {
        if (radioValue == glbFreightDetail[i].RouteNo) {
            totalQuantity = Number(totalQuantity) + Number(glbFreightDetail[i].QutVlmWgt);
        }
    }
    return totalQuantity;
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
            // var commoditytext = $("#ddlCommodity").find('option:selected').text();
            //var commodityvalue = $("#ddlCommodity").val();

            var freighttypetext = $("#ddlFreightType").find('option:selected').text();
            var freighttypevalue = $("#ddlFreightType").val();

            var pricingmethodtext = $("#ddlPricingMethod").find('option:selected').text();
            var pricingmethodvalue = $("#ddlPricingMethod").val();

            var minfee = $("#txtMinFee").val();
            var upto = $("#txtUpTo").val();
            var addfee = $("#txtAddPrice").val();
            var QutVlmWgt = $("#txtQutVlmWgt").val();
            var hazardous = $("#ddlHazardous").val();
            var temperature = $("#txtTemperature").val();
            var totalprice = $("#txtTotalPrice").val();

            var weight = $("#txtWeight").val();
            var ddlUnit = $("#ddlUnit").val();
            var noOfBox = $("#txtNoOfBox").val();
            var trailerCount = $("#txtTrailer").val();

            if ($("#tblShipmentDetail").attr("data-row-no") > 0) {
                //Edit array 
                var tblRowsCount = $("#tblShipmentDetail").attr("data-row-no");
                var rowindex = Number(tblRowsCount) - 1;
                var freightDetail = glbFreightDetail[rowindex];


                freightDetail.RouteNo = radioValue;
                freightDetail.PickupLocationId = pickupLocationId;
                freightDetail.DeliveryLocationId = deliveryLocationId;
                freightDetail.commoditytext = commoditytext;


                freightDetail.freighttypetext = freighttypetext;
                freightDetail.FreightTypeId = freighttypevalue;

                freightDetail.pricingmethodtext = pricingmethodtext;
                freightDetail.PricingMethodId = pricingmethodvalue;

                freightDetail.Hazardous = hazardous;
                freightDetail.Temperature = temperature;

                freightDetail.MinFee = minfee;
                freightDetail.UpTo = upto;
                freightDetail.UnitPrice = addfee;

                freightDetail.QutVlmWgt = QutVlmWgt;
                freightDetail.UnitPrice = addfee;

                freightDetail.TotalPrice = totalprice;

                freightDetail.NoOfBox = noOfBox;
                freightDetail.Weight = weight;
                freightDetail.Unit = ddlUnit;
                freightDetail.TrailerCount = trailerCount;
                $.alert({
                    title: 'Success!',
                    content: "<b>Data Successfully Updated.<br/> Don't forget to click on Get Quote button.</b>",
                    type: 'green',
                    typeAnimated: true,
                });

                $("#tblShipmentDetail").attr("data-row-no", 0);
                $("#btnAddFreight").text("Add");

                //calculate accessorial charges
                btnCalculatetotalFee();
                calculatefinalamount();
            }
            else {
                //Add in array
                var objFreightDetail = {};
                objFreightDetail.FreightDetailId = 0;
                objFreightDetail.RouteNo = radioValue;
                objFreightDetail.PickupLocationId = pickupLocationId;
                objFreightDetail.DeliveryLocationId = deliveryLocationId;
                objFreightDetail.commoditytext = commoditytext;


                objFreightDetail.freighttypetext = freighttypetext;
                objFreightDetail.FreightTypeId = freighttypevalue;

                objFreightDetail.pricingmethodtext = pricingmethodtext;
                objFreightDetail.PricingMethodId = pricingmethodvalue;

                objFreightDetail.Hazardous = hazardous;
                objFreightDetail.Temperature = temperature;

                objFreightDetail.MinFee = minfee;
                objFreightDetail.UpTo = upto;

                objFreightDetail.QutVlmWgt = QutVlmWgt;
                objFreightDetail.UnitPrice = addfee;

                objFreightDetail.TotalPrice = totalprice;

                objFreightDetail.NoOfBox = noOfBox;
                objFreightDetail.Weight = weight;
                objFreightDetail.Unit = ddlUnit;
                objFreightDetail.TrailerCount = trailerCount;

                glbFreightDetail.push(objFreightDetail);
                $.alert({
                    title: 'Success!',
                    content: "<b>Data Successfully Added.<br/> Don't forget to click on Get Quote button.</b>",
                    type: 'green',
                    typeAnimated: true,
                });
            }

            //Clear text box after submit
            ClearAddFreightDetail();


            //bind shipment table
            bindshipmenttable();

            //calculate accessorial charges
            btnCalculatetotalFee();
            calculatefinalamount();
        }
        else {
            //toastr.warning("Please select at least one Route Stop.")
            AlertPopup("Please select at least one Route Stop.")
        }
    }
    else {
        //toastr.warning("Please add at least one Pickup and Delivery location.")
        AlertPopup("Please add at least one Pickup and Delivery location.")
    }
}
//#endregion

//#region bind shipment table

function bindshipmenttable() {

    var shipmenttabledata = "";
    var radioValue = $("input[name='rdSelectedRoute']:checked").val();
    var srno = 1;
    var totalPrice = 0;
    $("#tblShipmentDetail tbody").empty();
    for (var i = 0; i < glbFreightDetail.length; i++) {
        if (radioValue == glbFreightDetail[i].RouteNo) {

            var Qty = "";
            if (glbFreightDetail[i].QutVlmWgt > 0) {
                Qty = glbFreightDetail[i].QutVlmWgt + " Pallet, ";
            }
            if (glbFreightDetail[i].NoOfBox > 0) {
                Qty += glbFreightDetail[i].NoOfBox + " Box, ";
            }
            if (glbFreightDetail[i].Weight != "" || glbFreightDetail[i].Weight != "") {
                Qty += glbFreightDetail[i].Weight + " " + glbFreightDetail[i].Unit + ", ";
            }
            if (glbFreightDetail[i].TrailerCount > 0) {
                Qty += glbFreightDetail[i].TrailerCount + " Trailer";
            }
            Qty = Qty.replace(/(^\s*,)|(,\s*$)/g, '');

            shipmenttabledata += "<tr align='center' ondblclick='javascript: edit_from_shipment(" + i + ");'>"
                + "<td><label name='serialno'>" + srno + "</label></td>"
                //+ "<td><label name = 'pickNdropRadiobtValue'>" + glbFreightDetail[i].RouteNo + "</label></td>"
                + "<td><label name='commoditytext'>" + glbFreightDetail[i].commoditytext + "</label></td>"
                + "<td><label name='freighttypetext'>" + glbFreightDetail[i].freighttypetext + "</label></td>"
                + "<td><label name='pricingmethodtext'>" + glbFreightDetail[i].pricingmethodtext + "</label></td>"
                + "<td  align='right'><label name='minfee'>" + glbFreightDetail[i].MinFee + "</label></td>"
                + "<td  align='right'><label name='addfee'>" + glbFreightDetail[i].UnitPrice + "</label></td>"
                + "<td  align='right'><label name='upto'>" + glbFreightDetail[i].UpTo + "</label></td>"
                //+ "<td  align='right'><label name='QutVlmWgt'>" + glbFreightDetail[i].QutVlmWgt + "</label></td>"
                //+ "<td  align='right'>" + glbFreightDetail[i].Weight + "</td>"
                //+ "<td  align='right'>" + glbFreightDetail[i].NoOfBox + "</td>"
                + "<td  align='right'>" + Qty + "</td>"
                + "<td  align='right'><label name='totalprice'>" + glbFreightDetail[i].TotalPrice + "</label></td>"
                + "<td><button type='button' class='edit_icon' onclick='edit_from_shipment(" + i + ")'> <i class='far fa-edit'></i> </button>|<button type='button' class='delete_icon' onclick='remove_row_from_shipment(" + i + ")'> <i class='far fa-trash-alt'></i> </button></td>"
                + "</tr>"
            srno = srno + 1;
            totalPrice = parseFloat(totalPrice) + parseFloat(glbFreightDetail[i].TotalPrice);
        }
    }
    if (totalPrice > 0) {
        shipmenttabledata += "<tr><td align='right' colspan='9'>Net Total</td><td  align='right'>" + totalPrice + "</td><td></td></tr>"
    }

    $("#tblShipmentDetail tbody").append(shipmenttabledata);

}
//#endregion

//#region function to remove .00 from quantity

function escapeRegExp(string) {
    return string.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
}

/* Define functin to find and replace specified term with replacement string */
function replaceAll(str, term, replacement) {
    return str.replace(new RegExp(escapeRegExp(term), 'g'), replacement);
}

//#endregion

//#region calcultate final amount
var calculatefinalamount = function () {
    var finalAmount = 0;


    for (var i = 0; i < glbFreightDetail.length; i++) {
        finalAmount = Number(finalAmount) + Number(glbFreightDetail[i].TotalPrice);
    }

    //for (var i = 0; i < glbAccessorialCharges.length; i++) {
    //    finalAmount = Number(finalAmount) + Number(glbAccessorialCharges[i].TotalAssessorialFee);
    //}

    var accessorialChargesDatabyRouteId = glbAccessorialFee.filter(x => x.IsDeleted == false);

    $.each(accessorialChargesDatabyRouteId, function (key, value) {
        finalAmount = Number(finalAmount) + Number(value.Amount);
    })


    $("#txtFinalTotalAmount").val(finalAmount);
}
//#endregion

//#region edit shipment detail by id
function edit_from_shipment(index) {

    var freightdetail = glbFreightDetail[index]
    $("#txtMinFee").val(freightdetail.MinFee);
    $("#txtUpTo").val(freightdetail.UpTo);
    $("#txtAddPrice").val(freightdetail.UnitPrice);
    $("#txtQutVlmWgt").val(freightdetail.QutVlmWgt == 0 ? "" : freightdetail.QutVlmWgt);
    $("#ddlHazardous").val(freightdetail.Hazardous == true ? "1" : "0");
    $("#txtTemperature").val(freightdetail.Temperature);
    $("#txtTotalPrice").val(freightdetail.TotalPrice);

    $("#txtCommodity").val(freightdetail.commoditytext);
    $("#ddlFreightType").val(freightdetail.FreightTypeId);
    $("#ddlPricingMethod").val(freightdetail.PricingMethodId);

    $("#txtWeight").val(freightdetail.Weight == 0 ? "" : freightdetail.Weight);
    if (freightdetail.Unit != "") {
        $("#ddlUnit").val(freightdetail.Unit);
    }

    $("#txtNoOfBox").val(freightdetail.NoOfBox == 0 ? "" : freightdetail.NoOfBox);
    $("#txtTrailer").val(freightdetail.TrailerCount == 0 ? "" : freightdetail.TrailerCount);

    showhideFreightType(freightdetail.freighttypetext);

    $("#tblShipmentDetail").attr("data-row-no", index + 1);
    $("#btnAddFreight").text("Update");
}
//#endregion

//#region remove row from shipment druing route deletion
function remove_shipment_row(_this) {

    row = $(_this).closest("tr");
    var deletedrow = $(row).find("input[name='rdSelectedRoute']").val();


    glbFreightDetail = $.grep(glbFreightDetail, function (value) {

        return value.RouteNo != deletedrow;
    });
    //bind shipment table
    bindshipmenttable();

    //calculate accessorial charges
    btnCalculatetotalFee();

    //Calculate total final amount
    calculatefinalamount();
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

//#region Find and remove selected table rows from Shipment table
function remove_row_from_shipment(index) {

    $.confirm({
        title: 'Confirmation!',
        content: "<b>Are you sure you want to delete? <br/> Don't forget to click on the Submit button to save all changes.</b> ",
        type: 'red',
        typeAnimated: true,
        buttons: {
            delete: {
                btnClass: 'btn-green',
                action: function () {
                    //Remove freight detail by id 
                    glbFreightDetail.splice(index, 1);

                    //Bind shipment detail
                    bindshipmenttable();

                    //manageRowsAfterDeletionShipment();

                    //calculate accessorial charges
                    btnCalculatetotalFee();

                    //Calculate total final amount
                    calculatefinalamount();
                }
            },
            cancel: {
                btnClass: 'btn-red',
            }
        }
    })


}
//#endregion

//#region add route stopes

var btnAddRouteStop = function () {
    $("#btnAddRouteStop").on("click", function () {

        if (validateLocation()) {
            if (contactInfoCounts > 0) {
                if (isFormValid("formRouteStop")) {
                    if ($("#ddlPickupLocation").val() > 0) {
                        if ($("#ddlDeliveryLocation").val() > 0) {
                            addRouteStops();
                            btnCalculatetotalFee();
                        }
                        else {
                            //toastr.warning("Please select a Delivery Location.")
                            AlertPopup("Please select a Delivery Location.")
                            ("Please add at least one Pickup and Delivery location.")
                        }
                    }
                    else {
                       // toastr.warning("Please select a Pickup Location.")
                        AlertPopup("Please select a Pickup Location.")
                    }

                }
            }
            else {
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

//#region add freight 
var btnAddFreight = function () {
    $("#btnAddFreight").on("click", function () {
        if (isFormValid("formAddFreight")) {
            if ($("#ddlPricingMethod").val() > 0) {
                AddShipmentDetail();
            }
            else {
                //toastr.warning("Please select a Pricing Method.")
                AlertPopup("Please select a Pricing Method.")
            }

        }
    });
}
//#endregion

//#region  freight clear
function ClearAddFreightDetail() {
    $("#formAddFreight input:text").val("");
    $("#ddlFreightType").val(1);
    $("#ddlPricingMethod").val(0);
    $("#ddlHazardous").val(0);
    $("#tblShipmentDetail").attr("data-row-no", 0);
   // $("#btnAddFreight").text("Add");
}
//#endregion

//#region clear freight on button click

btnClearFreight = function () {
    $("#btnClearFreight").on("click", function () {
        ClearAddFreightDetail();
    });
}
//#endregion


//#region validate pickup and delivery  location
function validateLocation() {

    var isvalid = true;
    var pickuplocationid = $("#ddlPickupLocation").val();
    var deliveryLocationid = $("#ddlDeliveryLocation").val();
    if (pickuplocationid == deliveryLocationid) {
       // toastr.warning("Please review your Pickup and Delivery Locations. These locations cannot be the same.");
        AlertPopup("Please review your Pickup and Delivery Locations. These locations cannot be the same.");
        isvalid = false;
    }
    return isvalid;
}
//#endregion

var activateDatatable = function () {
    $(".activateDatatable").DataTable();
}

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

//#region for open address popup
var openAddressModal = function () {
    $(".btnOpenAddressModal").on("click", function () {
        $('#formAddress').trigger("reset");


        $("#modalAddAddress").modal("show");
        $('#modalAddAddress').draggable();
    });
}
//#endregion

//#region function for apply selectize on customer dropdown
var bindCustomerDropdown = function () {
    $('#txtCustomer').selectize({
        createOnBlur: true,
        maxItems: 1,
        valueField: 'id',
        labelField: 'text',
        searchField: 'text',
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
                $("#txtEmailId").val(item.email);
                $("#txtPhone").val(item.phone);
                contactInfoCounts = item.ContactInfoCount;

                return '<div>' +
                    ('<span class="name txtCustomer" data-ContactInfoCount=' + item.ContactInfoCount + '>' + escape(item.text) + '</span>') +
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
            $('#txtCustomer').html("");
            $('#txtCustomer').append($("<option selected='selected'></option>").val(input).html(input))
        },
        onFocus: function () {
            var value = this.getValue();
            this.clear(true);
            this.unlock();
        }

    });
}
//#endregion

//#region get json value
function GetJsonValue() {
    var values = {};
    values.QuoteId = $("#hdnQuoteId").val();
    values.CustomerId = $("#txtCustomer").val();
    if (values.CustomerId > 0) {

        values.CustomerName = $("#txtCustomer").text();
    }
    else {
        values.CustomerName = $("#txtCustomer").val()
        values.CustomerId = 0;
    }
    values.Email = $("#txtEmailId").val();
    values.Phone = $("#txtPhone").val();
    values.QuotesName = $("#txtQuoteName").val();
    values.QuoteDate = $("#dtQuoteDate").val();
    values.ValidUptoDate = $("#dtValiedUpToDate").val();
    values.FinalTotalAmount = $("#txtFinalTotalAmount").val();
    values.QuoteStatusId = $("#ddlQuoteStatusId").val();
    values.QuoteStatus = $("#ddlQuoteStatusId").find('option:selected').text();

    var routeStopsDeatail = [];
    for (var i = 0; i < glbRouteStops.length; i++) {
        routeStopsDeatail.push({
            RouteStopId: glbRouteStops[i].RouteStopId,
            RouteNo: glbRouteStops[i].RouteNo,
            PickupLocationId: glbRouteStops[i].PickupLocationId,
            PickupLocation: glbRouteStops[i].PickupLocationText,
            PickDateTime: glbRouteStops[i].PickDateTime,
            DeliveryLocationId: glbRouteStops[i].DeliveryLocationId,
            DeliveryLocation: glbRouteStops[i].DeliveryLocationText,
            DeliveryDateTime: glbRouteStops[i].DeliveryDateTime,
        });
    }
    values.RouteStops = routeStopsDeatail;


    var FreightDetails = [];
    for (var i = 0; i < glbFreightDetail.length; i++) {
        FreightDetails.push({
            FreightDetailId: glbFreightDetail[i].FreightDetailId,
            RouteNo: glbFreightDetail[i].RouteNo,
            PickupLocationId: glbFreightDetail[i].PickupLocationId,
            DeliveryLocationId: glbFreightDetail[i].DeliveryLocationId,
            Commodity: glbFreightDetail[i].commoditytext,
            PricingMethodId: glbFreightDetail[i].PricingMethodId,
            PricingMethod: glbFreightDetail[i].pricingmethodtext,
            FreightTypeId: glbFreightDetail[i].FreightTypeId,
            FreightType: glbFreightDetail[i].freighttypetext,
            Hazardous: glbFreightDetail[i].Hazardous == 1 ? true : false,
            Temperature: glbFreightDetail[i].Temperature,
            MinFee: glbFreightDetail[i].MinFee,
            UpTo: glbFreightDetail[i].UpTo,
            QutVlmWgt: glbFreightDetail[i].QutVlmWgt,
            UnitPrice: glbFreightDetail[i].UnitPrice,
            TotalPrice: glbFreightDetail[i].TotalPrice,
            NoOfBox: glbFreightDetail[i].NoOfBox,
            Weight: glbFreightDetail[i].Weight,
            Unit: glbFreightDetail[i].Unit,
            TrailerCount: glbFreightDetail[i].TrailerCount
        })
    }
    values.ShipmentDetail = FreightDetails;

    values.QuoteAccessorialPrice = glbAccessorialFee;
    return values;
}
//#endregion

//#region Send Quote
btnSend = function () {
    $("#btnSend").on("click", function () {
        var quoteid = $("#hdnQuoteId").val();
        if (quoteid > 0) {

            $.ajax({
                url: baseUrl + '/Quote/Quote/SendQuoteEmail',
                data: { 'quoteid': quoteid },
                type: "GET",
                beforeSend: function () {
                    showLoader();
                },
                success: function (data) {
                    if (data.IsSuccess == true) {
                      //  toastr.success(data.Message, "")
                        SuccessPopup(data.Message)
                        AlertPopup
                        hideLoader();
                    }
                    else {
                       // toastr.error(data.Message, "");
                        AlertPopup(data.Message);
                        hideLoader();
                    }

                },
                complete: function () {
                    hideLoader();
                }
            });
        }
    });
}

//#endregion

//#region btn preview
btnPreview = function () {
    $("#btnPreview").on("click", function () {

        var values = {};
        values = GetJsonValue();

        if (validateContact()) {

            if (values.QuoteId > 0) {
                var url = baseUrl + "/Quote/Quote/ViewAndSendQuote/" + values.QuoteId;
                window.location.href = window.open(url, '_blank')
                return false;
            }
            else {


                $.ajax({
                    url: baseUrl + "/Quote/Quote/QuotePreview",
                    type: "POST",
                    data: JSON.stringify(values),
                    contentType: "application/json; charset=utf-8",
                    accepts: "html",
                    beforeSend: function () {
                        showLoader();
                    },
                    success: function (response) {

                        win = window.open("", "_blank");
                        win.document.body.innerHTML = response;;

                    },
                    error: function () {
                        hideLoader();
                    },
                    complete: function () {
                        hideLoader();
                    }
                });
            }
        }
    })
}
//#endregion

//Go BACK... Added on 08-Feb-2023
$("#btnGoBack").on("click", function () {
    window.location.href = baseUrl + "Quote/Quote/ViewQuote"
})
$("html").unbind().keyup(function (e) {
    console.log("Which Key: " + $(e.target) + " : " + document.getElementsByClassName("jconfirm").length + " : " + window.location.href.toLowerCase().indexOf("Index"));
    if (!$(e.target).is('input') && !$(e.target).is('textarea')) {
        console.log(e.which);
        //event.preventDefault();
        if (e.key === 'Backspace' || e.keyCode === 8) {
            //alert('backspace pressed');
            //window.location.href = baseUrl + "Shipment/Shipment/ViewShipmentList";
            if (document.getElementsByClassName("jconfirm").length == 0 && window.location.href.toLowerCase().indexOf("index") >= 0) {
                window.location.href = baseUrl + "Quote/Quote/ViewQuote";
            } else if (document.getElementsByClassName("jconfirm").length >= 1) {
                return;
            } else if (document.getElementsByClassName("jconfirm").length == 0 && window.location.href.toLowerCase().indexOf("index") < 0) {
                window.location.href = baseUrl + "Quote/Quote/ViewQuote";
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
                window.location.href = baseUrl + "Quote/Quote/ViewQuote";
            }
        }
    }
});
//
//

//#region save function for create Quotes
var btnSave = function () {
    $("#btnSave").on("click", function () {
        
        var values = {};
        values = GetJsonValue();

        if (validateContact()) {

            if (values.QuoteId > 0) {
                $.ajax({
                    url: baseUrl + "/Quote/Quote/EditQuote",
                    type: "POST",
                    data: JSON.stringify(values),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        isNeedToloaded = false;
                        if (response.IsSuccess) {
                            //toastr.success(response.Message);
                            //setInterval(function () {
                            //    window.location.href = baseUrl + "/Quote/Quote/ViewQuote";
                            //}, 1500)
                            $.alert({
                                title: 'Success!',
                                content: "<b>" + response.Message + "</b>",
                                type: 'green',
                                typeAnimated: true,
                                buttons: {
                                    Ok: {
                                        btnClass: 'btn-green',
                                        action: function () {
                                            window.location.href = baseUrl + "/Quote/Quote/ViewQuote";
                                        }
                                    },
                                }
                            });
                        }
                        else {
                           // toastr.error(response.Message);
                            AlertPopup(response.Message);
                        }
                    },
                    error: function () {

                    }
                });
            }
            else {
                $.ajax({
                    url: baseUrl + "/Quote/Quote/CreateQuote",
                    type: "POST",
                    data: JSON.stringify(values),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        isNeedToloaded = false;
                        if (response.IsSuccess) {
                            //toastr.success(response.Message);
                            //setInterval(function () {
                            //    window.location.href = baseUrl + "/Quote/Quote/ViewQuote";
                            //}, 1500)
                            $.alert({
                                title: 'Success!',
                                content: "<b>" + response.Message + "</b>",
                                type: 'green',
                                typeAnimated: true,
                                buttons: {
                                    Ok: {
                                        btnClass: 'btn-green',
                                        action: function () {
                                            window.location.href = baseUrl + "/Quote/Quote/ViewQuote";
                                        }
                                    },
                                }
                            });
                        }
                        else {
                           // toastr.error(response.Message);
                            AlertPopup(response.Message);
                        }
                    },
                    error: function () {

                    }
                });
            }
        }
    })
}
//#endregion

//#region bind  commodity dropdown list
function GetCommodityList() {
    $.ajax({
        url: baseUrl + 'Quote/Quote/GetCommodityList',
        data: {},
        type: "GET",
        async: true,
        // cache: false,
        success: function (data) {

            var ddlValue = "";
            $("#ddlCommodity").empty();

            // ddlValue += '<option value="0">Please Select Commodity</option>'
            for (var i = 0; i < data.length; i++) {
                ddlValue += '<option value="' + data[i].CommodityId + '">' + data[i].CommodityName + '</option>';
            }
            $("#ddlCommodity").append(ddlValue);
        }
    });
}
//#endregion

//#region bind freight type dropdownlist
function GetFreightType() {
    $.ajax({
        url: baseUrl + 'Quote/Quote/GetFreightType',
        data: {},
        type: "GET",
        async: true,
        // cache: false,
        success: function (data) {

            var ddlValue = "";
            $("#ddlFreightType").empty();

            ddlValue += '<option value="0">SELECT FREIGHT TYPE</option>'
            for (var i = 0; i < data.length; i++) {
                ddlValue += '<option value="' + data[i].FreightTypeId + '">' + data[i].FreightTypeName + '</option>';
            }
            $("#ddlFreightType").append(ddlValue);
        }
    });
}
//#endregion

function freighttypeOnchange() {
    $("#ddlFreightType").change(function () {
        

        var selectedtext = $.trim($("#ddlFreightType").find('option:selected').text());

        showhideFreightType(selectedtext);
        CalculateTotalPrice();
    });
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

function HidePricingMethodField() {
    $(".divTrailer").hide();
    $(".divWeight").hide();
    $(".divBox").hide();
    $(".divNoOfPcs").hide();
}
function ConvertFloat(el) {
    el.value = parseFloat(el.value).toFixed(2);
}
//#region bind pricing method dropdownlist
function GetPricingMethod() {
    $.ajax({
        url: baseUrl + 'Quote/Quote/GetPricingMehtod',
        data: {},
        type: "GET",
        async: true,
        // cache: false,
        success: function (data) {

            var ddlValue = "";
            $("#ddlPricingMethod").empty();

            ddlValue += '<option value="0">SELECT PRICING METHOD</option>'
            for (var i = 0; i < data.length; i++) {
                ddlValue += '<option value="' + data[i].PricingMethodId + '">' + $.trim(data[i].PricingMethodName).toUpperCase() + '</option>';
            }
            $("#ddlPricingMethod").append(ddlValue);
            BindUpTo();
        }
    });
}
//#endregion

//#region bindupto level on dropdown change
var ddlPricingMethod = function () {
    $("#ddlPricingMethod").change(function () {

        var FreightType = $("#ddlPricingMethod").find('option:selected').text();
        var arr = FreightType.split(/ (.*)/);
        $("#lblupto").text(arr[1]);
        ddlPricingMethodOnChange();
        CalculateTotalPrice();
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

//#region fill the field freight on change of freight
ddlPricingMethodOnChange = function () {

    var row = $("input[name='rdSelectedRoute']:checked").closest("tr")[0];

    var values = {};

    values.PickupLocationId = $.trim($(row).find("label[name='lblPickupLocationId']").text());
    values.DeliveryLocationId = $.trim($(row).find("label[name='lblDeliveryLocationId']").text());
    values.PricingMethodId = $.trim($("#ddlPricingMethod").val());
    values.FreightTypeId = $.trim($("#ddlFreightType").val());
    if ($("#dtRouteBody tr").length > 0) {
        var radioValue = $("input[name='rdSelectedRoute']:checked").val();

        if (radioValue > 0) {

            $("#txtMinFee").val("");
            $("#txtUpTo").val("");
            $("#txtAddPrice").val("");

            $.ajax({
                url: baseUrl + 'Quote/Quote/GetBaseFreightDetail',
                type: "POST",
                data: JSON.stringify(values),
                contentType: "application/json; charset=utf-8",
                async: true,
                success: function (data) {

                    if (data != null) {
                        $("#txtMinFee").val(data.MinFee);
                        $("#txtUpTo").val(data.UpTo);
                        $("#txtAddPrice").val(data.UnitPrice);
                    }
                    else {

                    }
                }
            });
        }
        else {
            //toastr.warning("Please select at least one Route Stop.")
            AlertPopup("Please select at least one Route Stop.")
            $("#ddlPricingMethod").val(0);
        }
    }
    else {
      //  toastr.warning("Please add at least one Pickup and Delivery location.")
        AlertPopup("Please add at least one Pickup and Delivery location.")
        $("#ddlPricingMethod").val(0);
    }
}
//#endregion

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
                        '<input class="col-md-1 chkunitfee" name="chkunitfee" id="chkunitfee_' + data[i].Id + '" type="checkbox" value=' + data[i].Id + ' />'
                    divunitfee += '<label class="col-sm-5 col-form-label" for="txtLoadingPerUnit">' + data[i].Name + '(' + data[i].PricingMethod + ')</label>'
                    divunitfee += '<input type="text" onchange="ConvertFloat(this)" onkeypress="return  onlyNumeric(event)" readonly=readonly  placeholder="Unit" id="txtUnit_' + data[i].Id + '" onfocusout="btnCalculatePerUnitFee(' + data[i].Id + ')" class="col-sm-1  form-control" />' +
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

//#region  add accessorial charges into array

var btnCalculatetotalFee = function () {
    
    var radioValue = $("input[name='rdSelectedRoute']:checked").val();

    if (radioValue > 0) {
        glbAccessorialFee = glbAccessorialFee.filter(x => x.RouteNo != radioValue && x.IsDeleted == false);

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

                glbAccessorialFee.push({
                    QuoteAccessorialPriceId: 0,
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
        });

        $("input[name=chkfixedfee]:checked").each(function () {
            var accessorialFeeTypeId = this.value;
            var fixedAmount = $("#txt_" + accessorialFeeTypeId + "").val();
            var accessorialChargesDatabyRouteId = glbAccessorialFee.filter(x => x.RouteNo == radioValue && x.AccessorialFeeTypeId == accessorialFeeTypeId && x.IsDeleted == false);

            if (accessorialChargesDatabyRouteId.length > 0) {
                accessorialChargesDatabyRouteId[0].Amount = fixedAmount;
            }
            else {

                glbAccessorialFee.push({
                    QuoteAccessorialPriceId: 0,
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
                glbAccessorialFee.push({
                    QuoteAccessorialPriceId: 0,
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
        });

        calculateTotalAccessorialAmount();
        calculatefinalamount();
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

//#region alert a message if route not selected and we add accessorial charges
function alertMessage(_this) {
    var radioValue = $("input[name='rdSelectedRoute']:checked").val();
    if (radioValue > 0) {
        return true;
    }
    else {
        $(_this).prop("checked", false);
       // toastr.warning("Please select at least one Route Stop.")
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
            if ($.trim(value.FeeType).toLowerCase() == $.trim("per unit").toLowerCase()) {

                $("#txtUnit_" + accessorialFeeTypeId + "").attr("readonly", false);
                $("#txtPerUnitAmount_" + accessorialFeeTypeId + "").attr("readonly", false);
                $("#txtAmount_" + accessorialFeeTypeId + "").attr("readonly", false);

                $("#txtUnit_" + accessorialFeeTypeId + "").val(value.Unit == 0 ? "" : value.Unit);
                $("#txtPerUnitAmount_" + accessorialFeeTypeId + "").val(value.AmtPerUnit == 0 ? "" : value.AmtPerUnit);
                $("#txtAmount_" + accessorialFeeTypeId + "").val(value.Amount == 0 ? "" : value.Amount);
                $("#chkunitfee_" + accessorialFeeTypeId + "").prop("checked", true);
            }
            else if ($.trim(value.FeeType).toLowerCase() == $.trim("fixed fee").toLowerCase()) {
                $("#txt_" + accessorialFeeTypeId + "").attr("readonly", false);
                $("#txt_" + accessorialFeeTypeId + "").val(value.Amount == 0 ? "" : value.Amount);
                $("#chkfixedfee_" + accessorialFeeTypeId + "").prop("checked", true);
            }
            else if ($.trim(value.FeeType).toLowerCase() == $.trim("other").toLowerCase()) {
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


function getUptoCount() {
    
    var minFee = $("#txtMinFee").val();
    var addPrice = $("#txtAddPrice").val();
    var uptoPrice = 0;
    if (minFee != "" && addPrice != "" && Number(addPrice) > 0) {
        uptoPrice = (Number(minFee) / Number(addPrice));
    }
    $("#txtUpTo").val(ConvertStringToFloat(uptoPrice));
}
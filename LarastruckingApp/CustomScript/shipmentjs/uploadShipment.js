var glbShipmentData = [];
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
//#region READY FUNCTION
$(function () {

    //fileDownload();
    CompanyList();
    bindSampleFile();
    //openModal();



    $("#btnSave").click(function () {
        isNeedToloaded = false;
        if (validateFileAndCompany()) {
            isNeedToloaded = true;
            //console.log("validateFileAndCompany(): " + validateFileAndCompany());
            $("form").submit(); // Submit the form

        }
    });
    $(".wrongdata").css("color", "white");
    $(".wrongdata").attr("bgcolor", "red");
    $("#flUploadShipment").css("padding", 0);

});
//#endregion
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

$("#ddlCompanyName").change(function () {
    if (!ValidateContactInfo()) {
        $.alert({
            title: 'Alert!',
            content: '<b>' + msgMissingContactInfo + '</b>',
            type: 'red',
            typeAnimated: true,
        });
    }

});

//#region shipment status
function ValidateContactInfo() {
    
    var isContactInfOAwb = false;
    var customerId = $("#ddlCompanyName").val();
    $.ajax({
        url: baseUrl + 'Shipment/UploadShipment/ValidateContactInfo',
        data: { "customerId": customerId },
        type: "GET",
        async: false,
        success: function (data) {
            
            isContactInfOAwb = data;

        }
    });
    return isContactInfOAwb;
}
//#endregion
//#region shipment status
function CompanyList() {
    $.ajax({
        url: baseUrl + 'Shipment/UploadShipment/GetCompanyName',
        data: {},
        type: "GET",
        async: false,
        success: function (data) {

            var ddlValue = "";
            $("#ddlCompanyName").empty();
            ddlValue += '<option value=" "> SELECT CUSTOMER   </option>';
            for (var i = 0; i < data.length; i++) {
                ddlValue += '<option value="' + data[i].CustomerId + '">' + data[i].CustomerName + '</option>';
            }
            $("#ddlCompanyName").append(ddlValue);

        }
    });
}
//#endregion

//#region ddlBind sample file on change
bindSampleFile = function () {
    $.ajax({
        url: baseUrl + 'Shipment/UploadShipment/GetUploadSample',
        data: {},
        type: "GET",
        async: false,
        success: function (data) {
            if (data != null) {
                var path = baseUrl + data;
                var a = $(".fldownload")
                    .attr("download", "SampleFile")
                var a = $(".fldownload")
                    .attr("href", path)
            }

        }
    });

}
//#endregion
//#region validate uploaded file and company 
function validateFileAndCompany() {
    var companyName = $("#ddlCompanyName").val();
    var Isvalid = false;
    if (companyName > 0) {
        var file = $('input[type="file"]').val();
        if (file != null && file != "") {


            var exts = ['XLS', 'xls', 'xlsx', 'XLSX'];

            // first check if file field has any value
            if (file) {

                // split file name at dot
                var get_ext = file.split('.');
                // reverse name to check extension
                get_ext = get_ext.reverse();
                // check file type is valid as given in 'exts' array
                if ($.inArray(get_ext[0].toLowerCase(), exts) > -1) {
                    Isvalid = true;
                    //console.log("Isvalid :"+Isvalid);
                    return Isvalid;
                } else {
                    // alert('File extension must be JPG or JPEG or PNG.');
                    AlertPopup('Upload files must have a .xls or .xlsx extension.');
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

        AlertPopup("Please select a customer.")
    }
}
//#endregion
function escapeRegExp(string) {
    
    var awb = string.replace(/[.,*+?^${}()|[\]\\]/g, '');
    awb = awb.replace(/-|\s/g, "");
    var awbn = "";
    if (awb.length == 11) {

        for (var i = 0; i < awb.length; i++) {
            awbn += awb[i];
            if (i == 2 || i == 6) {
                awbn = (awbn + "-");
            }
        }
        return awbn;
    }
    else {
        return string;
    }

}

//#region bind excel sheet data

function BindExceSheetData() {

    if (glbShipmentData != null) {
        if (glbShipmentData.length > 0) {
            var tableBody = "";
            $("#tblShipmentData tbody").empty();
            $("#ddlCompanyName").val(glbShipmentData[0].CustomerId);
            for (var i = 0; i < glbShipmentData.length; i++) {
                
                if (glbShipmentData[i].AWB != "") {
                    glbShipmentData[i].AWB = escapeRegExp(glbShipmentData[i].AWB);
                }

                tableBody += "<tr class='' ondblclick='javascript: editRow(" + i + ");'>";

                if (glbShipmentData[i].IsDateExpired) {

                    tableBody += "<td style='color:red'>" + ConvertDateEdit(glbShipmentData[i].Date, true) + "</td>";
                }
                else {
                    tableBody += "<td>" + ConvertDateEdit(glbShipmentData[i].Date, true) + "</td>";
                }

                //if (glbShipmentData[i].ETA != "") {
                //    tableBody += "<td>" + ConvertTime(glbShipmentData[i].ETA) + "</td>";
                //}
                //else {
                //    tableBody += "<td></td>";
                //}

                if (glbShipmentData[i].DeliveryDate != null) {


                    if (glbShipmentData[i].IsDeliveryDateExpired) {
                        tableBody += "<td style='color:red'>" + ConvertDateEdit(glbShipmentData[i].DeliveryDate, true) + "</td>";
                    }
                    else {
                        tableBody += "<td>" + ConvertDateEdit(glbShipmentData[i].DeliveryDate, true) + "</td>";

                    }
                }
                else {
                    tableBody += "<td></td >";
                }

                tableBody += "<td>" + glbShipmentData[i].ConsigneeNVendorName + "</td>";


                tableBody += "<td>" + glbShipmentData[i].CustomerPO + "</td>" +
                    "<td>" + glbShipmentData[i].OrderNo + "</td>" +
                    "<td>" + glbShipmentData[i].AWB + "</td>";
                if (glbShipmentData[i].PickUpLocationId == 0) {
                    
                    tableBody += "<td style='color:red'><label data-toggle='tooltip' data-placement='top' title='" + GetAddress(glbShipmentData[i].PickUpLocation) + "'>" + GetCompanyName(glbShipmentData[i].PickUpLocation) + "</label></td>";
                }
                else {
                    tableBody += "<td><label data-toggle='tooltip' data-placement='top' title='" + GetAddress(glbShipmentData[i].PickUpLocation) + "'>" + GetCompanyName(glbShipmentData[i].PickUpLocation) + "</label></td>";
                    //tableBody += "<td>" + glbShipmentData[i].PickUpLocation + "</td>";
                }
                if (glbShipmentData[i].DeliveryLocationId == 0) {
                    tableBody += "<td style='color:red'><label data-toggle='tooltip' data-placement='top' title='" + GetAddress(glbShipmentData[i].DeliveryLocation) + "'>" + GetCompanyName(glbShipmentData[i].DeliveryLocation) + "</label></td>";
                    //tableBody += "<td style='color:red'>" + glbShipmentData[i].DeliveryLocation + "</td>";
                }
                else {
                    tableBody += "<td><label data-toggle='tooltip' data-placement='top' title='" + GetAddress(glbShipmentData[i].DeliveryLocation) + "'>" + GetCompanyName(glbShipmentData[i].DeliveryLocation) + "</label></td>";
                    //tableBody += "<td>" + glbShipmentData[i].DeliveryLocation + "</td>";
                }
                if (glbShipmentData[i].FreightTypeId == 0) {
                    tableBody += "<td style='color:red'>" + glbShipmentData[i].FreightType + "</td>";
                }
                else {
                    tableBody += "<td>" + glbShipmentData[i].FreightType + "</td>";
                }



                tableBody += "<td>" + glbShipmentData[i].Commodity + "</td>" +
                    "<td>" + glbShipmentData[i].NoOfPallets + "</td>";


                tableBody += "<td>" + glbShipmentData[i].NoOfBox + "</td>" +
                    "<td>" + glbShipmentData[i].Weight + "</td>";

                if (glbShipmentData[i].IsUnit) {
                    tableBody += "<td>" + glbShipmentData[i].Unit + "</td>";
                }
                else {
                    tableBody += "<td style='color:red'>" + glbShipmentData[i].Unit + "</td>";
                }

                if (glbShipmentData[i].PricingMethodId == 0) {
                    tableBody += "<td style='color:red'>" + glbShipmentData[i].PricingMethod + "</td>";
                }
                else {
                    tableBody += "<td>" + glbShipmentData[i].PricingMethod + "</td>";
                }


                tableBody += "<td>" + glbShipmentData[i].ReqTemp + "</td>" +
                    "<td>" + glbShipmentData[i].Comments + "</td>" +
                    "<td>" + glbShipmentData[i].PartialPallet + "</td>" +
                    "<td>" + glbShipmentData[i].PartialBox + "</td>" +
                    "<td>" + glbShipmentData[i].RequestedBy + "</td>" +

                    "<td><a href='javascript:void(0)'class='btnOpenModal edit_icon' data-toggle='tooltip' title='Edit' onclick='javascript: editRow(" + i + ");' >" +
                    "<i class='far fa-edit'></i>" +
                    "</a>" +
                    " <a href='javascript:void (0)' id='btnDelete' class='delete_icon' data-toggle='tooltip' title='Delete' onclick='javascript:deleteRow(" + i + ");' >" +
                    "<i class='far fa-trash-alt'></i>" +
                    "</a></td >" +
                    "</tr>";
            }

            $("#tblShipmentData tbody").append(tableBody);
        }
    }
}
//#endregion//#region convert datetime stamp
function convertToJSONDate(strDate) {
    var dt = new Date(strDate);
    var newDate = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), dt.getHours(), dt.getMinutes(), dt.getSeconds(), dt.getMilliseconds());
    return '/Date(' + newDate.getTime() + ')/';
}
//#endregion

//#region deleteRow
function deleteRow(index) {
    $.confirm({
        title: 'Confirmation!',
        content: '<b>Do you want to delete this row.</b> ',
        type: 'red',
        typeAnimated: true,
        buttons: {
            delete: {
                btnClass: 'btn-blue',
                action: function () {
                    
                    glbShipmentData.splice(index, 1);
                    BindExceSheetData();

                }
            },
            cancel: {

                // btnClass: 'btn-red',
            }
        }
    })}
//#endregion

//#region edit  code edit row
function editRow(index) {
    manageLocation("ddlPickupLocation");
    manageLocation("ddlDeliveryLocation");
    var rowdetail = glbShipmentData[index];
    
    BindPricingMethodNFreightTypeNPcsType();
    //$('#uploadShipmentModal').trigger("reset");
    $("#rowValue").val(index);


    if (rowdetail.IsDateExpired) {

        $("#lblPickupDate").text(ConvertDate(rowdetail.Date, true));
        $("#lblPickupDate").css("color", "red");
        $("#ddlPickupDate").val(ConvertDate(rowdetail.Date, true));

    }
    else {
        $("#lblPickupDate").text("");
        $("#lblPickupDate").css("color", "black");
        $("#ddlPickupDate").val(ConvertDate(rowdetail.Date, true));
    }
    if (rowdetail.IsDeliveryDateExpired) {

        $("#lblDeliveryDate").text(ConvertDate(rowdetail.DeliveryDate, true));
        $("#lblDeliveryDate").css("color", "red");
        $("#ddlDeliveryDate").val(ConvertDate(rowdetail.DeliveryDate, true));

    }
    else {
        $("#lblDeliveryDate").text("");
        $("#lblDeliveryDate").css("color", "black");
        $("#ddlDeliveryDate").val(ConvertDate(rowdetail.DeliveryDate, true));

    }



    if (rowdetail.IsUnit) {
        $("#lblUnit").text("");
        $("#lblUnit").css("color", "black");
        $("#ddlUnit").val(rowdetail.Unit);
        $("#hdnUnit").val(rowdetail.IsUnit);
    }
    else {
        $("#hdnUnit").val(rowdetail.IsUnit);
        $("#lblUnit").text(rowdetail.Unit);
        $("#lblUnit").css("color", "red");
    }




    if (rowdetail.PickUpLocationId > 0) {
        $("#lblPickupLocation").css("color", "black");
        $("#lblPickupLocation").text("");
        var ddlpickup = "<option selected='selected' value=" + rowdetail.PickUpLocationId + ">" + rowdetail.PickUpLocation + "</option>";
        $("#ddlPickupLocation").empty();
        $("#ddlPickupLocation").append(ddlpickup);
        $(".ddlPickupLocation").text(rowdetail.PickUpLocation);
    }
    else {
        $("#lblPickupLocation").text(rowdetail.PickUpLocation);
        $("#lblPickupLocation").css("color", "red");
    }


    if (rowdetail.DeliveryLocationId > 0) {
        $("#lblDeliveryLocation").css("color", "black");
        $("#lblDeliveryLocation").text("");
        var ddldelivery = "<option  selected='selected' value=" + rowdetail.DeliveryLocationId + ">" + rowdetail.DeliveryLocation + "</option>";
        $("#ddlDeliveryLocation").empty();
        $("#ddlDeliveryLocation").append(ddldelivery);
        $(".ddlDeliveryLocation").text(rowdetail.DeliveryLocation);
    }
    else {
        $("#lblDeliveryLocation").text(rowdetail.DeliveryLocation);
        $("#lblDeliveryLocation").css("color", "red");

    }

    if (rowdetail.PricingMethodId > 0) {
        $("#lblPricingMethod").text("");
        $("#ddlPricingMethod").val(rowdetail.PricingMethodId);
        $("#lblPricingMethod").css("color", "black");
    }
    else {
        $("#lblPricingMethod").text(rowdetail.PricingMethod.toUpperCase());
        $("#lblPricingMethod").css("color", "red");
    }


    if (rowdetail.FreightTypeId > 0) {
        $("#lblFreightType").text("");
        $("#lblFreightType").css("color", "black");
        $("#ddlFreightType").val(rowdetail.FreightTypeId);

    }
    else {
        $("#lblFreightType").text(rowdetail.FreightType.toUpperCase());
        $("#lblFreightType").css("color", "red");
        //$("#ddlFreightType").css('border-color', 'red');
    }

    $("#txtOrderNo").val(rowdetail.OrderNo);
    $("#txtCustomerPO").val(rowdetail.CustomerPO);
    $("#txtAirWayBill").val(rowdetail.AWB);


    $("#uploadShipmentModal").modal("show");
    $('#uploadShipmentModal').draggable();
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

//#region selectize on pickup and delivery location
var manageLocation = function (htmlcontrol) {
    
    var $select = $('#' + htmlcontrol).selectize();
    $select[0].selectize.destroy();

    $('#' + htmlcontrol).selectize({
        maxItems: 1,
        createOnBlur: true,
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
                        item.text = value.CompanyName.trim() + ', ' + value.Address1 + ' ' + value.City + ' ' + value.StateName + ' ' + value.Zip + ' ' + nickName;
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
//#endregion//#region bind pricing method , Pcs Type and Freight type dropdown
function BindPricingMethodNFreightTypeNPcsType() {

    $.ajax({
        url: baseUrl + 'Shipment/UploadShipment/BindFreightTypeNPricingMethod',
        data: {},
        type: "GET",
        async: false,
        // cache: false,
        success: function (data) {

            var ddlPricingMethod = "";
            var ddlPcsType = "";
            var ddlFreightType = "";
            var ddlUnit = "";

            var ddlpickup = "<option selected='selected' value='0'> SEARCH PICKUP LOCATION</option>";
            $("#ddlPickupLocation").empty();
            $("#ddlPickupLocation").append(ddlpickup);
            $(".ddlPickupLocation").text("SEARCH PICKUP LOCATION");


            var ddldelivery = "<option  selected='selected' value='0'>SEARCH DELIVERY LOCATION</option>";
            $("#ddlDeliveryLocation").empty();
            $("#ddlDeliveryLocation").append(ddldelivery);
            $(".ddlDeliveryLocation").text("SEARCH DELIVERY LOCATION");

            $("#ddlPricingMethod").empty();
            $("#ddlFreightType").empty();
            $("#ddlUnit").val("");


            ddlPricingMethod += '<option value="">SELECT PRICING METHOD</option>'
            ddlFreightType += '<option value="">SELECT FREIGHT  TYPE</option>'



            for (var i = 0; i < data.PricingMethod.length; i++) {
                ddlPricingMethod += '<option value="' + data.PricingMethod[i].PricingMethodId + '">' + data.PricingMethod[i].PricingMethodName + '</option>';
            }

            for (var i = 0; i < data.FreightType.length; i++) {
                ddlFreightType += '<option value="' + data.FreightType[i].FreightTypeId + '">' + data.FreightType[i].FreightTypeName + '</option>';
            }

            $("#ddlPricingMethod").append(ddlPricingMethod);
            $("#ddlPcsType").append(ddlPcsType);
            $("#ddlFreightType").append(ddlFreightType);


        }
    });
}
//#endregion//#region update record
$("#btnUpdate").click(function () {
     var isValid = true; var message = ""; var index = $("#rowValue").val();
    var rowdetail = glbShipmentData[index];

    var todayDate = new Date();
    var month = todayDate.getMonth() + 1;
    var day = todayDate.getDate() - 1;

    todayDate = (month < 10 ? '0' : '') + month + '/' +
        (day < 10 ? '0' : '') + day + '/' +
        todayDate.getFullYear();

    todayDate = new Date(Date.parse(todayDate));

    var pickUpdateStr = $("#ddlPickupDate").val();
    var deliveryDateStr = $("#ddlDeliveryDate").val();
    var pickupDate = new Date(Date.parse($("#ddlPickupDate").val()));
    var deliveryDate = new Date(Date.parse($("#ddlDeliveryDate").val()));
    var OrderNo = $("#txtOrderNo").val();
    var CustomerPO = $("#txtCustomerPO").val();

    var AWB = $("#txtAirWayBill").val();
    var FreightTypeId = $("#ddlFreightType").val();
    var PricingMethodId = $("#ddlPricingMethod").val();

    var DeliveryLocationId = $("#ddlDeliveryLocation").val();
    var PickUpLocationId = $("#ddlPickupLocation").val();

    if (isValid && pickUpdateStr == "") {
        isValid = false;
        message = "Please select Pickup Date";
    }
    if (isValid && todayDate > pickupDate) {
        isValid = false;
        message = "Pickup date should be greter then and equal to yesterday date.";
    }

    if (isValid && deliveryDateStr == "") {
        isValid = false;
        message = "Please select Delivery Date";
    }
    if (isValid && todayDate > deliveryDate && pickupDate > deliveryDate) {
        isValid = false;
        message = "Delivery date should be greater than and equal to yesterday date and also greater than Pickup date.";
    }

    if (isValid && !(PickUpLocationId > 0)) {
        isValid = false;
        message = "Please select a Pickup Location.";
    }

    if (isValid && !(DeliveryLocationId > 0)) {
        isValid = false;
        message = "Please select a Delivery Location.";
    }
    if (isValid && !(FreightTypeId > 0)) {
        isValid = false;
        message = "Please select a Freight Type.";
    }
    if (isValid && !(PricingMethodId > 0)) {
        isValid = false;
        message = "Please select a Pricing Method.";
    }
    if (isValid && (OrderNo == "" && CustomerPO == "" && AWB == "")) {
        isValid = false;
        message = "A reference number is required. Please enter an AWB, PO, or Order number.";
    }
    var ddlUnit = $("#ddlUnit").val();
    
    var hdnUnit = $("#hdnUnit").val();
    if (hdnUnit == "false") {
        if (isValid && ddlUnit.trim().toLowerCase() != "lbs" && ddlUnit.trim().toLowerCase() != "kg") {
            isValid = false;
            message = "Please select Unit.";
        }
    }
    if (isValid) {

        rowdetail.IsDateExpired = false;
        rowdetail.Date = convertToJSONDate(pickUpdateStr);

        rowdetail.IsDeliveryDateExpired = false;
        rowdetail.DeliveryDate = convertToJSONDate(deliveryDateStr);

        if (PickUpLocationId > 0) {
            var oldPickupLocationId = rowdetail.PickUpLocationId;
            rowdetail.PickUpLocationId = $("#ddlPickupLocation").val();
            rowdetail.PickUpLocation = $.trim($("#ddlPickupLocation").find('option:selected').text());
            // $("#ddlPickupLocation").find('option:selected').removeClass("selected");

            if ($("#chkPickupLocationSame").is(':checked')) {
                for (var i = 0; i < glbShipmentData.length > 0; i++) {
                    if (oldPickupLocationId == glbShipmentData[i].PickUpLocationId) {
                        glbShipmentData[i].PickUpLocationId = $("#ddlPickupLocation").val();
                        glbShipmentData[i].PickUpLocation = $.trim($("#ddlPickupLocation").find('option:selected').text());
                    }
                }
            }

        }

        if ($("#chkPickupLocationSame").is(':checked')) {
            
            for (var i = 0; i < glbShipmentData.length > 0; i++) {
                
                if ($("#lblPickupLocation").text().trim() == glbShipmentData[i].PickUpLocation.trim()) {
                    
                    glbShipmentData[i].PickUpLocationId = $("#ddlPickupLocation").val();
                    glbShipmentData[i].PickUpLocation = $.trim($("#ddlPickupLocation").find('option:selected').text());
                }
            }
        }

        if (DeliveryLocationId > 0) {
            var oldDeliveryLocationId = rowdetail.DeliveryLocationId;
            rowdetail.DeliveryLocationId = $("#ddlDeliveryLocation").val();
            rowdetail.DeliveryLocation = $.trim($("#ddlDeliveryLocation").find('option:selected').text());
            // $("#ddlPickupLocation").find('option:selected').removeClass("selected");
            //  $("#ddlDeliveryLocation").find('option:selected').prop('selected', false);
            //$('#select').prop('selected', false).find('option:first').prop('selected', true);
            if ($("#chkDeliveryLocationSame").is(':checked')) {
                for (var i = 0; i < glbShipmentData.length > 0; i++) {

                    if (oldDeliveryLocationId == glbShipmentData[i].DeliveryLocationId) {
                        glbShipmentData[i].DeliveryLocationId = $("#ddlDeliveryLocation").val();
                        glbShipmentData[i].DeliveryLocation = $.trim($("#ddlDeliveryLocation").find('option:selected').text());
                    }
                }
            }

        }

        if ($("#chkDeliveryLocationSame").is(':checked')) {
            for (var i = 0; i < glbShipmentData.length > 0; i++) {
                if ($("#lblDeliveryLocation").text().trim() == glbShipmentData[i].DeliveryLocation.trim()) {
                    glbShipmentData[i].DeliveryLocationId = $("#ddlDeliveryLocation").val();
                    glbShipmentData[i].DeliveryLocation = $.trim($("#ddlDeliveryLocation").find('option:selected').text());
                }
            }
        }

        var PricingMethodId = $("#ddlPricingMethod").val();
        if (PricingMethodId > 0) {
            rowdetail.PricingMethodId = $("#ddlPricingMethod").val();
            rowdetail.PricingMethod = $.trim($("#ddlPricingMethod").find('option:selected').text());
        }


        if (FreightTypeId > 0) {
            rowdetail.FreightTypeId = $("#ddlFreightType").val();
            rowdetail.FreightType = $.trim($("#ddlFreightType").find('option:selected').text())
        }



        if (ddlUnit != "") {
            rowdetail.IsUnit = true;
            rowdetail.Unit = $.trim($("#ddlUnit").find('option:selected').text())
        }

        if (OrderNo != "") {
            rowdetail.OrderNo = OrderNo;
        }



        if (CustomerPO != "") {
            rowdetail.CustomerPO = CustomerPO;
        }



        if (AWB != "") {
            rowdetail.AWB = AWB;
        }

        $('#uploadShipmentModal').modal('toggle')

        if ($("#chkPickUpLocation").is(':checked')) {
            for (var i = 0; i < glbShipmentData.length > 0; i++) {
                
                glbShipmentData[i].PickUpLocationId = $("#ddlPickupLocation").val();
                glbShipmentData[i].PickUpLocation = $.trim($("#ddlPickupLocation").find('option:selected').text());
            }
        }

        if ($("#chkDeliveryLocation").is(':checked')) {
            for (var i = 0; i < glbShipmentData.length > 0; i++) {
                glbShipmentData[i].DeliveryLocationId = $("#ddlDeliveryLocation").val();
                glbShipmentData[i].DeliveryLocation = $.trim($("#ddlDeliveryLocation").find('option:selected').text());
            }
        }





        $("#chkPickUpLocation").prop("checked", false);
        $("#chkDeliveryLocation").prop("checked", false);

        $("#chkPickupLocationSame").prop("checked", false);
        $("#chkDeliveryLocationSame").prop("checked", false);
        BindExceSheetData();


    }
    else {
        AlertPopup(message);
    }
})
//#endregion//#region 
$("#btnClear").click(function () {    $.confirm({
        title: 'Confirmation!',
        content: '<b>Do you want to clear grid.</b> ',
        type: 'red',
        typeAnimated: true,
        buttons: {
            clear: {
                btnClass: 'btn-blue',
                action: function () {
                    isNeedToloaded = false;
                    window.location.href = baseUrl + 'Shipment/UploadShipment/Index';

                }
            },
            cancel: {

                // btnClass: 'btn-red',
            }
        }
})    
});
//#endregion//#region save grid data in excel
$("#btnSaveData").click(function () {
    var rowdetail = glbShipmentData;

    var rowNo = "";
    var isvalid = false;
    for (var i = 0; i < glbShipmentData.length; i++) {
        rowNo = i;
        if (glbShipmentData[i].IsDateExpired) {
            isvalid = false;
            $.alert("Please correct the Pickup Date.")
            break;

        }
        else {
            isvalid = true;
        }

        if (glbShipmentData[i].IsDeliveryDateExpired > 0) {

            isvalid = false;
            $.alert("Please correct the Delivery Date.")
            break;

        }
        else {
            isvalid = true;
        }

        if (glbShipmentData[i].PickUpLocationId > 0) {
            isvalid = true;
        }
        else {
            isvalid = false;
            $.alert("Please correct the Pickup Location.")
            break;
        }

        if (glbShipmentData[i].DeliveryLocationId > 0) {
            isvalid = true;
        }
        else {
            isvalid = false;
            $.alert("Please correct the Delivery Location.")
            break;
        }




        if (glbShipmentData[i].FreightTypeId > 0) {
            isvalid = true;
        }
        else {
            isvalid = false;
            $.alert("Please correct the Freight Type.")
            break;
        }
        if (glbShipmentData[i].IsUnit) {
            isvalid = true;
        }
        else {
            isvalid = false;
            $.alert("Please correct the Unit.")
            break;
        }
        if (glbShipmentData[i].PricingMethodId > 0) {
            isvalid = true;
        }
        else {
            isvalid = false;
            $.alert("Please correct the Pricing Method.")
            break;
        }
        if (glbShipmentData[i].OrderNo == "" && glbShipmentData[i].CustomerPO == "" && glbShipmentData[i].AWB == "") {
            isValid = false;
            $.alert("A reference number is required. Please enter an AWB, PO, or Order number.");
        }


    }

    if (isvalid) {
        
        for (var i = 0; i < glbShipmentData.length; i++) {
            
            glbShipmentData[i].Date = ConvertDateEdit(glbShipmentData[i].Date, true);
            glbShipmentData[i].DeliveryDate = ConvertDateEdit(glbShipmentData[i].DeliveryDate, true);
        }
        var values = glbShipmentData;

        if (ValidateContactInfo()) {
            $.ajax({
                url: baseUrl + "/Shipment/UploadShipment/SaveExcelData",
                type: "POST",
                data: JSON.stringify(values),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    showLoader();
                },
                success: function (response) {
                    hideLoader();
                    if (response) {

                        $.alert({
                            title: 'Success!',
                            content: "<b>Data successfully uploaded</b>",
                            type: 'green',
                            typeAnimated: true,
                            buttons: {
                                Ok: {
                                    btnClass: 'btn-green',
                                    action: function () {
                                        isNeedToloaded = false;
                                        window.location.href = baseUrl + "/Shipment/Shipment/ViewShipmentList";
                                    }
                                },
                            }
                        });
                    }
                    else {
                        hideLoader();
                        //console.log("response: " + response.Message);
                        AlertPopup(response.Message);
                    }
                },
                error: function () {
                    console.log("response error: " );
                }
            });
        }
        else {
            AlertPopup(msgMissingContactInfo)
        }

    }
});//#endregion//#region check file name
function CheckFileName(_this) {
    

    var fileName = _this.files[0].name;
    $.ajax({
        url: baseUrl + "/Shipment/UploadShipment/CheckFileName",
        type: "Get",
        data: { fileName: fileName },
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: function (response) {
            if (response) {
                $.confirm({
                    title: 'Confirmation!',
                    content: '<b>Your file name already exists in the database. Are you sure you want to continue?</b> ',
                    type: 'red',
                    typeAnimated: true,
                    buttons: {
                        Continue: {
                            btnClass: 'btn-blue',
                            action: function () {
                                

                            }
                        },
                        cancel: {
                            btnClass: 'btn-blue',
                            action: function () {
                                
                                $("#flUploadShipment").val("");
                            }
                            // btnClass: 'btn-red',
                        }
                    }
                })

            }
        },
        error: function () {

        }
    });
}//#endregion
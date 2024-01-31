var glbTrailerRentalDetail = new Array();
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

$(function () {
    //bind customer dropdown
    bindCustomerDropdown();

    //bind pickup location 
    bindPickupLocation();

    //bind delivery location
    bindDeliveryLocation();

    //Add Trailer detail
    //btnAddTrailer();
    //#region clear route stops
    //btnClearRoute();


    //Save Trailer Rental
    //btnSave();

    //Get Trailer Rental detail by id
    GetTrailerRentalDetailById();
    //function for open address popup
    BindEquipment();
    BindPickUpAndDeliveryDriver();

   // openAddressModal();

    $(".selectize-control.withsearch > .selectize-input").css("width", "116%");

});

//#region bind equipment and driver
function BindEquipmentDriver() {
    
    var startDate = $("#dtStartDate").val();
    var endDate = $("#dtReturnedDate").val();
    if (endDate == "") {
        endDate = $("#dtEndDate").val();
    }

    if (startDate != "" && endDate != "") {
        if (new Date(startDate) < new Date(endDate)) {
            //bind PickUp and Delivery Dropdown
            BindPickUpAndDeliveryDriver()
            //Bind Equipment
            BindEquipment()
        }
        ValidateStartAndEndDate();
    }
    
}
//#endregion

//#region get trailerRental detail by id for edit
GetTrailerRentalDetailById = function () {

    var url = window.location.pathname;
    var trailerRentalId = url.substring(url.lastIndexOf('/') + 1);
    if (trailerRentalId > 0) {
        $.ajax({
            url: baseUrl + "/TrailerRental/TrailerRental/GetTrailerRentalDetailById",
            type: "GET",
            data: { trailerRentalId: trailerRentalId },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (response) {
                if (response != null) {

                    $("#hdnTrailerRentalId").val(response.TrailerRentalId);

                    var ddlCustomer = "<option selected='selected' value=" + response.CustomerId + ">" + response.CustomerName + "</option>";
                    $("#ddlCustomer").empty();
                    $("#ddlCustomer").append(ddlCustomer);
                    $(".ddlCustomer").text(response.CustomerName);

                    $("#txtGrandTotal").val(ConvertStringToFloat(response.GrandTotal));
                    $("#txtTrailerInstruction").val(response.TrailerInstruction);
                    if (response.TrailerRentalDetail != null) {
                        for (var i = 0; i < response.TrailerRentalDetail.length; i++) {

                            objTrailerRentalDetail = {};
                            objTrailerRentalDetail.TrailerRentalDetailId = response.TrailerRentalDetail[i].TrailerRentalDetailId;
                            objTrailerRentalDetail.TrailerRentalId = response.TrailerRentalDetail[i].TrailerRentalId;
                            objTrailerRentalDetail.StartDate = ConvertDateEdit(response.TrailerRentalDetail[i].StartDate, true);
                            console.log("objTrailerRentalDetail.StartDate: ", objTrailerRentalDetail.StartDate);
                            console.log("objTrailerRentalDetail.StartDate: ", ConvertDateEdit(response.TrailerRentalDetail[i].StartDate, true));
                            objTrailerRentalDetail.EndDate = ConvertDateEdit(response.TrailerRentalDetail[i].EndDate, true);
                            objTrailerRentalDetail.ReturnedDate = response.TrailerRentalDetail[i].ReturnedDate == null ? "" : ConvertDateEdit(response.TrailerRentalDetail[i].ReturnedDate, true);
                            objTrailerRentalDetail.PickUpLocationId = response.TrailerRentalDetail[i].PickUpLocationId;
                            objTrailerRentalDetail.PickUpLocationText = response.TrailerRentalDetail[i].PickUpLocationText;
                            objTrailerRentalDetail.DeliveryLocationId = response.TrailerRentalDetail[i].DeliveryLocationId;
                            objTrailerRentalDetail.DeliveryLocationText = response.TrailerRentalDetail[i].DeliveryLocationText;
                            objTrailerRentalDetail.NoOfDays = response.TrailerRentalDetail[i].NoOfDays;
                            objTrailerRentalDetail.EquipmentId = response.TrailerRentalDetail[i].EquipmentId;
                            objTrailerRentalDetail.EquipmentNo = response.TrailerRentalDetail[i].EquipmentNo;
                            objTrailerRentalDetail.FeePerDay = response.TrailerRentalDetail[i].FeePerDay > 0 ? response.TrailerRentalDetail[i].FeePerDay :"";
                            objTrailerRentalDetail.FixedFee = response.TrailerRentalDetail[i].FixedFee > 0 ? response.TrailerRentalDetail[i].FixedFee : "";
                            objTrailerRentalDetail.TotalFee = response.TrailerRentalDetail[i].TotalFee;
                            objTrailerRentalDetail.DeliveryDriverId = response.TrailerRentalDetail[i].DeliveryDriverId;
                            objTrailerRentalDetail.PickupDriverId = response.TrailerRentalDetail[i].PickupDriverId;
                            objTrailerRentalDetail.IsDeleted = false;
                            glbTrailerRentalDetail.push(objTrailerRentalDetail);
                        }

                    }
                    //Bind Equipment
                    BindEquipment();
                    BindPickUpAndDeliveryDriver();
                    if (response.TrailerRentalDetail.length == 1) {
                        edit_TrailerDetail(0)
                    }

                    BindTrailerDetail();


                }
            },
            error: function () {

            }
        });
    }

}
//#endregion

//#region function for apply selectize on customer dropdown
var bindCustomerDropdown = function () {

    var $select = $('#ddlCustomer').selectize();
    $select[0].selectize.destroy();

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
                    ('<span class="name ddlCustomer">' + escape(item.text) + '</span>') +
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
        }
    });
}
//#endregion//#region function for pickup location dropdown
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
//#endregion
function BindPickUpAndDeliveryDriver() {
    var value = {};
    value.TrailerRentalId = $("#hdnTrailerRentalId").val();
    value.CustomerId = $("#ddlCustomer").val();
    value.FirstPickupArrivalDate = $("#dtStartDate").val();
    value.LastPickupArrivalDate = $("#dtEndDate").val();    $.ajax({
        url: baseUrl + 'Shipment/Shipment/GetDriverList',
        data: JSON.stringify(value),
        type: "POST",
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        // cache: false,
        success: function (data) {
            ddlValue = "";
            glbDriver = JSON.parse(JSON.stringify(data));
            ddlValue += '<option value="0">SELECT DRIVER</option>'
            for (var i = 0; i < glbDriver.length; i++) {
                ddlValue += '<option value="' + glbDriver[i].DriverId + '">' + glbDriver[i].DriverName + '</option>';
            }
            $("#ddlDeliveryDriver").append(ddlValue);
            $("#ddlPickUpDriver").append(ddlValue);
        }
    });
    //
}

function IsStartAndEndDateValid() {
    
    var isvalid = true;
    var dtStartDate = $("#dtStartDate").val();
    var dtEndDate = $("#dtEndDate").val();
    var dtReturnedDate = $("#dtReturnedDate").val();
    var todayDate = new Date();
    var month = todayDate.getMonth() + 1;
    var day = todayDate.getDate() - 1;

    var yesterday = "";
    yesterday = (month < 10 ? '0' : '') + month + '-' +
        (day < 10 ? '0' : '') + day + '-' +
        todayDate.getFullYear();

    //yesterday = new Date(Date.parse(todayDate));


    //if (dtStartDate != "" && dtEndDate != "") {

    //    if (dtStartDate != "" && new Date(dtStartDate) < new Date(yesterday)) {
    //       // toastr.warning("Start Date should be greter then and equal to yesterday date.");
    //        AlertPopup("Start Date should be greter then and equal to yesterday date.");
    //        isvalid = false;
    //        return isvalid
    //    }

    //    if (new Date(dtStartDate) <= new Date(dtEndDate)) {
    //        isvalid = true;
    //    }
    //    else {
    //        //$("#dtPickUpToDate").val("");
    //        //toastr.warning("End Date should be greater than Start Date.");
    //        AlertPopup("End Date should be greater than Start Date.");
    //        return isvalid = false;

    //    }
    //    if (dtReturnedDate != "") {
    //        if (isvalid && new Date(dtReturnedDate) >= new Date(dtEndDate)) {
    //            isvalid = true;
    //        }
    //        else {
    //           // toastr.warning("Return Date should be greater than and equal to End Date.");
    //            AlertPopup("Return Date should be greater than and equal to End Date.");
    //            return isvalid = false;
    //        }
    //    }
    //}


    return isvalid;
}
//btnAddTrailer = function () {
    $("#btnAddTrailer").on("click", function () {
        
        if ($("#ddlCustomer").val() > 0) {
            if ($("#ddlDeliveryLocation").val() > 0) {
            if ($("#ddlPickupLocation").val() > 0) {               
                    if (IsStartAndEndDateValid()) {
                        if (isFormValid("trailerDetail")) {
                            objTrailerRentalDetail = {};
                            objTrailerRentalDetail.TrailerRentalDetailId = 0;
                            objTrailerRentalDetail.TrailerRentalId = 0;
                            objTrailerRentalDetail.StartDate = $("#dtStartDate").val();
                            objTrailerRentalDetail.EndDate = $("#dtEndDate").val();
                            objTrailerRentalDetail.ReturnedDate = $("#dtReturnedDate").val();
                            objTrailerRentalDetail.PickUpLocationId = $.trim($("#ddlPickupLocation").val());
                            objTrailerRentalDetail.PickUpLocationText = $.trim($("#ddlPickupLocation").find('option:selected').text());
                            objTrailerRentalDetail.DeliveryLocationId = $.trim($("#ddlDeliveryLocation").val());
                            objTrailerRentalDetail.DeliveryLocationText = $.trim($("#ddlDeliveryLocation").find('option:selected').text());
                            objTrailerRentalDetail.NoOfDays = $.trim($("#txtNoOfDays").val());
                            objTrailerRentalDetail.EquipmentId = $.trim($("#ddlEquipment").val());
                            objTrailerRentalDetail.EquipmentNo = $.trim($("#ddlEquipment").find('option:selected').text());

                            objTrailerRentalDetail.FeePerDay = $.trim($("#txtFeePerDay").val());
                            objTrailerRentalDetail.FixedFee = $.trim($("#txtFixedFee").val());

                            objTrailerRentalDetail.TotalFee = $.trim($("#txtTotalFee").val());
                            objTrailerRentalDetail.DeliveryDriverId = $.trim($("#ddlDeliveryDriver").val());


                            objTrailerRentalDetail.PickupDriverId = $.trim($("#ddlPickUpDriver").val());
                            objTrailerRentalDetail.IsDeleted = false;

                            if ($("#tblTrailerDetail").attr("data-row-no") > 0) {
                                
                                var tblRowsCount = $("#tblTrailerDetail").attr("data-row-no");
                                var rowindex = Number(tblRowsCount) - 1;
                                var trailerDetail = glbTrailerRentalDetail[rowindex];
                                objTrailerRentalDetail.TrailerRentalDetailId = trailerDetail.TrailerRentalDetailId;
                                objTrailerRentalDetail.TrailerRentalId = trailerDetail.TrailerRentalId;
                                glbTrailerRentalDetail[rowindex] = objTrailerRentalDetail;
                                //$("#dtStartDate").attr("disabled", false);
                                //$("#dtEndDate").attr("disabled", false);
                                $.alert({
                                    title: 'Success!',
                                    content: "<b>Your data has successfully been updated to your shipment.<br/>  Don't forget to click on the Submit button to save all changes.</b>",
                                    type: 'green',
                                    typeAnimated: true,
                                });
                                $("#tblTrailerDetail").attr("data-row-no", 0);
                                $("#btnAddTrailer").text("Add Trailer");
                            }
                            else {
                                $.alert({
                                    title: 'Success!',
                                    content: "<b>Your data has successfully been added to your shipment.<br/>  Don't forget to click on the Submit button to save all changes.</b>",
                                    type: 'green',
                                    typeAnimated: true,
                                });
                                glbTrailerRentalDetail.push(objTrailerRentalDetail);
                            }

                            BindTrailerDetail();
                            ClearTrailerRentalDetail();
                            manageLocation("ddlPickupLocation");
                            manageLocation("ddlDeliveryLocation");
                        }
                    }
                }
                else {
                //toastr.warning("Please select Pickup Location")
                AlertPopup("Please select Pickup Location")
                }

            }
            else {
               // toastr.warning("Please select a Delivery Location.")
                AlertPopup("Please select a Delivery Location.")
            }
        }
        else {
            $.alert({
                title: 'Alert!',
                content: '<b>Please select a customer.</b>',
                type: 'red',
                //typeAnimated: true,
            });
        }
    })


//};

$("table").on("mouseover", 'tr', function () {

    $(this).find(".far").css('color', 'white');

});

$("table").on("mouseout", 'tr', function () {

    $(this).find(".far").css('color', '#007bff');
    $(this).find(".fa-trash-alt").css('color', 'red');

});

function BindTrailerDetail() {
    $("#tblTrailerDetail tbody").empty();
    var tblTrailerDetailBody = "";
    var totalFee = 0;
    for (var i = 0; i < glbTrailerRentalDetail.length; i++) {
        if (glbTrailerRentalDetail[i].IsDeleted == false) {
            totalFee = (Number(totalFee) + Number(glbTrailerRentalDetail[i].TotalFee));
            tblTrailerDetailBody += "<tr ondblclick='javascript: edit_TrailerDetail(" + i + ");'>" +
                "<td>" + (i + 1) + "</td>" +
                "<td>" + glbTrailerRentalDetail[i].EquipmentNo + "</td>" +
                "<td><label data - toggle='tooltip' data-placement='top' title = '" + GetAddress(glbTrailerRentalDetail[i].DeliveryLocationText) + "' > " + GetCompanyName(glbTrailerRentalDetail[i].DeliveryLocationText) + "</label ></td>" +
                "<td><label data - toggle='tooltip' data-placement='top' title = '" + GetAddress(glbTrailerRentalDetail[i].PickUpLocationText) + "' > " + GetCompanyName(glbTrailerRentalDetail[i].PickUpLocationText) + "</label ></td>" +
                "<td>" + glbTrailerRentalDetail[i].StartDate + "</td>" +
                "<td>" + glbTrailerRentalDetail[i].EndDate + "</td>" +
                "<td>" + glbTrailerRentalDetail[i].FeePerDay + "</td>" +
                "<td>" + glbTrailerRentalDetail[i].FixedFee + "</td>" +
                "<td>" + glbTrailerRentalDetail[i].TotalFee + "</td>" +
                "<td><button type = 'button' class='edit_icon' onclick = 'edit_TrailerDetail(" + i + ")' > <i class='far fa-edit'></i> </button >| <button type='button' class='delete_icon' onclick='remove_row(" + i + ")'> <i class='far fa-trash-alt'></i> </button> </td></tr>"
        }
    }
    $("#tblTrailerDetail tbody").append(tblTrailerDetailBody);
    $("#txtGrandTotal").val(ConvertStringToFloat(totalFee));
}

//#region clear route stops
btnClearRoute = function () {

    $("#btnClearRoute").on("click", function () {
        ClearTrailerRentalDetail();
        $("#tblTrailerDetail").attr("data-row-no", 0);
        $("#btnAddTrailer").text("Add Trailer");

    })
}

function SplitString(text, count, insertDots) {
    return text.slice(0, count) + (((text.length > count) && insertDots) ? "..." : "");
}
//#region edit trailer detail detail by id
function edit_TrailerDetail(index) {
    
    $("#txtFeePerDay").val("");
    $("#txtFixedFee").val("");

    var trailerDetail = glbTrailerRentalDetail[index];
    //$("#dtStartDate").attr("disabled", "true");
    //$("#dtEndDate").attr("disabled", "true");
    $("#dtStartDate").val(trailerDetail.StartDate);
    $("#dtEndDate").val(trailerDetail.EndDate);
    $("#dtReturnedDate").val(trailerDetail.ReturnedDate);
    $("#ddlPickupLocation").val(trailerDetail.PickUpLocationId);
    $("#ddlDeliveryLocation").val(trailerDetail.DeliveryLocationId);
    
    $("#txtNoOfDays").val(trailerDetail.NoOfDays);
    $("#ddlEquipment").val(trailerDetail.EquipmentId);

    if (trailerDetail.FeePerDay > 0) {
        $("#txtFeePerDay").val(trailerDetail.FeePerDay);
    }
    if (trailerDetail.FixedFee > 0) {
        $("#txtFixedFee").val(trailerDetail.FixedFee);
    }
   

    $("#txtTotalFee").val(ConvertStringToFloat(trailerDetail.TotalFee));
    ValidateStartAndEndDate();
    if (trailerDetail.DeliveryDriverId > 0) {
        $("#ddlDeliveryDriver").val(trailerDetail.DeliveryDriverId);
    }
    else {
        $("#ddlDeliveryDriver").val(0);
    }

    if (trailerDetail.PickupDriverId > 0) {
        $("#ddlPickUpDriver").val(trailerDetail.PickupDriverId);
    }
    else {
        $("#ddlPickUpDriver").val(0);
    }


    if (trailerDetail.PickUpLocationId > 0) {
        var ddlpickup = "<option selected='selected' value=" + trailerDetail.PickUpLocationId + ">" + trailerDetail.PickUpLocationText + "</option>";
        $("#ddlPickupLocation").empty();
        $("#ddlPickupLocation").append(ddlpickup);
        $(".ddlPickupLocation").text(trailerDetail.PickUpLocationText);
    }
    if (trailerDetail.DeliveryLocationId > 0) {
        var ddldelivery = "<option  selected='selected' value=" + trailerDetail.DeliveryLocationId + ">" + trailerDetail.DeliveryLocationText + "</option>";
        $("#ddlDeliveryLocation").empty();
        $("#ddlDeliveryLocation").append(ddldelivery);
        $(".ddlDeliveryLocation").text(trailerDetail.DeliveryLocationText);
    }

    $("#tblTrailerDetail").attr("data-row-no", index + 1);
    $("#btnAddTrailer").text("Update");
}
//#endregion//#region Find and remove route and detail
function remove_row(index) {

    $.confirm({
        title: 'Confirmation!',
        content: "<b>Are you sure you want to Delete ?",
        type: 'red',
        typeAnimated: true,
        buttons: {
            delete: {
                btnClass: 'btn-green',
                action: function () {

                    var trailerDetail = glbTrailerRentalDetail[index];
                    trailerDetail.IsDeleted = true;

                    //Bind Route
                    BindTrailerDetail();
                }
            },
            cancel: {
                btnClass: 'btn-red',
            }
        }
    })


}
//#endregion
function BindEquipment() {

    var value = {};

    value.TrailerRentalId = $("#hdnTrailerRentalId").val();
    value.CustomerId = $("#ddlCustomer").val();
    value.FirstPickupArrivalDate = $("#dtStartDate").val();
    value.LastPickupArrivalDate = $("#dtEndDate").val();    $.ajax({
        url: baseUrl + 'TrailerRental/TrailerRental/GetEquipmentList',
        data: JSON.stringify(value),
        type: "POST",
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            ddlValue = "";
            glbEquipment = JSON.parse(JSON.stringify(data));
            $("#ddlEquipment").empty();
            ddlValue += '<option value="">SELECT EQUIPMENT</option>'
            for (var i = 0; i < glbEquipment.length; i++) {
                ddlValue += '<option value="' + glbEquipment[i].EDID + '">' + glbEquipment[i].EquipmentNo + '</option>';
            }
            $("#ddlEquipment").append(ddlValue);
        }
    });
}

function GetJsonValue() {
    
    var values = {};
    values.TrailerRentalId = $("#hdnTrailerRentalId").val();
    values.CustomerId = $("#ddlCustomer").val();
    values.GrandTotal = $("#txtGrandTotal").val();
    values.TrailerInstruction = $("#txtTrailerInstruction").val();
    values.TrailerRentalDetail = glbTrailerRentalDetail;
    return values;
}

//Go BACK... Added on 08-Feb-2023
$("#btnGoBack").on("click", function () {
    window.location.href = baseUrl + "TrailerRental/TrailerRental/ViewTrailerRental"
})
$("html").unbind().keyup(function (e) {
    console.log("Which Key: " + $(e.target) + " : " + $(".jconfirm-title") + " : " + document.getElementsByClassName("jconfirm-title")[0]);
    if (!$(e.target).is('input') && !$(e.target).is('textarea')) {
        console.log(e.which);
        //event.preventDefault();
        if (e.key === 'Backspace' || e.keyCode === 8) {
            //alert('backspace pressed');
            //window.location.href = baseUrl + "Shipment/Shipment/ViewShipmentList";
            if (document.getElementsByClassName("jconfirm").length == 0 && window.location.href.toLowerCase().indexOf("index") >= 0) {
                window.location.href = baseUrl + "TrailerRental/TrailerRental/ViewTrailerRental";
            } else if (document.getElementsByClassName("jconfirm").length == 0 && window.location.href.toLowerCase().indexOf("ViewTrailerRental") < 0) {
                window.location.href = baseUrl + "TrailerRental/TrailerRental/ViewTrailerRental";
            } else if (document.getElementsByClassName("jconfirm").length >= 1) {
                return;
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
                window.location.href = baseUrl + "TrailerRental/TrailerRental/ViewTrailerRental";
            }
        }
    }
});
//

//#region Save Trailer Detail
//btnSave = function () {
    $("#btnSave").on("click", function () {
        
        if ($("#ddlCustomer").val() > 0) {
            if (glbTrailerRentalDetail.length > 0) {


                var values = {};
                values = GetJsonValue();
                var url = baseUrl + "/TrailerRental/TrailerRental/SaveTrailerRental"
                if (values.TrailerRentalId > 0) {
                    url = baseUrl + "/TrailerRental/TrailerRental/EditTrailerRental"
                }

                $.ajax({
                    url: url,
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
                            ClearTrailerRentalDetail();
                           // toastr.success(response.Message);
                            //setInterval(function () {
                            //    window.location.href = baseUrl + "/TrailerRental/TrailerRental/ViewTrailerRental";
                            //}, 1500)
                            $.alert({
                                title: 'Succes!',
                                content: "<b>" + response.Message + "</b>",
                                type: 'green',
                                typeAnimated: true,
                                buttons: {
                                    Ok: {
                                        btnClass: 'btn-green',
                                        action: function () {
                                            window.location.href = baseUrl + "/TrailerRental/TrailerRental/ViewTrailerRental";
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
                    error: function (xhr, status, error) {
                        
                        hideLoader();
                        // toastr.error("Something went wrong.");
                        AlertPopup("Something went wrong.");
                        //var err = eval("(" + xhr.responseText + ")");
                        //alert(err.Message);
                    }
                });
            }
            else {
                $.alert({
                    title: 'Alert!',
                    content: '<b>Please add Trailer Rental Detail.</b>',
                    type: 'red',
                    //typeAnimated: true,
                });
            }
        }
        else {

            $.alert({
                title: 'Alert!',
                content: '<b>Please select a customer.</b>',
                type: 'red',
                //typeAnimated: true,
            });
        }
    })
//}
//#endregion 



//#endregion

function ClearTrailerRentalDetail() {
    
    var ddlpickup = "<option selected='selected' value='0'>SEARCH PICKUP LOCATION </option>";
    $("#ddlPickupLocation").empty();
    $("#ddlPickupLocation").append(ddlpickup);
    $(".ddlPickupLocation").text("SEARCH PICKUP LOCATION");


    var ddldelivery = "<option  selected='selected' value='0'> SEARCH DELIVERY LOCATION </option>";
    $("#ddlDeliveryLocation").empty();
    $("#ddlDeliveryLocation").append(ddldelivery);
    $(".ddlDeliveryLocation").text("SEARCH DELIVERY LOCATION");
    $("#dtStartDate").val("");
    $("#dtEndDate").val("");
    $("#dtReturnedDate").val("");
    $("#txtNoOfDays").val("");
    $("#ddlEquipment").val("");
    $("#txtFeePerDay").val("");
    $("#txtFixedFee").val("");
    $("#txtTotalFee").val("");
    $("#ddlDeliveryDriver").val("0");
    $("#ddlPickUpDriver").val("0");
    $("#txtTrailerInstruction").val();
    $("#txtGrandTotal").val();

    $("#tblTrailerDetail").attr("data-row-no", 0);
    $("#btnAddTrailer").text("Add Trailer");
}

function ValidateStartAndEndDate() {
    
    var startDate = $("#dtStartDate").val();
    var endDate = $("#dtReturnedDate").val();
    if (endDate == "") {
        endDate = $("#dtEndDate").val();
    }

    if (startDate != "" && endDate != "") {
        if (new Date(startDate) < new Date(endDate)) {
            var endDate = new Date(endDate);
            var startDate = new Date(startDate);
            var dayDiff = Math.ceil((endDate - startDate) / (1000 * 60 * 60 * 24));
            if (dayDiff > 0) {

                $("#txtNoOfDays").val(dayDiff);
            }
            var feePerDay = $("#txtFeePerDay").val();
            var fixedFee = $("#txtFixedFee").val();

            if (feePerDay != "" && fixedFee != "") {
               // toastr.warning("Please enter Fee/Day or Fixed Fee.");
                AlertPopup("Please enter Fee/Day or Fixed Fee.");
                ("Something went wrong.");
                $("#txtFeePerDay").val("");
                $("#txtFixedFee").val("");
                $("#txtTotalFee").val("");
            }
            else {

               
                

                if (feePerDay != "") {
                    $("#txtFeePerDay").val(ConvertStringToFloat(feePerDay));
                    var totalFee = feePerDay * dayDiff;
                    $("#txtTotalFee").val(ConvertStringToFloat(totalFee));

                }

                if (fixedFee != "") {
                    $("#txtFixedFee").val(ConvertStringToFloat(fixedFee));
                    $("#txtTotalFee").val(ConvertStringToFloat(fixedFee));
                }
            }
        }

    }


}

//#region for open address popup
var openAddressModal = function () {
    $(".btnOpenAddressModal").on("click", function () {
        $('#formAddress').trigger("reset");

        $("#modalAddAddress").modal("show");
        $('#modalAddAddress').draggable();
    });
}
//#endregion


function ValidateEquipment() {
    
    var value = {};
    value.TrailerRentalId = $("#hdnTrailerRentalId").val();
    value.CustomerId = $("#ddlCustomer").val();
    value.FirstPickupArrivalDate = $("#dtStartDate").val();
    value.LastPickupArrivalDate = $("#dtEndDate").val();
    value.EquipmentId = $("#ddlEquipment").val();
    var EquipmentNo = $.trim($("#ddlEquipment").find('option:selected').text());
    $.ajax({
        url: baseUrl + 'Shipment/Shipment/ValidateEquipment',
        data: JSON.stringify(value),
        type: "POST",
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        // cache: false,
        success: function (data) {
            if (data.length > 0) {
                var IsShipment = false;
                var IsFumigation = false;
                var shipmenttable = "<div class='col-md-12 table-responsive'><table style='width: 100%;' class='table-bordered table-striped cf w-100'  ><tr><th>AWB</th><th>Customer Po</th></tr>"
                var fumigationtable = "<div class='col-md-12 table-responsive'><table style='width: 100%;' class='table-bordered table-striped cf w-100'  ><tr><th>AWB</th><th>Container No.</th></tr>"
                for (var i = 0; i < data.length; i++) {
                    if (data[i].IsShipment) {

                        shipmenttable += "<tr><td>" + data[i].AWB + "</td><td>" + data[i].CustomerPO + "</td></tr>"
                        IsShipment = true;
                    }
                    else {

                        fumigationtable += "<tr><td>" + data[i].AWB + "</td><td>" + data[i].ContainerNo + "</td></tr>"
                        IsFumigation = true;
                    }
                }
                shipmenttable += "</table></div>";
                fumigationtable += "</table></div>";

                shipmenttable = IsShipment ? ('<br/><br> &nbsp;&nbsp;In Shipment<br/>&nbsp;&nbsp&nbsp;&nbsp' + shipmenttable) : "";
                fumigationtable = IsFumigation ? ('<br/><br/>&nbsp;&nbsp;In Fumigation<br/>&nbsp;&nbsp&nbsp;&nbsp;' + fumigationtable) : "";
                $.confirm({
                    boxWidth: '50%',
                    title: "Confirmation!",
                    content: '<b>Equipment No.' + EquipmentNo + ' is already assigned. ' + shipmenttable + '' + fumigationtable + ' </b> ',
                    type: 'blue',

                    typeAnimated: true,
                    buttons: {
                        confirm: {
                            btnClass: 'btn-green',
                            action: function () {

                            }
                        },
                        cancel: {
                            btnClass: 'btn-red',
                            action: function () {
                                $("#ddlEquipment").val("");
                            }
                        }
                    }
                })
            }
        },
        error: function () { }
    });

}

function ValidateDriver(_this) {
    

    var value = {};
    value.TrailerRentalId = $("#hdnTrailerRentalId").val();
    value.CustomerId = $("#ddlCustomer").val();
    value.FirstPickupArrivalDate = $("#dtStartDate").val();
    value.LastPickupArrivalDate = $("#dtEndDate").val();
    value.DriverId = _this.value;
    var driverName = $.trim($(_this).find('option:selected').text());
    $.ajax({
        url: baseUrl + 'Shipment/Shipment/ValidateDriver',
        data: JSON.stringify(value),
        type: "POST",
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        // cache: false,
        success: function (data) {
            if (data.length > 0) {

                var IsShipment = false;
                var IsFumigation = false;
                var shipmenttable = "<div class='col-md-12 table-responsive'><table style='width: 100%;' class='table-bordered table-striped cf w-100'  ><tr><th>AWB</th><th>Customer Po</th></tr>"
                var fumigationtable = "<div class='col-md-12 table-responsive'><table style='width: 100%;' class='table-bordered table-striped cf w-100'  ><tr><th>AWB</th><th>Container No.</th></tr>"
                for (var i = 0; i < data.length; i++) {
                    if (data[i].IsShipment) {

                        shipmenttable += "<tr><td>" + data[i].AWB + "</td><td>" + data[i].CustomerPO + "</td></tr>"
                        IsShipment = true;
                    }
                    else {

                        fumigationtable += "<tr><td>" + data[i].AWB + "</td><td>" + data[i].ContainerNo + "</td></tr>"
                        IsFumigation = true;
                    }
                }
                shipmenttable += "</table></div>";
                fumigationtable += "</table></div>";

                shipmenttable = IsShipment ? ('<br/><br> &nbsp;&nbsp;In Shipment<br/>&nbsp;&nbsp&nbsp;&nbsp' + shipmenttable) : "";
                fumigationtable = IsFumigation ? ('<br/><br/>&nbsp;&nbsp;In Fumigation<br/>&nbsp;&nbsp&nbsp;&nbsp;' + fumigationtable) : "";
                $.confirm({
                    boxWidth: '50%',
                    title: "Confirmation!",
                    content: '<b>' + driverName + ' is already assigned. ' + shipmenttable + '' + fumigationtable + ' </b> ',
                    type: 'blue',

                    typeAnimated: true,
                    buttons: {
                        confirm: {
                            btnClass: 'btn-green',
                            action: function () {

                            }
                        },
                        cancel: {
                            btnClass: 'btn-red',
                            action: function () {
                                $(_this).val("0");
                            }
                        }
                    }
                })
            }
        },
        error: function () { }
    });
}
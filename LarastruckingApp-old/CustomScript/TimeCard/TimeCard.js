$(function () {

    //Bind  Equipment
    BindEquipment();

    //Bind Driver
    BindDriver();

    //Save Time Card detail
    btnSave();

});function BindEquipment() {    $.ajax({
        url: baseUrl + 'TimeCard/TimeCard/GetEquipmentList',
        data: {},
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

function BindDriver() {    $.ajax({
        url: baseUrl + 'TimeCard/TimeCard/GetDriverList',
        data: {},
        type: "POST",
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        // cache: false,
        success: function (data) {
            debugger
            ddlValue = "";
            $("#ddlDriver").empty();
            glbDriver = JSON.parse(JSON.stringify(data));
            ddlValue += '<option value="0">SELECT DRIVER</option>'
            for (var i = 0; i < glbDriver.length; i++) {
                ddlValue += '<option value="' + glbDriver[i].DriverId + '">' + glbDriver[i].DriverName + '</option>';
            }
            $("#ddlDriver").append(ddlValue);
           
        }
    });
}

//#region save Time Card Detail
var btnSave = function () {

    $("#btnSave").on("click", function () {
        SaveTimeCard();
    })
}
//#endregion

function SaveTimeCard() {
    debugger
    var values = {};
    values = GetJsonValue();
    if (values.DriverId > 0) {
        $.ajax({
            url: baseUrl + '/TimeCard/TimeCard/DriverTimeCard',
            data: JSON.stringify(values),
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                ClearSelection();
                if (data == true) {
                    if (values.IsCheckIn == true) {
                        SuccessPopup("Check-in Successfully");
                    }
                    else {
                      
                        SuccessPopup("Check-out Successfully");
                    }
                  
                }
                else {
                    AlertPopup("Try again.");


                }
            }
        });
    }
    else {
        AlertPopup("Please select driver.");
    }
}



function GetJsonValue() {
    debugger;
    var values = {};
    values.DriverId = $("#ddlDriver").val();
    values.EquipmentId = $("#ddlEquipment").val();
    values.IsCheckIn = $("input[name='Check']:checked").val() == "1" ? true : false;
    values.ScanDateTime = $("#dtScanDateTime").val();
    return values;
}

function ClearSelection() {
    $("#ddlDriver").val(0);
    $("#ddlEquipment").val("");
    $("#rdoCheckIn").prop("checked", true);
    $("#dtScanDateTime").val("");
}
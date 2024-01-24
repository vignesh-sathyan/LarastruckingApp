var glbDriver = [];
var textBoxVal;
var rowNo;
var PreviousEquipment = "";
$(document).ready(function () {
    
    // GetEquipmentList();
    btnPickUpContinue();
    btnDeliveryContinue();
    btnCancel();
  

});
textBoxVal = $("#txtPickUpEquipment").val();

$("table").on("mouseover", 'tr', function () {
    $(this).find("select").css('color', 'white');
    $(this).find('option').css('background-color', '#007bff');
});

$("table").on("mouseout", 'tr', function () {
    $(this).find("after").css('color', 'black');
    $(this).find("select").css('color', 'black');

    $(this).find('option').css('background-color', 'transparent');
});





//#region bind equipment popup
function GetEquipmentList() {
    
    glbDriver = [];
    var value = {};
    var fumigationType = $("#ddlFumigationType").val();
    if (fumigationType == 2) {//2==On Floor
        if ($("#hdnIsPickUpLocation").val() == "true") {
            value.FirstPickupArrivalDate = $("#dtPickUpArrival").val();
            value.LastPickupArrivalDate = $("#dtFumigationArrival").val();
        }
        else {
            value.FirstPickupArrivalDate = $("#dtRelease").val();
            value.LastPickupArrivalDate = $("#dtDeliveryArrival").val();
        }

    }
    else {
        value.FirstPickupArrivalDate = $("#dtPickUpArrival").val();
        value.LastPickupArrivalDate = $("#dtDeliveryArrival").val();
    }

    value.FumigationId = $("#hdnfumigationId").val();
    value.CustomerId = $("#ddlCustomer").val();
    console.log("values Equipment List: ",value);
    $('#tblEquipmentDetails').DataTable({
        "bInfo": false,
        select: 'single',
        responsive: true,
        processing: true,
        serverSide: true,
        searching: true,
        bDestroy: true,
        "language": {
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
        },
        "ajax": {
            "url": baseUrl + "/Shipment/Shipment/LoadData",
            "type": "POST",
            "datatype": "json",
            "data": value,
            async: false
        },
        "columns": [

            {
                "data": "EDID",
                "name": "EDID",
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    var RouteNo = $("input[name='rdSelectedRoute']:checked").val();
                    var rowNo = $("#tblShipmentDetail").attr("data-row-no");
                    var tblRowsCount = 0;
                    if (rowNo != "") {
                        tblRowsCount = JSON.parse($("#tblShipmentDetail").attr("data-row-no"));
                    }
                    
                    if ($("#hdnIsPickUpLocation").val() == "true") {
                        
                        var pickUpEquipmentNdriver = glbEquipmentNdriver.filter(x => (tblRowsCount > 0 ? (x.RouteNo > 0 ? x.RouteNo == RouteNo : x.RouteNo == 0) : x.RouteNo == 0) && x.EquipmentId == row.EDID && (x.IsPickUp == true || x.IsPickUp == "true"));
                        console.log("Equipment Session: ", sessionStorage.getItem("equipmentid") == row.EDID);
                        if (pickUpEquipmentNdriver.length > 0 || sessionStorage.getItem("equipmentid") == row.EDID) {

                            return '<input type="checkbox" onchange="CheckEquipment(this)" id="chkEquipment" class="chkEquipment" name="chkEquipment" checked="checked" data-equipment-name="' + row.EquipmentNo + '" value="' + row.EDID + '"/>'
                        }
                        else {
                            return '<input type="checkbox" onchange="CheckEquipment(this)" id="chkEquipment" name="chkEquipment" data-equipment-name="' + row.EquipmentNo + '" value="' + row.EDID + '"/>'

                        }

                    }
                    else {
                        var DeliveryEquipmentNdriver = glbEquipmentNdriver.filter(x => (tblRowsCount > 0 ? (x.RouteNo > 0 ? x.RouteNo == RouteNo : x.RouteNo == 0) : x.RouteNo == 0) && x.EquipmentId == row.EDID && (x.IsPickUp == false || x.IsPickUp == "false"));
                        if (DeliveryEquipmentNdriver.length > 0) {
                            return '<input type="checkbox"onchange="CheckEquipment(this)" id="chkEquipment" class="chkEquipment"  name="chkEquipment" checked="checked" data-equipment-name="' + row.EquipmentNo + '" value="' + row.EDID + '"/>'
                        }
                        else {
                            return '<input type="checkbox" onchange="CheckEquipment(this)" id="chkEquipment" class="chkEquipment" name="chkEquipment" data-equipment-name="' + row.EquipmentNo + '" value="' + row.EDID + '"/>'
                        }
                    }
                }
            },
            { "data": "VehicleType", "name": "VehicleType", "autoWidth": true },
            {
                "name": "EquipmentNo",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    if (row.EquipmentNo != null) {
                       // console.log("Equipment List table: ",row);
                        // return '<label data-toggle="tooltip" data-placement="top" title="' + row.EquipmentNo + '">' + SplitString(row.EquipmentNo, 5, true) + '</label>'
                        if (row.IsAssigned) {
                            return '<label data-toggle="tooltip" onclick="ValidateEquipment(' + row.EDID + ',' + row.EquipmentNo + ')" data-placement="top" title="' + row.EquipmentNo + '">' + SplitString(row.EquipmentNo, 8, true) + ' <i style="color:red;font-size: 1.5em;" class="fa fa-info-circle" aria-hidden="true"></i></label>'
                        }
                        else {
                            return '<label data-toggle="tooltip" data-placement="top" title="' + row.EquipmentNo + '">' + row.EquipmentNo + '</label>'
                        }
                    } else {
                        return 'NA'

                    }
                }
            },
            {
                "name": "MaxLoad",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    if (row.MaxLoad != null) {
                        return '<label data-toggle="tooltip" data-placement="top" title="' + row.MaxLoad + '">' + row.MaxLoad + '</label>'
                    } else {
                        return 'NA'

                    }
                }
            },
            {
                "name": "Bed",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    if (row.Bed != null) {
                        return '<label  data-toggle="tooltip" data-placement="top" title="' + row.Bed + '">' + row.Bed + '</label>'
                    } else {
                        return 'NA'

                    }
                }
            },
            {
                "name": "DoorType",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    if (row.DoorType != null) {
                        return '<label data-toggle="tooltip" data-placement="top" title="' + row.DoorType + '">' + row.DoorType + '</label>'
                    } else {
                        return 'NA'

                    }
                }
            },
            {
                "name": "FreightType",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    if (row.FreightType != null) {
                        return '<label ata-toggle="tooltip" data-placement="top" title="' + row.FreightType + '">' + row.FreightType + '</label>'
                    } else {
                        return 'NA'

                    }
                }
            },

            { "name": "Action", "autoWidth": true },
            { "name": "Action", "autoWidth": true },
        ],
        "order": [[0, "desc"]],
        columnDefs: [
            {

                "targets": 7,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var RouteNo = $("input[name='rdSelectedRoute']:checked").val();
                    var rowNo = $("#tblShipmentDetail").attr("data-row-no");
                    var tblRowsCount = 0;
                    if (rowNo != "") {
                        tblRowsCount = JSON.parse($("#tblShipmentDetail").attr("data-row-no"));
                    }
                    console.log("Equipment row: ",row);
                    
                    if ($("#hdnIsPickUpLocation").val() == "true") {
                        
                        var pickUpEquipmentNdriver = glbEquipmentNdriver.filter(x => (tblRowsCount > 0 ? (x.RouteNo > 0 ? x.RouteNo == RouteNo : x.RouteNo == 0) : x.RouteNo == 0) && x.EquipmentId == row.EDID && x.IsPickUp && x.DriverId > 0);
                        if (pickUpEquipmentNdriver.length > 0 && pickUpEquipmentNdriver[0].DriverId > 0 || sessionStorage.getItem("equipmentid") == row.EDID) {
                            return '<div class="action-ic">' +
                                '<input type=hidden id="EqId" name="EqId" value = "" />' +
                                '<select id="ddldriver" onchange="ValidateDriver(this)" style="width:130px;border:none;background-color:transparent;" name="ddldriver">' + GetDriverList(pickUpEquipmentNdriver[0].DriverId) + '</select>'
                            '</div>'
                        }
                        else {
                            return '<div class="action-ic">' +
                                '<input type=hidden id="EqId" name="EqId" value = "" />' +
                                '<select id="ddldriver" onchange="ValidateDriver(this)" style="width:130px;border:none;background-color:transparent;" name="ddldriver">' + GetDriverList(0) + '</select>'
                            '</div>'


                        }
                    } else {
                        var DeliveryEquipmentNdriver = glbEquipmentNdriver.filter(x => (tblRowsCount > 0 ? (x.RouteNo > 0 ? x.RouteNo == RouteNo : x.RouteNo == 0) : x.RouteNo == 0) && x.EquipmentId == row.EDID && x.IsPickUp == false && x.DriverId > 0);
                        if (DeliveryEquipmentNdriver.length > 0 && DeliveryEquipmentNdriver[0].DriverId > 0) {
                            return '<div class="action-ic">' +
                                '<input type=hidden id="EqId" name="EqId" value = "" />' +
                                '<select id="ddldriver" onchange="ValidateDriver(this)" style="width:130px;border:none;background-color:transparent;" name="ddldriver">' + GetDriverList(DeliveryEquipmentNdriver[0].DriverId) + '</select>'
                            '</div>'
                        }
                        else {
                            return '<div class="action-ic">' +
                                '<input type=hidden id="EqId" name="EqId" value = "" />' +
                                '<select id="ddldriver" onchange="ValidateDriver(this)" style="width:130px;border:none;background-color:transparent;" name="ddldriver">' + GetDriverList(0) + '</select>'
                            '</div>'


                        }
                    }


                }
            },
            {
                "targets": 8,
                "orderable": false,
                "render": function (data, type, row, meta) {

                    return ' <input type="text" autocomplete = "off" placeholder = "HH:MM"  value="" disabled=disabled id = "dtScheduledCheckIn" name="dtScheduledCheckIn"  class="form-control dtTimer" />'

                }
            },
        ]
    });

    oTable = $('#tblEquipmentDetails').DataTable();


}
//#endregion

//#region bind Check In Time
function BindCheckInTime(_this) {
    var driverId = _this.value;
    
    $.ajax({
        url: baseUrl + 'Shipment/Shipment/GetCheckInTime',
        data: { "DriverId": driverId },
        type: "GET",
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        // cache: false,
        success: function (data) {
            if (data != null) {
                row = $(_this).closest("tr");
                $(row).find("input[name=dtScheduledCheckIn]").val(dateFormat(ConvertDate(data, true), "HH:MM"));
            }
        },
        error: function () { }
    });

}
//#endregion

function SplitString(text, count, insertDots) {
    return text.slice(0, count) + (((text.length > count) && insertDots) ? "..." : "");
}
//Fill text box
function Equipmentdata(equipmentid, equipmentno) {

    $("#txtEquipment").val(equipmentno);
    $("#modalEquipment").modal('toggle');

}

//#region geting pickup location driver and equipment detail in textbox 
btnPickUpContinue = function () {

    
   
   // console.log("txtPickUpEquipment: ", routedetail.PickUpEquipment);
    //var SelectedEquipment = $(_this).attr("data-equipment-name");
    
  
    $("#btnPickUpContinue").on('click', function () {

        var selectedCount = 0;
        var tableUsers = $('#tblEquipmentDetails').DataTable();
        var rows = tableUsers.rows({ 'search': 'applied' }).nodes();
        console.log("selectedCount rows : ", rows);
        var RouteNo = $("input[name='rdSelectedRoute']:checked").val();
        var rowNo = $("#tblShipmentDetail").attr("data-row-no");
        var tblRowsCount = 0;
        if (rowNo != "") {
            tblRowsCount = JSON.parse($("#tblShipmentDetail").attr("data-row-no"));
        }
        GetJsonValue(); //Added by DART to prefill the equipment in delivery popup.
        console.log("CountOfCheckBox: ", glbEquipmentNdriver.filter(x => (x.IsPickUp == "true" || x.IsPickUp == true) && (tblRowsCount > 0 ? (x.RouteNo > 0 ? x.RouteNo == RouteNo : x.RouteNo == 0) : x.RouteNo == 0)).length );
        if (glbEquipmentNdriver.filter(x => (x.IsPickUp == "true" || x.IsPickUp == true) && (tblRowsCount > 0 ? (x.RouteNo > 0 ? x.RouteNo == RouteNo : x.RouteNo == 0) : x.RouteNo == 0)).length < 2) {
            if (glbEquipmentNdriver.filter(x => (x.IsPickUp == "true" || x.IsPickUp == true) && (tblRowsCount > 0 ? (x.RouteNo > 0 ? x.RouteNo == RouteNo : x.RouteNo == 0) : x.RouteNo == 0)).length > 0) {
                if ($("#hdnfumigationId").val() > 0) {

                    if ($("#ddlStatus").val() == 1)//ORDER TAKEN
                    {
                        $("#ddlStatus").val(2);//DISPATCHED 
                    }

                }
                GetPickUpEquipmentNDrier()
                $("#modalEquipment").modal('toggle');
            }
            else {
                // toastr.warning("Please select Equipment Number(s) & Driver(s).")
                AlertPopup("Please select Equipment Number(s) & Driver(s).")
            }
        }
        else {
            // toastr.warning("Please select Equipment Number(s) & Driver(s).")
            AlertPopup("More than One Equipment Selected. Please select any one equipment.")
        }
       
    })
}
//#endregion

//#region geting delivery location driver and equipment detail in textbox 
btnDeliveryContinue = function () {
    $("#btnDeliveryContinue").on('click', function () {
        var selectedCount = 0;
        var selectedIDs = [];

        // Loop through each checkbox with the class "checkbox"
        $('.chkEquipment').each(function () {
            if ($(this).is(':checked')) {
                var id = $(this).data('id');
                selectedCount++;
                selectedIDs.push(id);
            }
        });

       // console.log("selectedCount: ", selectedCount);

        var rowNo = $("#tblShipmentDetail").attr("data-row-no");
        var tblRowsCount = 0;
        if (rowNo != "") {
            tblRowsCount = JSON.parse($("#tblShipmentDetail").attr("data-row-no"));
        }

        var RouteNo = $("input[name='rdSelectedRoute']:checked").val();

         //var coutnofbox = glbEquipmentNdriver.filter(x => (x.IsPickUp == false || x.IsPickUp == "false") && (tblRowsCount > 0 ? (x.RouteNo > 0 ? x.RouteNo == RouteNo : x.RouteNo == 0) : x.RouteNo == 0)).length;
              //  console.log("coutnofbox: ", coutnofbox);

        if (glbEquipmentNdriver.filter(x => (x.IsPickUp == false || x.IsPickUp == "false") && (tblRowsCount > 0 ? (x.RouteNo > 0 ? x.RouteNo == RouteNo : x.RouteNo == 0) : x.RouteNo == 0)).length < 2) {

            if (glbEquipmentNdriver.filter(x => (x.IsPickUp == false || x.IsPickUp == "false") && (tblRowsCount > 0 ? (x.RouteNo > 0 ? x.RouteNo == RouteNo : x.RouteNo == 0) : x.RouteNo == 0)).length > 0) {
                if ($("#hdnfumigationId").val() > 0) {

                    if ($("#ddlStatus").val() == 9)//ORDER TAKEN
                    {
                        $("#ddlStatus").val(14);//DISPATCHED 
                    }

                }

                GetDeliveryEquipmentNDrier()
                $("#modalEquipment").modal('toggle');
            }
            else {
                //  toastr.warning("Please select Equipment Number(s) & Driver(s).")
                AlertPopup("Please select Equipment Number(s) & Driver(s).")


            }
        }
        else {
            //  toastr.warning("please select equipment number(s) & driver(s).")
            AlertPopup("More than One Equipment Selected. Please select any one equipment.")


        }
       
    })
}
//#endregion

//#region bind equipment and driver
function GetPickUpEquipmentNDrier() {
    var RouteNo = $("input[name='rdSelectedRoute']:checked").val();
    var rowNo = $("#tblShipmentDetail").attr("data-row-no");
    var tblRowsCount = 0;
    if (rowNo != "") {
        tblRowsCount = JSON.parse($("#tblShipmentDetail").attr("data-row-no"));
    }

    if ((glbEquipmentNdriver.filter(x => (x.IsPickUp == "true" || x.IsPickUp == true) && (tblRowsCount > 0 ? (x.RouteNo > 0 ? x.RouteNo == RouteNo : x.RouteNo == 0) : x.RouteNo == 0))).length > 0) {
        var equpments = "";
        var drivers = "";
        $.each((glbEquipmentNdriver.filter(x => (x.IsPickUp == "true" || x.IsPickUp == true) && (tblRowsCount > 0 ? (x.RouteNo > 0 ? x.RouteNo == RouteNo : x.RouteNo == 0) : x.RouteNo == 0))), function (index, message) {
            equpments = equpments + message.EquipmentName + ","
            if (message.DriverId > 0) {
                drivers = drivers + message.DriverName + ","
            }
        });

        equpments = equpments.substring(0, equpments.lastIndexOf(","));
        drivers = drivers.substring(0, drivers.lastIndexOf(","));

        $("#txtPickUpEquipment").val(equpments);
        // $("#hdnPickUpEquipment").val(JSON.stringify(pickUpEquipment));
        $("#txtPickUpdriver").val(drivers);
        if ($("#txtDeliveryEquipment").val() == "") {
            $("#txtDeliveryEquipment").val(equpments);
        }
        
        //$("#txtDeliveryDriver").val(drivers);

        /*console.log("glbEquipmentNdriver Get Json:", glbEquipmentNdriver);
        console.log("glbEquipmentNdriver Get Json:", glbEquipmentNdriver.length);

        var tempLen = glbEquipmentNdriver.length;
        for (let i = 0; i < tempLen; i++) {
            if (glbEquipmentNdriver.length >= 1 && glbEquipmentNdriver[i].IsPickUp == true) {
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
                if (glbEquipmentNdriver[i + 1] == null && (glbRouteStops.length % 2 == 0))) {
                    glbEquipmentNdriver.push(deliveryObj);
                }

            }
        }*/

    }
}
//#endregion

//#region bind equipment and driver
function GetDeliveryEquipmentNDrier() {
    var RouteNo = $("input[name='rdSelectedRoute']:checked").val();
    var rowNo = $("#tblShipmentDetail").attr("data-row-no");
    var tblRowsCount = 0;
    if (rowNo != "") {
        tblRowsCount = JSON.parse($("#tblShipmentDetail").attr("data-row-no"));
    }

    if ((glbEquipmentNdriver.filter(x => (x.IsPickUp == "false" || x.IsPickUp == false) && (tblRowsCount > 0 ? (x.RouteNo > 0 ? x.RouteNo == RouteNo : x.RouteNo == 0) : x.RouteNo == 0))).length > 0) {
        var equpments = "";
        var drivers = "";
        $.each((glbEquipmentNdriver.filter(x => (x.IsPickUp == "false" || x.IsPickUp == false) && (tblRowsCount > 0 ? (x.RouteNo > 0 ? x.RouteNo == RouteNo : x.RouteNo == 0) : x.RouteNo == 0))), function (index, message) {
            equpments = equpments + message.EquipmentName + ","
            if (message.DriverId > 0) {
                drivers = drivers + message.DriverName + ","
            }
        });

        equpments = equpments.substring(0, equpments.lastIndexOf(","));
        drivers = drivers.substring(0, drivers.lastIndexOf(","));

        $("#txtDeliveryEquipment").val(equpments);
        // $("#hdnDeliveryEquipment").val(JSON.stringify(deliveryEquipment));
        $("#txtDeliveryDriver").val(drivers);

    }
}
//#endregion

//#region clear driver and equipment  textbox

function clearDriverNEquipment() {

    $("#txtEquipment").val("");

    $("#txtdriver").val("");
    $("#hdnEquipment").val("");

}
//#endregion

//#region cancel equipment popup
btnCancel = function () {
    $("#btnCancel").on("click", function () {
        $("#modalEquipment").modal('toggle');
    })
}
//#endregion

//#region bind driver list on popup
function GetDriverList(driverId) {
    
    //var data = glbRouteStops;
    var value = {};
    if ($("#hdnIsPickUpLocation").val() == "true") {
        value.FirstPickupArrivalDate = $("#dtPickUpArrival").val();
        value.LastPickupArrivalDate = $("#dtFumigationArrival").val();
    }
    else {
        value.FirstPickupArrivalDate = $("#dtFumigationArrival").val();
        value.LastPickupArrivalDate = $("#dtDeliveryArrival").val();
    }

    value.FumigationId = $("#hdnfumigationId").val();
    value.CustomerId = $("#ddlCustomer").val();

    var ddlValue = "";
    if (glbDriver.length > 0) {
        ddlValue += '<option value="0">Select Driver</option>'
        for (var i = 0; i < glbDriver.length; i++) {
            var RouteNo = $("input[name='rdSelectedRoute']:checked").val();
            

            //  ddlValue += '<option value="' + glbDriver[i].DriverId + '">' + glbDriver[i].DriverName + '</option>';
            if (driverId > 0 && driverId == glbDriver[i].DriverId) {
                ddlValue += '<option selected value="' + glbDriver[i].DriverId + '">' + glbDriver[i].DriverName + '</option>';
            }
            else {
                ddlValue += '<option value="' + glbDriver[i].DriverId + '">' + glbDriver[i].DriverName + '</option>';
            }

        }
    }
    else {

        $.ajax({
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
                ddlValue += '<option value="0">Select Driver</option>'
                for (var i = 0; i < glbDriver.length; i++) {
                    //  ddlValue += '<option value="' + glbDriver[i].DriverId + '">' + glbDriver[i].DriverName + '</option>';
                    if (driverId > 0 && driverId == glbDriver[i].DriverId) {
                        ddlValue += '<option selected value="' + glbDriver[i].DriverId + '">' + glbDriver[i].DriverName + '</option>';
                    }
                    else {
                        ddlValue += '<option value="' + glbDriver[i].DriverId + '">' + glbDriver[i].DriverName + '</option>';
                    }
                }
            }
        });
    }
    return ddlValue;


}
//#endregion

function ValidateEquipment(equipmentId, EquipmentNo) {
    
    var data = glbRouteStops;
    var value = {};
    value.FumigationId = $("#hdnfumigationId").val();
    value.CustomerId = $("#ddlCustomer").val();
    var fumigationType = $("#ddlFumigationType").val();
    if (fumigationType == 2) {
        if ($("#hdnIsPickUpLocation").val() == "true") {
            value.FirstPickupArrivalDate = $("#dtPickUpArrival").val();
            value.LastPickupArrivalDate = $("#dtFumigationArrival").val();
        }
        else {
            value.FirstPickupArrivalDate = $("#dtRelease").val();
            value.LastPickupArrivalDate = $("#dtDeliveryArrival").val();
        }
    }
    else {
        value.FirstPickupArrivalDate = $("#dtPickUpArrival").val();
        value.LastPickupArrivalDate = $("#dtDeliveryArrival").val();

    }
    value.EquipmentId = equipmentId;
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
                    title: "",
                    //content: '<b>Equipment No. ' + EquipmentNo + ' is assigned. ' + shipmenttable + '' + fumigationtable + ' </b> ',
                    content: '<b>This equipment has cargo assigned to it. Please review to make sure additional cargo fits. ' + shipmenttable + '' + fumigationtable + ' </b> ',
                    type: 'blue',
                    typeAnimated: true,
                    buttons: {
                        OK: {
                            btnClass: 'btn-blue',
                            action: function () {

                            }
                        },

                    }
                })
            }
        },
        error: function () { }
    });

}

function ValidateDriver(_this) {

    var data = glbRouteStops;
    var value = {};
    value.FumigationId = $("#hdnfumigationId").val();
    value.CustomerId = $("#ddlCustomer").val();
    if ($("#hdnIsPickUpLocation").val() == "true") {
        value.FirstPickupArrivalDate = $("#dtPickUpArrival").val();
        value.LastPickupArrivalDate = $("#dtFumigationArrival").val();
    }
    else {
        value.FirstPickupArrivalDate = $("#dtRelease").val();
        value.LastPickupArrivalDate = $("#dtDeliveryArrival").val();
    }
    //console.log("glbEquipmentNdriver.length: " + glbEquipmentNdriver.length); 
    value.DriverId = _this.value;
    var driverName = $.trim($(_this).find('option:selected').text());
    console.log("value.DriverId: ", value.DriverId);
    $("#hdnDriver").val(value.DriverId);
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
                    title: "",
                    //content: '<b>' + driverName + ' is assigned. ' + shipmenttable + '' + fumigationtable + ' </b> ',
                    content: '<b>This driver has shipments assigned to them. Please review to make sure they have time to handle additional shipments.' + shipmenttable + '' + fumigationtable + ' </b> ',
                    type: 'blue',

                    typeAnimated: true,
                    buttons: {
                        confirm: {
                            btnClass: 'btn-blue',
                            action: function () {
                                if (glbEquipmentNdriver.length > 0) {
                                    row = $(_this).closest("tr");
                                    var routeNo = $("input[name='rdSelectedRoute']:checked").val();


                                    var EqNum;
                                    var checkboxes = document.querySelectorAll("input[id='chkEquipment']");
                    for (var i = 0; i < checkboxes.length; i++) {
                        if (checkboxes[i].checked) {
                            EqNum = checkboxes[i].value;
                            
                        }
                        else {
                            console.log("EqNum false: ", EqNum);
                        }
                                    }
                                    console.log("EqNum: ", EqNum);
                                    var equipmentId = JSON.parse(EqNum);
                                    var rowNo = $("#tblShipmentDetail").attr("data-row-no");
                                    var tblRowsCount = 0;
                                    if (rowNo != "") {
                                        tblRowsCount = JSON.parse($("#tblShipmentDetail").attr("data-row-no"));
                                    }

                                    if (equipmentId > 0) {
                                        var IsPickup = JSON.parse($("#hdnIsPickUpLocation").val());
                                        var Index = glbEquipmentNdriver.findIndex(x => x.EquipmentId == equipmentId && (tblRowsCount > 0 ? (x.RouteNo > 0 ? x.RouteNo == routeNo : x.RouteNo == 0) : x.RouteNo == 0) && x.IsPickUp == IsPickup);
                                        if (Index > -1) {
                                            glbEquipmentNdriver[Index].DriverId = $(row).find("select[name=ddldriver]").val();
                                            glbEquipmentNdriver[Index].DriverName = $(row).find("select[name=ddldriver]").find('option:selected').text();
                                        }
                                    }
                                }
                            }
                        },
                        cancel: {
                            action: function () {
                                $("#ddldriver").val("0");
                            }
                        }
                    }
                })
            }
            else {
                
                if (glbEquipmentNdriver.length > 0) {
                    row = $(_this).closest("tr");
                    var routeNo = $("input[name='rdSelectedRoute']:checked").val();
                    var rowNo = $("#tblShipmentDetail").attr("data-row-no");
                    var tblRowsCount = 0;
                    if (rowNo != "") {
                        tblRowsCount = JSON.parse($("#tblShipmentDetail").attr("data-row-no"));
                    }

                    var checkboxes = document.querySelectorAll("input[id='chkEquipment']");
                    var EqNum;
                    for (var i = 0; i < checkboxes.length; i++) {
                        if (checkboxes[i].checked) {
                            EqNum = checkboxes[i].value;
                           
                        }
                        else {
                            console.log("EqNum false: ", EqNum);
                        }
                    }
                    console.log("EqNum: ", EqNum);
                    var equipmentId = JSON.parse(EqNum);
                    if (equipmentId > 0) {
                        var IsPickup = JSON.parse($("#hdnIsPickUpLocation").val());
                        var Index = glbEquipmentNdriver.findIndex(x => x.EquipmentId == equipmentId && (tblRowsCount > 0 ? (x.RouteNo > 0 ? x.RouteNo == routeNo : x.RouteNo == 0) : x.RouteNo == 0) && x.IsPickUp == IsPickup);
                        if (Index > -1) {

                            glbEquipmentNdriver[Index].DriverId = $(row).find("select[name=ddldriver]").val();
                            glbEquipmentNdriver[Index].DriverName = $(row).find("select[name=ddldriver]").find('option:selected').text();
                        }
                    }
                }
            }
        },
        error: function () { }
    });
    BindCheckInTime(_this);
}



function CheckEquipment(_this) {

    //rowNo = $("#tblShipmentDetail").attr("data-row-no");
    //console.log("textBoxVal: ", textBoxVal);
    //console.log("rowNo: ", rowNo);
    //console.log("glbRouteStops: ", glbRouteStops);
    //var routedetail = glbRouteStops[rowNo];
    //PreviousEquipment = routedetail.PickUpEquipment;
    //console.log("PreviousEquipment: ", PreviousEquipment);
    
    if ($(_this).is(":checked")) {
      
        //var selectedCount = $('.chkEquipment:checked').length;
        var table = $('#tblEquipmentDetails').DataTable();
        // console.log("selectedCount: ", selectedCount);
        var data_row = table.row($(_this).closest('tr')).data();



        var data = glbRouteStops;
        var value = {};
        value.FumigationId = $("#hdnfumigationId").val();
        value.CustomerId = $("#ddlCustomer").val();
        var fumigationType = $("#ddlFumigationType").val();

        if (fumigationType == 2) {//2==On Floor
            if ($("#hdnIsPickUpLocation").val() == "true") {
                value.FirstPickupArrivalDate = $("#dtPickUpArrival").val();
                value.LastPickupArrivalDate = $("#dtFumigationArrival").val();
            }
            else {
                value.FirstPickupArrivalDate = $("#dtRelease").val();
                value.LastPickupArrivalDate = $("#dtDeliveryArrival").val();
            }

        }
        else {
            value.FirstPickupArrivalDate = $("#dtPickUpArrival").val();
            value.LastPickupArrivalDate = $("#dtDeliveryArrival").val();
        }
        value.EquipmentId = data_row.EDID;
        $("#EqId").val(data_row.EDID);
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
                        title: "",
                        //content: '<b>Equipment No. ' + data_row.EquipmentNo + ' is assigned. ' + shipmenttable + '' + fumigationtable + ' </b> ',
                        content: '<b>This equipment has cargo assigned to it. Please review to make sure additional cargo fits.' + shipmenttable + '' + fumigationtable + ' </b> ',
                        type: 'blue',

                        typeAnimated: true,
                        buttons: {
                            Continue: {
                                btnClass: 'btn-blue',
                                action: function () {

                                    row = $(_this).closest("tr");
                                    console.log("Equipment Id: ", JSON.parse($(row).find("input[name=chkEquipment]").val()));
                                    var pickObj = {
                                        FumigationEquipmentNDriverId: 0,
                                        IsPickUp: JSON.parse($("#hdnIsPickUpLocation").val()),
                                        EquipmentId: JSON.parse($(row).find("input[name=chkEquipment]").val()),
                                        EquipmentName: $(row).find("input[name=chkEquipment]").attr("data-equipment-name"),
                                        DriverId: JSON.parse($(row).find("select[name=ddldriver]").val()),
                                        DriverName: $(row).find("select[name=ddldriver]").find('option:selected').text(),
                                        RouteNo: 0,
                                        IsDeleted: false

                                    };
                                    glbEquipmentNdriver.push(pickObj);
                                    console.log("glbEquipmentNdriver Pick: ", glbEquipmentNdriver);
                                }
                            },
                            cancel: {
                                action: function () {
                                    $(_this).prop("checked", false);
                                }
                                //btnClass: 'btn-red',
                                //alert();
                                // $(_this).prop("checked":false);
                            }

                        }
                    })
                }
                else {

                    row = $(_this).closest("tr");
                    glbEquipmentNdriver.push({
                        FumigationEquipmentNDriverId: 0,
                        IsPickUp: JSON.parse($("#hdnIsPickUpLocation").val()),
                        EquipmentId: JSON.parse($(row).find("input[name=chkEquipment]").val()),
                        EquipmentName: $(row).find("input[name=chkEquipment]").attr("data-equipment-name"),
                        DriverId: JSON.parse($(row).find("select[name=ddldriver]").val()),
                        DriverName: $(row).find("select[name=ddldriver]").find('option:selected').text(),
                        RouteNo: 0,
                        IsDeleted: false,
                    });
                }
            },
            error: function () { }
        });

    }
    else {
        PreviousEquipment = "";
        row = $(_this).closest("tr");
        var routeNo = $("input[name='rdSelectedRoute']:checked").val();
        var equipmentId = JSON.parse($(row).find("input[name=chkEquipment]").val());
        var isPickUp = JSON.parse($("#hdnIsPickUpLocation").val());
        var rowNo = $("#tblShipmentDetail").attr("data-row-no");
        var tblRowsCount = 0;
        if (rowNo != "") {
            tblRowsCount = JSON.parse($("#tblShipmentDetail").attr("data-row-no"));
        }

        var index = glbEquipmentNdriver.findIndex(x => x.EquipmentId == equipmentId && x.IsPickUp == isPickUp && (tblRowsCount > 0 ? (x.RouteNo > 0 ? x.RouteNo == routeNo : x.RouteNo == 0) : x.RouteNo == 0));
        if (index > -1) {
            glbEquipmentNdriver.splice(index, 1);
        }

    }


}
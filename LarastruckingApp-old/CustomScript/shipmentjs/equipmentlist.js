var glbDriver = [];

$(document).ready(function () {

    // GetEquipmentList();
    btnContinue();
    btnCancel();
    GetEquipmentList();
});

$("table").on("mouseover", 'tr', function () {
    $(this).find("select").css('color', 'white');
    $(this).find('option').css('background-color', '#007bff');
});

$("table").on("mouseout", 'tr', function () {

    $(this).find("select").css('color', 'black');
    $(this).find('option').css('background-color', 'transparent');
});
//#region bind equipment popup
function GetEquipmentList() {
    debugger;
    var data = glbRouteStops;

    var value = {};
    value.ShipmentId = $("#hdnShipmentId").val();
    value.CustomerId = $("#ddlCustomer").val();
    if (data.length > 0) {

        value.FirstPickupArrivalDate = data[0].PickDateTime;
        value.LastPickupArrivalDate = data[data.length - 1].DeliveryDateTimeTo;
    }
    glbDriver = [];
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
            "data": value,
            "datatype": "json",
            async: true
        },
        "columns": [

            {
                "data": "EDID",
                "name": "EDID",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    var equipments = equipment.filter(x => $.trim(x.EquipmentId) == $.trim(row.EDID));

                    if (equipments.length > 0) {

                        return '<input type="checkbox" onchange="CheckEquipment(this)" id="chkEquipment" name="chkEquipment" checked="checked" data-equipment-name="' + row.EquipmentNo + '" value="' + row.EDID + '"/>'
                    }
                    else {
                        return '<input type="checkbox" onchange="CheckEquipment(this)" id="chkEquipment" name="chkEquipment"  data-equipment-name="' + row.EquipmentNo + '" value="' + row.EDID + '"/>'
                    }

                }
            },
            { "data": "VehicleType", "name": "VehicleType", "autoWidth": true },
            {
                "name": "EquipmentNo",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    if (row.EquipmentNo != null) {
                        if (row.IsAssigned) {
                            return '<label data-toggle="tooltip" onclick="ValidateEquipment(' + row.EDID + ',' + row.EquipmentNo + ')" data-placement="top" title="' + row.EquipmentNo + '">' + SplitString(row.EquipmentNo, 8, true) + ' <i style="color:red;font-size: 1.5em;" class="fa fa-info-circle" aria-hidden="true"></i></label>'
                        }
                        else {
                            return '<label data-toggle="tooltip" data-placement="top" title="' + row.EquipmentNo + '">' + SplitString(row.EquipmentNo, 8, true) + '</label>'
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
                        return '<label data-toggle="tooltip" data-placement="top" title="' + row.Bed + '">' + row.Bed + '</label>'
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
                        return '<label data-toggle="tooltip" data-placement="top" title="' + row.FreightType + '">' + row.FreightType + '</label>'
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
                    var equipments = equipment.filter(x => $.trim(x.EquipmentId) == $.trim(row.EDID) && x.DriverId > 0);

                    if (equipments.length > 0 && equipments[0].DriverId > 0) {

                        return '<div class="action-ic">' +
                            '<select id="ddldriver" onchange="ValidateDriver(this)" style="width:130px;border:none;background-color:transparent;" name="ddldriver">' + GetDriverList(equipments[0].DriverId) + '</select>'
                        '</div>'

                    }
                    else {
                        return '<div class="action-ic">' +
                            '<select id="ddldriver" onchange="ValidateDriver(this)" style="width:130px;border:none;background-color:transparent;" name="ddldriver">' + GetDriverList(0) + '</select>'
                        '</div>'
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


function SplitString(text, count, insertDots) {
    return text.slice(0, count) + (((text.length > count) && insertDots) ? "..." : "");
}
//Fill text box
function Equipmentdata(equipmentid, equipmentno) {

    $("#txtEquipment").val(equipmentno);
    $("#modalEquipment").modal('toggle');

}

//#region geting equipment detail in textbox 
btnContinue = function () {
    $("#btnContinue").on('click', function () {

        //equipment = [];
        //$("input[name=chkEquipment]:checked").each(function () {
        //    row = $(this).closest("tr");

        //    equipment.push({
        //        ShipmentEquipmentNDriverId: 0,
        //        EquipmentId: $(row).find("input[name=chkEquipment]").val(),
        //        EquipmentName: $(row).find("input[name=chkEquipment]").attr("data-equipment-name"),
        //        DriverId: $(row).find("select[name=ddldriver]").val(),
        //        DriverName: $(row).find("select[name=ddldriver]").find('option:selected').text()

        //    })
        //});

        if (equipment.length > 0) {
            //#region usable code in future
            //var data = glbRouteStops;
            //var value = {};
            //value.ShipmentId = $("#hdnShipmentId").val();
            //value.CustomerId = $("#ddlCustomer").val();
            //if (data.length > 0) {

            //    value.FirstPickupArrivalDate = data[0].PickDateTime;
            //    value.LastPickupArrivalDate = data[data.length - 1].DeliveryDateTimeTo;
            //}
            //value.ShipmentEquipmentNdriver = equipment;
            //$.ajax({
            //    url: baseUrl + 'Shipment/Shipment/ValidateEquipmentNDriver',
            //    data: JSON.stringify(value),
            //    type: "POST",
            //    async: false,
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    // cache: false,
            //    success: function (data) { },
            //    error: function () { }
            //});
            //#endregion
            getEquipmentNDrier();

            if ($("#hdnShipmentId").val() > 0) {
                debugger;
                if ($("#ddlStatus").val() == 1)//ORDER TAKEN
                {
                    $("#ddlStatus").val(2);//DISPATCHED 
                }

            }

            $("#modalEquipment").modal('toggle');
        }
        else {
            //toastr.warning("Please select Equipment Number(s) & Driver(s).")
            AlertPopup("Please select Equipment Number(s) & Driver(s).");
        }
    })
}
//#endregion

//#region bind equipment and driver
function getEquipmentNDrier() {
    if (equipment.length > 0) {


        var equpments = "";
        var drivers = "";
        $.each(equipment, function (index, message) {
            equpments = equpments + message.EquipmentName + ","
            if (message.DriverId > 0) {
                drivers = drivers + message.DriverName + ","
            }
        });

        equpments = equpments.substring(0, equpments.lastIndexOf(","));
        drivers = drivers.substring(0, drivers.lastIndexOf(","));

        $("#txtEquipment").val(equpments);
        $("#hdnEquipment").val(JSON.stringify(equipment));
        $("#txtdriver").val(drivers);

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

    var data = glbRouteStops;
    if (data.length > 0) {
        var value = {};
        value.ShipmentId = $("#hdnShipmentId").val();
        value.CustomerId = $("#ddlCustomer").val();
        value.FirstPickupArrivalDate = data[0].PickDateTime;
        value.LastPickupArrivalDate = data[data.length - 1].DeliveryDateTimeTo;
        var ddlValue = "";
        if (glbDriver.length > 0) {
            ddlValue += '<option value="0">Select Driver</option>'
            for (var i = 0; i < glbDriver.length; i++) {

                if (driverId > 0 && driverId == glbDriver[i].DriverId) {
                    ddlValue += '<option isTSACertificate ="' + glbDriver[i].IsTSACertificate + '" selected value="' + glbDriver[i].DriverId + '">' + glbDriver[i].DriverName + '</option>';
                }
                else {
                    ddlValue += '<option isTSACertificate ="' + glbDriver[i].IsTSACertificate + '"  value="' + glbDriver[i].DriverId + '">' + glbDriver[i].DriverName + '</option>';
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
                        //ddlValue += '<option value="' + glbDriver[i].DriverId + '">' + glbDriver[i].DriverName + '</option>';

                        if (driverId > 0 && driverId == glbDriver[i].DriverId) {

                            ddlValue += '<option isTSACertificate ="' + glbDriver[i].IsTSACertificate + '" selected value="' + glbDriver[i].DriverId + '">' + glbDriver[i].DriverName + '</option>';
                        }
                        else {
                            ddlValue += '<option isTSACertificate ="' + glbDriver[i].IsTSACertificate + '" value="' + glbDriver[i].DriverId + '">' + glbDriver[i].DriverName + '</option>';
                        }
                    }
                }
            });
        }
        return ddlValue;
    }
}
//#endregion

function ValidateEquipment(equipmentId, EquipmentNo) {
    var data = glbRouteStops;
    var value = {};
    value.ShipmentId = $("#hdnShipmentId").val();
    value.CustomerId = $("#ddlCustomer").val();
    if (data.length > 0) {

        value.FirstPickupArrivalDate = data[0].PickDateTime;
        value.LastPickupArrivalDate = data[data.length - 1].DeliveryDateTimeTo;
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
                    content: '<b>This equipment has cargo assigned to it. Please review to make sure additional cargo fits.' + shipmenttable + '' + fumigationtable + ' </b> ',
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
    debugger
    var istsacertificate = $(_this).find('option:selected').attr("isTSACertificate") == "true" ? true : false;

    var IsBonded = false;
    if (glbFreightDetail.length > 0) {

        IsBonded = glbFreightDetail.filter(x => x.FreightTypeId == 2).length > 0 ? true : false;

    }


    if (IsBonded) {
        if (istsacertificate) {
            ValidateDrivers(_this);
            BindCheckInTime(_this);
        }
        else {
            $.alert({
                title: 'Alert!',
                content: '<b>This driver has not TSA Certificate.</b>',
                type: 'red',
                typeAnimated: true,
            });
            $("#ddldriver").val(0);
        }

    }
    else {
        ValidateDrivers(_this);
        BindCheckInTime(_this);
    }


}
//#region bind Check In Time
function BindCheckInTime(_this) {
    var driverId = _this.value;
    debugger
    $.ajax({
        url: baseUrl + 'Shipment/Shipment/GetCheckInTime',
        data: { "DriverId": driverId},
        type: "GET",
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        // cache: false,
        success: function (data) {
            if (data != null) {
                row = $(_this).closest("tr");
                $(row).find("input[name=dtScheduledCheckIn]").val(dateFormat(ConvertDate(data,true),"HH:MM" ));
            }
        },
        error: function () { }
    });

}
//#endregion

function ValidateDrivers(_this) {
    var data = glbRouteStops;
    var value = {};
    value.ShipmentId = $("#hdnShipmentId").val();
    value.CustomerId = $("#ddlCustomer").val();
    if (data.length > 0) {

        value.FirstPickupArrivalDate = data[0].PickDateTime;
        value.LastPickupArrivalDate = data[data.length - 1].DeliveryDateTimeTo;
    }
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
                    title: "",
                    //content: '<b>' + driverName + ' is assigned. ' + shipmenttable + '' + fumigationtable + ' </b> ',
                    content: '<b>This driver has shipments assigned to them. Please review to make sure they have time to handle additional shipments. ' + shipmenttable + '' + fumigationtable + ' </b> ',
                    type: 'blue',

                    typeAnimated: true,
                    buttons: {
                        confirm: {
                            btnClass: 'btn-blue',
                            action: function () {
                                debugger
                                if (equipment.length > 0) {
                                    row = $(_this).closest("tr");
                                    var equipmentId = $(row).find("input[name=chkEquipment]").val();
                                    if (equipmentId > 0) {
                                        var Index = equipment.findIndex(x => x.EquipmentId == equipmentId);
                                        if (Index > -1) {
                                            equipment[Index].DriverId = $(row).find("select[name=ddldriver]").val();
                                            equipment[Index].DriverName = $(row).find("select[name=ddldriver]").find('option:selected').text();

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
                debugger
                if (equipment.length > 0) {
                    row = $(_this).closest("tr");
                    var equipmentId = $(row).find("input[name=chkEquipment]").val();
                    if (equipmentId > 0) {
                        var Index = equipment.findIndex(x => x.EquipmentId == equipmentId);
                        if (Index > -1) {
                            equipment[Index].DriverId = $(row).find("select[name=ddldriver]").val();
                            equipment[Index].DriverName = $(row).find("select[name=ddldriver]").find('option:selected').text();
                        }
                    }
                }
            }
        },
        error: function () { }
    });
}

function CheckEquipment(_this) {
    debugger
    if ($(_this).is(":checked")) {
        var table = $('#tblEquipmentDetails').DataTable();

        var data_row = table.row($(_this).closest('tr')).data();

        var data = glbRouteStops;
        var value = {};
        value.ShipmentId = $("#hdnShipmentId").val();
        value.CustomerId = $("#ddlCustomer").val();
        if (data.length > 0) {

            value.FirstPickupArrivalDate = data[0].PickDateTime;
            value.LastPickupArrivalDate = data[data.length - 1].DeliveryDateTimeTo;
        }
        value.EquipmentId = data_row.EDID;
        $.ajax({
            url: baseUrl + 'Shipment/Shipment/ValidateEquipment',
            data: JSON.stringify(value),
            type: "POST",
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            // cache: false,
            success: function (data) {
                debugger
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
                        content: '<b>This equipment has cargo assigned to it. Please review to make sure additional cargo fits. ' + shipmenttable + '' + fumigationtable + ' </b> ',
                        type: 'blue',

                        typeAnimated: true,
                        buttons: {
                            Continue: {
                                btnClass: 'btn-blue',
                                action: function () {
                                    row = $(_this).closest("tr");
                                    debugger
                                    equipment.push({
                                        ShipmentEquipmentNDriverId: 0,
                                        EquipmentId: $(row).find("input[name=chkEquipment]").val(),
                                        EquipmentName: $(row).find("input[name=chkEquipment]").attr("data-equipment-name"),
                                        DriverId: $(row).find("select[name=ddldriver]").val(),
                                        DriverName: $(row).find("select[name=ddldriver]").find('option:selected').text(),

                                    })
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
                    equipment.push({
                        ShipmentEquipmentNDriverId: 0,
                        EquipmentId: $(row).find("input[name=chkEquipment]").val(),
                        EquipmentName: $(row).find("input[name=chkEquipment]").attr("data-equipment-name"),
                        DriverId: $(row).find("select[name=ddldriver]").val(),
                        DriverName: $(row).find("select[name=ddldriver]").find('option:selected').text(),

                    })
                }
            },
            error: function () { }
        });

    }
    else {
        debugger
        row = $(_this).closest("tr");
        var equipmentId = $(row).find("input[name=chkEquipment]").val();
        equipment = equipment.filter(x => x.EquipmentId != equipmentId);
    }
}
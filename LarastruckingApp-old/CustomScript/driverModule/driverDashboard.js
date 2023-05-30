//#region Ready State
$(function () {
    var lat;
    var long;
    var formatedCurrentAddress;
    getLocation();
    preTripShipmentDetail();
    savePreTripCheckList();
    saveFumigationPreTripCheckList();
    // For Save GPS Tracking History
    window.setInterval(fn_SaveGPSTracker, 5000);
});
//#endregion


$("#tblPreTripDetails").on("mouseover", 'tr', function () {
    $(this).find(".chng-color-Pickup").css('color', 'white');
    $(this).find(".chng-color-Deli").css('color', 'white');
    $(this).find(".chng-color-edit").css('color', 'white');

});

$("#tblPreTripDetails").on("mouseout", 'tr', function () {
    $(this).find(".chng-color-Pickup").css('color', '#007bff');
    $(this).find(".chng-color-Deli").css('color', '#007bff');
    $(this).find(".chng-color-edit").css('color', '#007bff');


});

$('#tblPreTripDetails').on('dblclick', 'tbody tr', function () {
    var table = $('#tblPreTripDetails').DataTable();
    var data_row = table.row($(this).closest('tr')).data();
    var encryptedRowIds = Encrypt(data_row.Id);
    if (data_row.Types == "Shipment") {
        //window.location.href = baseUrl + '/Driver/Detail/' + encodeURIComponent(encryptedRowIds);
        window.location.href = baseUrl + '/Driver/Detail/' + encryptedRowIds;
    }
    else {
        window.location.href = baseUrl + '/Driver/FumigationDetails/' + encryptedRowIds;
    }
});

//#region To redirect Direction Map 
function fn_getDirection(myLocation) {
    //#region code done by abid for map functionality
    //let _url = baseUrl + "Driver/GPSTracker?location='" + myLocation + "'";
    //debugger;
    ////alert(_url)
    //window.open(_url, '_blank');
    // myLocation = 'APK Perks Pvt.Ltd, Block-H, Bulding Number 141, 3rd Floor, Sector 63, Noida, Uttar Pradesh 201301';
    //#endregion

    // myLocation = myLocation.replace(/^[^,]+, */, '')
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

$('.myLocationClick').on('click', function (event) {
    alert('clicked');
});

//#region DataTable Binding
var preTripShipmentDetail = function () {
    $('#tblPreTripDetails').DataTable({
        //"bInfo": false,
        // dom: 'Blfrtip',
        select: 'single',
        responsive: true,
        filter: true,
        processing: true,
        serverSide: true,
        searching: true,
        bDestroy: true,
        //stateSave: true,

        "language": {
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
        },
        "ajax": {
            "url": baseUrl + "Driver/GetPreTripShipmentDetails",
            "type": "POST",
            "datatype": "json",
        },

        "columns": [



            {
                "data": "Id",
                "name": "Id",
                "autoWidth": true,
                "visible": false,
            },
            {
                "name": "StatusName",
                "autoWidth": true,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var PreStatus = "";
                    PreStatus = '<span class="badge" style="background-color:' + row.Colour + ';color:' + row.FontColor + '">' + row.StatusName + '</span>'
                    return PreStatus; StatusCheckForShipment(row.StatusName)
                }
            },
            {
                "data": "PickupLocation",
                "name": "PickupLocation",
                "autoWidth": true,
                "orderable": false,
                "render": function (data, type, row, meta) {

                    if (row.PickupLocation != null && row.PickupLocation != '') {
                        var pickuplocaton = row.PickupLocation.split("|");
                        var pickupdata = "";
                        if (pickuplocaton.length > 0) {
                            for (var i = 0; i < pickuplocaton.length; i++) {
                                pickupdata += "<a  href='javascript: void(0)' class='chng-color-Pickup notranslate' onclick=\"(fn_getDirection('" + GetAddress(pickuplocaton[i]) + " '))\" >  <label  data-toggle='tooltip' data-placement='top' title='" + GetAddress(pickuplocaton[i]) + "'>" + GetCompanyName(pickuplocaton[i]) + "</label></a><br/>"
                            }
                            pickupdata = pickupdata.trim("<br/>");
                            return pickupdata;
                        }
                    } else {
                        return 'NA'

                    }
                }
            },
            {
                "data": "DeliveryLocation",
                "name": "DeliveryLocation",
                "autoWidth": true,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    if (row.DeliveryLocation != null && row.DeliveryLocation != '') {
                        var deliveryLocation = row.DeliveryLocation.split("|");
                        var Deliverydata = "";
                        if (deliveryLocation.length > 0) {
                            for (var i = 0; i < deliveryLocation.length; i++) {
                                Deliverydata += "<a href='javascript: void(0)' class='chng-color-Deli notranslate' onclick=\"(fn_getDirection('" + GetAddress(deliveryLocation[i]) + " '))\" >  <label  data-toggle='tooltip' data-placement='top' title='" + GetAddress(deliveryLocation[i]) + "'>" + GetCompanyName(deliveryLocation[i]) + "</label></a><br/>"
                            }
                            Deliverydata = Deliverydata.trim("<br/>");
                            return Deliverydata;
                        }
                    } else {
                        return 'NA'

                    }
                }
            },
            {
                "data": "AirWayBill",
                "name": "AirWayBill",
                "autoWidth": true,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    if (row.AirWayBill != null && row.AirWayBill != '') {
                        var AirWayBill = row.AirWayBill.split(",");
                        var AirWayBillNO = "";
                        if (AirWayBill.length > 0) {
                            for (var i = 0; i < AirWayBill.length; i++) {
                                if (AirWayBill[i] != "") {
                                    AirWayBillNO += '<label data-toggle="tooltip" data-placement="top" >' + AirWayBill[i] + '</label><br/>'
                                }
                            }
                            AirWayBillNO = AirWayBillNO.trim("<br/>");
                            return AirWayBillNO;
                        }
                    } else {
                        return 'NA'

                    }
                }
            },

            { "data": "DriverEquipment", "name": "DriverEquipment", "autoWidth": true, "orderable": false },
            {
                "data": "Types",
                "name": "Types",
                "autoWidth": true,
                "orderable": false,
            },
            {
                //"data": "QuantityNMethod",
                "name": "QuantityNMethod",
                "autoWidth": true,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    if (row.QuantityNMethod != null && row.QuantityNMethod != '') {
                        return row.QuantityNMethod.replace(/,\s*$/, "");
                    }
                    else {
                        return 'NA';
                    }

                }
            },
            {
                "name": "PreTripStatus",
                "autoWidth": true,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    if (row.Types == "Shipment") {
                        return '<a href="javascript:void(0)" onclick="updatePreTripDetail(' + row.Id + ')">' + StatusCheck(row.PreTripStatus) + '</a>'
                    }
                    else {
                        return '<a href="javascript:void(0)" onclick="updateFumigationPreTripDetail(' + row.Id + ')">' + StatusCheck(row.PreTripStatus) + '</a>'
                    }
                }

            },
            //{
            //    "data": "DriverInstruction",
            //    "name": "DriverInstruction",
            //    "autoWidth": true,
            //    "orderable": false,
            //},
            {
                "name": "Action",
                "orderable": false,
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    var encryptedRowId = Encrypt(row.Id); //Function from globalJs file to encryptId 
                    if (row.Types == "Shipment") {
                        var btnShipmentDetail = '<a href="../Driver/Detail/' + encryptedRowId + '" class="edit_icon chng-color-edit" title="Edit" >'
                        //'<span class="custom-btn2">Start</span>' +
                        ////'<span class="custom-btn">Start Now<span><i class="fas fa-chevron-right"></i></span></span>' +
                        //'</a> ';
                        if ($.trim(row.StatusName).toLowerCase() == $.trim("DISPATCHED").toLowerCase()) {
                            btnShipmentDetail += '<span class="custom-btn2">Start</span></a>';
                        }
                        else {
                            btnShipmentDetail += '<span style="background: #ffc107;" class="custom-btn2">Next</span></a>';
                        }
                        return '<div class="action-ic">' + btnShipmentDetail + '</div>'
                    }
                    else {
                        var btnFumigationDetail = '<a href="../Driver/FumigationDetails/' + encryptedRowId + '" class="edit_icon chng-color-edit" title="Edit" >'
                        //'<span class="custom-btn2">Start</span>' +
                        ////'<span class="custom-btn">Start Now<span><i class="fas fa-chevron-right"></i></span></span>' +
                        //'</a> ';
                        if ($.trim(row.StatusName).toLowerCase() == $.trim("DISPATCHED").toLowerCase()) {
                            btnFumigationDetail += '<span  class="custom-btn2">Start</span></a>';
                        }
                        else {
                            btnFumigationDetail += '<span style="background: #ffc107;" class="custom-btn2">Next</span></a>';
                        }
                        return '<div class="action-ic">' + btnFumigationDetail + '</div>'
                    }
                }
            }

        ],
        "order": [[0, "desc"]],
    });

    oTable = $('#tblPreTripDetails').DataTable();

}
//#endregion

////#region Status Colour Change

//function StatusCheckForShipment(status) {

//    var PreStatus = "";
//    if ($.trim(status) == "Order Taken") {
//        PreStatus = '<span style=""">' + status + '</span>'
//    }
//    if ($.trim(status) == "Dispatched") {
//        PreStatus = '<span class="badge" style="background-color:#0071c3;color:white">' + status + '</span>'
//    }
//    if ($.trim(status) == "Immediate Attention") {
//        PreStatus = '<span class="badge" style="background-color:#f10802;color:white">' + status + '</span>'
//    }
//    if ($.trim(status) == "Change of Status / On Hold") {
//        PreStatus = '<span class="badge" style="background-color:#e8610b;color:white">' + status + '</span>'
//    }
//    if ($.trim(status) == "Loading at pick up location") {
//        PreStatus = '<span class="badge" style="background-color:#f86807;color:white">' + status + '</span>'
//    }
//    if ($.trim(status) == "In-Route to delivery location") {
//        PreStatus = '<span class="badge" style="background-color:#fffd01;color:black">' + status + '</span>'
//    }
//    if ($.trim(status) == "Delivered") {
//        PreStatus = '<span class="badge" style="background-color:#178a24;color:white">' + status + '</span>'
//    }
//    if ($.trim(status) == "Cancelled") {
//        PreStatus = '<span class="badge" style="background-color:#d93d42;color:white">' + status + '</span>'
//    }
//    if ($.trim(status) == "In-Progress") {
//        PreStatus = '<span class="badge" style="background-color:#95ccdb;color:white">' + status + '</span>'
//    }
//    return PreStatus;
//}

////#endregion

//#region Pre Trip Status check 

function StatusCheck(status) {
    var PreStatus = "";
    if (status == "PENDING") {
        PreStatus = '<span class="badge badge-warning">' + status + '</span>'
    }
    if (status == "INPROGRESS") {
        PreStatus = '<span class="badge badge-danger">' + status + '</span>'
    }
    if (status == "COMPLETE") {
        PreStatus = '<span class="badge badge-success">' + status + '</span>'
    }
    return PreStatus;
}

//#endregion

//#region Update Pre-Trip Information
var updatePreTripDetail = function (shippingId) {
    getPreTripCheckList(shippingId);
    $("#modalPreTrip").modal("show");
}
//#endregion

//#region Get Pre-Trip Check List Details
var getPreTripCheckList = function (shippingId) {
    $.ajax({
        url: baseUrl + "Driver/GetPreTripCheckList/" + shippingId,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            showLoader();
        },
        success: function (response) {
            if (response != null) {
                $("#hdnPreTripCheckupId").val(response.PreTripCheckupId);
                $("#hdnShipmentId").val(response.ShipmentId);
                $("#txtShipmentNumber").val(response.ShipmentRefNo);
                $("#hdnEquipmentId").val(response.EquipmentId);
                $("#txtEquipmentNumber").val(response.EquipmentNo);

                if (response.IsTiresGood !== null) {
                    $("input[name=IsTiresGood][value=" + response.IsTiresGood + "]").prop('checked', true);
                }
                if (response.IsBreaksGood !== null) {
                    $("input[name=IsBreaksGood][value=" + response.IsBreaksGood + "]").prop('checked', true);

                }
                if (response.Fuel !== null) {
                    $("input[name=Fuel][value='" + response.Fuel + "']").prop('checked', true);
                }
                if (response.LoadStraps !== null) {
                    $("input[name=LoadStraps][value='" + response.LoadStraps + "']").prop('checked', true);

                }

                $("#txtOveralCondition").val(response.OverAllCondition);
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

//#region Get TextBox Values
var getParameterValues = function () {
    var dto = {};
    dto.EquipmentId = $("#hdnEquipmentId").val();

    dto.Fuel = $.trim($("input[name='Fuel']:checked").val());
    if (dto.Fuel == "") {
        dto.Fuel = null
    }

    dto.IsBreaksGood = $.trim($("input[name='IsBreaksGood']:checked").val());
    if (dto.IsBreaksGood == "") {
        dto.IsBreaksGood = null
    }

    dto.IsTiresGood = $.trim($("input[name='IsTiresGood']:checked").val());
    if (dto.IsTiresGood == "") {
        dto.IsTiresGood = null
    }

    dto.LoadStraps = $.trim($("input[name='LoadStraps']:checked").val());

    dto.OverAllCondition = $.trim($("#txtOveralCondition").val());


    dto.PreTripCheckupId = $.trim($("#hdnPreTripCheckupId").val());
    dto.ShipmentId = $.trim($("#hdnShipmentId").val());
    return dto;
}
//#endregion

//#region Save Check List
var savePreTripCheckList = function () {
    $("#BtnAdd").on("click", function () {
        var dto = getParameterValues();
        $.ajax({
            url: baseUrl + "Driver/SavePreTripCheckList",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(dto),
            beforeSend: function () {
                showLoader();
            },
            success: function (response) {
                preTripShipmentDetail();
            },
            error: function (jqXHR, textStatus, errorThrown) {
            },
            complete: function () {
                $("#modalPreTrip").modal("hide");
                hideLoader();

            }
        });
    })
}
//#endregion

//#region GPS Tracker

var fn_SaveGPSTracker = function () {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var latitude = position.coords.latitude;
            var longitude = position.coords.longitude;

            saveLocation(latitude, longitude);


        });
    } else {
        console.log("Browser doesn't support geolocation!");
    }
}

function saveLocation(latitude, longitude) {

    // Date Time Format 
    var d = new Date($.now());
    date = ((d.getMonth() + 1) + "-" + d.getDate() + "-" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds());

    var dto = [];
    var data = {};
    data.UserId = 0;
    data.Latitude = latitude;
    data.longitude = longitude;
    data.CreatedOn = date;

    dto.push(data);
    $.ajax({
        url: baseUrl + "Driver/SaveGPSTracker",
        type: "POST",
        dataType: "json",
        //contentType: false,
        // processData: false,
        data: { dto: dto },
        beforeSend: function () {
            //showLoader();
        },
        success: function (response) {

        },
        error: function (jqXHR, textStatus, errorThrown) {
        },
        complete: function () {
            $("#divPopUpSign").modal("hide");
            // hideLoader();

        }
    });
}

//#endregion

//#region Update Fumigation Pre-Trip Information
var updateFumigationPreTripDetail = function (FumigationId) {
    getFumigationPreTripCheckList(FumigationId);
    $("#modalFumigationPreTrip").modal("show");
}
//#endregion

//#region Get Pre-Trip Check List Details
var getFumigationPreTripCheckList = function (FumigationId) {
    $.ajax({
        url: baseUrl + "Driver/GetPreTripCheckFumigationList/" + FumigationId,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            showLoader();
        },
        success: function (response) {
            if (response != null) {
                $("#hdnFumigationPreTripCheckupId").val(response.FumigationPreTripCheckupId);
                $("#hdnFumigationId").val(response.FumigationId);
                $("#txtShipmentNumber").val(response.ShipmentRefNo);
                $("#hdnEquipmentId").val(response.EquipmentId);
                $("#txtEquipmentNumber").val(response.EquipmentNo);

                if (response.IsTiresGood !== null) {
                    $("input[name=IsTiresGood][value=" + response.IsTiresGood + "]").prop('checked', true);
                }
                if (response.IsBreaksGood !== null) {
                    $("input[name=IsBreaksGood][value=" + response.IsBreaksGood + "]").prop('checked', true);

                }
                if (response.Fuel !== null) {
                    $("input[name=Fuel][value='" + response.Fuel + "']").prop('checked', true);
                }
                if (response.LoadStraps !== null) {
                    $("input[name=LoadStraps][value='" + response.LoadStraps + "']").prop('checked', true);

                }

                $("#txtOveralCondition").val(response.OverAllCondition);
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

//#region Get TextBox Values
var getFumiParameterValues = function () {
    var dto = {};
    dto.EquipmentId = $("#hdnEquipmentId").val();

    dto.Fuel = $.trim($("input[name='Fuel']:checked").val());
    if (dto.Fuel == "") {
        dto.Fuel = null
    }

    dto.IsBreaksGood = $.trim($("input[name='IsBreaksGood']:checked").val());
    if (dto.IsBreaksGood == "") {
        dto.IsBreaksGood = null
    }

    dto.IsTiresGood = $.trim($("input[name='IsTiresGood']:checked").val());
    if (dto.IsTiresGood == "") {
        dto.IsTiresGood = null
    }

    dto.LoadStraps = $.trim($("input[name='LoadStraps']:checked").val());

    dto.OverAllCondition = $.trim($("#txtOveralCondition").val());


    dto.FumigationPreTripCheckupId = $.trim($("#hdnFumigationPreTripCheckupId").val());
    dto.FumigationId = $.trim($("#hdnFumigationId").val());
    return dto;
}
//#endregion

//#region Save Fumigation Pre TripCheckUp List
var saveFumigationPreTripCheckList = function () {
    $("#BtnAddFumigation").on("click", function () {
        var dto = getFumiParameterValues();
        $.ajax({
            url: baseUrl + "Driver/SaveFumigationPreTripCheckList",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(dto),
            beforeSend: function () {
                showLoader();
            },
            success: function (response) {
                preTripShipmentDetail();
            },
            error: function (jqXHR, textStatus, errorThrown) {
            },
            complete: function () {
                $("#modalFumigationPreTrip").modal("hide");
                hideLoader();

            }
        });
    })
}
//#endregion

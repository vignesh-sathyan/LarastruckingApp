$(document).ready(function () {

    $(".divSearchBox").hide();
    GetOrderTakenShipmentList();
    //setTimeout(function () { GetOtherStatusShipmentList(); }, 1000);
   // var length = table.page.info().recordsTotal;
  

    // Call the function initially
   
    GetOtherStatusShipmentList();
    //bindCustomerDropdown();
    //bindCustomerDropdown2();
    btnViewShipment();
    btnViewShipment2();
    startEndDate();

    updateShipmentOrderTakenCount();
    updateShipmentInProgressCount();
    $("#shipmentProgress").text(updateShipmentOrderTakenCount() + updateShipmentInProgressCount());
    $("#shipmentOrder").text(updateShipmentOrderTakenCount() + updateShipmentInProgressCount());
    //console.log("shipment order count: ", updateShipmentOrderTakenCount());
    
    GetDriverShipment();
	// Replace 'New York' and 'Los Angeles' with your desired city names
	findDistanceBetweenCities('miami', 'pompano');
    //GetFreightType();
    $('#tblShipmentDetails input').unbind();

});

// Function to get coordinates (latitude and longitude) of a city using Google Maps Geocoding API
async function getCoordinates(cityName) {
  const apiKey = 'AIzaSyDXBtkoqacvZToBGD9TCaiZ9qUTrAJDJN4'; // Replace with your Google Maps API key
  const geocodeEndpoint = `https://maps.googleapis.com/maps/api/geocode/json?address=${encodeURIComponent(cityName)}&key=${apiKey}`;

  try {
    const response = await fetch(geocodeEndpoint);
    const data = await response.json();
	console.log("geocode response: ",data);
    if (data.status === 'OK') {
      const location = data.results[0].geometry.location;
      return location;
    } else {
      throw new Error('Unable to geocode the city.');
    }
  } catch (error) {
    console.error('Error:', error.message);
    return null;
  }
}

// Function to calculate distance between two sets of coordinates using Haversine formula
function calculateDistance(coord1, coord2) {
  const R = 6371; // Radius of the Earth in kilometers
  const lat1 = deg2rad(coord1.lat);
  const lon1 = deg2rad(coord1.lng);
  const lat2 = deg2rad(coord2.lat);
  const lon2 = deg2rad(coord2.lng);

  const dLat = lat2 - lat1;
  const dLon = lon2 - lon1;

  const a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
    Math.cos(lat1) * Math.cos(lat2) *
    Math.sin(dLon / 2) * Math.sin(dLon / 2);

  const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
  const distance = R * c; // Distance in kilometers

  return distance;
}

// Helper function to convert degrees to radians
function deg2rad(deg) {
  return deg * (Math.PI / 180);
}

// Example usage
async function findDistanceBetweenCities(city1, city2) {
  const city1Coords = await getCoordinates(city1);
  const city2Coords = await getCoordinates(city2);

  if (city1Coords && city2Coords) {
    const distance = calculateDistance(city1Coords, city2Coords);
    console.log(`The distance between ${city1} and ${city2} is approximately ${distance.toFixed(2)} kilometers.`);
  }
}




$("#dropdownMenuLink").click(function () {
    console.log("clicked");
    $("#menutab").css("z-index", "0");
});


$(window).scroll(function () {
    var scrollTop = $(this).scrollTop();
    //console.log("scrolling: ", scrollTop);
    if (scrollTop > 10) {
        $("#menutab").css("z-index", "2");
    } else {
        //$("#menutab").css("z-index", "0");
    }
});
//#region change color on hover
$("table").on("mouseover", 'tr', function () {

    $(this).find(".far").css('color', 'white');
    $(this).find(".fa-map-marked-alt").css('color', 'white');
   //$(this).find(".fa-bell").css('color', 'white');
    
});

$("table").on("mouseout", 'tr', function () {

    $(this).find(".far").css('color', '#007bff');
    $(this).find(".fa-download").css('color', '#007bff');
    $(this).find(".fa-map-marked-alt").css('color', '#007bff');
    $(this).find(".fa-trash-alt").css('color', 'red');
    //$(this).find(".fa-bell").css('color', 'red');
});
var table = $('#tblShipmentDetails').dataTable();

function updateShipmentOrderTakenCount() {
    var count = 0;
    $.ajax({
        type: 'GET',
        url: baseUrl + "/Shipment/Shipment/GetOrderTaken",
        //data: { "driverid": driverid },
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        success: function (msg) {
            // Do something interesting here.
            //console.log("count order taken", msg);
            count = msg;
           
          //  $("#shipmentOrder").text(count);
        },
        error: function (xhr, err) {
            console.log("error : " + err);
        }
    })
    return count;
   
}

function ShipmentCustomerDetail(shipmentid) {
    var count = 0;
    $.ajax({
        type: 'GET',
        url: baseUrl + "/Shipment/Shipment/CustomerDetail",
        data: { "shipmentid": shipmentid },
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        success: function (msg) {
            // Do something interesting here.
            //console.log("customer Detail: ", msg);
            count = msg;
            $("#cName").text(msg.CustomerName);
            $("#cAddress").text(msg.Address);
            $("#cContact").text(msg.Contact);
            $("#cEmail").text(msg.Email);
            $("#cPhone").text(msg.Phone);
            //$("#shipmentOrder").text(count);
        },
        error: function (xhr, err) {
            console.log("error : " + err);
        }
    })

}

function updateShipmentInProgressCount() {
    var count = 0;
    $.ajax({
        type: 'GET',
        url: baseUrl + "/Shipment/Shipment/GetShipmentInProgress",
        //data: { "driverid": driverid },
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        success: function (msg) {
            // Do something interesting here.
          
            count = msg;
            //$("#shipmentProgress").text(count);
        },
        error: function (xhr, err) {
            console.log("error : " + err);
        }
    })
    return count;

}



function GetDriverShipment() {
    var count = 0;
    var eq;
    var colorbg;
    var fontcolor;
    $.ajax({
        type: 'GET',
        url: baseUrl + "/Shipment/Shipment/DriverDetail",
        //data: { "driverid": driverid },
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        success: function (msg) {
            // Do something interesting here.
            console.log("Driver detail box: ",msg);
            count = msg;
            for (let x = 0; x < msg.length; x++) {
                if (msg[x].DriverName != null) {
                    //console.log("driverName: ", msg[x].DriverName);
                    var DeliveryLocation = msg[x].DeliveryLocation.split("|");
                    var DriverName = msg[x].DriverName.split("$");
                    var DashBoardName = msg[x].DashboardName.split("$")[0];
                    var DashBoardId = msg[x].DashboardName.split("$")[1];
                    //console.log("DeliveryLocation: ", DeliveryLocation.length);
                    for (let v = 0; v < DeliveryLocation.length; v++) {
                        if (msg[x].Status == "DISPATCHED") {
                            colorbg = "#bdd7ee";
                            fontcolor = "#000";
                        }
                        else if (msg[x].Status =="IN-FUMIGATION") {
                            colorbg = "#fe9900";
                            fontcolor = "#fff";
                        }
                        else if (msg[x].Status == "DELIVERED") {
                            colorbg = "#92d14f";
                            fontcolor = "#000";
                        }
                        else if (msg[x].Status == "IN-ROUTE") {
                            colorbg = "#fffd01";
                            fontcolor = "#000";
                        }
                        else if (msg[x].Status == "DRIVER ASSIGNED") {
                            colorbg = "#385687";
                            fontcolor = "#fff";
                        }
						 else if (msg[x].Status == "ARRIVED AT PICK UP LOCATION") {
							 msg[x].Status = "ARRIVED AT PU";
                            colorbg = "#fe9900";
                            fontcolor = "#fff";
                        }
						 else if (msg[x].Status == "ARRIVED AT DELIVERY LOCATION") {
							 msg[x].Status = "ARRIVED AT DEL";
                            colorbg = "#fe9900";
                            fontcolor = "#fff";
                        }
						else if (msg[x].Status == "UNLOADING AT DELIVERY LOCATION") {
							 msg[x].Status = "UNLOADING";
                            colorbg = "#fe9900";
                            fontcolor = "#fff";
                        }
                        else {
                            colorbg = "#fe9900";
                            fontcolor = "#000";
                        }
                        //colorbg = "";
                       // console.log("DeliveryLocation Address: ", GetCAddressNew(DeliveryLocation[v]));
                        eq += '<tr><td style="padding: 0;text-align: center;"><a href="' + baseUrl + '/' + DashBoardName + '/' + DashBoardName + '/Index/' + DashBoardId +'" style="color: #000;padding:10px;display:block;" class="DriverDashboard" target="_blank">' + DriverName[v] + '</a></td><td class="statusTab" style="white-space: pre;"><span style="background:' + colorbg + ';color:' + fontcolor +';padding: 2px 5px;border-radius: 5px;">' + msg[x].Status + '</span></td><td style="padding-bottom: 5px;padding-top: 5px;">' + DeliveryLocation[v].replace('$',', ') + '</td></tr>';
                    }

                }


               // $("#DriverName").text(msg[x].DriverName);
            }
            $("#DeliveryDet").append(eq);
           // $("#shipmentProgress").text(count);
        },
        error: function (xhr, err) {
            console.log("error : " + err);
        }
    })

}

//function GetDriverShipment() {                             //Gokul S (8-6-2023)
//    var count = 0;
//    var eq = ""; // Initialize eq variable as an empty string
//    var eqDispatch = ""; // Initialize eqDispatch variable as an empty string
//    var colorbg;
//    var fontcolor;
//    $.ajax({
//        type: 'GET',
//        url: baseUrl + "/Shipment/Shipment/DriverDetail",
//        //data: { "driverid": driverid },
//        contentType: 'application/json; charset=utf-8',
//        dataType: 'json',
//        async: false,
//        success: function (msg) {
//            count = msg;
//            for (let x = msg.length - 1; x >= 0; x--) { // Iterate in reverse order
//                if (msg[x].DriverName != null) {
//                    var DeliveryLocation = msg[x].DeliveryLocation.split("|");
//                    var DriverName = msg[x].DriverName.split("$");
//                    for (let v = 0; v < DeliveryLocation.length; v++) {
//                        if (msg[x].Status == "DISPATCHED") {
//                            colorbg = "#bdd7ee";
//                            fontcolor = "#000";
//                            eqDispatch += '<tr><td>' + DriverName[v] + '</td><td class="statusTab" style="white-space: pre;"><span style="background:' + colorbg + ';color:' + fontcolor +';padding: 2px 5px;border-radius: 5px;">' + msg[x].Status + '</span></td><td style="padding-bottom: 5px;padding-top: 5px;">' + DeliveryLocation[v].replace('$',', ') + '</td></tr>';
//                        }
//                        else {
//                            colorbg = "#fe9900";
//                            fontcolor = "#000";
//                            eq += '<tr><td>' + DriverName[v] + '</td><td class="statusTab" style="white-space: pre;"><span style="background:' + colorbg + ';color:' + fontcolor +';padding: 2px 5px;border-radius: 5px;">' + msg[x].Status + '</span></td><td style="padding-bottom: 5px;padding-top: 5px;">' + DeliveryLocation[v].replace('$',', ') + '</td></tr>';
//                        }
//                    }
//                }
//            }
//            $("#DeliveryDet").append(eq);
//            $("#DeliveryDet").append(eqDispatch); // Append the "DISPATCH" entries after the other entries
//        },
//        error: function (xhr, err) {
//            console.log("error : " + err);
//        }
//    });
//}

// For Redirecting to respective dashboard on double clicking the blue stripes 
$('#ShipmentRequest').on('dblclick',  function () {

    window.location.href = baseUrl + '/Shipment/Shipment/ViewShipmentList';
    
});
$('#ShipmentInProgress').on('dblclick', function () {

    window.location.href = baseUrl + '/Shipment/Shipment/ViewShipmentList';

});
$('#FumigationRequest').on('dblclick', function () {

    window.location.href = baseUrl + '/Fumigation/Fumigation/ViewFumigationList';

});
$('#FumigationInProgress').on('dblclick', function () {

    window.location.href = baseUrl + '/Fumigation/Fumigation/ViewFumigationList';

});

$('#tblShipmentDetails').on('dblclick', 'tbody tr', function () {
    var table = $('#tblShipmentDetails').DataTable();
    var data_row = table.row($(this).closest('tr')).data();
    window.location.href = baseUrl + '/Shipment/Shipment/Index/' + data_row.ShipmentId;
   // updateRowCount();
});
$('#tblShipmentDetails').on('click', 'tbody tr', function () {
    var table = $('#tblShipmentDetails').DataTable();
    var data_row = table.row($(this).closest('tr')).data();
    console.log("data_row: ", data_row);
    ShipmentCustomerDetail(data_row.ShipmentId);
    $("#ShipmentNotify").css("display","block");
    var iframe = $('#ShipmentNotify');
    var iframes = $('#gpsLive');
    iframes.attr('src', baseUrl + '/GpsTracker/GpsTracker/MainGpsTracker/' + data_row.ShipmentId + '?Equipment=' + data_row.Equipment);
    // Set the src attribute
    iframe.attr('src', baseUrl + '/Shipment/Shipment/ShipmentNotificationMaster/' + data_row.ShipmentId);

 
    
    //window.location.href = baseUrl + '/Shipment/Shipment/Index/' + data_row.ShipmentId;
    
});

$('#ShipmentNotify').on('load', function () {
    // Wait for the iframe to load
    var iframeContents = $('#ShipmentNotify').contents();

    // Attach a click event handler to the iframe contents
    iframeContents.on('click', function () {
        var iframeSrc = $('#ShipmentNotify').attr('src');
        var changeSrc = iframeSrc.replace('ShipmentNotificationMaster', 'ViewShipmentNotification');
        window.open(changeSrc, '_blank');
        // Perform any actions you need here
    });
});

$('#driver-list1').on('scroll', function () {
    var scrollTop = $(this).scrollTop();
   // console.log("scrolling");
    if (scrollTop > 0) {
        $('#DeliveryDet th').addClass('sticky-header');
    } else {
        $('#DeliveryDet th').removeClass('sticky-header');
    }
});
$('#tblShipmentDetails2').on('click', 'tbody tr', function () {
    var table = $('#tblShipmentDetails2').DataTable();
    var data_row = table.row($(this).closest('tr')).data();
    console.log("data_row for progress: ", data_row);
    ShipmentCustomerDetail(data_row.ShipmentId);
    $("#ShipmentNotify").css("display", "block");
    var iframe = $('#ShipmentNotify');

    // Set the src attribute
    iframe.attr('src', baseUrl + '/Shipment/Shipment/ShipmentNotificationMaster/' + data_row.ShipmentId);
    var iframes = $('#gpsLive');
    iframes.attr('src', baseUrl + '/GpsTracker/GpsTracker/MainGpsTracker/' + data_row.ShipmentId + '?Equipment=' + data_row.Equipment);
    //window.location.href = baseUrl + '/Shipment/Shipment/Index/' + data_row.ShipmentId;

});
//#endregion

$('#tblShipmentDetails input').keyup(function (e) {
    if (e.keyCode == 13) /* if enter is pressed */ {
        table.search($(this).val()).draw();
    }
});
$.fn.dataTable.ext.errMode = function (settings, helpPage, message) {
    //GetOrderTakenShipmentList(); 
    //GetOtherStatusShipmentList();
};

var d = new Date();
var month = d.getMonth() + 1;
var day = d.getDate();

var datetime = (month < 10 ? '0' : '') + month + '/' +
    (day < 10 ? '0' : '') + day + '/' +
    d.getFullYear() + "  " +
    (d.getHours() < 10 ? '0' : '') + d.getHours() + ":" +
    (d.getMinutes() < 10 ? '0' : '') + d.getMinutes() + ":" +
    (d.getSeconds() < 10 ? '0' : '') + d.getSeconds();

//#region Bind shipment
function GetOrderTakenShipmentList() {
    var values = {};
    values.StartDate = $("#dtStartedDate").val();
    values.EndDate = $("#dtEndDate").val();
    values.CustomerId = $("#ddlCustomer").val();
    values.IsOrderTaken = true;
    values.FreightTypeId = $("#ddlFreightType").val();
    values.ColumnName = null;
    values.SortedColumns = null;




   // console.log('Mainshipment Values : ' ,values);
    
    $('#tblShipmentDetails').DataTable({
        //"bInfo": false,
        dom: 'Blfrtip',

        buttons: [
            {
                extend: 'print',
                title: "",
                text: '<img src="../../Assets/images/printer.png" style="height:18px;margin-right: 5px;width:16px;"/> Print',
                messageBottom: datetime,
                exportOptions: {
                    // columns: ':visible',
                    stripHtml: false,
                    columns: [1, 2, 3, 4, 5, 6, 7, 8]
                },
                customize: function (win) {
                    var last = null;
                    var current = null;
                    var bod = [];


                    var css = '@page { size: landscape; }',
                        head = win.document.head || win.document.getElementsByTagName('head')[0],
                        style = win.document.createElement('style');

                    style.type = 'text/css';
                    style.media = 'print';

                    if (style.styleSheet) {
                        style.styleSheet.cssText = css;
                    }
                    else {
                        style.appendChild(win.document.createTextNode(css));
                    }

                    head.appendChild(style);
                    $(win.document.body)
                        .css('font-size', '10pt')
                        .prepend(
                            "<table id='9'><tr><td width='80%' ><b>REQUESTED SHIPMENTS</b></td><td width='20%'><div><img src='"+baseUrl+"/Images/Laraslogo.png' height='100px'/></div></td></tr></table>"
                        );


                }

            },

        ],

        select: 'single',
        "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
        filter: true,
        orderMulti: true,
        responsive: true,
        processing: true,
        serverSide: true,
        searching: true,
        bDestroy: true,
        stateSave: true,
        "language": {
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
        },
        "ajax": {
            "url": baseUrl + "/Shipment/Shipment/ViewShipment",
            "type": "POST",
            // console.log("values: ", values);
            "data": values,
            //"async": false,
            "datatype": "json",
        },
        "columns": [
            { "data": "ShipmentId", "name": "ShipmentId", "autoWidth": true, },
            { "data": "CustomerName", "name": "CustomerName", "autoWidth": true },
            { "data": "PickUpLocation", "name": "PickUpLocation", "autoWidth": true },
            { "data": "PickupDate", "name": "PickupDate", "autoWidth": "8%" },
            { "data": "DeliveryLocation", "name": "DeliveryLocation", "autoWidth": true },
            { "data": "DeliveryDate", "name": "DeliveryDate", "autoWidth": true },
            { "data": "AirWayBill", "name": "AirWayBill", "width": "7%" },
            { "data": "Quantity", "name": "Quantity", "autoWidth": true },
            { "data": "CreatedByName", "name": "CreatedByName", "autoWidth": true },
            { "data": "IsReady", "name": "IsReady", "autoWidth": true },
            { "data": "Comments", "name": "Comments", "autoWidth": true },
            { "name": "Action", "autoWidth": true },
        ],
        "order": [[0, "asc"]],
        columnDefs: [



            {
                "targets": 2,
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                   // console.log("shipment Order Taken: ", row);
                    if (row.PickupLocation != null && row.PickupLocation != '') {
                        var pickupList = row.PickupLocation.split('$');

                        var pickupdata = "";
                        if (pickupList.length > 0) {
                            for (var i = 0; i < pickupList.length; i++) {
                                pickupdata += '<label data-toggle="tooltip" data-placement="top" title="' + GetCAddress(pickupList[i]) + '">' + GetCompany(pickupList[i]) + '</label><br/>'
                            }
                            pickupdata = pickupdata.trim("<br/>");
                            return pickupdata;
                        }

                    }
                    else {

                        return 'NA'
                    }

                },
            },
            {
                "targets": 3,
                //  "orderable": false,
                "width": "8%",
                "render": function (data, type, row, meta) {
                    var pickupDate = "";
                    if (row.PickupDate != null && row.PickupDate != '') {

                        var pickupDateList = row.PickupDate.split('|');
                        if (pickupDateList.length > 0) {
                            for (var i = 0; i < pickupDateList.length; i++) {

                                pickupDate += '<label>' + ConvertSqlDateTimeNew(pickupDateList[i], true) + '</label><br/>'
                            }

                        }
                    }
                    return pickupDate;
                }
            },


            {
                "targets": 4,
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    if (row.DeliveryLocation != null && row.DeliveryLocation != '') {
                        var deliveryLocaton = row.DeliveryLocation.split("$");
                        var deliveryData = "";
                        if (deliveryLocaton.length > 0) {
                            for (var i = 0; i < deliveryLocaton.length; i++) {
                                deliveryData += '<label data-toggle="tooltip" data-placement="top" title="' + GetCAddress(deliveryLocaton[i]) + '">' + GetCompany(deliveryLocaton[i]) + '</label><br/>'
                            }
                            deliveryData = deliveryData.trim("<br/>");
                            return deliveryData;
                        }

                    } else {
                        return 'NA'

                    }
                }
            },

            {
                "targets": 5,
                //  "orderable": false,
                "width": "8%",
                "render": function (data, type, row, meta) {
                    var deliveryDate = "";
                    if (row.DeliveryDate != null && row.DeliveryDate != '') {



                        var deliveryDateList = row.DeliveryDate.split('|');
                        if (deliveryDateList.length > 0) {
                            for (var i = 0; i < deliveryDateList.length; i++) {
                                deliveryDate += '<label>' + ConvertSqlDateTimeNew(deliveryDateList[i], true) + '</label><br/>'
                            }
                        }
                    }

                    return deliveryDate;
                }
            },

            /*Dart changes for AirwayBill*/
            {
                "targets": 6,
                // "orderable": false,
                "render": function (data, type, row, meta) {
                    //console.log("row.AirWayBills: " + row.AirWayBills);
                    if (row.AirWayBill != null && row.AirWayBill != '' && row.AirWayBill != undefined) {
                        //console.log("row.AirWayBills:  " + row.AirWayBill);
                        var quantity = row.AirWayBill.replaceAll('|', '<br/>');
                        return '<label>' + quantity.replace(/,\s*$/, ""); + '</label>';
                    }

                    else {
                        return '<label>NA</label>';
                    }


                }
            },

            {
                "targets": 7,
                //    "orderable": false,
                "render": function (data, type, row, meta) {
                    if (row.Quantity != null && row.Quantity != '' && row.Quantity != undefined) {
                        var quantity = row.Quantity.replaceAll('|', '<br/>');
                        return '<label>' + quantity.replace(/,\s*$/, ""); + '</label>';
                    }
                    else {

                        return '<label>NA</label>';
                    }


                }
            },
            {
                "targets": 9,
                //  "orderable": false,
                "className": "text-center",
                "render": function (data, type, row, meta) {
                    if (row.IsReady) {
                        return '<input sy type="checkbox" checked="' + row.IsReady + '" onchange="ShipmenetIsReady(' + row.ShipmentId + ',this)" >';
                    }
                    else {

                        return '<input type="checkbox" onchange="ShipmenetIsReady(' + row.ShipmentId + ',this)" >';
                    }


                }
            },
            {
                // "data": "PickUpDriver",
                "targets": 10,
                "className": "comments",
                //   "orderable": false,
                "render": function (data, type, row, meta) {

                    if (row.Comments != "" && row.Comments != null) {
                        //console.log("row.Comments: ", row.Comments);
                        var comments = row.Comments.replaceAll('|', '<br/>');
                        var toolcomments = row.Comments;
                        return '<label class="two" data-toggle="tooltip" data-placement="top" title="' + toolcomments + '" style="max-width:250px;display:block;white-space: nowrap;overflow: hidden !important;text-overflow: ellipsis;">' + comments + '</label> <a href="javascript: void(0)" class="delete_icon" data-toggle="tooltip" title="Delete" onclick="javascript:DeleteComments(' + row.ShipmentId + ',this);" style="font-size: 13px;">' +
                            '<i class="fa fa-times"></i>';
                    }
                    else {
                        return '<label>NA</label>';
                    }



                }
            },
            {
                "targets": 11,
                "orderable": false,
                "render": function (data, type, row, meta) {



                    var btnEdit = '<a href="' + baseUrl + '/Shipment/Shipment/Index/' + row.ShipmentId + '" data-toggle="tooltip" title="Edit" class="edit_icon">' +
                        '<i class="far fa-edit"></i>' +
                        '</a>';
                    var btnDelete = ' | <a href="javascript: void(0)" class="delete_icon" data-toggle="tooltip" title="Delete" onclick="javascript:DeleteShipment(' + row.ShipmentId + ');" >' +
                        '<i class="far fa-trash-alt"></i>' +
                        '</a>';
                    var btnCopy = ' | <a href="javascript: void(0)" class="edit_icon" data-toggle="tooltip" title="Copy Shipment" onclick="javascript:CopyShipment(' + row.ShipmentId + ');" >' +
                        '<i class="far fa-clone"></i>' +
                        '</a>';
                    var btnPreview = '<a href="' + baseUrl + '/Shipment/Shipment/ViewShipmentNotification/' + row.ShipmentId + '" title="Preview Quote" target="_blank" id="btnPreview">' +
                        '<i class="far fa-eye"></i>' +
                        '</a> |';
                    btnEdit = (isUpdate == true) ? btnEdit : "";
                    btnDelete = (isDelete == true) ? btnDelete : "";
                    btnCopy = (isUpdate == true) ? btnCopy : "";
                    btnPreview = (isView == true) ? btnPreview : "";
                    return '<div class="action-ic"> ' + btnPreview + ' ' + btnEdit + ' ' + btnCopy + ' ' + btnDelete + '</div>'

                }
            },
            {
                "targets": 0,
                "visible": false,
            }
        ],
        initComplete: function () {
            // Task to perform after DataTable is fully loaded
            $("#tblShipmentDetails>tbody>tr:first").trigger('click'); 
            GetBarGraph();
            GetPipeBarGraph();
           // console.log('DataTable is fully loaded!');
            // Your code here...
        }
    });
    //document.getElementById("shipmentProgress").innerHTML = row.length;
    //console.log("Mainshipment row count :", data);
    //var oTable1 = $('#tblShipmentDetails').DataTable();

    //$("input[input='search']").keyup(function () {

    //    oTable1.search(this.value);
    //    oTable1.draw();
    //});

    var search_thread_tblShipmentDetails = null;
    $("#tblShipmentDetails_filter input")
        .unbind()
        .bind("input", function (e) {
			
            clearTimeout(search_thread_tblShipmentDetails);
            search_thread_tblShipmentDetails = setTimeout(function () {
                var dtable = $("#tblShipmentDetails").dataTable().api();
                console.log("dtable: ", dtable);
                var elem = $("#tblShipmentDetails_filter input");
			
				var replacedStr = $(elem).val().replace(/\//g, "-");
                console.log("elem value: ", replacedStr);
                return dtable.search(replacedStr).draw();
            }, 700);
        });
   // var search_thread_tblShipmentDetails1 = null;
    //search_thread_tblShipmentDetails1 = 
 
    //var table = $("#tblShipmentDetails").DataTable();
    //var currentOrder = table.order();
    //console.log("currentOrder: ", currentOrder);
 
   
    // Add a click event listener to the table headers
    //$('#tblShipmentDetails th').click(function () {
    //    console.log("currentOrder: ", currentOrder);
    //    // Reset the table ordering to its original state
    //    table.order(currentOrder).draw();
    //});

}

//var table = $('#tblShipmentDetails').DataTable();
//var rowCount = table.rows().count();
//console.log("MainPage shipment row count :", rowCount);   

//#endregion

function GetCompany(fullAddress) {

    var fullAddress = fullAddress;
    var lastIndex = fullAddress.lastIndexOf("||")
    var companyName = fullAddress.substring(0, lastIndex);
    var address = fullAddress.substring(lastIndex + 2);
    if (lastIndex > 0) {
        return companyName;
    }
    else {
        return fullAddress;
    }

}

function GetCAddress(fullAddress) {
    var fullAddress = fullAddress;
    var lastIndex = fullAddress.lastIndexOf("||")
    var companyName = fullAddress.substring(0, lastIndex);
    var address = fullAddress.substring(lastIndex + 2);
    if (lastIndex > 0) {
        return address;
    }
    else {
        return fullAddress;
    }

}

function ClearCopyShipment() {
    var $select = $('#ddlCustomerCopy').selectize();
    $select[0].selectize.destroy();
    var ddlCustomer = "<option selected='selected' value=" + 0 + ">SEARCH CUSTOMER</option>";
    $("#ddlCustomerCopy").empty();
    $("#ddlCustomerCopy").append(ddlCustomer);
    $(".ddlCustomerCopy").text("SEARCH CUSTOMER");


    $("#ddlPickupDate").val("");
    $("#ddlDeliveryDate").val("");
    $("#txtAirWayBill").val("");
    $("#hdnShipmentId").val(0);
}

function ValidateCopyShipment() {

    var isValid = true;
    var message = "";
    var customerId = $("#ddlCustomerCopy").val();
    if (customerId > 0) {
        isValid = true;
    }
    else {
        AlertPopup("Please select a customer.");

        isValid = false;
    }

    var pickupDate = $("#ddlPickupDate").val();
    var deliveryDate = $("#ddlDeliveryDate").val();

    var todayDate = new Date();
    var month = todayDate.getMonth() + 1;
    var day = todayDate.getDate() - 1;

    var yesterday = "";
    yesterday = (month < 10 ? '0' : '') + month + '-' +
        (day < 10 ? '0' : '') + day + '-' +
        todayDate.getFullYear();

    //yesterday = new Date(Date.parse(todayDate));

    // if (isValid && pickupDate != "" && pickupDate < yesterday) {
        // AlertPopup("Please review your Pickup Est. Arrival. It should be greater than, or equal to, yesterday's date.");
        // isValid = false;

    // }


    if (isValid && pickupDate != "" && deliveryDate != "") {

        console.log("pickupDate: " + new Date(pickupDate) + " : " + pickupDate);
        console.log("deliveryDate: " + new Date(deliveryDate) + " : " + deliveryDate);
        // if (pickupDate < deliveryDate) {
            // console.log("true");
            // isValid = true;
        // }
        // else {
            // //$("#dtArrivalDate").val("");
            // AlertPopup("Please review your Delivery Est. Arrival. It should be greater than your Pickup Est. Arrival.");
            // isValid = false;

        // }

    }

    return isValid;
}



function CopyShipment(ShipmentId) {
    $.confirm({
        title: 'Confirmation!',
        content: '<b>Are you sure you want to copy this shipment?</b> ',
        type: 'blue',
        typeAnimated: true,
        buttons: {
            Copy: {
                btnClass: 'btn-blue',
                action: function () {

                    // bindCustomerDropdownCopy();
                    ClearCopyShipment();
                    $.ajax({
                        url: baseUrl + 'Shipment/Shipment/GetCopyShipmentDetailById',
                        data: { "shipmentId": ShipmentId },
                        type: "Post",
                        // async: true,
                        //contentType: "application/json; charset=utf-8",
                        //dataType: "json",
                        success: function (data) {
                            if (data != null) {
                                //ClearCopyShipment();
                                //$('#shipmentModal').modal('toggle');

                                var ddlCustomer = "<option selected='selected' value=" + data.CustomerId + ">" + data.CustomerName + "</option>";
                                $("#ddlCustomerCopy").empty();
                                $("#ddlCustomerCopy").append(ddlCustomer);
                                $(".ddlCustomerCopy").text(data.CustomerName);

                                //$("#ddlPickupDate").val(ConvertDate(data.EstPickupArriaval, true));
                                //$("#ddlDeliveryDate").val(ConvertDate(data.EstDeliveryArrival, true));
                                $("#ddlPickupDate").val(ConvertDateEdit(data.EstPickupArriaval, true));
                                $("#ddlDeliveryDate").val(ConvertDateEdit(data.EstDeliveryArrival, true));
                                //("#txtAirWayBill").val(data.AWB);
                                $("#hdnShipmentId").val(data.ShipmentId);
                                console.log("DATE: " + data.EstPickupArriaval + " : " + ConvertDate(data.EstPickupArriaval, true) + " : " + data.EstDeliveryArrival + " : " + ConvertDate(data.EstDeliveryArrival));
                                bindCustomerDropdownCopy();
                            }
                            $("#shipmentModal").modal("show");
                            $('#shipmentModal').draggable();

                        },
                        error: function () {
                            alert();
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

$("#btnCancelShip").click(function () {
    //window.location.href = baseUrl + "/Shipment/Shipment/ViewShipmentList";
    $("#shipmentModal").modal("toggle");
});

$("#btnCopyShip").click(function () {

    if (ValidateCopyShipment()) {

        var values = {};
        values.CustomerId = $("#ddlCustomerCopy").val();
        values.EstPickupArriaval = $("#ddlPickupDate").val();
        values.EstDeliveryArrival = $("#ddlDeliveryDate").val();
        values.AWB = $("#txtAirWayBill").val();
        values.ShipmentId = $("#hdnShipmentId").val();

        $.ajax({
            url: baseUrl + 'Shipment/Shipment/SaveCopyShipmentDetail',
            data: JSON.stringify(values),
            type: "Post",
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    if (data == true) {
                        $.alert({
                            title: 'Success ',
                            content: '<b>Shipment successfully copied.</b>',
                            type: 'green',
                            typeAnimated: true,
                            buttons: {
                                Ok: {
                                    btnClass: 'btn-green',
                                    action: function () {
                                        window.location.href = baseUrl + "/Main/Index";
                                    }
                                },
                            }
                        });
                    }
                    else {
                        AlertPopup(data);
                    }
                    $('#shipmentModal').modal('toggle');
                } else {
                    AlertPopup(data);
                }
                $('#tblShipmentDetails').DataTable().clear().destroy();
                $('#tblShipmentDetails2').DataTable().clear().destroy();
                GetOrderTakenShipmentList();
                GetOtherStatusShipmentList();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("DATA: " + JSON.stringify(values));
                console.log("ERROR: " + textStatus + ": " + jqXHR.status + " " + errorThrown);
            }
        });


    }
})

//#region function for apply selectize on customer dropdown
var bindCustomerDropdownCopy = function () {

    var $select = $('#ddlCustomerCopy').selectize();
    $select[0].selectize.destroy();

    $('#ddlCustomerCopy').selectize({
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
                    ('<span class="name ddlCustomer" date-IsWaitingTimeRequired=' + item.IsWaitingTimeRequired + '>' + escape(item.text) + '</span>') +
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
        }
        //onChange: function () {
        //    var IsWaitingTimeRequired = $(".ddlCustomer").attr("date-IsWaitingTimeRequired");
        //    if (JSON.parse(IsWaitingTimeRequired) == true) {
        //        $('#chkDeliveryWaitingTime').prop('checked', true);
        //        $('#chkPickUpWaitingTime').prop('checked', true);
        //        $(".divWaitingTime").show();
        //    }
        //    else {
        //        $('#chkDeliveryWaitingTime').prop('checked', false);
        //        $('#chkPickUpWaitingTime').prop('checked', false);
        //        $(".divWaitingTime").hide();
        //    }
        //}
    });
}
//#endregion

function replaceBR(string) {

    var str = string.replace("<br/>", "");
    return str;
}
//#region function to remove .00 from quantity


function escapeRegExp(string) {
    return string.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
}

/* Define functin to find and replace specified term with replacement string */
function replaceAll(str, term, replacement) {
    return str.replace(new RegExp(escapeRegExp(term), 'g'), replacement);
}

//#endregion

$('#tblShipmentDetails2').on('dblclick', 'tbody tr', function () {
    var table = $('#tblShipmentDetails2').DataTable();
    var data_row = table.row($(this).closest('tr')).data();
    window.location.href = baseUrl + '/Shipment/Shipment/Index/' + data_row.ShipmentId;
});

//#region bind other status
function GetOtherStatusShipmentList() {

    var values = {};
    values.StartDate = $("#dtStartedDate").val();
    values.EndDate = $("#dtEndDate").val();
    values.CustomerId = $("#ddlCustomer").val();
    values.IsOrderTaken = false;
    values.FreightTypeId = $("#ddlFreightType").val();
    values.StatusId = $("#ddlStatus").val();
    $('#tblShipmentDetails2').DataTable({
        // "bInfo": false,
        serverSide: true,
        dom: 'Blfrtip',
        buttons: [
            {
                extend: 'print',
                //className:'btn btn-primary btn-sm',
                //orientation: 'landscape',
                //pageSize: 'LEGAL',
                text: '<img src="../../Assets/images/printer.png" style="height:18px;margin-right: 5px;width:16px;"> Print',
                title: "",
                messageBottom: datetime,
                exportOptions: {
                    columns: ':visible',
                    stripHtml: false,
                    columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10,11,12,13,14]
                },
                customize: function (win) {
                    
                    //$(win.document.body).find('table')
                    //.widths = ['8%', '8%', '8%', '8%', '8%', '8%', '12%', '8%', '8%', '8%', '8%', '8%'];
                    //win.content[0].table.widths = ['8%', '8%', '8%', '8%', '8%', '8%', '12%', '8%', '8%', '8%', '8%', '8%'];
                    var last = null;
                    var current = null;
                    var bod = [];

                    var css = '@page { size: landscape; }',
                        head = win.document.head || win.document.getElementsByTagName('head')[0],
                        style = win.document.createElement('style');

                    style.type = 'text/css';
                    style.media = 'print';

                    if (style.styleSheet) {
                        style.styleSheet.cssText = css;
                    }
                    else {
                        style.appendChild(win.document.createTextNode(css));
                    }
                  
            

                    head.appendChild(style);
                    $(win.document.body)
                        .css('font-size', '10pt')
                        .prepend(
                            "<table id='checkheader'><tr><td width='80%' ><b>SHIPMENTS IN PROGRESS</b></td><td width='20%'><div><img src='"+baseUrl+"/Images/Laraslogo.png' height='100px'/></div></td></tr></table>"
                        );
                },
                //customize: function (doc) {
                //    doc.styles['td:nth-child(7)'] = {
                //        'width': '100px',
                //        'max-width': '100px'
                //    }
                //}
                //messageTop: function () {


                //        return '<b style="color:red;">Hello How are you?</b>';

                //},
                //messageBottom: null
            },

        ],

        select: 'single',
        "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
        responsive: true,
        filter: true,
        OrderMulti:true,
        processing: true,
        serverSide: true,
        searching: true,
        bDestroy: true,
        stateSave: true,
        "language": {
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
        },
        "ajax": {
            "url": baseUrl + "/Shipment/Shipment/ViewShipmentProgress",
            "type": "POST",
            "data": values,
            //"async": false,
            "datatype": "json",
        },
        "columns": [
            { "data": "ShipmentId", "name": "ShipmentId", "autoWidth": true },
            { "data": "StatusName", "name": "StatusName", "autoWidth": true },
            { "data": "PickupDate", "name": "PickupDate", "autoWidth": true },
            { "data": "PickupLocation", "name": "PickupLocation", "autoWidth": true },
            { "data": "DeliveryDate", "name": "DeliveryDate", "autoWidth": true },
            { "data": "DeliveryLocation", "name": "DeliveryLocation", "autoWidth": true },
            { "data": "CustomerName", "name": "CustomerName", "autoWidth": true } ,                           
            { "data": "AirWayBill", "name": "AirWayBill", "autoWidth": true  },
            { "data": "Quantity", "name": "Quantity", "autoWidth": true },
            //{ "data": "CustomerPO", "name": "CustomerPO", "autoWidth": true },
           
            { "data": "Commodity", "name": "Commodity", "autoWidth": true },
            { "data": "Temperature", "name": "Temperature", "autoWidth": true },
            { "data": "Driver", "name": "Driver", "autoWidth": true },
            { "data": "Equipment", "name": "Equipment", "autoWidth": true },
            { "data": " WT ", "name": "WT", "autoWidth": true },
            { "data": " ST ", "name": "ST", "autoWidth": true },
            { "name": "Action", "width":"10%" },
        ],
        "order": [[0, "desc"]],
        columnDefs: [
            {
                "targets": 1,
              //  "orderable": false,
                "render": function (data, type, row, meta) {
                    return StatusCheckForShipment(row.StatusName)
                }
            },
            {
                "targets": 2,
              //  "orderable": false,
                "width": "8%",
                "render": function (data, type, row, meta) {
                    var pickupDate = "";
                    if (row.PickupDate != null && row.PickupDate != '') {


                        var pickupDateList = row.PickupDate.split('|');
                        if (pickupDateList.length > 0) {
                            for (var i = 0; i < pickupDateList.length; i++) {

                                pickupDate += '<label>' + ConvertSqlDateTimeNew(pickupDateList[i], true) + '</label><br/>'
                            }

                        }
                    }
                    return pickupDate;
                }
            },
            {
                "targets": 3,
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    if (row.PickupLocation != null && row.PickupLocation != '') {
                        var pickupList = row.PickupLocation.split('$');

                        var pickupdata = "";
                        if (pickupList.length > 0) {
                            for (var i = 0; i < pickupList.length; i++) {
                                pickupdata += '<label data-toggle="tooltip" data-placement="top" title="' + GetCAddress(pickupList[i]) + '">' + GetCompany(pickupList[i]) + '</label><br/>'
                            }
                            pickupdata = pickupdata.trim("<br/>");
                            return pickupdata;
                        }

                    }
                    else {

                        return 'NA'
                    }


                },
            },
            {
                "targets": 4,
              //  "orderable": false,
                "width": "8%",
                "render": function (data, type, row, meta) {
                    var deliveryDate = "";
                    if (row.DeliveryDate != null && row.DeliveryDate != '') {
                        var deliveryDateList = row.DeliveryDate.split('|');
                        if (deliveryDateList.length > 0) {
                            for (var i = 0; i < deliveryDateList.length; i++) {
                                deliveryDate += '<label>' + ConvertSqlDateTimeNew(deliveryDateList[i], true) + '</label><br/>'
                            }
                        }
                    }
                    return deliveryDate;
                }
            },
            {
                "targets": 5,
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    if (row.DeliveryLocation != null && row.DeliveryLocation != '') {
                        var deliveryLocaton = row.DeliveryLocation.split("$");
                        var deliveryData = "";
                        if (deliveryLocaton.length > 0) {
                            for (var i = 0; i < deliveryLocaton.length; i++) {
                                deliveryData += '<label data-toggle="tooltip" data-placement="top" title="' + GetCAddress(deliveryLocaton[i]) + '">' + GetCompany(deliveryLocaton[i]) + '</label><br/>'
                            }
                            deliveryData = deliveryData.trim("<br/>");
                            return deliveryData;
                        }

                    } else {
                        return 'NA'

                    }
                }
            },
          

            {
                "targets": 8,
               // "orderable": false,
                "render": function (data, type, row, meta) {
                    if (row.Quantity != null && row.Quantity != '' && row.Quantity != undefined) {
                        var quantity = row.Quantity.replaceAll('|', '<br/>');
                        return '<label>' + quantity.replace(/,\s*$/, ""); + '</label>';
                    }
                    else {

                        return '<label>NA</label>';
                    }

                }
            },
            {
                "targets": 9,
               // "orderable": false,
                "render": function (data, type, row, meta) {
                    return SameDatas(row.Commodity);

                }
            },
            {
                "targets": 10,
              //  "orderable": false,
                "render": function (data, type, row, meta) {
                    return SameDatas(row.Temperature);

                }
            },
            {
                "targets": 13,
              //  "orderable": false,
                "className": "text-center",
                "render": function (data, type, row, meta) {
                  //  console.log("row.WTReadyt: ",row.WTReady);
                  //  console.log("row : ",row);
                    if (row.WTReady) {
                        return '<input sy type="checkbox" checked="' + row.WTReady + '" onchange="ShipmenetWTReady(' + row.ShipmentId + ',this)" >';
                    }
                    else {

                        return '<input type="checkbox" onchange="ShipmenetWTReady(' + row.ShipmentId + ',this)" >';
                    }


                }
            },
            {
                "targets": 14,
              //  "orderable": false,
                "className": "text-center",
                "render": function (data, type, row, meta) {
                    if (row.STReady) {
                        return '<input sy type="checkbox" checked="' + row.STReady + '" onchange="ShipmenetSTReady(' + row.ShipmentId + ',this)" >';
                    }
                    else {

                        return '<input type="checkbox" onchange="ShipmenetSTReady(' + row.ShipmentId + ',this)" >';
                    }


                }
            },
            {
                "targets": 15,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var btnEdit = '<a href="' + baseUrl + '/Shipment/Shipment/Index/' + row.ShipmentId + '" data-toggle="tooltip" title="Edit" class="edit_icon">' +
                        '<i class="far fa-edit"></i>' +
                        '</a>';
                    var btnMap = '| <a href="javascript: void(0)" class="Map_icon" data-toggle="tooltip" id="redirectButton" title="Map" onclick="javascript:fn_RedirectToGpsTrackers(' + row.ShipmentId + ');" >' +
                        '<i class="fas fa-map-marked-alt"></i>' +
                        '</a>';
                    var btnDelete = ' | <a href="javascript: void(0)" class="delete_icon" data-toggle="tooltip" title="Delete" onclick="javascript:DeleteShipment(' + row.ShipmentId + ');" >' +
                        '<i class="far fa-trash-alt"></i>' +
                        '</a>';
                    var btnCopy = ' | <a href="javascript: void(0)" class="edit_icon" data-toggle="tooltip" title="Copy Shipment" onclick="javascript:CopyShipment(' + row.ShipmentId + ');" >' +
                        '<i class="far fa-clone"></i>' +
                        '</a>';
                    var btnPreview = '<a href="' + baseUrl + '/Shipment/Shipment/ViewShipmentNotification/' + row.ShipmentId + '" title="Shipment Preview" target="_blank" id="btnPreview">' +
                        '<i class="far fa-eye"></i>' +
                        '</a> |';
                    var needApprovment = '<a href="' + baseUrl + '/Shipment/Shipment/Index/' + row.ShipmentId + '" title="Notification" style="color:red;" class="delete_icon" target="_blank" id="btnPreview">' +
                        '<i class="fa fa-bell" aria-hidden="true"></i>' +
                        '</a> |';
                    var noNeedApprovment = '<a href="' + baseUrl + '/Shipment/Shipment/Index/' + row.ShipmentId + '" title="Notification"  target="_blank" id="btnPreview">' +
                        '<i class="fa fa-bell" aria-hidden="true"></i>' +
                        '</a> |';
                    btnEdit = (isUpdate == true) ? btnEdit : "";
                    btnDelete = (isDelete == true) ? btnDelete : "";
                    btnCopy = (isUpdate == true) ? btnCopy : "";
                    btnPreview = (isView == true) ? btnPreview : "";
                    var displayBell = row.ApproveStatus == 1 ? noNeedApprovment : needApprovment;
                    return '<div class="action-ic">' + btnPreview + ' ' + displayBell +' '+ btnEdit + ' ' + btnCopy + ' ' + btnDelete + ' ' + btnMap + '</div>'
                }
            },
            {
                "targets": 0,
                "visible": false,
            }
        ]
    });

    var search_thread_tblShipmentDetails2 = null;
    $("#tblShipmentDetails2_filter input")
        .unbind()
        .bind("input", function (e) {
		
            clearTimeout(search_thread_tblShipmentDetails2);
            search_thread_tblShipmentDetails2 = setTimeout(function () {
                var dtable = $("#tblShipmentDetails2").dataTable().api();
                var elem = $("#tblShipmentDetails2_filter input");
			
				var replacedStr = $(elem).val().replace(/\//g, "-");
                console.log("elem value: ", replacedStr);
                return dtable.search(replacedStr).draw();
            }, 700);
        });

}
//#region bind other status


//#region same data
function SameDatas(fieldData) {
    var isSame = false;
    if (fieldData != null && fieldData != '') {
        var fieldList = fieldData.split('$');
        if (fieldList.length > 0) {
            var count = 0;
            for (var i = 0; i < fieldList.length; i++) {
                if (fieldList[i] == fieldList[0]) {
                    count = count + 1;
                }
            }
            if (count == fieldList.length) {
                isSame = true;
            }
            if (isSame) {
                return '<label>' +fieldList[0]+ '</label>'
            }
            else {
                var fields = "";
                for (var i = 0; i < fieldList.length; i++) {
                    fields += '<label>' + fieldList[i] + '</label><br/>'
                }
                return fields;
            }
        }
    }
    else {
        return '<label></label>'
    }

}
//#endregion

//#region Bind shipment
function GetOtherStatusShipmentList1() {

    var values = {};
    values.StartDate = $("#dtStartedDate2").val();
    values.EndDate = $("#dtEndDate2").val();
    values.CustomerId = $("#ddlCustomer2").val();
    values.IsOrderTaken = false;
    values.FreightTypeId = $("#ddlFreightType2").val();
    $('#tblShipmentDetails2').DataTable({
        // "bInfo": false,
        select: 'single',
        filter: true,
        responsive: true,
        processing: true,
        serverSide: true,
        searching: true,
        bDestroy: true,
        //stateSave: true,
        "language": {
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
        },
        "ajax": {
            "url": baseUrl + "/Shipment/Shipment/ViewShipment",
            "type": "POST",
            "data": values,
            //"async": false,
            "datatype": "json",
        },
        "columns": [
            { "data": "ShipmentId", "name": "ShipmentId", "autoWidth": true },
            { "data": "Status", "name": "Status", "autoWidth": true },
            { "data": "CustomerName", "name": "CustomerName", "autoWidth": true },
            { "data": "PickUpLocation", "name": "PickUpLocation", "autoWidth": true },
            { "data": "", "name": "", "autoWidth": true },
            { "data": "DeliveryLocation", "name": "DeliveryLocation", "autoWidth": true },
            { "data": "", "name": "", "autoWidth": true },
            { "data": "AirWayBillNo", "name": "AirWayBillNo", "width": "12%" },
            { "data": "CustomerPO", "name": "CustomerPO", "autoWidth": true },
            { "data": "DriverName", "name": "DriverName", "autoWidth": true },
            { "data": "EquipmentNo", "name": "EquipmentNo", "autoWidth": true },
            { "data": "QutVolWgt", "name": "QutVolWgt", "autoWidth": true },
            { "name": "Action", "autoWidth": true },
        ],
        "order": [[0, "desc"]],
        columnDefs: [
            {
                "targets": 1,
                "orderable": true,
                "render": function (data, type, row, meta) {
                    return StatusCheckForShipment(row.Status)
                }
            },
            {
                "targets": 2,
                "orderable": true,

            },
            {
                "targets": 4,
                "orderable": false,
                "width": "10%",
                "render": function (data, type, row, meta) {

                    var pickupDate = "";
                    if (row.PickUpDateList.length > 0) {
                        for (var i = 0; i < row.PickUpDateList.length; i++) {
                            pickupDate += '<label>' + ConvertDate(row.PickUpDateList[i], true) + '</label><br/>'
                        }
                    }

                    return pickupDate;
                }
            },
            {
                "targets": 6,
                "orderable": false,
                "width": "10%",
                "render": function (data, type, row, meta) {
                    var deliveryDate = "";
                    if (row.DeliveryDateList.length > 0) {
                        for (var i = 0; i < row.DeliveryDateList.length; i++) {
                            deliveryDate += '<label>' + ConvertDate(row.DeliveryDateList[i], true) + '</label><br/>'
                        }
                    }

                    return deliveryDate;
                }
            },

            {
                "targets": 5,
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    if (row.DeliveryLocation != null && row.DeliveryLocation != '') {
                        var deliveryLocaton = row.DeliveryLocation.split("|");
                        var deliveryData = "";
                        if (deliveryLocaton.length > 0) {
                            for (var i = 0; i < deliveryLocaton.length; i++) {
                                deliveryData += '<label data-toggle="tooltip" data-placement="top" title="' + GetAddress(deliveryLocaton[i]) + '">' + GetCompanyName(deliveryLocaton[i]) + '</label><br/>'
                            }
                            deliveryData = deliveryData.trim("<br/>");
                            return deliveryData;
                        }

                        //return '<label data-toggle="tooltip" data-placement="top" title="' + GetAddress(row.DeliveryLocation) + '">' + GetCompanyName(row.DeliveryLocation) + '</label>'
                    } else {
                        return 'NA'

                    }
                }
            },

            {
                "targets": 3,
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    if (row.PickUpLocation != null && row.PickUpLocation != '') {
                        var pickuplocaton = row.PickUpLocation.split("|");
                        var pickupdata = "";
                        if (pickuplocaton.length > 0) {
                            for (var i = 0; i < pickuplocaton.length; i++) {
                                pickupdata += '<label data-toggle="tooltip" data-placement="top" title="' + GetAddress(pickuplocaton[i]) + '">' + GetCompanyName(pickuplocaton[i]) + '</label><br/>'
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
                "targets": 7,
                "orderable": true,

            },
            {
                "targets": 8,
                "orderable": true,

            },

            {
                "targets": 9,
                "autoWidth": true,
                "orderable": true,
                "render": function (data, type, row, meta) {

                    if (row.DriverName != null && row.DriverName != '') {

                        return '<label data-toggle="tooltip" data-placement="top" title="' + row.DriverName + '">' + SplitString(row.DriverName, 15, true) + '</label>'
                    } else {
                        return 'NA'

                    }
                }
            },
            {
                "targets": 10,
                "autoWidth": true,
                "orderable": true,
                "render": function (data, type, row, meta) {

                    if (row.EquipmentNo != null && row.EquipmentNo != '') {


                        return '<label href="javascript: void(0)" data-toggle="tooltip" data-placement="top" title="' + row.EquipmentNo + '">' + SplitString(row.EquipmentNo, 10, true) + '</label>'
                    } else {
                        return 'NA'

                    }
                }
            },
            {
                "targets": 11,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var Qty = "";
                    if (row.Quantity > 0) {
                        if (row.PartialPallete > 0) {
                            Qty = row.Quantity + "/" + row.PartialPallete + " Pallets, "
                        }
                        else {
                            Qty = row.Quantity + " Pallets, "
                        }

                    }
                    if (row.NoOfBox > 0) {
                        if (row.PartilalBox > 0) {
                            Qty += row.NoOfBox + "/" + row.PartilalBox + " Boxes, ";
                        }
                        else {
                            Qty += row.NoOfBox + " Boxes, ";
                        }

                    }

                    if (row.Weights != "" || row.Weights != "") {
                        Qty += row.Weights + ", ";
                    }

                    if (row.TrailerCount > 0) {
                        Qty += row.TrailerCount + " Trailer";
                    }
                    Qty = Qty.replace(/(^\s*,)|(,\s*$)/g, '');
                    return '<label>' + row.QutVolWgt + '</label>';

                }
            },
            {
                "targets": 12,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var btnEdit = '<a href="' + baseUrl + '/Shipment/Shipment/Index/' + row.ShipmentId + '" data-toggle="tooltip" title="Edit" class="edit_icon">' +
                        '<i class="far fa-edit"></i>' +
                        '</a>';
                    var btnMap = '| <a href="javascript: void(0)" class="Map_icon" data-toggle="tooltip" id="redirectButton" title="Map" onclick="javascript:fn_RedirectToGpsTrackers(' + row.ShipmentId + ');" >' +
                        '<i class="fas fa-map-marked-alt"></i>' +
                        '</a>';
                    var btnDelete = ' | <a href="javascript: void(0)" class="delete_icon" data-toggle="tooltip" title="Delete" onclick="javascript:DeleteShipment(' + row.ShipmentId + ');" >' +
                        '<i class="far fa-trash-alt"></i>' +
                        '</a>';
                    var btnCopy = ' | <a href="javascript: void(0)" class="edit_icon" data-toggle="tooltip" title="Copy Shipment" onclick="javascript:CopyShipment(' + row.ShipmentId + ');" >' +
                        '<i class="far fa-clone"></i>' +
                        '</a>';
                    var btnPreview = '<a href="' + baseUrl + '/Shipment/Shipment/ViewShipmentNotification/' + row.ShipmentId + '" title="Preview Quote" target="_blank" id="btnPreview">' +
                        '<i class="far fa-eye"></i>' +
                        '</a> |';
                    btnEdit = (isUpdate == true) ? btnEdit : "";
                    btnDelete = (isDelete == true) ? btnDelete : "";
                    btnCopy = (isUpdate == true) ? btnCopy : "";
                    btnPreview = (isView == true) ? btnPreview : "";
                    return '<div class="action-ic">' + btnPreview + ' ' + btnEdit + ' ' + btnCopy + ' ' + btnDelete + ' ' + btnMap + '</div>'
                }
            },
            {
                "targets": 0,
                "visible": false,
            }
        ]
    });

    //var oTable2 = $('#tblShipmentDetails2').DataTable();

    //$("input[input='search']").keyup(function () {

    //    oTable2.search(this.value);
    //    oTable2.draw();
    //});
    var search_thread_tblShipmentDetails2 = null;
    $("#tblShipmentDetails2_filter input")
        .unbind()
        .bind("input", function (e) {
            clearTimeout(search_thread_tblShipmentDetails2);
            search_thread_tblShipmentDetails2 = setTimeout(function () {
                var dtable = $("#tblShipmentDetails2").dataTable().api();
                var elem = $("#tblShipmentDetails2_filter input");
                return dtable.search($(elem).val()).draw();
            }, 700);
        });


}
//#endregion

//#region Redirect to Gps Tracker
var fn_RedirectToGpsTrackers = function (ShipmentId) {
    window.open(baseUrl + '/GpsTracker/GpsTracker/Index/' + ShipmentId + ' ');
}
//#endregion

function SplitString(text, count, insertDots) {
    return text.slice(0, count) + (((text.length > count) && insertDots) ? "..." : "");
}

//#region Detail  Data Delete
function DeleteShipment(listId) {

    $.confirm({
        title: 'Confirmation!',
        content: '<b>Are you sure you want to delete?</b> ',
        type: 'red',
        typeAnimated: true,
        buttons: {
            delete: {
                btnClass: 'btn-blue',
                action: function () {
                    $.ajax({
                        url: baseUrl + '/Shipment/Shipment/DeleteShipment',
                        data: { 'shipmentId': listId },
                        type: "GET",
                        success: function (data) {

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
                                       
                                                $('#tblShipmentDetails').DataTable().clear().destroy();
                                                $('#tblShipmentDetails2').DataTable().clear().destroy();
                                                GetOrderTakenShipmentList();
                                                GetOtherStatusShipmentList();
                                            }
                                        },
                                    }
                                });

                            }
                            else {
                                AlertPopup(data.Message)
                            }

                        }
                    });
                }
            },
            cancel: {

                // btnClass: 'btn-red',
            }
        }
    })

}
//#endregion


//#region -- Delete Comments

function DeleteComments(listId,news) {

    //var selectedRows = $('#tblShipmentDetails').DataTable().rows('.selected').data();

    var table = $('#tblShipmentDetails').DataTable();
  
    // Clear the text content of the cell
    //cell.text('');
    
    //cell.data('');
    // Get the selected cell(s)
  
   
    

    $.confirm({
        title: 'Confirmation!',
        content: '<b>Are you sure you want to delete the comment?</b> ',
        type: 'red',
        typeAnimated: true,
        buttons: {
            delete: {
                btnClass: 'btn-blue',
                action: function () {

                    var $parent = $(this).parent();
                    var col1 = $(news).closest('tr').find('.two').html("NA");
                    var col2 = $(news).closest('tr').find('.delete_icon').hide();
                    console.log("Selected row: ", col1);

                    $.ajax({
                        url: baseUrl + '/Shipment/Shipment/DeleteComments',
                        data: { 'shipmentId': listId },
                        type: "GET",
                        success: function (data) {

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

                                               // $('#tblShipmentDetails').DataTable().clear().destroy();
                                               // $('#tblShipmentDetails2').DataTable().clear().destroy();
                                                //GetOrderTakenShipmentList();
                                               // GetOtherStatusShipmentList();
                                            }
                                        },
                                    }
                                });

                            }
                            else {
                                AlertPopup(data.Message)
                            }

                        }
                    });
                }
            },
            cancel: {

                // btnClass: 'btn-red',
            }
        }
    })
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
                return '<div>' +
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
//#endregion

//#region function for apply selectize on customer dropdown

var bindCustomerDropdown2 = function () {
    $('#ddlCustomer2').selectize({
        createOnBlur: false,
        maxItems: 1,
        valueField: 'id',
        labelField: 'text',
        searchField: 'text',
        plugins: ['restore_on_backspace'],
        load: function (query, callback) {
            if (!query.length) return callback();
            $.ajax({
                url: baseUrl + "Customer/GetAllCustomer/?searchText=" + query,
                type: 'GET',
                dataType: 'json',
                async: false,
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
                return '<div>' +
                    '<span style="display: block;">' + escape(label) + '</span>' +
                    '</div>';
            }
        },
        create: function (input, callback) {
            $('#ddlCustomer2').html("");
            $('#ddlCustomer2').append($("<option selected='selected'></option>").val(input).html(input))
        },
        onFocus: function () {

            var value = this.getValue();
            this.clear(true);
            this.unlock();
        }
    });
}
//#endregion

btnViewShipment = function () {
    $("#btnViewShipment").on("click", function () {
        GetOrderTakenShipmentList();
    })
}

btnViewShipment2 = function () {
    
    $("#btnViewShipment2").on("click", function () {   
        GetOtherStatusShipmentList();
    })
}

//#region DATE
var startEndDate = function () {

    var options = {
        format: 'm-d-Y',
        formatTime: 'H:i',
        formatDate: 'm-d-Y',
        startDate: new Date(),
        minDate: false,
        minTime: true,
        roundTime: 'round',// ceil, floor, round
        step: 30,
        timepicker: false
    }

    jQuery('.jqueryui-marker-datepicker').datetimepicker(options);
}
//#endregion

//#region bind freight type dropdownlist
function GetFreightType() {
    $.ajax({
        url: baseUrl + 'Shipment/Shipment/BindFreightType',
        data: {},
        type: "Post",
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var ddlValue = "";
            $("#ddlFreightType").empty();
            $("#ddlFreightType2").empty();
            ddlValue += '<option value="0">SELECT FREIGHT TYPE</option>'
            for (var i = 0; i < data.length; i++) {
                ddlValue += '<option value="' + data[i].FreightTypeId + '">' + data[i].FreightTypeName + '</option>';
            }
            $("#ddlFreightType").append(ddlValue);
            $("#ddlFreightType2").append(ddlValue);
        }
    });

}
//#endregion

//#region Shipmenet Is Ready
function ShipmenetIsReady(shipmentId, event) {
    
    var isReady = $(event).is(":checked");
    var model = {};
    model.shipmentId = JSON.parse(shipmentId);
    model.ready = JSON.parse(isReady);    
    $.ajax({
        url: baseUrl + 'Shipment/Shipment/ShipmentIsReady',
        type: "POST",
        data: JSON.stringify(model),        
        async: false,       
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data && isReady) {
                SuccessPopup("Shipment is now ready state.")
            }
            else {
                SuccessPopup("Shipment is now not ready state.")
            }
        }
    });

}

function ShipmenetWTReady(shipmentId, event) {

    var isReady = $(event).is(":checked");
    var model = {};
    model.shipmentId = JSON.parse(shipmentId);
    model.ready = JSON.parse(isReady);
    $.ajax({
        url: baseUrl + 'Shipment/Shipment/ShipmentWTReady',
        type: "POST",
        data: JSON.stringify(model),
        async: false,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data && isReady) {
               // SuccessPopup("Extended Waiting Time advised to Customer!")
                $.alert({
                    title: 'Success!',
                    content: "<b>Extended Waiting Time advised to Customer!</b>",
                    type: 'green',
                    typeAnimated: true,
                    buttons: {
                        Ok: {
                            btnClass: 'btn-green',
                            action: function () {
                                window.location.reload();
                            }
                        },
                    }
                });
            }
            else {
                //SuccessPopup("Extended Waiting Time not advised to Customer!")
                $.alert({
                    title: 'Success!',
                    content: "<b>Extended Waiting Time not advised to Customer!</b>",
                    type: 'green',
                    typeAnimated: true,
                    buttons: {
                        Ok: {
                            btnClass: 'btn-green',
                            action: function () {
                                window.location.reload();
                            }
                        },
                    }
                });
            }
        },
        error: function () {
            //hideLoader();
            AlertPopup("Something went wrong.");
        }
    });

}

function ShipmenetSTReady(shipmentId, event) {

    var isReady = $(event).is(":checked");
    var model = {};
    model.shipmentId = JSON.parse(shipmentId);
    model.ready = JSON.parse(isReady);
    $.ajax({
        url: baseUrl + 'Shipment/Shipment/ShipmentSTReady',
        type: "POST",
        data: JSON.stringify(model),
        async: false,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data && isReady) {
                //SuccessPopup("Shipment in Storage!")
                $.alert({
                    title: 'Success!',
                    content: "<b>Shipment in Storage!</b>",
                    type: 'green',
                    typeAnimated: true,
                    buttons: {
                        Ok: {
                            btnClass: 'btn-green',
                            action: function () {
                                window.location.reload();
                            }
                        },
                    }
                });
            }
            else {
                //SuccessPopup("Shipment is not in Storage!")
                $.alert({
                    title: 'Success!',
                    content: "<b>Shipment is not in Storage!</b>",
                    type: 'green',
                    typeAnimated: true,
                    buttons: {
                        Ok: {
                            btnClass: 'btn-green',
                            action: function () {
                                window.location.reload();
                            }
                        },
                    }
                });
            }
        }
    });

}
//#endregion
function GetBarGraph() {
   //var x = setInterval(function(){
        var currentCount = updateShipmentOrderTakenCount() + updateShipmentInProgressCount();
		//if(currentCount!=undefined){
		//clearInterval(x);
        //console.log("currentCount shipmentorder: ", currentCount);
            var progress = (updateShipmentOrderTakenCount() / currentCount) * 100;
        var mydiv = 100 - progress;
    //console.log(progress);
    if (updateShipmentOrderTakenCount() == currentCount) {
        $('#total').css('padding', '0');
    }
    else {
        $('#total').css('padding', '0 8px');
    }
         $('#myProgress').css('display', 'flex');
         $('.progressBar').css('display', 'flex');
        $("#myBar").width(progress + '%');
        $('#total').width(mydiv + '%');
        $('#total').css('display', 'flex');
        $('#total').css('float', 'right');
       
        $('#label').css('color', '#fff');
            $("#label").text(updateShipmentOrderTakenCount());
         $("#totalCount").text();
		// }
		// else{
			// console.log("value not found");
		// }
	// },100);
}

function GetPipeBarGraph() {
	//var x = setInterval(function(){
        var currentCount = updateShipmentOrderTakenCount() + updateShipmentInProgressCount();
	//if(currentCount!=undefined){
	//	clearInterval(x);
		//console.log("currentCount shipment progres: ", currentCount);
        var progress = (updateShipmentInProgressCount() / currentCount) * 100;
    var mydiv = 100 - progress;
    //console.log("progress shipemtn: ",progress);
    if (updateShipmentInProgressCount() > 0) {
        $("#myPipeLineBar").css('padding', '0 8px');
      
        $('#Pipelabel').css('display', 'flex');
        $('#Pipelabel').css('color', '#fff');
        $("#Pipelabel").text(updateShipmentInProgressCount());
    }
    else {
        $("#myPipeLineBar").css('padding', '0');
        $('#Pipelabel').css('display', 'none');
    }
    if (progress > 10) {
        $("#myPipeLineBar").css('width', progress + '%');
        $('#Pipelabel').css('overflow', 'inherit');
        $('#myShipProgress').css('overflow', 'inherit');
        $('#myPipeLineBar').css('overflow', 'inherit');
    }
    else {
       
        $("#myPipeLineBar").css('width', progress + '%');
        $('#Pipelabel').css('overflow', 'inherit');
        $('#myShipProgress').css('overflow', 'inherit');
        $('#myPipeLineBar').css('overflow', 'inherit');
    }
    $('#myShipProgress').css('display', 'flex');
    $('.progressBar').css('display', 'flex');
   // $("#myPipeLineBar").width(progress + '%');
    $('#totalPipe').width(mydiv + '%');
    $('#totalPipe').css('display', 'flex');
    $('#totalPipe').css('float', 'right');
    $('#totalPipe').css('padding', '0 10px');
   
    
    $("#ShipmenttotalCount").text();
	//}
   /*  else{
		console.log("value not found");
	}
	},100); */

}
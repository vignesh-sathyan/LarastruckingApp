$(document).ready(function () {
    // $(".divSearch").hide();
    binddate();
    BindDriver();
    GetOrderTakenShipmentList();
    bindCustomerDropdown();
    btnViewShipment();
    startEndDate();
    GetFreightType();
    shipmentStatus();

});
$("table").on("mouseover", 'tr', function () {

    $(this).find(".far").css('color', 'white');
    $(this).find(".fa-map-marked-alt").css('color', 'white');
});

$("table").on("mouseout", 'tr', function () {

    $(this).find(".far").css('color', '#007bff');
    $(this).find(".fa-download").css('color', '#007bff');
    $(this).find(".fa-map-marked-alt").css('color', '#007bff');
    $(this).find(".fa-trash-alt").css('color', 'red');

});
function BindDriver() {
    $.ajax({
        url: baseUrl + 'Shipment/Shipment/GetAllDriver',
        data: {},
        type: "GET",
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
            $("#ddlDriver").append(ddlValue);

        }
    });
}

$('#tblShipmentDetails').on('dblclick', 'tbody tr', function () {
    var table = $('#tblShipmentDetails').DataTable();
    var data_row = table.row($(this).closest('tr')).data();
    window.location.href = baseUrl + '/Shipment/Shipment/Index/' + data_row.ShipmentId;
});
//#region select date  in Quote Date and Valid Thru
var binddate = function () {

    //var day = last.getDate();
    //var month = last.getMonth() + 1;
    //var year = last.getFullYear();

    var days; // Days you want to subtract
    var date = new Date();
    var last = new Date(date.getTime() - (7 * 24 * 60 * 60 * 1000));


    var quotemonth = last.getMonth() + 1;

    var quotedate = last.getDate();

    var startDate = (quotemonth < 10 ? ("0" + quotemonth) : quotemonth) + "-" + (quotedate < 10 ? ("0" + quotedate) : quotedate) + "-" + last.getFullYear();
    var todaydate = new Date();

    var validthru = (todaydate);

    var validmonth = validthru.getMonth() + 1;

    var validdate = validthru.getDate();

    var endDate = (validmonth < 10 ? ("0" + validmonth) : validmonth) + "-" + (validdate < 10 ? ("0" + validdate) : validdate) + "-" + validthru.getFullYear();

    $("#dtStartedDate").val(startDate);
    $("#dtEndDate").val(endDate);
}
//#endregion

//#region Bind shipment
//function GetOrderTakenShipmentList() {

//    var values = {};
//    values.StartDate = $("#dtStartedDate").val();
//    values.EndDate = $("#dtEndDate").val();
//    values.CustomerId = $("#ddlCustomer").val();
//    values.IsOrderTaken = true;
//    values.FreightTypeId = $("#ddlFreightType").val();
//    values.StatusId = $("#ddlStatus").val();
//    $('#tblShipmentDetails').DataTable({
//       // "bInfo": false,
//        //dom: 'Blfrtip',
//        select: 'single',
//        "lengthMenu": [[10,25, 50, 100], [10,25, 50, 100]],
//        responsive: true,
//        filter: true,
//        processing: true,
//        serverSide: true,
//        searching: true,
//        bDestroy: true,
//        stateSave:true,
//        "language": {
//            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
//        },
//        "ajax": {
//            "url": baseUrl + "/Shipment/Shipment/ViewAllShipment",
//            "type": "POST",
//            "data": values,
//            //"async": false,
//            "datatype": "json",
//        },
//        "columns": [
//            { "data": "ShipmentId", "name": "ShipmentId", "autoWidth": true },
//            { "data": "Status", "name": "Status", "autoWidth": true },
//            { "data": "CustomerName", "name": "CustomerName", "autoWidth": true },
//            { "data": "PickUpLocation", "name": "PickUpLocation", "autoWidth": true },
//            { "data": "PickUpDate", "name": "PickUpDate", "autoWidth": true },
//            { "data": "DeliveryLocation", "name": "DeliveryLocation", "autoWidth": true },
//            { "data": "DeliveryDate", "name": "DeliveryDate", "autoWidth": true },
//            { "data": "AirWayBillNo", "name": "AirWayBillNo", "width": "12%" },
//            { "data": "CustomerPO", "name": "CustomerPO", "autoWidth": true },
//            { "data": "DriverName", "name": "DriverName", "autoWidth": true },
//            { "data": "EquipmentNo", "name": "EquipmentNo", "autoWidth": true },
//            { "data": "QutVolWgt", "name": "QutVolWgt", "autoWidth": true },
//            { "name": "Action", "autoWidth": true },
//        ],
//        "order": [[0, "desc"]],
//        columnDefs: [
//            {
//                "targets": 1,
//                "orderable": false,
//                "render": function (data, type, row, meta) {
//                    return StatusCheckForShipment(row.Status)
//                }
//            },
//            {
//                "targets": 4,
//                "orderable": false,
//                "width": "10%",
//                "render": function (data, type, row, meta) {
//                    
//                    var pickupDate = "";
//                    if (row.PickUpDateList.length > 0) {
//                        for (var i = 0; i < row.PickUpDateList.length; i++) {
//                            pickupDate += '<label>' + ConvertDate(row.PickUpDateList[i], true) + '</label><br/>'
//                        }
//                    }

//                    return pickupDate;
//                }
//            },
//            {
//                "targets": 6,
//                "orderable": false,
//                "width": "10%",
//                "render": function (data, type, row, meta) {
//                    var deliveryDate = "";
//                    if (row.DeliveryDateList.length > 0) {
//                        for (var i = 0; i < row.DeliveryDateList.length; i++) {
//                            deliveryDate += '<label>' + ConvertDate(row.DeliveryDateList[i], true) + '</label><br/>'
//                        }
//                    }

//                    return deliveryDate;
//                }
//            },

//            {
//                "targets": 5,
//                "autoWidth": true,
//                "render": function (data, type, row, meta) {

//                    if (row.DeliveryLocation != null && row.DeliveryLocation != '') {
//                        var deliveryLocaton = row.DeliveryLocation.split("|");
//                        var deliveryData = "";
//                        if (deliveryLocaton.length > 0) {
//                            for (var i = 0; i < deliveryLocaton.length; i++) {
//                                deliveryData += '<label data-toggle="tooltip" data-placement="top" title="' + GetAddress(deliveryLocaton[i]) + '">' + GetCompanyName(deliveryLocaton[i]) + '</label><br/>'
//                            }
//                            deliveryData = deliveryData.trim("<br/>");
//                            return deliveryData;
//                        }

//                        //return '<label data-toggle="tooltip" data-placement="top" title="' + GetAddress(row.DeliveryLocation) + '">' + GetCompanyName(row.DeliveryLocation) + '</label>'
//                    } else {
//                        return 'NA'

//                    }
//                }
//            },

//            {
//                "targets": 3,
//                "autoWidth": true,
//                "render": function (data, type, row, meta) {

//                    if (row.PickUpLocation != null && row.PickUpLocation != '') {
//                        var pickuplocaton = row.PickUpLocation.split("|");
//                        var pickupdata = "";
//                        if (pickuplocaton.length > 0) {
//                            for (var i = 0; i < pickuplocaton.length; i++) {
//                                pickupdata += '<label data-toggle="tooltip" data-placement="top" title="' + GetAddress(pickuplocaton[i]) + '">' + GetCompanyName(pickuplocaton[i]) + '</label><br/>'
//                            }
//                            pickupdata = pickupdata.trim("<br/>");
//                            return pickupdata;
//                        }
//                    } else {
//                        return 'NA'

//                    }
//                }
//            },


//            {
//                "targets": 9,
//                "autoWidth": true,
//                "render": function (data, type, row, meta) {

//                    if (row.DriverName != null && row.DriverName != '') {

//                        return '<label data-toggle="tooltip" data-placement="top" title="' + row.DriverName + '">' + SplitString(row.DriverName, 15, true) + '</label>'
//                    } else {
//                        return 'NA'

//                    }
//                }
//            },
//            {
//                "targets": 10,
//                "autoWidth": true,
//                "render": function (data, type, row, meta) {

//                    if (row.EquipmentNo != null && row.EquipmentNo != '') {


//                        return '<label href="javascript: void(0)" data-toggle="tooltip" data-placement="top" title="' + row.EquipmentNo + '">' + SplitString(row.EquipmentNo, 10, true) + '</label>'
//                    } else {
//                        return 'NA'

//                    }
//                }
//            },
//            {
//                "targets": 11,
//                "orderable": false,
//                "render": function (data, type, row, meta) {
//                    var Qty = "";
//                    if (row.Quantity > 0) {
//                        if (row.PartialPallete > 0) {
//                            Qty = row.Quantity + "/" + row.PartialPallete + " Pallets, "
//                        }
//                        else {
//                            Qty = row.Quantity + " Pallets, "
//                        }

//                    }
//                    if (row.NoOfBox > 0) {
//                        if (row.PartilalBox > 0) {
//                            Qty += row.NoOfBox + "/" + row.PartilalBox + " Boxes, ";
//                        }
//                        else {
//                            Qty += row.NoOfBox + " Boxes, ";
//                        }

//                    }

//                    if (row.Weights != "" || row.Weights != "") {
//                        Qty += row.Weights + ", ";
//                    }

//                    if (row.TrailerCount > 0) {
//                        Qty += row.TrailerCount + " Trailer";
//                    }
//                    Qty = Qty.replace(/(^\s*,)|(,\s*$)/g, '');
//                    return '<label>' + row.QutVolWgt + '</label>';

//                }
//            },
//            {
//                "targets": 12,
//                "orderable": false,
//                "render": function (data, type, row, meta) {
//                    var btnEdit = '<a href="' + baseUrl + '/Shipment/Shipment/Index/' + row.ShipmentId + '" data-toggle="tooltip" title="Edit" class="edit_icon">' +
//                        '<i class="far fa-edit"></i>' +
//                        '</a>';
//                    var btnMap = '| <a href="javascript: void(0)" class="Map_icon" data-toggle="tooltip" id="redirectButton" title="Map" onclick="javascript:fn_RedirectToGpsTracker(' + row.ShipmentId + ');" >' +
//                        '<i class="fas fa-map-marked-alt"></i>' +
//                        '</a>';
//                    var btnDelete = ' | <a href="javascript: void(0)" class="delete_icon" data-toggle="tooltip" title="Delete" onclick="javascript:DeleteShipment(' + row.ShipmentId + ');" >' +
//                        '<i class="far fa-trash-alt"></i>' +
//                        '</a>';

//                    btnEdit = (isUpdate == true) ? btnEdit : "";
//                    btnDelete = (isDelete == true) ? btnDelete : "";

//                    return '<div class="action-ic">' + btnEdit + ' ' + btnDelete + ' ' + btnMap + '</div>'
//                }
//            },
//            {
//                "targets": 0,
//                "visible": false,
//            }
//        ]
//    });

//    //oTable = $('#tblShipmentDetails').DataTable();

//    //$("input[type='search']").keyup(function () {

//    //    oTable.search(this.value);
//    //    oTable.draw();
//    //});

//    var search_thread_tblShipmentDetails = null;
//    $("#tblShipmentDetails_filter input")
//        .unbind()
//        .bind("input", function (e) {
//            clearTimeout(search_thread_tblShipmentDetails);
//            search_thread_tblShipmentDetails = setTimeout(function () {
//                var dtable = $("#tblShipmentDetails").dataTable().api();
//                var elem = $("#tblShipmentDetails_filter input");
//                return dtable.search($(elem).val()).draw();
//            }, 700);
//        });

//}
var fn_RedirectToGpsTracker = function (ShipmentId) {
    window.open(baseUrl + '/GpsTracker/GpsTracker/Index/' + ShipmentId + ' ');
}

var d = new Date();
var month = d.getMonth() + 1;
var day = d.getDate();

var datetime = (month < 10 ? '0' : '') + month + '/' +
    (day < 10 ? '0' : '') + day + '/' +
    d.getFullYear() + "  " +
    (d.getHours() < 10 ? '0' : '') + d.getHours() + ":" +
    (d.getMinutes() < 10 ? '0' : '') + d.getMinutes() + ":" +
    (d.getSeconds() < 10 ? '0' : '') + d.getSeconds();


$.fn.dataTable.ext.errMode = function (settings, helpPage, message) {
    console.log("table error: ",message);

   // window.location.reload();
};

//Go BACK... Added on 08-Feb-2023
$("html").unbind().keyup(function (e) {
    if (e.key === 'Backspace' || e.keyCode === 8) {
        //history.back();
        if (!$(e.target).is('input') && !$(e.target).is('textarea')) {
            history.back();
        }
    }
});
//

function GetOrderTakenShipmentList() {

    $('#tblShipmentDetails').DataTable().clear().destroy();
    var ddlCustomer = "";
    if ($("#ddlCustomer").val() > 0) {
        ddlCustomer = ("Customer: " + $("#ddlCustomer option:selected").text() + "&nbsp &nbsp;");
    }

    var freightType = "";
    if ($("#ddlFreightType").val() > 0) {
        freightType = ("Freight Type: " + $("#ddlFreightType option:selected").text() + "&nbsp &nbsp;");
    }

    var startDate = $("#dtStartedDate").val();
    var endDate = $("#dtEndDate").val();


    var values = {};
    values.StartDate = $("#dtStartedDate").val();
    values.EndDate = $("#dtEndDate").val();
    values.CustomerId = $("#ddlCustomer").val();
    //values.IsOrderTaken = true;
    values.FreightTypeId = $("#ddlFreightType").val();
    values.StatusId = $("#ddlStatus").val();
    if ($("#ddlDriver").val() > 0) {
        values.DriverName = $("#ddlDriver option:selected").text();
    }

    $('#tblShipmentDetails').DataTable({
        // "bInfo": false,
        serverSide: true,
        dom: 'Blfrtip',
        "paging": true,
        buttons: [
            {
                text: 'GO BACK',
                extend: 'print',
                action: function (e, dt, node, config) {
                    //alert('Button activated');
                    history.back();
                }
            },
            {
                extend: 'print',
                orientation: 'landscape',
                pageSize: 'LEGAL',
                title: "",
                text: '<img src="../../Assets/images/printer.png" style="height:18px;margin-right: 5px;position: relative;top: 1px;width:16px;"/> Print',
                messageBottom: datetime,

                exportOptions: {
                    columns: ':visible',
                    stripHtml: false,
                    columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13]
                },
                customize: function (win) {

                    var last = null;
                    var current = null;
                    var bod = [];
                    //var now = new Date();
                    //var jsDate = now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear();
                    //
                    //win['footer'] = (function (page, pages) {
                    //    
                    //    return {
                    //        columns: [
                    //            {
                    //                alignment: 'left',
                    //                text: ['Created on: ', { text: jsDate.toString() }]
                    //            },
                    //            {
                    //                alignment: 'right',
                    //                text: ['page ', { text: page.toString() }, ' of ', { text: pages.toString() }]
                    //            }
                    //        ],
                    //        margin: 20
                    //    }
                    //});

                    var css = '@page { size: landscape; }',
                        head = win.document.head || win.document.getElementsByTagName('head')[0],
                        style = win.document.createElement('style');
                    var sts = $("#ddlStatus option:selected").text();
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
                            "<table style='text-transform:capitalize' id='checkheader'><tr><td width='80%' style='text-transform:capitalize;font-size:13px;'><br/><br/><b>" + sts + " SHIPMENTS <b><br/><b> " + ddlCustomer + "<br/> Date Range: " + startDate + " to " + endDate + "<br/> " + freightType + "</b> </td><td width='20%'><div><img src='http://larastruckinglogistics-app.azurewebsites.net/Images/Laraslogo.png' height='100px'/></div></td></tr></table>"
                        );
                    //$(win.document.footer)

                    //    .css('font-size', '10pt')
                    //    .prepend(
                    //        "<table style='text-transform:capitalize' id='checkheader'><tr><td width='80%' style='text-transform:capitalize;font-size:13px;'><br/><br/><b> " + ddlCustomer + "<br/> Date Range: " + startDate + " to " + endDate + "<br/> " + freightType + "</b> </td><td width='20%'><div><img src='http://larastruckinglogistics-app.azurewebsites.net/Images/Laraslogo.png' height='100px'/></div></td></tr></table>"
                    //    );
                }
            },
        ],
        select: 'single',
        "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
        responsive: true,
        orderMulti:true,
        filter: true,
        processing: true,
        serverSide: true,
        searching: true,
        bDestroy: true,
        stateSave: true,
        "language": {
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
        },
        "ajax": {
            "url": baseUrl + "/Shipment/Shipment/AllShipmentList",
            "type": "POST",
            "data": values,
            //"async": false,
            "datatype": "json",
        },
        "columns": [
            { "data": "ShipmentId", "name": "ShipmentId", "autoWidth": true },
            { "data": "StatusName", "name": "StatusName", "autoWidth": true },
            { "data": "CustomerName", "name": "CustomerName", "autoWidth": true },
            { "data": "PickupLocation", "name": "PickupLocation", "autoWidth": true },
            { "data": "PickupDate", "name": "PickupDate", "autoWidth": true },
            { "data": "DeliveryLocation", "name": "DeliveryLocation", "autoWidth": true },
            { "data": "DeliveryDate", "name": "DeliveryDate", "autoWidth": true },
            { "data": "AirWayBill", "name": "AirWayBill", "width": "7%" },
            //{ "data": "CustomerPO", "name": "CustomerPO", "autoWidth": true },
            { "data": "Quantity", "name": "Quantity", "autoWidth": true },
            { "data": "Commodity", "name": "Commodity", "autoWidth": true },
            { "data": "Driver", "name": "Driver", "autoWidth": true },
            { "data": "Equipment", "name": "Equipment", "autoWidth": true },
            { "data": " WT ", "name": "WT", "autoWidth": true },
            { "data": " ST ", "name": "ST", "autoWidth": true },
            { "name": "Action", "width": "8%" },
        ],
        "order": [[0, "desc"]],
        columnDefs: [
            {
                "targets": 1,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    return StatusCheckForShipment(row.StatusName)
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
                    //if (row.PickupLocation != null && row.PickupLocation != '')
                    //{
                    //        var pickuplocaton = row.PickUpLocation.split("|");
                    //        var pickupdata = "";
                    //        if (pickuplocaton.length > 0) {
                    //            for (var i = 0; i < pickuplocaton.length; i++) {
                    //                pickupdata += '<label data-toggle="tooltip" data-placement="top" title="' + GetCAddress(pickuplocaton[i]) + '">' + GetCompany(pickuplocaton[i]) + '</label><br/>'
                    //            }
                    //            pickupdata = pickupdata.trim("<br/>");
                    //            return pickupdata;
                    //        }
                    //    } else {
                    //        return 'NA'

                    //    }
                    //}
                },
            },
            {
                "targets": 4,
                // "orderable": false,
                "width": "8%",
                "render": function (data, type, row, meta) {
                    var pickupDate = "";
                    if (row.PickupDate != '' && row.PickupDate != null) {


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
                "targets": 6,
                // "orderable": false,
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
            //{
            //    "targets": 9,
            //    "autoWidth": true,
            //    "render": function (data, type, row, meta) {
            //        if (row.DriverName != null && row.DriverName != '') {
            //            return '<label data-toggle="tooltip" data-placement="top" title="' + row.DriverName + '">' + SplitString(row.DriverName, 15, true) + '</label>'
            //        } else {
            //            return 'NA'
            //        }
            //    }
            //},
            //{
            //    "targets": 10,
            //    "autoWidth": true,
            //    "render": function (data, type, row, meta) {

            //        if (row.EquipmentNo != null && row.EquipmentNo != '') {
            //            return '<label href="javascript: void(0)" data-toggle="tooltip" data-placement="top" title="' + row.EquipmentNo + '">' + SplitString(row.EquipmentNo, 10, true) + '</label>'
            //        } else {
            //            return 'NA'

            //        }
            //    }
            //},
            {
                "targets": 8,
                // "orderable": false,
                "render": function (data, type, row, meta) {

                    if (row.Quantity != null || row.Quantity != '' && row.Quantity != undefined) {
                        var quantity = row.Quantity.replaceAll('|', '<br/>');
                        return '<label>' + quantity.replace(/,\s*$/, ""); + '</label>';
                    }
                    else {

                        return '<label>NA</label>';
                    }

                }
            },
            {
                "targets": 12,
                //  "orderable": false,
                "className": "text-center",
                "render": function (data, type, row, meta) {
                    console.log("row.WTReadyt: ", row.WTReady);
                    console.log("row : ", row);
                    if (row.WTReady) {
                        return '<input sy type="checkbox" checked="' + row.WTReady + '" onchange="ShipmenetWTReady(' + row.ShipmentId + ',this)" >';
                    }
                    else {

                        return '<input type="checkbox" onchange="ShipmenetWTReady(' + row.ShipmentId + ',this)" >';
                    }


                }
            },
            {
                "targets": 13,
                //"orderable": false,
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
                "targets": 14,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var btnEdit = '<a href="' + baseUrl + '/Shipment/Shipment/Index/' + row.ShipmentId + '" data-toggle="tooltip" title="Edit" class="edit_icon">' +
                        '<i class="far fa-edit"></i>' +
                        '</a>';
                    var btnMap = '| <a href="javascript: void(0)" class="Map_icon" data-toggle="tooltip" id="redirectButton" title="Map" onclick="javascript:fn_RedirectToGpsTracker(' + row.ShipmentId + ');" >' +
                        '<i class="fas fa-map-marked-alt"></i>' +
                        '</a>';
                    var btnDelete = ' | <a href="javascript: void(0)" class="delete_icon" data-toggle="tooltip" title="Delete" onclick="javascript:DeleteShipment(' + row.ShipmentId + ');" >' +
                        '<i class="far fa-trash-alt"></i>' +
                        '</a>';
                    var btnPreview = '<a href="' + baseUrl + '/Shipment/Shipment/ViewShipmentNotification/' + row.ShipmentId + '" title="Shipment Preview" target="_blank" id="btnPreview">' +
                        '<i class="far fa-eye"></i>' +
                        '</a> |';
                    btnEdit = (isUpdate == true) ? btnEdit : "";
                    btnDelete = (isDelete == true) ? btnDelete : "";
                    btnPreview = (isView == true) ? btnPreview : "";
                    return '<div class="action-ic">  ' + btnPreview + ' ' + btnEdit + ' ' + btnDelete + ' ' + btnMap + '</div>'
                }
            },
            {
                "targets": 0,
                "visible": false,
            }
        ]
    });

    //oTable = $('#tblShipmentDetails').DataTable();

    //$("input[type='search']").keyup(function () {

    //    oTable.search(this.value);
    //    oTable.draw();
    //});

    var search_thread_tblShipmentDetails = null;
    $("#tblShipmentDetails_filter input")
        .unbind()
        .bind("input", function (e) {
            clearTimeout(search_thread_tblShipmentDetails);
            search_thread_tblShipmentDetails = setTimeout(function () {
                var dtable = $("#tblShipmentDetails").dataTable().api();
                var elem = $("#tblShipmentDetails_filter input");
                return dtable.search($(elem).val()).draw();
            }, 70);
        });

}
//#endregion

function SplitString(text, count, insertDots) {
    return text.slice(0, count) + (((text.length > count) && insertDots) ? "..." : "");
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

function replaceBR(string) {

    var str = string.replace("<br/>", "");
    return str;
}


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




//#region Detail  Data Delete
function DeleteShipment(listId) {

    $.confirm({
        title: 'Confirmation!',
        content: '<b>Are you sure you want to Delete.</b> ',
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

                                                GetOrderTakenShipmentList();
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
        createOnBlur: true,
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


btnViewShipment = function () {
    $("#btnViewShipment").on("click", function () {
        GetOrderTakenShipmentList();
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
            ddlValue += '<option >SELECT STATUS</option>';
            data = data.filter(x => (x.StatusId == 8 || x.StatusId == 11));
            for (var i = 0; i < data.length; i++) {
                ddlValue += '<option value="' + data[i].StatusId + '">' + data[i].StatusName + '</option>';
            }
            $("#ddlStatus").append(ddlValue);

        }
    });
}
//#endregion

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

$("table").on("mouseover", 'tr', function () {

    $(this).find(".far").css('color', 'white');

});

$("table").on("mouseout", 'tr', function () {

    $(this).find(".far").css('color', '#007bff');
    $(this).find(".fa-trash-alt").css('color', 'red');

});


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
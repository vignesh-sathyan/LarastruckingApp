//#region Ready State
$(function () {
    CustomerDashboardDetail();
    // GetTrackingStatus();
});
//#endregion

//#region sorting functionality for current page

//// Initialize the Datatable
//var table = $('#tblCutomerDetails').DataTable({
//    bSort: false,
//    // Everytime the page is drawn reset the header setters
//    drawCallback: function () {
//        $("#tblCutomerDetails > thead > tr > th > div").each(function (index, elem) {
//            elem.setAttribute("class", "default");
//        });
//    }
//});

//// Modify column headers of the table to add sort arrows (▲▼)
//let i = 0;
//$("#tblCutomerDetails > thead > tr > th").each(function (index, elem) {
//    if (i < ($("#tblCutomerDetails > thead > tr > th").length) - 1) {
//        if (i < ($("#tblCutomerDetails > thead > tr > th").length) - 2) {
//            if (i != 4 && i != 5) {
//                let columnHeader = $(this).text();
//                elem.innerHTML =
//                    '<div name="' + i + '" onclick="sortClickHandler(this)" class="default"><a href="javascript:void(0)">' +
//                    columnHeader + ' <span id="asc">▲</span><span id="desc">▼</span></a></div>';
//            }
//        }
//    }
//    i++;
//});

//// Handle sort click events
//window.sortClickHandler = function (elem) {
//    
//    let targetColNumber = elem.getAttribute("name");
//    let currentSortOrder = elem.getAttribute("class");
//    let targetSortOrder = "desc";

//    if (currentSortOrder == 'default') {
//        elem.setAttribute("class", targetSortOrder);
//    } else {
//        targetSortOrder = currentSortOrder == "asc" ? "desc" : "asc";
//        // Change the sort arrow color (blue)
//        elem.setAttribute("class", targetSortOrder);
//    }
//    // Change other headers to default color (grey)
//    $("#tblCutomerDetails > thead > tr > th > div").each(function (index, elem) {
//        if (elem.getAttribute("name") != targetColNumber) {
//            elem.setAttribute("class", "default");
//        }
//    });
//    // Sort the rows and redraw the table
//    sortRowsAndRender(targetColNumber, targetSortOrder);
//}

//// This function is used to sort almost any kind of data (numbers, strings, dates etc)
//// Modify this function to add your own comparator
//window.comparator = function (property, order) {
//    
//    // alert('from comparator')
//    let sort_order = 1;
//    if (order === "desc") {
//        sort_order = -1;
//    }
//    return function (a, b) {
//        //alert('a[property] ==' + a[[$('#tblShipmentDetails').DataTable().settings().init().columns[property].name]] + '  b[property]  ' + b[[$('#tblShipmentDetails').DataTable().settings().init().columns[property].name]])
//        if (a[$('#tblCutomerDetails').DataTable().settings().init().columns[property].name] < b[$('#tblCutomerDetails').DataTable().settings().init().columns[property].name]) {
//            return -1 * sort_order;
//        } else if (a[$('#tblCutomerDetails').DataTable().settings().init().columns[property].name] > b[$('#tblCutomerDetails').DataTable().settings().init().columns[property].name]) {
//            return 1 * sort_order;
//        } else {
//            return 0 * sort_order;
//        }
//    }
//}

//window.sortRowsAndRender = function (targetColumn, targetSortOrder) {
//    
//    // Get the rows of the current page and sort them 
//    let targetRows = $('#tblCutomerDetails').DataTable().rows({
//        page: 'current'
//    }).data().sort(comparator(targetColumn, targetSortOrder));

//    // Get the current row index positions
//    let targetRowIds = $('#tblCutomerDetails').DataTable().rows({
//        page: 'current'
//    }).indexes();
//    for (i = 0; i < targetRowIds.length; i++) {
//        // Overwrite existing rows with sorted rows (this is will redraw the table automatically)
//        $('#tblCutomerDetails').DataTable().row(targetRowIds[i]).data(targetRows[i]);
//    }
//}
//#endregion
//#region colour change on grid icon
$(".tblChange-bgcolor").on("mouseover", 'tr', function () {
    $(this).find(".chng-color-View").css('color', 'white');
    $(this).find(".chng-color-Track").css('color', 'white');
});


$(".tblChange-bgcolor").on("mouseout", 'tr', function () {
    $(this).find(".chng-color-View").css('color', '#007bff');
    $(this).find(".chng-color-Track").css('color', '#007bff');

});
//#endregion
$('#tblCutomerDetails').on('dblclick', 'tbody tr', function () {
   
    var table = $('#tblCutomerDetails').DataTable();
    var data_row = table.row($(this).closest('tr')).data();
    if (data_row.Types == "Shipment") {
        window.location.href = baseUrl + '/Customer/Detail/' + data_row.Id;
    }
    else {
        window.location.href = baseUrl + '/Customer/FumigationDetail/' + data_row.Id;
    }
});

//#region DataTable Binding
var CustomerDashboardDetail = function () {
    $('#tblCutomerDetails').DataTable({
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
            "url": baseUrl + "Customer/GetCustomerOldDashboardDetails",
            "type": "POST",
            "datatype": "json",
        },
        "columns": [
            { "data": "UserId", "name": "UserId", "autoWidth": true },
            { "data": "StatusName", "name": "StatusName", "autoWidth": true },
            { "data": "Types", "name": "Types", "autoWidth": true },
            { "data": "AirWayBill", "name": "AirWayBill", "autoWidth": true },
            { "data": "PickDateTime", "autoWidth": true },
            { "data": "DeliveryDateTime", "autoWidth": true },
            //{ "data": "ShipmentRefNo", "name": "ShipmentRefNo", "autoWidth": true },
            { "data": "DriverName", "name": "DriverName", "autoWidth": true },
            { "data": "CostumerEquipment", "name": "CostumerEquipment", "autoWidth": true },
            { "name": "View", "autoWidth": true },
            { "name": "Track", "autoWidth": true },

        ],
        "order": [[0, "desc"]],
        columnDefs: [
        //#region sortng for current page
            //{
            //    "targets": 1,
            //    "orderable": false,
            //    "render": function (data, type, row, meta) {
            //        return StatusCheckForShipment(row.StatusName)

            //    }
            //},
            //{
            //    "targets": 2,
            //    "orderable": false,                
            //},
            //{
            //    "targets": 3,
            //    "orderable": false,
            //},
            //{
            //    "targets": 4,
            //    "orderable": false,
            //    "render": function (data, type, row, meta) {
            //        return '<label>(' + ConvertDate(row.PickDateTime, true) + ')</label>'

            //    }
            //},
            //{
            //    "targets": 5,
            //    "orderable": false,
            //    "render": function (data, type, row, meta) {
            //        return '<label>(' + ConvertDate(row.DeliveryDateTime, true) + ')</label>'

            //    }
            //},
            //{
            //    "targets": 6,
            //    "orderable": false,
            //},
            //{
            //    "targets": 7,
            //    "orderable": false,
            //},
            //{
            //    "targets": 8,
            //    "orderable": false,
            //    "render": function (data, type, row, meta) {

            //        if (row.Types == "Shipment") {
            //            var btnTrackDetail =
            //                '<a href="' + baseUrl + '/Customer/TrackShipment/' + row.Id + '" data-toggle="tooltip" title="Track" class="chng-color-View">' +
            //                '<i class="far fa-list-alt"></i>' +
            //                '</a>';

            //            return '<div class="action-ic">' + btnTrackDetail + '</div>'
            //        }
            //        else {
            //            var btnTrackDetail =
            //                '<a href="' + baseUrl + '/Customer/TrackFumigation/' + row.Id + '" data-toggle="tooltip" title="Track" class="chng-color-Track">' +
            //                '<i class="far fa-list-alt"></i>' +
            //                '</a>';

            //            return '<div class="action-ic">' + btnTrackDetail + '</div>'
            //        }
            //    }
            //},
            //{
            //    "targets": 9,
            //    "orderable": false,
            //    "render": function (data, type, row, meta) {

            //        if (row.Types == "Shipment") {
            //            var btnShipmentDetail = '<a href="' + baseUrl + '/Customer/Detail/' + row.Id + '" data-toggle="tooltip" title="View" class="fas fa-eye-open chng-color-View">' +
            //                '<i class="fas fa-eye"></i>' +
            //                '</a> ';

            //            return '<div class="action-ic">' + btnShipmentDetail + '</div>'
            //        }
            //        else {
            //            var btnFumigationDetail = '<a href="' + baseUrl + '/Customer/FumigationDetail/' + row.Id + '" data-toggle="tooltip" title="View" class="fas fa-eye-open chng-color-Track">' +
            //                '<i class="fas fa-eye"></i>' +
            //                '</a> ';

            //            return '<div class="action-ic">' + btnFumigationDetail + '</div>'
            //        }
            //    }
            //},
            //{
            //    "targets": 0,
            //    "visible": false,
            //}
            //#endregion

            {
                "targets": 1,
                "orderable": true,
                "render": function (data, type, row, meta) {
                    return StatusCheckForShipment(row.StatusName)

                }
            },
            {
                "targets": 4,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    return '<label>(' + ConvertDate(row.PickDateTime, true) + ')</label>'

                }
            },
            {
                "targets": 5,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    return '<label>(' + ConvertDate(row.DeliveryDateTime, true) + ')</label>'

                }
            },
            {
                "targets": 8,
                "orderable": false,
                "render": function (data, type, row, meta) {

                    if (row.Types == "Shipment") {
                        var btnTrackDetail =
                            '<a href="' + baseUrl + '/Customer/TrackShipment/' + row.Id + '" data-toggle="tooltip" title="Track" class="chng-color-View">' +
                            '<i class="far fa-list-alt"></i>' +
                            '</a>';

                        return '<div class="action-ic">' + btnTrackDetail + '</div>'
                    }
                    else {
                        var btnTrackDetail =
                            '<a href="' + baseUrl + '/Customer/TrackFumigation/' + row.Id + '" data-toggle="tooltip" title="Track" class="chng-color-Track">' +
                            '<i class="far fa-list-alt"></i>' +
                            '</a>';

                        return '<div class="action-ic">' + btnTrackDetail + '</div>'
                    }
                }
            },
            {
                "targets": 9,
                "orderable": false,
                "render": function (data, type, row, meta) {

                    if (row.Types == "Shipment") {
                        var btnShipmentDetail = '<a href="' + baseUrl + '/Customer/Detail/' + row.Id + '" data-toggle="tooltip" title="View" class="fas fa-eye-open chng-color-View">' +
                            '<i class="fas fa-eye"></i>' +
                            '</a> ';

                        return '<div class="action-ic">' + btnShipmentDetail + '</div>'
                    }
                    else {
                        var btnFumigationDetail = '<a href="' + baseUrl + '/Customer/FumigationDetail/' + row.Id + '" data-toggle="tooltip" title="View" class="fas fa-eye-open chng-color-Track">' +
                            '<i class="fas fa-eye"></i>' +
                            '</a> ';

                        return '<div class="action-ic">' + btnFumigationDetail + '</div>'
                    }
                }
            },
            {
                "targets": 0,
                "visible": false,
            }
        ]
    });

    oTable = $('#tblCutomerDetails').DataTable();

}
//#endregion

//#region Status Colour Change

function StatusCheckForCustomer(status) {

    var PreStatus = "";
    if ($.trim(status) == "Order Taken") {
        PreStatus = '<span style=""">' + status + '</span>'
    }
    if ($.trim(status) == "Dispatched") {
        PreStatus = '<span class="badge" style="background-color:#0071c3;color:white">' + status + '</span>'
    }
    if ($.trim(status) == "Immediate Attention") {
        PreStatus = '<span class="badge" style="background-color:#f10802;color:white">' + status + '</span>'
    }
    if ($.trim(status) == "Change of Status / On Hold") {
        PreStatus = '<span class="badge" style="background-color:#e8610b;color:white">' + status + '</span>'
    }
    if ($.trim(status) == "Loading at pick up location") {
        PreStatus = '<span class="badge" style="background-color:#f86807;color:white">' + status + '</span>'
    }
    if ($.trim(status) == "In-Route to delivery location") {
        PreStatus = '<span class="badge" style="background-color:#fffd01;color:black">' + status + '</span>'
    }
    if ($.trim(status) == "Delivered") {
        PreStatus = '<span class="badge" style="background-color:#178a24;color:white">' + status + '</span>'
    }
    if ($.trim(status) == "Cancelled") {
        PreStatus = '<span class="badge" style="background-color:#d93d42;color:white">' + status + '</span>'
    }
    if ($.trim(status) == "In-Progress") {
        PreStatus = '<span class="badge" style="background-color:#95ccdb;color:white">' + status + '</span>'
    }
    return PreStatus;
}

//#endregion



$(document).ready(function () {

    var current_fs, next_fs, previous_fs;

    $(".next").click(function () {

        str1 = "next1";
        str2 = "next2";
        str3 = "next3";
        str3 = "next4";
        str3 = "next5";
        str3 = "next6";
        str3 = "next7";
        str3 = "next8";

        if (!str2.localeCompare($(this).attr('id')) && validate1(0) == true) {
            val2 = true;
        }
        else {
            val2 = false;
        }

        if (!str3.localeCompare($(this).attr('id')) && validate2(0) == true) {
            val3 = true;
        }
        else {
            val3 = false;
        }

        if (!str1.localeCompare($(this).attr('id')) || (!str2.localeCompare($(this).attr('id')) && val2 == true) || (!str3.localeCompare($(this).attr('id')) && val3 == true)) {
            current_fs = $(this).parent().parent();
            next_fs = $(this).parent().parent().next();

            $(current_fs).removeClass("show");
            $(next_fs).addClass("show");

            $("#progressbar li").eq($("fieldset").index(next_fs)).addClass("active");

            current_fs.animate({}, {
                step: function () {

                    current_fs.css({
                        'display': 'none',
                        'position': 'relative'
                    });

                    next_fs.css({
                        'display': 'block'
                    });
                }
            });
        }
    });

    $(".prev").click(function () {

        current_fs = $(this).parent().parent();
        previous_fs = $(this).parent().parent().prev();

        $(current_fs).removeClass("show");
        $(previous_fs).addClass("show");

        $("#progressbar li").eq($("fieldset").index(next_fs)).removeClass("active");

        current_fs.animate({}, {
            step: function () {

                current_fs.css({
                    'display': 'none',
                    'position': 'relative'
                });

                previous_fs.css({
                    'display': 'block'
                });
            }
        });
    });

    $('.radio-group .radio').click(function () {
        $(this).toggleClass('selected');
    });

});


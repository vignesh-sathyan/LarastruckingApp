//#region Ready State
$(function () {
    GetOrderTakenFumigationList();
    $('#tblOrderTakenFumigation').DataTable().columns.adjust();
    GetOtherFumigationList();
    updateOrderTakenCount();
    updateInProgressCount();
    

});
//#endregion

function updateOrderTakenCount() {
    var count = 0;
    $.ajax({
        type: 'GET',
        url: baseUrl + "/Fumigation/Fumigation/GetOrderTaken",
        //data: { "driverid": driverid },
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        success: function (msg) {
            // Do something interesting here.
            //console.log("count order taken", msg);
            count = msg;
            $("#fumigationOrder").text(count);
        },
        error: function (xhr, err) {
            console.log("error : " + err);
        }
    })

}

function CustomerDetail(shipmentid) {
    var count = 0;
    $.ajax({
        type: 'GET',
        url: baseUrl + "/Fumigation/Fumigation/CustomerDetail",
        data: { "fumigationid": shipmentid },
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        success: function (msg) {
            // Do something interesting here.
           // console.log("customer Detail fum: ", msg);
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

function updateInProgressCount() {
    var count = 0;
    $.ajax({
        type: 'GET',
        url: baseUrl + "/Fumigation/Fumigation/GetFumigationInProgress",
        //data: { "driverid": driverid },
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        success: function (msg) {
            // Do something interesting here.

            count = msg;
            $("#fumigationProgress").text(count);
        },
        error: function (xhr, err) {
            console.log("error : " + err);
        }
    })

}

$('#tblOrderTakenFumigation').on('dblclick', 'tbody tr', function () {
    var table = $('#tblOrderTakenFumigation').DataTable();
    var data_row = table.row($(this).closest('tr')).data();
    window.location.href = baseUrl + '/Fumigation/Fumigation/Index/' + data_row.FumigationId;
});

$('#tblOtherFumigation').on('dblclick', 'tbody tr', function () {
    var table = $('#tblOtherFumigation').DataTable();
    var data_row = table.row($(this).closest('tr')).data();
    console.log("data_row: ", data_row);
    window.location.href = baseUrl + '/Fumigation/Fumigation/Index/' + data_row.FumigationId;
});
$('#tblOrderTakenFumigation').on('click', 'tbody tr', function () {
    var table = $('#tblOrderTakenFumigation').DataTable();
    var data_row;
    if (table.row($(this).parent().closest('tr')).data() != undefined) {
        data_row = table.row($(this).parent().closest('tr')).data();
    }
    else {
        data_row = table.row($(this).closest('tr')).data();
    }
    console.log("data_row: ", data_row);
    CustomerDetail(data_row.FumigationId);
    $("#shipmentnotify").css("display", "block");
    var iframe = $('#shipmentnotify');

    // set the src attribute
    iframe.attr('src', baseurl + '/Fumigation/Fumigation/ViewFumigationNotification/' + data_row.FumigationId);
    //window.location.href = baseUrl + '/Shipment/Shipment/Index/' + data_row.ShipmentId;

});

$('#tblOtherFumigation').on('click', 'tbody tr', function () {
    var table = $('#tblOtherFumigation').DataTable();
    var data_row ;
    if (table.row($(this).parent().closest('tr')).data() != undefined) {
        data_row = table.row($(this).parent().closest('tr')).data();
    }
    else {
        data_row = table.row($(this).closest('tr')).data();
    }
    //console.log("data_row: ", data_row);

   
    CustomerDetail(data_row.FumigationId);
    $("#ShipmentNotify").css("display", "block");
    var iframe = $('#ShipmentNotify');

    // Set the src attribute
    iframe.attr('src', baseUrl + '/Fumigation/Fumigation/ViewFumigationNotification/' + data_row.FumigationId);
    // $("#ShipmentNotify").css("display", "block");
    //var iframe = $('#ShipmentNotify');

    // Set the src attribute
    // iframe.attr('src', baseUrl + '/Shipment/Shipment/ViewShipmentNotification/' + data_row.ShipmentId);
    //window.location.href = baseUrl + '/Shipment/Shipment/Index/' + data_row.ShipmentId;

});


var d = new Date();
var month = d.getMonth() + 1;
var day = d.getDate();

var datetime = (month < 10 ? '0' : '') + month + '/' +
    (day < 10 ? '0' : '') + day + '/' +
    d.getFullYear() + "  " +
    (d.getHours() < 10 ? '0' : '') + d.getHours() + ":" +
    (d.getMinutes() < 10 ? '0' : '') + d.getMinutes() + ":" +
    (d.getSeconds() < 10 ? '0' : '') + d.getSeconds();

//#region DataTable Binding
var GetOrderTakenFumigationList = function () {
    $('#tblOrderTakenFumigation').DataTable().clear().destroy();

  
    $('#tblOrderTakenFumigation').DataTable({
        // "bInfo": true,
       dom: 'rtip',
        //buttons: [
        //    {
        //        extend: 'print',
        //        orientation: 'landscape',
        //        pageSize: 'LEGAL',
        //        title: "",
        //        text: '<img src="../../Assets/images/printer.png" style="height:18px;margin-right: 5px;width:16px;"/> Print',
        //        messageBottom: datetime,
        //        //messageTop: datetime,
        //        exportOptions: {

        //            columns: ':visible',
        //            stripHtml: false,
        //            //columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19]

        //        },
        //        customize: function (win) {

        //            var last = null;
        //            var current = null;
        //            var bod = [];

        //            var css = '@page { size: landscape;}  ',
        //                head = win.document.head || win.document.getElementsByTagName('head')[0],
        //                style = win.document.createElement('style');

        //            style.type = 'text/css';
        //            style.media = 'print';

        //            if (style.styleSheet) {
        //                style.styleSheet.cssText = css;
        //            }
        //            else {
        //                style.appendChild(win.document.createTextNode(css));
        //            }

        //            head.appendChild(style);
        //            $(win.document.body)
        //                .css('font-size', '10pt')
        //                .prepend(
        //                    "<table id='checkheader'><tr><td width='80%' ><h3 style='font-size: 20px;'>REQUESTED FUMIGATION</h3></td><td width='20%'><div><img src='http://larastruckinglogistics-app.azurewebsites.net/Images/Laraslogo.png' height='100px'/></div></td></tr></table>"
        //                );
        //        }
        //    },
        //    {
        //        extend: 'colvis',
        //        text: '<img src="../../Assets/images/info.png" style="width:17px;;margin-top: -2px;height: 17px;"/> column visibility',
        //        columns: ':not(.noVis)',
        //    }

        //],

        select: 'single',
       // "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
        //responsive: true,
        "scrollX": true,
        "scrollCollapse": true,
        "fixedColumns": true,
        filter: true,
        orderMulti: true,
        processing: true,
        serverSide: true,
        searching: false,
        bDestroy: true,
        stateSave: true,
        "language": {
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
        },
        "ajax": {
            "url": baseUrl + "/Fumigation/Fumigation/GetFumigationList",
            "type": "POST",
            //"data": "FumigationId",
            "datatype": "json",
            //"async": false,
        },
        columnDefs: [
            {
                "targets": [0],
                "className": 'noVis',

            }
        ],
        "columns": [
            {
                "data": "FumigationId",
                "name": "FumigationId",
                "autoWidth": false,
                "visible": false,
            },
            {
                "name": "StatusName",
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    // console.log("row: ", row);
                    return StatusCheckForShipment(row.StatusName)
                }
            },

            {

                "name": "PickUpArrival",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                  
                    console.log("tblorder row: ", row);
                    ///console.log("tblorder row: ", row);
                    var isSame = false;
                    if (row.PickUpArrival != null && row.PickUpArrival != '') {
                       //  console.log("row.PickUpArrival: " + row.PickUpArrival);
                        var PickUpArrival = row.PickUpArrival.split("|");

                      //  if (ConvertDateNew(PickUpArrival[0], true) != "NaN/NaN NaN:NaN") {
                            if (PickUpArrival.length > 0) {
                                var count = 0;
                                for (var i = 0; i < PickUpArrival.length; i++) {
                                    //console.log("UTC time: " + new Date(PickUpArrival[0]).toUTCString());
                                    if (PickUpArrival[i] == PickUpArrival[0]) {
                                        count = count + 1;
                                    }
                                }
                                if (count == PickUpArrival.length) {
                                    isSame = true;
                                }
                                if (isSame) {
                                    var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                    var tabCon = '';
                                    var tabBottom = '</table>';
                                    var ftype = row.FumigationTypes.split('$');
                                    if (ftype.length > 1) {
                                        for (var i = 0; i < ftype.length; i++) {

                                            // objRouteStop.PickUpArrival = response.GetFumigationRouteDetail[i].PickUpArrival == null ? "" : ConvertDateEdit(response.GetFumigationRouteDetail[i].PickUpArrival, true);
                                            tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top">' + ConvertSqlDateTimeNew(PickUpArrival[0]) + '</label></td></tr>';
                                            //tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top">' + ConvertDateEdit(PickUpArrival[0], true) + '</label></td></tr>';
                                        }
                                        return tabTop + tabCon + tabBottom;
                                    }
                                    else {

                                        return '<label data-toggle="tooltip" data-placement="top">' + ConvertSqlDateTimeNew(PickUpArrival[0]) + '</label>';
                                        //return '<label data-toggle="tooltip" data-placement="top">' + ConvertDateEdit(PickUpArrival[0], true) + '</label>';
                                    }
                                    // var tabCon = '<label data-toggle="tooltip" data-placement="top">' + ConvertDateNew(PickUpArrival[0],true) + '</label>';
                                    // return tabCon;
                                }
                                else {
                                    var tabCon = '';
                                    var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                    // var tabCon = '';
                                    var tabBottom = '</table>';
                                    if (PickUpArrival.length > 0) {
                                        for (var i = 0; i < PickUpArrival.length; i++) {
                                            tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top">' + ConvertSqlDateTimeNew(PickUpArrival[i]) + '</label></td></tr>';
                                            //tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top">' + ConvertDateEdit(PickUpArrival[i],true) + '</label></td></tr>';
                                        }
                                        return tabTop + tabCon + tabBottom
                                    }
                                }
                            }
                      

                    } else {
                        return 'NA'

                    }

                }

            },

            {
                //"data": "FumigationTypes",
                "name": "FumigationTypes",
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    var isSame = false;
                    if (row.FumigationTypes != null && row.FumigationTypes != '') {
                        var fumigationTypesList = row.FumigationTypes.split("$");
                        // console.log("FumigationList: " + fumigationTypesList);
                        if (fumigationTypesList.length > 1) {
                            var count = 0;

                            var tabCon = '';
                            var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var tabBottom = '</table>';
                            if (fumigationTypesList.length > 0) {
                                for (var i = 0; i < fumigationTypesList.length; i++) {
                                    tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top">' + fumigationTypesList[i].trim() + '</label></td></tr>';
                                }
                                return tabTop + tabCon + tabBottom
                            }
                            // }
                        }
                        else {
                            var tabCon = '';
                            if (fumigationTypesList.length > 0) {
                                for (var i = 0; i < fumigationTypesList.length; i++) {
                                    tabCon = '<label data-toggle="tooltip" data-placement="top">' + fumigationTypesList[i].trim() + '</label>';;
                                }
                                return tabCon
                            }

                        }

                    } else {
                        return 'NA'

                    }
                }

            },
            {
                // "data": "CustomerName",
                "name": "CustomerName",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    var lengths = row.FumigationTypes;
                    const myArray = lengths.split(",");
                    if (row.CustomerName != null && row.CustomerName != '') {
                        //customer += customer;
                        return '<label data-toggle="tooltip" data-placement="top">' + row.CustomerName + '</label>'
                    } else {
                        return 'NA'

                    }
                }
            },

            {
                //"data": "PickUpLocation",
                "name": "PickUpLocation",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    var isSame = false;
                    if (row.PickUpLocation != null && row.PickUpLocation != '') {
                        var pickupLocation = row.PickUpLocation.split("|");
                        // console.log("pickupLocation: " + pickupLocation);
                        if (pickupLocation.length > 0) {
                            var count = 0;
                            for (var i = 0; i < pickupLocation.length; i++) {
                                if (pickupLocation[i] == pickupLocation[0]) {
                                    // console.log(pickupLocation[i] + " | " + pickupLocation[0]);
                                    count = count + 1;
                                }
                            }
                            if (count == pickupLocation.length) {
                                isSame = true;
                            }
                            if (isSame) {
                                var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                var tabCon = '';
                                var tabBottom = '</table>';
                                var ftype = row.FumigationTypes.split('$');
                                if (ftype.length > 1) {
                                    for (var i = 0; i < ftype.length; i++) {
                                        tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(pickupLocation[0]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(pickupLocation[0]) + '</label></td></tr>';
                                    }
                                    return tabTop + tabCon + tabBottom;
                                }
                                else {
                                    tabCon = '<label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(pickupLocation[0]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(pickupLocation[0]) + '</label>';
                                    return tabCon;
                                }


                            }
                            else {
                                var lblPickupLocation = "";
                                if (pickupLocation.length > 1) {
                                    for (var i = 0; i < pickupLocation.length; i++) {
                                        lblPickupLocation += '<tr><td><label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(pickupLocation[i]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(pickupLocation[i]) + '</label></td></tr>'
                                    }
                                    var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                    // var tabCon = '';
                                    var tabBottom = '</table>';
                                    //lblPickupLocation = lblPickupLocation.trim("<br/>");
                                    return tabTop + lblPickupLocation + tabBottom;
                                }
                                else {
                                    var tabCon = '';
                                    tabCon = '<label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(pickupLocation[0]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(pickupLocation[0]) + '</label>';
                                    return tabCon
                                }
                            }
                        }
                    }
                    else {

                        return 'NA'
                    }
                }
            },
            {

                "name": "Temperature",
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    var AWB_CP_CN = "";
                    var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                    // var tabCon = '';
                    var tabBottom = '</table>';
                    var type = row.FumigationTypes.split('$');
                    for (let i = 0; i < type.length; i++) {

                        if (type[i] == " AIR SHIPMENT" || "ON FLOOR") {

                            if ($.trim(row.AWB) != "") {
                                var pickupLocation = row.AWB.split(",");
                                //console.log("AWB: "+pickupLocation);
                                if (pickupLocation.length > 0) {

                                    var lblPickupLocation = "";
                                    if (pickupLocation.length > 1) {
                                        for (var i = 0; i < pickupLocation.length; i++) {
                                            AWB_CP_CN += '<tr><td><label data-toggle="tooltip" data-placement="top" title="' + pickupLocation[i] + '">' + pickupLocation[i] + '</label></td></tr>'
                                        }
                                        var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                        // var tabCon = '';
                                        var tabBottom = '</table>';
                                        //lblPickupLocation = lblPickupLocation.trim("<br/>");
                                        return tabTop + AWB_CP_CN + tabBottom;
                                    }
                                    else {
                                        return '<label data-toggle="tooltip" data-placement="top" title="' + row.AWB + '">' + row.AWB + '</label>'
                                    }

                                }

                            }
                            else {
                                return '<label data-toggle="tooltip" data-placement="top" title="' + row.ContainerNo + '"> NA </label>'
                            }
                        }
                        //else {
                        //    if ($.trim(row.ContainerNo) != "") {
                        //        console.log("FTYPE 1: " + type[i]);
                        //        var pickupLocation = row.ContainerNo.split(",");
                        //        if (pickupLocation.length > 0) {
                        //            console.log("container: " + pickupLocation);

                        //            var lblPickupLocation = "";
                        //            if (pickupLocation.length > 1) {
                        //                for (var i = 0; i < pickupLocation.length; i++) {
                        //                    AWB_CP_CN += '<tr><td><label data-toggle="tooltip" data-placement="top" title="' + pickupLocation[i] + '">' + pickupLocation[i] + '</label></td></tr>'
                        //                }
                        //                var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                        //                // var tabCon = '';
                        //                var tabBottom = '</table>';
                        //                return tabTop + AWB_CP_CN + tabBottom;
                        //                //lblPickupLocation = lblPickupLocation.trim("<br/>");

                        //            }
                        //            else {
                        //                return '<label data-toggle="tooltip" data-placement="top" title="' + row.ContainerNo + '">' + row.ContainerNo + '</label>'
                        //            }

                        //        }

                        //    }
                        //    else {
                        //        return '<label data-toggle="tooltip" data-placement="top" title="' + row.ContainerNo + '"> NA </label>'
                        //    }

                        //}

                        //else {
                        //    return '<label data-toggle="tooltip" data-placement="top" title="' + row.ContainerNo + '"> NA </label>'
                        //}
                        //else if ($.trim(row.CustomerPO) != "") {
                        //     var pickupLocation = row.CustomerPO.split(",");
                        //     if (pickupLocation.length > 0) {


                        //         var lblPickupLocation = "";
                        //         if (pickupLocation.length > 1) {
                        //             for (var i = 0; i < pickupLocation.length; i++) {
                        //                 AWB_CP_CN += '<tr><td><label data-toggle="tooltip" data-placement="top" title="' + pickupLocation[i] + '">' + pickupLocation[i] + '</label></td></tr>'
                        //             }
                        //             var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                        //             // var tabCon = '';
                        //             var tabBottom = '</table>';
                        //             //lblPickupLocation = lblPickupLocation.trim("<br/>");
                        //             return tabTop + AWB_CP_CN + tabBottom;
                        //         }
                        //         else {
                        //             return '<label data-toggle="tooltip" data-placement="top" title="' + row.CustomerPO + '">' + row.CustomerPO + '</label>'
                        //         }
                        //     }


                        // }
                        //else {
                        //    return '<label data-toggle="tooltip" data-placement="top" title="' + row.CustomerPO + '">  NA </label > '
                        //}



                    }

                }

            },
            {
                // "data": "VendorNconsignee",
                "name": "VendorNconsignee",
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    if (row.VendorNconsignee != null && row.VendorNconsignee != '') {

                        var vendorNconsignee = row.VendorNconsignee.split("|")

                        if (vendorNconsignee.length > 1) {
                            var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var tabBottom = '</table>';
                            var lblPickupLocation = "";
                            for (var i = 0; i < vendorNconsignee.length; i++) {
                                // console.log("vendorNconsignee: " + vendorNconsignee[i]);
                                lblPickupLocation += '<tr><td><label data-toggle="tooltip" data-placement="top" title= "'+row.VendorNconsignee+'" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + vendorNconsignee[i] + '</label></td></tr>';

                            }
                            return tabTop + lblPickupLocation + tabBottom
                        }

                        else {
                            var lblPickupLocation = "";
                            return '<label data-toggle="tooltip" data-placement="top" title= "' + row.VendorNconsignee +'" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + row.VendorNconsignee + '</label>';
                            //return lblPickupLocation

                        }

                    } else {
                        return 'NA'

                    }
                }

            },

            {
                //"data": "BoxCount",
                "name": "BoxCount",
                "render": function (data, type, row, meta) {

                    if (row.BoxCount != null && row.BoxCount != '') {
                        var BoxCount = row.BoxCount.split("|")

                        if (BoxCount.length > 1) {
                            var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var tabBottom = '</table>';
                            var lblPickupLocation = "";
                            for (var i = 0; i < BoxCount.length; i++) {
                                // console.log("vendorNconsignee: " + BoxCount[i]);
                                var boxcounts = BoxCount[i].split('.');
                                lblPickupLocation += '<tr><td><label data-toggle="tooltip" data-placement="top">' + boxcounts[0] + '</label></td></tr>';

                            }
                            return tabTop + lblPickupLocation + tabBottom
                        }

                        else {
                            var lblPickupLocation = "";
                            var boxcounts = row.BoxCount.split('.');
                            return '<label data-toggle="tooltip" data-placement="top">' + boxcounts[0] + '</label>';
                            //return lblPickupLocation

                        }

                    } else {
                        return 'NA'

                    }
                }

            },

            {
                //"data": "PalletCount",
                "name": "PalletCount",
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    if (row.PalletCount != null && row.PalletCount != '') {

                        var PalletCount = row.PalletCount.split("|")

                        if (PalletCount.length > 1) {
                            var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var tabBottom = '</table>';
                            var lblPickupLocation = "";
                            for (var i = 0; i < PalletCount.length; i++) {
                                var palletcounts = PalletCount[i].split('.');
                                //console.log("vendorNconsignee: " + PalletCount[i]);
                                lblPickupLocation += '<tr><td><label data-toggle="tooltip" data-placement="top">' + palletcounts[0] + '</label></td></tr>';

                            }
                            return tabTop + lblPickupLocation + tabBottom
                        }

                        else {
                            var lblPickupLocation = "";
                            var palletcounts = row.PalletCount.split('.');
                            return '<label data-toggle="tooltip" data-placement="top">' + palletcounts[0] + '</label>';
                            //return lblPickupLocation

                        }

                    } else {
                        return 'NA'

                    }
                }

            },
            {
                //"data": "TrailerPosition",
                "name": "TrailerPosition",
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    if (row.TrailerPosition != null && row.TrailerPosition != '') {
                        var TrailerPosition = row.TrailerPosition.split("|")

                        if (TrailerPosition.length > 1) {
                            var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var tabBottom = '</table>';
                            var lblPickupLocation = "";
                            for (var i = 0; i < TrailerPosition.length; i++) {
                                //console.log("vendorNconsignee: " + TrailerPosition[i]);
                                lblPickupLocation += '<tr><td><label data-toggle="tooltip" data-placement="top">' + TrailerPosition[i] + '</label></td></tr>';

                            }
                            return tabTop + lblPickupLocation + tabBottom
                        }

                        else {
                            var lblPickupLocation = "";
                            return '<label data-toggle="tooltip" data-placement="top">' + row.TrailerPosition + '</label>';
                            //return lblPickupLocation

                        }

                    } else {
                        return 'NA'

                    }
                }
            },
            {
                //"data": "Temperature",
                "name": "Temperature",
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    if ($.trim(row.Temperature) != "F") {

                        var Temperature = row.Temperature.split("|")

                        if (Temperature.length > 1) {
                            var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var temp1 = '';
                            var tabBottom = '</table>';
                            var lblPickupLocation = "";
                            for (var i = 0; i < Temperature.length; i++) {
                                // console.log("temperature: " + Temperature[i]);                              
                                var temp = Temperature[i].split('.');
                                if (temp.length != 1) {
                                    temp1 = temp[0] + '.0';
                                }
                                else {
                                    temp1 = "";

                                }

                                lblPickupLocation += '<tr><td><label data-toggle="tooltip" data-placement="top">' + temp1 + '</label></td></tr>';

                            }
                            return tabTop + lblPickupLocation + tabBottom
                        }

                        else {
                            var lblPickupLocation = "";
                            return '<label data-toggle="tooltip" data-placement="top">' + row.Temperature + '</label>';
                            //return lblPickupLocation

                        }

                    }
                    else {
                        return '<label></label>'
                    }

                }
            },
            {

                //"data": "FumigationSite",
                "name": "FumigationSite",
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    var isSame = false;
                    if (row.FumigationSite != null && row.FumigationSite != '') {
                        var fumigationSite = row.FumigationSite.split("|");
                        if (fumigationSite.length > 0) {
                            var count = 0;
                            for (var i = 0; i < fumigationSite.length; i++) {
                                if (fumigationSite[i] == fumigationSite[0]) {
                                    count = count + 1;
                                }
                            }
                            if (count == fumigationSite.length) {
                                isSame = true;
                            }
                        }
                        if (isSame) {
                            var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            var tabCon = '';
                            var tabBottom = '</table>';
                            var ftype = row.FumigationTypes.split('$');
                            if (ftype.length > 1) {
                                for (var i = 0; i < ftype.length; i++) {
                                    tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(fumigationSite[0]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(fumigationSite[0]) + '</label></td></tr>';
                                }
                                return tabTop + tabCon + tabBottom;
                            }
                            else {
                                return '<label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(fumigationSite[0]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(fumigationSite[0]) + '</label>'
                            }
                        }
                        else {
                            var lblfumigationSite = "";
                            if (fumigationSite.length > 1) {
                                for (var i = 0; i < fumigationSite.length; i++) {
                                    lblfumigationSite += '<tr><td><label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(fumigationSite[i]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(fumigationSite[i]) + '</label></td></tr>'
                                }
                                var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                // var tabCon = '';
                                var tabBottom = '</table>';
                                // var tabCon = '<tr><td><label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(fumigationSite[0]) + '">' + GetCompanyNew(fumigationSite[0]) + '</label><br/></td></tr>'; 
                                //lblfumigationSite = lblfumigationSite.trim("<br/>");
                                return tabTop + lblfumigationSite + tabBottom;
                            }
                        }
                    }
                    else {

                        return 'NA'
                    }

                }

            },
            {
                // "data": "PickUpEquipment",
                "name": "PickUpEquipment",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    var isSame = false;
                    //console.log("row: ",row);
                    if (row.PickUpEquipment != null && row.PickUpEquipment != '') {
                        var PickUpEquipment = row.PickUpEquipment.split("$");
                        // console.log("PickUpEquipment count:" + PickUpEquipment.length);
                        if (PickUpEquipment.length > 0) {
                            var count = 0;
                            for (var i = 0; i < PickUpEquipment.length; i++) {
                                if (PickUpEquipment[i] == PickUpEquipment[0]) {
                                    count = count + 1;
                                }
                            }
                            if (count == PickUpEquipment.length) {
                                //  console.log("count:" + count.length);
                                isSame = true;
                                //console.log("count:" +count);
                                //console.log("count:" + PickUpEquipment.length);
                                //console.log("count:" + PickUpEquipment);
                            }
                            if (isSame) {
                                // console.log("count:" + PickUpEquipment.length);
                                var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                var tabCon = '';
                                var tabBottom = '</table>';
                                var ftype = row.FumigationTypes.split('$');
                                if (ftype.length > 1) {
                                    for (var i = 0; i < ftype.length; i++) {
                                        tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top">' + PickUpEquipment[0] + '</label></td></tr>';
                                    }
                                    return tabTop + tabCon + tabBottom;
                                }
                                else {
                                    return '<label data-toggle="tooltip" data-placement="top">' + PickUpEquipment[0] + '</label>'
                                }
                            }
                            else {
                                // console.log("count:" + PickUpEquipment);
                                var lblfumigationSite = "";
                                if (PickUpEquipment.length > 1) {
                                    for (var i = 0; i < PickUpEquipment.length; i++) {
                                        lblfumigationSite += '<tr><td><label data-toggle="tooltip" data-placement="top">' + PickUpEquipment[i] + '</label></td></tr>'
                                    }
                                    var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                    // var tabCon = '';
                                    var tabBottom = '</table>';
                                    // var tabCon = '<tr><td><label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(fumigationSite[0]) + '">' + GetCompanyNew(fumigationSite[0]) + '</label><br/></td></tr>'; 
                                    //lblfumigationSite = lblfumigationSite.trim("<br/>");
                                    return tabTop + lblfumigationSite + tabBottom;
                                }
                            }
                        }
                        else {

                            return '<label data-toggle="tooltip" data-placement="top">' + row.PickUpEquipment + '</label>'


                        }

                    }
                    else {

                        return 'NA'
                    }


                }
            },
            {
                // "data": "PickUpDriver",
                "name": "PickUpDriver",
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    if (row.PickUpDriver != null && row.PickUpDriver != '') {

                        var PickUpDriver = row.PickUpDriver.split("$");

                        if (PickUpDriver.length > 1) {
                            var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var tabBottom = '</table>';
                            var lblPickupLocation = "";
                            for (var i = 0; i < PickUpDriver.length; i++) {
                                //console.log("vendorNconsignee: " + PickUpDriver[i]);
                                //var pickupeq = PickUpDriver[i].split('|');
                                lblPickupLocation += '<tr><td><label data-toggle="tooltip" data-placement="top">' + PickUpDriver[i] + '</label></td></tr>';

                            }
                            return tabTop + lblPickupLocation + tabBottom
                        }

                        else {
                            var lblPickupLocation = "";
                            var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var tabBottom = '</table>';
                            PickUpDriver = row.PickUpDriver.split("$");
                            for (var i = 0; i < PickUpDriver.length; i++) {
                                //console.log("Single value: " + PickUpDriver[i]);
                                lblPickupLocation += '<label data-toggle="tooltip" data-placement="top">' + PickUpDriver[i] + '</label>';

                            }
                            return lblPickupLocation

                        }

                    } else {
                        return 'NA'

                    }


                }
            },
            {
                //"data": "DeliveryLocation",
                "name": "DeliveryLocation",
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    var isSame = false;
                    if (row.DeliveryLocation != null && row.DeliveryLocation != '') {
                        var deliveryLocation = row.DeliveryLocation.split("|");

                        if (deliveryLocation.length > 0) {
                            var count = 0;
                            for (var i = 0; i < deliveryLocation.length; i++) {
                                if (deliveryLocation[i] == deliveryLocation[0]) {
                                    count = count + 1;
                                }
                            }
                            if (count == deliveryLocation.length) {
                                isSame = true;
                            }

                            if (isSame) {
                                var tabCon = '<label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(deliveryLocation[0]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(deliveryLocation[0]) + '</label><br/>';
                                return tabCon;
                            }
                            else {
                                var lbldeliveryLocation = "";
                                if (deliveryLocation.length > 0) {
                                    for (var i = 0; i < deliveryLocation.length; i++) {
                                        lbldeliveryLocation += '<tr><td><label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(deliveryLocation[i]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(deliveryLocation[i]) + '</label></td></tr>'
                                    }
                                    var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                    // var tabCon = '';
                                    var tabBottom = '</table>';
                                    //lbldeliveryLocation = lbldeliveryLocation.trim("<br/>");
                                    return tabTop + lbldeliveryLocation + tabBottom;
                                }
                            }
                        }
                    }

                    else {
                        return 'NA'

                    }




                    // return '<label data-toggle="tooltip" data-placement="top" title="' + GetAddress(row.DeliveryLocation) + '">' + GetCompanyName(row.DeliveryLocation) + '</label>'
                }
            },
            {
                // "data": "PickUpDriver",
                "name": "Comments",
                "orderable": false,
                "autoWidth": true,
                "className":"comments",
                "render": function (data, type, row, meta) {

                    if (row.Comments != "") {
                        console.log("row.Comments: ", row.Comments);
                        var comments = row.Comments.replaceAll('|', '<br/>');
                        var toolcomments = row.Comments;
                        return '<label class="two" data-toggle="tooltip" data-placement="top" title="' + toolcomments + '" style="max-width:250px;display:block;white-space: nowrap;overflow: hidden !important;text-overflow: ellipsis;margin-right: 20px;">' + comments + '</label><a href="javascript: void(0)" class="delete_icon" data-toggle="tooltip" title="Delete" onclick="javascript:DeleteComments(' + row.FumigationId + ',this);" style="font-size: 13px;">' +
                        '<i class="fa fa-times"></i>';
                    }
                    else {
                        return '<label>NA</label>';
                    }



                }
            },
            //{
            //    // "data": "DeliveryDriver",
            //    "name": "DeliveryDriver",
            //    "autoWidth": true,
            //    "render": function (data, type, row, meta) {

            //        if (row.DeliveryDriver != null && row.DeliveryDriver != '') {

            //            return '<label data-toggle="tooltip" data-placement="top">' + row.DeliveryDriver.replaceAll("$", "<br/>") + '</label>'
            //        } else {
            //            return 'NA'

            //        }
            //    }
            //},
            //{
            //    "data": "DeliveryEquipment",
            //    "name": "DeliveryEquipment",
            //    "autoWidth": true,
            //    "render": function (data, type, row, meta) {

            //        if (row.DeliveryEquipment != null && row.DeliveryEquipment != '') {

            //            return '<label data-toggle="tooltip" data-placement="top">' + row.DeliveryEquipment.replaceAll("$", "<br/>") + '</label>'
            //        } else {
            //            return 'NA'

            //        }
            //    }
            //},
            //{

            //    "name": "DeliveryArrival",
            //    "autoWidth": true,
            //    "render": function (data, type, row, meta) {

            //        if (row.DeliveryArrival != null) {
            //            return '<label>' + ConvertDateNew(row.DeliveryArrival, true) + '</label>'
            //        }
            //        else {
            //            return '<label></label>'
            //        }
            //    }
            //},

            {
                "name": "Action",
                "orderable": false,
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    var btnEdit = '<a href="' + baseUrl + '/Fumigation/Fumigation/Index/' + row.FumigationId + '" data-toggle="tooltip" title="Edit" class="edit_icon">' +
                        '<i class="far fa-edit"></i>' +
                        '</a>';
                    var btnMap = '| <a href="javascript: void(0)" class="Map_icon" data-toggle="tooltip" id="redirectButton" title="Map" onclick="javascript:fn_RedirectToGpsTracker(' + row.FumigationId + ');" >' +
                        '<i class="fas fa-map-marked-alt"></i>' +
                        '</a>';
                    var btnCopy = ' | <a href="javascript: void(0)" class="edit_icon" data-toggle="tooltip" title="Copy Shipment" onclick="javascript:CopyFumigation(' + row.FumigationId + ');" >' +
                        '<i class="far fa-clone"></i>' +
                        '</a>';
                    var btnDelete = ' | <a href="javascript: void(0)" class="delete_icon" data-toggle="tooltip" title="Delete" onclick="javascript:DeleteFumigation(' + row.FumigationId + ');" >' +
                        '<i class="far fa-trash-alt"></i>' +
                        '</a>';
                    var btnPreview = '<a href="' + baseUrl + '/Fumigation/Fumigation/ViewFumigationNotification/' + row.FumigationId + '" title="Preview Quote" target="_blank" id="btnPreview">' +
                        '<i class="far fa-eye"></i>' +
                        '</a> |';
                    btnEdit = (isUpdate == true) ? btnEdit : "";
                    btnDelete = (isDelete == true) ? btnDelete : "";
                    btnPreview = (isView == true) ? btnPreview : "";
                    return '<div class="action-ic">' + btnPreview + ' ' + btnEdit + ' ' + btnCopy + '  ' + btnDelete + ' ' + btnMap + '</div>'
                }

            },


        ],
        "order": [[0, "asc"]],


    });

    

    //oTable = $('#tblOrderTakenFumigation').DataTable();
    //$("input[type='search']").keyup(function () {

    //    oTable.search(this.value);
    //    oTable.draw();
    //});

    var search_thread_tblOrderTakenFumigation = null;
    $("#tblOrderTakenFumigation_filter input")
        .unbind()
        .bind("input", function (e) {
            clearTimeout(search_thread_tblOrderTakenFumigation);
            search_thread_tblOrderTakenFumigation = setTimeout(function () {
                var dtable = $("#tblOrderTakenFumigation").dataTable().api();
                var elem = $("#tblOrderTakenFumigation_filter input");
                return dtable.search($(elem).val()).draw();
            }, 700);
        });

}
//#endregion


//#region DataTable Binding
var GetOtherFumigationList = function () {
    $('#tblOtherFumigation').DataTable({
        "bInfo": true,
        dom: 'rtip',
        //buttons: [
        //    {
        //        extend: 'print',
        //        orientation: 'landscape',
        //        pageSize: 'LEGAL',
        //        title: "",
        //        text: '<img src="../../Assets/images/printer.png" style="height:18px;margin-right: 5px;position: relative;top: 1px;width:16px;"/> Print',
        //        messageBottom: datetime,
        //        //messageTop: datetime,
        //        exportOptions: {
        //            columns: ':visible',
        //            stripHtml: false,
        //            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19,20,21]
        //        },
        //        customize: function (win) {

        //            var last = null;
        //            var current = null;
        //            var bod = [];

        //            var css = '@page { size: landscape;margin:10px;}  ',
        //                head = win.document.head || win.document.getElementsByTagName('head')[0],
        //                style = win.document.createElement('style');

        //            style.type = 'text/css';
        //            style.media = 'print';

        //            if (style.styleSheet) {
        //                style.styleSheet.cssText = css;
        //            }
        //            else {
        //                style.appendChild(win.document.createTextNode(css));
        //            }

        //            head.appendChild(style);
        //            $(win.document.body)
        //                .css('font-size', '10pt')
        //                .prepend(
        //                    "<table id='checkheader'><tr><td width='80%' ><h3 style='font-size: 20px;'>FUMIGATION IN PROGRESS</h3> </td><td width='20%'><div><img src='http://larastruckinglogistics-app.azurewebsites.net/Images/Laraslogo.png' height='100px'/></div></td></tr></table>"
        //                );
        //        }
        //    },
        //    ,
        //    {
        //        extend: 'colvis',
        //        text: '<img src="../../Assets/images/info.png" style="width:17px;;margin-top: -2px;height: 17px;"/> column visibility',
        //        columns: ':not(.noVis)',
        //    }
        //],

        select: 'single',
        "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
        //responsive: true,
        "scrollX": true,
        orderMulti: true,
        processing: true,
        serverSide: true,
        searching: false,
        bDestroy: true,
        stateSave: true,
        "language": {
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
        },
        "ajax": {
            "url": baseUrl + "/Fumigation/Fumigation/GetOtherFumigationList",
            "type": "POST",
            "datatype": "json",
            //"async": false,
        },
        columnDefs: [
            {

                "targets": [0],
                "className": 'noVis'
            }
        ],
        "columns": [
            {
                "data": "FumigationId",
                "name": "FumigationId",
                "autoWidth": true,
                "visible": false,
            },

            {
                "name": "StatusName",
                "autoWidth": true,

                "render": function (data, type, row, meta) {
                   // console.log("tblotherrow : ",row);
                    return StatusCheckForShipment(row.StatusName)
                }
            },
            {
                // "data": "PickUpDriver",
                "name": "ActFumigationRelease",
               // "orderable": false,
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    //return SameDates(row.ActFumigationRelease);

                    var isSame = false;
                    if (row.ActFumigationRelease != null && row.ActFumigationRelease != " ") {
                        var ActFumigationRelease = row.ActFumigationRelease.split("$")


                       

                            if (ActFumigationRelease.length > 0) {
                                var count = 0;
                                for (var i = 0; i < ActFumigationRelease.length; i++) {
                                    //  console.log("time11: " + ConvertDateNew(ActFumigationRelease[i], true));
                                    if (ActFumigationRelease[i] == ActFumigationRelease[0]) {
                                        count = count + 1;
                                    }


                                }
                                if (count == ActFumigationRelease.length) {
                                    isSame = true;
                                }
                                if (isSame) {
                                    var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                    var tabCon = '';
                                    var tabBottom = '</table>';
                                    var ftype = row.FumigationTypes.split('$');
                                    if (ftype.length > 1) {
                                        for (var i = 0; i < ftype.length; i++) {
                                            if (ActFumigationRelease[0] != "NA") {
                                                tabCon += '<tr><td><label class="dtclick" data-toggle="tooltip" data-placement="top" title="">' + ConvertSqlDateTimeNew(ActFumigationRelease[0], true) + '</label></td></tr>';
                                            }
                                            else {
                                                tabCon += '<tr><td><label class="dtclick" data-toggle="tooltip" data-placement="top" title="">NA</label></td></tr>';
                                            }

                                        }
                                        return tabTop + tabCon + tabBottom;
                                    }
                                    else {
                                        if (ActFumigationRelease[0] != "NA") {
                                            return '<label class="dtclick" data-toggle="tooltip" data-placement="top" title="">' + ConvertSqlDateTimeNew(ActFumigationRelease[0], true) + '</label>'
                                        }
                                        else {
                                            return '<label class="dtclick" data-toggle="tooltip" data-placement="top" title="">NA</label>'
                                        }
                                    }
                                    //  return '<label data-toggle="tooltip" data-placement="top">' + fumigationTypesList[0] + '</label>'
                                }
                                else {
                                    var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                    var tabCon = '';
                                    var tabBottom = '</table>';
                                    for (let i = 0; i < ActFumigationRelease.length; i++) {
                                        if (ActFumigationRelease[i] != "NA") {
                                            tabCon += '<tr><td><label class="dtclick" data-toggle="tooltip" data-placement="top">' + ConvertSqlDateTimeNew(ActFumigationRelease[i], true) + '</label></td></tr>';
                                        }
                                        else {
                                            tabCon += '<tr><td><label class="dtclick" data-toggle="tooltip" data-placement="top">NA</label></td></tr>';
                                        }
                                    }
                                    return tabTop + tabCon + tabBottom
                                }
                            }
                       


                    } else {
                        return 'NA'

                    }

                }
            },

            {
                "data": "FumigationTypes",
                "name": "FumigationTypes",
                "autoWidth": true,
                "width": "6%",
                "render": function (data, type, row, meta) {
                    var isSame = false;
                    if (row.FumigationTypes != null && row.FumigationTypes != '') {
                        var fumigationTypesList = row.FumigationTypes.split("$")
                        if (fumigationTypesList.length > 0) {
                            var count = 0;
                            for (var i = 0; i < fumigationTypesList.length; i++) {
                                if (fumigationTypesList[i] == fumigationTypesList[0]) {
                                    count = count + 1;
                                }
                            }
                            if (count == fumigationTypesList.length) {
                                isSame = true;
                            }
                            if (isSame) {
                                var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                var tabCon = '';
                                var tabBottom = '</table>';
                                var ftype = row.FumigationTypes.split('$');
                                if (ftype.length > 1) {
                                    for (var i = 0; i < ftype.length; i++) {
                                        tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(fumigationTypesList[0]) + '">' + GetCompanyNew(fumigationTypesList[0]) + '</label></td></tr>';
                                    }
                                    return tabTop + tabCon + tabBottom;
                                }
                                else {
                                    return '<label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(fumigationTypesList[0]) + '">' + GetCompanyNew(fumigationTypesList[0]) + '</label>'
                                }
                                //  return '<label data-toggle="tooltip" data-placement="top">' + fumigationTypesList[0] + '</label>'
                            }
                            else {
                                var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                var tabCon = '';
                                var tabBottom = '</table>';
                                for (let i = 0; i < fumigationTypesList.length; i++) {
                                    tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top">' + fumigationTypesList[i] + '</label></td></tr>'
                                }
                                return tabTop + tabCon + tabBottom
                            }
                        }

                    } else {
                        return 'NA'

                    }
                }
            },
            {
                "data": "CustomerName",
                "name": "CustomerName",
                "autoWidth": true,

            },
            {

                "name": "PickUpLocation",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    //
                    var isSame = false;
                    if (row.PickupLocation != null && row.PickupLocation != '') {
                        var pickupLocation = row.PickupLocation.split("|");
                        // console.log("pickupLocation: " + pickupLocation);
                        if (pickupLocation.length > 0) {
                            var count = 0;
                            for (var i = 0; i < pickupLocation.length; i++) {
                                if (pickupLocation[i] == pickupLocation[0]) {
                                    // console.log(pickupLocation[i] + " | " + pickupLocation[0]);
                                    count = count + 1;
                                }
                            }
                            if (count == pickupLocation.length) {
                                isSame = true;
                            }
                            if (isSame) {
                                var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                var tabCon = '';
                                var tabBottom = '</table>';
                                var ftype = row.FumigationTypes.split('$');
                                if (ftype.length > 1) {
                                    for (var i = 0; i < ftype.length; i++) {
                                        tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(pickupLocation[0]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(pickupLocation[0]) + '</label></td></tr>';
                                    }
                                    return tabTop + tabCon + tabBottom;
                                }
                                else {
                                    return '<label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(pickupLocation[0]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(pickupLocation[0]) + '</label>'
                                }

                                // return '<label data-toggle="tooltip" data-placement="top" title="' + GetCAddress(pickupLocation[0]) + '">' + GetCompany(pickupLocation[0]) + '</label><br/>'
                            }
                            else {
                                var lblPickupLocation = "";
                                if (pickupLocation.length > 0) {
                                    for (var i = 0; i < pickupLocation.length; i++) {
                                        var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                        var tabCon = '';
                                        var tabBottom = '</table>';

                                        lblPickupLocation += '<tr><td><label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(pickupLocation[i]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(pickupLocation[i]) + '</label></td></tr>'
                                    }
                                    // lblPickupLocation = lblPickupLocation.trim("<br/>");
                                    return tabTop + lblPickupLocation + tabBottom;
                                }
                            }
                        }
                    }
                    else {

                        return 'NA'
                    }

                }
            },
            {
                "name": "AWB",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                  
                    var isSame = false;
                    if (row.AWB != null && row.AWB != '') {
                        //var removesp = row.AWB_CP_CN.replaceAll(" ", ""); //Commented by DART to retain the space...
                        var removesp = row.AWB;
                        var awb = removesp.split(',');
                        if (awb.length > 0) {
                            var count = 0;
                            for (var i = 0; i < awb.length; i++) {
                                if (awb[i] == awb[0]) {
                                    count = count + 1;
                                }
                            }
                            if (count == awb.length) {
                                isSame = true;
                            }
                            if (isSame) {
                                var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                var tabCon = '';
                                var tabBottom = '</table>';
                                var ftype = row.FumigationTypes.split('$');
                                if (ftype.length > 1) {
                                    for (var i = 0; i < ftype.length; i++) {
                                        tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top">' + awb[0] + '</label></td></tr>';
                                    }
                                    return tabTop + tabCon + tabBottom;
                                }
                                else {
                                    return '<label data-toggle="tooltip" data-placement="top">' + awb[0] + '</label>'
                                }
                                // return '<label data-toggle="tooltip" data-placement="top">' + awb[0] + '</label>'
                            }
                            else {
                                var awbNo = "";
                                for (var i = 0; i < awb.length; i++) {
                                    var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';

                                    var tabBottom = '</table>';
                                    awbNo += '<tr><td><label data-toggle="tooltip" data-placement="top">' + awb[i] + '</label></td></tr>'
                                }
                                return tabTop + awbNo + tabBottom;

                            }
                        }
                    }
                    else {
                        return 'NA'
                    }
                }
            },
            {
                "name": "VendorNconsignee",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    // console.log("tblother: ",row);
                    if (row.VendorNConsignee != null && row.VendorNConsignee != '') {
                        var isSame = false;
                        var vendorNconsignee = row.VendorNConsignee.split("|")
                        //console.log("vendorNconsignee: ", vendorNconsignee.length);
                        if (vendorNconsignee.length > 0) {
                            var count = 0;
                            for (var i = 0; i < vendorNconsignee.length; i++) {
                                if (vendorNconsignee[i] == vendorNconsignee[0]) {
                                    count = count + 1;
                                }
                            }
                            if (count == vendorNconsignee.length) {
                                isSame = true;
                            }
                        }
                        if (isSame) {

                            var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            var tabCon = '';
                            var tabBottom = '</table>';
                            var ftype = row.FumigationTypes.split('$');
                            if (ftype.length > 1) {
                                for (var i = 0; i < ftype.length; i++) {
                                    tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top" title="' + vendorNconsignee[0] + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + vendorNconsignee[0] + '</label></td></tr>';
                                }
                                return tabTop + tabCon + tabBottom;
                            }
                            else {
                                // console.log("Isame true: " + isSame);
                                //  console.log("Isame true: " + isSame);
                                return '<label data-toggle="tooltip" data-placement="top" title="' + row.VendorNConsignee + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + row.VendorNConsignee + '</label>'
                            }
                            // return '<label data-toggle="tooltip" data-placement="top">' + vendorNconsignee[0] + '</label>'
                        }
                        else {

                            var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            var tabCon = '';
                            var tabBottom = '</table>';
                            for (var i = 0; i < vendorNconsignee.length; i++) {
                                tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + vendorNconsignee[i] + '</label></td></tr>'
                            }
                            return tabTop + tabCon + tabBottom
                        }

                    } else {
                        return 'NA'

                    }
                }

            },
            {
                //"data": "BoxCount",
                "name": "BoxCount",
                "autoWidth": true,
              //  "orderable": false,
                "render": function (data, type, row, meta) {

                    if (row.BoxCount != null && row.BoxCount != '') {
                        var BoxCount = row.BoxCount.split("|")

                        if (BoxCount.length > 1) {
                            var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var tabBottom = '</table>';
                            var lblPickupLocation = "";
                            for (var i = 0; i < BoxCount.length; i++) {
                                // console.log("vendorNconsignee: " + BoxCount[i]);
                                var boxcounts = BoxCount[i].split('.');
                                lblPickupLocation += '<tr><td><label data-toggle="tooltip" data-placement="top">' + boxcounts[0] + '</label></td></tr>';

                            }
                            return tabTop + lblPickupLocation + tabBottom
                        }

                        else {
                            var lblPickupLocation = "";
                            var boxcounts = row.BoxCount.split('.');
                            return '<label data-toggle="tooltip" data-placement="top">' + boxcounts[0] + '</label>';
                            //return lblPickupLocation

                        }

                    } else {
                        return 'NA'

                    }
                }
            },
            {
                //"data": "PalletCount",
                "name": "PalletCount",
                "autoWidth": true,
               // "orderable": false,
                "render": function (data, type, row, meta) {

                    if (row.PalletCount != null && row.PalletCount != '') {
                        var PalletCount = row.PalletCount.split("|")

                        if (PalletCount.length > 1) {
                            var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var tabBottom = '</table>';
                            var lblPickupLocation = "";
                            for (var i = 0; i < PalletCount.length; i++) {
                                // console.log("vendorNconsignee: " + BoxCount[i]);
                                var boxcounts = PalletCount[i].split('.');
                                lblPickupLocation += '<tr><td><label data-toggle="tooltip" data-placement="top">' + boxcounts[0] + '</label></td></tr>';

                            }
                            return tabTop + lblPickupLocation + tabBottom
                        }

                        else {
                            var lblPickupLocation = "";
                            var boxcounts = row.PalletCount.split('.');
                            return '<label data-toggle="tooltip" data-placement="top">' + boxcounts[0] + '</label>';
                            //return lblPickupLocation

                        }

                    } else {
                        return 'NA'

                    }
                }
            },
            {
                //"data": "TrailerPosition",
                "name": "TrailerPosition",
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    if (row.TrailerPosition != null && row.TrailerPosition != '') {
                        var TrailerPosition = row.TrailerPosition.split("|")

                        if (TrailerPosition.length > 1) {
                            var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var tabBottom = '</table>';
                            var lblPickupLocation = "";
                            for (var i = 0; i < TrailerPosition.length; i++) {
                                //console.log("vendorNconsignee: " + TrailerPosition[i]);
                                lblPickupLocation += '<tr><td><label data-toggle="tooltip" data-placement="top">' + TrailerPosition[i] + '</label></td></tr>';

                            }
                            return tabTop + lblPickupLocation + tabBottom
                        }

                        else {
                            var lblPickupLocation = "";
                            return '<label data-toggle="tooltip" data-placement="top">' + row.TrailerPosition + '</label>';
                            //return lblPickupLocation

                        }

                    } else {
                        return 'NA'

                    }
                }
            },
            {
                "name": "FumigationSite",
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    var isSame = false;
                    if (row.FumigationSite != null && row.FumigationSite != '') {
                        var fumigationSite = row.FumigationSite.split("|");
                        if (fumigationSite.length > 0) {
                            var count = 0;
                            for (var i = 0; i < fumigationSite.length; i++) {
                                if (fumigationSite[i] == fumigationSite[0]) {
                                    count = count + 1;
                                }
                            }
                            if (count == fumigationSite.length) {
                                isSame = true;
                            }
                        }
                        if (isSame) {
                            var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            var tabCon = '';
                            var tabBottom = '</table>';
                            var ftype = row.FumigationTypes.split('$');
                            if (ftype.length > 1) {
                                for (var i = 0; i < ftype.length; i++) {
                                    tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(fumigationSite[0]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(fumigationSite[0]) + '</label></td></tr>';
                                }
                                return tabTop + tabCon + tabBottom;
                            }
                            else {
                                return '<label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(fumigationSite[0]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(fumigationSite[0]) + '</label><br/>'
                            }
                            //return '<label data-toggle="tooltip" data-placement="top" title="' + GetCAddress(fumigationSite[0]) + '">' + GetCompany(fumigationSite[0]) + '</label><br/>'
                        }
                        else {
                            var lblfumigationSite = "";
                            if (fumigationSite.length > 0) {
                                var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                var tabCon = '';
                                var tabBottom = '</table>';
                                for (var i = 0; i < fumigationSite.length; i++) {
                                    lblfumigationSite += '<tr><td><label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(fumigationSite[i]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(fumigationSite[i]) + '</label></td></tr>'
                                }
                                // lblfumigationSite = lblfumigationSite.trim("<br/>");
                                return tabTop + lblfumigationSite + tabBottom;
                            }
                        }
                    }
                    else {

                        return 'NA'
                    }


                }

            },
            {
                "data": "PickUpEquipment",
                "name": "PickUpEquipment",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    //console.log("row: ",row);
                    var isSame = false;
                    if (row.PickUpEquipment != null && row.PickUpEquipment != '') {

                        var pickUpEquipmentList = row.PickUpEquipment.split("$")
                        if (pickUpEquipmentList.length > 0) {
                            var count = 0;
                            for (var i = 0; i < pickUpEquipmentList.length; i++) {
                                if (pickUpEquipmentList[i] == pickUpEquipmentList[0]) {
                                    count = count + 1;
                                }
                            }
                            if (count == pickUpEquipmentList.length) {
                                isSame = true;
                            }
                            if (isSame) {
                                var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                var tabCon = '';
                                var tabBottom = '</table>';
                                var ftype = row.FumigationTypes.split('$');
                                if (ftype.length > 1) {
                                    for (var i = 0; i < ftype.length; i++) {
                                        tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top">' + pickUpEquipmentList[0] + '</label></td></tr>';
                                    }
                                    return tabTop + tabCon + tabBottom;
                                }
                                else {

                                    return '<label data-toggle="tooltip" data-placement="top">' + pickUpEquipmentList[0] + '</label>'
                                }

                                // return '<label data-toggle="tooltip" data-placement="top">' + pickUpEquipmentList[0] + '</label>'
                            }
                            else {
                                var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                var tabCon = '';
                                var tabBottom = '</table>';
                                for (var i = 0; i < pickUpEquipmentList.length; i++) {
                                    tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top">' + pickUpEquipmentList[i] + '</label></td></tr>'
                                }
                                return tabTop + tabCon + tabBottom;
                            }
                        }
                    } else {
                        return 'NA'

                    }
                }
            },

            {
                "name": "PickUpDriver",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    if (row.PickUpDriver != null && row.PickUpDriver != '') {
                        var isSame = false;
                        var pickUpDriver = row.PickUpDriver.split("$")
                        if (pickUpDriver.length > 0) {
                            var count = 0;
                            for (var i = 0; i < pickUpDriver.length; i++) {
                                if (pickUpDriver[i] == pickUpDriver[0]) {
                                    count = count + 1;
                                }
                            }
                            if (count == pickUpDriver.length) {
                                isSame = true;
                            }
                            if (isSame) {

                                var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                var tabCon = '';
                                var tabBottom = '</table>';
                                var ftype = row.FumigationTypes.split('$');
                                if (ftype.length > 1) {
                                    for (var i = 0; i < ftype.length; i++) {
                                        tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top">' + pickUpDriver[0] + '</label></td></tr>';
                                    }
                                    return tabTop + tabCon + tabBottom;
                                }
                                else {

                                    return '<label data-toggle="tooltip" data-placement="top">' + pickUpDriver[0] + '</label>'
                                }

                                //return '<label data-toggle="tooltip" data-placement="top">' + pickUpDriver[0] + '</label>'
                            }
                            else {
                                var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                var tabBottom = '</table>';
                                var tabCon = '';
                                for (var i = 0; i < pickUpDriver.length; i++) {
                                    tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top">' + pickUpDriver[i] + '</label></td></tr>'
                                }
                                return tabTop + tabCon + tabBottom
                            }
                        }
                    } else {
                        return 'NA'

                    }
                }
            },
            {
                "name": "ActLoadingStart",
                "autoWidth": true,
              //  "orderable": false,
                "render": function (data, type, row, meta) {
                    //console.log("SameDates(row.ActLoadingStart): " + SameDates(row.ActLoadingStart));
                    var isSame = false;

                    if (row.ActLoadingStart != null && row.ActLoadingStart != " ") {
                        var ActLoadingStart = row.ActLoadingStart.split("$")
                       
                            if (ActLoadingStart.length > 0) {
                                var count = 0;
                                for (var i = 0; i < ActLoadingStart.length; i++) {
                                    if (ActLoadingStart[i] == ActLoadingStart[0]) {
                                        count = count + 1;
                                    }
                                }
                                if (count == ActLoadingStart.length) {
                                    isSame = true;
                                }
                                if (isSame) {
                                    var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                    var tabCon = '';
                                    var tabBottom = '</table>';
                                    var ftype = row.FumigationTypes.split('$');
                                    if (ftype.length > 1) {
                                        for (var i = 0; i < ftype.length; i++) {
                                            if (ActLoadingStart[0] != "NA") {
                                                tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top" title="">' + ConvertSqlDateTimeNew(ActLoadingStart[0], true) + '</label></td></tr>';
                                            }
                                            else {
                                                tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top" title="">NA</label></td></tr>';
                                            }
                                        }
                                        return tabTop + tabCon + tabBottom;
                                    }
                                    else {
                                        if (ActLoadingStart[0] != "NA") {
                                            return '<label data-toggle="tooltip" data-placement="top" title="">' + ConvertSqlDateTimeNew(ActLoadingStart[0], true) + '</label>';
                                        }
                                        else {
                                            return '<label data-toggle="tooltip" data-placement="top" title="">NA</label>';
                                        }
                                    }
                                    //  return '<label data-toggle="tooltip" data-placement="top">' + fumigationTypesList[0] + '</label>'
                                }
                                else {
                                    var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                    var tabCon = '';
                                    var tabBottom = '</table>';
                                    for (let i = 0; i < ActLoadingStart.length; i++) {
                                        if (ActLoadingStart[i] != "NA") {
                                            tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top">' + ConvertSqlDateTimeNew(ActLoadingStart[i], true) + '</label></td></tr>';
                                        }
                                        else {
                                            tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top">NA</label></td></tr>';
                                        }
                                    }
                                    return tabTop + tabCon + tabBottom
                                }
                            }
                     

                    } else {
                        return 'NA'

                    }

                    // return SameDates(row.ActLoadingStart);

                }
            },
            {
                "name": "ActLoadingFinish",
                "autoWidth": true,
              //  "orderable": false,
                "render": function (data, type, row, meta) {
                    // console.log("ActLoadingFinish ", ConvertDateNew(ActLoadingFinish[0], true))
                    var isSame = false;

                    if (row.ActLoadingFinish != null && row.ActLoadingFinish != " ") {
                        var ActLoadingFinish = row.ActLoadingFinish.split("$")
                       
                            if (ActLoadingFinish.length > 0) {
                                var count = 0;
                                for (var i = 0; i < ActLoadingFinish.length; i++) {
                                    if (ActLoadingFinish[i] == ActLoadingFinish[0]) {
                                        count = count + 1;
                                    }
                                }
                                if (count == ActLoadingFinish.length) {
                                    isSame = true;
                                }
                                if (isSame) {
                                    var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                    var tabCon = '';
                                    var tabBottom = '</table>';
                                    var ftype = row.FumigationTypes.split('$');
                                    if (ftype.length > 1) {
                                        for (var i = 0; i < ftype.length; i++) {
                                            if (ActLoadingFinish[0]!= "NA") {
                                                tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top" title="">' + ConvertSqlDateTimeNew(ActLoadingFinish[0], true) + '</label></td></tr>';
                                            }
                                            else {
                                                tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top" title="">NA</label></td></tr>';
                                            }
                                        }
                                        return tabTop + tabCon + tabBottom;
                                    }
                                    else {
                                        if (ActLoadingFinish[0] != "NA") {
                                            return '<label data-toggle="tooltip" data-placement="top" title="">' + ConvertSqlDateTimeNew(ActLoadingFinish[0], true) + '</label>';
                                        }
                                        else {
                                            return '<label data-toggle="tooltip" data-placement="top" title="">NA</label>';
                                        }
                                    }
                                    //  return '<label data-toggle="tooltip" data-placement="top">' + fumigationTypesList[0] + '</label>'
                                }
                                else {
                                    var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                    var tabCon = '';
                                    var tabBottom = '</table>';
                                    for (let i = 0; i < ActLoadingFinish.length; i++) {
                                        if (ActLoadingFinish[i] != "NA") {
                                            tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top">' + ConvertSqlDateTimeNew(ActLoadingFinish[i], true) + '</label></td></tr>';
                                        }
                                        else {
                                            tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top">NA</label></td></tr>';
                                        }
                                    }
                                    return tabTop + tabCon + tabBottom
                                }
                            }
                        

                    } else {
                        return 'NA'

                    }


                    //return SameDates(row.ActLoadingFinish);

                }
            },
            //{
            //    "name": "RequestedAt",
            //    "autoWidth": true,
            //    "orderable": false,
            //    "render": function (data, type, row, meta) {
            //        // console.log("ConvertSqlDateTime(row.RequestedAt): " + ConvertSqlDateTimeNew(row.RequestedAt));
            //        // var request = ConvertSqlDateTime(row.RequestedAt);
            //        // var newdate = new Date(request);
            //        // var requesteddate = ConvertDateNew("11-05-2021 05:30", true);
            //        // console.log("requesteddatetype: " + typeof (newdate));
            //        // console.log("requesteddate: " + requesteddate);
            //        return '<label data-toggle="tooltip" data-placement="top">' + ConvertSqlDateTimeNew(row.RequestedAt) + '</label>'
            //        //return '<label data-toggle="tooltip" data-placement="top">' + ConvertDateNew(row.RequestedAt,true) + '</label>'
            //    }
            //},

            {
                "name": "Temperature",
             //   "orderable": false,
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    if (row.Temperature != null && row.Temperature != '') {
                        var isSame = false;

                        var temperature = row.Temperature.split("|")
                        //console.log("temp: " + temperature.length);

                        if (temperature.length > 0) {
                            var count = 0;
                            for (var i = 0; i < temperature.length; i++) {
                                if (temperature[i] == temperature[0]) {
                                    count = count + 1;
                                }
                            }
                            if (count == temperature.length) {
                                isSame = true;
                            }
                        }



                        if (isSame) {
                            var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            var tabCon = '';
                            var tabBottom = '</table>';
                            var ftype = row.FumigationTypes.split('$');
                            if (ftype.length > 1) {
                                for (var i = 0; i < ftype.length; i++) {
                                    tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top">' + temperature[i] + '</label></td></tr>';
                                }
                                return tabTop + tabCon + tabBottom;
                            }
                            else {

                                return '<label>' + temperature[0] + '</label>'
                            }
                            //return '<label>' + temperature[0] + '</label>'
                        }
                        else {
                            var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            var tabCon = '';
                            var tabBottom = '</table>';
                            for (var i = 0; i < temperature.length; i++) {


                                tabCon += '<tr><td><label>' + temperature[i] + '</label></td></tr>';
                            }
                            return tabTop + tabCon + tabBottom
                        }

                    } else {
                        return 'NA'

                    }


                }
            },
            {
                "data": "DeliveryEquipment",
                "name": "DeliveryEquipment",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    //console.log("row: ",row);
                    var isSame = false;
                    if (row.DeliveryEquipment != null && row.DeliveryEquipment != '') {

                        var deliveryEquipment = row.DeliveryEquipment.split("$")
                        if (deliveryEquipment.length > 0) {
                            var count = 0;
                            for (var i = 0; i < deliveryEquipment.length; i++) {
                                if (deliveryEquipment[i] == deliveryEquipment[0]) {
                                    count = count + 1;
                                }
                            }
                            if (count == deliveryEquipment.length) {
                                isSame = true;
                            }
                        }


                        if (isSame) {
                            var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            var tabCon = '';
                            var tabBottom = '</table>';
                            var ftype = row.FumigationTypes.split('$');
                            if (ftype.length > 1) {
                                for (var i = 0; i < ftype.length; i++) {
                                    tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top">' + deliveryEquipment[0] + '</label></td></tr>';
                                }
                                return tabTop + tabCon + tabBottom;
                            }
                            else {

                                return '<label data-toggle="tooltip" data-placement="top">' + deliveryEquipment[0] + '</label>'
                            }
                            // return '<label data-toggle="tooltip" data-placement="top">' + deliveryEquipment[0] + '</label>'
                        }
                        else {
                            var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            var tabBottom = '</table>';
                            var tabCon = '';
                            for (var i = 0; i < deliveryEquipment.length; i++) {
                                tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top">' + deliveryEquipment[i] + '</label></td></tr>'
                            }
                            return tabTop + tabCon + tabBottom

                        }

                    } else {
                        return 'NA'

                    }
                }
            },

            //{
            //    // "data": "PickUpDriver",
            //    "name": "ActFumigationIn",
            //    "orderable": false,
            //    "autoWidth": true,
            //    "render": function (data, type, row, meta) {

            //        return SameDates(row.ActFumigationIn);

            //    }
            //},

            //{
            //    // "data": "PickUpDriver",
            //    "name": "ActDepartureDate",
            //    "orderable": false,
            //    "autoWidth": true,
            //    "render": function (data, type, row, meta) {

            //        return SameDates(row.ActDepartureDate);

            //    }
            //},

            {
                // "data": "DeliveryDriver",
                "name": "DeliveryDriver",
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    if (row.DeliveryDriver != null && row.DeliveryDriver != '') {

                        var isSame = false;

                        var deliveryDriver = row.DeliveryDriver.split("$")
                        if (deliveryDriver.length > 0) {
                            var count = 0;
                            for (var i = 0; i < deliveryDriver.length; i++) {
                                if (deliveryDriver[i] == deliveryDriver[0]) {
                                    count = count + 1;
                                }
                            }
                            if (count == deliveryDriver.length) {
                                isSame = true;
                            }
                        }


                        if (isSame) {
                            var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            var tabCon = '';
                            var tabBottom = '</table>';
                            var ftype = row.FumigationTypes.split('$');
                            if (ftype.length > 1) {
                                for (var i = 0; i < ftype.length; i++) {
                                    tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top">' + deliveryDriver[0] + '</label></td></tr>';
                                }
                                return tabTop + tabCon + tabBottom;
                            }
                            else {

                                return '<label data-toggle="tooltip" data-placement="top">' + deliveryDriver[0] + '</label>'
                            }
                            // return '<label data-toggle="tooltip" data-placement="top">' + deliveryDriver[0] + '</label>'
                        }
                        else {
                            var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            var tabBottom = '</table>';
                            var tabCon = '';
                            for (var i = 0; i < deliveryDriver.length; i++) {
                                tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top">' + deliveryDriver[i] + '</label></td></tr>'
                            }
                            return tabTop + tabCon + tabBottom
                        }


                    } else {
                        return 'NA'

                    }
                }
            },


            {
                "name": "DeliveryLocation",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    var isSame = false;
                    if (row.DeliveryLocation != null && row.DeliveryLocation != '') {
                        var deliveryLocation = row.DeliveryLocation.split("|");

                        if (deliveryLocation.length > 0) {
                            var count = 0;
                            for (var i = 0; i < deliveryLocation.length; i++) {
                                if (deliveryLocation[i] == deliveryLocation[0]) {
                                    count = count + 1;
                                }
                            }
                            if (count == deliveryLocation.length) {
                                isSame = true;
                            }

                            if (isSame) {
                                var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                var tabCon = '';
                                var tabBottom = '</table>';
                                var ftype = row.FumigationTypes.split('$');
                                if (ftype.length > 1) {
                                    for (var i = 0; i < ftype.length; i++) {
                                        tabCon += '<tr><td><label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(deliveryLocation[0]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(deliveryLocation[0]) + '</label></td></tr>';
                                    }
                                    return tabTop + tabCon + tabBottom;
                                }
                                else {

                                    return '<label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(deliveryLocation[0]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(deliveryLocation[0]) + '</label>'
                                }
                                // return '<label data-toggle="tooltip" data-placement="top" title="' + GetCAddress(deliveryLocation[0]) + '">' + GetCompany(deliveryLocation[0]) + '</label><br/>'
                            }
                            else {
                                var lbldeliveryLocation = "";
                                if (deliveryLocation.length > 0) {
                                    var tabTop = '<table class="table table-bordered" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                    var tabCon = '';
                                    var tabBottom = '</table>';
                                    for (var i = 0; i < deliveryLocation.length; i++) {
                                        lbldeliveryLocation += '<tr><td><label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(deliveryLocation[i]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(deliveryLocation[i]) + '</label></td></tr>'
                                    }
                                    //lbldeliveryLocation = lbldeliveryLocation.trim("<br/>");
                                    return tabTop + lbldeliveryLocation + tabBottom;
                                }
                            }
                        }
                    }

                    else {
                        return 'NA'

                    }
                }
            },


            {
                "name": "WT",
             //   "orderable": false,
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                   // console.log("row.WT: ", row.FumigationId);
                    if (row.WTReady) {
                        return '<input sy type="checkbox" checked="' + row.WTReady + '" onchange="FumigationWTReady(' + row.FumigationId + ',this)" >';
                    }
                    else {

                        return '<input type="checkbox" onchange="FumigationWTReady(' + row.FumigationId + ',this)" >';
                    }


                }
            },
            {
                "name": "ST",
              //  "orderable": false,
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    if (row.STReady) {
                        return '<input sy type="checkbox" checked="' + row.STReady + '" onchange="FumigationSTReady(' + row.FumigationId + ',this)" >';
                    }
                    else {

                        return '<input type="checkbox" onchange="FumigationSTReady(' + row.FumigationId + ',this)" >';
                    }


                }
            },

            //{
            //    "name": "ActDeliveryArrival",
            //    "orderable": false,
            //    "autoWidth": true,
            //    "render": function (data, type, row, meta) {
            //        return SameDates(row.ActDeliveryArrival);

            //    }
            //},
            //{
            //    "name": "ActDeliveryDeparture",
            //    "orderable": false,
            //    "autoWidth": true,
            //    "render": function (data, type, row, meta) {

            //        return SameDates(row.ActDeliveryDeparture);

            //    }
            //},



            {
                "name": "Action",
                "orderable": false,
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    var btnEdit = '<a href="' + baseUrl + '/Fumigation/Fumigation/Index/' + row.FumigationId + '" data-toggle="tooltip" title="Edit" class="edit_icon">' +
                        '<i class="far fa-edit"></i>' +
                        '</a>';
                    var btnMap = '| <a href="javascript: void(0)" class="Map_icon" data-toggle="tooltip" id="redirectButton" title="Map" onclick="javascript:fn_RedirectToGpsTracker(' + row.FumigationId + ');" >' +
                        '<i class="fas fa-map-marked-alt"></i>' +
                        '</a>';
                    var btnCopy = ' | <a href="javascript: void(0)" class="edit_icon" data-toggle="tooltip" title="Copy Shipment" onclick="javascript:CopyFumigation(' + row.FumigationId + ');" >' +
                        '<i class="far fa-clone"></i>' +
                        '</a>';
                    var btnDelete = ' | <a href="javascript: void(0)" class="delete_icon" data-toggle="tooltip" title="Delete" onclick="javascript:DeleteFumigation(' + row.FumigationId + ');" >' +
                        '<i class="far fa-trash-alt"></i>' +
                        '</a>';
                    var btnPreview = '<a href="' + baseUrl + '/Fumigation/Fumigation/ViewFumigationNotification/' + row.FumigationId + '" title="Preview Quote" target="_blank" id="btnPreview">' +
                        '<i class="far fa-eye"></i>' +
                        '</a> |';
                    var needApprovment = '<a href="' + baseUrl + '/Fumigation/Fumigation/Index/' + row.FumigationId + '" title="Notification" style="color:red;" class="delete_icon" target="_blank" id="btnPreview">' +
                        '<i class="fa fa-bell" aria-hidden="true"></i>' +
                        '</a> |';
                    var noNeedApprovment = '<a href="' + baseUrl + '/Fumigation/Fumigation/Index/' + row.FumigationId + '" title="Notification"  target="_blank" id="btnPreview">' +
                        '<i class="fa fa-bell" aria-hidden="true"></i>' +
                        '</a> |';


                    btnEdit = (isUpdate == true) ? btnEdit : "";
                    btnDelete = (isDelete == true) ? btnDelete : "";
                    btnPreview = (isView == true) ? btnPreview : "";
                    var displayBell = row.ApproveStatus == 1 ? noNeedApprovment : needApprovment;

                    return '<div class="action-ic">' + btnPreview + ' ' + displayBell + ' ' + btnEdit + ' ' + btnCopy + '  ' + btnDelete + ' ' + btnMap + '</div>'
                }

            }

        ],
        "order": [[0, "desc"]],
    });

    //oTable = $('#tblOtherFumigation').DataTable();
    //$("input[type='search']").keyup(function () {

    //    oTable.search(this.value);
    //    oTable.draw();
    //});
    var search_thread_tblOtherFumigation = null;
    $("#tblOtherFumigation_filter input")
        .unbind()
        .bind("input", function (e) {
            clearTimeout(search_thread_tblOtherFumigation);
            search_thread_tblOtherFumigation = setTimeout(function () {
                var dtable = $("#tblOtherFumigation").dataTable().api();
                var elem = $("#tblOtherFumigation_filter input");
                return dtable.search($(elem).val()).draw();
            }, 700);
        });

}
//#endregion
function ClearCopyFumigation() {
    //var $select = $('#ddlCustomerCopy').selectize();
    //$select[0].selectize.destroy();
    //var ddlCustomer = "<option selected='selected' value=" + 0 + ">SEARCH CUSTOMER</option>";
    //$("#ddlCustomerCopy").empty();
    //$("#ddlCustomerCopy").append(ddlCustomer);
    //$(".ddlCustomerCopy").text("SEARCH CUSTOMER");


    //$("#ddlPickupDate").val("");
    //$("#ddlDeliveryDate").val("");
    //$("#txtAirWayBill").val("");
    //$("#hdnShipmentId").val(0);
}

function SameDates(fieldData) {
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
                return '<label>' + ConvertSqlDateTimeNew(fieldList[0], true) + '</label>'
            }
            else {
                var fields = "";
                for (var i = 0; i < fieldList.length; i++) {
                    fields += '<label>' + ConvertSqlDateTimeNew(fieldList[i], true) + '</label><br/>'
                }
                return fields;
            }
        }
    }
    else {
        return '<label></label>'
    }

}

function GetCompanyNew(fullAddress) {

    var fullAddress = fullAddress;
    var lastIndex = fullAddress.lastIndexOf("$")
    var companyName = fullAddress.substring(0, lastIndex);
    var address = fullAddress.substring(lastIndex + 2);
    if (lastIndex > 0) {
        return companyName;
    }
    else {
        return fullAddress;
    }

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

function GetCAddressNew(fullAddress) {
    var fullAddress = fullAddress;
    var lastIndex = fullAddress.lastIndexOf("$")
    var companyName = fullAddress.substring(0, lastIndex);
    //console.log("companyName: " + companyName);
    var address = fullAddress.substring(lastIndex + 2);
    //console.log("address: " + address);
    if (lastIndex > 0) {
        return address;
    }
    else {
        return fullAddress;
    }
}

function GetCAddress(fullAddress) {
    var fullAddress = fullAddress;
    var lastIndex = fullAddress.lastIndexOf("||")
    var companyName = fullAddress.substring(0, lastIndex);
    //console.log("companyName: " + companyName);
    var address = fullAddress.substring(lastIndex + 2);
    //console.log("address: " + address);
    if (lastIndex > 0) {
        return address;
    }
    else {
        return fullAddress;
    }
}

function CopyFumigation(FumigationId) {
    $.confirm({
        title: 'Confirmation!',
        content: '<b>Are you sure you want to copy this fumigation?</b> ',
        type: 'blue',
        typeAnimated: true,
        buttons: {
            Copy: {
                btnClass: 'btn-blue',
                action: function () {
                    $("#fumigationModal").modal("show");
                    $('#fumigationModal').draggable();

                    ClearCopyFumigation();
                    $.ajax({
                        url: baseUrl + 'Fumigation/Fumigation/GetFumigationDetailById',
                        data: { "fumigationId": FumigationId },
                        type: "Post",

                        success: function (data) {
                            if (data != null) {


                                var ddlCustomer = "<option selected='selected' value=" + data.CustomerId + ">" + data.CustomerName + "</option>";
                                $("#ddlCustomerCopy").empty();
                                $("#ddlCustomerCopy").append(ddlCustomer);
                                $(".ddlCustomerCopy").text(data.CustomerName);

                                //$("#txtAirWayBill").val(data.AWB);
                                $("#dtReqLoading").val(ConvertDateEdit(data.RequestedLoading, true));
                                $("#dtEstFumIn").val(ConvertDateEdit(data.FumigationIn, true));
                                $("#dtRelease").val(ConvertDateEdit(data.FumigatiionRelease, true));
                                $("#dtDelEstArrival").val(ConvertDateEdit(data.DelEstArrival, true));

                                $("#hdnFumigationId").val(data.FumigationId);

                                bindCustomerDropdownCopy();
                            }
                            $("#fumigationModal").modal("show");
                            $('#fumigationModal').draggable();

                        },
                        error: function () {

                            alert();
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

function ValidateCopyFumigation() {

    var isValid = true;
    var message = "";

    var customerId = $("#ddlCustomerCopy").val();
    if (customerId > 0) {
        isValid = true;
    }
    else {
        // toastr.warning("Please select a customer.");
        AlertPopup("Please select a customer.");

        isValid = false;
    }

    var requestedLoading = $("#dtReqLoading").val();
    var fumigationIn = $("#dtEstFumIn").val();
    var fumigatiionRelease = $("#dtRelease").val();
    var delEstArrival = $("#dtDelEstArrival").val();



    var todayDate = new Date();
    var month = todayDate.getMonth() + 1;
    var day = todayDate.getDate() - 1;

    var yesterday = "";
    yesterday = (month < 10 ? '0' : '') + month + '-' +
        (day < 10 ? '0' : '') + day + '-' +
        todayDate.getFullYear();

    //yesterday = new Date(Date.parse(todayDate));

    //if (isValid && requestedLoading != "" && requestedLoading < yesterday) {
    //    // toastr.warning("The Req. Loading should be greater than, or equal to, yesterday's date.");
    //    AlertPopup("The Req. Loading should be greater than, or equal to, yesterday's date.");
    //    isValid = false;
    //    return isValid
    //}


    //if (isValid && requestedLoading != "" && fumigationIn != "") {
    //    if (new Date(requestedLoading) <= new Date(fumigationIn)) {

    //    }
    //    else {
    //        //toastr.warning("The Est. Fum. In should be greater than the Req. Loading.");
    //        AlertPopup("The Est. Fum. In should be greater than the Req. Loading.");
    //        isValid = false;
    //        return isValid
    //    }

    //}

    //if (isValid && fumigationIn != "" && fumigatiionRelease != "") {
    //    if (new Date(fumigationIn) <= new Date(fumigatiionRelease)) {

    //    }
    //    else {
    //        // toastr.warning("Est. Fum. Release should be greater than Est. Fum. In.");
    //        AlertPopup("Est. Fum. Release should be greater than Est. Fum. In.");
    //        isValid = false;
    //        return isValid
    //    }

    //}

    //if (isValid && fumigatiionRelease != "" && delEstArrival != "") {
    //    if (new Date(fumigatiionRelease) <= new Date(delEstArrival)) {

    //    }
    //    else {
    //        //toastr.warning("Del. Est. Arrival should be greater than Est. Fum. Release.");
    //        AlertPopup("Del. Est. Arrival should be greater than Est. Fum. Release.");
    //        isValid = false;
    //        return isValid
    //    }

    //}
    return isValid;
}

$("#btnCopy").click(function () {

    if (ValidateCopyFumigation()) {

        var values = {};
        values.FumigationId = $("#hdnFumigationId").val();
        values.CustomerId = $("#ddlCustomerCopy").val();
        values.AWB = $("#txtAirWayBill").val();
        values.RequestedLoading = $("#dtReqLoading").val();
        values.FumigationIn = $("#dtEstFumIn").val();
        values.FumigatiionRelease = $("#dtRelease").val();
        values.DelEstArrival = $("#dtDelEstArrival").val();

        $.ajax({
            url: baseUrl + 'Fumigation/Fumigation/SaveCopyFumigatonDetail',
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
                            content: '<b>Fumigation successfully copied.</b>',
                            type: 'green',
                            typeAnimated: true,
                            buttons: {
                                Ok: {
                                    btnClass: 'btn-green',
                                    action: function () {
                                        window.location.href = baseUrl + "/Fumigation/Fumigation/ViewFumigationList";
                                    }
                                },
                            }
                        });
                    }
                    else {

                    }
                    $('#fumigationModal').modal('toggle');
                }

                GetFumigationList();
            }
        });


    }
})

$("#btnCancel").click(function () {
    //window.location.href = baseUrl + "/Fumigation/Fumigation/ViewFumigationList";
    $('#fumigationModal').modal('toggle');
});

//#region function for apply selectize on customer dropdown
var bindCustomerDropdownCopy = function () {

    var $select = $('#ddlCustomerCopy').selectize();
    $select[0].selectize.destroy();

    $('#ddlCustomerCopy').selectize({
        sortField: 'text',
        maxItems: 1,
        valueField: 'id',
        labelField: 'text',
        searchField: 'text',
        plugins: ['restore_on_backspace'],
        closeAfterSelect: false,
        selectOnTab: true,
        allowEmptyOption: true,
        options: [],
        load: function (query, callback) {
            if (!query.length) return callback();
            $.ajax({
                url: baseUrl + "Customer/GetAllCustomer/?searchText=" + query,
                type: 'GET',
                dataType: 'json',
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

        onFocus: function () {

            var value = this.getValue();
            this.clear(true);
            this.unlock();
        }

    });
}
//#endregion

function SplitString(text, count, insertDots) {
    return text.slice(0, count) + (((text.length > count) && insertDots) ? "..." : "");
}
//#region Detail  Data Delete
function DeleteFumigation(listId) {

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
                        url: baseUrl + '/Fumigation/Fumigation/DeleteFumigation',
                        data: { 'fumigationId': listId },
                        type: "GET",
                        success: function (data) {

                            if (data.IsSuccess == true) {
                                // toastr.success(data.Message, "")
                                SuccessPopup(data.Message)
                            }
                            else {
                                // toastr.error(data.Message, "")
                                AlertPopup(data.Message, "")
                            }
                            GetOrderTakenFumigationList();
                            GetOtherFumigationList();
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

function FumigationWTReady(fumigationId, event) {

    var isReady = $(event).is(":checked");
    var model = {};
    model.fumigationId = JSON.parse(fumigationId);

    model.ready = JSON.parse(isReady);
    $.ajax({
        url: baseUrl + 'Fumigation/Fumigation/FumigationWTReady',
        type: "POST",
        data: JSON.stringify(model),
        async: false,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data && isReady) {
                //SuccessPopup("Extended Waiting Time advised to Customer!")
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

function FumigationSTReady(fumigationId, event) {

    var isReady = $(event).is(":checked");
    var model = {};
    model.fumigationId = JSON.parse(fumigationId);
    model.ready = JSON.parse(isReady);
    $.ajax({
        url: baseUrl + 'Fumigation/Fumigation/FumigationSTReady',
        type: "POST",
        data: JSON.stringify(model),
        async: false,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data && isReady) {
                //SuccessPopup("Fumigation in Storage!")
                $.alert({
                    title: 'Success!',
                    content: "<b>Fumigation in Storage!</b>",
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
                //SuccessPopup("Fumigation is not in Storage!")
                $.alert({
                    title: 'Success!',
                    content: "<b>Fumigation is not in Storage!</b>",
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

function DeleteComments(listId,news) {

    var table = $('#tblShipmentDetails').DataTable();
 

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

                    $.ajax({
                        url: baseUrl + '/Fumigation/Fumigation/DeleteComments',
                        data: { 'FumigationId': listId },
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

                                               // $('#tblOrderTakenFumigation').DataTable().clear().destroy();
                                                // $('#tblShipmentDetails2').DataTable().clear().destroy();
                                                //GetOrderTakenFumigationList();
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

//#endregion//#region Redirect to Gps Tracker
var fn_RedirectToGpsTracker = function (FumigationId) {
    window.open(baseUrl + '/GpsTracker/GpsTracker/FumigationIndex/' + FumigationId);
}
//#endregion

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
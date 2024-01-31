//#region Ready State
$(function () {
    binddate();
    BindDriver();
    $("#tblFumigationList").floatingScroll();
    GetFumigationList();

    $("#tblFumigationList").css({ "display": "block", "overflow": "auto" });
    //$('#tblFumigationList').DataTable().draw();
    //$("input[type='search']").val("");
    //$('#tblFumigationList').DataTable().search('').columns().search('').draw();

    bindCustomerDropdown();
    startEndDate();
    shipmentStatus();
    btnViewFumigation();
});
//#endregion
btnViewFumigation = function () {
    $("#btnViewFumigaion").on("click", function () {
        GetFumigationList();
		//$("#tblFumigationList").DataTable().draw();
	
    });
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
//#endregion//#region function for apply selectize on customer dropdown
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
//#endregion//#region bind driver
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
            data = data.filter(x => (x.StatusId == 8 || x.StatusId == 11 || x.StatusId == 9 || x.StatusId == 13 || x.StatusId == 7));
            for (var i = 0; i < data.length; i++) {
               // console.log("status: " + data[i].StatusName);
                ddlValue += '<option value="' + data[i].StatusId + '">' + data[i].StatusName + '</option>';
            }
            $("#ddlStatus").append(ddlValue);

        }
    });
}
//#endregion
$('#tblFumigationList').on('dblclick', 'tbody tr', function () {
    var table = $('#tblFumigationList').DataTable();
    var data_row = table.row($(this).closest('tr')).data();
    window.location.href = baseUrl + '/Fumigation/Fumigation/Index/' + data_row.FumigationId;
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

//#region DataTable Binding
var GetFumigationList = function () {
    $('#tblFumigationList').DataTable().clear().destroy();
    //$("input[type='search']").val("");
    
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
   // $("#dtStartedDate").css('display', 'none');
    //$(".dtStartedDate").css('display', 'none');
   
    var values = {};
    values.StartDate = $("#dtStartedDate").val();
    values.EndDate = $("#dtEndDate").val();
    values.CustomerId = $("#ddlCustomer").val();
    //values.IsOrderTaken = true;
    values.FreightTypeId = $("#ddlFreightType").val();
    values.StatusId = $("#ddlStatus").val();
    values.StatusName = $("#ddlStatus option:selected").text();
   // console.log("Status Id: " + values.StatusId);
   // console.log("values: " ,values);
    //console.log("Status Name: " + values.StatusName);
    if (values.StatusName == "SELECT STATUS") {
        values.StatusName = "ALL FUMIGATIONS";

    }
    if ($("#ddlDriver").val() > 0) {
        values.DriverName = $("#ddlDriver option:selected").text();
    }
    if ($("#ddlStatus").val() > 0) {
        values.StatusName = $("#ddlStatus option:selected").text();
    }
    
    

    $('#tblFumigationList').DataTable({
        "bInfo": true,
        dom: 'Blfrtip',
        buttons: [
            {
                text: '<i class="fa fa-arrow-circle-left" aria-hidden="true"></i>&nbsp;GO BACK',
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
              //  orientation: 'portrait',
                //messageTop: datetime,
                exportOptions: {
                    columns: ':visible',
                    stripHtml: false,
                    columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13,14,15, 16, 17, 18, 19,20,21]
                },
                customize: function (win) {

                    var last = null;
                    var current = null;
                    var bod = [];
                    
                    //console.log("Selected Status: "+status);
                    //if (stauts == "SELECT STATUS") {
                   // //    status = "COMPLETED";
                   /// }
                    win.pageMargins = [10, 10, 10, 10];
                    var stauts = $('#ddlStatus option:selected').text();
                    var css = '@page { size: landscape;margin:0 10px;}table tbody tr:hover td{background:none;}  .table-striped tbody tr:nth-of-type(2n+1) .table td{background:#f2f2f2 !important;border:none;} .table tr{background:#f2f2f2 !important;}',
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
                        .css('font-size', '8pt')
                        .prepend(
                            "<table style='text-transform:capitalize' id='checkheader'><tr><td width='80%' style='text-transform:capitalize;font-size:13px;'><br/><b>FUMIGATION REPORT : " + values.StatusName + "</b>  <b> " + ddlCustomer + "&nbsp;&nbsp;   Date Range: " + startDate + " to " + endDate + "<br/> " + freightType + "</b> </td><td width='20%'><div><img src='"+baseUrl+"/Images/Laraslogo.png' height='100px'/></div></td></tr></table>"
                    );

                    $(win.document.body).find('table')
                        .addClass('compact')
                        .css('font-size', 'inherit');
                }
            },
            {
                extend: 'colvis',
                text: '<img src="../../Assets/images/info.png" style="width:17px;;margin-top: -2px;height: 17px;"/> <label style="margin-bottom:-2px;">column visibility</label>',
                columns: ':not(.noVis)',
            }
        ],

        select: 'single',
        "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
        //responsive: true,
        //"scrollX": true,
        orderMulti:true,
        processing: true,
        serverSide: true,
		searching: true,
        bDestroy: true,
        stateSave: true,
        "language": {
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
        },
        "ajax": {
            "url": baseUrl + "/Fumigation/Fumigation/ViewAllFumigation",
            "type": "POST",
            "data": values,
            "datatype": "json"
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
                    return StatusCheckForShipment(row.StatusName)
                }
            },
            {

                "name": "PickUpArrival",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    console.log("row: ",row);
                    var isSame = false;
                    if (row.PickUpArrival != null && row.PickUpArrival != '') {
                        var PickUpArrival = row.PickUpArrival.split("|")
                        if (PickUpArrival.length > 0) {
                            var count = 0;
                            for (var i = 0; i < PickUpArrival.length; i++) {
                                if (PickUpArrival[i] == PickUpArrival[0]) {
                                    count = count + 1;
                                }
                            }
                            if (count == PickUpArrival.length) {
                                isSame = true;
                            }
                            if (isSame) {
                                var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                var tabCon = '';
                                var tabBottom = '</table>';
                                var ftype = row.FumigationTypes.split('$');
                                //  console.log("ftype: " + ftype.length);
                                if (ftype.length > 1) {
                                    for (var i = 0; i < ftype.length; i++) {
                                        tabCon += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top">' + ConvertDate(PickUpArrival[0], true) + '</label></td></tr>';
                                    }
                                    return tabTop + tabCon + tabBottom;
                                }
                                else {
                                    return '<label data-toggle="tooltip" data-placement="top">' + ConvertDate(PickUpArrival[0], true) + '</label>'
                                }


                            }
                            else {
                                var tabCon = '';
                                var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                // var tabCon = '';
                                var tabBottom = '</table>';
                                if (PickUpArrival.length > 0) {
                                    for (var i = 0; i < PickUpArrival.length; i++) {
                                        tabCon += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top">' + ConvertDate(PickUpArrival[i], true) + '</label></td></tr>';
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
               // "data": "FumigationTypes",
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
                            var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var tabBottom = '</table>';
                            if (fumigationTypesList.length > 0) {
                                for (var i = 0; i < fumigationTypesList.length; i++) {
                                    tabCon += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top">' + fumigationTypesList[i] + '</label></td></tr>';
                                }
                                return tabTop + tabCon + tabBottom
                            }
                            // }
                        }
                        else {
                            var tabCon = '';
                            if (fumigationTypesList.length > 0) {
                                for (var i = 0; i < fumigationTypesList.length; i++) {
                                    tabCon = '<label data-toggle="tooltip" data-placement="top">' + fumigationTypesList[i] + '</label>';;
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
                "data": "CustomerName",
                "name": "CustomerName",
                "autoWidth": true,

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
                                var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                var tabCon = '';
                                var tabBottom = '</table>';
                                var ftype = row.FumigationTypes.split('$');
                                if (ftype.length > 1) {
                                    for (var i = 0; i < ftype.length; i++) {
                                        tabCon += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(pickupLocation[0]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(pickupLocation[0]) + '</label></td></tr>';
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
                                        lblPickupLocation += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(pickupLocation[i]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(pickupLocation[i]) + '</label></td></tr>'
                                    }
                                    var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
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
                    var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                    // var tabCon = '';
                    var tabBottom = '</table>';
                    var type = row.FumigationTypes.split(',');
                    for (let i = 0; i < type.length; i++) {

                        if ($.trim(row.AWB) != "") {
                            var pickupLocation = row.AWB.split(",");
                            console.log("AWB: "+pickupLocation);
                            if (pickupLocation.length > 0) {

                                var lblPickupLocation = "";
                                if (pickupLocation.length > 1) {
                                    for (let i = 0; i < pickupLocation.length; i++) {
                                        AWB_CP_CN += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top" title="' + pickupLocation[i] + '">' + pickupLocation[i] + '</label></td></tr>'
                                    }
                                    var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
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
                      //  else {
                       //     return '<label data-toggle="tooltip" data-placement="top" title="' + row.ContainerNo + '"> NA </label>'
                       // }
                       else if ($.trim(row.CustomerPO) != "") {
                            var pickupLocation = row.CustomerPO.split(",");
                            if (pickupLocation.length > 0) {


                                var lblPickupLocation = "";
                                if (pickupLocation.length > 1) {
                                    for (let i = 0; i < pickupLocation.length; i++) {
                                        AWB_CP_CN += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top" title="' + pickupLocation[i] + '">' + pickupLocation[i] + '</label></td></tr>'
                                    }
                                    var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                    // var tabCon = '';
                                    var tabBottom = '</table>';
                                    //lblPickupLocation = lblPickupLocation.trim("<br/>");
                                    return tabTop + AWB_CP_CN + tabBottom;
                                }
                                else {
                                    return '<label data-toggle="tooltip" data-placement="top" title="' + row.CustomerPO + '">' + row.CustomerPO + '</label>'
                                }
                            }


                        }
                       

                        else if ($.trim(row.ContainerNo) != "") {
                            var pickupLocation = row.ContainerNo.split(",");
                            if (pickupLocation.length > 0) {


                                var lblPickupLocation = "";
                                if (pickupLocation.length > 0) {
                                    for (let i = 0; i < pickupLocation.length; i++) {
                                        AWB_CP_CN += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top" title="' + pickupLocation[i] + '">' + pickupLocation[i] + '</label></td></tr>'
                                    }
                                    var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                    // var tabCon = '';
                                    var tabBottom = '</table>';
                                    return tabTop + AWB_CP_CN + tabBottom;
                                    //lblPickupLocation = lblPickupLocation.trim("<br/>");

                                }
                                else {
                                    return '<label data-toggle="tooltip" data-placement="top" title="' + row.ContainerNo + '">' + row.ContainerNo + '</label>'
                                }

                            }

                        }
                        else {
                            return '<label data-toggle="tooltip" data-placement="top" title="' + row.ContainerNo + '"> NA </label>'
                        }
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
                            var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var tabBottom = '</table>';
                            var lblPickupLocation = "";
                            for (let i = 0; i < vendorNconsignee.length; i++) {
                                // console.log("vendorNconsignee: " + vendorNconsignee[i]);
                                lblPickupLocation += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top" title="' + row.VendorNconsignee +'" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + vendorNconsignee[i] + '</label></td></tr>';

                            }
                            return tabTop + lblPickupLocation + tabBottom
                        }

                        else {
                            var lblPickupLocation = "";
                            return '<label data-toggle="tooltip" data-placement="top" title="' +row.VendorNconsignee+'" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + row.VendorNconsignee + '</label>';
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
                            var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var tabBottom = '</table>';
                            var lblPickupLocation = "";
                            for (let i = 0; i < BoxCount.length; i++) {
                                // console.log("vendorNconsignee: " + BoxCount[i]);
                                var boxcounts = BoxCount[i].split('.');
                                lblPickupLocation += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top">' + boxcounts[0] + '</label></td></tr>';

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
                // "data": "PalletCount",
                "name": "PalletCount",
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    if (row.PalletCount != null && row.PalletCount != '') {

                        var PalletCount = row.PalletCount.split("|")

                        if (PalletCount.length > 1) {
                            var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var tabBottom = '</table>';
                            var lblPickupLocation = "";
                            for (let i = 0; i < PalletCount.length; i++) {
                                var palletcounts = PalletCount[i].split('.');
                                //console.log("vendorNconsignee: " + PalletCount[i]);
                                lblPickupLocation += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top">' + palletcounts[0] + '</label></td></tr>';

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
                            var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var tabBottom = '</table>';
                            var lblPickupLocation = "";
                            for (var i = 0; i < TrailerPosition.length; i++) {
                                //console.log("vendorNconsignee: " + TrailerPosition[i]);
                                lblPickupLocation += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top">' + TrailerPosition[i] + '</label></td></tr>';

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
                            var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            var tabCon = '';
                            var tabBottom = '</table>';
                            var ftype = row.FumigationTypes.split('$');
                           // console.log("ftype: " + ftype.length);
                            if (ftype.length > 1) {
                                for (var i = 0; i < ftype.length; i++) {
                                    tabCon += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(fumigationSite[0]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(fumigationSite[0]) + '</label></td></tr>';
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
                                    lblfumigationSite += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(fumigationSite[i]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(fumigationSite[i]) + '</label></td></tr>'
                                }
                                var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
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
                "name": "FumigationArrival",
                "autoWidth": true,

                "render": function (data, type, row, meta) {

                    var isSame = false;
                    if (row.FumigationArrival != null && row.FumigationArrival != '') {
                        var FumigationArrival = row.FumigationArrival.split("|")
                        if (FumigationArrival.length > 0) {
                            var count = 0;
                            for (var i = 0; i < FumigationArrival.length; i++) {
                                if (FumigationArrival[i] == FumigationArrival[0]) {
                                    count = count + 1;
                                }
                            }
                            if (count == FumigationArrival.length) {
                                isSame = true;
                            }
                            if (isSame) {
                                var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                var tabCon = '';
                                var tabBottom = '</table>';
                                var ftype = row.FumigationTypes.split('$');
                              //  console.log("ftype: " + ftype.length);
                                if (ftype.length > 1) {
                                    for (var i = 0; i < ftype.length; i++) {
                                        tabCon += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top">' + ConvertDate(FumigationArrival[0], true) + '</label></td></tr>';
                                    }
                                    return tabTop + tabCon + tabBottom;
                                }
                                else {
                                    return '<label data-toggle="tooltip" data-placement="top">' + ConvertDate(FumigationArrival[0], true) + '</label>'
                                }
                                
                                       
                            }
                            else {
                                var tabCon = '';
                                var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                // var tabCon = '';
                                var tabBottom = '</table>';
                                if (FumigationArrival.length > 0) {
                                    for (var i = 0; i < FumigationArrival.length; i++) {
                                        tabCon += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top">' + ConvertDate(FumigationArrival[i], true) + '</label></td></tr>';
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
                "data": "PickUpEquipment",
                "name": "PickUpEquipment",
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    if (row.PickUpEquipment != null && row.PickUpEquipment != '') {

                        var PickUpEquipment = row.PickUpEquipment.split("$");

                        if (PickUpEquipment.length > 1) {
                            var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var tabBottom = '</table>';
                            var lblPickupLocation = "";
                            for (var i = 0; i < PickUpEquipment.length; i++) {
                                //console.log("vendorNconsignee: " + PickUpEquipment[i]);
                                // var pickupeq = PickUpEquipment[i].split('|');
                                lblPickupLocation += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top">' + PickUpEquipment[i] + '</label></td></tr>';

                            }
                            return tabTop + lblPickupLocation + tabBottom
                        }

                        else {
                            var lblPickupLocation = "";
                            var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var tabBottom = '</table>';
                            //PickUpEquipment = row.PickUpEquipment.split("|");
                            for (var i = 0; i < PickUpEquipment.length; i++) {
                                // console.log("Single value: " + PickUpEquipment[i]);
                                lblPickupLocation += '<label data-toggle="tooltip" data-placement="top">' + PickUpEquipment[i] + '</label>';

                            }
                            return lblPickupLocation

                        }

                    } else {
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
                            var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var tabBottom = '</table>';
                            var lblPickupLocation = "";
                            for (var i = 0; i < PickUpDriver.length; i++) {
                                //console.log("vendorNconsignee: " + PickUpDriver[i]);
                                //var pickupeq = PickUpDriver[i].split('|');
                                lblPickupLocation += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top">' + PickUpDriver[i] + '</label></td></tr>';

                            }
                            return tabTop + lblPickupLocation + tabBottom
                        }

                        else {
                            var lblPickupLocation = "";
                            var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
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
                //"data": "Temperature",
                "name": "Temperature",
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    if ($.trim(row.Temperature) != "F") {

                        var Temperature = row.Temperature.split("|")

                        if (Temperature.length > 1) {
                            var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
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

                                lblPickupLocation += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top">' + temp1 + '</label></td></tr>';

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
                "name": "ReleaseDate",
                "autoWidth": true,

                "render": function (data, type, row, meta) {
                   // console.log("row: ", row);

                    if (row.ReleaseDate != null && row.ReleaseDate != '') {
                        var ReleaseDate = row.ReleaseDate.split("|")
                        if (ReleaseDate.length > 0) {
                            var count = 0;
                            for (var i = 0; i < ReleaseDate.length; i++) {
                                if (ReleaseDate[i] == ReleaseDate[0]) {
                                    count = count + 1;
                                }
                            }
                            if (count == ReleaseDate.length) {
                                isSame = true;
                            }
                            if (isSame) {
                                var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                var tabCon = '';
                                var tabBottom = '</table>';
                                var ftype = row.FumigationTypes.split('$');
                                //  console.log("ftype: " + ftype.length);
                                if (ftype.length > 1) {
                                    for (var i = 0; i < ftype.length; i++) {
                                        tabCon += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top">' + ConvertDate(ReleaseDate[0], true) + '</label></td></tr>';
                                    }
                                    return tabTop + tabCon + tabBottom;
                                }
                                else {
                                    return '<label data-toggle="tooltip" data-placement="top">' + ConvertDate(ReleaseDate[0], true) + '</label>'
                                }


                            }
                            else {
                                var tabCon = '';
                                var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                                // var tabCon = '';
                                var tabBottom = '</table>';
                                if (ReleaseDate.length > 0) {
                                    for (var i = 0; i < ReleaseDate.length; i++) {
                                        tabCon += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top">' + ConvertDate(ReleaseDate[i], true) + '</label></td></tr>';
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
                "data": "DeliveryEquipment",
                "name": "DeliveryEquipment",
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    if (row.DeliveryEquipment != null && row.DeliveryEquipment != '') {

                        var DeliveryEquipment = row.DeliveryEquipment.split("$");

                        if (DeliveryEquipment.length > 1) {
                            var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var tabBottom = '</table>';
                            var lblPickupLocation = "";
                            for (var i = 0; i < DeliveryEquipment.length; i++) {
                                //console.log("vendorNconsignee: " + PickUpEquipment[i]);
                                // var pickupeq = PickUpEquipment[i].split('|');
                                lblPickupLocation += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top">' + DeliveryEquipment[i] + '</label></td></tr>';

                            }
                            return tabTop + lblPickupLocation + tabBottom
                        }

                        else {
                            var lblPickupLocation = "";
                            var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var tabBottom = '</table>';
                            //PickUpEquipment = row.PickUpEquipment.split("|");
                            for (var i = 0; i < DeliveryEquipment.length; i++) {
                                // console.log("Single value: " + PickUpEquipment[i]);
                                lblPickupLocation += '<label data-toggle="tooltip" data-placement="top">' + DeliveryEquipment[i] + '</label>';

                            }
                            return lblPickupLocation

                        }

                    } else {
                        return 'NA'

                    }

                    
                }
            },
            
            {
                // "data": "DeliveryDriver",
                "name": "DeliveryDriver",
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    if (row.DeliveryDriver != null && row.DeliveryDriver != '') {

                        var DeliveryDriver = row.DeliveryDriver.split("$");

                        if (DeliveryDriver.length > 1) {
                            var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var tabBottom = '</table>';
                            var lblPickupLocation = "";
                            for (var i = 0; i < DeliveryDriver.length; i++) {
                                //console.log("vendorNconsignee: " + PickUpDriver[i]);
                                //var pickupeq = PickUpDriver[i].split('|');
                                lblPickupLocation += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top">' + DeliveryDriver[i] + '</label></td></tr>';

                            }
                            return tabTop + lblPickupLocation + tabBottom
                        }

                        else {
                            var lblPickupLocation = "";
                            var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
                            // var tabCon = '';
                            var tabBottom = '</table>';
                            DeliveryDriver = row.DeliveryDriver.split("$");
                            for (var i = 0; i < DeliveryDriver.length; i++) {
                                //console.log("Single value: " + PickUpDriver[i]);
                                lblPickupLocation += '<label data-toggle="tooltip" data-placement="top">' + DeliveryDriver[i] + '</label>';

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
                                        lbldeliveryLocation += '<tr style="background: none;"><td><label data-toggle="tooltip" data-placement="top" title="' + GetCAddressNew(deliveryLocation[i]) + '" style="max-width: 100px;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;">' + GetCompanyNew(deliveryLocation[i]) + '</label></td></tr>'
                                    }
                                    var tabTop = '<table class="table" height="auto" style="margin:0px auto;" cellspacing="0" width="100%">';
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
                "name": "WT",
                //"orderable": false,
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
               // "orderable": false,
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

            //    "name": "DeliveryArrival",
            //    "autoWidth": true,
            //    "render": function (data, type, row, meta) {

            //        if (row.DeliveryArrival != null) {
            //            return '<label>' + ConvertDate(row.DeliveryArrival, true) + '</label>'
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

                    var btnDelete = ' | <a href="javascript: void(0)" class="delete_icon" data-toggle="tooltip" title="Delete" onclick="javascript:DeleteFumigation(' + row.FumigationId + ');" >' +
                        '<i class="far fa-trash-alt"></i>' +
                        '</a>';
                    var btnPreview = '<a href="' + baseUrl + '/Fumigation/Fumigation/ViewFumigationNotification/' + row.FumigationId + '" title="Preview Quote" target="_blank" id="btnPreview">' +
                        '<i class="far fa-eye"></i>' +
                        '</a> |';
                    btnEdit = (isUpdate == true) ? btnEdit : "";
                    btnDelete = (isDelete == true) ? btnDelete : "";
                    btnPreview = (isView == true) ? btnPreview : "";
                    return '<div class="action-ic">' + btnPreview + ' ' + btnEdit + '  ' + btnDelete + ' ' + btnMap + '</div>'
                }

            }

        ],
        "order": [[0, "desc"]],
    });

  var search_thread_tblOtherFumigation = null;
    $("#tblFumigationList_filter input")
        .unbind()
        .bind("input", function (e) {
		
            clearTimeout(search_thread_tblOtherFumigation);
            search_thread_tblOtherFumigation = setTimeout(function () {
                var dtable = $("#tblFumigationList").dataTable().api();
                var elem = $("#tblFumigationList_filter input");
			
                var replacedStr = $(elem).val().replace(/\//g, "-");
                console.log("elem value: ", replacedStr);
                return dtable.search(replacedStr).draw();
            }, 700);
        });
    oTable = $('#tblFumigationList').DataTable();
   // $("input[type='search']").unbind();
 /*    $("input[type='search']").unbind().keyup(function (e) {
        var value = $(this).val();
        //if (value.length > 3 || value.length<1 ) {
            //oTable.search(value).draw();
        //}
        if (e.key === 'Enter' || e.keyCode === 13) {
            oTable.search(value).draw();
        }    
    });
    $("input[type='search']").click(function () {
        var value = "";
      //  console.log("value: "+value);
        
        if (value = "") {
            oTable.search(value).draw();
        }

    }); */
    

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


function CopyFumigation(FumigationId) {
    $.confirm({
        title: 'Confirmation!',
        content: '<b>Are you sure you want to copy this fumigation?</b> ',
        type: 'blue',
        typeAnimated: true,
        buttons: {
            Copy: {
                btnClass: 'btn-green',
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
                                $("#dtReqLoading").val(ConvertDate(data.RequestedLoading, true));
                                $("#dtEstFumIn").val(ConvertDate(data.FumigationIn, true));
                                $("#dtRelease").val(ConvertDate(data.FumigatiionRelease, true));
                                $("#dtDelEstArrival").val(ConvertDate(data.DelEstArrival, true));

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

                 btnClass: 'btn-red',
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

    if (isValid && requestedLoading != "" && requestedLoading < yesterday) {
        // toastr.warning("The Req. Loading should be greater than, or equal to, yesterday's date.");
        AlertPopup("The Req. Loading should be greater than, or equal to, yesterday's date.");
        isValid = false;
        return isValid
    }


    if (isValid && requestedLoading != "" && fumigationIn != "") {
        if (new Date(requestedLoading) <= new Date(fumigationIn)) {

        }
        else {
            //toastr.warning("The Est. Fum. In should be greater than the Req. Loading.");
            AlertPopup("The Est. Fum. In should be greater than the Req. Loading.");
            isValid = false;
            return isValid
        }

    }

    if (isValid && fumigationIn != "" && fumigatiionRelease != "") {
        if (new Date(fumigationIn) <= new Date(fumigatiionRelease)) {

        }
        else {
            // toastr.warning("Est. Fum. Release should be greater than Est. Fum. In.");
            AlertPopup("Est. Fum. Release should be greater than Est. Fum. In.");
            isValid = false;
            return isValid
        }

    }

    if (isValid && fumigatiionRelease != "" && delEstArrival != "") {
        if (new Date(fumigatiionRelease) <= new Date(delEstArrival)) {

        }
        else {
            //toastr.warning("Del. Est. Arrival should be greater than Est. Fum. Release.");
            AlertPopup("Del. Est. Arrival should be greater than Est. Fum. Release.");
            isValid = false;
            return isValid
        }

    }
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
                btnClass: 'btn-green',
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
                            $('#tblFumigationList').DataTable().clear().destroy();
                            GetFumigationList();
                        }
                    });
                }
            },
            cancel: {

                 btnClass: 'btn-red',
            }
        }
    })

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
//DART: To enable horizontal scroll inside data grid
//const horwheel = require('horwheel');
//const wrapper = document.querySelector('#tblFumigationList');
//horwheel(wrapper);
$("#tblFumigationList .sorting").css({ "position": "sticky !important", "top": "0", "z-index": "1", "background": "#fff" });
//$("[data-paroller-factor]").paroller();


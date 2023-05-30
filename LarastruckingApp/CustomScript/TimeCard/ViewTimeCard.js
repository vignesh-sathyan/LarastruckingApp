$(document).ready(function () {
    $("#mytimeCard").hide();
    binddate();
    showHideDiv();
    BindWeekDate();
    $("#divTimeCard").show();
    bindUser();
    startEndDate();
    btnViewTimeCard();
    SelectReport();
});

//#region select date  in Quote Date and Valid Thru
var binddate = function () {
    
    let curr = new Date();
    bindStartOrEndDate(curr)
}
//#endregion


function bindStartOrEndDate(_date) {
    let curr = _date;
    let first = curr.getDate() - (curr.getDay() < 4 ? (curr.getDay() + 7) : curr.getDay()) + (0 + 4);
    let fistDate = new Date(curr.setDate(first));

    let last = curr.getDate() - (curr.getDay() < 4 ? (curr.getDay() + 7) : curr.getDay()) + (6 + 4);
    let lastDate = new Date(curr.setDate(last));

    $("#dtStartedDate").val(dateFormat(fistDate, "mm-dd-yyyy"));
    $("#dtEndDate").val(dateFormat(lastDate, "mm-dd-yyyy"));
}

function BindStartDates() {
    
    var date = $("#dtStartedDate").val();
    var from = date.split("-");
    var curr = new Date(from[2], from[0] - 1, from[1]);
    bindStartOrEndDate(curr);
}
function BindEndDates() {
    var date = $("#dtEndDate").val();
    var from = date.split("-");
    var curr = new Date(from[2], from[0] - 1, from[1]);
    bindStartOrEndDate(curr);

}

//#region colour change on grid icon
$("#tblTimeCard").on("mouseover", 'tr', function () {

    $(this).find(".chng-color-edit").css('color', 'white');
    $(this).find(".chng-color-Trash").css('color', 'white');
});


$("#tblTimeCard").on("mouseout", 'tr', function () {

    $(this).find(".chng-color-edit").css('color', '#007bff');
    $(this).find(".chng-color-Trash").css('color', 'red');

});
//#endregion

//$('#tblTimeCard').on('dblclick', 'tbody tr', function () {
//    
//    var table = $('#tblTimeCard').DataTable();
//    var data_row = table.row($(this).closest('tr')).data();
//    window.location.href = baseUrl + '/TimeCard/TimeCard//' + data_row.EDID;
//});

function GetTimeCardList() {
    $.fn.dataTable.ext.errMode = function (settings, helpPage, message) {
        //console.log(message);
    };
    $('#tblTimeCard').DataTable().clear().destroy();
    var values = {};
    values.StartDate = $("#dtStartedDate").val();
    values.EndDate = $("#dtEndDate").val();
    values.UserId = $("#ddlUser").val();


    $('#tblTimeCard').DataTable({
       // "bInfo": false,
        dom: 'Blfrtip',
        buttons: [
            {
                extend: 'print',
                orientation: 'landscape',
                pageSize: 'LEGAL',
                title: "",
                text: '<img src="../../Assets/images/printer.png" style="height:18px;margin-right: 5px;width:16px;"/> Print',
                messageBottom: datetime,
                //messageTop: datetime,
                exportOptions: {

                    columns: ':visible',
                    stripHtml: false,
                    //columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19]

                },
                customize: function (win) {

                    var last = null;
                    var current = null;
                    var bod = [];

                    var css = '@page { size: landscape;}  ',
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
                            "<table id='checkheader'><tr><td width='80%' ><h3 style='font-size: 20px;'>REQUESTED FUMIGATION</h3></td><td width='20%'><div><img src='http://larastruckinglogistics-app.azurewebsites.net/Images/Laraslogo.png' height='100px'/></div></td></tr></table>"
                        );
                }
            }

        ],
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
            "url": baseUrl + "/TimeCard/TimeCard/GetTimeCardList",
            "type": "POST",
            "data": values,
            "async": false,            
            "datatype": "json",
        },
        "columns": [
            { "data": "Id", "name": "Id", "autoWidth": true },
            {
                "name": "UserName",
                //"orderable": false,
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    return "<a href='#' class='chng-color-edit' onclick='ShowTimeCard(" + row.UserId + ")'>" + row.UserName + "</a>";
                }
            },
           // { "data": "UserName", "name": "UserName", "autoWidth": true },
            { "data": "Day", "name": "Day", "autoWidth": true },
            {
                "name": "InDateTime",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    if (row.InDateTime != null && row.InDateTime != undefined) {
                        return '<label>' + ConvertDate(row.InDateTime, true) + '</label><br/>'
                    }
                    else {
                        return '<label></label><br/>'
                    }

                }
            },
            {
                "name": "OutDateTime",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    if (row.InDateTime != null && row.OutDateTime != undefined) {
                        return '<label>' + ConvertDate(row.OutDateTime, true) + '</label><br/>'
                    }
                    else {
                        return '<label></label><br/>'
                    }

                }
            },
            { "data": "TotalHours", "name": "TotalHours", "autoWidth": true },
          

        ],
        "order": [[0, "desc"]],
        columnDefs: [
            {
                "targets": 0,
                "visible": false,
            }
        ]
    });

    oTable = $('#tblTimeCard').DataTable();

    $("input[type='search']").keyup(function () {

        oTable.search(this.value);
        oTable.draw();

    });

    

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

//#region Get Labor Report
function GetLaborReport() {

    $("#tblLaborReport").DataTable().clear().destroy();
    var values = {};
    var weekdate = $('#ddlWeekDate :selected').text();
    weekdate = weekdate.split("TO");

    values.StartDate = $.trim(weekdate[0]);
    values.EndDate = $.trim(weekdate[1]);
    values.UserId = $("#ddlUser").val();

    $('#tblLaborReport').DataTable({
        // "bInfo": false,
        dom: 'Blfrtip',
        select: 'single',
        serverSide: true,
        buttons: [
            {
                extend: 'print',
                title: "",
                text: '<img src="../../Assets/images/printer.png" style="height:18px;margin-right: 5px;width:16px;"/> Print',
                messageBottom: datetime,
                exportOptions: {
                    // columns: ':visible',
                    stripHtml: false,
                    columns: [0, 1, 2, 3, 4, 5]
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
                            "<table id='9'><tr><td width='80%' ></td><td width='20%'><div><img src='http://larastruckinglogistics-app.azurewebsites.net/Images/Laraslogo.png' height='100px'/></div></td></tr></table>"
                        );


                }

            },

        ],
        responsive: true,
        processing: true,
        searching: true,
        bDestroy: true,
        filter: true,
        "language": {
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
        },
        "ajax": {
            "url": baseUrl + "/TimeCard/TimeCard/GetLaborReport",
            "type": "POST",
            "data": values,
            "async": false,
            "datatype": "json",
        },
        "columns": [
            { "data": "Name", "name": "Name", "autoWidth": true },
            { "data": "TotalHours", "name": "TotalHours", "autoWidth": true },
            { "data": "HourlyRate", "name": "HourlyRate", "autoWidth": true },
            { "data": "TotalPaid", "name": "TotalPaid", "autoWidth": true },
            { "data": "Variation", "name": "Variation", "autoWidth": true },
            { "data": "Loangranted", "name": "Loangranted", "autoWidth": true },
            { "data": "Loanbalance", "name": "Loanbalance", "autoWidth": true },

        ],
        "order": [[0, "asc"]],
        columnDefs: [
            {
                "targets": 1,
                "orderable": false,
            },
            {
                "targets": 2,
                "orderable": false,
            },
            {
                "targets": 3,
                "orderable": false,
            },
            {
                "targets": 4,
                "orderable": false,
            },
            {
                "targets": 5,
                "orderable": false,
            },
            {
                "targets": 6,
                "orderable": false,
            },
        ]
    });

    oTable = $('#tblLaborReport').DataTable();

    $("input[type='search']").keyup(function () {
        oTable.search(this.value);
        oTable.draw();
    });
}
//#endregion

//#region Get Daily Report
function GetDailyRepot() {
    
    var values = {};
    
    var weekdate = $('#ddlWeekDate :selected').text();
    weekdate = weekdate.split("TO");
    values.StartDate = $.trim(weekdate[0]);
    values.EndDate = $.trim(weekdate[1]);
    values.UserId = isNaN(parseInt($("#ddlUser").val())) ? 0 : parseInt($("#ddlUser").val());
    values.PageNumber = 0;
    values.PageSize = 0;
    values.TotalCount = 0;
    //console.log("Dailyreport: ",values);
    $.ajax({
        url: baseUrl + 'TimeCard/TimeCard/GetDailyReport',
        data: { 'StartDate': values.StartDate, 'EndDate': values.EndDate, 'UserId': values.UserId },
        type: "POST",
        dataType: "html",
        success: function (data) {
            $("#divDailyReport").empty();
            $("#divDailyReport").html(data);
            //$("#divDailyReport").prepend("<table id='9'><tr><td width='80%' ></td><h3>Weekly Report</h3><td width='20%'><div><img src='http://larastruckinglogistics-app.azurewebsites.net/Images/Laraslogo.png' height='100px'/></div></td></tr></table>");
            
            $("#divDailyReport #selectedweek").text(weekdate);
            //console.log("dailyData: ",data);
        }
    });
}
//#endregion
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

function BindWeekDate() {    $.ajax({
        url: baseUrl + 'TimeCard/TimeCard/GetWeekDates',
        data: {},
        type: "POST",
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        // cache: false,
        success: function (data) {
            
            ddlValue = "";
            $("#ddlWeekDate").empty();
            glbWeekDate = JSON.parse(JSON.stringify(data));
            // ddlValue += '<option value="0">SELECT WeeK</option>'
            for (var i = glbWeekDate.length - 1; i >= 0; i--) {
                
                ddlValue += '<option value="">' + ConvertDate(glbWeekDate[i].WeekStartDay) + ' TO ' + ConvertDate(glbWeekDate[i].WeekEndDay) + '</option>';
            }
            $("#ddlWeekDate").append(ddlValue);

        }
    });
}


btnViewTimeCard = function () {
    $("#btnViewTimeCard").on("click", function () {
       // 
        SelectReport();
    })
}

//#region function for apply selectize on customer dropdown
var bindUser = function () {
    var $select = $('#ddlUser').selectize();
    $select[0].selectize.destroy();
    $('#ddlUser').selectize({
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
        load: function (query, callback) {
            if (!query.length) return callback();
            $.ajax({
                url: baseUrl + "TimeCard/TimeCard/GetUserList/?searchText=" + query,
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
                        item.id = value.UserId;
                        item.text = value.UserName;
                        customers.push(item);
                    });

                    callback(customers);
                },

            });
        },
        render: {
            item: function (item, escape) {
                return '<div>' +
                    ('<span class="name ddlUser">' + escape(item.text) + '</span>') +
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

//$('input[type=radio][name=report]').change(function () {
//    if (this.value.toLowerCase() == 'labor') {
//        showHideDiv();
//        $("#divLaborReport").show();
//    }
//    else if (this.value.toLowerCase() == 'weekly') {
//        showHideDiv();
//        $("#divWeeklyReport").show();
//    }
//    else if (this.value.toLowerCase() == 'daily') {
//        showHideDiv();
//        $("#divDailyReport").show();
//    }
//    else {
//        bindUser();
//        GetTimeCardList();
//    }
//});

function SelectReport() {
    
    var selectedValue = $("input[type='radio']:checked").val();
    if (selectedValue != undefined) {
        showHideDiv();
        if (selectedValue.toLowerCase() == 'labor') {

            $("#divLaborReport").show();
            GetLaborReport();
        }
        else if (selectedValue.toLowerCase() == 'weekly') {
            $("#divWeeklyReport").show();
            GetWeeklyReport()
        }
        else if (selectedValue.toLowerCase() == 'daily') {
            $("#divDailyReport").show();
            GetDailyRepot();
        }
        else {
            GetTimeCardList();
        }
    }
    else {
        GetTimeCardList();
    }
}

function showHideDiv() {
    $("#divTimeCard").hide();
    $("#divLaborReport").hide();
    $("#divDailyReport").hide();
    $("#divWeeklyReport").hide();
}


//#region Get Labor Report
function GetWeeklyReport() {

    $("#tblWeeklyReport").DataTable().clear().destroy();
    var values = {};
    var weekdate = $('#ddlWeekDate :selected').text();
    weekdate = weekdate.split("TO");

    //console.log(weekdate);

    values.StartDate = $.trim(weekdate[0]);
    values.EndDate = $.trim(weekdate[1]);
    values.UserId = $("#ddlUser").val();
    //console.log("Weekly Report: " + values.StartDate + " : " + values.EndDate + " : " + values.UserId);
    $('#tblWeeklyReport').DataTable({
        // "bInfo": false,
        dom: 'Blfrtip',
        select: 'single',
        serverSide: true,
        buttons: [
            {
                extend: 'print',
                title: "",
                text: '<img src="../../Assets/images/printer.png" style="height:18px;margin-right: 5px;width:16px;"/> Print',
                messageBottom: datetime,
                exportOptions: {
                    // columns: ':visible',
                    stripHtml: false,
                    columns: [0, 1, 2]
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
                            "<table id='9'><tr><td width='80%' ></td><h3>Weekly Report</h3><td width='20%'><div><img src='http://larastruckinglogistics-app.azurewebsites.net/Images/Laraslogo.png' height='100px'/></div></td></tr></table>"
                        );
                }

            },

        ],
        responsive: true,
        processing: true,
        searching: true,
        bDestroy: true,
        filter: true,
        "language": {
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
        },
        "ajax": {
            "url": baseUrl + "/TimeCard/TimeCard/GetWeeklyReport",
            "type": "POST",
            "data": values,
            "async": false,
            "datatype": "json",
        },
        "columns": [

            { "data": "Name", "name": "Name", "autoWidth": true },
            { "data": "TotalHours", "name": "TotalHours", "autoWidth": true },
            { "data": "TotalPaid", "name": "TotalPaid", "autoWidth": true },

        ],
        columnDefs: [
            {
                "targets": 1,
                "orderable": false,
            },
            {
                "targets": 2,
                "orderable": false,
            },

        ]
    });

    oTable = $('#tblWeeklyReport').DataTable();

    $("input[type='search']").keyup(function () {
        oTable.search(this.value);
        oTable.draw();
    });
}
//#endregion
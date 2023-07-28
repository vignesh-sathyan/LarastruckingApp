$(document).ready(function () {
    $("#mytimeCard").hide();
    GetDriverList();
    var userId = 0;

});

function goodbye(e) {
    if (isNeedToloaded) {

        if (!e) e = window.event;
        //e.cancelBubble is supported by IE - this will kill the bubbling process.
        e.cancelBubble = true;
        e.returnValue = 'You sure you want to leave?'; //This is displayed on the dialog

        //e.stopPropagation works in Firefox.
        if (e.stopPropagation) {
            e.stopPropagation();
            e.preventDefault();
        }

    }
}
window.onbeforeunload = goodbye;
//#region colour change on grid icon
$("table").on("mouseover", 'tr', function () {

    $(this).find(".chng-color-edit").css('color', 'white');
    $(this).find(".chng-color-Trash").css('color', 'white');
    row = $(this).closest("tr");
    $(row).find('input[readonly="readonly"]').css('color', 'white');

});

$("table").on("mouseout", 'tr', function () {

    $(this).find(".chng-color-edit").css('color', '#007bff');
    $(this).find(".chng-color-Trash").css('color', 'red');

    row = $(this).closest("tr");
    $(row).find('input[readonly="readonly"]').css('color', 'black');


});
//#endregion

$('#tblDriverDetails').on('dblclick', 'tbody tr', function () {

    var table = $('#tblDriverDetails').DataTable();
    var data_row = table.row($(this).closest('tr')).data();
    window.location.href = baseUrl + '/Driver/Index/' + data_row.DriverId;

});

function GetDriverList() {
    $('#tblDriverDetails').DataTable({
        // "bInfo": false,
        dom: 'Blfrtip',
        buttons: [
            {
                extend: 'print',
                orientation: 'landscape',
                pageSize: 'LEGAL',
                title: "",
                text: '<img src="../../Assets/images/printer.png" style="height:18px;margin-right: 5px;position: relative;top: 1px;width:16px;"/> Print',
               // messageBottom: datetime,
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
                            "<table id='checkheader'><tr><td width='80%' ><h3 style='font-size: 20px;'>View Driver</h3></td><td width='20%'><div><img src='http://larastruckinglogistics-app.azurewebsites.net/Images/Laraslogo.png' height='100px'/></div></td></tr></table>"
                        );
                }
            },
          

        ],
        select: 'single',
        filter: true,
        responsive: true,
        processing: true,
        serverSide: true,
        
        searching: true,
        bDestroy: true,
        "language": {
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
        },
        "ajax": {
            "url": baseUrl + "/Driver/LoadData",
            "type": "POST",
            "datatype": "json",
            "async": false,
        },
        "columns": [
            { "data": "DriverId", "name": "DriverId", "autoWidth": true },
            {
                "name": "FirstName",
                //"orderable": false,
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    if (row.IsActive == true) {

                        return "<a href='#' class='chng-color-edit' data- onclick='ShowTimeCard(" + row.UserId + ")'>" + row.FirstName + "</a>";
                    }
                    else {
                        return '<span>' + row.FirstName + '</span>';
                    }

                }
            },
            //{ "data": "FirstName", "name": "FirstName", "autoWidth": true },
            { "data": "LastName", "name": "LastName", "autoWidth": true },
            { "data": "Email", "name": "Email", "autoWidth": true },
        //    { "data": "Phone", "name": "Phone", "autoWidth": true },

        //    {
        //        "data": "IsActive",
        //        "name": "IsActive",
        //        "autoWidth": true,
        //        "render": function (data, type, row, meta) {
        //            return (row.IsActive == true) ? "ACTIVE" : "INACTIVE";
        //        }
        //    },
        //    {
        //        "name": "Leave",
        //        "orderable": false,
        //        "autoWidth": true,
        //        "render": function (data, type, row, meta) {
        //            if (row.UserId != 0) {
        //                if (row.TakenFrom != null && row.TakenTo != null) {

        //                    var fdate = ConvertDate(row.TakenFrom, true);
        //                    var tdate = ConvertDate(row.TakenTo, true);

        //                    return '<div class="action-ic">' +
        //                        '(' + fdate + ') - ' +
        //                        '(' + tdate + ')' +
        //                        '<a href="../Driver/Leave/' + row.UserId + '" class="edit_icon chng-color-edit" title="Edit Leave"> ' +
        //                        '<i class="far fa-edit"></i>' +
        //                        '</a> ' +
        //                        '</div>'
        //                }
        //                else {
        //                    return '<div class="action-ic">' +
        //                        '<a href="../Driver/Leave/' + row.UserId + '" class="edit_icon chng-color-edit" title="Apply Leave">' +
        //                        '<i class="far fa-edit"></i>' +
        //                        ' Apply </a> ' +
        //                        '</div>'
        //                }
        //            }
        //            else {
        //                return '<span> Please update </span>'
        //            }
        //        }
        //    },
        //    {
        //        "name": "LeaveStatus",
        //        "autoWidth": true,
        //        "render": function (data, type, row, meta) {

        //            if (row.LeaveStatus == "Approved") {

        //                return '<span class="badge badge-success">' + row.LeaveStatus + '</span>'
        //            }
        //            else if (row.LeaveStatus == "Unapproved") {

        //                return '<span class="badge badge-danger">' + row.LeaveStatus + '</span>'
        //            }
        //            else if (row.LeaveStatus == "Pending") {

        //                return '<span class="badge badge-warning">' + row.LeaveStatus + '</span>'
        //            }
        //            else if (row.LeaveStatus == "Cancelled") {

        //                return '<span class="badge badge-default">' + row.LeaveStatus + '</span>'
        //            }
        //            else {
        //                return '';
        //            }

        //        }
        //    },
        //    { "data": "IsTWIC", "name": "IsTWIC", "autoWidth": true },

        //    { "name": "Action", "autoWidth": true },
        ],
        "order": [[0, "desc"]],
        columnDefs: [
        //    {
        //        "targets": 9,
        //        "orderable": false,
        //        "render": function (data, type, row, meta) {

        //            var btnView = '<a href="../Driver/Documents/' + row.DriverId + '" class="edit_icon chng-color-edit" title="View Documents">' +
        //                '<i class="far fa-eye"></i>' +
        //                '</a>';
        //            var btnEdit = '<a href="../Driver/Index/' + row.DriverId + '" title="Edit Driver Detail" class="edit_icon chng-color-edit">' +
        //                '<i class="far fa-edit"></i>' +
        //                '</a>';
        //            var btnDelete = '<a href="javascript: void(0)" class="delete_icon chng-color-Trash" title="Delete Driver" onclick="javascript:DriverDelete(' + row.DriverId + ');" >' +
        //                '<i class="far fa-trash-alt"></i>' +
        //                '</a>';

        //            btnView = (isView == true) ? btnView : "";
        //            btnEdit = (isUpdate == true) ? btnEdit : "";
        //            btnDelete = (isDelete == true) ? btnDelete : "";


        //            return '<div class="action-ic">' + btnView + ' ' + btnEdit + ' ' + btnDelete + '</div>'
        //        }
        //    },
            {
                "targets": 0,
                "visible": false,
            }
        ]
    });

    //oTable = $('#tblDriverDetails').DataTable();

    //$("input[type='search']").keyup(function () {
    //    // console.log(this.value);

    //    oTable.search(this.value);
    //    oTable.draw();
    //});
    var search_thread_tblDriverDetails = null;
    $("#tblDriverDetails_filter input")
        .unbind()
        .bind("input", function (e) {
            clearTimeout(search_thread_tblDriverDetails);
            search_thread_tblDriverDetails = setTimeout(function () {
                var dtable = $("#tblDriverDetails").dataTable().api();
                var elem = $("#tblDriverDetails_filter input");
                return dtable.search($(elem).val()).draw();
            }, 700);
        });
}
//Show time card


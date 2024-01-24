$(document).ready(function () {

    GetReportAccidentList();

});


//#region colour change on grid icon
$(".tblChange-bgcolor").on("mouseover", 'tr', function () {

    $(this).find(".chng-color-View").css('color', 'white');
    $(this).find(".chng-color-Trash").css('color', 'white');
});


$(".tblChange-bgcolor").on("mouseout", 'tr', function () {

    $(this).find(".chng-color-View").css('color', '#007bff');
    $(this).find(".chng-color-Trash").css('color', 'red');

});
//#endregion

$('#tblAccidentReport').on('dblclick', 'tbody tr', function () {
    
    var table = $('#tblAccidentReport').DataTable();
    var data_row = table.row($(this).closest('tr')).data();
    var encryptedRowIds = Encrypt(data_row.AccidentReportId);
    window.location.href = baseUrl + '/AccidentReport/Index/' + encryptedRowIds;
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

function GetReportAccidentList() {
    $('#tblAccidentReport').DataTable({
        //"bInfo": false,
        dom: 'Blfrtip',
        serverSide: true,
        buttons: [
            {
                extend: 'print',
                title: "",
                text: '<img src="../../Assets/images/printer.png" style="height:18px;margin-right: 5px;position: relative;top: 1px;width:16px;"/> Print',
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
                            "<table id='9'><tr><td width='80%' ></td><td width='20%'><div><img src='"+baseUrl+"/Images/Laraslogo.png' height='100px'/></div></td></tr></table>"
                        );


                }

            },

        ],
        select: 'single',
        filter: true,
        responsive: true,
        processing: true,     
        searching: true,
        bDestroy: true,
        stateSave: true,
        "language": {
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
        },
        "ajax": {
            "url": baseUrl + "/AccidentReport/LoadData",
            "type": "POST",
            "datatype": "json",
            "async": false,
        },

        "columns": [
            { "data": "AccidentReportId", "name": "AccidentReportId", "autoWidth": true },
            { "data": "AccidentDate", "name": "AccidentDate", "autoWidth": true },
            { "data": "DriverName", "name": "DriverName", "autoWidth": true },
            { "data": "EquipmentNo", "name": "EquipmentNo", "autoWidth": true },
            { "data": "Year", "name": "Year", "autoWidth": true },
            { "data": "LicencePlate", "name": "LicencePlate", "autoWidth": true },
            { "data": "VehicleType", "name": "VehicleType", "autoWidth": true },
            { "data": "VIN", "name": "VIN", "autoWidth": true },           
            { "data": "PoliceReportNo", "name": "PoliceReportNo", "autoWidth": true },
            { "name": "Action", "autoWidth": true },
        ],
        "order": [[0, "desc"]],
        columnDefs: [
         
            {
                "targets": 1,
                "orderable": false,
                "width": "10%",
                "render": function (data, type, row, meta) {
                    return ConvertDate(row.AccidentDate, false);
                }
            },
            {
                "targets": 9,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var encryptedRowId = Encrypt(row.AccidentReportId);
                    var btnEdit = '<a href="../AccidentReport/Index/' + encryptedRowId + '" class="edit_icon chng-color-View">' +
                        '<i class="far fa-edit"></i>' +
                        '</a> |';
                    var btnView = '<a href="javascript: void(0)" data-encryptedRowId=' + encryptedRowId+' onclick="javascript:fn_ViewAccidentDocument(this);"  class="chng-color-View" title="View Documents">' +
                        '<i class="far fa-eye"></i>' +
                        '</a> ';
                    var btnDelete = '| <a href="javascript: void(0)" class="delete_icon chng-color-Trash" onclick="javascript:DeleteAccidentReport(' + row.AccidentReportId + ');" >' +
                        '<i class="far fa-trash-alt"></i>' +
                        '</a>';
                    btnEdit = (isUpdate == true) ? btnEdit : "";
                    btnDelete = (isDelete == true) ? btnDelete : "";
                    btnView = (isView == true) ? btnView : "";
                    return '<div class="action-ic">' + btnEdit + ' ' + btnView + ' ' + btnDelete + ' </div>'

                }
            },
            {
                "targets": 0,
                "visible": false,
            }

        ]
    });

    oTable = $('#tblAccidentReport').DataTable();

    $("input[type='search']").keyup(function () {
        // console.log(this.value);

        oTable.search(this.value);
        oTable.draw();
    });
}


//#region Redirect to View Accident Document
var fn_ViewAccidentDocument = function (_this) {
    
    var encryptedRowIds = $(_this).data("encryptedrowid");
    window.open(baseUrl + 'AccidentReport/ViewAccidentDocument/' + encryptedRowIds + ' ');
}
//#endregion



//Detail  Data Delete
function DeleteAccidentReport(listId) {
    
    $.confirm({
        title: 'Confirm!',
        content: 'Are you sure you want to Delete ?',
        type: 'red',
        typeAnimated: true,
        buttons: {
            Delete: {
                btnClass: 'btn-blue',
                action: function () {
                    $.ajax({
                        url: baseUrl + '/AccidentReport/DeleteAccidentReport',
                        data: { 'id': listId },
                        type: "GET",
                        // cache: false,
                        success: function (data) {
                            if (data.IsSuccess == true) {
                                //toastr.success(data.Message, "")
                                SuccessPopup(data.Message)
                            }
                            else {
                                //toastr.error(data.Message, "")
                                AlertPopup(data.Message)
                            }
                            
                            $('#tblAccidentReport').DataTable().clear().destroy();
                            GetReportAccidentList();
                        }
                    });


                }
            },
            cancel: {
                btnClass: 'btn-red',
            }

        }
    });


}
$(document).ready(function () {

    GetEquipmentList();
    btnViewEquipment();
});



//#region colour change on grid icon
$("#tblEquipmentDetails").on("mouseover", 'tr', function () {

    $(this).find(".chng-color-edit").css('color', 'white');
    $(this).find(".chng-color-Trash").css('color', 'white');
});


$("#tblEquipmentDetails").on("mouseout", 'tr', function () {

    $(this).find(".chng-color-edit").css('color', '#007bff');
    $(this).find(".chng-color-Trash").css('color', 'red');

});
//#endregion

$('#tblEquipmentDetails').on('dblclick', 'tbody tr', function () {
       var table = $('#tblEquipmentDetails').DataTable();
    var data_row = table.row($(this).closest('tr')).data();
    window.location.href = baseUrl + '/Equipment/AddEquipment/' + data_row.EDID;
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

function GetEquipmentList() {

    $('#tblEquipmentDetails').DataTable({
        serverSide: true,
        dom: 'Blfrtip',
       "paging": true,
        buttons: [
            {
                extend: 'print',
                orientation: 'landscape',
                pageSize: 'LEGAL',
                title: "",
                messageBottom: datetime,

                exportOptions: {
                    columns: ':visible',
                    stripHtml: false,
                    columns: [1, 2, 3, 4, 5, 6,8,9]
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
                            "<table style='text-transform:capitalize' id='checkheader'><tr><td width='80%' style='text-transform:capitalize;font-size:13px;'> </td><td width='20%'><div><img src='http://larastruckinglogistics-app.azurewebsites.net/Images/Laraslogo.png' height='100px'/></div></td></tr></table>"
                        );
                }
            },
        ],
        select: 'single',
        responsive: true,
        processing: true,
        serverSide: true,
        searching: true,
        bDestroy: true,
        filter: true,
        stateSave: true,
        "language": {
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
        },
        "ajax": {
            "url": baseUrl + "/Equipment/LoadData",
            "type": "POST",
            //"data": values,
            "async": false,
            "datatype": "json",
        },
        "columns": [
            { "data": "EDID", "name": "EDID", "autoWidth": true },
            { "data": "EquipmentNo", "name": "EquipmentNo", "autoWidth": true },
            { "data": "VehicleTypeName", "name": "VehicleTypeName", "autoWidth": true },

            {
                "name": "MaxLoad",
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    if (row.MaxLoad != null && row.MaxLoad != '') {
                        var max = row.MaxLoad.split("\n");

                        if (max.length > 1) {
                            max[0] += '...';
                        }
                        return '<label href="javascript: void(0)" data-toggle="tooltip" data-placement="top" title="' + row.MaxLoad + '">' + max[0] + '</label>'
                    } else {
                        return 'NA'

                    }
                }
            },
            { "data": "RollerBed", "name": "RollerBed", "autoWidth": true },
            {
                "name": "FreightType",
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    if (row.FreightType != null && row.FreightType != '') {
                        var max = row.FreightType.split(",");

                        if (max.length > 1) {
                            max[0] += '...';
                        }
                        return '<label href="javascript: void(0)" data-toggle="tooltip" data-placement="top" title="' + row.FreightType + '">' + max[0] + '</label>'
                    } else {
                        return 'NA'

                    }
                }
            },
            { "data": "LicencePlate", "name": "LicencePlate", "autoWidth": true },
            { "data": "Year", "name": "Year", "autoWidth": true },
            { "data": "VIN", "name": "VIN", "autoWidth": true },
            { "data": "Ownedby", "name": "Ownedby", "autoWidth": true },
            { "name": "Action", "autoWidth": true },
        ],
        "order": [[0, "desc"]],
        columnDefs: [
            {
                "targets": 10,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var btnEdit = '<a href="../Equipment/AddEquipment/' + row.EDID + '" class="edit_icon chng-color-edit">' +
                        '<i class="far fa-edit action-icon"></i><span class="btn btn-success action-btn grn-btn">Edit</span>'  +
                        '</a>';
                    var btnDelete = '<a href="javascript: void(0)" class="delete_icon chng-color-Trash" onclick="javascript:EquipmentDelete(' + row.EDID + ');" >' +
                        '<i class="far fa-trash-alt action-icon"></i><span class="btn btn-danger action-btn">Delete</span>' +
                        '</a>';
                    btnEdit = (isUpdate == true) ? btnEdit : "";
                    btnDelete = (isDelete == true) ? btnDelete : "";

                    return '<div class="action-ic">' + btnEdit + ' ' + btnDelete + '</div>'
                }
            },
            {
                "targets": 0,
                "visible": false,
            }
        ]
    });

    oTable = $('#tblEquipmentDetails').DataTable();



    $("input[type='search']").keyup(function () {

        oTable.search(this.value);
        oTable.draw();

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
btnViewEquipment = function () {
    $("#btnViewEquipment").on("click", function () {
        GetEquipmentList();
    })
}
//User Data Delete
function EquipmentDelete(listId) {
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
                        url: baseUrl + '/Equipment/EquipmentDelete',
                        data: { 'id': listId },
                        type: "GET",
                        success: function (data) {
                            if (data.IsSuccess == true) {
                               // toastr.success(data.Message, "")
                                SuccessPopup(data.Message)
                            }
                            else {
                                //toastr.error(data.Message, "")
                                AlertPopup(data.Message)
                            }
                            $('#tblEquipmentDetails').DataTable().clear().destroy();
                            GetEquipmentList();
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
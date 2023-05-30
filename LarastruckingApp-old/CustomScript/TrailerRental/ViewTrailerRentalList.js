$(document).ready(function () {
    GetTrailerRentalList();
});
$('#tblTrailerRentalDetail').on('dblclick', 'tbody tr', function () {
    debugger;
    var table = $('#tblTrailerRentalDetail').DataTable();
    var data_row = table.row($(this).closest('tr')).data();
    window.location.href = baseUrl + '/TrailerRental/TrailerRental/Index/' + data_row.TrailerRentalId;
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

//#region Bind shipment
function GetTrailerRentalList() {
    $('#tblTrailerRentalDetail').DataTable({
       // "bInfo": true,
         dom: 'Blfrtip',
        serverSide: true,
        buttons: [
            {
                extend: 'print',
                title: "",
                messageBottom: datetime,
                exportOptions: {
                    // columns: ':visible',
                    stripHtml: false,
                    columns: [1, 2, 3, 4, 5,6,7,8]
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

        select: 'single',
       // "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
        filter: true,
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
            "url": baseUrl + "/TrailerRental/TrailerRental/GetTrailerRentalList",
            "type": "POST",
            //"data": {},
            "async": false,
            "datatype": "json",
        },
        "columns": [
            { "data": "TrailerRentalId", "name": "TrailerRentalId", "autoWidth": true },
            { "data": "CustomerName", "name": "CustomerName", "autoWidth": true },
            { "data": "Equipment", "name": "Equipment", "autoWidth": true },
            {
                "name": "StartDate",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    if (row.StartDate != null) {
                        return '<label>' + ConvertDate(row.StartDate, true) + '</label>'
                    }
                    else {
                        return '<label></label>'
                    }

                }

            },
            {
                "name": "EndDate",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    if (row.EndDate != null) {
                        return '<label>' + ConvertDate(row.EndDate, true) + '</label>'
                    }
                    else {
                        return '<label></label>'
                    }

                }

            },
            {
                "name": "PickUpLocation",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    if (row.PickUpLocation != null) {
                        return '<label data-toggle="tooltip" data-placement="top" title="' + GetAddress(row.PickUpLocation) + '">' + GetCompanyName(row.PickUpLocation) + '</label>'
                    }
                    else {
                        return '<label></label>'
                    }

                }

            },

            //{ "data": "PickUpLocation", "name": "PickUpLocation", "autoWidth": true },
            { "data": "PickUpDriver", "name": "PickUpDriver", "autoWidth": true },
            //{ "data": "DeliveryLocation", "name": "DeliveryLocation", "autoWidth": true },
            {
                "name": "DeliveryLocation",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    if (row.DeliveryLocation != null) {
                        return '<label data-toggle="tooltip" data-placement="top" title="' + GetAddress(row.DeliveryLocation) + '">' + GetCompanyName(row.DeliveryLocation) + '</label>'
                    }
                    else {
                        return '<label></label>'
                    }

                }

            },
            { "data": "DeliveryDriver", "name": "DeliveryDriver", "autoWidth": true },
            {
                "name": "Action",
                "orderable": false,
                "autoWidth": true,
                "render": function (data, type, row, meta) {

                    var btnEdit = '<a href="' + baseUrl + '/TrailerRental/TrailerRental/Index/' + row.TrailerRentalId + '" data-toggle="tooltip" title="Edit" class="edit_icon">' +
                        '<i class="far fa-edit"></i>' +
                        '</a>';

                    var btnDelete = ' | <a href="javascript: void(0)" class="delete_icon" data-toggle="tooltip" title="Delete" onclick="javascript:DeleteTrailerRental(' + row.TrailerRentalId + ');" >' +
                        '<i class="far fa-trash-alt"></i>' +
                        '</a>';
                    btnEdit = (isUpdate == true) ? btnEdit : "";
                    btnDelete = (isDelete == true) ? btnDelete : "";

                    return '<div class="action-ic">' + btnEdit + ' ' + btnDelete + '</div>'
                }
            }
        ],
        "order": [[0, "desc"]],
        columnDefs: [

            {
                "targets": 0,
                "visible": false,
            }
        ]
    });

    var oTable1 = $('#tblTrailerRentalDetail').DataTable();

    $("input[input='search']").keyup(function () {

        oTable1.search(this.value);
        oTable1.draw();
    });
}
//#endregionfunction SplitString(text, count, insertDots) {
    return text.slice(0, count) + (((text.length > count) && insertDots) ? "..." : "");
}//#region Detail  Data Delete
function DeleteTrailerRental(listId) {

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
                        url: baseUrl + '/TrailerRental/TrailerRental/DeleteTrailerRental',
                        data: { 'trailerRentalId': listId },
                        type: "GET",
                        success: function (data) {

                            if (data.IsSuccess == true) {
                                // toastr.success(data.Message, "")
                                SuccessPopup(data.Message)
                            }
                            else {
                                //  toastr.error(data.Message, "")
                                AlertPopup(data.Message)
                            }
                            $('#tblTrailerRentalDetail').DataTable().clear().destroy();
                            GetTrailerRentalList();
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

$("table").on("mouseover", 'tr', function () {

    $(this).find(".far").css('color', 'white');
    $(this).find(".fa-map-marked-alt").css('color', 'white');

});

$("table").on("mouseout", 'tr', function () {

    $(this).find(".far").css('color', '#007bff');
    $(this).find(".fa-map-marked-alt").css('color', '007bff');
    $(this).find(".fa-trash-alt").css('color', 'red');

});
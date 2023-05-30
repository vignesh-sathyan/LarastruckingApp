
$(document).ready(function () {

    GetCustomerList();
    radioCheck();
});


//#region RADIO CHECKED FLEDGE CUSTOMER
var radioCheck = function () {
   $('input[name = "IsFullFledge"]').on("click", function () {
        var radio = $("input[name='IsFullFledge']:checked").val();
        GetCustomerList(radio);
    });
   
}
//#endregion

//#region colour change on grid icon
$("#tblcustomer").on("mouseover", 'tr', function () {

    $(this).find(".chng-color-edit").css('color', 'white');
    $(this).find(".chng-color-Trash").css('color', 'white');
});


$("#tblcustomer").on("mouseout", 'tr', function () {

    $(this).find(".chng-color-edit").css('color', '#007bff');
    $(this).find(".chng-color-Trash").css('color', 'red');

});
//#endregion

$('#tblcustomer').on('dblclick', 'tbody tr', function () {
    
    var table = $('#tblcustomer').DataTable();
    var data_row = table.row($(this).closest('tr')).data();
    window.location.href = baseUrl + '/Customer/Index/' + data_row.CustomerId;
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

function GetCustomerList(q) {

    $('#tblcustomer').DataTable({

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
                    columns: [1, 2, 3, 4]
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
            "url": baseUrl + "/Customer/LoadData/",
            "type": "POST",
            "data": { "type": q },
            "datatype": "json",
            "async": false,
        },
        "columns": [
            { "data": "CustomerId", "name": "CustomerId", "autoWidth": true },
            { "data": "CustomerName", "name": "CustomerName", "autoWidth": true },
            { "data": "BillingPhoneNumber", "name": "BillingPhoneNumber", "autoWidth": true },
            { "data": "BillingEmail", "name": "BillingEmail", "autoWidth": true },
            {
                "name": "IsActive",
                "autoWidth": true,
                "render": function (data, type, row, meta) {
                    return (row.IsActive == true) ? "ACTIVE" : "INACTIVE";
                }
            },
            { "name": "Action", "autoWidth": true },
        ],
        "order": [[0, "desc"]],
        columnDefs: [

            {
                "targets": 5,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var btnEdit = '<a href="../Customer/Index/' + row.CustomerId + '" class="edit_icon chng-color-edit" >' +
                        '<i class="far fa-edit"></i>' +
                        '</a> ';
                    var btnDelete = '<a href="javascript: void(0)" class="delete_icon chng-color-Trash" onclick="javascript:DeleteCustomer(' + row.CustomerId + ');" >' +
                        '<i class="far fa-trash-alt"></i>' +
                        '</a> ';

                    btnEdit = (isUpdate == true) ? btnEdit : "";
                    btnDelete = (isDelete == true) ? btnDelete : "";

                    return '<div class="action-ic">' + btnEdit + btnDelete + '</div>'
                }
            },
            {
                "targets": 0,
                "visible": false,
            }
        ]
    });

    oTable = $('#tblcustomer').DataTable();

    $("input[type='search']").keyup(function () {
        oTable.search(this.value);
        oTable.draw();
    });

}


//User Data Delete

function DeleteCustomer(listId) {
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
                        url: baseUrl + '/Customer/DeleteCustomer',
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
                            $('#tblcustomer').DataTable().clear().destroy();
                            GetCustomerList();
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
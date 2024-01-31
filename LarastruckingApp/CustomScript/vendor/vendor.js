$(function () {
    btnSave();
    btnClear();
    initDatatable();
});





var clearShipping = function () {
    $("#hdnVendorNconsigneeId").val("0");
    $('#txtVendorNconsigneeName').val("");
    $('#txtAddress').val("");
    $('#txtZipCode').val("");
    $('#txtCity').val("");
    $('#txtPone').val("");
    $('#txtFax').val("");
    $('#txtEmail').val("");
    $('#chkIsConsignee').prop("checked", false);
    $("#btnSave").attr("value", "SAVE");

}


//#region  Clear vendor
var btnClear = function () {
    $("#btnClear").on("click", function () {
        clearShipping();

    })
}
//#endregion







//#region SAVE Vendor
var btnSave = function () {
    $("#btnSave").on("click", function () {

        if (validateContact()) {

            addVendorAjax();

        }
    })
}
//#endregion

//#region add vendor
var vendor = function () {
    var vendorDto = {};
    vendorDto.VendorNconsigneeId = $.trim($("#hdnVendorNconsigneeId").val());
    vendorDto.VendorNconsigneeName = $.trim($("#txtVendorNconsigneeName").val());
    vendorDto.Address = $.trim($("#txtAddress").val());
    vendorDto.Country = $.trim($("#ddlCountry").val());
    vendorDto.State = $.trim($("#ddlState").val());
    vendorDto.Zip = $.trim($("#txtZipCode").val());
    vendorDto.City = $.trim($("#txtCity").val());
    vendorDto.Phone = $.trim($("#txtPone").val());

    vendorDto.Fax = $.trim($("#txtFax").val());
    vendorDto.Email = $.trim($("#txtEmail").val());
    if ($('#chkIsConsignee').is(":checked")) {
        vendorDto.IsConsignee = true;
    }
    else {
        vendorDto.IsConsignee = false;
    }


    return vendorDto;
}
//#endregion

//#region ADD Vendor AJAX CALL

var addVendorAjax = function () {
    
    var vendorModel = vendor();
    
    $.ajax({
        url: baseUrl + "Vendor/index",
        contentType: "application/json;charset=utf-8",
        type: "POST",
        data: JSON.stringify(vendorModel),
        dataType: "json",
        beforeSend: function (xhr, settings) {
            showLoader();
        },
        success: function (data, textStatus, jqXHR) {
            if (data.IsSuccess == true) {
                toastr.success(data.Message);
                clearShipping();
                initDatatable();
                $("#btnSave").attr("value", "SAVE");
            }
            else {
                toastr.warning(data.Message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {

        },
        complete: function () {
            hideLoader();
        }
    });
}

//#endregion


//User Data Edit
function EditVendor(listId) {
    $.ajax({
        url: baseUrl + '/Vendor/EditVendor',
        data: { 'id': listId },
        type: "POST",
        success: function (data) {
            $("#hdnVendorNconsigneeId").val(listId);
            $("#txtVendorNconsigneeName").val(data.VendorNconsigneeName);
            $("#txtAddress").val(data.Address);
            $("#txtPone").val(data.Phone);
            $("#txtCity").val(data.City);
            $("#ddlState").val(data.State);
            $("#ddlCountry").val(data.Country);
            $("#txtEmail").val(data.Email);
            $("#txtZipCode").val(data.Zip);
            $("#txtFax").val(data.Fax);

            if (data.IsConsignee) {
                $('#chkIsConsignee').prop("checked", true)
            }
            else {
                $('#chkIsConsignee').prop("checked", false)
            }
            $("#btnSave").attr("value", "Update");

        }
    });
}
//User Data Delete
function DeleteVendor(listId) {
    $.confirm({
        title: 'Confirmation!',
        content: '<b>Are you sure you want to Delete ?</b> ',
        type: 'red',
        typeAnimated: true,
        buttons: {
            confirm: {
                btnClass: 'btn-blue',
                action: function () {
                    $.ajax({
                        url: baseUrl + '/Vendor/DeleteVendor',
                        data: { 'id': listId },
                        type: "GET",
                        success: function (data) {

                            if (data.IsSuccess == true) {
                                toastr.success(data.Message);
                                initDatatable();
                            }
                            else {
                                toastr.error(data.Message);
                            }
                        },
                        error: function () {
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

var initDatatable = function () {
    $('#tblVendor').DataTable({
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
            "url": baseUrl + "/Vendor/LoadData",
            "type": "POST",
            "datatype": "json",
        },
        "columns": [
            { "data": "VendorNconsigneeId", "name": "VendorNconsigneeId", "autoWidth": true },
            { "data": "VendorNconsigneeName", "name": "VendorNconsigneeName", "autoWidth": true },

            { "data": "Address", "name": "Address", "autoWidth": true },
            { "data": "City", "name": "City", "autoWidth": true },
            { "data": "StateName", "name": "StateName", "autoWidth": true },
            { "data": "Zip", "name": "Zip", "autoWidth": true },
            { "data": "Email", "name": "Email", "autoWidth": true },
            { "data": "Phone", "name": "Phone", "autoWidth": true },

            { "data": "Action", "autoWidth": true },
        ],
        "order": [[0, "desc"]],
        columnDefs: [
            {
                "targets": 8,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    var btnEdit = '<a href="javascript: void(0)" class="edit_icon" onclick= "javascript:EditVendor(' + row.VendorNconsigneeId + ');" >' +
                        '<i class="far fa-edit"></i> | ' +
                        '</a> ';
                    var btnDelete = '<a href="javascript: void(0)" class="delete_icon" onclick="javascript:DeleteVendor(' + row.VendorNconsigneeId + ');" >' +
                        '<i class="far fa-trash-alt"></i>' +
                        '</a> ';

                    btnEdit = (isInsert == true) ? btnEdit : "";
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

    oTable = $('#tblAddress').DataTable();

    $("input[type='search']").keyup(function () {
        // console.log(this.value);

        oTable.search(this.value);
        oTable.draw();
    });


}

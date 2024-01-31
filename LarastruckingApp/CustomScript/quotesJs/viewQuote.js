$(document).ready(function () {
    bindCustomerDropdown();
    GetQuoteList();
    btnViewQuote();
    startEndDate();

});

$("table").on("mouseover", 'tr', function () {

    $(this).find(".far").css('color', 'white');
    $(this).find(".fa-map-marked-alt").css('color', 'white');
});

$("table").on("mouseout", 'tr', function () {

    $(this).find(".far").css('color', '#007bff');
    $(this).find(".fa-download").css('color', '#007bff');
    $(this).find(".fa-map-marked-alt").css('color', '007bff');
    $(this).find(".fa-trash-alt").css('color', 'red');

});

$('#tblQuoteDetails').on('dblclick', 'tbody tr', function () {
    
    var table = $('#tblQuoteDetails').DataTable();
    var data_row = table.row($(this).closest('tr')).data();
    window.location.href = baseUrl + '/Quote/Quote/Index/' + data_row.QuoteId;
});
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
function GetQuoteList() {
    var values = {};
    values.QuoteDate = $("#dtQuoteDate").val();
    values.ValidUptoDate = $("#dtValidUpto").val();
    values.CustomerId = $("#ddlCustomer").val();
    $('#tblQuoteDetails').DataTable({
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
            "url": baseUrl + "/Quote/Quote/LoadData",
            "type": "POST",
            "data": values,
            "async": false,
            "datatype": "json",
            "async": false,
        },
        "columns": [
            { "data": "QuoteId", "name": "QuoteId", "autoWidth": true },
            { "data": "CustomerName", "name": "CustomerName", "autoWidth": true },
            { "data": "QuotesName", "name": "QuotesName", "autoWidth": true },
            { "data": "DateRange", "autoWidth": true },
            { "data": "CreatedBy", "name": "CreatedBy", "autoWidth": true },
            { "data": "QuoteStatus", "name": "QuoteStatus", "autoWidth": true },
            { "name": "Action", "autoWidth": true },
        ],
        "order": [[0, "desc"]],
        columnDefs: [
            {
                "targets": 3,
               // "orderable": false,
                "render": function (data, type, row, meta) {
                    return '<label>(' + ConvertDate(row.QuoteDate) + ')</label> to <label>(' + ConvertDate(row.ValidUptoDate) + ')</label>'
                }
            },
            {
                "targets": 6,
                "orderable": false,
                "render": function (data, type, row, meta) {

                    var btnEdit = '<a href="' + baseUrl + '/Quote/Quote/Index/' + row.QuoteId + '" data-toggle="tooltip" title="Edit" class="edit_icon">' +
                        '<i class="far fa-edit"></i>' +
                        '</a> |';
                    var btnDelete = '<a href="javascript: void(0)" class="delete_icon" data-toggle="tooltip" title="Delete" onclick="javascript:QuoteDelete(' + row.QuoteId + ');" >' +
                        '<i class="far fa-trash-alt"></i>' +
                        '</a> | ';
                    var btnView = '<a href="javascript: void(0)" class="send_icon" data-toggle="tooltip" title="Send Quote" onclick="javascript:SendMail(' + row.QuoteId + ');" >' +
                        '<i class="far fa-envelope"></i>' +
                        '</a>';
                    var btnPreview = '<a href="' + baseUrl + '/Quote/Quote/ViewAndSendQuote/' + row.QuoteId + '" title="Preview Quote" target="_blank" id="btnPreview">' +
                        '<i class="far fa-eye"></i>' +
                        '</a> |';
                    btnEdit = (isUpdate == true) ? btnEdit : "";
                    btnDelete = (isDelete == true) ? btnDelete : "";
                    btnView = (isView == true) ? btnView : "";
                    btnPreview = (isView == true) ? btnPreview : "";
                    return '<div class="action-ic">' + btnPreview + ' ' + btnEdit + ' ' + btnDelete + ' ' + btnView + ' </div>'

                }
            },
            {
                "targets": 0,
                "visible": false,
            }
        ]
    });

    oTable = $('#tblQuoteDetails').DataTable();

    $("input[type='search']").keyup(function () {

        oTable.search(this.value);
        oTable.draw();
    });
}


//#endregion 

//Detail  Data Delete
function QuoteDelete(listId) {

    $.confirm({
        title: 'Confirmation!',
        content: "<b>Are you sure you want to Delete ?</b> ",
        type: 'blue',
        typeAnimated: true,
        buttons: {
            delete: {
                btnClass: 'btn-green',
                action: function () {
                    $.ajax({
                        url: baseUrl + '/Quote/Quote/DeleteQuote',
                        data: { 'id': listId },
                        type: "GET",
                        success: function (data) {

                            if (data.IsSuccess == true) {
                                //toastr.success(data.Message, "")
                                SuccessPopup(data.Message)
                            }
                            else {
                                //toastr.error(data.Message, "")
                                AlertPopup(data.Message)
                            }

                            GetQuoteList();


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

//#region send mail
function SendMail(quoteid) {
    $.confirm({
        title: 'Confirmation!',
        content: "<b>Please preview the Quote before sending it.</b> ",
        type: 'blue',
        typeAnimated: true,
        buttons: {
            Continue: {
                btnClass: 'btn-green',
                action: function () {

                    $.ajax({
                        url: baseUrl + '/Quote/Quote/SendQuoteEmail',
                        data: { 'quoteid': quoteid },
                        type: "GET",
                        beforeSend: function () {
                            showLoader();
                        },
                        success: function (data) {
                            if (data.IsSuccess == true) {
                               //toastr.success(data.Message, "")
                                SuccessPopup(data.Message)
                                hideLoader();
                            }
                            else {
                                //toastr.error(data.Message, "");
                                AlertPopup(data.Message);
                                hideLoader();
                            }
                            GetQuoteList();
                        },
                        complete: function () {
                            hideLoader();
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
//#endregion

//#region function for apply selectize on customer dropdown
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
//#endregion
btnViewQuote = function () {
    $("#btnViewShipment").on("click", function () {
        GetQuoteList();
    })
}
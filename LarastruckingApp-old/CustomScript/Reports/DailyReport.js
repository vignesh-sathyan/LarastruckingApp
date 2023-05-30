$(document).ready(function () {

    //GetDailyReportShipmentList();
    bindCustomerDropdown();
    shipmentStatus();
    GetFreightType();

    $('.multi').multi_select({
        data: $("#ddlStatusType").val()
    });
  
});

//#region shipment status
function shipmentStatus() {

    $.ajax({
        url: baseUrl + '/DailyReport/GetReportStatus',
        data: {},
        type: "GET",
        async: false,
        success: function (data) {

            var ddlValue = "";
            $("#ddlStatusType").empty();
            for (var i = 0; i < data.length; i++) {
                ddlValue += '<option value="' + data[i].StatusId + '">' + data[i].StatusName + '</option>';
            }
            $("#ddlStatusType").append(ddlValue);
          
        }
       
    });
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
//#region bind freight type dropdownlist
function GetFreightType() {


    $.ajax({
        url: baseUrl + 'Shipment/Shipment/BindFreightType',
        data: {},
        type: "Post",
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            var ddlValue = "";
            $("#ddlFreightType").empty();
            $("#ddlFreightType2").empty();
            ddlValue += '<option value="0">Select Freight Type</option>'
            for (var i = 0; i < data.length; i++) {
                ddlValue += '<option value="' + data[i].FreightTypeId + '">' + data[i].FreightTypeName + '</option>';
            }
            $("#ddlFreightType").append(ddlValue);
            $("#ddlFreightType2").append(ddlValue);
        }
    });

}
//#endregion

//#region Bind Daily Report shipment details
function GetDailyReportShipmentList() {

    var values = {};
    values.StartDate = $("#dtStartedDate").val();
    values.EndDate = $("#dtEndDate").val();
    values.CustomerId = $("#ddlCustomer").val();
    values.FreightTypeId = $("#ddlFreightType").val();
    values.statusId = $("#ddlStatusType").val();

    $('#tblShipmentDetails').DataTable({
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
            "url": baseUrl + "/Reports/DailyReport/GetDailyReport",
            "type": "POST",
            "data": values,
            "async": false,
            "datatype": "json",
        },
        "columns": [
            { "data": "ShipmentId", "name": "ShipmentId", "autoWidth": true },
            { "data": "StatusName", "name": "StatusName", "autoWidth": true },
            { "data": "CustomerName", "name": "CustomerName", "autoWidth": true },
            { "data": "PickUpAddress", "name": "PickUpAddress", "autoWidth": true },
            { "data": "PickUpArrivalDate", "name": "PickUpArrivalDate", "autoWidth": true },
            { "data": "DeliveryAddress", "name": "DeliveryAddress", "autoWidth": true },
            { "data": "DeliveryArrive", "name": "DeliveryArrive", "autoWidth": true },
            { "data": "AirWayBill", "name": "AirWayBill", "autoWidth": true },
            { "data": "CustomerPO", "name": "CustomerPO", "autoWidth": true },
            { "data": "DriverName", "name": "DriverName", "autoWidth": true },
            { "data": "DriverEquipment", "name": "DriverEquipment", "autoWidth": true },
            { "data": "ShipmentRefNo", "name": "ShipmentRefNo", "autoWidth": true },
            { "data": "QuantityNMethod", "name": "QuantityNMethod", "autoWidth": true },
            { "name": "Action", "autoWidth": true },
        ],
        "order": [[0, "desc"]],
        columnDefs: [
            {
                "targets": 1,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    return StatusCheckForShipment(row.StatusName)
                }
            },

            

            {
                "targets": 4,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    return '<label>' + ConvertDate(row.PickUpArrivalDate, true) + '</label>'
                }
            },
           
            {
                "targets": 6,
                "orderable": false,
                "render": function (data, type, row, meta) {
                    return '<label>' + ConvertDate(row.DeliveryArrive, true) + '</label>'
                }
            },

              {
                "targets": 13,
                "orderable": false,
                "render": function (data, type, row, meta) {



                    var btnEdit = '<a href="' + baseUrl + '/Shipment/Shipment/Index/' + row.ShipmentId + '" data-toggle="tooltip" title="Edit" class="edit_icon">' +
                        '<i class="far fa-edit"></i>' +
                        '</a>';
                    var btnDelete = ' | <a href="javascript: void(0)" class="delete_icon" data-toggle="tooltip" title="Delete" onclick="javascript:DeleteShipment(' + row.ShipmentId + ');" >' +
                        '<i class="far fa-trash-alt"></i>' +
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

    var oTable1 = $('#tblShipmentDetails').DataTable();

    $("input[input='search']").keyup(function () {

        oTable1.search(this.value);
        oTable1.draw();
    });
}
//#endregion//#region Status Colour Change

function StatusCheckForShipment(status) {

    var PreStatus = "";
    if ($.trim(status) == "Order Taken") {
        PreStatus = '<span style=""">' + status + '</span>'
    }
    if ($.trim(status) == "Dispatched") {
        PreStatus = '<span class="badge" style="background-color:#0071c3;color:white">' + status + '</span>'
    }
    if ($.trim(status) == "Immediate Attention") {
        PreStatus = '<span class="badge" style="background-color:#f10802;color:white">' + status + '</span>'
    }
    if ($.trim(status) == "Change of Status / On Hold") {
        PreStatus = '<span class="badge" style="background-color:#e8610b;color:white">' + status + '</span>'
    }
    if ($.trim(status) == "Loading at pick up location") {
        PreStatus = '<span class="badge" style="background-color:#f86807;color:white">' + status + '</span>'
    }
    if ($.trim(status) == "In-Route to delivery location") {
        PreStatus = '<span class="badge" style="background-color:#fffd01;color:black">' + status + '</span>'
    }
    if ($.trim(status) == "Delivered") {
        PreStatus = '<span class="badge" style="background-color:#178a24;color:white">' + status + '</span>'
    }
    if ($.trim(status) == "Cancelled") {
        PreStatus = '<span class="badge" style="background-color:#d93d42;color:white">' + status + '</span>'
    }
    if ($.trim(status) == "In-Progress") {
        PreStatus = '<span class="badge" style="background-color:#95ccdb;color:white">' + status + '</span>'
    }
    return PreStatus;
}

//#endregion
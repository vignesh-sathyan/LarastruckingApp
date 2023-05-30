
//#region READY FUNCTION
$(function () {
    applyLeaves();
    getPendingApprovedLeaves();

})
//#endregion

//#region colour change on grid icon
$("#tblLeave").on("mouseover", 'tbody tr', function () {

    $(this).find(".chng-color-edit").css('color', 'white');
    $(this).find(".chng-color-Trash").css('color', 'white');
});


$("#tblLeave").on("mouseout", 'tbody tr', function () {

    $(this).find(".chng-color-edit").css('color', '#007bff');
    $(this).find(".chng-color-Trash").css('color', 'red');

});
    //#endregion


//#region LEAVE PARAMETERS
var parameters = function () {
    var dto = {}
    var DriverLeave = {}

    DriverLeave.LeaveId = $.trim($('#DriverLeave_LeaveId').val())
    DriverLeave.UserId = $.trim($('#DriverLeave_UserId').val())
    DriverLeave.TakenFrom = $.trim($('#txtTakenFrom').val())
    DriverLeave.TakenTo = $.trim($('#txtTakenTo').val())
    DriverLeave.Reason = $.trim($('#txtReason').val())
    DriverLeave.LeaveStatusId = $.trim($('#DriverLeave_LeaveStatusId').val())
    DriverLeave.FirstName = $.trim($('#DriverLeave_FirstName').val())
    DriverLeave.LastName = $.trim($('#DriverLeave_LastName').val())
    DriverLeave.Email = $.trim($('#DriverLeave_Email').val())
    dto.DriverLeave = DriverLeave;
    return dto;
}
//#endregion

//#region APPLY LEAVES
var applyLeaves = function () {
    $('#BtnAdd').on('click', function () {
        if (validateContact()) {
            addAjax();
        }
    });
}
//#endregion

//#region RESET VALUES
var resetValues = function () {
    $("#DriverLeave_LeaveId").val("0");
    $("#DriverLeave_LeaveStatusId").val("1");
    $("#txtTakenFrom").val("");
    $("#txtTakenTo").val("");
    $("#txtReason").val("");
    $("#BtnAdd").text("Apply");
}
//#endregion

//#region GLOBAL DECLARATIONS
var ArrayLeaveStatus = ['Pending', 'Approved', 'Unapproved', 'Cancelled'];
var ArrayRoles = ['Accounting', 'Customer', 'Dispatcher', 'Driver', 'Management', 'Mechanics'];

var btnEditHtml = function (LeaveId) {
    return '<a href="javascript: void(0)" title="Edit" class="edit_icon" onclick="javascript:editBtnClick(' + LeaveId + ');" > <i class="far fa-edit"></i> </a> '
}

var btnCancelHtml = function (LeaveId) {
    return '<a href="javascript: void(0)" title="Cancel leave" class="edit_icon" onclick="javascript:cancelBtnClick(' + LeaveId + ');" > Cancel </a> '
}
//#endregion

//#region DRIVER CONDITION CHECK

var driverCondition = function (isDriver, dtTakenFrom, dtTodayDate, leaveStatus) {

    var dtFrom = ConvertDate(dtTakenFrom, true);
    var dtNow = ConvertDate(dtTodayDate, true);

    var response = {
        isAllowed: false,
        button: ''
    };
}

//#endregion

//#region ADD LEAVE AJAX CALL
var addAjax = function () {
    var entityModel = parameters();
    $.ajax({
        url: baseUrl + "Driver/Leave",
        contentType: "application/json;charset=utf-8",
        type: "POST",
        data: JSON.stringify(entityModel),
        dataType: "json",
        beforeSend: function (xhr, settings) {
            showLoader();
        },
        success: function (data, textStatus, jqXHR) {
            if (data.DriverLeave.IsSuccess == true) {
                resetValues();

                toastr.success(data.DriverLeave.Response);
                getPendingApprovedLeaves();
            }
            else {
                toastr.warning(data.DriverLeave.Response);
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

//#region GET PENDING-APPROVED LEAVES
var getPendingApprovedLeaves = function () {
    var id = $('#DriverLeave_UserId').val();
    if (id != 0) {
        $.ajax({
            url: baseUrl + 'Driver/GetApprovedPendingLeaves/' + id,
            type: 'GET',
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr, settings) {
            },
            success: function (data, textStatus, jqXHR) {

                if (data.leaves.length) {
                    $("#tblLeave tbody").html("");
                    $.each(data.leaves, function (index, value) {
                        var leave = "";
                        if (value.LeaveStatus == "Approved") {

                            leave = '<span class="badge badge-success">' + value.LeaveStatus + '</span>'
                        }
                        if (value.LeaveStatus == "Unapproved") {

                            leave = '<span class="badge badge-danger">' + value.LeaveStatus + '</span>'
                        }
                        else if (value.LeaveStatus == "Pending") {

                            leave = '<span class="badge badge-warning">' + value.LeaveStatus + '</span>'
                        }
                        else if (value.LeaveStatus == "Cancelled") {

                            leave = '<span class="badge badge-default">' + value.LeaveStatus + '</span>'
                        }

                        var dtFrom = ConvertDate(value.TakenFrom, true);
                        var dtNow = ConvertDate(value.TodayDate, true);
                        

                        var editControl = '<a href="javascript: void(0)" title="Edit" class="edit_icon chng-color-edit" onclick="javascript:editBtnClick(' + value.LeaveId + ');" >' +
                            '<i class="far fa-edit"></i>' +
                            '</a> ';


                        if (data.isDriver) {
                            var isEditable = (dtFrom >= dtNow) ? true : false;

                            if (isEditable && value.LeaveStatus == "Approved") {
                                editControl = btnCancelHtml(value.LeaveId);
                            }

                            if (value.LeaveStatus == "Unapproved" || value.LeaveStatus == "Cancelled") {
                                editControl = "";
                            }
                        }
                        else {
                            var isEditable = true;
                        }

                        // Enable editing feature of leave only if leave taken date is greater than today's date 

                        // in-case of driver-role also check if leave status is not equal to unapproved and approved

                        editControl = (isEditable == true) ? editControl : editControl = '';

                        //if (data.isDriver && (value.LeaveStatus == "Unapproved" || value.LeaveStatus == "Approved")) {
                        //    
                        //    editControl = '';
                        //}

                        var html = '<tr>' +
                            '<td> <label class="TakenFrom">' + ConvertDate(value.TakenFrom, true) + '</label></td>' +
                            '<td> <label class="TakenTo"> ' + ConvertDate(value.TakenTo, true) + '</label></td>' +
                            '<td> <label class="Reason"> ' + value.Reason + ' </label></td>' +
                            '<td> <label class="ContactExtension"> ' +
                            leave +
                            ' </label></td>' +
                            '<td>' +
                            '<div class="action-ic">' + editControl + '</div>' +
                            '</td>' +
                            '</tr>';


                        $("#tblLeave tbody").append(html);
                    });
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            },
            complete: function () {
            }

        });
    }
}
//#endregion

//#region EDIT LEAVE
var editBtnClick = function (id) {
    $.ajax({
        url: baseUrl + "Driver/LeaveEdit/" + id,
        contentType: "application/json;charset=utf-8",
        type: "GET",
        dataType: "json",
        beforeSend: function (xhr, settings) {
        },
        success: function (data, textStatus, jqXHR) {
            $("#DriverLeave_LeaveId").val(data.LeaveId);
            $("#txtTakenFrom").val(ConvertDate(data.TakenFrom, true));
            $("#txtTakenTo").val(ConvertDate(data.TakenTo, true));
            $("#txtReason").val(data.Reason);
            $("#DriverLeave_LeaveStatusId").val(data.LeaveStatusId);
            $("#BtnAdd").text("Update");
        },
        error: function (jqXHR, textStatus, errorThrown) {
        },
        complete: function () {
        }
    });

}
//#endregion

//#region CANCEL LEAVE
var cancelBtnClick = function (id) {
    $.ajax({
        url: baseUrl + "Driver/CancelLeave/" + id,
        contentType: "application/json;charset=utf-8",
        type: "POST",
        dataType: "json",
        beforeSend: function (xhr, settings) {
            showLoader();
        },
        success: function (data, textStatus, jqXHR) {
            if (data == true) {
                toastr.success("Leave cancelled successfully.");
                getPendingApprovedLeaves();
            }
            else {
                toastr.warning("Unable to process your request.");
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

//#region DELETE LEAVE
var deleteBtnClick = function (id) {

    if (confirm("Are you sure you want to Delete ?")) {
        $.ajax({
            url: baseUrl + "Driver/LeaveDelete/" + id,
            contentType: "application/json;charset=utf-8",
            type: "GET",
            dataType: "json",
            beforeSend: function (xhr, settings) {
            },
            success: function (data, textStatus, jqXHR) {
                if (data.IsSuccess == true) {
                    toastr.success(data.Message);
                    getPendingApprovedLeaves();
                }
                else {
                    toastr.warning(data.Message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            },
            complete: function () {
            }
        });
        return true;
    } else {
        return false;
    }
}
//#endregion

//#region Leave Dates 

function TakenFrom() {
    var TakenFrom = $("#txtTakenFrom").val();
    var TakenTo = $("#txtTakenTo").val();

    if (TakenFrom != "" && TakenTo != "") {


        if (TakenFrom < TakenTo) {

        }
        else {
            toastr.warning("Please select valid date.")
            $("#txtTakenFrom").val("");
        }
    }

}

function TakenTo() {
    var TakenFrom = $("#txtTakenFrom").val();
    var TakenTo = $("#txtTakenTo").val();
    if (TakenFrom != "" && TakenTo != "") {
        if (TakenFrom < TakenTo) {

        }
        else {
            toastr.warning("Please select valid date.")
            $("#txtTakenTo").val("");
        }
    }
    

}
//#endregion   

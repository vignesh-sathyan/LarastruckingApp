
$(document).ready(function () {
    //Florida=>3929 as default value
    $("#ddlState").val(3929);
});

$("#btnSave").click(function () {
    if (validateContact()) {
        $("form").submit(); // Submit the form
    }
});

function GetCountryList() {

    $.ajax({
        url: baseUrl +'/User/DropDownRoleBind',
        type: 'GET',
        success: function (data) {
            $.each(data, function (key, value) {
                $("#ddlRole").append($("<option></option>").val(value.RoleID).html(value.RoleName));
            });
            console.log(data);
        }
    })
}

function GetList() {
    $.ajax({
        url: baseUrl +'/User/GetUserlist',
        type: 'GET',
        success: function (data) {
            $("#divUserList").empty();
            $("#divUserList").append(data);
            console.log(data);
        }
    })
}




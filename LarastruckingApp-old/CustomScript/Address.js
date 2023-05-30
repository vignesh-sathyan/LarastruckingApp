var isNeedToloaded = true;
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

$(document).ready(function () {
    if ($("#AddressId").val() > 0) {
        $("#BtnSubmit").attr("value", "Update");
    }
    else {
        $("#BtnSubmit").attr("value", "Save");
        //Florida=>3929 as default value
        $("#ddlState").val(3929);
    }
});

//User Data Edit
function EditAddress(listId) {
    $.ajax({
        url: baseUrl + '/Address/EditAddress',
        data: { 'id': listId },
        type: "POST",
        success: function (data) {
            $("#AddressTypeId").focus();
            $("#AddressId").val(listId);
            $("#AddressTypeId").val(data.AddressTypeId);
            $("#Address1").val(data.Address1);
            $("#Address2").val(data.Address2);
            $("#City").val(data.City);
            $("#ddlState").val(data.State);
            $("#ddlCountry").val(data.Country);
            $("#ContactPerson").val(data.ContactPerson);
            $("#Phone").val(data.Phone);
            $("#Extension").val(data.Extension);
            $("#Email").val(data.Email);
            $("#Zip").val(data.Zip);

            $("#CompanyName").val(data.CompanyName);
            $("#AdditionalPhone1").val(data.AdditionalPhone1);
            $("#Extension1").val(data.Extension1);
            $("#AdditionalPhone2").val(data.AdditionalPhone2);
            $("#Extension2").val(data.Extension2);
            $("#Comments").val(data.Comments);
            if (data.IsAppointmentRequired == true) {
                $("#IsAppointmentRequired").prop("checked", true);
            }
            else {
                $("#IsAppointmentRequired").prop("checked", false);
            }
            $("#Website").val(data.Website);
            $("#CompanyNickname").val(data.CompanyNickname);

            $("#BtnSubmit").attr("value", "Update");

        }
    });
}
//User Data Delete
function DeleteAddress(listId) {
    $.confirm({
        title: 'Confirm!',
        content: '<b>Are you sure you want to Delete ?</b>',
        type: 'red',
        typeAnimated: true,
        buttons: {
            Delete: {
                btnClass: 'btn-blue',
                action: function () {
                    $.ajax({
                        url: baseUrl + '/Address/DeleteAddress',
                        data: { 'id': listId },
                        type: "GET",
                        success: function (data) {
                            if (data.IsSuccess == true) {
                                //toastr.success(data.Message, "")
                                SuccessPopup(data.Message)

                            }
                            else {
                               // toastr.error(data.Message, "")
                                AlertPopup(data.Message)
                            }
                            $('#tblAddress').DataTable().clear().destroy();
                            initDatatable();
                        }
                    });


                }
            },
            cancel: {
                
            }

        }
    });
}

function GetAddressList() {

    $.ajax({
        url: baseUrl + '/Address/GetAddressList',
        type: 'GET',
        success: function (data) {
            debugger
            $("#divAddressList").empty();
            $("#divAddressList").append(data);

        }
    })
}
function GetCityList() {

    var stateId = $("#ddState").val();

    $.ajax({
        url: baseUrl + '/Address/GetCityList',
        data: { 'StateId': stateId },
        type: "POST",
        // cache: false,
        success: function (data) {

            var ddlValue = "";
            $("#ddlcity").empty();

            ddlValue += '<option value="0">Please Select City</option>'
            for (var i = 0; i < data.length; i++) {
                ddlValue += '<option value="' + data[i].ID + '">' + data[i].Name + '</option>';
            }
            $("#ddlcity").append(ddlValue);
        }
    });
}
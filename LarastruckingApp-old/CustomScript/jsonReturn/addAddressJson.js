$(function () {
    btnSaveAddress();
});


var btnSaveAddress = function () {
    $("#BtnSubmit").on("click", function () {
        
        if (isFormValid("formAddress")) {
            addAddress();
        }
    });
}

var addAddress = function () {
    addressModel = {};
    addressModel.AddressTypeId = $("#AddressTypeId").val();
    addressModel.CompanyName = $("#CompanyName").val();
    addressModel.Email = $("#Email").val();
    addressModel.Phone = $("#Phone").val();

    addressModel.ContactPerson = $("#ContactPerson").val();
    addressModel.Extension = $("#Extension").val();

    addressModel.Address1 = $("#Address1").val();
    addressModel.Address2 = $("#Address2").val();
    addressModel.City = $("#City").val();
    addressModel.State = $("#ddlState").val();
    addressModel.Country = $("#ddlCountry").val();
    addressModel.Zip = $("#Zip").val();
    addressModel.AdditionalPhone1 = $("#AdditionalPhone1").val();
    addressModel.Extension1 = $("#Extension1").val();
    addressModel.AdditionalPhone2 = $("#AdditionalPhone2").val();
    addressModel.Extension2 = $("#Extension2").val();


    $.ajax({
        url: baseUrl + "Global/AddAddress",
        contentType: "application/json;charset=utf-8",
        type: "POST",
        data: JSON.stringify(addressModel),
        dataType: "json",
        beforeSend: function (xhr, settings) {
        },
        success: function (data, textStatus, jqXHR) {
            if (data.IsSuccess == true) {
               // toastr.success(data.Message);
                SuccessPopup(data.Message);
            }
            else {
                //toastr.warning(data.Message);
                AlertPopup(data.Message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            
        },
        complete: function () {
            $("#modalAddAddress").modal("hide");
        }
    });
}
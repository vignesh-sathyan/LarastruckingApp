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
$(function () {
    btnSave();
    btnMultipleContact();
    copyAddress();
   
});

$("#ddlBillingCountry").change(function () {
    bindBillingState();
});
$("#ddlShippingCountry").change(function () {
    bindShippingState();
});
//#region shipment status
function bindBillingState() {
    var countryId = $("#ddlBillingCountry").val();
    
    $.ajax({
        url: baseUrl + 'Customer/GetStates',
        data: {"countryId": countryId},
        type: "GET",
        async: false,
        success: function (data) {

            var ddlValue = "";
            $("#ddlBillingState").empty();
            for (var i = 0; i < data.length; i++) {
                ddlValue += '<option value="' + data[i].ID + '">' + $.trim(data[i].Name).toUpperCase() + '</option>';
            }
            $("#ddlBillingState").append(ddlValue);

        }
    });
}

function bindShippingState() {
    var countryId = $("#ddlShippingCountry").val();
    
    $.ajax({
        url: baseUrl + 'Customer/GetStates',
        data: { "countryId": countryId },
        type: "GET",
        async: false,
        success: function (data) {

            var ddlValue = "";
            $("#ddlShippingState").empty();
            for (var i = 0; i < data.length; i++) {
                ddlValue += '<option value="' + data[i].ID + '">' + $.trim(data[i].Name).toUpperCase() + '</option>';
            }
            $("#ddlShippingState").append(ddlValue);

        }
    });
}
//#endregion

//#region CHECKBOX SAME SHIPPING ADDRESS
var copyAddress = function () {
    $('#chkSameAddress').on("click", function () {

        if ($(this).is(":checked")) {
            copyBillToShipping();
        }
        else {
            clearShipping();
        }
    });
}

var copyBillToShipping = function () {
    var billingAddress1 = $.trim($('#txtBillingAddress1').val())
    var billingAddress2 = $.trim($('#txtBillingAddress2').val())
    var billingCity = $.trim($('#txtBillingCity').val())
    var billingStateId = $.trim($('#ddlBillingState').val())
    var billingZipCode = $.trim($('#txtBillingZip').val())
    var billingCountryId = $.trim($('#ddlBillingCountry').val())
    var billingPhoneNumber = $.trim($('#txtBillingPhone').val())
    var billingFax = $.trim($('#txtBillingFax').val())
    var billingEmail = $.trim($('#txtBillingEmail').val())
    var billingExtension = $.trim($('#txtBillingExtension').val())

    $('#txtShippingAddress1').val(billingAddress1);
    $('#txtShippingAddress2').val(billingAddress2);
    $('#txtShippingCity').val(billingCity);
   
    $('#txtShippingZip').val(billingZipCode);
    $('#ddlShippingCountry').val(billingCountryId);
    bindShippingState();
    $('#ddlShippingState').val(billingStateId);
    $('#txtShippingPhone').val(billingPhoneNumber);
    $('#txtShippingFax').val(billingFax);
    $('#txtShippingEmail').val(billingEmail);
    $('#txtShippingExtension').val(billingExtension);
}

var clearShipping = function () {
    $('#txtShippingAddress1').val("");
    $('#txtShippingAddress2').val("");
    $('#txtShippingCity').val("");
    $('#ddlShippingState').val("");
    $('#txtShippingZip').val("");
    $('#ddlShippingCountry').val("");
    $('#txtShippingPhone').val("");
    $('#txtShippingFax').val("");
    $('#txtShippingEmail').val("");
    $('#txtShippingExtension').val("");
}


//#endregion

function validateEmail() {
      // Get the input value
      var emailInput = document.getElementById('txtEmail').value;

      // Regular expression for email validation
      var emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

      // Test the input value against the pattern
      if (emailPattern.test(emailInput)) {
       // AlertPopup('Valid email format');
      } else {
        AlertPopup('Invalid email format');
      }
    }

//#region BUTTON MULTIPLE CONTACT
var btnMultipleContact = function () {
    $("#btnMultipleContact").on("click", function () {
       if($.trim($('#txtFirstName').val())!="" && $.trim($('#txtLastName').val())!="" && $.trim($('#txtPhone').val()) && $.trim($('#txtEmail').val())){
			if(!$("#txtPhone").val().match(/^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/)){
				AlertPopup("Enter valid Phone number");
			}
			validateEmail();
				
			 if (isFormValid("customerContactForm")) {
            var fName = $.trim($('#txtFirstName').val());
            var lName = $.trim($('#txtLastName').val());
            var phone = $.trim($('#txtPhone').val());
            var extension = $.trim($('#txtExtension').val());
            var email = $.trim($('#txtEmail').val());
            var title = $.trim($('#txtTitle').val());

            var params = {
                ContactFirstName: fName,
                ContactLastName: lName,
                ContactPhone: phone,
                ContactExtension: extension,
                ContactEmail: email,
                ContactTitle: title
            }

            $('#tblMultipleContacts tbody').append(tr(params));
            clearMultipleContactInputs();
        }
		}
		else{
			AlertPopup("Please fill all required fields");
		}
        
    })
}

var clearMultipleContactInputs = function () {

    var inputs = $('#customerContactForm').find('input');
    $.each(inputs, function (index, value) {
        $(this).val('');
    })
}
//#endregion

//#region CREATE TABLE TR
var tr = function (lableValues) {
    var html = '<tr>' +
        '<td> <label class="ContactFirstName">' + lableValues.ContactFirstName + '</label></td>' +
        '<td> <label class="ContactLastName"> ' + lableValues.ContactLastName + '</label></td>' +
        '<td> <label class="ContactPhone"> ' + lableValues.ContactPhone + ' </label></td>' +
        '<td> <label class="ContactExtension"> ' + lableValues.ContactExtension + ' </label></td>' +
        '<td> <label class="ContactEmail"> ' + lableValues.ContactEmail + ' </label></td>' +
        '<td> <label class="ContactTitle"> ' + lableValues.ContactTitle + ' </label></td>' +
        '<td>' +
        '<a href="javascript: void(0)" class="delete_icon" onclick="deleteTr(this)">' +
        '<i class="far fa-trash-alt"></i>' +
        '</a>' +
        '</td>' +
        '</tr>';

    return html;
}
//#endregion

//#region DELETE ROWS
var deleteTr = function (control) {
    console.log("control: ", control);
    $.confirm({
        title: 'Confirmation!',
        content: "<b>Are you sure you want to Delete ?</b> ",
        type: 'red',
        typeAnimated: true,
        buttons: {
            delete: {
                btnClass: 'btn-blue',
                action: function () {
                    $(control).parents('tr').remove();
                }
            },
            cancel: {
                //btnClass: 'btn-red',
            }
        }
    })
    

}
//#endregion

//#region ADD MULTIPLE CONTACTS

var contactArray = [];
var multipleContacts = function (tableId) {
    $('#' + tableId + ' tbody tr').each(function () {
        $(this).each(function (index, value) {
            var td = $(this).find('td');

            var arrProperty = [];
            var props = {};
            $(td).each(function (index, value) {

                var $label = $(this).find('label');
                var lblText = $.trim($label.attr('class'));
                var lblData = $.trim($label.text());

                props[lblText] = lblData;
            })
            contactArray.push(props);
        });
    })

    return contactArray;

}
//#endregion
//Go BACK... Added on 08-Feb-2023
$("html").unbind().keyup(function (e) {
    console.log("Which Key: " + $(e.target) + " : " + document.getElementsByClassName("jconfirm").length + " : " + window.location.href.toLowerCase().indexOf("Index"));
    if (!$(e.target).is('input') && !$(e.target).is('textarea')) {
        console.log(e.which);
        //event.preventDefault();
        if (e.key === 'Backspace' || e.keyCode === 8) {
            //alert('backspace pressed');
            //window.location.href = baseUrl + "Shipment/Shipment/ViewShipmentList";
            if (document.getElementsByClassName("jconfirm").length == 0 && window.location.href.toLowerCase().indexOf("index") >= 0) {
                window.location.href = baseUrl + "Customer/ViewCustomer";
            } else if (document.getElementsByClassName("jconfirm").length >= 1) {
                return;
            } else if (document.getElementsByClassName("jconfirm").length == 0 && window.location.href.toLowerCase().indexOf("index") < 0) {
                window.location.href = baseUrl + "Customer/ViewCustomer";
            }
        }
        if (e.key === 'Enter' || e.keyCode === 13) {
            var titleTxt = "";
            if (document.getElementsByClassName("jconfirm-title")[0] != undefined) {
                titleTxt = document.getElementsByClassName("jconfirm-title")[0].innerHTML;
            }
            console.log("ENTER: " + document.getElementsByClassName("jconfirm").length + " : " + titleTxt);
            if (document.getElementsByClassName("jconfirm").length == 0) {
                $("#btnSave").click();
            } else if (document.getElementsByClassName("jconfirm").length >= 1 && titleTxt.toLowerCase() == "success!") {
                window.location.href = baseUrl + "Customer/ViewCustomer";
            }
        }
    }
});
//
//#region SAVE CUSTOMER
var btnSave = function () {
    $("#btnSave").on("click", function () {
        if (validateContact()) {
            isNeedToloaded = false;
            addCustomerAjax();
        }
    })
}
//#endregion

//#region ADD CUSTOMER
var customer = function () {
    var customerDto = {}
    customerDto.CustomerId = $.trim($('#hdnCustomerId').val())
    customerDto.BaseAddressId = $.trim($('#BaseAddressId').val())
    customerDto.CustomerName = $.trim($('#txtCustomerName').val())

    //#region Multiple Contact
    var contacts = multipleContacts("tblMultipleContacts");
    var CustomerContacts = [];
    if (contacts != undefined && contacts.length) {
        $.each(contacts, function (index, value) {
            obj = {};
            obj.ContactFirstName = value.ContactFirstName
            obj.ContactLastName = value.ContactLastName
            obj.ContactPhone = value.ContactPhone
            obj.ContactExtension = value.ContactExtension
            obj.ContactEmail = value.ContactEmail
            obj.ContactTitle = value.ContactTitle

            CustomerContacts.push(obj);
        });
    }
    customerDto.CustomerContacts = CustomerContacts;
    //#endregion

    customerDto.Website = $.trim($('#txtWebsite').val())
    customerDto.Comments = $.trim($('#txtComment').val())

    if ($('#chkIsActive').prop("checked") == true) {
        customerDto.IsActive = true;
    }
    else {
        customerDto.IsActive = false;
    }

    if ($('#chkIsPickDropLocation').prop("checked") == true) {
        customerDto.IsPickDropLocation = true;
    }
    else {
        customerDto.IsPickDropLocation = false;
    }

    if ($('#chkIsWaitingTimeRequired').prop("checked") == true) {
        customerDto.IsWaitingTimeRequired = true;
    }
    else {
        customerDto.IsWaitingTimeRequired = false;
    }


    if ($('#chkTemperatureRequired').prop("checked") == true) {
        customerDto.IsTemperatureRequired = true;
    }
    else {
        customerDto.IsTemperatureRequired = false;
    }

    customerDto.IsFullFledgedCustomer = true;

    customerDto.BillingAddress1 = $.trim($('#txtBillingAddress1').val())
    customerDto.BillingAddress2 = $.trim($('#txtBillingAddress2').val())
    customerDto.BillingCity = $.trim($('#txtBillingCity').val())
    customerDto.BillingStateId = $.trim($('#ddlBillingState').val())
    customerDto.BillingZipCode = $.trim($('#txtBillingZip').val())
    customerDto.BillingCountryId = $.trim($('#ddlBillingCountry').val())
    customerDto.BillingPhoneNumber = $.trim($('#txtBillingPhone').val())
    customerDto.BillingFax = $.trim($('#txtBillingFax').val())
    customerDto.BillingEmail = $.trim($('#txtBillingEmail').val())
    customerDto.PALAccount = $.trim($('#txtPALAccount').val())
    customerDto.BillingExtension = $.trim($('#txtBillingExtension').val())



    customerDto.ShippingAddress1 = $.trim($('#txtShippingAddress1').val())
    customerDto.ShippingAddress2 = $.trim($('#txtShippingAddress2').val())
    customerDto.ShippingCity = $.trim($('#txtShippingCity').val())
    customerDto.ShippingStateId = $.trim($('#ddlShippingState').val())
    customerDto.ShippingZipCode = $.trim($('#txtShippingZip').val())
    customerDto.ShippingCountryId = $.trim($('#ddlShippingCountry').val())
    customerDto.ShippingPhoneNumber = $.trim($('#txtShippingPhone').val())
    customerDto.ShippingFax = $.trim($('#txtShippingFax').val())
    customerDto.ShippingEmail = $.trim($('#txtShippingEmail').val())
    customerDto.ShippingExtension = $.trim($('#txtShippingExtension').val())
    
    return customerDto;
}
//#endregion

//#region ADD CUSTOMER AJAX CALL

var addCustomerAjax = function () {
    
    var customerModel = customer();
    
    $.ajax({
        url: baseUrl + "Customer/index",
        contentType: "application/json;charset=utf-8",
        type: "POST",
        data: JSON.stringify(customerModel),
        dataType: "json",
        beforeSend: function (xhr, settings) {
            showLoader();
        },
        success: function (data, textStatus, jqXHR) {
            if (data.IsSuccess == true) {
                console.log("data.IsSuccess: ", data);
               // toastr.success(data.Message);
                //setInterval(function () {
                //    window.location = baseUrl + "Customer/ViewCustomer";
                //}, 700)
                $.alert({
                    title: 'Success!',
                    content: "<b>" + data.Message + "</b>",
                    type: 'green',
                    typeAnimated: true,
                    buttons: {
                        Ok: {
                            btnClass: 'btn-green',
                            action: function () {
                                window.location = baseUrl + "Customer/ViewCustomer";
                            }
                        },
                    }
                });
            }
            else {
               // toastr.warning(data.Message);
                AlertPopup(data.Message);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("error customer: ", errorThrown);
        },
        complete: function () {
            hideLoader();
        }
    });
}

//#endregion

//#region EDIT CUSTOMER AJAX CALL
var editCustomerAjax = function (params) {
    
    if (params != null) {
        $("#btnSave").val("Update");
        $("#btnSave label").text("UPDATE");
        $('#hdnCustomerId').val(params.CustomerId);
        $('#txtCustomerName').val(params.CustomerName);
        $('#txtWebsite').val(params.Website);
        $('#txtComment').val(params.Comments);

        if (params.IsActive == true) {
            $('#chkIsActive').prop("checked", true);
        }
        if (params.IsWaitingTimeRequired == true) {
            $('#chkIsWaitingTimeRequired').prop("checked", true);
        }
        if (params.IsPickDropLocation == true) {
            $('#chkIsPickDropLocation').prop("checked", true);
        }
        if (params.IsTemperatureRequired == true) {
            $('#chkTemperatureRequired').prop("checked", true);
        }

        $('#txtCompanyName').val(params.CompanyName);
        $('#txtAddPhone1').val(params.AdditionalPhone1);
        $('#txtAddExtension1').val(params.Extension1);
        $('#txtAddPhone2').val(params.AdditionalPhone2);
        $('#txtAddExtension2').val(params.Extension2);


        $('#txtBillingAddress1').val(params.BillingAddress1);
        $('#txtBillingAddress2').val(params.BillingAddress2);
        $('#txtBillingCity').val(params.BillingCity);
       
        $('#txtBillingZip').val(params.BillingZipCode);
        $('#ddlBillingCountry').val(params.BillingCountryId);
        bindBillingState();
        $('#ddlBillingState').val(params.BillingStateId);
        $('#txtBillingPhone').val(params.BillingPhoneNumber);
        $('#txtBillingFax').val(params.BillingFax);
        $('#txtBillingEmail').val(params.BillingEmail);
        $('#txtPALAccount').val(params.PALAccount);
        $('#txtBillingExtension').val(params.BillingExtension);

        $('#txtShippingAddress1').val(params.ShippingAddress1);
        $('#txtShippingAddress2').val(params.ShippingAddress2);
        $('#txtShippingCity').val(params.ShippingCity);
        
        $('#txtShippingZip').val(params.ShippingZipCode);
        $('#ddlShippingCountry').val(params.ShippingCountryId);
        bindShippingState();
        $('#ddlShippingState').val(params.ShippingStateId);
        $('#txtShippingPhone').val(params.ShippingPhoneNumber);
        $('#txtShippingFax').val(params.ShippingFax);
        $('#txtShippingEmail').val(params.ShippingEmail);
        $('#txtShippingExtension').val(params.ShippingExtension);
        console.log("update customer: ");

    }
}

//#endregion

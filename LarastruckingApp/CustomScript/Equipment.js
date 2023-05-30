var isNeedToloaded = true;
$(document).ready(function () {
    var edid = $("#EDID").val();

    if (edid > 0 && edid != undefined) {
        $("#btnSave").prop('value', 'Update');
        EditEquipment(edid);
    }
    else {

        $('.lblEqpIsActive').css("background", "#7ca337");
        $(".lblEqpIsActive").html("ACTIVE");
           

        $('.lblOutOfService').css("background", "#7ca337");
        $(".lblOutOfService").html("IN SERVICE");
    }
    CheckEquipmentNo();
});

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
// Data Edit
function EditEquipment(listId) {
    $.ajax({
        url: baseUrl + '/Equipment/EditEquipment',
        data: { 'id': listId },
        type: "POST",
        success: function (data) {
            //console.log(data)

            $("#QRCodeNo").val(data.QRCodeNo);
            $("#EDID").val(listId);
            $("#EquipmentNo").val(data.EquipmentNo);
            $("#VehicleType").val(data.VehicleType);
            $("#LicencePlate").val(data.LicencePlate);
            $("#Decal").val(data.Decal);
            $("#RegistrationExpiration").val(ConvertDate(data.RegistrationExpiration));
            $("#Year").val(data.Year);
            $("#Make").val(data.Make);
            $("#Color").val(data.Color);
            $("#VIN").val(data.VIN);
            $("#CubicFeet").val(data.CubicFeet);
            $("#LDimension").val(data.LDimension);
            $("#WDimension").val(data.WDimension);
            $("#HDimension").val(data.HDimension);
            $("#MaxLoad").val(data.MaxLoad);
            $("input[name='RollerBed'][value=" + data.RollerBed + "]").attr("checked", true);

            $("#ddlOwnedby").val(data.Ownedby);
            $("#Comments").val(data.Comments);

            $("#LeaseCompanyName").val(data.LeaseCompanyName);

            $("#LeaseEndDate").val(data.LeaseEndDate);


            if (data.FreightTypeIds.length) {
                var $select = $("#ddlFreightTypeId").selectize();

                var selectize = $select[0].selectize;
                var defaultValueIds = data.FreightTypeIds;
                selectize.setValue(defaultValueIds);
            }

            if (data.DoorTypeIds.length) {
                var $select = $("#DoorTypeIds").selectize();

                var selectize = $select[0].selectize;
                var defaultValueIds = data.DoorTypeIds;
                selectize.setValue(defaultValueIds);
            }

            $("#hdnRegistrationImageName").val(data.RegistrationImageName);
            $("#hdnInsuranceImageName").val(data.InsauranceImageName);
            $('#hdnRegistrationImageURL').val(data.RegistrationImageURL);
            $('#hdnInsuranceImageURL').val(data.InsuranceImageURL);
            
            if (data.Active == true) {
                $("#IsActive").prop("checked", true);
                $('.lblEqpIsActive').css("background", "#60b968");
                $(".lblEqpIsActive").html("ACTIVE");
            }
            else {
                $("#IsActive").prop("checked", false);
                $('.lblEqpIsActive').css("background", "red");
                $(".lblEqpIsActive").html("INACTIVE");
            }

            if (data.IsOutOfService == true) {
                $("#chkOutOfService").prop("checked", true);
               
                $('.lblOutOfService').css("background", "red");              
                $(".lblOutOfService").html("OUT OF SERVICE");
                $('#OutOfServiceDiv').show();
            }
            else {
                $("#chkOutOfService").prop("checked", false);
                $('.lblOutOfService').css("background", "#60b968");
                $(".lblOutOfService").html("IN SERVICE");
                $('#OutOfServiceDiv').hide();
            }

            if (data.Ownedby == "Leasing") {
                $("#LeaseCompanyId").show();
                $("#LeaseStartDateId").show();
            }
            else {
                $("#LeaseCompanyId").hide();
                $("#LeaseCompanyId").hide();
            }
            $("#LeaseStartDate").val(ConvertDate(data.LeaseStartDate));
            $("#LeaseEndDate").val(ConvertDate(data.LeaseEndDate));

            $("#OutOfServiceStartDate").val(ConvertDate(data.OutOfServiceStartDate));

            $("#OutOfServiceEndDate").val(ConvertDate(data.OutOfServiceEndDate));


            $("#btnSave").text('Update')


        }
    });
}

$("#IsActive").change(function () {
    var ischecked = $(this).is(':checked');
    if (ischecked) {
        $('.lblEqpIsActive').css("background", "#60b968");
        $(".lblEqpIsActive").html("ACTIVE");
    }
    else {
        $('.lblEqpIsActive').css("background", "red");
        $(".lblEqpIsActive").html("INACTIVE");
    }

});

$("#chkOutOfService").change(function () {
    var ischecked = $(this).is(':checked');
    if (ischecked) {
        $('.lblOutOfService').css("background", "red");
        $(".lblOutOfService").html("OUT OF SERVICE");
       
    }
    else {
        $('.lblOutOfService').css("background", "#60b968");
        $(".lblOutOfService").html("IN SERVICE");
    }

});

function GetEquipmentList() {

    $.ajax({
        url: baseUrl + '/Equipment/GetEquipmentList',
        type: 'GET',
        success: function (data) {
            $("#divEquipmentList").empty();
            $("#divEquipmentList").append(data);
        }
    })
}
function ConvertJsonDateToDate(jsondate) {

    if (jsondate != null && jsondate != "") {

        var parsedDate = new Date(parseInt(jsondate.substr(6)));

        var dt = parsedDate.getDate() < 10 ? ('0' + parsedDate.getDate()) : parsedDate.getDate();
        var mnt = parsedDate.getMonth() < 9 ? ('0' + (parsedDate.getMonth() + 1)) : (parsedDate.getMonth() + 1).toString();
        var yr = parsedDate.getFullYear().toString();

        return mnt + '-' + dt + '-' + yr;
    }

}

//Check duplicate equipment No
function CheckEquipmentNo() {
    $("#EquipmentNo").blur(function () {
        var edid = $("#EDID").val();
        $.ajax({
            url: baseUrl + '/Equipment/CheckEquipmentNo',
            data: { 'equipmentNo': $("#EquipmentNo").val(), 'equipmentId': edid },
            type: "POST",
            success: function (data) {
                if (data) {
                    //toastr.warning("Equipment No. already exist.")
                    AlertPopup("Equipment No. already exist.")
                    $("#EquipmentNo").val("");
                    $("#EquipmentNo").focus();
                }
            },
            error: function () { }
        });
    });

}

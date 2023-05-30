var glbDamageFile = [];
var glbProofOfTempFile = [];
var glbDamageFileCollection = [];
var damageDescriptionCollection = [];
//#region Ready State
$(function () {
    btnUpload();
    //btnUploadDamaged();

    // For Converting the Temperature
    convertTemp();

    // For Shipment Route Details Details Radio Button
    checkShipmentRadionButton();


});
//#endregion

//#region colour change on grid icon
$(".tblcolourChange").on("mouseover", 'tr', function () {

    $(this).find(".chng-color-view").css('color', 'white');
    $(this).find(".chng-color-Trash").css('color', 'white');
});


$(".tblcolourChange").on("mouseout", 'tr', function () {

    $(this).find(".chng-color-view").css('color', '#007bff');
    $(this).find(".chng-color-Trash").css('color', '#007bff');

});
//#endregion


//#region convert Temperture F to C
var convertTemp = function () {
    var actualTemp;
    $("#ddlTemperatureUnit").on("change", function () {
        var unit = $(this).val();
        var temp = $("#txtActualTemp").val();
        if (temp != '') {
            if (unit == 'C') {
                actualTemp = FahrenheitToCelsius(temp);
            }
            else {
                actualTemp = CelsiusToFahrenheit(temp);
            }

            $("#txtActualTemp").val(actualTemp)
        }
    })
}
//#endregion

//#region Button Upload
var btnUpload = function () {
    $(".btnProofOfTemp").on("click", function () {
        if (isFormValid('divProofOfTemp')) {

            var fileUploader = $("#fuProofOfTemperature");
            var actualTemp = $("#txtActualTemp").val();


            // Date Time Format 
            var d = new Date($.now());
            date = (d.getDate() + "-" + (d.getMonth() + 1) + "-" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds());
            //
            var FreightRadioValue = $("input[name='radiofreightId']:checked").val();
            var radioValue = $("input[name='radioShipmentRouteId']:checked").val();

            if (fileUploader.length) {
                var filesUploaded = fileUploader[0].files;

                for (let i = 0; i < filesUploaded.length; i++) {
                    if (radioValue > 0 && FreightRadioValue > 0) {
                        var ProofOfTempFile = {};
                        ProofOfTempFile.proofImageId = 0;
                        ProofOfTempFile.actualTemp = actualTemp;
                        ProofOfTempFile.FileName = filesUploaded[i].name;
                        ProofOfTempFile.Date = date;
                        ProofOfTempFile.shipmentRouteId = radioValue;
                        ProofOfTempFile.ShipmentFreightDetailId = FreightRadioValue;
                        glbProofOfTempFile.push(ProofOfTempFile);

                    }
                    bindProofOfTempFileTbl();
                }

            }

        }
    })
}
//#endregion

//#region Bind Proof Of Temp files 

function bindProofOfTempFileTbl() {

    var tr = "";
    $("#tblProofOfTemp tbody").empty();
    
    var FreightRadioValue = $("input[name='radiofreightId']:checked").val();
    var radioValue = $("input[name='radioShipmentRouteId']:checked").val();

    if (radioValue > 0 && FreightRadioValue > 0) {
        
        var ProofFiles = glbProofOfTempFile.filter(x => x.ShipmentFreightDetailId == FreightRadioValue && x.shipmentRouteId == radioValue);
        if (ProofFiles.length > 0) {
            for (var i = 0; i < ProofFiles.length; i++) {
                tr += '<tr data-file-url=' + ProofFiles[i].ImageUrl + ' ondblclick="javascript:ViewDocument(this)">' +

                    '<td>' + ProofFiles[i].actualTemp + '</td>' +
                    '<td>' + ProofFiles[i].FileName + '</td>' +
                    '<td>' + ProofFiles[i].Date + '</td>' +
                    '<td><button type="button" data-file-url=' + ProofFiles[i].ImageUrl + ' onclick="ViewDocument(this)" class="delete_icon chng-color-view"><i class="far fa-eye"></i></button><a href=' + ProofFiles[i].ImageUrl + ' download title="download" class="edit_icon chng-color-Trash fileDownload" ><i class="fa fa-download"></i></a></td>' +
                    '</tr>'

            }

        }
        ////else {
        ////    $("#hdrProof").css("display", "none");
        ////    $("#tblProofOfTemp").css("display", "none");
        ////}
    }
    $("#tblProofOfTemp tbody").append(tr);
}

//#endregion


//#region bind Damaged file Upload

function bindDamageFileTbl() {

    var tr = "";
    $("#tblDamagedFiles tbody").empty();

    var radioValue = $("input[name='radioShipmentRouteId']:checked").val();

    if (radioValue > 0) {

        var damageFiles = glbDamageFile.filter(x => x.shipmentRouteId == radioValue);
        if (damageFiles.length > 0) {
            for (var i = 0; i < damageFiles.length; i++) {
                tr += '<tr data-file-url=' + damageFiles[i].ImageUrl + ' ondblclick="javascript:ViewDocument(this)">' +
                    '<td>' + damageFiles[i].DamageDescription + '</td>' +
                    '<td>' + damageFiles[i].FileName + '</td>' +
                    '<td>' + damageFiles[i].Date + '</td>' +
                    '<td><button type="button" data-file-url=' + damageFiles[i].ImageUrl + ' onclick="ViewDocument(this)" class="delete_icon chng-color-view"><i class="far fa-eye"></i></button><a href=' + damageFiles[i].ImageUrl + ' download title="download" class="edit_icon chng-color-Trash fileDownload" ><i class="fa fa-download"></i></a></td>' +
                    '</tr>'

            }


        }
        //else {
        //    $("#hdrDamgeFiles").css("display", "none");
        //    $("#tblDamagedFiles").css("display", "none");
        //}

    }
    $("#tblDamagedFiles tbody").append(tr);

}
//#endregion



//#region Remove row in grid
function remove_damage_document_row(_this) {
    $(_this).closest("tr").remove();
}
//#endregion


//#region Shipment Details for Multiple Routes

function GetShipmentDetails(ShippingRoutesId) {

    $.ajax({
        url: baseUrl + '/Customer/GetCustomerShipmentRoutesDetails/' + ShippingRoutesId,
        type: "Get",
        dataType: "json",
        async: false,
        success: function (data) {
            if (data != null) {
                
                $("#hdShipmentId").val(data.ShipmentId);
                $("#txtShipRefNo").val(data.ShipmentRefNo);
                $("#txtAirWayBill").val(data.AirWayBill);
                $("#txtCustomerPO").val(data.CustomerPO);
                $("#txtOrderNo").val(data.OrderNo);
                $("#txtCustomerRef").val(data.CustomerRef);
                $("#txtContainerNo").val(data.ContainerNo);
                $("#txtPurchaseDoc").val(data.PurchaseDoc);
                $("#txtPickupLocation").val(data.PickUpLocation);
                $("#txtDeliveryLocation").val(data.DeliveryAddress);
                $("#txtPickupPhone").val(data.PickUpPhone);
                $("#txtPickupExtension").val(data.PickUpExtension);
                $("#txtEstimatedPickupArrival").val(ConvertDate(data.PickUpArrivalDate, true));
                $("#txtEstimatedDeliveryArrival").val(ConvertDate(data.DeliveryArrive, true));
                $("#txtDeliveryPhone").val(data.DeliveryPhone);
                $("#txtDeliveryExtension").val(data.DeliveryExtension);
                $("#txtCustomerShipmentStatus").val(data.StatusName);
                $("#txtCustomerShipmentSubStatus").val(data.SubStatusName);
                $("#txtOtherReason").val(data.Reason);
                $("#txtReceiverName").val(data.ReceiverName);
                $("#txtSign").attr('src', data.DigitalSignature);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}
//#endregion


function CheckRouteStops(routeId) {
   
    $("#txtDamagedFileName").val("");
    $("#fuDamageFiles").val("");
    GetShipmentDetails(routeId);
    GetShipmentFreightDetails(routeId);
    bindDamageFileTbl();
}
//#region Get Shipment Freight Details

function GetShipmentFreightDetails(ShipmentRouteStopeId) {
    $.ajax({
        url: baseUrl + '/Customer/GetShipmentFreightDetails/' + ShipmentRouteStopeId,
        type: "Get",
        dataType: "html",
        async: false,
        success: function (data) {
            
            if (data.length > 0) {
                
                $("#dvShipmentFreigtDetails").show();
                bindFreighttable(data);
                BindAccessorialCharges();
            }

            else {
                $("#dvShipmentFreigtDetails").hide();
                $("#txtTempReq").val("");

            }


        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}
//#endregion

function GetTemperatureREQ(_this) {
    $("#txtActualTemp").val("");
    $("#fuProofOfTemperature").val("");
    var data = $(_this).closest('tr').find('td .lblTemperatureRequired').text();
    $("#txtTempReq").val(data);

    bindProofOfTempFileTbl();

}
//#region bind Freight Details
bindFreighttable = function (_data) {
    $("#tblFreightDetailsTable").empty();

    $("#tblFreightDetailsTable").append(_data);


    //function for select first readio button
    checkFreightRadionButton();

}
//#endregion



//#region select default first Freight radion button
function checkFreightRadionButton() {
    var radioValue = $("input[name='radioShipmentRouteId']:checked").val();
    if (radioValue > 0) {
        $($("#tblFreightDetails tbody tr")[0]).find("input[type=radio]").prop("checked", true);

        var data = $($("#tblFreightDetails tbody tr")[0]).find('td .lblTemperatureRequired').text();
        $("#txtTempReq").val(data);

        var FreightRadioValue = $("input[name='radiofreightId']:checked").val();
        var radioValue = $("input[name='radioShipmentRouteId']:checked").val();
        GetShipmentProofOfTempFiles(radioValue, FreightRadioValue);


    }
}
//#endregion

//#region Shipment Route Radion button checked by Default
function checkShipmentRadionButton() {
    if ($("#tblShipmentRoute tbody tr").length > 0) {
        $($("#tblShipmentRoute tbody tr")[0]).find("input[type=radio]").prop("checked", true);
        var radioValue = $("input[name='radioShipmentRouteId']:checked").val();
        GetShipmentDetails(radioValue);
        GetShipmentFreightDetails(radioValue);
        GetPreTripCheckTimings(radioValue);
        GetShipmentDamagedFiles(radioValue);

    }

}
//#endregion

//#region Get Pre-Trip Check Timing Details 
var GetPreTripCheckTimings = function (ShippingRoutesId) {
    $.ajax({
        url: baseUrl + "Driver/GetPreTripCheckTimings/" + ShippingRoutesId,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            showLoader();
        },
        success: function (response) {
            if (response != null) {

                if (response.DriverPickupArrival == null) {
                    $("#txtActualPickupArrived").attr('disabled', false);

                }

                $("#txtActualPickupArrived").val(ConvertDate(response.DriverPickupArrival, true));
                $("#txtActualPickupDepart").val(ConvertDate(response.DriverPickupDeparture, true));
                $("#txtActualDeliveryArrived").val(ConvertDate(response.DriverDeliveryArrival, true));
                $("#txtActualDeliveryDepart").val(ConvertDate(response.DriverDeliveryDeparture, true));

                if (response.DriverPickupDeparture == null) {
                    $("#txtActualPickupDepart").attr('disabled', false);

                }
                if (response.DriverDeliveryArrival == null) {
                    $("#txtActualDeliveryArrived").attr('disabled', false);

                }
                if (response.DriverDeliveryDeparture == null) {
                    $("#txtActualDeliveryDepart").attr('disabled', false);

                }
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

//#region  bind Damaged Files

function GetShipmentDamagedFiles(ShipmentRouteStopeId) {
    $.ajax({
        url: baseUrl + '/Customer/GetShipmentDamagedFiles/' + ShipmentRouteStopeId,
        type: "Get",
        dataType: "json",
        async: false,
        success: function (data) {
            if (data != null) {
                glbDamageFile = new Array();
                //#region for binding damage Image
                for (let i = 0; i < data.length; i++) {
                    var damageFile = {};
                    damageFile.DamagedID = data[i].DamagedID;
                    damageFile.ImageUrl = data[i].ImageUrl;
                    damageFile.DamageDescription = data[i].DamagedDescription;
                    damageFile.FileName = data[i].DamagedImage;
                    damageFile.Date = ConvertDate(data[i].DamagedDate, true);
                    damageFile.shipmentRouteId = data[i].ShipmentRouteID;
                    damageFile.ImageUrl = data[i].ImageUrl;
                    damageFile.IsApproved = data[i].IsApproved;
                    glbDamageFile.push(damageFile);
                }
                bindDamageFileTbl();
            }
            //#endregion

        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}

//#endregion

//#region  bind Proof Of Temp Files

function GetShipmentProofOfTempFiles(ShipmentRouteStopeId, ShipmentFreightDetailId) {

    $.ajax({
        url: baseUrl + '/Customer/GetShipmentProofOfTempFiles/',
        type: "Get",
        dataType: "json",
        async: false,
        data: { "id": ShipmentRouteStopeId, "ShipmentFreightDetailId": ShipmentFreightDetailId },
        success: function (data) {
            if (data != null) {
                glbProofOfTempFile = new Array();
                //#region for binding Proof Of Temp Image
                for (let i = 0; i < data.length; i++) {
                    var ProofOfTempFile = {};
                    ProofOfTempFile.proofImageId = data[i].proofImageId;
                    ProofOfTempFile.actualTemp = data[i].proofActualTemp;
                    ProofOfTempFile.FileName = data[i].ProofImage;
                    ProofOfTempFile.Date = ConvertDate(data[i].ProofDate, true);
                    ProofOfTempFile.shipmentRouteId = data[i].ShipmentRouteID;
                    ProofOfTempFile.ShipmentFreightDetailId = data[i].ShipmentFreightDetailId;
                    ProofOfTempFile.ImageUrl = data[i].ImageUrl;
                    ProofOfTempFile.IsApproved = data[i].IsApproved;
                    glbProofOfTempFile.push(ProofOfTempFile);
                }
                bindProofOfTempFileTbl();
            }

            //#endregion

        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}

//#endregion


//#region View Document
function ViewDocument(_this) {
    
    var fileUrl = $(_this).attr("data-file-url");
    if (fileUrl != undefined) {
        var extn = fileUrl.substring(fileUrl.lastIndexOf('.') + 1);

        var isImg = isExtension(extn, _imgExts);
        var $divViewer = $("#divViewer");
        var docHeight = $(document).height();
        if (isImg) {
            $('.btnPrintImage').show();
            $('.btnPrintPdf').hide();
            var imgTag = '<img src="' + fileUrl + '" style="width: 100%; height:' + (docHeight - 500) + 'px" class="img-fluid" />';
            $divViewer.html(imgTag);
        }
        else {
            $('.btnPrintImage').hide();
            $('.btnPrintPdf').show();
            var docHeight = $(document).height();
            var iframe = '<iframe id="myiframe" src="' + fileUrl + '" style="width: 100%;height:' + (docHeight - 200) + 'px"></iframe>';
            $divViewer.html(iframe);
        }
        $("#modalDocument").modal("show");
    }
}




//#endregion

//#region CHECK EXTENSION
var _imgExts = ["jpg", "jpeg", "png", "jfif"];
var isExtension = function (ext, extnArray) {
    var result = false;
    var i;
    if (ext) {
        ext = ext.toLowerCase();
        for (i = 0; i < extnArray.length; i++) {
            if (extnArray[i].toLowerCase() === ext) {
                result = true;
                break;
            }
        }
    }
    return result;
}
//#endregion//#region print pdf
var printPdf = function () {
    $('.btnPrintPdf').on('click', function () {
        $('#myiframe').get(0).contentWindow.print();
    })
}
//#endregion

//#region print image
var printImage = function () {
    $('.btnPrintImage').on('click', function () {
        var divToPrint = document.getElementById("divViewer");
        newWin = window.open("", "");
        newWin.document.write(divToPrint.outerHTML);
        newWin.print();
        newWin.close();

    })
}
//#endregionfunction CheckImageExtension(_this) {
    
    var Isvalid = false;

    //var file = $('input[type="file"]').val();
    var file = _this.value;
    if (file != null && file != "") {
        var exts = ['jpg', 'jpeg', 'JPG', 'JPEG', 'png', 'pdf', 'PDF', 'JFIF', 'jfif'];
        // first check if file field has any value
        if (file) {
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array
            if ($.inArray(get_ext[0].toLowerCase(), exts) > -1) {
                Isvalid = true;
                return Isvalid;
            } else {
               // toastr.warning('Uploads must be in JPG, JPEG, PNG, PDF, or JFIF formats.');
                AlertPopup('Uploads must be in JPG, JPEG, PNG, PDF, or JFIF formats.');

                $(_this).val(null);
                return Isvalid;
            }
        }
    }
    else {
        //toastr.warning("Please Browse to select your Upload File.", "")
        AlertPopup("Please Browse to select your Upload File.", "")
        return Isvalid;
    }

}

//#region Update Freight Detail Information
var updateFreightDetails = function (ShipmentRouteStopeId) {
    
    getFreightDetails(ShipmentRouteStopeId);
    $("#divPopUpFreightDetails").modal("show");
}
//#endregion

//#region Get Pre-Trip Check List Details
var getFreightDetails = function (ShipmentRouteStopeId) {
    $.ajax({
        url: baseUrl + "Customer/GetFreightDetailsById/" + ShipmentRouteStopeId,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            showLoader();
        },
        success: function (response) {
            if (response != null) {
                $("#hdnShipmentRouteStopeId").val(response.ShipmentRouteStopeId);
                $("#hdnShipmentBaseFreightDetailId").val(response.ShipmentBaseFreightDetailId);
                $("#txtCommodity").val(response.Commodity);
                $("#txtFreight").val(response.FreightTypeName);
                $("#hdnFreightTypeId").val(response.FreightTypeId);
                $("#txtPricingName").val(response.PricingMethodName);
                $("#hdnPricingMethodId").val(response.PricingMethodId);
                $("#txtQuantity").val(response.QuantityNweight);
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
//#region Get TextBox Values
var getParameterValues = function () {
    var dto = {};
    dto.ShipmentRouteStopeId = $("#hdnShipmentRouteStopeId").val();
    dto.ShipmentBaseFreightDetailId = $.trim($("#hdnShipmentBaseFreightDetailId").val());
    dto.Commodity = $.trim($("#txtCommodity").val());
    dto.FreightTypeId = $.trim($("#hdnFreightTypeId").val());
    dto.PricingMethodId = $.trim($("#hdnPricingMethodId").val());
    dto.QuantityNweight = $.trim($("#txtQuantity").val());
    return dto;
}
//#endregion


//#region Update Freight Details
$("#BtnSave").on("click", function () {
    var dto = getParameterValues();
    $.ajax({
        url: baseUrl + "Customer/UpdateFreightDetails",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(dto),
        beforeSend: function () {
            showLoader();
        },
        success: function (response) {
            $("#divPopUpFreightDetails").modal("hide");
            $("#txtCommodity").val("");
            $("#txtFreight").val("");
            $("#txtPricingName").val("");
            $("#txtQuantity").val("");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $("#divPopUpFreightDetails").modal("hide");

        },
        complete: function () {
            $("#divPopUpFreightDetails").modal("hide");
            hideLoader();

        }
    });
})
//#endregion

//#region bind accessorial charges
function BindAccessorialCharges() {
    var routeId = $("input[name='radioShipmentRouteId']:checked").val();
    var shipmentId = $("#hdShipmentId").val();
    $("#tblAccessorialFee tbody").empty();
    $.ajax({
        url: baseUrl + "Customer/GetCustomerAccessorialCharge",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: { "shipmentId": shipmentId, "routeId": routeId },
        beforeSend: function () {
            showLoader();
        },
        success: function (response) {
            
            $("#divAccessorialchargs").hide();
            if (response.length > 0) {
                $("#divAccessorialchargs").show();
                
                var tblbody = "";
                for (var i = 0; i < response.length; i++) {
                    tblbody += "<tr><td>" + response[i].FeeType + "</td><td style='text-align:right'> $" + response[i].Amount + "</td></tr>"
                }
                $("#tblAccessorialFee tbody").append(tblbody);
            }


        },
        error: function (jqXHR, textStatus, errorThrown) {
            $("#divAccessorialchargs").hide();

        },
        complete: function () {
            hideLoader();

        }
    });

}

//#endregion
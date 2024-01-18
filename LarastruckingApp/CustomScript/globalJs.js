
//#region SESSION GLOBAL VARIABLE
var minutesForWarning = 5;
var sessionTimeout = 30;
var showWarning = true;
var showRedirect = true;

//#endregion

//#region JQUERY READY FUNCTION
$(function () {

    setInterval(CheckTime, 10000);
    btn_ok();
    get_current_year_and_previous();
    dateActivate();
    dateTimeActivate();
    phoneMask();
    // maask();
    airWayBill();
    dateTime();
    dateMask();
    showhideplusminus();
    $("input").keyup(function (e) {
        forceInputUppercase(e)
        //$(this).val($(this).val().toUpperCase());
    });
    $("textarea").keyup(function (e) {
        //$(this).val($(this).val().toUpperCase());
        forceInputUppercase(e);
    });

    //$(document).on('keyup', "input[type=text]", function (e) {
    //    forceInputUppercase(e)
    //});
    function forceInputUppercase(e) {
        var start = e.target.selectionStart;
        var end = e.target.selectionEnd;
        e.target.value = e.target.value.toUpperCase();
        e.target.setSelectionRange(start, end);
    }


    $('.noSpace').keyup(function () {
        this.value = this.value.replace(/\s/g, '');
    });
    //$("strong").keyup(function () {
    //    $(this).val($(this).val().toUpperCase());
    //});

});
//#endregion

//#region SESSION EXPIRE ALERT
var addMinutes = function (date, minutes) {
    return new Date(date.getTime() + minutes * 60 * 1000);
}
var remainingMinutes = function (date) {
    return Math.round((date - (new Date()).getTime()) / 60 / 1000);
}
var timeToWarn = addMinutes(new Date(), sessionTimeout - minutesForWarning);
var timeToEnd = addMinutes(new Date(), sessionTimeout);

function CheckTime() {
    //console.log("timeToEnd: ", timeToEnd);
    //console.log("timeToWarn: ", timeToWarn);
    if (showWarning && new Date() > timeToWarn && new Date() < timeToEnd) {
        showRedirect = false;
        showWarning = false;
        $("#session_msg").text("Your session will expire within " + remainingMinutes(timeToEnd) + " mins! Please refresh the page to continue working.");
        $('#modalSession').modal('show');
    }
    if (new Date() > timeToEnd) {
        if (showRedirect)

            // $("#session_msg").text("Session expired. You will be redirected to login page ");
            $("#session_msg").text("Your session has expired. Please login again to continue working. ");
        $('#modalSession').modal('show');
    }
    if (showRedirect == false)
        showRedirect = true;
}
var btn_ok = function () {
    $("#btnOk").on("click", function () {
        window.location.reload();
    });
}

//#endregion

//#region YEAR DROP-DOWN 
var get_current_year_and_previous = function () {
    var current = new Date();
    var current_year = current.getFullYear();

    for (let i = 0; i < 32; i++) {
        if (i == 0) {
            $("#Year").append(
                $('<option></option>').val(current_year - i).html(current_year - i)
            );
        }
        else {
            $("#Year").append(
                $('<option></option>').val(current_year - i).html(current_year - i)
            );
        }
    }

}

//#endregion

//#region CONVERT DATE
function ConvertDate(jsondate, time = false) {

    if (jsondate != null && jsondate != "") {
        var parsedDate = new Date(parseInt(jsondate.substr(6)));
       // console.log("parsedDate: " + parsedDate);
        var dt = parsedDate.getDate() < 10 ? ('0' + parsedDate.getDate()) : parsedDate.getDate();
        var mnt = parsedDate.getMonth() < 9 ? ('0' + (parsedDate.getMonth() + 1)) : (parsedDate.getMonth() + 1).toString();
        var yr = parsedDate.getFullYear().toString();

        var sec = parsedDate.getSeconds();
        sec = sec < 10 ? '0' + sec : sec;

        var min = parsedDate.getMinutes();
        min = min < 10 ? '0' + min : min;

        var hr = parsedDate.getHours();
        hr = hr < 10 ? '0' + hr : hr;

        if (time) {
            return mnt + '/' + dt + ' ' + hr + ':' + min;
        }
        else {
            return mnt + '-' + dt + '-' + yr;
        }
    }
}

function ConvertDateEdit(jsondate, time = false) {
	

    if (jsondate != null && jsondate != "") {
        var parsedDate = new Date(parseInt(jsondate.substr(6)));

        var dt = parsedDate.getDate() < 10 ? ('0' + parsedDate.getDate()) : parsedDate.getDate();
        var mnt = parsedDate.getMonth() < 9 ? ('0' + (parsedDate.getMonth() + 1)) : (parsedDate.getMonth() + 1).toString();
        var yr = parsedDate.getFullYear().toString();

        var sec = parsedDate.getSeconds();
        sec = sec < 10 ? '0' + sec : sec;

        var min = parsedDate.getMinutes();
        min = min < 10 ? '0' + min : min;

        var hr = parsedDate.getHours();
        hr = hr < 10 ? '0' + hr : hr;

        if (time) {
            return mnt + '/' + dt + '/' + yr + ' ' + hr + ':' + min;
        }
        else {
            return mnt + '/' + dt + '/' + yr;
        }
    }
}
//
/*
function ConvertDateEdit2(jsondate, time = false) {
	alert("jsondate:" +jsondate);

    if (jsondate != null && jsondate != "") {
        var parsedDate = new Date(parseInt(jsondate.substr(6)));

        var dt = parsedDate.getDate() < 10 ? ('0' + parsedDate.getDate()) : parsedDate.getDate();
        var mnt = parsedDate.getMonth() < 9 ? ('0' + (parsedDate.getMonth() + 1)) : (parsedDate.getMonth() + 1).toString();
        var yr = parsedDate.getFullYear().toString();

        var sec = parsedDate.getSeconds();
        sec = sec < 10 ? '0' + sec : sec;

        var min = parsedDate.getMinutes();
        min = min < 10 ? '0' + min : min;

        var hr = parsedDate.getHours();
        hr = hr < 10 ? '0' + hr : hr;
		alert("return: "+ mnt + '-' + dt + '-' + yr + ' ' + hr + ':' + min)
        if (time) {
            return mnt + '-' + dt + '-' + yr + ' ' + hr + ':' + min;
        }
        else {
            return mnt + '-' + dt + '-' + yr;
        }
    }
}
*/
//
function ConvertWithoutUTC(jsondate, time = false) {

    if (jsondate != null && jsondate != "") {


        var utcDate = jsondate;
        //alert("date: "+date);
        console.log("utcDate: " + jsondate)
        const date = new Date(utcDate);

        //console.log("date: "+date);
        //console.log("local: "+date.toLocaleString());
        //console.log("local to string: "+date.toLocaleString())
        var currentDay = date.getDate().toString();
        var currentMonth = date.getMonth() + 1;
        currentMonth = currentMonth.toString()
        var currentHour = date.getHours().toString();
        var currentMinutes = date.getMinutes().toString();
        //console.log("currentDay: "+currentDay+" currentMonth:"+currentMonth+" currentHour:"+currentHour+" currentMinutes:"+currentMinutes);		

        return addZero(currentMonth) + '/' + addZero(currentDay) + ' ' + addZero(currentHour) + ':' + addZero(currentMinutes);

    }
    else {
        return "NA"
    }
}



function ConvertDateNew(jsondate, time = false) {

    if (jsondate != null && jsondate != "") {


        var utcDate = jsondate + " utc";
        //alert("date: "+date);
     //   console.log("jsondate: " + jsondate);
       // console.log("utcDate: " + utcDate);
        const date = new Date(utcDate);

        //console.log("date: "+date);
        //console.log("local: "+date.toLocaleString());
        //console.log("local to string: "+date.toLocaleString())
        var currentDay = date.getDate().toString();
        var currentMonth = date.getMonth() + 1;
        currentMonth = currentMonth.toString()
        var currentHour = date.getHours().toString();
        var currentMinutes = date.getMinutes().toString();
        //console.log("currentDay: "+currentDay+" currentMonth:"+currentMonth+" currentHour:"+currentHour+" currentMinutes:"+currentMinutes);		

        return addZero(currentMonth) + '/' + addZero(currentDay) + ' ' + addZero(currentHour) + ':' + addZero(currentMinutes);

    }
    else {
        return "NA"
    }
}
function addZero(val){
	//console.log(val+" = "+val.length);
	if(val.length==1){
		return "0"+val;
	}else{
		return val;
	}
	
}

//#endregion
function parseDate(input) {
    // trimes and remove multiple spaces and split by expected characters
    var parts = input.trim().replace(/ +(?= )/g, '').split(/[\s-\/:]/)
    // new Date(year, month [, day [, hours[, minutes[, seconds[, ms]]]]])
    return new Date(parts[0], parts[1] - 1, parts[2] || 1, parts[3] || 0, parts[4] || 0, parts[5] || 0); // Note: months are 0-based
}
function ConvertSqlDateTime(sqlDate) {

    var date = parseDate(sqlDate);
    // var date = new Date(sqlDate);
    var dates = (((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '-' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate())) + '-' + date.getFullYear());
    //var dates = (((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '/' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate())) );
    var min = date.getMinutes();
    min = min < 10 ? '0' + min : min;

    var hr = date.getHours();
    hr = hr < 10 ? '0' + hr : hr;
    return dates = dates + ' ' + hr + ':' + min;
}

function ConvertSqlDateTimeNew(sqlDate) {

    var date = parseDate(sqlDate);
    var dates = (((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '/' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate())));
    var min = date.getMinutes();
    min = min < 10 ? '0' + min : min;

    var hr = date.getHours();
    hr = hr < 10 ? '0' + hr : hr;
    return dates = dates + ' ' + hr + ':' + min;

}
//#region convert time
function ConvertTime(jsondate) {

    if (jsondate != null && jsondate != "") {
        var parsedDate = new Date(jsondate);

        var dt = parsedDate.getDate() < 10 ? ('0' + parsedDate.getDate()) : parsedDate.getDate();
        var mnt = parsedDate.getMonth() < 9 ? ('0' + (parsedDate.getMonth() + 1)) : (parsedDate.getMonth() + 1).toString();
        var yr = parsedDate.getFullYear().toString();

        var sec = parsedDate.getSeconds();
        sec = sec < 10 ? '0' + sec : sec;

        var min = parsedDate.getMinutes();
        min = min < 10 ? '0' + min : min;

        var hr = parsedDate.getHours();
        hr = hr < 10 ? '0' + hr : hr;

        return hr + ':' + min;

    }
}
//#endregion


//#region FORM VALIDATION
function isFormValid(formId) {
    var valid = true;
    $("#" + formId + " input[data-required=required]").each(function () {
        $(this).removeClass('invalid');
        $(this).attr('title', '');

        $element = $(this)
        var $label = $("label[for='" + $element.attr('id') + "']").text();

        if (!$.trim($(this).val())) {

            $(this).addClass('invalid');
            if ($label != "") {
                $(this).attr('title', '' + $label + ' is required');
            }
            else {
                $(this).attr('title', 'This field is required');
            }

            valid = false;
        }

        if ($(this).attr("email") == "email" && !$(this).val().match(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/)) {
            $(this).addClass('invalid');
            $(this).attr('title', 'Enter valid email');
            valid = false;
        }

        if ($(this).attr("phone") == "phone" && !$(this).val().match(/^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/)) {

            $(this).addClass('invalid');
            $(this).attr('title', 'Enter valid Phone No');
            valid = false;
        }

        if ($(this).attr("zip") == "zip" && !$(this).val().match(/(^\d{5}$)|(^\d{5}-\d{4}$)/)) {
            $(this).addClass('invalid');
            $(this).attr('title', 'Enter valid Zip');
            valid = false;
        }

    });

    $("#" + formId + " select[data-required=required]").each(function () {
        $(this).removeClass('invalid');
        $(this).attr('title', '');

        $element = $(this)
        var $label = $("label[for='" + $element.attr('id') + "']").text();

        if (!$(this).val()) {

            $(this).addClass('invalid');
            if ($label != "") {
                $(this).attr('title', '' + $label + ' is required');
            }
            else {
                $(this).attr('title', 'This field is required');
            }

            valid = false;
        }
    });
    return valid;
}
//#endregion

//#region function for convert string to float 
function ConvertStringToFloat(value) {
    return parseFloat(value).toFixed(2)
}
//#endregion

//#region VALIDATE IMAGE EXTENSION
function Validatefile(_this) {

    CheckImageExtension(_this);
    encodeImagetoBase64(_this);
}
function encodeImagetoBase64(_this) {
    var Isvalid = false;
    var size = parseFloat(_this.files[0].size / 1024).toFixed(2);
    if (size <= 5120) //1048576=1MB
    {
        Isvalid = true;
    }
    else {
        AlertPopup("File must be less then or equal to 5MB");
        $(_this).val(null);
        Isvalid = false;
    }
    return Isvalid;
}
function CheckImageExtension(_this) {

    var Isvalid = false;
    //var file = $('input[name="RegistrationImageURL"]').val();
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
                AlertPopup('Uploads must be in JPG, JPEG, PNG, PDF, or JFIF formats.');
                //alert('File extension must be JPG or JPEG or PNG.');
                $(_this).val(null);
                return Isvalid;
            }
        }
    }
    else {
        AlertPopup("Please Browse to select your Upload File.", "")
        return Isvalid;
    }
}
//#endregion VALIDATE IMAGE EXTENSION

//#region DATE-TIME
var dateTimeActivate = function () {
    var now = new Date();
    var defaultDate = now - 1000 * 60 * 60 * 24 * 1;

    var options = {
        format: 'm/d/Y H:i',
        formatTime: 'H:i',
        formatDate: 'm/d/Y',
        startDate: new Date(),
        minDate: false,
        minTime: false,
        roundTime: 'round',// ceil, floor, round
        step: 30,
        timepicker: true
    }

    jQuery('.dtTimer').datetimepicker(options);
}
//#endregion

//#region DATE
var dateActivate = function () {

    var options = {
        format: 'm-d-Y',
        formatTime: 'H:i',
        formatDate: 'm-d-Y',
        startDate: new Date(),
        minDate: true,
        minTime: true,
        roundTime: 'round',// ceil, floor, round
        step: 30,
        timepicker: false
    }

    jQuery('.jqueryui-marker-datepicker').datetimepicker(options);
}
//#endregion

//#region PHONE MASK
var phoneMask = function () {
    $("input[phone='phone']").mask('(000) 000-0000');

}
//#endregion
//function maask() {
//    $.mask.definitions['H'] = "[0-2]";
//    $.mask.definitions['h'] = "[0-9]";
//    $.mask.definitions['M'] = "[0-5]";
//    $.mask.definitions['m'] = "[0-9]";

//    $(".hh").mask("Hh");
//    $(".mm").mask("Mm");
//}
//#region AirWayBill MASK
var airWayBill = function () {
    $("input[name='txtAirWayBill']").mask('000-0000-0000');
}
var dateTime = function () {
    $(".dtTimer").mask('00-00-0000 00:00');
}

var dateMask = function () {
    $(".dateMask").mask('00-00-0000');
}
//#endregion

//#region CONVERTER FORMULA
var CelsiusToFahrenheit = function (Celsius) {
    return ConvertStringToFloat(((Celsius * (9 / 5)) + 32), 2);
}

var FahrenheitToCelsius = function (Fahrenheit) {
    return ConvertStringToFloat((((Fahrenheit - 32) * 5) / 9), 2);
}

//#endregion


//#region split string
function SplitString(text, count, insertDots) {
    return text.slice(0, count) + (((text.length > count) && insertDots) ? "..." : "");
}
//#endregion



//#region plus minus
function showhideplusminus() {
    // Add minus icon for collapse element which is open by default
    $(".collapse.show").each(function () {
        $(this).prev(".cardHeading").find(".fa").addClass("fa-minus").removeClass("fa-plus");
    });
    // Toggle plus minus icon on show hide of collapse element
    $(".collapse").on('show.bs.collapse', function () {
        $(this).prev(".cardHeading").find(".fa").removeClass("fa-plus").addClass("fa-minus");
    }).on('hide.bs.collapse', function () {
        $(this).prev(".cardHeading").find(".fa").removeClass("fa-minus").addClass("fa-plus");
    });
}
//#endregion



//#region Status Colour Change

function StatusCheckForShipment(status) {

    var PreStatus = "";
    if ($.trim(status).toLowerCase() == $.trim("ORDER TAKEN").toLowerCase()) {
        PreStatus = '<span style=""">' + status + '</span>'
    }
    if ($.trim(status).toLowerCase() == $.trim("DISPATCHED").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#0095f9;color:#fff">' + status + '</span>'
    }
    if ($.trim(status).toLowerCase() == $.trim("LOADING").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#ff9400;color:white">' + status + '</span>'
    }
    if ($.trim(status).toLowerCase() == $.trim("IN-ROUTE").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#71d637;color:#fff">' + status + '</span>'
    }

    if ($.trim(status).toLowerCase() == $.trim("IN-FUMIGATION").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#ff4300;color:white">' + status + '</span>'
    }
    if ($.trim(status).toLowerCase() == $.trim("DELIVERED").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#017301;color:#fff">' + status + '</span>'
    }
    if ($.trim(status).toLowerCase() == $.trim("ON HOLD").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#ff4300;color:#fff">' + status + '</span>'
    }
    if ($.trim(status).toLowerCase() == $.trim("APPT PENDING").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#fdf82b;color:#fff">' + status + '</span>'
    }
    if ($.trim(status).toLowerCase() == $.trim("IMMED ATTENTION").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#e6061b;color:white">' + status + '</span>'
    }
    if ($.trim(status).toLowerCase() == $.trim("CANCELLED").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#e6061b;color:white">' + status + '</span>'
    }
    if ($.trim(status).toLowerCase() == $.trim("COMPLETED").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#00af50;color:white">' + status + '</span>'
    }

    if ($.trim(status).toLowerCase() == $.trim("IN QUEUE").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#f8468a;color:#fff">' + status + '</span>'
    }
    if ($.trim(status).toLowerCase() == $.trim("IN STORAGE").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#fdf82b;color:#fff">' + status + '</span>'
    }
    if ($.trim(status).toLowerCase() == $.trim("DRIVER ASSIGNED").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#1c2b85;color:#fff">' + status + '</span>'
    }
	if ($.trim(status).toLowerCase() == $.trim("ARRIVED AT PICK UP LOCATION").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#ffc000;color:#fff">ARRIVED PU</span>'
    }
	if ($.trim(status).toLowerCase() == $.trim("ARRIVED AT DELIVERY LOCATION").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#71d637;color:#fff">ARRIVED DELIVERY</span>'
    }
	if ($.trim(status).toLowerCase() == $.trim("UNLOADING AT DELIVERY LOCATION").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#019f3d;color:#fff">UNLOADING</span>'
    }
    return PreStatus;
}

//#endregion



//#region Status Colour Change

function CustomerStatusCheckForShipment(status) {

    var PreStatus = "";
    if ($.trim(status).toLowerCase() == $.trim("ORDER TAKEN").toLowerCase()) {
        PreStatus = '<span style=""">' + status + '</span>'
    }
    if ($.trim(status).toLowerCase() == $.trim("DISPATCHED").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#bdd7ee;color:black">' + status + '</span>'
    }
    if ($.trim(status).toLowerCase() == $.trim("LOADING AT PICK UP LOCATION").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#4774c9;color:white">' + status + '</span>'
    }
    if ($.trim(status).toLowerCase() == $.trim("IN-ROUTE TO DELIVERY LOCATION").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#fffd01;color:black">' + status + '</span>'
    }

    if ($.trim(status).toLowerCase() == $.trim("IN FUMIGATION").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#fe9900;color:white">' + status + '</span>'
    }
    if ($.trim(status).toLowerCase() == $.trim("DELIVERED").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#92d14f;color:black">' + status + '</span>'
    }
    if ($.trim(status).toLowerCase() == $.trim("SHIPMENT ON HOLD").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#ffbf00;color:black">' + status + '</span>'
    }
    if ($.trim(status).toLowerCase() == $.trim("APPOINTMENT PENDING").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#ff9a32;color:black">' + status + '</span>'
    }
    if ($.trim(status).toLowerCase() == $.trim("IMMEDIATE ATTENTION").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#fe0000;color:white">' + status + '</span>'
    }
    if ($.trim(status).toLowerCase() == $.trim("CANCELLED").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#fe0000;color:white">' + status + '</span>'
    }
    if ($.trim(status).toLowerCase() == $.trim("COMPLETED").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#00af50;color:white">' + status + '</span>'
    }

    if ($.trim(status).toLowerCase() == $.trim("IN QUEUE").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#fe9900;color:black">' + status + '</span>'
    }
    if ($.trim(status).toLowerCase() == $.trim("IN STORAGE").toLowerCase()) {
        PreStatus = '<span class="badge" style="background-color:#fe9900;color:black">' + status + '</span>'
    }
    return PreStatus;
}

//#endregion


function replaceBR(string) {

    var str = string.replace("<br/>", "");
    return str;
}


function GetCompanyName(fullAddress) {
    var fullAddress = fullAddress;
    var lastIndex = fullAddress.lastIndexOf(",")
    var companyName = fullAddress.substring(0, lastIndex);
    var address = fullAddress.substring(lastIndex + 1);
   // console.log("Address:" + address);
    if (lastIndex > 0) {
        //console.log("companyName:" + companyName);
        return companyName;
    }
    else {
        return fullAddress;
    }

}

function GetCompanyNameNew(fullAddress) {
    var fullAddress = fullAddress;
    var lastIndex = fullAddress.split("$")
    var companyName = fullAddress;
    //console.log("companyName:" + companyName);
    var address = fullAddress;
    if (lastIndex > 0) {
        return address;
    }
    else {
        return fullAddress;
    }

}

function GetAddress(fullAddress) {
    var fullAddress = fullAddress;
    var lastIndex = fullAddress.lastIndexOf(",")
    var companyName = fullAddress.substring(0, lastIndex);
    var address = fullAddress.substring(lastIndex + 1);
    if (lastIndex > 0) {
        return address;
    }
    else {
        return fullAddress;
    }

}

function GetAddressNew(fullAddress) {
    var fullAddress = fullAddress;
    //var lastIndex = fullAddress.lastIndexOf(",")
    //var companyName = fullAddress.substring(0, lastIndex);
    var address = fullAddress;
    if (lastIndex > 0) {
        return address;
    }
    else {
        return fullAddress;
    }

}

function GetCAddressNew(fullAddress) {
    var fullAddress = fullAddress;
    var lastIndex = fullAddress.lastIndexOf("$")
    var companyName = fullAddress.substring(0, lastIndex);
    //console.log("companyName: " + companyName);
    var address = fullAddress.substring(lastIndex + 2);
    //console.log("address: " + address);
    if (lastIndex > 0) {
        return address;
    }
    else {
        return fullAddress;
    }
}

function GetCompanyNew(fullAddress) {

    var fullAddress = fullAddress;
    var lastIndex = fullAddress.lastIndexOf("$")
    var companyName = fullAddress.substring(0, lastIndex);
    var address = fullAddress.substring(lastIndex + 2);
    if (lastIndex > 0) {
        return companyName;
    }
    else {
        return fullAddress;
    }

}

//Function to Encrypt the ID
function Encrypt(rawtext) {

    var encryptedRowIds = "";

    //#region in UTF8
    // encryptedRowIds = CryptoJS.enc.Utf8.parse(rawtext.toString());
    //#endregion

    //#region with dse
    //var iv = CryptoJS.enc.Utf8.parse('8080808080808080');
    ////var encryptedRowIds = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(planeId), key,
    //var encryptedRowIds = CryptoJS.DES.encrypt(CryptoJS.enc.Utf8.parse(planeId), key,
    //    {
    //        keySize: 128 / 8,
    //        iv: iv,
    //        mode: CryptoJS.mode.CBC,
    //        padding: CryptoJS.pad.Pkcs7
    //    });
    //#endregion

    //#region with date

    //var today = new Date();
    //var date = today.getFullYear() + 'G' + (("0" + (today.getMonth() + 1)).slice(-2)) + 'y' + ("0" + (today.getDate() + 1)).slice(-2)
    //encryptedRowIds = date + 'Hjkd' + date + rawtext + date;

    ///////////

    var arr = [];
    for (var i = 0; i < 5; i++) {
        arr.push(Math.floor(Math.random() * 9) + 1);
    }
    var today = new Date();
    var date = Date.now();
    date = date.toString().substring(0, 13);
    encryptedRowIds = date + (("0" + (today.getMonth() + 1)).slice(-2)) + arr[0] + arr[1] + arr[2] + rawtext + arr[3] + arr[4] + date;

    //#endregion

    //#region custome
    //rawtext = rawtext.toString();

    //for (i = 0; i < rawtext.length; i++) {
    //    encryptedRowIds += enc(rawtext[i]);
    //}

    //#endregion

    //#region sring to hex

    //rawtext = rawtext.toString();
    //for (var n = 0, l = rawtext.length; n < l; n++) {
    //    var hex = Number(rawtext.charCodeAt(n)).toString(16);
    //    encryptedRowIds += (hex + " ");
    //}
    //#endregion

    return encryptedRowIds.toString();
}

//Encryption Format
function enc(x) {

    switch (x) {
        case "0":
            return "=";
            break;
        case "1":
            return "i";
            break;
        case "2":
            return "B";
            break;
        case "3":
            return ">";
            break;
        case "4":
            return "j";
            break;
        case "5":
            return "X";
            break;
        case "6":
            return "$";
            break;
        case "7":
            return "Q";
            break;
        case "8":
            return "Y";
            break;
        case "9":
            return "Z";
            break;
    }
}


function AlertPopup(message) {
    $.alert({
        title: 'Alert!',
        content: "<b>" + message + "</b>",
        type: 'red',
        typeAnimated: true,
    });
}

function SuccessPopup(message) {
    $.alert({
        title: 'Success!',
        content: "<b>" + message + "</b>",
        type: 'green',
        typeAnimated: true,
    });
}

function ConfirmationPopup(message) {
    $.alert({
        title: 'Confirmation!',
        content: "<b>" + message + "</b>",
        type: 'red',
        typeAnimated: true,
    });
}

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
//#endregion
var dateFormat = function () {

    var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
        timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
        timezoneClip = /[^-+\dA-Z]/g,
        pad = function (val, len) {
            val = String(val);
            len = len || 2;
            while (val.length < len) val = "0" + val;
            return val;
        };

    // Regexes and supporting functions are cached through closure
    return function (date, mask, utc) {

        var dF = dateFormat;

        // You can't provide utc if you skip other args (use the "UTC:" mask prefix)
        if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
            mask = date;
            date = undefined;
        }

        // Passing date through Date applies Date.parse, if necessary
        date = date ? new Date(date) : new Date;
       // if (isNaN(date)) throw SyntaxError("invalid date");


        if (isNaN(date)) {
            // throw SyntaxError("invalid date");
            //   console.log("Date Error : ", isNaN(date));
        }
        else {
            // console.log("Date True : ", isNaN(date));
        }

        mask = String(dF.masks[mask] || mask || dF.masks["default"]);

        // Allow setting the utc argument via the mask
        if (mask.slice(0, 4) == "UTC:") {
            mask = mask.slice(4);
            utc = true;
        }

        var _ = utc ? "getUTC" : "get",
            d = date[_ + "Date"](),
            D = date[_ + "Day"](),
            m = date[_ + "Month"](),
            y = date[_ + "FullYear"](),
            H = date[_ + "Hours"](),
            M = date[_ + "Minutes"](),
            s = date[_ + "Seconds"](),
            L = date[_ + "Milliseconds"](),
            o = utc ? 0 : date.getTimezoneOffset(),
            flags = {
                d: d,
                dd: pad(d),
                ddd: dF.i18n.dayNames[D],
                dddd: dF.i18n.dayNames[D + 7],
                m: m + 1,
                mm: pad(m + 1),
                mmm: dF.i18n.monthNames[m],
                mmmm: dF.i18n.monthNames[m + 12],
                yy: String(y).slice(2),
                yyyy: y,
                h: H % 12 || 12,
                hh: pad(H % 12 || 12),
                H: H,
                HH: pad(H),
                M: M,
                MM: pad(M),
                s: s,
                ss: pad(s),
                l: pad(L, 3),
                L: pad(L > 99 ? Math.round(L / 10) : L),
                t: H < 12 ? "a" : "p",
                tt: H < 12 ? "am" : "pm",
                T: H < 12 ? "A" : "P",
                TT: H < 12 ? "AM" : "PM",
                Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
                o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
                S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
            };

        return mask.replace(token, function ($0) {
            return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
        });
    };
}();

// Some common format strings
dateFormat.masks = {
    "default": "ddd mmm dd yyyy HH:MM:ss",
    shortDate: "m/d/yy",
    mediumDate: "mmm d, yyyy",
    longDate: "mmmm d, yyyy",
    fullDate: "dddd, mmmm d, yyyy",
    shortTime: "h:MM TT",
    mediumTime: "h:MM:ss TT",
    longTime: "h:MM:ss TT Z",
    isoDate: "yyyy-mm-dd",
    isoTime: "HH:MM:ss",
    isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
    isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
};

// Internationalization strings
dateFormat.i18n = {
    dayNames: [
        "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
        "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
    ],
    monthNames: [
        "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
        "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
    ]
};

// For convenience...
Date.prototype.format = function (mask, utc) {
    return dateFormat(this, mask, utc);
};


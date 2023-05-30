var currentDate = new Date();
var isPrevious = false;
$(document).ready(function () {
    $('div').unbind('mouseenter mouseleave');
    $("#mytimeCard").hide();
    var userId = 0;
    var userRole = $("#hdnUserRole").val();
    if ($.trim(userRole).toLowerCase() == "Driver".toLocaleLowerCase() || $.trim(userRole).toLowerCase() == "Mechanics".toLocaleLowerCase()) {
        userId = $("#hdnUserId").val();
        ShowTimeCard(userId);
    }
});

function ShowTimeCard(UserId) {
    userId = UserId;
    currentDate = new Date();
    isPrevious = false;
    GetTimeCardData();
    // $("#spnWeekNo").text(GetCurrentWeekNu());
}

//Get current week
function GetCurrentWeekNu() {
    var d = new Date();
    var date = d.getDate();
    var day = d.getDay();

    return Math.ceil((date - 1 - day) / 7);
}

//Detail  Data Delete
function DriverDelete(listId) {
    $.confirm({
        title: 'Confirm!',
        content: 'Are you sure you want to Delete?',
        type: 'red',
        typeAnimated: true,
        buttons: {
            Delete: {
                btnClass: 'btn-blue',
                action: function () {

                    $.ajax({
                        url: baseUrl + '/Driver/DeleteDriver',
                        data: { 'id': listId },
                        type: "GET",
                        // cache: false,
                        success: function (data) {
                            if (data.IsSuccess == true) {
                                toastr.success(data.Message, "")
                            }
                            else {
                                toastr.error(data.Message, "")
                            }
                            $('#tblDriverDetails').DataTable().clear().destroy();
                            GetDriverList();
                        }
                    });
                }
            },
            cancel: {
                btnClass: 'btn-red',
            }

        }
    });
}

let row;
function SaveTime(_this, isCheckIn) {
    var isValidaionTrue = true;
    row = $(_this).closest("tr");
    var date = $(row).find("input[id='hdndate']").val();
    var outDate = $(row).find("input[id='txtOutDateTime']").val();

    //$(row).find("input[id='txtOutMin']").unbind('change');
    //$(row).find("input[id='txtInMin']").unbind('change');
    //$(row).find("input[id='txtOutHrs']").unbind('change');
    //$(row).find("input[id='txtInHrs']").unbind('change');

    var timeCardId = $(row).find("input[type='hidden']").val();
    var inHrs = $(row).find("input[id='txtInHrs']").val();
    var _thisValue = _this.value;

    if (inHrs >= 25) {

        $(row).find("input[id='txtInHrs']").val("00");
        isValidaionTrue = false;
    }

    var inMin = $(row).find("input[id='txtInMin']").val();
    if (inMin > 59 ) {
        $(row).find("input[id='txtInMin']").val("00");
        isValidaionTrue = false;
    }

    var outHrs = $(row).find("input[id='txtOutHrs']").val();
    if (outHrs >= 25 ) {
        $(row).find("input[id='txtOutHrs']").val("00");
        isValidaionTrue = false;
    }

    var outMin = $(row).find("input[id='txtOutMin']").val();
    if (outMin > 59 ) {

        $(row).find("input[id='txtOutMin']").val("00");
        isValidaionTrue = false;
    }

    var day = $(row).find("span[id='spnDay']").text();
    var inDateTime = null;
    if (inHrs >= 0 || inMin >= 0) {
        var ccInDateTime = date + ' ' + inHrs + ':' + inMin;
        inDateTime = new Date(ccInDateTime);
        console.log("isValidaionTrue success: " + isValidaionTrue);
    }
    else {
        isValidaionTrue = false;
        console.log("isValidaionTrue: " + isValidaionTrue);
        $(row).find("input[id='txtOutHrs']").val("00");
        $(row).find("input[id='txtOutMin']").val("00");
        AlertPopup("Please enter IN Hours or Minutes.");
    }

    var outDateTime = null;
    if (outHrs >= 0 || outMin >= 0) {
        var ccOutDateTime = outDate + ' ' + outHrs + ':' + outMin;
        outDateTime = new Date(ccOutDateTime);
    }

    //if (outDateTime != null && isValidaionTrue) {
    //    if (outDateTime < inDateTime) {
    //        isValidaionTrue = false;
    //      //  AlertPopup("Out date time should be greather then In date time");
    //    }
    //}
    if (outDateTime == null || outDateTime == "" && isValidaionTrue) {
       console.log("OutTime: " + outDateTime);
       console.log("outtime isValidaionTrue: " + isValidaionTrue);
        if (outDateTime < inDateTime) {
            isValidaionTrue = false;
            //console.log("isValidaionTrue: " + isValidaionTrue);
            
          //  AlertPopup("Out date time should be greather then In date time");
        }
    }
    if (outDateTime == null || outDateTime == ""  && inDateTime != null || inDateTime != "") {
        isValidaionTrue = true;
        console.log("data saved succesfully");
    }

    if (isValidaionTrue) {
        var values = {};
        values.Id = timeCardId;
        values.UserId = userId;
        values.InDateTime = inDateTime;
        values.OutDateTime = outDateTime;
        values.IsCheckIn = isCheckIn;
        values.Day = day;
       //console.log("savetimevalues: ",values);
        $.ajax({
            url: baseUrl + '/TimeCard/TimeCard/DispatcherTimeCard',
            data: JSON.stringify(values),
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data == true) {

                    $(row).find("input[id='txtOutMin']").bind('change');
                    $(row).find("input[id='txtInMin']").bind('change');
                    $(row).find("input[id='txtOutHrs']").bind('change');
                    $(row).find("input[id='txtInHrs']").bind('change');

                    //SuccessPopup("Success! Your data has been saved!");
                    GetTimeCardData();
                }
                else {
                    AlertPopup("Try again.");
                }
            }
        });

    }
}


function SaveTimeNew(_this, isCheckIn) {
    var isValidaionTrue = true;
    row = $(_this).closest("tr");
    var date = $(row).find("input[id='hdndate']").val();
    var outDate = $(row).find("input[id='txtOutDateTime']").val();

    $(row).find("input[id='txtOutMin']").unbind('change');
    $(row).find("input[id='txtInMin']").unbind('change');
    $(row).find("input[id='txtOutHrs']").unbind('change');
    $(row).find("input[id='txtInHrs']").unbind('change');

    var timeCardId = $(row).find("input[type='hidden']").val();
    var inHrs = $(row).find("input[id='txtInHrs']").val();
    var _thisValue = _this.value;

    if (inHrs >= 25) {

        $(row).find("input[id='txtInHrs']").val("00");
        isValidaionTrue = false;
    }

    var inMin = $(row).find("input[id='txtInMin']").val();
    if (inMin > 59) {
        $(row).find("input[id='txtInMin']").val("00");
        isValidaionTrue = false;
    }

    var outHrs = $(row).find("input[id='txtOutHrs']").val();
    if (outHrs >= 25) {
        $(row).find("input[id='txtOutHrs']").val("00");
        isValidaionTrue = false;
    }

    var outMin = $(row).find("input[id='txtOutMin']").val();
    if (outMin > 59) {

        $(row).find("input[id='txtOutMin']").val("00");
        isValidaionTrue = false;
    }

    var day = $(row).find("span[id='spnDay']").text();
    var inDateTime = null;
    if (inHrs >= 0 || inMin >= 0) {
        var ccInDateTime = date + ' ' + inHrs + ':' + inMin;
        inDateTime = new Date(ccInDateTime);
    }
    else {
        isValidaionTrue = false;

        $(row).find("input[id='txtOutHrs']").val("00");
        $(row).find("input[id='txtOutMin']").val("00");
        AlertPopup("Please enter IN Hours or Minutes.");
    }

    var outDateTime = null;
    if (outHrs >= 0 || outMin >= 0) {
        var ccOutDateTime = outDate + ' ' + outHrs + ':' + outMin;
        outDateTime = new Date(ccOutDateTime);
    }

    if (outDateTime != null && isValidaionTrue) {
        if (outDateTime < inDateTime) {
            isValidaionTrue = false;
            AlertPopup("Out date time should be greather then In date time");
        }
    }

    if (isValidaionTrue) {
        var values = {};
        values.Id = timeCardId;
        values.UserId = userId;
        values.InDateTime = inDateTime;
        values.OutDateTime = outDateTime;
        values.IsCheckIn = isCheckIn;
        values.Day = day;
        console.log("savetimevalues: ", values);
        $.ajax({
            url: baseUrl + '/TimeCard/TimeCard/DispatcherTimeCard',
            data: JSON.stringify(values),
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data == true) {

                    $(row).find("input[id='txtOutMin']").bind('change');
                    $(row).find("input[id='txtInMin']").bind('change');
                    $(row).find("input[id='txtOutHrs']").bind('change');
                    $(row).find("input[id='txtInHrs']").bind('change');

                    //SuccessPopup("Success! Your data has been saved!");
                    GetTimeCardData();
                }
                else {
                    AlertPopup("Try again.");
                }
            }
        });

    }
}

function ChangeDateStrToDate(_date) {

    var from = _date.split("-");
    var f = new Date(from[0], from[1] - 1, from[2])
    var timeCardDate = new Date(f);
    return dateFormat(timeCardDate, "mm-dd-yyyy");
}

$("#btnPrevious").click(function () {
    var weekStartDay = $("#spnStartDate").text();
    var from = weekStartDay.split("/");
    var date = new Date(from[2], from[0] - 1, from[1]);
    currentDate = new Date(date.setDate(date.getDate() - 7));
    if (isPrevious == false) {
        GetTimeCardData();
    }
    isPrevious = true;
});
$("#btnNext").click(function () {
    isPrevious = false;
    var todaydate = new Date();
    var weekStartDay = $("#spnStartDate").text();
    var from = weekStartDay.split("/");
    var date = new Date(from[2], from[0] - 1, from[1]);
    currentDate = new Date(date.setDate(date.getDate() + 7));
    if (currentDate < todaydate) {
        GetTimeCardData();
    }

});

function GetTimeCardData() {
    let curr = currentDate;// new Date();
    let first = curr.getDate() - (curr.getDay() < 4 ? (curr.getDay() + 7) : curr.getDay()) + (0 + 4);
    let fistDate = new Date(curr.setDate(first));
    fistDate = dateFormat(fistDate, "yyyy-mm-dd");

    let last = curr.getDate() - (curr.getDay() < 4 ? (curr.getDay() + 7) : curr.getDay()) + (6 + 4);
    let lastDate = new Date(curr.setDate(last));
    lastDate = dateFormat(lastDate, "yyyy-mm-dd");

    var values = {};
    values.UserId = userId;
    values.InDateTime = fistDate;
    values.OutDateTime = lastDate;
    $.ajax({
        url: baseUrl + '/TimeCard/TimeCard/GetTimeCardData',
        data: JSON.stringify(values),
        type: "POST",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            // if (data.length > 0) {
            BindTimeCardList(data);
            //}

        }
    });
}

function BindTimeCardList(_data) {
    if (_data != null) {
        $("#txtHourlyRate").attr('value', _data.HourlyRate);
        $("#txtTotalPay").attr('value', _data.TotalPay);
        $("#txtLoan").attr('value', _data.Loan);
        $("#txtDeductions").attr('value', _data.Deduction);
        $("#txtDescription").attr('value', _data.Description);

        $("#lblPrintHourlyRate").text(_data.HourlyRate);
        $("#lblPrintLoan").text(_data.Loan);
        $("#lblPrintTotalPay").text(_data.TotalPay);
        $("#lblPrintDeduction").text(_data.Deduction);
        $("#lblPrintDescription").text(_data.Description);
    }
    $("#spnDriverName").text(_data.UsernName);
    $("#spnPrintDriverName").text(_data.UsernName);

    var timeCard = "";
    var timCardPrint = "";
    //$("#spnDriverName").text(driverName);
    $("#timeCard tbody").empty();
    $("#tblTimeCardPrint tbody").empty();
    var weekday = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    var totalSec = 0;
    var totalHour = 0;
    var totalMin = 0;
    for (let i = 0; i <= 6; i++) {
        let curr = currentDate;
        let first = curr.getDate() - (curr.getDay() < 4 ? (curr.getDay() + 7) : curr.getDay()) + (i + 4);
        let date = new Date(curr.setDate(first));
        date = dateFormat(date, "yyyy-mm-dd");
        var from = date.split("-");
        var timeCardDate = new Date(from[0], from[1] - 1, from[2]);
        //var timeCardDate = new Date(f);

        var todayDate = new Date();
        var rquired = "";
        var inOnfocusout = "";
        var outOnfocusout = "";
        var dtpicker = "";
        var userRole = $("#hdnUserRole").val();
        if ($.trim(userRole).toLowerCase() == "Dispatcher".toLocaleLowerCase()) {
            if (timeCardDate != todayDate) {
                rquired = 'readonly="readonly"';
            }
            else {
                inOnfocusout = 'onfocusout="SaveTime(this,true)"';
                outOnfocusout = 'onfocusout="SaveTime(this,false)"';
                dtpicker = 'jqueryui-marker-datepicker';
            }
        }
        else if ($.trim(userRole).toLowerCase() == "Management".toLocaleLowerCase()) {

            if (timeCardDate > todayDate) {
                rquired = 'readonly="readonly"';
            }
            else {
                inOnfocusout = 'onfocusout="SaveTime(this,true)"';
                outOnfocusout = 'onfocusout="SaveTime(this,false)"';
                dtpicker = 'jqueryui-marker-datepicker';
            }
        }
        else {
            rquired = 'readonly="readonly"';
        }

        if (i == 0) {
            $("#spnStartDate").text(dateFormat(timeCardDate, "mm/dd/yyyy"));
            $("#spnPrintStartDate").text(dateFormat(timeCardDate, "mm/dd/yyyy"));
        }
        if (i == 6) {
            $("#spnEndtDate").text(dateFormat(timeCardDate, "mm/dd/yyyy"));
            $("#spnPrintEndtDate").text(dateFormat(timeCardDate, "mm/dd/yyyy"));
        }

        var timeCardList = _data.TimeCardList.filter(x => $.trim(x.Day.toUpperCase()) == $.trim(weekday[timeCardDate.getDay()]).toUpperCase());

        if (timeCardList.length > 0) {

            for (var j = 0; j < timeCardList.length; j++) {
                var inDate = ConvertDate(timeCardList[j].InDateTime, true);

                var inHrs = "";
                var inMin = "";
                if (inDate != undefined) {
                    inHrs = dateFormat(inDate, "HH");
                    inMin = dateFormat(inDate, "MM");
                }


                var outHrs = "";
                var outMin = "";
                var outDate = ConvertDate(timeCardList[j].OutDateTime, true);
                var diffHrs = "00";
                var diffMins = "00";
                if (outDate != undefined) {
                    outHrs = dateFormat(outDate, "HH");
                    outMin = dateFormat(outDate, "MM");

                    inDate = new Date(inDate);
                    outDate = new Date(outDate);

                    var diffMs = (outDate - inDate); // milliseconds between now & Christmas\
                    totalSec = totalSec + diffMs;
                    var diffDays = Math.floor(diffMs / 86400000); // days
                    //diffHrs = Math.floor((diffMs % 86400000) / 3600000); // hours
                    if (diffMs >= 0) {
                        
                        diffHrs = Math.floor((diffMs) / 3600000); // hours
                        diffHrs = diffHrs < 10 ? ('0' + diffHrs) : diffHrs;
                        totalHour = totalHour + parseInt(diffHrs);
                        //console.log("totalHour:"+totalHour);
                        //diffMins = Math.round(((diffMs % 86400000) % 3600000) / 60000); // minutes
                        diffMins = Math.round(((diffMs) % 3600000) / 60000); // minutes
                        diffMins = diffMins < 10 ? ('0' + diffMins) : diffMins;
                        totalMin = totalMin + parseInt(diffMins);
                        if (totalMin > 59) {

                            totalHour = totalHour + 1;
                            totalMin = totalMin - 60;
                            console.log("totalHour: " + totalHour);
                        }
                    }
                    else {
                         diffHrs = "00";
                         diffMins = "00";
                    }
                    
                }
                else {
                    outDate = inDate;
                }



                timeCard += '<tr><td><input type="hidden"  id="hdnTimeCardId" value="' + timeCardList[j].Id + '"/><input type="hidden" id="hdndate" value="' + date + '"/><span id="spnDay">' + weekday[timeCardDate.getDay()] + '</span></td><td class="text-center"><div class="timeWrapper d-inline-flex align-items-center">' +
                    '<input type="text" readonly="readonly" onkeypress="return onlyNumeric(event)" id="txtInDateTime" autocomplete="off" placeholder="MM-DD-YYYY" class="form-control txtInDate dateMask" value="' + dateFormat(timeCardDate, "mm-dd-yyyy") + '" /><span>&nbsp</span> ' +
                    '<input type="text" ' + rquired + ' onkeypress="return onlyNumeric(event)" id="txtInHrs" placeholder="HH" class="form-control hh"  ' + inOnfocusout + '  maxlength="2" value="' + inHrs + '" /><span>:</span>' +
                    '<input type="text" ' + rquired + ' onkeypress="return onlyNumeric(event)" id="txtInMin" placeholder="MM" class="form-control mm"  ' + inOnfocusout + '  maxlength="2" value="' + inMin + '" /></div></td>' +
                    '<td class="text-center">To</td>' +
                    '<td class="text-center">' +
                    '<div class="timeWrapper d-inline-flex align-items-center">' +
                    '<input type="text" ' + rquired + ' onkeypress="return onlyNumeric(event)" id="txtOutDateTime" autocomplete="off" placeholder="MM-DD-YYYY" class="form-control txtOutDate ' + dtpicker + ' dateMask" value="' + dateFormat(outDate, "mm-dd-yyyy") + '" /><span>&nbsp</span>' +
                    '<input type="text" ' + rquired + ' onkeypress="return onlyNumeric(event)" id="txtOutHrs" placeholder="HH" class="form-control hh"  ' + outOnfocusout + ' maxlength="2" value="' + outHrs + '" /><span>:</span>' +
                    '<input type="text" ' + rquired + ' onkeypress="return onlyNumeric(event)" id="txtOutMin" placeholder="MM" class="form-control mm"  ' + outOnfocusout + ' maxlength="2" value="' + outMin + '" /></div></td>' +
                    '<td>' + diffHrs + ':' + diffMins + '</td></tr>';

                timCardPrint += '<tr style="background:#fff;">' +
                    '<td style="text-align:left; padding-left:10px;">' +
                    '<span style="font-size:16px;">' + weekday[timeCardDate.getDay()] + '</span>' +
                    '</td>' +
                    '<td style="padding:7px;text-align:center;">' +
                    '<label style="background:#fff; height:35px; padding:5px 10px;font-size:17px; font-weight:400; line-height:1.5; color:#495057; border:0px solid #ced4da; border-radius:5px;">'
                    + dateFormat(timeCardDate, "mm-dd-yyyy") +
                    '</label>' +
                    '<span>&nbsp;</span>' +
                    '<label style="background:#fff; height:35px; padding:5px 10px; font-size:17px; font-weight:400; line-height:1.5; color:#495057; border:0px solid #ced4da; border-radius:5px;">' + inHrs + '</label>' +
                    '<span>:</span>' +
                    '<label style="background:#fff; height:35px; padding:5px 10px; font-size:17px; font-weight:400; line-height:1.5; color:#495057; border:0px solid #ced4da; border-radius:5px;">' + outMin + '</label>' +
                    '</td>' +
                    '<td style="padding:7px;font-size:16px;">To</td>' +
                    '<td style="padding:7px;text-align:center;">' +
                    '<label style="background:#fff; height:35px; padding:5px 10px; font-size:17px; font-weight:400; line-height:1.5; color:#495057; border:0px solid #ced4da; border-radius:5px;">' + dateFormat(outDate, "mm-dd-yyyy") + '</label>' +
                    '<span>&nbsp;</span>' +
                    '<label style="background:#fff; height:35px; padding:5px 10px; font-size:17px; font-weight:400; line-height:1.5; color:#495057; border:0px solid #ced4da; border-radius:5px;">' + outHrs + '</label>' +
                    '<span>:</span>' +
                    '<label style="background:#fff; height:35px; padding:5px 10px; font-size:17px; font-weight:400; line-height:1.5; color:#495057; border:0px solid #ced4da; border-radius:5px;">' + outMin + '</label>' +
                    '</td>' +
                    '<td style="font-size:16px;">' + diffHrs + ':' + diffMins + '</td>' +
                    '</tr> ';

            }
        }
        else {
            timeCard += '<tr><td><input type="hidden"  id="hdnTimeCardId" value=""/><input type="hidden" id="hdndate" value="' + date + '"/><span id="spnDay">' + weekday[timeCardDate.getDay()] + '</span></td><td class="text-center"><div class="timeWrapper d-inline-flex align-items-center">' +
                '<input type="text" readonly="readonly" onkeypress="return onlyNumeric(event)" id="txtInDateTime" autocomplete="off" placeholder="MM-DD-YYYY" class="form-control txtInDate dateMask" value="' + dateFormat(timeCardDate, "mm-dd-yyyy") + '" /><span>&nbsp</span>' +
                '<input type="text" ' + rquired + ' onkeypress="return onlyNumeric(event)" id="txtInHrs" placeholder="HH" class="form-control hh"  ' + inOnfocusout + '  maxlength="2" value="" /><span>:</span>' +
                '<input type="text" ' + rquired + ' onkeypress="return onlyNumeric(event)" id="txtInMin" placeholder="MM" class="form-control mm"  ' + inOnfocusout + '  maxlength="2" value="" /></div></td>' +
                '<td class="text-center">To</td>' +
                '<td class="text-center">' +
                '<div class="timeWrapper d-inline-flex align-items-center">' +
                '<input type="text" ' + rquired + ' onkeypress="return onlyNumeric(event)" id="txtOutDateTime" autocomplete="off" placeholder="MM-DD-YYYY" class="form-control txtOutDate ' + dtpicker + ' dateMask" value="' + dateFormat(timeCardDate, "mm-dd-yyyy") + '"  /><span>&nbsp</span>' +
                '<input type="text" ' + rquired + ' onkeypress="return onlyNumeric(event)" id="txtOutHrs" placeholder="HH" class="form-control hh"  ' + outOnfocusout + ' maxlength="2" value="" /><span>:</span>' +
                '<input type="text" ' + rquired + ' onkeypress="return onlyNumeric(event)" id="txtOutMin" placeholder="MM" class="form-control mm"  ' + outOnfocusout + ' maxlength="2" value="" /></div></td>' +
                '<td>00:00</td></tr>';

            timCardPrint += '<tr style="background:#fff;">' +
                '<td style="text-align:left; padding-left:10px;">' +
                '<span style="font-size:16px;">' + weekday[timeCardDate.getDay()] + '</span>' +
                '</td>' +
                '<td style="padding:7px;text-align:center;">' +
                '<label style="background:#fff; height:35px; padding:5px 10px; font-size:16px; font-weight:400; line-height:1.5; color:#495057; border:0px solid #ced4da; border-radius:5px;">'
                + dateFormat(timeCardDate, "mm-dd-yyyy") +
                '</label>' +
                '<span>&nbsp;</span>' +
                '<label style="background:#fff; height:35px; padding:5px 10px; font-size:16px; font-weight:400; line-height:1.5; color:#495057; border:0px solid #ced4da; border-radius:5px;">HH</label>' +
                '<span>:</span>' +
                '<label style="background:#fff; height:35px; padding:5px 10px; font-size:16px; font-weight:400; line-height:1.5; color:#495057; border:0px solid #ced4da; border-radius:5px;">MM</label>' +
                '</td>' +
                '<td style="padding:7px;font-size:16px;">To</td>' +
                '<td style="padding:7px;text-align:center;">' +
                '<label style="background:#fff; height:35px; padding:5px 10px; font-size:16px; font-weight:400; line-height:1.5; color:#495057; border:0px solid #ced4da; border-radius:5px;">' + dateFormat(timeCardDate, "mm-dd-yyyy") + '</label>' +
                '<span>&nbsp;</span>' +
                '<label style="background:#fff; height:35px; padding:5px 10px; font-size:16px; font-weight:400; line-height:1.5; color:#495057; border:0px solid #ced4da; border-radius:5px;">HH</label>' +
                '<span>:</span>' +
                '<label style="background:#fff; height:35px; padding:5px 10px; font-size:16px; font-weight:400; line-height:1.5; color:#495057; border:0px solid #ced4da; border-radius:5px;">MM</label>' +
                '</td>' +
                '<td style="font-size:16px;">00:00</td>' +
                '</tr> ';
        }
    }

    if (totalSec > 0 || totalHour>0 || totalMin > 0) {
        //console.log("totalSec: "+totalSec);
       // var TdiffHrs = Math.floor((totalSec / (1000 * 60 * 60))); // hours
        var TdiffHrs = totalHour; // hours
        //console.log("TdiffHrs: " + TdiffHrs);
        TdiffHrs = TdiffHrs < 10 ? ('0' + TdiffHrs) : TdiffHrs;

        //var TdiffMins = Math.floor((totalSec / (1000 * 60)) % 60); // minutes
        var TdiffMins = totalMin; // minutes
        TdiffMins = TdiffMins < 10 ? ('0' + TdiffMins) : TdiffMins;

        $("#spnGradTotal").text(TdiffHrs + ':' + TdiffMins);
        $("#spnPrintGrandTotal").text(TdiffHrs + ':' + TdiffMins);

    }
    else {
        $("#spnGradTotal").text('00:00');
        $("#spnPrintGrandTotal").text('00:00');
    }

    $("#timeCard tbody").append(timeCard);
    $("#tblTimeCardPrint tbody").append(timCardPrint);
    if ($.trim(userRole).toLowerCase() == "Management".toLocaleLowerCase()) {
        CalculateTotalPrice();
    }


    $("#modalTimeCard").modal("show");
    startEndDate();
}

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

//$("#btnCaculate").click(function () {
function btnCaculate() {
    $.confirm({
        title: 'Confirmation!',
        content: '<b>Are you sure to finalize Timecard for the week?</b>',
        type: 'blue',
        typeAnimated: true,
        buttons: {
            OK: {
                btnClass: 'btn-blue',
                action: function () {
                    SaveTimeCardAmount();
                }
            },
        }
    })
}
//});

function btnClear() {
    $("#txtHourlyRate").val("");
    $("#txtTotalPay").val("");
    $("#txtLoan").val("");
    $("#txtDeductions").val("");
    $("#txtDescription").val("");
}

function SaveTimeCardAmount() {
    var values = {};
    values.WeekStartDay = $("#spnStartDate").text();
    values.WeekEndDay = $("#spnEndtDate").text();
    values.HourlyRate = $("#txtHourlyRate").val();
    values.TotalPay = $("#txtTotalPay").val();
    values.Loan = $("#txtLoan").val();
    values.Deduction = $("#txtDeductions").val();
    values.Reimbursement = $("#txtReimbursements").val();
    values.Description = $("#txtDescription").val();
    values.Remaining = $("#txtRemaining").val();
    values.UserId = userId;

    $.ajax({
        url: baseUrl + '/TimeCard/TimeCard/SaveTimeCardAmount',
        data: JSON.stringify(values),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            if (data == true) {
                console.log("data: ", JSON.stringify(values));
                SuccessPopup("Success! Your data has been saved!");
            }
        }
    });
}

function CalculateTotalPrice() {

    var hourlyRate = $("#txtHourlyRate").val();
    var grandTotal = $("#spnGradTotal").text();
    var Loan = $("#txtLoan").val() == "" ? 0 : $("#txtLoan").val();
    var deduction = $("#txtDeductions").val() == "" ? 0 : $("#txtDeductions").val();
    var reimbursement = $("#txtReimbursements").val() == "" ? 0 : $("#txtReimbursements").val();

    if (Loan > 0) {
        $("#txtDeductions").prop('disabled', false);
    }
    else {
        $("#txtDeductions").prop('disabled', true);
    }
    if (parseFloat(Loan) >= parseFloat(deduction)) {
        var totalRemaining = Loan - deduction;
        $("#txtRemaining").val(ConvertStringToFloat(totalRemaining));
        $("#lblPrintRemaining").text(ConvertStringToFloat(totalRemaining));
    }
    else {
        $("#txtDeductions").val("");
        deduction = 0;
    }
    
    grandTotal = grandTotal.split(':');
    var totalPay = hourlyRate * grandTotal[0];

    totalPay = totalPay + ((hourlyRate / 60) * grandTotal[1]);

    if (parseFloat(totalPay) >= parseFloat(deduction)) {
        totalPay = parseFloat(totalPay) - parseFloat(deduction);
    }
    else {
        AlertPopup("Deduction amount should be less than or equal to Total Pay.");
        $("#txtDeductions").val("");
        deduction = 0;
    }
    if (parseFloat(reimbursement) >= 0) {
        totalPay = parseFloat(totalPay) + parseFloat(reimbursement);
        console.log("Reimbusrsement Success: " + reimbursement);
    }
    else {
        AlertPopup("Reimbursement amount should be less than or equal to Total Pay.");
        console.log("Reimbusrsement failure: " + reimbursement);
        //$("#txtReimbursements").val("");
        //reimbursement = 0;
    }
    $("#txtTotalPay").val(ConvertStringToFloat(totalPay));

    var hourlyRate = $("#txtHourlyRate").val();
    var totalPay = $("#txtTotalPay").val();
    var loan = $("#txtLoan").val();
    var deduction = $("#txtDeductions").val();
    var reimbursement = $("#txtReimbursements").val();

    var totalRemaining = $("#txtRemaining").val();


    $("#txtHourlyRate").attr('value', hourlyRate);
    $("#txtTotalPay").attr('value', totalPay);
    $("#txtLoan").attr('value', loan);
    $("#txtDeductions").attr('value', deduction);
    $("#txtReimbursements").attr('value', reimbursement);

    $("#txtRemaining").attr('value', totalRemaining);

}

function description() {
    var description = $("#txtDescription").val();
    $("#txtDescription").attr('value', description);
}

function PrintDiv() {

    var btnClose = document.getElementById("btnClose");
    var hrfPrint = document.getElementById("hrfPrint");
    var hrfEmail = document.getElementById("hrfEmail");
    var btnCaculate = document.getElementById("btnCaculate");
    var btnClear = document.getElementById("btnClear");
    var logodiv = document.getElementById("LarasPrintLogo");
    //Set the button visibility to 'hidden'

    btnClose.style.visibility = 'hidden';
    hrfPrint.style.visibility = 'hidden';
    hrfEmail.style.visibility = 'hidden';
    btnCaculate.style.visibility = 'hidden';
    btnClear.style.visibility = 'hidden';

    //$('#modalTimeCard').modal('toggle');
    //$('.modal-backdrop').remove();
    var divName = "modalTimeCard1";
    //var divName = "mytimeCard";

    var printContents = document.getElementById(divName).innerHTML;
    
    //console.log("logodiv:"+logodiv);
    //  Restore button visibility
    btnClose.style.visibility = 'visible';
    hrfPrint.style.visibility = 'visible';
    hrfEmail.style.visibility = 'visible';
    btnCaculate.style.visibility = 'visible';
    btnClear.style.visibility = 'visible';
    

    var originalContents = document.body.innerHTML;

    document.body.innerHTML = printContents;
    logodiv.style.display = 'block';
    window.print();
    
    document.body.innerHTML = originalContents;

}
function closePopup() {
    //window.location.reload();
    $('#modalTimeCard').modal('toggle');
    $('.modal-backdrop').remove();
}

function ShowEmailPopup() {
    $("#modalEmail").modal("show");
}

function btnSendEmail() {
    debugger
    $("#btnSendEmail").prop('disabled', true);
    var values = {};
    var divName = "mytimeCard";

    var printContents = document.getElementById(divName).innerHTML;
    var to = $("#txtTo").val();
    var subject = $("#txtSubject").val();
    var description = $("#txtBody").val();
    var user = $("#spnDriverName").text();
    var body = printContents;
    values.To = to;
    values.Subject = subject;
    values.Description = description;
    values.User = user;
    values.Body = body;
    var isValid = true;
    var message = "";
    if (to == "") {
        isValid = false;
        message = "Please enter email."
    }
    else if (subject == "") {
        isValid = false;
        message = "Please enter subject."
    }
    else if (description == "") {
        isValid = false;
        message = "Please enter description."
    }

    if (!isEmail(to)) {
        isValid = false;
        message = "Please enter valid email."
    }


    if (isValid) {

        $.ajax({
            url: baseUrl + '/TimeCard/TimeCard/SendTimeCard',
            type: "POST",
            beforeSend: function () {
                showLoader();
            },
            data: JSON.stringify(values),
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {

                if (data == true) {
                    SuccessPopup("Success! Your data has been sent!");
                }
                $("#btnSendEmail").prop('disabled', false);
                hideLoader();
            }
        });
    }
    else {
        $("#btnSendEmail").prop('disabled', false);
        AlertPopup(message);
    }

}

function isEmail(email) {
    debugger
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}
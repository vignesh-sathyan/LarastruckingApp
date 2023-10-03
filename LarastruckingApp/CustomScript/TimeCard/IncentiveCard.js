var currentDate = new Date();
var isPrevious = false;
var incentiveGrid = [];
$(document).ready(function () {
    $('div').unbind('mouseenter mouseleave');
    $("#mytimeCard").hide();
    var userId = 0;
  
    var userRole = $("#hdnUserRole").val();
    if ($.trim(userRole).toLowerCase() == "Driver".toLocaleLowerCase() || $.trim(userRole).toLowerCase() == "Mechanics".toLocaleLowerCase()) {
        userId = $("#hdnUserId").val();
        ShowTimeCard(userId);
       // incentiveGrid = GetIncentiveGridData();
    }
    console.log("userId: ", userId);
   // console.log("incentiveGrid: ", incentiveGrid);
});

function GetIncentiveGridData() {
    var IncentiveGrid = [];
    // console.log("BIND DATA: ", _data);
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
    // $("#modalTimeCard").modal("show");
    // console.log("Incentive Grid: ", values);
    $.ajax({
        url: baseUrl + '/TimeCard/TimeCard/GetIncentiveGridData',
        data: JSON.stringify(values),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        async:false,
        success: function (gridData) {
            if (gridData.length > 0) {
                console.log("GridData: ", gridData);
                incentiveGrid = gridData;
            }

        },
        error: function (xhr, err) {
            console.log("error : " + err);
        }
    });
    return incentiveGrid;
}

function countOccurrences(arr) {
    const counts = {};
    const array = arr;
    if (array.length > 0) {
        // Loop through the array
        for (const item of array) {
            // If the item is not in the counts object, initialize it with 1, otherwise, increment the count
            if (!counts[item]) {
                counts[item] = 1;
            } else {
                counts[item]++;
            }
        }
    }
    else {
        counts = "";
    }
 

    return counts;
}

function ShowTimeCard(UserId) {
    userId = UserId;
    console.log("Timecard UserId: ", userId);
    currentDate = new Date();
    isPrevious = false;
    GetTimeCardData();
  
    $("#btnSendEmails").css("display", "none");
    // $("#spnWeekNo").text(GetCurrentWeekNu());
}

function ShowTimeCards(UserId) {
    userId = UserId;
    currentDate = new Date();
    isPrevious = false;
    GetTimeCardData();
    $("#btnSendEmail").css("display", "none");
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

function saveAllTime() {
    //timeCard  

    $("table#timeCard > tbody > tr").each(function () {
        var rowlocal = this;
        var isValidaionTrue = true;
        var date = $(rowlocal).find("input[id='hdndate']").val();
        var outDate = $(rowlocal).find("input[id='txtOutDateTime']").val();
        var timeCardId = $(rowlocal).find("input[type='hidden']").val();
        var inHrs = $(rowlocal).find("input[id='txtInHrs']").val();
        var isRemovalFlag = $(rowlocal).find("input[id='hdnIsRemovalFlag']").attr('value');
        //console.log("isRemovalFlag:" + isRemovalFlag);
        if (inHrs >= 25) {

            $(rowlocal).find("input[id='txtInHrs']").val("00");
            isValidaionTrue = false;
        }

        var inMin = $(rowlocal).find("input[id='txtInMin']").val();
        if (inMin > 59) {
            $(rowlocal).find("input[id='txtInMin']").val("00");
            isValidaionTrue = false;
        }

        var outHrs = $(rowlocal).find("input[id='txtOutHrs']").val();
        if (outHrs >= 25) {
            $(rowlocal).find("input[id='txtOutHrs']").val("00");
            isValidaionTrue = false;
        }

        var outMin = $(rowlocal).find("input[id='txtOutMin']").val();
        if (outMin > 59) {

            $(rowlocal).find("input[id='txtOutMin']").val("00");
            isValidaionTrue = false;
        }

        var day = $(rowlocal).find("span[id='spnDay']").text();
        var inDateTime = null;
        if (inHrs >= 0 || inMin >= 0) {
            var ccInDateTime = date + ' ' + inHrs + ':' + inMin;
            inDateTime = new Date(ccInDateTime);
            //console.log("isValidaionTrue success: " + isValidaionTrue);
        }
        else {
            isValidaionTrue = false;
            //console.log("isValidaionTrue: " + isValidaionTrue);
            $(rowlocal).find("input[id='txtOutHrs']").val("00");
            $(rowlocal).find("input[id='txtOutMin']").val("00");
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
            //console.log("OutTime: " + outDateTime);
            //console.log("outtime isValidaionTrue: " + isValidaionTrue);
            if (outDateTime < inDateTime) {
                isValidaionTrue = false;
                //console.log("isValidaionTrue: " + isValidaionTrue);

                //  AlertPopup("Out date time should be greather then In date time");
            }
        }
        if (outDateTime == null || outDateTime == "" && inDateTime != null || inDateTime != "") {
            isValidaionTrue = true;
            //console.log("data saved succesfully");
        }
        var isCheckIn = true;
        if (isValidaionTrue) {
            if (parseInt(inHrs) == 0 && parseInt(inMin) == 0) {
                isCheckIn = false;
                //alert("isCheckIn: " + isCheckIn);
            }
            var values = {};
            values.Id = timeCardId;
            values.UserId = userId;
            values.InDateTime = inDateTime;
            values.OutDateTime = outDateTime;
            values.IsCheckIn = isCheckIn;
            values.Day = day;
            values.IsRemoveFlag = isRemovalFlag;
            console.log("SaveAllTime: " + inHrs + " : " + inMin + " : " + outHrs + " : " + outMin + " : " + timeCardId);
            console.log(values);
            if (timeCardId > 0) {
                if (inHrs == "" && inMin == "" && outHrs == "" && outMin == "") {
                    inHrs = "0";
                    inMin = "0";
                    outHrs = "0";
                    outMin = "0";
                    values.IsRemoveFlag = true;
                }
            }
            if (parseInt(inHrs) >= 0 || parseInt(inMin) >= 0 || parseInt(outHrs) >= 0 || parseInt(outMin) >= 0) {
                $.ajax({
                    url: baseUrl + '/TimeCard/TimeCard/DispatcherTimeCard',
                    data: JSON.stringify(values),
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    async: false,
                    success: function (data) {
                        if (data == true) {

                            $(rowlocal).find("input[id='txtOutMin']").bind('change');
                            $(rowlocal).find("input[id='txtInMin']").bind('change');
                            $(rowlocal).find("input[id='txtOutHrs']").bind('change');
                            $(rowlocal).find("input[id='txtInHrs']").bind('change');

                            //SuccessPopup("Success! Your data has been saved!!!!");
                            //GetTimeCardData(); //Commented by DART as it brings back the old values in text fields...
                        }
                        else {
                            AlertPopup("Try again.");
                        }
                    }
                });
            }

            /**/

        }
    });

}
function SaveTime(_this, isCheckIn) {
    var isValidaionTrue = true;
    row = $(_this).closest("tr");
    var date = $(row).find("input[id='hdndate']").val();
    var outDate = $(row).find("input[id='txtOutDateTime']").val();
    //console.log($(row).find("input[id='hdndate']").val());

    //$(row).find("input[id='txtOutMin']").unbind('change');
    //$(row).find("input[id='txtInMin']").unbind('change');
    //$(row).find("input[id='txtOutHrs']").unbind('change');
    //$(row).find("input[id='txtInHrs']").unbind('change');

    var timeCardId = $(row).find("input[type='hidden']").val();
    var inHrs = $(row).find("input[id='txtInHrs']").val();
    var isRemovalFlag = $(row).find("input[id='hdnIsRemovalFlag']").attr('value');
    //console.log("hdnIsRemovalFlag" + hdnIsRemovalFlag);
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
        //console.log("isValidaionTrue success: " + isValidaionTrue);
    }
    else {
        isValidaionTrue = false;
        //console.log("isValidaionTrue: " + isValidaionTrue);
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
        //console.log("OutTime: " + outDateTime);
        //console.log("outtime isValidaionTrue: " + isValidaionTrue);
        if (outDateTime < inDateTime) {
            isValidaionTrue = false;
            //console.log("isValidaionTrue: " + isValidaionTrue);

            //  AlertPopup("Out date time should be greather then In date time");
        }
    }
    if (outDateTime == null || outDateTime == "" && inDateTime != null || inDateTime != "") {
        isValidaionTrue = true;
        //console.log("data saved succesfully");
    }


    if (isValidaionTrue) {
        var values = {};
        values.Id = timeCardId;
        values.UserId = userId;
        values.InDateTime = inDateTime;
        values.OutDateTime = outDateTime;
        values.IsCheckIn = isCheckIn;
        values.Day = day;
        IsRemoveFlag = isRemovalFlag;

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

function BindNewTimecard(_this, isCheckIn) {
    var isValidaionTrue = true;
    row = $(_this).closest("tr");
    var date = $(row).find("input[id='hdndate']").val();
    var outDate = $(row).find("input[id='txtOutDateTime']").val();
    var inDate = $(row).find("input[id='txtInDateTime']").val();


    var outDateField = $(row).find("input[id='txtOutDateTime']");
    var inDateField = $(row).find("input[id='txtInDateTime']");

    var timeCardId = $(row).find("input[type='hidden']").val();
    timeCardId = "";//DART: For newly added time card, no need to have timecardid


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
        //console.log("isValidaionTrue success: " + isValidaionTrue);
    }
    else {
        //isValidaionTrue = false;
        //console.log("isValidaionTrue: " + isValidaionTrue);
        $(row).find("input[id='txtOutHrs']").val("00");
        $(row).find("input[id='txtOutMin']").val("00");
        AlertPopup("Please enter IN Hours or Minutes.");
    }

    var outDateTime = null;
    if (outHrs >= 0 || outMin >= 0) {
        var ccOutDateTime = outDate + ' ' + outHrs + ':' + outMin;
        outDateTime = new Date(ccOutDateTime);
    }

    if (outDateTime == null || outDateTime == "" && isValidaionTrue) {
        //console.log("OutTime: " + outDateTime);
        //console.log("outtime isValidaionTrue: " + isValidaionTrue);
        if (outDateTime < inDateTime) {
            isValidaionTrue = false;
            //console.log("isValidaionTrue: " + isValidaionTrue);

            //  AlertPopup("Out date time should be greather then In date time");
        }
    }
    if (outDateTime == null || outDateTime == "" && inDateTime != null || inDateTime != "") {
        isValidaionTrue = true;
        //console.log("data saved succesfully");
    }
    var timeCard = "";

    var rquired = "";
    var inOnfocusout = "";
    var outOnfocusout = "";
    //inOnfocusout = 'onfocusout="SaveTime(this,true)"';
    //outOnfocusout = 'onfocusout="SaveTime(this,false)"';
    inOnfocusout = 'onfocusout="RecalculateHourAndprice(this)"';
    outOnfocusout = 'onfocusout="RecalculateHourAndprice(this)"';
    var dtpicker = 'jqueryui-marker-datepicker';
    var diffHrs = "00";
    var diffMins = "00";
    var addIcon = "";
    var removeIcon = "";
    //addIcon = "<span onclick='BindNewTimecard(this,true); return false;'><i class='fas fa-plus'><span>";

    removeIcon = "<span id='removeIcon' onclick='RemoveTimecard(this,true); return false;'><i class='fas fa-minus'><span>";
    if (isValidaionTrue) {
        //console.log("DATE11: " + date + " : " + inDate + " : " + outDate + " : " + dtpicker);
        timeCard += '<tr><td><input type="text" style="display:none"   id="hdnIsRemovalFlag" value="false"/><input type="hidden"  id="hdnTimeCardId" value="' + timeCardId + '"/><input type="hidden" id="hdndate" value="' + date + '"/><span id="spnDay">' + day + '</span></td><td class="text-center"><div class="timeWrapper d-inline-flex align-items-center">' +
            '<input type="text" readonly="readonly" onkeypress="return onlyNumeric(event)" id="txtInDateTime" autocomplete="off" placeholder="MM-DD-YYYY" class="form-control txtInDate dateMask" value="' + dateFormat(inDate, "mm-dd-yyyy") + '" /><span>&nbsp</span> ' +
            '<input type="text" ' + rquired + ' onkeypress="return onlyNumeric(event)" id="txtInHrs" placeholder="HH" class="form-control hh"  ' + inOnfocusout + '  maxlength="2" value="" /><span>:</span>' +
            '<input type="text" ' + rquired + ' onkeypress="return onlyNumeric(event)" id="txtInMin" placeholder="MM" class="form-control mm"  ' + inOnfocusout + '  maxlength="2" value="" /></div></td>' +
            '<td class="text-center">To</td>' +
            '<td class="text-center">' +
            '<div class="timeWrapper d-inline-flex align-items-center">' +
            '<input type="text" ' + rquired + ' onkeypress="return onlyNumeric(event)" id="txtOutDateTime" autocomplete="off" placeholder="MM-DD-YYYY" class="form-control txtOutDate ' + dtpicker + ' dateMask" value="' + dateFormat(outDate, "mm-dd-yyyy") + '" ' + outOnfocusout + '  /><span>&nbsp</span>' +
            '<input type="text" ' + rquired + ' onkeypress="return onlyNumeric(event)" id="txtOutHrs" placeholder="HH" class="form-control hh"  ' + outOnfocusout + ' maxlength="2" value="" /><span>:</span>' +
            '<input type="text" ' + rquired + ' onkeypress="return onlyNumeric(event)" id="txtOutMin" placeholder="MM" class="form-control mm"  ' + outOnfocusout + ' maxlength="2" value="" />' + '</div></td><td width="50px" id="add-icon">' + addIcon + ' ' + removeIcon +
            '</td><td>' + diffHrs + ':' + diffMins + '</td></tr>';
        $(row).after(timeCard);
        $(row).find('#addIcon').remove();
        RecalculateHourAndprice(_this);
        ////DART added the below to enable the Date Picker in txtOutDate...
        startEndDate();
    }
}

function RemoveTimecard(_this, isChecked) {
    var rowlocal = $(_this).closest("tr");
    var timeCardId = $(rowlocal).find("input[type='hidden']").val();
    //console.log("timeCardId:"+timeCardId);
    var addIcon = "<span id='addIcon'  onclick='BindNewTimecard(this,true); return false;'><i class='fas fa-plus'><span>";
    //$(row).prev().find('#txtOutMin').after(addIcon);
    //alert($(row).prev())
    //alert($(row).prev().find('#add-icon'));
    $(rowlocal).prev().find('#add-icon').html(addIcon);



    var PreviousDifferenceInHourAndSec = $(rowlocal).find('td:last-child').text();
    var PreviousDifferenceInHourAndSecArr = $(rowlocal).find('td:last-child').text().split(':');
    var PreviousDifferenceInHour = PreviousDifferenceInHourAndSecArr[0];
    var PreviousDifferenceInAndSec = PreviousDifferenceInHourAndSecArr[1];

    totalSec = 0;
    totalHour = PreviousDifferenceInHour;
    totalMin = PreviousDifferenceInAndSec;
    var currentTotalHours = $("#spnGradTotal").text().split(':')[0];
    var currentTotalMins = $("#spnGradTotal").text().split(':')[1];

    var TdiffHrs = totalHour; // hours
    //console.log("TdiffHrs: " + TdiffHrs);
    TdiffHrs = Math.abs(parseInt(currentTotalHours) - parseInt(TdiffHrs));
    TdiffHrs = TdiffHrs.toString().length < 2 ? ('0' + TdiffHrs) : TdiffHrs;

    //var TdiffMins = Math.floor((totalSec / (1000 * 60)) % 60); // minutes
    var TdiffMins = totalMin; // minutes
    TdiffMins = Math.abs(parseInt(currentTotalMins) - parseInt(TdiffMins));
    //console.log(TdiffMins);
    if (TdiffMins > 59) {
        //TdiffMins = "00";
        TdiffHrs = parseInt(TdiffHrs) + 1;
        TdiffMins = TdiffMins - 60;
    }
    TdiffMins = TdiffMins.toString().length < 2 ? ('0' + TdiffMins) : TdiffMins;

    $("#spnGradTotal").text(TdiffHrs + ':' + TdiffMins);
    $("#spnPrintGrandTotal").text(TdiffHrs + ':' + TdiffMins);
    //$(row).find("input[id='hdnIsRemovalFlag']").val('true');
    $(rowlocal).find("input[id='hdnIsRemovalFlag']").attr('value', true);
    //$(row).find("input[id='hdnIsRemovalFlag']").bind('change');
    //console.log("hdnIsRemovalFlag:" + $(row).find("input[id='hdnIsRemovalFlag']").val());
    $(rowlocal).hide();


    /*$(row).find("input[id='txtOutMin']").val('00');
    $(row).find("input[id='txtInMin']").val('00');
    $(row).find("input[id='txtOutHrs']").val('00');
    $(row).find("input[id='txtInHrs']").val('00');*/
}

function RecalculateHourAndprice(_this) {
    var row = $(_this).closest("tr");
    if ($(_this).prop('nodeName') != "INPUT") {
        return false;
    }
    var date = $(row).find("input[id='hdndate']").val();
    var userRole = $("#hdnUserRole").val();
    var totalSec = parseInt($("#spnGradTotal").text().split(':')[1]);
    var totalHour = parseInt($("#spnGradTotal").text().split(':')[0]);
    var totalMin = 0;
    var currentElementVal = $(_this).val();
    //console.log("currentElementVal: " + currentElementVal + " : " + currentElementVal.length);
    if (currentElementVal.length <= 0) {//Avoid calling if nothing enter in the hours & Mins input
        // return false;
    }
    if (currentElementVal.length > 0) {
        currentElementVal = currentElementVal.length < 2 ? ('0' + currentElementVal) : currentElementVal;
    }
    $(_this).val(currentElementVal);

    if ($(_this).prop('id').includes('Hrs')) {
        if (parseInt(currentElementVal) > 23) {
            currentElementVal = 23
        }
    } else if ($(_this).prop('id').includes('Min')) {
        if (parseInt(currentElementVal) > 59) {
            currentElementVal = 59
        }
    } $(_this).val(currentElementVal.toString());

    var txtInHour = $(row).find("input[id='txtInHrs']").val();
    var txtInMin = $(row).find("input[id='txtInMin']").val();
    var txtInDate = $(row).find("input[id='txtInDateTime']").val();

    var txtOutHour = $(row).find("input[id='txtOutHrs']").val();
    var txtOutMin = $(row).find("input[id='txtOutMin']").val();
    var txtOutDate = $(row).find("input[id='txtOutDateTime']").val();
    if (txtInHour == "") txtInHour = "00";
    if (txtOutHour == "") txtOutHour = "00";
    var ccInDateTime = new Date(date + ' ' + txtInHour + ':' + txtInMin).getTime();
    var ccOutDateTime = new Date(txtOutDate + ' ' + txtOutHour + ':' + txtOutMin).getTime();
    //console.log(ccInDateTime);
    var inDate = ConvertDate("/Date(" + ccInDateTime + ")/", true);
    var outDate = ConvertDate("/Date(" + ccOutDateTime + ")/", true);

    console.log("txtOutDate: " + txtOutDate + " : " + outDate);
    console.log("InDate Bef: " + inDate);
    console.log("outDate Bef: " + outDate);
    var inHrs = "";
    var inMin = "";
    if (inDate != undefined) {
        console.log("InDate not undefined");
        console.log("outDate Aft: " + outDate);
        inHrs = dateFormat(inDate, "HH");
        inMin = dateFormat(inDate, "MM");
    }

    var outHrs = "";
    var outMin = "";


    var diffHrs = "00";
    var diffMins = "00";
    if (outDate != undefined) {
        console.log("OutDate not undefined");
        outHrs = dateFormat(outDate, "HH");
        outMin = dateFormat(outDate, "MM");
        console.log("inDate: " + inDate);
        console.log("outDate: " + outDate);
        inDate = new Date(inDate);
        outDate = new Date(outDate);

        var diffMs = (outDate - inDate); // milliseconds between now & Christmas\   
        diffMs = (parseInt(ccOutDateTime) - parseInt(ccInDateTime)); //DART: Added this to solve additional 1 hour due to time conversion from EDT to EST...
        totalSec = totalSec + diffMs;
        var diffDays = Math.floor(diffMs / 86400000); // days
        //diffHrs = Math.floor((diffMs % 86400000) / 3600000); // hours       
        console.log("inDate1: " + inDate);
        console.log("outDate1: " + outDate);


        if (diffMs >= 0) {

            diffHrs = Math.floor((diffMs) / 3600000); // hours
            //console.log("diffHrs" + diffHrs);
            diffHrs = diffHrs < 10 ? ('0' + diffHrs) : diffHrs;


            totalHour = totalHour + parseInt(diffHrs);
            //console.log("totalHour:"+totalHour);
            //diffMins = Math.round(((diffMs % 86400000) % 3600000) / 60000); // minutes
            diffMins = Math.round(((diffMs) % 3600000) / 60000); // minutes
            diffMins = diffMins < 10 ? ('0' + diffMins) : diffMins;
            totalMin = totalMin + parseInt(diffMins);
            //console.log(totalMin);
            //console.log(totalHour);
            if (totalMin > 59) {

                totalHour = totalHour + 1;
                totalMin = totalMin - 60;
                //console.log("totalHour: " + totalHour);
            }

        }
        else {
            diffHrs = "00";
            diffMins = "00";
        }

        var currentTotalHours = $("#spnGradTotal").text().split(':')[0];
        var currentTotalMins = $("#spnGradTotal").text().split(':')[1];


        var PreviousDifferenceInHourAndSec = $(row).find('td:last-child').text();
        //console.log("PreviousDifferenceInHourAndSec:" + PreviousDifferenceInHourAndSec);
        var PreviousDifferenceInHourAndSecArr = $(row).find('td:last-child').text().split(':');
        var PreviousDifferenceInHour = PreviousDifferenceInHourAndSecArr[0];
        var PreviousDifferenceInMin = PreviousDifferenceInHourAndSecArr[1];

        $(row).find('td:last-child').text(diffHrs + ":" + diffMins);

        var currentDifferenceInHourAndSec = $(row).find('td:last-child').text();
        //console.log("currentDifferenceInHourAndSec:" + currentDifferenceInHourAndSec);
        var currentDifferenceInHourAndSecArr = $(row).find('td:last-child').text().split(':');
        var currentDifferenceInHour = currentDifferenceInHourAndSecArr[0];
        var currentDifferenceInMin = currentDifferenceInHourAndSecArr[1];


        //const earlierDateTime = new Date(dateFormat(new Date(), "mm-dd-yyyy") + " "+PreviousDifferenceInHourAndSec).getTime();
        //console.log("earlierDateTime:"+earlierDateTime);
        //const laterDateTime = new Date(dateFormat(new Date(), "mm-dd-yyyy") + " " + currentDifferenceInHourAndSec).getTime();
        //console.log("laterDateTime:" + laterDateTime);
        //const recaldiffMS = laterDateTime - earlierDateTime;

        const earlierDateTimeMin = parseInt(parseInt(PreviousDifferenceInHour) * 60) + parseInt(PreviousDifferenceInMin);
        const laterDateTimeMin = parseInt(parseInt(currentDifferenceInHour) * 60) + parseInt(currentDifferenceInMin);
        const recaldiffMS = (laterDateTimeMin - earlierDateTimeMin) * 60000;
        //console.log("currentDifferenceInHour:" + currentDifferenceInHour * 60);
        //console.log("currentDifferenceInMin:", parseInt(parseInt(currentDifferenceInHour) * 60) + parseInt(currentDifferenceInMin));
        //console.log("recaldiffMS:" + recaldiffMS);


        var recalculateHours = Math.floor((recaldiffMS) / 3600000);//hours
        var recalculateMins = (Math.round(((recaldiffMS) % 3600000) / 60000)); // minutes);
        //var recalculateHours = Math.floor((recaldiffMS) / 60);//hours
        //var recalculateMins = (Math.round(recaldiffMS)); // minutes);
        //console.log("recaldiffMS:" + recalculateHours);
        //console.log("totalHour:" + recalculateMins);
        //totalSec = 0;
        totalHour = recalculateHours;
        totalMin = recalculateMins;

        console.log("MTC: " + totalSec + " : " + totalMin + " : " + totalHour);
        if (totalSec > 0 || totalHour > 0 || totalMin > 0) {
            //console.log("totalSec: "+totalSec);
            // var TdiffHrs = Math.floor((totalSec / (1000 * 60 * 60))); // hours
            var TdiffHrs = totalHour; // hours            
            TdiffHrs = Math.abs(parseInt(currentTotalHours) + parseInt(TdiffHrs));

            TdiffHrs = TdiffHrs.toString().length < 2 ? ('0' + TdiffHrs) : TdiffHrs;

            //var TdiffMins = Math.floor((totalSec / (1000 * 60)) % 60); // minutes
            var TdiffMins = totalMin; // minutes
            TdiffMins = Math.abs(parseInt(currentTotalMins) + parseInt(TdiffMins));
            //console.log(TdiffMins);
            if (TdiffMins > 59) {
                //TdiffMins = "00";
                TdiffHrs = parseInt(TdiffHrs) + 1;
                TdiffMins = TdiffMins - 60;
            }
            TdiffMins = TdiffMins.toString().length < 2 ? ('0' + TdiffMins) : TdiffMins;

            $("#spnGradTotal").text(TdiffHrs + ':' + TdiffMins);
            $("#spnPrintGrandTotal").text(TdiffHrs + ':' + TdiffMins);

        }
        else {
            $("#spnGradTotal").text('00:00');
            $("#spnPrintGrandTotal").text('00:00');
        }
        //DART: Added the bwlow block to fix the issue with total hours
        totalMin = 0;
        totalHour = 0;
        $("#timeCard > tbody > tr").each(function () {
            var mins = parseInt($(this).find('td:last').text().split(":")[1]);
            var hrs = parseInt($(this).find('td:last').text().split(":")[0]);
            totalHour = totalHour + hrs;
            totalMin = totalMin + mins;
            var minsToHrs = Math.floor(totalMin / 60);
            if (minsToHrs > 0) {
                totalHour = totalHour + minsToHrs;
                totalMin = totalMin - (minsToHrs * 60);
            }
            console.log("DAY Total: " + hrs + " : " + mins + " : " + totalHour + " : " + totalMin + " : " + minsToHrs);
        });
        var totalHourStr = "" + totalHour.toString();
        var totalMinStr = "" + totalMin.toString();
        if (totalHour.toString().length < 2) {
            totalHourStr = "0" + totalHour.toString();
        }
        if (totalMin.toString().length < 2) {
            totalMinStr = "0" + totalMin.toString();
        }
        if (totalHourStr == "") totalHourStr = "00";
        if (totalMinStr == "") totalMinStr = "00";
        $("#spnGradTotal").text(totalHourStr + ':' + totalMinStr);
        $("#spnPrintGrandTotal").text(totalHourStr + ':' + totalMinStr);
        //
        if ($.trim(userRole).toLowerCase() == "Management".toLocaleLowerCase()) {
            CalculateTotalPrice();
        }

    }
}


function SaveTimeNew(_this, isCheckIn) {
    var isValidaionTrue = true;
    row = $(_this).closest("tr");
    var date = $(row).find("input[id='hdndate']").val();
    var outDate = $(row).find("input[id='txtOutDateTime']").val();
    //IsRemoveFlag

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
        values.IsRemoveFlag = isRemovalFlag;
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
    //console.log("pre Date: ", currentDate);
    var weekStartDay = $("#spnStartDate").text();
    var from = weekStartDay.split("/");

    var date = new Date(from[2], from[0] - 1, from[1]);
    console.log("Previous bef Date: ", date);
    currentDate = new Date(date.setDate(date.getDate() - 7));
    console.log("Previous Date: ", currentDate);
    if (isPrevious == false) {
        GetTimeCardData();
    }
    isPrevious = true;
    GetTimeCardData();
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

function previous() {
    //console.log("pre Date: ", currentDate);
    var weekStartDay = $("#spnStartDate").text();
    var from = weekStartDay.split("/");

    var date = new Date(from[2], from[0] - 1, from[1]);
    console.log("Previous bef Date: ", date);
    currentDate = new Date(date.setDate(date.getDate() - 7));
    //console.log("Previous Date: ", currentDate);
    if (isPrevious == false) {
        GetTimeCardData();
    }
    isPrevious = true;
    GetTimeCardData();
}

function next() {
    isPrevious = false;
    var todaydate = new Date();
    var weekStartDay = $("#spnStartDate").text();
    var from = weekStartDay.split("/");
    var date = new Date(from[2], from[0] - 1, from[1]);
    currentDate = new Date(date.setDate(date.getDate() + 7));
    if (currentDate < todaydate) {
        GetTimeCardData();
    }
}

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
        url: baseUrl + '/TimeCard/TimeCard/GetIncentiveCardData',
        data: JSON.stringify(values),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            // if (data.length > 0) {
           // GetIncentiveGridData();
            BindTimeCardList(data);
            var HoursWorked = $("#spnGradTotal").text();
            HoursWorked = HoursWorked.split(':')[0];
            $("#txtHoursWorked").val(HoursWorked);
            //}

        }
    });
}
var removeIcon = "";
function calculateTotal(rowName) {
    // Get the input and value elements by rowName
    const inputElement = document.getElementById(rowName + 'Input');
    const valueElement = document.getElementById(rowName + 'Value');
    const resultElement = document.getElementById(rowName + 'Result');

    // Get the value from the input field
    const inputValue = parseFloat(inputElement.value) || 0;

    // Get the value from the corresponding <td>
    const value = parseFloat(valueElement.textContent) || 0;

    // Calculate the product
    const product = inputValue * value;

    // Update the result <td> with the product
    resultElement.textContent = product;
}

function BindTimeCardList(_data) {
    // GetIncentiveGridData();
    // let IncentiveGridData = GetIncentiveGridData();
    // console.log("GridData bef grid: ", IncentiveGridData);
    //  console.log("GridData bef : ", incentiveGrid);
    if (_data != null) {
        //$("#txtHourlyRate").attr('value', _data.HourlyRate);
        //$("#txtTotalPay").attr('value', _data.TotalPay);
        //$("#txtLoan").attr('value', _data.Loan);
        //$("#txtDeductions").attr('value', _data.Deduction);
        //$("#txtDescription").attr('value', _data.Description);

        ////DART commented the above and added the below block to fix the issue of same text field values appearing for all driver Time Cards...
        //console.log("Hourly Rate: " + _data.HourlyRate);

        $("#txtHourlyRate").val(_data.HourlyRate);
        $("#txtTotalPay").val(_data.TotalPay);
        $("#txtLoan").val(_data.Loan);
        $("#txtDeductions").val(_data.Deduction);
        $("#txtDailyRate").val(_data.DailyRate);
        $("#txtIncentive").val(_data.Incentive);
        $("#txtGrossPay").val(_data.GrossPay);
        $("#txtReimbursements").val(_data.Reimbursement);
        $("#txtDescription").val(_data.Description);
        ////DART END...

        $("#lblPrintHourlyRate").text(_data.HourlyRate);
        $("#lblPrintLoan").text(_data.Loan);
        $("#lblPrintTotalPay").text(_data.TotalPay);
        $("#lblPrintDeduction").text(_data.Deduction);
        $("#lblPrintDescription").text(_data.Description);
    }
    $("#spnDriverName").text(_data.UsernName);
    $("#spnPrintDriverName").text(_data.UsernName);

    $("#IncentiveCard tbody").empty();
    $("#tblTimeCardPrint tbody").empty();
    for (let i = 0; i <= 6; i++) {
        let curr = currentDate;
        let first = curr.getDate() - (curr.getDay() < 4 ? (curr.getDay() + 7) : curr.getDay()) + (i + 4);
        let date = new Date(curr.setDate(first));
        date = dateFormat(date, "yyyy-mm-dd");
        var from = date.split("-");
        var timeCardDate = new Date(from[0], from[1] - 1, from[2]);
        //var timeCardDate = new Date(f);

        var todayDate = new Date();

        var userRole = $("#hdnUserRole").val();
        if ($.trim(userRole).toLowerCase() == "Dispatcher".toLocaleLowerCase()) {
            if (timeCardDate != todayDate) {
                rquired = 'readonly="readonly"';
            }
            else {

            }
        }
        else if ($.trim(userRole).toLowerCase() == "Management".toLocaleLowerCase()) {

            if (timeCardDate > todayDate) {
                rquired = 'readonly="readonly"';
            }
            else {

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
    }
    var timeCard = "";
    var timCardPrint = "";

    // GetIncentiveGridData();
    incentiveGrid = GetIncentiveGridData();
    var datas = {};
    if (incentiveGrid.length > 0) {
        if (incentiveGrid.length > 0) {
            datas = incentiveGrid[0];
        }
        else {
            datas = {
                "Pallets": "",
                "Boxes": "",
                "Weight": "",
                "ShipmentLocation": "",
                "FumigationLocation": "",
                "KG": "",
                "LBS": "",
                "GridData": ""
            };
        }
        console.log("datas: ", incentiveGrid.length);
        console.log("datas values: ", datas);
        var ShipmentLocation = datas.ShipmentLocation || '';
        var ShipmentLocationArray = ShipmentLocation ? ShipmentLocation.split('$') : [];
        // var fumigationLocation = datas.FumigationLocation.split('$');
        var fumigationLocation = datas.FumigationLocation || ''; // Ensure fumigationLocation is a string or an empty string
        var fumigationLocationArray = fumigationLocation ? fumigationLocation.split('$') : []; // Split or return an empty array
        var GridData = JSON.parse(datas.GridData);
        console.log("datas GridData: ", GridData);
        //const arr = ["FLORIDA", "MIAMI", "FLORIDA"];
        let countLocation = "";
        if (ShipmentLocationArray.length >= 1) {
            countLocation = countOccurrences(ShipmentLocationArray)
        }
        let countFumLocation = "";
        if (fumigationLocationArray.length >= 1) {
            countFumLocation = countOccurrences(fumigationLocationArray)
        }
        console.log("ShipmentLocation count: ", countLocation);
        var shipmentLoc = "";
        var fumigationLoc = "";
        //for (let i = 4; i < GridData.length; i++) {
        //    for (const key in countLocation) {
        //        if (countLocation.hasOwnProperty(key)) {
        //            shipmentLoc += `<tr><td  style="font-weight: bold;">Shipment ${key}</td><td style="font-weight: bold;" id="ship${key}Value">${countLocation[key]}</td><td style="padding: 5px;"><input type="number" oninput="calculateTotal('ship${key}')" id="ship${key}Input" onkeypress="return onlyNumeric(event)" style="height:30px;" value="${GridData[i].Rate}"/></td><td id="ship${key}Result">${GridData[i].Total}</td></tr>`;
        //        }
        //    }
        //    for (const key in countFumLocation) {
        //        if (countFumLocation.hasOwnProperty(key)) {
        //            fumigationLoc += `<tr><td  style="font-weight: bold;">Fumigation ${key}</td><td style="font-weight: bold;" id="fumValue">${countFumLocation[key]}</td><td style="padding: 5px;"><input type="number" oninput="calculateTotal('fum')" id="fumInput" onkeypress="return onlyNumeric(event)" style="height:30px;" value="${GridData[i].Rate}"/></td><td id="fumResult">${GridData[i].Total}</td></tr>`;
        //        }
        //    }
        //}
        const shipCount = Object.keys(countLocation).length;
        const fumCount = Object.keys(countFumLocation).length;
        var shipRate = [];
        var shipTotal = [];
        var countKey = 0;
        for (let i = 4; i < GridData.length; i++) {

            console.log("value of i: " + i + " GridData[i].Rate", GridData[i].Rate);
            const rate = GridData[i].Rate;
            const total = GridData[i].Total;
            countKey++;
            if (countKey <= shipCount) {
                shipRate.push(rate);
                shipTotal.push(total);
                console.log("shipCount: ", shipCount);
                if (shipRate.length == shipCount) {
                    console.log("shipRate: ", shipRate);

                    // for (const key in countLocation) {
                    for (let j = 0; j < shipRate.length; j++) {
                        // if (countLocation.hasOwnProperty(key)) {
                        const key = Object.keys(countLocation)[j];
                        const value = countLocation[key];
                        console.log("value of rate: ", rate);
                        shipmentLoc += `<tr><td  style="font-weight: bold;">Shipment ${key}</td><td style="font-weight: bold;" id="ship${key}Value">${countLocation[key]}</td><td style="padding: 5px;"><input type="number" oninput="calculateTotal('ship${key}')" id="ship${key}Input" onkeypress="return onlyNumeric(event)" style="height:30px;" value="${shipRate[j]}"/></td><td id="ship${key}Result">${shipTotal[j]}</td></tr>`;
                        // }
                    }



                }
            }
            else {
                for (const key in countFumLocation) {
                if (countFumLocation.hasOwnProperty(key)) {
                    fumigationLoc += `<tr><td  style="font-weight: bold;">Fumigation ${key}</td><td style="font-weight: bold;" id="fumValue">${countFumLocation[key]}</td><td style="padding: 5px;"><input type="number" oninput="calculateTotal('fum')" id="fumInput" onkeypress="return onlyNumeric(event)" style="height:30px;" value="${rate}"/></td><td id="fumResult">${total}</td></tr>`;
                }
            }
            }



            // Create HTML for Shipment (countLocation length is 2)


            // Create HTML for Fumigation (countFumLocation length is 1)
            //for (const key in countFumLocation) {
            //    if (countFumLocation.hasOwnProperty(key)) {
            //        fumigationLoc += `<tr><td  style="font-weight: bold;">Fumigation ${key}</td><td style="font-weight: bold;" id="fumValue">${countFumLocation[key]}</td><td style="padding: 5px;"><input type="number" oninput="calculateTotal('fum')" id="fumInput" onkeypress="return onlyNumeric(event)" style="height:30px;" value="${rate}"/></td><td id="fumResult">${total}</td></tr>`;
            //    }
            //}
        }


        timeCard =

            '<tr style="color:#fff;"><th style="background:#7ca337 !important">PRODUCTIVITY RECORD</th><th style="background:#7ca337 !important">QUANTITY</th><th style="background:#7ca337 !important">RATE</th><th style="background:#7ca337 !important">TOTAL</th></tr>' +
            `<tr><td style="font-weight: bold;">PALLETS</td><td style="font-weight: bold;" id="palletsValue">${datas.Pallets}</td><td style="padding: 5px;"><input type="number" oninput="calculateTotal('pallets')" id="palletsInput" onkeypress="return onlyNumeric(event)" style="height:30px;" value="${GridData[0].Rate}"/></td><td id="palletsResult">${GridData[0].Total}</td></tr>` +
            `<tr><td style="font-weight: bold;">BOXES</td><td style="font-weight: bold;" id="boxesValue">${datas.Boxes}</td><td style="padding: 5px;"><input type="number" oninput="calculateTotal('boxes')" id="boxesInput" onkeypress="return onlyNumeric(event)" style="height:30px;" value="${GridData[1].Rate}"/></td><td id="boxesResult">${GridData[1].Total}</td></tr>` +
            `<tr><td style="font-weight: bold;">WEIGHT KGS</td><td style="font-weight: bold;" id="kgValue">${datas.KG}</td><td style="padding: 5px;"><input type="number" oninput="calculateTotal('kg')" id="kgInput" onkeypress="return onlyNumeric(event)" style="height:30px;" value="${GridData[2].Rate}"/></td><td id="kgResult">${GridData[2].Total}</td></tr>` +
            `<tr><td style="font-weight: bold;">WEIGHT LBS</td><td style="font-weight: bold;" id="lbsValue">${datas.LBS}</td><td style="padding: 5px;"><input type="number" oninput="calculateTotal('lbs')" id="lbsInput" onkeypress="return onlyNumeric(event)" style="height:30px;" value="${GridData[3].Rate}"/></td><td id="lbsResult">${GridData[3].Total}</td></tr>` +
            `${shipmentLoc}` +
            `${fumigationLoc}`;

        // _data.Pallets = datas[0].Pallets;
    }

    timCardPrint = '<tr><td></td></tr>';


    $("#IncentiveCard tbody").append(timeCard);
    $("#tblTimeCardPrint tbody").append(timCardPrint);


    if ($.trim(userRole).toLowerCase() == "Management".toLocaleLowerCase()) {
        CalculateTotalPrice();
    }

    $("#modalIncentiveCard").modal("show");
    //  console.log("after modal open: ", GetIncentiveGridData());
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
        content: '<b>Are you sure to finalize Incentivecard for the week?</b>',
        type: 'blue',
        typeAnimated: true,
        buttons: {
            OK: {
                btnClass: 'btn-blue',
                action: function () {
                   // saveAllTime();
                    SaveIncentiveCardAmount();
                }
            },
            cancel: {
                btnClass: 'btn-red',
            }
        }
    })
}
//});

function btnClear() {
    $("#txtHourlyRate").val("");
    $("#txtTotalPay").val("");
    $("#txtLoan").val("");
    $("#txtDeductions").val("");
    $("#txtHoursWorked").val("");
    $("#txtTotalCheck").val("");
    $("#txtRemaining").val("");
    $("#txtGrossPay").val("");
    $("#txtIncentive").val("");
    $("#txtDailyRate").val("");
}

function SaveIncentiveCardAmount() {
    var values = {};
    values.WeekStartDay = $("#spnStartDate").text();
    values.WeekEndDay = $("#spnEndtDate").text();
    values.HourlyRate = $("#txtHourlyRate").val();
    values.TotalPay = $("#txtTotalPay").val();
    values.Loan = $("#txtLoan").val();
    values.Deduction = $("#txtDeductions").val();
    values.TotalCheck = $("#txtTotalCheck").val();
    values.Incentive = $("#txtIncentive").val();
    values.DailyRate = $("#txtDailyRate").val();
    values.GrossPay = $("#txtGrossPay").val();
    values.Remaining = $("#txtRemaining").val();
    values.UserId = userId;
    // Get a reference to the table
    const table = document.getElementById('IncentiveCard');
    console.log("table.rows.length: ", table.rows.length);
    console.log("table.rows: ", table.rows);
    // Initialize an array to store the objects
    const dataArray = [];

    // Loop through the rows in the table
    for (let i = 1; i < table.rows.length; i++) { // Start from 1 to skip the header row
        const row = table.rows[i];
        console.log("rows: ", row);
        if (row.cells.length >= 4) {
            // Extract values from the 4th and 5th <td> elements
            const fourthTd = row.cells[2].querySelector('input').value;
            const fifthTd = row.cells[3].textContent;

            // Create an object with the extracted values and push it to the array
            const dataObject = {
                Rate: fourthTd,
                Total: fifthTd
            };

            dataArray.push(dataObject);
        }
    }

    // Now, dataArray contains an array of objects with values from the 4th and 5th <td> elements
    console.log("dataArray: ",dataArray);
    values.gridData = JSON.stringify(dataArray);
    console.log(values);
    //return false;
    $.ajax({
        url: baseUrl + '/TimeCard/TimeCard/SaveIncentiveCardAmount',
        data: JSON.stringify(values),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: function (data) {
            //console.log("data: ", data);
            if (data == true) {
                console.log("data true: ", JSON.stringify(values));
                SuccessPopup("Success! Your data has been saved!");
            }
        },
        error:function(XMLHttpRequest, textStatus, errorThrown) {
            //alert("Status: " + textStatus);

            console.log("Error: " + errorThrown);
        }
    });
}

function CalculateTotalPrice() {

    var hourlyRate = $("#txtHourlyRate").val();
    var grandTotal = $("#spnGradTotal").text();
    var Loan = $("#txtLoan").val() == "" ? 0 : $("#txtLoan").val();
    var deduction = $("#txtDeductions").val() == "" ? 0 : $("#txtDeductions").val();
    var reimbursement = $("#txtReimbursements").val() == "" ? 0 : $("#txtReimbursements").val();

    //ADDED FOR INCENTIVE
    var hoursWorked = $("#txtHoursWorked").val();
    var grossPay = $("#txtGrossPay").val();
    var incentive = $("#txtIncentive").val();
    var dailyRate = $("#txtDailyRate").val();

    if (Loan > 0) {
        $("#txtDeductions").prop('disabled', false);
    }
    else {
        $("#txtDeductions").prop('disabled', true);
    }


    grandTotal = grandTotal.split(':');
    var grossPay = hoursWorked * hourlyRate;
    if (parseInt(dailyRate) > 0) {
        grossPay = grossPay + parseInt(dailyRate);
    }
    else {
        grossPay = grossPay + parseInt(0);
    }

    //totalPay = totalPay + ((hourlyRate / 60) * grandTotal[1]);
    console.log("grossPay: " + grossPay);


    if (grossPay > 0) {
        $("#txtGrossPay").val(ConvertStringToFloat(grossPay));
    }
    if (parseInt(incentive) > 0) {
        totalPay = grossPay + parseInt(incentive);
    }
    else {
        totalPay = grossPay + parseInt(0);
    }
    //if (totalPay > 0) {
    //    if (parseFloat(totalPay) >= parseFloat(deduction)) {
    //        // console.log("totalPay: " + totalPay + " deduction: " + deduction);
    //        totalPay = parseFloat(totalPay) - parseFloat(deduction);
    //    }
    //    else {
    //        //console.log("totalPay: " + totalPay + " deduction: " + deduction);
    //        AlertPopup("Deduction amount should be less than or equal to Total Pay.");
    //        $("#txtDeductions").val("");
    //        deduction = 0;
    //    }
    //}
    if (parseInt(grossPay) > 0) {
        $("#txtTotalPay").val(ConvertStringToFloat(totalPay));
    }
    else {
        $("#txtTotalPay").val(0);
    }
    var totalRemaining = 0;

    if (parseFloat(Loan) >= parseFloat(deduction)) {
        totalRemaining = Loan - deduction;
        $("#txtRemaining").val(ConvertStringToFloat(totalRemaining));
        $("#lblPrintRemaining").text(ConvertStringToFloat(totalRemaining));
    }
    else {
        $("#txtDeductions").val("");
        deduction = 0;
    }
    var totalCheck = totalPay;
    if (parseInt(Loan) > 0) {
        totalCheck = totalCheck + parseInt(Loan) - deduction;
    }
    $("#txtTotalCheck").val(totalCheck);
    var hourlyRate = $("#txtHourlyRate").val();
    var totalPay = $("#txtTotalPay").val();
    var loan = $("#txtLoan").val();
    var deduction = $("#txtDeductions").val();
    var reimbursement = $("#txtReimbursements").val();
    var totalRemaining = $("#txtRemaining").val();
    var totalCheck = $("#txtTotalCheck").val();

    $("#txtHourlyRate").attr('value', hourlyRate);
    $("#txtTotalPay").attr('value', totalPay);
    $("#txtLoan").attr('value', loan);
    $("#txtDeductions").attr('value', deduction);
    $("#txtReimbursements").attr('value', reimbursement);
    $("#txtRemaining").attr('value', totalRemaining);
    $("#txtTotalCheck").attr('value', totalCheck);

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
    $("td").each(function () {
        $(this).addClass("blueremove");
    });
    var printContents = document.getElementById(divName).innerHTML;

    //console.log("logodiv:"+logodiv);
    //  Restore button visibility
    btnClose.style.visibility = 'visible';
    hrfPrint.style.visibility = 'visible';
    hrfEmail.style.visibility = 'visible';
    btnCaculate.style.visibility = 'visible';
    btnClear.style.visibility = 'visible';


    var originalContents = document.body.outerHTML;
    //console.log("originalContents: ", originalContents);

    document.body.innerHTML = printContents;
    //logodiv.style.display = 'block';
    window.print();

    document.body.innerHTML = originalContents;


}



if (window.matchMedia) {
    var mediaQueryList = window.matchMedia('print');
    mediaQueryList.addListener(function (mql) {
        if (mql.matches) {
            console.log('mql matches');
            //closePopup();
            //window.location.reload();
        } else {
            console.log('mql did NOT match');
            //closePopup();
            //$("body").removeClass("modal-open");

            //location.reload();
            $("td").each(function () {
                $(this).removeClass("blueremove");

            });
        }
    });
}




function closePopup() {
    //window.location.reload();
    console.log("popup closed");
    incentiveGrid = [];
    $(".dvloader").css("display", "none");
    $('#modalTimeCard').modal('toggle');
    $('.modal-backdrop').hide();
    $("#txtTo").text("");
}

function ShowEmailPopup() {
    $('#modalEmail').show();

    $("#modalEmail").modal("show");

    var textSub = $("#spnDriverName").text() + " | TIMECARD WEEK [" + $("#spnStartDate").text() + " To " + $("#spnEndtDate").text() + "]";
    $("#txtSubject").val(textSub);
    $("#txtBody").val("TIMECARD ATTACHED.");
    //console.log("Subject: " + textSub);
}

function btnSendEmail() {

    $("#btnSendEmail").prop('disabled', true);
    var values = {};
    var divName = "mytimeCard";
    //var divName = "modalTimeCard1";


    var printContents = document.getElementById(divName).innerHTML;

    /* Start taking Screenshot of TimeCard Popup */

    //$("#mytimeCard").css('display','block');

    var printDiv = $(document.createElement('div')).attr('id', 'printDiv');
    var imgPrint = document.querySelector("#theimage9");
    //console.log("printContents: ", printContents);
    //To hide the icons that are not getting rendered in image...
    document.getElementsByClassName("fas fa-arrow-circle-left")[0].style.display = 'none';
    document.getElementsByClassName("fas fa-arrow-circle-right")[0].style.display = 'none';
    document.getElementsByClassName("fa fa-save")[0].style.display = 'none';
    document.getElementsByClassName("fa fa-eraser")[0].style.display = 'none';
    document.getElementsByClassName("fa fa-envelope")[0].style.display = 'none';
    document.getElementsByClassName("fa fa-times")[0].style.display = 'none';
    document.getElementById("printImg").style.display = 'none';
    var clsNames = document.getElementsByClassName("fas fa-plus");
    var i;
    for (i = 0; i < clsNames.length; i++) {
        clsNames[i].style.display = 'none';
    }
    $("#txtHourlyRate").css("fontSize", "16px");
    $("#txtHourlyRate").css("padding-top", "12px");
    $("#txtLoan").css("fontSize", "16px");
    $("#txtLoan").css("padding-top", "12px");
    $("#txtTotalPay").css("fontSize", "16px");
    $("#txtTotalPay").css("padding-top", "12px");
    $("#txtDeductions").css("fontSize", "16px");
    $("#txtDeductions").css("padding-top", "12px");
    $("#txtReimbursements").css("fontSize", "16px");
    $("#txtReimbursements").css("padding-top", "12px");
    $("#txtRemaining").css("fontSize", "16px");
    $("#txtRemaining").css("padding-top", "12px");
    /*var dlrNames = document.getElementsByClassName("inpt-dollar");
    var i;
    for (i = 0; i < dlrNames.length; i++) {
        dlrNames[i].style.display = 'none';
    }*/
    //
    var canImg;

    if ($(window).width() < 993) {
        $("#modalTimeCard1").css("width", "900px");
        $("#timeCard tbody").css("display", "revert");
        $("#timeCard tbody td").css("display", "revert");
        $("tfoot tr").css("display", "revert");
        $(".modal-dialog").css("max-width", "100%");
        $(".modal-dialog").css("width", "900px");
        $("#timeCard").css("display", "table");
        $(".hr-rate .row").css("flex-wrap", "nowrap");
        console.log("Screen size: ", $(window).width());
    }
    else {
        // console.log("Screen size below: ", $(window).width());
    }

    html2canvas(document.querySelector("#modalTimeCard1"), {
        allowTaint: false,
        useCORS: true,
        scale: 1.1,
        //width: 1200,
        // height:768,
    }).then(function (canvas9) {
        var theimage9 = canvas9.toDataURL("image/png");
        //canImg = theimage9;
        //console.log("theimage9: ", theimage9);
        if (theimage9 != null) {
            document.querySelector("#theimage9").src = theimage9;
        }
        //document.getElementById("printTimeEmail").appendChild(canvas9); 

    });
    /* End of ScreenShot */

    //  printDiv.innerHTML = canva;
    // canva.appendTo(printDiv);
    // var printContents = document.getElementById("modalTimeCard").innerHTML;
    // console.log("printDiv: ", printDiv);
    setTimeout(function () {

        //TO show the icons back after screenshot...
        document.getElementsByClassName("fas fa-arrow-circle-left")[0].style.display = 'block';
        document.getElementsByClassName("fas fa-arrow-circle-right")[0].style.display = 'block';
        document.getElementsByClassName("fa fa-save")[0].style.display = 'block';
        document.getElementsByClassName("fa fa-eraser")[0].style.display = 'block';
        document.getElementsByClassName("fa fa-envelope")[0].style.display = 'block';
        document.getElementsByClassName("fa fa-times")[0].style.display = 'block';
        document.getElementById("printImg").style.display = 'block';

        var clsNames = document.getElementsByClassName("fas fa-plus");
        var i;
        for (i = 0; i < clsNames.length; i++) {
            clsNames[i].style.display = 'block';
        }

        $("#txtHourlyRate").css("fontSize", "11px");
        $("#txtHourlyRate").css("padding-top", "5px");
        $("#txtLoan").css("fontSize", "11px");
        $("#txtLoan").css("padding-top", "5px");
        $("#txtTotalPay").css("fontSize", "11px");
        $("#txtTotalPay").css("padding-top", "5px");
        $("#txtDeductions").css("fontSize", "11px");
        $("#txtDeductions").css("padding-top", "5px");
        $("#txtReimbursements").css("fontSize", "11px");
        $("#txtReimbursements").css("padding-top", "5px");
        $("#txtRemaining").css("fontSize", "11px");
        $("#txtRemaining").css("padding-top", "5px");


        var canva = document.querySelector("#theimage9").src;

        // console.log("imgPrint: ", imgPrint);
        var to = $.trim($("#txtTo").val());
        console.log("TO Address: ", to);
        var subject = $.trim($("#txtSubject").val());
        console.log("subject: ", subject);
        var description = $.trim($("#txtBody").val());
        var user = $("#spnDriverName").text();
        var body = canva;
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
        console.log("parse error: ", values);
        var parseString = JSON.stringify(values);
        console.log("parse stringify: ", parseString);
        var ParseJson = JSON.parse(parseString);
        console.log("parse json: ", JSON.parse(parseString));

        if (isValid) {

            //console.log("txto: " + $("#txtTo").val());
            //$("#txtTo").val(" ");

            $.ajax({
                url: baseUrl + '/TimeCard/TimeCard/SendTimeCard',
                type: "POST",
                beforeSend: function () {
                    showLoader();

                    $('.ajax-loader').css("visibility", "visible");
                    //SuccessPopup("Success! Your data has been sent!");
                    //$('#modalEmail').remove();
                    //  ("#mytimeCard").css('display', 'none');

                    //closePopup();
                },
                data: parseString,
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    // $("#btnClose").click();
                    SuccessPopup("Success! Your data has been sent!");
                    $('.modal-backdrop').hide();
                    $('#modalEmail').hide();
                    $(".dvloader").css("display", "none");
                    if ($(window).width() < 993) {
                        $("#modalTimeCard1").css("width", "100%");
                        $("#timeCard tbody").css("display", "flex");
                        $("#timeCard tbody td").css("display", "flex");
                        $("#timeCard").css("display", "flex");
                        $("tfoot tr").css("display", "flex");
                        $(".modal-dialog").css("max-width", "500px");
                        $(".hr-rate .row").css("flex-wrap", "wrap");
                        $(".modal-dialog").css("width", "auto");
                    }
                    if (data == true) {
                        $("#btnSendEmail").prop('disabled', false);
                        // hideLoader();
                    }


                },
                complete: function () {
                    $('.ajax-loader').css("visibility", "hidden");
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert("Status: " + textStatus);

                    console.log("Error: " + errorThrown);
                }

            });
        }
        else {
            $("#btnSendEmail").prop('disabled', false);
            AlertPopup(message);
        }
    }, 1000);
}

//function btnSendEmails() {

//    $("#btnSendEmails").prop('disabled', true);
//    var values = {};
//    var divName = "mytimeCard";


//    var printContents = document.getElementById(divName).innerHTML;
//    var to = $("#txtTo").val();
//    var subject = $("#txtSubject").val();
//    var description = $("#txtBody").val();
//    var user = $("#spnDriverName").text();
//    var body = printContents;
//    values.To = to;
//    values.Subject = subject;
//    values.Description = description;
//    values.User = user;
//    values.Body = body;
//    var isValid = true;
//    var message = "";
//    if (to == "") {
//        isValid = false;
//        message = "Please enter email."
//    }
//    else if (subject == "") {
//        isValid = false;
//        message = "Please enter subject."
//    }
//    else if (description == "") {
//        isValid = false;
//        message = "Please enter description."
//    }

//    if (!isEmail(to)) {
//        isValid = false;
//        message = "Please enter valid email."
//    }

//    console.log("sValid: "+isValid);
//    if (isValid) {

//        $.ajax({
//            url: baseUrl + '/TimeCard/TimeCard/SendTimeCard',
//            type: "POST",
//            beforeSend: function () {
//                //showLoader();
//              //  SuccessPopup("Success! Your data has been sent!");
//               // $('#modalEmail').remove();

//                //closePopup();
//            },
//            data: JSON.stringify(values),
//            contentType: "application/json; charset=utf-8",
//            dataType: 'json',
//            success: function (data) {
//                // $("#btnClose").click();
//                SuccessPopup("Success! Your data has been sent!");
//                $('#modalEmail').remove();
//                if (data == true) {
//                    console.log("Email Sent");
//                }
//                else {
//                    console.log("Email Sent Failed");
//                }
//                $("#btnSendEmails").prop('disabled', false);
//                hideLoader();
//            },

//        });
//    }
//    else {
//        $("#btnSendEmails").prop('disabled', false);
//        AlertPopup(message);
//    }

//}

function isEmail(email) {

    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}
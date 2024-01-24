$(function () {
    //$(document).tooltip({
    //    position: { my: "left top", at: "right top" },
    //    items: "input[required=required]",
    //    content: function () { return $(this).attr("title"); }
    //});
    
    $(".lblred").css("color", "#c21004");
});

//function to accept only numeric value
var onlyNumeric = function (event) {

    //// Allow only backspace and delete
    //var RegExp = new RegExp(/^\d*\.?\d*$/);
    //console.log("elem: " + event.charCode);
    if ((event.keyCode || event.charCode) == 46 ||
        (event.keyCode || event.charCode) == 8 ||
        (event.keyCode || event.charCode) == 9 ||
        (event.keyCode || event.charCode) == 37 ||
        (event.keyCode || event.charCode) == 38 ||
        (event.keyCode || event.charCode) == 39 ||
        (event.keyCode || event.charCode) == 40 && event.value.indexOf('.') === -1
    ) {
        // let it happen, don't do anything
    }
    else {

        // Ensure that it is a number and stop the keypress
        if ((event.keyCode || event.charCode) < 48 || (event.keyCode || event.charCode) > 57) {
            event.preventDefault();
        }
        return true;
    }


 

      

}

var onlyNumerics = function(evt, id) {
    try {
        var charCode = (evt.which) ? evt.which : event.keyCode;

        if (charCode == 46) {
            var txt = document.getElementById(id).value;
            if (!(txt.indexOf(".") > -1)) {

                return true;
            }
        }
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;

        return true;
    } catch (w) {
        alert(w);
    }
}

//function to accept only alphabets
var onlyAlphabets = function (event) {
    // Allow only backspace and delete
    if ((event.keyCode || event.charCode) == 46 ||
        (event.keyCode || event.charCode) == 8 ||
        (event.keyCode || event.charCode) == 9 ||
        (event.keyCode || event.charCode) == 32 ||
        (event.keyCode || event.charCode) == 37 ||
        (event.keyCode || event.charCode) == 38 ||
        (event.keyCode || event.charCode) == 39 ||
        (event.keyCode || event.charCode) == 40 ||
        (event.keyCode || event.charCode) == 188
    ) {
        // let it happen, don't do anything
    }
    else {
        // Ensure that it is a number and stop the keypress
        if (((event.keyCode || event.charCode) > 64 && (event.keyCode || event.charCode) < 91) ||
            ((event.keyCode || event.charCode) > 96 && (event.keyCode || event.charCode) < 123)) {

        }
        else {
            event.preventDefault();
        }
    }
}

//function to accept only alpha numeric characters
var onlyAlphanumeric = function (event) {
  
    // Allow only backspace and delete
    if ((event.keyCode || event.charCode) == 46 ||
        (event.keyCode || event.charCode) == 8 ||
        (event.keyCode || event.charCode) == 9 ||
        (event.keyCode || event.charCode) == 32 ||
        (event.keyCode || event.charCode) == 37 ||
        (event.keyCode || event.charCode) == 38 ||
        (event.keyCode || event.charCode) == 39 ||
        (event.keyCode || event.charCode) == 40) {
        // let it happen, don't do anything
    }
    else {
        var regex = new RegExp("^[ A-Za-z0-9_@.,/#&+)-]*$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    }
}



function validateContact() {
    var valid = true;
    $("form input[required=required], form textarea[required=required]").each(function () {
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

    $("form select[required=required]").each(function () {
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


var dimensionValidate = function () {
    $(".dimension").on("keyup", function () {
        validateDimensions();
    });
}

var validateDimensions = function () {
    var msg = "Enter a valid numeric number";
    var l = $("#LDimension");
    var w = $("#WDimension");
    var h = $("#HDimension");

    var lvalue = l.val();
    var wvalue = w.val();
    var hvalue = h.val();

    var onlyNumeric = new RegExp(/^-?\d+\.?\d*$/);

    if (!lvalue.match(onlyNumeric)) {
        $(l).addClass('invalid').attr('title', msg).focus();
        return false;
    }
    else {
        $(l).removeClass('invalid').removeAttr('title');
    }

    if (!wvalue.match(onlyNumeric)) {
        $(w).addClass('invalid').attr('title', msg).focus();
        return false;
    }
    else {
        $(w).removeClass('invalid').removeAttr('title');
    }

    if (!hvalue.match(onlyNumeric)) {
        $(h).addClass('invalid').attr('title', msg).focus();
        return false;
    }
    else {
        $(h).removeClass('invalid').removeAttr('title');
    }

    return true;
}

//#region Current date and Time
var Actualbinddate = function (_this) {
    
    var todaydate = new Date();

    var month = todaydate.getMonth() + 1;

    var dates = todaydate.getDate();

    var dates = (month < 10 ? ("0" + month) : month) + "-" + (dates < 10 ? ("0" + dates) : dates) + "-" + todaydate.getFullYear();

   // var time = todaydate.getHours() + ":" + todaydate.getMinutes();
    var time = (todaydate.getHours() < 10 ? ("0" + todaydate.getHours()) : todaydate.getHours()) + ":" + (todaydate.getMinutes() < 10 ? ("0" + todaydate.getMinutes()) : todaydate.getMinutes());

    var dateTime = dates + ' ' + time;

    $(_this).val(dateTime);
  

}
//#endregion

function onlyNumericOrcolon(event) {
    var regex = new RegExp("^[0-9-:]");
    var key = String.fromCharCode(event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
} 
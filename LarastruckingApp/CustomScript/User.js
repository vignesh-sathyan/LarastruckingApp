var isNeedToloaded = true;
$(document).ready(function () {
    $("#mytimeCard").hide();
    if ($("#Userid").val() > 0) {
        $("#btnSave").attr("value", "Update");
    }
    else {
        $("#btnSave").attr("value", "Save");
    }
    bindUsers;

    GetRollList();

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

//Go BACK... Added on 08-Feb-2023
$("html").unbind().keyup(function (e) {
    console.log("Which Key: " + $(e.target) + " : " + document.getElementsByClassName("jconfirm").length + " : " + window.location.href.toLowerCase().indexOf("Index"));
    if (!$(e.target).is('input')) {
        console.log(e.which);
        //event.preventDefault();
        if (e.key === 'Enter' || e.keyCode === 13) {
            var titleTxt = "";
            if (document.getElementsByClassName("jconfirm-title")[0] != undefined) {
                titleTxt = document.getElementsByClassName("jconfirm-title")[0].innerHTML;
            }
            console.log("ENTER: " + document.getElementsByClassName("jconfirm").length + " : " + titleTxt);
            if (document.getElementsByClassName("jconfirm").length == 0 && (document.getElementById("modalTimeCard").style.display == "" || document.getElementById("modalTimeCard").style.display == "none")) {
                $("#btnSave").click();
            } else if (document.getElementsByClassName("jconfirm").length >= 1 && titleTxt.toLowerCase() == "success!") {
                window.location.href = baseUrl + "User/UserRegistration";
            }
        }
    }
});
//

$("#btnSave").click(function () {
    if (validateContact()) {
        isNeedToloaded = false;
        $("form").submit(); // Submit the form
    }
});

//#region colour change on grid icon
$("#tblUserDetails").on("mouseover", 'tr', function () {
    $(this).find(".chng-color-edit").css('color', 'white');
    $(this).find(".chng-color-Trash").css('color', 'white');
});


$("#tblUserDetails").on("mouseout", 'tr', function () {

    $(this).find(".chng-color-edit").css('color', '#007bff');
    $(this).find(".chng-color-Trash").css('color', 'red');

});
//#endregion


$('#tblUserDetails').on('dblclick', 'tbody tr', function () {
    var table = $('#tblUserDetails').DataTable();
    var data_row = table.row($(this).closest('tr')).data();
    UserEdit(data_row.Userid);

});


var d = new Date();
var month = d.getMonth() + 1;
var day = d.getDate();

var datetime = (month < 10 ? '0' : '') + month + '/' +
    (day < 10 ? '0' : '') + day + '/' +
    d.getFullYear() + "  " +
    (d.getHours() < 10 ? '0' : '') + d.getHours() + ":" +
    (d.getMinutes() < 10 ? '0' : '') + d.getMinutes() + ":" +
    (d.getSeconds() < 10 ? '0' : '') + d.getSeconds();

var bindUsers = $('#tblUserDetails').DataTable({

    serverSide: true,
    dom: 'Blfrtip',
    "paging": true,
    buttons: [
        {
            extend: 'print',
            orientation: 'landscape',
            pageSize: 'LEGAL',
            title: "",
            text: '<img src="../../Assets/images/printer.png" style="height:18px;margin-right: 5px;position: relative;top: 1px;width:16px;"/> Print',
            messageBottom: datetime,

            exportOptions: {
                columns: ':visible',
                stripHtml: false,
                columns: [1, 2, 3, 4, 5, 6]
            },
            customize: function (win) {
                var last = null;
                var current = null;
                var bod = [];
                var css = '@page { size: landscape; }',
                    head = win.document.head || win.document.getElementsByTagName('head')[0],
                    style = win.document.createElement('style');

                style.type = 'text/css';
                style.media = 'print';

                if (style.styleSheet) {
                    style.styleSheet.cssText = css;
                }
                else {
                    style.appendChild(win.document.createTextNode(css));
                }

                head.appendChild(style);
                $(win.document.body)
                    .css('font-size', '10pt')
                    .prepend(
                        "<table style='text-transform:capitalize' id='checkheader'><tr><td width='80%' style='text-transform:capitalize;font-size:13px;'> </td><td width='20%'><div><img src='"+baseUrl+"/Images/Laraslogo.png' height='100px'/></div></td></tr></table>"
                    );
            }
        },
    ],
    select: 'single',
    responsive: true,
    processing: true,
    serverSide: true,
    searching: true,
    bDestroy: true,
    filter: true,
    stateSave: true,
    "language": {
        processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
    },
    "ajax": {
        "url": baseUrl + "/User/LoadData",
        "type": "POST",
        "datatype": "json",
        "async": false,
    },
    "columns": [
        { "data": "Userid", "name": "Userid", "autoWidth": true },
        { "data": "UserName", "name": "UserName", "autoWidth": true },
        {
            "name": "FirstName",
            //"orderable": false,
            "autoWidth": true,
            "render": function (data, type, row, meta) {
                return "<a href='#' class='chng-color-edit'  onclick='ShowTimeCard(" + row.Userid + ")'>" + row.FirstName + "</a>";
            }
        },

        { "data": "LastName", "name": "LastName", "autoWidth": true },
        { "data": "RoleName", "name": "RoleName", "autoWidth": true },
        { "data": "UserType", "name": "UserType", "autoWidth": true },
        {
            "data": "IsActive",
            "name": "IsActive",
            "autoWidth": true,
            "render": function (data, type, row, meta) {
                return (row.IsActive == true) ? "ACTIVE" : "INACTIVE";
            }
        },
        { "name": "Action", "autoWidth": true },
    ],
    "order": [[0, "desc"]],
    columnDefs: [

        {
            "targets": 7,
            "orderable": false,
            "render": function (data, type, row, meta) {
                var btnEdit = '<a href="javascript: void(0)" onclick="javascript:UserEdit(' + row.Userid + ');"><span class="fa fa-edit chng-color-edit"></span></a> ';
                var btnDelete = '<a href = "javascript: void(0)" class="delete_icon chng-color-Trash" onclick = "javascript:UserDelete(' + row.Userid + ');" > <i class="far fa-trash-alt"></i></a >';

                btnEdit = (isInsert == true) ? btnEdit : "";
                btnDelete = (isDelete == true) ? btnDelete : "";

                return '<div class="action-ic">' + btnEdit + btnDelete + '</div>'
            }
        },
        {
            "targets": 0,
            "visible": false,
        }
    ]
});

oTable = $('#tblUserDetails').DataTable();

$("input[type='search']").keyup(function () {
    oTable.search(this.value);
    oTable.draw();
});




//User Data Edit
function UserEdit(listId) {
    $.ajax({
        url: baseUrl + '/User/UserEdit',
        data: { 'id': listId },
        type: "POST",
        success: function (data) {
            $("#Userid").val(listId);
            $("#UserName").val(data.UserName);
            $("#Password").val(data.Password);
            $("#FirstName").val(data.FirstName);
            $("#LastName").val(data.LastName);
            $("#ddlRole").val(data.RoleID);
            $("#UserType").val(data.UserType);


            if (data.IsActive == true) {
                $("#IsActive").prop("checked", true);
            }
            else {
                $("#IsActive").prop("checked", false);
            }
            $("#btnSave").attr("value", "Update");

        }
    });
}

//User Data Delete
function UserDelete(listId) {

    $.confirm({
        title: 'Confirmation!',
        content: '<b>Are you sure you want to Delete?</b> ',
        type: 'red',
        typeAnimated: true,
        buttons: {
            confirm: {
                btnClass: 'btn-green',
                action: function () {
                    $.ajax({
                        url: baseUrl + '/User/Delete',
                        data: { 'id': listId },
                        type: "GET",
                        // cache: false,
                        success: function (data) {

                            if (data.IsSuccess == true) {
                                // toastr.success(data.Message, "")
                                SuccessPopup(data.Message)
                            }
                            else {
                                // toastr.error(data.Message, "")
                                AlertPopup(data.Message)
                            }
                            $('#tblUserDetails').DataTable().clear().destroy();
                            bindUsers();
                        }
                    });
                }
            },
            cancel: {
                btnClass: 'btn-red',
            }
        }
    })

}


function GetRollList() {

    $.ajax({
        url: baseUrl + '/User/DropDownRoleBind',
        type: 'GET',
        success: function (data) {
            $.each(data, function (key, value) {
                $("#ddlRole").append($("<option></option>").val(value.RoleID).html(value.RoleName));
            });
        }
    })
}

function GetList() {

    $.ajax({
        url: baseUrl + '/User/GetUserlist',
        type: 'GET',
        success: function (data) {
            $("#divUserList").empty();
            $("#divUserList").append(data);
        }
    })
}




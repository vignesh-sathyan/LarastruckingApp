//#region send mail
function SendMail(quoteid) {

    $.ajax({
        url: baseUrl + '/Quote/Quote/SendQuoteEmail',
        data: { 'quoteid': quoteid },
        type: "GET",
        beforeSend: function () {
            
            showLoader();
        },
        success: function (data) {

            if (data.IsSuccess == true) {
                //toastr.success(data.Message, "")
                SuccessPopup(data.Message)
                hideLoader();
            }
            else {
                //toastr.error(data.Message, "")
                AlertPopup(data.Message)
                hideLoader();
            }
        }
    });
}
//#endregion//#region 
function back(quoteid) {
    window.location.href = baseUrl + '/Quote/Quote/Index/' + quoteid;
}
//#endregion 
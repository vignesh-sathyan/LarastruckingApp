//#region READY FUNCTION
$(function () {
    fileViewer();
    printPdf(); printImage();
    $('.btnPrintImage').hide();
    $('.btnPrintPdf').hide();
});
//#endregion

//#region colour change on grid icon
$("#tblLeave").on("mouseover", 'tbody tr', function () {

    $(this).find(".chng-color-edit").css('color', 'white');
    $(this).find(".chng-color-Trash").css('color', 'white');
});


$("#tblLeave").on("mouseout", 'tbody tr', function () {

    $(this).find(".chng-color-edit").css('color', '#007bff');
    $(this).find(".chng-color-Trash").css('color', 'red');

});


//#region View Image/PDF
var fileViewer = function () {
    $(".fileViewer").on("click", function () {
        var fileUrl = $(this).attr("data-file-url");
        if (fileUrl != undefined) {
            var extn = fileUrl.substring(fileUrl.lastIndexOf('.') + 1);

            var isImg = isExtension(extn, _imgExts);
            var $divViewer = $("#divViewer");
            var docHeight = $(document).height();
            if (isImg) {
                $('.btnPrintImage').show();
                $('.btnPrintPdf').hide();
                var imgTag = '<img style="width: 100%; height:' + (docHeight - 120) + 'px" src="' + baseUrl + fileUrl + '" class="img-fluid" />';
                $divViewer.html(imgTag);
            }
            else {
                $('.btnPrintImage').hide();
                $('.btnPrintPdf').show();
              
                var iframe = '<iframe id="myiframe" src="' + baseUrl + fileUrl + '" style="width: 100%; height:' + (docHeight - 150) + 'px "></iframe>';
                $divViewer.html(iframe);
            }
        }
    });
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
//#endregion

//#region print pdf
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
//#endregion

//#region DEFINED BASE URL
var baseUrl = document.querySelector('meta[name="baseURL"]').content;

//var isDataProd = false;
//if (isDataProd) {
//    // Uncomment this when publishing in Azure
//     var baseUrl = "https://larastruckinglogistics-app.azurewebsites.net/";
//}
//else {
//    // Uncomment this when publishing in Data Prod
//  //var baseUrl = "http://dbprod/LarasTruckingLogistics-SCMS/";
//  var baseUrl = "http://localhost:5225/";
//}
//#endregion
//#region SHOW LOADER
var showLoader = function () {
    
    $(".dvloader").show();
}
//#endregion

//#region HIDE LOADER
var hideLoader = function () {
    $(".dvloader").hide();
}
//#endregion

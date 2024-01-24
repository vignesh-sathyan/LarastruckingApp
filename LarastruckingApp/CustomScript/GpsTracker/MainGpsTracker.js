
$(function () {
    GpsTrackerQuatrix();
});
function init() {
    //GpsTrackerQuatrix();
    //Get shipment Details
    GetFumigationDetails();
    GetDriverShipment();
    GetGpsTrakingMapDetails();
    //default radio button
    //checkDriverRadionButton();
    // get the Map Details 
    //GetFumigationGpsTrackerDetails();
    // Pervious Date calender 
    startEndDate();

    // GetDates();

}


function GetDriverShipment() {
    var EquipmentList = new Array();
    var count = 0;
    var eq;
    var colorbg;
    var fontcolor;
    $.ajax({
        type: 'GET',
        url: baseUrl + "/Shipment/Shipment/DriverDetail",
        //data: { "driverid": driverid },
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        success: function (msg) {
            // Do something interesting here.
            //console.log("DriverLsit: ", msg);
            for (let k = 0; k < msg.length; k++) {
                if (msg[k].DriverName != null) {
                    var Equipments = msg[k].Equipment.split("$");
                    //console.log("Equipment: ", msg[k].Equipment.split("$"));
                    for (let f = 0; f < Equipments.length; f++) {
                        EquipmentList.push(Equipments[f]);
                    }


                }
                else {
                    console.log("No Driver");
                }
            }

        },
        error: function (xhr, err) {
            console.log("error : " + err);
        }
    })

    var str = EquipmentList;
   // console.log("EquipmentList: ", str);
    return str;


}

var urlParams = new URLSearchParams(window.location.search);
var param1Value = urlParams.get('Equipment');
//console.log("param1Value: ", param1Value);
var metaValue = $('meta[name="baseUrl"]').attr('content');
//console.log("metavalue: ", metaValue);
baseUrl = metaValue;

var vehicleId;
var VehicleIcon;
var Description;
var Latitude;
var Longitude;
var vehicleDetails = [];
var currMarker;
var message;

function GpsTrackerQuatrix() {

    var vehicleLive;
    var vehicleList;
    vehicleDetails = [];
    var vehicleDetail;
    $.when(
        $.ajax({
            url: baseUrl + "/CustomScript/GpsTracker.aspx",
            success: function (data) {
                var pwdValues = data;
                let parser = new DOMParser();
                let doc = parser.parseFromString(pwdValues, 'text/html');
                if (data != null) {
                    // $("#txtShipRefNo").val(data.ShipmentRefNo);
                    pwdMaxDays = doc.getElementById("VehicleListLive").innerHTML;
                    //console.log("GpsData: ", data);
                    vehicleLive = $.parseJSON(pwdMaxDays);
                }
            },
            error: function () {
                console.log("error");
            }
        }),
        $.ajax({
            url: baseUrl + "/CustomScript/GpsTracker.aspx",
            success: function (data) {
                var pwdValues = data;
                let parser = new DOMParser();
                let doc = parser.parseFromString(pwdValues, 'text/html');
                if (data != null) {
                    // $("#txtShipRefNo").val(data.ShipmentRefNo);
                    pwdMaxDays = doc.getElementById("VehicleList").innerHTML;
                    //console.log("GpsData: ", pwdMaxDays);
                    vehicleList = $.parseJSON(pwdMaxDays);

                }
            },
            error: function () {
                console.log("error");
            }
        })
    ).then(function () {
        //console.log("VehicleListLive: ", vehicleLive);
        // console.log("VehicleList: ", vehicleList);
        //console.log("VehicleList: ", vehicleList.Data.length);
        for (let i = 0; i < vehicleList.Data.length; i++) {
            /*console.log("VehicleList: ", vehicleList.Data[i].VehicleIcon);
            vehicleDetails['vehicleId'] = vehicleList.Data[i].VehicleId;
            vehicleDetails['VehicleIcon'] = vehicleList.Data[i].VehicleIcon;
            vehicleDetails['Description'] = vehicleList.Data[i].Description;
            vehicleDetails['Latitude'] = vehicleLive.Data[i].Latitude;
            vehicleDetails['Longitude'] = vehicleLive.Data[i].Longitude;*/
            for (let j = 0; j < vehicleLive.Data.length; j++) {
                if (vehicleList.Data[i].VehicleId == vehicleLive.Data[j].VehicleId) {
                    vehicleDetails.push({
                        "VehicleId": vehicleList.Data[i].VehicleId,
                        "VehicleIcon": vehicleList.Data[i].VehicleIcon,
                        "Description": vehicleList.Data[i].Description,
                        "Latitude": vehicleLive.Data[i].Latitude,
                        "Longitude": vehicleLive.Data[i].Longitude,
                        "LocationText": vehicleLive.Data[i].LocationText
                    });
                }
            }
        }
        //console.log("vehicleDetails: ", vehicleDetails);
        init();
        // $('#result1').html(result1);
        // $('#result2').html(result2);
    });

    //return vehicleDetail;
}

//#region validate Dates

function validateStartEndDate() {
    let startDate = $("#dtStartedDate").val();
    let endDate = $("#dtEndDate").val();

    if (startDate != "" && endDate != "") {
        if (startDate <= endDate) {

        }
        else {
            $("#dtEndDate").val("");
            //toastr.warning("Please select valid date.");
            AlertPopup("Please select valid date.");

        }
    }

}
//#endregion

function GetDates() {
    // Date Time Format 
    var d = new Date($.now());
    startDate = (d.getDate() + "-" + (d.getMonth() + 1) + "-" + d.getFullYear() + " 00:00");
    endDate = (d.getDate() + "-" + (d.getMonth() + 1) + "-" + d.getFullYear() + " 12:00");
    //
    $("#dtStartedDate").val(startDate);
    $("#dtEndDate").val(endDate);
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


//#region get shipment details
GetFumigationDetails = function () {
    var FumigationId = $("#hdnFumigationId").val();

    if (FumigationId > 0) {
        $.ajax({
            url: baseUrl + "/GpsTracker/GpsTracker/GetFumigationDetails",
            type: "GET",
            data: { FumigationId: FumigationId },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {

                if (data != null) {
                    // $("#txtShipRefNo").val(data.ShipmentRefNo);
                    $("#txtAirwayBill").val(data.AirWayBill);
                    $("#dtStartedDate").val(ConvertDate(data.PickDateTime, true));
                    $("#dtEndDate").val(ConvertDate(data.DeliveryDateTime, true));


                    if (data.FumigationEquipmentNdriver != null) {

                        bindDrivertable(data);
                    }

                }
            },
            error: function () {

            }
        });
    }

}
//#endregion


//#region bind Freight Details
bindDrivertable = function (_data) {
    var dtDriverBody = "";
    $("#tblDriver tbody").empty();
    for (var i = 0; i < _data.FumigationEquipmentNdriver.length; i++) {
        let pickupLocation;
        let deliveryLocation;
        if (_data.FumigationEquipmentNdriver[i].IsPickUp == 1) {
            pickupLocation = _data.FumigationEquipmentNdriver[i].PickupLocation;
            deliveryLocation = _data.FumigationEquipmentNdriver[i].FumigationLocation;

        }
        else {
            pickupLocation = _data.FumigationEquipmentNdriver[i].FumigationLocation;
            deliveryLocation = _data.FumigationEquipmentNdriver[i].DeliveryLocation;
        }

        dtDriverBody += "<tr>" +
            "<td> <input type='radio' name='radioDriverId' id='radioDriverId' onchange='GetRadionButton(this)' value='" + _data.FumigationEquipmentNdriver[i].UserId + "' /> </td>" +
            "<td><label name='lblDriverName'>" + _data.FumigationEquipmentNdriver[i].DriverName + "</label> </td>" +
            "<td><label name='lblEquipmentName' id='lblEquipmentName'>" + _data.FumigationEquipmentNdriver[i].EquipmentName + "</label> </td>" +
            //"<td><label name='lblEquipmentName'>" + _data.FumigationEquipmentNdriver[i].AirWayBill + "</label> </td>" +

            "<td><label name='lblPickupLocation'>" + pickupLocation + "</label> </td>" +
            "<td><label name='lblDeliveryLocation'>" + deliveryLocation + "</label> </td>" +
            "</tr>";
    }
    $("#tblDriver tbody").append(dtDriverBody);

}
//#endregion

//#region Check Driver radio button 

function checkDriverRadionButton() {
    if ($("#tblDriver tbody tr").length > 0) {
        $($("#tblDriver tbody tr")[0]).find("input[type=radio]").prop("checked", true);

        message = $("#lblEquipmentName").text();
        //console.log("labeltext checked: " + message);
        GetFumigationGpsTrackerDetails();
    } else {
        alert("Fumigation details are not available as Driver or Equipment details are missing...")
    }
}
//#endregion

//#region recheck radion button 
function GetRadionButton(_this) {
    currMarker = "";
    var row = _this.parentNode.parentNode;
   // console.log("row: " + row);
    //Determine the Row Index.
    message = row.cells[2].textContent;

   // console.log("labeltext: " + message);
    GetFumigationGpsTrackerDetails();
    // GetGpsTrackerDetails(_this.value);
}
//#endregion



//#region Get Gps Tracker Details
GetFumigationGpsTrackerDetails = function () {
    var userId = $("input[name='radioDriverId']:checked").val();
    var startDate = $("#dtStartedDate").val();
    var endDate = $("#dtEndDate").val();
    var data = {};
    data.FumigationId = $("#hdnFumigationId").val();
    data.UserId = userId;
    data.StartDate = startDate;
    data.EndDate = endDate;

    if (data.FumigationId > 0) {
        $.ajax({
            url: baseUrl + "/GpsTracker/GpsTracker/GetGpsTrackerDetails",
            type: "GET",
            // data: data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {

                if (data != null) {

                    GetGpsTrakingMapDetails(data);

                }

            },
            error: function () {

            }
        });
    }

}
//#endregion

//#region Get Gps Tracker Details
GetTrackerDetails = function (get) {

    var userId = $("input[name='radioDriverId']:checked").val();
    var startDate = $("#dtStartedDate").val();
    var endDate = $("#dtEndDate").val();

    var data = {};
    data.DefaultDetails = get;
    data.FumigationId = $("#hdnFumigationId").val();
    data.UserId = userId;
    data.StartDate = startDate;
    data.EndDate = endDate;


    if (data.FumigationId > 0) {
        $.ajax({
            url: baseUrl + "/GpsTracker/GpsTracker/GetGpsTrackerDetails",
            type: "GET",
            data: data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {

                if (data != null) {
                    GetGpsTrakingMapDetails(data);

                }

            },
            error: function () {

            }
        });
    }

}
//#endregion

//#region Get GPS Tracking Details 
GetGpsTrakingMapDetails = function (shipmentLatLong) {
    //DART

    var shipLat = new Array();
    var currVehicleID = "";
    //var mapOptions=[];
    var myLatlng = [];
    var getEquipment = GetDriverShipment();
   // console.log("vehicleDetails: ", vehicleDetails);
   // console.log("currVehicleID: ", getEquipment);
    for (let j = 0; j < getEquipment.length; j++) {


        currVehicleID = getEquipment[j];
        //console.log("currVehicleID Loop : ", getEquipment[j]);
        for (let g = 0; g < vehicleDetails.length; g++) {
            //console.log("markers: ", parseInt(vehicleDetails[i].Description));
            if (parseInt(vehicleDetails[g].Description) == parseInt(currVehicleID)) {
                currMarker = vehicleDetails[g];
                shipLat.push(currMarker);

               // console.log("currMarker: ", currMarker);
                if (currMarker == null || currMarker == undefined) {
                    //alert("Current vehicle details not available in Quatrix...");
                    $("#ErrorMsgGPS").show();
                    $("#ErrorMsgGPS").text("Current vehicle details not available in Quatrix..");
                    $("#dvMap").hide();
                }
                //

               // console.log("shipLat: ", shipLat);
                shipmentLatLong = currMarker;
                // var markers = shipmentLatLong;
                var markers = shipLat;
               // console.log("markers: ", markers);
                //console.log("DATA: ", shipmentLatLong.Latitude);

                //if (shipmentLatLong.Latitude == undefined) {
                //alert("Current vehicle details not available in Quatrix...");

                //}


                if (shipmentLatLong != undefined) {
                    //console.log("DATA: ", markers[0].Latitude);
                    //window.onload = function () {

                    var mapOptions = {
                        center: new google.maps.LatLng(markers.Latitude, markers.longitude),
                        zoom: 15,
                        mapTypeId: google.maps.MapTypeId.ROADMAP
                    };

                    //console.log("DATA: ", markers)


                    var map = new google.maps.Map(document.getElementById("dvMap"), mapOptions);
                    var infoWindow = new google.maps.InfoWindow
                    var lat_lng = new Array();
                    var latlngbounds = new google.maps.LatLngBounds();

                    //for (i = 0; i < markers.length; i++) {
                    let icons = [];
                    var data = markers;
                    //console.log("DATA markers: ", data);
                    for (let c = 0; c < data.length; c++) {
                       // console.log("Loop Count: ", data[c].Latitude)
                        myLatlng.push({ lat: data[c].Latitude, lng: data[c].Longitude, description: data[c].Description, icon: data[c].VehicleIcon });

                    }
                      
                  //  console.log("DATA myLatlng : ", myLatlng);
                    lat_lng.push(myLatlng);
                    //console.log("data[0].VehicleIcon: ", data[0].VehicleIcon);


                    var marker;

                   // console.log("data.length: ", data.length);
                    for (let v = 0; v < data.length; v++) {
                      //  console.log("data.Description: ", data[v].Longitude);

                        marker = new google.maps.Marker({
                            position: { lat: data[v].Latitude, lng: data[v].Longitude },
                            map: map,
                            label: {
                                text: data[v].Description,
                                labelOrigin: new google.maps.Point(10, 50),
                                className: 'my-marker-label'
                            },
                            title: data[v].LocationText,
                            icon: {
                                url: data[v].VehicleIcon, scaledSize: new google.maps.Size(40, 24)
                            },

                        });
                       // console.log("marker: ", marker);
                        latlngbounds.extend(marker.position);

                        (function (marker, data) {
                            google.maps.event.addListener(marker, "click", function (e) {
                                //if (data.Event != null && data.Event != "" && data.Event != undefined) {
                                infoWindow.setContent(data.LocationText);
                                infoWindow.open(map, marker);
                                // }
                                // else {
                                //   infoWindow.setContent(ConvertDate(data.CreatedOn, true));
                                //}


                            });
                        })(marker, data);
                    }






                    // }
                    google.maps.event.addListener(map, 'zoom_changed', function () {
                        zoomChangeBoundsListener =
                            google.maps.event.addListener(map, 'bounds_changed', function (event) {
                                if (this.getZoom() > 19 && this.initialZoom == true) {
                                    // Change max/min zoom here
                                    this.setZoom(19);
                                    this.initialZoom = false;
                                }
                                google.maps.event.removeListener(zoomChangeBoundsListener);
                            });
                    });
                    map.initialZoom = true;
                    map.setCenter(latlngbounds.getCenter());
                    map.fitBounds(latlngbounds);

                    //***********ROUTING****************//

                    //Initialize the Path Array
                    var path = new google.maps.MVCArray();

                    //Initialize the Direction Service
                    var service = new google.maps.DirectionsService();

                    //Set the Path Stroke Color
                    var poly = new google.maps.Polyline({ map: map, strokeColor: '#4986E7' });


                   // console.log("lat_lng for check: ", lat_lng);
                    //console.log("lat_lngs for check: ", lat_lngs);
                   // console.log("lat_lng for check: ", lat_lng[0][1]);
                    //Loop and Draw Path Route between the Points on MAP
                    for (var i = 0; i < lat_lng.length; i++) {

                        if ((i) < lat_lng.length) {
                            var src = lat_lng[i];
                            var des = lat_lng[i + 1];
                            if (des == undefined) {
                                des = src;
                            }
                            path.push(src);
                            poly.setPath(path);
                            service.route({
                                origin: src,
                                destination: des,
                                travelMode: google.maps.DirectionsTravelMode.DRIVING
                            }, function (result, status) {
                                //console.log("result routes: ", result);
                                if (status == google.maps.DirectionsStatus.OK) {
                                    for (var i = 0, len = result.routes[0].overview_path.length; i < len; i++) {
                                        path.push(result.routes[0].overview_path[i]);
                                    }
                                }
                            });
                        }
                    }
                    // }
                }
                else {

                    var mapOptions = {
                        center: new google.maps.LatLng(41.850033, -87.6500523),
                        //zoom: 10,
                        mapTypeId: google.maps.MapTypeId.ROADMAP
                    };
                    var map = new google.maps.Map(document.getElementById("dvMap"), mapOptions);
                    var infoWindow = new google.maps.InfoWindow();
                    var lat_lng = new Array();
                    var latlngbounds = new google.maps.LatLngBounds();
                    //for (i = 0; i < markers.length; i++) {

                    //var data = markers[i]
                    //var myLatlng = new google.maps.LatLng(25.806199, -80.323813);
                    if (currMarker == null || currMarker == undefined) {
                        return;
                    }
                    var myLatlng = new google.maps.LatLng(currMarker.Latitude, currMarker.Longitude);
                    lat_lng.push(myLatlng);
                    let icons = {
                        url: currMarker.VehicleIcon,
                        scaledSize: { width: 40, height: 24 }
                    }
                    var marker = new google.maps.Marker({
                        position: myLatlng,
                        map: map,
                        title: currMarker.Description,
                        icon: icons
                    });
                    latlngbounds.extend(marker.position);
                    (function (marker, data) {
                        google.maps.event.addListener(marker, "click", function (e) {
                            infoWindow.setContent(currMarker.LocationText);
                            infoWindow.open(map, marker);
                        });
                    })(marker, 'No Data');
                    //}
                    google.maps.event.addListener(map, 'zoom_changed', function () {
                        zoomChangeBoundsListener =
                            google.maps.event.addListener(map, 'bounds_changed', function (event) {
                                if (this.getZoom() > 10 && this.initialZoom == true) {
                                    // Change max/min zoom here
                                    this.setZoom(10);
                                    this.initialZoom = false;
                                }
                                google.maps.event.removeListener(zoomChangeBoundsListener);
                            });
                    });
                    map.initialZoom = true;
                    map.setCenter(latlngbounds.getCenter());
                    map.fitBounds(latlngbounds);

                }
                //console.log("currMarker: " + currMarker);
            }
        }
    }

}


let vehicleData;
$(function () {
    fetch(baseUrl + "/Assets/vehiclesLiveData.json")
        .then((response) => response.json())
        .then((json) => {
            vehicleData = json.Data;
            vehicleData.sort(() => Math.random() - 0.5);
            console.log(vehicleData);
            init();
        });
});
function init() {
    getQuartixAuthTokenAPICall();
    getQuartixAuthToken();
    //Get shipment Details
    GetFumigationDetails();

    //GetGpsTrakingMapDetails();
    //default radio button
    checkDriverRadionButton();
    // get the Map Details 
    //GetFumigationGpsTrackerDetails();
    // Pervious Date calender 
    startEndDate();

    // GetDates();

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
    console.log("GetFumigationDetails: " + FumigationId);
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
            "<td><label name='lblEquipmentName'>" + _data.FumigationEquipmentNdriver[i].EquipmentName + "</label> </td>" +
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
        GetFumigationGpsTrackerDetails();
    }
}
//#endregion

//#region recheck radion button 
function GetRadionButton(_this) {
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
            url: baseUrl + "/GpsTracker/GpsTracker/GetFumigationGpsTrackerDetails",
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
            url: baseUrl + "/GpsTracker/GpsTracker/GetFumigationGpsTrackerDetails",
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
    
    var markers = shipmentLatLong;
    console.log("GPS: ");
    console.log(shipmentLatLong);
    if (shipmentLatLong != undefined && shipmentLatLong.length > 0) {

        //window.onload = function () {
        var mapOptions = {
            center: new google.maps.LatLng(markers[0].Latitude, markers[0].longitude),
            zoom: 10,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };


        var map = new google.maps.Map(document.getElementById("dvMap"), mapOptions);
        var infoWindow = new google.maps.InfoWindow
        var lat_lng = new Array();
        var latlngbounds = new google.maps.LatLngBounds();
        for (i = 0; i < markers.length; i++) {

            var data = markers[i]
            var myLatlng = new google.maps.LatLng(data.Latitude, data.longitude);
            lat_lng.push(myLatlng);
            var marker = new google.maps.Marker({
                position: myLatlng,
                map: map,
                title: data.Event
            });
            latlngbounds.extend(marker.position);
            (function (marker, data) {
                google.maps.event.addListener(marker, "click", function (e) {
                    if (data.Event != null && data.Event != "" && data.Event != undefined) {
                        infoWindow.setContent(data.Event + " <br> " + ConvertDate(data.CreatedOn, true));
                    }
                    else {
                        infoWindow.setContent(ConvertDate(data.CreatedOn, true));
                    }
                  
                    infoWindow.open(map, marker);
                });
            })(marker, data);
        }
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
            //center: new google.maps.LatLng(41.850033, -87.6500523),
            center: new google.maps.LatLng(25.81428, -80.30787),
            //zoom: 10,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        var map = new google.maps.Map(document.getElementById("dvMap"), mapOptions);
        var infoWindow = new google.maps.InfoWindow();
        var lat_lng = new Array();
        var latlngbounds = new google.maps.LatLngBounds();
        //for (i = 0; i < markers.length; i++) {

        //var data = markers[i]
        
        console.log("vehicleLiveData: " + vehicleData);
        var currVehicle = vehicleData[0];
        console.log("currVehicle: " + currVehicle.VehicleId + " : " + currVehicle.Latitude + " : " + currVehicle.Longitude);
        //var myLatlng = new google.maps.LatLng(25.806199, -80.323813);
        var myLatlng = new google.maps.LatLng(currVehicle.Latitude, currVehicle.Longitude);
        lat_lng.push(myLatlng);
        var marker = new google.maps.Marker({
            position: myLatlng,
            map: map,
            title: "" + currVehicle.VehicleId //'No Data'
        });
        latlngbounds.extend(marker.position);
        (function (marker, data) {
            google.maps.event.addListener(marker, "click", function (e) {
                infoWindow.setContent(currVehicle.LocationText);
                infoWindow.open(map, marker);
            });
        })(marker, currVehicle.VehicleId);
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
}

//DART: QUARTIX API Integration
getQuartixAuthTokenAPICall = function () {
    $.ajax({
        url: baseUrl + "/GpsTracker/GpsTracker/GetQuartixGpsTrackerTokenDetails",
        type: "GET",
        success: function (data) {
            console.log("LOCAL API CALLED SUCCESSFULLY...");
            if (data != null) {

            }

        },
        error: function () {
            console.log("LOCAL API CALL FAILED...");
        }
    });
}
getQuartixAuthToken = function () {
    var customerData = {};
    customerData.CustomerID = "Lara TC";
    customerData.UserName = "QWS";
    customerData.Password = "LaraTC2022";
    customerData.ApplicationName = "Lara TC.app";

    var apiURL = "http://qws.quartix.com/v2/api/auth";

    /*fetch(apiURL, {
        method: 'post', // Default is 'get'
        body: JSON.stringify(customerData),
        mode: 'no-cors'
    }).then(response => response.json()).then(json => console.log('Response11', json));*/

   $.ajax({
        url: "https://qws.quartix.com/v2/api/auth",
        type: "POST",
        data: JSON.stringify(customerData),
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        dataType: "text",
        xhrFields: {
           withCredentials: true
        },
        "headers": {
            "accept": "application/json",
            "Access-Control-Allow-Origin": "*"
        },
        //async: false,
        success: function (data) {
            console.log("SUCCESS: " + data.AccessToken);
            if (data != null) {
                //GetGpsTrakingMapDetails(data);

            }

        },
        error: function () {
            console.log("API Auth Failed...");
        }
    });

    /*var request = new XMLHttpRequest();
    var params = "CustomerID=Lara TC&UserName=QWS&Password=LaraTC2022&ApplicationName=Lara TC.app";
    request.open('POST', "https://qws.quartix.com/v2/api/auth", true);
    request.onreadystatechange = function () {
        if (request.readyState == 4) {
            console.log("SUCCESS: " + request.Data);
        } else {
            alert("HTTP Reqquest FAILED...");
        }
    };
    //request.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    //request.setRequestHeader("Content-length", params.length);
    //request.setRequestHeader("Connection", "close");
    request.send(params);*/
}
//
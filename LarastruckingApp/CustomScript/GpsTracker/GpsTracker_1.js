
$(document).ready(function () {

    //Get shipment Details
    GetShipmentDetails();

    // GetGpsTrakingMapDetails();
    //default radio button
    checkDriverRadionButton();
    // get the Map Details 
    // GetGpsTrackerDetails();
    // Pervious Date calender 
    startEndDate();

    // GetDates();
});

//#region get shipment details
GetShipmentDetails = function () {
    var shipmentId = $("#hdnShipmentId").val();

    if (shipmentId > 0) {
        $.ajax({
            url: baseUrl + "/GpsTracker/GpsTracker/GetShipmentDetails",
            type: "GET",
            data: { shipmentId: shipmentId },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {

                if (data != null) {
                    // $("#txtShipRefNo").val(data.ShipmentRefNo);
                    $("#txtAirwayBill").val(data.AirWayBill);
                    $("#dtStartedDate").val(ConvertDate(data.PickDateTime, true));
                    $("#dtEndDate").val(ConvertDate(data.DeliveryDateTime, true));

                    if (data.ShipmentEquipmentNdriver != null) {

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
//#region validate Dates

function validateStartEndDate() {
    let startDate = $("#dtStartedDate").val();
    let endDate = $("#dtEndDate").val();

    if (startDate != "" && endDate != "") {
        if (startDate <= endDate) {

        }
        else {
            $("#dtEndDate").val("");
            // toastr.warning("Please select valid date.");
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





//#region bind Freight Details
bindDrivertable = function (_data) {
    var dtDriverBody = "";
    $("#tblDriver tbody").empty();
    for (var i = 0; i < _data.ShipmentEquipmentNdriver.length; i++) {
        dtDriverBody += "<tr>" +
            "<td> <input type='radio' name='radioDriverId' id='radioDriverId' onchange='GetRadionButton()' value='" + _data.ShipmentEquipmentNdriver[i].UserId + "' /> </td>" +
            "<td><label name='lblDriverName'>" + _data.ShipmentEquipmentNdriver[i].DriverName + "</label> </td>" +
            "<td><label name='lblEquipmentName'>" + _data.ShipmentEquipmentNdriver[i].EquipmentName + "</label> </td>" +
            "</tr>";
    }
    $("#tblDriver tbody").append(dtDriverBody);

}
//#endregion

//#region Check Driver radio button 

function checkDriverRadionButton() {
    if ($("#tblDriver tbody tr").length > 0) {
        $($("#tblDriver tbody tr")[0]).find("input[type=radio]").prop("checked", true);
        GetGpsTrackerDetails();
    }
}
//#endregion

//#region recheck radion button 
function GetRadionButton() {
    GetGpsTrackerDetails();
    // GetGpsTrackerDetails(_this.value);
}
//#endregion



//#region Get Gps Tracker Details
GetGpsTrackerDetails = function () {
    var userId = $("input[name='radioDriverId']:checked").val();
    var startDate = $("#dtStartedDate").val();
    var endDate = $("#dtEndDate").val();
    var data = {};
    data.ShipmentId = $("#hdnShipmentId").val();
    data.UserId = userId;
    data.StartDate = startDate;
    data.EndDate = endDate;
    if (data.ShipmentId > 0) {
        $.ajax({
            url: baseUrl + "/GpsTracker/GpsTracker/GetGpsTrackerDetails",
            type: "GET",
            data: data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                
                if (data != null) {
                    //for (i = 0; i < data.length; i++)
                    //{
                    //    $("#dtStartedDate").val(ConvertDate(data[0].PickDateTime, true));
                    //    $("#dtEndDate").val(ConvertDate(data[0].DeliveryDateTime, true));
                    //}

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
    data.ShipmentId = $("#hdnShipmentId").val();
    data.UserId = userId;
    data.StartDate = startDate;
    data.EndDate = endDate;


    if (data.ShipmentId > 0) {
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
    
    var markers = shipmentLatLong;
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
        var number = 0;
        for (i = 0; i < markers.length; i++) {
            number = number+1;
            var data = markers[i]

            var myLatlng = new google.maps.LatLng(data.Latitude, data.longitude);
            lat_lng.push(myLatlng);
            //if (data.Event != null && data.Event != "" && data.Event != undefined) {
                var marker = new google.maps.Marker({
                    position: myLatlng,
                    map: map,
                    //icon: 'https://chart.googleapis.com/chart?chst=d_map_pin_letter&chld=' + number + '|FF776B|000000',
                    title: data.Event
                });
                latlngbounds.extend(marker.position);
                (function (marker, data) {
                    google.maps.event.addListener(marker, "click", function (e) {
                        if (data.Event != null && data.Event != "" && data.Event != undefined) {
                            infoWindow.setContent(data.Event + " <br> " + ConvertDate(data.CreatedOn, true));
                        } else {
                            infoWindow.setContent(ConvertDate(data.CreatedOn, true));
                        }
                       
                        infoWindow.open(map, marker);
                    });
                })(marker, data);
            //}
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
        var directionsService = new google.maps.DirectionsService();

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
        var myLatlng = new google.maps.LatLng(25.806199, -80.323813);
        lat_lng.push(myLatlng);
        var marker = new google.maps.Marker({
            position: myLatlng,
            map: map,
            title: 'No Data'
        });
        latlngbounds.extend(marker.position);
        (function (marker, data) {
            google.maps.event.addListener(marker, "click", function (e) {
                infoWindow.setContent('No Data');
                infoWindow.open(map, marker);
            });
        })(marker, 'No Data');
        //}
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

    }
}


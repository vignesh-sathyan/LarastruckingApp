using System.Web;
using System.Web.Optimization;

namespace LarastruckingApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                        "~/Assets/js/jquery.min.js",
                        "~/Assets/js/popper.min.js",
                        "~/Assets/js/bootstrap.min.js",
                        // "~/Assets/js/selectize/standalone/selectize.min.js",
                        "~/Assets/js/selectize/selectize.min.js",
                        "~/Assets/js/jquery.dataTables.min.js",
                        "~/Assets/js/dataTables.bootstrap4.min.js",
                        "~/Assets/js/dataTables.responsive.min.js",
                        "~/Assets/js/responsive.bootstrap4.min.js",
                        "~/Assets/js/custom.js",
                        "~/CustomScript/baseUrlJs.js",
                        "~/CustomScript/globalJs.js",
                        "~/CustomScript/Validation.js",
                        "~/CustomScript/messages.js"
                        ));

            bundles.Add(new StyleBundle("~/bundles/styles").Include(
                      "~/Assets/css/bootstrap.min.css",
                      "~/Assets/js/selectize/selectize.css",
                      "~/Assets/css/base/base.css",
                      "~/Assets/css/dataTables.bootstrap4.min.css",
                      "~/Assets/css/responsive.bootstrap4.min.css",
                      "~/Assets/css/style.css",
                      "~/Assets/css/responsive.css"

                      ));

            bundles.Add(new ScriptBundle("~/bundles/viewShipment").Include(
               "~/CustomScript/shipmentjs/viewShipmentList.js"
                              ));
            bundles.Add(new ScriptBundle("~/bundles/MainShipment").Include(
              "~/CustomScript/shipmentjs/MainShipment.js"
                             ));
            bundles.Add(new ScriptBundle("~/bundles/viewAllShipment").Include(
             "~/CustomScript/shipmentjs/viewAllShipment.js"
                            ));
            bundles.Add(new ScriptBundle("~/bundles/shipment").Include(
                 "~/CustomScript/shipmentjs/shipment.js",
                 "~/CustomScript/jsonReturn/addAddressJson.js"
                 ));
            bundles.Add(new ScriptBundle("~/bundles/gpsTracker").Include(
           "~/CustomScript/GpsTracker/GpsTracker.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/quote").Include(
        "~/CustomScript/quotesJs/managingQuotesJs.js",
        "~/CustomScript/jsonReturn/addAddressJson.js"
          ));

            bundles.Add(new ScriptBundle("~/bundles/viewQuote").Include(
        "~/CustomScript/quotesJs/viewQuote.js"
          ));

            bundles.Add(new ScriptBundle("~/bundles/fumigation").Include(
               "~/CustomScript/fumigationjs/fumigation.js",
               "~/CustomScript/jsonReturn/addAddressJson.js"
                             ));
            bundles.Add(new ScriptBundle("~/bundles/getFumigationList").Include(
             "~/CustomScript/fumigationjs/GetFumigationList.js"
                            ));
            bundles.Add(new ScriptBundle("~/bundles/MainFumigation").Include(
            "~/CustomScript/fumigationjs/MainPageFumigation.js"
                           ));
            bundles.Add(new ScriptBundle("~/bundles/viewAllFumigation").Include(
             "~/CustomScript/fumigationjs/ViewAllFumigation.js"
                            ));
            bundles.Add(new ScriptBundle("~/bundles/trailerRental").Include(
             "~/CustomScript/TrailerRental/TrailerRental.js",
              "~/CustomScript/jsonReturn/addAddressJson.js"
                            ));
            bundles.Add(new ScriptBundle("~/bundles/ViewTrailerRentalList").Include(
              "~/CustomScript/TrailerRental/ViewTrailerRentalList.js"
                            ));

            bundles.Add(new ScriptBundle("~/bundles/viewReportAccident").Include(
           "~/CustomScript/ViewReportAccident.js"
                           ));
            bundles.Add(new ScriptBundle("~/bundles/accidentDocViewer").Include(
            "~/CustomScript/accidentDocViewer.js"
                           ));
            bundles.Add(new ScriptBundle("~/bundles/accidentReport").Include(
           "~/CustomScript/Validation.js",
           "~/CustomScript/AccidentReport.js"
                           ));
            bundles.Add(new ScriptBundle("~/bundles/viewEquipment").Include(
         "~/CustomScript/ViewEquipment.js"
                          ));

            bundles.Add(new ScriptBundle("~/bundles/equipment").Include(
         "~/CustomScript/Equipment.js"
                          ));

            bundles.Add(new ScriptBundle("~/bundles/user").Include(
       "~/CustomScript/User.js",
       "~/CustomScript/TimeCard/MainTimeCard.js"
                        ));


            bundles.Add(new ScriptBundle("~/bundles/manageCustomerJs").Include(
      "~/CustomScript/customerJs/manageCustomerJs.js"
                        ));


            bundles.Add(new ScriptBundle("~/bundles/ViewCustomer").Include(
       "~/CustomScript/ViewCustomer.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/viewDriver").Include(
                    "~/CustomScript/ViewDriver.js",
                     "~/CustomScript/TimeCard/MainTimeCard.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/Incentive").Include(
                     "~/CustomScript/TimeCard/IncentiveCard.js",
                     "~/CustomScript/TimeCard/DriverIncentive.js"
                     ));
            bundles.Add(new ScriptBundle("~/bundles/Driver").Include(
                      "~/CustomScript/Driver.js"
                  ));

            bundles.Add(new ScriptBundle("~/bundles/Address").Include(
                     "~/CustomScript/Address.js"
                  ));
            bundles.Add(new ScriptBundle("~/bundles/uploadShipment").Include(
                   "~/CustomScript/shipmentjs/uploadShipment.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/manageLeave").Include(
       "~/CustomScript/leave/manageLeave.js"
                      ));


            bundles.Add(new ScriptBundle("~/bundles/driverDashboard").Include(
      "~/CustomScript/driverModule/driverDashboard.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/driverShipmentDetail").Include(
            "~/CustomScript/driverModule/preTripShipmentDetail.js"
                          ));


            bundles.Add(new ScriptBundle("~/bundles/driverFumigationDetail").Include(
            "~/CustomScript/driverModule/DriverFumigationDetails.js"
                          ));


            bundles.Add(new ScriptBundle("~/bundles/viewTimeCard").Include(
                "~/CustomScript/TimeCard/ViewTimeCard.js"
                         ));

            bundles.Add(new ScriptBundle("~/bundles/CustomerDashboard").Include(
              "~/CustomScript/customerJs/CustomerDashboard.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/OldShipmentDaashboard").Include(
            "~/CustomScript/customerJs/OldShipmentDaashboard.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/CustomerShipmentDetails").Include(
            "~/CustomScript/customerJs/CustomerShipmentDetails.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/CustomerFumigationDetails").Include(
            "~/CustomScript/customerJs/CustomerFumigationDetails.js"
                      ));
            //the following creates bundles in debug mode;
            //BundleTable.EnableOptimizations = true;
        }
    }
}

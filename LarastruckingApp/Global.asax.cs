using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Driver;
using LarastruckingApp.Infrastructure;
using LarastruckingApp.Log.Utility;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using LarastruckingApp.Repository.Repository;
using LarastruckingApp.Repository.Repository.DriverModule;
using LarastruckingApp.Resource;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;



namespace LarastruckingApp
{

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            System.Timers.Timer timScheduledTask = null;
            try
            {
                AreaRegistration.RegisterAllAreas();
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
                BundleTable.EnableOptimizations = true;

                AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;

                CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                newCulture.DateTimeFormat.ShortDatePattern = "MM-dd-yyyy";
                newCulture.DateTimeFormat.DateSeparator = "-";
                Thread.CurrentThread.CurrentCulture = newCulture;

                //#Timer

                // Dynamically create new timer
                timScheduledTask = new System.Timers.Timer();
                // Timer interval is set in miliseconds,
                // In this case, we'll run a task every minute 
                // timScheduledTask.Interval = 30000;
                timScheduledTask.Interval = 120000;
                timScheduledTask.Enabled = true;
                // Add handler for Elapsed event
                timScheduledTask.Elapsed +=
                new System.Timers.ElapsedEventHandler(timScheduledTask_Elapsed);
                timScheduledTask.Stop();
                timScheduledTask.Start();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                //timScheduledTask.Dispose();
            }
            //
        }

        void timScheduledTask_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //For shipment Waiting Time
           waitingTimeSendMail();

            //For Fumigation Waiting Time
            //waitingFumigationTimeSendMail();

            //For Driver Expiration Docs
            DriverDocsExpireSendMail();

             DeleteRecord();

        }

        #region Waiting time sending Mail
        /// <summary>
        /// Waiting time sending Mail
        /// </summary>

        public void waitingTimeSendMail()
        {

            GlobalWaitingTimeRepository GWTR = null;
            ExecuteSQLStoredProceduce Ex = null;
            StreamReader reader = null;
            try
            {
                Ex = new ExecuteSQLStoredProceduce();
                IShipmentRepository iShipmentRepository = new ShipmentRepository();
                IFumigationRepository iFumigationRepository = new FumigationRepository();
                GWTR = new GlobalWaitingTimeRepository(Ex, iShipmentRepository, iFumigationRepository);
                List<GetWaitingNotificationDetailsDto> getWaitingNotificationDetailsDtosLst = new List<GetWaitingNotificationDetailsDto>();
                getWaitingNotificationDetailsDtosLst = GWTR.GetWaitingNotificationDetails();

                if (getWaitingNotificationDetailsDtosLst.Count > 0)
                {
                    if (getWaitingNotificationDetailsDtosLst != null)
                    {
                        foreach (var item in getWaitingNotificationDetailsDtosLst)
                        {
                            string to = item.ShipmentEmailDTO.AllContactPerson;
                            string subject = string.Empty;
                            bool IsValid = true;
                            subject = LarastruckingResource.WaitingTimeStatus;

                            if (item.IsPickUpWaitingTimeRequired && item.IsEmailSentPWS == true && item.IsEmailSentPWE == false)
                            {
                                if (item.PickupDepartedOn != DateTime.MinValue)
                                {

                                    subject = LarastruckingResource.WaitingTimeTotal;
                                }

                            }

                            if (item.IsDeliveryWaitingTimeRequired && item.IsEmailSentDWS == true && item.IsEmailSentDWE == false)
                            {
                                if (item.DeliveryDepartedOn != DateTime.MinValue)
                                {
                                    subject = LarastruckingResource.WaitingTimeTotal;
                                }

                            }

                            if (!string.IsNullOrEmpty(item.ShipmentEmailDTO.AirWayBill))
                            {
                                subject = subject.Replace("@AWB", item.ShipmentEmailDTO.AirWayBill);
                                item.ShipmentEmailDTO.AWBPoOrderNO = " AWB# : " + item.ShipmentEmailDTO.AirWayBill;
                                IsValid = false;
                            }

                            if (IsValid && !string.IsNullOrEmpty(item.ShipmentEmailDTO.CustomerPO))
                            {
                                subject = subject.Replace("@AWB", item.ShipmentEmailDTO.CustomerPO);
                                item.ShipmentEmailDTO.AWBPoOrderNO = " PO# : " + item.ShipmentEmailDTO.CustomerPO;
                                IsValid = false;
                            }
                            if (IsValid && !string.IsNullOrEmpty(item.ShipmentEmailDTO.OrderNo))
                            {
                                subject = subject.Replace("@AWB", item.ShipmentEmailDTO.OrderNo);
                                item.ShipmentEmailDTO.AWBPoOrderNO = " ORDER# : " + item.ShipmentEmailDTO.OrderNo;

                            }
                            if (IsValid)
                            {
                                subject = subject.Replace("@AWB", "");
                            }



                            subject = subject.Replace("@STATUS", item.ShipmentEmailDTO.Status);

                            string bodywithsignature = LarastruckingResource.QuoteMailSignature;
                            string url = HostingEnvironment.MapPath("~/Views/Shared/_watingTimeEmailForCustomer.html");
                            reader = new StreamReader(url);
                            string readFile = reader.ReadToEnd();
                            string mailBody = string.Empty;
                            string myString = "<tr>";
                            myString = readFile;
                            myString = myString.Replace("$$LogoURL$$", item.ShipmentEmailDTO.LogoURL);
                            myString = myString.Replace("$$AWBPoOrderNO$$", item.ShipmentEmailDTO.AWBPoOrderNO);
                            myString = myString.Replace("$$OrderTaken$$", Convert.ToDateTime(item.ShipmentEmailDTO.OrderTaken).ToString("dddd | MMMM dd, yyyy"));
                            myString = myString.Replace("$$ESTDateTime$$", Convert.ToDateTime(item.ShipmentEmailDTO.ESTDateTime).ToString("dddd | MMMM dd, yyyy"));

                            myString = myString.Replace("$$CustomerName$$", item.ShipmentEmailDTO.CustomerName);
                            myString = myString.Replace("$$Consignee$$", item.ShipmentEmailDTO.Consignee);

                            string ShipmentStatusHistory = "";
                            bool isValid = false;
                            foreach (var status in item.ShipmentEmailDTO.ShipmentStatusHistory)
                            {
                                if (isValid == false)
                                {
                                    ShipmentStatusHistory += "<img style = 'width:auto;' src = " + status.ImageUrl + " />";
                                    isValid = true;
                                }
                                else
                                {

                                    ShipmentStatusHistory += "<img style = 'margin-bottom:0px;' src = " + item.ShipmentEmailDTO.StatusDotPath + " />";
                                    ShipmentStatusHistory += "<img style = 'width:auto;' src = " + status.ImageUrl + " />";
                                }


                            }
                            foreach (var status in item.ShipmentEmailDTO.ShipmentStatusList)
                            {
                                ShipmentStatusHistory += "<img style = 'margin-bottom:0px;' src = " + item.ShipmentEmailDTO.StatusGrayDotPath + " />";
                                ShipmentStatusHistory += "<img style = 'width:auto;' src = " + status.GrayImageURL + " />";

                            }
                            myString = myString.Replace("$$ShipmentStatusHistory$$", ShipmentStatusHistory);

                            string pickupAddress = item.PickupAddress;
                            string[] pickupAddressList = pickupAddress.Split(',');
                            string deliveryAddress = item.DeliveryAddress;
                            string[] deliveryAddressList = deliveryAddress.Split(',');



                            string waitingTimeBody = "";

                            if (item.IsPickUpWaitingTimeRequired == true && item.PickupArrivedOn <= DateTime.Now)
                            {
                                if (item.PickupArrivedOn != DateTime.MinValue)
                                {

                                    waitingTimeBody = "<tr>" +
                                                       "<td style='width:25%;text-align:left;'>" + item.PickupArrivedOn.ToString("MM/dd/yyyy HH:mm") + " HRS</td>" +
                                                       "<td style='width:25%;text-align:left;'>WAITING TIME STARTED | " + pickupAddressList[0] + "</td>" +
                                                       "</tr>";
                                }
                                if (item.PickupDepartedOn != DateTime.MinValue)
                                {

                                    waitingTimeBody += "<tr>" +
                                                       "<td style='width:25%;text-align:left;'>" + item.PickupDepartedOn.ToString("MM/dd/yyyy HH:mm") + " HRS</td>" +
                                                       "<td style='width:25%;text-align:left;'>WAITING TIME ENDED | " + pickupAddressList[0] + "</td>" +
                                                       "</tr>" +
                                                       "<tr>" +
                                                       "<td style='width:25%;text-align:left;'>TOTAL WAITING TIME</td>" +
                                                       "<td style='width:25%;text-align:left;'></td>" +
                                                       "</tr>" +
                                                       "<tr>" +
                                                       "<td style='width:25%;text-align:left;'>" + item.PickUpDateDifference + "</td>" +
                                                       "<td style='width:25%;text-align:left;'></td>" +
                                                       "</tr>";

                                }
                            }
                            if (item.IsDeliveryWaitingTimeRequired == true && item.DeliveryArrivedOn <= DateTime.Now)
                            {
                                if (item.DeliveryArrivedOn != DateTime.MinValue)
                                {

                                    waitingTimeBody = "<tr>" +
                                                       "<td style='width:25%;text-align:left;'>" + item.DeliveryArrivedOn.ToString("MM/dd/yyyy HH:mm") + " HRS</td>" +
                                                       "<td style='width:25%;text-align:left;'>WAITING TIME STARTED | " + deliveryAddressList[0] + "</td>" +
                                                       "</tr>";

                                }
                                if (item.DeliveryDepartedOn != DateTime.MinValue)
                                {

                                    waitingTimeBody += "<tr>" +
                                                       "<td style='width:25%;text-align:left;'>" + item.DeliveryDepartedOn.ToString("MM/dd/yyyy HH:mm") + " HRS</td>" +
                                                       "<td style='width:25%;text-align:left;'>WAITING TIME ENDED | " + deliveryAddressList[0] + "</td>" +
                                                       "</tr>" +
                                                       "<tr>" +
                                                       "<td style='width:25%;text-align:left;'>TOTAL WAITING TIME</td>" +
                                                       "<td style='width:25%;text-align:left;'></td>" +
                                                       "</tr>" +
                                                       "<tr>" +
                                                       "<td style='width:25%;text-align:left;'>" + item.DeliveryDateDifference + "</td>" +
                                                       "<td style='width:25%;text-align:left;'></td>" +
                                                       "</tr>";

                                }

                            }

                            myString = myString.Replace("$$WaitingTimeHistory$$", waitingTimeBody);


                            var routeDetail = item.ShipmentEmailDTO.ShipmentFreightDetail.Where(x => x.ShipmentRouteId == item.ShipmentRouteId).FirstOrDefault();

                            myString = myString.Replace("$$PickupLocation$$", routeDetail.PickupLocation);
                            myString = myString.Replace("$$DeliveryLocation$$", routeDetail.DeliveryLocation);
                            myString = myString.Replace("$$AWBPoOrderNO$$", item.ShipmentEmailDTO.AWBPoOrderNO);
                            myString = myString.Replace("$$FreightType$$", routeDetail.FreightType);
                            myString = myString.Replace("$$Commodity$$", routeDetail.Commodity);
                            myString = myString.Replace("$$Pallet$$", (routeDetail.QutWgtVlm).ToString());
                            myString = myString.Replace("$$NoOfBox$$", (routeDetail.NoOfBox).ToString());
                            myString = myString.Replace("$$Weight$$", (routeDetail.WeightWithUnit).ToString());
                            myString = myString + bodywithsignature;

                            mailBody = myString.ToString();

                            if (!string.IsNullOrEmpty(mailBody))
                            {

                                Email.AsyncSendEmail(to, subject, mailBody);
                                GWTR.UpdateWaitingTime(item);//item.WatingNotificationId, item.PickupArrivedOn, item.PickupDepartedOn, item.DeliveryArrivedOn, item.DeliveryDepartedOn);
                            }

                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                // reader.Dispose();
                GWTR.Dispose();
                Ex.Dispose();
            }
        }
        #endregion

        #region Waiting time Fumigation sending Mail
        //<summary>
        //Waiting time Fumigation sending Mail
        //</summary>

        public void waitingFumigationTimeSendMail()
        {
            ExecuteSQLStoredProceduce Ex = null;
            GlobalWaitingTimeRepository GWTR = null;
            StreamReader reader = null;
            try
            {


                Ex = new ExecuteSQLStoredProceduce();
                IShipmentRepository iShipmentRepository = new ShipmentRepository();
                IFumigationRepository iFumigationRepository = new FumigationRepository();
                GWTR = new GlobalWaitingTimeRepository(Ex, iShipmentRepository, iFumigationRepository);
                List<GetFumigationWaitingNotificationDetailsDto> getWaitingFumigationDetailsDtosLst = new List<GetFumigationWaitingNotificationDetailsDto>();
                getWaitingFumigationDetailsDtosLst = GWTR.GetFumigationWaitingNotificationDetails();


                if (getWaitingFumigationDetailsDtosLst.Count > 0)
                {
                    if (getWaitingFumigationDetailsDtosLst != null)
                    {
                        foreach (var item in getWaitingFumigationDetailsDtosLst)
                        {
                            //  var WaitinglogoURL = (LarastruckingApp.Entities.Common.Configurations.ImageURL + LarastruckingApp.Entities.Common.Configurations.LarasLogo);

                            string to = item.customerEmail;
                            string subject = string.Empty;
                            if (!string.IsNullOrEmpty(item.FumigationEmailDTO.AirWayBill))
                            {
                                subject = subject + " AWB# : " + item.FumigationEmailDTO.AirWayBill + ",";
                            }
                            if (!string.IsNullOrEmpty(item.FumigationEmailDTO.CustomerPO))
                            {
                                subject = subject + " PO# : " + item.FumigationEmailDTO.CustomerPO + ",";
                            }
                            if (!string.IsNullOrEmpty(item.FumigationEmailDTO.OrderNo))
                            {
                                subject = subject + " ORDERNO# : " + item.FumigationEmailDTO.OrderNo + ",";
                            }
                            item.AWBPoOrderNO = subject.Trim(',');

                            subject = LarastruckingResource.WaitingTimeStatus + subject.Trim(',') + " | STATUS : " + item.FumigationEmailDTO.Status;
                            string bodywithsignature = LarastruckingResource.QuoteMailSignature;
                            string url = HostingEnvironment.MapPath("~/Views/Shared/_watingTimeEmailForCustomer.html");
                            reader = new StreamReader(url);
                            string readFile = reader.ReadToEnd();
                            string mailBody = string.Empty;
                            string myString = "<tr>";
                            myString = readFile;
                            myString = myString.Replace("$$LogoURL$$", item.FumigationEmailDTO.LogoURL);
                            myString = myString.Replace("$$AWBPoOrderNO$$", item.FumigationEmailDTO.AWBPoOrderNO);
                            myString = myString.Replace("$$OrderTaken$$", Convert.ToDateTime(item.FumigationEmailDTO.OrderTaken).ToString("MM/dd/yyyy HH:mm"));

                            string ShipmentStatusHistory = "";
                            foreach (var value in item.FumigationEmailDTO.FumigationStatusHistory)
                            {
                                ShipmentStatusHistory += "<td><img src=" + value.ImageUrl + " /></td>";
                            }

                            string ShipmentRouteStop = "";
                            if (item.FumigationEmailDTO.FumigationRoute.Count > 0)
                            {
                                // foreach (var value1 in item.ShipmentEmailDTO.ShipmentRoutesStop)
                                // {
                                ShipmentRouteStop += "<td style='text-align:center;'> <a href=" + item.FumigationEmailDTO.ProofOfDeliveryURL + ">Click to view Proof of Delivery</a></td>";
                                // }
                            }
                            else
                            {
                                ShipmentRouteStop = "<td></td>";
                            }



                            string ShipmentAccessorialPrice = "";
                            foreach (var accessorial in item.FumigationEmailDTO.AccessorialPrice)
                            {
                                ShipmentAccessorialPrice += "<tr>" +
                                                        "<td style='width: 60%;text-transform: uppercase;' >" + accessorial.FeeType + "</td>" +
                                                         "<td>" + accessorial.Amount + "</td>" +
                                                          "</tr>";
                            }

                            string pickupAddress = item.PickupAddress;
                            string[] pickupAddressList = pickupAddress.Split(',');
                            string DeliveryAddress = item.DeliveryAddress;
                            string[] DeliveryAddressList = DeliveryAddress.Split(',');


                            string waitingTimeBody = "";

                            // if (item.IsPickUpWaitingTimeRequired == true)
                            // {
                            waitingTimeBody += "<tr>" +
                                         "<td valign='top' style = 'width:40%; text-transform: uppercase;'>PICKUP LOCATION:" + "</td>" +
                                          "<td style = 'width:60%; text-transform: uppercase;'>" + pickupAddressList[0] + "<br>" + pickupAddressList[1] + "<br>" + pickupAddressList[2] + "</td>" +
                                    "</tr>";
                            if (item.PickupArrivedOn != DateTime.MinValue)
                            {
                                waitingTimeBody += "<tr>" +
                                                   "<td style = 'width:40%; text-transform: uppercase;'>Waiting Time Started:" + "</td>" +
                                                   "<td style = 'width:60%; text-transform: uppercase;'>" + item.PickupArrivedOn.ToString("MM/dd/yyyy HH:mm") + "</td>" +
                                                   "</tr>";
                            }
                            if (item.PickupDepartedOn != DateTime.MinValue)
                            {
                                waitingTimeBody += "<tr>" +
                                                    "<td style = 'width:40%; text-transform: uppercase;'>Waiting Time Ended:" + "</td>" +
                                                    "<td style = 'width:60%; text-transform: uppercase;'>" + item.PickupDepartedOn.ToString("MM/dd/yyyy HH:mm") + "</td>" +
                                                    "</tr>" +

                                                     "<tr>" +
                                                      "<td style = 'width:40%; text-transform: uppercase;'>Total Waiting Time:" + "</td>" +
                                                      "<td style = 'width:60%; text-transform: uppercase;'>" + item.PickUpDateDifference + "</td>" +
                                                      "</tr>";
                            }
                            //}
                            // if (item.IsDeliveryWaitingTimeRequired == true)
                            //{
                            waitingTimeBody += "<tr>" +
                                       "<td valign='top' style = 'width:40%; text-transform: uppercase;'>Delivery LOCATION:" + "</td>" +
                                        "<td style = 'width:60%; text-transform: uppercase;'>" + DeliveryAddressList[0] + "<br>" + DeliveryAddressList[1] + "<br>" + DeliveryAddressList[2] + "</td>" +
                                  "</tr>";
                            if (item.DeliveryArrivedOn != DateTime.MinValue)
                            {

                                waitingTimeBody += "<tr>" +
                                       "<td style = 'width:40%; text-transform: uppercase;'>Waiting Time Started Date:" + "</td>" +
                                        "<td style = 'width:60%; text-transform: uppercase;'>" + item.DeliveryArrivedOn.ToString("MM/dd/yyyy HH:mm") + "</td>" +
                                  "</tr>";
                            }

                            if (item.DeliveryDepartedOn != DateTime.MinValue)
                            {
                                waitingTimeBody += "<tr>" +
                                       "<td style = 'width:40%; text-transform: uppercase;'>Waiting Time Ended Date:" + "</td>" +
                                        "<td style = 'width:60%; text-transform: uppercase;'>" + item.DeliveryDepartedOn.ToString("MM/dd/yyyy HH:mm") + "</td>" +
                                  "</tr>" +

                                   "<tr>" +
                                       "<td style = 'width:40%; text-transform: uppercase;'>Total Waiting Time:" + "</td>" +
                                        "<td style = 'width:60%; text-transform: uppercase;'>" + item.DeliveryDateDifference + "</td>" +
                                  "</tr>";
                            }
                            //}

                            myString = myString.Replace("$$waitingTimeBody$$", waitingTimeBody);

                            myString = myString.Replace("$$FreightType$$", item.FumigationEmailDTO.FreightType);
                            myString = myString.Replace("$$Commodity$$", item.FumigationEmailDTO.Commodity);
                            myString = myString.Replace("$$Pallet$$", (item.FumigationEmailDTO.Pallet).ToString());
                            myString = myString.Replace("$$NoOfBox$$", (item.FumigationEmailDTO.NoOfBox).ToString());
                            myString = myString.Replace("$$Weight$$", (item.FumigationEmailDTO.Weight).ToString());
                            myString = myString.Replace("$$ActualTemp$$", item.FumigationEmailDTO.ActualTemp);
                            myString = myString.Replace("$$ShipmentStatusHistory$$", ShipmentStatusHistory);
                            myString = myString.Replace("$$ProofOfDeliveryURL$$", ShipmentRouteStop);
                            myString = myString.Replace("$$ESTDateTime$$", Convert.ToDateTime(item.FumigationEmailDTO.ESTDateTime).ToString("MM/dd/yyyy HH:mm"));
                            myString = myString.Replace("$$ShipmentAccessorialPrice$$", ShipmentAccessorialPrice);


                            myString = myString.Replace("$$PickupAddress$$", item.PickupAddress);
                            myString = myString.Replace("$$PickupArrivedOn$$", Convert.ToString(item.PickupArrivedOn));
                            myString = myString.Replace("$$PickupDepartedOn$$", Convert.ToString(item.PickupDepartedOn));
                            myString = myString.Replace("$$PickUpDateDifference$$", item.PickUpDateDifference);
                            myString = myString.Replace("$$SDeliveryAddress$$", item.DeliveryAddress);
                            myString = myString.Replace("$$DeliveryArrivedOn$$", Convert.ToString(item.DeliveryArrivedOn));
                            myString = myString.Replace("$$DeliveryDepartedOn$$", Convert.ToString(item.DeliveryDepartedOn));
                            myString = myString.Replace("$$DeliveryDateDifference$$", item.DeliveryDateDifference);
                            //myString = myString.Replace("$$IsPickUpWaitingTimeRequired$$", Convert.ToString(item.IsPickUpWaitingTimeRequired));
                            //myString = myString.Replace("$$IsDeliveryWaitingTimeRequired$$", Convert.ToString(item.IsDeliveryWaitingTimeRequired));
                            myString = myString + bodywithsignature;

                            mailBody = myString.ToString();

                            if (!string.IsNullOrEmpty(mailBody))
                            {
                                Email.AsyncSendEmail(to, subject, mailBody);
                                GWTR.UpdateFumigationWaitingTime(item.FumiWatingNotificationId, item.PickupArrivedOn, item.PickupDepartedOn, item.DeliveryArrivedOn, item.DeliveryDepartedOn);
                            }



                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                reader.Dispose();
                GWTR.Dispose();
                Ex.Dispose();
            }
        }
        #endregion

        #region Driver Docs Expiration sending Mail
        // <summary>
        // Driver Docs Expiration sending Mail
        // </summary>

        public void DriverDocsExpireSendMail()
        {
            ExecuteSQLStoredProceduce Ex = null;
            GlobalWaitingTimeRepository GWTR = null;
            StreamReader reader = null;
            try
            {


                Ex = new ExecuteSQLStoredProceduce();
                IShipmentRepository iShipmentRepository = new ShipmentRepository();
                IFumigationRepository iFumigationRepository = new FumigationRepository();
                GWTR = new GlobalWaitingTimeRepository(Ex, iShipmentRepository, iFumigationRepository);
                List<GetDriverExpDetailsDTO> GetDriverExpDetails = new List<GetDriverExpDetailsDTO>();
                GetDriverExpDetails = GWTR.GetDriverExpDetails();
                string to = string.Empty;
                string mailBody = string.Empty;
                string subject = string.Empty;

                if (GetDriverExpDetails.Count > 0 && GetDriverExpDetails != null)
                {

                    foreach (var item in GetDriverExpDetails.GroupBy(x => x.DriverID))
                    {
                        var documentsList = item.Where(x => x.EmailSentDate == null || (x.EmailSentDate != null && x.EmailSentDate.Value.Date < Configurations.TodayDateTime.Date)).ToList();
                        if (documentsList.Count > 0)
                        {
                            //if (item.FirstOrDefault().EmailSentDate == null || (item.EmailSentDate != null && item.EmailSentDate.Value.Date < Configurations.TodayDateTime.Date))
                            //{
                            var DocsName = string.Empty;
                            string DriverDocuments = "";

                            foreach (var Driver in documentsList)
                            {
                                //if (Driver.EmailSentDate == null || (Driver.EmailSentDate != null && Driver.EmailSentDate.Value.Date < Configurations.TodayDateTime.Date))
                                //{
                                DocsName += Driver.DocumentName + " & ";
                                DriverDocuments += "<p><span style='font-size:14px;'> Document: </span>" + Driver.DocumentName + "</p><p><i></i></p><i></i>" +
                                                    "<p><span style='font-size:14px;'> Expiration Date: </span>" + Driver.DocumentExpiryDate.ToString("MM/dd/yyyy") + "</p><p><i></i></p><i></i>";
                                //}
                            }
                            to = item.FirstOrDefault().EmailId;
                            subject = LarastruckingResource.DriverDocumetExpiration + " " + DocsName.Remove(DocsName.Length - 3);
                            var DriverlogoURL = (LarastruckingApp.Entities.Common.Configurations.ImageURL + LarastruckingApp.Entities.Common.Configurations.LarasLogo);
                            string url = HostingEnvironment.MapPath("~/Views/Shared/_DriverExpirationDocsEmail.html");
                            reader = new StreamReader(url);
                            string readFile = reader.ReadToEnd();
                            //string DriverDocuments = "";
                            //foreach (var Driver in GetDriverExpDetails)
                            //{
                            //    DriverDocuments += "<p><span style='font-size:14px;'> Document: </span>" + Driver.DocumentName + "</p><p><i></i></p><i></i>" +
                            //                        "<p><span style='font-size:14px;'> Expiration Date: </span>" + Driver.DocumentExpiryDate.ToString("MM/dd/yyyy") + "</p><p><i></i></p><i></i>";
                            //}

                            string myString = "";
                            myString = readFile;
                            myString = myString.Replace("$$DriverName$$", item.FirstOrDefault().DriverName);
                            myString = myString.Replace("$$DriverDocuments$$", DriverDocuments);
                            myString = myString.Replace("$$DriverlogoURL$$", DriverlogoURL);
                            mailBody = myString.ToString();
                            if (!string.IsNullOrEmpty(mailBody))
                            {
                                Email.AsyncSendEmail(to, subject, mailBody);
                                GWTR.UpdateDriverDocsExpDetails(item.ToList());

                            }
                        }
                        //}
                    }
                    //if (!string.IsNullOrEmpty(mailBody))
                    //{
                    //    Email.AsyncSendEmail(to, subject, mailBody);
                    //GWTR.UpdateDriverDocsExpDetails(GetDriverExpDetails);

                    //}
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //+-reader.Dispose();
                GWTR.Dispose();
                Ex.Dispose();
            }

        }
        #endregion

        #region Delete Record
        public void DeleteRecord()
        {
            GlobalWaitingTimeRepository GWTR = null;
            ExecuteSQLStoredProceduce Ex = null;
            Ex = new ExecuteSQLStoredProceduce();
            IShipmentRepository iShipmentRepository = new ShipmentRepository();
            IFumigationRepository iFumigationRepository = new FumigationRepository();
            GWTR = new GlobalWaitingTimeRepository(Ex, iShipmentRepository, iFumigationRepository);
            GWTR.DeleteLastMailRecord();

        }

        #endregion

    }
}


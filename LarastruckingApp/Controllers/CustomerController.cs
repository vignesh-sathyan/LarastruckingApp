using LarastruckingApp.BusinessLayer.CustomerModule;
using LarastruckingApp.BusinessLayer.DriverModule;
using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.DTO;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.CustomerDto;
using LarastruckingApp.Entities.Permission;
using LarastruckingApp.Entities.quotesDto;
using LarastruckingApp.Infrastructure;
using LarastruckingApp.Log.Utility;
using LarastruckingApp.Resource;
using LarastruckingApp.ViewModel;
using LarastruckingApp.ViewModel.Customer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace LarastruckingApp.Controllers
{

    [Authorize]
    public class CustomerController : BaseController
    {
        #region Private Members
        /// <summary>
        /// defining private member
        /// </summary>
        private readonly ICustomerBAL iCustomerRepo;
        private readonly ICustomerModuleBAL iCustomerModule;
        private readonly PermissionsDto permissions;
        private MemberProfile memberProfile = null;

     
        #endregion

        #region Constuctor
        /// <summary>
        /// Constructor Injection
        /// </summary>
        /// <param name="iICustomerRegistrationBALBAL"></param>
        public CustomerController(ICustomerBAL iICustomerRegistrationBALBAL, ICustomerModuleBAL ICustomerModuleBAL)
        {
            iCustomerModule = ICustomerModuleBAL;
            iCustomerRepo = iICustomerRegistrationBALBAL;
            permissions = IsPermited.HasPermission();
            memberProfile = new MemberProfile();
           
        }
        #endregion

        #region REGION: ADMIN SECTION

        #region PRIVATE: Bind State and Country
        /// <summary>
        /// Private method to bind drop-downs
        /// </summary>
        private void BindStateCountryDropdown()
        {
            ViewBag.Country = iCustomerRepo.GetCountryList();
            ViewBag.State = new SelectList(iCustomerRepo.GetStateList(), "ID", "Name");
        }
        #endregion

        #region Bind State

        /// <summary>
        /// Bind State
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public ActionResult GetStates(int countryId)
        {
            var stateList = iCustomerRepo.GetStates(countryId);
            return Json(stateList, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region GET: Customer Index
        /// <summary>
        /// Customer Index
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize]
        public ActionResult Index(int id = 0)
        {
            try
            {
                CustomerDto objCustomerDTO = new CustomerDto();
                BindStateCountryDropdown();
                if (id > 0)
                {
                    objCustomerDTO = iCustomerRepo.FindById(id);
                }
                else
                {
                    objCustomerDTO.CustomerId = id;
                }
                return View(objCustomerDTO);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region POST: Add Customer
        /// <summary>
        /// Add Customer : httppost
        /// </summary>
        /// <param name="customerDto"></param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult Index(CustomerDto customerDto)
        {
            BindStateCountryDropdown();
            JsonResponse objJsonResponse = new JsonResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    if (customerDto.CustomerId == 0)
                    {
                        customerDto.CreatedBy = memberProfile.UserId;
                        customerDto.GuidInUser = Guid.NewGuid().ToString();


                        var result = iCustomerRepo.Add(customerDto);

                        if (result.IsSuccess)
                        {
                            string baseURL = LarastruckingApp.Entities.Common.Configurations.BaseURL;
                            string resetLink = baseURL.Trim() + LarastruckingResource.ForgotPasswordResetPassword + customerDto.GuidInUser;
                            string subject = LarastruckingResource.CreateNewPasswordLink;
                            string message = " Dear " + customerDto.CustomerName + ", <br /><br /> please click on the below link to create your password. <br /><br /> <a href=" + resetLink.Trim() + ">" + resetLink.Trim() + "</a> <br /><br /><br /><br />  Best Regards,<br/> Lara’s Trucking & Logistics";

                            string emailSent = string.Empty;

                            MailWithCCAttachDTO mailData = new MailWithCCAttachDTO();

                            mailData.MailPurpose = "Add Customer";
                            mailData.ToMail = customerDto.BillingEmail;
                            mailData.ToMailCC = string.Empty;
                            mailData.ToMailBCC = string.Empty;
                            mailData.MailSubject = subject;
                            mailData.MailBody = message;
                            mailData.strMailtype = string.Empty;
                            mailData.CreatedBy = memberProfile.UserId;
                            mailData.CreatedOn = Configurations.TodayDateTime;
                            Email.SendMailWithCCAttach(mailData, out emailSent);

                            objJsonResponse.IsSuccess = true;
                            objJsonResponse.Message = LarastruckingResource.DataSaveSuccessfully;
                            return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                        }
                        else if (result.IsSuccess == false && result.Response == LarastruckingResource.Exists)
                        {
                            objJsonResponse.IsSuccess = false;
                            objJsonResponse.Message = LarastruckingResource.CustomerExists;
                            return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            objJsonResponse.IsSuccess = false;
                            if (!string.IsNullOrEmpty(result.Response))
                            {
                                objJsonResponse.Message = result.Response;
                            }
                            else
                            {
                                objJsonResponse.Message = LarastruckingResource.SomethingWentWrong;
                            }
                            return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if (customerDto.CustomerId > 0)
                    {
                        var result = iCustomerRepo.Update(customerDto);

                        if (result.IsSuccess)
                        {
                            objJsonResponse.IsSuccess = true;
                            objJsonResponse.Message = LarastruckingResource.DataUpdateSuccessfully;
                            return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                        }
                        else if (result.IsSuccess == false && result.Response == LarastruckingResource.Exists)
                        {
                            objJsonResponse.IsSuccess = false;
                            objJsonResponse.Message = LarastruckingResource.CustomerExists;
                            return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            objJsonResponse.IsSuccess = false;
                            if (!string.IsNullOrEmpty(result.Response))
                            {
                                objJsonResponse.Message = result.Response;
                            }
                            else
                            {
                                objJsonResponse.Message = LarastruckingResource.SomethingWentWrong;
                            }
                            return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                 
                    objJsonResponse.IsSuccess = false;
                    objJsonResponse.Message = LarastruckingResource.InvalidInputs;
                    return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
               // ErrorLog("Customer Creation Failed: "+ex.Message.ToString());
                throw;
            }

            objJsonResponse.IsSuccess = false;
            objJsonResponse.Message = LarastruckingResource.InvalidInputs;
            return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region:Get View Customer
        /// <summary>
        /// Action method for load default view
        /// </summary>
        /// <returns></returns>

        [CustomAuthorize]
        public ActionResult ViewCustomer()
        {
            return View();
        }
        #endregion

        #region POST: Load Data
        /// <summary>
        /// Action method for bind  customer table
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadData(string type)
        {
            try
            {
                CustomerListViewModel dto = new CustomerListViewModel();

                string search = Request.Form.GetValues("search[value]").FirstOrDefault();
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();

                // Find Order Column
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var lstDriverDTO = iCustomerRepo.GetAllCustomers(type);

                if (!string.IsNullOrEmpty(search))
                {
                    lstDriverDTO = lstDriverDTO.Where(x => x.CustomerName.ToUpper().Contains(search.ToUpper()) ||
                                                        x.BillingPhoneNumber.ToUpper().Contains(search.ToUpper()) ||
                                                        x.BillingEmail.ToUpper().Contains(search.ToUpper()));
                }
                recordsTotal = lstDriverDTO.Count();

                dto.Customers = lstDriverDTO.ToList();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {

                    dto.Customers = sortColumnDir == "asc" ? dto.Customers.OrderBy(x => x.GetType().GetProperty(sortColumn).GetValue(x, null)).ToList() : dto.Customers.OrderByDescending(x => x.GetType().GetProperty(sortColumn).GetValue(x, null)).ToList();
                    dto.Customers = dto.Customers.Skip(skip).Take(pageSize).ToList();
                }
                dto.Permissions = permissions;
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dto.Customers, permissions = dto.Permissions }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region POST: Delete Customer
        /// <summary>
        /// Action method for delete customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteCustomer(int id = 0)
        {
            try
            {
                CustomerDto objCustomerDto = new CustomerDto();
                objCustomerDto.CustomerId = id;
                JsonResponse objJsonResponse = new JsonResponse();
                if (iCustomerRepo.Delete(objCustomerDto))
                {
                    objJsonResponse.IsSuccess = true;
                    objJsonResponse.Message = LarastruckingResource.DataDeleteSuccessfully;
                    return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    objJsonResponse.IsSuccess = false;
                    objJsonResponse.Message = LarastruckingResource.ErrorOccured;
                    return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Customer
        /// <summary>
        /// This method is used to get all customers for the purpose of binding it in the drop-down
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetAllCustomer(string searchText)
        {
            var customers = iCustomerRepo.GetAllCustomer(searchText);
            return Json(customers, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion


        #region REGION: CUSTOMER USER
        #region Customer: Get => Dashboard

        #region dashboard : Current Shipment 

        /// <summary>
        /// Customer Dashboard
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [CustomAuthorize]
        public ActionResult Dashboard()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Customer dashboard : Old Shipment 

        /// <summary>
        /// Customer Dashboard
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [CustomAuthorize]
        public ActionResult OldShipmentDashboard()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #endregion

        #region Customer : Get => Customer Details

        #region Customer Shipment Details 
        /// <summary>
        /// Customer Details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="CustomerId"></param>
        /// <returns></returns>

        [HttpGet]
        [CustomAuthorize]
        public ActionResult Detail(int id = 0)
        {
            try
            {
                CustomerQuotesViewModel custShipRoutes = new CustomerQuotesViewModel();
                custShipRoutes.CutomerShipmentRoutesDetail = iCustomerModule.GetCustomerShipmentRoutes(id);
                custShipRoutes.CustomerShipmentTrack = iCustomerModule.GetCustomerShipmentTrack(id);
                return View(custShipRoutes);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region Shipment Track Records
        /// <summary>
        /// Shipment Track Records
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        [CustomAuthorize]
        public ActionResult TrackShipment(int id = 0)
        {
            try
            {
                CustomerQuotesViewModel custShipRoutes = new CustomerQuotesViewModel();
                custShipRoutes.CustomerShipmentTrack = iCustomerModule.GetCustomerShipmentTrack(id);
                ViewBag.StatusList = new SelectList(iCustomerModule.GetStatusList().ToList(), "StatusId", "StatusName");
                return View(custShipRoutes);
            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion

        #region Get Shipment Freight Details
        /// <summary>
        /// Get Shipment Freight Details By  ShippingRoutes Id
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        public ActionResult GetShipmentFreightDetails(int id)
        {
            try
            {
                CustomerQuotesViewModel custShipRoutes = new CustomerQuotesViewModel();
                custShipRoutes.ShipmentFreightList = iCustomerModule.GetShipmentFreightDetails(id);
                return PartialView("_CustomerFreightDetails", custShipRoutes);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region  Get shipment Customer Accessorial Charge
        /// <summary>
        /// Get Customer Accessorial Charge
        /// </summary>
        /// <param name="hipmentId"></param>
        /// <param name="routeId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCustomerAccessorialCharge(int shipmentId, int routeId)
        {
            try
            {
                var customerAccessorialCharge = iCustomerModule.GetCustomerAccessorialCharge(shipmentId, routeId);
                return Json(customerAccessorialCharge, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion
        #region  Get Customer Accessorial Charge
        /// <summary>
        /// Get Customer Accessorial Charge
        /// </summary>
        /// <param name="hipmentId"></param>
        /// <param name="routeId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCustFumAccessorialCharge(int fumigationId, int routeId)
        {
            try
            {
                var customerAccessorialCharge = iCustomerModule.GetCustFumAccessorialCharge(fumigationId, routeId);
                return Json(customerAccessorialCharge, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion
        #region NON-ACTION METHOD

        #region Get Customer Details
        [HttpPost]
        public JsonResult GetCustomerDashboardDetails()
        {
            try
            {
                string search = Request.Form.GetValues((Configurations.SearchValue)).FirstOrDefault();
                var draw = Request.Form.GetValues(Configurations.Draw).FirstOrDefault();
                var start = Request.Form.GetValues(Configurations.Start).FirstOrDefault();
                var length = Request.Form.GetValues(Configurations.Length).FirstOrDefault();

                // Find Order Column
                var sortColumn = Request.Form.GetValues(Configurations.Columns + Request.Form.GetValues(Configurations.OrderColumn).FirstOrDefault() + Configurations.Name).FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues(Configurations.OrderDir).FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                DataTableFilterDto dto = new DataTableFilterDto()
                {
                    PageSize = pageSize,
                    PageNumber = skip,
                    SearchTerm = search,
                    SortColumn = sortColumn,
                    SortOrder = sortColumnDir,
                    TotalCount = recordsTotal

                };
                var CustInfoDetails = iCustomerModule.GetAllQuotes(dto, memberProfile.UserId);

                recordsTotal = (CustInfoDetails.Count > 0) ? CustInfoDetails.FirstOrDefault().TotalRecord : 0;



                return Json(new { draw = draw, recordsFiltered = dto.TotalCount, recordsTotal = dto.TotalCount, data = CustInfoDetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Shipment Location Details
        /// <summary>
        /// Get Shipment Details for Multiple Routes By Shipment Id 
        /// </summary>
        /// <param name="ShipmentId"></param>
        /// <returns></returns>
        public JsonResult GetCustomerShipmentRoutesDetails(int id)
        {
            try
            {
                var customerShipmentLocationList = iCustomerModule.GetCustomerShipmentRoutesDetails(id);
                return Json(customerShipmentLocationList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Shipment Damaged Files
        /// <summary>
        ///   Get Shipment Damaged Files By ShippingRoutes Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetShipmentDamagedFiles(int id)
        {
            try
            {
                var shipmentDamagedFiles = iCustomerModule.GetShipmentDamagedFiles(id);
                return Json(shipmentDamagedFiles, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Get shipment Proof of Temp 
        /// <summary>
        ///   Get shipment Proof of Temp 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetShipmentProofOfTempFiles(int id, int ShipmentFreightDetailId)
        {
            try
            {
                var shipmentProofFiles = iCustomerModule.GetShipmentProofOfTempFiles(id, ShipmentFreightDetailId);
                return Json(shipmentProofFiles, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Freight Details By ID 
        /// <summary>
        ///   Get Freight Details By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetFreightDetailsById(int id)
        {
            try
            {
                var FreightDetailsbyId = iCustomerModule.GetFreightDetailsById(id);
                return Json(FreightDetailsbyId, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Freight Details
        /// <summary>
        /// Update Freight Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateFreightDetails(UpdateFreightDetailsDTO dto)
        {
            try
            {

                var response = iCustomerModule.UpdateFreightDetails(dto);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #endregion

        #endregion

        #region Customer : Get => Fumigation Details on customer dashboard

        #region Fumigation Details on customer dashboard
        /// <summary>
        /// Customer Details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="CustomerId"></param>
        /// <returns></returns>

        [HttpGet]
        [CustomAuthorize]
        public ActionResult FumigationDetail(int id = 0)
        {
            try
            {
                CustomerQuotesViewModel custShipRoutes = new CustomerQuotesViewModel();
                custShipRoutes.CustomerFumigationRoutesDetail = iCustomerModule.GetCustomerFumigationRoutes(id);
                custShipRoutes.CustomerFumigationTrack = iCustomerModule.GetCustomerFumigationTrack(0, id);
                return View(custShipRoutes);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Fumigation Track Records
        /// <summary>
        /// Shipment Track Records
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        [CustomAuthorize]
        public ActionResult TrackFumigation(int RouteId, int Id = 0)
        {
            try
            {
                CustomerQuotesViewModel custShipRoutes = new CustomerQuotesViewModel();
                custShipRoutes.CustomerFumigationTrack = iCustomerModule.GetCustomerFumigationTrack(RouteId, Id);
                ViewBag.FumigationStatusList = new SelectList(iCustomerModule.GetFumigationStatusList().ToList(), "StatusId", "StatusName");
                return View(custShipRoutes);
            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion



        #endregion

        #region NON-Action METHODS

        #region Get Fumigation Location Details
        /// <summary>
        /// Get Fumigation Details for Multiple Routes By Shipment Id 
        /// </summary>
        /// <param name="FumigationId"></param>
        /// <returns></returns>
        public JsonResult GetCustomerFumigationRoutesDetails(int id)
        {
            try
            {
                var customerFumigationLocationList = iCustomerModule.GetCustomerFumigationRoutesDetails(id);
                return Json(customerFumigationLocationList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Fumigation Damaged Files
        /// <summary>
        ///   Get Fumigation Damaged Files By ShippingRoutes Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetFumigationDamagedFiles(int id)
        {
            try
            {
                var FumigationDamagedFiles = iCustomerModule.GetFumigationDamagedFiles(id);
                return Json(FumigationDamagedFiles, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Get Fumigation Proof of Temp 
        /// <summary>
        ///   Get Fumigation Proof of Temp 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetFumigationProofOfTempFiles(int id)
        {
            try
            {
                var FumigationProofFiles = iCustomerModule.GetFumigationProofOfTempFiles(id);
                return Json(FumigationProofFiles, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #endregion

        #endregion

        #region  Get Customer Details : old shipment 

        #region old shipment 
        [HttpPost]
        public JsonResult GetCustomerOldDashboardDetails()
        {
            try
            {
                string search = Request.Form.GetValues("search[value]").FirstOrDefault();
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();

                // Find Order Column
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                DataTableFilterDto dto = new DataTableFilterDto()
                {
                    PageSize = pageSize,
                    PageNumber = skip,
                    SearchTerm = search,
                    SortColumn = sortColumn,
                    SortOrder = sortColumnDir,
                    TotalCount = recordsTotal

                };
                var CustInfoDetails = iCustomerModule.GetOldShipmentDetails(dto, memberProfile.UserId);

                recordsTotal = (CustInfoDetails.Count > 0) ? CustInfoDetails.FirstOrDefault().TotalRecord : 0;

                if (CustInfoDetails.Count > 0)
                {
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        CustInfoDetails = sortColumnDir == "asc" ? CustInfoDetails.OrderBy(x => x.GetType().GetProperty(sortColumn).GetValue(x, null)).ToList()
                            : CustInfoDetails.OrderByDescending(x => x.GetType().GetProperty(sortColumn).GetValue(x, null)).ToList();
                    }
                }

                return Json(new { draw = draw, recordsFiltered = dto.TotalCount, recordsTotal = dto.TotalCount, data = CustInfoDetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #endregion
        #endregion

        public static void ErrorLog(string sErrMsg)
        {
            string sLogFormat;
            string sErrorTime;

            sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";

            //this variable used to create log filename format "
            //for example filename : ErrorLogYYYYMMDD
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString();
            string sDay = DateTime.Now.Day.ToString();
            sErrorTime = sYear + sMonth + sDay;
            string path = System.Web.HttpContext.Current.Server.MapPath("../Assets/ErrorLog");
            StreamWriter sw = new StreamWriter(path + sErrorTime, true);
            sw.WriteLine(sLogFormat + sErrMsg);
            sw.Flush();
            sw.Close();
        }
    }
}


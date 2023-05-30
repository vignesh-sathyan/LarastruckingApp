using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.Controllers;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Fumigation;
using LarastruckingApp.Infrastructure;
using LarastruckingApp.Resource;
using LarastruckingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LarastruckingApp.Entities.Enum;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Utility;
using LarastruckingApp.Log.Utility;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Net;
using System.Web.Configuration;

namespace LarastruckingApp.Areas.Fumigation.Controllers
{
    [Authorize]
    public class FumigationController : BaseController
    {

        #region Private Member
        /// <summary>
        /// Defining private members
        /// </summary>
        private readonly IShipmentBAL shipmentBAL = null;
        private readonly IFumigationBAL fumigationBAL = null;
        private readonly IDriverBAL driverBAL;
        private readonly IAddressBAL addressBAL;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iShipmentBAL"></param>
        public FumigationController(IShipmentBAL iShipmentBAL, IAddressBAL iAddressBAL, IFumigationBAL ifumigationBAL, IDriverBAL iDriverBAL)
        {
            shipmentBAL = iShipmentBAL;
            fumigationBAL = ifumigationBAL;
            driverBAL = iDriverBAL;
            addressBAL = iAddressBAL;
        }
        #endregion

        // GET: Fumigation/Fumigation
        [CustomAuthorize]
        public ActionResult Index(int id = 0)
        {
            IEnumerable<SelectListItem> WeightUnit = from Unit qt in Enum.GetValues(typeof(Unit))
                                                     select new SelectListItem
                                                     {
                                                         Text = qt.ToString(),
                                                         Value = Convert.ToInt32(qt).ToString()

                                                     };
            ViewBag.Id = id;
            ViewBag.WeightUnit = new SelectList(WeightUnit, "Value", "Text");
            ViewBag.Country = driverBAL.GetCountryList();
            ViewBag.State = new SelectList(driverBAL.GetStateList(), "ID", "Name");
            ViewBag.AddressType = BindAddressType();
            return View();
        }

        #region Get Address Type
        /// <summary>
        /// method for bind address type
        /// </summary>
        /// <returns></returns>
        public List<AddressTypeViewModel> BindAddressType()
        {
            List<AddressTypeViewModel> lstAddressTypeViewModel = null;
            try
            {
                lstAddressTypeViewModel = AutoMapperServices<AddressTypeDTO, AddressTypeViewModel>.ReturnObjectList(addressBAL.BindAddressType().ToList());
            }
            catch (Exception)
            {

                throw;
            }

            return lstAddressTypeViewModel;
        }

        #endregion

        #region Shipment Status
        /// <summary>
        /// shipment status
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetShipmentStatus()
        {
            try
            {
                var statusList = fumigationBAL.GetStatusList();
                return Json(statusList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Shipment Substatus
        /// <summary>
        /// Shipment Sub Status
        /// </summary>
        /// <param name="statusid"></param>
        /// <returns></returns>
        public ActionResult GetShipmentSubStatus(int statusid)
        {
            try
            {
                var substatusList = shipmentBAL.GetSubStatusList(statusid);
                return Json(substatusList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region GetFumigationTypeList
        /// <summary>
        /// Get Fumigation Type  List
        /// </summary>
        /// <returns></returns>
        public ActionResult GetFumigationTypeList()
        {
            var fumigationList = fumigationBAL.GetFumigationTypeList();
            return Json(fumigationList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region save fumigation detail
        [HttpPost]
        public ActionResult CreateFumigation(FumigationDTO model)
        {
            try
            {

                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();
                model.CreatedBy = objMemberProfile.UserId;
                if (model != null)
                {
                    var result = fumigationBAL.CreateFumigation(model);
                    objJsonResponse.IsSuccess = result.IsSuccess;
                    objJsonResponse.Message = result.IsSuccess ? LarastruckingResource.DataSaveSuccessfully : LarastruckingResource.SomethingWentWrong;
                    if (result.IsSuccess)
                    {
                        FumigationEmailDTO objfumigationEmail = new FumigationEmailDTO();
                        objfumigationEmail.CustomerId = model.CustomerId;
                        if (model.CustomerId > 0)
                        {
                            var fumigationEmailDetail = fumigationBAL.GetCustomerDetail(objfumigationEmail);
                            if (fumigationEmailDetail != null)
                            {
                                //SendFumigationMail(fumigationEmailDetail);
                            }

                        }
                    }
                    return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    objJsonResponse.IsSuccess = false;
                    objJsonResponse.Message = LarastruckingResource.SomethingWentWrong;
                    return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region edit fumigation
        /// <summary>
        /// Edit Fumigation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditFumigation(FumigationDTO model)
        {
            try
            {

                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();
                model.ModifiedBy = objMemberProfile.UserId;

                if (model != null)
                {
                    var result = fumigationBAL.EditFumigation(model);
                    objJsonResponse.IsSuccess = result.IsSuccess;
                    objJsonResponse.Message = result.IsSuccess ? LarastruckingResource.DataUpdateSuccessfully : LarastruckingResource.SomethingWentWrong;
                    if (result.IsSuccess)
                    {
                        if (result.IsMailNeedToSend)
                        {
                            FumigationEmailDTO objfumigationEmail = new FumigationEmailDTO();
                            objfumigationEmail.FumigationId = model.FumigationId;
                            if (model.FumigationId > 0)
                            {
                                var fumigationEmailDetail = fumigationBAL.GetCustomerDetail(objfumigationEmail);
                                if (fumigationEmailDetail != null)
                                {
                                    SendFumigationMail(fumigationEmailDetail);
                                }

                            }
                        }
                    }
                    return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    objJsonResponse.IsSuccess = false;
                    objJsonResponse.Message = LarastruckingResource.SomethingWentWrong;
                    return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region get accessorial fee type
        /// <summary>
        /// get accessorail fee type
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAccessorialFeeType()
        {
            try
            {
                var accessorialfeelist = shipmentBAL.GetShipmentAccessorialFeeType();
                return Json(accessorialfeelist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region view fumigation list
        /// <summary>
        /// Get method for view fumigation list
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize]
        public ActionResult ViewFumigationList()
        {
            return View();
        }
        #endregion

        #region get fumigation list from database
        /// <summary>
        /// Get fumigation list from database
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetFumigationList()
        {
            try
            {
                string search = Request.Form.GetValues(Configurations.SearchValue).FirstOrDefault();
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

                var preTripInfo = fumigationBAL.GetFumigationList(dto);
                return Json(new { draw = draw, recordsFiltered = dto.TotalCount, recordsTotal = dto.TotalCount, data = preTripInfo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region get other fumigation list from database
        /// <summary>
        /// Get other fumigation list from database
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetOtherFumigationList()
        {
            try
            {
                string search = Request.Form.GetValues(Configurations.SearchValue).FirstOrDefault();
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

                var preTripInfo = fumigationBAL.GetOtherFumigationList(dto);
                return Json(new { draw = draw, recordsFiltered = dto.TotalCount, recordsTotal = dto.TotalCount, data = preTripInfo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region view fumigation list get
        /// <summary>
        /// Get method for view fumigation list
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize]
        public ActionResult ViewAllFumigation()
        {
            return View();
        }
        #endregion

        #region View All Fumigation post
        /// <summary>
        /// View All Fumigation
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ViewAllFumigation(AllShipmentDTO model)
        {
            try
            {
                string search = Request.Form.GetValues(Configurations.SearchValue).FirstOrDefault();
                var draw = Request.Form.GetValues(Configurations.Draw).FirstOrDefault();
                var start = Request.Form.GetValues(Configurations.Start).FirstOrDefault();
                var length = Request.Form.GetValues(Configurations.Length).FirstOrDefault();

                // Find Order Column
                var sortColumn = Request.Form.GetValues(Configurations.Columns + Request.Form.GetValues(Configurations.OrderColumn).FirstOrDefault() + Configurations.Name).FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues(Configurations.OrderDir).FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                model.PageSize = pageSize;
                model.PageNumber = skip;
                model.SearchTerm = search;
                model.SortColumn = sortColumn;
                model.SortOrder = sortColumnDir;
                model.TotalCount = recordsTotal;
                var preTripInfo = fumigationBAL.ViewAllFumigation(model);
                return Json(new { draw = draw, recordsFiltered = model.TotalCount, recordsTotal = model.TotalCount, data = preTripInfo }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region delete fumigation
        /// <summary>
        ///delete fumigation By Id
        /// </summary>
        /// <param name="fumigationId"></param>
        /// <returns></returns>
        public ActionResult DeleteFumigation(int fumigationId)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();
                FumigationDTO objFumigation = new FumigationDTO();
                objFumigation.FumigationId = fumigationId;
                objFumigation.CreatedBy = objMemberProfile.UserId;
                objFumigation.CreatedOn = Configurations.TodayDateTime;

                objJsonResponse.IsSuccess = fumigationBAL.DeleteFumigation(objFumigation);
                objJsonResponse.Message = objJsonResponse.IsSuccess ? LarastruckingResource.DataDeleteSuccessfully : LarastruckingResource.ErrorOccured;
                return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region GetFumigationById
        /// <summary>
        /// get fumigation detail by id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFumigationById(int fumigationId)
        {
            var result = fumigationBAL.GetFumigationById(fumigationId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Upload proof of temperature
        /// <summary>
        ///Upload proof of temperature
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadProofofTemperature(FumigationProofOfTemprature model)
        {
            try
            {
                var TempimgUrl = "";
                //var fileName = "";
                bool isEmail = false;
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();
                double ActualTemp = 0.00;
                if (ModelState.IsValid)
                {
                    #region UPLOADED FILES : PROOF OF TEMP
                    List<FumigationProofOfTemprature> ListProofOfTempDto = new List<FumigationProofOfTemprature>();
                    if (model != null)
                    {
                        string fileName = string.Empty;
                        var tempFiles = model.filesObj;
                        var imgUrl = "";
                        var imgUrls = "";
                        for (int i=0; i<model.filesObj.Length; i++)
                        {
                            var tempFile = model.filesObj.GetValue(i);
                            if (tempFile != null)
                            {
                                model.ProofOfTemprature = tempFile as HttpPostedFileBase;
                                imgUrl = ImageUploader.Upload(model.ProofOfTemprature, out fileName, "Images/ProofFiles/Temperature", "proof_of_Temp_" + (i+1));
                                imgUrls = imgUrls + imgUrl + "|";
                                Console.WriteLine("tempFile: " + tempFile);
                            }
                        }
                        //var imgUrl = ImageUploader.Upload(model.ProofOfTemprature, out fileName, "Images/ProofFiles");
                        if (imgUrls != "")
                        {
                            imgUrls = imgUrls.Substring(0, imgUrls.Length - 1);
                        }
                        
                        if (imgUrl != "")
                        {
                            FumigationProofOfTemprature images = new FumigationProofOfTemprature();
                            images.FumigationRouteId = model.FumigationRouteId;

                            images.ImageUrl = imgUrl;
                            images.ImageName = fileName.ToString();
                            images.ActualTemperature = model.ActualTemperature;
                            images.DeliveryTemp = model.DeliveryTemp;
                            images.CreatedBy = objMemberProfile.UserId;
                            images.CreatedOn = Configurations.TodayDateTime;
                            ListProofOfTempDto.Add(images);
                            TempimgUrl = imgUrls;
                        }
                        //TempimgUrl = imgUrl;
                    }

                    #endregion

                    var result = fumigationBAL.UploadProofofTempDocument(ListProofOfTempDto);
                    if (result > 0)
                    {
                        objJsonResponse.Data = result;
                       
                            FumigationEmailDTO objfumigationEmail = new FumigationEmailDTO();
                            TemperatureEmailDTO objTemperatureEmail = new TemperatureEmailDTO();
                            objfumigationEmail.FumigationId = model.FumigationId;
                            if (model.FumigationId > 0)
                            {
                                var fumigationEmailDetail = fumigationBAL.GetCustomerDetail(objfumigationEmail);
                                var temperatureEmailDetail = fumigationBAL.GetTemperatureEmailDetail(model.FumigationRouteId);
                                if (fumigationEmailDetail != null)
                                {
                               // fumigationEmailDetail.PickUpArrival = PickUpArrival;
                                //string[] tempval = model.ActualTemperature.Split('.');
                                ActualTemp = Convert.ToDouble(model.ActualTemperature);
                                isEmail =  SendTemperatureMail(fumigationEmailDetail, temperatureEmailDetail, ActualTemp, TempimgUrl);
                                }

                            }
                        }
                    if (isEmail)
                    {
                        objJsonResponse.IsSuccess = result > 0;
                        ErrorLog("isEmail Success: "+ result + "Img URL: "+ TempimgUrl);
                        objJsonResponse.Message = objJsonResponse.IsSuccess ? LarastruckingResource.DataSaveSuccessfully : LarastruckingResource.SomethingWentWrong;
                        return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        objJsonResponse.IsSuccess = false;
                        ErrorLog("isEmail failure: " + result + "Img URL: " + TempimgUrl);
                        objJsonResponse.Message = "Unable to contact server and email could not be sent";
                        return Json(objJsonResponse, JsonRequestBehavior.AllowGet);

                    }
                   

                }
                else
                {
                    ErrorLog("Model State IsValid: false" );
                    objJsonResponse.IsSuccess = false;
                    objJsonResponse.Message = LarastruckingResource.SomethingWentWrong;
                    return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception exs)
            {
                ErrorLog("Model State IsValid: "+exs.Message);
                throw;
            }

        }
        #endregion

        #region Upload Damage Document
        /// <summary>
        ///Upload Damage Document
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadDamageDocument(FumigationDamageImages model)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();
                if (ModelState.IsValid)
                {
                    #region UPLOADED FILES: Damaged 
                    List<FumigationDamageImages> ListDamagedDto = new List<FumigationDamageImages>();
                    if (model.DamageImage != null)
                    {
                        foreach (var damagedUploadFile in model.DamageImage)
                        {
                            string damagedfileName = string.Empty;
                            var imgUrl = ImageUploader.Upload(damagedUploadFile, out damagedfileName, "Images/DamagedFiles");
                            if (!string.IsNullOrEmpty(imgUrl))
                            {
                                FumigationDamageImages Image = new FumigationDamageImages();
                                Image.FumigationRouteId = model.FumigationRouteId;
                                Image.ImageDescription = model.ImageDescription;
                                Image.ImageUrl = imgUrl;
                                Image.ImageName = damagedfileName;
                                Image.CreatedBy = objMemberProfile.UserId;
                                Image.CreatedOn = Configurations.TodayDateTime;
                                ListDamagedDto.Add(Image);
                            }
                        }
                    }

                    #endregion
                    var result = fumigationBAL.UploadDamageDocument(ListDamagedDto);
                    if (result > 0)

                    {
                        objJsonResponse.Data = result;
                    }
                    objJsonResponse.IsSuccess = result > 0;
                    objJsonResponse.Message = objJsonResponse.IsSuccess ? LarastruckingResource.DataSaveSuccessfully : LarastruckingResource.SomethingWentWrong;
                    return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    objJsonResponse.IsSuccess = false;
                    objJsonResponse.Message = LarastruckingResource.SomethingWentWrong;
                    return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        #region delete proof of temprature
        /// <summary>
        /// delete proof of temprature
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public ActionResult DeleteProofOfTemprature(int imageId)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();

                if (imageId > 0)
                {
                    FumigationProofOfTemprature objProofOfTemprature = new FumigationProofOfTemprature();
                    objProofOfTemprature.ProofOfTempratureId = imageId;
                    objProofOfTemprature.CreatedBy = objMemberProfile.UserId;
                    objProofOfTemprature.CreatedOn = Configurations.TodayDateTime;
                    objJsonResponse.IsSuccess = fumigationBAL.DeleteProofOfTemprature(objProofOfTemprature);
                    objJsonResponse.Message = objJsonResponse.IsSuccess ? LarastruckingResource.DataDeleteSuccessfully : LarastruckingResource.SomethingWentWrong;
                    return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    objJsonResponse.IsSuccess = false;
                    objJsonResponse.Message = LarastruckingResource.SomethingWentWrong;
                    return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region delete damage image
        /// <summary>
        ///delete damage image
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public ActionResult DeleteDamageImage(int damageId)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();

                if (damageId > 0)
                {
                    FumigationDamageImages objDamageImage = new FumigationDamageImages();
                    objDamageImage.DamageId = damageId;
                    objDamageImage.CreatedBy = objMemberProfile.UserId;
                    objDamageImage.CreatedOn = Configurations.TodayDateTime;
                    objJsonResponse.IsSuccess = fumigationBAL.DeleteDamageFile(objDamageImage);
                    objJsonResponse.Message = objJsonResponse.IsSuccess ? LarastruckingResource.DataDeleteSuccessfully : LarastruckingResource.SomethingWentWrong;
                    return Json(objJsonResponse, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    objJsonResponse.IsSuccess = false;
                    objJsonResponse.Message = LarastruckingResource.SomethingWentWrong;
                    return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Add Route Stops
        /// <summary>
        /// add route stops
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddRouteStops(GetFumigationRouteDTO model)
        {
            try
            {
                var fumigationRouteId = fumigationBAL.AddRouteStops(model);
                return Json(fumigationRouteId, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Approved fumigation
        /// <summary>
        /// Approved the fumigation if fumigation is on hold
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApprovedFumigation(FumigationDTO model)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();

                model.CreatedBy = objMemberProfile.UserId;
                model.CreatedOn = Configurations.TodayDateTime;
                if (model != null)
                {
                    var result = fumigationBAL.ApprovedFumigation(model);
                    objJsonResponse.IsSuccess = result.IsSuccess;
                    if (result.IsSuccess)
                    {
                        objJsonResponse.Message = LarastruckingResource.ApproveShipment;
                        FumigationEmailDTO objfumigationEmail = new FumigationEmailDTO();
                        objfumigationEmail.FumigationId = model.FumigationId;
                        if (model.FumigationId > 0)
                        {
                            var fumigationEmailDetail = fumigationBAL.GetCustomerDetail(objfumigationEmail);
                            if (fumigationEmailDetail != null)
                            {
                                SendFumigationMail(fumigationEmailDetail);
                            }

                        }

                        return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        objJsonResponse.Message = LarastruckingResource.SomethingWentWrong;
                        return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    objJsonResponse.IsSuccess = false;
                    objJsonResponse.Message = LarastruckingResource.SomethingWentWrong;
                    return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        #endregion

        #region SendFumigationMail
        /// <summary>
        /// Send Fumigation Mail
        /// </summary>
        /// <param name="model"></param>
        public void SendFumigationMail(FumigationEmailDTO model)
        {
            StringReader sr = null;
            Document pdfDoc = null;
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();

                if (model != null)
                {
                    var customerDetail = model;
                    string to = customerDetail.AllContactPerson;
                    string subject = string.Empty;
                    subject = LarastruckingResource.LarasFumigationAdvice;
                    bool IsValid = true;
                    if (!string.IsNullOrEmpty(customerDetail.AirWayBill))
                    {
                        subject = subject.Replace("@AWB/PO/ORD", customerDetail.AirWayBill);
                        customerDetail.AWBPoOrderNO = " AWB # " + customerDetail.AirWayBill;
                        IsValid = false;
                    }
                    if (IsValid && !string.IsNullOrEmpty(customerDetail.CustomerPO))
                    {
                        subject = subject.Replace("@AWB/PO/ORD", customerDetail.CustomerPO);
                        customerDetail.AWBPoOrderNO = " PO # " + customerDetail.CustomerPO;
                        IsValid = false;
                    }
                    if (IsValid && !string.IsNullOrEmpty(customerDetail.OrderNo))
                    {
                        subject = subject.Replace("@AWB/PO/ORD", customerDetail.OrderNo);
                        customerDetail.AWBPoOrderNO = " Order # " + customerDetail.OrderNo;
                        IsValid = false;
                    }
                    if (IsValid)
                    {
                        subject = subject.Replace("@AWB/PO/ORD", "");
                    }

                    subject = subject.Replace("@TRLR", customerDetail.Trailer);
                    subject = subject.Replace("@STATUS", customerDetail.Status);


                    string bodywithsignature = this.RenderViewToStringAsync<FumigationEmailDTO>("_FumigationStatusEmail", customerDetail) + LarastruckingResource.QuoteMailSignature;
                    sr = new StringReader(bodywithsignature);
                    pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    using (MemoryStream stream = new System.IO.MemoryStream())
                    {

                        byte[] bytes = new byte[] { };
                        Dictionary<byte[], string> fileAttach = new Dictionary<byte[], string>();
                        if (customerDetail.StatusId == 3)
                        {
                            if (customerDetail.DamageImages.Count > 0)
                            {
                                int demageNo = 1;
                                foreach (var damageImages in customerDetail.DamageImages)
                                {
                                    string path = damageImages.ImageUrl;
                                    using (var webClient = new WebClient())
                                    {
                                        bytes = webClient.DownloadData(path);
                                    }
                                    string fileName = "";
                                    if (customerDetail.DamageImages.Count == 1)
                                    {
                                        fileName = "Demage." + damageImages.Ext;
                                    }
                                    else
                                    {
                                        fileName = "Demage" + demageNo + "." + damageImages.Ext;
                                    }

                                    fileAttach.Add(bytes, fileName);
                                    demageNo = demageNo + 1;
                                }
                            }
                        }
                        else
                        {
                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                            pdfDoc.Open();
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                            pdfDoc.Close();
                            bytes = stream.ToArray();
                            string fileName = "Fumigation.pdf";
                            fileAttach.Add(bytes, fileName);
                        }

                        string mailSentResponse = string.Empty;
                        MailWithCCAttachDTO mailData = new MailWithCCAttachDTO();
                        mailData.FileByte = bytes;
                        mailData.MailPurpose = "Fumigation";
                        mailData.ToMail = to;
                        mailData.ToMailCC = string.Empty;
                        mailData.ToMailBCC = string.Empty;
                        mailData.MailSubject = subject;
                        mailData.MailBody = bodywithsignature;
                        mailData.strMailtype = string.Empty;
                        mailData.CreatedBy = objMemberProfile.UserId;
                        mailData.CreatedOn = Configurations.TodayDateTime;
                        var deliveryStatus = customerDetail.FumigationStatusHistory.Where(x => x.StatusId == 7).FirstOrDefault();
                        if (deliveryStatus == null && customerDetail.StatusId != 3)
                        {
                            fileAttach = null;
                        }
                        Email.SendMailWithCCAttach(mailData, out mailSentResponse, fileAttach);
                        ErrorLog("Mail Response FUM : " + mailSentResponse);
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                pdfDoc.Close();
                sr.Dispose();

            }
        }
        #endregion

        #region Approve Proof Of Temprature
        /// <summary>
        /// Approve Proof of Temprature
        /// </summary>
        /// <param name="ProofOfTempratureId"></param>
        /// <returns></returns>
        public ActionResult ApprovedProofOFTemp(int proofOfTempratureId)
        {
            MemberProfile memberProfile = new MemberProfile();
            ProofOfTemperatureDTO proofOfTemperatureDTO = new ProofOfTemperatureDTO();
            if (proofOfTempratureId > 0)
            {
                proofOfTemperatureDTO.ProofOfTempratureId = proofOfTempratureId;
                proofOfTemperatureDTO.ApprovedOn = Configurations.TodayDateTime;
                proofOfTemperatureDTO.ApprovedBy = memberProfile.UserId;
                var data = fumigationBAL.ApprovedProofOFTemp(proofOfTemperatureDTO);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Approve Damage Image
        /// <summary>
        /// Approve Damage Image
        /// </summary>
        /// <param name="ProofOfTempratureId"></param>
        /// <returns></returns>
        public ActionResult ApprovedDamageImage(int damageId)
        {
            MemberProfile memberProfile = new MemberProfile();
            FumigationDamageImages damageDocument = new FumigationDamageImages();
            if (damageId > 0)
            {
                damageDocument.DamageId = damageId;
                damageDocument.ApprovedOn = Configurations.TodayDateTime;
                damageDocument.ApprovedBy = memberProfile.UserId;
                var data = fumigationBAL.ApprovedDamageImage(damageDocument);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region view fumigation notification
        /// <summary>
        /// view fumigation notification
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ViewFumigationNotification(int id)
        {
            FumigationEmailDTO model = new FumigationEmailDTO();
            model.FumigationId = id;
            var fumigationEmailDetail = fumigationBAL.GetCustomerDetail(model);
            bool IsValid = true;
            if (!string.IsNullOrEmpty(fumigationEmailDetail.AirWayBill))
            {

                fumigationEmailDetail.AWBPoOrderNO = " AWB # " + fumigationEmailDetail.AirWayBill;
                IsValid = false;
            }
            if (IsValid && !string.IsNullOrEmpty(fumigationEmailDetail.CustomerPO))
            {
                fumigationEmailDetail.AWBPoOrderNO = " PO : " + fumigationEmailDetail.CustomerPO;
                IsValid = false;
            }
            if (IsValid && !string.IsNullOrEmpty(fumigationEmailDetail.OrderNo))
            {
                fumigationEmailDetail.AWBPoOrderNO = " Order : " + fumigationEmailDetail.OrderNo;
                IsValid = false;
            }


            return View(fumigationEmailDetail);
        }

        #endregion

        #region get fumigation detail by id for copy
        /// <summary>
        /// Get proof Of delivery
        /// </summary>
        /// <param name="fumigationId"></param>
        /// <returns></returns>
        public ActionResult GetFumigationDetailById(int fumigationId)
        {
            var fumigationList = fumigationBAL.GetFumigationDetailById(fumigationId);
            return Json(fumigationList, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Copy Fumigation
        /// <summary>
        /// Copy Fumigation detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveCopyFumigatonDetail(CopyFumigationDTO model)
        {
            MemberProfile objMemberProfile = new MemberProfile();
            try
            {
                if (model != null)
                {
                    model.CreatedBy = objMemberProfile.UserId;
                    model.CreatedDate = Configurations.TodayDateTime;
                }
                var IsSuccess = fumigationBAL.SaveCopyFumigatonDetail(model);
                return Json(IsSuccess, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion



        #region Get max route no.
        /// <summary>
        /// Get max route no.
        /// </summary>
        /// <param name="fumigationId"></param>
        /// <returns></returns>

        public ActionResult GetMaxRouteNo(int fumigationId)
        {
            int? maxRouteNo = fumigationBAL.GetMaxRouteNo(fumigationId);
            return Json(maxRouteNo, JsonRequestBehavior.AllowGet);

        }
        #endregion

        
        public bool SendTemperatureMail(FumigationEmailDTO model,TemperatureEmailDTO model1, double ActualTemp,string tempURL)
        {
            StringReader sr = null;
            Document pdfDoc = null;
            bool IsValid = false;
            bool isEmail = false;
            string ImgURL = WebConfigurationManager.AppSettings["ImageURL"].ToString();
            string MailCC = WebConfigurationManager.AppSettings["ProofTempCC"].ToString();
            string messageTemp = string.Empty;
            string ext = string.Empty;
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();

                if (model != null)
                {
                    var customerDetail = model;
                    var temperatureDetail = model1;
                    string to = customerDetail.AllContactPerson;
                    string subject = string.Empty;
                    subject = LarastruckingResource.LarasFumigationAdvice;
                    if (ActualTemp>40 && ActualTemp<=50)
                    {
                        messageTemp = "IMMEDIATE ATTENTION HIGH TEMPERATURE";
                        subject = "LARA'S FUMIGATION LOADING & TEMPERATURE REPORT | @AWB/PO/ORD | " + messageTemp;
                    }
                    else if (ActualTemp>50)
                    {
                        messageTemp = "IMMEDIATE ATTENTION EXTREMELY HIGH TEMPERATURE";
                        subject = "LARA'S FUMIGATION LOADING & TEMPERATURE REPORT | @AWB/PO/ORD | " + messageTemp;

                    }
                    else if (ActualTemp > 35 && ActualTemp <= 40)
                    {
                        messageTemp = "ACCEPTABLE TEMPERATURE";
                        subject = "LARA'S SHIPMENT LOADING & TEMPERATURE REPORT | @AWB/PO/ORD | " + messageTemp;

                    }
                    else
                    {
                        messageTemp = "POTENTIAL LOW TEMERATURE DAMAGE";
                        subject = "LARA'S SHIPMENT LOADING & TEMPERATURE REPORT | @AWB/PO/ORD | " + messageTemp;
                    }
                    if (!string.IsNullOrEmpty(temperatureDetail.AirWayBill))
                    {
                        subject = subject.Replace("@AWB/PO/ORD", temperatureDetail.AirWayBill);
                        temperatureDetail.AirWayBill = " AWB # " + temperatureDetail.AirWayBill;
                        IsValid = false;
                    }
                    if (IsValid && !string.IsNullOrEmpty(temperatureDetail.CustomerPO))
                    {
                        subject = subject.Replace("@AWB/PO/ORD", temperatureDetail.CustomerPO);
                        temperatureDetail.CustomerPO = " PO # " + temperatureDetail.CustomerPO;
                        IsValid = false;
                    }
                    if (IsValid && !string.IsNullOrEmpty(temperatureDetail.OrderNo))
                    {
                        subject = subject.Replace("@AWB/PO/ORD", temperatureDetail.OrderNo);
                        temperatureDetail.OrderNo = " Order # " + temperatureDetail.OrderNo;
                        IsValid = false;
                    }
                    if (IsValid)
                    {
                        subject = subject.Replace("@AWB/PO/ORD", "");
                    }
                    temperatureDetail.ActTemp = messageTemp;
                    temperatureDetail.ActTemperature = ActualTemp;
                    temperatureDetail.PickUpArrival = Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(temperatureDetail.PickUpArrival));
                    temperatureDetail.DriverPickUpArrival = Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(temperatureDetail.DriverPickUpArrival));
                    temperatureDetail.DriverLoadingStartTime = Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(temperatureDetail.DriverLoadingStartTime));
                    temperatureDetail.DriverLoadingFinishTime = Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(temperatureDetail.DriverLoadingFinishTime));
                    temperatureDetail.DriverFumigationIn = Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(temperatureDetail.DriverFumigationIn));

                    //subject = subject.Replace("@TRLR", customerDetail.Trailer);
                    //subject = subject.Replace("@STATUS", customerDetail.Status);


                    string bodywithsignature = this.RenderViewToStringAsync<TemperatureEmailDTO>("_TemperatureEmail", temperatureDetail);
                    sr = new StringReader(bodywithsignature);
                    pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

                   
                    using (MemoryStream stream = new System.IO.MemoryStream())
                    {

                        byte[] bytes = new byte[] { };
                        Dictionary<byte[], string> fileAttach = new Dictionary<byte[], string>();
                        var imgFiles = tempURL.Split('|');
                        for (int i = 0; i < imgFiles.Length; i++)
                        {
                            var tempImg = imgFiles[i];
                            string pathURL = tempImg.Replace("\\", "/");
                            string[] extUrl = pathURL.Split('.');
                            ext = extUrl[1];
                            string path = ImgURL + pathURL;
                            using (var webClient = new WebClient())
                            {
                                bytes = webClient.DownloadData(path);
                            }
                            string fileName = "proofoftemp_" + (i+1) + "." + ext;


                            fileAttach.Add(bytes, fileName);
                        }
                        string mailSentResponse = string.Empty;
                        MailWithCCAttachDTO mailData = new MailWithCCAttachDTO();
                        mailData.FileByte = bytes;
                        mailData.MailPurpose = "Fumigation";
                        mailData.ToMail = to;
                        mailData.ToMailCC = MailCC;
                        mailData.ToMailBCC = string.Empty;
                        mailData.MailSubject = subject;
                        mailData.MailBody = bodywithsignature;
                        mailData.strMailtype = string.Empty;
                        mailData.CreatedBy = objMemberProfile.UserId;
                        mailData.CreatedOn = Configurations.TodayDateTime;
                        var deliveryStatus = customerDetail.FumigationStatusHistory.Where(x => x.StatusId == 7).FirstOrDefault();
                        if (deliveryStatus == null && customerDetail.StatusId != 3)
                        {
                           // fileAttach = null;
                        }
                        //mailData.MailSubject = "LARA Test";
                        //mailData.MailBody = "Temperature report delivered";
                        //ErrorLog("Mail DATA : " + mailData.MailBody);
                        ErrorLog("fileAttach : " + fileAttach.ToString());
                        if (mailData.MailBody != "")
                        {
                            //IsValid = true;
                            isEmail = true;
                            Email.SendMailWithCCAttach(mailData, out mailSentResponse, fileAttach);
                            ErrorLog("Mail Response : " + mailSentResponse);
                        }
                        //isEmail = true;


                    }

                }

                return isEmail;

            }
            catch (Exception ex)
            {
                // throw;
                ErrorLog("Mail error : "+ex.Message);
                return isEmail;
            }
            finally
            {
                pdfDoc.Close();
                sr.Dispose();

            }
        }

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
            string path = System.Web.HttpContext.Current.Server.MapPath("../../Assets/ErrorLog");
            StreamWriter sw = new StreamWriter(path + sErrorTime, true);
            sw.WriteLine(sLogFormat + sErrMsg);
            sw.Flush();
            sw.Close();
        }
    }
}
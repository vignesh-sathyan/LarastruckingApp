using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.Controllers;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Enum;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Infrastructure;
using LarastruckingApp.Log.Utility;
using LarastruckingApp.Resource;
using LarastruckingApp.Utility;
using LarastruckingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;

namespace LarastruckingApp.Areas.Shipment.Controllers
{

    [Authorize]
    public class ShipmentController : BaseController
    {

        #region Private Member
        /// <summary>
        /// Defining private members
        /// </summary>
        private readonly IShipmentBAL shipmentBAL = null;
        private readonly IAddressBAL addressBAL;
        private readonly IDriverBAL driverBAL;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iShipmentBAL"></param>
        public ShipmentController(IShipmentBAL iShipmentBAL, IAddressBAL iAddressBAL, IDriverBAL iDriverBAL)
        {
            shipmentBAL = iShipmentBAL;
            addressBAL = iAddressBAL;
            driverBAL = iDriverBAL;
        }
        #endregion

        // GET: Shipment/Shipment
        /// <summary>
        /// default method for page load
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        
        [CustomAuthorize]
        public ActionResult Index(int id = 0)
        {
            IEnumerable<SelectListItem> WeightUnit = from Unit qt in Enum.GetValues(typeof(Unit))
                                                     select new SelectListItem
                                                     {
                                                         Text = qt.ToString(),
                                                         Value = Convert.ToInt32(qt).ToString()

                                                     };
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
                var statusList = shipmentBAL.GetStatusList();
                return Json(statusList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetFreightType
        /// <summary>
        /// get freight type
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFreightType(CustomernNRouteInfoDTO model)
        {
            try
            {
                var freightTypeList = shipmentBAL.getFreightType(model);
                return Json(freightTypeList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region BindFreightType
        /// <summary>
        /// get freight type
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BindFreightType()
        {
            try
            {
                var freightTypeList = shipmentBAL.bindFreightType();
                return Json(freightTypeList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region ShipmenetIsReady
        /// <summary>
        /// get freight type
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ShipmentIsReady(ShipmentIsReadyDTO model)
        {
            try
            {
                var IsSuccess = shipmentBAL.ShipmentIsReady(model.shipmentId, model.ready);
                return Json(IsSuccess, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        [HttpPost]
        public ActionResult ShipmentWTReady(ShipmentIsReadyDTO model)
        {
            try
            {
                var IsSuccess = shipmentBAL.ShipmentWTReady(model.shipmentId, model.ready);
                return Json(IsSuccess, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        public ActionResult ShipmentSTReady(ShipmentIsReadyDTO model)
        {
            try
            {
                var IsSuccess = shipmentBAL.ShipmentSTReady(model.shipmentId, model.ready);
                return Json(IsSuccess, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region GetPricingMethod
        /// <summary>
        /// get pricing method
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPricingMethod(CustomernNRouteInfoDTO model)
        {
            try
            {
                var pricingMethodList = shipmentBAL.getPricingMethod(model);
                return Json(pricingMethodList, JsonRequestBehavior.AllowGet);
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

        #region EquipmentList
        /// <summary>
        /// get equipment list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EquipmentList()
        {
            return PartialView("_prEquipmentList");
        }

        #endregion

        #region LoadData
        /// <summary>
        /// Load J query Data Grid
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadData(ValidateDriverNEquipmentDTO model)
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
                var lstEquipmentDTO = shipmentBAL.EquipmnetList(model);
                if (!string.IsNullOrEmpty(search))
                {
                    lstEquipmentDTO = lstEquipmentDTO.Where(x => x.VehicleType.ToUpper().Contains(search.ToUpper()) ||
                                                                    x.EquipmentNo.Contains(search.ToUpper()) ||
                                                                    x.EquipmentNo.ToUpper().Contains(search.ToUpper()) ||
                                                                    x.MaxLoad.ToUpper().Contains(search.ToUpper()) ||
                                                                    x.Bed.ToUpper().Contains(search.ToUpper())).ToList();
                }
                recordsTotal = lstEquipmentDTO.Count();
                var data = lstEquipmentDTO.ToList();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    data = sortColumnDir == "asc" ? data.OrderBy(x => x.GetType().GetProperty(sortColumn).GetValue(x, null)).ToList() : data.OrderByDescending(x => x.GetType().GetProperty(sortColumn).GetValue(x, null)).ToList();
                    data = data.Skip(skip).Take(pageSize).ToList();
                }
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region LoadData
        /// <summary>
        /// Load J query Data Grid
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LoadAllData(ValidateDriverNEquipmentDTO model)
        {
            try
            {
                string search = "";
                var draw = 1;
               // var start = Request.Form.GetValues("start").FirstOrDefault();
                //var length = Request.Form.GetValues("length").FirstOrDefault();

                // Find Order Column
                var sortColumn = "";
                var sortColumnDir = "asc";
                //int pageSize = length != null ? Convert.ToInt32(length) : 0;
                //int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var lstEquipmentDTO = shipmentBAL.EquipmnetList(model);
                if (!string.IsNullOrEmpty(search))
                {
                    lstEquipmentDTO = lstEquipmentDTO.Where(x => x.VehicleType.ToUpper().Contains(search.ToUpper()) ||
                                                                    x.EquipmentNo.Contains(search.ToUpper()) ||
                                                                    x.EquipmentNo.ToUpper().Contains(search.ToUpper()) ||
                                                                    x.MaxLoad.ToUpper().Contains(search.ToUpper()) ||
                                                                    x.Bed.ToUpper().Contains(search.ToUpper())).ToList();
                }
                recordsTotal = lstEquipmentDTO.Count();
                var data = lstEquipmentDTO.ToList();
                List<ShipmentEquipmentDTO> shipmentEquipmentList = new List<ShipmentEquipmentDTO>();
                
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    //data = sortColumnDir == "asc" ? data.OrderBy(x => x.GetType().GetProperty(sortColumn).GetValue(x, null)).ToList() : data.OrderByDescending(x => x.GetType().GetProperty(sortColumn).GetValue(x, null)).ToList();
                    data = data.Skip(0).Take(lstEquipmentDTO.Count).ToList();
                    shipmentEquipmentList = data;
                }
                return Json(shipmentEquipmentList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region bind driver dropdown
        /// <summary>
        /// get driver list
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDriverList(ValidateDriverNEquipmentDTO model)
        {
            var driverlist = shipmentBAL.DriverList(model);
            return Json(driverlist, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region get all driver dropdown
        /// <summary>
        /// get driver list
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllDriver()
        {
            var driverlist = shipmentBAL.DriverList();
            return Json(driverlist, JsonRequestBehavior.AllowGet);
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

        #region save shipment detail
        public ActionResult CreateShipment(ShipmentDTO model)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();

                model.CreatedBy = objMemberProfile.UserId;
                if (model != null)
                {
                    var result = shipmentBAL.Add(model);
                    objJsonResponse.IsSuccess = result.IsSuccess;

                    if (result.IsSuccess)
                    {

                        objJsonResponse.Message = LarastruckingResource.DataSaveSuccessfully;
                        ShipmentEmailDTO customerShipmentDTO = new ShipmentEmailDTO();
                        customerShipmentDTO.CustomerId = Convert.ToInt32(model.CustomerId);
                        customerShipmentDTO.ShipmentId = model.ShipmentId;
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

        public void SendMail(ShipmentEmailDTO model)
        {
            StringReader sr = null;
            Document pdfDoc = null;
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();

                var customerDetail = shipmentBAL.GetCustomerDetail(model);
                if (customerDetail != null)
                {
                    string to = customerDetail.AllContactPerson;
                    string subject = string.Empty;
                    subject = LarastruckingResource.ShipmentStatusChanges;
                    bool IsValid = true;
                    if (!string.IsNullOrEmpty(customerDetail.AirWayBill))
                    {
                        subject = subject.Replace("@AWB", customerDetail.AirWayBill);
                        customerDetail.AWBPoOrderNO = " AWB # " + customerDetail.AirWayBill;
                        IsValid = false;
                    }
                    if (IsValid && !string.IsNullOrEmpty(customerDetail.CustomerPO))
                    {
                        subject = subject.Replace("@AWB", customerDetail.CustomerPO);
                        customerDetail.AWBPoOrderNO = " PO # " + customerDetail.CustomerPO;
                        IsValid = false;
                    }
                    if (IsValid && !string.IsNullOrEmpty(customerDetail.OrderNo))
                    {
                        subject = subject.Replace("@AWB", customerDetail.OrderNo);
                        customerDetail.AWBPoOrderNO = " Order # " + customerDetail.OrderNo;
                        IsValid = false;
                    }
                    if (IsValid)
                    {
                        subject = subject.Replace("@AWB", "");
                    }

                    subject = subject.Replace("@STATUS", customerDetail.Status);
                    string bodywithsignature = this.RenderViewToStringAsync<ShipmentEmailDTO>("_ShipmentStatusEmail", customerDetail) + LarastruckingResource.QuoteMailSignature;
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
                                int damage = 1;
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
                                        fileName = "Demage" + damage + "." + damageImages.Ext;
                                    }
                                    fileAttach.Add(bytes, fileName);
                                    damage = damage + 1;
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
                            string fileName = "shipment.pdf";

                            fileAttach.Add(bytes, fileName);
                        }
                        string mailSentResponse = string.Empty;
                        MailWithCCAttachDTO mailData = new MailWithCCAttachDTO();
                        mailData.FileByte = bytes;
                        mailData.MailPurpose = "Shipment";
                        mailData.ToMail = to;
                        mailData.ToMailCC = string.Empty;
                        mailData.ToMailBCC = string.Empty;
                        mailData.MailSubject = subject;
                        mailData.MailBody = bodywithsignature;
                        mailData.strMailtype = string.Empty;
                        mailData.CreatedBy = objMemberProfile.UserId;
                        mailData.CreatedOn = Configurations.TodayDateTime;
                        var deliveryStatus = customerDetail.ShipmentStatusHistory.Where(x => x.StatusId == 7).FirstOrDefault();
                        if (deliveryStatus == null && customerDetail.StatusId != 3)
                        {
                            fileAttach = null;
                        }
                        Email.SendMailWithCCAttach(mailData, out mailSentResponse, fileAttach);

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

        #region validate Route
        /// <summary>
        /// validate route stops from Quote
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidateRouteStops(ValidateRouteStopDTO model)
        {
            try
            {
                var response = shipmentBAL.ValidateRouteStop(model);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }


        }
        #endregion

        #region  ViewShipmentList
        /// <summary>
        /// View Shipment 
        /// </summary>
        /// <returns></returns>

       
        [CustomAuthorize]
        public ActionResult ViewShipmentList()
        {
            return View();
        }
        #endregion

        #region View shipment list
        /// <summary>
        /// acction method for bind quote table
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ViewShipment(AllShipmentDTO model)
        {
            try
            {
                string search = Request.Form.GetValues("search[value]").FirstOrDefault();
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                 string sortColumns = null;
                string sortOrder = null;
                //string[] soretedColumns = null;
                List<string> soretedColumns = new List<string>();
               
                int columnLength = int.Parse(draw);
                // Find Order Column
                for (int i = 0; i < columnLength; i++)
                {
                    var formVal = Request.Form.GetValues("order[" + i + "][column]");
                    var orderVal = Request.Form.GetValues("order[" + i + "][dir]");
                    var orderVals = Request.Form.GetValues("order[0][dir]");

                    if (formVal != null)
                    {
                        
                       sortColumns = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[" + i + "][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                        sortOrder = Request.Form.GetValues("order[" + i + "][dir]").FirstOrDefault();
                        soretedColumns.Add(sortColumns);
                    }
                   
                   
                }

                String[] str = soretedColumns.ToArray();
                string sorted = "";
                if (str.Length>=2)
                {
                    sorted = string.Join(",", str);
                    sorted = sorted.Substring(0, sorted.LastIndexOf(","));
                }
                else
                {
                    sorted = "null";
                }
            
                
                var sortColumn = sortColumns;

                    var sortColumnDir = sortOrder;


                    int pageSize = length != null ? Convert.ToInt32(length) : 0;
                    int skip = start != null ? Convert.ToInt32(start) : 0;

                    int recordsTotal = 0;

                    model.PageSize = pageSize;
                    model.PageNumber = skip;
                    model.SearchTerm = search;
                    model.SortColumn = sortColumn;
                    model.SortOrder = sortColumnDir;
                    model.TotalCount = recordsTotal;
                    model.SortedColumns = sorted;


                    IEnumerable<AllShipmentList> shipmentList = await shipmentBAL.ViewShipmentList(model);
                    var data = shipmentList.ToList();
                    recordsTotal = data.Count > 0 ? data.Select(x => x.TotalCount).FirstOrDefault() : 0;

                   
                
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
               
                
            
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region View shipment Progress list
        /// <summary>
        /// acction method for bind quote table
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ViewShipmentProgress(AllShipmentDTO model)
        {
            try
            {
                string search = Request.Form.GetValues("search[value]").FirstOrDefault();
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                string sortColumns = null;
                string sortOrder = null;
                //string[] soretedColumns = null;
                List<string> soretedColumns = new List<string>();

                int columnLength = int.Parse(draw);
                // Find Order Column
                for (int i = 0; i < columnLength; i++)
                {
                    var formVal = Request.Form.GetValues("order[" + i + "][column]");
                    var orderVal = Request.Form.GetValues("order[" + i + "][dir]");

                    if (formVal != null)
                    {

                        sortColumns = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[" + i + "][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                        sortOrder = Request.Form.GetValues("order[" + i + "][dir]").FirstOrDefault();
                        soretedColumns.Add(sortColumns);
                    }


                }

                String[] str = soretedColumns.ToArray();
                string sorted = "";
                if (str.Length >= 2)
                {
                    sorted = string.Join(",", str);
                    sorted = sorted.Substring(0, sorted.LastIndexOf(","));
                }
                else
                {
                    sorted = "null";
                }


                var sortColumn = sortColumns;

                //var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var sortColumnDir = sortOrder;


                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                model.PageSize = pageSize;
                model.PageNumber = skip;
                model.SearchTerm = search;
                model.SortColumn = sortColumn;
                model.SortOrder = sortColumnDir;
                model.TotalCount = recordsTotal;
                model.SortedColumns = sorted;


                IEnumerable<AllShipmentList> shipmentList = await shipmentBAL.ViewShipmentList(model);
                var data = shipmentList.ToList();
                recordsTotal = data.Count > 0 ? data.Select(x => x.TotalCount).FirstOrDefault() : 0;



                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }



            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region GetShipmentById
        /// <summary>
        /// get shipment detail by id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetShipmentById(int shipmentId)
        {
            var result = shipmentBAL.GetShipmentById(shipmentId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region edit shipment
        /// <summary>
        /// edit shipment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult EditShipment(GetShipmentDTO model)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();

                model.CreatedBy = objMemberProfile.UserId;
                model.CreatedDate = Configurations.TodayDateTime;
                if (model != null)
                {
                    var result = shipmentBAL.EditShipment(model);
                    objJsonResponse.IsSuccess = result.IsSuccess;
                    if (result.IsSuccess)
                    {
                        ShipmentEmailDTO customerShipmentDTO = new ShipmentEmailDTO();
                        customerShipmentDTO.CustomerId = Convert.ToInt32(model.CustomerId);
                        customerShipmentDTO.ShipmentId = model.ShipmentId;
                        //result.IsMailNeedToSend = true;
                        if (result.IsMailNeedToSend)
                        {
                            SendMail(customerShipmentDTO);
                           // Email.SendMessage();

                        }
                        objJsonResponse.Message = LarastruckingResource.DataUpdateSuccessfully;
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
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Upload Damage Document
        /// <summary>
        ///Upload Damage Document
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadDamageDocument(GetDamageImages model)
        {

            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();
                if (ModelState.IsValid)
                {

                    #region UPLOADED FILES: Damaged 
                    List<GetDamageImages> ListDamagedDto = new List<GetDamageImages>();
                    if (model.DamageImage != null)
                    {
                        foreach (var damagedUploadFile in model.DamageImage)
                        {
                            string damagedfileName = string.Empty;
                            var imgUrl = ImageUploader.Upload(damagedUploadFile, out damagedfileName, "Images/DamagedFiles");
                            if (imgUrl != null)
                            {
                                GetDamageImages Image = new GetDamageImages();
                                Image.ShipmentRouteStopId = model.ShipmentRouteStopId;
                                Image.ImageDescription = model.ImageDescription; //"Damaged";
                                Image.ImageUrl = imgUrl;
                                Image.ImageName = damagedfileName;
                                Image.CreatedBy = objMemberProfile.UserId;
                                Image.CreatedOn = Configurations.TodayDateTime;
                                ListDamagedDto.Add(Image);
                            }
                        }
                    }

                    #endregion


                    var result = shipmentBAL.UploadDamageDocument(ListDamagedDto);
                    if (result > 0)

                    {
                        objJsonResponse.Data = result;
                        objJsonResponse.IsSuccess = true;
                        objJsonResponse.Message = LarastruckingResource.DataSaveSuccessfully;
                        return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        objJsonResponse.IsSuccess = false;
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

        #region Upload proof of temperature
        /// <summary>
        ///Upload proof of temperature
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadProofofTemperature(GetProofOfTemprature model)
        {
            try
            {
                var TempimgUrl = "";
                bool isEmail = false;
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();
                double ActualTemp = 0.00;

                if (ModelState.IsValid)
                {
                    #region UPLOADED FILES : PROOF OF TEMP
                    List<GetProofOfTemprature> ListProofOfTempDto = new List<GetProofOfTemprature>();
                    if (model != null)
                    {
                        string fileName = string.Empty;
                        //var imgUrl = ImageUploader.Upload(model.ProofOfTemprature, out fileName, "Images/ProofFiles");

                      
                        var tempFiles = model.filesObj;
                        var imgUrl = "";
                        var imgUrls = "";
                        for (int i = 0; i < model.filesObj.Length; i++)
                        {
                            var tempFile = model.filesObj.GetValue(i);
                            if (tempFile != null)
                            {
                                model.ProofOfTemprature = tempFile as HttpPostedFileBase;
                                imgUrl = ImageUploader.Upload(model.ProofOfTemprature, out fileName, "Images/ProofFiles", "proof_of_Temp_" + (i + 1));
                                imgUrls = imgUrls + imgUrl + "|";
                                Console.WriteLine("tempFile: " + tempFile);
                            }
                        }
                        //var imgUrl = ImageUploader.Upload(model.ProofOfTemprature, out fileName, "Images/ProofFiles");
                        if (imgUrls != "")
                        {
                            imgUrls = imgUrls.Substring(0, imgUrls.Length - 1);
                        }

                        if (imgUrl != null)
                        {
                            GetProofOfTemprature images = new GetProofOfTemprature();
                            images.ShipmentRouteStopId = model.ShipmentRouteStopId;
                            images.FreightDetailId = model.FreightDetailId;
                            images.ImageUrl = imgUrl;
                            images.ImageName = fileName;
                            images.ActualTemperature = model.ActualTemperature;
                            images.CreatedBy = objMemberProfile.UserId;
                            images.CreatedOn = Configurations.TodayDateTime;
                            ListProofOfTempDto.Add(images);
                            TempimgUrl = imgUrls;
                        }
                    }

                    #endregion

                    var result = shipmentBAL.UploadProofofTempDocument(ListProofOfTempDto);
                    //if (result > 0)

                    //{
                    //    objJsonResponse.Data = result;
                    //    objJsonResponse.IsSuccess = true;
                    //    objJsonResponse.Message = LarastruckingResource.DataSaveSuccessfully;
                    //    return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                    //}
                    if (result > 0)
                    {
                       // objJsonResponse.Data = result;

                        ShipmentEmailDTO objfumigationEmail = new ShipmentEmailDTO();
                        TemperatureEmailSipmentDTO objTemperatureEmail = new TemperatureEmailSipmentDTO();
                        objfumigationEmail.ShipmentId = model.ShipmentId;
                        if (model.ShipmentId > 0)
                        {
                            var shipmentEmailDetail = shipmentBAL.GetCustomerDetail(objfumigationEmail);
                            var temperatureEmailDetail = shipmentBAL.GetTemperatureEmailDetail(model.ShipmentId);
                            if (shipmentEmailDetail != null)
                            {
                                // fumigationEmailDetail.PickUpArrival = PickUpArrival;
                                //string[] tempval = model.ActualTemperature.Split('.');
                                ActualTemp = Convert.ToDouble(model.ActualTemperature);
                                isEmail = SendTemperatureMail(shipmentEmailDetail, temperatureEmailDetail, ActualTemp, TempimgUrl);
                            }
                            objJsonResponse.Data = result;
                            objJsonResponse.IsSuccess = true;
                            objJsonResponse.Message = LarastruckingResource.DataSaveSuccessfully;
                            return Json(objJsonResponse, JsonRequestBehavior.AllowGet);

                        }
                    }
                    else
                    {
                        objJsonResponse.IsSuccess = false;
                        objJsonResponse.Message = LarastruckingResource.SomethingWentWrong;
                        return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                    }
                    if (isEmail)
                    {
                        objJsonResponse.IsSuccess = result > 0;
                        ErrorLog("isEmail Success: " + result + "Img URL: " + TempimgUrl);
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
        public ActionResult AddRouteStops(GetShipmentRouteStopDTO model)
        {
            try
            {
                var result = shipmentBAL.AddRouteStops(model);
                return Json(result.ShipmentRouteStopId, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Add Freight Detail
        /// <summary>
        /// Add freight detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AddFreightDetail(GetShipmentFreightDetailDTO model)
        {
            try
            {
                var result = shipmentBAL.AddFreightDetail(model);
                return Json(result.ShipmentFreightDetailId, JsonRequestBehavior.AllowGet);
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
                    GetProofOfTemprature objProofOfTemprature = new GetProofOfTemprature();
                    objProofOfTemprature.ProofOfTempratureId = imageId;
                    objProofOfTemprature.CreatedBy = objMemberProfile.UserId;
                    objProofOfTemprature.CreatedOn = Configurations.TodayDateTime;
                    if (shipmentBAL.DeleteProofOfTemprature(objProofOfTemprature))
                    {
                        objJsonResponse.IsSuccess = true;
                        objJsonResponse.Message = LarastruckingResource.DataDeleteSuccessfully;
                        return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        objJsonResponse.IsSuccess = false;
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

        #region delete damage image
        /// <summary>
        ///delete damage image
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public ActionResult DeleteDamageImage(int DamageId)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();

                if (DamageId > 0)
                {
                    GetDamageImages objDamageImage = new GetDamageImages();
                    objDamageImage.DamageId = DamageId;
                    objDamageImage.CreatedBy = objMemberProfile.UserId;
                    objDamageImage.CreatedOn = Configurations.TodayDateTime;
                    if (shipmentBAL.DeleteDamageFile(objDamageImage))
                    {
                        objJsonResponse.IsSuccess = true;
                        objJsonResponse.Message = LarastruckingResource.DataDeleteSuccessfully;
                        return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        objJsonResponse.IsSuccess = false;
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

        #region Delete shipment
        /// <summary>
        ///Delete shipment By Id
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        public ActionResult DeleteShipment(int shipmentId)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();
                ShipmentDTO objShipment = new ShipmentDTO();
                objShipment.ShipmentId = shipmentId;
                objShipment.CreatedBy = objMemberProfile.UserId;
                objShipment.CreatedDate = Configurations.TodayDateTime;
                if (shipmentBAL.DeleteShipment(objShipment))
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

        #region Approved shipment
        /// <summary>
        /// Approved the shipment if shipment is on hold
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApprovedShipment(ShipmentDTO model)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();

                model.CreatedBy = objMemberProfile.UserId;
                model.CreatedDate = Configurations.TodayDateTime;
                if (model != null)
                {
                    var result = shipmentBAL.ApprovedShipment(model);
                    objJsonResponse.IsSuccess = result.IsSuccess;
                    if (result.IsSuccess)
                    {
                        objJsonResponse.Message = LarastruckingResource.ApproveShipment;
                        ShipmentEmailDTO customerShipmentDTO = new ShipmentEmailDTO();
                        customerShipmentDTO.ShipmentId = model.ShipmentId;
                        SendMail(customerShipmentDTO);
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

        #region View all shipment
        /// <summary>
        /// View all shipment
        /// </summary>
        /// <returns></returns>

       
        [CustomAuthorize]

        public ActionResult ViewAllShipment()
        {
            return View();
        }
        #endregion

        #region View all shipment
        /// <summary>
        /// View all shipment
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ViewAllShipment(ViewShipmentDTO model)
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

                model.search = search;
                model.sortColumn = sortColumn;
                model.sortColumnDir = sortColumnDir;
                model.skip = skip;
                model.pageSize = pageSize;

                IEnumerable<ViewShipmentListDTO> shipmentList = shipmentBAL.ViewAllShipmentList(model, out recordsTotal);
                var data = shipmentList.ToList();

                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region All Shipment List
        /// <summary>
        /// Get All Shipment
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AllShipmentList(AllShipmentDTO model)
        {
            try
            {

                string search = Request.Form.GetValues(Configurations.SearchValue).FirstOrDefault();
                var draw = Request.Form.GetValues(Configurations.Draw).FirstOrDefault();
                var start = Request.Form.GetValues(Configurations.Start).FirstOrDefault();
                var length = Request.Form.GetValues(Configurations.Length).FirstOrDefault();

                string sortColumns = null;
                string sortOrder = null;
                //string[] soretedColumns = null;
                List<string> soretedColumns = new List<string>();

                int columnLength = int.Parse(draw);
                // Find Order Column
                for (int i = 0; i < columnLength; i++)
                {
                    var formVal = Request.Form.GetValues("order[" + i + "][column]");
                    var orderVal = Request.Form.GetValues("order[" + i + "][dir]");

                    if (formVal != null)
                    {

                        sortColumns = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[" + i + "][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                        sortOrder = Request.Form.GetValues("order[" + i + "][dir]").FirstOrDefault();
                        soretedColumns.Add(sortColumns);
                    }


                }

                String[] str = soretedColumns.ToArray();
                string sorted = "";
                if (str.Length >= 2)
                {
                    sorted = string.Join(",", str);
                    sorted = sorted.Substring(0, sorted.LastIndexOf(","));
                }
                else
                {
                    sorted = "null";
                }


                var sortColumn = sortColumns;

                //var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var sortColumnDir = sortOrder;

                // Find Order Column
                //var sortColumn = Request.Form.GetValues(Configurations.Columns + Request.Form.GetValues(Configurations.OrderColumn).FirstOrDefault() + Configurations.Name).FirstOrDefault();
                //var sortColumnDir = Request.Form.GetValues(Configurations.OrderDir).FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                //AllShipmentDTO dto = new AllShipmentDTO()
                //
                // AllShipmentDTO model = new AllShipmentDTO();
                model.PageSize = pageSize;
                model.PageNumber = skip;
                model.SearchTerm = search;
                model.SortColumn = sortColumn;
                model.SortedColumns = sorted;
                model.SortOrder = sortColumnDir;
                model.TotalCount = recordsTotal;
                //};

                var preTripInfo = shipmentBAL.AllShipmentList(model);


                return Json(new { draw = draw, recordsFiltered = model.TotalCount, recordsTotal = model.TotalCount, data = preTripInfo }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                throw;
            }


        }
        #endregion

        #region Approve Proof Of Temprature
        /// <summary>
        /// Approve Proof of Temprature
        /// </summary>
        /// <param name="ProofOfTempratureId"></param>
        /// <returns></returns>
        public ActionResult ApprovedProofOFTemp(int ProofOfTempratureId)
        {
            MemberProfile memberProfile = new MemberProfile();
            GetProofOfTemprature proofOfTemperatureDTO = new GetProofOfTemprature();
            if (ProofOfTempratureId > 0)
            {
                proofOfTemperatureDTO.ProofOfTempratureId = ProofOfTempratureId;
                proofOfTemperatureDTO.ApprovedOn = Configurations.TodayDateTime;
                proofOfTemperatureDTO.ApprovedBy = memberProfile.UserId;
                var data = shipmentBAL.ApprovedProofOFTemp(proofOfTemperatureDTO);
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
            GetDamageImages damageDocument = new GetDamageImages();
            if (damageId > 0)
            {
                damageDocument.DamageId = damageId;
                damageDocument.ApprovedOn = Configurations.TodayDateTime;
                damageDocument.ApprovedBy = memberProfile.UserId;
                var data = shipmentBAL.ApprovedDamageImage(damageDocument);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region validate equipment and driver
        /// <summary>
        ///validate equipment and driver
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidateEquipmentNDriver(ValidateDriverNEquipmentDTO model)
        {
            var data = shipmentBAL.ValidateEquipmentNDriver(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region validate equipment 
        /// <summary>
        ///validate equipment 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public ActionResult ValidateEquipment(ValidateDriverNEquipmentDTO model)
        {
            var data = shipmentBAL.ValidateEquipment(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region validate  driver
        /// <summary>
        ///validate driver
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidateDriver(ValidateDriverNEquipmentDTO model)
        {
            var data = shipmentBAL.ValidateDriver(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Shipment Detail By Id
        /// <summary>
        ///  Get Shipment Detail By Id
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCopyShipmentDetailById(int shipmentId)
        {

            var shipmentdetail = shipmentBAL.GetCopyShipmentDetailById(shipmentId);
            return Json(shipmentdetail, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Copy Shipment
        /// <summary>
        /// Copy shipment detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveCopyShipmentDetail(CopyShipmentDTO model)
        {
            MemberProfile objMemberProfile = new MemberProfile();
            try
            {
                if (model != null)
                {
                    model.CreatedBy = objMemberProfile.UserId;
                    model.CreatedDate = Configurations.TodayDateTime;
                }
                var IsSuccess = shipmentBAL.SaveCopyShipmentDetail(model);
                return Json(IsSuccess, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion

        #region view shipment notification
        /// <summary>
        /// view shipment notification
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ViewShipmentNotification(int id)
        {
            ShipmentEmailDTO model = new ShipmentEmailDTO();
            model.ShipmentId = id;

            var customerDetail = shipmentBAL.GetCustomerDetail(model);
            bool IsValid = true;
            if (!string.IsNullOrEmpty(customerDetail.AirWayBill))
            {

                customerDetail.AWBPoOrderNO = " AWB # " + customerDetail.AirWayBill;
                IsValid = false;
            }
            if (IsValid && !string.IsNullOrEmpty(customerDetail.CustomerPO))
            {

                customerDetail.AWBPoOrderNO = " PO # " + customerDetail.CustomerPO;
                IsValid = false;
            }
            if (IsValid && !string.IsNullOrEmpty(customerDetail.OrderNo))
            {

                customerDetail.AWBPoOrderNO = " Order # " + customerDetail.OrderNo;
                IsValid = false;
            }

            return View(customerDetail);
        }
        #endregion


        #region view shipment notification
        /// <summary>
        /// view shipment notification
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShipmentNotificationMaster(int id)
        {
            ShipmentEmailDTO model = new ShipmentEmailDTO();
            model.ShipmentId = id;

            var customerDetail = shipmentBAL.GetCustomerDetail(model);
            bool IsValid = true;
            if (!string.IsNullOrEmpty(customerDetail.AirWayBill))
            {

                customerDetail.AWBPoOrderNO = " AWB # " + customerDetail.AirWayBill;
                IsValid = false;
            }
            if (IsValid && !string.IsNullOrEmpty(customerDetail.CustomerPO))
            {

                customerDetail.AWBPoOrderNO = " PO # " + customerDetail.CustomerPO;
                IsValid = false;
            }
            if (IsValid && !string.IsNullOrEmpty(customerDetail.OrderNo))
            {

                customerDetail.AWBPoOrderNO = " Order # " + customerDetail.OrderNo;
                IsValid = false;
            }

            return View(customerDetail);
        }
        #endregion

        #region Get max route no.
        /// <summary>
        /// Get max route no.
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>

        public ActionResult GetMaxRouteNo(int shipmentId)
        {
            int? maxRouteNo = shipmentBAL.GetMaxRouteNo(shipmentId);
            return Json(maxRouteNo, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Get Driver Check In time
        /// <summary>
        /// Get Driver Check In time
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCheckInTime(int driverId)
        {
            var result = shipmentBAL.GetCheckInTime(driverId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [HttpGet]
        public ActionResult DriverPhone(int driverid)
        {
            var driverlist = shipmentBAL.DriverPhone(driverid);
            return Json(driverlist, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CustomerName(int CustomerID)
        {
            var customer = shipmentBAL.CustomerName(CustomerID);
            return Json(customer, JsonRequestBehavior.AllowGet);
        }

        public bool SendTemperatureMail(ShipmentEmailDTO model, TemperatureEmailSipmentDTO model1, double ActualTemp, string tempURL)
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
                    if (ActualTemp > 40 && ActualTemp <= 50)
                    {
                        messageTemp = "IMMEDIATE ATTENTION HIGH TEMPERATURE";
                        subject = "LARA'S SHIPMENT LOADING & TEMPERATURE REPORT | @AWB/PO/ORD | " + messageTemp;
                    }
                    else if (ActualTemp > 50)
                    {
                        messageTemp = "IMMEDIATE ATTENTION EXTREMELY HIGH TEMPERATURE";
                        subject = "LARA'S SHIPMENT LOADING & TEMPERATURE REPORT | @AWB/PO/ORD | " + messageTemp;

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
                    if (!string.IsNullOrEmpty(temperatureDetail.CustomerPO))
                    {
                        subject = subject.Replace("@AWB/PO/ORD", temperatureDetail.CustomerPO);
                        temperatureDetail.CustomerPO = " PO # " + temperatureDetail.CustomerPO;
                        IsValid = false;
                    }
                    if (!string.IsNullOrEmpty(temperatureDetail.OrderNo))
                    {
                        subject = subject.Replace("@AWB/PO/ORD", temperatureDetail.OrderNo);
                        temperatureDetail.OrderNo = " Order # " + temperatureDetail.OrderNo;
                        IsValid = false;
                    }
                    if (!string.IsNullOrEmpty(temperatureDetail.ContainerNo))
                    {
                        subject = subject.Replace("@AWB/PO/ORD", temperatureDetail.ContainerNo);
                        temperatureDetail.ContainerNo = " Container # " + temperatureDetail.ContainerNo;
                        IsValid = false;
                    }
                    if (IsValid)
                    {
                        subject = subject.Replace("@AWB/PO/ORD", "");
                    }
                    temperatureDetail.ActTemp = messageTemp;
                    temperatureDetail.ActTemperature = ActualTemp;
                    temperatureDetail.PickUpArrival = Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(temperatureDetail.PickUpArrival));
                    temperatureDetail.PickUpDeparture = Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(temperatureDetail.PickUpDeparture));
                    temperatureDetail.DeliveryArrival = Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(temperatureDetail.DeliveryArrival));
                    temperatureDetail.DeliveryDeparture = Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(temperatureDetail.DeliveryDeparture));
                   // temperatureDetail.DriverFumigationIn = Configurations.ConvertUTCtoLocalTime(Convert.ToDateTime(temperatureDetail.DriverFumigationIn));

                    //subject = subject.Replace("@TRLR", customerDetail.Trailer);
                    //subject = subject.Replace("@STATUS", customerDetail.Status);


                    string bodywithsignature = this.RenderViewToStringAsync<TemperatureEmailSipmentDTO>("_ShipmentTemperature", temperatureDetail);
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
                            string fileName = "proofoftemp_" + (i + 1) + "." + ext;


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
                      //  var deliveryStatus = customerDetail.FumigationStatusHistory.Where(x => x.StatusId == 7).FirstOrDefault();
                        //if (deliveryStatus == null && customerDetail.StatusId != 3)
                        //{
                        //    // fileAttach = null;
                        //}
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
                ErrorLog("Mail error : " + ex.Message);
                return isEmail;
            }
            finally
            {
                pdfDoc.Close();
                sr.Dispose();

            }
        }

        #region Delete Comments
        public ActionResult DeleteComments(int shipmentId)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();
                ShipmentDTO objShipment = new ShipmentDTO();
                objShipment.ShipmentId = shipmentId;
                objShipment.CreatedBy = objMemberProfile.UserId;
                objShipment.CreatedDate = Configurations.TodayDateTime;
                if (shipmentBAL.DeleteComments(objShipment))
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

        #region Get GetOrderTaken
        public ActionResult GetOrderTaken()
        {
            try
            {
                var IsSuccess = shipmentBAL.GetOrderTaken();
                return Json(IsSuccess, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Get ShipmentInProgress
        public ActionResult GetShipmentInProgress()
        {
            try
            {
                var IsSuccess = shipmentBAL.GetShipmentInProgress();
                return Json(IsSuccess, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Get CustomerDetail
        public ActionResult CustomerDetail(int shipmentid)
        {
            try
            {
                var IsSuccess = shipmentBAL.CustomerDetail(shipmentid);
                return Json(IsSuccess, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Get DriverDetails
        public ActionResult DriverDetail()
        {
            try
            {
                var IsSuccess = shipmentBAL.DriverDetail();
                return Json(IsSuccess, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion
    }

}
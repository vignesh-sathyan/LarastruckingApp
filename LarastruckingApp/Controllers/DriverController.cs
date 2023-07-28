using Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using LarastruckingApp.BusinessLayer.DriverModule;
using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.Common;
using LarastruckingApp.DTO;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Driver;
using LarastruckingApp.Entities.Driver_Fumigation;
using LarastruckingApp.Entities.Enum;
using LarastruckingApp.Entities.Fumigation;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Infrastructure;
using LarastruckingApp.Log.Utility;
using LarastruckingApp.Resource;
using LarastruckingApp.Utility;
using LarastruckingApp.ViewModel;
using LarastruckingApp.ViewModel.PreTrip;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LarastruckingApp.Controllers
{
    [Authorize]
    public class DriverController : BaseController
    {
        #region Private Member
        /// <summary>
        /// Private member declaration
        /// </summary>
        private readonly IDriverBAL iDriverRepo;
        private readonly ILeaveBAL iLeaveRepo;
        private readonly IDriverModuleBAL iDriverModuleRepo;
        private MemberProfile memberProfile = null;
        private readonly IShipmentBAL shipmentBAL = null;
        private readonly IFumigationBAL fumigationBAL = null;

        EncryptAndDecrypt encryptAndDecrypt = new EncryptAndDecrypt();
        #endregion

        #region Contructor
        /// <summary>
        ///  Constructor Injection
        /// </summary>
        /// <param name="iDriverBAL"></param>
        /// <param name="ILeaveBAL"></param>
        public DriverController(IDriverBAL iDriverBAL, ILeaveBAL ILeaveBAL, IDriverModuleBAL IDriverModuleBAL, IShipmentBAL iShipmentBAL, IFumigationBAL iFumigationBAL)
        {
            fumigationBAL = iFumigationBAL;
            shipmentBAL = iShipmentBAL;
            iDriverRepo = iDriverBAL;
            iLeaveRepo = ILeaveBAL;
            iDriverModuleRepo = IDriverModuleBAL;
            memberProfile = new MemberProfile();
        }
        #endregion

        #region SECTION 1: ADMIN SECTION- DRIVER MODULE
        ///
        /// This section is driver module for admin section. 
        ///
        #region Private DropDown Bind
        /// <summary>
        /// This method is used to bind dropdowns
        /// </summary>
        private void BindDropDown()
        {
            ViewBag.Country = iDriverRepo.GetCountryList();
            ViewBag.State = new SelectList(iDriverRepo.GetStateList(), "ID", "Name");
            ViewBag.Languages = new SelectList(iDriverRepo.GetLanguageList(), "LanguageId", "Language");
        }
        #endregion

        #region Index: Get
        /// <summary>
        /// Default method for page load and geting user infromation by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize]
        public ActionResult Index(int id = 0)
        {
            try
            {
                BindDropDown();
                DriverViewModel objDriverViewModel = new DriverViewModel();
                ViewBag.Vehicle = new SelectList(iDriverRepo.GetEquipment(), "EDID", "LicencePlate");

                objDriverViewModel.DriverID = id;

                return View(objDriverViewModel);
            }
            catch (Exception)
            {

                throw;
            }


        }
        #endregion

        #region Index: POST ADD
        /// <summary>
        /// Action method for adding  Driver detail
        /// </summary>
        /// <param name="driverViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(DriverViewModel driverViewModel)
        {
            try
            {
                BindDropDown();
                if (ModelState.IsValid)
                {
                    DriverDTO objDriverDTOinsert = AutoMapperServices<DriverViewModel, DriverDTO>.ReturnObject(driverViewModel);
                    objDriverDTOinsert.CreatedBy = memberProfile.UserId;
                    objDriverDTOinsert.GuidInUser = Guid.NewGuid().ToString();
                    JsonResponse objJsonResponse = new JsonResponse();
                    var result = iDriverRepo.Add(objDriverDTOinsert);
                    if (result.IsSuccess)
                    {


                        string baseURL = LarastruckingApp.Entities.Common.Configurations.BaseURL;
                        string resetLink = baseURL.Trim() + LarastruckingResource.ForgotPasswordResetPassword + objDriverDTOinsert.GuidInUser;
                        string subject = LarastruckingResource.CreateNewPasswordLink;
                        string message = " Dear " + objDriverDTOinsert.FirstName + " " + objDriverDTOinsert.LastName + ", <br /><br /> please click on the below link to create your password. <br /><br /> <a href=" + resetLink.Trim() + ">" + resetLink.Trim() + "</a> <br /><br /><br /><br />  Best Regards,<br/> Lara’s Trucking & Logistics";

                        string EmailSent = string.Empty;

                        MailWithCCAttachDTO mailData = new MailWithCCAttachDTO();

                        mailData.MailPurpose = "Add Driver";
                        mailData.ToMail = objDriverDTOinsert.EmailId;
                        mailData.ToMailCC = string.Empty;
                        mailData.ToMailBCC = string.Empty;
                        mailData.MailSubject = subject;
                        mailData.MailBody = message;
                        mailData.strMailtype = string.Empty;
                        mailData.CreatedBy = memberProfile.UserId;
                        mailData.CreatedOn = Configurations.TodayDateTime;
                        Email.SendMailWithCCAttach(mailData, out EmailSent);


                        objJsonResponse.Data = result.DriverID;
                        objJsonResponse.IsSuccess = true;
                        TempData["SuccessMessage"] = LarastruckingResource.DataSaveSuccessfully;
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
                else
                {
                    return View(driverViewModel);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Upload Driver Document        
        /// <summary>
        /// Upload driver document
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadDriverDocument(UploadDriverDocumentViewModel model)
        {
            MemberProfile objMemberProfile = new MemberProfile();
            int DriverId = 0;
            UserRoleDTO dto = new UserRoleDTO();
            if (memberProfile.UserRole == "Management" || memberProfile.UserRole == "Accounting")
            {
                dto.UserID = memberProfile.UserId;
                dto.RoleName = memberProfile.UserRole;
            }
            else
            {
                dto.UserID = DriverId;
            }

            bool isSuccess = false;
            if (model.DriverDocument != null)
            {
                foreach (var driverFile in model.DriverDocument)
                {
                    isSuccess = false;
                    string damagedfileName = string.Empty;
                    var imgUrl = ImageUploader.Upload(driverFile, out damagedfileName, Configurations.FUDriverDocPath);
                    if (imgUrl != null)
                    {

                        DriverDocumentDTO objDriverDocumentDTO = new DriverDocumentDTO();
                        objDriverDocumentDTO.DriverId = model.DriverId;
                        objDriverDocumentDTO.CreatedBy = objMemberProfile.UserId;
                        objDriverDocumentDTO.DocumentTypeId = model.DocumentType;
                        objDriverDocumentDTO.DocumentName = model.DocumentName;
                        objDriverDocumentDTO.ImageURL = imgUrl;
                        objDriverDocumentDTO.ImageName = driverFile.FileName;
                        objDriverDocumentDTO.DocumentExpiryDate = Configurations.ConvertLocalToUTC(model.ExpiryDate);
                        objDriverDocumentDTO.CreatedDate = Configurations.TodayDateTime;
                        objDriverDocumentDTO.UserId = dto.UserID;
                        objDriverDocumentDTO.UserRole = dto.RoleName;
                        if (iDriverRepo.AddDriverDocument(objDriverDocumentDTO).IsSuccess)
                        {
                            isSuccess = true;
                        };

                    }
                }
            }
            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region UpdateDriver
        /// <summary>
        /// Action method for Update Driver detail
        /// </summary>
        /// <param name="driverViewModel"></param>
        /// <returns></returns>
        public ActionResult UpdateDriver(DriverViewModel driverViewModel)
        {
            try
            {
                ViewBag.Country = iDriverRepo.GetCountryList();
                ViewBag.State = new SelectList(iDriverRepo.GetStateList(), "ID", "Name");

                if (ModelState.IsValid)
                {
                    if (driverViewModel.DriverID > 0)
                    {

                        DriverDTO objDriverDTO = AutoMapperServices<DriverViewModel, DriverDTO>.ReturnObject(driverViewModel);
                        var result = iDriverRepo.Update(objDriverDTO);
                        JsonResponse objJsonResponse = new JsonResponse();
                        objJsonResponse.Data = driverViewModel.DriverID;
                        if (result.IsSuccess)
                        {

                            objJsonResponse.IsSuccess = true;
                            TempData["SuccessMessage"] = LarastruckingResource.DataUpdateSuccessfully;
                            return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            objJsonResponse.IsSuccess = false;
                            objJsonResponse.Message = result.Response;
                            return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    return View(driverViewModel);
                }
                return View(driverViewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region delete driver document
        /// <summary>
        ///delete driver document
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public ActionResult DeletetDriverDocument(int documentId)
        {
            try
            {

                JsonResponse objJsonResponse = new JsonResponse();

                if (documentId > 0)
                {

                    if (iDriverRepo.DeleteDocument(documentId))
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

        #region ConvertBase64toImage

        /// <summary>
        /// Method for convert image in to base64
        /// </summary>
        /// <param name="baseCode"></param>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public string ConvertBase64toImage(string baseCode, string imageName)
        {
            string ext = "." + imageName.Split('.')[1];
            string base64 = baseCode.Split(',')[1];
            byte[] imageBytes = Convert.FromBase64String(base64);
            string path = System.Web.HttpContext.Current.Server.MapPath($"~/{Configurations.FUDriverDocPath}");
            var str = Guid.NewGuid().ToString("N").Substring(0, 5) + DateTime.Now.ToString("ddMMyyhhmms") + ext;
            string imageNameWithPath = path + str;


            System.IO.File.WriteAllBytes(imageNameWithPath, imageBytes);
            string returnpath = $"/{Configurations.FUDriverDocPath}" + str;
            return returnpath;

        }
        #endregion

        #region DeleteDriver
        /// <summary>
        /// Action method for delete driver detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteDriver(int id = 0)
        {
            try
            {
                DriverDTO objDriverDTO = new DriverDTO();
                objDriverDTO.DriverID = id;
                JsonResponse objJsonResponse = new JsonResponse();

                if (iDriverRepo.Delete(objDriverDTO))
                {
                    objJsonResponse.IsSuccess = true;
                    objJsonResponse.Message = LarastruckingResource.msgDeleteDriver;
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

        #region ViewDriver
        /// <summary>
        /// Action method for showing Driver Detail
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize]
        public ActionResult ViewDriver()
        {
            return View();

        }
        #endregion

        #region DriverIncentive
        /// <summary>
        /// Action method for showing Driver Detail
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize]
        public ActionResult DriverIncentive()
        {
            return View();

        }
        #endregion

        #region LoadData
        /// <summary>
        /// Acction Method for bind driver table
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadData()
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

                var driverList = iDriverRepo.DriverList();

                if (!string.IsNullOrEmpty(search))
                {
                    driverList = driverList.Where(x => x.FirstName.ToUpper()
                                                                    .Contains(search.ToUpper()) ||
                                                       x.LastName.ToUpper()
                                                                    .Contains(search.ToUpper()) ||
                                                       x.Email.ToUpper()
                                                                    .Contains(search.ToUpper()) ||
                                                       x.Phone.ToUpper()
                                                                    .Contains(search.ToUpper()));
                }
                recordsTotal = driverList.Count();
                var data = driverList.ToList();

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

        #region GetDocumetTypeList
        /// <summary>
        /// Get Documnet Type
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDocumetTypeList()
        {
            var data = iDriverRepo.DocumentList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetDriverInfoById
        /// <summary>
        /// Get Driver Info by id
        /// </summary>
        /// <param name="DriverId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDriverInfoById(int DriverId)
        {
            try
            {
                var data = iDriverRepo.FindById(DriverId);
                if (data != null)
                {
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Apply Leave
        /// <summary>
        /// This method is to load leave apply page specific to driver
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize]
        public ActionResult Leave(int? id)
        {
            LeaveManageDto dto = new LeaveManageDto();

            int driverId = 0;
            driverId = Convert.ToInt32(id);
            if (memberProfile.UserRole == "Driver")
            {
                driverId = memberProfile.UserId;
            }


            dto.DriverLeave.UserId = Convert.ToInt32(driverId);
            dto.LeaveStatus = iLeaveRepo.GetLeaveStatus();

            if (User.IsInRole(LarastruckingResource.UserRole_Driver))
            {
                dto.LeaveStatus = dto.LeaveStatus.Where(p => p.LeaveStatus == "Cancelled" || p.LeaveStatus == "Pending").ToList();
            }

            var driverDetail = iDriverRepo.GetDriverBasicDetail(driverId);

            if (driverDetail != null)
            {
                dto.DriverLeave.FirstName = driverDetail.FirstName;
                dto.DriverLeave.LastName = driverDetail.LastName;
                dto.DriverLeave.Email = driverDetail.EmailId;
                dto.DriverLeave.Phone = driverDetail.Phone;

            }

            dto.Leaves = iLeaveRepo.GetLeaveHistory(driverId);
            return View(dto);
        }
        #endregion

        #region Add Leaves
        /// <summary>
        /// This method is for applying leave for a driver
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Leave(LeaveManageDto dto)
        {
            dto.DriverLeave.AppliedBy = memberProfile.UserId;

            dto.LeaveStatus = iLeaveRepo.GetLeaveStatus();

            if (User.IsInRole(LarastruckingResource.UserRole_Driver))
            {
                dto.LeaveStatus = dto.LeaveStatus.Where(p => p.LeaveStatus == "Cancelled" || p.LeaveStatus == "Pending").ToList();
            }


            // set default leave status id to 1: Pending;
            // This is applicable when a driver is applying a leave
            dto.DriverLeave.LeaveStatusId = dto.DriverLeave.LeaveStatusId == 0 ? 1 : dto.DriverLeave.LeaveStatusId;


            var response = new DriverLeaveDto();
            if (dto.DriverLeave.LeaveId > 0)
            {
                response = iLeaveRepo.Update(dto.DriverLeave);
            }
            else
            {
                response = iLeaveRepo.Add(dto.DriverLeave);
            }

            if (response.IsSuccess)
            {
                dto.DriverLeave.IsSuccess = response.IsSuccess;
                dto.DriverLeave.Response = response.Response;

                MemberProfile objMeberProfile = new MemberProfile();
                dto.DriverLeave.UserId = objMeberProfile.UserId;
                string AppliedByNameb = objMeberProfile.FullName;
                iLeaveRepo.SendLeavMail(dto, AppliedByNameb);
            }

            return Json(dto, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get All Pending and Approved Leaves 
        /// <summary>
        /// Method to get all leaves applied by a driver
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetApprovedPendingLeaves(int id)
        {
            IList<DriverLeaveDto> leaves = iLeaveRepo.GetLeaveHistory(id);

            var isDriver = User.IsInRole(LarastruckingResource.UserRole_Driver);
            if (leaves == null)
            {
                leaves = new List<DriverLeaveDto>();
            }
            return Json(new { leaves, isDriver }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Leave Edit
        /// <summary>
        /// Method to edit leave detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult LeaveEdit(int id)
        {
            DriverLeaveDto leaves = iLeaveRepo.FindById(id);
            if (leaves == null)
            {
                leaves = new DriverLeaveDto();
            }
            return Json(leaves, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Leave Delete
        /// <summary>
        /// Method to delete leave 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult LeaveDelete(int id)
        {
            DriverLeaveDto objDriverDTO = new DriverLeaveDto();
            objDriverDTO.LeaveId = id;

            JsonResponse objJsonResponse = new JsonResponse();
            objJsonResponse.IsSuccess = iLeaveRepo.Delete(objDriverDTO);
            if (objJsonResponse.IsSuccess)
            {
                objJsonResponse.IsSuccess = true;
                objJsonResponse.Message = LarastruckingResource.msgDeleteDriver;
                return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                objJsonResponse.IsSuccess = false;
                objJsonResponse.Message = LarastruckingResource.ErrorOccured;
                return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Driver Documents List
        [HttpGet]
        [CustomAuthorize]
        public ActionResult Documents(int id)
        {
            try
            {
                DriverDetailsDto docs = iDriverRepo.GetDriverDocuments(id);
                return View(docs);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Download Files
        /// <summary>
        /// Method to download file based on the document id
        /// </summary>
        /// <param name="docId"></param>
        /// <returns></returns>
        public FileResult Download(int id)
        {
            var doc = iDriverRepo.DownloadDocument(id);

            if (doc != null)
            {
                var ext = Path.GetExtension(doc.ImageURL);
                var contentType = GetContentType(ext);
                var filePath = Server.MapPath(doc.ImageURL);
                return File(filePath, contentType, doc.ImageName);
            }

            return null;
        }
        #endregion

        #region Set Content-Type 
        public string GetContentType(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".png":
                    return "image/png";

                case ".jpg":


                case ".jpeg":
                    return "image/jpeg";

                case ".pdf":
                    return "application/pdf";

                default:
                    return "application/pdf";
            }
        }
        #endregion

        #region Driver Details Import 
        [HttpGet]
        [CustomAuthorize]
        public ActionResult ImportDrivers()
        {
            return View();

        }


        [HttpPost]
        public ActionResult ImportDrivers(HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    IExcelDataReader excelReader = null;
                    //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                    string extetion = Path.GetExtension(file.FileName);
                    if (extetion.ToLower() == ".xls")
                    {
                        excelReader = ExcelReaderFactory.CreateBinaryReader(file.InputStream);
                    }
                    else if (extetion.ToLower() == ".xlsx")
                    {
                        //... //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(file.InputStream);
                    }


                    //... //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                    //DataSet result = excelReader.AsDataSet();
                    //... //4. DataSet - Create column names from first row
                    excelReader.IsFirstRowAsColumnNames = true;
                    DataSet result = excelReader.AsDataSet();

                    MemberProfile mp = new MemberProfile();
                    // string ConStr = string.Empty;
                    // string path = Server.MapPath("DRIVERDETAILSFILE.xlsx");
                    //connection string for that file which extantion is .xlsx    
                    // ConStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;ReadOnly=False;HDR=Yes;\"";
                    foreach (DataRow row in result.Tables[0].Rows)
                    {
                        try
                        {
                            DriverDTO driver = new DriverDTO();
                            driver.FirstName = Convert.ToString(row["First Name"]);
                            driver.LastName = Convert.ToString(row["Last Name"]);
                            driver.DOB = Convert.ToString(row["DOB"]);
                            driver.Address1 = Convert.ToString(row["Address 1"]);
                            driver.Address2 = Convert.ToString(row["Address2"]);
                            driver.Country = 231;
                            driver.State = 3929;
                            driver.City = Convert.ToString(row["City"]);
                            driver.ZipCode = Convert.ToString(row["Zip Code "]);
                            driver.CellNumber = Convert.ToString(row["Cell Phone "]);
                            driver.DriverLicence = Convert.ToString(row["Driver License"]);
                            driver.STANumber = Convert.ToString(row["STA Number"]);
                            driver.CitizenShip = Convert.ToString(row["Citizenship"]);
                            driver.EmailId = Convert.ToString(row["email"]);
                            driver.FullTime = Convert.ToBoolean(row["Full Time"]);
                            driver.MedicalConditions = Convert.ToString(row["Medical Conditions"]);
                            driver.BloodGroup = Convert.ToString(row["Blood Group"]);
                            driver.EmergencyContactOne = Convert.ToString(row["Contact 1"]);
                            driver.EmergencyPhoneNoOne = Convert.ToString(row["Phone 1"]);
                            driver.RelationshipStatus1 = Convert.ToString(row["Relationship"]);
                            driver.EmergencyContactTwo = Convert.ToString(row["Contact 2"]);
                            driver.EmergencyPhoneNoTwo = Convert.ToString(row["Phone 2"]);
                            driver.RelationshipStatus2 = Convert.ToString(row["Relationship"]);
                            driver.CreatedBy = mp.UserId;
                            driver.CreatedDate = Configurations.TodayDateTime;
                            driver.ModifiedBy = mp.UserId;
                            driver.ModifiedDate = Configurations.TodayDateTime;
                            iDriverRepo.Add(driver);
                        }
                        catch (Exception)
                        {
                            throw;
                            ////making query    
                            //string query = "INSERT INTO [Sheet1$] ([First Name], [Last Name]) VALUES('" + row["First Name"] + "', '" + row["Last Name"] + "')";
                            ////Providing connection    
                            //OleDbConnection conn = new OleDbConnection(ConStr);
                            ////checking that connection state is closed or not if closed the     
                            ////open the connection    
                            //if (conn.State == ConnectionState.Closed)
                            //{
                            //    conn.Open();
                            //}
                            ////create command object    
                            //OleDbCommand cmd = new OleDbCommand(query, conn);
                            //cmd.ExecuteNonQuery();
                        }
                    }

                    TempData["SuccessMessage"] = LarastruckingResource.DataSaveSuccessfully;
                    return RedirectToAction("Index");


                }
            }
            catch (Exception)
            {

            }
            return View();

        }


        #endregion

        #region Mandatory Driver Documents
        /// <summary>
        /// Mandatory Driver Documents
        /// </summary>
        /// <param name="DriverId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult DriverDocumetsType(int driverID)
        {
            try
            {
                if (driverID > 0)
                {
                    var data = iDriverRepo.DriverDocumetsType(driverID);
                    if (data)
                    {
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #endregion

        #region SECTION 2: DRIVER SECTION- DRIVER MODULE
        ///
        /// This section is driver module for driver roles. 
        ///

        #region Driver: Get => Dashboard

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

        #region Driver: Get => Shipment Details

        [HttpGet]
        [CustomAuthorize]
        public ActionResult Detail(string id)
        {
            try
            {
                int shipmentId = 0;
                if (!String.IsNullOrEmpty(id))
                {
                    shipmentId = Convert.ToInt32(encryptAndDecrypt.DecryptDES(id));
                }
                int DriverId = 0;
                UserRoleDTO dto = new UserRoleDTO();
                if (memberProfile.UserRole == "Driver")
                {
                    dto.UserID = memberProfile.UserId;
                    dto.RoleName = memberProfile.UserRole;
                }
                else
                {
                    dto.UserID = DriverId;
                }
                PreTripViewModel preTripVm = new PreTripViewModel();
                preTripVm.ShipmentsDetail = iDriverModuleRepo.GetShipmentRoutes(shipmentId, dto.UserID);
                preTripVm.DriverLanguage = iDriverModuleRepo.GetDriverLanguage(memberProfile.UserId);
                ViewBag.OpenedShipment = shipmentId;
                return View(preTripVm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Select all Shipment 
        ///// <summary>
        ///// Select Shipment Routes Stops 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        public JsonResult SelectShipmentsDetail(int id)
        {
            try
            {
                int driverId = 0;
                UserRoleDTO dto = new UserRoleDTO();
                if (memberProfile.UserRole == "Driver")
                {
                    dto.UserID = memberProfile.UserId;
                    dto.RoleName = memberProfile.UserRole;
                }
                else
                {
                    dto.UserID = driverId;
                }
                PreTripViewModel preTripVm = new PreTripViewModel();
                preTripVm.ShipmentsDetail = iDriverModuleRepo.GetShipmentRoutes(id, dto.UserID);
                return Json(preTripVm.ShipmentsDetail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Driver Status
        /// <summary>
        /// Customer status
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDriverStatus()
        {
            try
            {
                var statusList = iDriverModuleRepo.GetStatusList();
                return Json(statusList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Driver Substatus
        /// <summary>
        /// Shipment Sub Status
        /// </summary>
        /// <param name="statusid"></param>
        /// <returns></returns>
        public ActionResult GetDriverSubStatus(int statusid)
        {
            try
            {
                var substatusList = iDriverModuleRepo.GetSubStatusList(statusid);
                return Json(substatusList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region GPS Tracking 
        /// <summary>
        /// GPS Tracking 
        /// </summary>
        /// <returns></returns>
        public ActionResult GPSTracker(string location)
        {
            ViewBag.location = location;
            return PartialView("_getDirectionDetails");
        }

        #endregion

        #region Fumigation Details
        /// <summary>
        /// Fumigation Details
        /// </summary>
        /// <returns></returns>
        public ActionResult FumigationDetails(string id)
        {
            try
            {
                int fumigationId = 0;
                if (!String.IsNullOrEmpty(id))
                {
                    fumigationId = Convert.ToInt32(encryptAndDecrypt.DecryptDES(id));
                }
                int driverId = 0;
                UserRoleDTO dto = new UserRoleDTO();
                if (memberProfile.UserRole == "Driver")
                {
                    dto.UserID = memberProfile.UserId;
                    dto.RoleName = memberProfile.UserRole;
                }
                else
                {
                    dto.UserID = driverId;
                }
                PreTripViewModel preTripVm = new PreTripViewModel();
                preTripVm.FumigationDetail = iDriverModuleRepo.GetFumigationRoutes(fumigationId, dto.UserID);
                preTripVm.DriverLanguage = iDriverModuleRepo.GetDriverLanguage(memberProfile.UserId);
                return View(preTripVm);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region NON-ACTION METHODS

        #region Get Pre-Trip Shipping Details
        [HttpPost]
        public JsonResult GetPreTripShipmentDetails()
        {
            try
            {
                string search = Request.Form.GetValues(Configurations.SearchValue).FirstOrDefault();
                var draw = Request.Form.GetValues(Configurations.Draw).FirstOrDefault();
                var start = Request.Form.GetValues(Configurations.Start).FirstOrDefault();
                var length = Request.Form.GetValues(Configurations.Length).FirstOrDefault();
                var sortColumn = "";
                var sortColumnDir = "";
                if (string.IsNullOrEmpty(search))
                {
                    // Find Order Column
                    sortColumn = Request.Form.GetValues(Configurations.Columns + Request.Form.GetValues(Configurations.OrderColumn).FirstOrDefault() + Configurations.Name).FirstOrDefault();
                    sortColumnDir = Request.Form.GetValues(Configurations.OrderDir).FirstOrDefault();
                }

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

                var preTripInfo = iDriverModuleRepo.GetPreTripShipmentDetails(dto, memberProfile.UserId);

                //foreach (var item in preTripInfo)
                //{
                //    if (item.AirWayBill == null)
                //    {
                //        if (item.AirWayBill == null && item.CustomerPO != null)
                //        {
                //            item.AirWayBill = item.CustomerPO;
                //        }
                //        else if (item.AirWayBill == null && item.OrderNo != null)
                //        {
                //            item.AirWayBill = item.OrderNo;
                //        }
                //    }
                //}


                return Json(new { draw = draw, recordsFiltered = dto.TotalCount, recordsTotal = dto.TotalCount, data = preTripInfo }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region Get Pre-Trip Check List
        /// <summary>
        /// Get pre trip check list detail based on shipping id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPreTripCheckList(int id)
        {
            try
            {
                var preTripCheckList = iDriverModuleRepo.GetPreTripCheckList(id);
                return Json(preTripCheckList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region Save Pre-Trip Check List
        /// <summary>
        /// Save pre trip check list detail based on shipping id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SavePreTripCheckList(DriverPreTripDto dto)
        {
            try
            {
                dto.UserId = memberProfile.UserId;
                var response = iDriverModuleRepo.SavePreTripCheckList(dto);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region Cancel Leave Functionality
        /// <summary>
        /// This method is used to cancel the leaves that are being applied.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CancelLeave(int id)
        {
            try
            {
                var response = iLeaveRepo.CancelLeave(id);
                return Json(response, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetShipmentLocationDetails(int id)
        {
            try
            {
                var shipmentLocationList = iDriverModuleRepo.GetShipmentLocationDetails(id);
                return Json(shipmentLocationList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Save Pre-Trip Add Shipment Detail
        /// <summary>
        /// Save Pre-Trip Add Shipment Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SavePreTripShipmentDetail(PreTripAddShipmentDetailDto dto)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();
                dto.CreatedBy = objMemberProfile.UserId;
                dto.CreatedOn = Configurations.TodayDateTime;

                if (ModelState.IsValid)
                {

                    bool isEmailNeedToSend = false;
                    var result = iDriverModuleRepo.SavePreTripShipmentDetail(dto, out isEmailNeedToSend);
                    if (result)
                    {
                        ShipmentEmailDTO shipmentEmailDTO = new ShipmentEmailDTO();

                        shipmentEmailDTO.ShipmentId = dto.ShipmentId;

                        if (isEmailNeedToSend)
                        {
                            SendMail(shipmentEmailDTO);
                        }

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
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Save Proof of Temperature
        /// <summary>
        ///Save Proof of Temperature
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveProofOfTemperature(PreTripAddShipmentDetailDto dto)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();
                dto.CreatedBy = objMemberProfile.UserId;
                dto.CreatedOn = Configurations.TodayDateTime;

                if (ModelState.IsValid)
                {
                    #region UPLOADED FILES : PROOF OF TEMP
                    PreTripShipmentImagesDto listProofOfTempDto = new PreTripShipmentImagesDto();
                    if (dto.UploadedTemperatureProofFiles != null)
                    {
                        string fileName = string.Empty;
                        string renamefile = !string.IsNullOrEmpty(dto.AWBPOORD) ? dto.AWBPOORD : Convert.ToString(dto.ShipmentRouteId);
                        var imgUrl = ImageUploader.Upload(dto.UploadedTemperatureProofFiles, out fileName, "Images/ProofFiles", renamefile);

                        if (imgUrl != null)
                        {
                            PreTripShipmentImagesDto images = new PreTripShipmentImagesDto();
                            images.ImageUrl = imgUrl;
                            images.ImageName = fileName;
                            images.ActualTemperature = dto.ActualTemperature;
                            listProofOfTempDto = images;
                        }
                    }
                    dto.UploadedProofOfTempFile = listProofOfTempDto;
                    #endregion

                    var result = iDriverModuleRepo.SaveProofOfTemperature(dto);
                    if (result)

                    {

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

        #region Save Damaged Files
        /// <summary>
        ///  Save Damaged Files
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveDamagedFiles(PreTripAddShipmentDetailDto dto)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();
                dto.CreatedBy = objMemberProfile.UserId;
                dto.CreatedOn = Configurations.TodayDateTime;

                if (ModelState.IsValid)
                {
                    #region UPLOADED FILES: Damaged 
                    List<ShipmentDamagedImagesDto> listDamagedDto = new List<ShipmentDamagedImagesDto>();
                    if (dto.DamagedFiles != null)
                    {
                        foreach (var damagedUploadFile in dto.DamagedFiles)
                        {
                            string damagedfileName = string.Empty;
                            string renamefile = !string.IsNullOrEmpty(dto.AWBPOORD) ? dto.AWBPOORD : Convert.ToString(dto.ShipmentRouteId);
                            var imgUrl = ImageUploader.Upload(damagedUploadFile, out damagedfileName, "Images/DamagedFiles", renamefile);
                            if (imgUrl != null)
                            {
                                ShipmentDamagedImagesDto Image = new ShipmentDamagedImagesDto();
                                Image.ImageDescription = dto.ImageDescription;
                                Image.ImageUrl = imgUrl;
                                Image.ImageName = damagedfileName;
                                listDamagedDto.Add(Image);
                            }
                        }
                    }

                    #endregion
                    dto.UploadedDamagedFile = listDamagedDto;

                    var result = iDriverModuleRepo.SaveDamagedFiles(dto);
                    if (result)

                    {

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

        #region Send Mail for shipmenet
        /// <summary>
        /// Send Mail for shipmenet
        /// </summary>
        /// <param name="model"></param>
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
                        if (customerDetail.StatusId == 5)
                        {
                            if (customerDetail.ProofOfTemprature.Count > 0)
                            {
                                int tempNo = 1;
                                foreach (var proofOfTemp in customerDetail.ProofOfTemprature)
                                {
                                    string path = proofOfTemp.ImageUrl;
                                    using (var webClient = new WebClient())
                                    {
                                        bytes = webClient.DownloadData(path);
                                    }
                                    string fileName = "";
                                    if (customerDetail.ProofOfTemprature.Count == 1)
                                    {
                                        fileName = "Temp." + proofOfTemp.Ext;
                                    }
                                    else
                                    {
                                        fileName = "Temp" + tempNo + "." + proofOfTemp.Ext;
                                    }
                                    fileAttach.Add(bytes, fileName);
                                    tempNo = tempNo + 1;
                                }
                            }

                        }
                        else if (customerDetail.StatusId == 3)
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
                        if (deliveryStatus == null && customerDetail.StatusId != 5)
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
        #endregion

        #region Send Mail for Fumigation
        public void SendFumigationMail(FumigationEmailDTO model)
        {
            StringReader sr = null;
            Document pdfDoc = null;
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                var customerDetail = fumigationBAL.GetCustomerDetail(model);
                if (customerDetail != null)
                {
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
                        if (customerDetail.StatusId == 5)
                        {
                            if (customerDetail.ProofOfTemprature.Count > 0)
                            {
                                int tempNo = 1;
                                foreach (var proofOfTemp in customerDetail.ProofOfTemprature)
                                {
                                    string path = proofOfTemp.ImageUrl;
                                    using (var webClient = new WebClient())
                                    {
                                        bytes = webClient.DownloadData(path);
                                    }
                                    string fileName = "";
                                    if (customerDetail.ProofOfTemprature.Count == 1)
                                    {
                                        fileName = "Temp." + proofOfTemp.Ext;
                                    }
                                    else
                                    {
                                        fileName = "Temp" + tempNo + "." + proofOfTemp.Ext;
                                    }

                                    fileAttach.Add(bytes, fileName);
                                    tempNo = tempNo + 1;
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
                        if (deliveryStatus == null && customerDetail.StatusId != 5)
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
                PreTripViewModel preTripViewModel = new PreTripViewModel();
                preTripViewModel.ShipmentFreightList = iDriverModuleRepo.GetShipmentFreightDetails(id);
                return PartialView(Configurations._FreightDetails, preTripViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Shipment Freight Details (return lst of Freight)
        /// <summary>
        /// Get Shipment Freight Details By  ShippingRoutes Id (return lst of Freight)
        /// </summary>
        /// <param name="ShippingRoutesId"></param>
        /// <returns></returns>
        public ActionResult GetShipmentFreight(int id)
        {
            try
            {
                PreTripViewModel preTripViewModel = new PreTripViewModel();
                preTripViewModel.ShipmentFreightList = iDriverModuleRepo.GetShipmentFreightDetails(id);
                return Json(preTripViewModel.ShipmentFreightList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion



        #region Get Pre-Trip Check Timings for Arrival & departure
        /// <summary>
        /// Get Pre-Trip Check Timings for Arrival & departure
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetPreTripCheckTimings(int id)
        {
            try
            {
                var preTripCheckTimmingList = iDriverModuleRepo.GetPreTripCheckTimings(id);
                return Json(preTripCheckTimmingList, JsonRequestBehavior.AllowGet);
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
                var shipmentDamagedFiles = iDriverModuleRepo.GetShipmentDamagedFiles(id);
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
                var shipmentProofFiles = iDriverModuleRepo.GetShipmentProofOfTempFiles(id, ShipmentFreightDetailId);
                return Json(shipmentProofFiles, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Delete row for Proof of Temp Files
        /// <summary>
        ///  Delete row for Proof of Temp Files
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
                    ShipmentProofOfTempEditBind objProofOfTemprature = new ShipmentProofOfTempEditBind();
                    objProofOfTemprature.proofImageId = imageId;
                    objProofOfTemprature.CreatedBy = objMemberProfile.UserId;
                    objProofOfTemprature.CreatedOn = Configurations.TodayDateTime;
                    if (iDriverModuleRepo.DeleteProofOfTemprature(objProofOfTemprature))
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

        #region  Delete Damage Files
        /// <summary>
        /// Delete Damage Files
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public ActionResult DeleteDamageFiles(int imageId)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();

                if (imageId > 0)
                {
                    ShipmentDamagedEditBindDto objDeleteDamageFiles = new ShipmentDamagedEditBindDto();
                    objDeleteDamageFiles.DamagedID = imageId;
                    objDeleteDamageFiles.CreatedBy = objMemberProfile.UserId;
                    objDeleteDamageFiles.CreatedOn = Configurations.TodayDateTime;
                    if (iDriverModuleRepo.DeleteDamageFiles(objDeleteDamageFiles))
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

        #region Save Signature pad
        /// <summary>
        /// Save Signature pad
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult UpdateSignaturePadDetail(PreTripAddShipmentDetailDto model)
        {
            try
            {

                bool isSuccess = false;
                if (model.DigitalSignature != null)
                {

                    isSuccess = false;
                    string digitalSignature = string.Empty;
                    var imgUrl = ImageUploader.ConvertBase64ToImage(model.DigitalSignature, out digitalSignature, Configurations.FUSignatureDocPath);
                    if (imgUrl != null)
                    {
                        model.DigitalSignaturePath = imgUrl;

                        var result = iDriverModuleRepo.UpdateSignaturePadDetail(model);
                        if (result)
                        {
                            isSuccess = true;
                        }

                    }

                }

                return Json(isSuccess, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region Select Signature 
        /// <summary>
        /// Select Signature 
        /// </summary>
        /// <param name="shipmentRouteId"></param>
        /// <returns></returns>
        public JsonResult SelectSignatureDetail(string id)
        {
            try
            {
                bool isSuccess = false;
                int sRouteId = Convert.ToInt32(id);
                if (sRouteId > 0)
                {
                    var result = iDriverModuleRepo.SelectignaturePadDetail(sRouteId);
                    if (result)
                    {
                        isSuccess = true;
                    }
                }
                return Json(isSuccess, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region SaveGpsTracker
        /// <summary>
        /// SaveGpsTracker
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveGPSTracker(List<SaveGpsTrackingHistoryDto> dto)
        {
            try
            {
                bool isSuccess = false;
                if (dto != null)
                {
                    isSuccess = false;
                    foreach (var driverTrackShipment in dto)
                    {

                        driverTrackShipment.UserId = memberProfile.UserId;
                        var result = iDriverModuleRepo.SaveGPSTracker(driverTrackShipment);
                        if (result)
                        {
                            isSuccess = true;
                        }
                    }
                }
                return Json(isSuccess, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion



        #region Save Shipment web-Camera 
        /// <summary>
        /// Save Shipment web-Camera 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult saveShipmentWebCamera(SaveShipmentWebCameraDTO dto)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();
                dto.CreatedBy = objMemberProfile.UserId;
                dto.CreatedOn = Configurations.TodayDateTime;

                if (dto != null)
                {

                    #region web-Cam Base64 Covert into an image

                    PreTripShipmentImagesDto listCamImageDto = new PreTripShipmentImagesDto();
                    if (dto.ImageUrl != null)
                    {
                        string webcamimg = string.Empty;
                        var imgUrl = ImageUploader.saveImageCam(dto.ImageUrl, out webcamimg, "Images/ProofFiles");

                        if (imgUrl != null)
                        {
                            PreTripShipmentImagesDto images = new PreTripShipmentImagesDto();
                            images.ImageDescription = LarastruckingResource.ProofOfTemperature;
                            images.ImageUrl = imgUrl;
                            images.ActualTemperature = dto.ActualTemperature;
                            images.ImageName = webcamimg;
                            listCamImageDto = images;
                        }
                    }
                    dto.UploadedProofOfTempFile = listCamImageDto;

                    #endregion



                    var result = iDriverModuleRepo.saveShipmentWebCamera(dto);
                    if (result)
                    {
                        //isSuccess = true;
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

        #region Save Damage web-Camera  
        /// <summary>
        /// Save web-Camera  
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult saveShipmentDamageWebCamera(SaveShipmentDamageWebCamDTO dto)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();
                dto.CreatedBy = objMemberProfile.UserId;
                dto.CreatedOn = Configurations.TodayDateTime;
                if (dto != null)
                {
                    #region web-Cam Base64 Covert into an image


                    ShipmentDamagedImagesDto listDamagedDto = new ShipmentDamagedImagesDto();
                    if (dto.ImageUrl != null)
                    {


                        string damagedfileName = string.Empty;
                        var imgUrl = ImageUploader.saveImageCam(dto.ImageUrl, out damagedfileName, "Images/DamagedFiles");

                        if (imgUrl != null)
                        {
                            ShipmentDamagedImagesDto Image = new ShipmentDamagedImagesDto();
                            Image.ImageDescription = dto.ImageDescription;
                            Image.ImageUrl = imgUrl;
                            Image.ImageName = damagedfileName;
                            listDamagedDto = Image;
                        }
                    }
                    dto.UploadedDamagedFile = listDamagedDto;

                    #endregion



                    var result = iDriverModuleRepo.saveShipmentDamageWebCamera(dto);
                    if (result)
                    {

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

        #region Save Waiting Time
        /// <summary>
        /// Save Waiting Time
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveWaitingTime(SaveShipmentWaitingNotifiDto dto)
        {
            try
            {
                var response = iDriverModuleRepo.SaveWaitingTime(dto);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion


        #region Select Shipment Routes Stops
        ///// <summary>
        ///// Select Shipment Routes Stops 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        public JsonResult SelectShipmentRoutesStopDetail(string id)
        {
            try
            {
                int sRouteId = Convert.ToInt32(id);
                var result = iDriverModuleRepo.GetShipmentRoutesStopDetail(sRouteId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get shipment commment deail
        /// <summary>
        /// Get shipment commment deail
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        public JsonResult GetShipmentComments(int shipmentId)
        {
            try
            {
                var commentList = iDriverModuleRepo.GetShipmentComments(shipmentId);
                return Json(commentList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion



        #region Get fumigation commment deail
        /// <summary>
        /// Get fumigation commment deail
        /// </summary>
        /// <param name="fumigationId"></param>
        /// <returns></returns>
        public JsonResult GetFumigationComments(int fumigationId)
        {
            try
            {
                var commentList = iDriverModuleRepo.GetFumigatonComments(fumigationId);
                return Json(commentList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #endregion


        #endregion

        #region SECTION 3 : FUMIGATION SECTION - DRIVER MODULE

        #region Get Pre-Trip Check List
        /// <summary>
        /// Get pre trip check list detail based on shipping id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPreTripCheckFumigationList(int id)
        {
            try
            {
                var preTripCheckList = iDriverModuleRepo.GetPreTripCheckFumigationList(id);
                return Json(preTripCheckList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region Save Fumigation Pre-Trip Check List
        /// <summary>
        /// Save Fumigation pre trip check list detail based on Fumigation id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveFumigationPreTripCheckList(FumigationPreTripCheckUpDTO dto)
        {
            try
            {
                dto.UserId = memberProfile.UserId;
                var response = iDriverModuleRepo.SaveFumigationPreTripCheckList(dto);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region Get Fumigation Location Details
        /// <summary>
        /// Get Fumigation Details for Multiple Routes By Shipment Id 
        /// </summary>
        /// <param name="FumigationId"></param>
        /// <returns></returns>
        public JsonResult GetFumigationLocationDetails(int id)
        {
            try
            {
                var fumigationLocationList = iDriverModuleRepo.GetFumigationRoutesDetails(id);
                return Json(fumigationLocationList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Fumigation Freight Details
        /// <summary>
        /// Get Fumigation Freight Details By  FumigationRoutes Id
        /// </summary>
        /// <param name="FumigationRoutesId"></param>
        /// <returns></returns>
        public ActionResult GetFumigationFreightDetails(int id)
        {
            try
            {
                PreTripViewModel preTripViewModel = new PreTripViewModel();
                preTripViewModel.FumimgationFreightList = iDriverModuleRepo.GetFumigationFreightDetails(id);
                return PartialView("_FumigationFreightDetails", preTripViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Save Fumigation Detail
        /// <summary>
        /// Save Fumigation Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveFumigationtDetail(SaveFumigationDetailsDTO dto)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();
                dto.CreatedBy = objMemberProfile.UserId;
                dto.CreatedOn = Configurations.TodayDateTime;


                if (ModelState.IsValid)
                {

                    bool isEmailNeedToSend = false;
                    var result = iDriverModuleRepo.SaveFumigationtDetail(dto, out isEmailNeedToSend);
                    if (result)
                    {
                        FumigationEmailDTO fumigationEmailDTO = new FumigationEmailDTO();

                        fumigationEmailDTO.FumigationId = dto.FumigationId;

                        if (isEmailNeedToSend)
                        {
                            SendFumigationMail(fumigationEmailDTO);
                        }
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
                var fumigationDamagedFiles = iDriverModuleRepo.GetFumigationDamagedFiles(id);
                return Json(fumigationDamagedFiles, JsonRequestBehavior.AllowGet);
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
                var fumigationProofFiles = iDriverModuleRepo.GetFumigationProofOfTempFiles(id);
                return Json(fumigationProofFiles, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Pre-Trip Check Timings for Arrival & departure
        /// <summary>
        /// Get Pre-Trip Check Timings for Arrival & departure
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetDriverActualTimings(int id)
        {
            try
            {
                var driverActualTimingsList = iDriverModuleRepo.GetDriverActualTimings(id);
                return Json(driverActualTimingsList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Save Fumigation Signature pad
        /// <summary>
        /// Save Fumigation Signature pad
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult UpdateFumigationSignaturePadDetail(SaveFumigationDetailsDTO model)
        {
            try
            {

                bool isSuccess = false;
                if (model.DigitalSignature != null)
                {

                    isSuccess = false;
                    string digitalSignature = string.Empty;
                    var imgUrl = ImageUploader.ConvertBase64ToImage(model.DigitalSignature, out digitalSignature, Configurations.FUSignatureDocPath);
                    if (imgUrl != null)
                    {

                        model.DigitalSignaturePath = imgUrl;
                        var result = iDriverModuleRepo.UpdateFumigationSignaturePadDetail(model);
                        if (result)
                        {
                            isSuccess = true;
                        }

                    }

                }

                return Json(isSuccess, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Select Fumigation Signature pad
        /// <summary>
        /// Select Fumigation Signature pad
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult SelectFumigationSignaturePadDetail(string id)
        {
            try
            {

                bool isSuccess = false;
                int fumigationRoutId = (Convert.ToInt32(id));
                if (fumigationRoutId > 0)
                {
                    var result = iDriverModuleRepo.SelectFumigationSignaturePadDetail(fumigationRoutId);
                    if (result)
                    {
                        isSuccess = true;
                    }
                }
                return Json(isSuccess, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Save Fumigation GpsTracker
        /// <summary>
        /// Save Fumigation GpsTracker
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveFumigationGPSTracker(List<SaveFumigationGpsTrackingHistoryDTO> dto)
        {

            try
            {
                bool isSuccess = false;
                if (dto != null)
                {
                    isSuccess = false;
                    foreach (var driverTrackShipment in dto)
                    {

                        driverTrackShipment.UserId = memberProfile.UserId;
                        var result = iDriverModuleRepo.SaveFumigationGPSTracker(driverTrackShipment);
                        if (result)
                        {
                            isSuccess = true;
                        }
                    }
                }
                return Json(isSuccess, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Delete row for Proof of Temp Files
        /// <summary>
        ///  Delete row for Proof of Temp Files
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>

        public ActionResult DeleteFumigationProofOfTemprature(int imageId)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();

                if (imageId > 0)
                {
                    FumigationProofOfTempEditBind objProofOfTemprature = new FumigationProofOfTempEditBind();
                    objProofOfTemprature.proofImageId = imageId;
                    objProofOfTemprature.CreatedBy = objMemberProfile.UserId;
                    objProofOfTemprature.CreatedOn = Configurations.TodayDateTime;
                    if (iDriverModuleRepo.DeleteFumigationProofOfTemprature(objProofOfTemprature))
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

        #region  Delete Damage Files
        /// <summary>
        /// Delete Damage Files
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public ActionResult DeleteFumigationDamageFiles(int imageId)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();

                if (imageId > 0)
                {
                    FumigationDamagedEditBindDto objDeleteDamageFiles = new FumigationDamagedEditBindDto();
                    objDeleteDamageFiles.DamagedID = imageId;
                    objDeleteDamageFiles.CreatedBy = objMemberProfile.UserId;
                    objDeleteDamageFiles.CreatedOn = Configurations.TodayDateTime;
                    if (iDriverModuleRepo.DeleteFumigationDamageFiles(objDeleteDamageFiles))
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

        #region Fumigation Status
        /// <summary>
        /// shipment status
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDriverFumimgationStatus()
        {
            try
            {
                var statusList = iDriverModuleRepo.GetFumigationStatusList();
                return Json(statusList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Save web-Camera  
        /// <summary>
        /// Save web-Camera  
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult saveWebCamera(SaveWebCameraDTO dto)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();
                dto.CreatedBy = objMemberProfile.UserId;
                dto.CreatedOn = Configurations.TodayDateTime;

                if (dto != null)
                {

                    #region web-Cam Base64 Covert into an image

                    FumigationProofImagesDto listCamImageDto = new FumigationProofImagesDto();
                    if (dto.ImageUrl != null)
                    {

                        string webcamimg = string.Empty;
                        var imgUrl = ImageUploader.saveImageCam(dto.ImageUrl, out webcamimg, "Images/ProofFiles");

                        if (imgUrl != null)
                        {
                            FumigationProofImagesDto images = new FumigationProofImagesDto();
                            images.ImageDescription = LarastruckingResource.ProofOfTemperature;
                            images.ImageUrl = imgUrl;
                            images.ActualTemperature = dto.ActualTemperature;
                            images.ImageName = webcamimg;
                            listCamImageDto = images;
                        }
                    }
                    dto.UploadedProofOfTempFile = listCamImageDto;

                    #endregion



                    var result = iDriverModuleRepo.saveWebCamera(dto);
                    if (result)
                    {
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

        #region Save Damage web-Camera  
        /// <summary>
        /// Save web-Camera  
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveDamageWebCamera(SaveDamageWebCamDTO dto)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();
                dto.CreatedBy = objMemberProfile.UserId;
                dto.CreatedOn = Configurations.TodayDateTime;

                if (dto != null)
                {

                    #region web-Cam Base64 Covert into an image


                    FumigationDamagedImagesDto ListDamagedDto = new FumigationDamagedImagesDto();
                    if (dto.ImageUrl != null)
                    {

                        string damagedfileName = string.Empty;
                        var imgUrl = ImageUploader.saveImageCam(dto.ImageUrl, out damagedfileName, "Images/DamagedFiles");

                        if (imgUrl != null)
                        {
                            FumigationDamagedImagesDto Image = new FumigationDamagedImagesDto();
                            Image.ImageDescription = dto.ImageDescription;
                            Image.ImageUrl = imgUrl;
                            Image.ImageName = damagedfileName;
                            ListDamagedDto = Image;
                        }
                    }
                    dto.UploadedDamagedFile = ListDamagedDto;

                    #endregion



                    var result = iDriverModuleRepo.SaveDamageWebCamera(dto);
                    if (result)
                    {
                        // isSuccess = true;
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

        #region Save Proof of Temperature
        /// <summary>
        ///Save Proof of Temperature
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveFumimgationProofOfTemperature(SaveFumigationDetailsDTO dto)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();
                dto.CreatedBy = objMemberProfile.UserId;
                dto.CreatedOn = Configurations.TodayDateTime;

                if (ModelState.IsValid)
                {

                    #region UPLOADED FILES : PROOF OF TEMP
                    FumigationProofImagesDto listProofOfTempDto = new FumigationProofImagesDto();
                    if (dto.UploadedTemperatureProofFiles != null)
                    {
                        string fileName = string.Empty;
                        var imgUrl = ImageUploader.Upload(dto.UploadedTemperatureProofFiles, out fileName, "Images/ProofFiles");

                        if (imgUrl != null)
                        {
                            FumigationProofImagesDto images = new FumigationProofImagesDto();
                            images.ImageDescription = LarastruckingResource.ProofOfTemperature;
                            images.ImageUrl = imgUrl;
                            images.ImageName = fileName;
                            //images.ActualTemperature = dto.ActualTemperature;
                            listProofOfTempDto = images;
                        }
                    }
                    dto.UploadedProofOfTempFile = listProofOfTempDto;
                    #endregion

                    var result = iDriverModuleRepo.SaveFumigationProofOfTemperature(dto);
                    if (result)

                    {

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

        #region Save Damaged Files
        /// <summary>
        ///  Save Damaged Files
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveFumigationDamagedFiles(SaveFumigationDetailsDTO dto)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();
                dto.CreatedBy = objMemberProfile.UserId;
                dto.CreatedOn = Configurations.TodayDateTime;

                if (ModelState.IsValid)
                {
                    #region UPLOADED FILES: Damaged 
                    List<FumigationDamagedImagesDto> ListDamagedDto = new List<FumigationDamagedImagesDto>();
                    if (dto.DamagedFiles != null)
                    {
                        foreach (var damagedUploadFile in dto.DamagedFiles)
                        {
                            string damagedfileName = string.Empty;
                            var imgUrl = ImageUploader.Upload(damagedUploadFile, out damagedfileName, "Images/DamagedFiles");
                            if (imgUrl != null)
                            {
                                FumigationDamagedImagesDto Image = new FumigationDamagedImagesDto();
                                Image.ImageDescription = dto.ImageDescription;
                                Image.ImageUrl = imgUrl;
                                Image.ImageName = damagedfileName;
                                ListDamagedDto.Add(Image);
                            }
                        }
                    }

                    #endregion
                    dto.UploadedDamagedFile = ListDamagedDto;

                    var result = iDriverModuleRepo.SaveFumigationDamagedFiles(dto);
                    if (result)

                    {

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

        #region Save Fumigation Waiting Time
        /// <summary>
        /// Save Fumigation Waiting Time
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveFumigationWaitingTime(SaveFumigationWaitingNotifiDto dto)
        {
            try
            {
                var response = iDriverModuleRepo.SaveFumigationWaitingTime(dto);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region check Status
        /// <summary>
        /// Check Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IsStatusExist(int statusId, int fumigationId)
        {
            var response = iDriverModuleRepo.IsStatusExist(statusId, fumigationId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Fumigation Last Status
        /// <summary>
        /// Get Fumigation Last Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetLastStatus(int statusId, int fumigationId)
        {
            var response = iDriverModuleRepo.GetLastStatus(statusId, fumigationId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Shipment Last Status
        /// <summary>
        /// Get Shipment Last Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetShipmentLastStatus(int statusId, int shipmentId)
        {
            var response = iDriverModuleRepo.GetShipmentLastStatus(statusId, shipmentId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Validate Reuired Field
        /// <summary>
        /// Check Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ValidateReuiredField(int statusId, int fumigationId)
        {
            var response = iDriverModuleRepo.ValidateReuiredField(statusId, fumigationId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion
    }
}

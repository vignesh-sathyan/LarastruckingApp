using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.Common;
using LarastruckingApp.DTO;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Infrastructure;
using LarastruckingApp.Resource;
using LarastruckingApp.Utility;
using LarastruckingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LarastruckingApp.Controllers
{
    [Authorize]
    public class AccidentReportController : BaseController
    {

        #region Private Member
        /// <summary>
        /// Defining private members
        /// </summary>
        private IAccidentReportBAL iAccidentRepo;
        private IEquipmentBAL iEquipmentRepo;
        EncryptAndDecrypt encryptAndDecrypt = new EncryptAndDecrypt();
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor injection
        /// </summary>
        /// <param name="iAccidentReportBAL"></param>
        /// <param name="iEquipmentBAL"></param>
        public AccidentReportController(IAccidentReportBAL iAccidentReportBAL, IEquipmentBAL iEquipmentBAL)
        {
            iAccidentRepo = iAccidentReportBAL;
            iEquipmentRepo = iEquipmentBAL;
        }
        #endregion

        #region Index
        /// <summary>
        /// Accident report index method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [CustomAuthorize]
        public ActionResult Index(string id)
        {
            try
            {
                int accidentReportId = 0;
                if (!String.IsNullOrEmpty(id))
                {
                    accidentReportId = Convert.ToInt32(encryptAndDecrypt.DecryptDES(id));
                }
                AccidentReportViewModel objAccidentReportViewModel = new AccidentReportViewModel();
                objAccidentReportViewModel.AccidentReportId = accidentReportId;
                return View(objAccidentReportViewModel);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region SaveAccidentReport
        /// <summary>
        /// Action mehtod for save Report Accident
        /// </summary>
        /// <param name="accidentReportViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveAccidentReport(AccidentReportViewModel accidentReportViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MemberProfile objMemberProfile = new MemberProfile();

                    AccidentReportDTO objAccidentReportDTO = AutoMapperServices<AccidentReportViewModel, AccidentReportDTO>.ReturnObject(accidentReportViewModel);
                    objAccidentReportDTO.AccidentDate = Configurations.ConvertLocalToUTC(Convert.ToDateTime(accidentReportViewModel.AccidentDate));
                    objAccidentReportDTO.CreatedBy = objMemberProfile.UserId;
                    var result = iAccidentRepo.Add(objAccidentReportDTO);
                    if (result.IsSuccess)
                    {

                        JsonResponse objJsonResponse = new JsonResponse();
                        objJsonResponse.IsSuccess = true;
                        objJsonResponse.Data = result.AccidentReportId;
                        objJsonResponse.Message = LarastruckingResource.msgAddAccidentReport;
                        TempData["SuccessMessage"] = LarastruckingResource.DataSaveSuccessfully;
                        return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return View(accidentReportViewModel);
                    }

                }
                else
                {
                    return View(accidentReportViewModel);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region UpdateAccidentReport
        /// <summary>
        /// Action method for Update Report Accident
        /// </summary>
        /// <param name="accidentReportViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateAccidentReport(AccidentReportViewModel accidentReportViewModel)
        {
            try
            {
                if (accidentReportViewModel.AccidentReportId > 0)
                {
                    MemberProfile objMemberProfile = new MemberProfile();
                    AccidentReportDTO objAccidentReportDTO = AutoMapperServices<AccidentReportViewModel, AccidentReportDTO>.ReturnObject(accidentReportViewModel);
                    objAccidentReportDTO.AccidentDate = Configurations.ConvertLocalToUTC(Convert.ToDateTime(accidentReportViewModel.AccidentDate));
                    objAccidentReportDTO.ModifiedBy = objMemberProfile.UserId;
                    objAccidentReportDTO.ModifiedDate = DateTime.UtcNow;
                    var result = iAccidentRepo.Update(objAccidentReportDTO);
                    if (result.IsSuccess)
                    {
                        JsonResponse objJsonResponse = new JsonResponse();
                        objJsonResponse.Data = accidentReportViewModel.AccidentReportId;
                        objJsonResponse.IsSuccess = true;
                        TempData["SuccessMessage"] = LarastruckingResource.DataUpdateSuccessfully;
                        return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return View(accidentReportViewModel);
                    }
                }
                else
                {
                    return View(accidentReportViewModel);
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
            try
            {


                string ext = "." + imageName.Split('.')[1];
                string base64 = baseCode.Split(',')[1];
                byte[] imageBytes = Convert.FromBase64String(base64);
                string path = System.Web.HttpContext.Current.Server.MapPath($"~/{Configurations.FUAccidentReportPath}");
                var str = Guid.NewGuid().ToString("N").Substring(0, 5) + DateTime.Now.ToString("ddMMyyhhmms") + ext;
                string imageNameWithPath = path + "/" + str;


                System.IO.File.WriteAllBytes(imageNameWithPath, imageBytes);
                string returnpath = $"/{Configurations.FUAccidentReportPath}" + str;
                return returnpath;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region GetDocumetTypeList
        /// <summary>
        /// Action method for get document type list
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDocumetTypeList()
        {
            try
            {
                var accidentDocument = iAccidentRepo.AccidentDocumentList();
                return Json(accidentDocument, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region GetDriverList
        /// <summary>
        /// Driver List
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDriverList()
        {
            try
            {
                var driverlist = iAccidentRepo.GetDriverList().ToList();
                return Json(driverlist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region ViewAccidentReport
        /// <summary>
        /// Action method for load View Accident Report
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [CustomAuthorize]
        public ActionResult ViewAccidentReport()
        {
            return View();

        }
        #endregion

        #region LoadData
        /// <summary>
        /// Action Method for bind View Accident Report
        /// </summary>
        /// <returns></returns>        
        [HttpPost]
        public ActionResult LoadData()
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();

                UserDTO _user = new UserDTO();
                _user.Userid = objMemberProfile.UserId;
                _user.RoleName = objMemberProfile.UserRole;

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
                List<AccidentReportDTO> lstAccidentReportDTO = iAccidentRepo.ViewAccidentReport(_user);
                var accidentReportList = lstAccidentReportDTO;
                if (!string.IsNullOrEmpty(search))
                {
                    accidentReportList = accidentReportList.Where(x => x.VIN.ToUpper().Contains(search.ToUpper())
                    || x.VehicleType.ToUpper().Contains(search.ToUpper())
                    || x.LicencePlate.ToUpper().Contains(search.ToUpper())
                    //|| x.Year == (Convert.ToInt32( search))
                    || x.DriverName.ToUpper().Contains(search.ToUpper())
                    ).ToList();
                }
                recordsTotal = accidentReportList.Count();
                var data = accidentReportList.ToList();
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

        #region SearchByLicensePlate
        /// <summary>
        /// Search By License Plate No
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchByLicensePlate(string search)
        {
            try
            {
                IEnumerable<EquipmentDTO> lstEquipmentDTO = iEquipmentRepo.SearchByLicensePlate(search).ToList();
                return Json(lstEquipmentDTO, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region SearchByVIN
        /// <summary>
        /// Search By VIN No.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchByVIN(string search)
        {
            try
            {
                IEnumerable<EquipmentDTO> lstEquipmentDTO = iEquipmentRepo.SearchByVIN(search).ToList();
                return Json(lstEquipmentDTO, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Search by equipment no.
        /// <summary>
        /// Search By Equipment No.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchByEquipmentNo(string search)
        {
            try
            {
                IEnumerable<EquipmentDTO> lstEquipmentDTO = iEquipmentRepo.SearchByEquipmentNo(search).ToList();
                return Json(lstEquipmentDTO, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region DeleteAccidentReport
        /// <summary>
        /// Delete Report Accident
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteAccidentReport(int id)
        {

            try
            {
                int accidentReportId = id;
                //if (!String.IsNullOrEmpty(id))
                //{
                //    accidentReportId = Convert.ToInt32(encryptAndDecrypt.DecryptDES(id));
                //}
                AccidentReportDTO objAccidentReportDTO = new AccidentReportDTO();
                objAccidentReportDTO.AccidentReportId = accidentReportId;
                JsonResponse objJsonResponse = new JsonResponse();
                if (iAccidentRepo.Delete(objAccidentReportDTO))
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetReportAccidentById
        /// <summary>
        /// Get Report Accident By Id FOR Edit
        /// </summary>
        /// <param name="AccidentReportId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetReportAccidentById(int AccidentReportId)
        {
            try
            {
                var reportAccidentdata = iAccidentRepo.FindById(AccidentReportId);
                if (reportAccidentdata != null)
                {
                    return Json(reportAccidentdata, JsonRequestBehavior.AllowGet);
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

        #region Upload AccidentDocument        
        /// <summary>
        /// Upload AccidentDocument
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadAccidentDocument(UploadAccidentDocumentViewModel model)
        {
            try
            {


                MemberProfile objMemberProfile = new MemberProfile();
                bool isSuccess = false;
                if (model.AccidentImage != null)
                {
                    foreach (var accidetnImg in model.AccidentImage)
                    {
                        isSuccess = false;
                        string damagedfileName = string.Empty;
                        var imgUrl = ImageUploader.Upload(accidetnImg, out damagedfileName, Configurations.FUAccidentReportPath);
                        if (!string.IsNullOrEmpty(imgUrl))
                        {
                            AccidentReportDocumentDTO objAccidentReportDocumentDTO = new AccidentReportDocumentDTO();
                            objAccidentReportDocumentDTO.AccidentReportId = model.AccidentReportId;
                            objAccidentReportDocumentDTO.CreatedBy = objMemberProfile.UserId;
                            objAccidentReportDocumentDTO.ImageURL = imgUrl;
                            objAccidentReportDocumentDTO.ImageName = accidetnImg.FileName;
                            objAccidentReportDocumentDTO.AccidentDocumentId = model.AccidentDocumentId;
                            objAccidentReportDocumentDTO.DocumentName = model.DocumentName;

                            if (iAccidentRepo.AddAccidentDocument(objAccidentReportDocumentDTO).IsSuccess)
                            {
                                isSuccess = true;
                            }

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

        #region delete accident document
        /// <summary>
        ///delete accident document
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public ActionResult DeleteAccidentDocument(string documentId)
        {
            try
            {
                int docId = 0;
                if (!String.IsNullOrEmpty(documentId))
                {
                    docId = Convert.ToInt32(encryptAndDecrypt.DecryptDES(documentId));
                }

                JsonResponse objJsonResponse = new JsonResponse();

                if (docId > 0)
                {

                    if (iAccidentRepo.DeleteDoucument(docId))
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

        #region  view accident document
        /// <summary>
        /// view accident document
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize]
        public ActionResult ViewAccidentDocument(string Id)
        {
            try
            {
                int docId = 0;
                if (!String.IsNullOrEmpty(Id))
                {
                    docId = Convert.ToInt32(encryptAndDecrypt.DecryptDES(Id));
                }

                var reportData = iAccidentRepo.ViewAccidentReportDocument(docId);
                return View(reportData);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

    }
}
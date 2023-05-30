using Excel;
using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Infrastructure;
using LarastruckingApp.Resource;
using LarastruckingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LarastruckingApp.Controllers
{
    [Authorize]
    public class EquipmentController : BaseController
    {
        #region Private Member
        /// <summary>
        /// Defining private member
        /// </summary>
        private IEquipmentBAL iEquipmentRepo;
        private IQuoteBAL quoteBAL;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor injection
        /// </summary>
        /// <param name="iEquipmentBAL"></param>
        public EquipmentController(IEquipmentBAL iEquipmentBAL, IQuoteBAL iQuoteBAL)
        {
            iEquipmentRepo = iEquipmentBAL;
            quoteBAL = iQuoteBAL;
        }
        #endregion

        #region Equipment Index 
        /// <summary>
        /// Equipment Index 
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize]
        public ActionResult Index()
        {
            ModelState.Clear();
            return View();
        }
        #endregion

        #region GetEquipmentById
        /// <summary>
        /// Get Equipment Data By d
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ///
        public ActionResult GetEquipmentById(int id = 0)
        {
            try
            {
                EquipmentViewModel objEquipmentViewModel = new EquipmentViewModel();
                if (id > 0)
                {
                    var data = iEquipmentRepo.FindById(id);
                    objEquipmentViewModel = AutoMapperServices<EquipmentDTO, EquipmentViewModel>.ReturnObject(data);
                }
                return View(objEquipmentViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region BindVehicleType
        /// <summary>
        /// Bind Equipment Type
        /// </summary>
        /// <returns></returns>
        public List<VehicleTypeViewModel> BindVehicleType()
        {
            List<VehicleTypeViewModel> lstVehicleTypeViewModel = null;
            try
            {
                lstVehicleTypeViewModel = AutoMapperServices<VehicleTypeDTO, VehicleTypeViewModel>.ReturnObjectList(iEquipmentRepo.BindVehicleType().ToList());
            }
            catch (Exception)
            {
                throw;
            }
            return lstVehicleTypeViewModel;
        }
        #endregion

        #region AddEquipment: HTTP GET
        /// <summary>
        /// AddEquipment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize]
        public ActionResult AddEquipment(int id = 0)
        {

            EquipmentViewModel dto = new EquipmentViewModel();
            dto.EDID = id;

            DropDownBind(dto);

            ViewBag.VehicleType = BindVehicleType();
            return View(dto);

        }
        #endregion

        #region Private DroopDown Binding
        private void DropDownBind(EquipmentViewModel dto)
        {
            var equipmentTypes = iEquipmentRepo.BindVehicleType().ToList();
            var freightstypes = quoteBAL.GetFreightTypeList();
            var doorTypes = iEquipmentRepo.GetDoorTypes();

            dto.GetEquipmentTypes.AddRange(equipmentTypes);
            dto.GetFreightTypes.AddRange(freightstypes);
            dto.GetDoorTypes.AddRange(doorTypes);
        }
        #endregion

        #region AddEquipment: HTTP POST
        /// <summary>
        /// Create and Update Equipment Controller
        /// </summary>
        /// <param name="equipmentViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEquipment(EquipmentViewModel equipmentViewModel, HttpPostedFileBase RegistrationImageURL, HttpPostedFileBase InsuranceImageURL)
        {
            try
            {
                MemberProfile mp = new MemberProfile();
                DropDownBind(equipmentViewModel);

                ViewBag.VehicleType = BindVehicleType();
                if (ModelState.IsValid)
                {

                    if (equipmentViewModel.EDID > 0)//For Update
                    {
                        EquipmentDTO objEquipmentDTOUpdate = AutoMapperServices<EquipmentViewModel, EquipmentDTO>.ReturnObject(equipmentViewModel);
                        if (RegistrationImageURL != null)
                        {
                            objEquipmentDTOUpdate.RegistrationImageURL = ConvertBase64toImage(RegistrationImageURL);
                            objEquipmentDTOUpdate.RegistrationImageName = RegistrationImageURL.FileName;
                        }
                        else
                        {
                            objEquipmentDTOUpdate.RegistrationImageURL = equipmentViewModel.hdnRegistrationImageURL;
                            objEquipmentDTOUpdate.RegistrationImageName = equipmentViewModel.hdnRegistrationImageName;
                        }
                        if (InsuranceImageURL != null)
                        {
                            objEquipmentDTOUpdate.InsuranceImageURL = ConvertBase64toImage(InsuranceImageURL);
                            objEquipmentDTOUpdate.InsauranceImageName = InsuranceImageURL.FileName;
                        }
                        else
                        {
                            objEquipmentDTOUpdate.InsuranceImageURL = equipmentViewModel.hdnInsuranceImageURL;
                            objEquipmentDTOUpdate.InsauranceImageName = equipmentViewModel.hdnInsuranceImageName;
                        }

                        objEquipmentDTOUpdate.ModifiedBy = mp.UserId;
                        objEquipmentDTOUpdate.ModifiedOn = Configurations.TodayDateTime;

                        var userDetail = iEquipmentRepo.Update(objEquipmentDTOUpdate);

                        if (userDetail.IsSuccess)
                        {
                            TempData["SuccessMessage"] = LarastruckingResource.DataUpdateSuccessfully;
                            return RedirectToAction("ViewEquipment");
                        }
                    }
                    else
                    {
                        EquipmentDTO objEquipmentDTO = AutoMapperServices<EquipmentViewModel, EquipmentDTO>.ReturnObject(equipmentViewModel);
                        objEquipmentDTO.CreatedBy = mp.UserId;
                        objEquipmentDTO.CreatedOn = Configurations.TodayDateTime;
                        objEquipmentDTO.ModifiedBy = mp.UserId;
                        objEquipmentDTO.ModifiedOn = Configurations.TodayDateTime;

                        if (RegistrationImageURL != null)
                        {
                            objEquipmentDTO.RegistrationImageURL = ConvertBase64toImage(RegistrationImageURL);
                            objEquipmentDTO.RegistrationImageName = RegistrationImageURL.FileName;

                        }
                        if (InsuranceImageURL != null)
                        {
                            objEquipmentDTO.InsuranceImageURL = ConvertBase64toImage(InsuranceImageURL);
                            objEquipmentDTO.InsauranceImageName = InsuranceImageURL.FileName;

                        }

                        if (iEquipmentRepo.Add(objEquipmentDTO).IsSuccess == true)
                        {
                            TempData["SuccessMessage"] = LarastruckingResource.DataSaveSuccessfully;
                            return RedirectToAction("ViewEquipment");
                        }
                    }
                }
                else
                {
                    string messages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                    TempData["ErrorMessage"] = LarastruckingResource.ErrorOccured;
                    return View(equipmentViewModel);
                }
            }
            catch (Exception)
            {
                return View(equipmentViewModel);
            }
            return View();
        }
        #endregion

        #region ConvertBase64toImage
        /// <summary>
        /// Convert Base64 to Image
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string ConvertBase64toImage(HttpPostedFileBase file)
        {
            try
            {
                var InputFileName = Path.GetFileName(file.FileName);
                string ext = InputFileName.Split('.')[1];
                var str = Guid.NewGuid().ToString("N").Substring(0, 5) + DateTime.Now.ToString("ddMMyyhhmms") + "." + ext;
                var ServerSavePath = Path.Combine(Server.MapPath($"~/{Configurations.FUEquipmentPath}") + str);
                file.SaveAs(ServerSavePath);
                string returnpath = $"/{Configurations.FUEquipmentPath}" + InputFileName;
                return returnpath;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region EditEquipment
        /// <summary>
        /// Edit Equipment Data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditEquipment(int id)
        {
            try
            {
                EquipmentViewModel objeEquipmentViewModel = new EquipmentViewModel();
                var equipmentData = iEquipmentRepo.FindById(id);
                if (equipmentData.MaxLoad != null)
                {
                    equipmentData.MaxLoad = equipmentData.MaxLoad.Replace(",", Environment.NewLine);
                }

                objeEquipmentViewModel = AutoMapperServices<EquipmentDTO, EquipmentViewModel>.ReturnObject(equipmentData);
                return new JsonResult()
                {
                    Data = objeEquipmentViewModel,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = Int32.MaxValue
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ViewEquipment
        /// <summary>
        /// Get Equipment inserted data
        /// </summary>
        /// <returns></returns>       
        [CustomAuthorize]
        public ActionResult ViewEquipment()
        {
            return View();
        }
        #endregion

        #region EquipmentDelete
        /// <summary>
        /// Driver Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EquipmentDelete(int id = 0)
        {
            JsonResponse objJsonResponse = new JsonResponse();
            try
            {
                EquipmentDTO equipmentDTO = new EquipmentDTO();
                equipmentDTO.EDID = id;
                if (iEquipmentRepo.Delete(equipmentDTO))
                {
                    objJsonResponse.IsSuccess = true;
                    objJsonResponse.Message = LarastruckingResource.DataDeleteSuccessfully;
                }
                else
                {
                    objJsonResponse.Message = LarastruckingResource.ErrorOccured;
                }
                return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        public ActionResult LoadData()
        {
            try
            {
                MemberProfile mp = new MemberProfile();
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

                IEnumerable<EquipmentDTO> lstEquipmentDTO = iEquipmentRepo.List.Where(x=>(mp.UserRole.ToLower().Trim()==("Mechanics".ToLower().Trim()))?(x.Active==true):1==1).ToList();
                List<EquipmentViewModel> lstDriverViewModel = AutoMapperServices<EquipmentDTO, EquipmentViewModel>.ReturnObjectList(lstEquipmentDTO.ToList());

              if (!string.IsNullOrEmpty(search))
                {
                    lstDriverViewModel = lstDriverViewModel.Where(x => x.Model.ToUpper().Contains(search.ToUpper()) ||
                                                                     x.Year.ToString().Contains(search.ToUpper()) ||
                                                                     x.RollerBed.ToUpper().Contains(search.ToUpper()) ||
                                                                    x.EquipmentNo.ToUpper().Contains(search.ToUpper()) ||
                                                                    x.FreightType.ToUpper().Contains(search.ToUpper()) ||
                                                                    x.LicencePlate.ToUpper().Contains(search.ToUpper()) ||
                                                                    x.MaxLoad.ToUpper().Contains(search.ToUpper()) ||
                                                                    x.Ownedby.ToUpper().Contains(search.ToUpper()) ||
                                                                    x.VehicleTypeName.ToUpper().Contains(search.ToUpper()) ||
                                                                    x.VIN.ToUpper().Contains(search.ToUpper()

                                                                    )).ToList();
                }
                recordsTotal = lstDriverViewModel.Count();
                var data = lstDriverViewModel.ToList();

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

        #region Check Duplicate Equipment No.
        /// <summary>
        /// Check Duplicate Equipment No.
        /// </summary>
        /// <param name="equipmentNo"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult CheckEquipmentNo(string equipmentNo, int equipmentId)
        {
            try
            {
                return Json(iEquipmentRepo.CheckEquipmentNo(equipmentNo, equipmentId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region import excel 

        /// <summary>
        /// Import Equipment by excel 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ImportEquipments()
        {
            return View();

        }

        /// <summary>
        /// Import Equipment
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImportEquipments(HttpPostedFileBase file)
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

                    List<VehicleTypeViewModel> VehicleTypeList = BindVehicleType();
                    //... //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                    //DataSet result = excelReader.AsDataSet();
                    //... //4. DataSet - Create column names from first row
                    excelReader.IsFirstRowAsColumnNames = true;
                    DataSet result = excelReader.AsDataSet();

                    MemberProfile mp = new MemberProfile();
                    //string ConStr = string.Empty;
                    //string path = Server.MapPath("ADDRESSESFILE.xlsx");
                    //connection string for that file which extantion is .xlsx    
                   // ConStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;ReadOnly=False;HDR=Yes;\"";


                    foreach (DataRow row in result.Tables[0].Rows)
                    {
                        try
                        {

                            EquipmentDTO Equipment = new EquipmentDTO();
                            string VehicleType = Convert.ToString(row["Equipment Type"]);
                            var data = VehicleTypeList.Where(x => x.VehicleTypeName.ToLower().Trim().Contains(VehicleType.ToLower().Trim())).Select(x => x.VehicleTypeID).FirstOrDefault();
                            Equipment.VehicleType = Convert.ToInt32(data);
                            Equipment.EquipmentNo = Convert.ToString(row["Equipment Number"]);
                            Equipment.LicencePlate = Convert.ToString(row["License Plate"]);
                            Equipment.Decal = Convert.ToInt32(row["Decal"]);
                            Equipment.RegistrationExpiration = Convert.ToDateTime(row["Registration Expiration"]);
                            Equipment.Year = Convert.ToInt32(row["Year"]);
                            Equipment.Make = Convert.ToString(row["Make"]);
                            Equipment.Color = Convert.ToString(row["Color"]);
                            Equipment.VIN = Convert.ToString(row["VIN"]);
                            Equipment.CubicFeet = Convert.ToString(row["Capacity "]);
                            Equipment.LDimension = Convert.ToString(row["L"]);
                            Equipment.WDimension = Convert.ToString(row["w"]);
                            Equipment.HDimension = Convert.ToString(row["H"]);
                            // Equipment.DoorTypeIds.Add(Convert.ToString(row["Door Type"]));
                            Equipment.FreightTypeIds.Add(1);
                            Equipment.MaxLoad = Convert.ToString(row["Max Load"]);
                            Equipment.RollerBed = Convert.ToString(row["Bed"]);
                            Equipment.Ownedby = Convert.ToString(row["Owned by"]);
                            Equipment.CreatedBy = mp.UserId;
                            Equipment.CreatedOn = Configurations.TodayDateTime;
                            Equipment.ModifiedBy = mp.UserId;
                            Equipment.ModifiedOn = Configurations.TodayDateTime;

                            iEquipmentRepo.Add(Equipment);
                        }
                        catch (Exception)
                        {
                            throw;
                            ////making query    
                            //string query = "INSERT INTO [Sheet1$] ([ADDRESS TYPE], [COMPANY'S NAME]) VALUES('" + row["ADDRESS TYPE"] + "','" + row["COMPANY'S NAME"] + "')";
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
                    return RedirectToAction("ViewEquipment");


                }
            }
            catch (Exception)
            {

            }
            return View();

        }

        #endregion
    }
}

using Excel;
using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Infrastructure;
using LarastruckingApp.Log.Utility;
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
    public class AddressController : BaseController
    {
        #region Private Members
        /// <summary>
        /// Defining private members
        /// </summary>
        private IAddressBAL iAddressRepo;
        private IDriverBAL iDriverRepo;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor Injection
        /// </summary>
        /// <param name="iAddressBAL"></param>
        /// <param name="iDriverBAL"></param>
        public AddressController(IAddressBAL iAddressBAL, IDriverBAL iDriverBAL)
        {
            iAddressRepo = iAddressBAL;
            iDriverRepo = iDriverBAL;
        }
        #endregion

        #region Address View Page
        /// <summary>
        /// Load address view page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Country = iDriverRepo.GetCountryList();
            ViewBag.State = new SelectList(iDriverRepo.GetStateList(), "ID", "Name");

            return View();
        }
        #endregion

        #region Add Address
        /// <summary>
        /// Load add address page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize]

        public ActionResult AddAddress()
        {
            try
            {
                AddressViewModel objAddressViewModel = new AddressViewModel();
                ViewBag.Country = iDriverRepo.GetCountryList();
                ViewBag.State = new SelectList(iDriverRepo.GetStateList(), "ID", "Name");
                ViewBag.AddressType = BindAddressType();


                return View(objAddressViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region BindAddressType
        /// <summary>
        /// Method to bind address type
        /// </summary>
        /// <returns></returns>
        public List<AddressTypeViewModel> BindAddressType()
        {
            List<AddressTypeViewModel> lstAddressTypeViewModel = null;
            try
            {
                lstAddressTypeViewModel = AutoMapperServices<AddressTypeDTO, AddressTypeViewModel>.ReturnObjectList(iAddressRepo.BindAddressType().ToList());
            }
            catch (Exception)
            {
                throw;
            }
            return lstAddressTypeViewModel;
        }
        #endregion

        #region Add Address
        /// <summary>
        /// Action Method for add and update address
        /// </summary>
        /// <param name="objAddressViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddAddress(AddressViewModel objAddressViewModel)
        {
            try
            {
                MemberProfile mp = new MemberProfile();

                ViewBag.AddressType = BindAddressType();
                ViewBag.Country = iDriverRepo.GetCountryList();
                ViewBag.State = new SelectList(iDriverRepo.GetStateList(), "ID", "Name");
                if (ModelState.IsValid)//For Update
                {
                    if (objAddressViewModel.AddressId > 0)
                    {
                        AddressDTO objAddressDTOUpdate = AutoMapperServices<AddressViewModel, AddressDTO>.ReturnObject(objAddressViewModel);
                        objAddressDTOUpdate.ModifiedBy = mp.UserId;
                        objAddressDTOUpdate.ModifiedOn = Configurations.TodayDateTime;

                        var AddressUpdateDetail = iAddressRepo.Update(objAddressDTOUpdate);
                        if (AddressUpdateDetail.IsSuccess == true)
                        {
                            TempData["SuccessMessage"] = LarastruckingResource.DataUpdateSuccessfully;
                            return RedirectToAction("AddAddress");
                        }
                    }
                    else
                    {//for Add
                        AddressDTO objAddressDTO = AutoMapperServices<AddressViewModel, AddressDTO>.ReturnObject(objAddressViewModel);
                        objAddressDTO.CreatedBy = mp.UserId;
                        objAddressDTO.CreatedOn = Configurations.TodayDateTime;
                        objAddressDTO.ModifiedBy = mp.UserId;
                        objAddressDTO.ModifiedOn = Configurations.TodayDateTime;

                        var AddressUpdateDetail = iAddressRepo.Add(objAddressDTO);
                        if (AddressUpdateDetail.IsSuccess == true)
                        {
                            TempData["SuccessMessage"] = LarastruckingResource.DataSaveSuccessfully;
                            return RedirectToAction("AddAddress");
                        }
                    }

                }
                else
                {
                    return View(objAddressViewModel);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }
        #endregion

        #region GetAddressList
        /// <summary>
        /// Method to get Get Address List
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAddressList()
        {
            try
            {
                return PartialView("_AddressDataList");
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region EditAddress
        /// <summary>
        /// Edit Address
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditAddress(int id)
        {
            try
            {
                AddressViewModel objAddressViewModel = new AddressViewModel();
                var addressData = iAddressRepo.FindById(id);
                objAddressViewModel = AutoMapperServices<AddressDTO, AddressViewModel>.ReturnObject(addressData);
                return new JsonResult()
                {
                    Data = objAddressViewModel,
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

        #region Delete Address
        /// <summary>
        /// Method to delete address
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteAddress(int id = 0)
        {

            JsonResponse objJsonResponse = new JsonResponse();
            try
            {
                AddressDTO objAddressDTO = new AddressDTO();

                objAddressDTO.AddressId = id;
                if (iAddressRepo.Delete(objAddressDTO))
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

        #region Load Data
        /// <summary>
        /// Load address data
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadData()
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

            IEnumerable<AddressDTO> lstAddressDTO = iAddressRepo.List;

            List<AddressViewModel> lstDriverViewModel = AutoMapperServices<AddressDTO, AddressViewModel>.ReturnObjectList(lstAddressDTO.ToList());

            if (!string.IsNullOrEmpty(search))
            {
                lstDriverViewModel = lstDriverViewModel.Where(x => x.AddressTypeName.ToUpper().Contains(search.ToUpper())
                                                              || x.Address1.ToUpper().Contains(search.ToUpper())
                                                              || x.Phone.ToUpper().Contains(search.ToUpper())
                                                              || x.CompanyName.ToUpper().Contains(search.ToUpper())
                                                              || x.Zip.ToUpper().Contains(search.ToUpper())
                                                                || x.StateName.ToUpper().Contains(search.ToUpper())
                                                                 || x.Email.ToUpper().Contains(search.ToUpper())
                                                              || x.City.ToUpper().Contains(search.ToUpper())
                                                                 | x.CompanyNickname.ToUpper().Contains(search.ToUpper())).ToList();
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
        #endregion

        #region Find Address auto-complete
        /// <summary>
        /// List of Address
        /// </summary>
        [AllowAnonymous]
        public JsonResult GetAddress(string searchText)
        {
            var addresses = iAddressRepo.GetAddress(searchText);
            return Json(addresses, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region import address excel sheet
        /// <summary>
        /// Get method
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ImportClients()
        {
            return View();

        }
        #endregion

        #region import address excel sheet post method
        /// <summary>
        /// import file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImportClients(HttpPostedFileBase file)
        {
            try
            {
                //var request = Request.Files;
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

                    List<AddressTypeViewModel> addressTypeList = BindAddressType();
                    //... //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                    //DataSet result = excelReader.AsDataSet();
                    //... //4. DataSet - Create column names from first row
                    excelReader.IsFirstRowAsColumnNames = true;
                    DataSet result = excelReader.AsDataSet();
                    //          List<AddressDTO> userExceptionList = new List<AddressDTO>();
                    //          List<AddressDTO> AddressList = new List<AddressDTO>();
                    MemberProfile mp = new MemberProfile();
                    // string ConStr = string.Empty;
                    //string path = Server.MapPath("ADDRESSESFILE.xlsx");
                    //connection string for that file which extantion is .xlsx    
                    // ConStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;ReadOnly=False;HDR=Yes;\"";


                    foreach (DataRow row in result.Tables[0].Rows)
                    {
                        try
                        {

                            AddressDTO address = new AddressDTO();
                            string addressType = Convert.ToString(row["ADDRESS TYPE"]);
                            var data = addressTypeList.Where(x => x.AddressTypeName.ToLower().Trim().Contains(addressType.ToLower().Trim())).Select(x => x.AddressTypeID).FirstOrDefault();
                            address.AddressTypeId = Convert.ToInt32(data);
                            address.CompanyName = Convert.ToString(row["COMPANY'S NAME"]);
                            address.Address1 = Convert.ToString(row["ADDRESS 1"]);
                            address.Address2 = Convert.ToString(row["ADDRESS 2"]);
                            address.Country = 231;
                            address.State = 3929;
                            address.City = Convert.ToString(row["CITY"]);
                            address.Zip = Convert.ToString(row["ZIP CODE"]);
                            address.Phone = Convert.ToString(row["PHONE NUMBER"]);
                            address.AdditionalPhone1 = Convert.ToString(row["ADDITIONAL PHONES"]);
                            address.AdditionalPhone2 = Convert.ToString(row["ADDITIONAL PHONES2"]);
                            address.ContactPerson = Convert.ToString(row["CONTACT PERSON"]);
                            address.Extension = Convert.ToString(row["EXTENSION"]);
                            address.AdditionalPhone1 = Convert.ToString(row["ADDITIONAL PHONES"]);
                            address.CreatedBy = mp.UserId;
                            address.CreatedOn = Configurations.TodayDateTime;
                            address.ModifiedBy = mp.UserId;
                            address.ModifiedOn = Configurations.TodayDateTime;

                            iAddressRepo.Add(address);
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
                    return RedirectToAction("AddAddress");


                }
            }
            catch (Exception)
            {

            }
            return View();

        }
        #endregion

        [HttpGet]
        public ActionResult TimeZone()
        {
            DateTime currentime = DateTime.Now;
            DateTime utcTime = Configurations.ConvertLocalToUTC(currentime);
            DateTime estTime = Configurations.ConvertDateTime(utcTime);
            ViewBag.currentime = currentime;
            ViewBag.utcTime = utcTime;
            ViewBag.estTime = estTime;


            return View();
        }
    }
}
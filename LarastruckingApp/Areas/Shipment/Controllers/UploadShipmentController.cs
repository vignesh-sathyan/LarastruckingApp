using Excel;
using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.Controllers;
using LarastruckingApp.DAL.Interface;
using LarastruckingApp.DTO;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Enum;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Infrastructure;
using LarastruckingApp.Resource;
using LarastruckingApp.ViewModel;
using LarastruckingApp.ViewModel.Shipment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace LarastruckingApp.Areas.Shipment.Controllers
{
    [Authorize]
    public class UploadShipmentController : BaseController
    {
        // GET: Shipment/UploadShipment
        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> weightUnit = from Unit qt in Enum.GetValues(typeof(Unit))
                                                     select new SelectListItem
                                                     {
                                                         Text = qt.ToString(),
                                                         Value = Convert.ToInt32(qt).ToString()

                                                     };
            ViewBag.WeightUnit = new SelectList(weightUnit, "Value", "Text");
            return View();
        }

        #region Private Member
        /// <summary>
        /// Defining private members
        /// </summary>
        private IUploadShipmentBAL uploadShipmentBAL;
        private MemberProfile memberProfile = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="IUploadShipmentBAL"></param>
        public UploadShipmentController(IUploadShipmentBAL IUploadShipmentBAL)
        {
            uploadShipmentBAL = IUploadShipmentBAL;
            memberProfile = new MemberProfile();

        }
        #endregion

        #region GetCompanyName
        /// <summary>
        /// Get Company name
        /// </summary>
        /// <returns></returns
        public ActionResult GetCompanyName()
        {
            int customerId = 0;
            UserRoleDTO dto = new UserRoleDTO();

            if (memberProfile.UserRole == "Customer")
            {
                dto.UserID = memberProfile.UserId;
                dto.RoleName = memberProfile.UserRole;
            }
            else
            {
                dto.UserID = customerId;
            }
            var data = uploadShipmentBAL.GetCompanyName(dto);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetSample
        /// <summary>
        /// Get shipment sample by address id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUploadSample()
        {
            string samplePath = uploadShipmentBAL.GetSample();
            return Json(samplePath, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region import address excel sheet post method
        /// <summary>
        /// import file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file, int? customerId)
        {
            var data = new List<UploadShipmentDTO>();
            try
            {
                IEnumerable<SelectListItem> weightUnit = from Unit qt in Enum.GetValues(typeof(Unit))
                                                         select new SelectListItem
                                                         {
                                                             Text = qt.ToString(),
                                                             Value = Convert.ToInt32(qt).ToString()

                                                         };
                ViewBag.WeightUnit = new SelectList(weightUnit, Configurations.value, Configurations.text);
                //var request = Request.Files;
                if (file != null && file.ContentLength > 0)
                {
                    IExcelDataReader excelReader = null;
                    //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                    string extetion = Path.GetExtension(file.FileName);
                    if (extetion.ToLower() == Configurations.xls)
                    {
                        excelReader = ExcelReaderFactory.CreateBinaryReader(file.InputStream);
                    }
                    else if (extetion.ToLower() == Configurations.xlsx)
                    {
                        //... //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(file.InputStream);
                    }


                    //... //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                    //DataSet result = excelReader.AsDataSet();
                    //... //4. DataSet - Create column names from first row
                    excelReader.IsFirstRowAsColumnNames = true;
                    DataSet result = excelReader.AsDataSet();



                    //connection string for that file which extantion is .xlsx    

                    List<UploadShipmentDTO> uploadShipmentList = new List<UploadShipmentDTO>();
                    if (result != null)
                    {
                        if (customerId > 0)
                        {
                            try
                            {
                                foreach (DataRow row in result.Tables[0].Rows)
                                {
                                    if (row[Configurations.date] != DBNull.Value)
                                    {
                                        MemberProfile objMemberProfile = new MemberProfile();
                                        UploadShipmentDTO uploadShipmentObj = new UploadShipmentDTO();
                                        uploadShipmentObj.Date = Convert.ToDateTime(row[Configurations.date]);
                                        uploadShipmentObj.ETA = Convert.ToString(row[Configurations.eTA_HH_MM]) ?? string.Empty;
                                        DateTime eta = DateTime.Now;
                                        if (!String.IsNullOrEmpty(uploadShipmentObj.ETA))
                                        {
                                            eta = Convert.ToDateTime(uploadShipmentObj.ETA);
                                            uploadShipmentObj.Date = Convert.ToDateTime(uploadShipmentObj.Date.ToString(Configurations.mmddyyyy) + " " + eta.ToString(Configurations.hhmm));
                                        }

                                        uploadShipmentObj.IsDateExpired = (DateTime.Now.Date.AddDays(-1) > uploadShipmentObj.Date.Date);
                                        DateTime? defaultdate = null;
                                        if (!String.IsNullOrEmpty(Convert.ToString(row[Configurations.delDate])))
                                        {
                                            uploadShipmentObj.DeliveryDate = Convert.ToDateTime(row[Configurations.delDate]);
                                            if (!String.IsNullOrEmpty(uploadShipmentObj.ETA))
                                            {
                                                uploadShipmentObj.DeliveryDate = Convert.ToDateTime(uploadShipmentObj.DeliveryDate.Value.ToString(Configurations.mmddyyyy) + " " + eta.ToString(Configurations.hhmm));
                                                uploadShipmentObj.DeliveryDate = uploadShipmentObj.DeliveryDate.Value.AddHours(2);
                                            }



                                            uploadShipmentObj.IsDeliveryDateExpired = (DateTime.Now.Date.AddDays(-1) > Convert.ToDateTime(uploadShipmentObj.DeliveryDate.Value.Date));
                                        }
                                        else
                                        {
                                            uploadShipmentObj.DeliveryDate = defaultdate;
                                        }

                                        uploadShipmentObj.ConsigneeNVendorName = Convert.ToString(row[Configurations.consigneeNvendor]).Trim() ?? string.Empty;
                                        uploadShipmentObj.CustomerPO = Convert.ToString(row[Configurations.customerPO]).Trim();
                                        uploadShipmentObj.OrderNo = Convert.ToString(row[Configurations.orderNo]).Trim() ?? string.Empty;
                                        uploadShipmentObj.AWB = Convert.ToString(row[Configurations.awb]).Trim();
                                        uploadShipmentObj.PickUpLocation = Convert.ToString(row[Configurations.pickupLocation]).Trim();
                                        uploadShipmentObj.DeliveryLocation = Convert.ToString(row[Configurations.deliveryLocation]).Trim();
                                        uploadShipmentObj.FreightType = Convert.ToString(row[Configurations.freightType]).Trim();
                                        uploadShipmentObj.Commodity = Convert.ToString(row[Configurations.commodity]).Trim();
                                        uploadShipmentObj.NoOfPallets = Convert.ToString(row[Configurations.noOfPallets]).Trim();

                                        if (!string.IsNullOrEmpty(Convert.ToString(row[Configurations.totalBox])))
                                        {
                                            uploadShipmentObj.PartialBox = Convert.ToInt32(row[Configurations.totalBox]);
                                        }
                                        else
                                        {
                                            uploadShipmentObj.PartialBox = 0;
                                        }

                                        if (!string.IsNullOrEmpty(Convert.ToString(row[Configurations.totalPallet])))
                                        {
                                            uploadShipmentObj.PartialPallet = Convert.ToInt32(row[Configurations.totalPallet]);
                                        }
                                        else
                                        {
                                            uploadShipmentObj.PartialPallet = 0;
                                        }


                                        if (!String.IsNullOrEmpty(Convert.ToString(row[Configurations.noOfBox])))
                                        {
                                            uploadShipmentObj.NoOfBox = Convert.ToDecimal(row[Configurations.noOfBox]);
                                        }
                                        else
                                        {
                                            uploadShipmentObj.NoOfBox = 0;
                                        };
                                        if (!String.IsNullOrEmpty(Convert.ToString(row[Configurations.weight])))
                                        {
                                            uploadShipmentObj.Weight = Convert.ToDecimal(row[Configurations.weight]);
                                        }
                                        else
                                        {
                                            uploadShipmentObj.Weight = 0;
                                        }
                                        uploadShipmentObj.Unit = Convert.ToString(row[Configurations.unitkglb]).Trim();
                                        uploadShipmentObj.PricingMethod = Convert.ToString(row[Configurations.pricingMethod]).Trim();
                                        uploadShipmentObj.ReqTemp = Convert.ToString(row[Configurations.reqTemp]).Trim();
                                        uploadShipmentObj.Comments = Convert.ToString(row[Configurations.comment]).Trim();
                                        uploadShipmentObj.RequestedBy = Convert.ToString(row[Configurations.requestedBy]).Trim();
                                        uploadShipmentObj.CustomerId = customerId;
                                        uploadShipmentObj.CreatedBy = objMemberProfile.UserId;
                                        uploadShipmentObj.UploadedFileName = file.FileName;
                                        uploadShipmentList.Add(uploadShipmentObj);
                                    }
                                }

                                data = uploadShipmentBAL.ValidedUploadShipment(uploadShipmentList);
                            }
                            catch (Exception)
                            {
                                TempData[Configurations.errorMessage] = LarastruckingResource.ExcelFileFormate;

                            }
                        }
                    }


                    return View(data);

                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();

        }
        #endregion

        #region bind pricing method and freight type dropdown
        /// <summary>
        /// bind pricing method and freight type and pcs type dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BindFreightTypeNPricingMethod()
        {
            var data = uploadShipmentBAL.BindFreightTypeNPricingMethod();

            IEnumerable<SelectListItem> weightUnit = from Unit qt in Enum.GetValues(typeof(Unit))
                                                     select new SelectListItem
                                                     {
                                                         Text = qt.ToString(),
                                                         Value = Convert.ToInt32(qt).ToString()

                                                     };
            ViewBag.WeightUnit = new SelectList(weightUnit, "Value", "Text");
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region save upload document
        /// <summary>
        /// save upload document
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult SaveExcelData(List<UploadShipmentDTO> model)
        {
            try
            {
                var result = uploadShipmentBAL.SaveExcelData(model);
                if (result)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region check fileNmae
        public ActionResult CheckFileName(string fileName)
        {
            var result = uploadShipmentBAL.CheckFileName(fileName);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region validate contact info count
        /// <summary>
        ///  validate contact info count
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public ActionResult ValidateContactInfo(int customerId)
        {
            bool isContactInfoAwb = uploadShipmentBAL.ValidateContactInfo(customerId);

            return Json(isContactInfoAwb, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
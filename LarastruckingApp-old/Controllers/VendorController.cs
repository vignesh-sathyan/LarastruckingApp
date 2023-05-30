using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Infrastructure;
using LarastruckingApp.Resource;
using LarastruckingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LarastruckingApp.Controllers
{
    [Authorize]
    public class VendorController : Controller
    {

        #region Private Members
        /// <summary>
        /// defining private member
        /// </summary>
        private readonly ICustomerBAL iCustomerBAL;
        private readonly IVendorBAL iVendorBAL;
        #endregion

        #region Constuctor
        /// <summary>
        /// Constructor Injection
        /// </summary>
        /// <param name="iICustomerRegistrationBALBAL"></param>
        public VendorController(ICustomerBAL iICustomerRegistrationBAL, IVendorBAL iVendorBll)
        {

            iCustomerBAL = iICustomerRegistrationBAL;
            iVendorBAL = iVendorBll;

        }
        #endregion

        #region Default action method
        // GET: Vendor
        [CustomAuthorize]
        public ActionResult Index()
        {
            BindStateCountryDropdown();
            return View();
        }
        #endregion

        #region Index Post Add and Update Vendor
        /// <summary>
        /// Add and Update Vendor
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(VendorNconsigneeDTO model)
        {
            BindStateCountryDropdown();
            JsonResponse objJsonResponse = new JsonResponse();
            MemberProfile objmemberProfile = new MemberProfile();
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.VendorNconsigneeId == 0)
                    {
                        model.CreatedBy = objmemberProfile.UserId;
                        model.CreatedOn = Configurations.TodayDateTime;

                        var result = iVendorBAL.Add(model);

                        if (result.IsSuccess)
                        {

                            objJsonResponse.IsSuccess = true;
                            objJsonResponse.Message = LarastruckingResource.DataSaveSuccessfully;
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
                    else if (model.VendorNconsigneeId > 0)
                    {
                        var result = iVendorBAL.Update(model);
                        result.ModifyBy = objmemberProfile.UserId;
                        result.ModifyOn = Configurations.TodayDateTime;

                        if (result.IsSuccess)
                        {
                            objJsonResponse.IsSuccess = true;
                            objJsonResponse.Message = LarastruckingResource.DataUpdateSuccessfully;
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
            catch (Exception)
            {
                throw;
            }

            return View();
        }

        #endregion

        #region PRIVATE: Bind State and Country
        /// <summary>
        /// Private method to bind drop-downs
        /// </summary>
        private void BindStateCountryDropdown()
        {
            ViewBag.Country = iCustomerBAL.GetCountryList();
            ViewBag.State = new SelectList(iCustomerBAL.GetStateList(), "ID", "Name");
        }
        #endregion

        #region GetVendorList
        /// <summary>
        /// Method to get Get Vendor List
        /// </summary>
        /// <returns></returns>
        public ActionResult GetVendorList()
        {
            try
            {
                return PartialView("_VendorList");
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Edit Vendor
        /// <summary>
        /// Edit Vendor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditVendor(int id)
        {
            try
            {               
                var vendorData = iVendorBAL.FindById(id);
                return new JsonResult()
                {
                    Data = vendorData,
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

        #region Delete Vendor
        /// <summary>
        /// Method to delete vendor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteVendor(int id = 0)
        {

            JsonResponse objJsonResponse = new JsonResponse();
            try
            {
                VendorNconsigneeDTO objVendorNconsigneeDTO = new VendorNconsigneeDTO();


                objVendorNconsigneeDTO.VendorNconsigneeId = id;
                if (iVendorBAL.Delete(objVendorNconsigneeDTO))
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
        /// Load Vendor data
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

            IEnumerable<VendorNconsigneeDTO> lstAddressDTO = iVendorBAL.List;



            if (!string.IsNullOrEmpty(search))
            {
                lstAddressDTO = lstAddressDTO.Where(x => x.VendorNconsigneeName.ToUpper().Contains(search.ToUpper())
                                                              || x.Email.ToUpper().Contains(search.ToUpper())
                                                              || x.Phone.ToUpper().Contains(search.ToUpper())
                                                              || x.StateName.ToUpper().Contains(search.ToUpper())
                                                              || x.Zip.ToUpper().Contains(search.ToUpper())
                                                              || x.City.ToUpper().Contains(search.ToUpper())).ToList();
            }
            recordsTotal = lstAddressDTO.Count();
            var data = lstAddressDTO.Skip(skip).Take(pageSize).ToList();
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                data = sortColumnDir == "asc" ? data.OrderBy(x => x.GetType().GetProperty(sortColumn).GetValue(x, null)).ToList() : data.OrderByDescending(x => x.GetType().GetProperty(sortColumn).GetValue(x, null)).ToList();

            }
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
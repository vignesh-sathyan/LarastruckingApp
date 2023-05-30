using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.Controllers;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Entities.TrailerRental;
using LarastruckingApp.Infrastructure;
using LarastruckingApp.Resource;
using LarastruckingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LarastruckingApp.Areas.TrailerRental.Controllers
{
    [Authorize]
    public class TrailerRentalController : BaseController
    {


        #region Private Member
        /// <summary>
        /// Defining private members
        /// </summary>
        private readonly IShipmentBAL shipmentBAL = null;
        private readonly ITrailerRentalBAL trailerRentalBAL = null;
        private readonly IAddressBAL addressBAL;
        private readonly IDriverBAL driverBAL;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iShipmentBAL"></param>
        public TrailerRentalController(IShipmentBAL iShipmentBAL, ITrailerRentalBAL itrailerRentalBAL, IAddressBAL iAddressBAL, IDriverBAL iDriverBAL)
        {
            shipmentBAL = iShipmentBAL;
            trailerRentalBAL = itrailerRentalBAL;
            addressBAL = iAddressBAL;
            driverBAL = iDriverBAL;
        }
        #endregion

        // GET: TrailerRental/TrailerRental
        [CustomAuthorize]
        [HttpGet]
        public ActionResult Index()
        {
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

        [HttpPost]
        public ActionResult GetEquipmentList(ValidateDriverNEquipmentDTO model)
        {
            var equipmentList = shipmentBAL.EquipmnetList(model);
            return Json(equipmentList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveTrailerRental(TrailerRentalDTO model)
        {
            MemberProfile mp = new MemberProfile();
            JsonResponse objJsonResponse = new JsonResponse();
            if (model != null)
            {
                model.CreatedBy = mp.UserId;
                model.CreatedDate = Configurations.TodayDateTime;
                var result = trailerRentalBAL.SaveTrailerRental(model);
                objJsonResponse.IsSuccess = result;
                objJsonResponse.Message = result ? LarastruckingResource.DataSaveSuccessfully : LarastruckingResource.SomethingWentWrong;
            }
            else
            {
                objJsonResponse.IsSuccess = false;
                objJsonResponse.Message = LarastruckingResource.SomethingWentWrong;
            }
            return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [CustomAuthorize]
        public ActionResult ViewTrailerRental()
        {
            return View();
        }

        public JsonResult GetTrailerRentalList()
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

                var preTripInfo = trailerRentalBAL.GetTrailerRentalList(dto);


                return Json(new { draw = draw, recordsFiltered = dto.TotalCount, recordsTotal = dto.TotalCount, data = preTripInfo }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }


        }

        #region Get Trailer Rental Detail by id
        /// <summary>
        /// get trailer rental detial by id
        /// </summary>
        /// <param name="trailerRentalId"></param>
        /// <returns></returns>
        public ActionResult GetTrailerRentalDetailById(int trailerRentalId)
        {
            var result = trailerRentalBAL.GetTrailerRentalDetailById(trailerRentalId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public ActionResult EditTrailerRental(TrailerRentalDTO model)
        {
            try
            {

          
            MemberProfile mp = new MemberProfile();
            JsonResponse objJsonResponse = new JsonResponse();
            if (model != null)
            {
                model.CreatedBy = mp.UserId;
                model.CreatedDate = Configurations.TodayDateTime;
                var result = trailerRentalBAL.EditTrailerRental(model);
                objJsonResponse.IsSuccess = result;
                objJsonResponse.Message = result ? LarastruckingResource.DataUpdateSuccessfully : LarastruckingResource.SomethingWentWrong;
            }
            else
            {
                objJsonResponse.IsSuccess = false;
                objJsonResponse.Message = LarastruckingResource.SomethingWentWrong;
            }
            return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }


        #region delete trailer rental
        /// <summary>
        ///delete trailer retnal By Id
        /// </summary>
        /// <param name="trailerRentalId"></param>
        /// <returns></returns>
        public ActionResult DeleteTrailerRental(int trailerRentalId)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();
                TrailerRentalDTO objTrailerRental = new TrailerRentalDTO();
                objTrailerRental.TrailerRentalId = trailerRentalId;
                objTrailerRental.CreatedBy = objMemberProfile.UserId;
                objTrailerRental.CreatedDate = Configurations.TodayDateTime;

                objJsonResponse.IsSuccess = trailerRentalBAL.DeleteTrailerRental(objTrailerRental);
                objJsonResponse.Message = objJsonResponse.IsSuccess ? LarastruckingResource.DataDeleteSuccessfully : LarastruckingResource.ErrorOccured;
                return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


     
    }

}
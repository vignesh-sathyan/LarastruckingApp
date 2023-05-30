using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Infrastructure;
using LarastruckingApp.Resource;
using LarastruckingApp.ViewModel;
using System;
using System.Web.Mvc;

namespace LarastruckingApp.Controllers
{
    public class GlobalController : BaseController
    {
        #region Private Members
        /// <summary>
        /// Defining private members
        /// </summary>
        private IAddressBAL iAddressRepo;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor injection
        /// </summary>
        /// <param name="iAddressBAL"></param>
        public GlobalController(IAddressBAL iAddressBAL)
        {
            iAddressRepo = iAddressBAL;
        }
        #endregion

        #region Add Address
        /// <summary>
        /// Add address method
        /// </summary>
        /// <param name="objAddressViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddAddress(AddressViewModel objAddressViewModel)
        {
            JsonResponse objJsonResponse = new JsonResponse();

            try
            {
                if (ModelState.IsValid)//For Update
                {
                    MemberProfile mp = new MemberProfile();

                    AddressDTO objAddressDTO = AutoMapperServices<AddressViewModel, AddressDTO>.ReturnObject(objAddressViewModel);

                    objAddressDTO.IsActive = true;
                    objAddressDTO.CreatedBy = mp.UserId;
                    objAddressDTO.CreatedOn = Configurations.TodayDateTime;
                    objAddressDTO.ModifiedBy = mp.UserId;
                    objAddressDTO.ModifiedOn = Configurations.TodayDateTime;

                    var response = iAddressRepo.Add(objAddressDTO);

                    if (response.IsSuccess)
                    {
                        objJsonResponse.IsSuccess = true;
                        objJsonResponse.Message = LarastruckingResource.DataSaveSuccessfully;
                        return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                    }
                }
                objJsonResponse.IsSuccess = false;
                objJsonResponse.Message = "Input data is not valid";
                return Json(objJsonResponse, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                objJsonResponse.IsSuccess = false;
                objJsonResponse.Message = "Exception occured";
                return Json(objJsonResponse, JsonRequestBehavior.AllowGet);

            }
        }
        #endregion
    }
}
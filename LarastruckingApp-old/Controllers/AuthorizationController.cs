using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.DTO;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using LarastruckingApp.Resource;
using LarastruckingApp.Infrastructure;

namespace LarastruckingApp.Controllers
{
    [Authorize]
    public class AuthorizationController : BaseController
    {
        #region Private Members
        /// <summary>
        /// Defining private members
        /// </summary>
        private readonly IPageAutharizationBAL iPageAutharizationBAL;
        private readonly IRoleBAL iRoleBAL;
        #endregion

        #region Constructor
        /// <summary>
        /// Page Autharization Constructor
        /// </summary>
        /// <param name="_iPageAutharizationBAL"></param>
        /// <param name="_iRoleBAL"></param>
        public AuthorizationController(IPageAutharizationBAL _iPageAutharizationBAL, IRoleBAL _iRoleBAL)
        {
            iPageAutharizationBAL = _iPageAutharizationBAL;
            iRoleBAL = _iRoleBAL;

        }
        #endregion

        #region Index
        /// <summary>
        /// Get Page Authorization Data
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize]
        public ActionResult Index()
        {
            try
            {
                int RoleId = 1;
                PageAutharizationViewModel objPageAuthorization = new PageAutharizationViewModel();
                objPageAuthorization.listPageAuthorization = AutoMapperServices<PageAuthorizationDTO, PageAutharizationViewModel>.ReturnObjectList(iPageAutharizationBAL.GetPageAuthorization(RoleId));
                objPageAuthorization.listRole = BindRole();
                objPageAuthorization.RoleId = RoleId;
                return View("Index", objPageAuthorization);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Index: HTTP POST
        /// <summary>
        /// Method to set permission
        /// </summary>
        /// <param name="objPermissionCollection"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Index(PageAutharizationViewModel objPermissionCollection)
        {
            try
            {
                int result = 0;
                int roleId = objPermissionCollection.RoleId;
                PageAutharizationViewModel objPageAuthorization = new PageAutharizationViewModel();
                foreach (var item in objPermissionCollection.listPageAuthorization)
                {
                    PageAuthorizationDTO objPageAuthorizationDTO = new PageAuthorizationDTO();
                    objPageAuthorizationDTO.RoleId = Convert.ToInt32(item.RoleId);
                    objPageAuthorizationDTO.PageId = Convert.ToInt32(item.PageId);
                    objPageAuthorizationDTO.CanView = item.CanView;
                    objPageAuthorizationDTO.CanInsert = item.CanInsert;
                    objPageAuthorizationDTO.CanUpdate = item.CanUpdate;
                    objPageAuthorizationDTO.CanDelete = item.CanDelete;
                    objPageAuthorizationDTO.IsPricingMethod = item.IsPricingMethod;
                    result = iPageAutharizationBAL.InsertUpdatePageAuthorization(objPageAuthorizationDTO);
                    roleId = item.RoleId;
                }
                if (result > 0)
                {
                    TempData["SuccessMessage"] = LarastruckingResource.msgPermissionUpdateSuccessfully;
                }
                var resutl = iPageAutharizationBAL.GetPageAuthorization(roleId);
                objPageAuthorization.listPageAuthorization = AutoMapperServices<PageAuthorizationDTO, PageAutharizationViewModel>.ReturnObjectList(resutl);
                objPageAuthorization.listRole = BindRole();
                objPageAuthorization.RoleId = roleId;
                return View("Index", objPageAuthorization);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Bind Role
        /// <summary>
        /// Bind Role Data
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public List<RoleViewModel> BindRole()
        {
            List<RoleViewModel> lstRoleViewModel = null;
            try
            {
                lstRoleViewModel = AutoMapperServices<RoleDTO, RoleViewModel>.ReturnObjectList(iRoleBAL.List.ToList());
            }
            catch (Exception)
            {
                throw;
            }
            return lstRoleViewModel;
        }
        #endregion

        #region Search Data
        /// <summary>
        /// Search Page Authorization Data
        /// </summary>
        /// <param name="objFormCollection"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        public ActionResult SearchData(FormCollection objFormCollection)
        {
            try
            {
                int roleId = 0;
                if (!string.IsNullOrEmpty(objFormCollection["RoleId"]))
                {
                    roleId = Convert.ToInt32(objFormCollection["RoleId"].ToString());
                }
                PageAutharizationViewModel objPageAuthorization = new PageAutharizationViewModel();
                objPageAuthorization.listPageAuthorization = AutoMapperServices<PageAuthorizationDTO, PageAutharizationViewModel>.ReturnObjectList(iPageAutharizationBAL.GetPageAuthorization(roleId));

                objPageAuthorization.listRole = BindRole();
                objPageAuthorization.RoleId = roleId;
                return View("Index", objPageAuthorization);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
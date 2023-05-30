using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.DTO;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Infrastructure;
using LarastruckingApp.Log.Utility;
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
    public class UserController : BaseController
    {
        #region Private Member
        /// <summary>
        /// Defining private members
        /// </summary>
        private readonly IUserBAL iUserRepo;
        private readonly IRoleBAL iRoleRepo;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor Injection
        /// </summary>
        /// <param name="iUserBAL"></param>
        /// <param name="iRoleBAL"></param>
        public UserController(IUserBAL iUserBAL, IRoleBAL iRoleBAL)
        {
            iUserRepo = iUserBAL;
            iRoleRepo = iRoleBAL;

        }
        #endregion



        #region UserRegistration
        /// <summary>
        /// Action method for user registration 
        /// </summary>
        /// <returns></returns>
     
        [CustomAuthorize]

        public ActionResult UserRegistration()
        {
            UserViewModel userViewModel = new UserViewModel();
            return View(userViewModel);
        }
        #endregion

        #region UserRegistration: HttpPost
        /// <summary>
        /// Post action method for user registration.
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserRegistration(UserViewModel objUserViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MemberProfile profile = new MemberProfile();

                    if (objUserViewModel.Userid > 0)
                    {
                        UserDTO objUserDTOUpdate = AutoMapperServices<UserViewModel, UserDTO>.ReturnObject(objUserViewModel);
                        objUserDTOUpdate.ModifiedBy = profile.UserId;
                        objUserDTOUpdate.ModifiedOn = DateTime.UtcNow;
                        var response = iUserRepo.UpdateUser(objUserDTOUpdate);

                        if (response > 0)
                        {
                            TempData["SuccessMessage"] = LarastruckingResource.DataUpdateSuccessfully;
                            return RedirectToAction("UserRegistration");
                        }
                        else if (response == -1)
                        {
                            TempData["ErrorMessage"] = $"{objUserViewModel.UserName} {LarastruckingResource.AlreadyExist}";
                            return View(objUserViewModel);
                        }
                        else
                        {
                            TempData["ErrorMessage"] = LarastruckingResource.SomethingWentWrong;
                            return View(objUserViewModel);
                        }

                    }

                    else
                    {
                        int result = 0;
                        UserDTO objUserDTOinsert = AutoMapperServices<UserViewModel, UserDTO>.ReturnObject(objUserViewModel);
                        objUserDTOinsert.GUID = Guid.NewGuid().ToString();
                        objUserDTOinsert.GuidGenratedDateTime = DateTime.Now;
                        objUserDTOinsert.CreatedOn = DateTime.Now;

                        result = iUserRepo.AddUserRoleRegisteration(objUserDTOinsert);

                        if (result == 0)
                        {
                            TempData["ErrorMessage"] = $"{objUserViewModel.UserName} {LarastruckingResource.AlreadyExist}";
                            return View(objUserViewModel);
                        }
                        else if (result == 1)
                        {
                            if (!string.IsNullOrEmpty(objUserDTOinsert.UserName))
                            {
                                string baseURL = LarastruckingApp.Entities.Common.Configurations.BaseURL;
                                string resetLink = baseURL.Trim() + LarastruckingResource.ForgotPasswordResetPassword + objUserDTOinsert.GUID;
                                string subject = LarastruckingResource.CreateNewPasswordLink;
                                string message = " Dear " + objUserDTOinsert.FirstName + " " + objUserDTOinsert.LastName + ", <br /><br /> please click on the below link to create your password. <br /><br /> <a href='" + resetLink.Trim() + "'>" + resetLink.Trim() + "</a> <br /><br /><br /><br />  Best Regards,<br/> Lara’s Trucking & Logistics";
                                Email.AsyncSendEmail(objUserDTOinsert.UserName, subject, message);

                                TempData["SuccessMessage"] = LarastruckingResource.UserSuccess;
                            }
                            else
                            {
                                TempData["ErrorMessage"] = LarastruckingResource.PleaseEnterRegisteredUserName;
                            }
                            return RedirectToAction("UserRegistration");

                        }
                        else
                        {
                            TempData["ErrorMessage"] = LarastruckingResource.SomethingWentWrong;
                            return View(objUserViewModel);
                        }

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View(objUserViewModel);
        }
        #endregion

        #region UserEdit
        /// <summary>
        /// Post method for user edit.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserEdit(int id)
        {
            UserViewModel userViewModel = new UserViewModel();
            var data = iUserRepo.FindById(id);
            userViewModel = AutoMapperServices<UserDTO, UserViewModel>.ReturnObject(data);
            return new JsonResult()
            {
                Data = userViewModel,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }
        #endregion

        #region Delete
        /// <summary>
        /// Action method to delete user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {

            UserDTO objUserDTO = new UserDTO()
            {
                Userid = id
            };
            JsonResponse objJsonResponse = new JsonResponse();
            if (iUserRepo.Delete(objUserDTO))
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
        #endregion

        #region Get User List
        /// <summary>
        /// Action method for user details.
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserList()
        {
            return PartialView("_UserList");
        }
        #endregion

        #region GET: Login
        /// <summary>
        /// Action method for login.
        /// </summary>
        /// <returns></returns>

        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<UserDTO> user = new List<UserDTO>();
                return Redirect(user); //RedirectToAction("ViewEquipment", "Equipment");
            }
            else
            {
                LoginViewModel obLoginViewModel = new LoginViewModel();
                return View(obLoginViewModel);
            }
        }
        #endregion

        #region POST: Login
        /// <summary>
        /// Post action method for Login. 
        /// </summary>
        /// <param name="userLoginModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel userLoginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    userLoginModel.Password = PasswordUtility.TripleDesEncrypt(userLoginModel.Password);
                    var objUserDTO = AutoMapperServices<LoginViewModel, UserDTO>.ReturnObject(userLoginModel);
                    IEnumerable<UserDTO> objUserDTOa = iUserRepo.Login(objUserDTO);
                    if (objUserDTOa.Count() > 0)
                    {
                        var roles = objUserDTOa.Select(p => p.RoleName).ToList();

                        ClaimsTypeProperty ctp = new ClaimsTypeProperty();
                        ctp.Identity_UserId = objUserDTOa.FirstOrDefault().Userid;
                        ctp.Identity_UserName = objUserDTOa.FirstOrDefault().UserName;
                        ctp.Identity_Role = roles;
                        ctp.Identity_FullName = $"{objUserDTOa.FirstOrDefault().FirstName}  {objUserDTOa.FirstOrDefault().LastName}";

                        ctp.Identity_Permissions = objUserDTOa.Select(p => new ActionButtonDto()
                        {
                            DisplayOrder = p.DisplayOrder,
                            IsMenu = p.IsMenu,
                            DisplayIcon = p.DisplayIcon,
                            PageName = p.PageName,
                            ControllerName = p.ControllerName ?? string.Empty,
                            AreaName = p.AreaName ?? string.Empty,
                            ActionName = p.ActionName ?? string.Empty,
                            CanDelete = p.CanDelete,
                            CanInsert = p.CanInsert,
                            CanUpdate = p.CanUpdate,
                            CanView = p.CanView,
                            IsPricingMethod = p.IsPricingMethod

                        }).ToList();

                        IdentityHelper objIdentyHelper = new IdentityHelper();
                        objIdentyHelper.IdentitySignin(ctp);

                        return Redirect(objUserDTOa.ToList());

                    }
                    else
                    {
                        userLoginModel.result = LarastruckingResource.LoginIncorrectDetails;
                        return View(userLoginModel);
                    }
                }
                else
                {

                    userLoginModel.result = LarastruckingResource.LoginIncorrectDetails;
                    return View(userLoginModel);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion

        #region Redirection Based on Role
        /// <summary>
        /// This is 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        private ActionResult Redirect(List<UserDTO> user)
        {

            var userdetail = user.Where(x => x.IsMenu && x.CanView).OrderBy(x => x.DisplayOrder).ToList();
            string actionName = userdetail.Select(x => x.ActionName).FirstOrDefault();
            string controllerName = userdetail.Select(x => x.ControllerName).FirstOrDefault();
            string areaName = userdetail.Select(x => x.AreaName).FirstOrDefault() ?? string.Empty;
            if (userdetail.ToList().Count() > 0)
            {
                return RedirectToAction(actionName, controllerName, new { area = areaName });
            }

            else
            {
                return RedirectToAction("LogOut");
            }
        }
        #endregion

        #region LogOut
        public ActionResult LogOut()
        {
            IdentityHelper objIdentyHelper = new IdentityHelper();
            objIdentyHelper.IdentitySignout();
            Session.Clear();
            Session.Abandon();
            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-30);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.MinValue);

            return RedirectToAction("Login", "User");

        }
        #endregion

        #region DropDown Role Bind
        /// <summary>
        /// Bind role in drop-down
        /// </summary>
        /// <returns></returns>
        public ActionResult DropDownRoleBind()
        {
            try
            {
                IEnumerable<RoleDTO> RoleDTO = null;
                RoleDTO = iRoleRepo.GetRole();
                RoleDTO = RoleDTO.Where(r => r.RoleName != LarastruckingResource.UserRole_Driver && r.RoleName != LarastruckingResource.UserRole_Customer);

                return new JsonResult()
                {
                    Data = RoleDTO,
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

        #region LoadData
        /// <summary>
        /// Action method for bind table user
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
                IEnumerable<UserDTO> lstUserDTO = iUserRepo.List;
                List<UserViewModel> lstDriverViewModel = AutoMapperServices<UserDTO, UserViewModel>.ReturnObjectList(lstUserDTO.ToList());
                var UserList = lstDriverViewModel;
                if (!string.IsNullOrEmpty(search))
                {
                    UserList = UserList.Where(x => x.FirstName.ToUpper().Contains(search.ToUpper())
                    || x.RoleName.ToUpper().Contains(search.ToUpper())
                    || x.UserType.ToUpper().Contains(search.ToUpper())
                    || x.LastName.ToUpper().Contains(search.ToUpper())
                    || x.UserName.ToUpper().Contains(search.ToUpper())).ToList();
                }
                recordsTotal = UserList.Count();
                var data = UserList.ToList();
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

        #region default page if 
        /// <summary>
        /// default page if page not found
        /// </summary>
        /// <returns></returns>
        public ActionResult PageNotFound()
        {
            return View();
        }
        #endregion
    }
}

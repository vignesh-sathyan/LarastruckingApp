using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.Resource;
using LarastruckingApp.DTO;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.ViewModel;
using System;
using System.Web.Mvc;
using LarastruckingApp.Log.Utility;

namespace LarastruckingApp.Controllers
{
    public class ForgotPasswordController : BaseController
    {
        #region Private Member
        /// <summary>
        /// defining private member
        /// </summary>
        private readonly IForgotPasswordBAL iForgotPasswordRepo;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor Injection
        /// </summary>
        /// <param name="iForgotPasswordBAL"></param>
        public ForgotPasswordController(IForgotPasswordBAL iForgotPasswordBAL)
        {
            iForgotPasswordRepo = iForgotPasswordBAL;
        }
        #endregion

        #region ForgotPassword
        /// <summary>
        /// Defult action method
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        #endregion

        #region ForgotPassword: HttpPost
        /// <summary>
        /// Action method for Save GUID and send mail for user
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ForgotPassword(string UserName)
        {
            if (!string.IsNullOrEmpty(UserName))
            {

                UserDTO userData = iForgotPasswordRepo.ResetUserIsExist(UserName);
                string strFullNmae = userData.FirstName + " " + (userData.LastName == null ? string.Empty : userData.LastName);
                if (userData != null && userData.Userid > 0)
                {
                    string strguid = Guid.NewGuid().ToString();
                    string baseURL = Configurations.BaseURL;
                    string resetLink = baseURL.Trim() + "ForgotPassword/ResetPassword/" + strguid;
                    string subject = "Forgot password link";
                    string message = " Dear " + strFullNmae + ", <br /> Please click on the link below to reset your password. <br /><br /> <a href='"+ resetLink.Trim() + "'>" + resetLink.Trim() + "</a> <br /><br /><br /><br /> Best Regards,<br/> Lara’s Trucking & Logistics";
                    Email.AsyncSendEmail(userData.UserName, subject, message);

                    userData.GUID = strguid;
                    userData.GuidGenratedDateTime = DateTime.Now;
                    var userDetail = iForgotPasswordRepo.UpdateResetPassword(userData);
                    if (userDetail.IsSuccess == true)
                    {
                        TempData["SuccessMessage"] = LarastruckingResource.ResetPasswordLinkSendOnYourRegisteredEmailaddress;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = LarastruckingResource.EnterRegisteredUserName;
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = LarastruckingResource.EnterRegisteredUserName;
                }
            }
            else
            {
                TempData["ErrorMessage"] = LarastruckingResource.EnterRegisteredUserName;
            }
            return View();
        }
        #endregion

        #region ResetPassword
        /// <summary>
        /// Action method for genrating password
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ResetPassword(Guid? id = null)
        {

            ResetPasswordViewModel objResetPasswordViewModel = new ResetPasswordViewModel();
            if (iForgotPasswordRepo.varifyUserGuidTime(id))
            {
                if (id != null)
                {
                    objResetPasswordViewModel.GUID = id;
                    objResetPasswordViewModel.ShowHide = true;
                }
            }
            return View(objResetPasswordViewModel);
        }
        #endregion

        #region ResetPassword: HttpPost
        /// <summary>
        /// action method for adding  new password
        /// </summary>
        /// <param name="resetPasswordViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                UserDTO userData = iForgotPasswordRepo.varifyUserGuid(resetPasswordViewModel.GUID);
                if (userData.Userid > 0)
                {
                    userData.GUID = string.Empty;
                    userData.GuidGenratedDateTime = null;
                    userData.ResetPasswordDateTime = DateTime.UtcNow;
                    userData.Password = PasswordUtility.TripleDesEncrypt(resetPasswordViewModel.Password);
                    if (iForgotPasswordRepo.UpdateResetPassword(userData).IsSuccess == true)
                    {

                        TempData["SuccessMessage"] = LarastruckingResource.PasswordSuccessfullyChanged;
                    }
                    return RedirectToAction("Login", "User");
                }
            }
            else
            {
                resetPasswordViewModel.ShowHide = true;
                return View(resetPasswordViewModel);
            }
            return View();
        }
        #endregion
    }
}
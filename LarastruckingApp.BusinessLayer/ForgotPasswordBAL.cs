using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.DAL;
using LarastruckingApp.DAL.Interface;
using LarastruckingApp.DTO;
using System;

namespace LarastruckingApp.BusinessLayer
{
    public class ForgotPasswordBAL : IForgotPasswordBAL
    {
        #region Private Member
        /// <summary>
        /// Private Member
        /// </summary>
        private readonly IForgotPasswordDAL iForgotPasswordRepo;
        #endregion

        #region ForgotPasswordBAL
        /// <summary>
        /// Forgot Password BAL
        /// </summary>
        /// <param name="iForgotPasswordDAL"></param>
        public ForgotPasswordBAL(IForgotPasswordDAL iForgotPasswordDAL)
        {
            iForgotPasswordRepo = iForgotPasswordDAL;
        }
        #endregion

        #region ResetUserIsExist
        /// <summary>
        /// Method  for checking user exist or not in db
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public UserDTO ResetUserIsExist(string UserName)
        {
            return iForgotPasswordRepo.ResetUserIsExist(UserName);
        }
        #endregion

        #region UpdateResetPassword
        /// <summary>
        /// Update Reset Password
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public UserDTO UpdateResetPassword(UserDTO entity)
        {
            return iForgotPasswordRepo.UpdateResetPassword(entity);
        }
        #endregion

        #region varifyUserGuid
        /// <summary>
        ///  /// Method  for varify user GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public UserDTO varifyUserGuid(Guid? guid)
        {
            return iForgotPasswordRepo.varifyUserGuid(guid);
        }
        #endregion

        #region varifyUserGuidTime
        /// <summary>
        /// Method for varifying user GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool varifyUserGuidTime(Guid? guid)
        {
            var data = iForgotPasswordRepo.varifyUserGuid(guid);
            DateTime FromDate = DateTime.UtcNow;
            DateTime ToDate = Convert.ToDateTime(data.GuidGenratedDateTime);
            TimeSpan span = FromDate.Subtract(ToDate);
            int min = Convert.ToInt32(span.TotalMinutes);
            if(min<=15)
            {
                return true;
            }
            else
            {
                return false;
            }            
        }
        #endregion
    }
}

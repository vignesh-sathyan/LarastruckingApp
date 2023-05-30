using LarastruckingApp.DTO;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Linq;

namespace LarastruckingApp.Repository.Repository
{

    public class ForgotPasswordRepository : IForgotPasswordRepository
    {
        #region Private Member
        /// <summary>
        /// defining Private members
        /// </summary>
        private readonly LarastruckingDBEntities userContext;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ForgotPasswordRepository()
        {
            userContext = new LarastruckingDBEntities();
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
            try
            {


                UserDTO objUserDTO = new UserDTO();
                var result = (from user in userContext.tblUsers where user.UserName.Trim() == UserName.Trim() && user.IsDeleted == false select user).FirstOrDefault();
                if (result != null)
                {
                    objUserDTO = AutoMapperServices<tblUser, UserDTO>.ReturnObject(result);
                }
                return objUserDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region varifyUserGuid
        /// <summary>
        /// Method  for varify user GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public UserDTO varifyUserGuid(Guid? guid)
        {
            try
            {

                string strguid = guid.ToString();
                UserDTO objUserDTO = new UserDTO();
                var result = (from user in userContext.tblUsers where user.GUID == strguid && user.IsDeleted == false select user).FirstOrDefault();
                if (result != null)
                {
                    objUserDTO = AutoMapperServices<tblUser, UserDTO>.ReturnObject(result);
                }
                return objUserDTO;

            }
            catch (Exception)
            {

                throw;
            }
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
            try
            {


                UserDTO objUserDTO = new UserDTO();

                var objtblUser = userContext.tblUsers.Find(entity.Userid);
                if (objtblUser != null)
                {

                    objtblUser.GUID = entity.GUID;
                    objtblUser.GuidGenratedDateTime = entity.GuidGenratedDateTime;
                    objtblUser.ResetPasswordDateTime = entity.ResetPasswordDateTime;

                    if (entity.Password != null)
                    {
                        objtblUser.Password = entity.Password;
                    }
                    userContext.Entry(objtblUser).State = System.Data.Entity.EntityState.Modified;
                    objUserDTO = AutoMapperServices<tblUser, UserDTO>.ReturnObject(objtblUser);
                    objUserDTO.IsSuccess = userContext.SaveChanges() > 0 ? true : false;

                }
                return objUserDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}

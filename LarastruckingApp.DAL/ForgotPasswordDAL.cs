using LarastruckingApp.DAL.Interface;
using LarastruckingApp.DTO;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL
{
    public class ForgotPasswordDAL : IForgotPasswordDAL
    {
        public  IForgotPasswordRepository iForgotPasswordRepo;

        public ForgotPasswordDAL(IForgotPasswordRepository iForgotPasswordRepository)
        {
            iForgotPasswordRepo = iForgotPasswordRepository;
        }

        /// <summary>
        /// Method  for checking user exist or not in db
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public UserDTO ResetUserIsExist(string UserName)
        {
            return iForgotPasswordRepo.ResetUserIsExist(UserName);
        }

        public UserDTO UpdateResetPassword(UserDTO entity)
        {
            return iForgotPasswordRepo.UpdateResetPassword(entity);
        }

        /// <summary>
        /// Method  for varify user GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>

        public UserDTO varifyUserGuid(Guid? guid)
        {
            return iForgotPasswordRepo.varifyUserGuid(guid);
        }
    }
}

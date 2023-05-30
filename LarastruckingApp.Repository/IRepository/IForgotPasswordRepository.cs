using LarastruckingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.IRepository
{
    public interface IForgotPasswordRepository
    {
        UserDTO ResetUserIsExist(string UserName);
        UserDTO varifyUserGuid(Guid? guid);
        UserDTO UpdateResetPassword(UserDTO entity);
    }
}

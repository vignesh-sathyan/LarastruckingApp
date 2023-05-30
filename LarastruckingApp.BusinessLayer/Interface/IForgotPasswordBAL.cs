using LarastruckingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer.Interface
{
    public interface IForgotPasswordBAL
    {
        UserDTO ResetUserIsExist(string UserName);
        UserDTO varifyUserGuid(Guid? guid);

        bool varifyUserGuidTime(Guid? guid);
        UserDTO UpdateResetPassword(UserDTO entity);
    }
}

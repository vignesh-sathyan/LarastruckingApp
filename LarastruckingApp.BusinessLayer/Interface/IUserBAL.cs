using LarastruckingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer.Interface
{
    public interface IUserBAL:ICommonBAL<UserDTO>
    {
        UserDTO FindByUsername(UserDTO objUserDTO);
        IEnumerable<UserDTO> Login(UserDTO objUserDTO);
        int AddUserRoleRegisteration(UserDTO objUserDTO);
        int UpdateUser(UserDTO entity);
    }
}

using LarastruckingApp.DTO;
using System.Collections.Generic;

namespace LarastruckingApp.DAL.Interface
{
    public interface IUserDAL: ICommonDAL<UserDTO>
    {
        IEnumerable<UserDTO> Login(UserDTO objUserDTO);

        UserDTO FindByUsername(UserDTO objUserDTO);
        int AddUserRoleRegisteration(UserDTO entity);
        int UpdateUser(UserDTO entity);

    }
}

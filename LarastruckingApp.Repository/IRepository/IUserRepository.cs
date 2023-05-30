using LarastruckingApp.DTO;
using LarastruckingApp.Repository.EF;
using System.Collections.Generic;

namespace LarastruckingApp.Repository.IRepository
{
    public interface IUserRepository: IRepository<UserDTO>
    {
        IEnumerable<UserDTO> Login(UserDTO objUserDTO);
        UserDTO FindByUsername(UserDTO objUserDTO);
        int AddUserRoleRegisteration(UserDTO objUserDTO);
        int UpdateUser(UserDTO entity);
    }
}

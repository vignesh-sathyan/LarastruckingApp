using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LarastruckingApp.DTO;

namespace LarastruckingApp.DAL.Interface
{
    public interface IRoleDAL: ICommonDAL<RoleDTO>
    {
        IEnumerable<RoleDTO> GetRole();
    }
}

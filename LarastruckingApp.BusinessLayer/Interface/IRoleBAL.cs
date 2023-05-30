using LarastruckingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer.Interface
{
    public interface IRoleBAL : ICommonBAL<RoleDTO>
    {
        IEnumerable<RoleDTO> GetRole();
    }
}

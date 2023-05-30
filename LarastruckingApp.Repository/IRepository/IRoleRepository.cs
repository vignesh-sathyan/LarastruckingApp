using LarastruckingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.IRepository
{
    public interface IRoleRepository : IRepository<RoleDTO>
    {
        IEnumerable<RoleDTO> GetRole();
    }
}

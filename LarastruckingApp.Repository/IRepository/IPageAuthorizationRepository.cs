using LarastruckingApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.IRepository
{
    public interface IPageAuthorizationRepository:IRepository<PageAuthorizationDTO>
    {
        int InsertUpdatePageAuthorization(PageAuthorizationDTO objPageAuthorizationDTO);
        List<PageAuthorizationDTO> GetPageAuthorization(int roleId);
    }
}

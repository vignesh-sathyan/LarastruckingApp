using LarastruckingApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL.Interface
{
    public interface IPageAuthorizationDAL : ICommonDAL<PageAuthorizationDTO>
    {      
        int InsertUpdatePageAuthorization(PageAuthorizationDTO entity);
        List<PageAuthorizationDTO> GetPageAuthorization(int roleId);

    }
}

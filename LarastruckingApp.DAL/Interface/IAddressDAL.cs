using LarastruckingApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL.Interface
{
   public interface IAddressDAL: ICommonDAL<AddressDTO>
    {
        IEnumerable<AddressTypeDTO> BindAddressType();
        IList<AddressDTO> GetAddress(string address);
    }
}

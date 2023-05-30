using LarastruckingApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer.Interface
{
  public interface IAddressBAL: ICommonBAL<AddressDTO>
    {
        IEnumerable<AddressTypeDTO> BindAddressType();
        IList<AddressDTO> GetAddress(string address);
    }
}

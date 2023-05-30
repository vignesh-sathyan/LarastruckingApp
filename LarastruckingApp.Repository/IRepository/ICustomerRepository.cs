using LarastruckingApp.DTO;
using LarastruckingApp.Entities.CustomerDto;
using LarastruckingApp.Entities.quotesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.IRepository
{
    public interface ICustomerRepository : IRepository<CustomerDto>
    {
        #region Get All Customers
        /// <summary>
        /// For accessing the all active customer list.
        /// </summary>
        IEnumerable<CustomerDto> GetAllCustomers(string type);

        #endregion

        List<CountryDTO> GetCountryList();
        List<StateDTO> GetStateList();
        List<CityDTO> GetCityList(int stateId);
        IList<CustomerQuotesDto> GetAllCustomer(string searchText);
        List<StateDTO> GetStates(int countryId);
    }
}

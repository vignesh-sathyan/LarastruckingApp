using LarastruckingApp.DTO;
using LarastruckingApp.Entities.CustomerDto;
using LarastruckingApp.Entities.quotesDto;
using System.Collections.Generic;

namespace LarastruckingApp.DAL.Interface
{
    public interface ICustomerDAL : ICommonDAL<CustomerDto>
    {
        #region Get All Customers
        IEnumerable<CustomerDto> GetAllCustomers(string type);

        #endregion

        List<CountryDTO> GetCountryList();
        List<StateDTO> GetStateList();
        List<CityDTO> GetCityList(int stateId);
        IList<CustomerQuotesDto> GetAllCustomer(string searchText);
        List<StateDTO> GetStates(int countryId);
    }
}

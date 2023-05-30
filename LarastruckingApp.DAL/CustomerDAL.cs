using LarastruckingApp.DAL.Interface;
using LarastruckingApp.DTO;
using LarastruckingApp.Entities.CustomerDto;
using LarastruckingApp.Entities.quotesDto;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL
{
    public class CustomerDAL : ICustomerDAL
    {
        private ICustomerRepository iCustomerRepo;

        public CustomerDAL(ICustomerRepository iCustomerRepository)
        {
            iCustomerRepo = iCustomerRepository;

        }

        public IEnumerable<CustomerDto> List
        {
            get
            {
                return iCustomerRepo.List;
            }
        }

        #region Get All Customers
        /// <summary>
        /// For accessing the all active customer list.
        /// </summary>
        public IEnumerable<CustomerDto> GetAllCustomers(string type)
        {
            return iCustomerRepo.GetAllCustomers(type);
        }

        #endregion


        public CustomerDto Add(CustomerDto entity)
        {
            return iCustomerRepo.Add(entity);

        }

        public bool Delete(CustomerDto entity)
        {
            return iCustomerRepo.Delete(entity);
        }

        public CustomerDto FindById(int Id)
        {
            return iCustomerRepo.FindById(Id);
        }

        public CustomerDto Update(CustomerDto entity)
        {
            return iCustomerRepo.Update(entity);
        }

        public List<CountryDTO> GetCountryList()
        {
            return iCustomerRepo.GetCountryList();
        }

        public List<StateDTO> GetStateList()
        {
            return iCustomerRepo.GetStateList();
        }

        public List<CityDTO> GetCityList(int stateId)
        {
            return iCustomerRepo.GetCityList(stateId);
        }

        #region Get Customer
        /// <summary>
        /// This method is used to get all customers for the purpose of binding it in the drop-down
        /// </summary>
        public IList<CustomerQuotesDto> GetAllCustomer(string searchText)
        {
            return iCustomerRepo.GetAllCustomer(searchText);
        }


        #endregion

        #region GetStates
        /// <summary>
        /// To find state list.
        /// </summary>
        /// <returns></returns>
        public List<StateDTO> GetStates(int countryId)
        {
            return iCustomerRepo.GetStates(countryId);
        }
        #endregion
    }
}

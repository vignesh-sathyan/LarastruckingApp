using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.DAL.Interface;
using LarastruckingApp.DTO;
using LarastruckingApp.Entities.CustomerDto;
using LarastruckingApp.Entities.quotesDto;
using System;
using System.Collections.Generic;

namespace LarastruckingApp.BusinessLayer
{
    public class CustomerBAL : ICustomerBAL
    {
        #region Private Member
        /// <summary>
        /// private member
        /// </summary>
        private readonly ICustomerDAL iCustomerRepo;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iCustomerDAL"></param>
        public CustomerBAL(ICustomerDAL iCustomerDAL)
        {
            iCustomerRepo = iCustomerDAL;
        }
        #endregion

        #region Constructor
        /// <summary>
        /// To get customer list.
        /// </summary>
        public IEnumerable<CustomerDto> List
        {
            get
            {
                return iCustomerRepo.List;
            }
        }
        #endregion

        #region Get All Customers
        /// <summary>
        /// For accessing the all active customer list.
        /// </summary>
        public IEnumerable<CustomerDto> GetAllCustomers(string type)
        {
            return iCustomerRepo.GetAllCustomers(type);
        }

        #endregion

        #region Add
        /// <summary>
        /// To add customer.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public CustomerDto Add(CustomerDto entity)
        {
           return iCustomerRepo.Add(entity);
        }
        #endregion

        #region Delete
        /// <summary>
        /// To delete customer.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(CustomerDto entity)
        {
          return iCustomerRepo.Delete(entity);
        }
        #endregion

        #region Update
        /// <summary>
        /// To update customer.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public CustomerDto Update(CustomerDto entity)
        {
           return iCustomerRepo.Update(entity);
        }
        #endregion

        #region FindById
        /// <summary>
        /// To find customer by id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerDto FindById(int Id)
        {
            try
            {
                return iCustomerRepo.FindById(Id);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region GetCountryList
        /// <summary>
        /// To get country list.
        /// </summary>
        /// <returns></returns>
        public List<CountryDTO> GetCountryList()
        {
            return iCustomerRepo.GetCountryList();
        }
        #endregion

        #region GetStateList
        /// <summary>
        /// To get state list.
        /// </summary>
        /// <returns></returns>
        public List<StateDTO> GetStateList()
        {
            return iCustomerRepo.GetStateList();
        }
        #endregion

        #region GetCityList
        /// <summary>
        /// To get City list.
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public List<CityDTO> GetCityList(int stateId)
        {
            return iCustomerRepo.GetCityList(stateId);
        }
        #endregion

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

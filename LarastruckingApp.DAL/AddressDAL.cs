using LarastruckingApp.DAL.Interface;
using LarastruckingApp.Entities;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL
{
    public class AddressDAL : IAddressDAL
    {
        #region Private member
        /// <summary>
        /// Address Data Access Layer
        /// </summary>
        private readonly IAddressRepository iAddressRepository;
        #endregion

        #region AddressDAL
        public AddressDAL(IAddressRepository iAddressRepository)
        {
            this.iAddressRepository = iAddressRepository;
        }/// <summary>
        #endregion

        #region List
        public IEnumerable<AddressDTO> List
        {
            get
            {
                return iAddressRepository.List;
            }
        }
        #endregion

        #region BindAddressType
        public IEnumerable<AddressTypeDTO> BindAddressType()
        {

            return iAddressRepository.BindAddressType();

        }
        #endregion

        #region Add
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AddressDTO Add(AddressDTO entity)
        {
            return iAddressRepository.Add(entity);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(AddressDTO entity)
        {
            return iAddressRepository.Delete(entity);
        }
        #endregion

        #region FindById
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AddressDTO FindById(int Id)
        {
            return iAddressRepository.FindById(Id);
        }
        #endregion

        #region Update
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AddressDTO Update(AddressDTO entity)
        {
            return iAddressRepository.Update(entity);
        }
        #endregion

        #region Find Address auto-complete
        /// <summary>
        /// List of Address
        /// </summary>
        public IList<AddressDTO> GetAddress(string address)
        {
            return iAddressRepository.GetAddress(address);
        }
        #endregion

    }
}

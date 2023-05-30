using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.DAL.Interface;
using LarastruckingApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer
{
   public class AddressBAL: IAddressBAL
    {
        #region Private member
        /// <summary>
        /// Private member
        /// </summary>
        private IAddressDAL iAddressDAL;
        #endregion

        #region AddressBAL
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iAddressDAL"></param>
        public AddressBAL(IAddressDAL iAddressDAL)
        {
            this.iAddressDAL = iAddressDAL;
        }
        #endregion

        #region List
        /// <summary>
        ///  Method to get list
        /// </summary>
        public IEnumerable<AddressDTO> List
        {
            get
            {
                return iAddressDAL.List;
            }
        }
        #endregion

        #region BindAddressType
        /// <summary>
        /// Method to bind address type
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AddressTypeDTO> BindAddressType()
        {
           
                return iAddressDAL.BindAddressType();

        }
        #endregion

        #region Add
        /// <summary>
        /// Method to add address
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AddressDTO Add(AddressDTO entity)
        {
            return iAddressDAL.Add(entity);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Method to delete
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(AddressDTO entity)
        {
            return iAddressDAL.Delete(entity);
        }
        #endregion

        #region FindById
        /// <summary>
        /// Method to find address by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AddressDTO FindById(int Id)
        {
            return iAddressDAL.FindById(Id);
        }
        #endregion

        #region Update
        /// <summary>
        /// Method to update
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AddressDTO Update(AddressDTO entity)
        {
            return iAddressDAL.Update(entity);
        }
        #endregion

        #region Find Address auto-complete
        /// <summary>
        /// List of Address
        /// </summary>
        public IList<AddressDTO> GetAddress(string address)
        {
            return iAddressDAL.GetAddress(address);
        }
        #endregion

    }
}

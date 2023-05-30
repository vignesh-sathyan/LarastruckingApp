using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.DAL.Interface;
using LarastruckingApp.DTO;
using System.Collections.Generic;

namespace LarastruckingApp.BusinessLayer
{
    /// <summary>
    /// User Role Busines Layer
    /// </summary>
    public class UserRoleBAL : IUserRoleBAL
    {
        #region Private Member
        /// <summary>
        /// Defining private member
        /// </summary>
        private IUserRoleDAL iUserRoleRepo;
        #endregion

        #region UserRoleBAL
        public UserRoleBAL(IUserRoleDAL iUserRoleDAL)
        {
            iUserRoleRepo = iUserRoleDAL;
        }
        #endregion

        #region List
        /// <summary>
        /// User Role List
        /// </summary>
        public IEnumerable<UserRoleDTO> List
        {
            get
            {
                return iUserRoleRepo.List;
            }
        }
        #endregion

        #region Add
        /// <summary>
        /// Add User Role
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public UserRoleDTO Add(UserRoleDTO entity)
        {
            return iUserRoleRepo.Add(entity);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete User Role
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(UserRoleDTO entity)
        {
            return iUserRoleRepo.Delete(entity);
        }
        #endregion

        #region FindById
        /// <summary>
        /// Find User Role Data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserRoleDTO FindById(int id)
        {
            return iUserRoleRepo.FindById(id);
        }
        #endregion

        #region Update
        /// <summary>
        /// Update User Role
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public UserRoleDTO Update(UserRoleDTO entity)
        {
            return iUserRoleRepo.Update(entity);
        }
        #endregion
    }
}

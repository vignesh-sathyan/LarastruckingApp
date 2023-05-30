using LarastruckingApp.DTO;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        #region Private Member
        /// <summary>
        /// defining Private members
        /// </summary>
        private readonly LarastruckingDBEntities userContext;
        #endregion

        #region UserRoleRepository
        /// <summary>
        /// User Role Repository constructor
        /// </summary>
        public UserRoleRepository()
        {
            userContext = new LarastruckingDBEntities();
        }
        #endregion

        #region List
        /// <summary>
        /// User Role List
        /// </summary>
        public IEnumerable<UserRoleDTO> List => throw new NotImplementedException();
        #endregion

        #region Add
        /// <summary>
        /// Add User Role
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public UserRoleDTO Add(UserRoleDTO entity)
        {
            try
            {
                UserRoleDTO objUserRoleDTO = new UserRoleDTO();
                tblUserRole objtblUserRole = AutoMapperServices<UserRoleDTO, tblUserRole>.ReturnObject(entity);
                userContext.tblUserRoles.Add(objtblUserRole);
                objUserRoleDTO = AutoMapperServices<tblUserRole, UserRoleDTO>.ReturnObject(objtblUserRole);
                objUserRoleDTO.IsSuccess = userContext.SaveChanges() > 0 ? true : false;
                return objUserRoleDTO;
            }
            catch (Exception)
            {

                throw;
            }
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
            throw new NotImplementedException();
        }
        #endregion

        #region FindById
        /// <summary>
        /// Find User Role
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public UserRoleDTO FindById(int Id)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
        #endregion
    }
}

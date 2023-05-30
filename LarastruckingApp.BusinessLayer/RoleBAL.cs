using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.DAL.Interface;
using LarastruckingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer
{
    public class RoleBAL : IRoleBAL
    {
        #region Private Member
        /// <summary>
        /// Defining private member
        /// </summary>
        private IRoleDAL iRoleRepo;
        #endregion

        #region RoleBAL
        /// <summary>
        /// Constructor Injection
        /// </summary>
        /// <param name="iRoleDAL"></param>
        public RoleBAL(IRoleDAL iRoleDAL)
        {
            iRoleRepo = iRoleDAL;
        }
        #endregion

        #region List
        /// <summary>
        /// Role List
        /// </summary>
        public IEnumerable<RoleDTO> List
        {
            get
            {
                return iRoleRepo.List;
            }
        }
        #endregion

        #region Add
        /// <summary>
        /// Add Role
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public RoleDTO Add(RoleDTO entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete Role
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(RoleDTO entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region FindById
        /// <summary>
        /// Find Role by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        public RoleDTO FindById(int Id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Update
        /// <summary>
        /// Update Role 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public RoleDTO Update(RoleDTO entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region GetRole
        /// <summary>
        /// Get Role List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleDTO> GetRole()
        {
            return iRoleRepo.GetRole();
        }
        #endregion
    }
}

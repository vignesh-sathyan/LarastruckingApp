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
    public class RoleRepository : IRoleRepository
    {
        #region Private Member
        /// <summary>
        /// defining Private members
        /// </summary>
        private readonly LarastruckingDBEntities userContext;
        #endregion

        #region RoleRepository
        /// <summary>
        /// RoleRepository constructor
        /// </summary>
        public RoleRepository()
        {
            userContext = new LarastruckingDBEntities();
        }
        #endregion

        #region List
        /// <summary>
        /// Get Roles
        /// </summary>
        public IEnumerable<RoleDTO> List
        {
            get
            {
                try
                {


                    IEnumerable<RoleDTO> lstRoleDTO = AutoMapperServices<tblRole, RoleDTO>.ReturnObjectList(userContext.tblRoles.Where(role => role.IsActive == true && role.IsDeleted == false).ToList());
                    return lstRoleDTO;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        #endregion

        #region Add
        /// <summary>
        /// Method to add
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
        /// Method to delete
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(RoleDTO entity)
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

        #region FindById
        /// <summary>
        /// Method to find by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public RoleDTO FindById(int Id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region GetRole
        /// <summary>
        /// Method to get role
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleDTO> GetRole()
        {
            try
            {

                var result = (from r in userContext.tblRoles where r.IsActive == true select r).ToList();
                IEnumerable<RoleDTO> objRoleDTO = AutoMapperServices<tblRole, RoleDTO>.ReturnObjectList(result.ToList());
                return objRoleDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}

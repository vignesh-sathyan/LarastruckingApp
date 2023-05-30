using LarastruckingApp.DAL.Interface;
using LarastruckingApp.DTO;
using LarastruckingApp.Repository.IRepository;
using LarastruckingApp.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL
{
    public class RoleDAL : IRoleDAL
    {
        private readonly IRoleRepository iRoleRepo;
        public RoleDAL(IRoleRepository iRoleRepository)
        {
            iRoleRepo = iRoleRepository;
        }
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

        /// <summary>
        /// Add Role
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public RoleDTO Add(RoleDTO entity)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Delete Role 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(RoleDTO entity)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Find Role by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        public RoleDTO FindById(int Id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update Role 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public RoleDTO Update(RoleDTO entity)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Role List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleDTO> GetRole()
        {
            return iRoleRepo.GetRole();
        }
    }
}

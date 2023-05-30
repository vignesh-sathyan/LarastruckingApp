using LarastruckingApp.DAL.Interface;
using LarastruckingApp.DTO;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;

namespace LarastruckingApp.DAL
{
    /// <summary>
    /// User Role Data Access Layer
    /// </summary>
    public class UserRoleDAL : IUserRoleDAL
    {
        private readonly IUserRoleRepository iUserRoleRepo;
        public UserRoleDAL(IUserRoleRepository iUserRoleRepository)
        {
            iUserRoleRepo = iUserRoleRepository;
        }
        /// <summary>
        /// User Role Data List
        /// </summary>
        public IEnumerable<UserRoleDTO> List => throw new NotImplementedException();
        /// <summary>
        /// Add User Role
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public UserRoleDTO Add(UserRoleDTO entity)
        {
           return iUserRoleRepo.Add(entity);
        }
        /// <summary>
        /// Delete User Role
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(UserRoleDTO entity)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Find User Role Data
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public UserRoleDTO FindById(int Id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Update User Role
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public UserRoleDTO Update(UserRoleDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}

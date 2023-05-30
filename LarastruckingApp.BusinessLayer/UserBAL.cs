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
    public class UserBAL : IUserBAL
    {
        #region Private Member
        /// <summary>
        /// Private member
        /// </summary>
        private IUserDAL iUserRepo;
        #endregion

        #region UserBAL
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iUserDAL"></param>
        public UserBAL(IUserDAL iUserDAL)
        {
            iUserRepo = iUserDAL;
        }
        #endregion

        #region List
        /// <summary>
        /// method to get list of user
        /// </summary>
        public IEnumerable<UserDTO> List
        {
            get
            {
                return iUserRepo.List;
            }
        }
        #endregion

        #region Add
        /// <summary>
        /// method to add user
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public UserDTO Add(UserDTO entity)
        {
            return iUserRepo.Add(entity);
        }
        #endregion

        #region Delete
        /// <summary>
        /// method to delete user
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(UserDTO entity)
        {
            return iUserRepo.Delete(entity);
        }
        #endregion

        #region FindById
        /// <summary>
        /// method to get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserDTO FindById(int id)
        {
            return iUserRepo.FindById(id);
        }
        #endregion

        #region Update
        /// <summary>
        /// method to update user
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public UserDTO Update(UserDTO entity)
        {
            return iUserRepo.Update(entity);
        }
        #endregion

        #region UpdateUser
        /// <summary>
        /// method to update user
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateUser(UserDTO entity)
        {
            return iUserRepo.UpdateUser(entity);
        }
        #endregion

        #region FindByUsername
        /// <summary>
        /// method to FindByUsername
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public UserDTO FindByUsername(UserDTO entity)
        {
            // UserDAL _userdal = new UserDAL();
            return iUserRepo.FindByUsername(entity);
        }
        #endregion

        #region Login
        /// <summary>
        /// FindByUsername
        /// </summary>
        /// <param name="objUserDTO"></param>
        /// <returns></returns>
        public IEnumerable<UserDTO> Login(UserDTO objUserDTO)
        {
            return iUserRepo.Login(objUserDTO);
        }
        #endregion

        #region AddUserRoleRegisteration
        /// <summary>
        /// method to Add User Role Registeration
        /// </summary>
        /// <param name="objUserDTO"></param>
        /// <returns></returns>
        public int AddUserRoleRegisteration(UserDTO objUserDTO)
        {
            return iUserRepo.AddUserRoleRegisteration(objUserDTO);
        }
        #endregion
    }
}

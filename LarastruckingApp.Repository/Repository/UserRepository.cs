using LarastruckingApp.DTO;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using LarastruckingApp.Resource;
using System;
using System.Collections.Generic;
using System.Linq;


namespace LarastruckingApp.Repository.Repository
{
    public class UserRepository : IUserRepository
    {

        #region Private Member
        /// <summary>
        /// defining Private members
        /// </summary>
        private readonly LarastruckingDBEntities userContext;
        #endregion

        #region UserRepository
        /// <summary>
        /// UserRepository constructor
        /// </summary>
        public UserRepository()
        {
            userContext = new LarastruckingDBEntities();
        }
        #endregion

        #region List
        /// <summary>
        /// Method to get list
        /// </summary>
        public IEnumerable<UserDTO> List
        {
            get
            {
                try
                {
                    var data = (from U in userContext.tblUsers
                                join UR in userContext.tblUserRoles on U.Userid equals UR.UserID
                                join R in userContext.tblRoles on UR.RoleID equals R.RoleID
                                where U.IsDeleted == false && (R.RoleName != LarastruckingResource.UserRole_Driver && R.RoleName != LarastruckingResource.UserRole_Customer)
                                select new UserDTO
                                {
                                    Userid = U.Userid,
                                    RoleID = R.RoleID,
                                    RoleName = R.RoleName ?? string.Empty,
                                    FirstName = U.FirstName ?? string.Empty,
                                    LastName = U.LastName ?? string.Empty,
                                    UserName = U.UserName ?? string.Empty,
                                    UserType = U.UserType ?? string.Empty,
                                    IsActive = U.IsActive
                                }).OrderByDescending(o => o.Userid);
                    return data.ToList();
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
        ///  Method to add
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public UserDTO Add(UserDTO entity)
        {
            UserDTO objUserDTO = new UserDTO();
            tblUser objtblUser = AutoMapperServices<UserDTO, tblUser>.ReturnObject(entity);
            userContext.tblUsers.Add(objtblUser);
            objUserDTO = AutoMapperServices<tblUser, UserDTO>.ReturnObject(objtblUser);
            objUserDTO.IsSuccess = userContext.SaveChanges() > 0 ? true : false;
            objUserDTO.Userid = objtblUser.Userid;
            return objUserDTO;
        }
        #endregion

        #region AddUserRoleRegisteration
        /// <summary>
        ///  Method to add user role registration
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int AddUserRoleRegisteration(UserDTO dto)
        {
            try
            {


                var response = userContext.usp_UserRegistration(dto.UserName,
                    dto.Password,
                    dto.FirstName,
                    dto.LastName,
                    0,
                    dto.RoleID,
                    dto.IsActive,
                    dto.IsDeleted,
                    dto.GUID,
                    DateTime.UtcNow,
                    DateTime.UtcNow,
                    dto.UserType).ToList();

                if (response.Count > 0)
                {
                    if (response[0].Equals("EXIST"))
                    {
                        return 0;
                    }
                    else if (response[0].Equals("INSERTED"))
                    {
                        return 1;
                    }
                }

                return -1;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Delete
        /// <summary>
        ///  Method to delete
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(UserDTO entity)
        {
            try
            {


                bool result = false;
                var table = userContext.tblUsers.Find(entity.Userid);
                if (table != null)
                {
                    table.IsDeleted = true;
                    table.IsActive = false;
                    userContext.Entry(table).State = System.Data.Entity.EntityState.Modified;
                    result = userContext.SaveChanges() > 0 ? true : false;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Update
        /// <summary>
        ///  Method to get update
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public UserDTO Update(UserDTO entity)
        {
            try
            {
                UserDTO objUserDTO = new UserDTO();

                var objtblUser = userContext.tblUsers.Find(entity.Userid);
                if (objtblUser != null)
                {

                    objtblUser.Userid = entity.Userid;
                    objtblUser.FirstName = entity.FirstName;
                    objtblUser.LastName = entity.LastName;
                    objtblUser.UserType = entity.UserType;
                    objtblUser.IsActive = entity.IsActive;
                    objtblUser.ModifiedBy = entity.ModifiedBy;
                    objtblUser.ModifiedOn = entity.ModifiedOn;
                    if (!string.IsNullOrEmpty(entity.Password))
                    {
                        objtblUser.Password = entity.Password;
                    }
                    userContext.Entry(objtblUser).State = System.Data.Entity.EntityState.Modified;
                    objUserDTO = AutoMapperServices<tblUser, UserDTO>.ReturnObject(objtblUser);
                    objUserDTO.IsSuccess = userContext.SaveChanges() > 0 ? true : false;

                }
                return objUserDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Update User
        /// <summary>
        ///  Method to updatet user
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateUser(UserDTO entity)
        {
            try
            {


                var isEmailExist = userContext.tblUsers.Any(u => u.UserName == entity.UserName
                                                                && u.IsDeleted != true
                                                                && u.Userid != entity.Userid);

                if (isEmailExist)
                {
                    return -1; // -1 = Username already taken by different user
                }
                else
                {
                    var dbEntry = userContext.tblUsers.Find(entity.Userid);

                    if (dbEntry != null)
                    {
                        dbEntry.Userid = entity.Userid;
                        dbEntry.UserName = entity.UserName;
                        dbEntry.FirstName = entity.FirstName;
                        dbEntry.LastName = entity.LastName;
                        dbEntry.UserType = entity.UserType;
                        dbEntry.IsActive = entity.IsActive;
                        dbEntry.ModifiedBy = entity.ModifiedBy;
                        dbEntry.ModifiedOn = entity.ModifiedOn;
                        if (!string.IsNullOrEmpty(entity.Password))
                        {
                            dbEntry.Password = entity.Password;
                        }

                        userContext.Entry(dbEntry).State = System.Data.Entity.EntityState.Modified;

                        var userRole = userContext.tblUserRoles.Where(r => r.UserID == entity.Userid);

                        if (userRole.Any())
                        {
                            userContext.tblUserRoles.RemoveRange(userRole);
                        }
                        var roles = new tblUserRole()
                        {
                            UserID = entity.Userid,
                            RoleID = entity.RoleID,
                            CreatedOn = entity.CreatedOn,
                            ModifiedOn = entity.ModifiedOn,
                            ModifiedBy = entity.ModifiedBy
                        };
                        userContext.tblUserRoles.Add(roles);

                        return userContext.SaveChanges();
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region ResetPassword
        /// <summary>
        ///  Method to reset password
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public UserDTO ResetPassword(UserDTO entity)
        {
            try
            {
                UserDTO objUserDTO = new UserDTO();

                var objtblUser = userContext.tblUsers.Find(entity.Userid);
                if (objtblUser != null)
                {

                    objtblUser.Userid = entity.Userid;
                    objtblUser.FirstName = entity.FirstName;
                    objtblUser.LastName = entity.LastName;
                    objtblUser.UserName = entity.UserName;
                    objtblUser.IsActive = entity.IsActive;
                    objtblUser.ModifiedBy = entity.ModifiedBy;
                    objtblUser.ModifiedOn = entity.ModifiedOn;
                    if (!string.IsNullOrEmpty(entity.Password))
                    {
                        objtblUser.Password = entity.Password;
                    }
                    userContext.Entry(objtblUser).State = System.Data.Entity.EntityState.Modified;
                    objUserDTO = AutoMapperServices<tblUser, UserDTO>.ReturnObject(objtblUser);
                    objUserDTO.IsSuccess = userContext.SaveChanges() > 0 ? true : false;

                }
                return objUserDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region FindById
        /// <summary>
        ///  Method to find by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public UserDTO FindById(int Id)
        {
            try
            {


                UserDTO objUserDTO = (
                                from U in userContext.tblUsers
                                join R in userContext.tblUserRoles on U.Userid equals R.UserID
                                where U.Userid == Id && U.IsDeleted == false
                                select new UserDTO
                                {
                                    Userid = U.Userid,
                                    RoleID = (int)R.RoleID,
                                    FirstName = U.FirstName,
                                    LastName = U.LastName,
                                    UserName = U.UserName,
                                    UserType = U.UserType,
                                    IsActive = U.IsActive
                                }
                              ).FirstOrDefault();

                return objUserDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Login
        /// <summary>
        ///  Method to login
        /// </summary>
        /// <param name="objUserDTO"></param>
        /// <returns></returns>
        public IEnumerable<UserDTO> Login(UserDTO objUserDTO)
        {
            try
            {
                var dbExists = userContext.usp_AuthenticateUser(objUserDTO.UserName.ToUpper(), objUserDTO.Password).ToList();

                List<UserDTO> users = new List<UserDTO>();
                if (dbExists.Count > 0)
                {
                    foreach (var u in dbExists)
                    {
                        UserDTO user = new UserDTO();
                        user.IsMenu = (u.IsMenu == true) ? true : false;
                        user.DisplayIcon = u.DisplayIcon;
                        user.DisplayOrder = u.DisplayOrder;
                        user.PageName = u.PageName;
                        user.AreaName = u.AreaName;
                        user.ActionName = u.ActionName;
                        user.FirstName = u.FirstName;
                        user.LastName = u.LastName;
                        user.ControllerName = u.ControllerName;
                        user.RoleName = u.RoleName;
                        user.CanInsert = (u.CanInsert == true) ? true : false;
                        user.CanUpdate = (u.CanUpdate == true) ? true : false;
                        user.CanDelete = (u.CanDelete == true) ? true : false;
                        user.CanView = (u.CanView == true) ? true : false;
                        user.IsPricingMethod = (u.IsPricingMethod == true) ? true : false;
                        user.UserName = u.UserName;
                        user.Userid = u.Userid;

                        users.Add(user);
                    }

                }

                return users;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region FindByUsername
        /// <summary>
        ///  Method to find by username
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public UserDTO FindByUsername(UserDTO entity)
        {
            try
            {
                UserDTO objUserDTO = null;
                var result = (from r in userContext.tblUsers where r.UserName == entity.UserName.Trim() && !r.IsDeleted select r).FirstOrDefault();
                if (result != null)
                {
                    objUserDTO = AutoMapperServices<tblUser, UserDTO>.ReturnObject(result);
                }
                return objUserDTO;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}

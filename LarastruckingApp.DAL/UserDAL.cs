using LarastruckingApp.DAL.Interface;
using LarastruckingApp.DTO;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using LarastruckingApp.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL
{
    public class UserDAL : IUserDAL
    {
        private readonly IUserRepository iUserRepo;
        public UserDAL(IUserRepository iUserRepository)
        {
            iUserRepo = iUserRepository;
            
        }
     
        public IEnumerable<UserDTO> List
        {
            get
            {
                return iUserRepo.List;
            }
        }

        public UserDTO Add(UserDTO entity)
        {
           return iUserRepo.Add(entity);
            
        }

        public bool Delete(UserDTO entity)
        {
           return iUserRepo.Delete(entity);
        }

        public UserDTO FindById(int Id)
        {
           return iUserRepo.FindById(Id);
        }

        public UserDTO Update(UserDTO entity)
        {
            return iUserRepo.Update(entity);
        }
        public int UpdateUser(UserDTO entity)
        {
            return iUserRepo.UpdateUser(entity);
        }
        
        public UserDTO FindByUsername(UserDTO entity)
        {
            return iUserRepo.FindByUsername(entity);
        }
        public IEnumerable<UserDTO> Login(UserDTO entity)
        {
           return iUserRepo.Login(entity);
        }
        public int AddUserRoleRegisteration(UserDTO entity)
        {
            return iUserRepo.AddUserRoleRegisteration(entity);
        }
    }
}

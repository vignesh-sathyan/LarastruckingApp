using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LarastruckingApp.Resource;

namespace LarastruckingApp.ViewModel
{
    public class UserViewModel
    {
        public int Userid { get; set; }
        //[Required(ErrorMessageResourceName = "UsernameRequired", ErrorMessageResourceType = typeof(LarastruckingResource))]
        [EmailAddress(ErrorMessage = "Please Enter Valid EmailID")]
        [DisplayName("Email")]
        public string UserName { get; set; } 
        public string Password { get; set; }        
        [DisplayName("First Name")]
        [Required(ErrorMessage = "Please enter first name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public System.DateTime CreatedOn { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string result { get; set; }
        public int RoleID { get; set; }
        public string GUID { get; set; }
        public Nullable<System.DateTime> ResetPasswordDateTime { get; set; }
        public Nullable<System.DateTime> GuidGenratedDateTime { get; set; }
        //public virtual ICollection<UserRoleViewModel> tblUserRoles { get; set; }
        [DisplayName("User Type")]
        public string UserType { get; set; }
        public string RoleName { get; set; }
    }
}
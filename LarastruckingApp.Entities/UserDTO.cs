using System;
using System.Collections.Generic;

namespace LarastruckingApp.DTO
{
    public class UserDTO:CommonDTO
    {
        public int Userid { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string GUID { get; set; }
        public Nullable<System.DateTime> ResetPasswordDateTime { get; set; }
        public Nullable<System.DateTime> GuidGenratedDateTime { get; set; }
        public string UserType { get; set; }
        public string OwnedBy { get; set; }
        public int RoleID { get; set; }               
        public string RoleName { get; set; }
        public int? DisplayOrder { get; set; }
        public string PageName { get; set; }
        public string AreaName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool IsMenu { get; set; }
        public string DisplayIcon { get; set; }
        public bool CanInsert { get; set; }
        public bool CanView { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public bool IsPricingMethod { get; set; }
    }
}

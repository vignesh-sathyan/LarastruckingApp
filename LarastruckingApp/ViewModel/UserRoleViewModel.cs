using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LarastruckingApp.ViewModel
{
    public class UserRoleViewModel
    {
        public int UserRoleID { get; set; }
        public long UserID { get; set; }
        public int RoleID { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; } = DateTime.Now;
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
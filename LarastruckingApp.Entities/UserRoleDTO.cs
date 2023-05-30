using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DTO
{
    public class UserRoleDTO:CommonDTO
    {
        public int UserRoleID { get; set; }
        public long UserID { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; } = DateTime.Now;
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }

        //public virtual tblRole tblRole { get; set; }
        //public virtual tblUser tblUser { get; set; }

    }
}

using LarastruckingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities
{
   public class VendorNconsigneeDTO:CommonDTO
    {
        public int VendorNconsigneeId { get; set; }
        public string VendorNconsigneeName { get; set; }
        public string Address { get; set; }
        public Nullable<int> Country { get; set; }
        public Nullable<int> State { get; set; }
        public string StateName { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
       public string Fax { get; set; }
        public string Email { get; set; }
        public string Zip { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> ModifyBy { get; set; }
        public Nullable<System.DateTime> ModifyOn { get; set; }
        public Nullable<bool> IsConsignee { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DTO
{
   public class CustomerDTO: CommonDTO
    {
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Contact { get; set; }
        public string Account { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string AddressOne { get; set; }
        public string AddressTwo { get; set; }
        public Nullable<int> Country { get; set; }
        public Nullable<int> State { get; set; }
        public string City { get; set; }
        public string StateName { get; set; }
        public string Zip { get; set; }
        public string Comments { get; set; }
        public bool IsPickDropLocation { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}

using LarastruckingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities
{
    public class AddressDTO : CommonDTO
    {
        public int AddressId { get; set; }
        public int AddressTypeId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public int? State { get; set; }
        public string StateName { get; set; }
        public int? Country { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public string Extension { get; set; }
        public string CompanyName { get; set; }
        public string AdditionalPhone1 { get; set; }
        public string Extension1 { get; set; }
        public string AdditionalPhone2 { get; set; }
        public string Extension2 { get; set; }

        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string AddressTypeName { get; set; }
        public string Comments { get; set; }
        public bool IsAppointmentRequired { get; set; }
        public string Website { get; set; }
        public string CompanyNickname { get; set; }
    }
}

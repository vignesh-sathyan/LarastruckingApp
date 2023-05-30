using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LarastruckingApp.ViewModel
{
    public class AddressViewModel
    {
        public int AddressId { get; set; } = 0;
        [DisplayName("Address Type")]
        public int AddressTypeId { get; set; }
        [DisplayName("Address 1")]
        [Required(ErrorMessage = "Please enter address")]
        public string Address1 { get; set; }

        [DisplayName("Address 2")]
        public string Address2 { get; set; }
        [DisplayName("City")]
        [Required(ErrorMessage = "Please enter city")]
        public string City { get; set; }
        public int? State { get; set; }
        public int? Country { get; set; }

        [DisplayName("Zip Code")]
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        [DisplayName("Contact Person")]
        public string ContactPerson { get; set; }
        public string Extension { get; set; }

        public string CompanyName { get; set; }
        public string AdditionalPhone1 { get; set; }
        public string Extension1 { get; set; }
        public string AdditionalPhone2 { get; set; }
        public string Extension2 { get; set; }


        public DateTime? CreateOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string AddressTypeName { get; set; }
        public string StateName { get; set; }
        public string Comments { get; set; }
        public bool IsAppointmentRequired { get; set; }
        public string Website { get; set; }
        public string CompanyNickname { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LarastruckingApp.ViewModel
{
    public class CustomerViewModel : PermissionsViewModel
    {
        public long CustomerID { get; set; }

        [DisplayName("Customer Name")]
        [Required(ErrorMessage = "Please enter customer name")]
        public string CustomerName { get; set; }

        [DisplayName("Contact Person")]
        [Required(ErrorMessage = "Please enter contact person")]
        public string Contact { get; set; }

        [DisplayName("Account")]
        [Required(ErrorMessage = "Please enter account")]
        public string Account { get; set; }

        [DisplayName("Phone")]
        // [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        [Required(ErrorMessage = "Please enter phone")]
        public string Phone { get; set; }

        [DisplayName("Fax")]
        public string Fax { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; }


        public string Website { get; set; }

        [DisplayName("Address 1")]
        [Required(ErrorMessage = "Please enter address 1")]
        public string AddressOne { get; set; }
        [DisplayName("Address 2")]
        public string AddressTwo { get; set; }

        [DisplayName("Country")]
        [Required(ErrorMessage = "Please select country")]
        public Nullable<int> Country { get; set; }

        [DisplayName("State")]
        [Required(ErrorMessage = "Please select state")]
        public Nullable<int> State { get; set; }


        [DisplayName("City")]
        [Required(ErrorMessage = "Please enter city")]
        public string City { get; set; }

        [DisplayName("Zip Code")]
        [Required(ErrorMessage = "Please enter zip code")]
        public string Zip { get; set; }
        public string Comments { get; set; }
        [DisplayName("Add pickup/delivery location")]
        public bool IsPickDropLocation { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        [DisplayName("Active")]
        public bool IsActive { get; set; }
    }
}
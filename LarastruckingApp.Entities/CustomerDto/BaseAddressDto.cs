using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.CustomerDto
{
    public class BaseAddressDto
    {
        public int BaseAddressId { get; set; }
        public int BaseAddressCategoryId { get; set; }

        [DisplayName("Address 1")]
        [Required(ErrorMessage = "Please enter address 1")]
        public string Address1 { get; set; }

        [DisplayName("Address 2")]
        public string Address2 { get; set; }

        [DisplayName("City")]
        [Required(ErrorMessage = "Please enter city")]
        public string City { get; set; }

        [DisplayName("State")]
        [Required(ErrorMessage = "Please select state")]
        public int StateId { get; set; }

        [DisplayName("Country")]
        [Required(ErrorMessage = "Please select country")]
        public int CountryId { get; set; }

        [DisplayName("Zip Code")]
        [Required(ErrorMessage = "Please enter zip code")]
        public string ZipCode { get; set; }

        [DisplayName("Phone")]
        [Required(ErrorMessage = "Please enter phone")]
        public string PhoneNumber { get; set; }

        public string Fax { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; }

    }
}

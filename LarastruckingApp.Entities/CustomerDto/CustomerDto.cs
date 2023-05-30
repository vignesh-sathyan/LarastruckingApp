using LarastruckingApp.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.CustomerDto
{
    public class CustomerDto
    {
        #region Customer Info
        public long CustomerId { get; set; }
        public string GuidInUser { get; set; }

        [DisplayName("Customer Name")]
        [Required(ErrorMessage = "Please enter customer name")]
        public string CustomerName { get; set; }

        public string Website { get; set; }
        public string Comments { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }
        public bool IsFullFledgedCustomer { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsSuccess { get; set; }
        public string Response { get; set; }
        public bool IsDeleted { get; set; }

        #endregion

        #region Billing Address Info
        public int BaseAddressId { get; set; }


        [DisplayName("Address 1")]
        [Required(ErrorMessage = "Please enter address 1")]
        public string BillingAddress1 { get; set; }

        [DisplayName("Address 2")]
        public string BillingAddress2 { get; set; }

        [DisplayName("City")]
        [Required(ErrorMessage = "Please enter city")]
        public string BillingCity { get; set; }

        [DisplayName("State")]
        [Required(ErrorMessage = "Please select state")]
        public int? BillingStateId { get; set; }
        public string BillingState { get; set; }

        [DisplayName("Country")]
        [Required(ErrorMessage = "Please select country")]
        public int? BillingCountryId { get; set; }
        public string BillingCountry { get; set; }

        [DisplayName("Zip Code")]
        [Required(ErrorMessage = "Please enter zip code")]
        public string BillingZipCode { get; set; }

        [DisplayName("Phone")]
        [Required(ErrorMessage = "Please enter phone")]
        public string BillingPhoneNumber { get; set; }

        public string BillingFax { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string BillingEmail { get; set; }
        public string PALAccount { get; set; }
       
        public string BillingExtension { get; set; }
        #endregion

        #region Shipping Address Info
        [DisplayName("Address 1")]
        [Required(ErrorMessage = "Please enter address 1")]
        public string ShippingAddress1 { get; set; }

        [DisplayName("Address 2")]
        public string ShippingAddress2 { get; set; }

        [DisplayName("City")]
        [Required(ErrorMessage = "Please enter city")]
        public string ShippingCity { get; set; }

        [DisplayName("State")]
        [Required(ErrorMessage = "Please select state")]
        public int? ShippingStateId { get; set; }
        public string ShippingState { get; set; }

        [DisplayName("Country")]
        [Required(ErrorMessage = "Please select country")]
        public int? ShippingCountryId { get; set; }
        public string ShippingCountry { get; set; }

        [DisplayName("Zip Code")]
        [Required(ErrorMessage = "Please enter zip code")]
        public string ShippingZipCode { get; set; }

        [DisplayName("Phone")]
        [Required(ErrorMessage = "Please enter phone")]
        public string ShippingPhoneNumber { get; set; }

        public string ShippingFax { get; set; }

       // [Required(ErrorMessage = "Please enter email")]
       // [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string ShippingEmail { get; set; }
        public string ShippingExtension { get; set; }
        #endregion

        #region Customer Contact
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<CustomerContact> CustomerContacts { get; set; }
        #endregion

        public bool IsPickDropLocation { get; set; }
        public bool IsWaitingTimeRequired { get; set; }
        public bool IsTemperatureRequired { get; set; }
    }
}

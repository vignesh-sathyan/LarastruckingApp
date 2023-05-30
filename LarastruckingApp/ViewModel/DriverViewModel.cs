using LarastruckingApp.Repository.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LarastruckingApp.ViewModel
{
    public class DriverViewModel
    {
        public int DriverID { get; set; }

        [Required(ErrorMessage = "Please Enter First Name")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Please Enter Valid EmailID")]
        [DisplayName("Email")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Please Enter Address")]
        [DisplayName("Address 1")]
        public string Address1 { get; set; }

        [DisplayName("Address 2")]
        public string Address2 { get; set; }

        //[Required(ErrorMessage = "Please Select Country")]
        [DisplayName("Country")]
        public int Country { get; set; }
        public string CitizenShip { get; set; }

        //[Required(ErrorMessage = "Please Select State")]
        public int State { get; set; }
        [Required(ErrorMessage = "Please Select City")]
        public string City { get; set; }
        public string Phone { get; set; }


        [DisplayName("Cell #")]
        public string CellNumber { get; set; }

        [DisplayName("Blood Group")]
        public string BloodGroup { get; set; }
        public string Vehicle { get; set; }
        [DisplayName("Medical Conditions")]
        public string MedicalConditions { get; set; }
        [DisplayName("Contact 1")]
        public string EmergencyContactOne { get; set; }

        [DisplayName("Phone No. 1")]
        public string EmergencyPhoneNoOne { get; set; }
        [DisplayName("Relationship")]
        public string RelationshipStatus1 { get; set; }
        


        [DisplayName("Contact 2")]
        public string EmergencyContactTwo { get; set; }
        [DisplayName("Phone No. 2")]
        public string EmergencyPhoneNoTwo { get; set; }
        [DisplayName("Relationship")]
        public string RelationshipStatus2 { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }

        public List<DriverDocument> DriverDocuments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<DriverDocumentViewModel> DriverDocumentList { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public int[] RemoveElement { get; set; }

        public string ZipCode { get; set; }

        [DisplayName("Preferred Language")]
        public int LanguageId { get; set; }

        [DisplayName("Extension")]
        public string Extension { get; set; }

        [DisplayName("STA #")]
        public string STANumber { get; set; }

        [DisplayName("Expiration Date")]
        public Nullable<System.DateTime> ExpirationDate { get; set; }
    }



    public class DriverDocument
    {
        public int DocumentId { get; set; }
        public string UploadDocument { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentName { get; set; }
        public DateTime? DocumentExpiryDate { get; set; }
        public string ImageName { get; set; }
    }
}
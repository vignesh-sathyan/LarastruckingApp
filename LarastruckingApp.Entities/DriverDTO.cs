using LarastruckingApp.Entities.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DTO
{
    public class DriverDTO : CommonDTO
    {
        public string GuidInUser { get; set; }
        public int DriverID { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CitizenShip { get; set; }
        public int Country { get; set; }
        public int State { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string CellNumber { get; set; }
       
        public string BloodGroup { get; set; }
        public string Vehicle { get; set; }
        public string MedicalConditions { get; set; }
        public string EmergencyContactOne { get; set; }
        public string EmergencyPhoneNoOne { get; set; }
        public string RelationshipStatus1 { get; set; }

        public string EmergencyContactTwo { get; set; }
        public string EmergencyPhoneNoTwo { get; set; }
        public string RelationshipStatus2 { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DOB { get; set; }
        public string ZipCode { get; set; }
        public string DriverLicence { get; set; }
        public string STANumber { get; set; }
        public bool FullTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<DriverDocumentDTO> DriverDocumentList { get; set; }
        public List<DriverDocumentDto> DriverDocumentDto { get; set; }
        public int? LanguageId { get; set; }
        public string Extension { get; set; }
        public DateTime? ExpirationDate { get; set; }

    }
    
}

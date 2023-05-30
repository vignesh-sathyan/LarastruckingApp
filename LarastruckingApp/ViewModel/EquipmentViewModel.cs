using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Equipments;
using LarastruckingApp.Repository.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LarastruckingApp.ViewModel
{
    public class EquipmentViewModel
    {
        public int EDID { get; set; }
        [Display(Name = "Equipment Type")]
        [Required(ErrorMessage = "The VehicleType field is required")]       
        public int VehicleType { get; set; }
        public string Model { get; set; }
        [Display(Name = "Licence Plate")]
        [Required(ErrorMessage = "The LicencePlate field is required")]
        public string LicencePlate { get; set; }
        [Display(Name = "Equipment No")]
        [Required(ErrorMessage = "The Equipment No field is required")]
        public string EquipmentNo { get; set; }
        [Display(Name = "W")]
        public string WDimension { get; set; }
        [Display(Name = "H")]
        public string HDimension { get; set; }
        [Display(Name = "Dimension")]
        public string LDimension { get; set; }
        [Required(ErrorMessage = "The Make field is required")]
        public string Make { get; set; }
        [Display(Name = "VIN")]
        [Required(ErrorMessage = " The VIN field is required")]
        public string VIN { get; set; }
        [Display(Name = "Cubic Feet")]
        public string CubicFeet { get; set; }

        [Display(Name ="Owned By")]
        public string Ownedby { get; set; }
        public bool Active { get; set; }
        public string Comments { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        [Display(Name = "Lessee Provider")]
        public string LeaseCompanyName { get; set; }
        public string VehicleTypeName { get; set; }
        public int? Decal { get; set; }
        [Display(Name = "Registration Expiration")]       
        public DateTime? RegistrationExpiration { get; set; }
        [Display(Name = "Year")]
        [Required(ErrorMessage = "The Year is required")]
        public int? Year { get; set; }
        public string Color { get; set; }
        [Display(Name = "Lease Start Date")]       
        public DateTime? LeaseStartDate { get; set; }
        [Display(Name = "Lease End Date")]        
        public DateTime? LeaseEndDate { get; set; }
        [Display(Name = "Registration File")]
        public string RegistrationImageURL { get; set; }
        public string RegistrationImageName { get; set; }
        [Display(Name = "Insurance File")]
        public string InsuranceImageURL { get; set; }
        public string InsauranceImageName { get; set; }
        public string hdnInsuranceImageURL { get; set; }
        public string hdnInsuranceImageName { get; set; }
        public string hdnRegistrationImageURL { get; set; }
        public string hdnRegistrationImageName { get; set; }
        [Display(Name = "QR Code No.")]
        public string QRCodeNo { get; set; }

        private List<VehicleTypeDTO> equipmentTypes = new List<VehicleTypeDTO>();
        private List<FreightTypeDTO> freightTypes = new List<FreightTypeDTO>();
        private List<DoorTypeDto> doorTypeDto = new List<DoorTypeDto>();

        public List<VehicleTypeDTO> GetEquipmentTypes
        {
            get
            {
                return equipmentTypes;
            }
        }
        public List<FreightTypeDTO> GetFreightTypes
        {
            get
            {
                return freightTypes;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<int> DoorTypeIds { get; set; }
        public List<DoorTypeDto> GetDoorTypes
        {
            get
            {
                return doorTypeDto;
            }
        }

        public bool IsOutOfService { get; set; }
        public DateTime? OutOfServiceStartDate { get; set; }
        public DateTime? OutOfServiceEndDate { get; set; }
        public List<int> FreightTypeIds { get; set; }
        public string FreightType { get; set; }
        public string MaxLoad { get; set; }
        public string RollerBed { get; set; }

    }
}
using LarastruckingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities
{
    /// <summary>
    /// Equipment DTO
    /// </summary>
    public class EquipmentDTO: CommonDTO 
    {
        public int EDID { get; set; }
        public int VehicleType { get; set; }
        public string Model { get; set; }
        public string EquipmentNo { get; set; }
        public string LicencePlate { get; set; }      
        public string WDimension { get; set; }
        public string HDimension { get; set; }
        public string LDimension { get; set; }
        public string Make { get; set; }
        public string VIN { get; set; }
        public string CubicFeet { get; set; }
        public string Ownedby { get; set; }
        public bool Active { get; set; }
        public string Comments { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string LeaseCompanyName { get; set; }
        public string VehicleTypeName { get; set; }
        public int? Decal { get; set; }
        public DateTime? RegistrationExpiration { get; set; }
        public int? Year { get; set; }
        public string Body { get; set; }
        public string Color { get; set; }
        public DateTime? LeaseStartDate { get; set; }
        public DateTime? LeaseEndDate { get; set; }
        public string RegistrationImageURL { get; set; }
        public string RegistrationImageName { get; set; }
        public string InsuranceImageURL { get; set; }
        public string InsauranceImageName { get; set; }

        public bool? IsOutOfService { get; set; }
        public DateTime? OutOfServiceStartDate { get; set; }
        public DateTime? OutOfServiceEndDate { get; set; }
        public List<int> DoorTypeIds { get; set; }
        public List<int> FreightTypeIds { get; set; }
        public string FreightType { get; set; }
        public string MaxLoad { get; set; }
        public string RollerBed { get; set; }
        public List<string> FreightTypeList { get; set; }
        public string QRCodeNo { get; set; }
    }
}

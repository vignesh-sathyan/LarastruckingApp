using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Equipments
{
    public class AddEquipmentDto
    {
        private List<VehicleTypeDTO> equipmentTypes = new List<VehicleTypeDTO>();
        private List<FreightTypeDTO> freightTypes = new List<FreightTypeDTO>();

        public int EquipmentId { get; set; }

        [Required(ErrorMessage = "Equipment number is required")]
        public string EquipmentNumber { get; set; }
        public int EquipmentTypeId { get; set; }

        [Required(ErrorMessage = "License plate is required")]
        public string LicensePlate { get; set; }
        public int Decal { get; set; }
        public DateTime RegistrationExpirationDate { get; set; }

        [Required(ErrorMessage = "Please select a year")]
        public int MakeYear { get; set; }

        [Required(ErrorMessage = "Make field is required")]
        public string Make { get; set; }
        public int Color { get; set; }

        [Required(ErrorMessage = "Vin is required")]
        public string Vin { get; set; }
        public decimal Capacity { get; set; }
        public decimal DimensionWidth { get; set; }
        public decimal DimensionHeigth { get; set; }
        public decimal DimensionLength { get; set; }
        public int DoorTypeId { get; set; }
        public int FreightTypeId { get; set; }
        public string MaxLoad { get; set; }
        public int RollerBedId { get; set; }
        public string RegistrationFileUrl { get; set; }
        public string InsuranceFileUrl { get; set; }
        public int OwnedBy { get; set; }
        public string Lessee { get; set; }
        public DateTime LesseeStartDate { get; set; }
        public DateTime LesseeEndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsOutOfService { get; set; }
        public DateTime OutOfServiceStartDate { get; set; }
        public DateTime OutOfServiceEndDate { get; set; }
        public string Comment { get; set; }
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
    }
}

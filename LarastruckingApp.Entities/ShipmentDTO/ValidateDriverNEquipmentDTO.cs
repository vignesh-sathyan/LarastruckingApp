using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
  public  class ValidateDriverNEquipmentDTO
    {
        public int? ShipmentId { get; set; }
        public int? FumigationId { get; set; }
        public int? TrailerRentalId { get; set; }
        public int CustomerId { get; set; }
        public int? EquipmentId { get; set; }
        public int? DriverId { get; set; }
        public DateTime? FirstPickupArrivalDate { get; set; }
        public DateTime? LastPickupArrivalDate { get; set; }
        public List<ShipmentEquipmentNdriverDTO> ShipmentEquipmentNdriver { get; set; }
    }
}

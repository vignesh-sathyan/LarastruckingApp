using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
  public  class MatchEquipmentNDriverDTO
    {
        public bool IsShipment { get; set; }
        public string AWB { get; set; }
        public string CustomerPO { get; set; }
        public string ContainerNo { get; set; }
        public int? ShipmentId { get; set; }
        public int? FumigationId { get; set; }
        public int? DriverId { get; set; }
        public int? EquipmentId { get; set; } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
   public class GetShipmentEquipmentNDriverDTO
    {
        public int ShipmentEquipmentNDriverId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> DriverId { get; set; }
        public string DriverName { get; set; }
        public Nullable<int> EquipmentId { get; set; }
        public string EquipmentName { get; set; }
    }
}

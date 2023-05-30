using LarastruckingApp.Entities.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
    public class ShipmentEquipmentDTO
    {
        public int EDID { get; set; }
        public string VehicleType { get; set; }
        public string EquipmentNo { get; set; }
        public string FreightType { get; set; }
        public string DoorType { get; set; }
        public string MaxLoad { get; set; }
        public string Bed { get; set; }
        public List<string> FreightTypeList { get; set; }
        
        public List<string> DoorTypeList { get; set; }
        public bool IsAssigned { get; set; }
        public DateTime? ScheduledCheckIn { get; set; }

    }
}

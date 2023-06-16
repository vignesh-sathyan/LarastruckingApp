using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
    public class ShipmentDriverDetailDTO
    {
        public string DriverName { get; set; }
        public int? ShipmentId { get; set; }
        public string Status { get; set; }
        public string DeliveryLocation { get; set; }
        public string Equipment { get; set; }

    }
}

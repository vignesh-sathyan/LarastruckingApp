using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
    public class CopyShipmentDTO
    {
        public int ShipmentId { get; set; }
        public string AWB { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime? EstPickupArriaval { get; set; }
        public DateTime? EstDeliveryArrival { get; set; }
        public int CreatedBy{get;set;}
         public DateTime CreatedDate { get; set; }
    }
}

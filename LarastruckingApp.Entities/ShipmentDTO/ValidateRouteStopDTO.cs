using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
   public class ValidateRouteStopDTO
    {
        public int CustomerId { get; set; }
        public int PickupLocation { get; set; }
        public int DeliveryLocation { get; set; }
        public DateTime ArrivalDate { get; set; }
    }
}

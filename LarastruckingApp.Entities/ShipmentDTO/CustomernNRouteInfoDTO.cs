using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
    public class CustomernNRouteInfoDTO
    {
        public int CustomerId { get; set; }
        public int PickupLocationId { get; set; }
        public int DeliveryLocationId { get; set; }
        public DateTime? PickupArrivalDate { get; set; }
    }
}

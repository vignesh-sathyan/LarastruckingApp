using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver
{
    public class PreTripTimmingDetailsDto
    {
        public int ShippingRoutesId { get; set; }
        public Nullable<System.DateTime> DriverPickupArrival { get; set; }
        public Nullable<System.DateTime> DriverPickupDeparture { get; set; }
        public Nullable<System.DateTime> DriverDeliveryArrival { get; set; }
        public Nullable<System.DateTime> DriverDeliveryDeparture { get; set; }
        public string ReceiverName { get; set; }
    }
}

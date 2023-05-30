using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
    public class ShipmentRoutesStopDTO
    {
        public Nullable<int> RouteNo { get; set; }
        public Nullable<int> PickupLocationId { get; set; }
        public Nullable<System.DateTime> PickDateTime { get; set; }
        public Nullable<System.DateTime> PickDateTimeTo { get; set; }
        public Nullable<int> DeliveryLocationId { get; set; }    
        public Nullable<System.DateTime> DeliveryDateTime { get; set; }      
        public Nullable<System.DateTime> DeliveryDateTimeTo { get; set; }
        public string Comment { get; set; }
        public bool IsAppointmentRequired { get; set; }
        public bool IsPickUpWaitingTimeRequired { get; set; }
        public bool IsDeliveryWaitingTimeRequired { get; set; }
    }
}

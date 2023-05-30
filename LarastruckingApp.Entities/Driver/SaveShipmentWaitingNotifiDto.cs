using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver
{
    public class SaveShipmentWaitingNotifiDto
    {
        public int WatingNotificationId { get; set; }
        public Nullable<int> ShipmentId { get; set; }
        public Nullable<int> ShipmentRouteId { get; set; }
        public Nullable<System.DateTime> PickupArrivedOn { get; set; }
        public Nullable<System.DateTime> PickupDepartedOn { get; set; }
        public Nullable<System.DateTime> DeliveryArrivedOn { get; set; }
        public Nullable<System.DateTime> DeliveryDepartedOn { get; set; }
        public Nullable<int> PickUpLocationId { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public string EquipmentNo { get; set; }
        public Nullable<int> DriverId { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsEmailSentPWS { get; set; }
        public bool IsEmailSentPWE { get; set; }
        public bool IsEmailSentDWS { get; set; }
        public bool IsEmailSentDWE { get; set; }
        public Nullable<int> DeliveryLocationId { get; set; }

    }
}

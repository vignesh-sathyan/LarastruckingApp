using LarastruckingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
    public class GetShipmentRouteStopDTO : CommonDTO
    {
        public int? ShipmentId { get; set; }
        public int ShipmentRouteStopId { get; set; }
        public Nullable<int> RouteNo { get; set; }
        public Nullable<int> PickupLocationId { get; set; }
        public string PickupLocation { get; set; }
        public Nullable<System.DateTime> PickDateTime { get; set; }
        public Nullable<System.DateTime> PickDateTimeTo { get; set; }
        public Nullable<int> DeliveryLocationId { get; set; }
        public string DeliveryLocation { get; set; }
        public Nullable<System.DateTime> DeliveryDateTime { get; set; }
        public Nullable<System.DateTime> DeliveryDateTimeTo { get; set; }
        public string Comment { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? ActPickupArrival { get; set; }
        public Nullable<System.DateTime> ActPickupDeparture { get; set; }
        public Nullable<System.DateTime> ActDeliveryArrival { get; set; }
        public Nullable<System.DateTime> ActDeliveryDeparture { get; set; }
        public string ReceiverName { get; set; }
        public string DigitalSignature { get; set; }
        public bool IsAppointmentRequired { get; set; }
        public bool IsAppointmentNeeded { get; set; }
        public bool IsPickUpWaitingTimeRequired { get; set; }
        public bool IsDeliveryWaitingTimeRequired { get; set; }
        public bool IsWaitingTimeNeeded { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public string DigitalSignaturePath { get; set; }
        public DateTime? SignatureDateTime { get; set; }
    }
}

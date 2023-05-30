using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver
{
    public class SaveFumigationWaitingNotifiDto
    {
        public int FumiWatingNotificationId { get; set; }
        public Nullable<int> FumigationId { get; set; }
        public Nullable<int> FumigationRoutsId { get; set; }
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

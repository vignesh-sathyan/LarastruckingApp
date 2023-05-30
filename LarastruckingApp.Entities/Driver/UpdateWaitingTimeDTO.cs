using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver
{
    public class UpdateWaitingTimeDTO
    {
        public int WatingNotificationId { get; set; }
        public DateTime? PickupArrivedOn { get; set; }
        public DateTime? PickupDepartedOn { get; set; }
        public DateTime? DeliveryArrivedOn { get; set; }
        public DateTime? DeliveryDepartedOn { get; set; }
    }
}

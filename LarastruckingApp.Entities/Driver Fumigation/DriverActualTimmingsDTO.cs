using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver_Fumigation
{
    public class DriverActualTimmingsDTO
    {
        public int FumigationRoutsId { get; set; }
        public Nullable<System.DateTime> DriverPickupArrival { get; set; }
        public Nullable<System.DateTime> DriverPickupDeparture { get; set; }
        public Nullable<System.DateTime> DriverDeliveryArrival { get; set; }
        public Nullable<System.DateTime> DriverDeliveryDeparture { get; set; }
        public Nullable<System.DateTime> DepartureDate { get; set; }
        public Nullable<System.DateTime> DriverFumigationIn { get; set; }
        public Nullable<System.DateTime> DriverLoadingStartTime { get; set; }
        public Nullable<System.DateTime> DriverLoadingFinishTime { get; set; }
        public Nullable<System.DateTime> DriverFumigationRelease { get; set; }
        public string ReceiverName { get; set; }

    }
}


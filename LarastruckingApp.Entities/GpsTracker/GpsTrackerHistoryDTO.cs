using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.GpsTracker
{
    public class GpsTrackerHistoryDTO
    {
        public Nullable<int> ShipmentId { get; set; }
        public Nullable<int> ShipmentRouteId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Latitude { get; set; }
        public string longitude { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Event { get; set; }
        public Nullable<System.DateTime> PickDateTime { get; set; }
        public Nullable<System.DateTime> DeliveryDateTime { get; set; }
        public string DefaultDetails { set; get; }
        public Nullable<int> FumigationId { get; set; }
        public Nullable<int> FumigationRoutsId { get; set; }
        public int DriverGPSID { get; set; }  
    }
}

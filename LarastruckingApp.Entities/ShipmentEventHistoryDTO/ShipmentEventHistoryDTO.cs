using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentEventHistoryDTO
{
   public class ShipmentEventHistoryDTO
    {
        public int ID { get; set; }
        public Nullable<int> ShipmentId { get; set; }
        public Nullable<int> ShipmentRouteStopId { get; set; }
        public Nullable<int> ShipmentFreightDetailId { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Event { get; set; }
        public string EventDetail { get; set; }
        public Nullable<System.DateTime> EventDateTime { get; set; }
    }
}

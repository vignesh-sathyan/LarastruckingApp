using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LarastruckingApp.Entities.Driver
{
    public class SaveGpsTrackingHistoryDto
    {
        public int UserId { get; set; }
        public string Latitude { get; set; }
        public string longitude { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> ShipmentId { get; set; }
        public Nullable<int> ShipmentRouteId { get; set; }
        public string Event { get; set; }
    }
}

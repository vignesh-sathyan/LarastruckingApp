using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.QuotesDto
{
    public class GetRouteStopsDTO
    {
        public int? QuoteId { get; set; }
        public int RouteStopId { get; set; }
        public int? RouteNo { get; set; }
        public int? PickupLocationId { get; set; }
        public string PickupLocation { get; set; }
        public int? DeliveryLocationId { get; set; }
        public string DeliveryLocation { get; set; }
        public DateTime? PickDateTime { get; set; }
        public DateTime? DeliveryDateTime { get; set; }
    }
}

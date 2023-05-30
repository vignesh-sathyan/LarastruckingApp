using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.QuotesDto
{
   public class FreightDetailDto
    {
        public int? RouteNo { get; set; }
        public int? PickupLocationId { get; set; }
        public int? DeliveryLocationId { get; set; }
        public string Commodity { get; set; }
        public int? FreightTypeId { get; set; }
        public int? PricingMethodId { get; set; }
        public decimal? MinFee { get; set; }
        public decimal? UpTo { get; set; }
        public decimal? UnitPrice { get; set; }
        public bool Hazardous { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? QutVlmWgt { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? NoOfBox { get; set; }
        public decimal? Weight { get; set; }
        public string Unit { get; set; }
        public int? TrailerCount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.QuoteDto
{
    public class BaseFreightDto
    {
        public int PickupLocationId { get; set; }
        public int DeliveryLocationId { get; set; }
        public int PricingMethodId { get; set; }
        public int FreightTypeId { get; set; }
        public decimal? MinFee { get; set; }
        public decimal? UpTo { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}

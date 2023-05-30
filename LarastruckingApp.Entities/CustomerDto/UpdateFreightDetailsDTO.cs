using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.CustomerDto
{
    public class UpdateFreightDetailsDTO
    {
        public int ShipmentBaseFreightDetailId { get; set; }
        public int? ShipmentRouteStopeId { get; set; }
        public string Commodity { get; set; }
        public int? FreightTypeId { get; set; }
        public int? PricingMethodId { get; set; }
        // public string QuantityNweight { get; set; }
        public Nullable<decimal> QuantityNweight { get; set; }

    }
}

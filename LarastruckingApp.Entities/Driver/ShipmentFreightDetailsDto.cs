using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver
{
    public class ShipmentFreightDetailsDto
    {
        public int ShipmentBaseFreightDetailId { get; set; }
        public int ShipmentRouteStopeId { get; set; }
        public string Commodity { get; set; }
        public string FreightTypeName { get; set; }
        public int? FreightTypeId { get; set; }
        public string PricingMethodName { get; set; }
        public int? PricingMethodId { get; set; }
        public string QuantityNweight { get; set; }
        public string NoOfBox { get; set; }
        public string WeightUnit { get; set; }
        public string TemperatureRequired { get; set; }

       public string Comments { get; set; }
    }
}

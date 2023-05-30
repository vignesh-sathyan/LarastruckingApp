using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver_Fumigation
{
    public class FumigationFreightDetailsDto
    {
        public int RouteNo { get; set; }        
        public int FumigationRoutsId { get; set; }
        public int FumigationId { get; set; }
        public string Commodity { get; set; }
       // public string FreightTypeName { get; set; }
        public string PricingMethodName { get; set; }
        public string QuantityNweight { get; set; }
        public string BoxCount { get; set; }
        public string TrailerPosition { get; set; }
        public string TemperatureRequired { get; set; }

    }
}

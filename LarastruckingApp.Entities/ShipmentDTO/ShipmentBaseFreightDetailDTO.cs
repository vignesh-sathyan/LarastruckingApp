using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
    public class ShipmentBaseFreightDetailDTO
    {
        public int PickupLocationId { get; set; }
        public int DeliveryLocationId { get; set; }
        public string Commodity { get; set; }
        public Nullable<int> FreightTypeId { get; set; }
        public Nullable<int> PricingMethodId { get; set; }
        public Nullable<decimal> MinFee { get; set; }
        public Nullable<decimal> UpTo { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public bool Hazardous { get; set; }
        public Nullable<decimal> Temperature { get; set; }
        public string TemperatureType { get; set; }
        public Nullable<decimal> QutWgtVlm { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public int? NoOfBox { get; set; }
        public decimal? Weight { get; set; }
        public string Unit { get; set; }
        public int? TrailerCount { get; set; }
        public string Comments { get; set; }
        public bool IsPartialShipment { get; set; }
        public int? PartialPallet { get; set; }
        public int? PartialBox { get; set; }
    }
}

using LarastruckingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
    public class GetShipmentFreightDetailDTO : CommonDTO
    {
        public long? CustomerId { get; set; }
        public int? ShipmentRouteId { get; set; }
        public int ShipmentFreightDetailId { get; set; }
        public int? ShipmentId { get; set; }
        public int? RouteNo { get; set; }
        public int? PickupLocationId { get; set; }
        public int? DeliveryLocationId { get; set; }
        public string Commodity { get; set; }
        public Nullable<int> FreightTypeId { get; set; }
        public string FreightType { get; set; }
        public Nullable<int> PricingMethodId { get; set; }
        public string PricingMethod { get; set; }
        public Nullable<decimal> MinFee { get; set; }
        public Nullable<decimal> UpTo { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public bool Hazardous { get; set; }
        public Nullable<decimal> Temperature { get; set; }
        public string TemperatureType { get; set; }
        public Nullable<decimal> QutWgtVlm { get; set; }
        public Nullable<decimal> NonePartialPallet { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }

        public bool IsDeleted { get; set; }

        public string PcsType { get; set; }
        public int? PcsTypeId { get; set; }
        public int? NoOfBox { get; set; }
        public int? NonePartialBox { get; set; }
        public decimal? Weight { get; set; }
        public string WeightWithUnit { get; set; }
        public string Unit { get; set; }
        public int? TrailerCount { get; set; }
        public string Comments { get; set; }
        public bool IsPartialShipment { get; set; }
        public int? PartialPallet { get; set; }
        public int? PartialBox { get; set; }
        public List<ShipmentBaseFreightDetailDTO> ShipmentWeightList { get; set; }
        public string PickupLocation { get; set; }

        public string DeliveryLocation { get; set; }
        public string Equipments { get; set; }
        public string Drivers { get; set; }
        public string LoadingTemp { get; set; }
        public string DeliveryTemp { get; set; }
        public string LoadingDamageDetail { get; set; }
        public string DeliveryDamageDetail { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
    public class ViewShipmentListDTO
    {
        public int ShipmentId { get; set; }
        public string ShipmentRefNo { get; set; }
        public string Status { get; set; }
        public long? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string DriverName { get; set; }
        public string VehicleNo { get; set; }
        public List<DateTime?> PickUpDateList { get; set; }

        public DateTime? PickUpDate { get; set; }
        public string PickupDateTime { get; set; }
        public string PickUpLocation { get; set; }

        public List<DateTime?> DeliveryDateList { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string DeliveryLocation { get; set; }
        public string AirWayBillNo { get; set; }
        public string CustomerPO { get; set; }
        public string EquipmentNo { get; set; }
        public List<string> PickupLocationList { get; set; }
        public List<string> DeliveryLocationList { get; set; }
        public List<ShipmentDriverDTO> Driver { get; set; }
        public List<ShipmentEquipmentDTO> Equipment { get; set; }
        public List<GetShipmentFreightDetailDTO> ShipmentFreightDetail { get; set; }
        public decimal? Quantity { get; set; }
        public List<ShipmentBaseFreightDetailDTO> Weight { get; set; }
        public string QutVolWgt { get; set; }
        public string Weights { get; set; }
        public int? NoOfBox { get; set; }
        public int? TrailerCount { get; set; }

        public int? PartialPallete { get; set; }
        public int? PartilalBox { get; set; }


    }
}

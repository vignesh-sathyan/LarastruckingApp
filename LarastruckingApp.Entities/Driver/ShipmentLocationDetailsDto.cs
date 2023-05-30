using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver
{
    public class ShipmentLocationDetailsDto
    {
        public int ShipmentId { get; set; }
        public int ShippingRoutesId { get; set; }
        public string ShipmentRefNo { get; set; }
        public String AirWayBill { get; set; }
        public String CustomerPO { get; set; }
        public String OrderNo { get; set; }
        public String CustomerRef { get; set; }
        public String ContainerNo { get; set; }
        public string PurchaseDoc { get; set; }
        public string PickUpLocation { get; set; }
        public string DeliveryAddress { get; set; }
        public string PickUpPhone { get; set; }
        public string PickUpExtension { get; set; }
        public DateTime PickUpArrivalDate { get; set; }
        public DateTime DeliveryArrive { get; set; }
        public string DeliveryPhone { get; set; }
        public string DeliveryExtension { get; set; }
        public string DigitalSignature { get; set; }
        public string ReceiverName { get; set; }
        public int DriverId { get; set; }
        public int EquipmentId { get; set; }

        public string EquipmentNo { get; set; }
        public Nullable<long> CustomerId { get; set; }
        // For Shipment Status and Sub stauts for Driver 
        public int? StatusId { get; set; }
        public int? SubStatusId { get; set; }
        public string ShipmentReason { get; set; }
        public bool IsTemperatureRequired { get; set; }

    }
}

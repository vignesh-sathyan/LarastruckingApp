using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.CustomerDto
{
    public class CustomerShipLocationDetailsDto
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

        // For Customer Status and Sub status//////
        public string StatusName { get; set; }
        public string SubStatusName { get; set; }
        public string Reason { get; set; }
        public int? StatusId { get; set; }
        public int? SubStatusId { get; set; }
       
        ///////////////////////////////////////////////
    }
}

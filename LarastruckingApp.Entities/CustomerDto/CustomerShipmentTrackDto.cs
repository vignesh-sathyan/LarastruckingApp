using LarastruckingApp.Entities.ShipmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.CustomerDto
{
    public class CustomerShipmentTrackDto
    {
        public Nullable<int> ShipmentId { get; set; }
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
        public DateTime? PickUpArrivalDate { get; set; }
        public DateTime? DeliveryArrive { get; set; }
        public int CustomerId { get; set; }
        public string StatusName { get; set; }
        public string SubStatusName { get; set; }
        public int StatusId { get; set; }
        public Nullable<int> SubStatusId { get; set; }
        public string Reason { get; set; }
        public System.DateTime CreatedOn { get; set; }
       public List<CustomerStatusTrackDto> CustomerStatusTrack { get; set; }

        public List<ShipmentStatusHistoryDTO> ShipmentStatusHistory { get; set; }
        public List<ShipmentStatusDTO> ShipmentStatusList { get; set; }
        public string StatusGrayDotPath { get; set; }
        public string StatusDotPath { get; set; }
    }
}

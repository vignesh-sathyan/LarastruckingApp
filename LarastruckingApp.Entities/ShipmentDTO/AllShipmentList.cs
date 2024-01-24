using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
    public class AllShipmentList
    {
        public int ShipmentId { get; set; }
        public string StatusName { get; set; }
        public string CustomerName { get; set; }
        //public int CustomerID { get; set; }
        public string PickupLocation { get; set; }
        public string PickupDate { get; set; }
        public string DeliveryLocation { get; set; }
        public string DeliveryDate { get; set; }
        public string AirWayBill { get; set; }
        public string CustomerPO { get; set; }
        public string Driver { get; set; }
        public string Equipment { get; set; }
        public string Quantity { get; set; }
        public int TotalCount { get; set; }
        public string CreatedByName { get; set; }
        public bool IsReady { get; set; }
        public int ApproveStatus { get; set; }
        public string Temperature { get; set; }
        public string Commodity { get; set; }    
        public string Comments { get; set; }    
        public bool WTReady { get; set; }    
        public bool STReady { get; set; }    
    }
}

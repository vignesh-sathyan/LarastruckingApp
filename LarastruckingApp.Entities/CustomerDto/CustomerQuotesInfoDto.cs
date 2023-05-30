using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LarastruckingApp.Entities.CustomerDto
{
    public class CustomerQuotesInfoDto
    {
        public int RNO { get; set; }
        public int Id { get; set; }
        public int RouteId { get; set; }
        public long CustomerId { get; set; }
        public int TotalRecord { get; set; }
        public int UserId { get; set; }
        public string AirWayBill { get; set; }
        public DateTime PickDateTime { get; set; }
        public DateTime DeliveryDateTime { get; set; }
        public int EquipmentId { get; set; }
        //public int ShipmentId { get; set; }
       // public string ShipmentRefNo { get; set; }
        public string StatusName { get; set; }
        public string DriverName { get; set; }
        public string CostumerEquipment { get; set; }

        public string Types { get; set; }

        public DateTime? CreatedOn { get; set; }
        
    }
}


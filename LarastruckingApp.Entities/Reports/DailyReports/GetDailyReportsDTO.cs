using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Reports.DailyReports
{
   public class GetDailyReportsDTO
   {
        public int ShipmentId { get; set; }
        public string CustomerName { get; set; }
        public string PickUpAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public Nullable<System.DateTime> PickUpArrivalDate { get; set; }
        public Nullable<System.DateTime> DeliveryArrive { get; set; }
        public string AirWayBill { get; set; }
        public string CustomerPO { get; set; }
        public string DriverName { get; set; }
        public string DriverEquipment { get; set; }
        public string ShipmentRefNo { get; set; }
        public string QuantityNMethod { get; set; }
        public string StatusName { get; set; }


    }
}

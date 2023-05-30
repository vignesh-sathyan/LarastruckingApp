using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.CustomerFumigation
{
    public class CustomerFumigationRoutesDto
    {
        public int RouteOrder { get; set; }
        public int FumigationId { get; set; }
        public int CustomerId { get; set; }
        public int FumigationRoutsId { get; set; }
        public int PickupLocation { get; set; }
        public string PickupAddress { get; set; }
        public string PickupCity { get; set; }
        public string PickupState { get; set; }
        public string PickupCountry { get; set; }
        public DateTime PickupDateTime { get; set; }
        public int DeliveryLocation { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryCity { get; set; }
        public string DeliveryState { get; set; }
        public string DeliveryCountry { get; set; }
        public DateTime DeliveryDateTime { get; set; }
        public int FumigationSite { get; set; }
        public string FumigationAddress { get; set; }
        public DateTime FumigationDateTime { get; set; }
        public bool IsPickUp { get; set; }
        public string AirWayBill { get; set; }

        public string CustomerPO { get; set; }
        public string QuantityNMethod { set; get; }
        public string DriverName { get; set; }
        public string DriverEquipment { get; set; }

    }
}

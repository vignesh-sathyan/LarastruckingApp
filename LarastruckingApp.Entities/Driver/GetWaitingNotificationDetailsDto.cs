using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver
{
    public class GetWaitingNotificationDetailsDto
    {
        public int WatingNotificationId { get; set; }
        public Nullable<int> ShipmentId { get; set; }
        public Nullable<int> ShipmentRouteId { get; set; }
        //public string ShipmentRefNo { get; set; }
        //public string AirWayBill { get; set; }  
        //public string CustomerPO { get; set; }
        //public string OrderNo { get; set; }
        //public string AWBPoOrderNO { get; set; }
        //public string CustomerRef { get; set; }
        //public string ContainerNo { get; set; }
        //public string PurchaseDoc { get; set; }
        public DateTime PickupArrivedOn { get; set; }
        public DateTime PickupDepartedOn { get; set; }
        public DateTime DeliveryArrivedOn { get; set; }
        public DateTime DeliveryDepartedOn { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string customerEmail { get; set; }
        public Nullable<int> DriverId { get; set; }
        public string EquipmentNo { get; set; }
        public Nullable<int> PickupLocationId { get; set; }
        public string PickupAddress { get; set; }
        public Nullable<System.DateTime> PickDateTime { get; set; }
        public string PickupCity { get; set; }
        public string PickupState { get; set; }
        public string PickupCountry { get; set; }
        public Nullable<int> DeliveryLocationId { get; set; }
        public string DeliveryAddress { get; set; }
        public Nullable<System.DateTime> DeliveryDateTime { get; set; }
        public string DeliveryCity { get; set; }
        public string DeliveryState { get; set; }
        public string DeliveryCountry { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsEmailSentPWS { get; set; }
        public bool IsEmailSentPWE { get; set; }
        public bool IsEmailSentDWS { get; set; }
        public bool IsEmailSentDWE { get; set; }
        public string PickUpDateDifference { get; set; }
        public string DeliveryDateDifference { get; set; }
        public bool IsPickUpWaitingTimeRequired { get; set; }
        public bool IsDeliveryWaitingTimeRequired { get; set; }
        public string StatusName { get; set; }
        //public string Commodity { get; set; }
        //public string FreightTypeName {get;set;}
        //public string PricingMethodName { get; set; }
        //public string TemperatureRequired { get; set; }
        //public string QuantityNweight { get; set; }
        //public int NoOfBox { get; set; }
        //public decimal Weight { get; set; }

        public ShipmentEmailDTO ShipmentEmailDTO { get; set; }


    }
}

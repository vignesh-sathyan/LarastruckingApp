//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LarastruckingApp.Repository.EF
{
    using System;
    
    public partial class usp_GetWaitingTimeDetails_Result
    {
        public int WatingNotificationId { get; set; }
        public Nullable<int> ShipmentId { get; set; }
        public Nullable<int> ShipmentRouteId { get; set; }
        public Nullable<System.DateTime> PickupArrivedOn { get; set; }
        public Nullable<System.DateTime> PickupDepartedOn { get; set; }
        public Nullable<System.DateTime> DeliveryArrivedOn { get; set; }
        public Nullable<System.DateTime> DeliveryDepartedOn { get; set; }
        public Nullable<int> PickUpLocationId { get; set; }
        public Nullable<int> DeliveryLocationId { get; set; }
        public long CustomerId { get; set; }
        public Nullable<int> customerUserId { get; set; }
        public string customerEmail { get; set; }
        public string CustomerName { get; set; }
        public Nullable<int> DriverId { get; set; }
        public string DriverName { get; set; }
        public string PickupAddress { get; set; }
        public Nullable<System.DateTime> PickDateTime { get; set; }
        public string PickupCity { get; set; }
        public string PickupState { get; set; }
        public string PickupCountry { get; set; }
        public string DeliveryAddress { get; set; }
        public Nullable<System.DateTime> DeliveryDateTime { get; set; }
        public string DeliveryCity { get; set; }
        public string DeliveryState { get; set; }
        public string DeliveryCountry { get; set; }
        public string EquipmentNo { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsEmailSentPWS { get; set; }
        public bool IsEmailSentPWE { get; set; }
        public bool IsEmailSentDWS { get; set; }
        public bool IsEmailSentDWE { get; set; }
        public string PickUpDateDifference { get; set; }
        public string DeliveryDateDifference { get; set; }
        public bool IsPickUpWaitingTimeRequired { get; set; }
        public bool IsDeliveryWaitingTimeRequired { get; set; }
    }
}

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
    
    public partial class usp_GetShipment_OrderTaken_List1_Result
    {
        public Nullable<int> TotalCount { get; set; }
        public int ShipmentId { get; set; }
        public string AirWayBill { get; set; }
        public string CustomerPO { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string StatusName { get; set; }
        public Nullable<long> CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Driver { get; set; }
        public string Equipment { get; set; }
        public string PickupDate { get; set; }
        public string DeliveryDate { get; set; }
        public string PickupLocation { get; set; }
        public string DeliveryLocation { get; set; }
        public string Quantity { get; set; }
        public string CreatedByName { get; set; }
        public bool IsReady { get; set; }
    }
}

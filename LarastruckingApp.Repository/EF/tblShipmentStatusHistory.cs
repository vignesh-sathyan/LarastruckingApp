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
    using System.Collections.Generic;
    
    public partial class tblShipmentStatusHistory
    {
        public int ShipmentStatusHistoryId { get; set; }
        public Nullable<int> ShipmentId { get; set; }
        public int StatusId { get; set; }
        public Nullable<int> SubStatusId { get; set; }
        public string Reason { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
    
        public virtual tblShipment tblShipment { get; set; }
        public virtual tblShipmentStatu tblShipmentStatu { get; set; }
        public virtual tblShipmentStatu tblShipmentStatu1 { get; set; }
        public virtual tblUser tblUser { get; set; }
        public virtual tblShipmentSubStatu tblShipmentSubStatu { get; set; }
    }
}

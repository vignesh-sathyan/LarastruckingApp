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
    
    public partial class tblDamagedImage
    {
        public int DamagedID { get; set; }
        public Nullable<int> ShipmentRouteID { get; set; }
        public string ImageName { get; set; }
        public string ImageDescription { get; set; }
        public string ImageUrl { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public bool IsApproved { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
    
        public virtual tblUser tblUser { get; set; }
        public virtual tblShipmentRoutesStop tblShipmentRoutesStop { get; set; }
    }
}

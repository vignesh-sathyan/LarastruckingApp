
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
    
public partial class tblDriverGpsTrakingHistory
{

    public int DriverGpsID { get; set; }

    public Nullable<int> UserId { get; set; }

    public string Latitude { get; set; }

    public string longitude { get; set; }

    public Nullable<System.DateTime> CreatedOn { get; set; }

    public Nullable<int> ShipmentId { get; set; }

    public Nullable<int> ShipmentRouteId { get; set; }

    public string Event { get; set; }

    public Nullable<int> FumigationId { get; set; }

    public Nullable<int> FumigationRoutsId { get; set; }



    public virtual tblFumigation tblFumigation { get; set; }

    public virtual tblFumigationRout tblFumigationRout { get; set; }

    public virtual tblShipment tblShipment { get; set; }

    public virtual tblShipmentRoutesStop tblShipmentRoutesStop { get; set; }

    public virtual tblUser tblUser { get; set; }

}

}

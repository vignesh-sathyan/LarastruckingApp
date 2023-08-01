
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
    
public partial class tblTrailerRentalDetail
{

    public int TrailerRentalDetailId { get; set; }

    public Nullable<int> TrailerRentalId { get; set; }

    public Nullable<int> DeliveryLocationId { get; set; }

    public Nullable<int> PickUpLocationId { get; set; }

    public Nullable<int> DeliveryDriverId { get; set; }

    public Nullable<int> PickupDriverId { get; set; }

    public System.DateTime StartDate { get; set; }

    public System.DateTime EndDate { get; set; }

    public Nullable<System.DateTime> ReturnedDate { get; set; }

    public Nullable<int> NoOfDays { get; set; }

    public Nullable<int> EquipmentId { get; set; }

    public Nullable<decimal> FeePerDay { get; set; }

    public Nullable<decimal> TotalFee { get; set; }

    public System.DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public Nullable<System.DateTime> ModifiedDate { get; set; }

    public Nullable<int> ModifiedBy { get; set; }

    public Nullable<bool> IsDeleted { get; set; }

    public Nullable<int> DeletedBy { get; set; }

    public Nullable<System.DateTime> DeletedDate { get; set; }

    public Nullable<decimal> FixedFee { get; set; }



    public virtual tblAddress tblAddress { get; set; }

    public virtual tblAddress tblAddress1 { get; set; }

    public virtual tblDriver tblDriver { get; set; }

    public virtual tblDriver tblDriver1 { get; set; }

    public virtual tblTrailerRental tblTrailerRental { get; set; }

    public virtual tblUser tblUser { get; set; }

    public virtual tblUser tblUser1 { get; set; }

    public virtual tblUser tblUser2 { get; set; }

}

}


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
    
public partial class tblTimeCard
{

    public int Id { get; set; }

    public int UserId { get; set; }

    public Nullable<int> EquipmentId { get; set; }

    public Nullable<System.DateTime> InDateTime { get; set; }

    public Nullable<System.DateTime> OutDateTime { get; set; }

    public Nullable<int> CreatedBy { get; set; }

    public string Day { get; set; }

    public System.DateTime CreatedOn { get; set; }

}

}


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
    
public partial class tblPageAuthorization
{

    public int Id { get; set; }

    public int RoleId { get; set; }

    public int PageId { get; set; }

    public bool CanView { get; set; }

    public bool CanInsert { get; set; }

    public bool CanUpdate { get; set; }

    public bool CanDelete { get; set; }

    public bool IsPricingMethod { get; set; }



    public virtual tblPage tblPage { get; set; }

    public virtual tblRole tblRole { get; set; }

}

}

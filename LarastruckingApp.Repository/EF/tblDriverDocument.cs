
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
    
public partial class tblDriverDocument
{

    public int DocumentId { get; set; }

    public Nullable<int> DriverId { get; set; }

    public Nullable<int> DocumentTypeId { get; set; }

    public string DocumentName { get; set; }

    public Nullable<System.DateTime> DocumentIssueDate { get; set; }

    public Nullable<System.DateTime> DocumentExpiryDate { get; set; }

    public string ImageURL { get; set; }

    public string ImageName { get; set; }

    public bool IsActive { get; set; }

    public System.DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public Nullable<System.DateTime> ModifiedDate { get; set; }

    public bool IsDeleted { get; set; }

    public Nullable<int> ModifiedBy { get; set; }

    public Nullable<System.DateTime> EmailSentDate { get; set; }



    public virtual tblDocumentType tblDocumentType { get; set; }

    public virtual tblDocumentType tblDocumentType1 { get; set; }

    public virtual tblDriver tblDriver { get; set; }

}

}

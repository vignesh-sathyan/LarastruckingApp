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
    
    public partial class usp_getDriverDetails_Result
    {
        public int DriverID { get; set; }
        public Nullable<int> UserId { get; set; }
        public string DriverName { get; set; }
        public string EmailId { get; set; }
        public int DocumentId { get; set; }
        public string DocumentName { get; set; }
        public Nullable<int> DocumentTypeId { get; set; }
        public Nullable<System.DateTime> DocumentExpiryDate { get; set; }
        public bool ActiveDriver { get; set; }
        public bool ActiveDocument { get; set; }
        public Nullable<System.DateTime> EmailSentDate { get; set; }
    }
}

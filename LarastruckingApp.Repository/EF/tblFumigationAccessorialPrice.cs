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
    
    public partial class tblFumigationAccessorialPrice
    {
        public int FumigationAccessorialPriceId { get; set; }
        public Nullable<int> FumigationId { get; set; }
        public Nullable<int> FumigationRoutesId { get; set; }
        public Nullable<int> AccessorialFeeTypeId { get; set; }
        public Nullable<decimal> Unit { get; set; }
        public Nullable<decimal> AmtPerUnit { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public string Reason { get; set; }
    
        public virtual tblAccessorialFeesType tblAccessorialFeesType { get; set; }
        public virtual tblFumigation tblFumigation { get; set; }
        public virtual tblFumigationRout tblFumigationRout { get; set; }
    }
}

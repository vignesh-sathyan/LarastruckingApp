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
    
    public partial class tblBaseAddresstest
    {
        public int BaseAddressId { get; set; }
        public long CustomerId { get; set; }
        public string BillingAddress1 { get; set; }
        public string BillingAddress2 { get; set; }
        public string BillingCity { get; set; }
        public Nullable<int> BillingStateId { get; set; }
        public Nullable<int> BillingCountryId { get; set; }
        public string BillingZipCode { get; set; }
        public string BillingPhoneNumber { get; set; }
        public string BillingFax { get; set; }
        public string BillingEmail { get; set; }
        public string ShippingAddress1 { get; set; }
        public string ShippingAddress2 { get; set; }
        public string ShippingCity { get; set; }
        public Nullable<int> ShippingStateId { get; set; }
        public Nullable<int> ShippingCountryId { get; set; }
        public string ShippingZipCode { get; set; }
        public string ShippingPhoneNumber { get; set; }
        public string ShippingFax { get; set; }
        public string ShippingEmail { get; set; }
        public string PALAccount { get; set; }
    }
}

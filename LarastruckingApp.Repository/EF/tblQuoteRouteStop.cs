
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
    
public partial class tblQuoteRouteStop
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public tblQuoteRouteStop()
    {

        this.tblAssessorialPrices = new HashSet<tblAssessorialPrice>();

        this.tblAssessorialPrices1 = new HashSet<tblAssessorialPrice>();

        this.tblCustomerBaseFreightDetails = new HashSet<tblCustomerBaseFreightDetail>();

        this.tblCustomerBaseFreightDetails1 = new HashSet<tblCustomerBaseFreightDetail>();

        this.tblQuoteAccessorialPrices = new HashSet<tblQuoteAccessorialPrice>();

    }


    public int QuoteRouteStopsId { get; set; }

    public Nullable<int> QuoteId { get; set; }

    public Nullable<int> RouteNo { get; set; }

    public Nullable<int> PickupLocationId { get; set; }

    public Nullable<int> DeliveryLocationId { get; set; }

    public Nullable<System.DateTime> PickDateTime { get; set; }

    public Nullable<System.DateTime> DeliveryDateTime { get; set; }



    public virtual tblAddress tblAddress { get; set; }

    public virtual tblAddress tblAddress1 { get; set; }

    public virtual tblAddress tblAddress2 { get; set; }

    public virtual tblAddress tblAddress3 { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<tblAssessorialPrice> tblAssessorialPrices { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<tblAssessorialPrice> tblAssessorialPrices1 { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<tblCustomerBaseFreightDetail> tblCustomerBaseFreightDetails { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<tblCustomerBaseFreightDetail> tblCustomerBaseFreightDetails1 { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<tblQuoteAccessorialPrice> tblQuoteAccessorialPrices { get; set; }

    public virtual tblQuote tblQuote { get; set; }

    public virtual tblQuote tblQuote1 { get; set; }

}

}

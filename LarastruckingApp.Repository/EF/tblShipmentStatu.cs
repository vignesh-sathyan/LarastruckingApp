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
    
    public partial class tblShipmentStatu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblShipmentStatu()
        {
            this.tblFumigations = new HashSet<tblFumigation>();
            this.tblFumigationStatusHistories = new HashSet<tblFumigationStatusHistory>();
            this.tblFumigationStatusHistories1 = new HashSet<tblFumigationStatusHistory>();
            this.tblFumigationStatusHistories2 = new HashSet<tblFumigationStatusHistory>();
            this.tblShipmentStatusHistories = new HashSet<tblShipmentStatusHistory>();
            this.tblShipmentSubStatus = new HashSet<tblShipmentSubStatu>();
            this.tblShipmentStatusHistories1 = new HashSet<tblShipmentStatusHistory>();
        }
    
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Colour { get; set; }
        public string ImageURL { get; set; }
        public Nullable<int> FumigationDisplayOrder { get; set; }
        public Nullable<bool> IsFumigation { get; set; }
        public Nullable<bool> IsShipment { get; set; }
        public string StatusAbbreviation { get; set; }
        public string FontColor { get; set; }
        public Nullable<int> DisplayOrderCustomer { get; set; }
        public Nullable<int> FumigationDisplayOrderCustomer { get; set; }
        public Nullable<int> DriverAssignOrder { get; set; }
        public string GrayImageURL { get; set; }
        public string SpanishStatusAbbreviation { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblFumigation> tblFumigations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblFumigationStatusHistory> tblFumigationStatusHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblFumigationStatusHistory> tblFumigationStatusHistories1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblFumigationStatusHistory> tblFumigationStatusHistories2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblShipmentStatusHistory> tblShipmentStatusHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblShipmentSubStatu> tblShipmentSubStatus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblShipmentStatusHistory> tblShipmentStatusHistories1 { get; set; }
    }
}

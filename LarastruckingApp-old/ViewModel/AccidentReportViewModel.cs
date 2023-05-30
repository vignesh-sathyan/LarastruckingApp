using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LarastruckingApp.ViewModel
{
    public class AccidentReportViewModel
    {
        public int AccidentReportId { get; set; }
        public Nullable<int> EquipmentId { get; set; }
        public Nullable<int> DriverId { get; set; }
        public Nullable<DateTime> AccidentDate { get; set; }
        public string Address { get; set; }
        public string AccidentTime { get; set; }
        public string Comments { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }

        public string LicencePlate { get; set; }
        public string VIN { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public int VehicleType { get; set; }
        public string Address1 { get; set; }
        public string EquipmentNo { get; set; }
        public string DriverName { get; set; }
        public string Phone { get; set; }
        public string EmailId { get; set; }
        public string PoliceReportNo { get; set; }
        public List<AccidentReportDocument> AccidentReportDocuments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public int[] RemoveElement { get; set; }
    }


    public class AccidentReportDocument
    {
        public int DocumentId { get; set; }
        public string UploadDocument { get; set; }
        public int AccidentReportId { get; set; }
        public string DocumentName { get; set; }
        public int DocumentTypeId { get; set; }
        public string ImageName { get; set; }
    }
}
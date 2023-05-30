using LarastruckingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities
{
    public class AccidentReportDTO : CommonDTO
    {
        public int AccidentReportId { get; set; }
        public Nullable<int> EquipmentId { get; set; }
        public Nullable<int> DriverId { get; set; }
        public Nullable<System.DateTime> AccidentDate { get; set; }
        public string Address { get; set; }
        public string AccidentTime { get; set; }
        public string Comments { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }


        public string EquipmentNo { get; set; }
        public string LicencePlate { get; set; }
        public string VIN { get; set; }
        public string Model { get; set; }
        public int? Year { get; set; }
        public string VehicleType { get; set; }     
        public string DriverName { get; set; }
        public string PhoneNo { get; set; }
        public string EmailId { get; set; }
        public string PoliceReportNo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<AccidentReportDocumentDTO> AccidentReportDocumentList { get; set; }
    }
}

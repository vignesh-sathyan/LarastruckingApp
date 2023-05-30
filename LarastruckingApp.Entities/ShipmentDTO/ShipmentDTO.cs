using LarastruckingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
    public class ShipmentDTO : CommonDTO
    {
        public int ShipmentId { get; set; }
        public Nullable<int> QuoteId { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<int> SubStatusId { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public string RequestedBy { get; set; }
        public string Reason { get; set; }
        public string ShipmentRefNo { get; set; }
        public string AirWayBill { get; set; }
        public string CustomerPO { get; set; }
        public string OrderNo { get; set; }
        public string CustomerRef { get; set; }
        public string ContainerNo { get; set; }
        public string PurchaseDoc { get; set; }
        public int EquipmentId { get; set; }
        public int DriverId { get; set; }
        public Nullable<decimal> FinalTotalAmount { get; set; }
        public string DriverInstruction { get; set; }
        public string VendorNconsignee { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public List<ShipmentRoutesStopDTO> ShipmentRoutesStop { get; set; }
        public List<ShipmentAccessorialPriceDTO> AccessorialPrice { get; set; }
        public List<ShipmentEquipmentNdriverDTO> ShipmentEquipmentNdriver { get; set; }
        public List<ShipmentBaseFreightDetailDTO> ShipmentFreightDetail { get; set; }
    }
}

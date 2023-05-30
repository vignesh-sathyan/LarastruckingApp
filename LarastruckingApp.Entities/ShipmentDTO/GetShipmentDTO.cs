using LarastruckingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LarastruckingApp.Entities.ShipmentDTO
{
    public class GetShipmentDTO : CommonDTO
    {
        public int ShipmentId { get; set; }
        public Nullable<int> QuoteId { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string StatusName { get; set; }
        public Nullable<int> SubStatusId { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public string CustomerName { get; set; }
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
        public bool IsDeleted { get; set; }
        public bool IsOnHold { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }

        public bool IsMailNeedToSend { get; set; }
        public string VendorNconsignee { get; set; }
        public List<GetShipmentRouteStopDTO> ShipmentRoutesStop { get; set; }
        public List<GetShipmentAccessorialPriceDTO> AccessorialPrice { get; set; }
        public List<GetShipmentEquipmentNDriverDTO> ShipmentEquipmentNdriver { get; set; }
        public List<GetShipmentFreightDetailDTO> ShipmentFreightDetail { get; set; }
        public List<GetDamageImages> DamageImages { get; set; }
        public List<GetProofOfTemprature> ProofOfTemprature { get; set; }
        public int ContactInfoCount { get; set; }
        public bool IsWaitingTimeRequired { get; set; }
        public List<ShipmentCommentDTO> ShipmentComments { get; set; }


    }
}

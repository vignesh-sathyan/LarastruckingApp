using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LarastruckingApp.Entities.ShipmentDTO
{
    public class UploadShipmentDTO
    {
        public int? CustomerId { get; set; }
        public DateTime Date { get; set; }
        public bool IsDateExpired { get; set; }
        public string ETA { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public bool IsDeliveryDateExpired { get; set; }
        public string ConsigneeNVendorName { get; set; }
        public string CustomerPO { get; set; }
        public string OrderNo { get; set; }
        public string AWB { get; set; }
        public string PickUpLocation { get; set; }
        public int PickUpLocationId { get; set; }
        public string DeliveryLocation { get; set; }
        public int DeliveryLocationId { get; set; }
        public string FreightType { get; set; }
        public int FreightTypeId { get; set; }
        public string Commodity { get; set; }
        public string NoOfPallets { get; set; }
        public string PcsType { get; set; }
        public int PcsTypeId { get; set; }
        public decimal NoOfBox { get; set; }
        public decimal Weight { get; set; }
        public string Unit { get; set; }
        public bool IsUnit { get; set; }
        public int? PricingMethodId { get; set; }
        public string PricingMethod { get; set; }
        public string ReqTemp { get; set; }
        public string Comments { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsAirWayBillNoExist { get; set; }
        public string UploadedFileName { get; set; }
        public int PartialBox { get; set; }
        public int PartialPallet { get; set; }
        public string RequestedBy { get; set; }

    }
}
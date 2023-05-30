using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LarastruckingApp.ViewModel.Shipment
{
    public class UploadShipmentViewModel
    {
        public DateTime Date { get; set; }
        public string ETA { get; set; }
        public string ConsigneeNVendorName { get; set; }
        public string CustomerPO { get; set; }
        public string AWB { get; set; }
        public string PickUpLocation { get; set; }
        public string DeliveryLocation { get; set; }
        public string FreightType { get; set; }
        public string NoPcs { get; set; }
        public string PcsType { get; set; }
        public decimal Weight { get; set; }
        public string Unit { get; set; }
        public string PricingMethod { get; set; }
        public string ReqTemp { get; set; }
    }
}
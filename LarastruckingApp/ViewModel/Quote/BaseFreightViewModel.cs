using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LarastruckingApp.ViewModel.Quote
{
    public class BaseFreightViewModel
    {
        public int? PickupLocationId { get; set; }
        public int? DeliveryLocationId { get; set; }
        public int? PricingMethodId { get; set; }
        public int? FreightTypeId { get; set; }
    }
}
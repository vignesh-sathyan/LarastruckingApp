﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver
{
    public class ShipmentRoutesDto
    {
        public int RouteOrder { get; set; }
        public int Id { get; set; }
        public int ShippingId { get; set; }
        public int ShippingRoutesId { get; set; }
        public int PickupLocationId { get; set; }
        public string PickupAddress { get; set; }
        public string PickupCity { get; set; }
        public string PickupState { get; set; }
        public string PickupCountry { get; set; }
        public DateTime PickupDateTime { get; set; }
        public int DeliveryLocationId { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryCity { get; set; }
        public string DeliveryState { get; set; }
        public string DeliveryCountry { get; set; }
        public DateTime DeliveryDateTime { get; set; }
        public int DriverId { get; set; }
        public int EquipmentId { get; set; }
        public string EquipmentNo { get; set; }
        public Nullable<long> CustomerId { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver
{
    public class PreTripShipmentDto
    {
        public long RNO { get; set; }
        public int UserId { get; set; }
        public int TotalRecord { get; set; }
        public int PreTripCheckupId { get; set; }
        public DateTime PickDateTime { get; set; }
        public int EquipmentId { get; set; }
        public int ShipmentId { get; set; }
        public string AirWayBill { get; set; }
        public string QuantityNMethod { get; set; }
        public string StatusName { get; set; }
        public string DriverName { get; set; }
        public string LicencePlate { get; set; }
        public string PreTripStatus { get; set; }
        public string DriverEquipment { get; set; }
        public string Types { get; set; }
        public int Id { get; set; }
        public int TotalCount { get; set; }
        public string FontColor { get; set; }
        public string Colour { get; set; }
        public string CustomerPO { get; set; }
        public string OrderNo { get; set; }
        public string DriverInstruction { get; set; }
        public string PickupLocation { get; set; }
        public string DeliveryLocation { get; set; }
        

    }
}

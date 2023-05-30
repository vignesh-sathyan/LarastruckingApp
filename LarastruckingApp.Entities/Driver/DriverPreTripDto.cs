using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver
{
    public class DriverPreTripDto
    {
        public int UserId { get; set; }
        public int ShipmentId { get; set; }
        public string ShipmentRefNo { get; set; }
        public int EquipmentId { get; set; }
        public string EquipmentNo { get; set; }
        public int PreTripCheckupId { get; set; }
        public bool? IsTiresGood { get; set; }
        public bool? IsBreaksGood { get; set; }
        public string Fuel { get; set; }
        public string LoadStraps { get; set; }
        public string OverAllCondition { get; set; }
    }
}

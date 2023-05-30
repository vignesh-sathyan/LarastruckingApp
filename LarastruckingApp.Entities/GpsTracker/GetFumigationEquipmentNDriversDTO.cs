using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.GpsTracker
{
    public class GetFumigationEquipmentNDriversDTO
    {
        public int FumigationEquipmentNDriverId { get; set; }
        public int? RouteNo { get; set; }
        public int? UserId { get; set; }
        public int? EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public int? DriverId { get; set; }
        public string DriverName { get; set; }
        public string PickupLocation { get; set; }
        public string DeliveryLocation { get; set; }
        public string FumigationLocation { get; set; }
        public bool? IsPickUp { get; set; }
        public bool? IsDeleted { get; set; }
        public string AirWayBillNo { get; set; }
    }
}

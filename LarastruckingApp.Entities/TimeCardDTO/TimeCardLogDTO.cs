using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.TimeCardDTO
{
    public class TimeCardLogDTO
    {
        //public int? DriverId { get; set; }
        public string DriverName { get; set; }
        public int? UserId { get; set; }
        public int? EquipmentId { get; set; }
        public string EquipmentNo { get; set; }
        public string QRCodeNo { get; set; }
        public bool IsCheckIn { get; set; }
        public string InOutStatus { get; set; }
        public DateTime? ScanDateTime { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool IsSuccess { get; set; }
        public string PublicIp { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
    public class ShipmentDriverDTO
    {
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public bool IsTSACertificate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
    public class ShipmentSubStatusDTO
    {
        public int SubStatusId { get; set; }
        public int StatusId { get; set; }
        public string SubStatusName { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string SpanishSubStatusName { get; set; }
    }
}

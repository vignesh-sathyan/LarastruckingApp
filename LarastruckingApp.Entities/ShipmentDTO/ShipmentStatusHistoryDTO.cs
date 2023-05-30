using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
    public class ShipmentStatusHistoryDTO
    {
        public int ShipmentStatusHistoryId { get; set; }
        public int? StatusId { get; set; }
        public Nullable<DateTime> DateTime { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public int? SubStatusId { get; set; }
        public string Colour { get; set; }
        public string ImageUrl { get; set; }
        public string Reason { get; set; }

    }
}

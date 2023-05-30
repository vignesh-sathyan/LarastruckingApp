using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
    public class ViewShipmentDTO
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CustomerId { get; set; }
        public bool IsOrderTaken { get; set; }
        public int FreightTypeId { get; set; }
        public int StatusId { get; set; }
        public string search { get; set; }
        public string sortColumn { get; set; }
        public string sortColumnDir { get; set; }

        public int skip { get; set; }
        public int pageSize { get; set; }
    }
}

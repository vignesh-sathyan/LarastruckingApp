using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
    public class AllShipmentDTO
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CustomerId { get; set; }
        public bool IsOrderTaken { get; set; }
        public int FreightTypeId { get; set; }
        public int StatusId { get; set; }
        public string SearchTerm { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public string DriverName { get; set; }
        public string DriverId { get; set; }
        public string ColumnName{ get; set; }
        public string SortedColumns{ get; set; }

    }
}

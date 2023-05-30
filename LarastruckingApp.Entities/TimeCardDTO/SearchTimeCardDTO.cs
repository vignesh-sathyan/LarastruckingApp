using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.TimeCardDTO
{
    public class SearchTimeCardDTO
    {
        public string SearchTerm { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int? UserId { get; set;}
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}

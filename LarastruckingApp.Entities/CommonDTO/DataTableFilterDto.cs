using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Common
{
    public class DataTableFilterDto
    {
        public string SearchTerm { get; set; }
        public string SortColumn { get; set; }
        public string SortedColumns { get; set; }
        public string SortOrder { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    


    }
}

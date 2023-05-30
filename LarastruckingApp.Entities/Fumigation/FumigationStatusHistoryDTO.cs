using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Fumigation
{
    public class FumigationStatusHistoryDTO
    {
        public int FumigationHistoryId { get; set; }
        public int? StatusId { get; set; }
        public DateTime? DateTime { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public string Colour { get; set; }
        public string ImageUrl { get; set; }
        public string Reason { get; set; }
        public int? SubStatusId { get; set; }
    }
}

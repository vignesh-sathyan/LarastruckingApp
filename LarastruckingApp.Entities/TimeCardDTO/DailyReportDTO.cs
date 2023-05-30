using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.TimeCardDTO
{
    public class DailyReportDTO
    {
        public string UserName { get; set; }
        public DateTime? InDateTime { get; set; }
        public DateTime? OutDateTime { get; set; }
        public string Day { get; set; }
        public TimeSpan InOutDiff { get; set; }
    }

    public class DailyReportDTOList
    {
        public string UserName { get; set; }
        public List<DailyReportDTO> DailyReportDTOs { get; set; }
    }
}

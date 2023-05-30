using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.TimeCardDTO
{
    public class WeeklyReportDTO
    {
        public string Name { get; set; }
        public string TotalHours { get; set; }
        public decimal OverTime { get; set; }
        public decimal LateArrivals { get; set; }
        public decimal Absences { get; set; }
        public decimal TotalPaid { get; set; }
        public int TotalCount { get; set; }
    }
}

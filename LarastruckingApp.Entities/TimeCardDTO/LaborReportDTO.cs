using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.TimeCardDTO
{
    public class LaborReportDTO
    {
        public string Name { get; set; }
        public string TotalHours { get; set; }
        public Nullable<decimal> TotalPaid { get; set; }
        public Nullable<decimal> HourlyRate  { get; set; }
        public Nullable<decimal> Variation { get; set; }
        public Nullable<decimal> Loangranted { get; set; }
        public Nullable<decimal> Loanbalance { get; set; }
    }
}

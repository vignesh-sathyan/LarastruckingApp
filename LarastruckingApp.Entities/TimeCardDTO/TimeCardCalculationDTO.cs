using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.TimeCardDTO
{
    public class TimeCardCalculationDTO
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public System.DateTime WeekStartDay { get; set; }
        public System.DateTime WeekEndDay { get; set; }
        public Nullable<decimal> HourlyRate { get; set; }
        public Nullable<decimal> TotalPay { get; set; }
        public Nullable<decimal> Loan { get; set; }
        public Nullable<decimal> Deduction { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<decimal> Remaining { get; set; }
        public Nullable<decimal> Reimbursement { get; set; }
    }
}

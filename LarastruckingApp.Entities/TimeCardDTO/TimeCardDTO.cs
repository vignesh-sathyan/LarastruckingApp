using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.TimeCardDTO
{
   public class TimeCardDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        //public int DriverId { get; set; }
        public int? EquipmentId { get; set; }
        public Nullable<System.DateTime> InDateTime { get; set; }
        public Nullable<System.DateTime> OutDateTime { get; set; }
        public int CreatedBy { get; set; }
        public string Day { get; set; }
        public bool IsCheckIn { get; set; }
        public bool IsRemoveFlag { get; set; }
        
    }

    public class GetTimeCardCalculationDTO
    {        
        public Nullable<decimal> HourlyRate { get; set; }
        public Nullable<decimal> TotalPay { get; set; }
        public Nullable<decimal> Loan { get; set; }
        public Nullable<decimal> Deduction { get; set; }
        public Nullable<decimal> Reimbursement { get; set; }
        public string Description { get; set; }        
        public List<TimeCardDTO> TimeCardList { get; set; }
        public string UsernName { get; set; }
        public Nullable<decimal> Remaining { get; set; }
    }
}

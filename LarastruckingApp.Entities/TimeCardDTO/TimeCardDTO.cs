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

    public class GetIncentiveCardCalculationDTO
    {
        public Nullable<decimal> HourlyRate { get; set; }
        public Nullable<decimal> TotalPay { get; set; }
        public Nullable<decimal> Loan { get; set; }
        public Nullable<decimal> Deduction { get; set; }
        public Nullable<decimal> DailyRate { get; set; }
        public Nullable<decimal> GrossPay { get; set; }
        public Nullable<decimal> Incentive { get; set; }
        public Nullable<decimal> TotalCheck { get; set; }
        public Nullable<decimal> Reimbursement { get; set; }
        public string Description { get; set; }
        public string Pallets { get; set; }
        public string Boxes { get; set; }
        public string Weight { get; set; }
        public List<TimeCardDTO> TimeCardList { get; set; }
        public string UsernName { get; set; }
        public Nullable<decimal> Remaining { get; set; }
    }

    public class GetIncentiveGridDTO
    {
        
        public string Pallets { get; set; }
        public string Boxes { get; set; }
        public string Weight { get; set; }
        public string ShipmentLocation { get; set; }
        public string FumigationLocation { get; set; }
        public string KG { get; set; }
        public string LBS { get; set; }
        public string GridData { get; set; }
  
    }
    public class SaveIncentiveGrid
    {
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> Tate { get; set; }
        public Nullable<decimal> Total { get; set; }

    }

}

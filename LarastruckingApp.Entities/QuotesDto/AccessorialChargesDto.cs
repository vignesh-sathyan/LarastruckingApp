using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.QuotesDto
{
    public class AccessorialChargesDto
    {

        public int? RouteNo { get; set; }
        public decimal? LoadingPerUnit { get; set; }
        public decimal? LoadingAmount { get; set; }
        public decimal? UnloadingPerUnit { get; set; }
        public decimal? UnloadingAmount { get; set; }
        public decimal? PalletExchangePerUnit { get; set; }
        public decimal? PalletExchangeAmount { get; set; }
        public decimal? DieselRefuelingAmount { get; set; }
        public decimal? OvernightAmount { get; set; }
        public decimal? SameDayAmount { get; set; }
        public decimal? TotalAssessorialFee { get; set; }
        public string Reason { get; set; }
    }
}

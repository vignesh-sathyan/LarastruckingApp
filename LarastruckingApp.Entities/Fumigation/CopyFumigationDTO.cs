using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Fumigation
{
    public class CopyFumigationDTO
    {

        public int FumigationId { get; set; }
        public string AWB { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime? RequestedLoading { get; set; }
        public DateTime? FumigationIn { get; set; }
   
        public DateTime? FumigatiionRelease { get; set; }
        public DateTime? DelEstArrival { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

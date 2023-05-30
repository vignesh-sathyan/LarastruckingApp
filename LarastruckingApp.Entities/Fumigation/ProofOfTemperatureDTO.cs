using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Fumigation
{
    public class ProofOfTemperatureDTO
    {
        public int ProofOfTempratureId { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<DateTime> ApprovedOn { get; set; }
    }
}

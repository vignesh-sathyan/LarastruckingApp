using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities
{
   public class PricingMethodDTO
    {
        public int PricingMethodId { get; set; }
        public string PricingMethodName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}

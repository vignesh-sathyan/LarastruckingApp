using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.QuotesDto
{
   public class SendQuoteAccessorialPriceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PricingMethod { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}

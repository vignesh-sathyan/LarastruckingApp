using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.QuotesDto
{
    public class SearchQuoteDTO
    {
        public System.DateTime? QuoteDate { get; set; }
        public System.DateTime? ValidUptoDate { get; set; }
        public Nullable<long> CustomerId { get; set; }
    }
}

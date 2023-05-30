using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.QuotesDto
{
    public class ViewQuoteDTO
    {
        public int QuoteId { get; set; }
        public string QuotesName { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public string CustomerName { get; set; } 
        public System.DateTime QuoteDate { get; set; }
        public System.DateTime ValidUptoDate { get; set; }
        public Nullable<int> QuoteStatusId { get; set; }
        public string QuoteStatus { get; set; }
        public string CreatedBy { get; set; }
    }
}

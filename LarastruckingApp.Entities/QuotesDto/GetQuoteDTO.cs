using LarastruckingApp.Entities.CustomerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.QuotesDto
{
    public class GetQuoteDTO
    {
        public int QuoteId { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string QuotesName { get; set; }
        public System.DateTime QuoteDate { get; set; }
        public System.DateTime ValidUptoDate { get; set; }
        public Nullable<decimal> FinalTotalAmount { get; set; }
        public Nullable<decimal> FinalTotalAmountWithoutAcc { get; set; }
        public Nullable<int> QuoteStatusId { get; set; }
        public string QuoteStatus { get; set; }
        public string LogoUrl { get; set; }
        public string TermAndCondition { get; set; }
        public List<GetRouteStopsDTO> RouteStops { get; set; }
        public List<GetFreightDetailDTO> ShipmentDetail { get; set; }

        public List<GetQuoteAccessorialPriceDTO> AccessorialPrice { get; set; }
        public List<SendQuoteAccessorialPriceDTO> SendQuoteAccessorialPrice { get; set; }
        public List<CustomerContact> CustomerContact { get; set; }
        public string AllCustomer { get; set; }
        public List<CustomerContact> ContactPersons { get; set; }
        public string AllContactPerson { get; set; }
        public int ContactInfoCount { get; set; }
    }
}

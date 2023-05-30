using LarastruckingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.QuotesDto
{
  public class QuoteDTO : CommonDTO
    {
        public int QuoteId { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string QuotesName { get; set; }
        public System.DateTime QuoteDate { get; set; }
        public System.DateTime ValidUptoDate { get; set; }
        public Nullable<decimal> FinalTotalAmount { get; set; }
        public Nullable<int> QuoteStatusId { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public List<RouteStopDto> RouteStops { get; set; }
        public List<FreightDetailDto> ShipmentDetail { get; set; }
        public List<AccessorialChargesDto> AccessorialPrice { get; set; }
        public List<QuoteAccessorialPriceDTO> QuoteAccessorialPrice { get; set; }
    }
}

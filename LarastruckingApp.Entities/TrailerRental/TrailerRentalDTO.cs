using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.TrailerRental
{
    public class TrailerRentalDTO
    {
        public int TrailerRentalId { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string TrailerInstruction { get; set; }
        public Nullable<decimal> GrandTotal { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public List<TrailerRentalDetailDTO> TrailerRentalDetail { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.TrailerRental
{
   public class TrailerRentalListDTO
    {
        public int TrailerRentalId { get; set; }
        public string CustomerName { get; set; }
        public string Equipment { get; set; }
        public string PickUpDriver { get; set; }
        public string DeliveryDriver { get; set; }
        public string PickUpLocation { get; set; }
        public string DeliveryLocation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}

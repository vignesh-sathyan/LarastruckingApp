using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
  public  class CustomerShipmentDTO
    {
        public int? ShipmentId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ShipmentRefNo { get; set; }
        public string CustomerMail { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public int StatusId { get; set; }
        public int? SubStatusId { get; set; }
    }
}

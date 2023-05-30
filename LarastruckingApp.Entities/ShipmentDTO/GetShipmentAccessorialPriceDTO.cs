using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
  public  class GetShipmentAccessorialPriceDTO
    {
        public int ShipmentAccessorialPriceId { get; set; }
        public Nullable<int> RouteNo { get; set; }
        public Nullable<int> AccessorialFeeTypeId { get; set; }
        public Nullable<decimal> Unit { get; set; }
        public Nullable<decimal> AmtPerUnit { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string FeeType { get; set; }
        public bool IsDeleted { get; set; }
        public string Reason { get; set; }
    }
}

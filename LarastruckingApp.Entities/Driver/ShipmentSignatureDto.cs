using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LarastruckingApp.Entities.Driver
{
    public class ShipmentSignatureDto
    {
        public int? ShippingRoutesId { get; set; }
        public string DigitalSignature { get; set; }

        public string ReceiverName { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver_Fumigation
{
    public class FumigationSignatureDto
    {
        public int? FumigationRoutsId { get; set; }
        public string DigitalSignature { get; set; }

        public string ReceiverName { get; set; }
    }
}

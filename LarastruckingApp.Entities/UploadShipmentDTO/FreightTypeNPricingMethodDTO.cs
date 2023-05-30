using LarastruckingApp.Entities.ShipmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.UploadShipmentDTO
{
    public class FreightTypeNPricingMethodDTO
    {
        public List<GetFreightTypeDTO> FreightType { get; set; }
        public List<GetPricingMethodDTO> PricingMethod { get; set; }
        public List<GetPricingMethodDTO> PcsType { get; set; }
       
        
    }
}

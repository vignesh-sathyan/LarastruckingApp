using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Fumigation
{
   public class GetFumigationDTO
    {
        public int FumigationId { get; set; }
        public float CustomerId { get; set; }
        public int? StatusId { get; set; }
        public int? SubStatusId { get; set; }
        public string Reason { get; set; }
        public string VendorNconsignee { get; set; }
        public string RequestedBy { get; set; }
        public string ShipmentRefNo { get; set; }
        public string Comments { get; set; }
        public string CustomerName { get; set; }
        public string StatusName { get; set; }
        public bool IsOnHold { get; set; }
        public List<GetFumigationRouteDTO> GetFumigationRouteDetail { get; set; }
        public List<GetFumigationAccessorialPriceDTO> GetFumigationAccessorialPrice { get; set; }
        public List<FumigationProofOfTemprature> ProofOfTemprature { get; set; }
        public List<FumigationDamageImages> DamageImages { get; set; }
        public List<GetFumigationEquipmentNDriver> FumigationEquipmentNDriver { get; set; }
        public int ContactInfoCount { get; set; }
        public List<FumigationCommentDTO> FumigationComment { get; set; }
    }
}

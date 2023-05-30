using LarastruckingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Fumigation
{
    public class FumigationDTO : CommonDTO
    {
        public int FumigationId { get; set; }
        public int CustomerId { get; set; }
        public int StatusId { get; set; }
        public int? SubStatusId { get; set; }
        public string Reason { get; set; }
        public string VendorNconsignee { get; set; }
        public string RequestedBy { get; set; }
        public string ShipmentRefNo { get; set; }
        public string Comments { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }

        public List<FumigationRoute> FumigationRouteDetail { get; set; }
        public List<FumigationAccessorialPriceDTO> AccessorialPrice { get; set; }
        public List<FumigationEquipmentNDriver> FumigationEquipmentNdriver { get; set; }
        public bool IsMailNeedToSend { get; set; }
        public string FumigationComment { get; set; }
    }

    //values.FumigationRouteDetail = glbRouteStops;
}

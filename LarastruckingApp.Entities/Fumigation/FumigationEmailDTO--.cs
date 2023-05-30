using LarastruckingApp.Entities.CustomerDto;
using LarastruckingApp.Entities.ShipmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Fumigation
{
    public class FumigationEmailDTO
    {
        public int? FumigationId { get; set; }
        public int CustomerId { get; set; }
        public string Consignee { get; set; }
        public string CustomerName { get; set; }
        public string ShipmentRefNo { get; set; }
        public string CustomerMail { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public int? StatusId { get; set; }
        public int? SubStatusId { get; set; }
        public string AirWayBill { get; set; }
        public string OrderNo { get; set; }
        public string CustomerPO { get; set; }
        public DateTime? OrderTaken { get; set; }
        public DateTime? ESTDateTime { get; set; }
        public string LogoURL { get; set; }
        public string Commodity { get; set; }
        public string FreightType { get; set; }
        public decimal? NoOfBox
        {
            get; set;
        }
        public decimal? Pallet { get; set; }
        public string Weight { get; set; }

        public string ActualTemp { get; set; }
        public string AWBPoOrderNO { get; set; }
        public List<FumigationStatusHistoryDTO> FumigationStatusHistory { get; set; }
        public List<GetFumigationAccessorialPriceDTO> AccessorialPrice { get; set; }
        public List<GetFumigationRouteDTO> FumigationRoute { get; set; }
        public List<FumigationProofOfTemprature> ProofOfTemprature { get; set; }
        public List<FumigationDamageImages> DamageImages { get; set; }
        public List<GetFumigationEquipmentNDriver> FumigationEquipmentNDriver { get; set; }
        public string ProofOfDeliveryURL { get; set; }
        public List<CustomerContact> ContactPersons { get; set; }
        public string AllContactPerson { get; set; }
        public string Trailer { get; set; }
        public List<ShipmentStatusDTO> ShipmentStatusList { get;set;}

        public string StatusGrayDotPath { get; set; }
        public string StatusDotPath { get; set; }
        public string FumigationType { get; set; }
        public string Equipments { get; set; }
        public string Drivers { get; set; }

        public string Tempratures { get; set; }
        public string DamageDetails { get; set; }
    }
}

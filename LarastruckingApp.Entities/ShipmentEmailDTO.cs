using LarastruckingApp.Entities.CustomerDto;
using LarastruckingApp.Entities.ShipmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities
{
    public class ShipmentEmailDTO
    {
        public int? ShipmentId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Consignee { get; set; }
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
        public string StatusGrayDotPath { get; set; }
        public string StatusDotPath { get; set; }
        public List<ShipmentStatusHistoryDTO> ShipmentStatusHistory { get; set; }
        public List<GetShipmentAccessorialPriceDTO> AccessorialPrice { get; set; }
        public List<GetShipmentFreightDetailDTO> ShipmentFreightDetail { get; set; }

        public string Commodity { get; set; }
        public string FreightType { get; set; }
        public decimal? NoOfBox
        {
            get; set;
        }
        public decimal? Pallet { get; set; }
        public string Weight { get; set; }
        public List<GetProofOfTemprature> ProofOfTemprature { get; set; }
        public List<GetDamageImages> DamageImages { get; set; }
        public string ActualTemp { get; set; }
        public string AWBPoOrderNO { get; set; }
        public List<GetShipmentRouteStopDTO> ShipmentRoutesStop { get; set; }
        public string ProofOfDeliveryURL { get; set; }

        public List<CustomerContact> ContactPersons { get; set; }
        public List<ShipmentStatusDTO> ShipmentStatusList { get; set; }


        public string AllContactPerson { get; set; }
        public List<GetShipmentEquipmentNDriverDTO> ShipmentEquipmentNdriver { get; set; }

        public string Equipments { get; set; }
        public string Drivers { get; set; }
        public string LoadingTemp { get; set; }
        public string DeliveryTemp { get; set; }
        public string LoadingDamageDetail { get; set; }
        public string DeliveryDamageDetail { get; set; }
    }

    public class TemperatureEmailSipmentDTO
    {
        public int? ShipmentId { get; set; }
        public int? ShipmentRoutsId { get; set; }
        public DateTime? PickUpArrival { get; set; }
        public DateTime? PickUpDeparture { get; set; }
       public DateTime? DeliveryArrival { get; set; }
       public DateTime? DeliveryDeparture { get; set; }
        //public DateTime? DriverFumigationIn { get; set; }
        public string AirWayBill { get; set; }
        public string OrderNo { get; set; }
        public string CustomerPO { get; set; }
        public string LogoURL { get; set; }
        public string ActTemp { get; set; }
        public double? ActTemperature { get; set; }

    }
}

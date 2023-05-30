using LarastruckingApp.Entities.CustomerDto;
using LarastruckingApp.Entities.Fumigation;
using LarastruckingApp.Entities.ShipmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.CustomerFumigation
{
    public class CustomerFumigationTrackDto
    {
        public int FumigationId { get; set; }
        public int FumigationRoutsId { get; set; }
        public string ShipmentRefNo { get; set; }
        public string AirWayBill { get; set; }
        public string CustomerPO { get; set; }
        public string ContainerNo { get; set; }
        public string PickUpLocation { get; set; }
        public string DeliveryAddress { get; set; }
        public string FumigationAddress { get; set; }
        public Nullable<System.DateTime> PickUpArrivalDate { get; set; }
        public Nullable<System.DateTime> FumigationDateTime { get; set; }
        public Nullable<System.DateTime> FumigationDepartureDateTime { get; set; }
        public Nullable<System.DateTime> DeliveryArrive { get; set; }
        public int CustomerId { get; set; }
        public string StatusName { get; set; }
        public string SubStatusName { get; set; }
        public int StatusId { get; set; }
        public Nullable<int> SubStatusId { get; set; }
        public string Reason { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public List<CustomerStatusTrackDto> CustomerStatusTrack { get; set; }
        public List<FumigationStatusHistoryDTO> FumigationStatusHistory { get; set; }
        public List<ShipmentStatusDTO> ShipmentStatusList { get; set; }
        public string StatusGrayDotPath { get; set; }
        public string StatusDotPath { get; set; }

    }
}

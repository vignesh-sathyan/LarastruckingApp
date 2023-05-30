using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.CustomerFumigation
{
    public  class CustomerFumigationLocationDetailsDto
    {
        public int FumigationId { get; set; }
        public int FumigationRoutsId { get; set; }
        public string ShipmentRefNo { get; set; }
        public string AirWayBill { get; set; }
        public string CustomerPO { get; set; }
        public string ContainerNo { get; set; }
        public string PickUpLocation { get; set; }
        public string FumigationAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public string PickUpPhone { get; set; }
        public string PickUpExtension { get; set; }
        public DateTime PickUpArrivalDate { get; set; }
        public DateTime DeliveryArrive { get; set; }
        public string DeliveryPhone { get; set; }
        public string DeliveryExtension { get; set; }
        public string FumigationPhone { get; set; }
        public string FumigationExtension { get; set; }
        public string DigitalSignature { get; set; }
        public string ReceiverName { get; set; }

        // For Customer Status and Sub status//////
        public string StatusName { get; set; }
        public string SubStatusName { get; set; }
        public string Reason { get; set; }
        public int? StatusId { get; set; }
        public int? SubStatusId { get; set; }

        ///////////////////////////////////////////////
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver_Fumigation
{
    public class FumigationLocationDetailsDTO
    {
        public int FumigationId { get; set; }
        public int FumigationRoutsId { get; set; }
        public string ShipmentRefNo { get; set; }
        public String AirWayBill { get; set; }
        public String CustomerPO { get; set; }
      //  public String OrderNo { get; set; }
      //  public String CustomerRef { get; set; }
        public String ContainerNo { get; set; }
      //  public string PurchaseDoc { get; set; }
        public string PickUpLocation { get; set; }
        public string DeliveryAddress { get; set; }
        public string PickUpPhone { get; set; }
        public string PickUpExtension { get; set; }
        public DateTime PickUpArrivalDate { get; set; }
        public DateTime DeliveryArrive { get; set; }
        public string DeliveryPhone { get; set; }
        public string DeliveryExtension { get; set; }

        public string FumigationAddress { get; set; }
        public string FumigationPhone { get; set; }
        public string FumigationExtension { get; set; }
        public string DigitalSignature { get; set; }
        public string ReceiverName { get; set; }

        // For Shipment Status and Sub stauts for Driver 
        public int? StatusId { get; set; }
        public int? SubStatusId { get; set; }
        public string ShipmentReason { get; set; }
        public string FumigationReason { get; set; }
        ///////////////////////////////////////////////
    }
}

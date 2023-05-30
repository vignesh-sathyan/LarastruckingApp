using LarastruckingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LarastruckingApp.Entities.Driver
{
    public class PreTripAddShipmentDetailDto : CommonDTO
    {
        // For PickUp And Delivery 
        public Nullable<int> ShipmentFreightDetailId { get; set; }
        public Nullable<int> ShipmentId { get; set; }
        public Nullable<int> ShipmentRouteId { get; set; }
        public Nullable<System.DateTime> DriverPickupArrival { get; set; }
        public Nullable<System.DateTime> DriverPickupDeparture { get; set; }
        public Nullable<System.DateTime> DriverDeliveryArrival { get; set; }
        public Nullable<System.DateTime> DriverDeliveryDeparture { get; set; }
        
        // For Maintaning Temperature 
        public string ActualTemperature { get; set; }
        public bool IsLoading { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        //
        // For Maintaning Status and Sub status
        public int StatusId { get; set; }
        public Nullable<int> SubStatusId { get; set; }
        public string Reason { get; set; }
        public string FileName { get; set; }

        // For Proof of Temp
        public int ImageType { get; set; }
        public string ImageName { get; set; }
        public string ImageDescription { get; set; }
        public string ImageUrl { get; set; }
      
        //
        // For Upload Proof and Damaged Files
        public HttpPostedFileBase UploadedTemperatureProofFiles { get; set; }
        public List<HttpPostedFileBase> DamagedFiles { get; set; }
        public PreTripShipmentImagesDto UploadedProofOfTempFile { get; set; }
        public List<ShipmentDamagedImagesDto> UploadedDamagedFile { get; set; }
        //

        // For Signature 
        public HttpPostedFileBase SignatureDocument { get; set; }
        public string ReceiverName { get; set; }
        public string DigitalSignature { get; set; }
        public string DigitalSignaturePath { get; set; }
        //
        public string ShipmentComment { get; set; }

        public string AWBPOORD { get; set; }
    }
}

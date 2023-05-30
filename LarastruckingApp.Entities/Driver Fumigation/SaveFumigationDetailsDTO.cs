using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LarastruckingApp.Entities.Driver_Fumigation
{
   public class SaveFumigationDetailsDTO
    {
        // For PickUp And Delivery 
        public Nullable<int> ShipmentFreightDetailId { get; set; }
        public Nullable<int> FumigationId { get; set; }
        public Nullable<int> FumigationRoutsId { get; set; }
        public Nullable<System.DateTime> DriverPickupArrival { get; set; }
        public Nullable<System.DateTime> DriverPickupDeparture { get; set; }
        public Nullable<System.DateTime> DriverDeliveryArrival { get; set; }
        public Nullable<System.DateTime> DriverDeliveryDeparture { get; set; }
        public Nullable<System.DateTime> DepartureDate { get; set; }

        // For Maintaning Temperature 
        public string ActualTemperature { get; set; }
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
        public FumigationProofImagesDto UploadedProofOfTempFile { get; set; }
        public List<FumigationDamagedImagesDto> UploadedDamagedFile { get; set; }
        //

        // For Signature 
        public HttpPostedFileBase SignatureDocument { get; set; }
        public string ReceiverName { get; set; }
        public string DigitalSignature { get; set; }
        //

        public Nullable<System.DateTime> DriverFumigationIn { get; set; }
        public Nullable<System.DateTime> DriverLoadingStartTime { get; set; }
        public Nullable<System.DateTime> DriverLoadingFinishTime { get; set; }
        public Nullable<System.DateTime> DriverFumigationRelease { get; set; }
        public string DeliveryTemp { get; set; }
        public string DigitalSignaturePath { get; set; }
        public string FumigationComment { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LarastruckingApp.Entities.Driver_Fumigation
{
    public class SaveWebCameraDTO
    {
        public Nullable<int> FumigationId { get; set; }
        public Nullable<int> FumigationRoutsId { get; set; }
        public int ImageType { get; set; }
        public string ImageName { get; set; }
        public string ImageDescription { get; set; }
        public string ImageUrl { get; set; }
        public string ActualTemperature { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public bool IsWebCam { get; set; }
        public HttpPostedFileBase UploadedTemperatureProofFiles { get; set; }
        public FumigationProofImagesDto UploadedProofOfTempFile { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LarastruckingApp.Entities.ShipmentDTO
{
    public class GetProofOfTemprature
    {
        public int ProofOfTempratureId { get; set; }
        public int? FreightDetailId { get; set; }
        public int ShipmentRouteStopId { get; set; }
        public int ShipmentId { get; set; }
        public int? RouteNo { get; set; }
        public string ImageName { get; set; }
        public string ImageDescription { get; set; }
        public string ImageUrl { get; set; }
        public string ActualTemperature { get; set; }
        public bool IsLoading { get; set; }
        public HttpPostedFileBase ProofOfTemprature { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public bool IsApproved { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<DateTime> ApprovedOn { get; set; }
        public string Ext { get; set; }
        public Array filesObj { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LarastruckingApp.Entities.Fumigation
{
 public  class FumigationProofOfTemprature
    {
        public int ProofOfTempratureId { get; set; }

        public int FumigationRouteId { get; set; }
        public int FumigationId { get; set; }
        public int? RouteNo { get; set; }
        public string ImageName { get; set; }
        public string ImageDescription { get; set; }
        public string ImageUrl { get; set; }
        public string ActualTemperature { get; set; }
        public HttpPostedFileBase ProofOfTemprature { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public bool IsApproved { get; set; }
        public string DeliveryTemp { get; set; }
        public bool IsLoading { get; set; }
        public string Ext { get; set; }
        public Array filesObj { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver
{
    public class PreTripShipmentImagesDto
    {
        public Nullable<int> ShipmentFreightDetailId { get; set; }
        public Nullable<int> ShipmentRouteId { get; set; }
        public int ImageType { get; set; }
        public string ImageName { get; set; }
        public string ImageDescription { get; set; }
        public string ImageUrl { get; set; }
        public string ActualTemperature { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        //public bool IsApproved { get; set; }
        //public Nullable<int> ApprovedBy { get; set; }
        //public Nullable<DateTime> ApprovedOn { get; set; }
    }
}

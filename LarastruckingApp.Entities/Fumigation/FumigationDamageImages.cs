using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LarastruckingApp.Entities.Fumigation
{
    public class FumigationDamageImages
    {
        public int DamageId { get; set; }
        public int FumigationRouteId { get; set; }
        public int? RouteNo { get; set; }
        public string ImageName { get; set; }
        public string ImageDescription { get; set; }
        public string ImageUrl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<HttpPostedFileBase> DamageImage { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public bool IsApproved { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<DateTime> ApprovedOn { get; set; }
        public string Ext { get; set; }
    }
}

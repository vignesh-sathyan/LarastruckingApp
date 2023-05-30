using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver_Fumigation
{
   public class FumigationDamagedEditBindDto
    {
        public int DamagedID { get; set; }
        public int FumigationRouteId { get; set; }
        public String DamagedImage { get; set; }
        public string DamagedDescription { get; set; }
        public System.DateTime DamagedDate { get; set; }
        public string ImageUrl { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public bool IsApproved { get; set; }
    }
}

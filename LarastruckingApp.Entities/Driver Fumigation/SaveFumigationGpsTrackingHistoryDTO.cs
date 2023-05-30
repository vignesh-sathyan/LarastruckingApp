using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver_Fumigation
{
   public class SaveFumigationGpsTrackingHistoryDTO
    {
        public int UserId { get; set; }
        public string Latitude { get; set; }
        public string longitude { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> FumigationId { get; set; }
        public Nullable<int> FumigationRoutsId { get; set; }
        public string Event { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.CustomerDto
{
    public class CustomerStatusTrackDto
    {
        public int? ShipmentId { get; set; }
        public int? FumigationId { get; set; }
        public string StatusName { get; set; }
        public string SubStatusName { get; set; }
        public string Reason { get; set; }
        public int? StatusId { get; set; }
        public int? SubStatusId { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        
    }
}

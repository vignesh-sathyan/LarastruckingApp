using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.ShipmentDTO
{
    public class ShipmentStatusDTO
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string StatusAbbreviation { get; set; }
        public int DisplayOrder { get; set; }
        public int? FumigationDisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string GrayImageURL { get; set; }
        public string SpanishStatusAbbreviation { get; set; }
    }
}

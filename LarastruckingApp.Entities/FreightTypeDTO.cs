using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities
{
    public class FreightTypeDTO
    {
        public int FreightTypeId { get; set; }
        public string FreightTypeName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}

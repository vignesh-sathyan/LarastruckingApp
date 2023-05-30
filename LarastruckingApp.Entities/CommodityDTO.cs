using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities
{
   public class CommodityDTO
    {
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

    }
}

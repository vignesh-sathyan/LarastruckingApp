using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Fumigation
{
  public  class GetFumigationEquipmentNDriver
    {
        public int FumigationEquipmentNDriverId { get; set; }
        public int? FumigationRoutsId { get; set; }
        public int? RouteNo { get; set; }
        public int? EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public int? DriverId { get; set; }
        public string DriverName { get; set; }
        public bool? IsPickUp { get; set; }
        public bool? IsDeleted { get; set; }
       
    }
}

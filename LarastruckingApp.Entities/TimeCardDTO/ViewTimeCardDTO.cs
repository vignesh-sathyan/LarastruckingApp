using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.TimeCardDTO
{
    public class ViewTimeCardDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string EquipmentNo { get; set; }
        public DateTime? InDateTime { get; set; }
        public DateTime? OutDateTime { get; set; }
        public string TotalHours { get; set; }
        public string Day{ get; set; }
        public int TotalCount { get; set; }
        
    }
}

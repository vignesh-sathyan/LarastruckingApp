using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.MainPage
{
    public class Main
    {
        [Key]
        public string AirWayBill { get; set; }
        public string CustomerPO { get; set; }
        public string ContainerNo { get; set; }
    }
}

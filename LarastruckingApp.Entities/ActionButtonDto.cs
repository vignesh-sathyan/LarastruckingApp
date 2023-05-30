using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp
{
    public class ActionButtonDto
    {
        public int? DisplayOrder { get; set; }
        public string PageName { get; set; }
        public string AreaName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool IsMenu { get; set; }
        public string DisplayIcon { get; set; }
        public bool CanInsert { get; set; }
        public bool CanView { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public bool IsPricingMethod { get; set; }
    }
}

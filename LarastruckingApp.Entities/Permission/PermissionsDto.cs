using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Permission
{
  public  class PermissionsDto
    {
        public bool IsInsert { get; set; }
        public bool IsView { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsPricingMethod { get; set; }
    }
}

using LarastruckingApp.Entities.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DTO
{
    public abstract class CommonDTO
    {
        public bool IsSuccess { get; set; }
        public string Response { get; set; }

    }
}

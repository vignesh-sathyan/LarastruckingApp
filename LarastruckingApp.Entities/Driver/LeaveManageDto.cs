using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver
{
    public class LeaveManageDto
    {
        public LeaveManageDto()
        {
            DriverLeave = new DriverLeaveDto();
        }
        public DriverLeaveDto DriverLeave { get; set; }
        public IList<DriverLeaveDto> Leaves { get; set; }
        public IList<LeaveStatusDto> LeaveStatus { get; set; }
    }
}

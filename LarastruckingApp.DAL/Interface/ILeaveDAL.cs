using LarastruckingApp.Entities.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL.Interface
{
    public interface ILeaveDAL : ICommonDAL<DriverLeaveDto>
    {
        #region Leave History
        IList<DriverLeaveDto> GetLeaveHistory(int driverId);
        #endregion

        #region Leave Status
        IList<LeaveStatusDto> GetLeaveStatus();
        #endregion


        #region Leave details
        LeaveManageDto GetLeaveDetails(int driverId);
        #endregion

        #region Cancel Leave
        bool CancelLeave(int leaveId);
        #endregion

    }
}

using LarastruckingApp.Entities.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer.Interface
{
    public interface ILeaveBAL : ICommonBAL<DriverLeaveDto>
    {
        #region Leave History
        IList<DriverLeaveDto> GetLeaveHistory(int driverId);
        #endregion

        #region Leave Status
        IList<LeaveStatusDto> GetLeaveStatus();
        #endregion

        #region Leave Details
        LeaveManageDto GetLeaveDetails(int driverId);
        #endregion

        #region Send Leave Mail
        string SendLeavMail(LeaveManageDto model, string appliedby = null);
        #endregion

        #region Cancel Leave
        bool CancelLeave(int leaveId);
        #endregion

    }
}

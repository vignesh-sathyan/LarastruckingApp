using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Reports.DailyReports;
using LarastruckingApp.Entities.ShipmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.IRepository
{
    public interface IDailyReportRepository
    {
        #region Get Daily Report by All Status
        /// <summary>
        ///  Get Daily Report by All Status
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        IList<GetDailyReportsDTO> GetDailyReport(GetAllDetailsDailyReportDTO dto, int userId);
        #endregion

        #region Report status list
        /// <summary>
        /// get status list
        /// </summary>
        /// <returns></returns>
        List<ShipmentStatusDTO> GetStatusList();
        #endregion
    }
}

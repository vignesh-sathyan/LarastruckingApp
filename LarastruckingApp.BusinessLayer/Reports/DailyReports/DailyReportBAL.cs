using LarastruckingApp.DAL.Reports.DailyReports;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Reports.DailyReports;
using LarastruckingApp.Entities.ShipmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer.Reports.DailyReports
{
    public class DailyReportBAL : IDailyReportBAL
    {
        #region Private Member
        /// <summary>
        /// defining Private members
        /// </summary>
        private readonly IDailyReportDAL IDailyReportDAL;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor Injection
        /// </summary>
        /// <param name="iDailyReportDAL"></param>
        public DailyReportBAL(IDailyReportDAL iDailyReportDAL)
        {
            this.IDailyReportDAL = iDailyReportDAL;
        }

        #endregion

        #region Get Daily Report by All Status
        /// <summary>
        /// Get Daily Report by All Status
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<GetDailyReportsDTO> GetDailyReport(GetAllDetailsDailyReportDTO dto, int userId)
        {
            return IDailyReportDAL.GetDailyReport(dto, userId);
        }
        #endregion

        #region Daily Report status

        /// <summary>
        /// get status list
        /// </summary>
        /// <returns></returns>
        public List<ShipmentStatusDTO> GetStatusList()
        {
            return IDailyReportDAL.GetStatusList();
        }
        #endregion
    }
}

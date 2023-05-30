using LarastruckingApp.DAL.Reports.DailyReports;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Reports.DailyReports;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL
{
    public class DailyReportDAL : IDailyReportDAL
    {
        #region Private Memeber
        /// <summary>
        /// defining Private members
        /// </summary>
        private readonly IDailyReportRepository IDailyReportRepository;
        #endregion

        #region constructor
        /// <summary>
        /// constructor injection
        /// </summary>
        /// <param name="iDailyReportRepository"></param>

        public DailyReportDAL (IDailyReportRepository iDailyReportRepository)
        {
            this.IDailyReportRepository = iDailyReportRepository;
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
            return IDailyReportRepository.GetDailyReport(dto, userId);
        }
        #endregion

        #region Report status list
        /// <summary>
        /// get status list
        /// </summary>
        /// <returns></returns>
        public List<ShipmentStatusDTO> GetStatusList()
        {
            return IDailyReportRepository.GetStatusList();
        }
        #endregion
    }
}

using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Reports.DailyReports;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.Repository.Reports
{
    public class DailyReportRepository : IDailyReportRepository
    {
        #region Private Members
        /// <summary>
        /// defining Private members
        /// </summary>
        private readonly LarastruckingDBEntities ReportContext;
        private readonly ExecuteSQLStoredProceduce _dbContext;
        #endregion

        #region Contructor
        /// <summary>
        /// Constructor Injection
        /// </summary>
        /// <param name="IGpsTrackingDAL"></param>

        public DailyReportRepository(ExecuteSQLStoredProceduce dbContext)
        {

            ReportContext = new LarastruckingDBEntities();
            this._dbContext = dbContext;
        }
        #endregion
        
        #region Get Daily Report by All Status
        /// <summary>
        /// Get Daily Report by All Status
        /// </summary>
        /// <returns></returns>

        public IList<GetDailyReportsDTO> GetDailyReport(GetAllDetailsDailyReportDTO dto, int userId)
        {
            try
            {
                var totalCount = new SqlParameter
                {
                    ParameterName = "@TotalCount",
                    Value = 0,
                    Direction = ParameterDirection.Output
                };
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@UserId", userId),
                        new SqlParameter("@statusId", dto.StatusId),
                        //new SqlParameter("@CustomerId", dto.CustomerId),
                        //new SqlParameter("@FreightTypeId", dto.FreightTypeId),
                        new SqlParameter("@SearchTerm", dto.SearchTerm),
                        new SqlParameter("@SortColumn", dto.SortColumn),
                        new SqlParameter("@SortOrder", dto.SortOrder),
                        new SqlParameter("@PageNumber", dto.PageNumber),
                        new SqlParameter("@PageSize", dto.PageSize),
                        totalCount
                    };
                var result = _dbContext.ExecuteStoredProcedure<GetDailyReportsDTO>("usp_DailyReportDashboard", sqlParameters);
                dto.TotalCount = Convert.ToInt32(totalCount.Value);
                return result != null && result.Count > 0 ? result : new List<GetDailyReportsDTO>();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Daily Report status list
        /// <summary>
        /// get status list
        /// </summary>
        /// <returns></returns>
        public List<ShipmentStatusDTO> GetStatusList()
        {
            try
            {

                var statuslist = ReportContext.tblShipmentStatus.Where(x => x.IsActive && x.IsDeleted == false).OrderBy(x => x.DisplayOrder);
                return AutoMapperServices<tblShipmentStatu, ShipmentStatusDTO>.ReturnObjectList(statuslist.ToList());
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion
    }
}

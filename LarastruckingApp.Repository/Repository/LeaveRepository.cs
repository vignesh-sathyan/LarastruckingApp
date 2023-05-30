using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Driver;
using LarastruckingApp.Repository.EF;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Repository.Repository
{
    public class LeaveRepository : ILeaveRepository
    {
        #region Private Members
        /// <summary>
        /// defining Private members
        /// </summary>
        private readonly ExecuteSQLStoredProceduce dbCtx;
        #endregion

        #region Constructor
        /// <summary>
        /// Cosntructor
        /// </summary>
        /// <param name="dbContext"></param>
        public LeaveRepository(ExecuteSQLStoredProceduce dbContext)
        {
            dbCtx = dbContext;
        }
        #endregion

        #region List
        /// <summary>
        /// List
        /// </summary>
        public IEnumerable<DriverLeaveDto> List => throw new NotImplementedException();
        #endregion

        #region Leave Add
        /// <summary>
        /// Method to add driver leave
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DriverLeaveDto Add(DriverLeaveDto entity)
        {
            try
            {


                entity.AppliedOn = Configurations.TodayDateTime;
                entity.TakenFrom = Configurations.ConvertLocalToUTC(entity.TakenFrom);
                entity.TakenTo = Configurations.ConvertLocalToUTC(entity.TakenTo);

                var isFromDateExists = dbCtx.tblLeaves.Any(
                    l => l.UserId == entity.UserId &&
                     l.LeaveStatusId != 4 &&
                    (l.TakenFrom <= entity.TakenFrom && l.TakenTo >= entity.TakenFrom));

                if (isFromDateExists)
                {
                    entity.IsSuccess = false;
                    entity.Response = $"Already applied leave for the selected from date";
                    return entity;
                }

                var isToDateExists = dbCtx.tblLeaves.Any(
                            l => l.UserId == entity.UserId &&
                             l.LeaveStatusId != 4 &&
                            (l.TakenFrom <= entity.TakenTo && l.TakenTo >= entity.TakenTo));

                if (isToDateExists)
                {
                    entity.IsSuccess = false;
                    entity.Response = $"Already applied leave for the selected to date";
                    return entity;
                }

                var addLeave = new tblLeave()
                {
                    UserId = entity.UserId,
                    TakenFrom = entity.TakenFrom,
                    TakenTo = entity.TakenTo,
                    Reason = entity.Reason,
                    AppliedBy = entity.AppliedBy,
                    AppliedOn = entity.AppliedOn,
                    LeaveStatusId = entity.LeaveStatusId,
                    ModifiedBy = entity.AppliedBy,
                    ModifiedOn = Configurations.TodayDateTime
                };

                dbCtx.tblLeaves.Add(addLeave);
                entity.IsSuccess = dbCtx.SaveChanges() > 0 ? true : false;
                entity.Response = "Leave applied successfully";

                return entity;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Delete Leave
        /// <summary>
        /// Method to  delete driver leave
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(DriverLeaveDto entity)
        {
            try
            {

                bool isDeleted = false;
                var dbExist = dbCtx.tblLeaves.Find(entity.LeaveId);
                if (dbExist != null)
                {
                    dbCtx.tblLeaves.Remove(dbExist);
                    isDeleted = dbCtx.SaveChanges() > 0 ? true : false;
                }
                return isDeleted;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Leave: Find By Id
        /// <summary>
        /// Method to  find driver leave by leave id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DriverLeaveDto FindById(int Id)
        {
            try
            {


                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 2),
                        new SqlParameter("@LeaveId", Id),
                    };

                var result = dbCtx.ExecuteStoredProcedure<DriverLeaveDto>("usp_LeaveManage", sqlParameters);
                var leavedetail = result.Count > 0 ? result[0] : null;

                if (leavedetail != null)
                {
                    leavedetail.TakenFrom = Configurations.ConvertDateTime(leavedetail.TakenFrom);
                    leavedetail.TakenTo = Configurations.ConvertDateTime(leavedetail.TakenTo);
                    leavedetail.TodayDate = Configurations.ConvertDateTime(leavedetail.TodayDate);
                }

                return leavedetail;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        #region Leave: FindById Leave History
        /// <summary>
        /// Method to  get driver leave history driver leave
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns></returns>
        public IList<DriverLeaveDto> GetLeaveHistory(int driverId)
        {
            try
            {


                List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@SpType", 1),
                        new SqlParameter("@UserId", driverId),
                    };

                var result = dbCtx.ExecuteStoredProcedure<DriverLeaveDto>("usp_LeaveManage", sqlParameters);
                result = result.Count > 0 ? result : null;

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        item.TakenFrom = Configurations.ConvertDateTime(item.TakenFrom);
                        item.TakenTo = Configurations.ConvertDateTime(item.TakenTo);
                        item.TodayDate = Configurations.ConvertDateTime(item.TodayDate);
                    }
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Update Leave
        /// <summary>
        /// Method to update driver leave
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DriverLeaveDto Update(DriverLeaveDto entity)
        {
            try
            {


                entity.AppliedOn = Configurations.TodayDateTime;
                entity.TakenFrom = Configurations.ConvertLocalToUTC(entity.TakenFrom);
                entity.TakenTo = Configurations.ConvertLocalToUTC(entity.TakenTo);

                var isFromDateExists = dbCtx.tblLeaves.Any(
                                        l => l.UserId == entity.UserId &&
                                        l.LeaveId != entity.LeaveId &&
                                         l.LeaveStatusId != 4 &&
                                        (l.TakenFrom <= entity.TakenFrom && l.TakenTo >= entity.TakenFrom));

                if (isFromDateExists)
                {
                    entity.IsSuccess = false;
                    entity.Response = $"Already applied leave for the selected from date";
                    return entity;
                }

                var isToDateExists = dbCtx.tblLeaves.Any(
                            l => l.UserId == entity.UserId &&
                            l.LeaveId != entity.LeaveId &&
                             l.LeaveStatusId != 4 &&
                            (l.TakenFrom <= entity.TakenTo && l.TakenTo >= entity.TakenTo));

                if (isToDateExists)
                {
                    entity.IsSuccess = false;
                    entity.Response = $"Already applied leave for the selected to date";
                    return entity;
                }

                var dbExist = dbCtx.tblLeaves.Find(entity.LeaveId);

                if (dbExist != null)
                {
                    dbExist.TakenFrom = entity.TakenFrom;
                    dbExist.TakenTo = entity.TakenTo;
                    dbExist.Reason = entity.Reason;
                    dbExist.LeaveStatusId = entity.LeaveStatusId;
                    dbExist.ModifiedBy = entity.AppliedBy;
                    dbExist.ModifiedOn = Configurations.TodayDateTime;

                    entity.IsSuccess = dbCtx.SaveChanges() > 0 ? true : false;
                }
                if (entity.IsSuccess)
                {
                    entity.Response = "Leave updated successfully";
                }
                else
                {
                    entity.Response = "Unable to process your request";
                }
                return entity;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Leave Status
        /// <summary>
        /// Method to get leave status driver leave
        /// </summary>
        /// <returns></returns>
        public IList<LeaveStatusDto> GetLeaveStatus()
        {
            try
            {

                var status = dbCtx.tblLeaveStatus
                                        .Select(s => new LeaveStatusDto()
                                        {
                                            LeaveStatusId = s.LeaveStatusId,
                                            LeaveStatus = s.LeaveStatus
                                        });
                return status.ToList();

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Leave details
        /// <summary>
        /// Method to get leave details driver leave
        /// </summary>
        /// <returns></returns>
        public LeaveManageDto GetLeaveDetails(int driverId)
        {
            try
            {


                LeaveManageDto dto = new LeaveManageDto();
                var leaveDetails = dbCtx.tblLeaves.Where(s => s.UserId == driverId).FirstOrDefault();
                if (leaveDetails != null)
                {
                    dto.DriverLeave.TakenFrom = Configurations.ConvertDateTime(leaveDetails.TakenFrom);
                    dto.DriverLeave.TakenTo = Configurations.ConvertDateTime(leaveDetails.TakenTo);
                    dto.DriverLeave.Reason = leaveDetails.Reason;
                }

                return dto;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Cancel Leave
        public bool CancelLeave(int leaveId)
        {
            try
            {


                var dbExists = dbCtx.tblLeaves.Find(leaveId);

                if (dbExists != null)
                {
                    dbExists.LeaveStatusId = 4; // Update leave status to 'cancelled' 
                    return dbCtx.SaveChanges() > 0 ? true : false;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

    }
}

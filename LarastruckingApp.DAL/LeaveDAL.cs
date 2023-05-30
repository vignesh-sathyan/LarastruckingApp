using LarastruckingApp.DAL.Interface;
using LarastruckingApp.Entities.Driver;
using LarastruckingApp.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.DAL
{
    public class LeaveDAL : ILeaveDAL
    {
        #region Private Members
        private readonly ILeaveRepository leaveRepo;
        #endregion

        #region Constructor
        public LeaveDAL(ILeaveRepository repo)
        {
            leaveRepo = repo;
        }
        #endregion

        #region List
        public IEnumerable<DriverLeaveDto> List => throw new NotImplementedException();
        #endregion

        #region Leave Add
        public DriverLeaveDto Add(DriverLeaveDto entity)
        {
           return leaveRepo.Add(entity);
        }
        #endregion

        #region Delete Leave
        public bool Delete(DriverLeaveDto entity)
        {
            return leaveRepo.Delete(entity);
        }
        #endregion

        #region Leave: Find By Id
        public DriverLeaveDto FindById(int Id)
        {
            return leaveRepo.FindById(Id);
        }

        #endregion

        #region Leave: Get Leave History
        public IList<DriverLeaveDto> GetLeaveHistory(int driverId)
        {
            return leaveRepo.GetLeaveHistory(driverId);
        }
        #endregion

        #region Update Leave
        public DriverLeaveDto Update(DriverLeaveDto entity)
        {
            return leaveRepo.Update(entity);
        }

        #endregion

        #region Leave Status
        public IList<LeaveStatusDto> GetLeaveStatus()
        {
            return leaveRepo.GetLeaveStatus();
        }
        #endregion

        #region Leave Details
        public LeaveManageDto GetLeaveDetails(int driverId)
        {
            return leaveRepo.GetLeaveDetails(driverId);
        }
        #endregion

        #region Cancel Leave
        public bool CancelLeave(int leaveId)
        {
            return leaveRepo.CancelLeave(leaveId);
        }
        #endregion


    }
}

using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.DAL.Interface;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Driver;
using LarastruckingApp.Entities.Enum;
using LarastruckingApp.Log.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.BusinessLayer
{
    public class LeaveBAL : ILeaveBAL
    {
        #region Private Members
        /// <summary>
        /// Private Members
        /// </summary>
        private readonly ILeaveDAL leaveRepo;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repo"></param>
        public LeaveBAL(ILeaveDAL repo)
        {
            leaveRepo = repo;
        }
        #endregion

        #region List
        /// <summary>
        /// this method is used to get List
        /// </summary>
        public IEnumerable<DriverLeaveDto> List => throw new NotImplementedException();
        #endregion

        #region Leave Add
        /// <summary>
        /// this method is used to add leave
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DriverLeaveDto Add(DriverLeaveDto entity)
        {
            return leaveRepo.Add(entity);
        }
        #endregion

        #region Delete Leave
        /// <summary>
        /// this method is used to delete List
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(DriverLeaveDto entity)
        {
            return leaveRepo.Delete(entity);
        }
        #endregion

        #region Leave: Find By Id
        /// <summary>
        /// this method is used to get leave by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DriverLeaveDto FindById(int Id)
        {
            return leaveRepo.FindById(Id);
        }

        #endregion

        #region Leave: Get Leave History
        /// <summary>
        /// this method is used to get leave history
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns></returns>
        public IList<DriverLeaveDto> GetLeaveHistory(int driverId)
        {
            return leaveRepo.GetLeaveHistory(driverId);
        }
        #endregion

        #region Update Leave
        /// <summary>
        /// this method is used to update leave
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DriverLeaveDto Update(DriverLeaveDto entity)
        {
            return leaveRepo.Update(entity);
        }

        #endregion

        #region Leave Status
        /// <summary>
        /// this method is used to get leave status
        /// </summary>
        /// <returns></returns>
        public IList<LeaveStatusDto> GetLeaveStatus()
        {
            return leaveRepo.GetLeaveStatus();
        }
        #endregion

        #region Leave Details
        /// <summary>
        /// this method is used to get leave Details
        /// </summary>
        /// <returns></returns>
        public LeaveManageDto GetLeaveDetails(int driverId)
        {
            return leaveRepo.GetLeaveDetails(driverId);
        }
        #endregion

        #region send confirmation mail
        public string SendLeavMail(LeaveManageDto model, string appliedby = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table width='100%' border='1' cellspacing='0' cellpadding='2'>");
            sb.Append("<tr><td><b>Name</b></td><td>" + model.DriverLeave.FirstName + " " + model.DriverLeave.LastName + "</td></tr>");
            sb.Append("<tr><td><b>Leave placed date</b></td><td>" + DateTime.Now + "</td></tr>");
            sb.Append("<tr><td><b>Leave start date</b></td><td>" + model.DriverLeave.TakenFrom + "</td></tr>");
            sb.Append("<tr><td><b>Leave end date</b></td><td>" + model.DriverLeave.TakenTo + "</td></tr>");
            sb.Append("<tr><td><b>Leave placed by</b></td><td>" + appliedby + "</td></tr>");
            sb.Append("<tr><td><b>Current leave status</b></td><td>" + Enum.GetName(typeof(LeaveStatus), model.DriverLeave.LeaveStatusId) + "</td></tr>");
            sb.Append("<tr><td><b>Leave reason</b></td><td>" + model.DriverLeave.Reason + "</td></tr>");
            sb.Append("</table>");
            sb.Append("<br /><br />  Best Regards,<br/> Lara’s Trucking & Logistics");
            string sentMessage = string.Empty;
            string mailsubject = "leave for " + model.DriverLeave.FirstName + " " + model.DriverLeave.LastName;


            MailWithCCAttachDTO mailData = new MailWithCCAttachDTO();
            mailData.MailPurpose = "Driver";
            mailData.ToMail = model.DriverLeave.Email;
            mailData.ToMailCC = string.Empty;
            mailData.ToMailBCC = string.Empty;
            mailData.MailSubject = mailsubject;
            mailData.MailBody = sb.ToString();
            mailData.strMailtype = string.Empty;
            mailData.CreatedBy = model.DriverLeave.UserId;
            mailData.CreatedOn = Configurations.TodayDateTime;

            Email.SendMailWithCCAttach(mailData, out sentMessage);
            return sentMessage;
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

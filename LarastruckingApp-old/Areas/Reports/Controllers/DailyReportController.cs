using LarastruckingApp.BusinessLayer.Reports.DailyReports;
using LarastruckingApp.Controllers;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Reports.DailyReports;
using LarastruckingApp.Entities.ShipmentDTO;
using LarastruckingApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LarastruckingApp.Areas.Reports.Controllers
{
    [Authorize]
    public class DailyReportController : BaseController
    {
        #region Private Member
        /// <summary>
        ///  Defining Private Member
        /// </summary>
        private readonly IDailyReportBAL IDailyReportBAL;
        private MemberProfile memberProfile;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iDailyReportBAL"></param>
        public DailyReportController(IDailyReportBAL iDailyReportBAL)
        {
            IDailyReportBAL = iDailyReportBAL;
            memberProfile = new MemberProfile();
        }
        #endregion

        #region section : Action Result  

        #region Get Report List 
        /// <summary>
        /// Get Report List 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region view Report List 
        [HttpPost]
        public ActionResult GetDailyReport(GetAllDetailsDailyReportDTO dto)
        {
            try
            {
               // string search = Request.Form.GetValues("search[value]").FirstOrDefault();
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
//var start = Request.Form.GetValues("start").FirstOrDefault();
//var length = Request.Form.GetValues("length").FirstOrDefault();

                // Find Order Column
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
               // int pageSize = length != null ? Convert.ToInt32(length) : 0;
//int skip = start != null ? Convert.ToInt32(start) : 0;
//int recordsTotal = 0;

                
               
                var preTripInfo = IDailyReportBAL.GetDailyReport(dto, memberProfile.UserId);


                if (preTripInfo.Count > 0)
                {
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        preTripInfo = sortColumnDir == "asc" ? preTripInfo.OrderBy(x => x.GetType().GetProperty(sortColumn).GetValue(x, null)).ToList()
                            : preTripInfo.OrderByDescending(x => x.GetType().GetProperty(sortColumn).GetValue(x, null)).ToList();
                    }
                }

                return Json(new { draw = draw, recordsFiltered = dto.TotalCount, recordsTotal = dto.TotalCount, data = preTripInfo }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Report Status
        /// <summary>
        /// Customer status
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetReportStatus()
        {
            try
            {
                var statusList = IDailyReportBAL.GetStatusList();
                return Json(statusList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #endregion
    }
}
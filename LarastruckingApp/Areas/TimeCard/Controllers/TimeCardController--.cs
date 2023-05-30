using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.Controllers;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.TimeCardDTO;
using LarastruckingApp.Infrastructure;
using LarastruckingApp.Log.Utility;
using LarastruckingApp.Resource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LarastruckingApp.Areas.TimeCard.Controllers
{
    [Authorize]
    public class TimeCardController : BaseController
    {

        #region Private Member
        /// <summary>
        /// Defining private members
        /// </summary>
        private readonly ITimeCardBAL timeCardBAL = null;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iTimeCardBAL"></param>
        public TimeCardController(ITimeCardBAL iTimeCardBAL)
        {
            timeCardBAL = iTimeCardBAL;
        }
        #endregion

        // GET: TimeCard/TimeCard
        public ActionResult Index()
        {
            return View();
        }

        #region Driver Time Card
        /// <summary>
        /// Driver Time Card
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize]
        public ActionResult DriverTimeCard()
        {
            return View();
        }
        #endregion

        #region DriverTimeCard
        /// <summary>
        /// Driver Time Card
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DriverTimeCard(TimeCardLogDTO entity)
        {
            MemberProfile objMemberProfile = new MemberProfile();
            try
            {
                entity.UserId = objMemberProfile.UserId;
                var result = timeCardBAL.MarkDriverTimeCard(entity);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion



        #region User Time Card
        /// <summary>
        /// User Time Card
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserTimeCard(TimeCardLogDTO entity)
        {
            MemberProfile objMemberProfile = new MemberProfile();
            try
            {
                MyIp ip = new MyIp();
                entity.UserId = objMemberProfile.UserId;
                var result = timeCardBAL.MarkUserTimeCard(entity);
                ip.Result = result;
                ip.PublicIp = entity.PublicIp;
                ip.LaraIp = Configurations.LaraIp;
                return Json(ip, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        public class MyIp
        {
            public int Result { get; set; }
            public string LaraIp { get; set; }
            public string PublicIp { get; set; }
        }

        #region Get User List
        /// <summary>
        /// Get User List
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserList(string searchText)
        {
            try
            {
                var result = timeCardBAL.GetUserList(searchText);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetEquipmentList
        /// <summary>
        /// Get Equipment List
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEquipmentList()
        {
            try
            {
                var result = timeCardBAL.GetEquipmentList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region View Time Card
        /// <summary>
        /// View Time Card
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewTimeCard()
        {
            return View();
        }
        #endregion

        #region Get TimeCard List
        /// <summary>
        ///  Get TimeCar dList
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ActionResult GetTimeCardList(SearchTimeCardDTO entity)
        {
            try
            {
                string search = Request.Form.GetValues("search[value]").FirstOrDefault();
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();

                // Find Order Column
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                entity.PageSize = pageSize;
                entity.PageNumber = skip;
                entity.SearchTerm = search;
                entity.SortColumn = sortColumn;
                entity.SortOrder = sortColumnDir;
                entity.TotalCount = recordsTotal;

                var data = timeCardBAL.GetTimeCardList(entity);
                recordsTotal = data.Count > 0 ? data.Select(x => x.TotalCount).FirstOrDefault() : 0;
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region Save Time Card data by dispetcher
        /// <summary>
        /// Save Time Card data by dispetcher
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ActionResult DispatcherTimeCard(TimeCardDTO entity)
        {
            try
            {

                var date = DateTime.Now;
                MemberProfile mp = new MemberProfile();
                entity.CreatedBy = mp.UserId;

                var result = timeCardBAL.DispatcherTimeCard(entity);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Save Time Card data by dispetcher
        /// <summary>
        /// Save Time Card data by dispetcher
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTimeCardData(TimeCardDTO entity)
        {
            try
            {
                var timeCardData = timeCardBAL.GetTimeCardData(entity);
                return Json(timeCardData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Get Driver Time Card Detail
        /// <summary>
        /// Get Driver TimeCard Detail
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDriverTimeCardDetail()
        {
            try
            {
                MemberProfile memberProfile = new MemberProfile();
                TimeCardDTO entity = new TimeCardDTO();
                entity.UserId = memberProfile.UserId;
                var timeCardData = timeCardBAL.GetDriverTimeCardDetail(entity);
                return Json(timeCardData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Save Time Card Amount
        /// <summary>
        /// Save Time Card Amount
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult SaveTimeCardAmount(TimeCardCalculationDTO entity)
        {
            MemberProfile mp = new MemberProfile();
            var result = timeCardBAL.SaveTimeCardAmount(entity);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region my time card
        /// <summary>
        /// my time card
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize]
        public ActionResult MyTimeCard()
        {
            return View();
        }
        #endregion

        #region Get Week Date
        /// <summary>
        /// Get Week Date
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWeekDates()
        {
            try
            {
                var result = timeCardBAL.GetWeekDates();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region  Labor Report
        /// <summary>
        /// Labor Report
        /// </summary>
        /// <param name="modal"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetLaborReport(SearchTimeCardDTO entity)
        {
            try
            {
                string search = Request.Form.GetValues("search[value]").FirstOrDefault();
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();

                // Find Order Column
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                entity.PageSize = pageSize;
                entity.PageNumber = skip;
                entity.SearchTerm = search;
                entity.SortColumn = sortColumn;
                entity.SortOrder = sortColumnDir;
                entity.TotalCount = recordsTotal;

                var result = timeCardBAL.GetLaborReport(entity);
                return Json(new { recordsFiltered = result.Count(), recordsTotal = result.Count(), data = result }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Weekly Report
        /// <summary>
        /// Weekly Report
        /// </summary>
        /// <param name="modal"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetWeeklyReport(SearchTimeCardDTO entity)
        {
            try
            {
                string search = Request.Form.GetValues("search[value]").FirstOrDefault();
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();

                // Find Order Column
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                entity.PageSize = pageSize;
                entity.PageNumber = skip;
                entity.SearchTerm = search;
                entity.SortColumn = sortColumn;
                entity.SortOrder = sortColumnDir;
                entity.TotalCount = recordsTotal;
                var result = timeCardBAL.GetWeeklyReport(entity);
                //return Json(new { recordsFiltered = result.Count(), recordsTotal = result.Count(), data = result }, JsonRequestBehavior.AllowGet);
                recordsTotal = result.Count > 0 ? result.Select(x => x.TotalCount).FirstOrDefault() : 0;
                return Json(new { recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = result }, JsonRequestBehavior.AllowGet);
                //  return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region  Labor Report
        /// <summary>
        /// Labor Report
        /// 
        /// </summary>
        /// <param name="modal"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDailyReport(SearchTimeCardDTO modal)
        {
            try
            {
                var result = timeCardBAL.GetDailyReport(modal);
                return PartialView("_DailyReport", result);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        public string GetIPAddress()
        {
            String address = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                address = stream.ReadToEnd();
            }
            int first = address.IndexOf("Address: ") + 9;
            int last = address.LastIndexOf("</body>");
            address = address.Substring(first, last - first);
            return address;
        }

        /// <summary>
        /// Send Time Card Email
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendTimeCard(TimeCardEmail model)
        {
            SendTimeCardEmail(model);
            return Json(true, JsonRequestBehavior.AllowGet);

        }
        #region send attachment
        /// <summary>
        /// Send attachment to client 
        /// </summary>
        /// <param name="model"></param>

        public string SendTimeCardEmail(TimeCardEmail model)
        {
            StringReader sr = null;
            Document pdfDoc = null;
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                var strview = model.Body; //this.RenderViewToStringAsync<GetQuoteDTO>("_prQuotePreview", model);
                sr = new StringReader(strview);
                pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                using (MemoryStream stream = new System.IO.MemoryStream())
                {

                   PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                   pdfDoc.Open();
                   XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                   pdfDoc.Close();
                    byte[] bytes = stream.ToArray();
                    string to = model.To;
                    string subject = model.Subject;
                    string body = "Hi"+" "+model.User+",</br></br>" + model.Description;
                    string bodywithsignature = body + LarastruckingResource.QuoteMailSignature;
                    string fileName = model.User+"_"+"TimeCard.pdf";
                    Dictionary<byte[], string> fileAttach = new Dictionary<byte[], string>();
                    fileAttach.Add(bytes, fileName);
                    string mailSentResponse = string.Empty;
                    MailWithCCAttachDTO mailData = new MailWithCCAttachDTO();
                    mailData.FileByte = bytes;
                    mailData.MailPurpose = "TimeCard";
                    mailData.ToMail = to;
                   
                    mailData.ToMailCC = string.Empty;
                    mailData.ToMailBCC = string.Empty;
                    mailData.MailSubject = subject;
                    mailData.MailBody = bodywithsignature;
                    mailData.strMailtype = string.Empty;
                    mailData.CreatedBy = objMemberProfile.UserId;
                    mailData.CreatedOn = Configurations.TodayDateTime;
                    Email.SendMailWithCCAttach(mailData, out mailSentResponse, fileAttach);
                    return mailSentResponse;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                //pdfDoc.Close();
                sr.Dispose();
            }
        }
        #endregion

        public class TimeCardEmail
        {
            public string To { get; set; }
            public string Subject { get; set; }
            public string Description { get; set; }
            public string User { get; set; }
            public string Body { get; set; }
        }
    }
}
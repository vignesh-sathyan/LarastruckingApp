using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using LarastruckingApp.BusinessLayer;
using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.Controllers;
using LarastruckingApp.Entities;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Entities.Enum;
using LarastruckingApp.Entities.QuoteDto;
using LarastruckingApp.Entities.QuotesDto;
using LarastruckingApp.Infrastructure;
using LarastruckingApp.Log.Utility;
using LarastruckingApp.Resource;
using LarastruckingApp.ViewModel;
using LarastruckingApp.ViewModel.Quote;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace LarastruckingApp.Areas.Quote.Controllers
{
    [Authorize]
    public class QuoteController : BaseController
    {
        #region Private Member
        private readonly IShipmentBAL shipmentBAL = null;
        private readonly IQuoteBAL quoteBAL;
        private readonly IAddressBAL addressBAL;
        private readonly IDriverBAL driverBAL;
        #endregion

        #region Contructor
        public QuoteController(IQuoteBAL iQuoteBAL, IAddressBAL iAddressBAL, IDriverBAL iDriverBAL, IShipmentBAL iShipmentBAL)
        {
            shipmentBAL = iShipmentBAL;
            quoteBAL = iQuoteBAL;
            addressBAL = iAddressBAL;
            driverBAL = iDriverBAL;

        }
        #endregion

        #region Index: Get
        /// <summary>
        /// Default method for page load
        /// </summary>
        /// <returns></returns>
        // GET: Quote/Quote
        [Authorize]
        [CustomAuthorize]
        public ActionResult Index(int id = 0)
        {
            IEnumerable<SelectListItem> QuoteStatus = from QuoteStatus qt in Enum.GetValues(typeof(QuoteStatus))
                                                      select new SelectListItem
                                                      {
                                                          Text = qt.ToString(),
                                                          Value = Convert.ToInt32(qt).ToString()

                                                      };
            ViewBag.QuoteStatus = new SelectList(QuoteStatus, "Value", "Text");

            IEnumerable<SelectListItem> WeightUnit = from Unit qt in Enum.GetValues(typeof(Unit))
                                                     select new SelectListItem
                                                     {
                                                         Text = qt.ToString(),
                                                         Value = Convert.ToInt32(qt).ToString()

                                                     };
            ViewBag.WeightUnit = new SelectList(WeightUnit, "Value", "Text");
            ViewBag.Country = driverBAL.GetCountryList();
            ViewBag.State = new SelectList(driverBAL.GetStateList(), "ID", "Name");
            ViewBag.AddressType = BindAddressType();

            return View();
        }
        #endregion

        #region Create Quote
        /// <summary>
        /// method for save Quote detail into database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateQuote(QuoteDTO model)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();
                QuoteDTO objQuoteDTO = model;
                objQuoteDTO.CreatedBy = objMemberProfile.UserId;
                if (model != null)
                {
                    var result = quoteBAL.Add(objQuoteDTO);
                    if (result.IsSuccess)
                    {
                        objJsonResponse.IsSuccess = true;
                        objJsonResponse.Message = LarastruckingResource.DataSaveSuccessfully;
                        return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                    }
                    else if (result.IsSuccess == false && result.Response == LarastruckingResource.Exists)
                    {
                        objJsonResponse.IsSuccess = false;
                        objJsonResponse.Message = LarastruckingResource.CustomerExists;
                        return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        objJsonResponse.IsSuccess = false;
                        objJsonResponse.Message = LarastruckingResource.SomethingWentWrong;
                        return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    objJsonResponse.IsSuccess = false;
                    objJsonResponse.Message = LarastruckingResource.SomethingWentWrong;
                    return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;

            }
        }
        #endregion

        #region Get Address Type
        /// <summary>
        /// method for bind address type
        /// </summary>
        /// <returns></returns>
        public List<AddressTypeViewModel> BindAddressType()
        {
            List<AddressTypeViewModel> lstAddressTypeViewModel = null;
            try
            {
                lstAddressTypeViewModel = AutoMapperServices<AddressTypeDTO, AddressTypeViewModel>.ReturnObjectList(addressBAL.BindAddressType().ToList());
            }
            catch (Exception)
            {
                throw;
            }
            return lstAddressTypeViewModel;
        }

        #endregion

        #region get commodity list
        /// <summary>
        /// get commodity list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCommodityList()
        {
            try
            {
                var commoditylist = quoteBAL.GetCommodityList();
                return Json(commoditylist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;

            }
        }
        #endregion

        #region get freight type
        /// <summary>
        /// get freight type
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFreightType()
        {
            try
            {
                var freighttypelist = quoteBAL.GetFreightTypeList();
                return Json(freighttypelist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;

            }
        }
        #endregion

        #region get pricing method
        /// <summary>
        /// get pricing method
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPricingMehtod()
        {
            try
            {
                var pricingmethodlist = quoteBAL.GetPricingMethodList();
                return Json(pricingmethodlist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;

            }
        }
        #endregion

        #region get base freight detail
        /// <summary>
        /// method for get base freight detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetBaseFreightDetail(BaseFreightViewModel model)
        {
            try
            {
                var basefreightdeail = AutoMapperServices<BaseFreightViewModel, BaseFreightDto>.ReturnObject(model);
                var baseFreightDeatail = quoteBAL.GetBaseFreightDetail(basefreightdeail);
                return Json(baseFreightDeatail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;

            }
        }
        #endregion

        #region  view quote
        /// <summary>
        /// default method for load view Quote
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [CustomAuthorize]
        public ActionResult ViewQuote()
        {
            return View();
        }
        #endregion

        #region LoadData
        /// <summary>
        /// acction method for bind quote table
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadData(SearchQuoteDTO model)
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
                IEnumerable<ViewQuoteDTO> lstQuoteDTO = quoteBAL.GetQuoteDetail(model);

                var quoteDetailList = lstQuoteDTO;
                if (!string.IsNullOrEmpty(search))
                {
                    quoteDetailList = quoteDetailList.Where(x => x.CustomerName.ToUpper().Contains(search.ToUpper()) || x.QuotesName.ToUpper().Contains(search.ToUpper()) || x.QuoteStatus.ToUpper().Contains(search.ToUpper()) || x.CreatedBy.ToUpper().Contains(search.ToUpper())).ToList();
                }
                recordsTotal = quoteDetailList.Count();
                var data = quoteDetailList.ToList();
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    data = sortColumnDir == "asc" ? data.OrderBy(x => x.GetType().GetProperty(sortColumn).GetValue(x, null)).ToList() : data.OrderByDescending(x => x.GetType().GetProperty(sortColumn).GetValue(x, null)).ToList();
                    data = data.Skip(skip).Take(pageSize).ToList();
                }
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region DeleteQuote
        /// <summary>
        /// Action method for delete quote detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteQuote(int id = 0)
        {
            try
            {
                QuoteDTO objQuoteDTO = new QuoteDTO();
                objQuoteDTO.QuoteId = id;
                JsonResponse objJsonResponse = new JsonResponse();

                if (quoteBAL.Delete(objQuoteDTO))
                {
                    objJsonResponse.IsSuccess = true;
                    objJsonResponse.Message = LarastruckingResource.DataDeleteSuccessfully;
                    return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    objJsonResponse.IsSuccess = false;
                    objJsonResponse.Message = LarastruckingResource.ErrorOccured;
                    return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetQuoteById
        /// <summary>
        /// get quote detail by id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetQuoteById(int quoteId)
        {
            var result = quoteBAL.GetQuoteById(quoteId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region:Edit Quote
        /// <summary>
        /// Edit Quote detail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult EditQuote(QuoteDTO model)
        {
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                JsonResponse objJsonResponse = new JsonResponse();
                QuoteDTO objQuoteDTO = model;// AutoMapperServices<QuoteViewModel, QuoteDTO>.ReturnObject(model);
                objQuoteDTO.CreatedBy = objMemberProfile.UserId;
                if (model != null)
                {
                    var result = quoteBAL.Update(objQuoteDTO);
                    if (result.IsSuccess)
                    {
                        objJsonResponse.IsSuccess = true;
                        objJsonResponse.Message = LarastruckingResource.DataUpdateSuccessfully;
                        return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        objJsonResponse.IsSuccess = false;
                        objJsonResponse.Message = LarastruckingResource.SomethingWentWrong;
                        return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    objJsonResponse.IsSuccess = false;
                    objJsonResponse.Message = LarastruckingResource.SomethingWentWrong;
                    return Json(objJsonResponse, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;

            }
        }
        #endregion

        #region: Quote Preview
        /// <summary>
        /// Get Quote Preview
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        [HttpPost]
        public ActionResult QuotePreview(GetQuoteDTO model)
        {
            try
            {
                var data = PartialView("_prQuotePreview", model);
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region send attachment
        /// <summary>
        /// Send attachment to client 
        /// </summary>
        /// <param name="model"></param>

        public string SendAttachmentToClient(GetQuoteDTO model)
        {
            StringReader sr = null;
            Document pdfDoc = null;
            try
            {
                MemberProfile objMemberProfile = new MemberProfile();
                var strview = this.RenderViewToStringAsync<GetQuoteDTO>("_prQuotePreview", model);
                sr = new StringReader(strview);
                pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                using (MemoryStream stream = new System.IO.MemoryStream())
                {

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    byte[] bytes = stream.ToArray();
                    string to = model.Email;
                    string subject = LarastruckingResource.LarasQuoteAdvice;
                    subject = subject.Replace("$CUSTOMERNAME", model.CustomerName);
                    subject = subject.Replace("$QUOTENAME", model.QuotesName);
                    string body = "Hi " + model.CustomerName + "," + LarastruckingResource.QuoteMailBody;
                    string bodywithsignature = body + " " + model.QuotesName + "." + LarastruckingResource.QuoteMailSignature;
                    string fileName = LarastruckingResource.QuoteFileName;
                    Dictionary<byte[], string> fileAttach = new Dictionary<byte[], string>();
                    fileAttach.Add(bytes, fileName);
                    string mailSentResponse = string.Empty;
                    MailWithCCAttachDTO mailData = new MailWithCCAttachDTO();
                    mailData.FileByte = bytes;
                    mailData.MailPurpose = "Quote";
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
                pdfDoc.Close();
                sr.Dispose();
            }
        }
        #endregion


        #region send quote email
        /// <summary>
        /// Send Quote email to customer
        /// </summary>
        /// <param name="quoteid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SendQuoteEmail(int quoteid)
        {

            try
            {
                var result = quoteBAL.SendQuote(quoteid);
                string isSend = SendAttachmentToClient(result);

                JsonResponse objJsonResponse = new JsonResponse();
                if (isSend == "Sent")
                {
                    objJsonResponse.IsSuccess = true;
                    objJsonResponse.Message = LarastruckingResource.QuoteSuccessMessage;
                }
                else
                {
                    objJsonResponse.Message = LarastruckingResource.SomethingWentWrong;
                }

                return Json(objJsonResponse, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region view and send Quote 
        /// <summary>
        /// view and send quote
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ViewAndSendQuote(int id)
        {
            try
            {
                var result = quoteBAL.SendQuote(id);
                return View(result);
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        #region get accessorial fee type
        /// <summary>
        /// get accessorail fee type
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAccessorialFeeType()
        {
            try
            {
                var accessorialfeelist = shipmentBAL.GetShipmentAccessorialFeeType();
                return Json(accessorialfeelist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion



        #region Get max route no.
        /// <summary>
        /// Get max route no.
        /// </summary>
        /// <param name="quoteId"></param>
        /// <returns></returns>

        public ActionResult GetMaxRouteNo(int quoteId)
        {
            int? maxRouteNo = quoteBAL.GetMaxRouteNo(quoteId);
            return Json(maxRouteNo, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }
}
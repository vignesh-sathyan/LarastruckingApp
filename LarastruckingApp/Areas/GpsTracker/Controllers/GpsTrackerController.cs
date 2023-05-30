using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.Entities.GpsTracker;
using LarastruckingApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LarastruckingApp.Areas.GpsTracker.Controllers
{
    [Authorize]
    public class GpsTrackerController : Controller
    {
        #region  Private Member
        /// <summary>
        /// Defining Private Member
        /// </summary>
      
        private readonly IGpsTrackingBAL IGpsTrackingBAL;
        public MemberProfile memberProfile;
        #endregion

        #region Contructor
        /// <summary>
        ///  Constructor Injection
        /// </summary>
        /// <param name="iShipmentBAL"></param>
        /// <param name="iGpsTrackingBAL"></param>
        public GpsTrackerController(IGpsTrackingBAL iGpsTrackingBAL)
        {
            IGpsTrackingBAL = iGpsTrackingBAL;
            memberProfile = new MemberProfile();
        }
        #endregion

        #region section : Action Result  


        #region Index: Get
        // GET: GPS Tracker/GpsTracker

        [HttpGet]
        [CustomAuthorize]
        public ActionResult Index(int id = 0)
        {
            ViewBag.ShipmentId = id;
            return View();
        }
        #endregion

        #region Index: Get
        // GET: GPS Tracker/GpsTracker

        [HttpGet]
        [CustomAuthorize]
        public ActionResult FumigationIndex(int id = 0)
        {
            ViewBag.FumigationId = id;
            return View();
        }
        #endregion

        #endregion

        #region section : Json Result 

        #region Get Shipment Details and Gps Tracking Details 
        /// <summary>
        ///  Get Shipment Details and Gps Tracking Details 
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetShipmentDetails(int shipmentId)
        {
            var result = IGpsTrackingBAL.GetShipmentDetails(shipmentId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Gps Tracking Details 
        /// <summary>
        ///  Get Shipment Details and Gps Tracking Details 
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetGpsTrackerDetails(GpsTrackerHistoryDTO dto)
        {
            var result = IGpsTrackingBAL.GetGpsTrackerDetails(dto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
         
        #region Get Fumigation Details and Gps Tracking Details 
        /// <summary>
        ///  Get Fumigation Details and Gps Tracking Details 
        /// </summary>
        /// <param name="FumigationId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetFumigationDetails(int FumigationId)
        {
            var result = IGpsTrackingBAL.GetFumigationDetails(FumigationId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Gps Fumigation Tracking Details 
        /// <summary>
        ///  Get Fumigation Details and Gps Tracking Details 
        /// </summary>
        /// <param name="FumigationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetFumigationGpsTrackerDetails(GpsTrackerHistoryDTO dto)
        {
            var result = IGpsTrackingBAL.GetFumigationGpsTrackerDetails(dto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

         
        #endregion
    }
}
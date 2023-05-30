using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LarastruckingApp.Controllers
{
    [Authorize]
    public class ShipmentController : Controller
    {
        #region Index: Get
        /// <summary>
        ///  Index: Get
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        #endregion
    }
}
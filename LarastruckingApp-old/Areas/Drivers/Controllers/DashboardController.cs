using LarastruckingApp.BusinessLayer.DriverModule;
using LarastruckingApp.Controllers;
using System.Web.Mvc;

namespace LarastruckingApp.Areas.Drivers.Controllers
{
    [Authorize(Roles = "Driver")]
    public class DashboardController : BaseController
    {
        #region Constructor
        public DashboardController()
        {
        }
        #endregion

        #region Dashboard
        public ActionResult Index()
        {
            return View();
        }
        #endregion

    }
}
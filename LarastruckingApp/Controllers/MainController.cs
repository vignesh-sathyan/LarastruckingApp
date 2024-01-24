using LarastruckingApp.BusinessLayer.Interface;
using LarastruckingApp.DTO;
using LarastruckingApp.Entities.Common;
using LarastruckingApp.Infrastructure;
using LarastruckingApp.Log.Utility;
using LarastruckingApp.Resource;
using LarastruckingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LarastruckingApp.Controllers
{
    [Authorize]
    public class MainController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}

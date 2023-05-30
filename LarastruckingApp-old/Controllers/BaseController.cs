using LarastruckingApp.Log;
using System;
using System.Web.Mvc;
using System.Web.SessionState;

namespace LarastruckingApp.Controllers
{
    public class BaseController : Controller
    {
        #region Private Members
        /// <summary>
        /// Defining private members
        /// </summary>
        private readonly ILogger iLog;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor of Base Controller
        /// </summary>
        public BaseController()
        {
            iLog = Logger.GetInstance;
        }
        #endregion

        #region OnException
        /// <summary>
        /// Logging OnException 
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            iLog.LogExceptions(filterContext.Exception.Message, filterContext.Exception.StackTrace, Convert.ToString(filterContext.Exception.InnerException == null ? string.Empty : Convert.ToString(filterContext.Exception.InnerException.InnerException)));
            filterContext.ExceptionHandled = true;
            View("Error").ExecuteResult(ControllerContext);
        }
        #endregion

        #region SharedSession
        /// <summary>
        /// Shared session property
        /// </summary>
        public HttpSessionState SharedSession
        {
            get
            {
                return System.Web.HttpContext.Current.Session;
            }
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LarastruckingApp.Infrastructure
{
    public sealed class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        //private readonly string[] allowedroles;
        //public CustomAuthorizeAttribute(params string[] roles)
        //{
            //this.allowedroles = roles;
        //}
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var requestData = httpContext.Request.RequestContext.RouteData;
            string currentAction = requestData.GetRequiredString("action");
            string currentController = requestData.GetRequiredString("controller");
            //string currentArea = requestData.Values["area"] as string;
            MemberProfile mp = new MemberProfile();
            if (mp != null)
            {
                var permission = mp.Permissions.ToList();
                authorize = (permission.Where(x => x.ControllerName == currentController && x.ActionName == currentAction && x.CanView==true).FirstOrDefault()) == null ? false : true;

            }
            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {      {"area",""},
                    { "controller", "User" },
                    { "action", "PageNotFound" }
               });
        }
    }

}
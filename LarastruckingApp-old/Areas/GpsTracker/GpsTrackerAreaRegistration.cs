using System.Web.Mvc;

namespace LarastruckingApp.Areas.GpsTracker
{
    public class GpsTrackerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GpsTracker";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GpsTracker_default",
                "GpsTracker/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
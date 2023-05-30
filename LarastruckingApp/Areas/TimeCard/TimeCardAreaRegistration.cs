using System.Web.Mvc;

namespace LarastruckingApp.Areas.TimeCard
{
    public class TimeCardAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TimeCard";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TimeCard_default",
                "TimeCard/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
using System.Web.Mvc;

namespace LarastruckingApp.Areas.Quote
{
    public class QuoteAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Quote";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Quote_default",
                "Quote/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
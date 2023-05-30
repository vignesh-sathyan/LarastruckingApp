using System.Web.Mvc;

namespace LarastruckingApp.Areas.TrailerRental
{
    public class TrailerRentalAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TrailerRental";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TrailerRental_default",
                "TrailerRental/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
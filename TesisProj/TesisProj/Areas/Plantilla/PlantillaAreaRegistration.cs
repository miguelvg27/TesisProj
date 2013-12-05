using System.Web.Mvc;

namespace TesisProj.Areas.Plantilla
{
    public class PlantillaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Plantilla";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Plantilla_default",
                "Plantilla/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

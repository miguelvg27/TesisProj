using System.Web.Mvc;

namespace TesisProj.Areas.Modelos
{
    public class ModelosAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Modelos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Modelos_default",
                "Modelos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

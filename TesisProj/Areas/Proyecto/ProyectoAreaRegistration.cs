using System.Web.Mvc;

namespace TesisProj.Areas.Proyecto
{
    public class ProyectoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Proyecto";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Proyecto_default",
                "Proyecto/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

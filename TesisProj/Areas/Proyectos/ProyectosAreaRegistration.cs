using System.Web.Mvc;

namespace TesisProj.Areas.Proyectos
{
    public class ProyectosAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Proyectos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Proyectos_default",
                "Proyectos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

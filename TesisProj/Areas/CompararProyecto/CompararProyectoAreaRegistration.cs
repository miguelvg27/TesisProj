using System.Web.Mvc;

namespace TesisProj.Areas.CompararProyecto
{
    public class CompararProyectoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CompararProyecto";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CompararProyecto_default",
                "CompararProyecto/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

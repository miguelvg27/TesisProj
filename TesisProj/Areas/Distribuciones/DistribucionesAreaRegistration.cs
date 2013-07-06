using System.Web.Mvc;

namespace TesisProj.Areas.Distribuciones
{
    public class DistribucionesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Distribuciones";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Distribuciones_default",
                "Distribuciones/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

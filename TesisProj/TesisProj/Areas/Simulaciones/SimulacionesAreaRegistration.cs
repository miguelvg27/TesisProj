using System.Web.Mvc;

namespace TesisProj.Areas.Simulaciones
{
    public class SimulacionesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Simulaciones";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Simulaciones_default",
                "Simulaciones/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

using System.Web.Mvc;

namespace TesisProj.Areas.Comparacion
{
    public class ComparacionAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Comparacion";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Comparacion_default",
                "Comparacion/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

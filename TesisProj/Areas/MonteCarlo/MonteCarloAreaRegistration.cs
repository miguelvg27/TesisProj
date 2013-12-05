using System.Web.Mvc;

namespace TesisProj.Areas.MonteCarlo
{
    public class MonteCarloAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MonteCarlo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MonteCarlo_default",
                "MonteCarlo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

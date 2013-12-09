using System.Web.Mvc;

namespace TesisProj.Areas.IridiumTest
{
    public class IridiumTestAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "IridiumTest";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "IridiumTest_default",
                "IridiumTest/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TesisProj.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /FailWhale/

        public ActionResult FailWhale()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

        public ActionResult DeniedWhale()
        {
            return View();
        }
    }
}

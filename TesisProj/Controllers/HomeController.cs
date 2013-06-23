using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Filters;
using TesisProj.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Administra las evaluaciones financieras de tus proyectos de inversión.";
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "En desarrollo";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Miguel AVG y asociados";

            return View();
        }
    }
}

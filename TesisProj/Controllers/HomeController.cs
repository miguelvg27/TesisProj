using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Administra las evaluaciones financieras de tus proyectos de inversión.";
            using (TProjContext context = new TProjContext()) 
            {
                context.TipoElementos.All();
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "En desarrollo";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Miguel AVG y asociados";

            return View();
        }
    }
}

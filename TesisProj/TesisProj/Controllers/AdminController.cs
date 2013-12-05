using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TesisProj.Controllers
{
    [Authorize(Roles="nav")]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            ViewBag.Message = "Administra los parámetros y plantillas de las evaluaciones financieras.";
            return View();
        }
    }
}

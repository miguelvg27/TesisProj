using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Seguridad.Controllers
{
    public class SeguridadController : Controller
    {
        //
        // GET: /Seguridad/Seguridad/
        TProjContext context = new TProjContext();

        public ActionResult Index()
        {
            List<Proyecto> proyectos = context.Proyectos.Where(p => p.Creador.UserName == User.Identity.Name).ToList();
            return View(proyectos);
        }

    }
}

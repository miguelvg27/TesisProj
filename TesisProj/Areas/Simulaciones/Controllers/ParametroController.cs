using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Modelos.Models;
using TesisProj.Areas.Simulaciones.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Simulaciones.Controllers
{
    public class ParametroController : Controller
    {
        //
        // GET: /Simulaciones/Parametro/

        TProjContext context = new TProjContext();

        public ActionResult Index(int p, int y)
        {
            Asignacion a = new Asignacion();
            ViewData["Distribuciones"] = context.TablaDistribucion.All();
            a.Celdas = context.TablaProyecto.One(r => r.Id == y).parametro;//representa el arreglo de celdas
            a.Distribuciones = context.TablaDistribucion.All();
            return View(a);
        }

        [HttpPost]
        public JsonResult _GetModelos(int? Distribuciones)
        {
            var t=context.TablaModelo.Where(m => m.Distribucion.Id == Distribuciones);
            return Json(new SelectList(t, "Id", "Nombre"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult _GetModelo(int modelo)
        {
            ModeloSimlacion mm = new ModeloSimlacion();

            if (modelo == 1)
            {
                mm.Binomial = new Binomial();
            }
            if (modelo == 2)
            {
                mm.Geometrica = new Geometrica();
            }
            if (modelo == 3)
            {
                mm.Hipergeometrica = new Hipergeometrica();
            }
            if (modelo == 4)
            {
                mm.Pascal = new Pascal();
            }
            if (modelo == 5)
            {
                mm.Poisson = new Poisson();
            }
            if (modelo == 6)
            {
                mm.Uniforme = new Uniforme();
            }
            if (modelo == 7)
            {
                mm.Normal = new Normal();
            }
            Session["Modelo"] = mm;
            return Json(new SelectList(context.TablaModelo.Where(m => m.Id == modelo), "ID", "Nombre"), JsonRequestBehavior.AllowGet);
        }
    }
}

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
            Session["Celdas_a_simular"] = a.Celdas;// esto permite parsar el modelo entre controller a simular
            a.Distribuciones = context.TablaDistribucion.All();
            return View(a);
        }

        [HttpPost]
        public JsonResult _GetModelos(int? Distribuciones)
        {
            var t=context.TablaDistribucion.One(m => m.Id == Distribuciones).Modelos;
            return Json(new SelectList(t, "Id", "Nombre"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult _GetModelo(int modelo)
        {
            ModeloSimlacion mm = new ModeloSimlacion();
            Session["Modelo"] = mm;
            return Json(new SelectList(context.TablaModelo.Where(m => m.Id == modelo), "Id", "Nombre"), JsonRequestBehavior.AllowGet);
        }
    }
}

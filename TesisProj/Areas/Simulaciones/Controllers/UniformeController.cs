using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Modelos.Models;
using TesisProj.Areas.Simulaciones.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Simulaciones.Controllers
{
    public class UniformeController : Controller
    {
        //
        // GET: /Simulaciones/Uniforme/
        TProjContext context = new TProjContext();

        [HttpGet]
        public ActionResult Index(int idParametro)
        {
            Parametro p = context.Parametros.Include("Elemento").Include("Celdas").Where(e => e.Id == idParametro).FirstOrDefault();
            ModeloSimlacion m = new ModeloSimlacion();
            ViewBag.idParametro = idParametro;
            double a = 0; 
            double b = p.Celdas.Max(e => Convert.ToDouble(e.Valor));
            m.Uniforme = new Uniforme(a,b);
            m.Nombre = "Uniforme";
            MaestroSimulacion maestro = new MaestroSimulacion(m);
            maestro.ActualizarCeldas("Uniforme", p);
            p.CeldasSensibles = maestro.CeldasSensibles;

            Session["Parametro"] = p;
            Session["GraficoSimulacion"] = m.Uniforme.graficar;
            Session["Celdas_simulada"] = p.CeldasSensibles;

            return View(m.Uniforme);
        }

        [ChildActionOnly]
        public ActionResult _CeldasSimuladas()
        {
            return PartialView((List<Celda>)Session["Celdas_simulada"]);
        }

        [ChildActionOnly]
        public ActionResult _GraficoSimulacion()
        {
            return PartialView((List<Grafico>)Session["GraficoSimulacion"]);
        }

        [HttpPost]
        public ActionResult Index(Uniforme n, int idParametro)
        {
            Parametro p = context.Parametros.Include("Elemento").Include("Celdas").Where(e => e.Id == idParametro).FirstOrDefault();
            ViewBag.idParametro = idParametro;
            ModeloSimlacion m = new ModeloSimlacion();
            double a = n.a;
            double b = n.b;
            m.Uniforme = new Uniforme(a, b);
            m.Nombre = "Uniforme";
            MaestroSimulacion maestro = new MaestroSimulacion(m);
            maestro.ActualizarCeldas("Uniforme", p);
            p.CeldasSensibles = maestro.CeldasSensibles;

            Session["Parametro"] = p;
            Session["GraficoSimulacion"] = m.Uniforme.graficar;
            Session["Celdas_simulada"] = p.CeldasSensibles;

            return View(m.Uniforme);
        }

    }
}

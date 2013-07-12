using System;
using System.Collections.Generic;
using System.Data;
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
        public ActionResult Index(int idParametro, int ProyectoId)
        {
            Parametro p = context.Parametros.Include("Elemento").Include("Celdas").Include("Uniforme").Where(e => e.Id == idParametro).FirstOrDefault();

            ViewBag.idParametro = idParametro;
            ViewBag.idProyecto = ProyectoId;
            double a = 0;
            double b = p.Celdas.Max(e => Convert.ToDouble(e.Valor));
            p.uniforme.u_a = a;
            p.uniforme.u_b = b;
            p.uniforme.Nombre = "Uniforme";
            MaestroSimulacion maestro = new MaestroSimulacion();
            maestro.uniforme = p.uniforme;
            maestro.ActualizarCeldas("Uniforme", p);
            p.CeldasSensibles = maestro.CeldasSensibles;
            p.normal.IsEliminado = true;
            p.binomial.IsEliminado = true;
            p.geometrica.IsEliminado = true;
            p.hipergeometrica.IsEliminado = true;
            p.pascal.IsEliminado = true;
            p.poison.IsEliminado = true;
            p.uniforme.IsEliminado = false;
            Session["GraficoSimulacion"] = p.uniforme.graficar;
            Session["Celdas_simulada"] = p.CeldasSensibles;
            context.Entry(p).State = EntityState.Modified;
            context.SaveChanges();
            return View(p.uniforme);
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
        public ActionResult Index(Uniforme n, int idParametro, int ProyectoId)
        {
            Parametro p = context.Parametros.Include("Elemento").Include("Celdas").Where(e => e.Id == idParametro).FirstOrDefault();
            ViewBag.idParametro = idParametro;
            ViewBag.idProyecto = ProyectoId;
            double a = n.u_a;
            double b = n.u_b;
            p.uniforme.u_a = a;
            p.uniforme.u_b = b;
            p.uniforme.Nombre = "Uniforme";
            MaestroSimulacion maestro = new MaestroSimulacion();
            maestro.uniforme = p.uniforme;
            maestro.ActualizarCeldas("Uniforme", p);
            p.CeldasSensibles = maestro.CeldasSensibles;
            p.normal.IsEliminado = true;
            p.binomial.IsEliminado = true;
            p.geometrica.IsEliminado = true;
            p.hipergeometrica.IsEliminado = true;
            p.pascal.IsEliminado = true;
            p.poison.IsEliminado = true;
            p.uniforme.IsEliminado = false;
            Session["GraficoSimulacion"] = p.uniforme.graficar;
            Session["Celdas_simulada"] = p.CeldasSensibles;
            context.Entry(p).State = EntityState.Modified;
            context.SaveChanges();
            return View(p.uniforme);
        }
    }
}


                
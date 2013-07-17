using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Modelos.Models;
using TesisProj.Areas.Proyectos.Models;
using TesisProj.Areas.Simulaciones.Models;
using TesisProj.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Simulaciones.Controllers
{
    public class NormalController : Controller
    {
        //
        // GET: /Simulaciones/Normal/
        TProjContext context = new TProjContext();

        [HttpGet]
        public ActionResult Index(int idParametro, int ProyectoId)
        {
            using (TProjContext context = new TProjContext())
            {
                Parametro p = context.Parametros.Include("Elemento").Include("Celdas").Include("Normal").Where(e => e.Id == idParametro).FirstOrDefault();

                ViewBag.idProyecto = ProyectoId;
                ViewBag.idParametro = idParametro;
                double mean = p.Celdas.Average(e => Convert.ToDouble(e.Valor));
                double std = Calculos.DesviacionStandard(p.Celdas.Select(e => Convert.ToDouble(e.Valor)).ToList());
                p.normal.n_mean = mean;
                p.normal.n_std = std;
                p.normal.n_K = p.Celdas.Count;
                p.normal.Nombre = "Normal";
                MaestroSimulacion maestro = new MaestroSimulacion();
                maestro.normal = p.normal;
                maestro.ActualizarCeldas( "Normal",p);
                p.CeldasSensibles = maestro.CeldasSensibles;
                p.normal.IsEliminado = false;
                p.binomial.IsEliminado = true;
                p.geometrica.IsEliminado = true;
                p.hipergeometrica.IsEliminado = true;
                p.pascal.IsEliminado = true;
                p.poisson.IsEliminado = true;
                p.uniforme.IsEliminado = true;
                Session["GraficoSimulacion"] = p.normal.graficar;
                Session["Celdas_simulada"] = p.CeldasSensibles;
                context.Entry(p).State = EntityState.Modified;
                context.SaveChanges();

                return View(p.normal);
            }
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
        public ActionResult Index(Normal n, int idParametro, int ProyectoId)
        {

            Parametro p = context.Parametros.Include("Elemento").Include("Celdas").Include("Normal").Where(e => e.Id == idParametro).FirstOrDefault();

            ViewBag.idProyecto = ProyectoId;
            ViewBag.idParametro = idParametro;
            double mean = n.n_mean;
            double std = n.n_std;
            p.normal.n_mean = mean;
            p.normal.n_std = std;
            p.normal.n_K = p.Celdas.Count;
            p.normal.Nombre = "Normal";
            MaestroSimulacion maestro = new MaestroSimulacion();
            maestro.normal = p.normal;
            maestro.ActualizarCeldas("Normal",p);
            p.CeldasSensibles = maestro.CeldasSensibles;
            p.normal.IsEliminado = false;
            p.binomial.IsEliminado = true;
            p.geometrica.IsEliminado = true;
            p.hipergeometrica.IsEliminado = true;
            p.pascal.IsEliminado = true;
            p.poisson.IsEliminado = true;
            p.uniforme.IsEliminado = true;
            Session["GraficoSimulacion"] = p.normal.graficar;
            Session["Celdas_simulada"] = p.CeldasSensibles;
            context.Entry(p).State = EntityState.Modified;
            context.SaveChanges();
            return View(p.normal);
        }

    }
}

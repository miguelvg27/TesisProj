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
    public class PoissonController : Controller
    {
        //
        // GET: /Simulaciones/Poisson/
        TProjContext context = new TProjContext();

        [HttpGet]
        public ActionResult Index(int idParametro, int ProyectoId)
        {
            using (TProjContext context = new TProjContext())
            {
                Parametro p = context.Parametros.Include("Elemento").Include("Celdas").Include("Poisson").Where(e => e.Id == idParametro).FirstOrDefault();

                ViewBag.idProyecto = ProyectoId;
                ViewBag.idParametro = idParametro;
                double mean = p.Celdas.Average(e => Convert.ToDouble(e.Valor));

                p.poisson.po_L = mean;
                p.poisson.po_K = p.Celdas.Count;
                p.poisson.limiteInf = 0;
                p.poisson.limiteSup = p.Celdas.Count;
                p.poisson.Nombre = "Poisson";

                MaestroSimulacion maestro = new MaestroSimulacion();
                maestro.poisson = p.poisson;
                maestro.ActualizarCeldas("Poisson", p);
                p.CeldasSensibles = maestro.CeldasSensibles;
                p.normal.IsEliminado = true;
                p.binomial.IsEliminado = true;
                p.geometrica.IsEliminado = true;
                p.hipergeometrica.IsEliminado = true;
                p.pascal.IsEliminado = true;
                p.poisson.IsEliminado = false;
                p.uniforme.IsEliminado = true;
                Session["GraficoSimulacion"] = p.poisson.graficar;
                Session["Celdas_simulada"] = p.CeldasSensibles;
                context.Entry(p).State = EntityState.Modified;
                context.SaveChanges();
                return View(p.poisson);
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
        public ActionResult Index(Poisson po ,int idParametro, int ProyectoId)
        {
            using (TProjContext context = new TProjContext())
            {
                Parametro p = context.Parametros.Include("Elemento").Include("Celdas").Include("Normal").Include("Poisson").Where(e => e.Id == idParametro).FirstOrDefault();

                ViewBag.idProyecto = ProyectoId;
                ViewBag.idParametro = idParametro;

                p.poisson.po_L = po.po_L;
                p.poisson.po_K = po.limiteInf;
                p.poisson.limiteInf = po.limiteInf;
                p.poisson.limiteSup = po.limiteInf + po.limiteInf;
                p.poisson.Nombre = "Poisson";

                MaestroSimulacion maestro = new MaestroSimulacion();
                maestro.poisson = p.poisson;
                maestro.ActualizarCeldas("Poisson", p);
                p.CeldasSensibles = maestro.CeldasSensibles;
                p.normal.IsEliminado = true;
                p.binomial.IsEliminado = true;
                p.geometrica.IsEliminado = true;
                p.hipergeometrica.IsEliminado = true;
                p.pascal.IsEliminado = true;
                p.poisson.IsEliminado = false;
                p.uniforme.IsEliminado = true;
                Session["GraficoSimulacion"] = p.poisson.graficar;
                Session["Celdas_simulada"] = p.CeldasSensibles;
                context.Entry(p).State = EntityState.Modified;
                context.SaveChanges();
                return View(p.poisson);
            }
        }

    }
}

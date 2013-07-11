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
        public ActionResult Index(int idParametro)
        {
            Parametro p = context.Parametros.Include("Elemento").Include("Celdas").Where(e => e.Id == idParametro).FirstOrDefault();
            ModeloSimlacion m = context.TablaModeloSimulacion.One(s=>s.Id==7);
            ViewBag.idParametro = idParametro;
            double mean = p.Celdas.Average(e => Convert.ToDouble(e.Valor));
            double std = Calculos.DesviacionStandard(p.Celdas.Select(e => Convert.ToDouble(e.Valor)).ToList());
            m.Normal = new Normal(mean,std,p.Celdas.Count);
            m.Nombre = "Normal";
            MaestroSimulacion maestro = new MaestroSimulacion(m);
            maestro.ActualizarCeldas("Normal",p);
            p.CeldasSensibles = maestro.CeldasSensibles;

            Session["GraficoSimulacion"] = m.Normal.graficar;
            Session["Celdas_simulada"] = p.CeldasSensibles;
            context.TablaNormal.ModifyElement(m.Normal);
            p.modelo = maestro.modelobase;
            context.Entry(p).State = EntityState.Modified;
            context.SaveChanges();

            return View(m.Normal);
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
        public ActionResult Index(Normal n, int idParametro)
        {
            Parametro p = context.Parametros.Include("Elemento").Include("Celdas").Where(e => e.Id == idParametro).FirstOrDefault();
            ViewBag.idParametro = idParametro;
            ModeloSimlacion m = context.TablaModeloSimulacion.One(s => s.Id == 7);
            double mean = n.mean;
            double std = n.std;
            m.Normal = new Normal(mean, std,p.Celdas.Count);
            m.Nombre = "Normal";
            MaestroSimulacion maestro = new MaestroSimulacion(m);
            maestro.ActualizarCeldas("Normal", p);
            p.CeldasSensibles = maestro.CeldasSensibles;

            Session["GraficoSimulacion"] = m.Normal.graficar;
            Session["Celdas_simulada"] = p.CeldasSensibles;
            context.TablaNormal.ModifyElement(m.Normal);
            p.modelo = maestro.modelobase;
            context.Entry(p).State = EntityState.Modified;
            context.SaveChanges();
            return View(m.Normal);
        }

    }
}

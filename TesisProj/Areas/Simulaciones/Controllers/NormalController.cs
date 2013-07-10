using System;
using System.Collections.Generic;
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
            ModeloSimlacion m = new ModeloSimlacion();
            ViewBag.idParametro = idParametro;
            double mean = p.Celdas.Average(e => Convert.ToDouble(e.Valor));
            double std = Calculos.DesviacionStandard(p.Celdas.Select(e => Convert.ToDouble(e.Valor)).ToList());
            m.Normal = new Normal(mean,std);
            m.Nombre = "Normal";
            MaestroSimulacion maestro = new MaestroSimulacion(m);
            maestro.ActualizarCeldas("Normal",p);
            p.CeldasSensibles = maestro.GetCeldasSimuladas();

            List<double> grafico = new List<double>();
            
            foreach (Celda c in p.CeldasSensibles)
            {
                double aux = Convert.ToDouble(c.Valor);
                grafico.Add(aux);
            }

            m.Normal.GetFuncionSimpleArreglo(grafico);
            Session["Grafico"] = m.Normal.graficar;
            Session["Celdas_simulada"] = p.CeldasSensibles;

            return View(m.Normal);
        }

        [ChildActionOnly]
        public ActionResult _CeldasSimuladas()
        {
            return PartialView((List<Celda>)Session["Celdas_simulada"]);
        }

        [ChildActionOnly]
        public ActionResult _Grafico()
        {
            return PartialView((List<Grafico>)Session["Grafico"]);
        }

        [HttpPost]
        public ActionResult Index(Normal n)
        {
            return View();
        }

    }
}

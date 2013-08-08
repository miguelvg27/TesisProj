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
                p.normal = new Normal();
                Double mean= Math.Round(p.Celdas.Average(e => Convert.ToDouble(e.Valor)),2);
                p.normal.n_mean = mean;
                p.normal.n_std = 1;
                
                p.normal.numeroCeldas = p.Celdas.Count;
                p.normal.maximo = mean + 100;
                
                if (mean - 100<0) {
                    p.normal.minimo =0;
                }else{
                    p.normal.minimo =mean-100;
                }

                p.normal.amplitud = 0.1*10*Math.Truncate(Math.Log10(p.normal.maximo - p.normal.minimo));
                p.normal.minimoEsperado = p.normal.minimo;
                p.normal.maximoEsperado = p.normal.maximo;
                
                List<Grafico>  CeldasSensibles =p.normal.GetAleatoriosEsperados(p.Celdas.Count);
                p.CeldasSensibles= new List<Celda>();
                for(int i=0; i<CeldasSensibles.Count; i++)
                {
                    decimal valor = Math.Round(Convert.ToDecimal(CeldasSensibles[i].fx),2);
                    p.CeldasSensibles.Add(new Celda { 
                        IdParametro = p.Celdas[i].IdParametro, 
                        Valor = valor, 
                        Periodo = p.Celdas[i].Periodo 
                    });
                }
                Session["FuncionProbabilidad"] = p.normal.GetFuncionProbabilidad();
                Session["FuncionEsperados"] = p.normal.GetFuncionEsperados();
                Session["CeldasSensibles"] = p.CeldasSensibles;
                Session["AleatoriosTotales"] = p.normal.GetAleatoriosTotales();
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

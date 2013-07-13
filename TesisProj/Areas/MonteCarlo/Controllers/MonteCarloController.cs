using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Modelos.Models;
using TesisProj.Areas.MonteCarlo.Models;
using TesisProj.Areas.Simulaciones.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.MonteCarlo.Controllers
{
    public class MonteCarloController : Controller
    {

        TProjContext context = new TProjContext();
        //
        // GET: /MonteCarlo/MonteCarlo/

        [HttpGet]
        public ActionResult Index(int idProyecto)
        {
            ViewBag.idProyecto = idProyecto;
            AlgoritmoMonteCarlo mc = new AlgoritmoMonteCarlo();
            mc.Parametros = context.Parametros.Include("Elemento").Include("Celdas").Include("Normal").Include("Uniforme").Where(e => e.Elemento.IdProyecto == idProyecto).Where(oo => oo.Sensible == true).ToList();
            return View(mc);
        }

        [HttpPost]
        public ActionResult Index(AlgoritmoMonteCarlo mc, int idProyecto)
        {
            mc.Parametros = context.Parametros.Include("Elemento").Include("Celdas").Include("Normal").Include("Uniforme").Where(e => e.Elemento.IdProyecto == idProyecto).Where(oo => oo.Sensible == true).ToList();
            Proyecto proy = context.Proyectos.Find(idProyecto);


            List<TempGrafico> salida1 = new List<TempGrafico>();
            List<TempGrafico> salida2= new List<TempGrafico>();
            List<TempGrafico> salida3 = new List<TempGrafico>();
            List<TempGrafico> salida4 = new List<TempGrafico>();


            //Temporales prara simular resultados
            List<Grafico> grafico1 = new List<Grafico>();
            List<Grafico> grafico2 = new List<Grafico>();
            List<Grafico> grafico3 = new List<Grafico>();
            List<Grafico> grafico4 = new List<Grafico>();

            List<Grafico> graficoVanInversionista = new List<Grafico>();
            List<Grafico> graficoVanProyecto = new List<Grafico>();
            List<Grafico> graficoTirInversionista = new List<Grafico>();
            List<Grafico> graficoTirProyecto = new List<Grafico>();

            for (int u = 1; u <= mc.NumeroSimulaciones; u++)
            {
                foreach (Elemento e in proy.Elementos)
                {
                    foreach (Parametro p in e.Parametros.Where(o=>o.Sensible==true))
                    {
                        if (p.normal.IsEliminado == false)
                        {
                            MaestroSimulacion maestro = new MaestroSimulacion();
                            maestro.normal = p.normal;
                            maestro.ActualizarCeldas("Normal", p);
                            p.CeldasSensibles = maestro.CeldasSensibles;
                        }
                        if (p.uniforme.IsEliminado == false)
                        {
                            MaestroSimulacion maestro = new MaestroSimulacion();
                            maestro.uniforme = p.uniforme;
                            maestro.ActualizarCeldas("Uniforme", p);
                            p.CeldasSensibles = maestro.CeldasSensibles;
                        }

                        using (StreamWriter sw = new StreamWriter(@"C:\Reportes\MonteCarlo.txt", true))
                        {
                            sw.WriteLine("Normal fx" + " - " + DateTime.Now.ToString());
                            sw.WriteLine("|x" + "  -  " + "fx|");
                            int i = 0;
                            if (p.uniforme.IsEliminado == false) sw.WriteLine("Uniforme");
                            if (p.normal.IsEliminado == false) sw.WriteLine("Normal");
                            foreach (Celda g in p.CeldasSensibles)
                            {
                                sw.WriteLine("|" + ++i + "  -  " + g.Valor + "|");
                            }
                            sw.WriteLine();
                        }
                    }
                }
                grafico1.Add(SimularVanInversionista(u, 100));
                grafico2.Add(SimularVanProyecto(u, 100));
                grafico3.Add(SimularTirInversionista(u, 1));
                grafico4.Add(SimularTirProyecto(u, 1));
            }

            int amplitudVanInversionista = Amplitud(GraficoMinimo(grafico1, 1), GraficoMaximo(grafico1, 1));
            int amplitudVanProyecto = Amplitud(GraficoMinimo(grafico2, 1), GraficoMaximo(grafico2, 1));
            int amplitudTirInversionista = Amplitud(GraficoMinimo(grafico3, 100), GraficoMaximo(grafico3, 100));
            int amplitudTirProyecto = Amplitud(GraficoMinimo(grafico4, 100), GraficoMaximo(grafico4, 100));

            for (int y = 0; y < mc.NumeroIntervalos; y++)
            {
                TempGrafico TMPVanInversionista = new TempGrafico();
                TMPVanInversionista.Indice = GraficoMinimo(grafico1, 1) + amplitudVanInversionista * y;
                TMPVanInversionista.Cantidad = 0;
                salida1.Add(TMPVanInversionista);

                TempGrafico TMPVanProyecto = new TempGrafico();
                TMPVanProyecto.Indice = GraficoMinimo(grafico2, 1) + amplitudVanProyecto * y;
                TMPVanProyecto.Cantidad = 0;
                salida2.Add(TMPVanProyecto);

                TempGrafico TMPTirInversionista = new TempGrafico();
                TMPTirInversionista.Indice = GraficoMinimo(grafico3, 1) + amplitudTirInversionista * y;
                TMPTirInversionista.Cantidad = 0;
                salida3.Add(TMPTirInversionista);

                TempGrafico TMTirProyecto = new TempGrafico();
                TMTirProyecto.Indice = GraficoMinimo(grafico4, 1) + amplitudTirProyecto * y;
                TMTirProyecto.Cantidad = 0;
                salida4.Add(TMPVanInversionista);
            }

            for (int u = 0; u < mc.NumeroSimulaciones; u++)
            {
                for (int q = 0; q < salida1.Count - 1; q++)
                {
                    salida1[q].Cantidad = grafico1.Where(c => ((c.fx >= salida1[q].Indice) && (c.fx <= salida1[q].Indice))).Count();
                }

                for (int q = 0; q < salida2.Count - 1; q++)
                {
                    salida2[q].Cantidad = grafico2.Where(c => ((c.fx >= salida2[q].Indice) && (c.fx <= salida2[q].Indice))).Count();
                }

                for (int q = 0; q < salida3.Count - 1; q++)
                {
                    salida3[q].Cantidad = grafico3.Where(c => ((c.fx >= salida3[q].Indice) && (c.fx <= salida3[q].Indice))).Count();
                }

                for (int q = 0; q < salida4.Count - 1; q++)
                {
                    salida4[q].Cantidad = grafico4.Where(c => ((c.fx >= salida4[q].Indice) && (c.fx <= salida4[q].Indice))).Count();
                }


            }

            foreach (TempGrafico t in salida1)
            {
                graficoVanInversionista.Add(new Grafico { fx = t.Cantidad, x = t.Indice, sfx = (t.Cantidad * 100).ToString(), sx = t.Indice.ToString() });
            }
            foreach (TempGrafico t in salida2)
            {
                graficoVanProyecto.Add(new Grafico { fx = t.Cantidad, x = t.Indice, sfx = (t.Cantidad * 100).ToString(), sx = t.Indice.ToString() });
            }
            foreach (TempGrafico t in salida3)
            {
                graficoTirInversionista.Add(new Grafico { fx = t.Cantidad, x = t.Indice, sfx = (t.Cantidad * 100).ToString(), sx = t.Indice.ToString() });
            }
            foreach (TempGrafico t in salida4)
            {
                graficoTirProyecto.Add(new Grafico { fx = t.Cantidad, x = t.Indice, sfx = (t.Cantidad * 100).ToString(), sx = t.Indice.ToString() });
            }


            mc.VanInversionista = graficoVanInversionista;
            mc.VanProyecto = graficoVanProyecto;
            mc.TirInversionista = graficoTirInversionista;
            mc.TirProyecto = graficoTirProyecto;

            mc.MaxVanInversionista = GraficoMaximo(grafico1, 1);
            mc.MaxVanProyecto = GraficoMaximo(grafico2, 1);
            mc.MaxTirProyecto = GraficoMaximo(grafico3, 1);
            mc.MaxTirInversionista = GraficoMaximo(grafico4, 1);

            mc.MinVanInversionista = GraficoMinimo(grafico1, 1);
            mc.MinVanProyecto = GraficoMinimo(grafico2, 1);
            mc.MinTirProyecto = GraficoMinimo(grafico3, 1);
            mc.MinTirInversionista = GraficoMinimo(grafico4, 1);

            Session["_GraficoVanInversionista"] = mc.VanInversionista;
            Session["_GraficoVanProyecto"] = mc.VanProyecto;
            Session["_GraficoTirProyecto"] = mc.TirProyecto;
            Session["_GraficoTirInversionista"] = mc.TirInversionista;

            return RedirectToAction("Resultados",mc);
        }

        public ActionResult Resultados(AlgoritmoMonteCarlo salida)
        {
            return View(salida);
        }

        [ChildActionOnly]
        public ActionResult _GraficoVanInversionista()
        {
            return PartialView((List<Grafico>)Session["_GraficoVanInversionista"]);
        }

        [ChildActionOnly]
        public ActionResult _GraficoVanProyecto()
        {
            return PartialView((List<Grafico>)Session["_GraficoVanProyecto"]);
        }

        [ChildActionOnly]
        public ActionResult _GraficoTirProyecto()
        {
            return PartialView((List<Grafico>)Session["_GraficoTirProyecto"]);
        }

        [ChildActionOnly]
        public ActionResult _GraficoTirInversionista()
        {
            return PartialView((List<Grafico>)Session["_GraficoTirInversionista"]);
        }

        private int GraficoMinimo(List<Grafico> l, int porcentaje)
        {
            return Convert.ToInt32(Math.Truncate(l.Min(p => p.fx * porcentaje)));
        }

        private int GraficoMaximo(List<Grafico> l, int porcentaje)
        {
            return Convert.ToInt32(Math.Truncate(l.Max(p => p.fx * porcentaje)));
        }

        private int Amplitud(int minimo, int maximo)
        {
            return 1;
        }

        private Grafico SimularVanProyecto(int u,int ajuste)
        {
            Random r1 = new Random();
            Grafico gr = new Grafico();
            gr.fx = r1.NextDouble() * ajuste;
            gr.x = u;
            gr.sx = Convert.ToString(u);
            gr.sfx = Convert.ToString(Math.Round(gr.fx * 100, 2));
            return gr;
        }

        private Grafico SimularVanInversionista(int u, int ajuste)
        {
            Random r1 = new Random();
            Grafico gr = new Grafico();
            gr.fx = r1.NextDouble() * ajuste;
            gr.x = u;
            gr.sx = Convert.ToString(u);
            gr.sfx = Convert.ToString(Math.Round(gr.fx * 100, 2));
            return gr;
        }

        private Grafico SimularTirInversionista(int u, int ajuste)
        {
            Random r1 = new Random();
            Grafico gr = new Grafico();
            gr.fx = r1.NextDouble() * ajuste;
            gr.x = u;
            gr.sx = Convert.ToString(u);
            gr.sfx = Convert.ToString(Math.Round(gr.fx * 100, 2));
            return gr;
        }

        private Grafico SimularTirProyecto(int u, int ajuste)
        {
            Random r1 = new Random();
            Grafico gr = new Grafico();
            gr.fx = r1.NextDouble() * ajuste;
            gr.x = u;
            gr.sx = Convert.ToString(u);
            gr.sfx = Convert.ToString(Math.Round(gr.fx * 100, 2));
            return gr;
        }
    }
}

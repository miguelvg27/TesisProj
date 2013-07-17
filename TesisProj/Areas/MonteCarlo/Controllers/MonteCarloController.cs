using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Modelo.Controllers;
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
            Session["idProyecto"] = idProyecto;
            ViewBag.idProyecto = idProyecto;
            AlgoritmoMonteCarlo mc = new AlgoritmoMonteCarlo();
            mc.Parametros = context.Parametros.Include("Elemento").Include("Celdas").Include("Normal").Include("Uniforme").Where(e => e.Elemento.IdProyecto == idProyecto).Where(oo => oo.Sensible == true).ToList();
            return View(mc);
        }

        [HttpPost]
        public ActionResult Index(AlgoritmoMonteCarlo mc, int idProyecto)
        {
            ViewBag.idProyecto = (int)Session["idProyecto"];
            mc.Parametros = context.Parametros.Include("Elemento").Include("Celdas").Include("Normal").Include("Uniforme").Where(e => e.Elemento.IdProyecto == idProyecto).Where(oo => oo.Sensible == true).ToList();
            Proyecto proy = context.Proyectos.Find(idProyecto);
            List<Hash>  vanE = new List<Hash>();
            List<Hash>  vanF = new List<Hash>();
            List<Hash>  tirE = new List<Hash>();
            List<Hash>  tirF = new List<Hash>();

            //Temporales prara simular resultados


            List<Grafico> graficoVanInversionista = new List<Grafico>();
            List<Grafico> graficoVanProyecto = new List<Grafico>();
            List<Grafico> graficoTirInversionista = new List<Grafico>();
            List<Grafico> graficoTirProyecto = new List<Grafico>();

            for (int u = 1; u <= mc.NumeroSimulaciones; u++)
            {
                List<Parametro> Parametrossensibles = new List<Parametro>();

                foreach (Elemento e in proy.Elementos)
                {
                    foreach (Parametro p in e.Parametros.Where(o=>o.Sensible==true))
                    {
                        //Actualizo modelos de simulacion 
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
                        if (p.poisson.IsEliminado == false)
                        {
                            MaestroSimulacion maestro = new MaestroSimulacion();
                            maestro.poisson = p.poisson;
                            maestro.ActualizarCeldas("Poisson", p);
                            p.CeldasSensibles = maestro.CeldasSensibles;
                        }
                        Parametrossensibles.Add(new Parametro
                        {
                            Nombre = p.Nombre,
                            Referencia = p.Referencia,
                            IdElemento = p.IdElemento,
                            Elemento = p.Elemento,
                            IdTipoParametro = p.IdTipoParametro,
                            TipoParametro = p.TipoParametro,
                            Constante = p.Constante,
                            Sensible = p.Sensible,
                            CeldasSensibles = p.CeldasSensibles,
                            Celdas = p.Celdas,
                            normal = p.normal,
                            uniforme = p.uniforme,
                            binomial = p.binomial,
                            geometrica = p.geometrica,
                            hipergeometrica = p.hipergeometrica,
                            pascal = p.pascal,
                            poisson = p.poisson
                        });
                       // Alacena(p);

                    }
                    foreach (Parametro p in e.Parametros.Where(o => o.Sensible == false))
                    {
                        Parametrossensibles.Add(new Parametro
                        {
                            Nombre = p.Nombre,
                            Referencia = p.Referencia,
                            IdElemento = p.IdElemento,
                            Elemento = p.Elemento,
                            IdTipoParametro = p.IdTipoParametro,
                            TipoParametro = p.TipoParametro,
                            Constante = p.Constante,
                            Sensible = p.Sensible,
                            CeldasSensibles = p.CeldasSensibles,
                            Celdas = p.Celdas,
                            normal = p.normal,
                            uniforme = p.uniforme,
                            binomial = p.binomial,
                            geometrica = p.geometrica,
                            hipergeometrica = p.hipergeometrica,
                            pascal = p.pascal,
                            poisson = p.poisson
                        });
                        // Alacena(p);

                    }

                }

                //Aca las celdas para los elementos y sus parametros ya estan simuladoas con un modelo
                //Debo Almacenar los resultados que me da Miguel en cada simulacion
                SimAns r = MetodoMiguel(proy,Parametrossensibles);
                vanE.Add(new Hash {Valor=r.VanE});
                vanF.Add(new Hash {Valor=r.VanF});
                tirE.Add(new Hash {Valor=r.TirE});
                tirF.Add(new Hash {Valor=r.TirF});
            }

             

            //ya tengo los valores obtenidos
            //los agrupo en intervalos 

            List<Grafico> GraficoVanE = new List<Grafico>();
            List<Grafico> GraficoVanF = new List<Grafico>();
            List<Grafico> GraficoTirE = new List<Grafico>();
            List<Grafico> GraficoTirF = new List<Grafico>();
            
            int _x_,_x_puls_;
            double _fx_;

            for(int i=1 ;i<=mc.NumeroIntervalos;i++)
            {
                _x_=PuntoIntervalo(vanE.Min(n=>n.Valor), vanE.Max(n=>n.Valor), mc.NumeroIntervalos, i);
                _x_puls_=PuntoIntervalo(vanE.Min(n=>n.Valor), vanE.Max(n=>n.Valor), mc.NumeroIntervalos, i+1);
                _fx_=vanE.Where(n=>(n.Valor>=_x_ && n.Valor<_x_puls_)).Count();
               
                GraficoVanE.Add(new Grafico
                {
                    x = _x_,
                    fx =_fx_,
                    sx =_x_.ToString(),
                    sfx=_fx_.ToString()
                });

                _x_ = PuntoIntervalo(vanF.Min(n => n.Valor), vanF.Max(n => n.Valor), mc.NumeroIntervalos, i);
                _x_puls_ = PuntoIntervalo(vanF.Min(n => n.Valor), vanF.Max(n => n.Valor), mc.NumeroIntervalos, i + 1);
                _fx_ = vanF.Where(n => (n.Valor >= _x_ && n.Valor < _x_puls_)).Count();

                GraficoVanF.Add(new Grafico
                {
                    x = _x_,
                    fx = _fx_,
                    sx = _x_.ToString(),
                    sfx = _fx_.ToString()
                });

                _x_ = PuntoIntervalo(tirE.Min(n => n.Valor), tirE.Max(n => n.Valor), mc.NumeroIntervalos, i);
                _x_puls_ = PuntoIntervalo(tirE.Min(n => n.Valor), tirE.Max(n => n.Valor), mc.NumeroIntervalos, i + 1);
                _fx_ = tirE.Where(n => (n.Valor >= _x_ && n.Valor < _x_puls_)).Count();

                GraficoTirE.Add(new Grafico
                {
                    x = _x_,
                    fx = _fx_,
                    sx = _x_.ToString(),
                    sfx = _fx_.ToString()
                });

                _x_ = PuntoIntervalo(tirF.Min(n => n.Valor), tirF.Max(n => n.Valor), mc.NumeroIntervalos, i);
                _x_puls_ = PuntoIntervalo(tirF.Min(n => n.Valor), tirF.Max(n => n.Valor), mc.NumeroIntervalos, i + 1);
                _fx_ = tirF.Where(n => (n.Valor >= _x_ && n.Valor < _x_puls_)).Count();

                GraficoTirF.Add(new Grafico
                {
                    x = _x_,
                    fx = _fx_,
                    sx = _x_.ToString(),
                    sfx = _fx_.ToString()
                });

            }


            mc.VanEconomico = GraficoVanE;
            mc.VanFinanciero = GraficoVanF;
            mc.TirEconomico = GraficoTirE;
            mc.TirFinanciero = GraficoTirF;

            mc.MaxVanEconomico = vanE.Max(n => n.Valor);
            mc.MaxVanFinanciero = vanF.Max(n => n.Valor);
            mc.MaxTirEconomico = tirE.Max(n => n.Valor);
            mc.MaxTirFinanciero = tirF.Max(n => n.Valor);

            mc.MinVanEconomico = vanE.Min(n => n.Valor);
            mc.MinVanFinanciero = vanF.Min(n => n.Valor);
            mc.MinTirEconomico = tirE.Min(n => n.Valor);
            mc.MinTirFinanciero = tirF.Min(n => n.Valor);

            Session["_GraficoVanInversionista"] = mc.VanEconomico;
            Session["_GraficoVanProyecto"] = mc.VanFinanciero;
            Session["_GraficoTirProyecto"] = mc.TirEconomico;
            Session["_GraficoTirInversionista"] = mc.TirFinanciero;

            return RedirectToAction("Resultados",mc);
        }

        public SimAns MetodoMiguel(Proyecto proy, List<Parametro> parametros)
        {
            context.Configuration.ProxyCreationEnabled = false;

            int horizonte = proy.Horizonte;
            int preoperativos = proy.PeriodosPreOp;
            int cierre = proy.PeriodosCierre;

            var operaciones = context.Operaciones.Where(o => o.IdProyecto == proy.Id).OrderBy(s => s.Secuencia).ToList();
            var formulas = context.Formulas.Include("Elemento").Where(f => f.Elemento.IdProyecto == proy.Id).ToList();
            var tipoformulas = context.TipoFormulas.ToList();

            return ProyectoController.simular(horizonte, preoperativos, cierre, operaciones, parametros, formulas, tipoformulas, true);
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

        private int PuntoIntervalo(double minimo, double maximo, int TotalIntrervalo, int n)
        {
            // n = mc.NumeroIntervalos 
            return Convert.ToInt32( Math.Round(minimo + n * ((maximo - minimo) / TotalIntrervalo),1));
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="p"></param>

        private void Alacena(Parametro p)
        {
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
}

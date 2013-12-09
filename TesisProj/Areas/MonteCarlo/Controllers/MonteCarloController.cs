using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.IridiumTest.Models;
using TesisProj.Areas.Modelo.Controllers;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.MonteCarlo.Models;
using TesisProj.Areas.Simulaciones.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.MonteCarlo.Controllers
{
    public class MonteCarloController : Controller
    {
        //
        // GET: /MonteCarlo/MonteCarlo/
        TProjContext context = new TProjContext();

        [HttpGet]
        public ActionResult Index(int idProyecto)
        {
            Session["idProyecto"] = idProyecto;
            ViewBag.idProyecto = idProyecto;
            MetodoMonteCarlo mc = new MetodoMonteCarlo();
            //mc.Parametros = context.Parametros.Include("Elemento").Include("Celdas").Where(e => e.Elemento.IdProyecto == idProyecto).Where(oo => oo.Sensible == true).ToList();
            return View(mc);
        }

        [HttpPost]
        public ActionResult Index(MetodoMonteCarlo mc, int idProyecto)
        {
            Proyecto proy = context.Proyectos.Find(idProyecto);
            List<Result> vanE = new List<Result>();
            List<Result> vanF = new List<Result>();
            List<Result> tirE = new List<Result>();
            List<Result> tirF = new List<Result>();

            for (int u = 1; u <= mc.NumeroSimulaciones; u++)
            {
                //List<Proyecto> Proyectos = new List<Proyecto>();

                List<Parametro> Parametrossensibles = new List<Parametro>();
                foreach (Elemento e in proy.Elementos)
                {
                    //Proyecto PROYECTOSIMULADO = new Proyecto();
                    //PROYECTOSIMULADO.Nombre = proy.Nombre;
                    //PROYECTOSIMULADO.Elementos = new List<Elemento>();


                    List<Parametro> parametros = e.Parametros.Where(o=>o.Sensible==true).ToList();

                    //Elemento ex = new Elemento();
                    //ex.Id = e.Id;
                    //ex.IdProyecto = e.IdProyecto;
                    //ex.IdTipoElemento = e.IdTipoElemento;
                    //ex.Nombre = e.Nombre;
                    //ex.Parametros = new List<Parametro>();

                    foreach (Parametro p in parametros)
                    {
                        String[] z= p.XML_ModeloAsignado.Split('|');
                        ModeloSimulacion modelo = new ModeloSimulacion(z[0], Convert.ToDouble(z[1]), Convert.ToDouble(z[2]), Convert.ToDouble(z[3]), Convert.ToDouble(z[4]));
                        p.CeldasSensibles = RetornarCeldas(modelo,p.Celdas.Count,p.Celdas[2]);

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
                            XML_ModeloAsignado=p.XML_ModeloAsignado
                        }
                        );
                    }

                    //PROYECTOSIMULADO.Elementos.Add(ex);
                }
                //Aca las celdas para los elementos y sus parametros ya estan simuladoas con un modelo
                //Debo Almacenar los resultados que me da Miguel en cada simulacion
                SimAns r = MetodoMiguel(proy, Parametrossensibles);
                vanE.Add(new Result { ValorObtenidoD = r.VanE});
                vanF.Add(new Result { ValorObtenidoD = r.VanF });
                tirE.Add(new Result { ValorObtenidoD = r.TirE });
                tirF.Add(new Result { ValorObtenidoD = r.TirF });
            }
            //ya tengo los valores obtenidos
            //los agrupo en intervalos 

            List<Graphic> GraficoVanE = new List<Graphic>();
            List<Graphic> GraficoVanF = new List<Graphic>();
            List<Graphic> GraficoTirE = new List<Graphic>();
            List<Graphic> GraficoTirF = new List<Graphic>();

            int _x_, _x_puls_;
            double _fx_;

            for (int i = 1; i <= mc.NumeroIntervalos; i++)
            {
                _x_ = PuntoIntervalo(vanE.Min(u => u.ValorObtenidoD), vanE.Max(u => u.ValorObtenidoD), mc.NumeroIntervalos, i);
                _x_puls_ = PuntoIntervalo(vanE.Min(u => u.ValorObtenidoD), vanE.Max(u => u.ValorObtenidoD), mc.NumeroIntervalos, i + 1);
                _fx_ = vanE.Where(n => (n.ValorObtenidoD >= _x_ && n.ValorObtenidoD < _x_puls_)).Count();

                GraficoVanE.Add(new Graphic { fx = _fx_, N = _x_ });

                _x_ = PuntoIntervalo(vanF.Min(u => u.ValorObtenidoD), vanF.Max(u => u.ValorObtenidoD), mc.NumeroIntervalos, i);
                _x_puls_ = PuntoIntervalo(vanF.Min(u => u.ValorObtenidoD), vanF.Max(u => u.ValorObtenidoD), mc.NumeroIntervalos, i + 1);
                _fx_ = vanF.Where(n => (n.ValorObtenidoD >= _x_ && n.ValorObtenidoD < _x_puls_)).Count();

                GraficoVanF.Add(new Graphic { fx = _fx_, N = _x_ });

                _x_ = PuntoIntervalo(tirE.Min(u => u.ValorObtenidoD), tirE.Max(u => u.ValorObtenidoD), mc.NumeroIntervalos, i);
                _x_puls_ = PuntoIntervalo(tirE.Min(u => u.ValorObtenidoD), tirE.Max(u => u.ValorObtenidoD), mc.NumeroIntervalos, i + 1);
                _fx_ = tirE.Where(n => (n.ValorObtenidoD >= _x_ && n.ValorObtenidoD < _x_puls_)).Count();

                GraficoTirE.Add(new Graphic { fx = _fx_, N = _x_ });

                _x_ = PuntoIntervalo(tirE.Min(u => u.ValorObtenidoD), tirE.Max(u => u.ValorObtenidoD), mc.NumeroIntervalos, i);
                _x_puls_ = PuntoIntervalo(tirE.Min(u => u.ValorObtenidoD), tirE.Max(u => u.ValorObtenidoD), mc.NumeroIntervalos, i + 1);
                _fx_ = tirE.Where(n => (n.ValorObtenidoD >= _x_ && n.ValorObtenidoD < _x_puls_)).Count();

                GraficoTirE.Add(new Graphic { fx = _fx_, N = _x_ });

            }

            mc.VanEconomico = GraficoVanE;
            mc.VanFinanciero = GraficoVanF;
            mc.TirEconomico = GraficoTirE;
            mc.TirFinanciero = GraficoTirF;

            mc.MaxVanEconomico = Math.Round(vanE.Max(n => n.ValorObtenidoD),2);
            mc.MaxVanFinanciero =Math.Round( vanF.Max(n => n.ValorObtenidoD),2);
            mc.MaxTirEconomico = Math.Round(tirE.Max(n => n.ValorObtenidoD),2);
            mc.MaxTirFinanciero =Math.Round( tirF.Max(n => n.ValorObtenidoD),2);

            mc.MinVanEconomico =Math.Round( vanE.Min(n => n.ValorObtenidoD),2);
            mc.MinVanFinanciero =Math.Round( vanF.Min(n => n.ValorObtenidoD),2);
            mc.MinTirEconomico =Math.Round( tirE.Min(n => n.ValorObtenidoD),2);
            mc.MinTirFinanciero = Math.Round(tirF.Min(n => n.ValorObtenidoD), 2);

            Session["_GraficoVanInversionista"] = mc.VanEconomico;
            Session["_GraficoVanProyecto"] = mc.VanFinanciero;
            Session["_GraficoTirProyecto"] = mc.TirEconomico;
            Session["_GraficoTirInversionista"] = mc.TirFinanciero;

            return RedirectToAction("Resultados", mc);

        }
        public ActionResult Resultados(MetodoMonteCarlo salida)
        {
            return View(salida);
        }

        [ChildActionOnly]
        public ActionResult _GraficoVanInversionista()
        {
            return PartialView((List<Graphic>)Session["_GraficoVanInversionista"]);
        }

        [ChildActionOnly]
        public ActionResult _GraficoVanProyecto()
        {
            return PartialView((List<Graphic>)Session["_GraficoVanProyecto"]);
        }

        [ChildActionOnly]
        public ActionResult _GraficoTirProyecto()
        {
            return PartialView((List<Graphic>)Session["_GraficoTirProyecto"]);
        }

        [ChildActionOnly]
        public ActionResult _GraficoTirInversionista()
        {
            return PartialView((List<Graphic>)Session["_GraficoTirInversionista"]);
        }


        private int PuntoIntervalo(double minimo, double maximo, int TotalIntrervalo, int n)
        {
            // n = mc.NumeroIntervalos 
            return Convert.ToInt32(Math.Round(minimo + n * ((maximo - minimo) / TotalIntrervalo), 1));
        }

        private SimAns MetodoMiguel(Proyecto proy, List<Parametro> parametros)
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
        private List<Celda> RetornarCeldas(ModeloSimulacion modelo, int cantidad, Celda celda)
        {
            List<Celda> salida = new List<Celda>();

            if (modelo.beta != null) 
            {
                for (int i = 1; i <= cantidad; i++)
                {
                    Celda c = new Celda();
                    c.IdParametro = celda.IdParametro;
                    c.Parametro = celda.Parametro;
                    c.Periodo = celda.Periodo;
                    c.Valor = Convert.ToDecimal(modelo.beta.Sample());
                    salida.Add(c);
                }
                return salida;
                
            }

            if (modelo.binomial != null) 
            {
                for (int i = 1; i <= cantidad; i++)
                {
                    Celda c = new Celda();
                    c.IdParametro = celda.IdParametro;
                    c.Parametro = celda.Parametro;
                    c.Periodo = celda.Periodo;
                    c.Valor = Convert.ToDecimal(modelo.binomial.Sample());
                    salida.Add(c);
                }
                return salida;
            }

            if (modelo.chicuadrado != null) 
            {
                for (int i = 1; i <= cantidad; i++)
                {
                    Celda c = new Celda();
                    c.IdParametro = celda.IdParametro;
                    c.Parametro = celda.Parametro;
                    c.Periodo = celda.Periodo;
                    c.Valor = Convert.ToDecimal(modelo.chicuadrado.Sample());
                    salida.Add(c);
                }
                return salida;
            }

            if (modelo.exponencial != null) 
            {
                for (int i = 1; i <= cantidad; i++)
                {
                    Celda c = new Celda();
                    c.IdParametro = celda.IdParametro;
                    c.Parametro = celda.Parametro;
                    c.Periodo = celda.Periodo;
                    c.Valor = Convert.ToDecimal(modelo.exponencial.Sample());
                    salida.Add(c);
                }
                return salida;
            }
            
            if (modelo.f != null) 
            {
                for (int i = 1; i <= cantidad; i++)
                {
                    Celda c = new Celda();
                    c.IdParametro = celda.IdParametro;
                    c.Parametro = celda.Parametro;
                    c.Periodo = celda.Periodo;
                    c.Valor = Convert.ToDecimal(modelo.f.Sample());
                    salida.Add(c);
                }
                return salida;
            }

            if (modelo.gamma != null) 
            {
                for (int i = 1; i <= cantidad; i++)
                {
                    Celda c = new Celda();
                    c.IdParametro = celda.IdParametro;
                    c.Parametro = celda.Parametro;
                    c.Periodo = celda.Periodo;
                    c.Valor = Convert.ToDecimal(modelo.gamma.Sample());
                    salida.Add(c);
                }
                return salida;
            }

            if (modelo.geometrica != null) 
            {
                for (int i = 1; i <= cantidad; i++)
                {
                    Celda c = new Celda();
                    c.IdParametro = celda.IdParametro;
                    c.Parametro = celda.Parametro;
                    c.Periodo = celda.Periodo;
                    c.Valor = Convert.ToDecimal(modelo.geometrica.Sample());
                    salida.Add(c);
                }
                return salida;
            }

            if (modelo.hipergeometrica != null) 
            {
                for (int i = 1; i <= cantidad; i++)
                {
                    Celda c = new Celda();
                    c.IdParametro = celda.IdParametro;
                    c.Parametro = celda.Parametro;
                    c.Periodo = celda.Periodo;
                    c.Valor = Convert.ToDecimal(modelo.hipergeometrica.Sample());
                    salida.Add(c);
                }
                return salida;
            }

            if (modelo.normal != null) 
            {
                for (int i = 1; i <= cantidad; i++)
                {
                    Celda c = new Celda();
                    c.IdParametro = celda.IdParametro;
                    c.Parametro = celda.Parametro;
                    c.Periodo = celda.Periodo;
                    c.Valor = Convert.ToDecimal(modelo.normal.Sample());
                    salida.Add(c);
                }
                return salida;
            }

            if (modelo.pareto != null) 
            {
                for (int i = 1; i <= cantidad; i++)
                {
                    Celda c = new Celda();
                    c.IdParametro = celda.IdParametro;
                    c.Parametro = celda.Parametro;
                    c.Periodo = celda.Periodo;
                    c.Valor = Convert.ToDecimal(modelo.pareto.Sample());
                    salida.Add(c);
                }
                return salida;
            }

            if (modelo.poisson != null) 
            {
                for (int i = 1; i <= cantidad; i++)
                {
                    Celda c = new Celda();
                    c.IdParametro = celda.IdParametro;
                    c.Parametro = celda.Parametro;
                    c.Periodo = celda.Periodo;
                    c.Valor = Convert.ToDecimal(modelo.poisson.Sample());
                    salida.Add(c);
                }
                return salida;
            }

            if (modelo.tstudent != null) 
            {
                for (int i = 1; i <= cantidad; i++)
                {
                    Celda c = new Celda();
                    c.IdParametro = celda.IdParametro;
                    c.Parametro = celda.Parametro;
                    c.Periodo = celda.Periodo;
                    c.Valor = Convert.ToDecimal(modelo.tstudent.Sample());
                    salida.Add(c);
                }
                return salida;
            }

            if (modelo.uniformecontinua != null) 
            {
                for (int i = 1; i <= cantidad; i++)
                {
                    Celda c = new Celda();
                    c.IdParametro = celda.IdParametro;
                    c.Parametro = celda.Parametro;
                    c.Periodo = celda.Periodo;
                    c.Valor = Convert.ToDecimal(modelo.uniformecontinua.Sample());
                    salida.Add(c);
                }
                return salida;
            }
            if (modelo.uniformediscreta != null) 
            {
                for (int i = 1; i <= cantidad; i++)
                {
                    Celda c = new Celda();
                    c.IdParametro = celda.IdParametro;
                    c.Parametro = celda.Parametro;
                    c.Periodo = celda.Periodo;
                    c.Valor = Convert.ToDecimal(modelo.uniformediscreta.Sample());
                    salida.Add(c);
                }
                return salida;
            }

            if (modelo.weibull != null) 
            {
                for (int i = 1; i <= cantidad; i++)
                {
                    Celda c = new Celda();
                    c.IdParametro = celda.IdParametro;
                    c.Parametro = celda.Parametro;
                    c.Periodo = celda.Periodo;
                    c.Valor = Convert.ToDecimal(modelo.weibull.Sample());
                    salida.Add(c);
                }
                return salida;
            }
            
            return new List<Celda>();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.IridiumTest.Models;
using TesisProj.Areas.Modelo;
using TesisProj.Areas.Modelo.Controllers;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.MonteCarlo.Models;
using TesisProj.Areas.Simulaciones.Models;
using TesisProj.Models.Storage;
using System.IO;
using System.Data.Entity;
using TesisProj.Areas.Plantilla.Models;

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
            Session["_GraficoVanInversionista"] = null;
            Session["_GraficoVanProyecto"] = null;
            Session["_GraficoTirProyecto"] = null;
            Session["_GraficoTirInversionista"] = null;
            Session["graphicListVanE"] = null;
            Session["graphicListVanF"] = null;

            //mc.Parametros = context.Parametros.Include("Elemento").Include("Celdas").Where(e => e.Elemento.IdProyecto == idProyecto).Where(oo => oo.Sensible == true).ToList();
            return View(mc);
        }

        private List<double> StringToArray(string str)
        {
            List<double> arr = str.Split(',').Select(s => double.Parse(s)).ToList();
            return arr;
        }

        private string ArrayToString(double[] arr)
        {
            string str = String.Join(",", arr.Select(p => p.ToString()).ToArray());
            return str;
        }

        [HttpPost]
        public ActionResult Index(MetodoMonteCarlo mc, int idProyecto)
        {
            List<Graphic> GraficoVanE = new List<Graphic>();
            List<Graphic> GraficoVanF = new List<Graphic>();
            List<Graphic> GraficoTirE = new List<Graphic>();
            List<Graphic> GraficoTirF = new List<Graphic>();

            List<Result> vanE = new List<Result>();
            List<Result> vanF = new List<Result>();
            List<Result> tirE = new List<Result>();
            List<Result> tirF = new List<Result>();

            Proyecto proyecto = context.Proyectos.Find(idProyecto);
            int horizonte = proyecto.Horizonte;
            int preoperativos = proyecto.PeriodosPreOp;
            int cierre = proyecto.PeriodosCierre;

            //  Sólo operaciones que van a la simulación. Elementos se filtrarán más adelante
            var operaciones = context.Operaciones.Where(o => o.IdProyecto == idProyecto && o.Simular).ToList();
            var elementos = context.Elementos.Include(f => f.Formulas).Include(f => f.Parametros).Include("Parametros.Celdas").Where(e => e.IdProyecto == idProyecto).ToList();
            var tipoformulas = context.TipoFormulas.ToList();
            
            //  Sólo precalcular operaciones NO sensibles
            foreach (Operacion operacion in operaciones)
            {
                if (!operacion.Sensible) operacion.Valores = StringToArray(operacion.strValores);
            }

            foreach (TipoFormula tipoformula in tipoformulas)
            {
                tipoformula.Valores = new double[horizonte];
                tipoformula.ValoresInvariante = new double[horizonte];
                Array.Clear(tipoformula.ValoresInvariante, 0, horizonte);
            }


            //  Sólo precalcular fórmulas NO sensibles y asignar modelos de distribución a parámetros sensibles
            foreach (Elemento elemento in elementos)
            {
                foreach (Formula formula in elemento.Formulas.Where(f => !f.Sensible))
                {
                        formula.Valores = StringToArray(formula.strValores);
                        TipoFormula tipoformula = tipoformulas.First(t => t.Id == formula.IdTipoFormula);
                        tipoformula.ValoresInvariante = tipoformula.ValoresInvariante.Zip(formula.Valores, (x, y) => x + y).ToArray();
                }

                foreach (Parametro parametro in elemento.Parametros.Where(p => p.Sensible))
                {
                    String[] z = parametro.XML_ModeloAsignado.Split('|');
                    List<ListField> lista = context.ListFields.Where(pe => pe.Modelo == z[0]).ToList();
                    parametro.Modelo = new ModeloSimulacion(z[0], Convert.ToDouble(z[1]), Convert.ToDouble(z[2]), Convert.ToDouble(z[3]), Convert.ToDouble(z[4]), lista);
                }

                //  Filtrar parámetros y fórmulas que no van a la simulación
                elemento.Formulas = elemento.Formulas.Where(f => f.Simular).ToList();
                elemento.Parametros = elemento.Parametros.Where(p => p.Simular).ToList();
            }

            //  Filtrar elementos que no van a la simulación
            elementos = elementos.Where(e => e.Simular).ToList();

            for (int i = 0; i < mc.NumeroSimulaciones; i++)
            {
                // K * #tipoformulas
                foreach (TipoFormula tipoformula in tipoformulas)
                {
                    Array.Clear(tipoformula.Valores, 0, horizonte);
                    tipoformula.Valores = tipoformula.Valores.Zip(tipoformula.ValoresInvariante, (x, y) => x + y).ToArray();
                }

                // K * #elementos * #parametros

                foreach (Elemento elemento in elementos)
                {
                    foreach (Parametro parametro in elemento.Parametros)
                    {
                        if (parametro.Sensible)
                        {
                            //  SENSIBILIZAR CELDAS
                            parametro.CeldasSensibles = RetornarCeldasNEW(parametro.Modelo, horizonte);
                        }
                    }
                }

                // GUARDAR RESULTADOS
                ProyectoLite r = StaticProyecto.SimularProyecto(horizonte, preoperativos, cierre, operaciones, elementos, tipoformulas);
                GraficoVanE.Add(new Graphic { N = i, fx = r.VanE });
                GraficoVanF.Add(new Graphic { N = i, fx = r.VanF });
                GraficoTirE.Add(new Graphic { N = i, fx = r.TirE * 100 });
                GraficoTirF.Add(new Graphic { N = i, fx = r.TirF * 100 });

                vanE.Add(new Result { ValorObtenidoD = r.VanE });
                vanF.Add(new Result { ValorObtenidoD = r.VanF });
                tirE.Add(new Result { ValorObtenidoD = r.TirE * 100 });
                tirF.Add(new Result { ValorObtenidoD = r.TirF * 100 });


                if (r.VanE > 0) mc.probabilidadVanE++;
                if (r.VanF > 0) mc.probabilidadVanF++;

                // E = K * (1 + #elementos * #parametros + #tipoformulas)
            }

            //ya tengo los valores obtenidos
            //los agrupo en intervalos 
            mc.probabilidadVanE = Math.Round((100.0 * mc.probabilidadVanE) / (mc.NumeroSimulaciones * 1.0), 4);
            mc.probabilidadVanF = Math.Round((100.0 * mc.probabilidadVanF) / (mc.NumeroSimulaciones * 1.0), 4);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Nuevo requerimiento de intervalos
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            List<GraphicList> graphicListVanE = new List<GraphicList>();
            List<GraphicList> graphicListVanF = new List<GraphicList>();

            GraficoVanE = GraficoVanE.OrderBy(i => i.fx).ToList();
            GraficoVanF = GraficoVanF.OrderBy(i => i.fx).ToList();
            GraficoTirE = GraficoTirE.OrderBy(i => i.fx).ToList();
            GraficoTirF = GraficoTirF.OrderBy(i => i.fx).ToList();

            double Rango = vanE.Max(i => i.ValorObtenidoD) - vanE.Min(i => i.ValorObtenidoD);
            System.Console.Write(Rango);
            double Amplitud = (Rango) / ((mc.NumeroIntervalos) * 1.0);
            System.Console.Write(Amplitud);
            double minimo = vanE.Min(i => i.ValorObtenidoD);

            double _fx_;
            double _Ax_ = 0;

            for (int u = 1; u <= mc.NumeroIntervalos; u++)
            {
                _fx_ = vanE.Where(n => n.ValorObtenidoD >= minimo + Amplitud * (u - 1) && n.ValorObtenidoD <= minimo + Amplitud * u).Count();
                _Ax_ = _Ax_ + _fx_;
                GraphicList glist = new GraphicList(minimo, minimo + Amplitud * u, (100.0 * _fx_) / (mc.NumeroSimulaciones * 1.0), (100.0 * _Ax_) / (mc.NumeroSimulaciones * 1.0));
                graphicListVanE.Add(glist);
            }

            Rango = vanF.Max(i => i.ValorObtenidoD) - vanF.Min(i => i.ValorObtenidoD);
            Amplitud = (Rango) / ((mc.NumeroIntervalos) * 1.0);
            minimo = vanF.Min(i => i.ValorObtenidoD);
            _Ax_ = 0;

            for (int u = 1; u <= mc.NumeroIntervalos; u++)
            {
                _fx_ = vanF.Where(n => n.ValorObtenidoD >= minimo + Amplitud * (u - 1) && n.ValorObtenidoD <= minimo + Amplitud * u).Count();
                _Ax_ = _Ax_ + _fx_;
                GraphicList glist = new GraphicList(minimo, minimo + Amplitud * u, (100.0 * _fx_) / (mc.NumeroSimulaciones * 1.0), (100.0 * _Ax_) / (mc.NumeroSimulaciones * 1.0));
                graphicListVanF.Add(glist);
            }

            Session["graphicListVanE"] = graphicListVanE;
            Session["graphicListVanF"] = graphicListVanF;

            mc.VanEconomico = GraficoVanE;
            mc.VanFinanciero = GraficoVanF;
            mc.TirEconomico = GraficoTirE;
            mc.TirFinanciero = GraficoTirF;

            mc.MaxVanEconomico = vanE.Max(n => n.ValorObtenidoD);
            mc.MaxVanFinanciero = vanF.Max(n => n.ValorObtenidoD);
            mc.MaxTirEconomico = tirE.Max(n => n.ValorObtenidoD);
            mc.MaxTirFinanciero = tirF.Max(n => n.ValorObtenidoD);

            mc.MinVanEconomico = vanE.Min(n => n.ValorObtenidoD);
            mc.MinVanFinanciero = vanF.Min(n => n.ValorObtenidoD);
            mc.MinTirEconomico = tirE.Min(n => n.ValorObtenidoD);
            mc.MinTirFinanciero = tirF.Min(n => n.ValorObtenidoD);

            mc.VanEconomico = GraficoVanE;
            mc.VanFinanciero = GraficoVanF;
            mc.TirEconomico = GraficoTirE;
            mc.TirFinanciero = GraficoTirF;

            mc.MaxVanEconomico = vanE.Max(n => n.ValorObtenidoD);
            mc.MaxVanFinanciero = vanF.Max(n => n.ValorObtenidoD);
            mc.MaxTirEconomico = tirE.Max(n => n.ValorObtenidoD);
            mc.MaxTirFinanciero = tirF.Max(n => n.ValorObtenidoD);

            mc.MinVanEconomico = vanE.Min(n => n.ValorObtenidoD);
            mc.MinVanFinanciero = vanF.Min(n => n.ValorObtenidoD);
            mc.MinTirEconomico = tirE.Min(n => n.ValorObtenidoD);
            mc.MinTirFinanciero = tirF.Min(n => n.ValorObtenidoD);

            Session["_GraficoVanInversionista"] = mc.VanEconomico;
            Session["_GraficoVanProyecto"] = mc.VanFinanciero;
            Session["_GraficoTirProyecto"] = mc.TirFinanciero;
            Session["_GraficoTirInversionista"] = mc.TirEconomico;

            Session["idProyectop"] = idProyecto;

            return RedirectToAction("Resultados", mc);
        }

        [HttpPost]
        public ActionResult IndexOLD(MetodoMonteCarlo mc, int idProyecto)
        {
            ViewBag.idProyecto = (int)Session["idProyecto"];

            Proyecto proy = context.Proyectos.Find(idProyecto);
            List<Result> vanE = new List<Result>();
            List<Result> vanF = new List<Result>();
            List<Result> tirE = new List<Result>();
            List<Result> tirF = new List<Result>();

            List<Graphic> GraficoVanE = new List<Graphic>();
            List<Graphic> GraficoVanF = new List<Graphic>();
            List<Graphic> GraficoTirE = new List<Graphic>();
            List<Graphic> GraficoTirF = new List<Graphic>();

            for (int u = 1; u <= mc.NumeroSimulaciones; u++)
            {
                List<Parametro> Parametrossensibles = new List<Parametro>();
                foreach (Elemento e in proy.Elementos)
                {
                    foreach (Parametro p in e.Parametros.Where(o => o.Sensible == true))
                    {
                        String[] z= p.XML_ModeloAsignado.Split('|');
                        TProjContext db = new TProjContext();
                        List<ListField> lista = db.ListFields.Where(pe => pe.Modelo == z[0]).ToList();
                        ModeloSimulacion modelo = new ModeloSimulacion(z[0], Convert.ToDouble(z[1]), Convert.ToDouble(z[2]), Convert.ToDouble(z[3]), Convert.ToDouble(z[4]),lista);
                        p.CeldasSensibles = new List<Celda>();
                        p.CeldasSensibles = RetornarCeldas(modelo,p.Celdas.Count,p.Celdas[2]);

                        Parametrossensibles.Add(new Parametro
                        {
                            Nombre = p.Nombre,
                            Referencia = p.Referencia,
                            IdElemento = p.IdElemento,
                            Elemento = p.Elemento,
                            IdTipoDato = p.IdTipoDato,
                            TipoDato = p.TipoDato,
                            Constante = p.Constante,
                            Sensible = p.Sensible,
                            CeldasSensibles = p.CeldasSensibles,
                            Celdas = p.Celdas,
                            XML_ModeloAsignado=p.XML_ModeloAsignado
                        }
                        );
                    }

                    foreach (Parametro p in e.Parametros.Where(o => o.Sensible == false))
                    {
                        Parametrossensibles.Add(new Parametro
                        {
                            Nombre = p.Nombre,
                            Referencia = p.Referencia,
                            IdElemento = p.IdElemento,
                            Elemento = p.Elemento,
                            IdTipoDato = p.IdTipoDato,
                            TipoDato = p.TipoDato,
                            Constante = p.Constante,
                            Sensible = p.Sensible,
                            CeldasSensibles = p.CeldasSensibles,
                            Celdas = p.Celdas,
                            XML_ModeloAsignado = p.XML_ModeloAsignado
                        }
                        );
                    }

                    //PROYECTOSIMULADO.Elementos.Add(ex);
                }
                //Aca las celdas para los elementos y sus parametros ya estan simuladoas con un modelo
                //Debo Almacenar los resultados que me da Miguel en cada simulacion
                //ProyectoLite r = MetodoMiguel(proy, Parametrossensibles);
                ProyectoLite r = MetodoMiguel(proy, proy.Elementos);
                GraficoVanE.Add(new Graphic { N = u, fx = r.VanE });
                GraficoVanF.Add(new Graphic { N = u, fx = r.VanF });
                GraficoTirE.Add(new Graphic { N = u, fx = r.TirE * 100 });
                GraficoTirF.Add(new Graphic { N = u, fx = r.TirF * 100 });

                vanE.Add(new Result { ValorObtenidoD = r.VanE});
                vanF.Add(new Result { ValorObtenidoD = r.VanF });
                tirE.Add(new Result { ValorObtenidoD = r.TirE * 100 });
                tirF.Add(new Result { ValorObtenidoD = r.TirF * 100 });


                if (r.VanE > 0) mc.probabilidadVanE++;
                if (r.VanF > 0) mc.probabilidadVanF++;
            }

            //ya tengo los valores obtenidos
            //los agrupo en intervalos 
            mc.probabilidadVanE = Math.Round((100.0*mc.probabilidadVanE) / (mc.NumeroSimulaciones * 1.0), 4);
            mc.probabilidadVanF = Math.Round((100.0*mc.probabilidadVanF) / (mc.NumeroSimulaciones * 1.0), 4);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Nuevo requerimiento de intervalos
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            List<GraphicList> graphicListVanE = new List<GraphicList>();
            List<GraphicList> graphicListVanF = new List<GraphicList>();
            //List<GraphicList> GraficoVanE = new List<Graphic>();
            //List<GraphicList> GraficoVanF = new List<Graphic>();
            //List<GraphicList> GraficoTirE = new List<Graphic>();
            //List<GraphicList> GraficoTirF = new List<Graphic>();

            GraficoVanE = GraficoVanE.OrderBy(i => i.fx).ToList();
            GraficoVanF = GraficoVanF.OrderBy(i => i.fx).ToList();
            GraficoTirE = GraficoTirE.OrderBy(i => i.fx).ToList();
            GraficoTirF = GraficoTirF.OrderBy(i => i.fx).ToList();

            double Rango = vanE.Max(i => i.ValorObtenidoD) - vanE.Min(i => i.ValorObtenidoD);
            System.Console.Write(Rango);
            double Amplitud = (Rango) / ((mc.NumeroIntervalos) * 1.0);
            System.Console.Write(Amplitud);
            double minimo = vanE.Min(i => i.ValorObtenidoD);

            double _fx_;
            double _Ax_=0;

            for (int u = 1; u <= mc.NumeroIntervalos; u++)
            {
                _fx_ = vanE.Where(n => n.ValorObtenidoD >= minimo + Amplitud * (u - 1) && n.ValorObtenidoD <= minimo + Amplitud * u).Count();
                _Ax_ = _Ax_+_fx_;
                GraphicList glist = new GraphicList(minimo, minimo + Amplitud * u, (100.0 * _fx_) / (mc.NumeroSimulaciones * 1.0), (100.0 * _Ax_) / (mc.NumeroSimulaciones * 1.0));
                graphicListVanE.Add(glist);
            }

            Rango = vanF.Max(i => i.ValorObtenidoD) - vanF.Min(i => i.ValorObtenidoD);
            Amplitud = (Rango) / ((mc.NumeroIntervalos) * 1.0);
            minimo = vanF.Min(i => i.ValorObtenidoD);
            _Ax_ = 0;

            for (int u = 1; u <= mc.NumeroIntervalos; u++)
            {
                _fx_ = vanF.Where(n => n.ValorObtenidoD >= minimo + Amplitud * (u-1) && n.ValorObtenidoD <= minimo + Amplitud * u).Count();
                _Ax_ = _Ax_ + _fx_;
                GraphicList glist = new GraphicList(minimo, minimo + Amplitud * u, (100.0 * _fx_) / (mc.NumeroSimulaciones * 1.0), (100.0 * _Ax_) / (mc.NumeroSimulaciones * 1.0));
                graphicListVanF.Add(glist);
            }

            Session["graphicListVanE"] = graphicListVanE;
            Session["graphicListVanF"] = graphicListVanF;

            //double Rango = vanE.Max(i => i.ValorObtenidoD) - vanE.Min(i => i.ValorObtenidoD);
            //double Amplitud = (Rango) / ((mc.NumeroIntervalos) * 1.0);
            //double minimo = vanE.Min(i => i.ValorObtenidoD);
            //double _fx_;

            //for (int u = 1; u <= mc.NumeroIntervalos; u++)
            //{
            //    _fx_ = vanE.Where(n => n.ValorObtenidoD > minimo && n.ValorObtenidoD <= minimo + Amplitud * u).Count();
            //    GraphicList
            //    graphicList.Add( );
            //}

            //Rango = vanF.Max(i => i.ValorObtenidoD) - vanF.Min(i => i.ValorObtenidoD);
            //Amplitud = (Rango) / ((mc.NumeroIntervalos) * 1.0);
            //minimo = vanF.Min(i => i.ValorObtenidoD);


            //for (int u = 1; u <= mc.NumeroIntervalos; u++)
            //{
            //    _fx_ = vanF.Where(n => n.ValorObtenidoD > minimo && n.ValorObtenidoD <= minimo + Amplitud * u).Average(R => R.ValorObtenidoD);
            //    GraficoVanF.Add(new Graphic { fx = _fx_, N = u });
            //}

            //Rango = tirE.Max(i => i.ValorObtenidoD) - tirE.Min(i => i.ValorObtenidoD);
            //Amplitud = (Rango) / ((mc.NumeroIntervalos) * 1.0);
            //minimo = tirE.Min(i => i.ValorObtenidoD);


            //for (int u = 1; u <= mc.NumeroIntervalos; u++)
            //{
            //    _fx_ = tirE.Where(n => n.ValorObtenidoD > minimo && n.ValorObtenidoD <= minimo + Amplitud * u).Average(R => R.ValorObtenidoD);
            //    GraficoTirE.Add(new Graphic { fx = _fx_, N = u });
            //}

            //Rango = tirF.Max(i => i.ValorObtenidoD) - tirF.Min(i => i.ValorObtenidoD);
            //Amplitud = (Rango) / ((mc.NumeroIntervalos) * 1.0);
            //minimo = tirF.Min(i => i.ValorObtenidoD);


            //for (int u = 1; u <= mc.NumeroIntervalos; u++)
            //{
            //    _fx_ = tirF.Where(n => n.ValorObtenidoD > minimo && n.ValorObtenidoD <= minimo + Amplitud * u).Average(R => R.ValorObtenidoD);
            //    GraficoTirF.Add(new Graphic { fx = _fx_, N = u });
            //}

            mc.VanEconomico = GraficoVanE;
            mc.VanFinanciero = GraficoVanF;
            mc.TirEconomico = GraficoTirE;
            mc.TirFinanciero = GraficoTirF;

            mc.MaxVanEconomico = vanE.Max(n => n.ValorObtenidoD);
            mc.MaxVanFinanciero = vanF.Max(n => n.ValorObtenidoD);
            mc.MaxTirEconomico = tirE.Max(n => n.ValorObtenidoD);
            mc.MaxTirFinanciero = tirF.Max(n => n.ValorObtenidoD);

            mc.MinVanEconomico = vanE.Min(n => n.ValorObtenidoD);
            mc.MinVanFinanciero = vanF.Min(n => n.ValorObtenidoD);
            mc.MinTirEconomico = tirE.Min(n => n.ValorObtenidoD);
            mc.MinTirFinanciero = tirF.Min(n => n.ValorObtenidoD);



            //mc.MaxVanEconomico = Math.Round(vanE.Max(n => n.ValorObtenidoD),2);
            //mc.MaxVanFinanciero =Math.Round( vanF.Max(n => n.ValorObtenidoD),2);
            //mc.MaxTirEconomico = Math.Round(tirE.Max(n => n.ValorObtenidoD),2);
            //mc.MaxTirFinanciero =Math.Round( tirF.Max(n => n.ValorObtenidoD),2);

            //mc.MinVanEconomico =Math.Round( vanE.Min(n => n.ValorObtenidoD),2);
            //mc.MinVanFinanciero =Math.Round( vanF.Min(n => n.ValorObtenidoD),2);
            //mc.MinTirEconomico =Math.Round( tirE.Min(n => n.ValorObtenidoD),2);
            //mc.MinTirFinanciero = Math.Round(tirF.Min(n => n.ValorObtenidoD), 2);

            Session["_GraficoVanInversionista"] = mc.VanEconomico;
            Session["_GraficoVanProyecto"] = mc.VanFinanciero;
            Session["_GraficoTirProyecto"] = mc.TirFinanciero;
            Session["_GraficoTirInversionista"] = mc.TirEconomico;

            Session["idProyectop"] = idProyecto;

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

        [ChildActionOnly]
        public ActionResult _ResumenVANE()
        {
            return PartialView((List<GraphicList>)Session["graphicListVanE"]);
        }

        [ChildActionOnly]
        public ActionResult _ResumenVANF()
        {
            return PartialView((List<GraphicList>)Session["graphicListVanF"]);
        }

        private int PuntoIntervalo(double minimo, double maximo, int TotalIntrervalo, int n)
        {
            // n = mc.NumeroIntervalos 
            return Convert.ToInt32(Math.Round(minimo + n * ((maximo - minimo) / TotalIntrervalo), 1));
        }

        //private ProyectoLite MetodoMiguel(Proyecto proy, List<Parametro> parametros)
        //{
        //    context.Configuration.ProxyCreationEnabled = false;

        //    int horizonte = proy.Horizonte;
        //    int preoperativos = proy.PeriodosPreOp;
        //    int cierre = proy.PeriodosCierre;

        //    var operaciones = context.Operaciones.Where(o => o.IdProyecto == proy.Id).OrderBy(s => s.Secuencia).ToList();
        //    var formulas = context.Formulas.Include("Elemento").Where(f => f.Elemento.IdProyecto == proy.Id).ToList();
        //    var tipoformulas = context.TipoFormulas.ToList();

        //    return ProyectoController.simular(horizonte, preoperativos, cierre, operaciones, parametros, formulas, tipoformulas, true);
        //}

        private ProyectoLite MetodoMiguel(Proyecto proy, List<Elemento> elementos)
        {
            context.Configuration.ProxyCreationEnabled = false;

            int horizonte = proy.Horizonte;
            int preoperativos = proy.PeriodosPreOp;
            int cierre = proy.PeriodosCierre;

            var operaciones = context.Operaciones.Where(o => o.IdProyecto == proy.Id).OrderBy(s => s.Secuencia).ToList();
            //var formulas = context.Formulas.Include("Elemento").Where(f => f.Elemento.IdProyecto == proy.Id).ToList();
            var tipoformulas = context.TipoFormulas.ToList();

            //return ProyectoController.simular(horizonte, preoperativos, cierre, operaciones, parametros, formulas, tipoformulas, true);
            return StaticProyecto.simular(horizonte, preoperativos, cierre, operaciones, elementos, tipoformulas, true);
        }

        public  List<Celda> RetornarCeldas(ModeloSimulacion modelo, int cantidad, Celda celda)
        {
            List<Celda> salida = new List<Celda>();

            if (modelo.beta != null) 
            {
                for (int i = 1; i <= cantidad; i++)
                {
                    Celda c = new Celda();
                    c.IdParametro = celda.IdParametro;
                    c.Parametro = celda.Parametro;
                    c.Periodo = i;
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
                    c.Periodo = i;
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
                    c.Periodo = i;;
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
                    c.Periodo = i;;
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
                    c.Periodo = i;;
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
                    c.Periodo = i;;
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
                    c.Periodo = i;;
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
                    c.Periodo = i;;
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
                    c.Periodo = i;;
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
                    c.Periodo = i;;
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
                    c.Periodo = i;;
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
                    c.Periodo = i;;
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
                    c.Periodo = i;;
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
                    c.Periodo = i;;
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
                    c.Periodo = i;;
                    c.Valor = Convert.ToDecimal(modelo.weibull.Sample());
                    salida.Add(c);
                }
                return salida;
            }
            
            return new List<Celda>();
        }

        public List<Celda> RetornarCeldasNEW(ModeloSimulacion modelo, int cantidad)
        {
            List<Celda> salida = new List<Celda>();

            for (int i = 1; i <= cantidad; i++)
            {
                Celda c = new Celda { Periodo = i };
                salida.Add(c);

                if (modelo.beta != null) 
                {
                    c.Valor = Convert.ToDecimal(modelo.beta.Sample());
                    continue;
                }

                if (modelo.binomial != null)
                {
                    c.Valor = Convert.ToDecimal(modelo.binomial.Sample());
                    continue;
                }
                    
                if (modelo.chicuadrado != null)
                {
                    c.Valor = Convert.ToDecimal(modelo.chicuadrado.Sample());
                    continue;
                }

                if (modelo.exponencial != null)
                {
                    
                        c.Valor = Convert.ToDecimal(modelo.exponencial.Sample());
                        continue;
                }

                if (modelo.f != null)
                {
                    c.Valor = Convert.ToDecimal(modelo.f.Sample());
                    continue;
                }

                if (modelo.gamma != null)
                {
                    c.Valor = Convert.ToDecimal(modelo.gamma.Sample());
                    continue;
                }

                if (modelo.geometrica != null)
                {                      
                    c.Valor = Convert.ToDecimal(modelo.geometrica.Sample());
                    continue;
                }

                if (modelo.hipergeometrica != null)
                {
                    c.Valor = Convert.ToDecimal(modelo.hipergeometrica.Sample());
                    continue;
                }

                if (modelo.normal != null)
                {
                    c.Valor = Convert.ToDecimal(modelo.normal.Sample());
                    continue;
                }

                if (modelo.pareto != null)
                {
                    c.Valor = Convert.ToDecimal(modelo.pareto.Sample());
                    continue;
                }

                if (modelo.poisson != null)
                {
                    c.Valor = Convert.ToDecimal(modelo.poisson.Sample());
                    continue;
                }

                if (modelo.tstudent != null)
                {
                    c.Valor = Convert.ToDecimal(modelo.tstudent.Sample());
                    continue;
                }

                if (modelo.uniformecontinua != null)
                {
                    c.Valor = Convert.ToDecimal(modelo.uniformecontinua.Sample());
                    continue;
                }

                if (modelo.uniformediscreta != null)
                {
                    c.Valor = Convert.ToDecimal(modelo.uniformediscreta.Sample());
                    continue;
                }

                if (modelo.weibull != null)
                {
                    c.Valor = Convert.ToDecimal(modelo.weibull.Sample());
                    continue;
                }
            }

            return salida;
        }

    }
}

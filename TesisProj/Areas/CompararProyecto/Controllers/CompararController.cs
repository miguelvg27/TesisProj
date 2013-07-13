using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using TesisProj.Areas.CompararProyecto.Models;
using TesisProj.Areas.Modelo.Controllers;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Modelos.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.CompararProyecto.Controllers
{
    public class CompararController : Controller
    {
        //
        // GET: /CompararProyecto/Comparar/

        TProjContext context = new TProjContext();

        [HttpGet]
        public ActionResult Index()
        {
            List<Comparar> c = new List<Comparar>();
            List<Proyecto> lista = context.Proyectos.ToList();
            int cont =0;
            int[] i=new int[100];

            foreach (Proyecto p in lista)
            {
                c.Add(new Comparar { proyecto = p, Compara = false, Id = p.Id });
                i[cont]=p.Id;
                cont=+1;
            }

            ViewData["checkedRecords"] =i ;

            return View(c);
        }

        [HttpPost]
        public ActionResult Index(int[] checkedRecords)
        {

            List<Proyecto> lista = new List<Proyecto>();
            foreach (int i in checkedRecords)
            {
                lista.Add(context.Proyectos.Find(i));
            }

            List<Grafico> graficosVanE = new List<Grafico>();
            List<Grafico> graficosVanF = new List<Grafico>();
            List<Grafico> graficosTirE = new List<Grafico>();
            List<Grafico> graficosTirF = new List<Grafico>();

            foreach (Proyecto proyecto in lista)
            {
                int horizonte = proyecto.Horizonte;
                int preoperativos = proyecto.PeriodosPreOp;
                int cierre = proyecto.PeriodosCierre;

                var operaciones = context.Operaciones.Where(o => o.IdProyecto == proyecto.Id).OrderBy(s => s.Secuencia).ToList();
                var formulas = context.Formulas.Include("Elemento").Where(f => f.Elemento.IdProyecto == proyecto.Id).ToList();
                var parametros = context.Parametros.Include("Elemento").Include("Celdas").Where(e => e.Elemento.IdProyecto == proyecto.Id).ToList();
                var tipoformulas = context.TipoFormulas.ToList();
                
                SimAns resultado = ProyectoController.simular(horizonte, preoperativos, cierre, operaciones, parametros, formulas, tipoformulas, false);
		        
                graficosVanE.Add(Asignar(proyecto.Id, resultado.VanE));
                graficosVanF.Add(Asignar(proyecto.Id, resultado.VanF));
                graficosTirF.Add(Asignar(proyecto.Id, resultado.TirF));
                graficosTirE.Add(Asignar(proyecto.Id, resultado.TirE));

            }
            Session["_GraficoVanE"] = graficosVanE;
            Session["_GraficoVanF"] = graficosVanF;
            Session["_GraficoTirE"] = graficosTirE;
            Session["_GraficoTirF"] = graficosTirF;


            checkedRecords = checkedRecords ?? new int[] { 3, 4, 5 };
            ViewData["checkedRecords"] = checkedRecords;

            List<Comparar> c = new List<Comparar>();
            List<Proyecto> lproyecto = context.Proyectos.ToList();

            foreach (Proyecto p in lproyecto)
            {
                c.Add(new Comparar { proyecto = p, Compara = false, Id = p.Id });
            }

            return View(c);
        }

        [ChildActionOnly]
        public ActionResult _GraficoVanE()
        {
            return PartialView((List<Grafico>)Session["_GraficoVanE"]);
        }

        [ChildActionOnly]
        public ActionResult _GraficoVanF()
        {
            return PartialView((List<Grafico>)Session["_GraficoVanF"]);
        }

        [ChildActionOnly]
        public ActionResult _GraficoTirE()
        {
            return PartialView((List<Grafico>)Session["_GraficoTirE"]);
        }

        [ChildActionOnly]
        public ActionResult _GraficoTirF()
        {
            return PartialView((List<Grafico>)Session["_GraficoTirF"]);
        }

        private Grafico Asignar(int x, double fx)
        {
            Grafico gr = new Grafico();
            gr.fx = fx;
            gr.x = x;
            gr.sx = Convert.ToString(x);
            gr.sfx = Convert.ToString(Math.Round(gr.fx, 2));
            return gr;
        }

    }
}

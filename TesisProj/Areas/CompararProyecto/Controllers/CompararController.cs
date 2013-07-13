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

            List<Grafico> graficos = new List<Grafico>();

            foreach (Proyecto proyecto in lista)
            {
                context.Configuration.ProxyCreationEnabled = false;

                int horizonte = proyecto.Horizonte;

                var operaciones = context.Operaciones.Where(o => o.IdProyecto == proyecto.Id).OrderBy(s => s.Secuencia).ToList();
                var formulas = context.Formulas.Include("Elemento").Where(f => f.Elemento.IdProyecto == proyecto.Id).ToList();
                var parametros = context.Parametros.Include("Elemento").Include("Celdas").Where(e => e.Elemento.IdProyecto == proyecto.Id).ToList();
                var tipoformulas = context.TipoFormulas.ToList();

                //SimAns resultado = ProyectoController.CalcularProyecto(horizonte, operaciones, parametros, formulas, tipoformulas);

            }


            checkedRecords = checkedRecords ?? new int[] { 3, 4, 5 };
            ViewData["checkedRecords"] = checkedRecords;
            return View();
        }

        public ActionResult _GraficoVan()
        {
            return View();
        }

        public ActionResult _GraficoTir()
        {
            return View();
        }

        private Grafico Simular(int u, int ajuste)
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

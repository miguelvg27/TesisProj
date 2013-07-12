using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.MonteCarlo.Models;
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
            AlgoritmoMonteCarlo mc = new AlgoritmoMonteCarlo();
            mc.Parametros = context.Parametros.Include("Elemento").Include("Celdas").Include("Normal").Include("Uniforme").Where(e => e.Elemento.IdProyecto == idProyecto).Where(oo=>oo.Sensible==true).ToList();
            List<TipoGrafico> t = new List<TipoGrafico>();
            t.Add(new TipoGrafico { Text = "Columnas", Id = 1 });
            t.Add(new TipoGrafico { Text = "Barras", Id = 2 });
            t.Add(new TipoGrafico { Text = "Lineas", Id = 3 });
            t.Add(new TipoGrafico { Text = "Areas", Id = 4 });
            ViewBag.Tipos = t;
            return View(mc);
        }

        [HttpPost]
        public ActionResult Index(AlgoritmoMonteCarlo retorno, int idProyecto)
        {
            AlgoritmoMonteCarlo mc = new AlgoritmoMonteCarlo();
            mc.Parametros = context.Parametros.Include("Elemento").Include("Celdas").Include("Normal").Include("Uniforme").Where(e => e.Elemento.IdProyecto == idProyecto).Where(oo => oo.Sensible == true).ToList();
            List<TipoGrafico> t = new List<TipoGrafico>();
            t.Add(new TipoGrafico { Text = "Columnas", Id = 1 });
            t.Add(new TipoGrafico { Text = "Barras", Id = 2 });
            t.Add(new TipoGrafico { Text = "Lineas", Id = 3 });
            t.Add(new TipoGrafico { Text = "Areas", Id = 4 });
            ViewBag.Tipos = t;
            return View(mc);
        }

    }
}

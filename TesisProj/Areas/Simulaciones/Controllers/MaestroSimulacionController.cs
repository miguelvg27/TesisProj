using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Modelos.Models;
using TesisProj.Areas.Simulaciones.Models;
using TesisProj.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Simulaciones.Controllers
{
    public class MaestroSimulacionController : Controller
    {
        //
        // GET: /Simulaciones/MaestroSimulacion/
        TProjContext context = new TProjContext();

        public ActionResult Index(int idProyecto)
        {
            ViewBag.ProyectoId = idProyecto;
            var elementos = context.Elementos.Include("Elemento").Include("Celdas").Include("Normal").Include("Uniforme").Include("Poisson").Where(e => e.IdProyecto == idProyecto);
            List<Elemento> salidaElementos = new List<Elemento>();
            bool conf = false;
            foreach(Elemento e in elementos)
            {
                conf = false;
                List<Parametro> salidaParametros = new List<Parametro>();
                foreach (Parametro p in e.Parametros)
                {
                    if (p.Sensible)
                    {
                        conf = true;
                        ModeloSimlacion m = new ModeloSimlacion();
                        if (p.binomial == null) p.binomial = new Binomial();
                        if (p.geometrica == null) p.geometrica = new Geometrica();
                        if (p.hipergeometrica == null) p.hipergeometrica = new Hipergeometrica();
                        if (p.pascal == null) p.pascal = new Pascal();
                        if (p.poisson == null) p.poisson = new Poisson();
                        if (p.uniforme == null) p.uniforme = new Uniforme();
                        if (p.normal == null) p.normal = new Normal();
                        context.Entry(p).State = EntityState.Modified;
                        context.SaveChanges();
                        Parametro aux = new Parametro();
                        aux = p;
                        salidaParametros.Add(aux);
                    }
                }
                if (conf)
                {
                    Elemento elemento = new Elemento();
                    elemento = e;
                    elemento.Parametros = new List<Parametro>();
                    elemento.Parametros.AddRange(salidaParametros);
                    salidaElementos.Add(elemento);
                    conf = false;
                }
            }
            return View(salidaElementos);
        }

    }
}

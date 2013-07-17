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
            var parametros = context.Parametros.Include("Elemento").Include("Celdas").Include("Normal").Include("Uniforme").Include("Poisson").Where(e => e.Elemento.IdProyecto == idProyecto).ToList();
            ViewBag.ProyectoId = idProyecto;
            List<Parametro> salida = new List<Parametro>();

                foreach (Parametro parametro in parametros)
                {
                    if (parametro.Sensible)
                    {
                        ModeloSimlacion m = new ModeloSimlacion();

                        if (parametro.binomial==null) parametro.binomial = new Binomial(); 
                        if (parametro.geometrica==null) parametro.geometrica = new Geometrica();
                        if (parametro.hipergeometrica == null) parametro.hipergeometrica = new Hipergeometrica();
                        if (parametro.pascal==null) parametro.pascal = new Pascal();
                        if (parametro.poisson == null) parametro.poisson = new Poisson();
                        if (parametro.uniforme == null) parametro.uniforme = new Uniforme();
                        if (parametro.normal ==null) parametro.normal = new Normal();
                        context.Entry(parametro).State = EntityState.Modified;
                        context.SaveChanges();
                        Parametro aux =new Parametro();
                        aux = parametro;
                        salida.Add(aux);
                    }
                }
                return View(salida);
        }

    }
}

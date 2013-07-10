using System;
using System.Collections.Generic;
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
            var parametros = context.Parametros.Include("Elemento").Include("Celdas").Where(e => e.Elemento.IdProyecto == idProyecto).ToList();
            ViewBag.ProyectoId = idProyecto;
            List<Parametro> salida = new List<Parametro>();

                foreach (Parametro parametro in parametros)
                {
                    if (parametro.Sensible)
                    {
                        ModeloSimlacion m = new ModeloSimlacion();

                        m.Binomial = new Binomial { Id=1};
                        m.Geometrica = new Geometrica { Id = 2 };
                        m.Hipergeometrica = new Hipergeometrica { Id = 3 };
                        m.Pascal = new Pascal { Id = 4 };
                        m.Poisson = new Poisson { Id = 5 };
                        m.Uniforme = new Uniforme { Id = 6 };
                        m.Normal = new Normal { Id = 7 };
                        parametro.maestrosimulacion = new MaestroSimulacion(m);
                        Parametro aux =new Parametro();
                        aux = parametro;
                        salida.Add(aux);
                    }
                }


                return View(salida);
        }

    }
}

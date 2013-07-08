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
            List<MaestroSimulacion> salida = new List<MaestroSimulacion>();

                foreach (Parametro parametro in parametros)
                {
                    if (parametro.Sensible)
                    {
                        ModeloSimlacion m = new ModeloSimlacion();
                        double mean =parametro.Celdas.Average(e =>Convert.ToDouble(e.Valor));
                        double std =Calculos.DesviacionStandard(parametro.Celdas.Select(e => Convert.ToDouble(e.Valor)).ToList());
                        m.Normal= new Normal(mean,std);
                        MaestroSimulacion aux = new MaestroSimulacion(parametro, m);
                        salida.Add(aux);
                    }
                }
            

            return View(salida);
        }

    }
}

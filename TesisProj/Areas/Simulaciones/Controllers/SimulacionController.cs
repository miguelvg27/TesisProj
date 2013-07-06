using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Simulaciones.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Simulaciones.Controllers
{
    public class SimulacionController : Controller
    {
        //
        // GET: /Simulacion/Simulacion/
        TProjContext context = new TProjContext();


        [HttpGet]
        public ViewResult Index(int id)
        {
            List<Simulacion> salida = new List<Simulacion>();
            Simulacion aux = new Simulacion();
            aux.proyecto = context.TablaProyecto.One(p => p.Id == id);
            salida.Add(aux);
            Session["Modelo"] = null;
            return View(salida);
        }

        public ViewResult Edit()
        {
            return View(context.TablaProyecto.One(p => p.Id == 1));
        }

    }
}

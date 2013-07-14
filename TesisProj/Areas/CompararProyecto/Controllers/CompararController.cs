using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using TesisProj.Areas.CompararProyecto.Models;
using TesisProj.Areas.Modelo.Models;
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
            return View();
        }

        [HttpPost]
        public ActionResult Index(int[] checkedRecords)
        {
            checkedRecords = checkedRecords ?? new int[] { 3, 4, 5 };
            ViewData["checkedRecords"] = checkedRecords;

            List<Comparar> c = new List<Comparar>();
            List<Proyecto> lista = context.Proyectos.ToList();

            foreach (Proyecto p in lista)
            {
                c.Add(new Comparar { proyecto = p, Compara = false, Id = p.Id });
            }

            ViewData["checkedRecords"] = c;
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

    }
}

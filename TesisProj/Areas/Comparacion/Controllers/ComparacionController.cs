using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using TesisProj.Areas.Comparacion.Models;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Comparacion.Controllers
{
    public class ComparacionController : Controller
    {

        TProjContext context = new TProjContext();
        //
        // GET: /Comparacion/Comparacion/

        public ActionResult Index()
        {
            List<Comparar> c = new List<Comparar>();
            List<Proyecto> lista = context.Proyectos.ToList();

            foreach (Proyecto p in lista)
            {
                c.Add(new Comparar{ proyecto=p, Compara=false, Id=p.Id });
            }
            return View(c);
        }
        
        [GridAction]
        public ActionResult _CheckBoxesAjax()
        {
            List<Comparar> c = new List<Comparar>();
            List<Proyecto> lista = context.Proyectos.ToList();
            Random r = new Random();
            foreach (Proyecto p in lista)
            {
                c.Add(new Comparar { proyecto = p, 
                    Compara = false, 
                    Id = p.Id, 
                    VanInversionista=Math.Round(r.NextDouble()*100,2),
                    VanProyecto = Math.Round(r.NextDouble() * 100, 2),
                    TirInversionista = Math.Round(r.NextDouble() * 100, 2),
                    TirProyecto= Math.Round(r.NextDouble() * 100, 2)
                });
            }

            return View(new GridModel(c));
        }

        public ActionResult DisplayCheckedOrders(int[] checkedRecords)
        {
            List<Comparar> c = new List<Comparar>();
            List<Proyecto> lista = context.Proyectos.ToList();
            Random r = new Random();
            foreach (Proyecto p in lista)
            {
                c.Add(new Comparar
                {
                    proyecto = p,
                    Compara = false,
                    Id = p.Id,
                    VanInversionista = Math.Round(r.NextDouble() * 100, 2),
                    VanProyecto = Math.Round(r.NextDouble() * 100, 2),
                    TirInversionista = Math.Round(r.NextDouble() * 100, 2),
                    TirProyecto = Math.Round(r.NextDouble() * 100, 2)
                });
            }
            checkedRecords = checkedRecords ?? new int[] { };
            return PartialView("_Resultados", c.Where(o => checkedRecords.Contains(o.Id)));
        }

    }
}

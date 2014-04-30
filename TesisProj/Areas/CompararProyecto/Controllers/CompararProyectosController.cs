using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TesisProj.Areas.CompararProyecto.Models;
using TesisProj.Areas.IridiumTest.Models;
using TesisProj.Areas.Modelo;
using TesisProj.Areas.Modelo.Controllers;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.CompararProyecto.Controllers
{
    [Authorize(Roles = "nav")]
    public class CompararProyectosController : Controller
    {
        //
        // GET: /CompararProyecto/Comparar/
        private TProjContext context = new TProjContext();

        [HttpGet]
        public ActionResult Index()
        {
            List<ProyectoLite> c = new List<ProyectoLite>();

            int[] i;
            i = new int[100];
            int id=context.UserProfiles.Where(n=>n.UserName==User.Identity.Name).FirstOrDefault().UserId;
            c = StaticProyecto.getProyectoList(context, id);
           // Session["Nombre"] = context.Proyectos.Where(y => y.Id == id).FirstOrDefault().Nombre;
            ViewData["Proyectos"]  = c;
            ViewData["checkedRecords"] = i;
            Session["_GraficoVanF"] = null;
            Session["_GraficoVanE"] = null;
            Session["_GraficoTirE"] = null;
            Session["_GraficoTirF"] = null;

            return View(c);
        }

        [HttpPost]
        public ActionResult Index(int[] checkedRecords)
        {
            int id = context.UserProfiles.Where(n => n.UserName == User.Identity.Name).FirstOrDefault().UserId;
            List<ProyectoLite> listaProyectos = StaticProyecto.getProyectoList(context,id);
            ViewData["Proyectos"] = listaProyectos;

            if (checkedRecords == null)
            {
                ViewData["checkedRecords"] = new int[100];
                Session["_GraficoVanF"] = null;
                Session["_GraficoVanE"] = null;
                Session["_GraficoTirE"] = null;
                Session["_GraficoTirF"] = null;
                return View(new List<ProyectoLite>());
            }

            List<ProyectoLite> lista = new List<ProyectoLite>();
            foreach (int i in checkedRecords)
                foreach (ProyectoLite p in listaProyectos)
                    if (i == p.Id)
                        lista.Add(p);

            List<Graphic> graficosVanE = new List<Graphic>();
            List<Graphic> graficosVanF = new List<Graphic>();
            List<Graphic> graficosTirE = new List<Graphic>();
            List<Graphic> graficosTirF = new List<Graphic>();

            foreach (ProyectoLite proyecto in lista)
            {
                graficosVanE.Add(new Graphic { N = proyecto.Id, fx = proyecto.VanE });
                graficosVanF.Add(new Graphic { N = proyecto.Id, fx = proyecto.VanF });
                graficosTirE.Add(new Graphic { N = proyecto.Id, fx = proyecto.TirE });
                graficosTirF.Add(new Graphic { N = proyecto.Id, fx = proyecto.TirF });
            }

            Session["_GraficoVanE"] = graficosVanE;
            Session["_GraficoVanF"] = graficosVanF;
            Session["_GraficoTirE"] = graficosTirE;
            Session["_GraficoTirF"] = graficosTirF;

            checkedRecords = checkedRecords ?? new int[] { 3, 4, 5 };
            ViewData["checkedRecords"] = checkedRecords;

            return View(new List<ProyectoLite>());
        }

        [ChildActionOnly]
        public ActionResult _GraficoVanE()
        {
            return PartialView((List<Graphic>)Session["_GraficoVanE"]);
        }

        [ChildActionOnly]
        public ActionResult _GraficoVanF()
        {
            return PartialView((List<Graphic>)Session["_GraficoVanF"]);
        }

        [ChildActionOnly]
        public ActionResult _GraficoTirE()
        {
            return PartialView((List<Graphic>)Session["_GraficoTirE"]);
        }

        [ChildActionOnly]
        public ActionResult _GraficoTirF()
        {
            return PartialView((List<Graphic>)Session["_GraficoTirF"]);
        }

        private Graphic Asignar(int x, double fx)
        {
            Graphic gr = new Graphic();
            gr.fx = fx;
            gr.N = x;
            return gr;
        }
    }
}
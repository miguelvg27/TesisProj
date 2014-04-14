using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using TesisProj.Areas.CompararProyecto.Models;
using TesisProj.Areas.IridiumTest.Models;
using TesisProj.Areas.Modelo;
using TesisProj.Areas.Modelo.Controllers;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Seguridad.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.CompararProyecto.Controllers
{
    [Authorize(Roles = "nav")]
    public class CompararVersionesController : Controller
    {
        //
        // GET: /CompararProyecto/CompararVersiones/
        private TProjContext context = new TProjContext();


        [HttpPost]
        public ActionResult _AjaxLoading(string text)
        {
            Thread.Sleep(1000);
            int idUser = context.UserProfiles.First(u => u.UserName == User.Identity.Name).UserId;
            List<ProyectoLite> proyectos = StaticProyecto.getProyectoList(context, idUser);
            return new JsonResult { Data = new SelectList(proyectos.ToList(), "Id", "Nombre") };
        }

        [HttpPost]
        public JsonResult Refresh(int Id)
        {
            Session["Id"] = Id;
            ViewData["Versiones"] = StaticProyecto.getVersionesList(context, Id);
            return Json("Ok", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<ProyectoLite> c = new List<ProyectoLite>();
            //List<Proyecto> proyectos;
            //List<Proyecto> colab;

            //int idUser;
            //int cont = 0;
            int[] i;

            if (Session["Id"] == null)
            {
                i = new int[100];
                ViewData["checkedRecords"] = i;
                return View(c);
            }
            else
            {
                i = new int[100];
                c=StaticProyecto.getVersionesList(context, (int)Session["Id"]);
                ViewData["Versiones"] = c;
                ViewData["checkedRecords"] = i;
                return View(c);
            }
            
            //List<Proyecto> lista = proyectos.Union(colab).ToList();
            //i = new int[100];

            //foreach (Proyecto p in lista)
            //{
            //    c.Add(new Comparar { proyecto = p, Compara = false, Id = p.Id });
            //    i[cont] = p.Id;
            //    cont = +1;
            //}
        }

        [HttpPost]
        public ActionResult Index(int[] checkedRecords)
        {
            if (checkedRecords == null)
            {
                ViewData["checkedRecords"] = new int[100];
                return View(new List<Comparar>());
            }

            List<ProyectoLite> listaversiones = StaticProyecto.getVersionesList(context, (int)Session["Id"]);
            List<ProyectoLite> lista = new List<ProyectoLite>();
            foreach (int i in checkedRecords)
            {
                foreach (ProyectoLite p in listaversiones)
                {
                    if (i == p.Id)
                    {
                        lista.Add(p);
                    }
                }
                
                
            }

            List<Graphic> graficosVanE = new List<Graphic>();
            List<Graphic> graficosVanF = new List<Graphic>();
            List<Graphic> graficosTirE = new List<Graphic>();
            List<Graphic> graficosTirF = new List<Graphic>();

            foreach (ProyectoLite proyecto in lista)
            {
                graficosVanE.Add(new Graphic { N=proyecto.Id, fx=proyecto.VanE  });
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

            //List<Comparar> c = new List<Comparar>();

            //int idUser = context.UserProfiles.First(u => u.UserName == User.Identity.Name).UserId;
            //var proyectos = context.Proyectos.Include(rp => rp.Creador).Where(pr => pr.Creador.UserName.Equals(User.Identity.Name)).ToList();
            //var colab = context.Colaboradores.Include(rc => rc.Proyecto).Where(cr => cr.IdUsuario == idUser).Select(cr => cr.Proyecto).Include(p => p.Creador).ToList();

            //List<Proyecto> lproyecto = proyectos.Union(colab).ToList();
            ////List<Proyecto> lproyecto = context.Proyectos.ToList();

            //foreach (Proyecto p in lproyecto)
            //{
            //    c.Add(new Comparar { proyecto = p, Compara = false, Id = p.Id });
            //}

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

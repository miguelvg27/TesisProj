using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Proyectos.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Proyectos.Controllers
{
    public class Pedro_ProyectoController : Controller
    {
        //
        // GET: /Proyectos/Pedro_Proyecto/

        TProjContext context = new TProjContext();


        [HttpGet]
        public ViewResult Index()
        {
            if (context.TablaProyecto.All().Count == 0)
            {
                Pedro_Proyecto p = new Pedro_Proyecto();
                p.Nombre = "Primer Proyeto";
                p.fecha = DateTime.Now;
                p.parametro = new Pedro_Parametro();
                p.parametro.Nombre = "Capital de trabajo";
                p.parametro.periodos = 11;
                p.parametro.valor = 1000;
                p.parametro.interes = 20;
                Create(p);
            }
            return View(context.TablaProyecto.All());
        }

        [HttpGet]
        public ViewResult Details(int id)
        {
            return View(context.TablaProyecto.One(d => d.Id == id));
        }

        [HttpGet]
        public ActionResult Create()
        {
            Pedro_Proyecto p = new Pedro_Proyecto();
            p.parametro = new Pedro_Parametro();
            return View(p);
        }

        [HttpPost]
        public ActionResult Create(Pedro_Proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                using (TProjContext context = new TProjContext())
                {
                    proyecto.parametro = new Pedro_Parametro(proyecto.parametro.Nombre, proyecto.parametro.valor, proyecto.parametro.periodos, proyecto.parametro.interes);
                    Pedro_Parametro xx = Pedro_Cronograma.GenerarElementos(proyecto.parametro.Nombre, proyecto.parametro.periodos, proyecto.parametro.valor, proyecto.parametro.interes);
                    proyecto.parametro = xx;
                    proyecto.Resultado = proyecto.parametro.Elementos.Sum(o => o.valor) - 2 * proyecto.parametro.valor;
                    context.TablaProyecto.AddElement(proyecto);
                    return RedirectToAction("Index");
                }
            }

            return View(proyecto);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (TProjContext context = new TProjContext())
            {
                return View(context.TablaProyecto.One(d => d.Id == id));
            }
        }

        [HttpPost]
        public ActionResult Edit(Pedro_Proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                using (TProjContext context = new TProjContext())
                {
                    context.TablaProyecto.ModifyElement(proyecto);
                    return RedirectToAction("Index");
                }
            }
            return View(proyecto);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (TProjContext context = new TProjContext())
            {
                return View(context.TablaProyecto.One(d => d.Id == id));
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            using (TProjContext context = new TProjContext())
            {
                context.TablaProyecto.RemoveElementByID(id);
                return RedirectToAction("Index");
            }

        }

    }
}

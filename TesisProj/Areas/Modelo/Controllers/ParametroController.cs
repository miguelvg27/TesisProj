using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Modelo.Models;

namespace TesisProj.Areas.Modelo.Controllers
{
    public partial class ProyectoController : Controller
    {
        //
        // GET: /Modelo/Proyecto/Catalog/5

        public ActionResult Catalog(int id)
        {
            Elemento elemento = db.Elementos.Find(id);
            Proyecto proyecto = db.Proyectos.Find(elemento.IdProyecto);
            if (elemento == null)
            {
                return HttpNotFound();
            }
            var parametros = db.Parametros.Include("Elemento").Include("TipoParametro").Where(p => p.IdElemento == elemento.Id);

            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.ProyectoId = proyecto.Id;

            return View(parametros.ToList());
        }

        //
        // GET: /Modelo/Proyecto/CreateParametro/5

        public ActionResult CreateParametro(int idElemento = 0)
        {
            Elemento elemento = db.Elementos.Find(idElemento);
            if (elemento == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdTipoParametro = new SelectList(db.TipoParametros, "Id", "Nombre");
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(e => e.Id == elemento.Id), "Id", "Nombre", elemento.Id);
            ViewBag.IdElementoReturn = elemento.Id;

            return View();
        }

        //
        // POST: /Modelo/Proyecto/CreateParametro

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateParametro(Parametro parametro)
        {
            if (ModelState.IsValid)
            {
                db.Parametros.Add(parametro);
                db.SaveChanges();

                return RedirectToAction("Journal", new { id = parametro.IdElemento });
            }

            ViewBag.IdTipoParametro = new SelectList(db.TipoParametros, "Id", "Nombre", parametro.IdTipoParametro);
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(e => e.Id == parametro.IdElemento), "Id", "Nombre", parametro.IdElemento);
            ViewBag.IdElementoReturn = parametro.IdElemento;

            return View(parametro);
        }

        //
        // GET: /Modelo/Proyecto/EditParametro/5

        public ActionResult EditParametro(int id = 0)
        {
            Parametro parametro = db.Parametros.Find(id);
            if (parametro == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdTipoParametro = new SelectList(db.TipoParametros, "Id", "Nombre", parametro.IdTipoParametro);
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(e => e.Id == parametro.IdElemento), "Id", "Nombre", parametro.IdElemento);
            ViewBag.IdElementoReturn = parametro.IdElemento;

            return View();
        }

        //
        // POST: /Modelo/Proyecto/EditParametro

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditParametro(Parametro parametro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parametro).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Journal", new { id = parametro.IdElemento });
            }

            ViewBag.IdTipoParametro = new SelectList(db.TipoParametros, "Id", "Nombre", parametro.IdTipoParametro);
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(e => e.Id == parametro.IdElemento), "Id", "Nombre", parametro.IdElemento);
            ViewBag.IdElementoReturn = parametro.IdElemento;

            return View(parametro);
        }

        //
        // GET: /Modelo/Proyecto/DeleteParametro/5

        public ActionResult DeleteParametro(int id = 0)
        {
            Parametro parametro = db.Parametros.Find(id);
            if (parametro == null)
            {
                return HttpNotFound();
            }

            parametro.TipoParametro = db.TipoParametroes.Find(parametro.IdTipoParametro);

            return View(parametro);
        }

        //
        // POST: /Modelo/Proyecto/DeleteParametro/5

        [HttpPost, ActionName("DeleteParametro")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteParametroConfirmed(int id)
        {
            Parametro parametro = db.Parametros.Find(id);
            try
            {
                db.Parametros.Remove(parametro);
                db.SaveChanges();
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No se puede eliminar porque existen registros dependientes.");
                parametro.TipoParametro = db.TipoParametroes.Find(parametro.IdTipoParametro);
                return View("DeleteElemento", parametro);
            }

            return RedirectToAction("Catalog", new { id = parametro.IdElemento });
        }

    }
}

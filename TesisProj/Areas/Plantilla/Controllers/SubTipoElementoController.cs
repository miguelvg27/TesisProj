using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Plantilla.Models;
using TesisProj.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Plantilla.Controllers
{
    public class SubTipoElementoController : Controller
    {
        private TProjContext db = new TProjContext();

        //
        // GET: /Plantilla/SubTipoElemento/

        public ActionResult Index()
        {
            var subtipoelementos = db.SubTipoElementos.Include(s => s.TipoElemento).OrderBy(s => s.TipoElemento.Nombre);
            return View(subtipoelementos.ToList());
        }

        //
        // GET: /Plantilla/SubTipoElemento/Details/5

        public ActionResult Details(int id = 0)
        {
            SubTipoElemento subtipoelemento = db.SubTipoElementos.Find(id);
            subtipoelemento.TipoElemento = db.TipoElementos.Find(subtipoelemento.IdTipoElemento);
            if (subtipoelemento == null)
            {
                return HttpNotFound();
            }
            return View(subtipoelemento);
        }

        //
        // GET: /Plantilla/SubTipoElemento/Create

        public ActionResult Create()
        {
            ViewBag.IdTipoElemento = new SelectList(db.TipoElementos.OrderBy(t => t.Nombre), "Id", "Nombre");
            return View();
        }

        //
        // POST: /Plantilla/SubTipoElemento/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SubTipoElemento subtipoelemento)
        {
            if (ModelState.IsValid)
            {
                db.SubTipoElementos.Add(subtipoelemento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdTipoElemento = new SelectList(db.TipoElementos.OrderBy(t => t.Nombre), "Id", "Nombre", subtipoelemento.IdTipoElemento);
            return View(subtipoelemento);
        }

        //
        // GET: /Plantilla/SubTipoElemento/Edit/5

        public ActionResult Edit(int id = 0)
        {
            SubTipoElemento subtipoelemento = db.SubTipoElementos.Find(id);
            if (subtipoelemento == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdTipoElemento = new SelectList(db.TipoElementos.OrderBy(t => t.Nombre), "Id", "Nombre", subtipoelemento.IdTipoElemento);
            return View(subtipoelemento);
        }

        //
        // POST: /Plantilla/SubTipoElemento/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SubTipoElemento subtipoelemento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subtipoelemento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdTipoElemento = new SelectList(db.TipoElementos.OrderBy(t => t.Nombre), "Id", "Nombre", subtipoelemento.IdTipoElemento);
            return View(subtipoelemento);
        }

        //
        // GET: /Plantilla/SubTipoElemento/Delete/5

        public ActionResult Delete(int id = 0)
        {
            SubTipoElemento subtipoelemento = db.SubTipoElementos.Find(id);
            subtipoelemento.TipoElemento = db.TipoElementos.Find(subtipoelemento.IdTipoElemento);
            if (subtipoelemento == null)
            {
                return HttpNotFound();
            }
            return View(subtipoelemento);
        }

        //
        // POST: /Plantilla/SubTipoElemento/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubTipoElemento subtipoelemento = db.SubTipoElementos.Find(id);
            db.SubTipoElementos.Remove(subtipoelemento);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
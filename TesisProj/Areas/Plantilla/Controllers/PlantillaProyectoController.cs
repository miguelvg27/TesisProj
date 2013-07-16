using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Plantilla.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Plantilla.Controllers
{
    [Authorize(Roles = "admin")]
    public class PlantillaProyectoController : Controller
    {
        private TProjContext db = new TProjContext();

        //
        // GET: /Plantilla/PlantillaProyecto/

        public ActionResult Index()
        {
            return View(db.PlantillaProyectos.OrderBy(t => t.Nombre).ToList());
        }

        //
        // GET: /Plantilla/PlantillaProyecto/Details/5

        public ActionResult Details(int id = 0)
        {
            PlantillaProyecto plantillaproyecto = db.PlantillaProyectos.Find(id);
            if (plantillaproyecto == null)
            {
                return HttpNotFound();
            }

            return View(plantillaproyecto);
        }

        //
        // GET: /Plantilla/PlantillaProyecto/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Plantilla/PlantillaProyecto/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlantillaProyecto plantillaproyecto)
        {
            if (ModelState.IsValid)
            {
                db.PlantillaProyectos.Add(plantillaproyecto);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(plantillaproyecto);
        }

        //
        // GET: /Plantilla/PlantillaProyecto/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PlantillaProyecto plantillaproyecto = db.PlantillaProyectos.Find(id);
            if (plantillaproyecto == null)
            {
                return HttpNotFound();
            }

            return View(plantillaproyecto);
        }

        //
        // POST: /Plantilla/PlantillaProyecto/Assoc/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlantillaProyecto plantillaproyecto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(plantillaproyecto).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(plantillaproyecto);
        }

        //
        // GET: /Plantilla/PlantillaProyecto/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PlantillaProyecto plantillaproyecto = db.PlantillaProyectos.Find(id);
            if (plantillaproyecto == null)
            {
                return HttpNotFound();
            }

            return View(plantillaproyecto);
        }

        //
        // POST: /Plantilla/PlantillaProyecto/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlantillaProyecto plantillaproyecto = db.PlantillaProyectos.Find(id);
            try
            {
                db.PlantillaProyectos.Remove(plantillaproyecto);
                db.SaveChanges();
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No se puede eliminar porque existen registros dependientes.");
                return View("Delete", plantillaproyecto);
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
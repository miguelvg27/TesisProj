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
    [Authorize]
    public class PlantillaSalidaProyectoController : Controller
    {
        private TProjContext db = new TProjContext();

        //
        // GET: /Plantilla/PlantillaSalidaProyecto/

        public ActionResult Index(int id = 0)
        {
            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(id);
            if (plantilla == null)
            {
                return HttpNotFound();
            }

            var salidaproyectos = db.PlantillaSalidaProyectos.Include(s => s.PlantillaProyecto).OrderBy(s => s.Secuencia);

            ViewBag.IdPlantilla = id;
            ViewBag.Plantilla = plantilla.Nombre;

            return View(salidaproyectos.ToList());
        }

        //
        // GET: /Plantilla/PlantillaSalidaProyecto/Details/5

        public ActionResult Details(int id = 0)
        {
            PlantillaSalidaProyecto salidaproyecto = db.PlantillaSalidaProyectos.Find(id);
            if (salidaproyecto == null)
            {
                return HttpNotFound();
            }

            salidaproyecto.PlantillaProyecto = db.PlantillaProyectos.Find(salidaproyecto.IdPlantillaProyecto);
            
            return View(salidaproyecto);
        }

        //
        // GET: /Plantilla/PlantillaSalidaProyecto/Create

        public ActionResult Create(int idPlantilla = 0)
        {
            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(idPlantilla);
            if (plantilla == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdPlantilla = idPlantilla;
            ViewBag.IdPlantillaProyecto = new SelectList(db.PlantillaProyectos.Where(p => p.Id == plantilla.Id), "Id", "Nombre");

            var salidas = db.PlantillaSalidaProyectos.Where(f => f.IdPlantillaProyecto == plantilla.Id);
            int defSecuencia = salidas.Count() > 0 ? salidas.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;

            return View();
        }

        //
        // POST: /Plantilla/PlantillaSalidaProyecto/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlantillaSalidaProyecto salidaproyecto)
        {
            if (ModelState.IsValid)
            {
                db.PlantillaSalidaProyectos.Add(salidaproyecto);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = salidaproyecto.IdPlantillaProyecto });
            }

            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(salidaproyecto.IdPlantillaProyecto);
            ViewBag.IdPlantilla = plantilla.Id;
            ViewBag.IdPlantillaProyecto = new SelectList(db.PlantillaProyectos.Where(p => p.Id == plantilla.Id), "Id", "Nombre", salidaproyecto.IdPlantillaProyecto);

            var salidas = db.PlantillaSalidaProyectos.Where(f => f.IdPlantillaProyecto == plantilla.Id);
            int defSecuencia = salidas.Count() > 0 ? salidas.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;
            
            return View(salidaproyecto);
        }

        //
        // GET: /Plantilla/PlantillaSalidaProyecto/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PlantillaSalidaProyecto salidaproyecto = db.PlantillaSalidaProyectos.Find(id);
            if (salidaproyecto == null)
            {
                return HttpNotFound();
            }

            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(salidaproyecto.IdPlantillaProyecto);
            ViewBag.IdPlantillaProyecto = new SelectList(db.PlantillaProyectos.Where(p => p.Id == plantilla.Id), "Id", "Nombre", salidaproyecto.IdPlantillaProyecto);

            return View(salidaproyecto);
        }

        //
        // POST: /Plantilla/PlantillaSalidaProyecto/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlantillaSalidaProyecto salidaproyecto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salidaproyecto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = salidaproyecto.IdPlantillaProyecto });
            }
            
            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(salidaproyecto.IdPlantillaProyecto);
            ViewBag.IdPlantillaProyecto = new SelectList(db.PlantillaProyectos.Where(p => p.Id == plantilla.Id), "Id", "Nombre", salidaproyecto.IdPlantillaProyecto);
            
            return View(salidaproyecto);
        }

        //
        // GET: /Plantilla/PlantillaSalidaProyecto/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PlantillaSalidaProyecto salidaproyecto = db.PlantillaSalidaProyectos.Find(id);
            if (salidaproyecto == null)
            {
                return HttpNotFound();
            }

            salidaproyecto.PlantillaProyecto = db.PlantillaProyectos.Find(salidaproyecto.IdPlantillaProyecto);

            return View(salidaproyecto);
        }

        //
        // POST: /Plantilla/PlantillaSalidaProyecto/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlantillaSalidaProyecto salidaproyecto = db.PlantillaSalidaProyectos.Find(id);
            try
            {
                db.PlantillaSalidaProyectos.Remove(salidaproyecto);
                db.SaveChanges();
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No se puede eliminar porque existen registros dependientes.");
                salidaproyecto.PlantillaProyecto = db.PlantillaProyectos.Find(salidaproyecto.IdPlantillaProyecto);
                return View("Delete", salidaproyecto);
            }
            
            return RedirectToAction("Index", new { id = salidaproyecto.IdPlantillaProyecto });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
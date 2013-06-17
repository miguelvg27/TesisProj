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
    public class SalidaProyectoController : Controller
    {
        private TProjContext db = new TProjContext();

        //
        // GET: /Plantilla/SalidaProyecto/

        public ActionResult Index(int id = 0)
        {
            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(id);
            if (plantilla == null)
            {
                return HttpNotFound();
            }

            var salidaproyectos = db.SalidaProyectos.Include(s => s.PlantillaProyecto).OrderBy(s => s.Secuencia);

            ViewBag.IdPlantilla = id;
            ViewBag.Plantilla = plantilla.Nombre;

            return View(salidaproyectos.ToList());
        }

        //
        // GET: /Plantilla/SalidaProyecto/Details/5

        public ActionResult Details(int id = 0)
        {
            SalidaProyecto salidaproyecto = db.SalidaProyectos.Find(id);
            if (salidaproyecto == null)
            {
                return HttpNotFound();
            }

            salidaproyecto.PlantillaProyecto = db.PlantillaProyectos.Find(salidaproyecto.IdPlantillaProyecto);
            
            return View(salidaproyecto);
        }

        //
        // GET: /Plantilla/SalidaProyecto/Create

        public ActionResult Create(int idPlantilla = 0)
        {
            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(idPlantilla);
            if (plantilla == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdPlantilla = idPlantilla;
            ViewBag.IdPlantillaProyecto = new SelectList(db.PlantillaElementos.Where(p => p.Id == plantilla.Id), "Id", "Nombre");

            var salidas = db.SalidaProyectos.Where(f => f.IdPlantillaProyecto == plantilla.Id);
            int defSecuencia = salidas.Count() > 0 ? salidas.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;

            return View();
        }

        //
        // POST: /Plantilla/SalidaProyecto/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SalidaProyecto salidaproyecto)
        {
            if (ModelState.IsValid)
            {
                db.SalidaProyectos.Add(salidaproyecto);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = salidaproyecto.IdPlantillaProyecto });
            }

            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(salidaproyecto.IdPlantillaProyecto);
            ViewBag.IdPlantilla = plantilla.Id;
            ViewBag.IdPlantillaProyecto = new SelectList(db.PlantillaElementos.Where(p => p.Id == plantilla.Id), "Id", "Nombre");

            var salidas = db.SalidaProyectos.Where(f => f.IdPlantillaProyecto == plantilla.Id);
            int defSecuencia = salidas.Count() > 0 ? salidas.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;
            
            return View(salidaproyecto);
        }

        //
        // GET: /Plantilla/SalidaProyecto/Edit/5

        public ActionResult Edit(int id = 0)
        {
            SalidaProyecto salidaproyecto = db.SalidaProyectos.Find(id);
            if (salidaproyecto == null)
            {
                return HttpNotFound();
            }

            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(salidaproyecto.IdPlantillaProyecto);
            ViewBag.IdPlantillaProyecto = new SelectList(db.PlantillaElementos.Where(p => p.Id == plantilla.Id), "Id", "Nombre", salidaproyecto.IdPlantillaProyecto);

            return View(salidaproyecto);
        }

        //
        // POST: /Plantilla/SalidaProyecto/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SalidaProyecto salidaproyecto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salidaproyecto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = salidaproyecto.IdPlantillaProyecto });
            }
            
            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(salidaproyecto.IdPlantillaProyecto);
            ViewBag.IdPlantillaProyecto = new SelectList(db.PlantillaElementos.Where(p => p.Id == plantilla.Id), "Id", "Nombre", salidaproyecto.IdPlantillaProyecto);
            
            return View(salidaproyecto);
        }

        //
        // GET: /Plantilla/SalidaProyecto/Delete/5

        public ActionResult Delete(int id = 0)
        {
            SalidaProyecto salidaproyecto = db.SalidaProyectos.Find(id);
            if (salidaproyecto == null)
            {
                return HttpNotFound();
            }

            salidaproyecto.PlantillaProyecto = db.PlantillaProyectos.Find(salidaproyecto.IdPlantillaProyecto);

            return View(salidaproyecto);
        }

        //
        // POST: /Plantilla/SalidaProyecto/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SalidaProyecto salidaproyecto = db.SalidaProyectos.Find(id);
            db.SalidaProyectos.Remove(salidaproyecto);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = salidaproyecto.IdPlantillaProyecto });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
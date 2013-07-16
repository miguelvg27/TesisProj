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
    [Authorize(Roles = "admin")]
    public class PlantillaOperacionController : Controller
    {
        private TProjContext db = new TProjContext();

        //
        // GET: /Plantilla/PlantillaOperacion/

        public ActionResult Index(int id = 0)
        {
            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(id);
            if (plantilla == null)
            {
                return HttpNotFound();
            }

            var plantillaoperacions = db.PlantillaOperaciones.Include(o => o.PlantillaProyecto).Where(o => o.IdPlantillaProyecto == plantilla.Id).OrderBy(o => o.Secuencia);

            ViewBag.IdPlantilla = id;
            ViewBag.Plantilla = plantilla.Nombre;

            return View(plantillaoperacions.ToList());
        }

        //
        // GET: /Plantilla/PlantillaOperacion/Details/5

        public ActionResult Details(int id = 0)
        {
            PlantillaOperacion plantillaoperacion = db.PlantillaOperaciones.Find(id);
            if (plantillaoperacion == null)
            {
                return HttpNotFound();
            }

            plantillaoperacion.PlantillaProyecto = db.PlantillaProyectos.Find(plantillaoperacion.IdPlantillaProyecto);

            return View(plantillaoperacion);
        }

        //
        // GET: /Plantilla/PlantillaOperacion/Create

        public ActionResult Create(int idPlantilla = 0)
        {
            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(idPlantilla);
            if (plantilla == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdPlantilla = idPlantilla;
            ViewBag.IdPlantillaProyecto = new SelectList(db.PlantillaProyectos.Where(p => p.Id == plantilla.Id), "Id", "Nombre");

            ViewBag.Plantilla = plantilla.Nombre;

            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.OperacionesGlobales, "Value", "Text");
            ViewBag.ListTipos = new SelectList(db.TipoFormulas.OrderBy(o => o.Nombre).ToList(), "Referencia", "Nombre");
            ViewBag.ListOperaciones = new SelectList(db.PlantillaOperaciones.Where(f => f.IdPlantillaProyecto == idPlantilla).OrderBy(f => f.Nombre).ToList(), "Referencia", "Nombre");

            var operaciones = db.PlantillaOperaciones.Where(f => f.IdPlantillaProyecto == plantilla.Id);
            int defSecuencia = operaciones.Count() > 0 ? operaciones.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;

            return View();
        }

        //
        // POST: /Plantilla/PlantillaOperacion/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlantillaOperacion plantillaoperacion)
        {
            if (ModelState.IsValid)
            {
                db.PlantillaOperaciones.Add(plantillaoperacion);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = plantillaoperacion.IdPlantillaProyecto });
            }

            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(plantillaoperacion.IdPlantillaProyecto);
            ViewBag.IdPlantilla = plantilla.Id;
            ViewBag.IdPlantillaProyecto = new SelectList(db.PlantillaProyectos.Where(p => p.Id == plantilla.Id), "Id", "Nombre", plantillaoperacion.IdPlantillaProyecto);

            ViewBag.Plantilla = plantilla.Nombre;

            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.OperacionesGlobales, "Value", "Text");
            ViewBag.ListTipos = new SelectList(db.TipoFormulas.OrderBy(o => o.Nombre).ToList(), "Referencia", "Nombre");
            ViewBag.ListOperaciones = new SelectList(db.PlantillaOperaciones.Where(f => f.IdPlantillaProyecto == plantillaoperacion.IdPlantillaProyecto).OrderBy(f => f.Nombre).ToList(), "Referencia", "Nombre");

            var operacion = db.PlantillaOperaciones.Where(f => f.IdPlantillaProyecto == plantilla.Id);
            int defSecuencia = operacion.Count() > 0 ? operacion.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;

            return View(plantillaoperacion);
        }

        //
        // GET: /Plantilla/PlantillaOperacion/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PlantillaOperacion plantillaoperacion = db.PlantillaOperaciones.Find(id);
            if (plantillaoperacion == null)
            {
                return HttpNotFound();
            }

            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(plantillaoperacion.IdPlantillaProyecto);
            ViewBag.IdPlantillaProyecto = new SelectList(db.PlantillaProyectos.Where(p => p.Id == plantilla.Id), "Id", "Nombre", plantillaoperacion.IdPlantillaProyecto);

            ViewBag.Plantilla = db.PlantillaProyectos.Find(plantillaoperacion.IdPlantillaProyecto).Nombre;

            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.OperacionesGlobales, "Value", "Text");
            ViewBag.ListTipos = new SelectList(db.TipoFormulas.OrderBy(o => o.Nombre).ToList(), "Referencia", "Nombre");
            ViewBag.ListOperaciones = new SelectList(db.PlantillaOperaciones.Where(f => f.IdPlantillaProyecto == plantillaoperacion.IdPlantillaProyecto).OrderBy(f => f.Nombre).ToList(), "Referencia", "Nombre");

            return View(plantillaoperacion);
        }

        //
        // POST: /Plantilla/PlantillaOperacion/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlantillaOperacion plantillaoperacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(plantillaoperacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = plantillaoperacion.IdPlantillaProyecto });
            }

            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(plantillaoperacion.IdPlantillaProyecto);
            ViewBag.IdPlantillaProyecto = new SelectList(db.PlantillaProyectos.Where(p => p.Id == plantilla.Id), "Id", "Nombre", plantillaoperacion.IdPlantillaProyecto);

            ViewBag.Plantilla = db.PlantillaProyectos.Find(plantillaoperacion.IdPlantillaProyecto).Nombre;

            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.OperacionesGlobales, "Value", "Text");
            ViewBag.ListTipos = new SelectList(db.TipoFormulas.OrderBy(o => o.Nombre).ToList(), "Referencia", "Nombre");
            ViewBag.ListOperaciones = new SelectList(db.PlantillaOperaciones.Where(f => f.IdPlantillaProyecto == plantillaoperacion.IdPlantillaProyecto).OrderBy(f => f.Nombre).ToList(), "Referencia", "Nombre");

            return View(plantillaoperacion);
        }

        //
        // GET: /Plantilla/PlantillaOperacion/Delete/5

        public ActionResult Delete(int id)
        {
            PlantillaOperacion plantillaoperacion = db.PlantillaOperaciones.Find(id);
            try
            {
                var salidas = db.PlantillaSalidaOperaciones.Where(p => p.IdOperacion == plantillaoperacion.Id).ToList();

                foreach (PlantillaSalidaOperacion salida in salidas)
                {
                    db.PlantillaSalidaOperacionesRequester.RemoveElementByID(salida.Id);
                }

                db.PlantillaOperacionesRequester.RemoveElementByID(plantillaoperacion.Id);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No se puede eliminar porque existen registros dependientes.");
            }

            return RedirectToAction("Index", new { id = plantillaoperacion.IdPlantillaProyecto });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
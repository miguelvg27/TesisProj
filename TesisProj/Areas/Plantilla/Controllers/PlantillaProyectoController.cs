using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Plantilla.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Plantilla.Controllers
{
    [Authorize(Roles = "nav")]
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

        public ActionResult Delete(int id)
        {
            PlantillaProyecto plantillaproyecto = db.PlantillaProyectos.Find(id);
            try
            {
                var salidaoperaciones = db.PlantillaSalidaOperaciones.Include(s => s.Operacion).Where(p => p.Operacion.IdPlantillaProyecto == plantillaproyecto.Id).ToList();

                foreach (PlantillaSalidaOperacion salida in salidaoperaciones)
                {
                    db.PlantillaSalidaOperacionesRequester.RemoveElementByID(salida.Id);
                }

                var salidas = db.PlantillaSalidaProyectos.Where(s => s.IdPlantillaProyecto == plantillaproyecto.Id).ToList();

                foreach (PlantillaSalidaProyecto salida in salidas)
                {
                    db.PlantillaSalidaProyectosRequester.RemoveElementByID(salida.Id);
                }

                var operaciones = db.PlantillaOperaciones.Where(o => o.IdPlantillaProyecto == plantillaproyecto.Id).ToList();

                foreach (PlantillaOperacion operacion in operaciones)
                {
                    db.PlantillaOperacionesRequester.RemoveElementByID(operacion.Id);
                }

                db.PlantillaProyectosRequester.RemoveElementByID(plantillaproyecto.Id);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No se puede eliminar porque existen registros dependientes.");
            }

            return RedirectToAction("Index");
        }

        public ActionResult DuplicarPlantilla(int id)
        {
            PlantillaProyecto plantilla = db.PlantillaProyectos.Include(e => e.Salidas).Include(e => e.Operaciones).Include(e => e.Salidas.Select(s => s.Operaciones)).FirstOrDefault(e => e.Id == id);

            if (plantilla == null)
            {
                return HttpNotFound();
            }

            string nombre = "Copia de " + plantilla.Nombre + " ";
            string nombreTest = nombre;
            int i = 1;

            while (db.PlantillaProyectos.Any(p => p.Nombre.Equals(nombreTest)))
            {
                nombreTest = nombre + i++;
            }

            int idPlantilla = db.PlantillaProyectosRequester.AddElement(new PlantillaProyecto { Nombre = nombreTest });


            foreach (PlantillaOperacion operacion in plantilla.Operaciones.OrderBy(o => o.Secuencia))
            {
                db.PlantillaOperacionesRequester.AddElement(new PlantillaOperacion(operacion, idPlantilla));
            }

            foreach (PlantillaSalidaProyecto salida in plantilla.Salidas)
            {
                int idSalida = db.PlantillaSalidaProyectosRequester.AddElement(new PlantillaSalidaProyecto(salida, idPlantilla));

                foreach (PlantillaSalidaOperacion cruce in salida.Operaciones)
                {
                    int idOperacion = db.PlantillaOperaciones.First(o => o.Referencia.Equals(cruce.Operacion.Referencia) && o.IdPlantillaProyecto == idPlantilla).Id;
                    db.PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = idSalida, IdOperacion = idOperacion, Secuencia = cruce.Secuencia });
                }
            }

            return RedirectToAction("Edit", new { id = idPlantilla } );
        }

        public ActionResult VolverPlantilla(int id)
        {
            Proyecto plantilla = db.Proyectos.Include(e => e.Salidas).Include(e => e.Operaciones).Include(e => e.Salidas.Select(s => s.Operaciones)).FirstOrDefault(e => e.Id == id);

            if (plantilla == null)
            {
                return HttpNotFound();
            }

            string nombre = "Copia de " + plantilla.Nombre + " ";
            string nombreTest = nombre;
            int i = 1;

            while (db.PlantillaProyectos.Any(p => p.Nombre.Equals(nombreTest)))
            {
                nombreTest = nombre + i++;
            }

            int idPlantilla = db.PlantillaProyectosRequester.AddElement(new PlantillaProyecto { Nombre = nombreTest });


            foreach (Operacion operacion in plantilla.Operaciones.OrderBy(o => o.Secuencia))
            {
                db.PlantillaOperacionesRequester.AddElement(new PlantillaOperacion(operacion, idPlantilla));
            }

            foreach (SalidaProyecto salida in plantilla.Salidas)
            {
                int idSalida = db.PlantillaSalidaProyectosRequester.AddElement(new PlantillaSalidaProyecto(salida, idPlantilla));

                foreach (SalidaOperacion cruce in salida.Operaciones)
                {
                    int idOperacion = db.PlantillaOperaciones.First(o => o.Referencia.Equals(cruce.Operacion.Referencia) && o.IdPlantillaProyecto == idPlantilla).Id;
                    db.PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = idSalida, IdOperacion = idOperacion, Secuencia = cruce.Secuencia });
                }
            }

            return RedirectToAction("EditProyecto", "AnonPlantilla", new { id = idPlantilla, idProyecto = id });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
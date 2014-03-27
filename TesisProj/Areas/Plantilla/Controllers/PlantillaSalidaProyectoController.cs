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
    [Authorize(Roles = "nav")]
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

            var salidaproyectos = db.PlantillaSalidaProyectos.Include(s => s.PlantillaProyecto).Where(s => s.IdPlantillaProyecto == plantilla.Id).OrderBy(s => s.Secuencia);

            ViewBag.IdPlantilla = id;
            ViewBag.Plantilla = plantilla.Nombre;

            return View(salidaproyectos.ToList());
        }

        //
        // GET: /Plantilla/PlantillaSalidaProyecto/Assoc/5

        public ActionResult Assoc(int id = 0)
        {
            PlantillaSalidaProyecto salida = db.PlantillaSalidaProyectos.Find(id);
            if (salida == null)
            {
                return HttpNotFound();
            }

            ViewBag.Plantilla = db.PlantillaProyectos.Find(salida.IdPlantillaProyecto).Nombre;

            var asociados = db.PlantillaSalidaOperaciones.Include(p => p.Operacion).Where(p => p.IdSalida == salida.Id).OrderBy(s => s.Secuencia).Select(s => s.Operacion);
            var opciones = db.PlantillaOperaciones.Where(o => o.IdPlantillaProyecto == salida.IdPlantillaProyecto);
            ViewBag.Asociados = new MultiSelectList(asociados.ToList(), "Id", "ListName");
            ViewBag.Opciones = new MultiSelectList(opciones.OrderBy(o => o.Secuencia).ToList(), "Id", "ListName");

            return View(salida);
        }

        //
        // POST: /Plantilla/PlantillaSalidaProyecto/Assoc

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assoc(PlantillaSalidaProyecto salida, FormCollection form)
        {
            string strSeleccionados = form["Asociados"];
            if (strSeleccionados == null) return RedirectToAction("Index", new { id = salida.IdPlantillaProyecto });

            string[] seleccionados = strSeleccionados.Split(',');
            var operaciones = db.PlantillaSalidaOperaciones.Where(s => s.IdSalida == salida.Id);
            foreach (PlantillaSalidaOperacion oldOperacion in operaciones)
            {
                db.PlantillaSalidaOperaciones.Remove(oldOperacion);
            }
            db.SaveChanges();

            for(int i = 0; i < seleccionados.Length; i++)
            {
                int idOperacion = int.Parse(seleccionados[i]);
                db.PlantillaSalidaOperaciones.Add(new PlantillaSalidaOperacion { IdOperacion = idOperacion, IdSalida = salida.Id, Secuencia = i });
            }
            db.SaveChanges();

            return RedirectToAction("Index", new { id = salida.IdPlantillaProyecto });
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
            ViewBag.Plantilla = plantilla.Nombre;

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
            ViewBag.Plantilla = plantilla.Nombre;

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
            ViewBag.Plantilla = plantilla.Nombre;
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
            ViewBag.Plantilla = plantilla.Nombre;
            ViewBag.IdPlantillaProyecto = new SelectList(db.PlantillaProyectos.Where(p => p.Id == plantilla.Id), "Id", "Nombre", salidaproyecto.IdPlantillaProyecto);
            
            return View(salidaproyecto);
        }

        //
        // GET: /Plantilla/PlantillaSalidaProyecto/Delete/5

        public ActionResult Delete(int id)
        {
            PlantillaSalidaProyecto salidaproyecto = db.PlantillaSalidaProyectos.Find(id);
            try
            {
                var operaciones = db.PlantillaSalidaOperaciones.Where(p => p.IdSalida == salidaproyecto.Id).ToList();

                foreach (PlantillaSalidaOperacion operacion in operaciones)
                {
                    db.PlantillaSalidaOperacionesRequester.RemoveElementByID(operacion.Id);
                }

                db.PlantillaSalidaProyectosRequester.RemoveElementByID(salidaproyecto.Id);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No se puede eliminar porque existen registros dependientes.");
            }
            
            return RedirectToAction("Index", new { id = salidaproyecto.IdPlantillaProyecto });
        }

        public ActionResult DuplicarPlantilla(int id)
        {
            PlantillaSalidaProyecto plantilla = db.PlantillaSalidaProyectos.Include(s => s.Operaciones).FirstOrDefault(e => e.Id == id);

            if (plantilla == null)
            {
                return HttpNotFound();
            }

            string nombre = "Copia de " + plantilla.Nombre + " ";
            string nombreTest = nombre;
            int i = 1;

            while (db.PlantillaSalidaProyectos.Any(p => p.Nombre.Equals(nombreTest)))
            {
                nombreTest = nombre + i++;
            }

            int seq = db.PlantillaSalidaProyectos.Where(s => s.IdPlantillaProyecto == plantilla.IdPlantillaProyecto).Max(s => s.Secuencia) + 1;
            int idCopia = db.PlantillaSalidaProyectosRequester.AddElement(new PlantillaSalidaProyecto { Nombre = nombreTest, Secuencia = seq, PeriodoFinal = plantilla.PeriodoFinal, PeriodoInicial = plantilla.PeriodoInicial, IdPlantillaProyecto = plantilla.IdPlantillaProyecto });


            foreach (PlantillaSalidaOperacion operacion in plantilla.Operaciones.OrderBy(o => o.Secuencia))
            {
                db.PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = idCopia, IdOperacion = operacion.IdOperacion, Secuencia = operacion.Secuencia });
            }

            return RedirectToAction("Edit", new { id = idCopia });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
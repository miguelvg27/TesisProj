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

            var asociados = db.PlantillaSalidaOperaciones.Include(p => p.Operacion).Where(p => p.IdSalida == salida.Id).Select(p => p.Operacion);
            var opciones = db.PlantillaOperaciones.Where(o => o.IdPlantillaProyecto == salida.IdPlantillaProyecto).Except(asociados);
            ViewBag.Asociados = new MultiSelectList(asociados.OrderBy(o => o.Secuencia).ToList(), "Id", "Nombre");
            ViewBag.Opciones = new MultiSelectList(opciones.OrderBy(o => o.Secuencia).ToList(), "Id", "Nombre");

            return View(salida);
        }

        //
        // POST: /Plantilla/PlantillaSalidaProyecto/Assoc

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assoc(PlantillaSalidaProyecto salida, FormCollection form, string add, string remove, string addall, string removeall)
        {
            string seleccionados;
            int idOperacion;
            int idSalida = salida.Id;

            seleccionados = form["Opciones"];
            if (!string.IsNullOrEmpty(add) && !string.IsNullOrEmpty(seleccionados))
            {
                foreach (string sIdOperacion in seleccionados.Split(','))
                {
                    idOperacion = int.Parse(sIdOperacion);
                    if (!db.PlantillaSalidaOperaciones.Any(p => p.IdSalida == idSalida && p.IdOperacion == idOperacion))
                    {
                        db.PlantillaSalidaOperaciones.Add(new PlantillaSalidaOperacion { IdOperacion = idOperacion, IdSalida = idSalida });
                        db.SaveChanges();
                    }
                }
            }

            seleccionados = form["Asociados"];
            if (!string.IsNullOrEmpty(remove) && !string.IsNullOrEmpty(seleccionados))
            {

                PlantillaSalidaOperacion operacion;
                foreach (string sIdOperacion in seleccionados.Split(','))
                {
                    idOperacion = int.Parse(sIdOperacion);
                    operacion = db.PlantillaSalidaOperaciones.FirstOrDefault(p => p.IdOperacion == idOperacion && p.IdSalida == idSalida);
                    if (operacion != null)
                    {
                        db.PlantillaSalidaOperaciones.Remove(operacion);
                        db.SaveChanges();
                    }
                }
            }

            if (!string.IsNullOrEmpty(addall))
            {
                var plantillas = db.PlantillaOperaciones.Where(o => o.IdPlantillaProyecto == salida.IdPlantillaProyecto).ToList();
                foreach (PlantillaOperacion operacion in plantillas)
                {
                    idOperacion = operacion.Id;
                    if (!db.PlantillaSalidaOperaciones.Any(p => p.IdSalida == idSalida && p.IdOperacion == idOperacion))
                    {
                        db.PlantillaSalidaOperaciones.Add(new PlantillaSalidaOperacion { IdSalida = idSalida, IdOperacion = idOperacion });
                        db.SaveChanges();
                    }
                }
            }

            if (!string.IsNullOrEmpty(removeall))
            {
                var plantillas = db.PlantillaSalidaOperaciones.Where(p => p.IdSalida == idSalida).ToList();
                foreach (PlantillaSalidaOperacion operacion in plantillas)
                {
                    db.PlantillaSalidaOperaciones.Remove(operacion);
                    db.SaveChanges();
                }
            }

            var asociados = db.PlantillaSalidaOperaciones.Include(p => p.Operacion).Where(p => p.IdSalida == salida.Id).Select(p => p.Operacion);
            var opciones = db.PlantillaOperaciones.Where(o => o.IdPlantillaProyecto == salida.IdPlantillaProyecto).Except(asociados);
            ViewBag.Asociados = new MultiSelectList(asociados.OrderBy(o => o.Secuencia).ToList(), "Id", "Nombre");
            ViewBag.Opciones = new MultiSelectList(opciones.OrderBy(o => o.Secuencia).ToList(), "Id", "Nombre");
            ViewBag.Plantilla = db.PlantillaProyectos.Find(salida.IdPlantillaProyecto).Nombre;

            return View(salida);
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
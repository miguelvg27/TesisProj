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
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            ViewBag.IdPlantilla = plantilla.Id;
            ViewBag.Plantilla = plantilla.Nombre;
            
            var salidaproyectos = db.PlantillaSalidaProyectos.Include(s => s.PlantillaProyecto).Where(s => s.IdPlantillaProyecto == plantilla.Id).OrderBy(s => s.Secuencia);
            return View(salidaproyectos.ToList());
        }

        //
        // GET: /Plantilla/PlantillaSalidaProyecto/Assoc/5

        public ActionResult Assoc(int id = 0)
        {
            PlantillaSalidaProyecto salida = db.PlantillaSalidaProyectos.Find(id);
            if (salida == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            var asociados = db.PlantillaSalidaOperaciones.Include(p => p.Operacion).Where(p => p.IdSalida == salida.Id).OrderBy(s => s.Secuencia).Select(s => s.Operacion);
            var opciones = db.PlantillaOperaciones.Where(o => o.IdPlantillaProyecto == salida.IdPlantillaProyecto);
            ViewBag.Asociados = new MultiSelectList(asociados.ToList(), "Id", "ListName");
            ViewBag.Opciones = new MultiSelectList(opciones.OrderBy(o => o.Secuencia).ToList(), "Id", "ListName");
            ViewBag.Plantilla = db.PlantillaProyectos.Find(salida.IdPlantillaProyecto).Nombre;

            return View(salida);
        }

        //
        // POST: /Plantilla/PlantillaSalidaProyecto/Assoc/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assoc(PlantillaSalidaProyecto salida, FormCollection form)
        {
            string strSeleccionados = form["Asociados"];
            if (strSeleccionados == null) return RedirectToAction("Index", new { id = salida.IdPlantillaProyecto });

            string[] seleccionados = strSeleccionados.Split(',');
            var operaciones = db.PlantillaSalidaOperaciones.Where(s => s.IdSalida == salida.Id).ToList();
            foreach (PlantillaSalidaOperacion oldOperacion in operaciones)
            {
                db.PlantillaSalidaOperacionesRequester.RemoveElementByID(oldOperacion.Id);
            }

            for(int i = 0; i < seleccionados.Length; i++)
            {
                int idOperacion = int.Parse(seleccionados[i]);
                db.PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdOperacion = idOperacion, IdSalida = salida.Id, Secuencia = i });
            }

            return RedirectToAction("Index", new { id = salida.IdPlantillaProyecto });
        }

        //
        // GET: /Plantilla/PlantillaSalidaProyecto/Create

        public ActionResult Create(int idPlantilla = 0)
        {
            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(idPlantilla);
            if (plantilla == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            ViewBag.IdPlantilla = plantilla.Id;
            ViewBag.Plantilla = plantilla.Nombre;
            ViewBag.IdPlantillaProyecto = new SelectList(db.PlantillaProyectos.Where(p => p.Id == plantilla.Id), "Id", "Nombre");

            // Begin: Get sequence

            var salidas = db.PlantillaSalidaProyectos.Where(f => f.IdPlantillaProyecto == plantilla.Id);
            int defSecuencia = salidas.Count() > 0 ? salidas.Max(f => f.Secuencia) + 10 : 10;
            ViewBag.defSecuencia = defSecuencia;

            // End: Get sequence

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
                db.PlantillaSalidaProyectosRequester.AddElement(salidaproyecto);
                return RedirectToAction("Index", new { id = salidaproyecto.IdPlantillaProyecto });
            }

            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(salidaproyecto.IdPlantillaProyecto);

            ViewBag.IdPlantilla = plantilla.Id; 
            ViewBag.Plantilla = plantilla.Nombre;
            ViewBag.IdPlantillaProyecto = new SelectList(db.PlantillaProyectos.Where(p => p.Id == plantilla.Id), "Id", "Nombre", salidaproyecto.IdPlantillaProyecto);

            // Begin: Get sequence

            var salidas = db.PlantillaSalidaProyectos.Where(f => f.IdPlantillaProyecto == plantilla.Id);
            int defSecuencia = salidas.Count() > 0 ? salidas.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;

            // End: Get sequence

            return View(salidaproyecto);
        }

        //
        // GET: /Plantilla/PlantillaSalidaProyecto/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PlantillaSalidaProyecto salidaproyecto = db.PlantillaSalidaProyectos.Find(id);
            if (salidaproyecto == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(salidaproyecto.IdPlantillaProyecto);

            ViewBag.IdPlantilla = plantilla.Id;
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
                db.PlantillaSalidaProyectosRequester.ModifyElement(salidaproyecto);
                return RedirectToAction("Index", new { id = salidaproyecto.IdPlantillaProyecto });
            }
            
            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(salidaproyecto.IdPlantillaProyecto);

            ViewBag.IdPlantilla = plantilla.Id;
            ViewBag.Plantilla = plantilla.Nombre;
            ViewBag.IdPlantillaProyecto = new SelectList(db.PlantillaProyectos.Where(p => p.Id == plantilla.Id), "Id", "Nombre", salidaproyecto.IdPlantillaProyecto);
            
            return View(salidaproyecto);
        }

        //
        // GET: /Plantilla/PlantillaSalidaProyecto/Delete/5

        public ActionResult Delete(int id)
        {
            PlantillaSalidaProyecto salidaproyecto = db.PlantillaSalidaProyectos.Find(id);
            if (salidaproyecto == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            var operaciones = db.PlantillaSalidaOperaciones.Where(p => p.IdSalida == salidaproyecto.Id).ToList();
            foreach (PlantillaSalidaOperacion operacion in operaciones)
            {
                db.PlantillaSalidaOperacionesRequester.RemoveElementByID(operacion.Id);
            }

            db.PlantillaSalidaProyectosRequester.RemoveElementByID(salidaproyecto.Id);
            return RedirectToAction("Index", new { id = salidaproyecto.IdPlantillaProyecto });
        }

        //
        // GET: /Plantilla/PlantillaSalidaProyecto/DuplicarPlantilla/5

        public ActionResult DuplicarPlantilla(int id)
        {
            PlantillaSalidaProyecto plantilla = db.PlantillaSalidaProyectos.Include(s => s.Operaciones).FirstOrDefault(e => e.Id == id);
            if (plantilla == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Begin: Get unique name

            string nombre = "Copia de " + plantilla.Nombre + " ";
            string nombreTest = nombre;
            int i = 1;

            while (db.PlantillaSalidaProyectos.Any(p => p.Nombre.Equals(nombreTest)))
            {
                nombreTest = nombre + i++;
            }

            // End: Get unique name

            int seqDuplicado = db.PlantillaSalidaProyectos.Where(s => s.IdPlantillaProyecto == plantilla.IdPlantillaProyecto).Max(s => s.Secuencia) + 10;
            int idDuplicado = db.PlantillaSalidaProyectosRequester.AddElement(new PlantillaSalidaProyecto { Nombre = nombreTest, Secuencia = seqDuplicado, PeriodoFinal = plantilla.PeriodoFinal, PeriodoInicial = plantilla.PeriodoInicial, IdPlantillaProyecto = plantilla.IdPlantillaProyecto });

            foreach (PlantillaSalidaOperacion operacion in plantilla.Operaciones.OrderBy(o => o.Secuencia))
            {
                db.PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = idDuplicado, IdOperacion = operacion.IdOperacion, Secuencia = operacion.Secuencia });
            }

            return RedirectToAction("Edit", new { id = idDuplicado });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
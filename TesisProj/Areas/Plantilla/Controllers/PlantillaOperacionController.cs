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
    [Authorize(Roles = "nav")]
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
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            ViewBag.IdPlantilla = plantilla.Id;
            ViewBag.Plantilla = plantilla.Nombre;
            
            var plantillaoperacions = db.PlantillaOperaciones.Include(o => o.TipoDato).Where(o => o.IdPlantillaProyecto == plantilla.Id).OrderBy(o => o.Secuencia);
            return View(plantillaoperacions.ToList());
        }

        //
        // GET: /Plantilla/PlantillaOperacion/Create

        public ActionResult Create(int idPlantilla = 0)
        {
            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(idPlantilla);
            if (plantilla == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            ViewBag.IdPlantilla = idPlantilla;
            ViewBag.Plantilla = plantilla.Nombre;
            ViewBag.IdTipoDato = new SelectList(db.TipoDatos.OrderBy(t => t.Nombre), "Id", "Nombre");
            ViewBag.IdPlantillaProyecto = new SelectList(db.PlantillaProyectos.Where(p => p.Id == plantilla.Id), "Id", "Nombre");
            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.OperacionesGlobales, "Value", "Text");
            ViewBag.ConstantesList = new SelectList(db.Constantes.OrderBy(c => c.Nombre), "Valor", "Nombre");
            ViewBag.ListTipos = new SelectList(db.TipoFormulas.OrderBy(o => o.Nombre).ToList(), "Referencia", "ListName");
            ViewBag.ListOperaciones = new SelectList(db.PlantillaOperaciones.Where(f => f.IdPlantillaProyecto == idPlantilla).OrderBy(f => f.Secuencia).ToList(), "Referencia", "ListName");

            // Begin: Get sequence

            var operaciones = db.PlantillaOperaciones.Where(f => f.IdPlantillaProyecto == plantilla.Id);
            int defSecuencia = operaciones.Count() > 0 ? operaciones.Max(f => f.Secuencia) + 10 : 10;
            ViewBag.defSecuencia = defSecuencia;

            // End: Get sequence

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
                db.PlantillaOperacionesRequester.AddElement(plantillaoperacion);
                return RedirectToAction("Index", new { id = plantillaoperacion.IdPlantillaProyecto });
            }

            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(plantillaoperacion.IdPlantillaProyecto);
            
            ViewBag.IdPlantilla = plantilla.Id;
            ViewBag.Plantilla = plantilla.Nombre;
            ViewBag.IdTipoDato = new SelectList(db.TipoDatos.OrderBy(t => t.Nombre), "Id", "Nombre", plantillaoperacion.IdTipoDato);
            ViewBag.IdPlantillaProyecto = new SelectList(db.PlantillaProyectos.Where(p => p.Id == plantilla.Id), "Id", "Nombre", plantillaoperacion.IdPlantillaProyecto);
            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.OperacionesGlobales, "Value", "Text");
            ViewBag.ConstantesList = new SelectList(db.Constantes.OrderBy(c => c.Nombre), "Valor", "Nombre");
            ViewBag.ListTipos = new SelectList(db.TipoFormulas.OrderBy(o => o.Nombre).ToList(), "Referencia", "ListName");
            ViewBag.ListOperaciones = new SelectList(db.PlantillaOperaciones.Where(f => f.IdPlantillaProyecto == plantillaoperacion.IdPlantillaProyecto && plantillaoperacion.Secuencia > f.Secuencia).OrderBy(f => f.Secuencia).ToList(), "Referencia", "ListName");

            return View(plantillaoperacion);
        }

        //
        // GET: /Plantilla/PlantillaOperacion/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PlantillaOperacion plantillaoperacion = db.PlantillaOperaciones.Find(id);
            if (plantillaoperacion == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(plantillaoperacion.IdPlantillaProyecto);
            ViewBag.Plantilla = plantilla.Nombre;
            ViewBag.IdPlantillaProyecto = new SelectList(db.PlantillaProyectos.Where(p => p.Id == plantilla.Id), "Id", "Nombre", plantillaoperacion.IdPlantillaProyecto);
            ViewBag.IdTipoDato = new SelectList(db.TipoDatos.OrderBy(t => t.Nombre), "Id", "Nombre", plantillaoperacion.IdTipoDato);
            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.OperacionesGlobales, "Value", "Text");
            ViewBag.ConstantesList = new SelectList(db.Constantes.OrderBy(c => c.Nombre), "Valor", "Nombre");
            ViewBag.ListTipos = new SelectList(db.TipoFormulas.OrderBy(o => o.Nombre).ToList(), "Referencia", "ListName");
            ViewBag.ListOperaciones = new SelectList(db.PlantillaOperaciones.Where(f => f.IdPlantillaProyecto == plantillaoperacion.IdPlantillaProyecto && plantillaoperacion.Secuencia > f.Secuencia).OrderBy(f => f.Secuencia).ToList(), "Referencia", "ListName");

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
                db.PlantillaOperacionesRequester.ModifyElement(plantillaoperacion);
                return RedirectToAction("Index", new { id = plantillaoperacion.IdPlantillaProyecto });
            }

            PlantillaProyecto plantilla = db.PlantillaProyectos.Find(plantillaoperacion.IdPlantillaProyecto);
            ViewBag.Plantilla = plantilla.Nombre;
            ViewBag.IdPlantillaProyecto = new SelectList(db.PlantillaProyectos.Where(p => p.Id == plantilla.Id), "Id", "Nombre", plantillaoperacion.IdPlantillaProyecto);
            ViewBag.IdTipoDato = new SelectList(db.TipoDatos.OrderBy(t => t.Nombre), "Id", "Nombre", plantillaoperacion.IdTipoDato);
            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.OperacionesGlobales, "Value", "Text");
            ViewBag.ConstantesList = new SelectList(db.Constantes.OrderBy(c => c.Nombre), "Valor", "Nombre");
            ViewBag.ListTipos = new SelectList(db.TipoFormulas.OrderBy(o => o.Nombre).ToList(), "Referencia", "ListName");
            ViewBag.ListOperaciones = new SelectList(db.PlantillaOperaciones.Where(f => f.IdPlantillaProyecto == plantillaoperacion.IdPlantillaProyecto && plantillaoperacion.Secuencia > f.Secuencia).OrderBy(f => f.Secuencia).ToList(), "Referencia", "ListName");

            return View(plantillaoperacion);
        }

        //
        // GET: /Plantilla/PlantillaOperacion/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PlantillaOperacion plantillaoperacion = db.PlantillaOperaciones.Find(id);
            if (plantillaoperacion == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            var salidas = db.PlantillaSalidaOperaciones.Where(p => p.IdOperacion == plantillaoperacion.Id).ToList();
            foreach (PlantillaSalidaOperacion salida in salidas)
            {
                db.PlantillaSalidaOperacionesRequester.RemoveElementByID(salida.Id);
            }

            db.PlantillaOperacionesRequester.RemoveElementByID(plantillaoperacion.Id);
            return RedirectToAction("Index", new { id = plantillaoperacion.IdPlantillaProyecto });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
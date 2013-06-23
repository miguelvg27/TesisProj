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
    public class PlantillaSalidaElementoController : Controller
    {
        private TProjContext db = new TProjContext();

        //
        // GET: /Plantilla/PlantillaSalidaElemento/

        public ActionResult Index(int id = 0)
        {
            PlantillaElemento plantilla = db.PlantillaElementos.Find(id);
            if (plantilla == null)
            {
                return HttpNotFound();
            }

            var salidaelementos = db.PlantillaSalidaElementos.Include(s => s.PlantillaElemento).Include(s => s.Formula).OrderBy(s => s.Secuencia);

            ViewBag.IdPlantilla = id;
            ViewBag.Plantilla = plantilla.Nombre;
            TipoElemento tipo = db.TipoElementos.Find(plantilla.IdTipoElemento);
            ViewBag.TipoPlantilla = tipo != null ? tipo.Nombre : "";

            return View(salidaelementos.ToList());
        }

        //
        // GET: /Plantilla/PlantillaSalidaElemento/Details/5

        public ActionResult Details(int id = 0)
        {
            PlantillaSalidaElemento salidaelemento = db.PlantillaSalidaElementos.Find(id);
            if (salidaelemento == null)
            {
                return HttpNotFound();
            }
            
            salidaelemento.Formula = db.PlantillaFormulas.Find(salidaelemento.IdFormula);
            salidaelemento.PlantillaElemento = db.PlantillaElementos.Find(salidaelemento.IdPlantillaElemento);

            return View(salidaelemento);
        }

        //
        // GET: /Plantilla/PlantillaSalidaElemento/Create

        public ActionResult Create(int idPlantilla = 0)
        {
            PlantillaElemento plantilla = db.PlantillaElementos.Find(idPlantilla);
            if (plantilla == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdPlantilla = idPlantilla;
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == plantilla.Id), "Id", "Nombre");
            ViewBag.IdFormula = new SelectList(db.PlantillaFormulas.Where(f => f.IdPlantillaElemento == plantilla.Id).OrderBy(t => t.Nombre), "Id", "Nombre");

            var salidaelementos = db.PlantillaSalidaElementos.Where(f => f.IdPlantillaElemento == plantilla.Id);
            int defSecuencia = salidaelementos.Count() > 0 ? salidaelementos.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;

            return View();
        }

        //
        // POST: /Plantilla/PlantillaSalidaElemento/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlantillaSalidaElemento salidaelemento)
        {
            if (ModelState.IsValid)
            {
                db.PlantillaSalidaElementos.Add(salidaelemento);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = salidaelemento.IdPlantillaElemento });
            }

            PlantillaElemento plantilla = db.PlantillaElementos.Find(salidaelemento.IdPlantillaElemento);
            ViewBag.IdPlantilla = salidaelemento.IdPlantillaElemento;
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == plantilla.Id), "Id", "Nombre", salidaelemento.IdPlantillaElemento);
            ViewBag.IdFormula = new SelectList(db.PlantillaFormulas.Where(f => f.IdPlantillaElemento == plantilla.Id).OrderBy(t => t.Nombre), "Id", "Nombre", salidaelemento.IdFormula);

            var salidaelementos = db.PlantillaSalidaElementos.Where(f => f.IdPlantillaElemento == plantilla.Id);
            int defSecuencia = salidaelementos.Count() > 0 ? salidaelementos.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;

            return View(salidaelemento);
        }

        //
        // GET: /Plantilla/PlantillaSalidaElemento/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PlantillaSalidaElemento salidaelemento = db.PlantillaSalidaElementos.Find(id);
            if (salidaelemento == null)
            {
                return HttpNotFound();
            }
            
            PlantillaElemento plantilla = db.PlantillaElementos.Find(salidaelemento.IdPlantillaElemento);
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == salidaelemento.IdPlantillaElemento), "Id", "Nombre", salidaelemento.IdPlantillaElemento);
            ViewBag.IdFormula = new SelectList(db.PlantillaFormulas.Where(f => f.IdPlantillaElemento == salidaelemento.IdPlantillaElemento), "Id", "Nombre", salidaelemento.IdFormula);
            
            return View(salidaelemento);
        }

        //
        // POST: /Plantilla/PlantillaSalidaElemento/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlantillaSalidaElemento salidaelemento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salidaelemento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = salidaelemento.IdPlantillaElemento });
            }

            PlantillaElemento plantilla = db.PlantillaElementos.Find(salidaelemento.IdPlantillaElemento);
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == salidaelemento.IdPlantillaElemento), "Id", "Nombre", salidaelemento.IdPlantillaElemento);
            ViewBag.IdFormula = new SelectList(db.PlantillaFormulas.Where(f => f.IdPlantillaElemento == salidaelemento.IdPlantillaElemento), "Id", "Nombre", salidaelemento.IdFormula);

            return View(salidaelemento);
        }

        //
        // GET: /Plantilla/PlantillaSalidaElemento/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PlantillaSalidaElemento salidaelemento = db.PlantillaSalidaElementos.Find(id);
            if (salidaelemento == null)
            {
                return HttpNotFound();
            }

            salidaelemento.Formula = db.PlantillaFormulas.Find(salidaelemento.IdFormula);
            salidaelemento.PlantillaElemento = db.PlantillaElementos.Find(salidaelemento.IdPlantillaElemento);

            return View(salidaelemento);
        }

        //
        // POST: /Plantilla/PlantillaSalidaElemento/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlantillaSalidaElemento salidaelemento = db.PlantillaSalidaElementos.Find(id);
            try
            {
                db.PlantillaSalidaElementos.Remove(salidaelemento);
                db.SaveChanges();
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No se puede eliminar porque existen registros dependientes.");
                salidaelemento.Formula = db.PlantillaFormulas.Find(salidaelemento.IdFormula);
                salidaelemento.PlantillaElemento = db.PlantillaElementos.Find(salidaelemento.IdPlantillaElemento);
                return View("Delete", salidaelemento);
            }

            return RedirectToAction("Index", new { id = salidaelemento.IdPlantillaElemento });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
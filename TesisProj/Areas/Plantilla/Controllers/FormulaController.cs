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
    public class FormulaController : Controller
    {
        private TProjContext db = new TProjContext();

        //
        // GET: /Plantilla/Formula/

        public ActionResult Index(int id = 0)
        {
            PlantillaElemento plantilla = db.PlantillaElementos.Find(id);
            if (plantilla == null)
            {
                return HttpNotFound();
            }

            var formulas = db.Formulas.Include(f => f.PlantillaElemento).Include(f => f.TipoFormula).Where(f => f.IdPlantillaElemento == id).OrderBy(f => f.Secuencia);

            ViewBag.IdPlantilla = id;
            ViewBag.Plantilla = plantilla.Nombre;
            TipoElemento tipo = db.TipoElementos.Find(plantilla.IdTipoElemento);
            ViewBag.TipoPlantilla = tipo != null ? tipo.Nombre : "";

            return View(formulas.ToList());
        }

        //
        // GET: /Plantilla/Formula/Details/5

        public ActionResult Details(int id = 0)
        {
            Formula formula = db.Formulas.Find(id);
            if (formula == null)
            {
                return HttpNotFound();
            }

            formula.TipoFormula = db.TipoFormulas.Find(formula.IdTipoFormula);
            formula.PlantillaElemento = db.PlantillaElementos.Find(formula.IdPlantillaElemento);

            return View(formula);
        }

        //
        // GET: /Plantilla/Formula/Create

        public ActionResult Create(int idPlantilla = 0)
        {
            PlantillaElemento plantilla = db.PlantillaElementos.Find(idPlantilla);
            if (plantilla == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdPlantilla = idPlantilla;
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == plantilla.Id), "Id", "Nombre");
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.Where(t => t.IdTipoElemento == plantilla.IdTipoElemento).OrderBy(t => t.Nombre), "Id", "Nombre");

            var formulas = db.Formulas.Where(f => f.IdPlantillaElemento == plantilla.Id);
            int defSecuencia = formulas.Count() > 0 ? formulas.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;

            return View();
        }

        //
        // POST: /Plantilla/Formula/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Formula formula)
        {
            if (ModelState.IsValid)
            {
                db.Formulas.Add(formula);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = formula.IdPlantillaElemento });
            }

            PlantillaElemento plantilla = db.PlantillaElementos.Find(formula.IdPlantillaElemento);
            ViewBag.IdPlantilla = formula.IdPlantillaElemento;
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == formula.IdPlantillaElemento), "Id", "Nombre", formula.IdPlantillaElemento);
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.Where(t => t.IdTipoElemento == plantilla.IdTipoElemento).OrderBy(t => t.Nombre), "Id", "Nombre", formula.IdTipoFormula);

            var formulas = db.Formulas.Where(f => f.IdPlantillaElemento == plantilla.Id);
            int defSecuencia = formulas.Count() > 0 ? formulas.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;

            return View(formula);
        }

        //
        // GET: /Plantilla/Formula/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Formula formula = db.Formulas.Find(id);
            if (formula == null)
            {
                return HttpNotFound();
            }

            PlantillaElemento plantilla = db.PlantillaElementos.Find(formula.IdPlantillaElemento);
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == formula.IdPlantillaElemento), "Id", "Nombre", formula.IdPlantillaElemento);
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.Where(t => t.IdTipoElemento == plantilla.IdTipoElemento).OrderBy(t => t.Nombre), "Id", "Nombre", formula.IdTipoFormula);
            
            return View(formula);
        }

        //
        // POST: /Plantilla/Formula/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Formula formula)
        {
            if (ModelState.IsValid)
            {
                db.Entry(formula).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = formula.IdPlantillaElemento });
            }

            PlantillaElemento plantilla = db.PlantillaElementos.Find(formula.IdPlantillaElemento);
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == formula.IdPlantillaElemento), "Id", "Nombre", formula.IdPlantillaElemento);
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.Where(t => t.IdTipoElemento == plantilla.IdTipoElemento).OrderBy(t => t.Nombre), "Id", "Nombre", formula.IdTipoFormula);
            
            return View(formula);
        }

        //
        // GET: /Plantilla/Formula/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Formula formula = db.Formulas.Find(id);
            if (formula == null)
            {
                return HttpNotFound();
            }

            formula.TipoFormula = db.TipoFormulas.Find(formula.IdTipoFormula);
            formula.PlantillaElemento = db.PlantillaElementos.Find(formula.IdPlantillaElemento);

            return View(formula);
        }

        //
        // POST: /Plantilla/Formula/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Formula formula = db.Formulas.Find(id);
            db.Formulas.Remove(formula);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = formula.IdPlantillaElemento });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
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

            var formulas = db.Formulas.Include(f => f.PlantillaElemento).Include(f => f.TipoFormula);

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

        public ActionResult Create(int id = 0)
        {
            PlantillaElemento plantilla = db.PlantillaElementos.Find(id);
            if (plantilla == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdPlantilla = id;
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == id), "Id", "Nombre");
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.OrderBy(t => t.Nombre), "Id", "Nombre");
            
            return View();
        }

        //
        // POST: /Plantilla/Formula/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Formula formula, int id)
        {
            if (ModelState.IsValid)
            {
                db.Formulas.Add(formula);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = id });
            }

            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == formula.IdPlantillaElemento), "Id", "Nombre", formula.IdPlantillaElemento);
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.OrderBy(t => t.Nombre), "Id", "Nombre", formula.IdTipoFormula);
            
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

            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos, "Id", "Nombre", formula.IdPlantillaElemento);
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas, "Id", "Nombre", formula.IdTipoFormula);
            
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
                return RedirectToAction("Index", new { id = formula.IdTipoFormula });
            }

            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == formula.IdPlantillaElemento), "Id", "Nombre", formula.IdPlantillaElemento);
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.OrderBy(t => t.Nombre), "Id", "Nombre", formula.IdTipoFormula);
            
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
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
    public class PlantillaFormulaController : Controller
    {
        private TProjContext db = new TProjContext();

        //
        // GET: /Plantilla/PlantillaFormula/

        public ActionResult Index(int id = 0)
        {
            PlantillaElemento plantilla = db.PlantillaElementos.Find(id);
            if (plantilla == null)
            {
                return HttpNotFound();
            }

            var formulas = db.PlantillaFormulas.Include(f => f.PlantillaElemento).Include(f => f.TipoFormula).Where(f => f.IdPlantillaElemento == id).OrderBy(f => f.Secuencia);

            ViewBag.IdPlantilla = id;
            ViewBag.Plantilla = plantilla.Nombre;
            TipoElemento tipo = db.TipoElementos.Find(plantilla.IdTipoElemento);
            ViewBag.TipoPlantilla = tipo != null ? tipo.Nombre : "";

            return View(formulas.ToList());
        }

        //
        // GET: /Plantilla/PlantillaFormula/Details/5

        public ActionResult Details(int id = 0)
        {
            PlantillaFormula formula = db.PlantillaFormulas.Find(id);
            if (formula == null)
            {
                return HttpNotFound();
            }

            formula.TipoFormula = db.TipoFormulas.Find(formula.IdTipoFormula);
            formula.PlantillaElemento = db.PlantillaElementos.Find(formula.IdPlantillaElemento);

            return View(formula);
        }

        //
        // GET: /Plantilla/PlantillaFormula/Create

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

            ViewBag.Plantilla = plantilla.Nombre;

            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.FormulasGlobales, "Value", "Text");
            ViewBag.ListParametros = new SelectList(db.PlantillaParametros.Where(p => p.IdPlantillaElemento == idPlantilla).OrderBy(o => o.Nombre).ToList(), "Referencia", "Nombre");
            ViewBag.ListFormulas = new SelectList(db.PlantillaFormulas.Where(f => f.IdPlantillaElemento == idPlantilla).OrderBy(f => f.Nombre).ToList(), "Referencia", "Nombre");

            var formulas = db.PlantillaFormulas.Where(f => f.IdPlantillaElemento == plantilla.Id);
            int defSecuencia = formulas.Count() > 0 ? formulas.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;

            return View();
        }

        //
        // POST: /Plantilla/PlantillaFormula/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlantillaFormula formula)
        {
            if (ModelState.IsValid)
            {
                db.PlantillaFormulas.Add(formula);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = formula.IdPlantillaElemento });
            }

            PlantillaElemento plantilla = db.PlantillaElementos.Find(formula.IdPlantillaElemento);
            ViewBag.IdPlantilla = formula.IdPlantillaElemento;
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == formula.IdPlantillaElemento), "Id", "Nombre", formula.IdPlantillaElemento);
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.Where(t => t.IdTipoElemento == plantilla.IdTipoElemento).OrderBy(t => t.Nombre), "Id", "Nombre", formula.IdTipoFormula);

            ViewBag.Plantilla = plantilla.Nombre;

            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.FormulasGlobales, "Value", "Text");
            ViewBag.ListParametros = new SelectList(db.PlantillaParametros.Where(p => p.IdPlantillaElemento == formula.IdPlantillaElemento).OrderBy(o => o.Nombre).ToList(), "Referencia", "Nombre");
            ViewBag.ListFormulas = new SelectList(db.PlantillaFormulas.Where(f => f.IdPlantillaElemento == formula.IdPlantillaElemento).OrderBy(f => f.Nombre).ToList(), "Referencia", "Nombre");

            var formulas = db.PlantillaFormulas.Where(f => f.IdPlantillaElemento == plantilla.Id);
            int defSecuencia = formulas.Count() > 0 ? formulas.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;

            return View(formula);
        }

        //
        // GET: /Plantilla/PlantillaFormula/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PlantillaFormula formula = db.PlantillaFormulas.Find(id);
            if (formula == null)
            {
                return HttpNotFound();
            }

            PlantillaElemento plantilla = db.PlantillaElementos.Find(formula.IdPlantillaElemento);
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == formula.IdPlantillaElemento), "Id", "Nombre", formula.IdPlantillaElemento);
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.Where(t => t.IdTipoElemento == plantilla.IdTipoElemento).OrderBy(t => t.Nombre), "Id", "Nombre", formula.IdTipoFormula);

            ViewBag.Plantilla = plantilla.Nombre;

            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.FormulasGlobales, "Value", "Text");
            ViewBag.ListParametros = new SelectList(db.PlantillaParametros.Where(p => p.IdPlantillaElemento == formula.IdPlantillaElemento).OrderBy(o => o.Nombre).ToList(), "Referencia", "Nombre");
            ViewBag.ListFormulas = new SelectList(db.PlantillaFormulas.Where(f => f.IdPlantillaElemento == formula.IdPlantillaElemento).OrderBy(f => f.Nombre).ToList(), "Referencia", "Nombre");

            return View(formula);
        }

        //
        // POST: /Plantilla/PlantillaFormula/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlantillaFormula formula)
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

            ViewBag.Plantilla = plantilla.Nombre;

            return View(formula);
        }

        //
        // GET: /Plantilla/PlantillaFormula/Delete/5

        public ActionResult Delete(int id)
        {
            PlantillaFormula formula = db.PlantillaFormulas.Find(id);
            try
            {
                db.PlantillaFormulasRequester.RemoveElementByID(formula.Id);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No se puede eliminar porque existen registros dependientes.");
                formula.TipoFormula = db.TipoFormulas.Find(formula.IdTipoFormula);
                formula.PlantillaElemento = db.PlantillaElementos.Find(formula.IdPlantillaElemento);
                return View("Delete", formula);
            }
            
            return RedirectToAction("Index", new { id = formula.IdPlantillaElemento });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
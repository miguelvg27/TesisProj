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
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            ViewBag.IdPlantilla = id;
            ViewBag.Plantilla = plantilla.Nombre;
            ViewBag.TipoPlantilla = db.TipoElementos.Find(plantilla.IdTipoElemento).Nombre;

            var formulas = db.PlantillaFormulas.Include(f => f.TipoDato).Include(f => f.TipoFormula).Where(f => f.IdPlantillaElemento == id).OrderBy(f => f.Secuencia);
            return View(formulas.ToList());
        }

        //
        // GET: /Plantilla/PlantillaFormula/Create

        public ActionResult Create(int idPlantilla = 0)
        {
            PlantillaElemento plantilla = db.PlantillaElementos.Find(idPlantilla);
            if (plantilla == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            ViewBag.IdPlantilla = idPlantilla;
            ViewBag.Plantilla = plantilla.Nombre;
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == plantilla.Id), "Id", "Nombre");
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.Where(t => t.IdTipoElemento == plantilla.IdTipoElemento).OrderBy(t => t.Nombre), "Id", "Nombre");
            ViewBag.IdTipoDato = new SelectList(db.TipoDatos.OrderBy(t => t.Nombre), "Id", "Nombre");
            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.FormulasGlobales, "Value", "Text");
            ViewBag.ConstantesList = new SelectList(db.Constantes.OrderBy(c => c.Nombre), "Valor", "Nombre");
            ViewBag.ListParametros = new SelectList(db.PlantillaParametros.Where(p => p.IdPlantillaElemento == idPlantilla).OrderBy(o => o.Nombre).ToList(), "Referencia", "ListName");
            ViewBag.ListFormulas = new SelectList(db.PlantillaFormulas.Where(f => f.IdPlantillaElemento == idPlantilla).OrderBy(f => f.Secuencia).ToList(), "Referencia", "ListName");

            // Begin: Get sequence

            var formulas = db.PlantillaFormulas.Where(f => f.IdPlantillaElemento == plantilla.Id);
            int defSecuencia = formulas.Count() > 0 ? formulas.Max(f => f.Secuencia) + 10 : 10;
            ViewBag.defSecuencia = defSecuencia;

            // End: Get sequence

            return View();
        }

        //
        // POST: /Plantilla/PlantillaFormula/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlantillaFormula plantillaformula)
        {
            if (ModelState.IsValid)
            {
                db.PlantillaFormulasRequester.AddElement(plantillaformula);
                return RedirectToAction("Index", new { id = plantillaformula.IdPlantillaElemento });
            }

            PlantillaElemento plantilla = db.PlantillaElementos.Find(plantillaformula.IdPlantillaElemento);

            ViewBag.IdPlantilla = plantilla.Id;
            ViewBag.Plantilla = plantilla.Nombre;
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == plantilla.Id), "Id", "Nombre", plantillaformula.IdPlantillaElemento);
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.Where(t => t.IdTipoElemento == plantilla.IdTipoElemento).OrderBy(t => t.Nombre), "Id", "Nombre", plantillaformula.IdTipoFormula);
            ViewBag.IdTipoDato = new SelectList(db.TipoDatos.OrderBy(t => t.Nombre), "Id", "Nombre", plantillaformula.IdTipoDato);
            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.FormulasGlobales, "Value", "Text");
            ViewBag.ConstantesList = new SelectList(db.Constantes.OrderBy(c => c.Nombre), "Valor", "Nombre");
            ViewBag.ListParametros = new SelectList(db.PlantillaParametros.Where(p => p.IdPlantillaElemento == plantillaformula.IdPlantillaElemento).OrderBy(o => o.Nombre).ToList(), "Referencia", "ListName");
            ViewBag.ListFormulas = new SelectList(db.PlantillaFormulas.Where(f => f.IdPlantillaElemento == plantillaformula.IdPlantillaElemento && plantillaformula.Secuencia > f.Secuencia).OrderBy(f => f.Secuencia).ToList(), "Referencia", "ListName");

            return View(plantillaformula);
        }

        //
        // GET: /Plantilla/PlantillaFormula/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PlantillaFormula plantillaformula = db.PlantillaFormulas.Find(id);
            if (plantillaformula == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            PlantillaElemento plantilla = db.PlantillaElementos.Find(plantillaformula.IdPlantillaElemento);

            ViewBag.IdPlantilla = plantilla.Id;
            ViewBag.Plantilla = plantilla.Nombre;
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == plantilla.Id), "Id", "Nombre", plantillaformula.IdPlantillaElemento);
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.Where(t => t.IdTipoElemento == plantilla.IdTipoElemento).OrderBy(t => t.Nombre), "Id", "Nombre", plantillaformula.IdTipoFormula);
            ViewBag.IdTipoDato = new SelectList(db.TipoDatos.OrderBy(t => t.Nombre), "Id", "Nombre", plantillaformula.IdTipoDato);
            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.FormulasGlobales, "Value", "Text");
            ViewBag.ConstantesList = new SelectList(db.Constantes.OrderBy(c => c.Nombre), "Valor", "Nombre");
            ViewBag.ListParametros = new SelectList(db.PlantillaParametros.Where(p => p.IdPlantillaElemento == plantillaformula.IdPlantillaElemento).OrderBy(o => o.Nombre).ToList(), "Referencia", "ListName");
            ViewBag.ListFormulas = new SelectList(db.PlantillaFormulas.Where(f => f.IdPlantillaElemento == plantillaformula.IdPlantillaElemento && plantillaformula.Secuencia > f.Secuencia).OrderBy(f => f.Secuencia).ToList(), "Referencia", "ListName");

            return View(plantillaformula);
        }

        //
        // POST: /Plantilla/PlantillaFormula/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlantillaFormula plantillaformula)
        {
            if (ModelState.IsValid)
            {
                db.PlantillaFormulasRequester.ModifyElement(plantillaformula);
                return RedirectToAction("Index", new { id = plantillaformula.IdPlantillaElemento });
            }

            PlantillaElemento plantilla = db.PlantillaElementos.Find(plantillaformula.IdPlantillaElemento);
            
            ViewBag.IdPlantilla = plantilla.Id;
            ViewBag.Plantilla = plantilla.Nombre;
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == plantilla.Id), "Id", "Nombre", plantillaformula.IdPlantillaElemento);
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.Where(t => t.IdTipoElemento == plantilla.IdTipoElemento).OrderBy(t => t.Nombre), "Id", "Nombre", plantillaformula.IdTipoFormula);
            ViewBag.IdTipoDato = new SelectList(db.TipoDatos.OrderBy(t => t.Nombre), "Id", "Nombre", plantillaformula.IdTipoDato);
            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.FormulasGlobales, "Value", "Text");
            ViewBag.ConstantesList = new SelectList(db.Constantes.OrderBy(c => c.Nombre), "Valor", "Nombre");
            ViewBag.ListParametros = new SelectList(db.PlantillaParametros.Where(p => p.IdPlantillaElemento == plantillaformula.IdPlantillaElemento).OrderBy(o => o.Nombre).ToList(), "Referencia", "ListName");
            ViewBag.ListFormulas = new SelectList(db.PlantillaFormulas.Where(f => f.IdPlantillaElemento == plantillaformula.IdPlantillaElemento && plantillaformula.Secuencia > f.Secuencia).OrderBy(f => f.Secuencia).ToList(), "Referencia", "ListName");

            return View(plantillaformula);
        }

        //
        // GET: /Plantilla/PlantillaFormula/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PlantillaFormula plantillaformula = db.PlantillaFormulas.Find(id);
            if (plantillaformula == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            db.PlantillaFormulasRequester.RemoveElementByID(plantillaformula.Id);
            return RedirectToAction("Index", new { id = plantillaformula.IdPlantillaElemento });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
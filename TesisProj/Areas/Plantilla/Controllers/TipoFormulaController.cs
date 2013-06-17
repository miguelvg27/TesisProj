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
    public class TipoFormulaController : Controller
    {
        private TProjContext db = new TProjContext();

        //
        // GET: /Plantilla/TipoFormula/

        public ActionResult Index()
        {
            var tipoformulas = db.TipoFormulas.Include(f => f.TipoElemento).OrderBy(f => f.TipoElemento.Nombre);
            return View(tipoformulas.ToList());
        }

        //
        // GET: /Plantilla/TipoFormula/Details/5

        public ActionResult Details(int id = 0)
        {
            TipoFormula tipoformula = db.TipoFormulas.Find(id);
            tipoformula.TipoElemento = db.TipoElementos.Find(tipoformula.IdTipoElemento);
            if (tipoformula == null)
            {
                return HttpNotFound();
            }
            return View(tipoformula);
        }

        //
        // GET: /Plantilla/TipoFormula/Create

        public ActionResult Create()
        {
            ViewBag.IdTipoElemento = new SelectList(db.TipoElementos.OrderBy(t => t.Nombre), "Id", "Nombre");
            return View();
        }

        //
        // POST: /Plantilla/TipoFormula/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TipoFormula tipoformula)
        {
            if (ModelState.IsValid)
            {
                db.TipoFormulas.Add(tipoformula);
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }

            ViewBag.IdTipoElemento = new SelectList(db.TipoElementos.OrderBy(t => t.Nombre), "Id", "Nombre", tipoformula.IdTipoElemento);
            return View(tipoformula);
        }

        //
        // GET: /Plantilla/TipoFormula/Edit/5

        public ActionResult Edit(int id = 0)
        {
            TipoFormula tipoformula = db.TipoFormulas.Find(id);
            if (tipoformula == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.IdTipoElemento = new SelectList(db.TipoElementos.OrderBy(t => t.Nombre), "Id", "Nombre", tipoformula.IdTipoElemento); 
            return View(tipoformula);
        }

        //
        // POST: /Plantilla/TipoFormula/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TipoFormula tipoformula)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoformula).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.IdTipoElemento = new SelectList(db.TipoElementos.OrderBy(t => t.Nombre), "Id", "Nombre", tipoformula.IdTipoElemento);
            return View(tipoformula);
        }

        //
        // GET: /Plantilla/TipoFormula/Delete/5

        public ActionResult Delete(int id = 0)
        {
            TipoFormula tipoformula = db.TipoFormulas.Find(id);
            tipoformula.TipoElemento = db.TipoElementos.Find(tipoformula.IdTipoElemento);
            if (tipoformula == null)
            {
                return HttpNotFound();
            }
            return View(tipoformula);
        }

        //
        // POST: /Plantilla/TipoFormula/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoFormula tipoformula = db.TipoFormulas.Find(id);
            db.TipoFormulas.Remove(tipoformula);
            db.SaveChanges();  
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
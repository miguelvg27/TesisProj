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
    public class TipoDatoController : Controller
    {
        private TProjContext db = new TProjContext();

        //
        // GET: /Plantilla/TipoDato/

        public ActionResult Index()
        {
            return View(db.TipoDatos.OrderBy(p => p.Nombre).ToList());
        }

        //
        // GET: /Plantilla/TipoDato/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Plantilla/TipoDato/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TipoDato tipodato)
        {
            if (ModelState.IsValid)
            {
                db.TipoDatosRequester.AddElement(tipodato);     
                return RedirectToAction("Index");
            }
            return View(tipodato);
        }

        //
        // GET: /Plantilla/TipoDato/Edit/5

        public ActionResult Edit(int id = 0)
        {
            TipoDato tipodato = db.TipoDatos.Find(id);
            if (tipodato == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }
            return View(tipodato);
        }

        //
        // POST: /Plantilla/TipoDato/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TipoDato tipodato)
        {
            if (ModelState.IsValid)
            {
                db.TipoDatosRequester.ModifyElement(tipodato);
                return RedirectToAction("Index");
            }
            return View(tipodato);
        }

        //
        // GET: /Plantilla/TipoDato/Delete/5

         public ActionResult Delete(int id)
        {
            TipoDato tipodato = db.TipoDatos.Find(id);
            if (tipodato == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            try
            {
                db.TipoDatosRequester.RemoveElementByID(id);
            }
            catch (Exception)
            {
                ModelState.AddModelError("ErrorIndex", "No se puede eliminar porque existen registros dependientes.");
                var tipodatos = db.TipoDatos.OrderBy(p => p.Nombre);
                return View("Index", tipodatos.ToList());
            }
 
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
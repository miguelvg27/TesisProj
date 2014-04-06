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
    public class TipoElementoController : Controller
    {
        private TProjContext db = new TProjContext();

        //
        // GET: /Plantilla/TipoElemento/

        public ActionResult Index()
        {
            return View(db.TipoElementos.OrderBy(t => t.Nombre).ToList());
        }


        //
        // GET: /Plantilla/TipoElemento/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Plantilla/TipoElemento/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TipoElemento tipoelemento)
        {
            if (ModelState.IsValid)
            {
                db.TipoElementosRequester.AddElement(tipoelemento);
                return RedirectToAction("Index");
            }

            return View(tipoelemento);
        }

        //
        // GET: /Plantilla/TipoElemento/Edit/5

        public ActionResult Edit(int id = 0)
        {
            TipoElemento tipoelemento = db.TipoElementos.Find(id);
            if (tipoelemento == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            return View(tipoelemento);
        }

        //
        // POST: /Plantilla/TipoElemento/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TipoElemento tipoelemento)
        {
            if (ModelState.IsValid)
            {
                db.TipoElementosRequester.ModifyElement(tipoelemento);
                return RedirectToAction("Index");
            }

            return View(tipoelemento);
        }

        //
        // GET: /Plantilla/TipoElemento/Delete/5

        public ActionResult Delete(int id = 0)
        {
            TipoElemento tipoelemento = db.TipoElementos.Find(id);
            if (tipoelemento == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            try
            {
                db.TipoElementosRequester.RemoveElementByID(id);
            }
            catch (Exception)
            {
                ModelState.AddModelError("ErrorIndex", "No se puede eliminar porque existen registros dependientes.");
                var tipoelementos = db.TipoElementos.OrderBy(p => p.Nombre);
                return View("Index", tipoelementos.ToList());
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
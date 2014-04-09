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
    public class ConstanteController : Controller
    {
        private TProjContext db = new TProjContext();

        //
        // GET: /Plantilla/Constante/

        public ActionResult Index()
        {
            return View(db.Constantes.OrderBy(p => p.Nombre).ToList());
        }

        //
        // GET: /Plantilla/Constante/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Plantilla/Constante/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Constante constante)
        {
            if (ModelState.IsValid)
            {
                db.ConstantesRequester.AddElement(constante);     
                return RedirectToAction("Index");
            }
            return View(constante);
        }

        //
        // GET: /Plantilla/Constante/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Constante constante = db.Constantes.Find(id);
            if (constante == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }
            return View(constante);
        }

        //
        // POST: /Plantilla/Constante/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Constante constante)
        {
            if (ModelState.IsValid)
            {
                db.ConstantesRequester.ModifyElement(constante);
                return RedirectToAction("Index");
            }
            return View(constante);
        }

        //
        // GET: /Plantilla/Constante/Delete/5

         public ActionResult Delete(int id = 0)
        {
            Constante constante = db.Constantes.Find(id);
            if (constante == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }
            db.ConstantesRequester.RemoveElementByID(id);

 
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
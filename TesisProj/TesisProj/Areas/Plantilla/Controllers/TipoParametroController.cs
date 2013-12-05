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
    public class TipoParametroController : Controller
    {
        private TProjContext db = new TProjContext();

        //
        // GET: /Plantilla/TipoParametro/

        public ActionResult Index()
        {
            return View(db.TipoParametros.OrderBy(p => p.Nombre).ToList());
        }

        //
        // GET: /Plantilla/TipoParametro/Details/5

        public ActionResult Details(int id = 0)
        {
            TipoParametro tipoparametro = db.TipoParametros.Find(id);
            if (tipoparametro == null)
            {
                return HttpNotFound();
            }
            return View(tipoparametro);
        }

        //
        // GET: /Plantilla/TipoParametro/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Plantilla/TipoParametro/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TipoParametro tipoparametro)
        {
            if (ModelState.IsValid)
            {
                db.TipoParametros.Add(tipoparametro);
                db.SaveChanges();     
                return RedirectToAction("Index");
            }
            return View(tipoparametro);
        }

        //
        // GET: /Plantilla/TipoParametro/Edit/5

        public ActionResult Edit(int id = 0)
        {
            TipoParametro tipoparametro = db.TipoParametros.Find(id);
            if (tipoparametro == null)
            {
                return HttpNotFound();
            }      
            return View(tipoparametro);
        }

        //
        // POST: /Plantilla/TipoParametro/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TipoParametro tipoparametro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoparametro).State = EntityState.Modified;
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }    
            return View(tipoparametro);
        }

        //
        // GET: /Plantilla/TipoParametro/Delete/5
         public ActionResult Delete(int id)
        {
            TipoParametro tipoparametro = db.TipoParametros.Find(id);
            try
            {
                db.TipoParametros.Remove(tipoparametro);
                db.SaveChanges();   
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No se puede eliminar porque existen registros dependientes.");
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
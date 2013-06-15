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
    public class ParametroController : Controller
    {
        private TProjContext db = new TProjContext();

        //
        // GET: /Plantilla/Parametro/

        public ActionResult Index(int id = 0)
        {
            var parametros = db.Parametros.Include(p => p.PlantillaElemento).Include(p => p.TipoParametro).Where(p => id > 0? p.IdPlantillaElemento == id : true);
            return View(parametros.ToList());
        }

        //
        // GET: /Plantilla/Parametro/Details/5

        public ActionResult Details(int id = 0)
        {
            Parametro parametro = db.Parametros.Find(id);
            parametro.TipoParametro = db.TipoParametros.Find(parametro.IdTipoParametro);
            parametro.PlantillaElemento = db.PlantillaElementos.Find(parametro.IdPlantillaElemento);
            if (parametro == null)
            {
                return HttpNotFound();
            }
            return View(parametro);
        }

        //
        // GET: /Plantilla/Parametro/Create

        public ActionResult Create()
        {
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos, "Id", "Nombre");
            ViewBag.IdTipoParametro = new SelectList(db.TipoParametros, "Id", "Nombre");
            return View();
        }

        //
        // POST: /Plantilla/Parametro/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Parametro parametro)
        {
            if (ModelState.IsValid)
            {
                db.Parametros.Add(parametro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos, "Id", "Nombre", parametro.IdPlantillaElemento);
            ViewBag.IdTipoParametro = new SelectList(db.TipoParametros, "Id", "Nombre", parametro.IdTipoParametro);
            return View(parametro);
        }

        //
        // GET: /Plantilla/Parametro/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Parametro parametro = db.Parametros.Find(id);
            if (parametro == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos, "Id", "Nombre", parametro.IdPlantillaElemento);
            ViewBag.IdTipoParametro = new SelectList(db.TipoParametros, "Id", "Nombre", parametro.IdTipoParametro);
            return View(parametro);
        }

        //
        // POST: /Plantilla/Parametro/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Parametro parametro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parametro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos, "Id", "Nombre", parametro.IdPlantillaElemento);
            ViewBag.IdTipoParametro = new SelectList(db.TipoParametros, "Id", "Nombre", parametro.IdTipoParametro);
            return View(parametro);
        }

        //
        // GET: /Plantilla/Parametro/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Parametro parametro = db.Parametros.Find(id);
            parametro.TipoParametro = db.TipoParametros.Find(parametro.IdTipoParametro);
            parametro.PlantillaElemento = db.PlantillaElementos.Find(parametro.IdPlantillaElemento);
            if (parametro == null)
            {
                return HttpNotFound();
            }
            return View(parametro);
        }

        //
        // POST: /Plantilla/Parametro/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Parametro parametro = db.Parametros.Find(id);
            db.Parametros.Remove(parametro);
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
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
            PlantillaElemento plantilla = db.PlantillaElementos.Find(id);
            if (plantilla == null)
            {
                return HttpNotFound();
            }

            var parametros = db.Parametros.Include(p => p.PlantillaElemento).Include(p => p.TipoParametro).Where(p => p.IdPlantillaElemento == id).OrderBy(p => p.TipoParametro.Nombre);

            ViewBag.IdPlantilla = id;
            ViewBag.Plantilla = plantilla.Nombre;
            TipoElemento tipo = db.TipoElementos.Find(plantilla.IdTipoElemento);
            ViewBag.TipoPlantilla = tipo != null ? tipo.Nombre : "";
            
            return View(parametros.ToList());
        }

        //
        // GET: /Plantilla/Parametro/Details/5

        public ActionResult Details(int id = 0)
        {
            Parametro parametro = db.Parametros.Find(id);
            if (parametro == null)
            {
                return HttpNotFound();
            }
            
            parametro.TipoParametro = db.TipoParametros.Find(parametro.IdTipoParametro);
            parametro.PlantillaElemento = db.PlantillaElementos.Find(parametro.IdPlantillaElemento);
            
            return View(parametro);
        }

        //
        // GET: /Plantilla/Parametro/Create

        public ActionResult Create(int id = 0)
        {
            PlantillaElemento plantilla = db.PlantillaElementos.Find(id);
            if (plantilla == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdPlantilla = id;
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == id), "Id", "Nombre");
            ViewBag.IdTipoParametro = new SelectList(db.TipoParametros.OrderBy(t => t.Nombre), "Id", "Nombre");

            return View();
        }

        //
        // POST: /Plantilla/Parametro/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Parametro parametro, int id)
        {
            if (ModelState.IsValid)
            {
                db.Parametros.Add(parametro);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = id });
            }

            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == parametro.IdPlantillaElemento), "Id", "Nombre", parametro.IdPlantillaElemento);
            ViewBag.IdTipoParametro = new SelectList(db.TipoParametros.OrderBy(t => t.Nombre), "Id", "Nombre", parametro.IdTipoParametro);
            
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

            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == parametro.IdPlantillaElemento), "Id", "Nombre", parametro.IdPlantillaElemento);
            ViewBag.IdTipoParametro = new SelectList(db.TipoParametros.OrderBy(t => t.Nombre), "Id", "Nombre", parametro.IdTipoParametro);
            
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
                return RedirectToAction("Index", new { id = parametro.IdPlantillaElemento });
            }
            
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == parametro.IdPlantillaElemento), "Id", "Nombre", parametro.IdPlantillaElemento);
            ViewBag.IdTipoParametro = new SelectList(db.TipoParametros.OrderBy(t => t.Nombre), "Id", "Nombre", parametro.IdTipoParametro);
            
            return View(parametro);
        }

        //
        // GET: /Plantilla/Parametro/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Parametro parametro = db.Parametros.Find(id);
            if (parametro == null)
            {
                return HttpNotFound();
            }

            parametro.TipoParametro = db.TipoParametros.Find(parametro.IdTipoParametro);
            parametro.PlantillaElemento = db.PlantillaElementos.Find(parametro.IdPlantillaElemento);
            
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
            return RedirectToAction("Index", new { id = parametro.IdPlantillaElemento });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
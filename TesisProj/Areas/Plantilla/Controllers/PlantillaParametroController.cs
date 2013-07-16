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
    public class PlantillaParametroController : Controller
    {
        private TProjContext db = new TProjContext();

        //
        // GET: /Plantilla/PlantillaParametro/

        public ActionResult Index(int id = 0)
        {
            PlantillaElemento plantilla = db.PlantillaElementos.Find(id);
            if (plantilla == null)
            {
                return HttpNotFound();
            }

            var parametros = db.PlantillaParametros.Include(p => p.PlantillaElemento).Include(p => p.TipoParametro).Where(p => p.IdPlantillaElemento == id).OrderBy(p => p.TipoParametro.Nombre);

            ViewBag.IdPlantilla = id;
            ViewBag.Plantilla = plantilla.Nombre;
            TipoElemento tipo = db.TipoElementos.Find(plantilla.IdTipoElemento);
            ViewBag.TipoPlantilla = tipo != null ? tipo.Nombre : "";
            
            return View(parametros.ToList());
        }

        //
        // GET: /Plantilla/PlantillaParametro/Details/5

        public ActionResult Details(int id = 0)
        {
            PlantillaParametro parametro = db.PlantillaParametros.Find(id);
            if (parametro == null)
            {
                return HttpNotFound();
            }
            
            parametro.TipoParametro = db.TipoParametros.Find(parametro.IdTipoParametro);
            parametro.PlantillaElemento = db.PlantillaElementos.Find(parametro.IdPlantillaElemento);
            
            return View(parametro);
        }

        //
        // GET: /Plantilla/PlantillaParametro/Create

        public ActionResult Create(int idPlantilla = 0)
        {
            PlantillaElemento plantilla = db.PlantillaElementos.Find(idPlantilla);
            if (plantilla == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdPlantilla = idPlantilla;
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == plantilla.Id), "Id", "Nombre");
            ViewBag.IdTipoParametro = new SelectList(db.TipoParametros.OrderBy(t => t.Nombre), "Id", "Nombre");
            ViewBag.Plantilla = plantilla.Nombre;

            return View();
        }

        //
        // POST: /Plantilla/PlantillaParametro/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlantillaParametro parametro)
        {
            if (ModelState.IsValid)
            {
                db.PlantillaParametros.Add(parametro);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = parametro.IdPlantillaElemento });
            }

            ViewBag.IdPlantilla = parametro.IdPlantillaElemento;
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == parametro.IdPlantillaElemento), "Id", "Nombre", parametro.IdPlantillaElemento);
            ViewBag.IdTipoParametro = new SelectList(db.TipoParametros.OrderBy(t => t.Nombre), "Id", "Nombre", parametro.IdTipoParametro);

            PlantillaElemento plantilla = db.PlantillaElementos.Find(parametro.IdPlantillaElemento);
            ViewBag.Plantilla = plantilla.Nombre;

            return View(parametro);
        }

        //
        // GET: /Plantilla/PlantillaParametro/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PlantillaParametro parametro = db.PlantillaParametros.Find(id);
            if (parametro == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == parametro.IdPlantillaElemento), "Id", "Nombre", parametro.IdPlantillaElemento);
            ViewBag.IdTipoParametro = new SelectList(db.TipoParametros.OrderBy(t => t.Nombre), "Id", "Nombre", parametro.IdTipoParametro);

            PlantillaElemento plantilla = db.PlantillaElementos.Find(parametro.IdPlantillaElemento);
            ViewBag.Plantilla = plantilla.Nombre;

            return View(parametro);
        }

        //
        // POST: /Plantilla/PlantillaParametro/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlantillaParametro parametro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parametro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = parametro.IdPlantillaElemento });
            }
            
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == parametro.IdPlantillaElemento), "Id", "Nombre", parametro.IdPlantillaElemento);
            ViewBag.IdTipoParametro = new SelectList(db.TipoParametros.OrderBy(t => t.Nombre), "Id", "Nombre", parametro.IdTipoParametro);

            PlantillaElemento plantilla = db.PlantillaElementos.Find(parametro.IdPlantillaElemento);
            ViewBag.Plantilla = plantilla.Nombre;

            return View(parametro);
        }

        //
        // GET: /Plantilla/PlantillaParametro/Delete/5

        public ActionResult Delete(int id)
        {
            PlantillaParametro parametro = db.PlantillaParametros.Find(id);
            try
            {
                db.PlantillaParametrosRequester.RemoveElementByID(parametro.Id);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No se puede eliminar porque existen registros dependientes.");
            }
            
            return RedirectToAction("Index", new { id = parametro.IdPlantillaElemento });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
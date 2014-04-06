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
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            ViewBag.IdPlantilla = id;
            ViewBag.Plantilla = plantilla.Nombre;
            ViewBag.TipoPlantilla = db.TipoElementos.Find(plantilla.IdTipoElemento).Nombre;
            
            var parametros = db.PlantillaParametros.Include(p => p.TipoDato).Where(p => p.IdPlantillaElemento == id).OrderBy(p => p.TipoDato.Nombre);
            return View(parametros.ToList());
        }

        //
        // GET: /Plantilla/PlantillaParametro/Create

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
            ViewBag.IdTipoDato = new SelectList(db.TipoDatos.OrderBy(t => t.Nombre), "Id", "Nombre");

            return View();
        }

        //
        // POST: /Plantilla/PlantillaParametro/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlantillaParametro plantillaparametro)
        {
            if (ModelState.IsValid)
            {
                db.PlantillaParametrosRequester.AddElement(plantillaparametro);
                return RedirectToAction("Index", new { id = plantillaparametro.IdPlantillaElemento });
            }

            PlantillaElemento plantilla = db.PlantillaElementos.Find(plantillaparametro.IdPlantillaElemento);

            ViewBag.IdPlantilla = plantilla.Id;
            ViewBag.Plantilla = plantilla.Nombre;
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == plantilla.Id), "Id", "Nombre", plantillaparametro.IdPlantillaElemento);
            ViewBag.IdTipoDato = new SelectList(db.TipoDatos.OrderBy(t => t.Nombre), "Id", "Nombre", plantillaparametro.IdTipoDato);

            return View(plantillaparametro);
        }

        //
        // GET: /Plantilla/PlantillaParametro/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PlantillaParametro plantillaparametro = db.PlantillaParametros.Find(id);
            if (plantillaparametro == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            PlantillaElemento plantilla = db.PlantillaElementos.Find(plantillaparametro.IdPlantillaElemento);

            ViewBag.IdPlantilla = plantilla.Id;
            ViewBag.Plantilla = plantilla.Nombre;
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == plantilla.Id), "Id", "Nombre",  plantillaparametro.IdPlantillaElemento);
            ViewBag.IdTipoDato = new SelectList(db.TipoDatos.OrderBy(t => t.Nombre), "Id", "Nombre", plantillaparametro.IdTipoDato);

            return View(plantillaparametro);
        }

        //
        // POST: /Plantilla/PlantillaParametro/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlantillaParametro plantillaparametro)
        {
            if (ModelState.IsValid)
            {
                db.PlantillaParametrosRequester.ModifyElement(plantillaparametro);
                return RedirectToAction("Index", new { id = plantillaparametro.IdPlantillaElemento });
            }
            
            PlantillaElemento plantilla = db.PlantillaElementos.Find(plantillaparametro.IdPlantillaElemento);

            ViewBag.IdPlantilla = plantilla.Id;
            ViewBag.Plantilla = plantilla.Nombre;
            ViewBag.IdPlantillaElemento = new SelectList(db.PlantillaElementos.Where(p => p.Id == plantilla.Id), "Id", "Nombre",  plantillaparametro.IdPlantillaElemento);
            ViewBag.IdTipoDato = new SelectList(db.TipoDatos.OrderBy(t => t.Nombre), "Id", "Nombre", plantillaparametro.IdTipoDato);

            return View(plantillaparametro);
        }

        //
        // GET: /Plantilla/PlantillaParametro/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PlantillaParametro plantillaparametro = db.PlantillaParametros.Find(id);
            if (plantillaparametro == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            db.PlantillaParametrosRequester.RemoveElementByID(plantillaparametro.Id);
 
            return RedirectToAction("Index", new { id = plantillaparametro.IdPlantillaElemento });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
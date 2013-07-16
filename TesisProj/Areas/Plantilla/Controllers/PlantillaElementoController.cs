﻿using System;
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
    public class PlantillaElementoController : Controller
    {
        private TProjContext db = new TProjContext();

        //
        // GET: /Plantilla/PlantillaElemento/
        public ActionResult Index()
        {
            var plantillaelementos = db.PlantillaElementos.Include(p => p.TipoElemento).OrderBy(p => p.TipoElemento.Nombre);
            return View(plantillaelementos.ToList());
        }

        //
        // GET: /Plantilla/PlantillaElemento/Details/5

        public ActionResult Details(int id = 0)
        {
            PlantillaElemento plantillaelemento = db.PlantillaElementos.Find(id);
            if (plantillaelemento == null)
            {
                return HttpNotFound();
            }

            plantillaelemento.TipoElemento = db.TipoElementos.Find(plantillaelemento.IdTipoElemento);
            return View(plantillaelemento);
        }

        //
        // GET: /Plantilla/PlantillaElemento/Create

        public ActionResult Create()
        {
            ViewBag.IdTipoElemento = new SelectList(db.TipoElementos.OrderBy(t => t.Nombre), "Id", "Nombre");
            return View();
        }

        //
        // POST: /Plantilla/PlantillaElemento/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlantillaElemento plantillaelemento)
        {
            if (ModelState.IsValid)
            {
                db.PlantillaElementos.Add(plantillaelemento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdTipoElemento = new SelectList(db.TipoElementos.OrderBy(t => t.Nombre), "Id", "Nombre", plantillaelemento.IdTipoElemento);
            return View(plantillaelemento);
        }

        //
        // GET: /Plantilla/PlantillaElemento/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PlantillaElemento plantillaelemento = db.PlantillaElementos.Find(id);
            if (plantillaelemento == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdTipoElemento = new SelectList(db.TipoElementos.OrderBy(t => t.Nombre), "Id", "Nombre", plantillaelemento.IdTipoElemento);
            return View(plantillaelemento);
        }

        //
        // POST: /Plantilla/PlantillaElemento/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlantillaElemento plantillaelemento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(plantillaelemento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdTipoElemento = new SelectList(db.TipoElementos.OrderBy(t => t.Nombre), "Id", "Nombre", plantillaelemento.IdTipoElemento);
            return View(plantillaelemento);
        }

        //
        // GET: /Plantilla/PlantillaElemento/Delete/5

        public ActionResult Delete(int id)
        {
            PlantillaElemento plantillaelemento = db.PlantillaElementos.Find(id);
            try
            {
                var formulas = db.PlantillaFormulas.Where(f => f.IdPlantillaElemento == plantillaelemento.Id).OrderByDescending(f => f.Secuencia).ToList();

                foreach (PlantillaFormula formula in formulas)
                {
                    db.PlantillaFormulasRequester.RemoveElementByID(formula.Id);
                }

                var parametros = db.PlantillaParametrosRequester.Where(p => p.IdPlantillaElemento == plantillaelemento.Id).ToList();

                foreach (PlantillaParametro parametro in parametros)
                {
                    db.PlantillaParametrosRequester.RemoveElementByID(parametro.Id);
                }

                db.PlantillaElementosRequester.RemoveElementByID(plantillaelemento.Id);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No se puede eliminar porque existen registros dependientes.");
                plantillaelemento.TipoElemento = db.TipoElementos.Find(plantillaelemento.IdTipoElemento);
                return View("Delete", plantillaelemento);
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
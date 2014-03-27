using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Plantilla.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Plantilla.Controllers
{
    [Authorize(Roles = "nav")]
    public class AnonPlantillaController : Controller
    {
        private TProjContext db = new TProjContext();

        public ActionResult EditProyecto(int id = 0, int idProyecto = 0)
        {
            PlantillaProyecto plantillaproyecto = db.PlantillaProyectos.Find(id);
            if (plantillaproyecto == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdProyecto = idProyecto;
            return View(plantillaproyecto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProyecto(PlantillaProyecto plantillaproyecto, int idProyecto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(plantillaproyecto).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Cine", "Proyecto", new { Area = "Modelo", id = idProyecto });
            }

            ViewBag.IdProyecto = idProyecto;
            return View(plantillaproyecto);
        }

        public ActionResult EditElemento(int id = 0, int idElemento = 0)
        {
            PlantillaElemento plantillaelemento = db.PlantillaElementos.Find(id);
            if (plantillaelemento == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdElemento = idElemento;

            return View(plantillaelemento);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditElemento(PlantillaElemento plantillaelemento, int idElemento = 0)
        {
            if (ModelState.IsValid)
            {
                db.Entry(plantillaelemento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Programa", "Proyecto", new { Area = "Modelo", id = idElemento });
            }

            ViewBag.IdElemento = idElemento;

            return View(plantillaelemento);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}

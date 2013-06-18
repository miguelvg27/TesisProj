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
    [Authorize]
    public class PlantillaProyectoController : Controller
    {
        private TProjContext db = new TProjContext();

        //
        // GET: /Plantilla/PlantillaProyecto/

        public ActionResult Index()
        {
            return View(db.PlantillaProyectos.OrderBy(t => t.Nombre).ToList());
        }

        //
        // GET: /Plantilla/PlantillaProyecto/Details/5

        public ActionResult Details(int id = 0)
        {
            PlantillaProyecto plantillaproyecto = db.PlantillaProyectos.Find(id);
            if (plantillaproyecto == null)
            {
                return HttpNotFound();
            }

            return View(plantillaproyecto);
        }

        //
        // GET: /Plantilla/PlantillaProyecto/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Plantilla/PlantillaProyecto/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlantillaProyecto plantillaproyecto)
        {
            if (ModelState.IsValid)
            {
                db.PlantillaProyectos.Add(plantillaproyecto);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(plantillaproyecto);
        }

        //
        // GET: /Plantilla/PlantillaProyecto/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PlantillaProyecto plantillaproyecto = db.PlantillaProyectos.Find(id);
            if (plantillaproyecto == null)
            {
                return HttpNotFound();
            }

            return View(plantillaproyecto);
        }

        //
        // POST: /Plantilla/PlantillaProyecto/Assoc/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlantillaProyecto plantillaproyecto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(plantillaproyecto).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(plantillaproyecto);
        }

        public ActionResult Assoc(int id = 0)
        {
            PlantillaProyecto plantillaproyecto = db.PlantillaProyectos.Find(id);
            if (plantillaproyecto == null)
            {
                return HttpNotFound();
            }

            var asociados = db.PlantillaElementoProyectos.Include(p => p.Elemento).Where(p => p.IdProyecto == plantillaproyecto.Id).Select(p => p.Elemento).OrderBy(e => e.Nombre);
            var opciones = db.PlantillaElementos.OrderBy(p => p.Nombre).Except(asociados);
            ViewBag.Asociados = new MultiSelectList(asociados.ToList(), "Id", "Nombre");
            ViewBag.Opciones = new MultiSelectList(opciones.ToList(), "Id", "Nombre");

            return View(plantillaproyecto);
        }

        //
        // POST: /Plantilla/PlantillaProyecto/Assoc/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assoc(PlantillaProyecto plantillaproyecto, FormCollection form, string add, string remove, string addall, string removeall)
        {
            string seleccionados;
            int idElemento;
            int idProyecto = plantillaproyecto.Id;

            seleccionados = form["Opciones"];
            if(!string.IsNullOrEmpty(add) && !string.IsNullOrEmpty(seleccionados))
            {
                foreach(string sIdElemento in seleccionados.Split(','))
                {
                    idElemento = int.Parse(sIdElemento);
                    if(!db.PlantillaElementoProyectos.Any(p => p.IdProyecto == idProyecto && p.IdElemento == idElemento))
                    {
                        db.PlantillaElementoProyectos.Add(new PlantillaElementoProyecto { IdProyecto = idProyecto, IdElemento = idElemento });
                        db.SaveChanges();
                    }
                }
            }

            seleccionados = form["Asociados"];
            if (!string.IsNullOrEmpty(remove) && !string.IsNullOrEmpty(seleccionados))
            {
                
                PlantillaElementoProyecto elemento;
                foreach (string sIdElemento in seleccionados.Split(','))
                {
                    idElemento = int.Parse(sIdElemento);
                    elemento = db.PlantillaElementoProyectos.FirstOrDefault(p => p.IdElemento == idElemento && p.IdProyecto == idProyecto);
                    if (elemento != null)
                    {
                        db.PlantillaElementoProyectos.Remove(elemento);
                        db.SaveChanges();
                    }
                }
            }

            if (!string.IsNullOrEmpty(addall))
            {
                var plantillas = db.PlantillaElementos.ToList();
                foreach(PlantillaElemento elemento in plantillas)
                {
                    idElemento = elemento.Id;
                    if (!db.PlantillaElementoProyectos.Any(p => p.IdProyecto == idProyecto && p.IdElemento == idElemento))
                    {
                        db.PlantillaElementoProyectos.Add(new PlantillaElementoProyecto { IdProyecto = idProyecto, IdElemento = idElemento });
                        db.SaveChanges();
                    }
                }
            }

            if (!string.IsNullOrEmpty(removeall))
            {
                var plantillas = db.PlantillaElementoProyectos.Where(p => p.IdProyecto == idProyecto).ToList();
                foreach (PlantillaElementoProyecto elemento in plantillas)
                {
                    db.PlantillaElementoProyectos.Remove(elemento);
                    db.SaveChanges();
                }
            }        

            var asociados = db.PlantillaElementoProyectos.Include(p => p.Elemento).Where(p => p.IdProyecto == idProyecto).Select(p => p.Elemento).OrderBy(e => e.Nombre);
            var opciones = db.PlantillaElementos.OrderBy(p => p.Nombre).Except(asociados);
            ViewBag.Asociados = new MultiSelectList(asociados.ToList(), "Id", "Nombre");
            ViewBag.Opciones = new MultiSelectList(opciones.ToList(), "Id", "Nombre");

            return View(plantillaproyecto);
        }

        //
        // GET: /Plantilla/PlantillaProyecto/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PlantillaProyecto plantillaproyecto = db.PlantillaProyectos.Find(id);
            if (plantillaproyecto == null)
            {
                return HttpNotFound();
            }

            return View(plantillaproyecto);
        }

        //
        // POST: /Plantilla/PlantillaProyecto/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlantillaProyecto plantillaproyecto = db.PlantillaProyectos.Find(id);
            db.PlantillaProyectos.Remove(plantillaproyecto);
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
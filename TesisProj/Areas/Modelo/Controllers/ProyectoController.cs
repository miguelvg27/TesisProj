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

namespace TesisProj.Areas.Modelo.Controllers
{
    [Authorize]
    public partial class ProyectoController : Controller
    {
        private TProjContext db = new TProjContext();

        //
        // GET: /Modelo/Proyecto/

        public ActionResult Index()
        {
            var proyectos = db.Proyectos.Include(p => p.Creador).Include(p => p.Modificador);
            return View(proyectos.ToList());
        }

        //
        // GET: /Modelo/Console/5

        public ActionResult Console(int id = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            proyecto.Creador = db.UserProfiles.Find(proyecto.IdCreador);
            proyecto.Modificador = db.UserProfiles.Find(proyecto.IdModificador);
            return View(proyecto);
        }

        //
        // GET: /Modelo/Proyecto/Details/5

        public ActionResult Details(int id = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            proyecto.Creador = db.UserProfiles.Find(proyecto.IdCreador);
            proyecto.Modificador = db.UserProfiles.Find(proyecto.IdModificador);
            return View(proyecto);
        }

        //
        // GET: /Modelo/Proyecto/Create

        public ActionResult Create()
        {
            ViewBag.IdCreador = new SelectList(db.UserProfiles.Where(u => u.UserName == User.Identity.Name), "UserId", "UserName");
            ViewBag.IdModificador = new SelectList(db.UserProfiles.Where(u => u.UserName == User.Identity.Name), "UserId", "UserName");
            ViewBag.Now = DateTime.Today.ToShortDateString();
            ViewBag.Version = 1;
            return View();
        }

        //
        // POST: /Modelo/Proyecto/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                db.Proyectos.Add(proyecto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCreador = new SelectList(db.UserProfiles.Where(u => u.UserName == User.Identity.Name), "UserId", "UserName", proyecto.IdCreador);
            ViewBag.IdModificador = new SelectList(db.UserProfiles.Where(u => u.UserName == User.Identity.Name), "UserId", "UserName", proyecto.IdModificador);
            return View(proyecto);
        }

        //
        // GET: /Modelo/Proyecto/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCreador = new SelectList(db.UserProfiles.Where(u => u.UserId == proyecto.IdCreador), "UserId", "UserName", proyecto.IdCreador);
            ViewBag.IdModificador = new SelectList(db.UserProfiles.Where(u => u.UserId == proyecto.IdModificador), "UserId", "UserName", proyecto.IdModificador);
            return View(proyecto);
        }

        //
        // POST: /Modelo/Proyecto/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proyecto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Console", new { id = proyecto.Id });
            }
            ViewBag.IdCreador = new SelectList(db.UserProfiles.Where(u => u.UserId == proyecto.IdCreador), "UserId", "UserName", proyecto.IdCreador);
            ViewBag.IdModificador = new SelectList(db.UserProfiles.Where(u => u.UserId == proyecto.IdModificador), "UserId", "UserName", proyecto.IdModificador);
            return View(proyecto);
        }

        //
        // GET: /Modelo/Proyecto/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            proyecto.Creador = db.UserProfiles.Find(proyecto.IdCreador);
            proyecto.Modificador = db.UserProfiles.Find(proyecto.IdModificador);
            return View(proyecto);
        }

        //
        // POST: /Modelo/Proyecto/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            try
            {
                db.Proyectos.Remove(proyecto);
                db.SaveChanges();
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No se puede eliminar porque existen registros dependientes.");
                return View("Delete", proyecto);
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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Seguridad.Models;
using TesisProj.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Seguridad.Controllers
{
    [Authorize]
    public class ColaboradoresController : Controller
    {
        private TProjContext db = new TProjContext();
        private int userId = 0;
        //
        // GET: /Seguridad/Default1/

        private int getUserId()
        {
            if (userId < 1)
            {
                try
                {
                    userId = db.UserProfiles.First(u => u.UserName == User.Identity.Name).UserId;
                }
                catch (Exception)
                {
                }
            }

            return userId;
        }

        public ActionResult Index(int id)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            if(proyecto == null)
            {
                return HttpNotFound();
            }
            var colaboradors = db.Colaboradores.Include(c => c.Usuario).Include(c => c.Proyecto).Where(c => c.IdProyecto == proyecto.Id);
            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.ProyectoId = proyecto.Id;

            return View(colaboradors.ToList());
        }

        //
        // GET: /Seguridad/Default1/Create

        public ActionResult Create(int IdProyecto)
        {
            Proyecto proyecto = db.Proyectos.Find(IdProyecto);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.IdProyectoReturn = proyecto.Id;
            ViewBag.IdUsuario = new SelectList(db.UserProfiles, "UserId", "UserName");
            ViewBag.IdProyecto = new SelectList(db.Proyectos.Where(p => p.Id == IdProyecto), "Id", "Nombre", IdProyecto);
            return View();
        }

        //
        // POST: /Seguridad/Default1/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int idProyecto, string colaborador)
        {
            Proyecto proyecto = db.Proyectos.Find(idProyecto);
            string creador = db.UserProfiles.Where(u => u.UserId == proyecto.IdCreador).First().UserName;

            if (!db.UserProfiles.Any(u => u.UserName.Equals(colaborador)))
            {
                ModelState.AddModelError("IdUsuario", "No existe este nombre de usuario");
            }
            if (creador.Equals(colaborador) || db.Colaboradores.Include(c => c.Usuario).Any(c => c.Usuario.UserName.Equals(colaborador)))
            {
                ModelState.AddModelError("IdUsuario", "El usuario ya ha sido agregado al proyecto.");
            }
            
            if (ModelState.IsValid)
            {
                int idUsuario = db.UserProfiles.First(u => u.UserName.Equals(colaborador)).UserId;

                Colaborador uColaborador = new Colaborador { IdProyecto = idProyecto, IdUsuario = idUsuario, SoloLectura = true };
                uColaborador.Usuario = db.UserProfiles.Find(idUsuario);

                db.ColaboradoresRequester.AddElement(uColaborador, true, idProyecto, getUserId());

                return RedirectToAction("Index", new { id = idProyecto });
            }

            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.IdProyectoReturn = proyecto.Id;
            ViewBag.IdUsuario = new SelectList(db.UserProfiles, "UserId", "UserName");
            ViewBag.IdProyecto = new SelectList(db.Proyectos.Where(p => p.Id == idProyecto), "Id", "Nombre", idProyecto);
            return View();
        }

        //
        // GET: /Seguridad/Default1/Edit/5

        public ActionResult Edit(int idColaborador, bool policy)
        {
            db.Configuration.ProxyCreationEnabled = false;
            Colaborador colaborador = db.Colaboradores.AsNoTracking().First(c => c.Id == idColaborador);
            colaborador.Usuario = db.UserProfiles.Find(colaborador.IdUsuario);
            if (ModelState.IsValid)
            {
                colaborador.SoloLectura = policy;
                db.ColaboradoresRequester.ModifyElement(colaborador, true, colaborador.IdProyecto, getUserId());

                db.Configuration.ProxyCreationEnabled = true;
                return RedirectToAction("Index", new { id = colaborador.IdProyecto });
            }

            db.Configuration.ProxyCreationEnabled = true;
            return RedirectToAction("Index", new { id = colaborador.IdProyecto });
        }

        //
        // GET: /Seguridad/Default1/Delete/5

        public ActionResult Delete(int id)
        {
            Colaborador colaborador = db.Colaboradores.Find(id);

            if (colaborador == null)
            {
                return HttpNotFound();
            }

            db.ColaboradoresRequester.RemoveElementByID(colaborador.Id, true, true, colaborador.IdProyecto, getUserId());

            return RedirectToAction("Index", new { id = colaborador.IdProyecto });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
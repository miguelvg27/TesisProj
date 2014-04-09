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
    [Authorize(Roles = "nav")]
    public class ColaboradoresController : Controller
    {
        private TProjContext db = new TProjContext();
        private int userId = 0;

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

        // Permisos: Creador
        // GET: /Seguridad/Colaboradores/Index/5

        public ActionResult Index(int id = 0)
        {
            // Check url
            Proyecto proyecto = db.Proyectos.Find(id);
            if (proyecto == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Check user
            int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == proyecto.Id);
            if (current == null || !current.Creador)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.ProyectoId = proyecto.Id;

            var colaboradores = db.Colaboradores.Include(c => c.Usuario).Include(c => c.Proyecto).Where(c => c.IdProyecto == proyecto.Id);
            return View(colaboradores.ToList());
        }

        // Permisos: Creador
        // GET: /Seguridad/Colaboradores/Create?idProyecto=5

        public ActionResult Create(int idProyecto = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(idProyecto);
            if (proyecto == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == proyecto.Id);
            if (current == null || !current.Creador)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.IdProyectoReturn = proyecto.Id;
            ViewBag.IdUsuario = new SelectList(db.UserProfiles, "UserId", "UserName");
            ViewBag.IdProyecto = new SelectList(db.Proyectos.Where(p => p.Id == idProyecto), "Id", "Nombre", idProyecto);
            return View();
        }

        // Permisos: Creador
        // POST: /Seguridad/Colaboradores/Create?idProyecto=5&strColaborador="user"

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int idProyecto = 0, string strColaborador = "")
        {
            Proyecto proyecto = db.Proyectos.Find(idProyecto);
            if (!db.UserProfiles.Any(u => u.UserName.Equals(strColaborador)))
            {
                ModelState.AddModelError("IdUsuario", "No existe este nombre de usuario");
            }
                        
            if (ModelState.IsValid)
            {
                UserProfile usuario = db.UserProfiles.First(u => u.UserName.Equals(strColaborador));
                Colaborador colaborador = new Colaborador { IdProyecto = proyecto.Id, IdUsuario = usuario.UserId, SoloLectura = true, Creador = false };
                colaborador.Usuario = usuario; // Logging issues

                db.ColaboradoresRequester.AddElement(colaborador, true, proyecto.Id, getUserId());
                return RedirectToAction("Index", new { id = proyecto.Id });
            }

            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.IdProyectoReturn = proyecto.Id;
            ViewBag.IdUsuario = new SelectList(db.UserProfiles, "UserId", "UserName");
            ViewBag.IdProyecto = new SelectList(db.Proyectos.Where(p => p.Id == idProyecto), "Id", "Nombre", idProyecto);
            return View();
        }

        // Permisos: Creador
        // GET: /Seguridad/Colaboradores/Edit?idColaborador=5&idProyecto=5&policy=true

        public ActionResult Edit(int idColaborador = 0, int idProyecto = 0, bool policy = false)
        {
            Proyecto proyecto = db.Proyectos.Find(idProyecto);
            Colaborador colaborador = db.Colaboradores.Include(c => c.Usuario).FirstOrDefault(c => c.Id == idColaborador && c.IdProyecto == idProyecto);
            if (proyecto == null || colaborador == null || colaborador.IdUsuario == proyecto.IdCreador || colaborador.Creador)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == proyecto.Id);
            if (current == null || !current.Creador)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            colaborador.SoloLectura = policy;
            db.ColaboradoresRequester.ModifyElement(colaborador, true, colaborador.IdProyecto, getUserId());

            return RedirectToAction("Index", new { id = colaborador.IdProyecto });
        }

        // Permisos: Creador
        // GET: /Seguridad/Colaboradores/Delete?idColaborador=5&idProyecto=5

        public ActionResult Delete(int idColaborador = 0, int idProyecto = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(idProyecto);
            Colaborador colaborador = db.Colaboradores.FirstOrDefault(c => c.Id == idColaborador && c.IdProyecto == idProyecto);
            if (proyecto == null || colaborador == null || colaborador.IdUsuario == proyecto.IdCreador || colaborador.Creador)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == proyecto.Id);
            if (current == null || !current.Creador)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
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
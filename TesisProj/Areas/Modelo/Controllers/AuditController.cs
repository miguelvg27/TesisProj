using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Seguridad.Models;

namespace TesisProj.Areas.Modelo.Controllers
{
    public partial class ProyectoController : Controller
    {
        // Permisos: Creador, Editor
        // GET: /Modelo/Audit/Muro/5

        public ActionResult Muro(int id = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            if (proyecto == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Check user
            int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == proyecto.Id);
            if (current == null || current.SoloLectura)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.ProyectoId = proyecto.Id;

            var audits = db.Audits.Include(a => a.Usuario).Where(a => a.IdProyecto == proyecto.Id).OrderByDescending(a => a.Fecha);
            return View(audits.ToList());
        }

        // Permisos: Creador, Editor
        // GET: /Modelo/Audit/Muro/5

        public ActionResult Fisco(int id = 0)
        {
            Audit audit = db.Audits.Include(a => a.Usuario).FirstOrDefault(a => a.Id == id);
            if (audit == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Check user
            int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == audit.IdProyecto);
            if (current == null || current.SoloLectura)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            Proyecto proyecto = db.Proyectos.Find(audit.IdProyecto);
            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.IdProyecto = proyecto.Id;

            return View(audit);
        }

    }
}

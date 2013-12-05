using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Modelo.Models;

namespace TesisProj.Areas.Modelo.Controllers
{
    public partial class ProyectoController : Controller
    {
        //
        // GET: /Modelo/Audit/

        public ActionResult Muro(int id = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }

            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.ProyectoId = proyecto.Id;

            var audits = db.Audits.Include("Usuario").Where(a => a.IdProyecto == proyecto.Id).OrderByDescending(a => a.Fecha);

            return View(audits.ToList());
        }

        public ActionResult Fisco(int id = 0)
        {
            Audit audit = db.Audits.Find(id);
            if (audit == null)
            {
                return HttpNotFound();
            }
            audit.Usuario = db.UserProfiles.Find(audit.IdUsuario);
            Proyecto proyecto = db.Proyectos.Find(audit.IdProyecto);
            ViewBag.Proyecto = proyecto.Nombre;

            return View(audit);
        }

    }
}

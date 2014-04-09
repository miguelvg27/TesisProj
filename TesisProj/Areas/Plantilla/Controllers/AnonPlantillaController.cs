using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Plantilla.Models;
using TesisProj.Areas.Seguridad.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Plantilla.Controllers
{
    [Authorize(Roles = "nav")]
    public class AnonPlantillaController : Controller
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
                    return 0;
                }
            }

            return userId;
        }

        //
        // GET: /Plantilla/AnonPlantilla/EditProyecto/5&idProyecto=10

        public ActionResult EditProyecto(int id = 0, int idProyecto = 0)
        {
            PlantillaProyecto plantillaproyecto = db.PlantillaProyectos.Find(id);
            Proyecto proyecto = db.Proyectos.Find(idProyecto); 
            if (plantillaproyecto == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Get project and check user
            int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == proyecto.Id);
            if (current == null || current.SoloLectura)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            ViewBag.IdProyecto = idProyecto;
            return View(plantillaproyecto);
        }

        //
        // POST: /Plantilla/AnonPlantilla/EditProyecto/5&idProyecto=10

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProyecto(PlantillaProyecto plantillaproyecto, int idProyecto)
        {
            if (ModelState.IsValid)
            {
                db.PlantillaProyectosRequester.ModifyElement(plantillaproyecto);
                return RedirectToAction("Cine", "Proyecto", new { Area = "Modelo", id = idProyecto });
            }

            ViewBag.IdProyecto = idProyecto;
            return View(plantillaproyecto);
        }

        //
        // GET: /Plantilla/AnonPlantilla/EditElemento/5&idElemento=10

        public ActionResult EditElemento(int id = 0, int idElemento = 0)
        {
            PlantillaElemento plantillaelemento = db.PlantillaElementos.Find(id);
            Elemento elemento = db.Elementos.Find(id);
            if (plantillaelemento == null || elemento == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Get project and check user
            int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == elemento.IdProyecto);
            if (current == null || current.SoloLectura)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            ViewBag.IdElemento = idElemento;
            return View(plantillaelemento);
        }

        //
        // POST: /Plantilla/AnonPlantilla/EditElemento/5&idElemento=10

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditElemento(PlantillaElemento plantillaelemento, int idElemento = 0)
        {
            if (ModelState.IsValid)
            {
                db.PlantillaElementosRequester.ModifyElement(plantillaelemento);
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

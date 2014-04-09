using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Modelo.Controllers
{
    public partial class ProyectoController : Controller
    {

        //
        // GET: /Modelo/Operacion/

        public ActionResult Corolario(int id = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            if (proyecto == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.ProyectoId = proyecto.Id;

            var operaciones = db.Operaciones.Include(o => o.Proyecto).Where(o => o.IdProyecto == proyecto.Id); 

            return View(operaciones.ToList());
        }

        //
        // GET: /Modelo/Operacion/Create

        public ActionResult CreateOperacion(int idProyecto = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(idProyecto);
            if (proyecto == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            ViewBag.IdProyecto = new SelectList(db.Proyectos.Where(p => p.Id == proyecto.Id), "Id", "Nombre", proyecto.Id);
            ViewBag.IdProyectoReturn = proyecto.Id;
            ViewBag.Proyecto = proyecto.Nombre;

            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.OperacionesGlobales, "Value", "Text");
            ViewBag.ListTipos = new SelectList(db.TipoFormulas.OrderBy(o => o.Nombre).ToList(), "Referencia", "Nombre");
            ViewBag.ListOperaciones = new SelectList(db.Operaciones.Where(f => f.IdProyecto == idProyecto).OrderBy(f => f.Nombre).ToList(), "Referencia", "Nombre");

            var operaciones = db.Operaciones.Where(f => f.IdProyecto == proyecto.Id);
            int defSecuencia = operaciones.Count() > 0 ? operaciones.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;
            
            return View();
        }

        //
        // POST: /Modelo/Operacion/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOperacion(Operacion operacion)
        {
            if (ModelState.IsValid)
            {
                db.OperacionesRequester.AddElement(operacion, true, operacion.IdProyecto, getUserId());
                return RedirectToAction("Corolario", new { id = operacion.IdProyecto });
            }

            ViewBag.IdProyecto = new SelectList(db.Proyectos, "Id", "Nombre", operacion.IdProyecto);
            ViewBag.IdProyectoReturn = operacion.IdProyecto;

            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.OperacionesGlobales, "Value", "Text");
            ViewBag.ListTipos = new SelectList(db.TipoFormulas.OrderBy(o => o.Nombre).ToList(), "Referencia", "Nombre");
            ViewBag.ListOperaciones = new SelectList(db.Operaciones.Where(f => f.IdProyecto == operacion.IdProyecto && f.Secuencia < operacion.Secuencia).OrderBy(f => f.Nombre).ToList(), "Referencia", "Nombre");

            ViewBag.Proyecto = db.Proyectos.Find(operacion.IdProyecto).Nombre;
            var operaciones = db.Operaciones.Where(f => f.IdProyecto == operacion.IdProyecto);
            int defSecuencia = operaciones.Count() > 0 ? operaciones.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;

            return View(operacion);
        }

        //
        // GET: /Modelo/Operacion/Edit/5

        public ActionResult EditOperacion(int id = 0)
        {
            Operacion operacion = db.Operaciones.Find(id);
            if (operacion == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            ViewBag.IdProyecto = new SelectList(db.Proyectos.Where(p => p.Id == operacion.IdProyecto), "Id", "Nombre", operacion.IdProyecto);
            ViewBag.Proyecto = db.Proyectos.Find(operacion.IdProyecto).Nombre;
            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.OperacionesGlobales, "Value", "Text");
            ViewBag.ListTipos = new SelectList(db.TipoFormulas.OrderBy(o => o.Nombre).ToList(), "Referencia", "Nombre");
            ViewBag.ListOperaciones = new SelectList(db.Operaciones.Where(f => f.IdProyecto == operacion.IdProyecto && f.Secuencia < operacion.Secuencia).OrderBy(f => f.Nombre).ToList(), "Referencia", "Nombre");

            return View(operacion);
        }

        //
        // POST: /Modelo/Operacion/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOperacion(Operacion operacion)
        {
            if (ModelState.IsValid)
            {
                db.OperacionesRequester.ModifyElement(operacion, true, operacion.IdProyecto, getUserId());
                return RedirectToAction("Corolario", new { id = operacion.IdProyecto });
            }

            ViewBag.IdProyecto = new SelectList(db.Proyectos.Where(p => p.Id == operacion.IdProyecto), "Id", "Nombre", operacion.IdProyecto);
            ViewBag.Proyecto = db.Proyectos.Find(operacion.IdProyecto).Nombre;
            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.OperacionesGlobales, "Value", "Text");
            ViewBag.ListTipos = new SelectList(db.TipoFormulas.OrderBy(o => o.Nombre).ToList(), "Referencia", "Nombre");
            ViewBag.ListOperaciones = new SelectList(db.Operaciones.Where(f => f.IdProyecto == operacion.IdProyecto && f.Secuencia < operacion.Secuencia).OrderBy(f => f.Nombre).ToList(), "Referencia", "Nombre");

            return View(operacion);
        }

        //
        // GET: /Modelo/Operacion/Delete/5

        public ActionResult DeleteOperacion(int id)
        {
            Operacion operacion = db.Operaciones.Find(id);

            try
            {
                var salidas = db.SalidaOperaciones.Where(s => s.IdOperacion == operacion.Id).ToList();

                foreach (SalidaOperacion salida in salidas)
                {
                    db.SalidaOperacionesRequester.RemoveElementByID(salida.Id);
                }

                db.OperacionesRequester.RemoveElementByID(operacion.Id, true, true, operacion.IdProyecto, getUserId());
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No se puede eliminar porque existen registros dependientes.");
                return View("DeleteOperacion", operacion);
            }

            return RedirectToAction("Corolario", new { id = operacion.IdProyecto });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
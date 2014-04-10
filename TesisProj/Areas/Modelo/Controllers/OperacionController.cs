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

namespace TesisProj.Areas.Modelo.Controllers
{
    public partial class ProyectoController : Controller
    {

        // Permisos: Creador, Editor
        // GET: /Modelo/Operacion/Corolario/5

        public ActionResult Corolario(int id = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            if (proyecto == null)
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

            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.ProyectoId = proyecto.Id;

            var operaciones = db.Operaciones.Include(o => o.TipoDato).Where(o => o.IdProyecto == proyecto.Id); 
            return View(operaciones.ToList());
        }

        // Permisos: Creador, Editor
        // GET: /Modelo/Operacion/Create?idProyecto=5

        public ActionResult CreateOperacion(int idProyecto = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(idProyecto);
            if (proyecto == null)
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

            ViewBag.IdProyecto = new SelectList(db.Proyectos.Where(p => p.Id == proyecto.Id), "Id", "Nombre", proyecto.Id);
            ViewBag.IdProyectoReturn = proyecto.Id;
            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.IdTipoDato = new SelectList(db.TipoDatos.OrderBy(t => t.Nombre), "Id", "Nombre");
            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.OperacionesGlobales, "Value", "Text");
            ViewBag.ConstantesList = new SelectList(db.Constantes.OrderBy(c => c.Nombre), "Valor", "Nombre");
            ViewBag.ListTipos = new SelectList(db.TipoFormulas.OrderBy(o => o.Nombre).ToList(), "Referencia", "ListName");
            ViewBag.ListOperaciones = new SelectList(db.Operaciones.Where(f => f.IdProyecto == idProyecto).OrderBy(f => f.Secuencia).ToList(), "Referencia", "ListName");

            // Begin: Get sequence

            var operaciones = db.Operaciones.Where(f => f.IdProyecto == proyecto.Id).ToList();
            int defSecuencia = operaciones.Count() > 0 ? operaciones.Max(f => f.Secuencia) + 10 : 10;
            ViewBag.defSecuencia = defSecuencia;

            // End: Get sequence

            return View();
        }

        // Permisos: Creador, Editor
        // POST: /Modelo/Operacion/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOperacion(Operacion operacion)
        {
            if (ModelState.IsValid)
            {
                operacion.TipoDato = db.TipoDatos.Find(operacion.IdTipoDato);
                db.OperacionesRequester.AddElement(operacion, true, operacion.IdProyecto, getUserId());
                return RedirectToAction("Corolario", new { id = operacion.IdProyecto });
            }

            ViewBag.IdProyecto = new SelectList(db.Proyectos, "Id", "Nombre", operacion.IdProyecto);
            ViewBag.IdProyectoReturn = operacion.IdProyecto;
            ViewBag.IdTipoDato = new SelectList(db.TipoDatos.OrderBy(t => t.Nombre), "Id", "Nombre", operacion.IdTipoDato);
            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.OperacionesGlobales, "Value", "Text");
            ViewBag.ConstantesList = new SelectList(db.Constantes.OrderBy(c => c.Nombre), "Valor", "Nombre");
            ViewBag.ListTipos = new SelectList(db.TipoFormulas.OrderBy(o => o.Nombre).ToList(), "Referencia", "ListName");
            ViewBag.ListOperaciones = new SelectList(db.Operaciones.Where(f => f.IdProyecto == operacion.IdProyecto && f.Secuencia < operacion.Secuencia).OrderBy(f => f.Secuencia).ToList(), "Referencia", "ListName");

            ViewBag.Proyecto = db.Proyectos.Find(operacion.IdProyecto).Nombre;

            return View(operacion);
        }

        // Permisos: Creador, Editor
        // GET: /Modelo/Operacion/Edit/5

        public ActionResult EditOperacion(int id = 0)
        {
            Operacion operacion = db.Operaciones.Find(id);
            if (operacion == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Get project and check user
            int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == operacion.IdProyecto);
            if (current == null || current.SoloLectura)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            ViewBag.IdProyecto = new SelectList(db.Proyectos.Where(p => p.Id == operacion.IdProyecto), "Id", "Nombre", operacion.IdProyecto);
            ViewBag.Proyecto = db.Proyectos.Find(operacion.IdProyecto).Nombre;
            ViewBag.IdTipoDato = new SelectList(db.TipoDatos.OrderBy(t => t.Nombre), "Id", "Nombre", operacion.IdTipoDato);
            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.OperacionesGlobales, "Value", "Text");
            ViewBag.ConstantesList = new SelectList(db.Constantes.OrderBy(c => c.Nombre), "Valor", "Nombre");
            ViewBag.ListTipos = new SelectList(db.TipoFormulas.OrderBy(o => o.Nombre).ToList(), "Referencia", "ListName");
            ViewBag.ListOperaciones = new SelectList(db.Operaciones.Where(f => f.IdProyecto == operacion.IdProyecto && f.Secuencia < operacion.Secuencia).OrderBy(f => f.Secuencia).ToList(), "Referencia", "ListName");

            return View(operacion);
        }

        // Permisos: Creador, Editor
        // POST: /Modelo/Operacion/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOperacion(Operacion operacion)
        {
            if (ModelState.IsValid)
            {
                operacion.TipoDato = db.TipoDatos.Find(operacion.IdTipoDato);
                db.OperacionesRequester.ModifyElement(operacion, true, operacion.IdProyecto, getUserId());
                return RedirectToAction("Corolario", new { id = operacion.IdProyecto });
            }

            ViewBag.IdProyecto = new SelectList(db.Proyectos.Where(p => p.Id == operacion.IdProyecto), "Id", "Nombre", operacion.IdProyecto);
            ViewBag.Proyecto = db.Proyectos.Find(operacion.IdProyecto).Nombre;
            ViewBag.IdTipoDato = new SelectList(db.TipoDatos.OrderBy(t => t.Nombre), "Id", "Nombre", operacion.IdTipoDato);
            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.OperacionesGlobales, "Value", "Text");
            ViewBag.ConstantesList = new SelectList(db.Constantes.OrderBy(c => c.Nombre), "Valor", "Nombre");
            ViewBag.ListTipos = new SelectList(db.TipoFormulas.OrderBy(o => o.Nombre).ToList(), "Referencia", "ListName");
            ViewBag.ListOperaciones = new SelectList(db.Operaciones.Where(f => f.IdProyecto == operacion.IdProyecto && f.Secuencia < operacion.Secuencia).OrderBy(f => f.Secuencia).ToList(), "Referencia", "Nombre");

            return View(operacion);
        }

        // Permisos: Creador, Editor
        // GET: /Modelo/Operacion/Delete/5

        public ActionResult DeleteOperacion(int id = 0)
        {
            Operacion operacion = db.Operaciones.Find(id);
            if (operacion == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Get project and check user
            int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == operacion.IdProyecto);
            if (current == null || current.SoloLectura)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            var salidas = db.SalidaOperaciones.Where(s => s.IdOperacion == operacion.Id).ToList();
            foreach (SalidaOperacion salida in salidas)
            {
                db.SalidaOperacionesRequester.RemoveElementByID(salida.Id);
            }

            db.OperacionesRequester.RemoveElementByID(operacion.Id, true, true, operacion.IdProyecto, getUserId());
            return RedirectToAction("Corolario", new { id = operacion.IdProyecto });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
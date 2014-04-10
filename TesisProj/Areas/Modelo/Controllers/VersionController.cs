﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Seguridad.Models;
using TesisProj.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Modelo.Controllers
{
    public partial class ProyectoController : Controller
    {

        // Permisos: Creador, Editor
        // Get /Modelo/Proyecto/Origenes/5

        public ActionResult Origenes(int id = 0)
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

            var versiones = db.DbVersions.Include(v => v.Creador).Where(v => v.IdProyecto == proyecto.Id).OrderByDescending(v => v.Fecha);
            return View(versiones.ToList());
        }

        // Permisos: Creador, Editor
        // GET: /Modelo/Proyecto/GuardarVersion/5

        public ActionResult GuardarVersion(int id = 0)
        {
            // Begin: Zona crítica

            db.Configuration.ProxyCreationEnabled = false;

            Proyecto proyecto = db.Proyectos.AsNoTracking().FirstOrDefault(p => p.Id == id);
            if (proyecto == null)
            {
                db.Configuration.ProxyCreationEnabled = true;
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Check user
            int currentId = getUserId();
            Colaborador current = db.Colaboradores.AsNoTracking().FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == proyecto.Id);
            if (current == null || current.SoloLectura)
            {
                db.Configuration.ProxyCreationEnabled = true;
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            CalcularResultados(id);

            // Set new version

            proyecto.Version = proyecto.Version + 1;
            proyecto.Calculado = DateTime.Now;
            db.ProyectosRequester.ModifyElement(proyecto, true, proyecto.Id, getUserId());

            // Get project (no proxy)

            proyecto = db.Proyectos
                .Include(p => p.Elementos)
                .Include(p => p.Elementos.Select(e => e.Formulas))
                .Include(p => p.Elementos.Select(e => e.Parametros))
                .Include("Elementos.Parametros.Celdas")
                .Include(p => p.Salidas)
                .Include(p => p.Operaciones)
                .Include(p => p.Operaciones.Select(o => o.Salidas))
                .FirstOrDefault(p => p.Id == id);

            // Init serializer

            XmlSerializer s = new XmlSerializer(typeof(Proyecto));
            MemoryStream memStream = new MemoryStream();
            s.Serialize(memStream, proyecto);

            // Save version register

            db.DbVersionsRequester.AddElement(new DbVersion { IdProyecto = proyecto.Id, IdUsuario = getUserId(), Data = memStream.ToArray(), Extension = "xml", Mime = "application/xml", Fecha = DateTime.Now, Version = proyecto.Version });
            db.Configuration.ProxyCreationEnabled = true;

            // Fin: Zona crítica

            return RedirectToAction("Origenes", new { id = proyecto.Id });
        }

        // Permisos: Creador, Editor
        // GET: /Modelo/Proyecto/GetVersion/5

        public ActionResult GetVersion(int id = 0)
        {
            var version = db.DbVersions.Find(id);
            if (version == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Check user
            int currentId = getUserId();
            Colaborador current = db.Colaboradores.AsNoTracking().FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == version.IdProyecto);
            if (current == null || current.SoloLectura)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            return File(version.Data, version.Mime);
        }

        // Permisos: Creador, Editor
        // GET: /Modelo/Proyecto/RestaurarVersion/5

        public ActionResult RestaurarVersion(int id = 0)
        {
            // Begin: Zona crítica

            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.ValidateOnSaveEnabled = false;

            var version = db.DbVersions.Find(id);
            if (version == null)
            {
                db.Configuration.ProxyCreationEnabled = true;
                db.Configuration.ValidateOnSaveEnabled = true;
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Check user
            int currentId = getUserId();
            Colaborador current = db.Colaboradores.AsNoTracking().FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == version.IdProyecto);
            if (current == null || current.SoloLectura)
            {
                db.Configuration.ProxyCreationEnabled = true;
                db.Configuration.ValidateOnSaveEnabled = true;
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Get project (no proxy)

            Proyecto proyecto = db.Proyectos
                .Include(p => p.Elementos)
                .Include(p => p.Elementos.Select(e => e.Formulas))
                .Include(p => p.Elementos.Select(e => e.Parametros))
                .Include("Elementos.Parametros.Celdas")
                .Include(p => p.Salidas)
                .Include(p => p.Operaciones)
                .Include(p => p.Operaciones.Select(o => o.Salidas))
                .FirstOrDefault(p => p.Id == version.IdProyecto);

            // Set version

            int restversion = proyecto.Version + 1;

            // Init serializer

            XmlSerializer s = new XmlSerializer(typeof(Proyecto));
            MemoryStream memStream = new MemoryStream();
            s.Serialize(memStream, proyecto);

            // Save current version

            db.DbVersionsRequester.AddElement(new DbVersion { IdProyecto = proyecto.Id, IdUsuario = getUserId(), Data = memStream.ToArray(), Extension = "xml", Mime = "application/xml", Fecha = DateTime.Now, Version = restversion });

            // Wipe the project related entities

            var salidaoperaciones = db.SalidaOperaciones.Where(sxp => sxp.Operacion.IdProyecto == proyecto.Id).ToList();
            foreach (SalidaOperacion salidaoperacion in salidaoperaciones)
            {
                db.SalidaOperacionesRequester.RemoveElementByID(salidaoperacion.Id);
            }

            var operaciones = db.Operaciones.Where(o => o.IdProyecto == proyecto.Id).ToList();
            foreach (Operacion operacion in operaciones)
            {
                db.OperacionesRequester.RemoveElementByID(operacion.Id);
            }

            var salidas = db.SalidaProyectos.Where(y => y.IdProyecto == proyecto.Id).ToList();
            foreach (SalidaProyecto salida in salidas)
            {
                db.SalidaProyectosRequester.RemoveElementByID(salida.Id);
            }

            var formulas = db.Formulas.Where(f => f.Elemento.IdProyecto == proyecto.Id).ToList();
            foreach (Formula formula in formulas)
            {
                db.FormulasRequester.RemoveElementByID(formula.Id);
            }

            var celdas = db.Celdas.Where(c => c.Parametro.Elemento.IdProyecto == proyecto.Id).ToList();
            foreach (Celda celda in celdas)
            {
                db.CeldasRequester.RemoveElementByID(celda.Id);
            }

            var parametros = db.Parametros.Where(p => p.Elemento.IdProyecto == proyecto.Id).ToList();
            foreach (Parametro parametro in parametros)
            {
                db.ParametrosRequester.RemoveElementByID(parametro.Id);
            }

            var elementos = db.Elementos.Where(e => e.IdProyecto == proyecto.Id).ToList();
            foreach (Elemento elemento in elementos)
            {
                db.ElementosRequester.RemoveElementByID(elemento.Id);
            }

            // Deserialize

            s = new XmlSerializer(typeof(Proyecto));
            memStream = new MemoryStream(version.Data);
            Proyecto proyecto_dirty = (Proyecto) s.Deserialize(memStream);

            // Set version (only for logging issues)

            proyecto_dirty.Version = restversion;

            // Save project

            db.ProyectosRequester.ModifyElement(proyecto_dirty, true, proyecto_dirty.Id, getUserId());

            db.Configuration.ProxyCreationEnabled = true;
            db.Configuration.ValidateOnSaveEnabled = true;

            // End: Zona crítica

            return RedirectToAction("Origenes", new { id = version.IdProyecto });
        }

        // Permisos: Anon
        // GET: /Modelo/Proyecto/LoadXML

        public ActionResult LoadXml()
        {
            return View();
        }

        // 
        // POST: /Modelo/Proyecto/LoadXml

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadXml(HttpPostedFileBase file)
        {
            try
            {

                XmlSerializer s = new XmlSerializer(typeof(Proyecto));
                MemoryStream memStream = new MemoryStream();

                file.InputStream.CopyTo(memStream);
                memStream.Position = 0;

                Proyecto proyecto_dirty = (Proyecto)s.Deserialize(memStream);

                db.Configuration.ValidateOnSaveEnabled = false;
                db.ProyectosRequester.AddElement(proyecto_dirty);
                db.ColaboradoresRequester.AddElement(new Colaborador { IdProyecto = proyecto_dirty.Id, Creador = true, SoloLectura = false, IdUsuario = getUserId() });
                db.AuditsRequester.AddElement(new Audit { IdProyecto = proyecto_dirty.Id, Fecha = DateTime.Now, IdUsuario = getUserId(), Transaccion = "Crear", TipoObjeto = proyecto_dirty.GetType().ToString(), Original = proyecto_dirty.LogValues() });
                db.Configuration.ValidateOnSaveEnabled = true;
                return RedirectToAction("Console", new { id = proyecto_dirty.Id });

            }
            catch (Exception)
            {
                db.Configuration.ValidateOnSaveEnabled = true;
            }

            return RedirectToAction("LoadXml");
        }

        // Permisos: Creador, Editor
        // GET: /Modelo/Proyecto/DuplicarVersion/5

        public ActionResult DuplicarVersion(int id = 0)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.ValidateOnSaveEnabled = false;

            DbVersion version = db.DbVersions.Find(id);
            if (version == null)
            {
                db.Configuration.ValidateOnSaveEnabled = true;
                db.Configuration.ProxyCreationEnabled = true;
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Check user
            int currentId = getUserId();
            Colaborador current = db.Colaboradores.AsNoTracking().FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == version.IdProyecto);
            if (current == null || current.SoloLectura)
            {
                db.Configuration.ProxyCreationEnabled = true;
                db.Configuration.ValidateOnSaveEnabled = true;
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            try
            {
                XmlSerializer s = new XmlSerializer(typeof(Proyecto));
                MemoryStream memStream = new MemoryStream(version.Data);
                Proyecto proyecto_dirty = (Proyecto)s.Deserialize(memStream);

                // New project attributes

                proyecto_dirty.Version = 0;
                proyecto_dirty.Creacion = DateTime.Now;
                proyecto_dirty.Calculado = proyecto_dirty.Creacion;
                proyecto_dirty.IdCreador = getUserId();

                string nombre = "Copia de " + proyecto_dirty.Nombre + " ";
                string nombreTest = nombre;
                int i = 1;

                while (db.Proyectos.Any(p => p.Nombre.Equals(nombreTest)))
                {
                    nombreTest = nombre + i++;
                }

                proyecto_dirty.Nombre = nombreTest;

                db.ProyectosRequester.AddElement(proyecto_dirty);
                
                // Create raw audit element (as no Id exists before save)
                db.ColaboradoresRequester.AddElement(new Colaborador { IdProyecto = proyecto_dirty.Id, Creador = true, SoloLectura = false, IdUsuario = getUserId() });
                db.AuditsRequester.AddElement(new Audit { IdProyecto = proyecto_dirty.Id, Fecha = DateTime.Now, IdUsuario = getUserId(), Transaccion = "Crear", TipoObjeto = proyecto_dirty.GetType().ToString(), Original = proyecto_dirty.LogValues() });

                db.Configuration.ValidateOnSaveEnabled = true;
                db.Configuration.ProxyCreationEnabled = true;

                return RedirectToAction("Edit", new { id = proyecto_dirty.Id });

            }
            catch (Exception)
            {
                db.Configuration.ValidateOnSaveEnabled = true;
                db.Configuration.ProxyCreationEnabled = true;
            }

            return RedirectToAction("Origenes", new { id = version.IdProyecto });
        }
    }
}

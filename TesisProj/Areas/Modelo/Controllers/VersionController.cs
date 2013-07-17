using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Modelo.Controllers
{
    public partial class ProyectoController : Controller
    {

        //
        // Get /Modelo/Proyecto/Origenes/5

        public ActionResult Origenes(int id)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }

            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.ProyectoId = proyecto.Id;

            var versiones = db.DbVersions.Include(v => v.Creador).Where(v => v.IdProyecto == proyecto.Id).OrderByDescending(v => v.Fecha);

            return View(versiones.ToList());
        }

        //
        // GET: /Modelo/Proyecto/GuardarVersion/5

        public ActionResult GuardarVersion(int id)
        {

            //
            // Comienza zona crítica de serialización

            db.Configuration.ProxyCreationEnabled = false;

            Proyecto proyecto = db.Proyectos.AsNoTracking().FirstOrDefault(p => p.Id == id);

            if (proyecto == null)
            {
                db.Configuration.ProxyCreationEnabled = true;
                return HttpNotFound();
            }

            proyecto.Version = proyecto.Version + 1;
            db.ProyectosRequester.ModifyElement(proyecto, true, proyecto.Id, getUserId());

            proyecto = db.Proyectos
                .Include(p => p.Elementos)
                .Include(p => p.Elementos.Select(e => e.Formulas))
                .Include(p => p.Elementos.Select(e => e.Parametros))
                .Include("Elementos.Parametros.Celdas")
                .Include(p => p.Salidas)
                .Include(p => p.Operaciones)
                .Include(p => p.Operaciones.Select(o => o.Salidas))
                .FirstOrDefault(p => p.Id == id);

            XmlSerializer s = new XmlSerializer(typeof(Proyecto));
            MemoryStream memStream = new MemoryStream();
            s.Serialize(memStream, proyecto);
            
            db.DbVersionsRequester.AddElement(new DbVersion { IdProyecto = proyecto.Id, IdUsuario = getUserId(), Data = memStream.ToArray(), Extension = "xml", Mime = "application/xml", Fecha = DateTime.Now, Version = proyecto.Version });

            db.Configuration.ProxyCreationEnabled = true;

            //
            // Finaliza zona crítica de serialización

            return RedirectToAction("Origenes", new { id = proyecto.Id });
        }

        //
        // GET: /Modelo/Proyecto/GetVersion/5

        public ActionResult GetVersion(int id)
        {
            var version = db.DbVersions.Find(id);
            if (version == null)
            {
                return HttpNotFound();
            }
            return File(version.Data, version.Mime);
        }

        //
        // GET: /Modelo/Proyecto/RestaurarVersion/5

        public ActionResult RestaurarVersion(int id)
        {
            //
            // Comienza zona crítica de serialización

            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.ValidateOnSaveEnabled = false;

            var version = db.DbVersions.Find(id);
            
            if (version == null)
            {
                db.Configuration.ProxyCreationEnabled = true;
                db.Configuration.ValidateOnSaveEnabled = true;
                return HttpNotFound();
            }

            Proyecto proyecto = db.Proyectos
                .Include(p => p.Elementos)
                .Include(p => p.Elementos.Select(e => e.Formulas))
                .Include(p => p.Elementos.Select(e => e.Parametros))
                .Include("Elementos.Parametros.Celdas")
                .Include(p => p.Salidas)
                .Include(p => p.Operaciones)
                .Include(p => p.Operaciones.Select(o => o.Salidas))
                .FirstOrDefault(p => p.Id == version.IdProyecto);

            int restversion = proyecto.Version + 1;

            XmlSerializer s = new XmlSerializer(typeof(Proyecto));
            MemoryStream memStream = new MemoryStream();
            s.Serialize(memStream, proyecto);

            db.DbVersionsRequester.AddElement(new DbVersion { IdProyecto = proyecto.Id, IdUsuario = getUserId(), Data = memStream.ToArray(), Extension = "xml", Mime = "application/xml", Fecha = DateTime.Now, Version = restversion });

            //
            // Elimino toda la información del proyecto...

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

            //
            // Fin de eliminación de toda la información del proyecto...

            s = new XmlSerializer(typeof(Proyecto));
            memStream = new MemoryStream(version.Data);
            Proyecto proyecto_dirty = (Proyecto) s.Deserialize(memStream);

            proyecto_dirty.Version = restversion;

            db.ProyectosRequester.ModifyElement(proyecto_dirty, true, proyecto_dirty.Id, getUserId());

            db.Configuration.ProxyCreationEnabled = true;
            db.Configuration.ValidateOnSaveEnabled = true;

            //
            // Finaliza zona crítica de serialización

            return RedirectToAction("Origenes", new { id = version.IdProyecto });
        }

        public ActionResult LoadXml()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadXml(HttpPostedFileBase file)
        {
            //
            // Inicia zona crítica de serialización

            try
            {

                XmlSerializer s = new XmlSerializer(typeof(Proyecto));
                MemoryStream memStream = new MemoryStream();

                file.InputStream.CopyTo(memStream);
                memStream.Position = 0;

                Proyecto proyecto_dirty = (Proyecto)s.Deserialize(memStream);

                db.Configuration.ValidateOnSaveEnabled = false;
                db.ProyectosRequester.AddElement(proyecto_dirty);
                db.Configuration.ValidateOnSaveEnabled = true;
                return RedirectToAction("Console", new { id = proyecto_dirty.Id });

            }
            catch (Exception)
            {
                db.Configuration.ValidateOnSaveEnabled = true;
            }

            return RedirectToAction("LoadXml");
        }

        public ActionResult DuplicarVersion(int id)
        {
            //
            // Inicia zona crítica de serialización

            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.ValidateOnSaveEnabled = false;

            DbVersion version = db.DbVersions.Find(id);

            if (version == null)
            {
                db.Configuration.ValidateOnSaveEnabled = true;
                db.Configuration.ProxyCreationEnabled = true;
                return HttpNotFound();
            }

            try
            {
                XmlSerializer s = new XmlSerializer(typeof(Proyecto));
                MemoryStream memStream = new MemoryStream(version.Data);
                Proyecto proyecto_dirty = (Proyecto)s.Deserialize(memStream);

                proyecto_dirty.Version = 0;
                proyecto_dirty.Creacion = DateTime.Now;
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

                db.Configuration.ValidateOnSaveEnabled = true;
                db.Configuration.ProxyCreationEnabled = true;

                return RedirectToAction("Console", new { id = proyecto_dirty.Id });

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

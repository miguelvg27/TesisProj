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
    public partial class ProyectoController : Controller
    {

        //
        // GET: /Modelo/Proyecto/Cine/5

        public ActionResult Cine(int id = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }

            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.ProyectoId = proyecto.Id;

            var salidaproyectos = db.SalidaProyectos.Include(s => s.Proyecto).Where(s => s.IdProyecto == proyecto.Id);
            
            return View(salidaproyectos.ToList());
        }

        //
        // GET: /Modelo/Proyecto/Pelicula/5

        public ActionResult Pelicula(int id = 0)
        {
            SalidaProyecto salida = db.SalidaProyectos.Find(id);
            Proyecto proyecto = db.Proyectos.Find(salida.IdProyecto);
            int horizonte = proyecto.Horizonte;

            if (salida == null)
            {
                return HttpNotFound();
            }

            var operaciones = db.Operaciones.Where(o => o.IdProyecto == salida.IdProyecto).OrderBy(s => s.Secuencia).ToList();
            var formulas = db.Formulas.Include("Elemento").Where(f => f.Elemento.IdProyecto == salida.IdProyecto).ToList();
            var parametros = db.Parametros.Include("Elemento").Include("Celdas").Where(e => e.Elemento.IdProyecto == salida.IdProyecto).ToList();
            var tipoformulas = db.TipoFormulas.ToList();

            //  Lleno los valores de las referencias a tipos de fórmula

            foreach (TipoFormula tipoformula in tipoformulas)
            {
                tipoformula.Valores = new List<double>();

                for (int i = 0; i < horizonte; i++)
                {
                    tipoformula.Valores.Add(0);
                }

                var formulitas = formulas.Where(f => f.IdTipoFormula == tipoformula.Id).ToList();

                foreach (Formula formulita in formulitas)
                {
                    var refFormulitas = formulas.Where(f => f.Secuencia < formulita.Secuencia && f.IdElemento == formulita.IdElemento).ToList();
                    var refParametros = parametros.Where(p => p.IdElemento == formulita.IdElemento).ToList();
                    formulita.Valores = formulita.Evaluar(horizonte, refFormulitas, refParametros);

                    for (int i = 0; i < horizonte; i++)
                    {
                        tipoformula.Valores[i] = tipoformula.Valores[i] + formulita.Valores[i];
                    }
                }
            }

            foreach (Operacion operacion in operaciones)
            {
                var refoperaciones = operaciones.Where(o => o.Secuencia < operacion.Secuencia && o.IdProyecto == salida.IdProyecto).ToList();
                operacion.Valores = operacion.Evaluar(horizonte, refoperaciones, tipoformulas, formulas, parametros);
            }

            ViewBag.IdProyecto = salida.IdProyecto;
            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.Salida = salida.Nombre;
            ViewBag.Horizonte = proyecto.Horizonte;

            var exoperaciones = db.SalidaOperaciones.Where(s => s.IdSalida == salida.Id).Select(s => s.Operacion).ToList();

            return View(operaciones.Intersect(exoperaciones).ToList());
        }

        //
        // POST: /Modelo/Proyecto/Assoc/5

        public ActionResult Assoc(int id = 0)
        {
            SalidaProyecto salida = db.SalidaProyectos.Find(id);
            if (salida == null)
            {
                return HttpNotFound();
            }

            var asociados = db.SalidaOperaciones.Include(p => p.Operacion).Where(p => p.IdSalida == salida.Id).Select(p => p.Operacion);
            var opciones = db.Operaciones.Where(o => o.IdProyecto == salida.IdProyecto).Except(asociados);
            ViewBag.Asociados = new MultiSelectList(asociados.OrderBy(o => o.Secuencia).ToList(), "Id", "Nombre");
            ViewBag.Opciones = new MultiSelectList(opciones.OrderBy(o => o.Secuencia).ToList(), "Id", "Nombre");

            return View(salida);
        }

        //
        // POST: /Modelo/Proyecto/Assoc

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assoc(SalidaProyecto salida, FormCollection form, string add, string remove, string addall, string removeall)
        {
            string seleccionados;
            int idOperacion;
            int idSalida = salida.Id;

            seleccionados = form["Opciones"];
            if (!string.IsNullOrEmpty(add) && !string.IsNullOrEmpty(seleccionados))
            {
                foreach (string sIdOperacion in seleccionados.Split(','))
                {
                    idOperacion = int.Parse(sIdOperacion);
                    if (!db.SalidaOperaciones.Any(p => p.IdSalida == idSalida && p.IdOperacion == idOperacion))
                    {
                        db.SalidaOperaciones.Add(new SalidaOperacion { IdOperacion = idOperacion, IdSalida = idSalida });
                        db.SaveChanges();
                    }
                }
            }

            seleccionados = form["Asociados"];
            if (!string.IsNullOrEmpty(remove) && !string.IsNullOrEmpty(seleccionados))
            {

                SalidaOperacion operacion;
                foreach (string sIdOperacion in seleccionados.Split(','))
                {
                    idOperacion = int.Parse(sIdOperacion);
                    operacion = db.SalidaOperaciones.FirstOrDefault(p => p.IdOperacion == idOperacion && p.IdSalida == idSalida);
                    if (operacion != null)
                    {
                        db.SalidaOperaciones.Remove(operacion);
                        db.SaveChanges();
                    }
                }
            }

            if (!string.IsNullOrEmpty(addall))
            {
                var operaciones = db.Operaciones.Where(o => o.IdProyecto == salida.IdProyecto).ToList();
                foreach (Operacion operacion in operaciones)
                {
                    idOperacion = operacion.Id;
                    if (!db.SalidaOperaciones.Any(p => p.IdSalida == idSalida && p.IdOperacion == idOperacion))
                    {
                        db.SalidaOperaciones.Add(new SalidaOperacion { IdSalida = idSalida, IdOperacion = idOperacion });
                        db.SaveChanges();
                    }
                }
            }

            if (!string.IsNullOrEmpty(removeall))
            {
                var operaciones = db.SalidaOperaciones.Where(p => p.IdSalida == idSalida).ToList();
                foreach (SalidaOperacion operacion in operaciones)
                {
                    db.SalidaOperaciones.Remove(operacion);
                    db.SaveChanges();
                }
            }

            var asociados = db.SalidaOperaciones.Include(p => p.Operacion).Where(p => p.IdSalida == salida.Id).Select(p => p.Operacion);
            var opciones = db.Operaciones.Where(o => o.IdProyecto == salida.IdProyecto).Except(asociados);
            ViewBag.Asociados = new MultiSelectList(asociados.OrderBy(o => o.Secuencia).ToList(), "Id", "Nombre");
            ViewBag.Opciones = new MultiSelectList(opciones.OrderBy(o => o.Secuencia).ToList(), "Id", "Nombre");

            return View(salida);
        }

        //
        // GET: /Modelo/SalidaProyecto/Create

        public ActionResult CreateSalidaProyecto(int idProyecto = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(idProyecto);
            if (proyecto == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdProyecto = new SelectList(db.Proyectos.Where(p => p.Id == proyecto.Id), "Id", "Nombre", proyecto.Id);
            ViewBag.IdProyectoReturn = proyecto.Id;

            var salidas = db.SalidaProyectos.Where(f => f.IdProyecto == proyecto.Id);
            int defSecuencia = salidas.Count() > 0 ? salidas.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;

            return View();
        }

        //
        // POST: /Modelo/SalidaProyecto/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSalidaProyecto(SalidaProyecto salidaproyecto)
        {
            if (ModelState.IsValid)
            {
                db.SalidaProyectos.Add(salidaproyecto);
                db.SaveChanges();
                return RedirectToAction("Cine", new { id = salidaproyecto.IdProyecto });
            }

            ViewBag.IdProyecto = new SelectList(db.Proyectos.Where(p => p.Id == salidaproyecto.IdProyecto), "Id", "Nombre", salidaproyecto.IdProyecto);
            ViewBag.IdProyectoReturn = salidaproyecto.IdProyecto;

            var salidas = db.SalidaProyectos.Where(f => f.IdProyecto == salidaproyecto.IdProyecto);
            int defSecuencia = salidas.Count() > 0 ? salidas.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;
            
            return View(salidaproyecto);
        }

        //
        // GET: /Modelo/SalidaProyecto/Edit/5

        public ActionResult EditSalidaProyecto(int id = 0)
        {
            SalidaProyecto salidaproyecto = db.SalidaProyectos.Find(id);
            if (salidaproyecto == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.IdProyecto = new SelectList(db.Proyectos.Where(p => p.Id == salidaproyecto.IdProyecto), "Id", "Nombre", salidaproyecto.IdProyecto);
            
            return View(salidaproyecto);
        }

        //
        // POST: /Modelo/SalidaProyecto/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSalidaProyecto(SalidaProyecto salidaproyecto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salidaproyecto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Cine", new { id = salidaproyecto.IdProyecto });
            }

            ViewBag.IdProyecto = new SelectList(db.Proyectos.Where(p => p.Id == salidaproyecto.IdProyecto), "Id", "Nombre", salidaproyecto.IdProyecto);
            
            return View(salidaproyecto);
        }

        //
        // GET: /Modelo/SalidaProyecto/Delete/5

        public ActionResult DeleteSalidaProyecto(int id = 0)
        {
            SalidaProyecto salidaproyecto = db.SalidaProyectos.Find(id);
            if (salidaproyecto == null)
            {
                return HttpNotFound();
            }
            return View(salidaproyecto);
        }

        //
        // POST: /Modelo/SalidaProyecto/Delete/5

        [HttpPost, ActionName("DeleteSalidaProyecto")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSalidaProyectoConfirmed(int id)
        {
            SalidaProyecto salidaproyecto = db.SalidaProyectos.Find(id);

            try
            {
                db.SalidaProyectos.Remove(salidaproyecto);
                db.SaveChanges();
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No se puede eliminar porque existen registros dependientes.");
                return View("DeleteSalidaProyecto", salidaproyecto);
            }

            return RedirectToAction("Cine", new { id = salidaproyecto.IdProyecto });
        }
    }
}
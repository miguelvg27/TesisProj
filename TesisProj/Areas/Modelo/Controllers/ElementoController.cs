using System;
using System.Collections.Generic;
using System.Data;
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
        // GET: /Modelo/Proyecto/Journal/5

        public ActionResult Journal(int id = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }

            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.ProyectoId = proyecto.Id;
            ViewBag.TipoElementos = db.TipoElementos.OrderBy(t => t.NombrePlural).ToList();

            var elementos = db.Elementos.Where(e => e.IdProyecto == proyecto.Id);

            return View(elementos.ToList());
        }

        //
        // GET: /Modelo/Proyecto/ResultadosElemento/5

        public ActionResult ResultadosElemento(int id = 0)
        {
            Elemento elemento = db.Elementos.Find(id);
            Proyecto proyecto = db.Proyectos.Find(elemento.IdProyecto);
            if (elemento == null)
            {
                return HttpNotFound();
            }

            var formulas = db.Formulas.Where(s => s.IdElemento == elemento.Id).OrderBy(s => s.Secuencia).ToList();
            var salidas = formulas.Where(s => s.Visible).ToList();

            foreach (Formula salida in salidas)
            {
                var refs = formulas.Where(f => f.Secuencia < salida.Secuencia && f.IdElemento == elemento.Id).ToList();
                var parametros = db.Parametros.Include("Celdas").Where(p => p.IdElemento == elemento.Id).ToList();
                salida.Valores = salida.Evaluar(proyecto.Horizonte, refs, parametros);
            }

            ViewBag.IdElemento = elemento.Id;
            ViewBag.IdProyecto = elemento.IdProyecto;
            ViewBag.Elemento = elemento.Nombre;
            ViewBag.TipoElemento = db.TipoElementos.Find(elemento.IdTipoElemento).Nombre;
            ViewBag.Horizonte = proyecto.Horizonte;

            return View(salidas);
        }

        //
        // GET: /Modelo/Proyecto/CreateElemento?idProyecto=5&idTipoElemento=1

        public ActionResult CreateElemento(int idProyecto = 0, int idTipoElemento = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(idProyecto);
            TipoElemento tipo = db.TipoElementos.Find(idTipoElemento);
            if (proyecto == null || tipo == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdPlantilla = new SelectList(db.PlantillaElementos.Where(p => p.IdTipoElemento == tipo.Id), "Id", "Nombre");
            ViewBag.IdTipoElemento = new SelectList(db.TipoElementos.Where(t => t.Id == tipo.Id), "Id", "Nombre", tipo.Id);
            ViewBag.IdProyecto = new SelectList(db.Proyectos.Where(p => p.Id == proyecto.Id), "Id", "Nombre", proyecto.Id);
            ViewBag.IdProyectoReturn = proyecto.Id;

            return View();
        }

        //
        // POST: /Modelo/Proyecto/CreateElemento

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateElemento(Elemento elemento, int IdPlantilla = 0)
        {

        //  Valida que no ingrese una fórmula única que ya exista en el proyecto

            if (IdPlantilla > 0)
            {
                var formulasUnicasProyecto = db.Formulas.Include("Elemento").Include("TipoFormula").Where(f => f.Elemento.IdTipoElemento == elemento.IdTipoElemento && f.Elemento.IdProyecto == elemento.IdProyecto && f.TipoFormula.Unico).ToList();
                var formulasUnicasPlantilla = db.PlantillaFormulas.Include("TipoFormula").Where(p => p.IdPlantillaElemento == IdPlantilla && p.TipoFormula.Unico).ToList();

                foreach (Formula formula in formulasUnicasProyecto)
                {
                    if (formulasUnicasPlantilla.Any(f => f.IdTipoFormula == formula.IdTipoFormula))
                    {
                        ModelState.AddModelError("IdPlantilla", "Esta plantilla contiene una fórmula cuyo tipo es único por proyecto y ya existe en el proyecto actual.");
                        break;
                    }
                }
            }

            if (ModelState.IsValid)
            {
                db.Elementos.Add(elemento);
                db.SaveChanges();

                if (IdPlantilla > 0)
                {
                    var parametros = db.PlantillaParametros.Where(p => p.IdPlantillaElemento == IdPlantilla);

                    foreach (PlantillaParametro plantilla in parametros)
                    {
                        db.Parametros.Add(new Parametro(plantilla, elemento.Id));
                    }

                    db.SaveChanges();

                    var formulas = db.PlantillaFormulas.Where(f => f.IdPlantillaElemento == IdPlantilla).OrderBy(f => f.Secuencia).ToList();

                    foreach (PlantillaFormula plantilla in formulas)
                    {
                        Formula item = new Formula(plantilla, elemento.Id);
                        db.Formulas.Add(item);
                        db.SaveChanges();
                    }
                    
                    return RedirectToAction("PutParametros", new { id = elemento.Id });
                }

                return RedirectToAction("Catalog", new { id = elemento.Id });
            }

            ViewBag.IdPlantilla = new SelectList(db.PlantillaElementos.Where(p => p.IdTipoElemento == elemento.IdTipoElemento), "Id", "Nombre");
            ViewBag.IdTipoElemento = new SelectList(db.TipoElementos.Where(t => t.Id == elemento.IdTipoElemento), "Id", "Nombre", elemento.IdTipoElemento);
            ViewBag.IdProyecto = new SelectList(db.Proyectos.Where(p => p.Id == elemento.IdProyecto), "Id", "Nombre", elemento.IdProyecto);
            ViewBag.IdProyectoReturn = elemento.IdProyecto;

            return View(elemento);
        }

        //
        // GET: /Modelo/Proyecto/EditElemento/5

        public ActionResult EditElemento(int id)
        {
            Elemento elemento = db.Elementos.Find(id);
            if (elemento == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdTipoElemento = new SelectList(db.TipoElementos.Where(t => t.Id == elemento.IdTipoElemento), "Id", "Nombre", elemento.IdTipoElemento);
            ViewBag.IdProyecto = new SelectList(db.Proyectos.Where(p => p.Id == elemento.IdProyecto), "Id", "Nombre", elemento.IdProyecto);

            return View(elemento);
        }

        //
        // POST: /Modelo/Proyecto/EditElemento/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditElemento(Elemento elemento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(elemento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Journal", new { id = elemento.IdProyecto });
            }

            ViewBag.IdTipoElemento = new SelectList(db.TipoElementos.Where(t => t.Id == elemento.IdTipoElemento), "Id", "Nombre", elemento.IdTipoElemento);
            ViewBag.IdProyecto = new SelectList(db.Proyectos.Where(p => p.Id == elemento.IdProyecto), "Id", "Nombre", elemento.IdProyecto);

            return View(elemento);
        }

        //
        // GET: /Modelo/Proyecto/DeleteElemento/5

        public ActionResult DeleteElemento(int id = 0)
        {
            Elemento elemento = db.Elementos.Find(id);
            if (elemento == null)
            {
                return HttpNotFound();
            }
            elemento.TipoElemento = db.TipoElementos.Find(elemento.IdTipoElemento);

            return View(elemento);
        }

        //
        // POST: /Modelo/Proyecto/DeleteElemento/5

        [HttpPost, ActionName("DeleteElemento")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteElementoConfirmed(int id)
        {
            Elemento elemento = db.Elementos.Find(id);
            try
            {
                db.Elementos.Remove(elemento);
                db.SaveChanges();
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No se puede eliminar porque existen registros dependientes.");
                elemento.TipoElemento = db.TipoElementos.Find(elemento.IdTipoElemento);
                return View("DeleteElemento", elemento);
            }

            return RedirectToAction("Journal", new { id = elemento.IdProyecto });
        }
    }
}

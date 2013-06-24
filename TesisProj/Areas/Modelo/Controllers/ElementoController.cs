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

                    var formulas = db.PlantillaFormulas.Where(f => f.IdPlantillaElemento == IdPlantilla);
                    List<Formula> buffer = new List<Formula>();

                    foreach (PlantillaFormula plantilla in formulas)
                    {
                        Formula item = new Formula(plantilla, elemento.Id);
                        db.Formulas.Add(item);
                        buffer.Add(item);
                    }

                    db.SaveChanges();

                    var salidas = db.PlantillaSalidaElementos.Include("Formula").Where(s => s.IdPlantillaElemento == IdPlantilla);

                    foreach (PlantillaSalidaElemento plantilla in salidas)
                    {
                        Formula formula = buffer.Where(f => f.Referencia == plantilla.Formula.Referencia).FirstOrDefault();
                        if (formula != null)
                        {
                            db.SalidaElementos.Add(new SalidaElemento(plantilla, elemento.Id, formula.Id));
                        }
                    }

                    db.SaveChanges();
                }

                return RedirectToAction("Journal", new { id = elemento.IdProyecto });
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

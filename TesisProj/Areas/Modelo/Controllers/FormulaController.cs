using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Plantilla.Models;

namespace TesisProj.Areas.Modelo.Controllers
{
    public partial class ProyectoController : Controller
    {
        //
        // GET: /Modelo/Protecto/Cuaderno/5

        public ActionResult Cuaderno(int id = 0)
        {
            Elemento elemento = db.Elementos.Find(id);
            Proyecto proyecto = db.Proyectos.Find(elemento.IdProyecto);

            if (elemento == null)
            {
                return HttpNotFound();
            }

            var formulas = db.Formulas.Include("Elemento").Include("TipoFormula").Where(f => f.Elemento.Id == id).OrderBy(f => f.Secuencia);

            ViewBag.elemento = elemento.Nombre;
            TipoElemento tipo = db.TipoElementos.Find(elemento.IdTipoElemento);
            ViewBag.TipoElemento = tipo != null ? tipo.Nombre : "";
            ViewBag.IdProyecto = proyecto.Id;
            ViewBag.ElementoId = elemento.Id;

            return View(formulas.ToList());
        }

        //
        // GET: /Modelo/Formula/Create

        public ActionResult CreateFormula(int idElemento = 0)
        {
            Elemento elemento = db.Elementos.Find(idElemento);
            if (elemento == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdElementoReturn = idElemento;
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(p => p.Id == elemento.Id), "Id", "Nombre");
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.Where(t => t.IdTipoElemento == elemento.IdTipoElemento).OrderBy(t => t.Nombre), "Id", "Nombre");

            var formulas = db.Formulas.Where(f => f.IdElemento == elemento.Id);
            int defSecuencia = formulas.Count() > 0 ? formulas.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;

            return View();
        }

        //
        // POST: /Modelo/Formula/Create

        [HttpPost]
        public ActionResult CreateFormula(Formula formula)
        {
            if (ModelState.IsValid)
            {
                db.Formulas.Add(formula);
                db.SaveChanges();
                return RedirectToAction("Cuaderno", new { id = formula.IdElemento });
            }

            Elemento elemento = db.Elementos.Find(formula.IdElemento);
            ViewBag.IdElementoReturn = elemento.Id;
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(p => p.Id == elemento.Id), "Id", "Nombre");
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.Where(t => t.IdTipoElemento == elemento.IdTipoElemento).OrderBy(t => t.Nombre), "Id", "Nombre", formula.IdTipoFormula);

            var formulas = db.Formulas.Where(f => f.IdElemento == elemento.Id);
            int defSecuencia = formulas.Count() > 0 ? formulas.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;

            return View();
        }

        //
        // GET: /Modelo/Formula/Edit/5

        public ActionResult EditFormula(int id)
        {
            Formula formula = db.Formulas.Find(id);
            if (formula == null)
            {
                return HttpNotFound();
            }

            Elemento elemento = db.Elementos.Find(formula.IdElemento);
            ViewBag.IdElementoReturn = elemento.Id;
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(p => p.Id == elemento.Id), "Id", "Nombre", formula.IdElemento);
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.Where(t => t.IdTipoElemento == elemento.IdTipoElemento).OrderBy(t => t.Nombre), "Id", "Nombre", formula.IdTipoFormula);

            return View();
        }

        //
        // POST: /Modelo/Formula/Edit/5

        [HttpPost]
        public ActionResult EditFormula(Formula formula)
        {
            if (ModelState.IsValid)
            {
                db.Entry(formula).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Cuaderno", new { id = formula.IdElemento });
            }

            Elemento elemento = db.Elementos.Find(formula.IdElemento);
            ViewBag.IdElementoReturn = elemento.Id;
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(p => p.Id == elemento.Id), "Id", "Nombre", formula.IdElemento);
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.Where(t => t.IdTipoElemento == elemento.IdTipoElemento).OrderBy(t => t.Nombre), "Id", "Nombre", formula.IdTipoFormula);

            return View(formula);
        }

        //
        // GET: /Modelo/Formula/Delete/5

        public ActionResult DeleteFormula(int id)
        {
            Formula formula = db.Formulas.Find(id);
            if (formula == null)
            {
                return HttpNotFound();
            }

            formula.TipoFormula = db.TipoFormulas.Find(formula.IdTipoFormula);

            return View(formula);
        }

        //
        // POST: /Modelo/Formula/Delete/5

        [HttpPost, ActionName("DeleteFormula")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedFormula(int id)
        {
            Formula formula = db.Formulas.Find(id);
            try
            {
                db.Formulas.Remove(formula);
                db.SaveChanges();
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No se puede eliminar porque existen registros dependientes.");
                formula.TipoFormula = db.TipoFormulas.Find(formula.IdTipoFormula);
                return View("DeleteFormula", formula);
            }

            return RedirectToAction("Cuaderno", new { id = formula.IdElemento });
        }
        
    }
}

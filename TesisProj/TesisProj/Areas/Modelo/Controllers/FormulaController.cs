﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Plantilla.Models;
using TesisProj.Models;

namespace TesisProj.Areas.Modelo.Controllers
{
    public partial class ProyectoController : Controller
    {
        //
        // GET: /Modelo/Proyecto/Cuaderno/5

        public ActionResult Cuaderno(int id = 0)
        {
            Elemento elemento = db.Elementos.Find(id);
            Proyecto proyecto = db.Proyectos.Find(elemento.IdProyecto);

            if (elemento == null || proyecto == null)
            {
                return HttpNotFound();
            }

            var formulas = db.Formulas.Include("Elemento").Include("TipoFormula").Where(f => f.Elemento.Id == id).OrderBy(f => f.Secuencia);

            ViewBag.Elemento = elemento.Nombre;
            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.IdProyecto = proyecto.Id;
            ViewBag.ElementoId = elemento.Id;

            return View(formulas.ToList());
        }

        //
        // GET: /Modelo/Proyecto/CreateFormula/5

        public ActionResult CreateFormula(int idElemento = 0)
        {
            Elemento elemento = db.Elementos.Find(idElemento);
            Proyecto proyecto = db.Proyectos.Find(elemento.IdProyecto);

            if (elemento == null || proyecto == null)
            {
                return HttpNotFound();
            }

            ViewBag.Elemento = elemento.Nombre;
            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.IdElementoReturn = idElemento;
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(p => p.Id == elemento.Id), "Id", "Nombre");
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.Where(t => t.IdTipoElemento == elemento.IdTipoElemento).OrderBy(t => t.Nombre), "Id", "Nombre");

            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.FormulasGlobales, "Value", "Text");
            ViewBag.ListParametros = new SelectList(db.Parametros.Where(p => p.IdElemento == idElemento).OrderBy(o => o.Nombre).ToList(), "Referencia", "Nombre");
            ViewBag.ListFormulas = new SelectList(db.Formulas.Where(f => f.IdElemento == idElemento).OrderBy(f => f.Nombre).ToList(), "Referencia", "Nombre");

            var formulas = db.Formulas.Where(f => f.IdElemento == elemento.Id);
            int defSecuencia = formulas.Count() > 0 ? formulas.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;

            return View();
        }

        //
        // POST: /Modelo/Proyecto/CreateFormula

        [HttpPost]
        public ActionResult CreateFormula(Formula formula)
        {
            if (ModelState.IsValid)
            {
                formula.Elemento = db.Elementos.Find(formula.IdElemento);
                formula.TipoFormula = db.TipoFormulas.Find(formula.IdTipoFormula);
                db.FormulasRequester.AddElement(formula, true, formula.Elemento.IdProyecto, getUserId());
                return RedirectToAction("Cuaderno", new { id = formula.IdElemento });
            }

            Elemento elemento = db.Elementos.Find(formula.IdElemento);
            Proyecto proyecto = db.Proyectos.Find(elemento.IdProyecto);
            ViewBag.Elemento = elemento.Nombre;
            ViewBag.Proyecto = proyecto.Nombre;

            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.FormulasGlobales, "Value", "Text");
            ViewBag.ListParametros = new SelectList(db.Parametros.Where(p => p.IdElemento == formula.IdElemento).OrderBy(o => o.Nombre).ToList(), "Referencia", "Nombre");
            ViewBag.ListFormulas = new SelectList(db.Formulas.Where(f => f.IdElemento == formula.IdElemento && f.Secuencia < formula.Secuencia).OrderBy(f => f.Nombre).ToList(), "Referencia", "Nombre");

            ViewBag.IdElementoReturn = elemento.Id;
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(p => p.Id == elemento.Id), "Id", "Nombre");
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.Where(t => t.IdTipoElemento == elemento.IdTipoElemento).OrderBy(t => t.Nombre), "Id", "Nombre", formula.IdTipoFormula);

            var formulas = db.Formulas.Where(f => f.IdElemento == elemento.Id);
            int defSecuencia = formulas.Count() > 0 ? formulas.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;

            return View(formula);
        }

        //
        // GET: /Modelo/Proyecto/EditFormula/5

        public ActionResult EditFormula(int id)
        {
            Formula formula = db.Formulas.Find(id);
            if (formula == null)
            {
                return HttpNotFound();
            }

            Elemento elemento = db.Elementos.Find(formula.IdElemento);
            Proyecto proyecto = db.Proyectos.Find(elemento.IdProyecto);
            ViewBag.Elemento = elemento.Nombre;
            ViewBag.Proyecto = proyecto.Nombre;

            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.FormulasGlobales, "Value", "Text");
            ViewBag.ListParametros = new SelectList(db.Parametros.Where(p => p.IdElemento == formula.IdElemento).OrderBy(o => o.Nombre).ToList(), "Referencia", "Nombre");
            ViewBag.ListFormulas = new SelectList(db.Formulas.Where(f => f.IdElemento == formula.IdElemento && f.Secuencia < formula.Secuencia).OrderBy(f => f.Nombre).ToList(), "Referencia", "Nombre");

            ViewBag.IdElementoReturn = elemento.Id;
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(p => p.Id == elemento.Id), "Id", "Nombre", formula.IdElemento);
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.Where(t => t.IdTipoElemento == elemento.IdTipoElemento).OrderBy(t => t.Nombre), "Id", "Nombre", formula.IdTipoFormula);

            return View(formula);
        }

        //
        // POST: /Modelo/Proyecto/EditFormula

        [HttpPost]
        public ActionResult EditFormula(Formula formula)
        {
            if (ModelState.IsValid)
            {
                formula.Elemento = db.Elementos.Find(formula.IdElemento);
                formula.TipoFormula = db.TipoFormulas.Find(formula.IdTipoFormula);
                db.FormulasRequester.ModifyElement(formula, true, formula.Elemento.IdProyecto, getUserId());

                return RedirectToAction("Cuaderno", new { id = formula.IdElemento });
            }

            Elemento elemento = db.Elementos.Find(formula.IdElemento);
            Proyecto proyecto = db.Proyectos.Find(elemento.IdProyecto);
            ViewBag.Elemento = elemento.Nombre;
            ViewBag.Proyecto = proyecto.Nombre;


            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.FormulasGlobales, "Value", "Text");
            ViewBag.ListParametros = new SelectList(db.Parametros.Where(p => p.IdElemento == formula.IdElemento).OrderBy(o => o.Nombre).ToList(), "Referencia", "Nombre");
            ViewBag.ListFormulas = new SelectList(db.Formulas.Where(f => f.IdElemento == formula.IdElemento && f.Secuencia < formula.Secuencia).OrderBy(f => f.Nombre).ToList(), "Referencia", "Nombre");


            ViewBag.IdElementoReturn = elemento.Id;
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(p => p.Id == elemento.Id), "Id", "Nombre", formula.IdElemento);
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.Where(t => t.IdTipoElemento == elemento.IdTipoElemento).OrderBy(t => t.Nombre), "Id", "Nombre", formula.IdTipoFormula);

            return View(formula);
        }

        //
        // GET: /Modelo/Proyecto/DeleteFormula/5

        public ActionResult DeleteFormula(int id)
        {
            Formula formula = db.Formulas.Find(id);
            try
            {
                db.FormulasRequester.RemoveElementByID(formula.Id, true, true, formula.Elemento.IdProyecto, getUserId());
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No se puede eliminar porque existen registros dependientes.");
            }

            return RedirectToAction("Cuaderno", new { id = formula.IdElemento });
        }
        
    }
}

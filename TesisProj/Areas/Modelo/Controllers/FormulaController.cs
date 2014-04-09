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
using TesisProj.Models;

namespace TesisProj.Areas.Modelo.Controllers
{
    public partial class ProyectoController : Controller
    {
        // Permisos: Creador, Editor
        // GET: /Modelo/Proyecto/Cuaderno/5

        public ActionResult Cuaderno(int id = 0)
        {
            // Check Url
            Elemento elemento = db.Elementos.Find(id);
            if (elemento == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Get project and check user
            Proyecto proyecto = db.Proyectos.Find(elemento.IdProyecto); int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == proyecto.Id);
            if (current == null || current.SoloLectura)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }
            
            ViewBag.Elemento = elemento.Nombre;
            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.IdProyecto = proyecto.Id;
            ViewBag.ElementoId = elemento.Id;

            var formulas = db.Formulas.Include(f => f.Elemento).Include(f => f.TipoFormula).Include(f =>f.TipoDato).Where(f => f.Elemento.Id == id).OrderBy(f => f.Secuencia);
            return View(formulas.ToList());
        }

        // Permisos: Creador, Editor, Revisor
        // GET: /Modelo/Proyecto/Programa/5

        public ActionResult Programa(int id = 0)
        {
            Elemento elemento = db.Elementos.Find(id);
            if (elemento == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Get project and check user
            Proyecto proyecto = db.Proyectos.Find(elemento.IdProyecto); int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == proyecto.Id);
            if (current == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            var formulas = db.Formulas.Include(f => f.TipoDato).Where(s => s.IdElemento == elemento.Id).OrderBy(s => s.Secuencia).ToList();
            var parametros = db.Parametros.Include(p => p.Celdas).Where(p => p.IdElemento == elemento.Id).ToList();
            var salidas = new List<Formula>();

            foreach (Formula formula in formulas)
            {
                formula.Evaluar(proyecto.Horizonte, proyecto.PeriodosPreOp, proyecto.PeriodosCierre, salidas, parametros);
                salidas.Add(formula);
            }

            bool IsCreador = current.Creador;
            bool IsEditor = !current.Creador && !current.SoloLectura;
            bool IsRevisor = current.SoloLectura;

            ViewBag.IsCreador = IsCreador;
            ViewBag.IsEditor = IsEditor;
            ViewBag.IsRevisor = IsRevisor;

            ViewBag.IdElemento = elemento.Id;
            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.IdProyecto = elemento.IdProyecto;
            ViewBag.Elemento = elemento.Nombre;
            ViewBag.TipoElemento = db.TipoElementos.Find(elemento.IdTipoElemento).Nombre;
            ViewBag.Horizonte = proyecto.Horizonte;

            return View(salidas.Where(f => f.Visible).ToList());
        }

        // Permisos: Creador, Editor
        // GET: /Modelo/Proyecto/CreateFormula/5

        public ActionResult CreateFormula(int idElemento = 0)
        {
            // Check Url
            Elemento elemento = db.Elementos.Find(idElemento);
            if (elemento == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Get project and check user
            Proyecto proyecto = db.Proyectos.Find(elemento.IdProyecto); int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == proyecto.Id);
            if (current == null || current.SoloLectura)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            ViewBag.Elemento = elemento.Nombre;
            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.IdElementoReturn = idElemento;
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(p => p.Id == elemento.Id), "Id", "Nombre");
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.Where(t => t.IdTipoElemento == elemento.IdTipoElemento).OrderBy(t => t.Nombre), "Id", "Nombre");
            ViewBag.IdTipoDato = new SelectList(db.TipoDatos.OrderBy(t => t.Nombre), "Id", "Nombre");
            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.FormulasGlobales, "Value", "Text");
            ViewBag.ConstantesList = new SelectList(db.Constantes.OrderBy(c => c.Nombre), "Valor", "Nombre");
            ViewBag.ListParametros = new SelectList(db.Parametros.Where(p => p.IdElemento == idElemento).OrderBy(o => o.Nombre).ToList(), "Referencia", "Nombre");
            ViewBag.ListFormulas = new SelectList(db.Formulas.Where(f => f.IdElemento == idElemento).OrderBy(f => f.Secuencia).ToList(), "Referencia", "ListName");

            var formulas = db.Formulas.Where(f => f.IdElemento == elemento.Id);
            int defSecuencia = formulas.Count() > 0 ? formulas.Max(f => f.Secuencia) + 10 : 10;
            ViewBag.defSecuencia = defSecuencia;

            return View();
        }

        // Permisos: Creador, Editor
        // POST: /Modelo/Proyecto/CreateFormula

        [HttpPost]
        public ActionResult CreateFormula(Formula formula)
        {
            if (ModelState.IsValid)
            {
                formula.Elemento = db.Elementos.Find(formula.IdElemento);
                formula.TipoFormula = db.TipoFormulas.Find(formula.IdTipoFormula);
                formula.TipoDato = db.TipoDatos.Find(formula.IdTipoDato);
                db.FormulasRequester.AddElement(formula, true, formula.Elemento.IdProyecto, getUserId());
                return RedirectToAction("Cuaderno", new { id = formula.IdElemento });
            }

            Elemento elemento = db.Elementos.Find(formula.IdElemento);
            Proyecto proyecto = db.Proyectos.Find(elemento.IdProyecto);
            ViewBag.Elemento = elemento.Nombre;
            ViewBag.Proyecto = proyecto.Nombre;

            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.FormulasGlobales, "Value", "Text");
            ViewBag.ConstantesList = new SelectList(db.Constantes.OrderBy(c => c.Nombre), "Valor", "Nombre");
            ViewBag.ListParametros = new SelectList(db.Parametros.Where(p => p.IdElemento == formula.IdElemento).OrderBy(o => o.Nombre).ToList(), "Referencia", "Nombre");
            ViewBag.ListFormulas = new SelectList(db.Formulas.Where(f => f.IdElemento == formula.IdElemento && f.Secuencia < formula.Secuencia).OrderBy(f => f.Secuencia).ToList(), "Referencia", "ListName");

            ViewBag.IdElementoReturn = elemento.Id;
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(p => p.Id == elemento.Id), "Id", "Nombre");
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.Where(t => t.IdTipoElemento == elemento.IdTipoElemento).OrderBy(t => t.Nombre), "Id", "Nombre", formula.IdTipoFormula);
            ViewBag.IdTipoDato = new SelectList(db.TipoDatos.OrderBy(t => t.Nombre), "Id", "Nombre", formula.IdTipoDato);

            return View(formula);
        }

        // Permisos: Creador, Editor
        // GET: /Modelo/Proyecto/EditFormula/5

        public ActionResult EditFormula(int id = 0)
        {
            Formula formula = db.Formulas.Find(id);
            if (formula == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Get project and check user
            Elemento elemento = db.Elementos.Find(formula.IdElemento);
            Proyecto proyecto = db.Proyectos.Find(elemento.IdProyecto); int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == proyecto.Id);
            if (current == null || current.SoloLectura)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            ViewBag.Elemento = elemento.Nombre;
            ViewBag.Proyecto = proyecto.Nombre;

            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.FormulasGlobales, "Value", "Text");
            ViewBag.ConstantesList = new SelectList(db.Constantes.OrderBy(c => c.Nombre), "Valor", "Nombre");
            ViewBag.ListParametros = new SelectList(db.Parametros.Where(p => p.IdElemento == formula.IdElemento).OrderBy(o => o.Nombre).ToList(), "Referencia", "Nombre");
            ViewBag.ListFormulas = new SelectList(db.Formulas.Where(f => f.IdElemento == formula.IdElemento && f.Secuencia < formula.Secuencia).OrderBy(f => f.Secuencia).ToList(), "Referencia", "ListName");

            ViewBag.IdElementoReturn = elemento.Id;
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(p => p.Id == elemento.Id), "Id", "Nombre", formula.IdElemento);
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.Where(t => t.IdTipoElemento == elemento.IdTipoElemento).OrderBy(t => t.Nombre), "Id", "Nombre", formula.IdTipoFormula);
            ViewBag.IdTipoDato = new SelectList(db.TipoDatos.OrderBy(t => t.Nombre), "Id", "Nombre", formula.IdTipoDato);

            return View(formula);
        }

        // Permisos: Creador, Editor
        // POST: /Modelo/Proyecto/EditFormula

        [HttpPost]
        public ActionResult EditFormula(Formula formula)
        {
            if (ModelState.IsValid)
            {
                formula.Elemento = db.Elementos.Find(formula.IdElemento);
                formula.TipoFormula = db.TipoFormulas.Find(formula.IdTipoFormula);
                formula.TipoDato = db.TipoDatos.Find(formula.IdTipoDato);
                db.FormulasRequester.ModifyElement(formula, true, formula.Elemento.IdProyecto, getUserId());

                return RedirectToAction("Cuaderno", new { id = formula.IdElemento });
            }

            Elemento elemento = db.Elementos.Find(formula.IdElemento);
            Proyecto proyecto = db.Proyectos.Find(elemento.IdProyecto);
            ViewBag.Elemento = elemento.Nombre;
            ViewBag.Proyecto = proyecto.Nombre;

            ViewBag.GlobalList = new SelectList(Generics.VariablesGlobales, "Value", "Text");
            ViewBag.FuncionesList = new SelectList(Generics.FormulasGlobales, "Value", "Text");
            ViewBag.ConstantesList = new SelectList(db.Constantes.OrderBy(c => c.Nombre), "Valor", "Nombre");
            ViewBag.ListParametros = new SelectList(db.Parametros.Where(p => p.IdElemento == formula.IdElemento).OrderBy(o => o.Nombre).ToList(), "Referencia", "Nombre");
            ViewBag.ListFormulas = new SelectList(db.Formulas.Where(f => f.IdElemento == formula.IdElemento && f.Secuencia < formula.Secuencia).OrderBy(f => f.Secuencia).ToList(), "Referencia", "ListName");

            ViewBag.IdElementoReturn = elemento.Id;
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(p => p.Id == elemento.Id), "Id", "Nombre", formula.IdElemento);
            ViewBag.IdTipoFormula = new SelectList(db.TipoFormulas.Where(t => t.IdTipoElemento == elemento.IdTipoElemento).OrderBy(t => t.Nombre), "Id", "Nombre", formula.IdTipoFormula);
            ViewBag.IdTipoDato = new SelectList(db.TipoDatos.OrderBy(t => t.Nombre), "Id", "Nombre", formula.IdTipoDato);

            return View(formula);
        }

        // Permisos: Creador, Editor
        // GET: /Modelo/Proyecto/DeleteFormula/5

        public ActionResult DeleteFormula(int id = 0)
        {
            Formula formula = db.Formulas.Find(id);
            if (formula == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Get project and check user
            Elemento elemento = db.Elementos.Find(formula.IdElemento);
            Proyecto proyecto = db.Proyectos.Find(elemento.IdProyecto); int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == proyecto.Id);
            if (current == null || current.SoloLectura)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            db.FormulasRequester.RemoveElementByID(formula.Id, true, true, formula.Elemento.IdProyecto, getUserId());
            return RedirectToAction("Cuaderno", new { id = formula.IdElemento });
        }
        
    }
}

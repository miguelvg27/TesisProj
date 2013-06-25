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
        // GET: /Modelo/Proyecto/Pizarra/5

        public ActionResult Pizarra(int id = 0)
        {
            Elemento elemento = db.Elementos.Find(id);
            if (elemento == null)
            {
                return HttpNotFound();
            }

            var salidaelementos = db.SalidaElementos.Include("Elemento").Include("Formula").OrderBy(s => s.Secuencia);

            ViewBag.elemento = elemento.Nombre;
            TipoElemento tipo = db.TipoElementos.Find(elemento.IdTipoElemento);
            ViewBag.TipoPlantilla = tipo != null ? tipo.Nombre : "";
            ViewBag.IdElementoReturn = id;

            return View(salidaelementos.ToList());
        }

        //
        // GET: /Modelo/Proyecto/CreateSalidaElemento

        public ActionResult CreateSalidaElemento(int idElemento = 0)
        {
            Elemento elemento = db.Elementos.Find(idElemento);
            if (elemento == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdElementoReturn = idElemento;
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(p => p.Id == elemento.Id), "Id", "Nombre");
            ViewBag.IdFormula = new SelectList(db.Formulas.Where(f => f.IdElemento == elemento.Id).OrderBy(t => t.Nombre), "Id", "Nombre");

            var salidaelementos = db.SalidaElementos.Where(f => f.IdElemento == elemento.Id);
            int defSecuencia = salidaelementos.Count() > 0 ? salidaelementos.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;

            return View();
        }

        //
        // POST: /Modelo/Proyecto/CreateElemento

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSalidaElemento(SalidaElemento salidaelemento)
        {
            if (ModelState.IsValid)
            {
                db.SalidaElementos.Add(salidaelemento);
                db.SaveChanges();
                return RedirectToAction("Pizarra", new { id = salidaelemento.IdElemento });
            }
            
            Elemento elemento = db.Elementos.Find(salidaelemento.IdElemento);
            ViewBag.IdElementoReturn = elemento.Id;
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(p => p.Id == elemento.Id), "Id", "Nombre", salidaelemento.IdElemento);
            ViewBag.IdFormula = new SelectList(db.Formulas.Where(f => f.IdElemento == elemento.Id).OrderBy(t => t.Nombre), "Id", "Nombre", salidaelemento.IdFormula);

            var salidaelementos = db.SalidaElementos.Where(f => f.IdElemento == elemento.Id);
            int defSecuencia = salidaelementos.Count() > 0 ? salidaelementos.Max(f => f.Secuencia) + 1 : 1;
            ViewBag.defSecuencia = defSecuencia;

            return View();            
        }

        //
        // GET: /Modelo/Proyecto/EditElemento/5

        public ActionResult EditSalidaElemento(int id)
        {
            SalidaElemento salidaelemento = db.SalidaElementos.Find(id);
            if (salidaelemento == null)
            {
                return HttpNotFound();
            }

            Elemento plantilla = db.Elementos.Find(salidaelemento.IdElemento);
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(p => p.Id == salidaelemento.IdElemento), "Id", "Nombre", salidaelemento.IdElemento);
            ViewBag.IdFormula = new SelectList(db.Formulas.Where(f => f.IdElemento == salidaelemento.IdElemento), "Id", "Nombre", salidaelemento.IdFormula);

            return View(salidaelemento);
        }

        //
        // POST: /Modelo/Proyecto/SalidaElemento/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSalidaElemento(SalidaElemento salidaelemento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salidaelemento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Pizarra", new { id = salidaelemento.IdElemento });
            }

            Elemento plantilla = db.Elementos.Find(salidaelemento.IdElemento);
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(p => p.Id == salidaelemento.IdElemento), "Id", "Nombre", salidaelemento.IdElemento);
            ViewBag.IdFormula = new SelectList(db.Formulas.Where(f => f.IdElemento == salidaelemento.IdElemento), "Id", "Nombre", salidaelemento.IdFormula);

            return View(salidaelemento);
        }

        //
        // GET: /Modelo/SalidaElemento/Delete/5

        public ActionResult DeleteSalidaElemento(int id)
        {
            SalidaElemento salidaelemento = db.SalidaElementos.Find(id);
            if (salidaelemento == null)
            {
                return HttpNotFound();
            }

            return View(salidaelemento);
        }

        //
        // POST: /Modelo/SalidaElemento/Delete/5

        [HttpPost, ActionName("DeleteSalidaElemento")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSalidaElementoConfirmed(int id)
        {
            SalidaElemento salidaelemento = db.SalidaElementos.Find(id);
            try
            {
                db.SalidaElementos.Remove(salidaelemento);
                db.SaveChanges();
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No se puede eliminar porque existen registros dependientes.");

                return View("DeleteSalidaElemento", salidaelemento);
            }

            return RedirectToAction("Pizarra", new { id = salidaelemento.IdElemento });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Modelo.Models;

namespace TesisProj.Areas.Modelo.Controllers
{
    public partial class ProyectoController : Controller
    {
        //
        // GET: /Modelo/Proyecto/Catalog/5

        public ActionResult Catalog(int id)
        {
            Elemento elemento = db.Elementos.Find(id);
            Proyecto proyecto = db.Proyectos.Find(elemento.IdProyecto);
            if (elemento == null)
            {
                return HttpNotFound();
            }
            var parametros = db.Parametros.Include("Elemento").Include("TipoDato").Where(p => p.IdElemento == elemento.Id).OrderBy(p => p.Nombre);

            ViewBag.Elemento = elemento.Nombre;
            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.ElementoId = elemento.Id;
            ViewBag.ProyectoId = proyecto.Id;

            return View(parametros.ToList());
        }

        //
        // GET: /Modelo/Proyecto/PutParametros/5

        public ActionResult PutParametros(int id = 0)
        {
            Elemento elemento = db.Elementos.Find(id);
            Proyecto proyecto = db.Proyectos.Find(elemento.IdProyecto);
            if (elemento == null)
            {
                return HttpNotFound();
            }

            var parametros = db.Parametros.Include("TipoDato").Where(p => p.IdElemento == elemento.Id).OrderBy(p => p.Nombre); ;
            // List<Celda> celdas = new List<Celda>();

            foreach(Parametro parametro in parametros)
            {
                for (int periodo = 1; periodo <= proyecto.Horizonte; periodo++)
                {
                    db.Celdas.Add(new Celda { IdParametro = parametro.Id, Periodo = periodo, Valor = 0 });
                }
            }

            db.SaveChanges();

            var celdas = db.Celdas.Include(c => c.Parametro).Where(c => c.Parametro.IdElemento == id && c.Periodo == 1).ToList();

            ViewBag.IdProyecto = proyecto.Id;
            ViewBag.IdElemento = elemento.Id;
            ViewBag.Elemento = elemento.Nombre;
            ViewBag.Proyecto = proyecto.Nombre;

            return View(celdas);
        }

        //
        // POST: /Modelo/Proyecto/PutParametros
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PutParametros(List<Celda> celdas, int IdElemento, int IdProyecto)
        {
            Proyecto proyecto = db.Proyectos.Find(IdProyecto);
            int Horizonte = proyecto.Horizonte;

            if (ModelState.IsValid)
            {

                foreach (Celda celda in celdas)
                {
                    Parametro parametro = db.Parametros.Find(celda.IdParametro);
                    for (int periodo = 2; periodo <= Horizonte; periodo++)
                    {
                        Celda nextCelda = db.Celdas.First(c => c.Periodo == periodo && c.IdParametro == celda.IdParametro);
                        nextCelda.Valor = celda.Valor;
                        db.CeldasRequester.ModifyElement(celda);
                        celda.Parametro = parametro;
                    }
                }

                return RedirectToAction("SetParametros", new { id = IdElemento });
            }

            foreach (Celda celda in celdas)
            {
                Parametro parametro = db.Parametros.Find(celda.IdParametro);
                celda.Parametro = parametro;
            }

            Elemento elemento = db.Elementos.Find(IdElemento);

            ViewBag.IdProyecto = proyecto.Id;
            ViewBag.IdElemento = elemento.Id;
            ViewBag.Elemento = elemento.Nombre;
            ViewBag.Proyecto = proyecto.Nombre;

            return View(celdas);
            
        }

        //
        // GET: /Modelo/Proyecto/VerParametros/5

        public ActionResult VerParametros(int id = 0)
        {
            Elemento elemento = db.Elementos.Find(id);
            Proyecto proyecto = db.Proyectos.Find(elemento.IdProyecto);
            if (elemento == null || proyecto == null)
            {
                return HttpNotFound();
            }

            var parametros = db.Parametros.Include("TipoDato").Where(p => p.IdElemento == elemento.Id).OrderBy(p => p.Nombre); ;
            var celdas = db.Celdas.Include("Parametro").Where(c => c.Parametro.IdElemento == id && (c.Parametro.Constante ? c.Periodo == 1 : true));


            int idUser = getUserId();

            bool IsCreador = (idUser == proyecto.IdCreador);
            bool IsEditor = IsCreador ? false : db.Colaboradores.Any(c => c.IdProyecto == proyecto.Id && c.IdUsuario == idUser && !c.SoloLectura);
            bool IsRevisor = (IsCreador || IsEditor) ? false : true;

            ViewBag.IsCreador = IsCreador;
            ViewBag.IsEditor = IsEditor;
            ViewBag.IsRevisor = IsRevisor;

            ViewBag.IdProyecto = proyecto.Id;
            ViewBag.IdElemento = elemento.Id;
            ViewBag.Parametros = parametros.ToList();
            ViewBag.Horizonte = celdas.Any(c => c.Periodo > 1) ? proyecto.Horizonte : 1;
            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.Elemento = elemento.Nombre;

            return View(celdas.ToList());
        }

        //
        // GET: /Modelo/Proyecto/SetParametros/5

        public ActionResult SetParametros(int id = 0)
        {
            Elemento elemento = db.Elementos.Find(id);
            Proyecto proyecto = db.Proyectos.Find(elemento.IdProyecto);
            if (elemento == null)
            {
                return HttpNotFound();
            }

            var parametros = db.Parametros.Include("TipoDato").Where(p => p.IdElemento == elemento.Id).OrderBy(p => p.Nombre); ;
            var celdas = db.Celdas.Include("Parametro").Where(c => c.Parametro.IdElemento == id && (c.Parametro.Constante ? c.Periodo == 1 : true));

            ViewBag.IdProyecto = proyecto.Id;
            ViewBag.IdElemento = elemento.Id;
            ViewBag.Parametros = parametros.ToList();
            ViewBag.Horizonte = celdas.Any(c => c.Periodo > 1) ? proyecto.Horizonte : 1;
            ViewBag.Elemento = elemento.Nombre;
            ViewBag.Proyecto = proyecto.Nombre;

            return View(celdas.ToList());
        }

        //
        // GET: /Modelo/Proyecto/SetParametros

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetParametros(List<Celda> celdas, int IdElemento, int IdProyecto)
        {
            if (ModelState.IsValid)
            {
                foreach (Celda celda in celdas)
                {
                    celda.Parametro = db.Parametros.Include(p => p.Elemento).FirstOrDefault(p => p.Id == celda.IdParametro);
                    db.CeldasRequester.ModifyElement(celda, true, IdProyecto, getUserId());
                }

                return RedirectToAction("VerParametros", new { id = IdElemento });
            }

            var parametros = db.Parametros.Include("TipoDato").Where(p => p.IdElemento == IdElemento).OrderBy(p => p.Nombre); ;
            Elemento elemento = db.Elementos.Find(IdElemento);
            Proyecto proyecto = db.Proyectos.Find(IdProyecto);

            ViewBag.IdProyecto = IdProyecto;
            ViewBag.IdElemento = IdElemento;
            ViewBag.Parametros = parametros.ToList();
            ViewBag.Horizonte = celdas.Any(c => c.Periodo > 1) ? proyecto.Horizonte : 1;
            ViewBag.Elemento = elemento.Nombre;
            ViewBag.Proyecto = proyecto.Nombre;

            return View(celdas.ToList());    
        }

        //
        // GET: /Modelo/Proyecto/CreateParametro/5

        public ActionResult CreateParametro(int idElemento = 0)
        {
            Elemento elemento = db.Elementos.Find(idElemento);
            Proyecto proyecto = db.Proyectos.Find(elemento.IdProyecto);
            if (elemento == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdTipoParametro = new SelectList(db.TipoDatos, "Id", "Nombre");
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(e => e.Id == elemento.Id), "Id", "Nombre", elemento.Id);
            ViewBag.IdElementoReturn = elemento.Id;
            ViewBag.IdProyecto = proyecto.Id;
            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.Elemento = elemento.Nombre;

            return View();
        }

        //
        // POST: /Modelo/Proyecto/CreateParametro

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateParametro(Parametro parametro, string ValorInicial, int IdProyecto)
        {
            decimal valor = 0;
            Proyecto proyecto = db.Proyectos.Find(IdProyecto);
            try
            {
                valor = decimal.Parse(ValorInicial);
            }
            catch
            {
                ModelState.AddModelError("ValorInicial", "El valor inicial debe ser numérico.");
            }

            if (ModelState.IsValid)
            {
                parametro.Elemento = db.Elementos.Find(parametro.IdElemento);
                parametro.TipoDato = db.TipoDatos.Find(parametro.IdTipoDato);
                db.ParametrosRequester.AddElement(parametro, true, parametro.Elemento.IdProyecto, getUserId());              

                for (int periodo = 1; periodo <= proyecto.Horizonte; periodo++)
                {
                    db.CeldasRequester.AddElement(new Celda { IdParametro = parametro.Id, Periodo = periodo, Valor = valor });
                }

                return RedirectToAction("SetParametros", new { id = parametro.IdElemento });
            }

            ViewBag.IdTipoParametro = new SelectList(db.TipoDatos, "Id", "Nombre", parametro.IdTipoDato);
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(e => e.Id == parametro.IdElemento), "Id", "Nombre", parametro.IdElemento);
            ViewBag.IdElementoReturn = parametro.IdElemento;

            Elemento elemento = db.Elementos.Find(parametro.IdElemento);
            ViewBag.IdProyecto = proyecto.Id;
            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.Elemento = elemento.Nombre;

            return View(parametro);
        }

        //
        // GET: /Modelo/Proyecto/EditParametro/5

        public ActionResult EditParametro(int id = 0)
        {
            Parametro parametro = db.Parametros.Find(id);
            if (parametro == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdTipoParametro = new SelectList(db.TipoDatos, "Id", "Nombre", parametro.IdTipoDato);
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(e => e.Id == parametro.IdElemento), "Id", "Nombre", parametro.IdElemento);
            ViewBag.IdElementoReturn = parametro.IdElemento;

            Elemento elemento = db.Elementos.Find(parametro.IdElemento);
            Proyecto proyecto = db.Proyectos.Find(elemento.IdProyecto);

            ViewBag.Elemento = elemento.Nombre;
            ViewBag.Proyecto = proyecto.Nombre;

            return View(parametro);
        }

        //
        // POST: /Modelo/Proyecto/EditParametro

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditParametro(Parametro parametro)
        {
            if (ModelState.IsValid)
            {
                parametro.Elemento = db.Elementos.Find(parametro.IdElemento);
                parametro.TipoDato = db.TipoDatos.Find(parametro.IdTipoDato);
                db.ParametrosRequester.ModifyElement(parametro, true, parametro.Elemento.IdProyecto, getUserId());

                return RedirectToAction("SetParametros", new { id = parametro.IdElemento });
            }

            ViewBag.IdTipoParametro = new SelectList(db.TipoDatos, "Id", "Nombre", parametro.IdTipoDato);
            ViewBag.IdElemento = new SelectList(db.Elementos.Where(e => e.Id == parametro.IdElemento), "Id", "Nombre", parametro.IdElemento);
            ViewBag.IdElementoReturn = parametro.IdElemento;

            Elemento elemento = db.Elementos.Find(parametro.IdElemento);
            Proyecto proyecto = db.Proyectos.Find(elemento.IdProyecto);

            ViewBag.Elemento = elemento.Nombre;
            ViewBag.Proyecto = proyecto.Nombre;

            return View(parametro);
        }

        //
        // GET: /Modelo/Proyecto/DeleteParametro/5

        public ActionResult DeleteParametro(int id)
        {
            Parametro parametro = db.Parametros.Find(id);
            try
            {
                var celdas = db.Celdas.Where(c => c.IdParametro == parametro.Id).ToList();
                foreach (Celda celda in celdas)
                {
                    db.CeldasRequester.RemoveElementByID(celda.Id);
                }

                db.ParametrosRequester.RemoveElementByID(parametro.Id, true, true, parametro.Elemento.IdProyecto, getUserId());
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No se puede eliminar porque existen registros dependientes.");
            }

            return RedirectToAction("Catalog", new { id = parametro.IdElemento });
        }

    }
}

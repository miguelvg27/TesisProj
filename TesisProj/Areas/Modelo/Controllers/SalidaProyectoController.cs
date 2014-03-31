using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Plantilla.Models;
using TesisProj.Models;
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
            ViewBag.LastCalculated = proyecto.Calculado;
            ViewBag.LastModified = db.Audits.Where(a => a.IdProyecto == id).Count() > 0 ? db.Audits.Where(a => a.IdProyecto == id).Max(a => a.Fecha) : proyecto.Creacion;

            var salidaproyectos = db.SalidaProyectos.Include(s => s.Proyecto).Where(s => s.IdProyecto == proyecto.Id);

            int idUser = getUserId();

            bool IsCreador = (idUser == proyecto.IdCreador);
            bool IsEditor = IsCreador ? false : db.Colaboradores.Any(c => c.IdProyecto == proyecto.Id && c.IdUsuario == idUser && !c.SoloLectura);
            bool IsRevisor = (IsCreador || IsEditor) ? false : true;

            ViewBag.IsCreador = IsCreador;
            ViewBag.IsEditor = IsEditor;
            ViewBag.IsRevisor = IsRevisor;
            
            return View(salidaproyectos.ToList());
        }

        public static List<Operacion> CalcularProyecto(int horizonte, int preoperativos, int cierre, List<Operacion> operaciones, List<Parametro> parametros, List<Formula> formulas, List<TipoFormula> tipoformulas, bool simular = false)
        {
            //  Lleno los valores de las referencias a tipos de fórmula

            foreach (TipoFormula tipoformula in tipoformulas)
            {
                tipoformula.Valores = new double[horizonte];
                Array.Clear(tipoformula.Valores, 0, horizonte);

                //
                //  Cojo las fórmulas del proyecto de ese tipo
                var formulitas = formulas.Where(f => f.IdTipoFormula == tipoformula.Id).ToList();

                foreach (Formula formulita in formulitas)
                {
                    //  Cojo las fórmulas y parámetros del elemento de referencia (secuencia menor)
                    var refFormulitas = formulas.Where(f => f.Secuencia < formulita.Secuencia && f.IdElemento == formulita.IdElemento).ToList();
                    var refParametros = parametros.Where(p => p.IdElemento == formulita.IdElemento).ToList();
                    formulita.Valores = formulita.Evaluar(horizonte, preoperativos, cierre, refFormulitas, refParametros, simular);
                    
                    //  Sumo los elementos
                    tipoformula.Valores = tipoformula.Valores.Zip(formulita.Valores, (x, y) => x + y).ToArray();
                }
            }

            foreach (Operacion operacion in operaciones)
            {
                //  Cojo las operaciones de referencia (secuencia menor)
                var refoperaciones = operaciones.Where(o => o.Secuencia < operacion.Secuencia).ToList();
                operacion.Valores = operacion.Evaluar(horizonte, preoperativos, cierre, refoperaciones, tipoformulas, formulas, parametros);
            }

            return operaciones;
        }

        //
        // GET: /Modelo/Proyecto/Pelicula/5

        public ActionResult Pelicula(int id = 0)
        {
            SalidaProyecto salida = db.SalidaProyectos.Find(id);
            Proyecto proyecto = db.Proyectos.Find(salida.IdProyecto);

            if (salida == null)
            {
                return HttpNotFound();
            }

            var exoperaciones = db.SalidaOperaciones.Where(s => s.IdSalida == salida.Id).OrderBy(s => s.Secuencia).Select(s => s.Operacion).ToList();

            ViewBag.IdProyecto = salida.IdProyecto;
            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.Salida = salida.Nombre;
            ViewBag.Inicio = Convert.ToInt32(Generics.SimpleParse(salida.PeriodoInicial, proyecto.Horizonte, 1, proyecto.PeriodosPreOp, proyecto.PeriodosCierre));
            ViewBag.Horizonte = Convert.ToInt32(Generics.SimpleParse(salida.PeriodoFinal, proyecto.Horizonte, proyecto.Horizonte, proyecto.PeriodosPreOp, proyecto.PeriodosCierre));

            foreach(Operacion operacion in exoperaciones)
            {
                operacion.Valores = operacion.strValores != null ? StringToArray(operacion) : new List<double>();
            }

            return View(exoperaciones.ToList());
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

            var asociados = db.SalidaOperaciones.Include(p => p.Operacion).Where(p => p.IdSalida == salida.Id).OrderBy(p => p.Secuencia).Select(p => p.Operacion);
            var opciones = db.Operaciones.Where(o => o.IdProyecto == salida.IdProyecto);
            ViewBag.Asociados = new MultiSelectList(asociados.ToList(), "Id", "ListName");
            ViewBag.Opciones = new MultiSelectList(opciones.OrderBy(o => o.Secuencia).ToList(), "Id", "ListName");
            ViewBag.Proyecto = db.Proyectos.Find(salida.IdProyecto).Nombre;

            return View(salida);
        }

        //
        // POST: /Modelo/Proyecto/Assoc

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assoc(SalidaProyecto salida, FormCollection form)
        {
            string strSeleccionados = form["Asociados"];
            if (strSeleccionados == null) return RedirectToAction("Cine", new { id = salida.IdProyecto });

            string[] seleccionados = strSeleccionados.Split(',');
            var operaciones = db.SalidaOperaciones.Where(s => s.IdSalida == salida.Id);
            foreach (SalidaOperacion oldOperacion in operaciones)
            {
                db.SalidaOperaciones.Remove(oldOperacion);
            }
            db.SaveChanges();

            for (int i = 0; i < seleccionados.Length; i++)
            {
                int idOperacion = int.Parse(seleccionados[i]);
                db.SalidaOperaciones.Add(new SalidaOperacion { IdOperacion = idOperacion, IdSalida = salida.Id, Secuencia = i });
            }
            db.SaveChanges();

            db.SalidaProyectosRequester.ModifyElement(salida, true, salida.IdProyecto, getUserId());

            return RedirectToAction("Cine", new { id = salida.IdProyecto });
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
            ViewBag.Proyecto = proyecto.Nombre;

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
                db.SalidaProyectosRequester.AddElement(salidaproyecto, true, salidaproyecto.IdProyecto, getUserId());
                return RedirectToAction("Cine", new { id = salidaproyecto.IdProyecto });
            }

            ViewBag.IdProyecto = new SelectList(db.Proyectos.Where(p => p.Id == salidaproyecto.IdProyecto), "Id", "Nombre", salidaproyecto.IdProyecto);
            ViewBag.IdProyectoReturn = salidaproyecto.IdProyecto;

            ViewBag.Proyecto = db.Proyectos.Find(salidaproyecto.IdProyecto).Nombre;

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
            ViewBag.Proyecto = db.Proyectos.Find(salidaproyecto.IdProyecto).Nombre;

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
                db.SalidaProyectosRequester.ModifyElement(salidaproyecto, true, salidaproyecto.IdProyecto, getUserId());
                return RedirectToAction("Cine", new { id = salidaproyecto.IdProyecto });
            }

            ViewBag.IdProyecto = new SelectList(db.Proyectos.Where(p => p.Id == salidaproyecto.IdProyecto), "Id", "Nombre", salidaproyecto.IdProyecto);
            ViewBag.Proyecto = db.Proyectos.Find(salidaproyecto.IdProyecto).Nombre;

            return View(salidaproyecto);
        }


        public ActionResult Calc(int id = 0)
        {

            //
            //  Comienza zona crítica 

            db.Configuration.ProxyCreationEnabled = false;
            Proyecto proyecto = db.Proyectos.Find(id);

            if (proyecto == null)
            {
                db.Configuration.ProxyCreationEnabled = true;
                return HttpNotFound();
            }

            int horizonte = proyecto.Horizonte;
            int preoperativos = proyecto.PeriodosPreOp;
            int cierre = proyecto.PeriodosCierre;

            var operaciones = db.Operaciones.Where(o => o.IdProyecto == id).OrderBy(s => s.Secuencia).ToList();
            var formulas = db.Formulas.Include("Elemento").Where(f => f.Elemento.IdProyecto == id).ToList();
            var parametros = db.Parametros.Include("Elemento").Include("Celdas").Where(e => e.Elemento.IdProyecto == id).ToList();
            var tipoformulas = db.TipoFormulas.ToList();

            CalcularProyecto(horizonte, preoperativos, cierre, operaciones, parametros, formulas, tipoformulas);
            db.Configuration.ProxyCreationEnabled = true;

            //
            //  Finaliza zona crítica

            foreach (Operacion operacion in operaciones)
            {
                operacion.strValores = ArrayToString(operacion);
                db.OperacionesRequester.ModifyElement(operacion);
            }

            proyecto.Calculado = DateTime.Now;
            db.ProyectosRequester.ModifyElement(proyecto);

            return RedirectToAction("Cine", new { id = id });
        }

        private string ArrayToString(Operacion operacion)
        {
            string str = String.Join(",", operacion.Valores.Select(p => p.ToString()).ToArray());
            return str;
        }

        private List<double> StringToArray(Operacion operacion)
        {
            List<double> arr = operacion.strValores.Split(',').Select(s => double.Parse(s)).ToList();
            return arr;
        }

        //
        // GET: /Modelo/SalidaProyecto/Delete/5

        public ActionResult DeleteSalidaProyecto(int id)
        {
            SalidaProyecto salidaproyecto = db.SalidaProyectos.Find(id);

            try
            {
                var operaciones = db.SalidaOperaciones.Where(s => s.IdSalida == salidaproyecto.Id).ToList();

                foreach (SalidaOperacion operacion in operaciones)
                {
                    db.SalidaOperacionesRequester.RemoveElementByID(operacion.Id);
                }

                db.SalidaProyectosRequester.RemoveElementByID(salidaproyecto.Id, true, true, salidaproyecto.IdProyecto, getUserId());
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No se puede eliminar porque existen registros dependientes.");
            }

            return RedirectToAction("Cine", new { id = salidaproyecto.IdProyecto });
        }

        public ActionResult DuplicarSalida(int id)
        {
            SalidaProyecto salida = db.SalidaProyectos.Include(s => s.Operaciones).FirstOrDefault(e => e.Id == id);

            if (salida == null)
            {
                return HttpNotFound();
            }

            string nombre = "Copia de " + salida.Nombre + " ";
            string nombreTest = nombre;
            int i = 1;

            while (db.SalidaProyectos.Any(p => p.Nombre.Equals(nombreTest)))
            {
                nombreTest = nombre + i++;
            }

            int seq = db.SalidaProyectos.Where(s => s.IdProyecto == salida.IdProyecto).Max(s => s.Secuencia) + 1;
            int idCopia = db.SalidaProyectosRequester.AddElement(new SalidaProyecto { Nombre = nombreTest, Secuencia = seq, PeriodoFinal = salida.PeriodoFinal, PeriodoInicial = salida.PeriodoInicial, IdProyecto = salida.IdProyecto }, true, salida.IdProyecto, getUserId());


            foreach (SalidaOperacion operacion in salida.Operaciones.OrderBy(o => o.Secuencia))
            {
                db.SalidaOperacionesRequester.AddElement(new SalidaOperacion { IdSalida = idCopia, IdOperacion = operacion.IdOperacion, Secuencia = operacion.Secuencia });
            }

            return RedirectToAction("EditSalidaProyecto", new { id = idCopia });
        }
    }
}
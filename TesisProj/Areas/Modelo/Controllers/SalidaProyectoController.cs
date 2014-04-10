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
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Modelo.Controllers
{
    public partial class ProyectoController : Controller
    {

        // Permisos: Creador, Editor, Revisor
        // GET: /Modelo/Proyecto/Cine/5

        public ActionResult Cine(int id = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            if (proyecto == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Get project and check user
            int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == proyecto.Id);
            if (current == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            bool IsCreador = current.Creador;
            bool IsEditor = !current.Creador && !current.SoloLectura;
            bool IsRevisor = current.SoloLectura;

            ViewBag.IsCreador = IsCreador;
            ViewBag.IsEditor = IsEditor;
            ViewBag.IsRevisor = IsRevisor;

            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.ProyectoId = proyecto.Id;
            ViewBag.LastCalculated = proyecto.Calculado;
            ViewBag.LastModified = db.Audits.Where(a => a.IdProyecto == id).Count() > 0 ? db.Audits.Where(a => a.IdProyecto == id).Max(a => a.Fecha) : proyecto.Creacion;

            var salidaproyectos = db.SalidaProyectos.Include(s => s.Proyecto).Where(s => s.IdProyecto == proyecto.Id);           
            return View(salidaproyectos.ToList());
        }


        // Permisos: Creador, Editor, Revisor
        // GET: /Modelo/Proyecto/Calcular/5
        public ActionResult Calcular(int id = 0)
        {

            //  Comienza zona crítica 
            db.Configuration.ProxyCreationEnabled = false;
            Proyecto proyecto = db.Proyectos.Find(id);
            if (proyecto == null)
            {
                db.Configuration.ProxyCreationEnabled = true;
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Get project and check user
            int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == proyecto.Id);
            if (current == null)
            {
                db.Configuration.ProxyCreationEnabled = true;
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            int horizonte = proyecto.Horizonte;
            int preoperativos = proyecto.PeriodosPreOp;
            int cierre = proyecto.PeriodosCierre;

            var operaciones = db.Operaciones.Where(o => o.IdProyecto == id).OrderBy(s => s.Secuencia).ToList();

            var elementos = db.Elementos.Include(f => f.Formulas).Include(f => f.Parametros).Include("Parametros.Celdas").Where(e => e.IdProyecto == id).ToList();
            var tipoformulas = db.TipoFormulas.ToList();

            CalcularProyecto(horizonte, preoperativos, cierre, operaciones, elementos, tipoformulas);
            db.Configuration.ProxyCreationEnabled = true;

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

        // Permisos: Creador, Editor, Revisor
        // horizonte: Horizonte del proyecto
        // preoperativos: Períodos preoperativos del proyecto
        // cierre: Períodos de cierre del proyecto
        // operaciones: Operaciones del proyecto ordenadas por secuencia
        // parámetros: Parámetros de todos los elementos del proyecto
        // formulas: Fórmulas de todos los elementos del proyecto
        // tipoformulas: Arreglo de todos los tipo de fórmula
        // simular: Flag de simulación

        public static List<Operacion> CalcularProyecto(int horizonte, int preoperativos, int cierre, List<Operacion> operaciones, List<Elemento> elementos, List<TipoFormula> tipoformulas, bool simular = false)
        {
            foreach (TipoFormula tipoformula in tipoformulas)
            {
                tipoformula.Valores = new double[horizonte];
                Array.Clear(tipoformula.Valores, 0, horizonte);
            }
            
            foreach (Elemento elemento in elementos)
            {
                var refFormulas = new List<Formula>();
                var valFormulas = elemento.Formulas;
                var refParametros = elemento.Parametros;

                foreach (Formula formula in valFormulas)
                {
                    formula.Evaluar(horizonte, preoperativos, cierre, refFormulas, refParametros, simular);
                    refFormulas.Add(formula);

                    //  Sumo los elementos
                    var tipoformula = tipoformulas.First(t => t.Id == formula.IdTipoFormula);
                    tipoformula.Valores = tipoformula.Valores.Zip(formula.Valores, (x, y) => x + y).ToArray();
                }
            }

            var refOperaciones = new List<Operacion>();
            foreach (Operacion operacion in operaciones)
            {
                operacion.Evaluar(horizonte, preoperativos, cierre, refOperaciones, tipoformulas);
                refOperaciones.Add(operacion);
            }

            return operaciones;
        }

        // Permisos: Creador, Editor, Revisor
        // GET: /Modelo/Proyecto/Pelicula/5

        public ActionResult Pelicula(int id = 0)
        {
            SalidaProyecto salida = db.SalidaProyectos.Find(id);
            if (salida == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

             // Get project and check user
            Proyecto proyecto = db.Proyectos.Find(salida.IdProyecto); int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == proyecto.Id);
            if (current == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            var exoperaciones = db.SalidaOperaciones.Where(s => s.IdSalida == salida.Id).OrderBy(s => s.Secuencia).Select(s => s.Operacion).Include(o => o.TipoDato).ToList();

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

        // Permisos: Creador, Editor
        // POST: /Modelo/Proyecto/Assoc/5

        public ActionResult Assoc(int id = 0)
        {
            SalidaProyecto salida = db.SalidaProyectos.Find(id);
            if (salida == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Get project and check user
            Proyecto proyecto = db.Proyectos.Find(salida.IdProyecto); int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == proyecto.Id);
            if (current == null || current.SoloLectura)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
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
            var operaciones = db.SalidaOperaciones.Where(s => s.IdSalida == salida.Id).ToList();
            foreach (SalidaOperacion oldOperacion in operaciones)
            {
                db.SalidaOperacionesRequester.RemoveElementByID(oldOperacion.Id);
            }

            for (int i = 0; i < seleccionados.Length; i++)
            {
                int idOperacion = int.Parse(seleccionados[i]);
                db.SalidaOperacionesRequester.AddElement(new SalidaOperacion { IdOperacion = idOperacion, IdSalida = salida.Id, Secuencia = i });
            }

            db.SalidaProyectosRequester.ModifyElement(salida, true, salida.IdProyecto, getUserId());
            return RedirectToAction("Cine", new { id = salida.IdProyecto });
        }

        // Permisos: Creador, Editor
        // GET: /Modelo/Proyecto/CreateSalidaProyecto

        public ActionResult CreateSalidaProyecto(int idProyecto = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(idProyecto);
            if (proyecto == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Get project and check user
            int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == proyecto.Id);
            if (current == null || current.SoloLectura)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            ViewBag.IdProyecto = new SelectList(db.Proyectos.Where(p => p.Id == proyecto.Id), "Id", "Nombre", proyecto.Id);
            ViewBag.IdProyectoReturn = proyecto.Id;
            ViewBag.Proyecto = proyecto.Nombre;

            var salidas = db.SalidaProyectos.Where(f => f.IdProyecto == proyecto.Id);
            int defSecuencia = salidas.Count() > 0 ? salidas.Max(f => f.Secuencia) + 10 : 10;
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
            return View(salidaproyecto);
        }

        // Permisos: Creador, Editor
        // GET: /Modelo/Proyecto/EditSalidaProyecto/5

        public ActionResult EditSalidaProyecto(int id = 0)
        {
            SalidaProyecto salida = db.SalidaProyectos.Find(id);
            if (salida == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Get project and check user
            Proyecto proyecto = db.Proyectos.Find(salida.IdProyecto); int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == proyecto.Id);
            if (current == null || current.SoloLectura)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            ViewBag.IdProyecto = new SelectList(db.Proyectos.Where(p => p.Id == salida.IdProyecto), "Id", "Nombre", salida.IdProyecto);
            ViewBag.Proyecto = db.Proyectos.Find(salida.IdProyecto).Nombre;

            return View(salida);
        }

        //
        // POST: /Modelo/Proyecto/EditSalidaProyecto/5

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

        // Permisos: Creador, Editor
        // GET: /Modelo/Proyecto/DeleteSalidaProyecto/5

        public ActionResult DeleteSalidaProyecto(int id = 0)
        {
            SalidaProyecto salida = db.SalidaProyectos.Find(id);
            if (salida == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Get project and check user
            Proyecto proyecto = db.Proyectos.Find(salida.IdProyecto); int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == proyecto.Id);
            if (current == null || current.SoloLectura)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            var operaciones = db.SalidaOperaciones.Where(s => s.IdSalida == salida.Id).ToList();
            foreach (SalidaOperacion operacion in operaciones)
            {
                db.SalidaOperacionesRequester.RemoveElementByID(operacion.Id);
            }

            db.SalidaProyectosRequester.RemoveElementByID(salida.Id, true, true, salida.IdProyecto, getUserId());
            return RedirectToAction("Cine", new { id = salida.IdProyecto });
        }

        // Permisos: Creador, Editor
        // GET: /Modelo/Proyecto/DeleteSalidaProyecto/5

        public ActionResult DuplicarSalida(int id = 0)
        {
            SalidaProyecto salida = db.SalidaProyectos.Find(id);
            if (salida == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            // Get project and check user
            Proyecto proyecto = db.Proyectos.Find(salida.IdProyecto); int currentId = getUserId();
            Colaborador current = db.Colaboradores.FirstOrDefault(c => c.IdUsuario == currentId && c.IdProyecto == proyecto.Id);
            if (current == null || current.SoloLectura)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }

            string nombre = "Copia de " + salida.Nombre + " ";
            string nombreTest = nombre;
            int i = 1;

            while (db.SalidaProyectos.Any(p => p.Nombre.Equals(nombreTest)))
            {
                nombreTest = nombre + i++;
            }

            int seq = db.SalidaProyectos.Where(s => s.IdProyecto == salida.IdProyecto).Max(s => s.Secuencia) + 10;
            int idCopia = db.SalidaProyectosRequester.AddElement(new SalidaProyecto { Nombre = nombreTest, Secuencia = seq, PeriodoFinal = salida.PeriodoFinal, PeriodoInicial = salida.PeriodoInicial, IdProyecto = salida.IdProyecto }, true, salida.IdProyecto, getUserId());

            foreach (SalidaOperacion operacion in salida.Operaciones.OrderBy(o => o.Secuencia))
            {
                db.SalidaOperacionesRequester.AddElement(new SalidaOperacion { IdSalida = idCopia, IdOperacion = operacion.IdOperacion, Secuencia = operacion.Secuencia });
            }

            return RedirectToAction("EditSalidaProyecto", new { id = idCopia });
        }
    }
}
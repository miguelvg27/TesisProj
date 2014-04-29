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

            int horizonte = proyecto.Horizonte;
            int preoperativos = proyecto.PeriodosPreOp;
            int cierre = proyecto.PeriodosCierre;
            var operaciones = db.Operaciones.Where(o => o.IdProyecto == id).ToList();
            var elementos = db.Elementos.Include(f => f.Formulas).Include(f => f.Parametros).Include("Parametros.Celdas").Where(e => e.IdProyecto == id).ToList();
            var tipoformulas = db.TipoFormulas.ToList();

            CalcularProyecto(horizonte, preoperativos, cierre, operaciones, elementos, tipoformulas);
            LimpiarRuta(operaciones, elementos, tipoformulas);
            MarcarRutaSensible(operaciones, elementos, tipoformulas);
            MarcarRutaSimulable(operaciones, elementos, tipoformulas);
            GuardarCalculoRuta(proyecto.Id, operaciones, elementos);

            proyecto = db.Proyectos.Find(id);
            proyecto.Calculado = DateTime.Now;
            db.ProyectosRequester.ModifyElement(proyecto);

            return RedirectToAction("Cine", new { id = id });
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
            if (exoperaciones.Any(o => o.strValores == null))
            {
                return RedirectToAction("Calcular", new { id = id });
            }

            ViewBag.IdProyecto = salida.IdProyecto;
            ViewBag.Proyecto = proyecto.Nombre;
            ViewBag.Salida = salida.Nombre;
            ViewBag.Inicio = Convert.ToInt32(Generics.SimpleParse(salida.PeriodoInicial, proyecto.Horizonte, 1, proyecto.PeriodosPreOp, proyecto.PeriodosCierre));
            ViewBag.Horizonte = Convert.ToInt32(Generics.SimpleParse(salida.PeriodoFinal, proyecto.Horizonte, proyecto.Horizonte, proyecto.PeriodosPreOp, proyecto.PeriodosCierre));

            foreach(Operacion operacion in exoperaciones)
            {
                operacion.Valores = StringToArray(operacion.strValores);
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

        private string ArrayToString(double[] arr)
        {
            string str = String.Join(",", arr.Select(p => p.ToString()).ToArray());
            return str;
        }

        private List<double> StringToArray(string str)
        {
            List<double> arr = str.Split(',').Select(s => double.Parse(s)).ToList();
            return arr;
        }

        //  Precondición: Los elementos del proyecto deben estar con los parámetros y fórmulas incluidos
        //  Postcondición: Las fórmulas y las operaciones tendrán su arreglo de valores lleno en el horizonte del proyecto

        public void CalcularProyecto(int horizonte, int preoperativos, int cierre, List<Operacion> operaciones, List<Elemento> elementos, List<TipoFormula> tipoformulas)
        {
            foreach (TipoFormula tipoformula in tipoformulas)
            {
                tipoformula.Valores = new double[horizonte];
                Array.Clear(tipoformula.Valores, 0, horizonte);
            }

            foreach (Elemento elemento in elementos)
            {
                var refFormulas = new List<Formula>();
                var valFormulas = elemento.Formulas.OrderBy(f => f.Secuencia);
                var refParametros = elemento.Parametros;

                foreach (Formula formula in valFormulas)
                {
                    formula.Evaluar(horizonte, preoperativos, cierre, refFormulas, refParametros, false);
                    
                    //  Sumo los elementos
                    TipoFormula tipoformula = tipoformulas.First(t => t.Id == formula.IdTipoFormula);
                    tipoformula.Valores = tipoformula.Valores.Zip(formula.Valores, (x, y) => x + y).ToArray();                   
                    refFormulas.Add(formula);
                }
            }

            var refOperaciones = new List<Operacion>();
            var valOperaciones = operaciones.OrderBy(o => o.Secuencia);
            foreach (Operacion operacion in valOperaciones)
            {
                operacion.Evaluar(horizonte, preoperativos, cierre, refOperaciones, tipoformulas);
                refOperaciones.Add(operacion);           
            }

            return;
        }

        //  Precondición: Los elementos del proyecto deben estar con los parámetros y fórmulas incluidos
        //  Postcondición: Los elementos, fórmulas (y sus tipos) y operaciones tendrán sus flags limpios

        public void LimpiarRuta(List<Operacion> operaciones, List<Elemento> elementos, List<TipoFormula> tipoformulas)
        {
            foreach (TipoFormula tf in tipoformulas)
            {
                tf.Sensible = false;
                tf.Simular = false;
            }

            foreach (Elemento elemento in elementos)
            {
                elemento.Sensible = false;
                elemento.Simular = false;

                foreach (Formula formula in elemento.Formulas)
                {
                    formula.Sensible = false;
                    formula.Simular = false;
                }

                foreach (Parametro parametro in elemento.Parametros)
                {
                    parametro.Simular = false;
                }
            }

            foreach (Operacion operacion in operaciones)
            {
                operacion.Sensible = false;
                operacion.Simular = false;
            }

            return;
        }

        //  Precondición: Los elementos deben estar con los parámetros y fórmulas incluidos
        //  Postcondición: Los elementos, fórmulas (y sus tipos) y operaciones que utilicen parámetros sensibles serán marcados

        public void MarcarRutaSensible(List<Operacion> operaciones, List<Elemento> elementos, List<TipoFormula> tipoformulas)
        {
            foreach (TipoFormula tf in tipoformulas) tf.Sensible = false;

            foreach (Elemento elemento in elementos)
            {
                var refFormulas = new List<Formula>();
                var valFormulas = elemento.Formulas.OrderBy(f => f.Secuencia);
                var refParametros = elemento.Parametros;

                foreach (Formula formula in elemento.Formulas)
                {
                    formula.Sensible = formula.EsSensible(refFormulas.Where(f => !f.Sensible).ToList(), refParametros.Where(p => !p.Sensible).ToList());
                    refFormulas.Add(formula);

                    TipoFormula tipoformula = tipoformulas.First(t => t.Id == formula.IdTipoFormula);
                    tipoformula.Sensible = tipoformula.Sensible || formula.Sensible; 
                }

                elemento.Sensible = elemento.Formulas.Any(f => f.Sensible);
            }

            var refOperaciones = new List<Operacion>();
            var valOperaciones = operaciones.OrderBy(o => o.Secuencia);

            foreach (Operacion operacion in operaciones)
            {
                operacion.Sensible = operacion.EsSensible(refOperaciones.Where(o => !o.Sensible).ToList(), tipoformulas.Where(t => !t.Sensible).ToList());
                refOperaciones.Add(operacion); 
            }

            return;
        }

        //  Precondición: Se debe haber marcado la ruta sensible. No se hará verificación
        //  Precondición: Los elementos deben estar con los parámetros y fórmulas incluidos
        //  Postcondición: Los elementos, fórmulas, operaciones y parámetros variantes serán marcados

        public void MarcarRutaSimulable(List<Operacion> operaciones, List<Elemento> elementos, List<TipoFormula> tipoformulas)
        {
            string[] targets = { "TIRE", "VANE", "TIRF", "VANF" };
            Queue<Operacion> queueOperaciones = new Queue<Operacion>();

            //  Vaciamos una posible ruta anterior
            foreach (Operacion o in operaciones) o.Simular = false;

            //  Metemos en la cola las operaciones de los indicadores objetivo
            //  Encolaremos todas las operaciones que son necesarias para el cálculo de las operaciones target
            foreach (string strOperacion in targets)
            {
                Operacion operacion = operaciones.FirstOrDefault(o => o.Referencia.Equals(strOperacion));
                if (operacion != null && operacion.Sensible) queueOperaciones.Enqueue(operacion);
            }

            //  Mientras hayan objetos en la cola...
            while (queueOperaciones.Count > 0)
            {
                //  Desapilamos una operación y la marcamos
                Operacion operacion = queueOperaciones.Dequeue();
                operacion.Simular = true;

                //  Recopilamos las operaciones de menor secuencia que la desapilada para marcar las necesarias para su cálculo
                List<Operacion> refOperaciones = operaciones.Where(o => o.Secuencia < operacion.Secuencia).OrderBy(o => o.Secuencia).ToList();

                //  Evaluamos la operación para saber si realmente es necesaria dicha operación
                //  En caso sea necesaria (si no se logró calcular la operación sin ella), se encola
                foreach (Operacion refOperacion in refOperaciones)
                {
                    if (!refOperacion.Sensible || refOperacion.Simular) continue;
                    
                    List<Operacion> wrapperOperacion = new List<Operacion>(); wrapperOperacion.Add(refOperacion);
                    refOperacion.Simular = operacion.EsSensible(refOperaciones.Except(wrapperOperacion).ToList(), tipoformulas);

                    if (refOperacion.Simular) queueOperaciones.Enqueue(refOperacion);
                }
            }

            foreach (TipoFormula tf in tipoformulas) tf.Simular = false;

            foreach (TipoFormula tipoformula in tipoformulas)
            {
                List<TipoFormula> wrapperTipoFormula = new List<TipoFormula>(); wrapperTipoFormula.Add(tipoformula);
                var operacionesMarcadas = operaciones.Where(o => o.Simular).ToList();
                foreach (Operacion operacion in operacionesMarcadas)
                {
                    //  Recopilamos las operaciones de menor secuencia que la desapilada para marcar los tipos necesarias para su cálculo
                    List<Operacion> refOperaciones = operaciones.Where(o => o.Secuencia < operacion.Secuencia).OrderBy(o => o.Secuencia).ToList();
                    tipoformula.Simular = operacion.EsSensible(refOperaciones, tipoformulas.Except(wrapperTipoFormula).ToList());
                    if (tipoformula.Simular) break;
                }
            }

            //  Como los elementos no dependen de otros, como las operaciones, se puede descartar los invariantes
            var elementosSensibles = elementos.Where(e => e.Sensible).ToList();
            var tipoformulasMarcados = tipoformulas.Where(t => t.Simular).ToList();

            foreach (Elemento elemento in elementosSensibles)
            {
                Queue<Formula> queueFormulas = new Queue<Formula>();
                var refParametros = elemento.Parametros.ToList();

                foreach (Formula formula in elemento.Formulas)
                {
                    formula.Simular = false;
                    if (tipoformulasMarcados.Any(t => t.Id == formula.IdTipoFormula))
                    {
                        queueFormulas.Enqueue(formula);
                    }
                }

                elemento.Simular = queueFormulas.Count > 0;

                while (queueFormulas.Count > 0)
                {
                    //  Desapilamos una operación y la marcamos
                    Formula formula = queueFormulas.Dequeue();
                    formula.Simular = true;

                    //  Recopilamos las operaciones de menor secuencia que la desapilada para marcar las necesarias para su cálculo
                    List<Formula> refFormulas = elemento.Formulas.Where(o => o.Secuencia < formula.Secuencia).OrderBy(o => o.Secuencia).ToList();

                    //  Evaluamos la operación para saber si realmente es necesaria dicha operación
                    //  En caso sea necesaria (si no se logró calcular la operación sin ella), se encola
                    foreach (Formula refFormula in refFormulas)
                    {
                        if (!refFormula.Sensible || refFormula.Simular) continue;

                        List<Formula> wrapperFormula = new List<Formula>(); wrapperFormula.Add(refFormula);
                        refFormula.Simular = formula.EsSensible(refFormulas.Except(wrapperFormula).ToList(), refParametros);

                        if (refFormula.Simular) queueFormulas.Enqueue(refFormula);
                    }

                    foreach (Parametro refParametro in refParametros)
                    {
                        if (refParametro.Simular) continue;

                        List<Parametro> wrapperParametro = new List<Parametro>(); wrapperParametro.Add(refParametro);
                        refParametro.Simular = formula.EsSensible(refFormulas, refParametros.Except(wrapperParametro).ToList());
                    }
                }

            }
        }

        public void GuardarCalculoRuta(int idProyecto, List<Operacion> operaciones, List<Elemento> elementos)
        {
            foreach (Operacion operacion in operaciones)
            {
                operacion.strValores = ArrayToString(operacion.Valores.ToArray());
                db.OperacionesRequester.ModifyElement(operacion);
            }

            foreach (Elemento elemento in elementos)
            {
                db.ElementosRequester.ModifyElement(elemento);

                foreach (Formula formula in elemento.Formulas)
                {
                    formula.strValores = ArrayToString(formula.Valores.ToArray());
                    db.FormulasRequester.ModifyElement(formula);
                }
            }
        }
    }
}
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
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Modelo.Controllers
{
    [Authorize(Roles = "nav")]
    public partial class ProyectoController : Controller
    {
        private TProjContext db = new TProjContext();
        private int userId = 0;

        public static SimAns simular(int horizonte, int preoperativos, int cierre, List<Operacion> operaciones, List<Parametro> parametros, List<Formula> formulas, List<TipoFormula> tipoformulas, bool siSimular = true)
        {
            SimAns resultado = new SimAns { TirE = 0, TirF = 0, VanE = 0, VanF = 0 };

            CalcularProyecto(horizonte, preoperativos, cierre, operaciones, parametros, formulas, tipoformulas, siSimular);

            Operacion tire = operaciones.FirstOrDefault(o => o.Referencia.Equals("TIRE"));
            Operacion tirf = operaciones.FirstOrDefault(o => o.Referencia.Equals("TIRF"));
            Operacion vane = operaciones.FirstOrDefault(o => o.Referencia.Equals("VANE"));
            Operacion vanf = operaciones.FirstOrDefault(o => o.Referencia.Equals("VANF"));

            try
            {
                resultado.TirE = tire == null ? 0 : tire.Valores[0];
                resultado.TirF = tirf == null ? 0 : tirf.Valores[0];
                resultado.VanE = vane == null ? 0 : vane.Valores[0];
                resultado.VanF = vanf == null ? 0 : vanf.Valores[0];
            }
            catch (Exception)
            {
            }
        
            return resultado;
        }

        //
        // GET: /Modelo/Proyecto/

        private int getUserId()
        {
            if(userId < 1){
                try
                {
                    userId = db.UserProfiles.First(u => u.UserName == User.Identity.Name).UserId;
                }
                catch (Exception)
                {
                    return 0;
                }
            }

            return userId;
        }

        public ActionResult Index()
        {
            int idUser = getUserId();
            var proyectos = db.Proyectos.Include(p => p.Creador).Where(p => p.Creador.UserName.Equals(User.Identity.Name)).ToList();
            var colab = db.Colaboradores.Include(c => c.Proyecto).Where(c => c.IdUsuario == idUser).Select(c => c.Proyecto).Include(p => p.Creador).ToList();
            
            return View(proyectos.Union(colab).ToList());
        }

        //
        // GET: /Modelo/Console/5

        public ActionResult Console(int id = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            if (proyecto == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }
            proyecto.Creador = db.UserProfiles.Find(proyecto.IdCreador);
            
            int idUser = getUserId();

            bool IsCreador = (idUser == proyecto.IdCreador);
            bool IsEditor = IsCreador ? false : db.Colaboradores.Any(c => c.IdProyecto == proyecto.Id && c.IdUsuario == idUser && !c.SoloLectura);
            bool IsRevisor = (IsCreador || IsEditor) ? false : true;

            ViewBag.IsCreador = IsCreador;
            ViewBag.IsEditor = IsEditor;
            ViewBag.IsRevisor = IsRevisor;

            return View(proyecto);
        }

        //
        // GET: /Modelo/Proyecto/Create

        public ActionResult Create()
        {
            ViewBag.IdCreador = new SelectList(db.UserProfiles.Where(u => u.UserName == User.Identity.Name), "UserId", "UserName");
            ViewBag.IdModificador = new SelectList(db.UserProfiles.Where(u => u.UserName == User.Identity.Name), "UserId", "UserName");
            ViewBag.IdPlantilla = new SelectList(db.PlantillaProyectos.OrderBy(p => p.Nombre), "Id", "Nombre");
            ViewBag.Now = DateTime.Now.ToShortDateString();
            ViewBag.Version = 0;
            return View();
        }

        //
        // POST: /Modelo/Proyecto/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Proyecto proyecto, int IdPlantilla = 0)
        {
            proyecto.Creacion = DateTime.Now;
            if (ModelState.IsValid)
            {
                proyecto.Creacion = DateTime.Now;
                proyecto.Calculado = DateTime.Now;
                db.ProyectosRequester.AddElement(proyecto);

                if (IdPlantilla > 0)
                {

                    //
                    // Copiar operaciones
                    var operaciones = db.PlantillaOperaciones.Where(p => p.IdPlantillaProyecto == IdPlantilla).ToList();

                    foreach (PlantillaOperacion plantilla in operaciones)
                    {
                        db.OperacionesRequester.AddElement(new Operacion(plantilla, proyecto.Id));
                    }

                    //
                    // Copiar salidas y su asociación con operaciones
                    var salidas = db.PlantillaSalidaProyectos.Include("Operaciones").Where(p => p.IdPlantillaProyecto == IdPlantilla).ToList();

                    foreach (PlantillaSalidaProyecto plantilla in salidas)
                    {
                        int idSalida = db.SalidaProyectosRequester.AddElement(new SalidaProyecto(plantilla, proyecto.Id));

                        foreach (PlantillaSalidaOperacion cruce in plantilla.Operaciones)
                        {
                            int idOperacion = db.Operaciones.First(o => o.IdProyecto == proyecto.Id && o.Referencia == cruce.Operacion.Referencia).Id;
                            db.SalidaOperacionesRequester.AddElement(new SalidaOperacion { IdSalida = idSalida, IdOperacion = idOperacion, Secuencia = cruce.Secuencia });
                        }
                    }
                }
                db.ColaboradoresRequester.AddElement(new Colaborador { IdProyecto = proyecto.Id, Creador = true, SoloLectura = false, IdUsuario = getUserId() });
                db.AuditsRequester.AddElement(new Audit { IdProyecto = proyecto.Id, Fecha = DateTime.Now, IdUsuario = getUserId(), Transaccion = "Crear", TipoObjeto = proyecto.GetType().ToString(), Original = proyecto.LogValues() });

                return RedirectToAction("Console", new { id = proyecto.Id });
            }

            ViewBag.IdPlantilla = new SelectList(db.PlantillaProyectos.OrderBy(p => p.Nombre), "Id", "Nombre", IdPlantilla);
            ViewBag.IdCreador = new SelectList(db.UserProfiles.Where(u => u.UserName == User.Identity.Name), "UserId", "UserName", proyecto.IdCreador);
            ViewBag.Version = 0;

            return View(proyecto);
        }

        //
        // GET: /Modelo/Proyecto/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            if (proyecto == null)
            {
                return RedirectToAction("DeniedWhale", "Error", new { Area = "" });
            }
            ViewBag.IdCreador = new SelectList(db.UserProfiles.Where(u => u.UserId == proyecto.IdCreador), "UserId", "UserName", proyecto.IdCreador);
            ViewBag.PreHorizonte = proyecto.Horizonte;
            return View(proyecto);
        }

        //
        // POST: /Modelo/Proyecto/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Proyecto proyecto, int PreHorizonte)
        {
            if (ModelState.IsValid)
            {
                db.ProyectosRequester.ModifyElement(proyecto, true, proyecto.Id, getUserId());

                if (PreHorizonte < proyecto.Horizonte)
                {
                    var parametros = db.Parametros.Include("Elemento").Where(p => p.Elemento.IdProyecto == proyecto.Id).ToList();
                    foreach (Parametro parametro in parametros)
                    {
                        Celda celda = db.Celdas.Where(c => c.IdParametro == parametro.Id).OrderByDescending(c => c.Periodo).FirstOrDefault();
                        if(celda == null) continue;
                        int deltaPeriodos =  proyecto.Horizonte - celda.Periodo;
                        decimal valor = celda.Valor;
                        for (int i = 1; i <= deltaPeriodos; i++)
                        {
                            db.CeldasRequester.AddElement(new Celda { IdParametro = celda.IdParametro, Periodo = celda.Periodo + i, Valor = celda.Valor });
                        }
                    } 
                }
                
                return RedirectToAction("Console", new { id = proyecto.Id });
            }
            ViewBag.IdCreador = new SelectList(db.UserProfiles.Where(u => u.UserId == proyecto.IdCreador), "UserId", "UserName", proyecto.IdCreador);
            return View(proyecto);
        }

        //
        // GET: /Modelo/Proyecto/Delete/5

        public ActionResult Delete(int id)
        {
            Proyecto proyecto = db.Proyectos.Find(id);
            try
            {
                var colaboradores = db.Colaboradores.Where(c => c.IdProyecto == proyecto.Id).ToList();

                foreach (Colaborador colaborador in colaboradores)
                {
                    db.ColaboradoresRequester.RemoveElementByID(colaborador.Id);
                }

                var versions = db.DbVersions.Where(v => v.IdProyecto == proyecto.Id).ToList();

                foreach (DbVersion version in versions)
                {
                    db.DbVersionsRequester.RemoveElementByID(version.Id);
                }

                var audits = db.Audits.Where(a => a.IdProyecto == proyecto.Id).ToList();

                foreach (Audit audit in audits)
                {
                    db.AuditsRequester.RemoveElementByID(audit.Id);
                }

                var salidaoperaciones = db.SalidaOperaciones.Where(sxp => sxp.Operacion.IdProyecto == proyecto.Id).ToList();

                foreach (SalidaOperacion salidaoperacion in salidaoperaciones)
                {
                    db.SalidaOperacionesRequester.RemoveElementByID(salidaoperacion.Id);
                }

                var operaciones = db.Operaciones.Where(o => o.IdProyecto == proyecto.Id).ToList();

                foreach (Operacion operacion in operaciones)
                {
                    db.OperacionesRequester.RemoveElementByID(operacion.Id);
                }

                var salidas = db.SalidaProyectos.Where(s => s.IdProyecto == proyecto.Id).ToList();

                foreach (SalidaProyecto salida in salidas)
                {
                    db.SalidaProyectosRequester.RemoveElementByID(salida.Id);
                }

                var formulas = db.Formulas.Where(f => f.Elemento.IdProyecto == proyecto.Id).ToList();

                foreach (Formula formula in formulas)
                {
                    db.FormulasRequester.RemoveElementByID(formula.Id);
                }

                var celdas = db.Celdas.Where(c => c.Parametro.Elemento.IdProyecto == proyecto.Id).ToList();

                foreach (Celda celda in celdas)
                {
                    db.CeldasRequester.RemoveElementByID(celda.Id);
                }

                var parametros = db.Parametros.Where(p => p.Elemento.IdProyecto == proyecto.Id).ToList();

                foreach (Parametro parametro in parametros)
                {
                    db.ParametrosRequester.RemoveElementByID(parametro.Id);
                }

                var elementos = db.Elementos.Where(e => e.IdProyecto == proyecto.Id).ToList();

                foreach (Elemento elemento in elementos)
                {
                    db.ElementosRequester.RemoveElementByID(elemento.Id);
                }

                db.ProyectosRequester.RemoveElementByID(id);
            }
            catch (Exception)
            {
                ModelState.AddModelError("Nombre", "No se puede eliminar porque existen registros dependientes.");
                return RedirectToAction("Console", new { id = proyecto.Id });
            }

            return RedirectToAction("Index");
        }
    }
}
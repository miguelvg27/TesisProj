using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Simulaciones.Models;
using TesisProj.Models;
using TesisProj.Models.Storage;
namespace TesisProj.Areas.Simulaciones.Controllers
{
    public class ParametrosSenciblesController : Controller
    {
        //
        // GET: /Simulaciones/ParametrosSencibles/

        public ActionResult Index(int idProyecto)
        {
            using (TProjContext context = new TProjContext())
            {
                ViewBag.ProyectoId = idProyecto;
                var elementos = context.Elementos.Include("Parametros").Where(e => e.IdProyecto == idProyecto);
                List<Elemento> salidaElementos = new List<Elemento>();
                bool conf = false;
                foreach (Elemento e in elementos)
                {
                    conf = false;
                    List<Parametro> salidaParametros = new List<Parametro>();
                    foreach (Parametro p in e.Parametros)
                    {
                        if (p.Sensible)
                        {
                            conf = true;
                            Parametro aux = new Parametro();
                            aux = p;
                            salidaParametros.Add(aux);
                        }
                    }
                    if (conf)
                    {
                        Elemento elemento = new Elemento();
                        elemento = e;
                        elemento.Parametros = new List<Parametro>();
                        elemento.Parametros.AddRange(salidaParametros);
                        salidaElementos.Add(elemento);
                        conf = false;
                    }
                }
                return View(salidaElementos);
            }
        }

    }
}

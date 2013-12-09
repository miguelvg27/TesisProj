using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TesisProj.Areas.IridiumTest.Models;
using TesisProj.Areas.IridiumTest.Models.Discrete;
using TesisProj.Areas.Simulaciones.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Simulaciones.Controllers
{
    public class HiperGeometricaController : Controller
    {
        //
        // GET: /HiperGeometrica/

        [HttpGet]
        public ActionResult Index(int ProyectoId, int ParametroId)
        {
            TProjContext db = new TProjContext();
            List<ListField> lista = db.ListFields.Where(p => p.Modelo == "HiperGeometrica").ToList();
            ModeloSimulacion modelo = new ModeloSimulacion("HiperGeometrica", lista);
            Session["_GraficoProbabilidad"] = null;
            Session["_GraficoMuestra"] = null;
            Session["ParametroId"] = ParametroId;
            Session["ProyectoId"] = ProyectoId;
            ViewBag.ParametroId = ParametroId;
            ViewBag.ProyectoId = ProyectoId;
            return View(modelo.hipergeometrica);
        }

        [HttpPost]
        public ActionResult Index(_HiperGeometrica h)
        {
            //g.ParamsIN[0].valorD  N
            //g.ParamsIN[1].valorD  k
            //g.ParamsIN[0].valorD  n
            //g.ParamsIN[1].valorD  muuestras
            ModeloSimulacion modelo = new ModeloSimulacion("HiperGeometrica", h.ParamsIN[0].valorI, h.ParamsIN[1].valorI, h.ParamsIN[2].valorI, 0);
            modelo.hipergeometrica.GetModelo();
            modelo.hipergeometrica.GetSimulacion(h.ParamsIN[3].valorI);
            modelo.hipergeometrica.GetResumen();

            Session["_GraficoProbabilidad"] = modelo.hipergeometrica.Graphics;
            Session["_GraficoMuestra"] = modelo.hipergeometrica.Results;

            ViewBag.ParametroId = (int)Session["ParametroId"];
            ViewBag.ProyectoId = (int)Session["ProyectoId"];

            for (int i = 0; i < modelo.hipergeometrica.ParamsIN.Count; i++)
            {
                try
                {
                    modelo.hipergeometrica.ParamsIN[i].valorD = h.ParamsIN[i].valorD;
                    modelo.hipergeometrica.ParamsIN[i].valorI = h.ParamsIN[i].valorI;
                }
                catch
                {
                    break;
                }
            }

            Asignar((int)Session["ProyectoId"], (int)Session["ParametroId"], "HiperGeometrica", h.ParamsIN[0].valorI, h.ParamsIN[1].valorI, h.ParamsIN[2].valorI, 0, modelo.hipergeometrica.ParamsIN);
            return View(modelo.hipergeometrica);
        }

        public void Asignar(int ProyectoId, int ParametroId, string Nombre, double a, double b, double c, double d, List<Param> parametros)
        {
            string cadena = "";
            using (TProjContext context = new TProjContext())
            {
                var parametro = context.Parametros.Include("Elemento").Include("Celdas").Where(e => e.Id == ParametroId).Where(oo => oo.Sensible == true).FirstOrDefault();
                parametro.XML_ModeloAsignado = Nombre + "|" + a + "|" + b + "|" + c + "|" + d + "|";
                foreach (Param p in parametros)
                {
                    cadena += "°" + p.indice + "?" + p.nombre + "?" + p.rango + "?" + p.valorD + "?" + p.valorI;
                }
                parametro.XML_ModeloAsignado += cadena;

                context.ParametrosRequester.ModifyElement(parametro, true, ProyectoId, context.UserProfiles.First(u => u.UserName == User.Identity.Name).UserId);
            }
        }

        [ChildActionOnly]
        public ActionResult _GraficoProbabilidad()
        {
            return PartialView((List<Graphic>)Session["_GraficoProbabilidad"]);
        }

        [ChildActionOnly]
        public ActionResult _GraficoMuestra()
        {
            return PartialView((List<Result>)Session["_GraficoMuestra"]);
        }

        [ChildActionOnly]
        public ActionResult _Resumen()
        {
            return PartialView((List<Result>)Session["_GraficoMuestra"]);
        }

        public FileContentResult getImg(int Id)
        {
            TProjContext db = new TProjContext();
            List<ListField> lista = db.ListFields.Where(p => p.Modelo == "HiperGeometrica").ToList();
            ModeloSimulacion m = new ModeloSimulacion("HiperGeometrica", lista);
            byte[] byteArray;
            if (Id == -1)
                byteArray = m.hipergeometrica.Resumen;
            else
                byteArray = m.hipergeometrica.Formulates[Id].Imagen;

            if (byteArray != null)
            {
                return new FileContentResult(byteArray, "image/jpeg");
            }
            else
            {
                return null;
            }
        }
    }
}
using TesisProj.Areas.Simulaciones.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using TesisProj.Models.Storage;
using TesisProj.Areas.IridiumTest.Models;
using TesisProj.Areas.IridiumTest.Models.Continuous;

namespace TesisProj.Areas.Simulaciones.Controllers
{
    public class ChiCuadradoController : Controller
    {
        //
        // GET: /ChiCuadrado/

        [HttpGet]
        public ActionResult Index(int ProyectoId, int ParametroId)
        {
            TProjContext db = new TProjContext();
            List<ListField> lista = db.ListFields.Where(p => p.Modelo == "ChiCuadrado").ToList();
            ModeloSimulacion modelo = new ModeloSimulacion("ChiCuadrado", lista);
            Session["_GraficoProbabilidad"] = null;
            Session["_GraficoMuestra"] = null;
            Session["ParametroId"] = ParametroId;
            Session["ProyectoId"] = ProyectoId;
            ViewBag.ParametroId = ParametroId;
            ViewBag.ProyectoId = ProyectoId;
            return View(modelo.chicuadrado);
        }

        [HttpPost]
        public ActionResult Index(_ChiCuadrado c)
        {
            //u.ParamsIN[0].valorI  freedn
            //u.ParamsIN[1].valorI  MUESTRA
            TProjContext db = new TProjContext();
            List<ListField> lista = db.ListFields.Where(p => p.Modelo == "ChiCuadrado").ToList();
            ModeloSimulacion modelo = new ModeloSimulacion("ChiCuadrado", c.ParamsIN[0].valorD,0, 0, 0,lista);
            modelo.chicuadrado.GetModelo();
            modelo.chicuadrado.GetSimulacion(c.ParamsIN[1].valorI);
            modelo.chicuadrado.GetResumen();
            
            Session["_GraficoProbabilidad"] = modelo.chicuadrado.Graphics;
            Session["_GraficoMuestra"] = modelo.chicuadrado.Results;

            for (int i = 0; i < modelo.chicuadrado.ParamsIN.Count; i++)
            {
                try
                {
                    modelo.chicuadrado.ParamsIN[i].valorD = c.ParamsIN[i].valorD;
                    modelo.chicuadrado.ParamsIN[i].valorI = c.ParamsIN[i].valorI;
                }
                catch
                {
                    break;
                }
            }
            ViewBag.ParametroId = (int)Session["ParametroId"];
            ViewBag.ProyectoId = (int)Session["ProyectoId"];
            Asignar((int)Session["ProyectoId"], (int)Session["ParametroId"], "ChiCuadrado", c.ParamsIN[0].valorD, 0, 0, 0, modelo.chicuadrado.ParamsIN);
            return View(modelo.chicuadrado);
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
            List<ListField> lista = db.ListFields.Where(p => p.Modelo == "ChiCuadrado").ToList();
            ModeloSimulacion m = new ModeloSimulacion("ChiCuadrado", lista);
            byte[] byteArray;
            if (Id == -1)
                byteArray = m.chicuadrado.Resumen;
            else
                byteArray = m.chicuadrado.Formulates[Id].Imagen;
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

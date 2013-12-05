using IridiumTest.Models;
using IridiumTest.Models.Discrete;
using TesisProj.Areas.Simulaciones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Simulaciones.Controllers
{
    public class GeometricaController : Controller
    {
        //
        // GET: /Geometrica/

        [HttpGet]
        public ActionResult Index(int ProyectoId, int ParametroId)
        {
            ModeloSimulacion modelo = new ModeloSimulacion("Geometrica");
            Session["_GraficoProbabilidad"] = null;
            Session["_GraficoMuestra"] = null;
            Session["ParametroId"] = ParametroId;
            Session["ProyectoId"] = ProyectoId;
            ViewBag.ParametroId = ParametroId;
            ViewBag.ProyectoId = ProyectoId;
            return View(modelo.geometrica);
        }

        [HttpPost]
        public ActionResult Index(_Geometrica g)
        {
            //g.ParamsIN[0].valorD  probabilidad de exito
            //g.ParamsIN[1].valorD  muuestras
            ModeloSimulacion modelo = new ModeloSimulacion("Geometrica", g.ParamsIN[0].valorD, 0, 0, 0);
            modelo.geometrica.GetModelo();
            modelo.geometrica.GetSimulacion(g.ParamsIN[1].valorI);
            modelo.geometrica.GetResumen();

            Session["_GraficoProbabilidad"] = modelo.geometrica.Graphics;
            Session["_GraficoMuestra"] = modelo.geometrica.Results;

            ViewBag.ParametroId = (int)Session["ParametroId"];
            ViewBag.ProyectoId = (int)Session["ProyectoId"];

            for (int i = 0; i < modelo.geometrica.ParamsIN.Count; i++)
            {
                try
                {
                    modelo.geometrica.ParamsIN[i].valorD = g.ParamsIN[i].valorD;
                    modelo.geometrica.ParamsIN[i].valorI = g.ParamsIN[i].valorI;
                }
                catch
                {
                    break;
                }
            }

            Asignar((int)Session["ProyectoId"], (int)Session["ParametroId"], "Geometrica", g.ParamsIN[0].valorD, 0, 0, 0,modelo.geometrica.ParamsIN);
            return View(modelo.geometrica);
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
            ModeloSimulacion m = new ModeloSimulacion("Geometrica");
            byte[] byteArray;
            if (Id == -1)
                byteArray = m.geometrica.Resumen;
            else
                byteArray = m.geometrica.Formulates[Id].Imagen;
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

using TesisProj.Areas.Simulaciones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Models.Storage;
using TesisProj.Areas.IridiumTest.Models;
using TesisProj.Areas.IridiumTest.Models.Discrete;

namespace TesisProj.Areas.Simulaciones.Controllers
{
    public class BinomialController : Controller
    {
        //
        // GET: /Binomial/

        [HttpGet]
        public ActionResult Index(int ProyectoId, int ParametroId)
        {
            TProjContext db = new TProjContext();
            List<ListField> lista = db.ListFields.Where(p => p.Modelo == "Binomial").ToList();
            ModeloSimulacion modelo = new ModeloSimulacion("Binomial", lista);
            Session["_GraficoProbabilidad"] = null;
            Session["_GraficoMuestra"] = null;
            Session["ParametroId"] = ParametroId;
            Session["ProyectoId"] = ProyectoId;
            ViewBag.ParametroId = ParametroId;
            ViewBag.ProyectoId = ProyectoId;
            return View(modelo.binomial);
        }

        [HttpPost]
        public ActionResult Index(_Binomial b)
        {
            ModeloSimulacion modelo = new ModeloSimulacion("Binomial",b.ParamsIN[0].valorD,b.ParamsIN[1].valorI,0,0);
            modelo.binomial.GetModelo();
            modelo.binomial.GetSimulacion(b.ParamsIN[2].valorI);
            modelo.binomial.GetResumen();

            Session["_GraficoProbabilidad"] = modelo.binomial.Graphics;
            Session["_GraficoMuestra"] = modelo.binomial.Results;
            for (int i = 0; i < modelo.binomial.ParamsIN.Count; i++)
            {
                try
                {
                    modelo.binomial.ParamsIN[i].valorD = b.ParamsIN[i].valorD;
                    modelo.binomial.ParamsIN[i].valorI = b.ParamsIN[i].valorI;
                }
                catch
                {
                    break;
                }
            }

            ViewBag.ParametroId = (int)Session["ParametroId"];
            ViewBag.ProyectoId = (int)Session["ProyectoId"];
            Asignar((int)Session["ProyectoId"], (int)Session["ParametroId"], "Binomial", b.ParamsIN[0].valorD, b.ParamsIN[1].valorI, 0, 0, modelo.binomial.ParamsIN);
            return View(modelo.binomial);
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
            List<ListField> lista = db.ListFields.Where(p => p.Modelo == "Binomial").ToList();
            ModeloSimulacion m = new ModeloSimulacion("Binomial", lista);
            byte[] byteArray;
            if (Id == -1)
                byteArray = m.binomial.Resumen;
            else
                byteArray = m.binomial.Formulates[Id].Imagen;

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

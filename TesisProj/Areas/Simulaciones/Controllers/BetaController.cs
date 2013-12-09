using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using TesisProj.Areas.Simulaciones.Models;
using TesisProj.Models.Storage;
using TesisProj.Areas.Modelo.Models;
using System;
using TesisProj.Areas.IridiumTest.Models.Continuous;
using TesisProj.Areas.IridiumTest.Models;

namespace TesisProj.Areas.Simulaciones.Controllers
{
    public class BetaController : Controller
    {
        //
        // GET: /Beta/

        [HttpGet]
        public ActionResult Index(int ProyectoId, int ParametroId)
        {
            TProjContext db = new TProjContext();
            List<ListField> lista = db.ListFields.Where(p=>p.Modelo=="Beta").ToList();
            ModeloSimulacion modelo = new ModeloSimulacion("Beta",lista );
            Session["_GraficoProbabilidad"] = null;
            Session["_GraficoMuestra"] = null;
            Session["ParametroId"] = ParametroId;
            Session["ProyectoId"] = ProyectoId;
            ViewBag.ParametroId = ParametroId;
            ViewBag.ProyectoId = ProyectoId;
            return View(modelo.beta);
        }

        [HttpPost]
        public ActionResult Index(_Beta b)
        {
            //u.ParamsIN[0].valorI  MINIMO
            //u.ParamsIN[1].valorI  MAXIMO
            //u.ParamsIN[2].valorI  MUESTRA

            TProjContext db = new TProjContext();
            List<ListField> lista = db.ListFields.Where(p => p.Modelo == "Beta").ToList();
            ModeloSimulacion modelo = new ModeloSimulacion("Beta", b.ParamsIN[0].valorD, b.ParamsIN[1].valorD, 0, 0,lista);
            modelo.beta.GetModelo();
            modelo.beta.GetSimulacion(b.ParamsIN[2].valorI);
            modelo.beta.GetResumen();

            Session["_GraficoProbabilidad"] = modelo.beta.Graphics;
            Session["_GraficoMuestra"] = modelo.beta.Results;

            ViewBag.ParametroId = (int)Session["ParametroId"];
            ViewBag.ProyectoId = (int)Session["ProyectoId"];

            for (int i = 0; i < modelo.beta.ParamsIN.Count; i++)
            {
                try
                {
                    modelo.beta.ParamsIN[i].valorD = b.ParamsIN[i].valorD;
                    modelo.beta.ParamsIN[i].valorI = b.ParamsIN[i].valorI;
                }
                catch
                {
                    break;
                }
            }

            Asignar((int)Session["ProyectoId"], (int)Session["ParametroId"], "Beta", b.ParamsIN[0].valorD, b.ParamsIN[1].valorD, 0, 0, modelo.beta.ParamsIN);
            return View(modelo.beta);
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

        public ActionResult Asignar(int ProyectoId,int ParametroId)
        {
            ModeloSimulacion modelo = (ModeloSimulacion)Session["Modelo"];
            using (TProjContext context = new TProjContext())
            {
                var parametro = context.Parametros.Include("Elemento").Include("Celdas").Where(e => e.Id==ParametroId).Where(oo => oo.Sensible == true).FirstOrDefault();
                parametro.XML_ModeloAsignado = "Beta" + "|" + modelo.beta.ParamsIN[0].valorD + "|" + modelo.beta.ParamsIN[1].valorD + "|" + "0" + "|" + "0";
                context.ParametrosRequester.ModifyElement(parametro, true, ProyectoId, context.UserProfiles.First(u => u.UserName == User.Identity.Name).UserId);
            }

            return RedirectToAction("Index", "Simulaciones/ParametrosSencibles?idProyecto="+ProyectoId);
            
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
            List<ListField> lista = db.ListFields.Where(p => p.Modelo == "Beta").ToList();
            ModeloSimulacion m = new ModeloSimulacion("Beta", lista);
            byte[] byteArray;
            if (Id == -1)
                byteArray = m.beta.Resumen;
            else
                byteArray = m.beta.Formulates[Id].Imagen;
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
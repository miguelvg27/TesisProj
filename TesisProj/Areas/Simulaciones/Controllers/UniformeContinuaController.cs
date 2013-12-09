using TesisProj.Areas.Simulaciones.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using TesisProj.Models.Storage;
using TesisProj.Areas.IridiumTest.Models.Continuous;
using TesisProj.Areas.IridiumTest.Models;

namespace TesisProj.Areas.Simulaciones.Controllers
{
    public class UniformeContinuaController : Controller
    {
        //
        // GET: /UniformeContinua/

        [HttpGet]
        public ActionResult Index(int ProyectoId, int ParametroId)
        {
            TProjContext db = new TProjContext();
            List<ListField> lista = db.ListFields.Where(p => p.Modelo == "UniformeContinua").ToList();
            ModeloSimulacion modelo = new ModeloSimulacion("UniformeContinua", lista);

            Session["_GraficoProbabilidad"] = null;
            Session["_GraficoMuestra"] = null;
            Session["ParametroId"] = ParametroId;
            Session["ProyectoId"] = ProyectoId;
            ViewBag.ParametroId = ParametroId;
            ViewBag.ProyectoId = ProyectoId;
            return View(modelo.uniformecontinua);
        }

        [HttpPost]
        public ActionResult Index(_UniformeContinua u)
        {
            //u.ParamsIN[0].valorI  MINIMO
            //u.ParamsIN[1].valorI  MAXIMO
            //u.ParamsIN[2].valorI  MUESTRA
            TProjContext db = new TProjContext();
            List<ListField> lista = db.ListFields.Where(p => p.Modelo == "UniformeContinua").ToList();
            ModeloSimulacion modelo = new ModeloSimulacion("UniformeContinua", u.ParamsIN[0].valorD, u.ParamsIN[1].valorD, 0, 0,lista);
            modelo.uniformecontinua.GetModelo();
            modelo.uniformecontinua.GetSimulacion(u.ParamsIN[2].valorI);
            modelo.uniformecontinua.GetResumen();

            Session["_GraficoProbabilidad"] = modelo.uniformecontinua.Graphics;
            Session["_GraficoMuestra"] = modelo.uniformecontinua.Results;

            ViewBag.ParametroId = (int)Session["ParametroId"];
            ViewBag.ProyectoId = (int)Session["ProyectoId"];

            for (int i = 0; i < modelo.uniformecontinua.ParamsIN.Count; i++)
            {
                try
                {
                    modelo.uniformecontinua.ParamsIN[i].valorD = u.ParamsIN[i].valorD;
                    modelo.uniformecontinua.ParamsIN[i].valorI = u.ParamsIN[i].valorI;
                }
                catch
                {
                    break;
                }
            }

            Asignar((int)Session["ProyectoId"], (int)Session["ParametroId"], "UniformeContinua", u.ParamsIN[0].valorD, u.ParamsIN[1].valorD, 0, 0, modelo.uniformecontinua. ParamsIN);
            return View(modelo.uniformecontinua);
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
            List<ListField> lista = db.ListFields.Where(p => p.Modelo == "UniformeContinua").ToList();
            ModeloSimulacion m = new ModeloSimulacion("UniformeContinua", lista);

            byte[] byteArray;
            if (Id == -1)
                byteArray = m.uniformecontinua.Resumen;
            else
                byteArray = m.uniformecontinua.Formulates[Id].Imagen;
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
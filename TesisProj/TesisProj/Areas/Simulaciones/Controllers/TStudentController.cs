using IridiumTest.Models;
using IridiumTest.Models.Continuous;
using TesisProj.Areas.Simulaciones.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Simulaciones.Controllers
{
    public class TStudentController : Controller
    {
        //
        // GET: /Tstudent/

        [HttpGet]
        public ActionResult Index(int ProyectoId, int ParametroId)
        {
            ModeloSimulacion modelo = new ModeloSimulacion("TStudent");
            Session["_GraficoProbabilidad"] = null;
            Session["_GraficoMuestra"] = null;
            Session["ParametroId"] = ParametroId;
            Session["ProyectoId"] = ProyectoId;
            ViewBag.ParametroId = ParametroId;
            ViewBag.ProyectoId = ProyectoId;
            return View(modelo.tstudent);
        }

        [HttpPost]
        public ActionResult Index(_TStudent u)
        {
            //u.ParamsIN[0].valorI  MINIMO
            //u.ParamsIN[1].valorI  MAXIMO
            //u.ParamsIN[2].valorI  MUESTRA0
            ModeloSimulacion modelo = new ModeloSimulacion("TStudent", u.ParamsIN[0].valorD, u.ParamsIN[1].valorD, u.ParamsIN[2].valorD, 0);
            modelo.tstudent.GetModelo();
            modelo.tstudent.GetSimulacion(u.ParamsIN[3].valorI);
            modelo.tstudent.GetResumen();

            Session["_GraficoProbabilidad"] = modelo.tstudent.Graphics;
            Session["_GraficoMuestra"] = modelo.tstudent.Results;

            ViewBag.ParametroId = (int)Session["ParametroId"];
            ViewBag.ProyectoId = (int)Session["ProyectoId"];

            for (int i = 0; i < modelo.tstudent.ParamsIN.Count; i++)
            {
                try
                {
                    modelo.tstudent.ParamsIN[i].valorD = u.ParamsIN[i].valorD;
                    modelo.tstudent.ParamsIN[i].valorI = u.ParamsIN[i].valorI;
                }
                catch
                {
                    break;
                }
            }

            Asignar((int)Session["ProyectoId"], (int)Session["ParametroId"], "TStudent", u.ParamsIN[0].valorD, u.ParamsIN[1].valorD, u.ParamsIN[2].valorD, 0, modelo.tstudent.ParamsIN);
            return View(modelo.tstudent);
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
            ModeloSimulacion m = new ModeloSimulacion("TStudent");
            byte[] byteArray;
            if (Id == -1)
                byteArray = m.tstudent.Resumen;
            else
                byteArray = m.tstudent.Formulates[Id].Imagen;
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

using IridiumTest.Models;
using IridiumTest.Models.Continuous;
using TesisProj.Areas.Simulaciones.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace TesisProj.Areas.Simulaciones.Controllers
{
    public class GammaController : Controller
    {
        //
        // GET: /Gamma/


        [HttpGet]
        public ActionResult Index(int ProyectoId,int ParametroId)
        {
            ModeloSimulacion modelo = new ModeloSimulacion("Gamma");
            Session["_GraficoProbabilidad"] = null;
            Session["_GraficoMuestra"] = null;
            ViewBag.ParametroId = ParametroId;
            ViewBag.ProyectoId = ProyectoId;
            return View(modelo.gamma);
        }

        [HttpPost]
        public ActionResult Index(_Gamma u)
        {
            //u.ParamsIN[0].valorI  MINIMO
            //u.ParamsIN[1].valorI  MAXIMO
            //u.ParamsIN[2].valorI  MUESTRA
            ModeloSimulacion modelo = new ModeloSimulacion("Gamma", u.ParamsIN[0].valorD, 0, 0, 0);
            modelo.gamma.GetModelo();
            modelo.gamma.GetSimulacion(u.ParamsIN[2].valorI);
            modelo.gamma.GetResumen();
            Session["_GraficoProbabilidad"] = modelo.gamma.Graphics;
            Session["_GraficoMuestra"] = modelo.gamma.Results;
            return View(modelo.gamma);
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
            ModeloSimulacion m = new ModeloSimulacion("Gamma");
            byte[] byteArray;
            if (Id == -1)
                byteArray = m.gamma.Resumen;
            else
                byteArray = m.gamma.Formulates[Id].Imagen;
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

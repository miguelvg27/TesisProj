using TesisProj.Areas.Simulaciones.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using TesisProj.Models.Storage;
using TesisProj.Areas.IridiumTest.Models;
using TesisProj.Areas.IridiumTest.Models.Continuous;
using System;

namespace TesisProj.Areas.Simulaciones.Controllers
{
    public class NormalController : Controller
    {
        //
        // GET: /Normal/

        [HttpGet]
        public ActionResult Index(int ProyectoId, int ParametroId)
        {
            TProjContext db = new TProjContext();
            List<ListField> lista = db.ListFields.Where(p => p.Modelo == "Normal").ToList();
            ModeloSimulacion modelo = new ModeloSimulacion("Normal", lista);
            Session["_GraficoProbabilidad"] = null;
            Session["_GraficoMuestra"] = null;
            Session["Opcion"] = 2;
            Session["ParametroId"] = ParametroId;
            Session["ProyectoId"] = ProyectoId;
            ViewBag.ParametroId = ParametroId;
            ViewBag.ProyectoId = ProyectoId;
            return View(modelo.normal);
        }

        [HttpPost]
        public ActionResult Index(_Normal u)
        {
            //u.ParamsIN[0].valorI  MINIMO
            //u.ParamsIN[1].valorI  MAXIMO
            //u.ParamsIN[2].valorI  MUESTRA
            TProjContext db = new TProjContext();
            List<ListField> lista = db.ListFields.Where(p => p.Modelo == "Normal").ToList();
            ModeloSimulacion modelo = new ModeloSimulacion("Normal", u.ParamsIN[0].valorD, u.ParamsIN[1].valorD, 0, 0, lista);
            modelo.normal.GetModelo();
            modelo.normal.GetSimulacion(u.ParamsIN[2].valorI);
            modelo.normal.GetResumen();

            Session["_GraficoProbabilidad"] = modelo.normal.Graphics;
            Session["_GraficoMuestra"] = modelo.normal.Results;

            ViewBag.ParametroId = (int)Session["ParametroId"];
            ViewBag.ProyectoId = (int)Session["ProyectoId"];

            for (int i = 0; i < modelo.normal.ParamsIN.Count; i++)
            {
                try
                {
                    modelo.normal.ParamsIN[i].valorD = u.ParamsIN[i].valorD;
                    modelo.normal.ParamsIN[i].valorI = u.ParamsIN[i].valorI;
                }
                catch
                {
                    break;
                }
            }

            Asignar((int)Session["ProyectoId"], (int)Session["ParametroId"], "Normal", u.ParamsIN[0].valorD, u.ParamsIN[1].valorD, 0, 0, modelo.normal.ParamsIN);
            return View(modelo.normal);
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
                cadena = cadena.Replace("-999999999999", "0");
                parametro.XML_ModeloAsignado += cadena;
                context.ParametrosRequester.ModifyElement(parametro, true, ProyectoId, context.UserProfiles.First(u => u.UserName == User.Identity.Name).UserId);
            }
        }
        
        [HttpGet]
        public ActionResult Index2(int ProyectoId, int ParametroId)
        {
            TProjContext db = new TProjContext();
            List<ListField> lista = db.ListFields.Where(p => p.Modelo == "Normal").ToList();
            ModeloSimulacion modelo = new ModeloSimulacion("Normal", lista);
            Session["Opcion"] = 2;
            Session["ParametroId"] = ParametroId;
            Session["ProyectoId"] = ProyectoId;
            ViewBag.ParametroId = ParametroId;
            ViewBag.ProyectoId = ProyectoId;
            if (Session["Index2"] == null)
            {
                Session["_GraficoProbabilidad"] = null;
                Session["_GraficoMuestra"] = null;
                return View(modelo.normal);
            }
            else
            {
                _Normal ss = (_Normal)Session["Index2"];
                Session["Index2"] = null;
                return View((ss));
            }
        }

        [HttpPost]
        public ActionResult Index2(string precision)
        {
            string[] valores1 = precision.Split('*');
            double media = Convert.ToDouble(valores1[0]);
            int muestras = Convert.ToInt32(valores1[1]);
            string[] Intervalos = valores1[2].Split('='); //27|28|.30
            double acumulaPromedio = 0;
            double acumulaFrecuecia = 0;
            string almacenar = ""; ;
            foreach (string s in Intervalos)
            {
                double min = Convert.ToDouble(s.Split('|')[0]);
                double max = Convert.ToDouble(s.Split('|')[1]);
                double fre = Convert.ToDouble(s.Split('|')[2]);
                almacenar += "[" + min.ToString() + ";" + max.ToString() + "]  -  " + fre.ToString() + ";";
                double promedio = (min + max)/2.0;
                acumulaPromedio += promedio * fre;
                acumulaFrecuecia += fre;
            }
            double xi = acumulaPromedio / acumulaFrecuecia;
            int contadorN = 0;
            double Sumatoria = 0;
            foreach (string s in Intervalos)
            {
                double min = Convert.ToDouble(s.Split('|')[0]);
                double max = Convert.ToDouble(s.Split('|')[1]);
                double promedio = (min + max) / 2.0;
                contadorN++;
                Sumatoria+=Math.Pow(xi - promedio, 2);
            }
            double DS = Math.Sqrt(Sumatoria / contadorN);
            TProjContext db = new TProjContext();
            List<ListField> lista = db.ListFields.Where(p => p.Modelo == "Normal").ToList();
            ModeloSimulacion modelo = new ModeloSimulacion("Normal", Math.Round(xi,2), DS, 0, 0,lista);
            modelo.normal.GetModelo();
            modelo.normal.GetSimulacion(muestras);
            modelo.normal.GetResumen();
            
            Session["_GraficoProbabilidad"] = modelo.normal.Graphics;
            Session["_GraficoMuestra"] = modelo.normal.Results;

            ViewBag.ParametroId = (int)Session["ParametroId"];
            ViewBag.ProyectoId = (int)Session["ProyectoId"];

            modelo.normal.ParamsIN[0].valorD = Math.Round(xi, 2);
            modelo.normal.ParamsIN[1].valorD = Math.Round(DS,2);
            Asignar2((int)Session["ProyectoId"], (int)Session["ParametroId"], "Normal", xi.ToString(), DS.ToString(), "0", "0", modelo.normal.ParamsIN,almacenar);

            Session["Index2"] = modelo.normal;
            //return RedirectToAction("Index2", new { ProyectoId = (int)Session["ParametroId"], ParametroId=(int)Session["ProyectoId"]});
            return View(modelo.normal);
        }
        public void Asignar2(int ProyectoId, int ParametroId, string Nombre, string a, string b, string c, string d, List<Param> parametros,string estimacion)
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
                cadena = cadena.Replace("-999999999999", estimacion);
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
            List<ListField> lista = db.ListFields.Where(p => p.Modelo == "Normal").ToList();
            ModeloSimulacion m = new ModeloSimulacion("Normal", lista);
            byte[] byteArray;
            if (Id == -1)
                byteArray = m.normal.Resumen;
            else
                byteArray = m.normal.Formulates[Id].Imagen;
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
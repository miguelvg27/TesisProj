using IridiumTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Simulaciones.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Simulaciones.Controllers
{
    public class ModeloSimulacionController : Controller
    {
        //
        // GET: /ModeloSimulacion/

        public ActionResult Index(int ProyectoId, int ElementoId,int ParametroId)
        {
            ModeloSimulacion modelo = new ModeloSimulacion();
            ViewBag.ProyectoId = ProyectoId;
            ViewBag.ElementoId = ElementoId;
            ViewBag.ParametroId = ParametroId;
            
            
            using (TProjContext context = new TProjContext())
            {
                var proyecto = context.Proyectos.Where(o => o.Id == ProyectoId).FirstOrDefault();
                var elemento = context.Elementos.Where(o => o.Id == ElementoId ).FirstOrDefault();
                var parametro = context.Parametros.Where(o => o.Id == ParametroId).FirstOrDefault();
                ViewBag.Titulo = "Proyecto: " + proyecto.Nombre + "    Elemento: " + elemento.Nombre + "    Parametro: " + parametro.Nombre;
                string CronogramaTitulo= "";
                string CronogramaValor = "";

                foreach (Celda c in parametro.Celdas)
                {
                     CronogramaTitulo+= c.Periodo+"|";
                }

                CronogramaTitulo = CronogramaTitulo.Substring(0, CronogramaTitulo.Length - 1);
                ViewBag.CronogramaTitulo = CronogramaTitulo.Split('|');

                foreach (Celda c in parametro.Celdas)
                {
                    CronogramaValor += Math.Round(c.Valor,2) + "|"; ;
                }

                CronogramaValor = CronogramaValor.Substring(0, CronogramaValor.Length - 1);
                ViewBag.CronogramaValor = CronogramaValor.Split('|');

                if (parametro.XML_ModeloAsignado != null)
                {
                    ViewBag.SubTitulo = "Modelo Asignado: " + parametro.XML_ModeloAsignado.Split('°')[0].Split('|')[0];
                    string nombre = parametro.XML_ModeloAsignado.Split('°')[0].Split('|')[0];
                    double a = Convert.ToDouble(parametro.XML_ModeloAsignado.Split('°')[0].Split('|')[1]);
                    double b = Convert.ToDouble(parametro.XML_ModeloAsignado.Split('°')[0].Split('|')[2]);
                    double c = Convert.ToDouble(parametro.XML_ModeloAsignado.Split('°')[0].Split('|')[3]);
                    double d = Convert.ToDouble(parametro.XML_ModeloAsignado.Split('°')[0].Split('|')[4]);
                    string[] todo = parametro.XML_ModeloAsignado.Split('°');
                    
                    List<Param> parametrosalmacenados = new List<Param>();

                    for(int i=1; i<todo.Count();i++)
                    {
                        string[] p = todo[i].Split('?');
                        parametrosalmacenados.Add(new Param { indice = Convert.ToInt16(p[0]), nombre = p[1], rango = p[2], valorD = Math.Round(Convert.ToDouble(p[3]),2), valorI = Convert.ToInt32(p[4]) });
                    }

                    ModeloSimulacion modelo2 = new ModeloSimulacion(nombre,a,b,c,d);
                    String simulaciones = CadenaSimulaciones(modelo2, nombre, proyecto.Horizonte);
                    string tabla = CadenaModelo(modelo2, nombre, parametrosalmacenados);
                    
                    ViewBag.Cabeceras = tabla.Split('°')[0].Split('|');
                    ViewBag.Valores = tabla.Split('°')[1].Split('|');
                    simulaciones = simulaciones.Substring(0, simulaciones.Length - 1);
                    ViewBag.ValoresSimulados = simulaciones.Split('|');
                }
                return View(modelo);
            }
        }

        private string CadenaSimulaciones(ModeloSimulacion m, string nombre, int horizonte)
        {
            String Simuladas = "";


            if (nombre.CompareTo("Binomial") == 0)
            {
                for (int y = 0; y < horizonte; y++)
                    Simuladas += Math.Round(m.binomial.Sample(), 2) + "|";
                return Simuladas;
            }


            if (nombre.CompareTo("Geometrica") == 0)
            {
                for (int y = 0; y < horizonte; y++)
                    Simuladas += Math.Round(Math.Abs(m.geometrica.Sample()), 2) + "|";
                return Simuladas;
            }

            if (nombre.CompareTo("HiperGeometrica") == 0)
            {
                for (int y = 0; y < horizonte; y++)
                    Simuladas += Math.Round(m.hipergeometrica.Sample(), 2) + "|";
                return Simuladas;
            }

            if (nombre.CompareTo("Poisson") == 0)
            {
                for (int y = 0; y < horizonte; y++)
                    Simuladas += Math.Round(m.poisson.Sample(), 2) + "|";
                return Simuladas;
            }

            if (nombre.CompareTo("UniformeDiscreta") == 0)
            {
                for (int y = 0; y < horizonte; y++)
                    Simuladas += Math.Round(m.uniformediscreta.Sample(), 2) + "|";
                return Simuladas;
            }

            if (nombre.CompareTo("Beta") == 0)
            {
                for (int y = 0; y < horizonte; y++)
                    Simuladas += Math.Round(m.beta.Sample(), 2) + "|";
                return Simuladas;
            }

            if (nombre.CompareTo("ChiCuadrado") == 0)
            {
                for (int y = 0; y < horizonte; y++)
                    Simuladas += Math.Round(m.chicuadrado.Sample(), 2) + "|";
                return Simuladas;
            }

            if (nombre.CompareTo("Exponencial") == 0)
            {
                for (int y = 0; y < horizonte; y++)
                    Simuladas += Math.Round(m.exponencial.Sample(), 2) + "|";
                return Simuladas;
            }

            if (nombre.CompareTo("F") == 0)
            {
                for (int y = 0; y < horizonte; y++)
                    Simuladas += Math.Round(m.f.Sample(), 2) + "|";
                return Simuladas;
            }

            if (nombre.CompareTo("Gamma") == 0)
            {
                for (int y = 0; y < horizonte; y++)
                    Simuladas += Math.Round(m.gamma.Sample(), 2) + "|";
                return Simuladas;
            }

            if (nombre.CompareTo("Normal") == 0)
            {
                for (int y = 0; y < horizonte; y++)
                    Simuladas += Math.Round(m.normal.Sample(), 2) + "|";
                return Simuladas;
            }

            if (nombre.CompareTo("Pareto") == 0)
            {
                for (int y = 0; y < horizonte; y++)
                    Simuladas += Math.Round(m.pareto.Sample(), 2) + "|";
                return Simuladas;
            }

            if (nombre.CompareTo("TStudent") == 0)
            {
                for (int y = 0; y < horizonte; y++)
                    Simuladas += Math.Round(m.tstudent.Sample(), 2) + "|";
                return Simuladas;
            }

            if (nombre.CompareTo("UniformeContinua") == 0)
            {
                for (int y = 0; y < horizonte; y++)
                    Simuladas += Math.Round(m.uniformecontinua.Sample(), 2) + "|";
                return Simuladas;
            }

            if (nombre.CompareTo("Weibull") == 0)
            {
                for (int y = 0; y < horizonte; y++)
                    Simuladas += Math.Round(m.weibull.Sample(), 2) + "|";
                return Simuladas;
            }
            return Simuladas;
        }

        private string  CadenaModelo(ModeloSimulacion m, string nombre, List<Param> parametros)
        {
            String salida = "";

            foreach (Param p in parametros)
                salida += p.nombre + " (" + p.rango + ") " + "|";

            salida = salida.Substring(0, salida.Length - 1);
            salida += "°";

            foreach (Param p in parametros)
                salida += (p.valorI == 0 ? p.valorD : p.valorI) + "|";

            salida = salida.Substring(0, salida.Length - 1);
            return salida;
        }

        public ActionResult Asignacion(string Name, int ProyectoId, int ParametroId)
        {
            ModeloSimulacion modelo = new ModeloSimulacion(Name);
            return RedirectToAction("Index", Name, new { ProyectoId = ProyectoId,ParametroId = ParametroId });
        }

        public FileContentResult getImg(string nombre)
        {
            ModeloSimulacion m = new ModeloSimulacion(nombre);
            byte[] byteArray=null;

            if (nombre.CompareTo("Binomial") == 0)
                byteArray = m.binomial.Imagen;

            if (nombre.CompareTo("Geometrica") == 0)
                byteArray = m.geometrica.Imagen;

            if (nombre.CompareTo("HiperGeometrica") == 0)
                byteArray = m.hipergeometrica.Imagen;

            if (nombre.CompareTo("Poisson") == 0)
                byteArray = m.poisson.Imagen;

            if (nombre.CompareTo("UniformeDiscreta") == 0)
                byteArray = m.uniformediscreta.Imagen;

            if (nombre.CompareTo("Beta") == 0)
                byteArray = m.beta.Imagen;

            if (nombre.CompareTo("ChiCuadrado") == 0)
                byteArray = m.chicuadrado.Imagen;

            if (nombre.CompareTo("Exponencial") == 0)
                byteArray = m.exponencial.Imagen;

            if (nombre.CompareTo("F") == 0)
                byteArray = m.f.Imagen;

            if (nombre.CompareTo("Gamma") == 0)
                byteArray = m.gamma.Imagen;

            if (nombre.CompareTo("Normal") == 0)
                byteArray = m.normal.Imagen;

            if (nombre.CompareTo("Pareto") == 0)
                byteArray = m.pareto.Imagen;

            if (nombre.CompareTo("TStudent") == 0)
                byteArray = m.tstudent.Imagen;

            if (nombre.CompareTo("UniformeContinua") == 0)
                byteArray = m.uniformecontinua.Imagen;

            if (nombre.CompareTo("Weibull") == 0)
                byteArray = m.weibull.Imagen;


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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Areas.Modelos.Models;
using TesisProj.Areas.Proyectos.Models;
using TesisProj.Models;

namespace TesisProj.Areas.Simulaciones.Controllers
{
    public class NormalController : Controller
    {
        //
        // GET: /Simulaciones/Normal/

        [HttpGet]
        public ActionResult Index( )
        {
            Pedro_Parametro EmpezarSimulacion = (Pedro_Parametro)Session["Celdas_a_simular"];
            Normal n= new Normal();
            n.mean = EmpezarSimulacion.Elementos.Average(e => e.valor);
            if(Calculos.DesviacionStandard(EmpezarSimulacion.Elementos.Select(e => e.valor).ToList())==0)
            {
                n.std =1;
            }else
            {
                n.std=Calculos.DesviacionStandard(EmpezarSimulacion.Elementos.Select(e => e.valor).ToList());
            }

            RandomGenerator rg = new RandomGenerator(new Random());

            Pedro_Parametro salida = new Pedro_Parametro() ;
            salida.Elementos = new List<Pedro_Elemento>();
            List<double> grafico = new List<double>();
            
            for (int i = 0; i < EmpezarSimulacion.Elementos.Count; i++)
            {
                double x=rg.NormalDeviate();
                Pedro_Elemento aux = new Pedro_Elemento(i, x + n.mean);
                grafico.Add(Math.Round(x + n.mean,1));
                salida.Elementos.Add(aux);
            }

            Session["Celdas_simulada"] = salida.Elementos;
            n.GetFuncionSimpleArreglo(grafico);
            Session["Grafico"] = n.graficar;
            return View(n);
        }

        public ActionResult _CeldasSimuladas()
        {
            return PartialView((List<Pedro_Elemento>)Session["Celdas_simulada"]);
        }

        public ActionResult _Grafico()
        {
            return PartialView((List<Grafico>)Session["Grafico"]);
        }

        [HttpPost]
        public ActionResult Index(Normal n)
        {
            return View();
        }

    }
}

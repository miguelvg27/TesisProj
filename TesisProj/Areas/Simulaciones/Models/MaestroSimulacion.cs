using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Modelos.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Simulaciones.Models
{
    public class MaestroSimulacion 
    {
        public virtual Normal normal { get; set; }
        public virtual Uniforme uniforme { get; set; }
        public virtual Binomial binomial { get; set; }
        public virtual Geometrica geometrica { get; set; }
        public virtual Hipergeometrica hipergeometrica { get; set; }
        public virtual Poisson poisson { get; set; }
        public virtual Pascal pascal { get; set; }

        public virtual List<Celda> CeldasSensibles { get; set; }
        public virtual List<Grafico> Graficos { get; set; }

        public int numeroCeldas { get; set; }


        public MaestroSimulacion()
        {
            this.Graficos = new List<Grafico>();
            this.CeldasSensibles = new List<Celda>();
            this.normal=null;
            this.uniforme=null;
            this.binomial=null;
            this.geometrica=null;
            this.hipergeometrica=null;
            this.poisson =null;
            this.pascal = null;
        }

        public void ActualizarCeldas(string modelo,Parametro parametro)
        {
            //aca genero los numeros aleatorios
            if (modelo=="Normal")
            {
                List<Celda> celdas = new List<Celda>();
                Graficos= normal.GenerarNumerosAleatorios(parametro.Celdas.Count);
                int i = 0;
                foreach (Grafico g in Graficos)
                {
                    decimal valor = Convert.ToDecimal(g.fx);
                    celdas.Add(new Celda { IdParametro = parametro.Celdas[i].IdParametro, Valor = valor, Periodo = parametro.Celdas[i].Periodo });
                    i++;
                }
                CeldasSensibles = celdas;                
            }

            if(modelo=="Uniforme")
            {
                List<Celda> celdas = new List<Celda>();
                Graficos = uniforme.GenerarNumerosAleatorios(parametro.Celdas.Count);
                int i = 0;
                foreach (Grafico g in Graficos)
                {
                    decimal valor = Convert.ToDecimal(g.fx);
                    celdas.Add(new Celda { IdParametro = parametro.Celdas[i].IdParametro, Valor = valor, Periodo = parametro.Celdas[i].Periodo });
                    i++;
                }
                CeldasSensibles = celdas;
            }

            if (modelo == "Poisson")
            {
                List<Celda> celdas = new List<Celda>();
                Graficos = poisson.GenerarNumerosAleatorios(parametro.Celdas.Count);
                int i = 0;
                foreach (Grafico g in Graficos)
                {
                    decimal valor = Convert.ToDecimal(g.fx);
                    celdas.Add(new Celda { IdParametro = parametro.Celdas[i].IdParametro, Valor = valor, Periodo = parametro.Celdas[i].Periodo });
                    i++;
                }
                CeldasSensibles = celdas;
            }
        }
    }
}
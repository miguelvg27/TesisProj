using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Modelos.Models;

namespace TesisProj.Areas.Simulaciones.Models
{
    public class MaestroSimulacion
    {
        public virtual ModeloSimlacion modelobase { get; set; }
        public virtual List<Celda> CeldasSensibles { get; set; }

        public MaestroSimulacion(ModeloSimlacion m)
        {
            this.modelobase = m;
        }

        public void ActualizarCeldas(string modelo,Parametro parametro)
        {
            if (modelobase.Nombre.Equals("Normal"))
            {
                List<Celda> celdas = new List<Celda>();
                RandomGenerator rg = new RandomGenerator();
                foreach (Celda c in parametro.Celdas)
                {
                    decimal valor = Convert.ToDecimal(rg.NormalDeviate() + modelobase.Normal.mean);
                    celdas.Add(new Celda { IdParametro = c.IdParametro, Valor = valor, Periodo = c.Periodo });
                }
                parametro.CeldasSensibles = celdas;
            }
        }

        public List<Celda> GetCeldasSimuladas()
        {
            return CeldasSensibles;
        }
    }
}
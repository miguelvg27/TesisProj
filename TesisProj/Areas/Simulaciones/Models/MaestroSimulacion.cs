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
        public virtual Parametro parametro { get; set; }
        public virtual ModeloSimlacion modelobase { get; set; }

        public MaestroSimulacion(Parametro p, ModeloSimlacion m)
        {
            this.parametro = p;
            this.modelobase = m;
            this.parametro.CeldasSensibles = new List<Celda>();
        }

        public void ActualizarCeldas(string modelo)
        {
            if (modelobase.Nombre.Equals("Normal"))
            {
                List<Celda> celdas = new List<Celda>();
                foreach (Celda c in parametro.Celdas)
                {
                    RandomGenerator rg = new RandomGenerator(new Random());
                    decimal valor = Convert.ToDecimal(rg.NormalDeviate() + modelobase.Normal.mean);
                    celdas.Add(new Celda { IdParametro = c.IdParametro, Valor = valor, Periodo = c.Periodo });
                }
                parametro.CeldasSensibles = celdas;
            }
        }

        public List<Celda> GetCeldasSimuladas()
        {
            return parametro.CeldasSensibles;
        }
    }
}
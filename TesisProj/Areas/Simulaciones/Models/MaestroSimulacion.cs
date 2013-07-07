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
        }

        public void ActualizarCeldas(string modelo)
        {
            if (modelobase.Nombre.Equals("Normal"))
            {
                foreach (Celda c in parametro.Celdas)
                {
                    RandomGenerator rg = new RandomGenerator(new Random());
                    c.Valor = Convert.ToDecimal(rg.NormalDeviate()+modelobase.Normal.mean);
                }
            }
        }

        public List<Celda> GeteldasSimuladas()
        {
            return parametro.Celdas;
        }
    }
}
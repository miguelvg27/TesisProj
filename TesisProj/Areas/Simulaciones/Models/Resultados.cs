using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesisProj.Areas.Modelos.Models;
using TesisProj.Models.Storage;


namespace TesisProj.Areas.Simulaciones.Models
{
    public class Resultados : DbObject
    {
        public virtual ICollection<Simulacion> simulaciones { get; set; }
        public virtual ICollection<double> ResultadosSimulaciones { get; set; }
        public double Minimo { get; set; }
        public double Maximo { get; set; }
        public double Promedio { get; set; }
        public double IC { get; set; }
        public double ColaDerecha { get; set; }
        public double ColaIzquierda { get; set; }
        public virtual ICollection<Grafico> grafico { get; set; }
        public bool GraficoBarras { get; set; }
        public bool GraficoPie { get; set; }
        public bool GraficoPuntos { get; set; }
        public bool GraficoLineas{ get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace TesisProj.Areas.Modelos.Models
{
    public class Normal : Grafico
    {

        #region Parametros

        [DisplayName("Media (u)")]
        public double u { get; set; }

        [DisplayName("Desviación Estandar (o)")]
        public double o { get; set; }

        [DisplayName("Amplitud del intervalo (k)")]
        public double K { get; set; }

        [DisplayName("Valor Esperado")]
        public double E { get; set; }

        [DisplayName("Varianza")]
        public double V { get; set; }

        #endregion

        #region Graficos

        [DisplayName("E(X)")]
        public string Esperado { get { return "~/Graficos/Normal/Esperado.png"; } }

        [DisplayName("VAR(X)")]
        public string Varianza { get { return "~/Graficos/Normal/Varianza.png"; } }

        [DisplayName("Función de Distribución")]
        public string fx_simple { get { return "~/Graficos/Normal/f_simple.png"; } }

        [DisplayName("Función de Distribución Acumulada")]
        public string Fx_acumulada { get { return "~/Graficos/Normal/f_acumulada.png"; } }

        #endregion

        public Normal()
        {
            this.u = 0;
            this.o = 0;
            this.K = 50;
            this.E = 0;
            this.V = 0;
        }

        public Normal(double k, double u, double o)
        {
            this.u = u;
            this.o = o;
            this.K = k;
            this.E = u;
            this.V = o;
        }

        #region Formulas

        public double GetEsperado()
        {
            return Math.Round(E, 2);
        }

        public double GetVarianza()
        {
            return Math.Round(V, 2);
        }

        private double GetFuncion(double K)
        {
            double z = Math.Pow(((K-u)/o),2);
            return (1/(Math.Sqrt(o)*Math.Sqrt(2*Math.PI)))*Math.Exp(-z/2);
        }


        public List<Grafico> GetFuncionSimpleArreglo()
        {
            List<Grafico> s = new List<Grafico>();
            for (double i = -100; i <= 100; i = i + K)
            {
                Grafico t = new Grafico();
                t.fx = GetFuncion(i);
                t.x = i;
                t.sx = Convert.ToString(i);
                t.sfx = Convert.ToString(Math.Round(t.fx * 100, 2));
                s.Add(t);
            }
            using (StreamWriter sw = new StreamWriter(@"C:\Modelo\Normal.txt", true))
            {
                sw.WriteLine("Normal fx" + " - " + DateTime.Now.ToString());
                sw.WriteLine("|x" + "  -  " + "fx|");
                foreach (Grafico g in s)
                {
                    sw.WriteLine("|" + g.sx + "  -  " + g.sfx + "|");
                }
                sw.WriteLine();
            }
            return s;
        }

        public List<Grafico> GetFuncionAcumuladaArreglo()
        {
            List<Grafico> s = new List<Grafico>();
            Double aux = new Double();
            aux = 0;
            for (double i = -100; i <= 100; i = i + K)
            {
                Grafico t = new Grafico();
                aux += GetFuncion(i);
                t.fx = aux;
                t.sfx = Convert.ToString(Math.Round(t.fx * 100, 2));
                t.x = i;
                t.sx = Convert.ToString(i);
                s.Add(t);
            }
            using (StreamWriter sw = new StreamWriter(@"C:\Modelo\Normal.txt", true))
            {
                sw.WriteLine("Normal Fx" + " - " + DateTime.Now.ToString());
                sw.WriteLine("|x" + "  -  " + "Fx|");
                foreach (Grafico g in s)
                {
                    sw.WriteLine("|" + g.sx + "  -  " + g.sfx + "|");
                }
                sw.WriteLine();
            }
            return s;
        }
        #endregion

    }
}
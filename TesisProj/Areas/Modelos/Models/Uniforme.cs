using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace TesisProj.Areas.Modelos.Models
{
    public class Uniforme : Grafico
    {
        #region Parametros

        [DisplayName("Limite Inferior (a)")]
        [Required]
        public double a { get; set; }

        [DisplayName("Limite Superior (b)")]
        [Required]
        public double b { get; set; }

        [DisplayName("Amplitud del intervalo (k)")]
        [Required]
        public double K { get; set; }

        [DisplayName("Valor Esperado")]
        public double E { get; set; }

        [DisplayName("Varianza")]
        public double V { get; set; }

        #endregion

        #region Graficos

        [DisplayName("E(X)")]
        public string Esperado { get { return "~/Graficos/Uniforme/Esperado.png"; } }

        [DisplayName("VAR(X)")]
        public string Varianza { get { return "~/Graficos/Uniforme/Varianza.png"; } }

        [DisplayName("Función de Distribución")]
        public string fx_simple { get { return "~/Graficos/Uniforme/f_simple.png"; } }

        [DisplayName("Función de Distribución Acumulada")]
        public string Fx_acumulada { get { return "~/Graficos/Uniforme/f_acumulada.png"; } }

        #endregion

        public Uniforme()
        {
            this.a = 0;
            this.b = 0;
            this.K = 0;
            this.E = 0;
            this.V = 0;
        }

        public Uniforme(double k, double a, double b)
        {
            this.a = a;
            this.b = b;
            this.K = k;
            this.E = (a+b)/2;
            this.V = Math.Pow(b-a,2)/12;
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

        private double GetFuncionSinple()
        {
            return (1/(b-a));
        }

        private double GetFuncionAcumulada(double K)
        {
            return ((K-a) / (b - a));
        }

        public List<Grafico> GetFuncionSimpleArreglo()
        {
            List<Grafico> s = new List<Grafico>();
            for (double i = a; i <= b; i = i + K)
            {
                Grafico t = new Grafico();
                t.fx = GetFuncionSinple();
                t.x = i;
                t.sx = Convert.ToString(i);
                t.sfx = Convert.ToString(Math.Round(t.fx * 100, 2));
                s.Add(t);
            }
            using (StreamWriter sw = new StreamWriter(@"C:\Modelo\Uniforme.txt", true))
            {
                sw.WriteLine("Uniforme fx" + " - " + DateTime.Now.ToString());
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
            for (double i = a; i <= b; i = i + K)
            {
                Grafico t = new Grafico();
                t.fx = GetFuncionAcumulada(i);
                t.x = i;
                t.sx = Convert.ToString(i);
                t.sfx = Convert.ToString(Math.Round(t.fx * 100, 2));
                s.Add(t);
            }
            using (StreamWriter sw = new StreamWriter(@"C:\Modelo\Uniforme.txt", true))
            {
                sw.WriteLine("Uniforme Fx" + " - " + DateTime.Now.ToString());
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
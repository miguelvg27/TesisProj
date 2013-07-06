using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using TesisProj.Models;


namespace TesisProj.Areas.Modelos.Models
{
    public class Poisson : Grafico
    {
        #region Parametros

        [DisplayName("Valor de Landa")]
        [Required]
        public double L { get; set; }

        [DisplayName("Numero de muestra")]
        [Required]
        public double K { get; set; }

        [DisplayName("Valor Esperado")]
        public double E { get; set; }

        [DisplayName("Varianza")]
        public double V { get; set; }

        #endregion

        #region Graficos

        [DisplayName("E(X)")]
        public string Esperado { get { return "~/Graficos/Poisson/Esperado.png"; } }

        [DisplayName("VAR(X)")]
        public string Varianza { get { return "~/Graficos/Poisson/Varianza.png"; } }

        [DisplayName("Función de Distribución")]
        public string fx_simple { get { return "~/Graficos/Poisson/f_simple.png"; } }

        [DisplayName("Función de Distribución Acumulada")]
        public string Fx_acumulada { get { return "~/Graficos/Poisson/f_acumulada.png"; } }

        #endregion

        public Poisson()
        {
            this.E = 0;
            this.V = 0;
            this.L = 0;
            this.K = 0;
        }

        public Poisson(int K,double L)
        {
            this.E = L;
            this.V = L;
            this.L = L;
            this.K = K;
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

        private double GetFuncion(int K)
        {
            return (Math.Exp(-L) * Math.Pow(L, K)) / (Calculos.Factorial(K));
        }

        public List<Grafico> GetFuncionSimpleArreglo()
        {
            List<Grafico> s = new List<Grafico>();
            for (int i = 0; i <= K; i++)
            {
                Grafico t = new Grafico();
                t.fx = GetFuncion(i);
                t.x = i;
                t.sx = Convert.ToString(i);
                t.sfx = Convert.ToString(Math.Round(t.fx * 100, 2));
                s.Add(t);
            }
            using (StreamWriter sw = new StreamWriter(@"C:\Modelo\Poisson.txt", true))
            {
                sw.WriteLine("Poisson fx" + " - " + DateTime.Now.ToString());
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
            for (int i = 0; i <=K; i++)
            {
                Grafico t = new Grafico();
                aux += GetFuncion(i);
                t.fx = aux;
                t.sfx = Convert.ToString(Math.Round(t.fx * 100, 2));
                t.x = i;
                t.sx = Convert.ToString(i);
                s.Add(t);
            }
            using (StreamWriter sw = new StreamWriter(@"C:\Modelo\Poisson.txt", true))
            {
                sw.WriteLine("Poisson Fx" + " - " + DateTime.Now.ToString());
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
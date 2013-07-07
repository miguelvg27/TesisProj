using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using TesisProj.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Modelos.Models
{
    public class Binomial : DbObject
    {

        #region Parametros

        [DisplayName("Numero de Pruebas Realizadas (N)")]
        [Required]
        public int N { get; set; }

        [DisplayName("Punto a Evaluar")]
        public int K { get; set; }

        [DisplayName("Probabilidad de Exito (p)")]
        [Required]
        public double P { get; set; }

        [DisplayName("Probabilidad de Fracaso (q)")]
        public double Q { get; set; }

        [DisplayName("Valor Esperado")]
        public double E { get; set; }

        [DisplayName("Varianza")]
        public double V { get; set; }

        #endregion

        #region Graficos

        [DisplayName("E(X)")]
        public string Esperado { get { return "~/Graficos/Binomial/Esperado.png"; } }

        [DisplayName("VAR(X)")]
        public string Varianza { get { return "~/Graficos/Binomial/Varianza.png"; } }

        [DisplayName("Función de Distribución")]
        public string fx_simple { get { return "~/Graficos/Binomial/f_simple.png"; } }

        [DisplayName("Función de Distribución Acumulada")]
        public string Fx_acumulada { get { return "~/Graficos/Binomial/f_acumulada.png"; } }

        #endregion

        public Binomial()
        {
            this.N = 0;
            this.P = 0;
            this.Q = 0;
            this.E = 0;
            this.V = 0;
            this.K = 0;
        }

        public Binomial(int N, double P, int K)
        {
            this.N = N;
            this.K = K;
            this.P = P;
            this.Q = 1 - P;
            this.E = N * P;
            this.V = N * P * (1 - P);
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

        private double GetFuncion(int X)
        {
            return Calculos.Combinatoria(N, X) * Math.Pow(P, X) * Math.Pow(Q, N - X);
        }

        public List<Grafico> GetFuncionSimpleArreglo()
        {
            List<Grafico> s = new List<Grafico>();
            for (int i = 0; i <= N; i++)
            {
                Grafico t = new Grafico();
                t.fx = GetFuncion(i);
                t.x = i;
                t.sx = Convert.ToString(i);
                t.sfx = Convert.ToString(Math.Round(t.fx * 100, 2));
                s.Add(t);
            }
            using (StreamWriter sw = new StreamWriter(@"C:\Modelo\Binomial.txt", true))
            {
                sw.WriteLine("Binomial fx" + " - " + DateTime.Now.ToString());
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
            for (int i = 0; i <= N; i++)
            {
                Grafico t = new Grafico();
                aux += GetFuncion(i);
                t.fx = aux;
                t.sfx = Convert.ToString(Math.Round(t.fx * 100, 2));
                t.x = i;
                t.sx = Convert.ToString(i);
                s.Add(t);
            }
            using (StreamWriter sw = new StreamWriter(@"C:\Modelo\Binomial.txt", true))
            {
                sw.WriteLine("Binomial Fx" + " - " + DateTime.Now.ToString());
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
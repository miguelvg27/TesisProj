using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesisProj.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using TesisProj.Models.Storage;


namespace TesisProj.Areas.Modelos.Models
{
    public class Pascal : DbObject
    {
        #region Parametros

        [DisplayName("Numero de Exitos que se espera que ocurra (r)")]
        [Required]
        public int R { get; set; }

        [DisplayName("Numero de pruebas a realizarse (k)")]
        [Required]
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
        public string Esperado { get { return "~/Graficos/Pascal/Esperado.png"; } }

        [DisplayName("VAR(X)")]
        public string Varianza { get { return "~/Graficos/Pascal/Varianza.png"; } }

        [DisplayName("Función de Distribución")]
        public string fx_simple { get { return "~/Graficos/Pascal/f_simple.png"; } }

        [DisplayName("Función de Distribución Acumulada")]
        public string Fx_acumulada { get { return "~/Graficos/Pascal/f_acumulada.png"; } }

        #endregion

        public Pascal()
        {
            this.R = 0;
            this.P = 0;
            this.Q = 0;
            this.E = 0;
            this.V = 0;
            this.K = 0;
        }

        public Pascal(int R,double P, int K)
        {
            this.R = R;
            this.P = P;
            this.K = K;
            this.Q = 1 - P;
            this.E = R *(1.0/P) ;
            this.V = R * (1.0 * Q) / Math.Pow(P, 2);
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
            return Calculos.Combinatoria(K-1, R-1) * Math.Pow(P, R) * Math.Pow(Q, K-R);
        }

        public List<Grafico> GetFuncionSimpleArreglo()
        {
            List<Grafico> s = new List<Grafico>();
            for (int i = R; i <= K; i++)
            {
                Grafico t = new Grafico();
                t.fx = GetFuncion(i);
                t.x = i;
                t.sx = Convert.ToString(i);
                t.sfx = Convert.ToString(Math.Round(t.fx * 100, 2));
                s.Add(t);
            }
            using (StreamWriter sw = new StreamWriter(@"C:\Modelo\Pascal.txt", true))
            {
                sw.WriteLine("Pascal fx" + " - " + DateTime.Now.ToString());
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
            for (int i = R; i <= K; i++)
            {
                Grafico t = new Grafico();
                aux += GetFuncion(i);
                t.fx = aux;
                t.sfx = Convert.ToString(Math.Round(t.fx * 100, 2));
                t.x = i;
                t.sx = Convert.ToString(i);
                s.Add(t);
            }
            using (StreamWriter sw = new StreamWriter(@"C:\Modelo\Pascal.txt", true))
            {
                sw.WriteLine("Pascal Fx" + " - " + DateTime.Now.ToString());
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
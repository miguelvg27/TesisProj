using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.ComponentModel;
using TesisProj.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Modelos.Models
{
    public class Hipergeometrica : DbObject
    {
        #region Parametros

        [DisplayName("Número de elementos (N)")]
        [Required]
        public int N { get; set; }

        [DisplayName("Número de elementos escogidos al azar (n)")]
        [Required]
        public int NN { get; set; }
                
        [DisplayName("Número de Exito (r)")]
        [Required]
        public int r { get; set; }

        [DisplayName("Número de Fracasos (r)")]
        [Required]
        public int fr { get; set; }

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
        public string Esperado { get { return "~/Graficos/Hipergeometrica/Esperado.png"; } }

        [DisplayName("VAR(X)")]
        public string Varianza { get { return "~/Graficos/Hipergeometrica/Varianza.png"; } }

        [DisplayName("Función de Distribución")]
        public string fx_simple { get { return "~/Graficos/Hipergeometrica/f_simple.png"; } }

        [DisplayName("Función de Distribución Acumulada")]
        public string Fx_acumulada { get { return "~/Graficos/Hipergeometrica/f_acumulada.png"; } }

        #endregion

        public Hipergeometrica()
        {
            this.N = 0;
            this.NN = 0;
            this.r = 0;
            this.fr = 0;
            this.E = 0;
            this.V = 0;
            this.Q = 0;
            this.P = 0; 
        }

        public Hipergeometrica(int N, int NN, int r)
        {
            this.N = N;
            this.NN = NN;
            this.r = r;
            this.fr = N-r;
            this.P = (r * 1.0) / N;
            this.Q = 1 - P;
            this.E = NN*P;
            this.V = NN * P * Q * ((N - NN) / (N - 1));
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

        public double GetFuncion(int k)
        {
            return (Calculos.Combinatoria(r, k) * Calculos.Combinatoria(N - r, NN - k)) / (Calculos.Combinatoria(N, NN)*1.0);
        }

        public List<Grafico> GetFuncionSimpleArreglo()
        {
            int a = Math.Max(0, NN + r - N);
            int b = Math.Min(NN, r);

            List<Grafico> s = new List<Grafico>();
            for (int i=a; i<= b; i++)
            {
                Grafico t = new Grafico();
                t.fx = GetFuncion(i);
                t.x = i;
                t.sx = Convert.ToString(i);
                t.sfx = Convert.ToString(Math.Round(t.fx * 100, 2));
                s.Add(t);
            }
            using (StreamWriter sw = new StreamWriter(@"C:\Modelo\Hipergeometrica.txt", true))
            {
                sw.WriteLine("Hipergeometrica fx" + " - " + DateTime.Now.ToString());
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
            int a = Math.Max(0, NN + r - N);
            int b = Math.Min(NN, r);

            List<Grafico> s = new List<Grafico>();
            Double aux = new Double();
            aux = 0;
            for (int i=a; i<= b; i++)
            {
                Grafico t = new Grafico();
                aux += GetFuncion(i);
                t.fx = aux;
                t.sfx = Convert.ToString(Math.Round(t.fx * 100, 2));
                t.x = i;
                t.sx = Convert.ToString(i);
                s.Add(t);
            }
            using (StreamWriter sw = new StreamWriter(@"C:\Modelo\Hipergeometrica.txt", true))
            {
                sw.WriteLine("Hipergeometrica Fx" + " - " + DateTime.Now.ToString());
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.ComponentModel;
using TesisProj.Models;
using TesisProj.Models.Storage;
using TesisProj.Areas.Distribuciones.Models;

namespace TesisProj.Areas.Modelos.Models
{
    public class Hipergeometrica : ModeloSimlacion
    {
        #region Parametros

        [DisplayName("Número de elementos (N)")]
        [Required]
        public int h_N { get; set; }

        [DisplayName("Número de elementos escogidos al azar (n)")]
        [Required]
        public int h_NN { get; set; }
                
        [DisplayName("Número de Exito (r)")]
        [Required]
        public int h_r { get; set; }

        [DisplayName("Número de Fracasos (r)")]
        [Required]
        public int h_fr { get; set; }

        [DisplayName("Probabilidad de Exito (p)")]
        [Required]
        public double h_P { get; set; }

        [DisplayName("Probabilidad de Fracaso (q)")]
        public double h_Q { get; set; }

        [DisplayName("Valor Esperado")]
        public double h_E { get; set; }

        [DisplayName("Varianza")]
        public double h_V { get; set; }

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
            this.h_N = 0;
            this.h_NN = 0;
            this.h_r = 0;
            this.h_fr = 0;
            this.h_E = 0;
            this.h_V = 0;
            this.h_Q = 0;
            this.h_P = 0;
            this.IsEliminado = true;
            Abreviatura="H(N,n,r)";
                    Nombre="Hipergeométrica";
                    Definicion="Combinatoria(n,k)*Combinatoria(N-r,n-k)";
                    Descripcion = "Un experimento hipergeométrico consiste en escoger al azar una muestra de tamaño n, uno a uno sin restitución " +
                                ", de N elementos o resultados posibles, donde r de los cuales pueden clasificarse como éxitos, y " +
                                "los N-r restantes como fracasos. En cada extracción, la probabilidad de que el elemento sea un éxito es diferente" +
                                "ya que la extracción es sin reposición. \n\n" +
                                "Nota: El Numerador de la funcion de la probabilidad hipergeometrica, se requiere que " +
                                "N-r>=n-k, donde resulta que k>=n+r-N, luego el menor valor que toma la variable " +
                                "aleatoria X es el numero : \n\n" +
                                "a=max(0,n+r-N).\n\n" +
                                "Por Otro lado, el mayor valor que debe vereficar k<=n y k<r, luego, el mayor valor " +
                                "que toma Xpuede denotarse por:\n\n" +
                                "b=min(n,r).";
        }

        public Hipergeometrica(int N, int NN, int r)
        {
            this.h_N = N;
            this.h_NN = NN;
            this.h_r = r;
            this.h_fr = N - r;
            this.h_P = (r * 1.0) / N;
            this.h_Q = 1 - h_P;
            this.h_E = NN * h_P;
            this.h_V = NN *h_P * h_Q * ((N - NN) / (N - 1));
        }

        #region Formulas

        public double GetEsperado()
        {
            return Math.Round(h_E, 2);
        }

        public double GetVarianza()
        {
            return Math.Round(h_V, 2);
        }

        public double GetFuncion(int k)
        {
            return (Calculos.Combinatoria(h_r, k) * Calculos.Combinatoria(h_N - h_r, h_NN - k)) / (Calculos.Combinatoria(h_N, h_NN) * 1.0);
        }

        public List<Grafico> GetFuncionSimpleArreglo()
        {
            int a = Math.Max(0, h_NN + h_r -h_N);
            int b = Math.Min(h_NN, h_r);

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
            int a = Math.Max(0, h_NN + h_r - h_N);
            int b = Math.Min(h_NN, h_r);

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
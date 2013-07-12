using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using TesisProj.Areas.Distribuciones.Models;
using TesisProj.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Modelos.Models
{
    public class Binomial : ModeloSimlacion
    {

        #region Parametros

        [DisplayName("Numero de Pruebas Realizadas (N)")]
        [Required]
        public int b_N { get; set; }

        [DisplayName("Punto a Evaluar")]
        public int b_K { get; set; }

        [DisplayName("Probabilidad de Exito (p)")]
        [Required]
        public double b_P { get; set; }

        [DisplayName("Probabilidad de Fracaso (q)")]
        public double b_Q { get; set; }

        [DisplayName("Valor Esperado")]
        public double b_E { get; set; }

        [DisplayName("Varianza")]
        public double b_V { get; set; }

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
            this.b_N = 0;
            this.b_P = 0;
            this.b_Q = 0;
            this.b_E = 0;
            this.b_V = 0;
            this.b_K = 0;
            this.IsEliminado = true;
            Abreviatura="B(n,p)";
                    Nombre="Binomial";
                    Definicion="Combinatoria(n,k)*Potencia(p,k)*Potencia(q,n-k)";
                    Descripcion = "La distribución binomial es una distribución de probabilidad discreta " +
                                "que mide el número de éxitos en una secuencia de n ensayos de Bernoulli " +
                                "independientes entre sí, con una probabilidad fija p de ocurrencia del " +
                                "éxito entre los ensayos. Un experimento de Bernoulli se caracteriza por " +
                                "ser dicotómico, esto es, sólo son posibles dos resultados. " +
                                "A uno de estos se denomina éxito y tiene una probabilidad de ocurrencia p y " +
                                "al otro, fracaso, con una probabilidad q = 1 - p. " +
                                "En la distribución binomial el anterior experimento se repite n veces, " +
                                "de forma independiente, y se trata de calcular la probabilidad de un " +
                                "determinado número de éxitos. Para n = 1, la binomial se convierte, de hecho, " +
                                "en una distribución de Bernoulli.";
        }

        public Binomial(int N, double P, int K)
        {
            this.b_N = N;
            this.b_K = K;
            this.b_P = P;
            this.b_Q = 1 - P;
            this.b_E = N * P;
            this.b_V = N * P * (1 - P);
        }

        #region Formulas

        public double GetEsperado()
        {
            return Math.Round(b_E, 2);
        }

        public double GetVarianza()
        {
            return Math.Round(b_V, 2);
        }

        private double GetFuncion(int X)
        {
            return Calculos.Combinatoria(b_N, X) * Math.Pow(b_P, X) * Math.Pow(b_Q, b_N - X);
        }

        public List<Grafico> GetFuncionSimpleArreglo()
        {
            List<Grafico> s = new List<Grafico>();
            for (int i = 0; i <= b_N; i++)
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
            for (int i = 0; i <= b_N; i++)
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
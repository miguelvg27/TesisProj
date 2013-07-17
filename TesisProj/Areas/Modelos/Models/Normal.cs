using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using TesisProj.Models.Storage;
using TesisProj.Areas.Distribuciones.Models;

namespace TesisProj.Areas.Modelos.Models
{
    public class Normal : ModeloSimlacion
    {

        #region Parametros

        [DisplayName("Media (u)")]
        public double n_mean { get; set; }

        [DisplayName("Desviación Estandar (o)")]
        public double n_std { get; set; }

        public int n_K { get; set; }

        [DisplayName("Valor Esperado")]
        public double n_E { get; set; }

        [DisplayName("Varianza")]
        public double n_V { get; set; }

        public List<Grafico> graficar { get; set; }

        private const double inf = -100;
        private const double avance = 0.001;
        private Int64 Contador { get; set; }
        private double AreaAcum { get; set; }

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
            n_mean = 0;
            n_std = 1;
     Abreviatura="N(u,o)";
                    Nombre="Normal";
                    Definicion=" ";
                    Descripcion = "Se dice que la variable aleatoria continua X, que toma los valores reales " +
                                  ", - inf < x < inf, es normal coon parametros u y o y se describe por " +
                                  "X - N[u,o], si su funcion de densidad de probabilidad es:\n\n";
        }

        public Normal(double m, double s, int k)
        {
            this.n_mean = m;
            this.n_std = s;
            this.n_K = k;
            this.n_E = m;
            this.n_V = s;

        }

        #region Formulas


        public double GetEsperado()
        {
            return Math.Round(n_E, 2);
        }

        public double GetVarianza()
        {
            return Math.Round(n_V, 2);
        }

        private double GetFuncion(double K)
        {
            double z = Math.Pow(((K - n_mean) / n_std), 2);
            return (1 / (Math.Sqrt(n_std) * Math.Sqrt(2 * Math.PI))) * Math.Exp(-z / 2);
        }

        public List<Grafico> GetFuncionSimpleArreglo()
        {
            List<Grafico> s = new List<Grafico>();
            for (double i = 0; i <= n_K; i++)
            {
                Grafico t = new Grafico();
                t.fx = GetFuncion(i);
                t.x = i;
                t.sx = Convert.ToString(i);
                t.sfx = Convert.ToString(Math.Round(t.fx * 100, 2));
                s.Add(t);
            }
            //using (StreamWriter sw = new StreamWriter(@"C:\Modelo\Normal.txt", true))
            //{
            //    sw.WriteLine("Normal fx" + " - " + DateTime.Now.ToString());
            //    sw.WriteLine("|x" + "  -  " + "fx|");
            //    foreach (Grafico g in s)
            //    {
            //        sw.WriteLine("|" + g.sx + "  -  " + g.sfx + "|");
            //    }
            //    sw.WriteLine();
            //}
            return s;
        }
        #endregion

        public List<Grafico> GenerarNumerosAleatorios(int Veces)
        {
            RandomGenerator rg = new RandomGenerator();
            List<Grafico> s = new List<Grafico>();
            double aux=0;
            for (int i = 1; i <= Veces; i++)
            {
                Grafico t = new Grafico();
                aux = rg.NormalDeviate()*n_std + n_mean;
                t.fx = aux;
                t.x = i;
                t.sx = Convert.ToString(i);
                t.sfx = Convert.ToString(Math.Round(aux));
                s.Add(t);
            }
            graficar = s;
            return graficar;
        }

    }

    public class RandomGenerator
    {
        private readonly Random _random;

        //indicates that an extra deviates was already calculated
        private bool _hasAnotherDeviate;

        //The other deviate calculated using the Box-Muller transformation
        private double _otherGaussianDeviate;

        public RandomGenerator()
            : this(new Random())
        {
        }

        public RandomGenerator(int seed)
            : this(new Random(seed))
        {
        }

        public RandomGenerator(Random random)
        {
            _random = random;
        }

        // returns a normally distributed deviate with zero mean and unit variance.
        // Adapted from Numerical Recipe page 289: Normal (Gaussian) Deviates
        public double NormalDeviate()
        {
            double rsq, v1, v2;
            if (_hasAnotherDeviate)
            {
                //we have an extra deviate handy. Reset the flag and return it
                _hasAnotherDeviate = false;
                return _otherGaussianDeviate;
            }
            do
            {
                v1 = UniformDeviate(-1, 1); //pick two uniform number
                v2 = UniformDeviate(-1, 1); //in the square extending from -1 to +1
                rsq = v1 * v1 + v2 * v2;    //see if they are in the unit circle
            } while (rsq >= 1.0 || rsq == 0.0);

            //now make the box-muller transformation to get two normal deviates.
            double fac = Math.Sqrt(-2.0 * Math.Log(rsq) / rsq);
            //Return one and save one for next time
            _otherGaussianDeviate = v1 * fac;
            _hasAnotherDeviate = true;
            return v2 * fac;
        }

        // Returns a uniformly distributed random number between min and max.
        public double UniformDeviate(double min, double max)
        {
            return (max - min) * _random.NextDouble() + min;
        }

        // Returns a random number between 0 and 1
        public double NextDouble()
        {
            return _random.NextDouble();
        }
    }
}
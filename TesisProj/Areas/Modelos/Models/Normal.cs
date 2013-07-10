using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Modelos.Models
{
    public class Normal : DbObject
    {

        #region Parametros

        [DisplayName("Media (u)")]
        public double mean { get; set; }

        [DisplayName("Desviación Estandar (o)")]
        public double std { get; set; }

        [DisplayName("Valor Esperado")]
        public double E { get; set; }

        [DisplayName("Varianza")]
        public double V { get; set; }

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
            mean = 0;
            std = 1;
        }

        public Normal(double m, double s)
        {
            this.mean = m;
            this.std = s;
            this.E = m;
            this.V = s;
        }

        #region Formulas

        private double Aleatorio(double fx)
        {
            double inicio = inf;
            double fin = inf + avance;
            Contador = 0;

            while (AreaAcum < fx)
            {
                Contador += 1;
                inicio = fin;
                fin = inicio + avance;
                AreaAcum += Area(inicio, avance);
            }

            return inicio;

        }

        private double Area(double z, double dx)
        {
            return Math.Exp(-Math.Pow(z, 2) / 2.0) / (std * Math.Sqrt(2 * Math.PI));
        }

        public double GetEsperado()
        {
            return Math.Round(E, 2);
        }

        public double GetVarianza()
        {
            return Math.Round(V, 2);
        }

        public void  GetFuncionSimpleArreglo(List<double> lista)
        {
            List<Grafico> s = new List<Grafico>();
            int i=0;
            foreach (double d in lista)
            {
                Grafico t = new Grafico();
                t.fx = d;
                t.x = i;
                t.sx = Convert.ToString(i);
                t.sfx = Convert.ToString(Math.Round(t.fx * 100, 2));
                s.Add(t);
                i++;
            }
            graficar = s;
        }

        //public List<Grafico> GetFuncionAcumuladaArreglo()
        //{
        //    List<Grafico> s = new List<Grafico>();
        //    Double aux = new Double();
        //    aux = 0;
        //    for (double i = -100; i <= 100; i = i + K)
        //    {
        //        Grafico t = new Grafico();
        //        aux += GetFuncion(i);
        //        t.fx = aux;
        //        t.sfx = Convert.ToString(Math.Round(t.fx * 100, 2));
        //        t.x = i;
        //        t.sx = Convert.ToString(i);
        //        s.Add(t);
        //    }
        //    using (StreamWriter sw = new StreamWriter(@"C:\Modelo\Normal.txt", true))
        //    {
        //        sw.WriteLine("Normal Fx" + " - " + DateTime.Now.ToString());
        //        sw.WriteLine("|x" + "  -  " + "Fx|");
        //        foreach (Grafico g in s)
        //        {
        //            sw.WriteLine("|" + g.sx + "  -  " + g.sfx + "|");
        //        }
        //        sw.WriteLine();
        //    }
        //    return s;
        //}
        #endregion

        public List<Grafico> GenerarNumerosAleatorios(int Veces)
        {
            RandomGenerator rg = new RandomGenerator();
            List<Grafico> s = new List<Grafico>();
            double aux=0;
            for (int i = 1; i <= Veces; i++)
            {
                Grafico t = new Grafico();
                aux = rg.NormalDeviate() + mean;
                t.fx = aux;
                t.x = i;
                t.sx = Convert.ToString(i);
                t.sfx = Convert.ToString(Math.Round(aux));
                s.Add(t);
            }
            return s;
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
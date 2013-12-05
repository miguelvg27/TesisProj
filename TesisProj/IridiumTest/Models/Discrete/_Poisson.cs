using IridiumTest.Const;
using IridiumTest.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IridiumTest.Models.Discrete
{
    public class _Poisson : Datos
    {
        private MathNet.Numerics.Distributions.Poisson modelo;

        public _Poisson()
        {
            modelo = new MathNet.Numerics.Distributions.Poisson(5);
            CrearComplemento();
        }

        public double Sample()
        {
            return Math.Round(modelo.Sample() * 1.0, 2);
        }

        private void CrearComplemento()
        {
            this.Titulo = "Poisson";
            this.Resumen = Img.Image2Bytes(_String.patchImgPoisson + "Resumen.png");
            this.link = "http://www.wolframalpha.com/input/?i=poisson+distribution&lk=4&num=1&lk=4&num=1";
            this.Definicion = "En teoría de probabilidad y estadística, la distribución de Poisson es una distribución de probabilidad discreta que expresa, a partir de una frecuencia " +
                               "de ocurrencia media, la probabilidad que ocurra un determinado número de eventos durante cierto periodo de tiempo. \n" +
                               "λ es un parámetro positivo que representa el número de veces que se espera que ocurra el fenómeno durante un intervalo dado. "+
                               "Por ejemplo, si el suceso estudiado tiene lugar en promedio 4 veces por minuto y estamos interesados en la probabilidad de que ocurra "+
                               "x veces dentro de un intervalo de 10 minutos, usaremos un modelo de distribución de Poisson con λ = 10×4 = 40.";

            this.Imagen = Img.Image2Bytes(_String.patchImgPoisson + "Imagen.png");

            this.ParamsIN = new List<Param> {
                            new Param { indice=1, nombre = "Lambda (λ)", rango = "0 <= l", valorD = 0 },
                            new Param { indice=2, nombre = "Muestras", rango="M > 0",valorI=0 }
            };

            this.ParamsOUT = new List<Param> {
                            new Param { indice=1, nombre = "Máximo", rango = "max"},
                            new Param { indice=2, nombre = "Mínimo", rango = "min"},
                            new Param { indice=3, nombre = "Media ", rango = "E(X)"},
                            new Param { indice=4, nombre = "Mediana ", rango = "med"},
                            new Param { indice=5, nombre = "Moda ", rango = "moda"},
                            new Param { indice=6, nombre = "Varianza ", rango="D(X)"},
                            new Param { indice=7, nombre = "Coeficiente de Simetria ", rango="y1" }
            };

            this.Formulates = new List<Formulate> {
                            new Formulate { NombreFormula="E(X)=", Imagen=Img.Image2Bytes(_String.patchImgPoisson+"EX.png")},
                            new Formulate { NombreFormula="D(X)=", Imagen=Img.Image2Bytes(_String.patchImgPoisson+"DX.png")},
                            new Formulate { NombreFormula="y1=", Imagen=Img.Image2Bytes(_String.patchImgPoisson+"y1.png")},
                            new Formulate { NombreFormula="P(X=r)", Imagen=Img.Image2Bytes(_String.patchImgPoisson+"PX.png")},
            };

            this.Graphics = new List<Graphic>();
            this.Results = new List<Result>();
        }

        public _Poisson(double lambda)
        {
            modelo = new MathNet.Numerics.Distributions.Poisson(lambda);
            CrearComplemento();
        }

        public void GetModelo()
        {
            IEnumerable<int> salida = modelo.Samples();

            for (int i = modelo.Minimum; i <= 25; i++)
            {
                Graphic g = new Graphic();
                g.fx = modelo.Probability(i);
                g.N = i;
                g.Ac = modelo.CumulativeDistribution(i * 1.0);
                g.limpia();
                Graphics.Add(g);
            }
        }

        public void GetSimulacion(int valores)
        {
            for (int i = 1; i <= valores; i++)
            {
                Result r = new Result();
                Random rnd = modelo.RandomSource;
                r.Probabilidad = rnd.NextDouble();
                r.ValorObtenidoI = modelo.Sample();
                Results.Add(r);
            }
        }

        public void GetResumen()
        {
            ParamsOUT[0].valorI = modelo.Maximum;
            ParamsOUT[1].valorI = modelo.Minimum;
            ParamsOUT[2].valorD = Math.Round(modelo.Mean, 2);
            ParamsOUT[3].valorI = modelo.Median;
            ParamsOUT[4].valorI = modelo.Mode;
            ParamsOUT[5].valorD = Math.Round(modelo.Variance, 2);
            ParamsOUT[6].valorD = Math.Round(modelo.Skewness, 2);

            Results = (from s in Results
                       group s by new { s.ValorObtenidoI } into g
                       select new Result()
                       {
                           ValorObtenidoI = g.Key.ValorObtenidoI,
                           cantidad = g.Count(),
                           Probabilidad = g.Average(o => o.Probabilidad)
                       }).OrderBy(u => u.ValorObtenidoI).ToList();
        }

        public void Save()
        {
            String patch = Const._String.patchFileTxt + "Poisson.txt";

            foreach (Param p in ParamsOUT)
            {
                Util.Out.WriteLineFile(patch, true, "Indice=" + p.indice + " Nombre=" + p.nombre + " ValorI=" + p.valorI + " valorD=" + p.valorD);
            }
            foreach (Graphic p in Graphics)
            {
                Util.Out.WriteLineFile(patch, true, "N=" + p.N + " fx=" + p.fx + " AC=" + p.Ac);
            }
            foreach (Result p in Results)
            {
                Util.Out.WriteLineFile(patch, true, "P=" + p.ValorObtenidoI + " Valor=" + p.cantidad);
            }
        }
    }
}
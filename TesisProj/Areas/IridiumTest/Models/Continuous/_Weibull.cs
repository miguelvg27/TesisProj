using System;
using System.Collections.Generic;
using System.Linq;

namespace TesisProj.Areas.IridiumTest.Models.Continuous
{
    public class _Weibull : Datos
    {
        private MathNet.Numerics.Distributions.Weibull modelo;

        private double Maximo { get; set; }

        private double Minimo { get; set; }

        public _Weibull()
        {
            modelo = new MathNet.Numerics.Distributions.Weibull(1, 1);
            CrearComplemento();
        }

        public _Weibull(List<ListField> lists)
        {
            modelo = new MathNet.Numerics.Distributions.Weibull(1, 1);
            CrearComplemento();
            this.Resumen = lists.Where(p => p.Modelo == this.Titulo && p.Atributo == "Resumen").FirstOrDefault().Imagen;
            this.Imagen = lists.Where(p => p.Modelo == this.Titulo && p.Atributo == "Imagen").FirstOrDefault().Imagen;
            this.Formulates = new List<Formulate> {
                            new Formulate { NombreFormula="E(X)=", Imagen=lists.Where(p => p.Modelo == this.Titulo && p.Atributo == "EX").FirstOrDefault().Imagen},
                            new Formulate { NombreFormula="D(X)=", Imagen=lists.Where(p => p.Modelo == this.Titulo && p.Atributo == "DX").FirstOrDefault().Imagen},
                            new Formulate { NombreFormula="y1=", Imagen=lists.Where(p => p.Modelo == this.Titulo && p.Atributo == "y1").FirstOrDefault().Imagen},
                            new Formulate { NombreFormula="P(X=r)", Imagen=lists.Where(p => p.Modelo == this.Titulo && p.Atributo == "PX").FirstOrDefault().Imagen}
            };
        }

        public double Sample()
        {
            return Math.Round(modelo.Sample() * 1.0, 2);
        }

        private void CrearComplemento()
        {
            this.Titulo = "Weibull";
            this.link = "http://www.wolframalpha.com/input/?i=weibull+distribution&lk=4&num=1";
            this.Definicion = "En teoría de la probabilidad y estadística, la distribución de Weibull es una distribución de probabilidad continua. " +
                               " Recibe su nombre de Waloddi Weibull, que la describió detalladamente en 1951, aunque fue descubierta inicialmente por Fréchet (1927) y " +
                               " aplicada por primera vez por Rosin y Rammler (1933) para describir la distribución de los tamaños de determinadas partículas.\n" +
                               " La distribución modela la distribución de fallos (en sistemas) cuando la tasa de fallos es proporcional a una potencia del tiempo:\n" +
                               " Un valor α<1 indica que la tasa de fallos decrece con el tiempo. \n" +
                               " Cuando α=1, la tasa de fallos es constante en el tiempo.\n" +
                               " Un valor α>1 indica que la tasa de fallos crece con el tiempo. ";

            this.ParamsIN = new List<Param> {
                            new Param { indice=1, nombre = "Forma", rango=" 0 < α",valorD=0 },
                            new Param { indice=2, nombre = "Escala", rango=" 0 < β",valorD=0 },
                            new Param { indice=3, nombre = "Muestras", rango="M > 0",valorI=0 }
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

            this.Graphics = new List<Graphic>();
            this.Results = new List<Result>();
        }

        public _Weibull(double a, double b)
        {
            modelo = new MathNet.Numerics.Distributions.Weibull(a, b);
            CrearComplemento();
        }

        public void GetModelo()
        {
            IEnumerable<double> salida = modelo.Samples();
            double intervalo = (modelo.Mean - modelo.Minimum) / 10.0;
            for (double i = modelo.Minimum; i <= modelo.Mean; i = i + intervalo)
            {
                Graphic g = new Graphic();
                g.fx = modelo.Density(i);
                g.Ac = modelo.CumulativeDistribution(i);
                Graphics.Add(g);
            }
        }

        public void GetSimulacion(int valores)
        {
            double d;
            for (int i = 1; i <= valores; i++)
            {
                Result r = new Result();
                Random rnd = modelo.RandomSource;
                d = modelo.Sample();
                r.Probabilidad = rnd.NextDouble();
                r.ValorObtenidoI = Convert.ToInt16(Math.Round(d));
                r.ValorObtenidoD = d;
                Results.Add(r);
            }
        }

        public void GetResumen()
        {
            ParamsOUT[0].valorD = Math.Round(modelo.Maximum, 2);
            ParamsOUT[1].valorD = Math.Round(modelo.Minimum, 2);
            ParamsOUT[2].valorD = Math.Round(modelo.Mean, 2);
            ParamsOUT[3].valorD = modelo.Median;
            ParamsOUT[4].valorD = modelo.Mode;
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

        //public void Save()
        //{
        //    String patch = Const._String.patchFileTxt + "Normal.txt";

        //    foreach (Param p in ParamsOUT)
        //    {
        //        Util.Out.WriteLineFile(patch, true, "Indice=" + p.indice + " Nombre=" + p.nombre + " ValorI=" + p.valorI + " valorD=" + p.valorD);
        //    }
        //    foreach (Graphic p in Graphics)
        //    {
        //        Util.Out.WriteLineFile(patch, true, "=N=" + p.N + " =fx=" + p.fx + " =AC=" + p.Ac);
        //    }
        //    //foreach (Graphic p in Graphics)
        //    //{
        //    //    Util.Out.WriteLineFile(patch, true, p.fx + "=" + p.Ac);
        //    //}
        //    foreach (Result p in Results)
        //    {
        //        Util.Out.WriteLineFile(patch, true, "=valorD=" + p.ValorObtenidoD + "=valorI=" + p.ValorObtenidoI + " =cantidad=" + p.cantidad);
        //    }
        //}
    }
}
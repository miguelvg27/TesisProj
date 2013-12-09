using System;
using System.Collections.Generic;
using System.Linq;

namespace TesisProj.Areas.IridiumTest.Models.Discrete
{
    public class _Binomial : Datos
    {
        private MathNet.Numerics.Distributions.Binomial modelo;

        public _Binomial()
        {
            modelo = new MathNet.Numerics.Distributions.Binomial(1, 1);
            CrearComplemento();
        }

        public _Binomial(List<ListField> lists)
        {
            modelo = new MathNet.Numerics.Distributions.Binomial(1, 1);
            CrearComplemento();
            this.Resumen = lists.Where(p => p.Modelo == this.Titulo && p.Atributo == "Resumen").FirstOrDefault().Imagen;
            this.Imagen = lists.Where(p => p.Modelo == this.Titulo && p.Atributo == "Imagen").FirstOrDefault().Imagen;
            this.Formulates = new List<Formulate> {
                            new Formulate { NombreFormula="E(X)=", Imagen=lists.Where(p => p.Modelo == this.Titulo && p.Atributo == "EX").FirstOrDefault().Imagen},
                            new Formulate { NombreFormula="D(X)=", Imagen=lists.Where(p => p.Modelo == this.Titulo && p.Atributo == "DX").FirstOrDefault().Imagen},
                            new Formulate { NombreFormula="y1=", Imagen=lists.Where(p => p.Modelo == this.Titulo && p.Atributo == "y1").FirstOrDefault().Imagen},
                            new Formulate { NombreFormula="y2=", Imagen=lists.Where(p => p.Modelo == this.Titulo && p.Atributo == "y2").FirstOrDefault().Imagen},
                            new Formulate { NombreFormula="P(X=r)", Imagen=lists.Where(p => p.Modelo == this.Titulo && p.Atributo == "PX").FirstOrDefault().Imagen}
            };
        }

        public double Sample()
        {
            return Math.Round(modelo.Sample() * 1.0, 2);
        }

        private void CrearComplemento()
        {
            this.Titulo = "Binomial";
            this.link = "http://www.wolframalpha.com/input/?i=binomial+distribution";
            this.Definicion = "En estadística, la distribución binomial es una distribución de probabilidad discreta que mide el número de éxitos  " +
                                "en una secuencia de n ensayos de Bernoulli independientes entre sí, con una probabilidad fija p de ocurrencia del " +
                                "éxito entre los ensayos. Un experimento de Bernoulli se caracteriza por ser dicotómico, esto es, sólo son posibles dos resultados. " +
                                "A uno de estos se denomina éxito y tiene una probabilidad de ocurrencia p y al otro, fracaso, con una probabilidad q = 1 - p. " +
                                "En la distribución binomial el anterior experimento se repite n veces, de forma independiente, y se trata de calcular la " +
                                "probabilidad de un determinado número de éxitos. Para n = 1, la binomial se convierte, de hecho, en una distribución de Bernoulli.";

            this.ParamsIN = new List<Param> {
                            new Param { indice=1, nombre = "Probabilidad de éxito (p)", rango = "0 <= p <= 1", valorD = 0 },
                            new Param { indice=2,nombre = "Número de ensayos (n)", rango="n > 0",valorI=0 },
                            new Param { indice=3,nombre = "Muestras", rango="m > 0",valorI=0 }
            };

            this.ParamsOUT = new List<Param> {
                            new Param { indice=1, nombre = "Máximo", rango = "max"},
                            new Param { indice=2, nombre = "Mínimo", rango = "min"},
                            new Param { indice=3, nombre = "Media ", rango = "E(X)"},
                            new Param { indice=4, nombre = "Mediana ", rango = "med"},
                            new Param { indice=5, nombre = "Moda ", rango = "moda"},
                            new Param { indice=6, nombre = "Varianza ", rango="D(X)"},
                            new Param { indice=7, nombre = "Coeficiente de Simetria ", rango="y1" },
            };

            this.Graphics = new List<Graphic>();
            this.Results = new List<Result>();
        }

        public _Binomial(double p, int n)
        {
            modelo = new MathNet.Numerics.Distributions.Binomial(p, n);
            CrearComplemento();
        }

        public void GetModelo()
        {
            IEnumerable<int> salida = modelo.Samples();

            for (int i = modelo.Minimum; i <= modelo.Maximum; i++)
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
                r.Probabilidad = Math.Round(rnd.NextDouble() * 100, 2);
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
                           Probabilidad = g.Average(d => d.Probabilidad)
                       }).OrderBy(u => u.ValorObtenidoI).ToList();
        }

        //public void Save()
        //{
        //    String patch = Const._String.patchFileTxt + "Binomial.txt";

        //    foreach (Param p in ParamsOUT)
        //    {
        //        Util.Out.WriteLineFile(patch, true, "Indice=" + p.indice + " Nombre=" + p.nombre + " ValorI=" + p.valorI + " valorD=" + p.valorD);
        //    }
        //    foreach (Graphic p in Graphics)
        //    {
        //        Util.Out.WriteLineFile(patch, true, "N=" + p.N + " fx=" + p.fx + " AC=" + p.Ac);
        //    }
        //    foreach (Result p in Results)
        //    {
        //        Util.Out.WriteLineFile(patch, true, "P=" + p.ValorObtenidoI + " Valor=" + p.cantidad);
        //    }
        //}
    }
}
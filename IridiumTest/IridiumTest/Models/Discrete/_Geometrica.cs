using IridiumTest.Const;
using IridiumTest.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IridiumTest.Models.Discrete
{
    public class _Geometrica : Datos
    {
        private MathNet.Numerics.Distributions.Geometric modelo;

        public _Geometrica()
        {
            modelo = new MathNet.Numerics.Distributions.Geometric(1);
            CrearComplemento();
        }

        public _Geometrica(double p)
        {
            modelo = new MathNet.Numerics.Distributions.Geometric(p);
            CrearComplemento();
        }

        public double Sample()
        {
            return Math.Round(modelo.Sample() * 1.0, 2);
        }

        public void CrearComplemento()
        {
            this.Titulo = "Geométrica";
            this.Resumen = Img.Image2Bytes(_String.patchImgGeometrica + "Resumen.png");

            this.link = "http://www.wolframalpha.com/input/?i=Geometric&a=*C.Geometric-_*ProbabilityDistribution-";
            this.Definicion = "En teoría de probabilidad y estadística, la distribución geométrica es cualquiera de las dos distribuciones de probabilidad discretas siguientes: " +
                              "la distribución de probabilidad del número X del ensayo de Bernoulli necesaria para obtener un éxito, contenido en el conjunto { 1, 2, 3,...} o " +
                              "la distribución de probabilidad del número Y = X − 1 de fallos antes del primer éxito, contenido en el conjunto { 0, 1, 2, 3,... }. ";

            Imagen = Img.Image2Bytes(_String.patchImgGeometrica + "Imagen.png");

            this.ParamsIN = new List<Param> {
                            new Param { indice=1, nombre = "Probabilidad de éxito (p)", rango = "0 <= p <= 1", valorD = 0 },
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
                            new Formulate { NombreFormula="E(X)=", Imagen=Img.Image2Bytes(_String.patchImgGeometrica+"EX.png")},
                            new Formulate { NombreFormula="D(X)=", Imagen=Img.Image2Bytes(_String.patchImgGeometrica+"DX.png")},
                            new Formulate { NombreFormula="y1=", Imagen=Img.Image2Bytes(_String.patchImgGeometrica+"y1.png")},
                            new Formulate { NombreFormula="P(X=r)", Imagen=Img.Image2Bytes(_String.patchImgGeometrica+"PX.png")},
            };

            this.Graphics = new List<Graphic>();
            this.Results = new List<Result>();
        }

        public void GetModelo()
        {
            IEnumerable<int> salida = modelo.Samples();

            for (int i = modelo.Minimum; i <= modelo.Maximum; i++)
            {
                Graphic g = new Graphic();
                g.fx = modelo.Probability(i);
                if (g.fx < 0.0001) break;
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
                r.ValorObtenidoI = Math.Abs(modelo.Sample());
                while (r.ValorObtenidoI == 0)
                {
                    rnd = modelo.RandomSource;
                    r.Probabilidad = rnd.NextDouble();
                    r.ValorObtenidoI = Math.Abs(modelo.Sample());
                }
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
            String patch = Const._String.patchFileTxt + "Geometrica.txt";

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
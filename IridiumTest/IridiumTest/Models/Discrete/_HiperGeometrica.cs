using IridiumTest.Const;
using IridiumTest.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IridiumTest.Models.Discrete
{
    public class _HiperGeometrica : Datos
    {
        private MathNet.Numerics.Distributions.Hypergeometric modelo;

        public _HiperGeometrica()
        {
            modelo = new MathNet.Numerics.Distributions.Hypergeometric(1,1,1);
            CrearComplemento();
        }

        public double Sample()
        {
            return Math.Round(modelo.Sample() * 1.0, 2);
        }

        private void CrearComplemento() 
        {
            this.Titulo = "Hiper Geométrica";
            this.Resumen = Img.Image2Bytes(_String.patchImgHiperGeometrica + "Resumen.png");
            this.link = "http://www.wolframalpha.com/input/?i=Hypergeometric";
            this.Definicion = "En teoría de la probabilidad la distribución hipergeométrica es una distribución discreta relacionada con muestreos aleatorios y sin reemplazo. " +
                               "Supóngase que se tiene una población de N elementos de los cuales, k pertenecen a la categoría A y N-k a la B. " +
                               "La distribución hipergeométrica mide la probabilidad de obtener x (0 < x < k) elementos de la categoría A en una muestra sin reemplazo de n " +
                               "elementos de la población original.";

            Imagen = Img.Image2Bytes(_String.patchImgHiperGeometrica + "Imagen.png");

            this.ParamsIN = new List<Param> {
                            new Param { indice=1, nombre = "Tamaño de la Población)", rango = "N >= 0", valorI = 0 },
                            new Param { indice=2, nombre = "Número de Éxitos", rango="0 <= k <= N",valorI=0 },
                            new Param { indice=3, nombre = "Tamaño de la SubPoblación", rango="0 <= n <= N",valorI=0 },
                            new Param { indice=4, nombre = "Muestras", rango="M > 0",valorI=0 }
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
                            new Formulate { NombreFormula="E(X)=", Imagen=Img.Image2Bytes(_String.patchImgHiperGeometrica+"EX.png")},
                            new Formulate { NombreFormula="D(X)=", Imagen=Img.Image2Bytes(_String.patchImgHiperGeometrica+"DX.png")},
                            new Formulate { NombreFormula="y1=", Imagen=Img.Image2Bytes(_String.patchImgHiperGeometrica+"y1.png")},
                            new Formulate { NombreFormula="P(X=r)", Imagen=Img.Image2Bytes(_String.patchImgHiperGeometrica+"PX.png")},
            };

            this.Graphics = new List<Graphic>();
            this.Results = new List<Result>();        
        }

        public _HiperGeometrica(int N, int K, int n)
        {
            modelo = new MathNet.Numerics.Distributions.Hypergeometric(N, K, n);
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
            ParamsOUT[3].valorI = Convert.ToInt16(Math.Round(modelo.Mean, 2));  //modelo.Median; error en la clase dll
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
            String patch = Const._String.patchFileTxt + "HiperGeometrica.txt";

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
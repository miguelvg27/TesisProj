using IridiumTest.Const;
using IridiumTest.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IridiumTest.Models.Continuous
{
    public class _UniformeContinua : Datos
    {
        private MathNet.Numerics.Distributions.ContinuousUniform modelo;

        private double Maximo { get; set; }

        private double Minimo { get; set; }

        public _UniformeContinua()
        {
            modelo = new MathNet.Numerics.Distributions.ContinuousUniform(0, 1);
            CrearComplemento();
        }

        public double Sample()
        {
            return Math.Round(modelo.Sample() * 1.0, 2);
        }

        private void CrearComplemento()
        {
            this.Titulo = "Uniforme Continua";
            this.Resumen = Img.Image2Bytes(_String.patchImgUniformeContinua + "Resumen.png");
            this.link = "http://www.wolframalpha.com/input/?i=uniform+distribution";
            this.Definicion = "En teoría de probabilidad y estadística, la distribución uniforme continua es una familia de distribuciones de probabilidad para variables "+
                               " aleatorias continuas, tales que cada miembro de la familia, todos los intervalos de igual longitud en la distribución en su rango son "+
                               " igualmente probables. El dominio está definido por dos parámetros, a y b, que son sus valores mínimo y máximo. ";

            Imagen = Img.Image2Bytes(_String.patchImgUniformeContinua + "Imagen.png");

            this.ParamsIN = new List<Param> {
                            new Param { indice=1, nombre = "Mínimo", rango="-inf < min < +inf",valorD=0 },
                            new Param { indice=2, nombre = "Máximo", rango="-inf < max < +inf",valorD=0 },
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

            this.Formulates = new List<Formulate> {
                            new Formulate { NombreFormula="E(X)=", Imagen=Img.Image2Bytes(_String.patchImgUniformeContinua+"EX.png")},
                            new Formulate { NombreFormula="D(X)=", Imagen=Img.Image2Bytes(_String.patchImgUniformeContinua+"DX.png")},
                            new Formulate { NombreFormula="y1=", Imagen=Img.Image2Bytes(_String.patchImgUniformeContinua+"y1.png")},
                            new Formulate { NombreFormula="P(X=r)", Imagen=Img.Image2Bytes(_String.patchImgUniformeContinua+"PX.png")},
            };

            this.Graphics = new List<Graphic>();
            this.Results = new List<Result>();
        }

        public _UniformeContinua(double min, double max)
        {
            modelo = new MathNet.Numerics.Distributions.ContinuousUniform(min, max);
            CrearComplemento();
        }

        public void GetModelo()
        {
            IEnumerable<double> salida = modelo.Samples();
            double intervalo = (modelo.Maximum - modelo.Minimum) / 10.0;
            for (double i = modelo.Minimum; i <= modelo.Maximum; i = i + intervalo)
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

        public void Save()
        {
            String patch = Const._String.patchFileTxt + "Normal.txt";

            foreach (Param p in ParamsOUT)
            {
                Util.Out.WriteLineFile(patch, true, "Indice=" + p.indice + " Nombre=" + p.nombre + " ValorI=" + p.valorI + " valorD=" + p.valorD);
            }
            foreach (Graphic p in Graphics)
            {
                Util.Out.WriteLineFile(patch, true, "=N=" + p.N + " =fx=" + p.fx + " =AC=" + p.Ac);
            }
            //foreach (Graphic p in Graphics)
            //{
            //    Util.Out.WriteLineFile(patch, true, p.fx + "=" + p.Ac);
            //}
            foreach (Result p in Results)
            {
                Util.Out.WriteLineFile(patch, true, "=valorD=" + p.ValorObtenidoD + "=valorI=" + p.ValorObtenidoI + " =cantidad=" + p.cantidad);
            }
        }
    }
}
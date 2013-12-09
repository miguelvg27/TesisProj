using System;
using System.Collections.Generic;
using System.Linq;

namespace TesisProj.Areas.IridiumTest.Models.Continuous
{
    public class _F : Datos
    {
        private MathNet.Numerics.Distributions.FisherSnedecor modelo;

        public _F()
        {
            modelo = new MathNet.Numerics.Distributions.FisherSnedecor(1, 1);
            CrearComplemento();
        }

        public _F(List<ListField> lists)
        {
            modelo = new MathNet.Numerics.Distributions.FisherSnedecor(1, 1);
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

        public _F(double d1, double d2)
        {
            modelo = new MathNet.Numerics.Distributions.FisherSnedecor(d1, d2);
            CrearComplemento();
        }

        public double Sample()
        {
            return Math.Round(modelo.Sample() * 1.0, 2);
        }

        private void CrearComplemento()
        {
            this.Titulo = "F";
            this.link = "http://www.wolframalpha.com/input/?i=f+distribution&lk=4&num=1&lk=4&num=1";
            this.Definicion = "Usada en teoría de probabilidad y estadística, la distribución F es una distribución de probabilidad continua. \n" +
                              " También se le conoce como distribución F de Snedecor (por George Snedecor) o como distribución F de Fisher-Snedecor.\n" +
                              " Una variable aleatoria de distribución F se construye como el siguiente cociente:\n" +
                              " F =(U1/n)/(U2/m) \n" +
                              " donde: \n" +
                              " U1 y U2 siguen una distribución chi-cuadrado con n y m grados de libertad respectivamente, y \n" +
                              " U1 y U2 son estadísticamente independientes.\n" +
                              " La distribución F aparece frecuentemente como la distribución nula de una prueba estadística, especialmente en el análisis de varianza. ";

            this.ParamsIN = new List<Param> {
                            new Param { indice=1, nombre = "Grado de Libertad (n)", rango = "0 < n", valorD = 0 },
                            new Param { indice=2, nombre = "Grado de Libertad (m)", rango = "0 < m", valorD = 0 },
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

        public void GetModelo()
        {
            IEnumerable<double> salida = modelo.Samples();
            for (double i = 1; i <= 8; i = i + 0.1)
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
                try
                {
                    r.ValorObtenidoI = Convert.ToInt16(Math.Round(d));
                }
                catch
                {
                    d = modelo.Sample();
                    r.ValorObtenidoI = Convert.ToInt16(Math.Round(d));
                }
                r.ValorObtenidoD = d;
                Results.Add(r);
            }
        }

        public void GetResumen()
        {
            ParamsOUT[0].valorD = Math.Round(modelo.Maximum, 2);
            ParamsOUT[1].valorD = Math.Round(modelo.Minimum, 2);
            try
            {
                ParamsOUT[2].valorD = Math.Round(modelo.Mean, 2);
            }
            catch
            {
                ParamsOUT[2].valorD = Math.Round(modelo.DegreesOfFreedom2 / (modelo.DegreesOfFreedom2 - 2), 2);
            }
            try
            {
                ParamsOUT[3].valorD = Math.Round(modelo.Mean, 2);
            }
            catch
            {
                ParamsOUT[3].valorD = Math.Round(modelo.Mean, 2);
                ParamsOUT[4].valorD = Math.Round(modelo.DegreesOfFreedom2 / (modelo.DegreesOfFreedom2 - 2), 2);
            }
            try
            {
                ParamsOUT[3].valorD = -1;
                ParamsOUT[3].valorD = -1;
            }
            catch
            {
                ParamsOUT[3].valorD = Math.Round(modelo.Mean, 2);
            }
            ParamsOUT[4].valorD = Math.Round(modelo.DegreesOfFreedom2 / (modelo.DegreesOfFreedom2 - 2), 2);
            try
            {
                ParamsOUT[5].valorD = Math.Round(modelo.Variance, 2);
            }
            catch
            {
                ParamsOUT[5].valorD = -1;
            }
            try
            {
                ParamsOUT[6].valorD = Math.Round(modelo.Skewness, 2);
            }
            catch
            {
                ParamsOUT[6].valorD = -1;
            }

            //
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
        //    String patch = Const._String.patchFileTxt + "eXPONENCIAOL.txt";

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
using System;
using System.Collections.Generic;
using System.Linq;

namespace TesisProj.Areas.IridiumTest.Models.Continuous
{
    public class _Normal : Datos
    {
        private MathNet.Numerics.Distributions.Normal modelo;
        private double minimo;
        private double maximo;

        public _Normal()
        {
            modelo = new MathNet.Numerics.Distributions.Normal(0, 1);
            CrearComplemento();
        }

        public _Normal(List<ListField> lists)
        {
            modelo = new MathNet.Numerics.Distributions.Normal(0,1);
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
            this.Titulo = "Normal";
            this.link = "http://www.wolframalpha.com/input/?i=DiscreteUniform";
            this.Definicion = "En estadística y probabilidad se llama distribución normal, distribución de Gauss o distribución gaussiana, a una de las distribuciones " +
                               " de probabilidad de variable continua que con más frecuencia aparece aproximada en fenómenos reales." +
                               " La importancia de esta distribución radica en que permite modelar numerosos fenómenos naturales, sociales y psicológicos. " +
                               " Mientras que los mecanismos que subyacen a gran parte de este tipo de fenómenos son desconocidos, por la enorme cantidad de variables " +
                               " incontrolables que en ellos intervienen, el uso del modelo normal puede justificarse asumiendo que cada observación se obtiene como la " +
                               " suma de unas pocas causas independientes";

            this.ParamsIN = new List<Param> {
                            new Param { indice=1, nombre = "Media", rango = "0 <= u", valorD = 0 },
                            new Param { indice=2, nombre = "Desviación Standar", rango = "1 <= o", valorD = 0 },
                            new Param { indice=3, nombre = "Muestras", rango="M > 0",valorI=0 },
                            new Param { indice=4, nombre = "Precision", rango="P > 0",valorD=0.0004 },
                            new Param { indice=5, nombre = "Cola Izquierda", rango="-inf < CI < +inf",valorD=0 },
                            new Param { indice=6, nombre = "Cola Derecha", rango="-inf < CD < +inf",valorD=0 },
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

        public _Normal(double mean, double std, double min, double max,List<ListField> lists)
        {
            this.minimo = min;
            this.maximo = max;
            modelo = new MathNet.Numerics.Distributions.Normal(mean, std);
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

        public void GetModelo()
        {
            IEnumerable<double> salida = modelo.Samples();
            for (double i = minimo; i <= maximo; i = i + modelo.Precision*10)
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
                r.ValorObtenidoI = Convert.ToInt32(Math.Round(d));
                r.ValorObtenidoD = Math.Round(d,3);
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

            double total = (from s in Results select s).Count();
            Results = (from s in Results
                       group s by new { s.ValorObtenidoI } into g
                       select new Result()
                       {
                           ValorObtenidoI = g.Key.ValorObtenidoI,
                           cantidad = Convert.ToInt32(Math.Round(g.Count()*total/100)),
                           Probabilidad = Math.Round((g.Count()*100/total),2)
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace TesisProj.Areas.IridiumTest.Models.Continuous
{
    public class _TStudent : Datos
    {
        private MathNet.Numerics.Distributions.StudentT modelo;

        public _TStudent()
        {
            modelo = new MathNet.Numerics.Distributions.StudentT();
            CrearComplemento();
        }

        public _TStudent(List<ListField> lists)
        {
            modelo = new MathNet.Numerics.Distributions.StudentT();
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

        public _TStudent(double location, double scale, double freedom,List<ListField> lists)
        {
            modelo = new MathNet.Numerics.Distributions.StudentT(location, scale, freedom);
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
            this.Titulo = "TStudent";
            this.link = "http://www.wolframalpha.com/input/?i=t+distribution&lk=4&num=2";
            this.Definicion = "En probabilidad y estadística, la distribución t (de Student) es una distribución de probabilidad que surge del problema de estimar la media " +
                                " de una población normalmente distribuida cuando el tamaño de la muestra es pequeño.\n" +
                               " Aparece de manera natural al realizar la prueba t de Student para la determinación de las diferencias entre dos medias muestrales y para la" +
                               "  construcción del intervalo de confianza para la diferencia entre las medias de dos poblaciones cuando se desconoce la desviación típica de" +
                               "  una población y ésta debe ser estimada a partir de los datos de una muestra.";

            this.ParamsIN = new List<Param> {
                            new Param { indice=1, nombre = "Localización", rango = "0 < µ", valorD = 0 },
                            new Param { indice=2, nombre = "Escala", rango = "0 < ο", valorD = 0 },
                            new Param { indice=3, nombre = "Grados de Libertad", rango = "0 < ʋ", valorD = 0 },
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

            this.Graphics = new List<Graphic>();
            this.Results = new List<Result>();
        }

        public void GetModelo()
        {
            IEnumerable<double> salida = modelo.Samples();
            for (double i = modelo.Mean - 5; i <= modelo.Mean + 5; i = i + 0.1)
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
                r.ValorObtenidoD = Math.Round(d, 3);
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
            try
            {
                ParamsOUT[6].valorD = Math.Round(modelo.Skewness, 2);
            }
            catch
            {
                ParamsOUT[6].valorD = 0;//Math.Round(modelo.Skewness, 2);
            }

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
        //    String patch = Const._String.patchFileTxt + "TStudent.txt";

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
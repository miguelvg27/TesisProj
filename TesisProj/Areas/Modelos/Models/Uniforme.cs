using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using TesisProj.Models.Storage;
using TesisProj.Areas.Distribuciones.Models;

namespace TesisProj.Areas.Modelos.Models
{
    public class Uniforme : ModeloSimlacion
    {
        #region Parametros

        [DisplayName("Limite Inferior (a)")]
        [Required]
        public double u_a { get; set; }

        [DisplayName("Limite Superior (b)")]
        [Required]
        public double u_b { get; set; }

        [DisplayName("Amplitud del intervalo (k)")]
        [Required]
        public double u_K { get; set; }

        [DisplayName("Valor Esperado")]
        public double u_E { get; set; }

        [DisplayName("Varianza")]
        public double u_V { get; set; }

        #endregion

        #region Graficos

        [DisplayName("E(X)")]
        public string Esperado { get { return "~/Graficos/Uniforme/Esperado.png"; } }

        [DisplayName("VAR(X)")]
        public string Varianza { get { return "~/Graficos/Uniforme/Varianza.png"; } }

        [DisplayName("Función de Distribución")]
        public string fx_simple { get { return "~/Graficos/Uniforme/f_simple.png"; } }

        [DisplayName("Función de Distribución Acumulada")]
        public string Fx_acumulada { get { return "~/Graficos/Uniforme/f_acumulada.png"; } }

        #endregion

        public Uniforme()
        {
            this.u_a = 0;
            this.u_b = 0;
            this.u_K = 0;
            this.u_E = 0;
            this.u_V = 0;
            this.IsEliminado = true;
                                Abreviatura="U(a,b)";
                    Nombre="Uniforme";
                    Definicion=" ";
                    Descripcion = "Se dice que la variable aleatoria continua X, tiene distribución " +
                                  "uniforme (o rectangular) en el intervalo [a,b], a < b, y se describe por " +
                                  "X - U[a,b], si su funcion de densidad de probabilidad es:\n\n";
        }

        public Uniforme(double a, double b)
        {
            this.u_a = a;
            this.u_b = b;
            this.u_K = 0;
            this.u_E = (a + b) / 2;
            this.u_V = Math.Pow(b - a, 2) / 12;
        }

        public List<Grafico> graficar { get; set; }

        #region Formulas

        public double GetEsperado()
        {
            return Math.Round(u_E, 2);
        }

        public double GetVarianza()
        {
            return Math.Round(u_V, 2);
        }

        private double GetFuncionSinple()
        {
            return (1 / (u_b - u_a));
        }

        private double GetFuncionAcumulada(double K)
        {
            return ((K - u_a) / (u_b - u_a));
        }

        public List<Grafico> GenerarNumerosAleatorios(int Veces)
        {
            Random r = new Random();
            List<Grafico> s = new List<Grafico>();
            for(int i =1;i<=Veces;i++)
            {
                Grafico t = new Grafico();
                t.fx = (u_b - u_a) * r.NextDouble() + u_a;
                t.x = i;
                t.sx = Convert.ToString(i);
                t.sfx = Convert.ToString(Math.Round((u_b - u_a) * r.NextDouble() + u_a));
                s.Add(t);
            }
            graficar = s;
            return graficar;
        }

        public List<Grafico> GetFuncionSimpleArreglo()
        {
            List<Grafico> s = new List<Grafico>();
            for (double i = u_a; i <= u_b; i = i + u_K)
            {
                Grafico t = new Grafico();
                t.fx = GetFuncionSinple();
                t.x = i;
                t.sx = Convert.ToString(i);
                t.sfx = Convert.ToString(Math.Round(t.fx * 100, 2));
                s.Add(t);
            }
            using (StreamWriter sw = new StreamWriter(@"C:\Modelo\Uniforme.txt", true))
            {
                sw.WriteLine("Uniforme fx" + " - " + DateTime.Now.ToString());
                sw.WriteLine("|x" + "  -  " + "fx|");
                foreach (Grafico g in s)
                {
                    sw.WriteLine("|" + g.sx + "  -  " + g.sfx + "|");
                }
                sw.WriteLine();
            }
            return s;
        }

        public List<Grafico> GetFuncionAcumuladaArreglo()
        {
            List<Grafico> s = new List<Grafico>();
            for (double i = u_a; i <= u_b; i = i + u_K)
            {
                Grafico t = new Grafico();
                t.fx = GetFuncionAcumulada(i);
                t.x = i;
                t.sx = Convert.ToString(i);
                t.sfx = Convert.ToString(Math.Round(t.fx * 100, 2));
                s.Add(t);
            }
            using (StreamWriter sw = new StreamWriter(@"C:\Modelo\Uniforme.txt", true))
            {
                sw.WriteLine("Uniforme Fx" + " - " + DateTime.Now.ToString());
                sw.WriteLine("|x" + "  -  " + "Fx|");
                foreach (Grafico g in s)
                {
                    sw.WriteLine("|" + g.sx + "  -  " + g.sfx + "|");
                }
                sw.WriteLine();
            }
            return s;
        }
        #endregion
    }
}
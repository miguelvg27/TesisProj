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
    public class Geometrica : ModeloSimlacion
    {
        
        #region Parametros

        [DisplayName("Éxito en el intento Nro")]
        [Required]
        public int g_k { get; set; }

        [DisplayName("Probabilidad de Exito (p)")]
        [Required]
        public double g_P { get; set; }

        [DisplayName("Probabilidad de Fracaso (q)")]
        public double g_Q { get; set; }

        [DisplayName("Valor Esperado")]
        public double g_E { get; set; }

        [DisplayName("Varianza")]
        public double g_V { get; set; }

        #endregion

        #region Graficos

        [DisplayName("E(X)")]
        public string Esperado { get { return "~/Graficos/Geometrica/Esperado.png"; } }

        [DisplayName("VAR(X)")]
        public string Varianza { get { return "~/Graficos/Geometrica/Varianza.png"; } }

        [DisplayName("Función de Distribución")]
        public string fx_simple { get { return "~/Graficos/Geometrica/f_simple.png"; } }

        [DisplayName("Función de Distribución Acumulada")]
        public string Fx_acumulada { get { return "~/Graficos/Geometrica/f_acumulada.png"; } }

        #endregion

        public Geometrica()
        {
            this.g_k = 0;
            this.g_P = 0;
            this.g_Q = 0;
            this.g_E = 0;
            this.g_V = 0;
            this.IsEliminado = true;
            Abreviatura="G(p)";
                    Nombre="Geometrica";
                    Definicion="Potencia(p,1)*Potencia(q,k-1)";
                    Descripcion = "La Distribución Geométrica es una distribución de probabilidad discreta la cual " +
                                "mide hasta que ocurra el primer éxitos en una secuencia de n ensayos de Bernoulli " +
                                "sucesivas w independientes ";
        }

        public Geometrica(int k, double P)
        {
            this.g_k = k;
            this.g_P = P;
            this.g_Q = (1 - P);
            this.g_E = 1.0 / P;
            this.g_V = (g_Q) / Math.Pow(P, 2);
        }

        #region Formulas

        public double GetEsperado()
        {
            return Math.Round(g_E, 2);
        }

        public double GetVarianza()
        {
            return Math.Round(g_V, 2);
        }

        public double GetFuncion(int X)
        {
            return g_P * Math.Pow(g_Q, X - 1);
        }

        public List<Grafico> GetFuncionSimpleArreglo()
        {
            List<Grafico> s = new List<Grafico>();
            for (int i = 1; i <= g_k; i++)
            {
                Grafico t = new Grafico();
                t.fx = GetFuncion(i);
                t.x = i;
                t.sx = Convert.ToString(i);
                t.sfx = Convert.ToString(Math.Round(t.fx * 100, 2));
                s.Add(t);
            }
            using (StreamWriter sw = new StreamWriter(@"C:\Modelo\Geometrica.txt", true))
            {
                sw.WriteLine("Geomerica fx" + " - " + DateTime.Now.ToString());
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
            Double aux = new Double();
            aux = 0;
            for (int i = 1; i <= g_k; i++)
            {
                Grafico t = new Grafico();
                aux += GetFuncion(i);
                t.fx = aux;
                t.sfx = Convert.ToString(Math.Round(t.fx * 100, 2));
                t.x = i;
                t.sx = Convert.ToString(i);
                s.Add(t);
            }
            using (StreamWriter sw = new StreamWriter(@"C:\Modelo\Geometrica.txt", true))
            {
                sw.WriteLine("Geometrica Fx" + " - " + DateTime.Now.ToString());
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
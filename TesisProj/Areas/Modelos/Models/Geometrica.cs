using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace TesisProj.Areas.Modelos.Models
{
    public class Geometrica : Grafico
    {
        
        #region Parametros

        [DisplayName("Éxito en el intento Nro")]
        [Required]
        public int k { get; set; }

        [DisplayName("Probabilidad de Exito (p)")]
        [Required]
        public double P { get; set; }

        [DisplayName("Probabilidad de Fracaso (q)")]
        public double Q { get; set; }

        [DisplayName("Valor Esperado")]
        public double E { get; set; }

        [DisplayName("Varianza")]
        public double V { get; set; }

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
            this.k = 0;
            this.P = 0;
            this.Q = 0;
            this.E = 0;
            this.V = 0;
        }

        public Geometrica(int k, double P)
        {
            this.k = k;
            this.P = P;
            this.Q = (1-P);
            this.E = 1.0/P;
            this.V = (Q)/Math.Pow(P,2);
        }

        #region Formulas

        public double GetEsperado()
        {
            return Math.Round(E, 2);
        }

        public double GetVarianza()
        {
            return Math.Round(V, 2);
        }

        public double GetFuncion(int X)
        {
            return P * Math.Pow(Q, X-1);
        }

        public List<Grafico> GetFuncionSimpleArreglo()
        {
            List<Grafico> s = new List<Grafico>();
            for (int i = 1; i<= k; i++)
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
            for (int i = 1; i <= k; i++)
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
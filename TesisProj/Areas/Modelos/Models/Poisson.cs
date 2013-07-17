using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using TesisProj.Models;
using TesisProj.Models.Storage;
using TesisProj.Areas.Distribuciones.Models;


namespace TesisProj.Areas.Modelos.Models
{
    public class Poisson : ModeloSimlacion
    {
       
        #region Parametros

        [DisplayName("Valor de Landa")]
        [Required]
        public double po_L { get; set; }

        [DisplayName("Numero de muestra")]
        [Required]
        public int po_K { get; set; }

        [DisplayName("Limite Inferior")]
        [Required]
        public int limiteInf { get; set; }

        [DisplayName("Limite Superior")]
        [Required]
        public int limiteSup{ get; set; }

        [DisplayName("Valor Esperado")]
        public double po_E { get; set; }

        [DisplayName("Varianza")]
        public double po_V { get; set; }

        public List<Grafico> graficar { get; set; }

        #endregion

        #region Graficos

        [DisplayName("E(X)")]
        public string Esperado { get { return "~/Graficos/Poisson/Esperado.png"; } }

        [DisplayName("VAR(X)")]
        public string Varianza { get { return "~/Graficos/Poisson/Varianza.png"; } }

        [DisplayName("Función de Distribución")]
        public string fx_simple { get { return "~/Graficos/Poisson/f_simple.png"; } }

        [DisplayName("Función de Distribución Acumulada")]
        public string Fx_acumulada { get { return "~/Graficos/Poisson/f_acumulada.png"; } }

        #endregion

        public Poisson()
        {
            this.po_E = 0;
            this.po_V = 0;
            this.po_L = 0;
            this.po_K = 0;
            this.IsEliminado = true;
            Abreviatura="P(l)";
                    Nombre="Poisson";
                    Definicion=" ";
                    Descripcion = "Se dice que la variabl ealeatoria discreta X cuyos valores  " +
                                  "posibles son : 0,1,2,... tienen distribucion de Poisson con parametro l (l>0) " +
                                  "si su funcion de Probabilidad es: \n\n";
        }

        public Poisson(int K,double L)
        {
            this.po_E = L;
            this.po_V = L;
            this.po_L = L;
            this.po_K = K;
        }

        #region Formulas

        public double GetEsperado()
        {
            return Math.Round(po_E, 2);
        }

        public double GetVarianza()
        {
            return Math.Round(po_V, 2);
        }

        private double GetFuncion(int K)
        {
            return (Math.Exp(-po_L) * Math.Pow(po_L, K)) / (Calculos.Factorial(K));
        }

        public List<Grafico> GenerarNumerosAleatorios(int k)
        {
            List<HashSimulacion> Indices = new List<HashSimulacion>();
            List<Grafico> s = new List<Grafico>();
            Double aux = 0;

            for (int i = limiteInf; i <= limiteSup; i++)
            {
                Indices.Add(new HashSimulacion { probabilidad = GetFuncion(i), numeroqaparece = i });
            }

            if (Indices.Sum(f => f.probabilidad) < 0.001)
            {
                for (int i = 1; i <= k; i++)
                {
                    Grafico t = new Grafico();
                    aux = 0;
                    t.fx = aux;
                    t.sfx = Convert.ToString(Math.Round(t.fx));
                    t.x = i;
                    t.sx = Convert.ToString(i);
                    s.Add(t);
                }
                graficar = s;
                return s;
            }

            int[] temporal = new int[1000];
            int cont = 0;

            foreach (HashSimulacion h in Indices)
            {
                int n = Convert.ToInt32(Math.Round(h.probabilidad *100, 2));
                for (int i = 1; i <= n; i++)
                {
                    temporal[cont] = h.numeroqaparece;
                    cont++;
                }
            }
            Random r = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < k; i++)
            {
                int f=r.Next(cont);
                Grafico t = new Grafico();
                aux = temporal[f]; ;
                t.fx = aux;
                t.sfx = Convert.ToString(Math.Round(t.fx));
                t.x = i;
                t.sx = Convert.ToString(i);
                s.Add(t);
            }

            int max= Convert.ToInt32(Math.Truncate(s.Max(o=>o.fx)));
            int min= Convert.ToInt32(Math.Truncate(s.Min(o=>o.fx)));
            graficar = new List<Grafico>();
            for (int y =min; y<=max;y++)
            {
                Grafico t = new Grafico();
                t.fx = s.Where(f=>f.fx==y).Count();
                t.sfx = Convert.ToString(Math.Round(t.fx));
                t.x = y;
                t.sx = Convert.ToString(y);
                graficar.Add(t);
            }
 
            return s;
        }

        public List<Grafico> GetFuncionAcumuladaArreglo()
        {
            List<Grafico> s = new List<Grafico>();
            Double aux = new Double();
            aux = 0;
            for (int i = 0; i <= po_K; i++)
            {
                Grafico t = new Grafico();
                aux += GetFuncion(i);
                t.fx = aux;
                t.sfx = Convert.ToString(Math.Round(t.fx * 100, 2));
                t.x = i;
                t.sx = Convert.ToString(i);
                s.Add(t);
            }
            using (StreamWriter sw = new StreamWriter(@"C:\Modelo\Poisson.txt", true))
            {
                sw.WriteLine("Poisson Fx" + " - " + DateTime.Now.ToString());
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
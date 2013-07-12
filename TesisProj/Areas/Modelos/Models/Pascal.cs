using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesisProj.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using TesisProj.Models.Storage;
using TesisProj.Areas.Distribuciones.Models;


namespace TesisProj.Areas.Modelos.Models
{
    public class Pascal : ModeloSimlacion
    {
        #region Parametros

        [DisplayName("Numero de Exitos que se espera que ocurra (r)")]
        [Required]
        public int pa_R { get; set; }

        [DisplayName("Numero de pruebas a realizarse (k)")]
        [Required]
        public int pa_K { get; set; }

        [DisplayName("Probabilidad de Exito (p)")]
        [Required]
        public double pa_P { get; set; }

        [DisplayName("Probabilidad de Fracaso (q)")]
        public double pa_Q { get; set; }

        [DisplayName("Valor Esperado")]
        public double pa_E { get; set; }

        [DisplayName("Varianza")]
        public double pa_V { get; set; }

        #endregion

        #region Graficos

        [DisplayName("E(X)")]
        public string Esperado { get { return "~/Graficos/Pascal/Esperado.png"; } }

        [DisplayName("VAR(X)")]
        public string Varianza { get { return "~/Graficos/Pascal/Varianza.png"; } }

        [DisplayName("Función de Distribución")]
        public string fx_simple { get { return "~/Graficos/Pascal/f_simple.png"; } }

        [DisplayName("Función de Distribución Acumulada")]
        public string Fx_acumulada { get { return "~/Graficos/Pascal/f_acumulada.png"; } }

        #endregion

        public Pascal()
        {
            this.pa_R = 0;
            this.pa_P = 0;
            this.pa_Q = 0;
            this.pa_E = 0;
            this.pa_V = 0;
            this.pa_K = 0;
            this.IsEliminado = true;
            Abreviatura="P(r,p)";
                    Nombre="Pascal";
                    Definicion = "Combinatoria(k-1,r-1)*Potencia(p,1)*Potencia(q,k-r)";
                    Descripcion = "Se denomina experimento binomial negativo o de pascal "+
                                  "a las repeticiones independientes de un experimento de "+
                                  "aleatorio de Bernulli hasta obtener el éxito número r. "+
                                  "En cada enseayo de Bernulli ´puede ocurrir un éxito con "+
                                  "probabilidad p o un fracaso con probabilidad q=p-1.\n\n"+
                                  "A la variable aleatoria X que se define como el número de "+
                                  "intentos hasta que ocurra el éxito número r se le denomina" +
                                  "variable aleatoria binomial negativa o de Pascal. Su rango "+
                                  "es el conjunto: Rx = {r,r+1,r+2,... }\n\n"+
                                  "Si k pertenece a Rx, el evento [X = k] ocurre, si resulta éxito "+
                                  "en la k-ésima prueba y en los restantes k-1 pruebas resultan r-1 éxitos "+
                                  "y (k-1)-(r-1) =k-r fracasos.\n\n";
        }

        public Pascal(int R,double P, int K)
        {
            this.pa_R = R;
            this.pa_P = P;
            this.pa_K = K;
            this.pa_Q = 1 - P;
            this.pa_E = R * (1.0 / P);
            this.pa_V = R * (1.0 * pa_Q) / Math.Pow(P, 2);
        }

        #region Formulas

        public double GetEsperado()
        {
            return Math.Round(pa_E, 2);
        }

        public double GetVarianza()
        {
            return Math.Round(pa_V, 2);
        }

        private double GetFuncion(int K)
        {
            return Calculos.Combinatoria(K - 1, pa_R - 1) * Math.Pow(pa_P, pa_R) * Math.Pow(pa_Q, K - pa_R);
        }

        public List<Grafico> GetFuncionSimpleArreglo()
        {
            List<Grafico> s = new List<Grafico>();
            for (int i = pa_R; i <= pa_K; i++)
            {
                Grafico t = new Grafico();
                t.fx = GetFuncion(i);
                t.x = i;
                t.sx = Convert.ToString(i);
                t.sfx = Convert.ToString(Math.Round(t.fx * 100, 2));
                s.Add(t);
            }
            using (StreamWriter sw = new StreamWriter(@"C:\Modelo\Pascal.txt", true))
            {
                sw.WriteLine("Pascal fx" + " - " + DateTime.Now.ToString());
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
            for (int i = pa_R; i <= pa_K; i++)
            {
                Grafico t = new Grafico();
                aux += GetFuncion(i);
                t.fx = aux;
                t.sfx = Convert.ToString(Math.Round(t.fx * 100, 2));
                t.x = i;
                t.sx = Convert.ToString(i);
                s.Add(t);
            }
            using (StreamWriter sw = new StreamWriter(@"C:\Modelo\Pascal.txt", true))
            {
                sw.WriteLine("Pascal Fx" + " - " + DateTime.Now.ToString());
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
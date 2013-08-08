using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TesisProj.Models.Storage;
using TesisProj.Areas.Distribuciones.Models;
using TesisProj.Areas.Modelo.Models;
using System.IO;

namespace TesisProj.Areas.Modelos.Models
{
    public class ModeloSimlacion:DbObject
    {
        #region Generales

        [DisplayName("Nombre de la Distribucion")]
        [Required]
        public string Nombre { get; set; }

        [DisplayName("Nombre Corto")]
        [Required]
        public string Abreviatura { get; set; }

        [DisplayName("Descripcion")]
        [Required]
        public string Descripcion { get; set; }

        [DisplayName("Definicion del modelo")]
        public string Definicion { get; set; }

        #endregion

        #region Parametros de Corte

        [DisplayName("Valor Mínimo")]
        public double minimo { get; set; }

        [DisplayName("Valor Máximo")]
        public double maximo { get; set; }

        [DisplayName("Amplitud")]
        public double amplitud { get; set; }

        public List<Grafico> graficoDistribucion { get; set; }

        public List<Grafico> graficoDistribucionEsperado { get; set; }

        [DisplayName("Valor Mínimo Esperado")]
        public double minimoEsperado { get; set; }

        [DisplayName("Valor Máximo Esperado")]
        public double maximoEsperado { get; set; }

        public List<Grafico> graficoEsperado{ get; set; }

        public List<Grafico> graficoSimulacion { get; set; }

        #endregion

        #region Parametros de Simulacion

        public int numeroCeldas { set; get; }

        #endregion

        public virtual Distribucion distribucion { get; set; }

        public ModeloSimlacion()
        {
        }

        protected  Grafico CrearGrafico(double n, Double fx)
        {
            Grafico t = new Grafico();
            t.fx = Math.Round(fx, 2);
            t.x = Math.Round(n, 2);
            t.sx = Convert.ToString(Math.Round(n, 2));
            t.sfx = Convert.ToString(Math.Round(t.fx, 2));
            return t;
        }

        public  void AlmacenarArchivo(String file, List<Grafico> s)
        {
            using (StreamWriter sw = new StreamWriter(@"C:\Modelo\" + file + ".txt", true))
            {
                sw.WriteLine("Normal fx" + " - " + DateTime.Now.ToString());
                sw.WriteLine("|x" + "  -  " + "fx|");
                foreach (Grafico g in s)
                {
                    sw.WriteLine("|" + g.sx + "  -  " + g.sfx + "|");
                }
                sw.WriteLine();
            }
        }



    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.MonteCarlo.Models
{
    public class AlgoritmoMonteCarlo : DbObject
    {
        public virtual List<Parametro> Parametros { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Numero de Escenarios")]
        [Range(1,10000,ErrorMessage="Debe Ingresar un numero de escenarios entre 1 y 10000 para el algoritmo de Monte Carlo")]
        public int NumeroSimulaciones { set; get; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Numero de Intervalos")]
        [Range(1, 10000, ErrorMessage = "Debe Ingresar un numero de Intervalos entre 1 y 10000 para poder agrupar el grafico")]
        public int NumeroIntervalos { set; get; }

        //[DisplayName("Grafico de Barras")]
        //[DisplayName("Grafico de Columnas")]
        //[DisplayName("Grafico de Lineas")]
        //[DisplayName("Grafico de Areas")]
        [DisplayName("Numero de Intervalos")]
        public  string grafico { get; set; }

        public AlgoritmoMonteCarlo()
        {
            
        }
    }

}
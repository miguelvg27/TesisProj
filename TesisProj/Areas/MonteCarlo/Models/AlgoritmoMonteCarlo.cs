using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Modelos.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.MonteCarlo.Models
{
    public class AlgoritmoMonteCarlo : DbObject
    {
        public virtual List<Parametro> Parametros { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Numero de Escenarios")]
        [Range(1, 10000, ErrorMessage = "Debe Ingresar un numero de escenarios entre 1 y 10000 para el algoritmo de Monte Carlo")]
        public int NumeroSimulaciones { set; get; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Numero de Intervalos")]
        [Range(1, 10000, ErrorMessage = "Debe Ingresar un numero de Intervalos entre 1 y 10000 para poder agrupar el grafico")]
        public int NumeroIntervalos { set; get; }

        public List<Grafico> VanInversionista { get; set; }
        public List<Grafico> VanProyecto { get; set; }
        public List<Grafico> TirInversionista { get; set; }
        public List<Grafico> TirProyecto { get; set; }

        public double MaxVanInversionista { get; set; }
        public double MaxVanProyecto { get; set; }
        public double MaxTirInversionista { get; set; }
        public double MaxTirProyecto { get; set; }

        public double MinVanInversionista { get; set; }
        public double MinVanProyecto { get; set; }
        public double MinTirInversionista { get; set; }
        public double MinTirProyecto { get; set; }
        
        public AlgoritmoMonteCarlo()
        {

        }
    }

}
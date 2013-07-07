using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Comparacion.Models
{
    public class Comparar:DbObject
    {
        public Proyecto proyecto { get; set; }
        public bool Compara {get;set;}
        public double VanProyecto { get; set; }
        public double VanInversionista { get; set; }
        public double TirInversionista { get; set; }
        public double TirProyecto { get; set; }

        public Comparar() 
        {
            Compara = false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesisProj.Areas.Modelo.Models;

namespace TesisProj.Areas.CompararProyecto.Models
{
    public class Comparar
    {
        public int Id { get; set; }
        public Proyecto proyecto { get; set; }
        public bool Compara { get; set; }
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
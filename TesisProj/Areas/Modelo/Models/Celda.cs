using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Modelo.Models
{
    public class Celda : DbObject
    {
        public int Periodo { get; set; }
        public object Valor { get; set; }
        public int Parametro { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesisProj.Models.Storage;


namespace TesisProj.Areas.Modelos.Models
{
    public class Grafico : DbObject
    {
        public string sx { get; set; }
        public string sfx { get; set; }

        public double x { get; set; }
        public double fx {get;set;}
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Proyectos.Models
{
    public class Pedro_Proyecto : DbObject
    {
        public string Nombre { get; set; }
        public DateTime fecha { get; set; }
        public virtual Pedro_Parametro parametro { get; set; }
        public double Resultado { get; set; }

        public Pedro_Proyecto()
        {
            fecha = DateTime.Now;
        }
    }
}
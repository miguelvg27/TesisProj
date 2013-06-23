using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesisProj.Models;

namespace TesisProj.Areas.Proyecto.Models
{
    public class Proyecto
    {
        public string Nombre { get; set; }
        public DateTime Creacion { get; set; }
        public DateTime Modificacion { get; set; }
        public UserProfile Creador { get; set; }
        public UserProfile Modificador { get; set; }
        public string Descripcion { get; set; }
    }
}

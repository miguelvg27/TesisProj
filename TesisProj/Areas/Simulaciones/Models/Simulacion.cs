using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesisProj.Areas.Proyectos.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Simulaciones.Models
{
    public class Simulacion : DbObject
    {
        public virtual Pedro_Proyecto proyecto { get; set; }
        public virtual ICollection<Asignacion> asignaciones { get; set; }
        public int CantidadSimulaciones { get; set; }

        public Simulacion()
        {
            asignaciones = new List<Asignacion>();
            CantidadSimulaciones = 1000;
        }
    }
}
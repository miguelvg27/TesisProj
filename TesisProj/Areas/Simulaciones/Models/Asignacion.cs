using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesisProj.Areas.Distribuciones.Models;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Proyectos.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Simulaciones.Models
{
    public class Asignacion : DbObject
    {
        //Miguel me da un arreglo de celdas al que yo le aplico distribucion para generar un nuevo modelo de celda

        public virtual Pedro_Parametro Celdas { get; set; } //esto es lo que no entiendo yo uso parametros y tu celdas
        public virtual ICollection<Distribucion> Distribuciones { get; set; }

        public Asignacion()
        {
            Distribuciones = new List<Distribucion>();
        }
    }
}
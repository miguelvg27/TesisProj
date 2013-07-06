using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Administracion.Models
{
    public class EstadoCivil : DbObject
    {
        public string Descripcion { get; set; }

        public EstadoCivil()
        {
            this.Id = 1;
            this.Descripcion = "No Especificar";
        }
        public EstadoCivil(int id, string descripcion)
        {
            this.Id = id;
            this.Descripcion = descripcion;
        }
    }
}
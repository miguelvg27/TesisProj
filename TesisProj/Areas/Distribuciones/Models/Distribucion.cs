using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TesisProj.Areas.Modelos.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Distribuciones.Models
{
    public class Distribucion : DbObject
    {
        [DisplayName("Tipo de Distribucion")]
        public string Tipo { get; set; }

        [DisplayName("Nombre Corto")]
        public string NombreCorto { get; set; }

        [DisplayName("Descripcion")]
        [UIHint("Editor")]
        public string Descripcion { get; set; }

        [DisplayName("Imagen")]
        public string Imagen { get; set; }

        public virtual List<ModeloSimlacion> Modelos { get; set; }

        public Distribucion()
        {
            Modelos = new List<ModeloSimlacion>();
        }
    }
}
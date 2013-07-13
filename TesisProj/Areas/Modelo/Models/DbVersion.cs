using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TesisProj.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Modelo.Models
{
    public class DbVersion : DbObject
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Proyecto")]
        public int IdProyecto { get; set; }

        [ForeignKey("IdProyecto")]
        public virtual Proyecto Proyecto { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Versión")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo {0} debe ser mayor que 0")]
        public int Version { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Creador")]
        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual UserProfile Creador { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Fecha de creación")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        public byte[] Data { get; set; }
        public string Extension { get; set; }
        public string Mime { get; set; }
    }
}
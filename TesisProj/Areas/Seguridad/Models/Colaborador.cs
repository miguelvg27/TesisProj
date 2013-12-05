using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Seguridad.Models
{
    [Table("UserProfileXProyecto")]
    public class Colaborador : DbObject
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Usuario")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Proyecto")]
        public int IdProyecto { get; set; }

        [XmlIgnore]
        [ForeignKey("IdUsuario")]
        public virtual UserProfile Usuario { get; set; }

        [XmlIgnore]
        [ForeignKey("IdProyecto")]
        public virtual Proyecto Proyecto { get; set; }

        [DisplayName("Solo lectura")]
        public bool SoloLectura { get; set; }

        public override string LogValues()
        {
            return "Colaborador = " + this.Usuario.UserName + Environment.NewLine +
                "Solo lectura = " + this.SoloLectura;
        }
    }
}
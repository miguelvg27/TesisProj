using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TesisProj.Models.Storage;
using TesisProj.Areas.Plantilla.Models;

namespace TesisProj.Areas.Plantilla.Models
{
    [Table("SubTipoElemento")]
    public class SubTipoElemento : DbObject
    {
        [Required(ErrorMessage="El campo {0} es obligatorio")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener un mínimo de {2} y un máximo de {1} carácteres.")]
        [DisplayName("Subtipo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Tipo")]
        public int IdTipoElemento { get; set; }

        [ForeignKey("IdTipoElemento")]
        public TipoElemento TipoElemento { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TesisProj.Models.Storage;

namespace TesisProj.Models
{
    [Table("TipoElemento")]
    public class TipoElemento : DbObject
    {
        [Required]
        [StringLength(30, ErrorMessage = "El campo {0} debe tener mínimo {2} carácteres.", MinimumLength = 1)]
        [DisplayName("Descripción")]
        public string Nombre { get; set; }

    }
}
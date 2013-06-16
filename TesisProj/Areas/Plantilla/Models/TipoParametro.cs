using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Plantilla.Models
{
    [Table("TipoParametro")]
    public class TipoParametro : DbObject, IValidatableObject
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener un mínimo de {2} y un máximo de {1} carácteres.")]
        [DisplayName("Tipo")]
        public string Nombre { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (TProjContext context = new TProjContext())
            {
                if (context.TipoParametros.Any(t => t.Nombre == this.Nombre && t.Id != this.Id))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo nombre.", new string[] { "Nombre" });
                }
            }
        }
    }
}
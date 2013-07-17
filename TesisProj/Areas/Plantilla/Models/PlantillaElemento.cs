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
    [Table("PlantillaElemento")]
    public class PlantillaElemento : DbObject, IValidatableObject
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener un mínimo de {2} y un máximo de {1} carácteres.")]
        [DisplayName("Plantilla")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Tipo")]
        public int IdTipoElemento { get; set; }

        [ForeignKey("IdTipoElemento")]
        public virtual TipoElemento TipoElemento { get; set; }

        [InverseProperty("PlantillaElemento")]
        public virtual List<PlantillaParametro> Parametros { get; set; }

        [InverseProperty("PlantillaElemento")]
        public virtual List<PlantillaFormula> Formulas { get; set; }

         public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (TProjContext context = new TProjContext())
            {
                if (context.PlantillaElementos.Any(p => p.Nombre == this.Nombre && (this.Id > 0 ? p.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo nombre bajo el mismo tipo.", new string[] { "Nombre" });
                }
            }
        }
    }
}
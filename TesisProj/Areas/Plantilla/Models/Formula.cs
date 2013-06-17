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
    [Table("Formula")]
    public class Formula : DbObject, IValidatableObject
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener un mínimo de {2} y un máximo de {1} carácteres.")]
        [DisplayName("Fórmula")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Secuencia")]
        [Range(1, int.MaxValue)]
        public int Secuencia { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Plantilla")]
        public int IdPlantillaElemento { get; set; }

        [ForeignKey("IdPlantillaElemento")]
        public PlantillaElemento PlantillaElemento { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Tipo")]
        public int IdTipoFormula { get; set; }

        [ForeignKey("IdTipoFormula")]
        public TipoFormula TipoFormula { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener un máximo de {1} carácteres.")]
        [DisplayName("Cadena")]
        public string Cadena { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (TProjContext context = new TProjContext())
            {
                if (context.Formulas.Any(f => f.Nombre == this.Nombre && f.IdPlantillaElemento == this.IdPlantillaElemento && (this.Id > 0 ? f.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo nombre en la misma plantilla.", new string[] { "Nombre" });
                }

                if (context.Formulas.Any(f => f.Secuencia == this.Secuencia && f.IdPlantillaElemento == this.IdPlantillaElemento && (this.Id > 0 ? f.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo número de secuencia en la misma plantilla.", new string[] { "Secuencia" });
                }
            }
        }

    }
}
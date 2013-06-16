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
    public class Parametro : DbObject, IValidatableObject
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener un mínimo de {2} y un máximo de {1} carácteres.")]
        [DisplayName("Parámetro")]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Plantilla")]
        public int IdPlantillaElemento { get; set; }

        [ForeignKey("IdPlantillaElemento")]
        public PlantillaElemento PlantillaElemento { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Tipo")]
        public int IdTipoParametro { get; set; }

        [ForeignKey("IdTipoParametro")]
        public TipoParametro TipoParametro { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (TProjContext context = new TProjContext())
            {
                if (context.Parametros.Any(p => p.Nombre == this.Nombre && p.IdPlantillaElemento == this.IdPlantillaElemento && p.Id != this.Id))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo nombre en la misma plantilla.", new string[] { "Nombre" });
                }
            }
        }
    }
}
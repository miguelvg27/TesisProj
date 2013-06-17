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
    [Table("ElementoXProyecto")]
    public class PlantillaElementoProyecto : DbObject, IValidatableObject
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Plantilla de proyecto")]
        public int IdProyecto { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Plantilla de elemento")]
        public int IdElemento { get; set; }

        [ForeignKey("IdProyecto")]
        public PlantillaProyecto Proyecto { get; set; }

        [ForeignKey("IdElemento")]
        public PlantillaElemento Elemento { get; set; }

        public String Nombre { get { return Elemento.Nombre; } }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (TProjContext context = new TProjContext())
            {
                if (context.PlantillaElementoProyectos.Any(p => p.IdProyecto == this.IdProyecto && p.IdElemento == this.IdElemento && (this.Id > 0 ? p.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con la misma combinación Proyecto/Elemento.", new string[] { "Proyecto", "Elemento" });
                }
            }
        }
    }
}
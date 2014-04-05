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
    [Table("TipoFormula")]
    public class TipoFormula : DbObject, IValidatableObject
    {
        [Required(ErrorMessage="El campo {0} es obligatorio")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener un mínimo de {2} y un máximo de {1} carácteres.")]
        [DisplayName("Tipo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Tipo de plantilla")]
        public int IdTipoElemento { get; set; }

        [ForeignKey("IdTipoElemento")]
        public TipoElemento TipoElemento { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener un mínimo de {2} y un máximo de {1} carácteres.")]
        [DisplayName("Referencia")]
        [RegularExpression("[A-Za-z]+[A-Za-z1-9_]*", ErrorMessage = "El campo solo puede contener alfanuméricos y debe comenzar con una letra.")]
        public string Referencia { get; set; }

        [DisplayName("Único por proyecto")]
        public bool Unico { get; set; }

        public string ListName { get { return Nombre + " (" + Referencia + ")"; } }

        public double[] Valores;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (TProjContext context = new TProjContext())
            {
                if (context.TipoFormulas.Any(t => t.Nombre == this.Nombre && t.IdTipoElemento == this.IdTipoElemento && (this.Id > 0 ? t.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo nombre .", new string[] { "Nombre" });
                }

                if (context.TipoFormulas.Any(f => f.Referencia == this.Referencia && (this.Id > 0 ? f.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo nombre de referencia.", new string[] { "Referencia" });
                }
            }
        }
    }
}
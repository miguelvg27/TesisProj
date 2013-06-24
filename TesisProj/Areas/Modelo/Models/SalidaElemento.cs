using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TesisProj.Areas.Plantilla.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Modelo.Models
{
    [Table("SalidaElemento")]
    public class SalidaElemento : DbObject, IValidatableObject
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener un mínimo de {2} y un máximo de {1} carácteres.")]
        [DisplayName("Salida")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Secuencia")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo {0} debe ser mayor que 0")]
        public int Secuencia { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Elemento")]
        public int IdElemento { get; set; }

        [ForeignKey("IdElemento")]
        public Elemento Elemento { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Fórmula")]
        public int IdFormula { get; set; }

        [ForeignKey("IdFormula")]
        public Formula Formula { get; set; }

        public List<object> Valores { get; set; }

        public SalidaElemento()
        {
        }

        public SalidaElemento(PlantillaSalidaElemento plantilla, int IdElemento, int IdFormula)
        {
            this.IdElemento = IdElemento;
            this.IdFormula = IdFormula;
            this.Nombre = plantilla.Nombre;
            this.Secuencia = plantilla.Secuencia;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (TProjContext context = new TProjContext())
            {
                if (context.SalidaElementos.Any(s => s.Nombre == this.Nombre && s.IdElemento == this.IdElemento && (this.Id > 0 ? s.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo nombre en el mismo elemento.", new string[] { "Nombre" });
                }

                if (context.SalidaElementos.Any(s => s.Secuencia == this.Secuencia && s.IdElemento == this.IdElemento && (this.Id > 0 ? s.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo número de secuencia en el mismo elemento.", new string[] { "Secuencia" });
                }
            }
        }

    }
}
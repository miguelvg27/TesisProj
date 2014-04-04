using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Plantilla.Models
{
    [Table("PlantillaParametro")]
    public class PlantillaParametro : DbObject, IValidatableObject
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener un mínimo de {2} y un máximo de {1} carácteres.")]
        [DisplayName("Parámetro")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener un mínimo de {2} y un máximo de {1} carácteres.")]
        [DisplayName("Referencia")]
        [RegularExpression("[A-Za-z]+[A-Za-z1-9]*", ErrorMessage = "El campo solo puede contener alfanuméricos y debe comenzar con una letra.")]
        public string Referencia { get; set; }

        [DisplayName("Valor único")]
        public bool Constante { get; set; }

        [DisplayName("Disponible para sensibilidad")]
        public bool Sensible { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Plantilla")]
        public int IdPlantillaElemento { get; set; }

        [ForeignKey("IdPlantillaElemento")]
        public virtual PlantillaElemento PlantillaElemento { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Tipo")]
        public int IdTipoDato { get; set; }

        [ForeignKey("IdTipoDato")]
        public virtual TipoDato TipoDato { get; set; }

        public PlantillaParametro(PlantillaParametro plantilla)
        {
            this.IdTipoDato = plantilla.IdTipoDato;
            this.Nombre = plantilla.Nombre;
            this.Referencia = plantilla.Referencia;
            this.Constante = plantilla.Constante;
            this.Sensible = plantilla.Sensible;
        }

        public PlantillaParametro(Parametro plantilla)
        {
            this.IdTipoDato = plantilla.IdTipoDato;
            this.Nombre = plantilla.Nombre;
            this.Referencia = plantilla.Referencia;
            this.Constante = plantilla.Constante;
            this.Sensible = plantilla.Sensible;
        }

        public PlantillaParametro() { }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (TProjContext context = new TProjContext())
            {
                if (context.PlantillaParametros.Any(p => p.Nombre == this.Nombre && p.IdPlantillaElemento == this.IdPlantillaElemento && (this.Id > 0 ? p.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo nombre en la misma plantilla.", new string[] { "Nombre" });
                }

                if (context.PlantillaFormulas.Any(f => f.Referencia == this.Referencia && f.IdPlantillaElemento == this.IdPlantillaElemento && (this.Id > 0 ? f.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe una fórmula con el mismo nombre de referencia en la misma plantilla.", new string[] { "Referencia" });
                }

                if (Generics.Reservadas.Contains(this.Referencia))
                {
                    yield return new ValidationResult("Ya existe una palabra reservada con el mismo nombre.", new string[] { "Referencia" });
                }
            }
        }
    }
}
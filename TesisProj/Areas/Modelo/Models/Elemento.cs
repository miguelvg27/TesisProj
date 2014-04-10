using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using TesisProj.Areas.Plantilla.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Modelo.Models
{
    [Table("Elemento")]
    public class Elemento : DbObject, IValidatableObject
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener un mínimo de {2} y un máximo de {1} carácteres.")]
        [DisplayName("Elemento")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Tipo")]
        public int IdTipoElemento { get; set; }

        [XmlIgnore]
        [ForeignKey("IdTipoElemento")]
        public virtual TipoElemento TipoElemento { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Proyecto")]
        public int IdProyecto { get; set; }

        [XmlIgnore]
        [ForeignKey("IdProyecto")]
        public virtual Proyecto Proyecto { get; set; }

        [InverseProperty("Elemento")]
        public virtual List<Parametro> Parametros { get; set; }

        [InverseProperty("Elemento")]
        public virtual List<Formula> Formulas { get; set; }

        public override string LogValues()
        {
            return "Nombre = " + this.Nombre + Environment.NewLine +
                "Tipo elemento = " + this.TipoElemento.Nombre; 
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (TProjContext context = new TProjContext())
            {
                if (context.Elementos.Any(p => p.Nombre == this.Nombre && p.IdProyecto == this.IdProyecto && (this.Id > 0 ? p.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo nombre en el proyecto.", new string[] { "Nombre" });
                }
            }
        }     
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using TesisProj.Areas.Plantilla.Models;
using TesisProj.Models;
using TesisProj.Models.Storage;
using System.Linq;
using TesisProj.Areas.Simulaciones.Models;

namespace TesisProj.Areas.Modelo.Models
{
    [Table("Parametro")]
    public class Parametro : DbObject, IValidatableObject
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

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Elemento")]
        public int IdElemento { get; set; }

        [XmlIgnore]
        [ForeignKey("IdElemento")]
        public virtual Elemento Elemento { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Tipo")]
        public int IdTipoParametro { get; set; }

        [XmlIgnore]
        [ForeignKey("IdTipoParametro")]
        public virtual TipoParametro TipoParametro { get; set; }

        [DisplayName("Constante")]
        public bool Constante { get; set; }

        [DisplayName("Sensible")]
        public bool Sensible { get; set; }

        [InverseProperty("Parametro")]
        public virtual List<Celda> Celdas { get; set; }

        public string XML_ModeloAsignado { get; set; }

        [NotMapped]
        public virtual List<Celda> CeldasSensibles { get; set; }

        public Parametro()
        {
        }

        public Parametro(PlantillaParametro plantilla, int IdElemento)
        {
            this.IdElemento = IdElemento;
            this.IdTipoParametro = plantilla.IdTipoParametro;
            this.Nombre = plantilla.Nombre;
            this.Referencia = plantilla.Referencia;
            this.Constante = plantilla.Constante;
            this.Sensible = plantilla.Sensible;
        }

        public override string LogValues()
        {
            return "Elemento = " + this.Elemento.Nombre + Environment.NewLine +
                "Nombre = " + this.Nombre + Environment.NewLine +
                "Referencia = " + this.Referencia + Environment.NewLine +
                "Tipo parámetro = " + this.TipoParametro.Nombre + Environment.NewLine +
                "Sensible = " + this.Sensible + Environment.NewLine +
                "Constante = " + this.Constante;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (TProjContext context = new TProjContext())
            {
                if (context.Parametros.Any(p => p.Nombre == this.Nombre && p.IdElemento == this.IdElemento && (this.Id > 0 ? p.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo nombre en la mismo elemento.", new string[] { "Nombre" });
                }

                if (context.Formulas.Any(f => f.Referencia == this.Referencia && f.IdElemento == this.IdElemento && (this.Id > 0 ? f.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe una fórmula con el mismo nombre de referencia en el mismo elemento.", new string[] { "Referencia" });
                }

                if (context.Parametros.Any(p => p.Referencia == this.Referencia && p.IdElemento == this.IdElemento && (this.Id > 0 ? p.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un parámetro con el mismo nombre de referencia en el mismo elemento.", new string[] { "Referencia" });
                }

                if (context.Parametros.Any(p => p.Referencia == this.Referencia && p.IdElemento == this.IdElemento && (this.Id > 0 ? p.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un parámetro con el mismo nombre de referencia en el mismo elemento.", new string[] { "Referencia" });
                }

                if (Generics.Reservadas.Contains(this.Referencia))
                {
                    yield return new ValidationResult("Ya existe una palabra reservada con el mismo nombre.", new string[] { "Referencia" });
                }
            }
        }
    }
}
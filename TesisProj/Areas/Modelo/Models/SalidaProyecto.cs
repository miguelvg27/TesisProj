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
    [Table("SalidaProyecto")]
    public class SalidaProyecto : DbObject, IValidatableObject
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener un mínimo de {2} y un máximo de {1} carácteres.")]
        [DisplayName("Salida")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Secuencia")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo {0} debe ser mayor que 0")]
        public int Secuencia { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener un máximo de {1} carácteres.")]
        [DisplayName("Período inicial")]
        public string PeriodoInicial { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener un máximo de {1} carácteres.")]
        [DisplayName("Período final")]
        public string PeriodoFinal { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Proyecto")]
        public int IdProyecto { get; set; }

        [XmlIgnore]
        [ForeignKey("IdProyecto")]
        public Proyecto Proyecto { get; set; }

        [XmlIgnore]
        [InverseProperty("Salida")]
        public List<SalidaOperacion> Operaciones { get; set; }

        public SalidaProyecto()
        {
        }

        public SalidaProyecto(PlantillaSalidaProyecto plantilla, int IdProyecto)
        {
            this.IdProyecto = IdProyecto;
            this.Nombre = plantilla.Nombre;
            this.PeriodoInicial = plantilla.PeriodoInicial;
            this.PeriodoFinal = plantilla.PeriodoFinal;
            this.Secuencia = plantilla.Secuencia;
        }

        public override string LogValues()
        {
            return "Nombre = " + this.Nombre + Environment.NewLine +
                "Secuencia = " + this.Secuencia + Environment.NewLine +
                "Período inicial = " + this.PeriodoInicial + Environment.NewLine +
                "Periodo final = " + this.PeriodoFinal;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (TProjContext context = new TProjContext())
            {
                if (context.SalidaProyectos.Any(s => s.Nombre == this.Nombre && s.IdProyecto == this.IdProyecto && (this.Id > 0 ? s.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo nombre en el proyecto.", new string[] { "Nombre" });
                }

                if (context.SalidaProyectos.Any(s => s.Secuencia == this.Secuencia && s.IdProyecto == this.IdProyecto && (this.Id > 0 ? s.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo número de secuencia en el proyecto.", new string[] { "Secuencia" });
                }

                //  Valida cadena de la fórmula

                bool cadenavalida = true;
                double testvalue = 0;
                MathParserNet.Parser parser = new MathParserNet.Parser();

                parser.AddVariable("Horizonte", 10);

                try
                {
                    testvalue = parser.SimplifyDouble(this.PeriodoInicial);
                }
                catch (Exception)
                {
                    cadenavalida = false;
                }

                if (!cadenavalida)
                {
                    yield return new ValidationResult("Cadena inválida. Solo puede contener Horizonte o números.", new string[] { "PeriodoInicial" });
                }

                cadenavalida = true;

                try
                {
                    testvalue = parser.SimplifyDouble(this.PeriodoFinal);
                }
                catch (Exception)
                {
                    cadenavalida = false;
                }

                if (!cadenavalida)
                {
                    yield return new ValidationResult("Cadena inválida. Solo puede contener Horizonte o números.", new string[] { "PeriodoFinal" });
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TesisProj.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Plantilla.Models
{
    [Table("PlantillaOperacion")]
    public class PlantillaOperacion : DbObject, IValidatableObject
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener un mínimo de {2} y un máximo de {1} carácteres.")]
        [DisplayName("Fórmula")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener un máximo de {1} carácteres.")]
        [DisplayName("Período inicial")]
        public string PeriodoInicial { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener un máximo de {1} carácteres.")]
        [DisplayName("Período final")]
        public string PeriodoFinal { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Secuencia")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo {0} debe ser mayor que 0")]
        public int Secuencia { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener un mínimo de {2} y un máximo de {1} carácteres.")]
        [DisplayName("Referencia")]
        [RegularExpression("[A-Za-z]+[A-Za-z1-9]*", ErrorMessage = "El campo solo puede contener alfanuméricos y debe comenzar con una letra.")]
        public string Referencia { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Plantilla")]
        public int IdPlantillaProyecto { get; set; }

        [ForeignKey("IdPlantillaProyecto")]
        public PlantillaProyecto PlantillaProyecto { get; set; }

        [DisplayName("Indicador")]
        public bool Indicador { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener un máximo de {1} carácteres.")]
        [DisplayName("Cadena")]
        public string Cadena { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (TProjContext context = new TProjContext())
            {
                if (context.PlantillaOperaciones.Any(f => f.Nombre == this.Nombre && f.IdPlantillaProyecto == this.IdPlantillaProyecto && (this.Id > 0 ? f.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo nombre en la misma plantilla.", new string[] { "Nombre" });
                }

                if (context.PlantillaOperaciones.Any(f => f.Referencia == this.Referencia && f.IdPlantillaProyecto == this.IdPlantillaProyecto && (this.Id > 0 ? f.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo nombre de referencia en la misma plantilla.", new string[] { "Referencia" });
                }

                if (context.PlantillaOperaciones.Any(f => f.Secuencia == this.Secuencia && f.IdPlantillaProyecto == this.IdPlantillaProyecto && (this.Id > 0 ? f.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo número de secuencia en la misma plantilla.", new string[] { "Secuencia" });
                }
            

                bool cadenavalida = true;
                double testvalue = 0;
                MathParserNet.Parser parser = new MathParserNet.Parser();
                var tipoformulas = context.TipoFormulas;
                var operaciones = context.PlantillaOperaciones.Where(o => o.IdPlantillaProyecto == this.IdPlantillaProyecto && o.Secuencia < this.Secuencia);
                parser.AddVariable("Horizonte", 10);

                foreach (TipoFormula tipoformula in tipoformulas)
                {
                    parser.AddVariable(tipoformula.Referencia, 2);
                }

                foreach (PlantillaOperacion operacion in operaciones)
                {
                    parser.AddVariable(operacion.Referencia, 2);
                }

                try
                {
                    testvalue = parser.SimplifyDouble(this.Cadena);
                }
                catch (Exception)
                {
                    cadenavalida = false;
                }

                if (!cadenavalida)
                {
                    yield return new ValidationResult("Cadena inválida. La operación solo puede contener referencias a tipos de fórmula.", new string[] { "Cadena" });
                }

                cadenavalida = true;

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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TesisProj.Areas.Plantilla.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Plantilla.Models
{
    [Table("PlantillaSalidaProyecto")]
    public class PlantillaSalidaProyecto : DbObject, IValidatableObject
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
        [DisplayName("Plantilla")]
        public int IdPlantillaProyecto { get; set; }

        [ForeignKey("IdPlantillaProyecto")]
        public PlantillaProyecto PlantillaProyecto { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener un máximo de {1} carácteres.")]
        [DisplayName("Cadena")]
        public string Cadena { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (TProjContext context = new TProjContext())
            {
                if (context.PlantillaSalidaProyectos.Any(s => s.Nombre == this.Nombre && s.IdPlantillaProyecto == this.IdPlantillaProyecto && (this.Id > 0 ? s.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo nombre en la misma plantilla.", new string[] { "Nombre" });
                }

                if (context.PlantillaSalidaProyectos.Any(s => s.Secuencia == this.Secuencia && s.IdPlantillaProyecto == this.IdPlantillaProyecto && (this.Id > 0 ? s.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo número de secuencia en la misma plantilla.", new string[] { "Secuencia" });
                }

                //  Valida cadena de la fórmula

                bool cadenavalida = true;
                double testvalue = 0;
                MathParserNet.Parser parser = new MathParserNet.Parser();

                var elementos = context.PlantillaElementoProyectos.Include("Elemento").Where(p => p.IdProyecto == this.IdPlantillaProyecto).Select(p => p.Elemento).ToList();
                List<PlantillaFormula> formulas = new List<PlantillaFormula>();
                foreach (PlantillaElemento elemento in elementos)
                {
                    formulas.AddRange(context.PlantillaFormulas.Where(f => f.IdPlantillaElemento == elemento.Id).ToList());
                }

                foreach (PlantillaFormula formula in formulas)
                {
                    parser.AddVariable(formula.Referencia, Math.PI);
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
                    yield return new ValidationResult("Cadena inválida. La fórmula solo puede contener las fórmulas de los elementos asociados a la plantilla.", new string[] { "Cadena" });
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Proyecto.Models
{
    [Table("SalidaProyecto")]
    public class SalidaProyecto : DbObject, IValidatableObject
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
        [DisplayName("Proyecto")]
        public int IdProyecto { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener un máximo de {1} carácteres.")]
        [DisplayName("Cadena")]
        public string Cadena { get; set; }

        public List<object> Valores { get; set; }

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

                var elementos = context.Elementos.Where(e => e.IdProyecto == this.IdProyecto).ToList();
                List<Formula> formulas = new List<Formula>();
                foreach (Elemento elemento in elementos)
                {
                    formulas.AddRange(context.Formulas.Where(f => f.IdElemento == elemento.Id).ToList());
                }

                foreach (Formula formula in formulas)
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
                    yield return new ValidationResult("Cadena inválida. La fórmula solo puede contener las fórmulas de los elementos del proyecto.", new string[] { "Cadena" });
                }
            }
        }
    }
}
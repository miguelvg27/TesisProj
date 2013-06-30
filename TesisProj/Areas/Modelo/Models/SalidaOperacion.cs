using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Plantilla.Models;
using TesisProj.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Modelo.Models
{
    [Table("OperacionXSalida")]
    public class SalidaOperacion : DbObject, IValidatableObject
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Operacion")]
        public int IdOperacion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Salida")]
        public int IdSalida { get; set; }

        [ForeignKey("IdOperacion")]
        public Operacion Operacion { get; set; }

        [ForeignKey("IdSalida")]
        public SalidaProyecto Salida { get; set; }

        public String Nombre { get { return Operacion.Nombre; } }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (TProjContext context = new TProjContext())
            {
                if (context.SalidaOperaciones.Any(p => p.IdSalida == this.IdSalida && p.IdOperacion == this.IdOperacion && (this.Id > 0 ? p.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con la misma combinación Salida/Operacion.", new string[] { "IdOperacion", "IdSalida" });
                }
            }
        }
    }
}
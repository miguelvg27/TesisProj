using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TesisProj.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Proyecto.Models
{
    [Table("Proyecto")]
    public class Proyecto : DbObject, IValidatableObject
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener un mínimo de {2} y un máximo de {1} carácteres.")]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Fecha de creación")]
        public DateTime Creacion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Fecha de última modificación")]
        public DateTime Modificacion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Creador")]
        public int IdCreador { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Última modificación")]
        public int IdModificador { get; set; }

        [ForeignKey("IdCreador")]
        public UserProfile Creador { get; set; }

        [ForeignKey("IdModificador")]
        public UserProfile Modificador { get; set; }

        [DisplayName("Descripción")]
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener un máximo de {1} carácteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Horizonte")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo {0} debe ser mayor que 0")]
        public int Horizonte { get; set; }

        [InverseProperty("Proyecto")]
        public List<SalidaProyecto> Salidas { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (TProjContext context = new TProjContext())
            {
                if (context.Proyectos.Any(p => p.Nombre == this.Nombre && (this.Id > 0 ? p.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un proyecto con el mismo nombre.", new string[] { "Nombre" });
                }
            }
        }
    }
}

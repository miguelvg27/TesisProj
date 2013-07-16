using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using TesisProj.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Modelo.Models
{
    [Table("Proyecto")]
    public class Proyecto : DbObject, IValidatableObject
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener un mínimo de {2} y un máximo de {1} carácteres.")]
        [DisplayName("Proyecto")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Fecha de creación")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Creacion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Creador")]
        public int IdCreador { get; set; }

        [XmlIgnore]
        [ForeignKey("IdCreador")]
        public virtual UserProfile Creador { get; set; }

        [XmlIgnore]
        public virtual List<UserProfile> Colaboradores { get; set; }

        [DisplayName("Descripción")]
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener un máximo de {1} carácteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Horizonte")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo {0} debe ser mayor que 0")]
        public int Horizonte { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Períodos preoperativos")]
        [Range(0, int.MaxValue, ErrorMessage = "El campo {0} debe ser mayor que 0")]
        public int PeriodosPreOp { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Períodos de cierre")]
        [Range(0, int.MaxValue, ErrorMessage = "El campo {0} debe ser mayor que 0")]
        public int PeriodosCierre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Última versión")]
        [Range(0, int.MaxValue, ErrorMessage = "El campo {0} debe ser mayor o igual que 0")]
        public int Version { get; set; }

        [InverseProperty("Proyecto")]
        public virtual List<Elemento> Elementos { get; set; }

        [InverseProperty("Proyecto")]
        public virtual List<SalidaProyecto> Salidas { get; set; }

        [InverseProperty("Proyecto")]
        public virtual List<Operacion> Operaciones { get; set; }

        public override string LogValues()
        {
            return "Nombre = " + this.Nombre + Environment.NewLine +
                "Version = " + this.Version + Environment.NewLine +
                "Horizonte = " + this.Horizonte + Environment.NewLine +
                "Descripcion = " + this.Descripcion + Environment.NewLine +
                "Períodos preoperativos = " + this.PeriodosPreOp + Environment.NewLine +
                "Períodos de cierre = " + this.PeriodosCierre;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (TProjContext context = new TProjContext())
            {
                if (context.Proyectos.Any(p => p.Nombre == this.Nombre && (this.Id > 0 ? p.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un proyecto con el mismo nombre.", new string[] { "Nombre" });
                }

                if (this.Horizonte < (this.PeriodosPreOp + this.PeriodosCierre))
                {
                    yield return new ValidationResult("El horizonte debe ser mayor a la suma de los períodos preoperativos y de cierre.", new string[] { "Horizonte" });
                }
            }
        }
    }

    public class SimAns
    {
        public double TirE { get; set; }
        public double TirF { get; set; }
        public double VanE { get; set; }
        public double VanF { get; set; }
    }

}

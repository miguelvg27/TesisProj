using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Plantilla.Models
{
    [Table("PlantillaOperacionXSalida")]
    public class PlantillaSalidaOperacion : DbObject
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Operacion")]
        public int IdOperacion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Salida")]
        public int IdSalida { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Secuencia")]
        public int Secuencia { get; set; }

        [ForeignKey("IdOperacion")]
        public virtual PlantillaOperacion Operacion { get; set; }

        [ForeignKey("IdSalida")]
        public virtual PlantillaSalidaProyecto Salida { get; set; }

        public String Nombre { get { return Operacion.Nombre; } }
    }
}
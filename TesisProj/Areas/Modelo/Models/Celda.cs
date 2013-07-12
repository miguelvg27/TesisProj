using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Modelo.Models
{
    [Table("Celda")]
    public class Celda : DbObject
    {
        [DisplayName("Período")]
        public int Periodo { get; set; }

        [DisplayName("Valor")]
        public decimal Valor { get; set; }

        [DisplayName("Parámetro")]
        public int IdParametro { get; set; }
        
        [ForeignKey("IdParametro")]
        public virtual Parametro Parametro { get; set; }

        public override string LogValues()
        {
            return "Elemento = " + this.Parametro.Elemento.Nombre + Environment.NewLine +
                "Parámetro = " + this.Parametro.Nombre + Environment.NewLine +
                "Período = " + this.Periodo + Environment.NewLine +
                "Valor = " + String.Format("{0:#,##0.00000}", this.Valor);
        }
    }
}
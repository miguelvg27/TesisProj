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
        public Parametro Parametro { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TesisProj.Models.Storage;
using TesisProj.Areas.Distribuciones.Models;
using TesisProj.Areas.Modelo.Models;

namespace TesisProj.Areas.Modelos.Models
{
    public class ModeloSimlacion:DbObject
    {
        #region Generales

        [DisplayName("Nombre de la Distribucion")]
        [Required]
        public string Nombre { get; set; }

        [DisplayName("Nombre Corto")]
        [Required]
        public string Abreviatura { get; set; }

        [DisplayName("Descripcion")]
        [Required]
        public string Descripcion { get; set; }

        [DisplayName("Definicion del modelo")]
        public string Definicion { get; set; }

        #endregion

        public virtual Distribucion distribucion { get; set; }

        public ModeloSimlacion()
        {
        }

    }
}
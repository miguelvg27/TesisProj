using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TesisProj.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Modelo.Models
{
    [Table("Audit")]
    public class Audit: DbObject
    {
        [DisplayName("Proyecto")]
        public int IdProyecto { get; set; }

        [ForeignKey("IdProyecto")]
        public Proyecto Proyecto { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Usuario")]
        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public UserProfile Usuario { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Transacción")]
        public string Transaccion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Tipo de objeto")]
        public string TipoObjeto { get; set; }

        [DisplayName("Datos originales")]
        public string Original { get; set; }

        [DisplayName("Datos modificados")]
        public string Modificado { get; set; }

        [DisplayName("Tipo de objeto")]
        public string Tipo
        {
            get 
            { 
                if(TipoObjeto.Equals(new Proyecto().GetType().ToString()))
                {
                    return "Proyecto";
                }

                if (TipoObjeto.Equals(new Elemento().GetType().ToString()))
                {
                    return "Elemento";
                }

                if (TipoObjeto.Equals(new Operacion().GetType().ToString()))
                {
                    return "Operación";
                }

                if (TipoObjeto.Equals(new SalidaProyecto().GetType().ToString()))
                {
                    return "Salida";
                }

                if (TipoObjeto.Equals(new SalidaOperacion().GetType().ToString()))
                {
                    return "Operación de una salida";
                }

                if (TipoObjeto.Equals(new Parametro().GetType().ToString()))
                {
                    return "Parámetro";
                }

                if (TipoObjeto.Equals(new Formula().GetType().ToString()))
                {
                    return "Fórmula";
                }

                if (TipoObjeto.Equals(new Celda().GetType().ToString()))
                {
                    return "Celda";
                }

                return "";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesisProj.Models.Storage;


namespace TesisProj.Areas.Proyectos.Models
{
    public class Pedro_Parametro : DbObject
    {
        public string Nombre { get; set; }
        public double valor { get; set; }
        public double interes { get; set; }
        public int periodos { get; set; }
        public virtual ICollection<Pedro_Elemento> Elementos { get; set; }

        public Pedro_Parametro() { }

        public Pedro_Parametro(string nombre, double valor, int periodos, double interes)
        {
            this.Nombre = nombre;
            this.valor = valor;
            this.periodos = periodos;
            this.interes = interes;
            this.Elementos = new List<Pedro_Elemento>();
        }
    }
}
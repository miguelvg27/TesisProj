using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Proyectos.Models
{
    public class Pedro_Elemento : DbObject
    {
        public string Nombre { get; set; }
        public double valor { get; set; }

        public Pedro_Elemento(){ }

        public Pedro_Elemento(int i,double valor) 
        { 
            this.Nombre="Periodo "+ i.ToString();
            this.valor = valor;
        }
    }
}
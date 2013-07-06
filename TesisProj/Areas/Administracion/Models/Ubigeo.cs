using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Administracion.Models
{
    public class Ubigeo : DbObject
    {
        public string CodDep { get; set; }
        public string Departamento { get; set; }
        public string CodPro { get; set; }
        public string Provincia { get; set; }
        public string CodDis { get; set; }
        public string Distrito { get; set; }

        //Departamentos
        //Aquellos registros donde los campos CodProv y CodDist son iguales a “00”
        //Ejm El departamento AYACUCHO tiene los siguientes códigos
        //CodDpto=”05”, CodProv=”00” y CodDist=”00”

        //Provincias
        //Aquellos registros donde el campo CodDist es igual a “00”
        //Ejm La provincia LA MAR tiene los siguientes códigos
        //CodDpto=”05”, CodProv=”05” y CodDist=”00”

        //Distritos
        //Todos los demás
        //Ejm El distrito SAMUGARI tiene los siguientes códigos
        //CodDpto=”05”, CodProv=”05” y CodDist=”09” 

        public Ubigeo(string CDep, string departamento, string CPro, string provincia, string CDis, string distrito)
        {
            this.CodDep = CDep;
            this.CodPro = CPro;
            this.CodDis = CDis;
            this.Departamento = departamento;
            this.Distrito = distrito;
            this.Provincia = provincia;
        }

        public Ubigeo() { }
    }
}
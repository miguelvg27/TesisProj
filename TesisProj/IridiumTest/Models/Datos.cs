using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IridiumTest.Models
{

    public class Datos 
    {
        //http://christoph.ruegg.name/blog/towards-mathnet-numerics-v3.html

        public string Titulo { get; set; }

        public string Definicion { get; set; }

        public byte[] Imagen { get; set; }

        public byte[] Resumen { get; set; }

        public string link { get; set; }

        public List<Param> ParamsIN {get; set;}

        public List<Param> ParamsOUT { get; set; }

        public List<Formulate> Formulates{ get; set; }

        public List<Graphic> Graphics { get; set; }

        public List<Result> Results { get; set; }

    }
}

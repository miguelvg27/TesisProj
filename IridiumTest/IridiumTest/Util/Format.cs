using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IridiumTest.Util
{
    public static class Format
    {
        public static string alinearDerecha(string linea, int cantCaracteres, string relleno, bool salto = true)
        {
            string salida = String.Empty;
            for (int i = 0; i < cantCaracteres - linea.Length; i++)
            {
                salida += relleno;
            }

            salida += linea.Trim();
            if (salto)
                salida += "\n";
            else
                salida += " ";
            return salida;
        }
    }
}

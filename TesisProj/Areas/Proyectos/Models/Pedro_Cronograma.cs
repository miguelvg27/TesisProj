using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesisProj.Areas.Proyectos.Models
{
    public  class Pedro_Cronograma
    {
        public static Pedro_Parametro GenerarElementos(string nombreParametro, int periodos, double valorParametro, double valorInteres)
        {
            Pedro_Parametro _parametro = new Pedro_Parametro(nombreParametro, valorParametro, periodos, valorInteres);
            _parametro.Elementos = new List<Pedro_Elemento>();
            for (int y = 0; y < periodos; y++)
            {
                if (y == 0)
                {
                    Pedro_Elemento aux = new Pedro_Elemento(y, valorParametro);
                    //_parametro.Elementos.Add(aux); no considera el valor inicial
                }
                else
                {
                    Pedro_Elemento aux = new Pedro_Elemento(y, (1+valorInteres/100)*(valorParametro / (periodos - 1)));
                    _parametro.Elementos.Add(aux);
                }
            }
            return _parametro;
        }
    }
}
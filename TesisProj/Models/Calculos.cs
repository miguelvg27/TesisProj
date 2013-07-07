using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesisProj.Models
{
    public class Calculos
    {
        public static Int64 Combinatoria(int n, int k)
        {
            return Factorial(n) / (Factorial(k) * Factorial(n - k));
        }

        public static Int64 Factorial(int j)
        {
            int i = j;
            Int64 d = 1;
            if (j < 0) i = Math.Abs(j);
            if (j == 0) return 1;
            while (i > 1)
            {
                d *= i--;
            }

            if (j < 0) return -d;
            else return d;
        }

        public static double DesviacionStandard(List<double> listaDatos)
        {
            double desvStd = 0;
            double N = 0, prom = 0, suma = 0, NrestadoUno = 0, sumapotencias = 0;
            N = listaDatos.Count;
            NrestadoUno = N - 1;
            foreach (double dato in listaDatos)
            {
                suma += dato;
            }
            prom = suma / N;
            foreach (double dato in listaDatos)
            {
                sumapotencias += Math.Pow((dato - prom), 2);
            }
            desvStd = Math.Sqrt((1 * sumapotencias) / NrestadoUno);
            return desvStd;
        }
    }
}
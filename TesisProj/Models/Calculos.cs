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
    }
}
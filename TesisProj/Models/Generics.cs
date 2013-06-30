using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesisProj.Models
{
    public class Generics
    {
        public static string[] Reservadas = { "Horizonte", "Amortizacion", "Intereses", "Cuota", "DepreciacionLineal", "DepreciacionAcelerada", "ValorResidual", "Periodo", "Tir", "Van" };

        public static double Ppmt(double i, double p, double N, double V)
        {
            return -Financial.PPmt(i, p, N, V);
        }

        public static double IPmt(double i, double p, double N, double V)
        {
            return -Financial.IPmt(i, p, N, V);
        }

        public static double Pmt(double i, double N, double V)
        {
            return -Financial.Pmt(i, N, V);
        }

        public static double Sln(double V, double l)
        {
            return Financial.SLN(V, 0, l);
        }

        public static double Syn(double V, double l, double p)
        {
            return Financial.SYD(V, 0, l, p);
        }

        public static double ResSln(double V, double vida, double pinicial, double horizonte)
        {
            double pfinal = pinicial + vida - 1;
            return pfinal > horizonte ? (pfinal - horizonte) * Sln(V, vida) : 0;
        }

        public static double Npv(double i, double[] saldo)
        {
            return Financial.NPV(i, ref saldo);
        }

        public static double Irr(double[] saldo)
        {
            return Financial.IRR(ref saldo);
        }

        public static bool Validar(string cadena, MathParserNet.Parser parser)
        {
            bool valida = true;
            try
            {
                double testvalue = parser.SimplifyDouble(cadena);
            }
            catch (Exception)
            {
                valida = false;
            }

            return valida;
        }
    }
}
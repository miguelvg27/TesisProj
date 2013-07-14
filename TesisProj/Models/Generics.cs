using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Plantilla.Models;

namespace TesisProj.Models
{
    public class Generics
    {
        public static string[] Reservadas = { "Horizonte", "PeriodosPreOperativos", "PeriodosCierre", "Amortizacion", "Intereses", "Cuota", "DepreciacionLineal", "DepreciacionAcelerada", "ValorResidual", "Periodo", "Tir", "Van" };

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

        public static double Sln(double V, double l, double pinicial,double pactual)
        {
            double pfinal = pinicial + l - 1;
            return pactual > pfinal ? 0 : Financial.SLN(V, 0, l);
        }

        public static double Syn(double V, double l, double p)
        {
            return Financial.SYD(V, 0, l, p);
        }

        public static double ResSln(double V, double vida, double pinicial, double pactual)
        {
            double pfinal = pinicial + vida - 1;
            return (pfinal - pactual) * Sln(V, vida, pinicial, pactual);
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

        public static double SimpleParse(string cadena, int horizonte, int periodo, int preop = 0, int cierre = 0)
        {
            MathParserNet.Parser parser = new MathParserNet.Parser();
            parser.AddVariable("Horizonte", horizonte);
            parser.AddVariable("Periodo", periodo);
            parser.AddVariable("PeriodosPreOperativos", preop);
            parser.AddVariable("PeriodosCierre", cierre);

            double value = 0;

            try
            {
                value = parser.SimplifyDouble(cadena);
            }
            catch (Exception)
            {
                Console.WriteLine("Fallo en el parse de la cadena: " + cadena);
            }

            return value;
        }

        public static double ComplexParse(string cadena, List<Operacion> operaciones)
        {
            double ret = 0;
            try
            {
                string f = cadena.Substring(0, 3);
                string toTest = cadena.Substring(4);
                toTest = toTest.Substring(0, toTest.Length - 1);

                if (f.Equals("Van"))
                {
                    string[] parametros = toTest.Split(',');
                    List<double> saldo = operaciones.FirstOrDefault(o => o.Referencia == parametros[1]).Valores;
                    double k = operaciones.FirstOrDefault(o => o.Referencia == parametros[0]).Valores[0];

                    ret = saldo[0] + Npv(k, saldo.Skip(1).ToArray());
                }

                if (f.Equals("Tir"))
                {
                    List<double> saldo = operaciones.FirstOrDefault(o => o.Referencia == toTest).Valores;
                    ret = Irr(saldo.ToArray());
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Fallo en el parse de la cadena: " + cadena);
            }

            return ret;
        }

        public static bool TestComplexParse(string cadena, List<Operacion> operaciones)
        {
            bool test = false;
            try
            {
                string f = cadena.Substring(0, 3);
                string toTest = cadena.Substring(4);
                toTest = toTest.Substring(0, toTest.Length - 1);

                if (f.Equals("Van"))
                {
                    string[] parametros = toTest.Split(',');
                    bool saldo = operaciones.Any(o => o.Referencia == parametros[1]);
                    bool k = operaciones.Any(o => o.Referencia == parametros[0]);
                    bool num = parametros.Count() == 2;
                    test = (saldo && k && num);
                }

                if (f.Equals("Tir"))
                {
                    bool saldo = operaciones.Any(o => o.Referencia == toTest);
                    test = saldo;
                }
            }
            catch (Exception)
            {
                test = false;
                Console.WriteLine("Fallo en el parse de la cadena: " + cadena);
            }

            return test;
        }

        public static bool TestComplexParse(string cadena, List<PlantillaOperacion> operaciones)
        {
            bool test = false;
            try
            {
                string f = cadena.Substring(0, 3);
                string toTest = cadena.Substring(4);
                toTest = toTest.Substring(0, toTest.Length - 1);

                if (f.Equals("Van"))
                {
                    string[] parametros = toTest.Split(',');
                    bool saldo = operaciones.Any(o => o.Referencia == parametros[1]);
                    bool k = operaciones.Any(o => o.Referencia == parametros[0]);
                    bool num = parametros.Count() == 2;
                    test = (saldo && k && num);
                }

                if (f.Equals("Tir"))
                {
                    bool saldo = operaciones.Any(o => o.Referencia == toTest);
                    test = saldo;
                }
            }
            catch (Exception)
            {
                test = false;
                Console.WriteLine("Fallo en el parse de la cadena: " + cadena);
            }

            return test;
        }
    }
}
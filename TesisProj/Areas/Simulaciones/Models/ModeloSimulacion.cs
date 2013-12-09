using System;
using System.Collections.Generic;
using TesisProj.Areas.IridiumTest.Models;
using TesisProj.Areas.IridiumTest.Models.Continuous;
using TesisProj.Areas.IridiumTest.Models.Discrete;

namespace TesisProj.Areas.Simulaciones.Models
{
    public class ModeloSimulacion
    {
        public string Nombre_UniformeDiscreta = "UniformeDiscreta";

        public _UniformeDiscreta uniformediscreta { get; set; }

        public string Nombre_Binomial = "Binomial";

        public _Binomial binomial { get; set; }

        public string Nombre_Geometrica = "Geometrica";

        public _Geometrica geometrica { get; set; }

        public string Nombre_HiperGeometrica = "HiperGeometrica";

        public _HiperGeometrica hipergeometrica { get; set; }

        public string Nombre_Poisson = "Poisson";

        public _Poisson poisson { get; set; }

        public string Nombre_Normal = "Normal";

        public _Normal normal { get; set; }

        public string Nombre_UniformeContinua = "UniformeContinua";

        public _UniformeContinua uniformecontinua { get; set; }

        public string Nombre_Exponencial = "Exponencial";

        public _Exponencial exponencial { get; set; }

        public string Nombre_Tstudent = "TStudent";

        public _TStudent tstudent { get; set; }

        public string Nombre_Gamma = "Gamma";

        public _Gamma gamma { get; set; }

        public string Nombre_Beta = "Beta";

        public _Beta beta { get; set; }

        public string Nombre_F = "F";

        public _F f { get; set; }

        public string Nombre_Weibull = "Weibull";

        public _Weibull weibull { get; set; }

        public string Nombre_Pareto = "Pareto";

        public _Pareto pareto { get; set; }

        public string Nombre_ChiCuadrado = "ChiCuadrado";

        public _ChiCuadrado chicuadrado { get; set; }

        public ModeloSimulacion()
        {
            binomial = null;
            geometrica = null;
            hipergeometrica = null;
            poisson = null;
            uniformediscreta = null;
        }

        public ModeloSimulacion(string nombre, List<ListField> list)
        {
            if (nombre.CompareTo("Binomial") == 0)
                binomial = new _Binomial(list);
            else
                binomial = null;

            if (nombre.CompareTo("Geometrica") == 0)
                geometrica = new _Geometrica(list);
            else
                geometrica = null;

            if (nombre.CompareTo("HiperGeometrica") == 0)
                hipergeometrica = new _HiperGeometrica(list);
            else
                hipergeometrica = null;

            if (nombre.CompareTo("Poisson") == 0)
                poisson = new _Poisson(list);
            else
                poisson = null;

            if (nombre.CompareTo("UniformeDiscreta") == 0)
                uniformediscreta = new _UniformeDiscreta(list);
            else
                uniformediscreta = null;

            if (nombre.CompareTo("Beta") == 0)
                beta = new _Beta(list);
            else
                beta = null;

            if (nombre.CompareTo("ChiCuadrado") == 0)
                chicuadrado = new _ChiCuadrado(list);
            else
                chicuadrado = null;

            if (nombre.CompareTo("Exponencial") == 0)
                exponencial = new _Exponencial(list);
            else
                exponencial = null;

            if (nombre.CompareTo("F") == 0)
                f = new _F(list);
            else
                f = null;

            if (nombre.CompareTo("Gamma") == 0)
                gamma = new _Gamma(list);
            else
                gamma = null;

            if (nombre.CompareTo("Normal") == 0)
                normal = new _Normal(list);
            else
                normal = null;

            if (nombre.CompareTo("Pareto") == 0)
                pareto = new _Pareto(list);
            else
                pareto = null;

            if (nombre.CompareTo("TStudent") == 0)
                tstudent = new _TStudent(list);
            else
                tstudent = null;

            if (nombre.CompareTo("UniformeContinua") == 0)
                uniformecontinua = new _UniformeContinua(list);
            else
                uniformecontinua = null;

            if (nombre.CompareTo("Weibull") == 0)
                weibull = new _Weibull(list);
            else
                weibull = null;
        }

        public ModeloSimulacion(string nombre, double a, double b, double c, double d)
        {
            #region Discreta

            if (nombre.CompareTo("Binomial") == 0)
                binomial = new _Binomial(Convert.ToDouble(a), Convert.ToInt16(b));
            else
                binomial = null;

            if (nombre.CompareTo("Geometrica") == 0)
                geometrica = new _Geometrica(Convert.ToDouble(a));
            else
                geometrica = null;

            if (nombre.CompareTo("HiperGeometrica") == 0)
                hipergeometrica = new _HiperGeometrica(Convert.ToInt16(a), Convert.ToInt16(b), Convert.ToInt16(c));
            else
                hipergeometrica = null;

            if (nombre.CompareTo("Poisson") == 0)
                poisson = new _Poisson(Convert.ToDouble(a));
            else
                poisson = null;

            if (nombre.CompareTo("UniformeDiscreta") == 0)
                uniformediscreta = new _UniformeDiscreta(Convert.ToInt16(a), Convert.ToInt16(b));
            else
                uniformediscreta = null;

            #endregion Discreta

            #region Continua

            if (nombre.CompareTo("Normal") == 0)
                normal = new _Normal(a, b, a - 15, a + 15);
            else
                normal = null;

            if (nombre.CompareTo("Beta") == 0)
                beta = new _Beta(a, b);
            else
                beta = null;

            if (nombre.CompareTo("ChiCuadrado") == 0)
                chicuadrado = new _ChiCuadrado(a);
            else
                chicuadrado = null;

            if (nombre.CompareTo("Exponencial") == 0)
                exponencial = new _Exponencial(a);
            else
                exponencial = null;

            if (nombre.CompareTo("TStudent") == 0)
                tstudent = new _TStudent(a, b, c);
            else
                tstudent = null;

            if (nombre.CompareTo("Gamma") == 0)
                gamma = new _Gamma(a, b);
            else
                gamma = null;

            if (nombre.CompareTo("F") == 0)
                f = new _F(a, b);
            else
                f = null;

            if (nombre.CompareTo("Weibull") == 0)
                weibull = new _Weibull(a, b);
            else
                weibull = null;

            if (nombre.CompareTo("Pareto") == 0)
                pareto = new _Pareto(a, b);
            else
                pareto = null;

            if (nombre.CompareTo("UniformeContinua") == 0)
                uniformecontinua = new _UniformeContinua(a, b);
            else
                uniformecontinua = null;

            #endregion Continua
        }

        #region Discreta

        public _Binomial GetBinomial()
        {
            return binomial;
        }

        public _Geometrica GetGeometrica()
        {
            return geometrica;
        }

        public _HiperGeometrica GetHiperGeometrica()
        {
            return hipergeometrica;
        }

        public _Poisson GetPoisson()
        {
            return poisson;
        }

        public _UniformeDiscreta GetUniformDiscrete()
        {
            return uniformediscreta;
        }

        #endregion Discreta

        #region Continua

        public _Beta GetBeta()
        {
            return beta;
        }

        public _ChiCuadrado GetChiCuadrado()
        {
            return chicuadrado;
        }

        public _Exponencial GetExponencial()
        {
            return exponencial;
        }

        public _F GetF()
        {
            return f;
        }

        public _Gamma GetGamma()
        {
            return gamma;
        }

        public _Normal GetNormal()
        {
            return normal;
        }

        public _Pareto GetPareto()
        {
            return pareto;
        }

        public _TStudent GetTstudent()
        {
            return tstudent;
        }

        public _UniformeContinua GetUniformeContinua()
        {
            return uniformecontinua;
        }

        public _Weibull GetWeibull()
        {
            return weibull;
        }

        #endregion Continua
    }
}
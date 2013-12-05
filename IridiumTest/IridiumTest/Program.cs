using IridiumTest.Models.Discrete;
using System;

namespace IridiumTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            _Binomial bi = new _Binomial(.21, 50);

            bi.GetModelo();
            bi.GetSimulacion(15000);
            bi.GetResumen();
            bi.Save();
            Console.Read();
        }
    }
}
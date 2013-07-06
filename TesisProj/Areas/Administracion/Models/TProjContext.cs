using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using System.Linq;
using System.Web;
using TesisProj.Areas.Administracion.Models;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Plantilla.Models;

namespace TesisProj.Models.Storage
{
    public partial class TProjContext : DbContext
    {
        public DbSet<Ubigeo> InternalUbigeo { get; set; }
        public DbRequester<Ubigeo> TablaUbigeo { get; set; }


        public DbSet<EstadoCivil> InternaEstadoCivil { get; set; }
        public DbRequester<EstadoCivil> TablaEstadoCivil { get; set; }

        public void RegistrarTablasAdministracion()
        {
            TablaUbigeo = new DbRequester<Ubigeo>(this, InternalUbigeo);
            TablaEstadoCivil = new DbRequester<EstadoCivil>(this, InternaEstadoCivil);

        }

        public void SeedUbigeo()
        {
            List<String[]> parsedData = new List<string[]>();
            //Miguel cambia esta parte para ponerla en tu maquina
            using (StreamReader readerFile = new StreamReader(@"C:\Users\PEDRO\Documents\GitHub\TesisProj\Carga\Ubigeo.txt"))
            {
                string linea;
                string[] lista;
                linea = readerFile.ReadLine();
                while ((linea = readerFile.ReadLine()) != null)
                {
                    lista = linea.Split('|');
                    TablaUbigeo.AddElement(new Ubigeo(lista[0], lista[1], lista[2], lista[3], lista[4], lista[5]));
                }
            }
        }

        public void SeedEstadoCivil()
        {
            TablaEstadoCivil.AddElement(new EstadoCivil(1, "No Especificar"));
            TablaEstadoCivil.AddElement(new EstadoCivil(2, "Soltero (a)"));
            TablaEstadoCivil.AddElement(new EstadoCivil(3, "Casado (a)"));
            TablaEstadoCivil.AddElement(new EstadoCivil(4, "Viudo (a)"));
            TablaEstadoCivil.AddElement(new EstadoCivil(5, "Divorciado (a)"));

        }

    }
}

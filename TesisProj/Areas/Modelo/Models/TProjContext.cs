using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Plantilla.Models;

namespace TesisProj.Models.Storage
{
    public partial class TProjContext : DbContext
    {
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Elemento> Elementos { get; set; }
        public DbSet<Parametro> Parametros { get; set; }
        public DbSet<Formula> Formulas { get; set; }
        public DbSet<SalidaProyecto> SalidaProyectos { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Celda> Celdas { get; set; }

        public DbRequester<Proyecto> ProyectosRequester { get; set; }
        public DbRequester<Elemento> ElementosRequester { get; set; }
        public DbRequester<Parametro> ParametrosRequester { get; set; }
        public DbRequester<Formula> FormulasRequester { get; set; }
        public DbRequester<SalidaProyecto> SalidaProyectosRequester { get; set; }
        public DbRequester<Celda> CeldasRequester { get; set; }

        public void RegistrarTablasProyecto()
        {
            ProyectosRequester = new DbRequester<Proyecto>(this, Proyectos);
            ElementosRequester = new DbRequester<Elemento>(this, Elementos);
            ParametrosRequester = new DbRequester<Parametro>(this, Parametros);
            FormulasRequester = new DbRequester<Formula>(this, Formulas);
            SalidaProyectosRequester = new DbRequester<SalidaProyecto>(this, SalidaProyectos);
            CeldasRequester = new DbRequester<Celda>(this, Celdas);
        }

        public void SeedProyecto()
        {
            SeedProyectos();
            SeedElementos();
            SeedParametros();
            SeedFormulas();
            SeedSalidaElementos();
            SeedSalidaProyectos();
        }

        public void SeedProyectos()
        {
        //    ProyectosRequester.AddElement(new Proyecto { Id = 1, IdCreador = 1, IdModificador = 1, Nombre = "Proyecto prueba", Descripcion = "El gran proyecto de las pruebas", Creacion = DateTime.Now, Modificacion = DateTime.Now, Horizonte = 10, Version = 1 });
        }

        public void SeedElementos()
        {
        }

        public void SeedParametros()
        {
        }

        public void SeedFormulas()
        {
        }

        public void SeedSalidaElementos()
        {
        }

        public void SeedSalidaProyectos()
        {
        }
    }
}
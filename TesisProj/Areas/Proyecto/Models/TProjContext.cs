using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TesisProj.Areas.Proyecto.Models;

namespace TesisProj.Models.Storage
{
    public partial class TProjContext : DbContext
    {
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Elemento> Elementos { get; set; }
        public DbSet<Parametro> Parametros { get; set; }
        public DbSet<Formula> Formulas { get; set; }
        public DbSet<SalidaElemento> SalidaElementos { get; set; }
        public DbSet<SalidaProyecto> SalidaProyectos { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbRequester<Proyecto> ProyectosRequester { get; set; }
        public DbRequester<Elemento> ElementosRequester { get; set; }
        public DbRequester<Parametro> ParametrosRequester { get; set; }
        public DbRequester<Formula> FormulasRequester { get; set; }
        public DbRequester<SalidaElemento> SalidaElementosRequester { get; set; }
        public DbRequester<SalidaProyecto> SalidaProyectosRequester { get; set; }

        public void RegistrarTablasProyecto()
        {
            ProyectosRequester = new DbRequester<Proyecto>(this, Proyectos);
            ElementosRequester = new DbRequester<Elemento>(this, Elementos);
            ParametrosRequester = new DbRequester<Parametro>(this, Parametros);
            FormulasRequester = new DbRequester<Formula>(this, Formulas);
            SalidaElementosRequester = new DbRequester<SalidaElemento>(this, SalidaElementos);
            SalidaProyectosRequester = new DbRequester<SalidaProyecto>(this, SalidaProyectos);
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
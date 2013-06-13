using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace TesisProj.Models.Storage
{
    public class TProjContext : DbContext
    {
        public DbSet<TipoElemento> InternalTipoElementos { get; set; }
        public DbRequester<TipoElemento> TipoElementos;
        public TProjContext() : base("TProjDb") 
        {
            RegistrarTablas();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToOneConstraintIntroductionConvention>();
        }

        public void RegistrarTablas()
        {
            TipoElementos = new DbRequester<TipoElemento>(this, InternalTipoElementos);
        }

        public void Seed(){
            SeedTipoElementos();
        }

        public void SeedTipoElementos(){
            TipoElementos.AddElement(new TipoElemento { Nombre = "Activo" });
            TipoElementos.AddElement(new TipoElemento { Nombre = "Gasto" });
            TipoElementos.AddElement(new TipoElemento { Nombre = "Operativo" });
            TipoElementos.AddElement(new TipoElemento { Nombre = "Financiamiento" });
        }
    }

//  public class TProjInitializer : DropCreateDatabaseIfModelChanges<TProjContext>
    public class TProjInitializer : DropCreateDatabaseAlways<TProjContext>
    {
        protected override void Seed(TProjContext context){
            context.Seed();
        }
    }
}
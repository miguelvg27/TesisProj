using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Plantilla.Models;

namespace TesisProj.Models.Storage
{
    public partial class TProjContext : DbContext
    {

        public TProjContext()
            : base("TProjDb")
        {
            RegistrarTablas();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToOneConstraintIntroductionConvention>();
            modelBuilder.Entity<Celda>().Property(c => c.Valor).HasPrecision(16, 5);
        }

        public void RegistrarTablas()
        {
            RegistrarTablasPlantilla();
            RegistrarTablasProyecto();

            //Pedro Curich
            RegistrarTablasProyectoPedro();
            RegistrarTablasDistribucion();
            RegistrarTablasModelo();
            RegistrarTablasAdministracion();

        }

        public void Seed()
        {
            SeedPlantilla();
            SeedProyecto();

            //Pedro Curich
            SeedEstadoCivil();
            SeedElementos();
            //seedModelo();
            SeedDistribuciones();


        }
    }

    //public class TProjInitializer : DropCreateDatabaseIfModelChanges<TProjContext>
    public class TProjInitializer : DropCreateDatabaseAlways<TProjContext>
    {
        protected override void Seed(TProjContext context)
        {
            context.Seed();
        }
    }
}

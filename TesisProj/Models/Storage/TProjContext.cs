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
            //: base("TProjDb")
            : base("TProjContext")
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

        }

        public void Seed()
        {
            SeedPlantilla();

            SeedUserProfiles();
            SeedProyecto(); 
        }
    }

    public class TProjInitializer : DropCreateDatabaseIfModelChanges<TProjContext>
    //public class TProjInitializer : DropCreateDatabaseAlways<TProjContext>
    {
        protected override void Seed(TProjContext context)
        {
            context.Database.ExecuteSqlCommand("ALTER TABLE PlantillaOperacion ALTER COLUMN Referencia VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CS_AS NULL");
            context.Database.ExecuteSqlCommand("ALTER TABLE PlantillaParametro ALTER COLUMN Referencia VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CS_AS NULL");
            context.Database.ExecuteSqlCommand("ALTER TABLE PlantillaFormula ALTER COLUMN Referencia VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CS_AS NULL");
            context.Database.ExecuteSqlCommand("ALTER TABLE TipoFormula ALTER COLUMN Referencia VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CS_AS NULL");
            context.Database.ExecuteSqlCommand("ALTER TABLE Operacion ALTER COLUMN Referencia VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CS_AS NULL");
            context.Database.ExecuteSqlCommand("ALTER TABLE Parametro ALTER COLUMN Referencia VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CS_AS NULL");
            context.Database.ExecuteSqlCommand("ALTER TABLE Formula ALTER COLUMN Referencia VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CS_AS NULL");

            context.Seed();
        }
    }
}

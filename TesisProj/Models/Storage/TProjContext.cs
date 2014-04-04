using Devtalk.EF.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using TesisProj.Areas.IridiumTest.Models;
using TesisProj.Areas.IridiumTest.Models.Discrete;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Plantilla.Models;

namespace TesisProj.Models.Storage
{
    public partial class TProjContext : DbContext
    {

        public TProjContext()
               : base(TesisProj.MvcApplication.ConnectionString)
        {
            RegistrarTablas(); 
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToOneConstraintIntroductionConvention>(); 
            modelBuilder.Entity<Celda>().Property(c => c.Valor).HasPrecision(16, 5);
            modelBuilder.Entity<ListField>().Property(p => p.Imagen).HasColumnType("Image");
        }

        public void RegistrarTablas()
        {
            RegistrarTablasPlantilla();
            RegistrarTablasProyecto();
            RegistrarTablaImagenes();

        }

        public void Seed()
        {
            Database.ExecuteSqlCommand("ALTER TABLE PlantillaOperacion ALTER COLUMN Referencia VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CS_AS NULL");
            Database.ExecuteSqlCommand("ALTER TABLE PlantillaParametro ALTER COLUMN Referencia VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CS_AS NULL");
            Database.ExecuteSqlCommand("ALTER TABLE PlantillaFormula ALTER COLUMN Referencia VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CS_AS NULL");
            Database.ExecuteSqlCommand("ALTER TABLE TipoFormula ALTER COLUMN Referencia VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CS_AS NULL");
            Database.ExecuteSqlCommand("ALTER TABLE Operacion ALTER COLUMN Referencia VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CS_AS NULL");
            Database.ExecuteSqlCommand("ALTER TABLE Parametro ALTER COLUMN Referencia VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CS_AS NULL");
            Database.ExecuteSqlCommand("ALTER TABLE Formula ALTER COLUMN Referencia VARCHAR(255) COLLATE SQL_Latin1_General_CP1_CS_AS NULL");

            SeedPlantilla();
            SeedUserProfiles();
            SeedProyecto();
            SeedTablaImagenes();
        }
    }

    public class TProjInitializer : CreateDatabaseIfNotExists<TProjContext>
    {
        protected override void Seed(TProjContext context)
        {
            context.Seed();
        }
    }

    public class TProjDropInitializer : DropCreateDatabaseAlways<TProjContext>
    {
        protected override void Seed(TProjContext context)
        {
            context.Seed();
        }
    }
}

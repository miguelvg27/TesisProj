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
           //   : base("TProjContext")
          // : base("Server=b2b54485-7ca7-43cc-9153-a28f004c1a40.sqlserver.sequelizer.com;Database=dbb2b544857ca743cc9153a28f004c1a40;User ID=bmprfcrbhkmmbkoa;Password=LybRmMNWAWF7BLMuV56FWdhmSZRF4PYFqLawYrGxYVpxHSHNUiQhAbmHqX7u5g2T;")
             : base("Server=623434a1-3168-4a29-9c5c-a28c00691ad4.sqlserver.sequelizer.com;Database=db623434a131684a299c5ca28c00691ad4;User ID=njjzhaanrftdgkpu;Password=F7bfgVTLsGYuJvUpberqPBU3pRbpVbQeeQvJpo2TLTLSjN6tAEfmgsgdmgYxUDXq")
        {
            RegistrarTablas(); 
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>(); 
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
            SeedPlantilla();
            SeedUserProfiles();
            SeedProyecto();
            SeedTablaImagenes();
        }
    }

    public class TProjInitializer : CreateDatabaseIfNotExists<TProjContext>
    //public class TProjInitializer:  DropCreateDatabaseIfModelChanges<TProjContext>
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
